using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class EsqueciSenhaValidator : AbstractValidator<EsqueciSenhaModel>
    {
        public EsqueciSenhaValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("O e-mail deve ser informado.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("O e-mail deve ser informado corretamente.");
        }
    }
}