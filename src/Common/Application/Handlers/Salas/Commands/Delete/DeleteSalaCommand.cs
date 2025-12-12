using Application.Interfaces;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Salas.Commands.Delete
{
    public class DeleteSalaCommand : IRequestWrapper<string>
    {
        public Guid Id { get; set; }
    }

    public class DeleteSalaCommandHandler : IRequestHandlerWrapper<DeleteSalaCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public DeleteSalaCommandHandler(IApplicationDbContext context,
                                        IDateTime dateTime
                                        ) {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<ServiceResult<string>> Handle(DeleteSalaCommand request, CancellationToken cancellationToken) {
            try {
                var entity = await _context.Salas
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("Sala não encontrada");
                }

                entity.ExcludedAt = _dateTime.Now;
                entity.IsDeleted = true;
                _context.Salas.Update(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success("Ok");
            } catch (Exception e) {
                throw;
            }
        }
    }

}
