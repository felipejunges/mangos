using FluentValidation;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Validators
{
    public class ReaberturaMesCartaoValidator : AbstractValidator<ReaberturaMesCartaoModel>
    {
        public ReaberturaMesCartaoValidator()
        {
            RuleFor(x => x.CartaoCreditoId).NotEmpty();

            RuleFor(x => x.MesReferencia).NotEmpty();
        }
    }
}