using Application.Interfaces;

namespace Application.Handlers.Profissionais.Commands.Update
{
    public class UpdateProfissionalCommandValidator : ProfissionalCommandValidator<UpdateProfissionalCommand>
    {
        protected readonly IApplicationDbContext _context;

        public UpdateProfissionalCommandValidator(IApplicationDbContext context)
            : base(context) {
            context = _context;

        }
    }
}
