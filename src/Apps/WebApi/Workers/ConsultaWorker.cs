using Application.Handlers.Agendamentos.Commands.Delete;
using Application.Handlers.Consultas.Commands.Update.FinalizarConsulta;
using Application.Interfaces; 
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Workers
{
    public class ConsultaMonitoramentoWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ConsultaMonitoramentoWorker> _logger;

        public ConsultaMonitoramentoWorker(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<ConsultaMonitoramentoWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consulta Monitoramento Worker iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Executando verificação de consultas em: {time}", DateTimeOffset.Now);

                    // Criamos um escopo novo para cada execução do loop, pois o DbContext é Scoped
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();
                        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                        await ProcessarFimDeConsultas(context, mediator, stoppingToken);
                        await ProcessarLimpezaAgendamentosNaoComparecidos(context, mediator, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao executar o Worker de Monitoramento de Consultas.");
                }

                // Aguarda 1 hora antes da próxima execução
                // Dica: Para testes, altere para TimeSpan.FromMinutes(1)
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        // 2. Finalizar Consultas após 1 hora de duração
        private async Task ProcessarFimDeConsultas(IApplicationDbContext context, ISender mediator, CancellationToken token)
        {
            var agora = DateTime.Now;
            // Regra: Status EmAndamento E DataHoraInicio foi há mais de 1 hora
            var tempoLimite = agora.AddHours(-1);

            var consultasParaFinalizar = await context.Consultas
                .Where(c => c.Status == ConsultaStatus.EmAndamento
                         && c.DataHoraInicio <= tempoLimite
                         && !c.IsDeleted)
                .Select(c => c.Id)
                .ToListAsync(token);

            foreach (var id in consultasParaFinalizar)
            {
                _logger.LogInformation($"Finalizando consulta automática (tempo expirado): {id}");
                await mediator.Send(new UpdateFinalizarConsultaCommand { ConsultaId = id }, token);
                await context.SaveChangesAsync(token);
            }
        }

        // 3. Remover Agendamentos esquecidos há 1 dias
        private async Task ProcessarLimpezaAgendamentosNaoComparecidos(
            IApplicationDbContext context,
            ISender mediator,
            CancellationToken token)
        {
            var agora = DateTime.UtcNow;
            var dataLimiteExclusao = agora.AddDays(-1);

            var agendamentosParaRemover = await context.Agendamentos
                .Include(a => a.Consulta)
                .Where(a =>
                    a.Consulta != null &&
                    a.Consulta.Status == ConsultaStatus.Agendada &&
                    a.DataHoraInicio <= dataLimiteExclusao &&
                    !a.IsDeleted)
                .Select(a => a.Id)
                .ToListAsync(token);

            foreach (var id in agendamentosParaRemover)
            {
                _logger.LogInformation($"Removendo agendamento expirado (24h): {id}");

                await mediator.Send(new DeleteAgendamentoCommand
                {
                    Id = id,
                    RetornarPacienteListaEspera = true
                }, token);
            }
        }

    }
}