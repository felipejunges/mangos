using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class MetaMovimentacaoDadosValidator : AbstractValidator<MetaMovimentacaoDadosModel>
    {
        public MetaMovimentacaoDadosValidator()
        {
            RuleFor(x => x.MesInicial).NotNull().WithMessage("O mês inicial é obrigatório.");

            RuleFor(x => x.MesFinal).NotNull().WithMessage("O mês final é obrigatório.");

            RuleFor(x => x.MesInicial).LessThanOrEqualTo(x => x.MesFinal).WithMessage("O mês inicial não pode ser superior ao mês final.");

            RuleFor(x => x.ValorMensal).NotNull().WithMessage("O valor mensal é obrigatório.");
        }
    }
}