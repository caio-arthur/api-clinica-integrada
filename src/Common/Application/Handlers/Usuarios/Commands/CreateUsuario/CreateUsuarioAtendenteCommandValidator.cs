using FluentValidation;

namespace Application.Handlers.Usuarios.Commands.CreateUsuario
{
    public class CreateUsuarioAtendenteCommandValidator : AbstractValidator<CreateUsuarioAtendenteCommand>
    {
        public CreateUsuarioAtendenteCommandValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email deve ser um endereço válido.")
                .MaximumLength(100).WithMessage("O email não pode exceder 100 caracteres.");
            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.")
                .MaximumLength(100).WithMessage("A senha não pode exceder 100 caracteres.");
        }
    }
}
