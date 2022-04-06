using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class LancamentoPagoInclusaoValidator : AbstractValidator<LancamentoPagoInclusaoModel>
    {
        public LancamentoPagoInclusaoValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(255).WithMessage("O tamanho máximo da descrição é de 255 caracteres.");

            RuleFor(x => x.DataPagamento).NotEmpty().WithMessage("A data do pagamento deve ser informada.");
        }
    }
}