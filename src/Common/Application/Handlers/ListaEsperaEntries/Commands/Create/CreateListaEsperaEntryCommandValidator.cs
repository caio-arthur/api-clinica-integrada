using Application.Interfaces;
using FluentValidation;

namespace Application.Handlers.ListaEsperaEntries.Commands.Create
{
    public class CreateListaEsperaEntryCommandValidator : ListaEsperaEntryCommandValidator<CreateListaEsperaEntryCommand>
    {
        protected readonly IApplicationDbContext _context;

        public CreateListaEsperaEntryCommandValidator(IApplicationDbContext context)
                : base(context)
        {
            RuleFor(v => v.DataEntrada)
                .NotEmpty().WithMessage("Data de entrada é obrigatória.");

        }

    }
}
