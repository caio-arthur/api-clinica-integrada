using Application.Interfaces;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Consultas.Commands.Delete
{
    public class DeleteConsultaCommand : IRequestWrapper<string>
    {
        public Guid Id { get; set; }
    }

    public class DeleteConsultaCommandHandler : IRequestHandlerWrapper<DeleteConsultaCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public DeleteConsultaCommandHandler(IApplicationDbContext context,
                                        IDateTime dateTime
                                        ) {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<ServiceResult<string>> Handle(DeleteConsultaCommand request, CancellationToken cancellationToken) {
            try {
                var entity = await _context.Consultas
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("Consulta não encontrada");
                }

                entity.ExcludedAt = _dateTime.Now;
                entity.IsDeleted = true;
                _context.Consultas.Update(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success("Ok");
            } catch (Exception e) {
                throw;
            }
        }
    }

}
