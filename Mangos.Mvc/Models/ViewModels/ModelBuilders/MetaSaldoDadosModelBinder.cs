using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class MetaSaldoDadosModelBinder : IModelBuilder<MetaSaldoDadosModel, MetaSaldo>
    {
        private readonly IMetaSaldoRepository _metaSaldoRepository;
        private readonly IUserResolverService _userResolverService;

        public MetaSaldoDadosModelBinder(IMetaSaldoRepository metaSaldoRepository, IUserResolverService userResolverService)
        {
            _metaSaldoRepository = metaSaldoRepository;
            _userResolverService = userResolverService;
        }

        public async Task<MetaSaldo?> CriarEntidadeAsync(MetaSaldoDadosModel metaSaldoDadosModel)
        {
            if (!metaSaldoDadosModel.CheckValidationHash())
                return null;

            MetaSaldo? metaSaldo;

            if (metaSaldoDadosModel.Id == 0)
                metaSaldo = new MetaSaldo() { Id = 0, GrupoId = metaSaldoDadosModel.GrupoId };
            else
                metaSaldo = await _metaSaldoRepository.ObterMetaSaldoAsync(metaSaldoDadosModel.Id);

            if (metaSaldo is null)
                return null;

            metaSaldo.Valor = metaSaldoDadosModel.Valor!.Value;
            metaSaldo.Mes = metaSaldoDadosModel.Mes!.Value;

            return metaSaldo;
        }

        public async Task<MetaSaldoDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return MetaSaldoDadosModel.Novo(_userResolverService.GrupoId);
            }
            else
            {
                var metaSaldo = await _metaSaldoRepository.ObterMetaSaldoAsync(id.Value);

                if (metaSaldo == null)
                    return null;

                if (metaSaldo.GrupoId != _userResolverService.GrupoId)
                    return null;

                return new MetaSaldoDadosModel(
                    id: metaSaldo.Id,
                    grupoId: metaSaldo.GrupoId,
                    valor: metaSaldo.Valor,
                    mes: metaSaldo.Mes
                );
            }
        }

        public Task AtualizarAsync(MetaSaldoDadosModel model)
        {
            return Task.CompletedTask;
        }
    }
}