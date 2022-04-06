using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class CategoriaDadosValidator : AbstractValidator<CategoriaDadosModel>
    {
        public CategoriaDadosValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(100).WithMessage("O tamanho máximo da descrição é de 100 caracteres.");

            RuleFor(x => x.CategoriaSuperiorId).NotEqual(x => x.Id).When(x => x.CategoriaSuperiorId != null)
                .WithMessage("A categoria não pode ser superior de si mesma.");
        }
    }
}