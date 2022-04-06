using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Dominio.Validators
{
    public class CartaoCreditoDadosValidator : AbstractValidator<CartaoCreditoDadosModel>
    {
        public CartaoCreditoDadosValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(100).WithMessage("O tamanho máximo da descrição é de 100 caracteres.");

            RuleFor(x => x.DiaFechamento).Must(x => x >= 1 && x <= 31).WithMessage("Dia do fechamento inválido.");
            RuleFor(x => x.DiaVencimento).Must(x => x >= 1 && x <= 31).WithMessage("Dia do vencimento inválido.");
        }
    }
}