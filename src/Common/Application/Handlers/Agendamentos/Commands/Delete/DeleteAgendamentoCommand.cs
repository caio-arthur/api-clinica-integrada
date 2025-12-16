using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Application.Handlers.Agendamentos.Commands.Delete
{
    public class DeleteAgendamentoCommand : IRequestWrapper<string>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public bool RetornarPacienteListaEspera { get; set; } = true;
    }

    public class DeleteAgendamentoCommandHandler : IRequestHandlerWrapper<DeleteAgendamentoCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public DeleteAgendamentoCommandHandler(IApplicationDbContext context,
                                        IDateTime dateTime
                                        ) {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<ServiceResult<string>> Handle(DeleteAgendamentoCommand request, CancellationToken cancellationToken) {
            try {
                var entity = await _context.Agendamentos
                    .Where(p => !p.IsDeleted)
                    .Include(p => p.Consulta)
                    .Include(p => p.Paciente)
                    .FirstOrDefaultAsync(p => p.Id == request.Id,cancellationToken) ?? throw new Exception("Agendamento não encontrado");

                // caso a consulta possua status agendada, também deve ser marcada para exclusão
                if (entity.Consulta != null && entity.Consulta.Status == ConsultaStatus.Agendada) {
                    entity.Consulta.ExcludedAt = _dateTime.Now;
                    entity.Consulta.IsDeleted = true;
                }

                if (request.RetornarPacienteListaEspera && entity.Paciente != null) 
                {
                    var lista = await _context.ListaEspera
                        .Where(p => !p.IsDeleted && p.PacienteId == entity.PacienteId)
                        .OrderByDescending(p => p.Created)
                        .FirstOrDefaultAsync(cancellationToken);

                    lista.Status = ListaStatus.Aguardando;
                    lista.DataEntrada = DateTime.Now;

                    entity.Paciente.Etapa = PacienteEtapa.ListaEspera;
                }
                else
                {
                    if (entity.Paciente != null)
                    {
                        entity.Paciente.Etapa = PacienteEtapa.ConsultaCancelada;

                    }
                }

                entity.ExcludedAt = _dateTime.Now;
                entity.IsDeleted = true;

                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success("Ok");
            } catch (Exception e) {
                throw;
            }
        }
    }

}
