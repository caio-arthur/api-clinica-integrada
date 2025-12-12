using Application.Interfaces;
using FluentValidation;

namespace Application.Handlers.Pacientes.Commands
{
    public class PacienteCommandValidator<T> : AbstractValidator<T> where T : PacienteCommand
    {
        protected readonly IApplicationDbContext _context;

        public PacienteCommandValidator(IApplicationDbContext context) {
            _context = context;

            //RuleFor(v => v.Nome)
            //    .NotEmpty().WithMessage("Nome é obrigatório.");
            //RuleFor(v => v.Telefone)
            //    .NotEmpty().WithMessage("Telefone é obrigatório."); 

        }
    }
}
