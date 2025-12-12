using Application.Interfaces;
using FluentValidation;

namespace Application.Handlers.ListaEsperaEntries.Commands
{
    public class ListaEsperaEntryCommandValidator<T> : AbstractValidator<T> where T : ListaEsperaEntryCommand
    {
        protected readonly IApplicationDbContext _context;

        public ListaEsperaEntryCommandValidator(IApplicationDbContext context) {
            _context = context;

            RuleFor(v => v.PacienteId)
                .NotEmpty().WithMessage("Paciente é obrigatório.");
            //RuleFor(v => v.Especialidade)
            //    .NotEmpty().WithMessage("Especialidade é obrigatória.");    

        }
    }
}
