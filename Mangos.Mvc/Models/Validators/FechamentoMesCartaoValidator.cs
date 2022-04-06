using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class FechamentoMesCartaoValidator : AbstractValidator<FechamentoMesCartaoModel>
    {
        public FechamentoMesCartaoValidator()
        {
            RuleFor(x => x.CartaoCreditoId).NotEmpty();

            RuleFor(x => x.MesReferencia).NotEmpty();
        }
    }
}