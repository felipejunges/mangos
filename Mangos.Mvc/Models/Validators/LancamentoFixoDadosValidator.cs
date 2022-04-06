using FluentValidation;
using Mangos.Dominio.Enums;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class LancamentoFixoDadosValidator : AbstractValidator<LancamentoFixoDadosModel>
    {
        public LancamentoFixoDadosValidator()
        {
            RuleFor(x => x.Tipo).NotNull().WithMessage("O tipo é obrigatório.");

            RuleFor(x => x.Periodicidade).NotNull().WithMessage("A periodicidade é obrigatória.");

            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(255).WithMessage("O tamanho máximo da descrição é de 255 caracteres.");

            RuleFor(x => x.DiaVencimento).NotNull().WithMessage("O dia do vencimento deve ser informado.");

            RuleFor(x => x.MesVencimento).Null().When(x => x.Periodicidade == PeriodicidadeLancamentoFixo.Mensal)
                .WithMessage("Não deve ser informado o mês de vencimento para esta periodicidade de lançamento.");
            RuleFor(x => x.MesVencimento).NotNull().When(x => x.Periodicidade == PeriodicidadeLancamentoFixo.Anual)
                .WithMessage("Deve ser informado o mês do vencimento.");

            RuleFor(x => x.CartaoCreditoId).NotNull().When(x => x.Tipo == TipoLancamentoFixo.ReceitaCartao || x.Tipo == TipoLancamentoFixo.DebitoCartao)
                .WithMessage("Deve ser selecionado o cartão de crédito.");
            RuleFor(x => x.CartaoCreditoId).Null().When(x => x.Tipo != TipoLancamentoFixo.ReceitaCartao && x.Tipo != TipoLancamentoFixo.DebitoCartao)
                .WithMessage("Não deve ser selecionado o cartão de crédito para este tipo de lançamento.");
        }
    }
}