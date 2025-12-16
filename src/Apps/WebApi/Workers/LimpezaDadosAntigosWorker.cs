using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Workers
{
    public class LimpezaDadosAntigosWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<LimpezaDadosAntigosWorker> _logger;

        public LimpezaDadosAntigosWorker(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<LimpezaDadosAntigosWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker de Limpeza de Dados Antigos iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Iniciando varredura para exclusão definitiva de registros (Hard Delete) em: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                        // Executa a lógica de limpeza
                        await LimparRegistrosAntigos(context, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao executar o Worker de Limpeza de Dados Antigos.");
                }

                // Aguarda 24 horas antes da próxima execução
                var proximaExecucao = TimeSpan.FromHours(24);
                _logger.LogInformation($"Próxima varredura agendada para daqui a {proximaExecucao.TotalHours} horas.");

                await Task.Delay(proximaExecucao, stoppingToken);
            }
        }

        private async Task LimparRegistrosAntigos(IApplicationDbContext context, CancellationToken token)
        {
            var dataLimite = DateTime.UtcNow.AddDays(-30);

            var consultasParaExcluir = await context.Consultas
                .IgnoreQueryFilters()
                .Where(c => c.IsDeleted && c.ExcludedAt < dataLimite)
                .ToListAsync(token);
        
            if (consultasParaExcluir.Any())
            {
                _logger.LogWarning($"Excluindo fisicamente {consultasParaExcluir.Count} consultas antigas.");
                context.Consultas.RemoveRange(consultasParaExcluir);
            }

            var agendamentosParaExcluir = await context.Agendamentos
                .IgnoreQueryFilters()
                .Where(a => a.IsDeleted && a.ExcludedAt < dataLimite)
                .ToListAsync(token);

            if (agendamentosParaExcluir.Any())
                {
                _logger.LogWarning($"Excluindo fisicamente {agendamentosParaExcluir.Count} agendamentos antigos.");
                context.Agendamentos.RemoveRange(agendamentosParaExcluir);
            }

            var listaEsperaParaExcluir = await context.ListaEspera
                .IgnoreQueryFilters()
                .Where(l => l.IsDeleted && l.ExcludedAt < dataLimite)
                .ToListAsync(token);

            if (listaEsperaParaExcluir.Any())
                {
                _logger.LogWarning($"Excluindo fisicamente {listaEsperaParaExcluir.Count} registros antigos da lista de espera.");
                context.ListaEspera.RemoveRange(listaEsperaParaExcluir);
            }

            var pacientesParaExcluir = await context.Pacientes
                .IgnoreQueryFilters()
                .Where(p => p.IsDeleted && p.ExcludedAt < dataLimite)
                .ToListAsync(token);

            if (pacientesParaExcluir.Any())
                {
                _logger.LogWarning($"Excluindo fisicamente {pacientesParaExcluir.Count} pacientes antigos.");
                context.Pacientes.RemoveRange(pacientesParaExcluir);
            }

            var equipesParaExcluir = await context.Equipes
                .IgnoreQueryFilters()
                .Where(e => e.IsDeleted && e.ExcludedAt < dataLimite)
                .ToListAsync(token);

            if (equipesParaExcluir.Any())
                {
                _logger.LogWarning($"Excluindo fisicamente {equipesParaExcluir.Count} equipes antigas.");
                context.Equipes.RemoveRange(equipesParaExcluir);
            }

            var profissionaisParaExcluir = await context.Profissionais
                .IgnoreQueryFilters()
                .Where(p => p.IsDeleted && p.ExcludedAt < dataLimite)
                .ToListAsync(token);

            if (profissionaisParaExcluir.Any())
            {
                _logger.LogWarning($"Excluindo fisicamente {profissionaisParaExcluir.Count} profissionais antigos.");
                context.Profissionais.RemoveRange(profissionaisParaExcluir);
            }

            if (consultasParaExcluir.Any() || 
                agendamentosParaExcluir.Any() || 
                listaEsperaParaExcluir.Any() || 
                pacientesParaExcluir.Any() || 
                equipesParaExcluir.Any() || 
                profissionaisParaExcluir.Any())
            {
                await context.SaveChangesAsync(token);
                _logger.LogInformation("Limpeza concluída com sucesso.");
            }
            else
            {
                _logger.LogInformation("Nenhum registro antigo encontrado para exclusão hoje.");
            }
        }


    }
}