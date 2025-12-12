using Application.Interfaces;

namespace Application.Handlers.ListaEsperaEntries.Commands.Update
{
    public class UpdateListaEsperaCommandValidator : ListaEsperaEntryCommandValidator<UpdateListaEsperaEntryCommand>
    {
        protected readonly IApplicationDbContext _context;

        public UpdateListaEsperaCommandValidator(IApplicationDbContext context)
                : base(context) 
        {


        }
    }
}
