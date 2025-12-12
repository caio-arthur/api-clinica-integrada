using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Agendamentos.Commands.Delete.CancelarAgendamento
{
    public class CancelarAgendamentoCommand : IRequestWrapper<string>
    {
        public Guid Id { get; set; }
    }

    public class CancelarAgendamentoCommandHandler : IRequestHandlerWrapper<CancelarAgendamentoCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CancelarAgendamentoCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<string>> Handle(CancelarAgendamentoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Agendamentos
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null)
                {
                    throw new Exception("Agendamento não encontrado");
                }

                if (entity.PacienteId != null)
                {
                    await AtualizarEtapaPaciente((Guid)entity.PacienteId, cancellationToken);
                }

                var consulta = await _context.Consultas.FirstOrDefaultAsync(x => x.AgendamentoId == entity.Id);
                consulta.Status = ConsultaStatus.Cancelada;
                entity.Status = AgendamentoStatus.Cancelado;

                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success("Ok");

            }
            catch (Exception e)
            {
                throw;

            }
        }

        public async Task AtualizarEtapaPaciente(Guid pacienteId, CancellationToken cancellationToken)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(x => x.Id == pacienteId, cancellationToken);
            if (paciente == null)
            {
                throw new Exception("Paciente não encontrado.");
            }
            paciente.Etapa = PacienteEtapa.ConsultaCancelada;
        }
    }
}
