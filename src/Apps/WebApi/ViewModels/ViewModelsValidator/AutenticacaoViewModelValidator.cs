using FluentValidation;
using WebApi.ViewModels;

namespace WebApi.ViewModels.ViewModelsValidator
{
    public class AutenticacaoViewModelValidator : AbstractValidator<AutenticacaoViewModel>
    {
        public AutenticacaoViewModelValidator()
        {


            RuleFor(x => x.Email)
              .NotEmpty().WithMessage("O usuario é obigatório.")
              .NotNull().WithMessage("O usuário é obigatório.")
              .EmailAddress().WithMessage("E-mail inválido.");

            RuleFor(x => x.Senha)
                   .NotEmpty().WithMessage("A senha é obigatória.")
                   .Length(5, 20).WithMessage("A senha deve conter entre 5 e 20 caracteres.")
                   .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}
