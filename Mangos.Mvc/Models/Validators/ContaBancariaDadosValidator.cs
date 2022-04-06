using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class ContaBancariaDadosValidator : AbstractValidator<ContaBancariaDadosModel>
    {
        public ContaBancariaDadosValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(100).WithMessage("O tamanho máximo da descrição é de 100 caracteres.");

            RuleFor(x => x.NumeroBanco).MaximumLength(4).WithMessage("O tamanho máximo do número do banco é de 4 caracteres.");
            RuleFor(x => x.Agencia).MaximumLength(8).WithMessage("O tamanho máximo da agência é de 8 caracteres.");
            RuleFor(x => x.NumeroConta).MaximumLength(20).WithMessage("O tamanho máximo da conta é de 20 caracteres.");
        }
    }
}