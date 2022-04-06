using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class MetaSaldoDadosValidator : AbstractValidator<MetaSaldoDadosModel>
    {
        public MetaSaldoDadosValidator()
        {
            RuleFor(x => x.Mes).NotNull().WithMessage("O mês é obrigatório.");

            RuleFor(x => x.Valor).NotNull().WithMessage("O valor é obrigatório.");
        }
    }
}