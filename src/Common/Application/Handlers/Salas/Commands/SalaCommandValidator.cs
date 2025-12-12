using Application.Interfaces;
using FluentValidation;

namespace Application.Handlers.Salas.Commands
{
    public class SalaCommandValidator<T> : AbstractValidator<T> where T : SalaCommand
    {
        protected readonly IApplicationDbContext _context;

        public SalaCommandValidator(IApplicationDbContext context) {
            _context = context;

            RuleFor(v => v.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(v => v.Especialidade)
                .NotEmpty().WithMessage("Especialidade é obrigatória.");
        }
    }
}
