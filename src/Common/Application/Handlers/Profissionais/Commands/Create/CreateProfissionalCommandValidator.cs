using Application.Interfaces;

namespace Application.Handlers.Profissionais.Commands.Create
{
    public class CreateProfissionalCommandValidator : ProfissionalCommandValidator<CreateProfissionalCommand>
    {
        protected readonly IApplicationDbContext _context;
        public CreateProfissionalCommandValidator(IApplicationDbContext context)
            : base(context)    
        {
            
        }
    }
}
