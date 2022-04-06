using FluentValidation;

namespace Mangos.Dominio.Commands.DespesasRapidas
{
    public class IncluirDespesaRapidaCommandValidator : AbstractValidator<IncluirDespesaRapidaCommand>
    {
        public IncluirDespesaRapidaCommandValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(100).WithMessage("O tamanho máximo da descrição é de 100 caracteres.");

            RuleFor(x => x.Valor).NotEmpty().WithMessage("O valor deve ser informado");

            RuleFor(x => x.PessoaId).NotEmpty().When(x => x.AtualizarCoordenadas);
            RuleFor(x => x.Latitude).NotEmpty().When(x => x.AtualizarCoordenadas);
            RuleFor(x => x.Longitude).NotEmpty().When(x => x.AtualizarCoordenadas);

            RuleFor(x => x.ContaBancariaId).Empty().When(x => x.CartaoCreditoId != null)
                .WithMessage("A conta bancária não deve ser selecionada se lançamento é de cartão de crédito.");

            RuleFor(x => x.CartaoCreditoId).Empty().When(x => x.ContaBancariaId != null)
                .WithMessage("O cartão não deve ser selecionado se lançamento é em conta bancária.");
        }
    }
}