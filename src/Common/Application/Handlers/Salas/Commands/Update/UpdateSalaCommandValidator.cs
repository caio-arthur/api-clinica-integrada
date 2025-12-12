using Application.Interfaces;

namespace Application.Handlers.Salas.Commands.Update
{
    public class UpdateSalaCommandValidator : SalaCommandValidator<UpdateSalaCommand>
    {
        protected readonly IApplicationDbContext _context;

        public UpdateSalaCommandValidator(IApplicationDbContext context)
            : base(context) {
            context = _context;

        }
    }
}
