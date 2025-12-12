using Application.Interfaces;
using FluentValidation;

namespace Application.Handlers.Pacientes.Commands.Create
{
    public class CreatePacienteCommandValidator : AbstractValidator<CreatePacienteCommand>
    {
        protected readonly IApplicationDbContext _context;
        public CreatePacienteCommandValidator(IApplicationDbContext context) {
            _context = context;

            RuleFor(v => v.Paciente.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(v => v.Paciente.Telefone)
                .NotEmpty().WithMessage("Telefone é obrigatório.");
            RuleFor(v => v.ListaEspera.Especialidade)
                .NotEmpty().WithMessage("Especialidade é obrigatória.");

            //RuleFor(v => v.ListaEspera.DataEntrada)
            //    .NotEmpty().WithMessage("Data de entrada é obrigatória quando a lista de espera é fornecida.")
            //    .Must(BeAValidDate).WithMessage("Data de entrada deve ser uma data válida e igual ou superior à data atual.");
        }

        //private bool BeAValidDate(DateTime date) {
        //    return date >= DateTime.Now.Date;
        //}
    }
}
