using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Agendamentos.Commands.Delete
{
    public class DeleteAgendamentoCommand : IRequestWrapper<string>
    {
        public Guid Id { get; set; }
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
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("Agendamento não encontrado");
                }

                // caso a consulta possua status agendada, também deve ser marcada para exclusão
                if (entity.Consulta != null && entity.Consulta.Status == ConsultaStatus.Agendada) {
                    entity.Consulta.ExcludedAt = _dateTime.Now;
                    entity.Consulta.IsDeleted = true;
                    _context.Consultas.Update(entity.Consulta);
                }

                entity.ExcludedAt = _dateTime.Now;
                entity.IsDeleted = true;
                _context.Agendamentos.Update(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success("Ok");
            } catch (Exception e) {
                throw;
            }
        }
    }

}
