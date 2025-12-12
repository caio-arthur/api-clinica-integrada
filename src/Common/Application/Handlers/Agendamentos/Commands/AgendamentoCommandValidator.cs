using Application.Interfaces;
using FluentValidation;

namespace Application.Handlers.Agendamentos.Commands
{
    public class AgendamentoCommandValidator<T> : AbstractValidator<T> where T : AgendamentoCommand
    {
        protected readonly IApplicationDbContext _context;

        public AgendamentoCommandValidator(IApplicationDbContext context) {
            _context = context;

            RuleFor(v => v.DataHoraInicio)
                .NotEmpty().WithMessage("DataHoraInicio é obrigatório.");
            RuleFor(v => v.DataHoraFim)
                .NotEmpty().WithMessage("DataHoraFim é obrigatório.");
            RuleFor(v => v.Tipo)
                .NotEmpty().WithMessage("Tipo é obrigatório.");
            RuleFor(v => v.Status)
                .NotEmpty().WithMessage("Status é obrigatório.");

            RuleFor(v => v)
                .Must(v => v.PacienteId.HasValue || !string.IsNullOrEmpty(v.NomePaciente) || !string.IsNullOrEmpty(v.NomeEquipe))
                .WithMessage("É necessário informar o Paciente OU (Nome do Aluno / Nome da Equipe).");
        }
    }
}
