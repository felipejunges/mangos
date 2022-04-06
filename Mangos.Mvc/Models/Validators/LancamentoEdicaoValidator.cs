using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class LancamentoEdicaoValidator : AbstractValidator<LancamentoEdicaoModel>
    {
        public LancamentoEdicaoValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(255).WithMessage("O tamanho máximo da descrição é de 255 caracteres.");

            RuleFor(x => x.DataPagamento).NotEmpty().When(x => x.Pago)
                .WithMessage("A data do pagamento deve ser informada.");

            RuleFor(x => x.ValorPago).NotEmpty().When(x => x.Pago)
                .WithMessage("O valor pago deve ser informado.");

            RuleFor(x => x.DataContaBancaria).NotEmpty().When(x => x.Pago && x.ContaBancariaId != null)
                .WithMessage("A data do débito em conta deve ser informada.");

            RuleFor(x => x.ContaBancariaId).NotEmpty().When(x => x.Pago && x.DataContaBancaria != null)
                .WithMessage("A conta bancária deve ser informada.");
        }
    }
}