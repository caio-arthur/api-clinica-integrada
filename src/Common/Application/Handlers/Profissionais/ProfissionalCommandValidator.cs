using Application.Interfaces;
using FluentValidation;

namespace Application.Handlers.Profissionais
{
    public class ProfissionalCommandValidator<T> : AbstractValidator<T> where T : ProfissionalCommand
    {
        protected readonly IApplicationDbContext _context;

        public ProfissionalCommandValidator(IApplicationDbContext context) {
            _context = context;

            RuleFor(v => v.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.");

            RuleFor(v => v.Tipo)
                .NotEmpty().WithMessage("Tipo é obrigatório.");
            RuleFor(v => v.Especialidade)
                .NotEmpty().WithMessage("Especialidade é obrigatório.");
        }
    }
}
