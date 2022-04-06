using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class LancamentoPagamentoValidator : AbstractValidator<LancamentoPagamentoModel>
    {
        public LancamentoPagamentoValidator()
        {
            RuleFor(x => x.DataPagamento).NotEmpty().WithMessage("A data do pagamento deve ser informada.");
        }
    }
}