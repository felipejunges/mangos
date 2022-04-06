using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class PessoaDadosValidator : AbstractValidator<PessoaDadosModel>
    {
        public PessoaDadosValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome é obrigatório.");
            RuleFor(x => x.Nome).MaximumLength(100).WithMessage("O tamanho máximo do nome é de 100 caracteres.");

            RuleFor(x => x.Telefone1).MaximumLength(20).WithMessage("O tamanho máximo do telefone 1 é de 20 caracteres.");
            RuleFor(x => x.Telefone2).MaximumLength(20).WithMessage("O tamanho máximo do telefone 2 é de 20 caracteres.");
            RuleFor(x => x.Telefone3).MaximumLength(20).WithMessage("O tamanho máximo do telefone 3 é de 20 caracteres.");

            RuleFor(x => x.Site).MaximumLength(100).WithMessage("O tamanho máximo do site é de 100 caracteres.");
            RuleFor(x => x.Email).MaximumLength(100).WithMessage("O tamanho máximo do e-mail é de 100 caracteres.");

            RuleFor(x => x.Tipo).NotNull().WithMessage("O tipo de pessoa é obrigatório.");
        }
    }
}