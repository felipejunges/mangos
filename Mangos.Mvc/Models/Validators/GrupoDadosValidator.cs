using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class GrupoDadosValidator : AbstractValidator<GrupoDadosModel>
    {
        public GrupoDadosValidator()
        {
            RuleFor(x => x.Descricao).NotNull().WithMessage("A descrição deve ser informada.");
            RuleFor(x => x.Descricao).MaximumLength(100).WithMessage("O tamanho máximo da descrição é de 100 caracteres.");

            RuleFor(x => x.MesesAntecedenciaGerarLancamento).GreaterThanOrEqualTo(0).WithMessage("A antecedência deve ser positiva.");
            RuleFor(x => x.MesesAntecedenciaGerarLancamentoCartao).GreaterThanOrEqualTo(0).WithMessage("A antecedência deve ser positiva.");
            RuleFor(x => x.MesesGraficosDashboard).GreaterThanOrEqualTo(1).WithMessage("A quantidade de meses deve ser superior ou igual à '1'.");
        }
    }
}