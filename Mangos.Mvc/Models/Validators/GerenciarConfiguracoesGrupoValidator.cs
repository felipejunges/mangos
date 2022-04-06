using FluentValidation;
using Mangos.Mvc.Models.GerenciarConta;

namespace Mangos.Mvc.Models.Validators
{
    public class GerenciarConfiguracoesGrupoValidator : AbstractValidator<GerenciarConfiguracoesGrupoModel>
    {
        public GerenciarConfiguracoesGrupoValidator()
        {
            RuleFor(x => x.MesesAntecedenciaGerarLancamento).GreaterThanOrEqualTo(0).WithMessage("A antecedência deve ser positiva.");
            RuleFor(x => x.MesesAntecedenciaGerarLancamentoCartao).GreaterThanOrEqualTo(0).WithMessage("A antecedência deve ser positiva.");
            RuleFor(x => x.MesesGraficosDashboard).GreaterThanOrEqualTo(1).WithMessage("A quantidade de meses deve ser superior ou igual à '1'.");
        }
    }
}