using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class MetaMovimentacaoDadosModelBinder : IModelBuilder<MetaMovimentacaoDadosModel, MetaMovimentacao>
    {
        private readonly IMetaMovimentacaoRepository _metaMovimentacaoRepository;
        private readonly IUserResolverService _userResolverService;

        public MetaMovimentacaoDadosModelBinder(IMetaMovimentacaoRepository metaMovimentacaoRepository, IUserResolverService userResolverService)
        {
            _metaMovimentacaoRepository = metaMovimentacaoRepository;
            _userResolverService = userResolverService;
        }

        public async Task<MetaMovimentacao?> CriarEntidadeAsync(MetaMovimentacaoDadosModel metaMovimentacaoDadosModel)
        {
            if (!metaMovimentacaoDadosModel.CheckValidationHash())
                return null;

            MetaMovimentacao? metaMovimentacao;

            if (metaMovimentacaoDadosModel.Id == 0)
                metaMovimentacao = new MetaMovimentacao() { Id = 0, GrupoId = metaMovimentacaoDadosModel.GrupoId };
            else
                metaMovimentacao = await _metaMovimentacaoRepository.ObterMetaMovimentacaoAsync(metaMovimentacaoDadosModel.Id);

            if (metaMovimentacao is null)
                return null;

            metaMovimentacao.MesInicial = metaMovimentacaoDadosModel.MesInicial!.Value;
            metaMovimentacao.MesFinal = metaMovimentacaoDadosModel.MesFinal!.Value;
            metaMovimentacao.ValorMensal = metaMovimentacaoDadosModel.ValorMensal!.Value;

            return metaMovimentacao;
        }

        public async Task<MetaMovimentacaoDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return MetaMovimentacaoDadosModel.Novo(_userResolverService.GrupoId);
            }
            else
            {
                var metaMovimentacao = await _metaMovimentacaoRepository.ObterMetaMovimentacaoAsync(id.Value);

                if (metaMovimentacao == null)
                    return null;

                if (metaMovimentacao.GrupoId != _userResolverService.GrupoId)
                    return null;

                return new MetaMovimentacaoDadosModel(
                    id: metaMovimentacao.Id,
                    grupoId: metaMovimentacao.GrupoId,
                    valorMensal: metaMovimentacao.ValorMensal,
                    mesInicial: metaMovimentacao.MesInicial,
                    mesFinal: metaMovimentacao.MesFinal
                );
            }
        }

        public Task AtualizarAsync(MetaMovimentacaoDadosModel model)
        {
            return Task.CompletedTask;
        }
    }
}