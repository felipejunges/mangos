using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Dominio.Validators
{
    public class TrocarSenhaValidator : AbstractValidator<TrocarSenhaModel>
    {
        public TrocarSenhaValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token inválido.");
            RuleFor(x => x.Senha).NotEmpty().WithMessage("A senha deve ser informada.");
            RuleFor(x => x.ConfirmeSenha).Equal(x => x.Senha).WithMessage("A senha e a confirmação devem ser iguais.");
        }
    }
}