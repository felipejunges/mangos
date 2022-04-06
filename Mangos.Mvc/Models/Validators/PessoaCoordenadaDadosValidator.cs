using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class PessoaCoordenadaDadosValidator : AbstractValidator<PessoaCoordenadaDadosModel>
    {
        public PessoaCoordenadaDadosValidator()
        {
            RuleFor(x => x.Latitude).NotNull().WithMessage("A latitude é obrigatória.");
            RuleFor(x => x.Longitude).NotNull().WithMessage("A longitude é obrigatória.");

            RuleFor(x => x.Latitude).GreaterThanOrEqualTo(-90).LessThanOrEqualTo(90).WithMessage("A latitude informada é inválida.");
            RuleFor(x => x.Longitude).GreaterThanOrEqualTo(-90).LessThanOrEqualTo(90).WithMessage("A longitude informada é inválida.");

            RuleFor(x => x.Observacao).MaximumLength(200).WithMessage("O tamanho máximo da descrição é de 200 caracteres.");
        }
    }
}