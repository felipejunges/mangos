using FluentValidation;
using Mangos.Dominio.Services;
using Mangos.Mvc.Models.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.Validators
{
    public class RendimentoMensalContaDadosValidator : AbstractValidator<RendimentoMensalContaDadosModel>
    {
        private readonly ContaBancariaService ContaBancariaService;

        public RendimentoMensalContaDadosValidator(ContaBancariaService contaBancariaService)
        {
            this.ContaBancariaService = contaBancariaService;

            RuleFor(x => x.ContaBancariaId).NotEmpty().WithMessage("A conta bancária deve ser informada.");

            RuleFor(x => x.ContaBancariaId).MustAsync(MatchContaBancariaGrupoId).WithMessage("A conta bancária selecionada é inválida.");

            RuleFor(x => x.Valor).NotEmpty().WithMessage("O valor deve ser informado.");

            RuleFor(x => x.MesReferencia).NotEmpty().WithMessage("O mês de referência deve ser informado.");
        }

        private async Task<bool> MatchContaBancariaGrupoId(RendimentoMensalContaDadosModel rendimentoMensalConta, int? contaBancariaId, CancellationToken cancellationToken)
        {
            if (contaBancariaId == null)
                return false;

            return await this.ContaBancariaService.VerificarGrupoId(contaBancariaId.Value, rendimentoMensalConta.GrupoId);
        }
    }
}