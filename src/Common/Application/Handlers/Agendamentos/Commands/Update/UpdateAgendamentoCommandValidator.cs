using Application.Interfaces;

namespace Application.Handlers.Agendamentos.Commands.Update
{
    public class UpdateAgendamentoCommandValidator : AgendamentoCommandValidator<UpdateAgendamentoCommand>
    {
        protected readonly IApplicationDbContext _context;

        public UpdateAgendamentoCommandValidator(IApplicationDbContext context)
            : base(context) {
            context = _context;

        }

    }
}
