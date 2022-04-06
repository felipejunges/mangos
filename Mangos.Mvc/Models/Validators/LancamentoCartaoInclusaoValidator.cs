using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class LancamentoCartaoInclusaoValidator : AbstractValidator<LancamentoCartaoInclusaoModel>
    {
        public LancamentoCartaoInclusaoValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(255).WithMessage("O tamanho máximo da descrição é de 255 caracteres.");

            RuleFor(x => x.CartaoCreditoId).NotEmpty().WithMessage("O cartão de crédito deve ser informado.");

            RuleFor(x => x.DataReferencia).NotEmpty().WithMessage("A data referência é obrigatória.");

            RuleFor(x => x.TipoLancamento).NotNull().WithMessage("O tipo de lançamento deve ser informado.");

            RuleFor(x => x.NumeroParcelas).NotNull().When(x => x.Parcelado)
                .WithMessage("O número de parcelas deve ser informado.");
        }
    }
}