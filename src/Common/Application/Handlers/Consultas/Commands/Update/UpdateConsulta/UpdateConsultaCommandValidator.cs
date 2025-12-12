using Application.Interfaces;

namespace Application.Handlers.Consultas.Commands.Update.UpdateConsulta
{
    public class UpdateConsultaCommandValidator : ConsultaCommandValidator<UpdateConsultaCommand>
    {
        protected readonly IApplicationDbContext _context;

        public UpdateConsultaCommandValidator(IApplicationDbContext context)
            : base(context) {
            context = _context;

        }
    }
}
