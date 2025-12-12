using Application.Interfaces;
using FluentValidation;

namespace Application.Handlers.Agendamentos.Commands.Create
{
    public class CreateAgendamentoCommandValidator : AbstractValidator<CreateAgendamentoCommand>
    {
        protected readonly IApplicationDbContext _context;
        public CreateAgendamentoCommandValidator(IApplicationDbContext context)
        {
            _context = context;

        }
    }
}
