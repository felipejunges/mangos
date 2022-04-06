using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class TransferenciaContaDadosValidator : AbstractValidator<TransferenciaContaDadosModel>
    {
        public TransferenciaContaDadosValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(255).WithMessage("O tamanho máximo da descrição é de 255 caracteres.");

            RuleFor(x => x.ContaBancariaOrigemId).NotEqual(x => x.ContaBancariaDestinoId)
                .WithMessage("A conta bancária destino deve ser diferente da conta bancária origem.");

            RuleFor(x => x.ContaBancariaOrigemId).NotNull().When(x => x.DataDebito != null)
                .WithMessage("A conta de origem deve ser informada.");

            RuleFor(x => x.ContaBancariaDestinoId).NotNull().When(x => x.DataCredito != null)
                .WithMessage("A conta de destino deve ser informada.");

            RuleFor(x => x.DataDebito).NotNull().When(x => x.DataCredito == null)
                .WithMessage("Ou a data débito ou a data crédito devem ser informadas.");

            RuleFor(x => x.DataCredito).NotNull().When(x => x.DataDebito == null)
                .WithMessage("Ou a data crédito ou a data débito devem ser informadas.");
        }
    }
}