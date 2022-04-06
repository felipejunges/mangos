using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class GrupoDadosModelBuilder : IModelBuilder<GrupoDadosModel, Grupo>
    {
        private readonly IGrupoRepository _grupoRepository;

        public GrupoDadosModelBuilder(IGrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
        }

        public async Task<Grupo?> CriarEntidadeAsync(GrupoDadosModel grupoDadosModel)
        {
            Grupo? grupo;

            if (grupoDadosModel.Id == 0)
                grupo = new Grupo() { Id = 0 };
            else
                grupo = await _grupoRepository.ObterGrupoAsync(grupoDadosModel.Id);

            if (grupo is null)
                return null;

            grupo.Descricao = grupoDadosModel.Descricao!;
            grupo.MesesAntecedenciaGerarLancamento = grupoDadosModel.MesesAntecedenciaGerarLancamento;
            grupo.MesesAntecedenciaGerarLancamentoCartao = grupoDadosModel.MesesAntecedenciaGerarLancamentoCartao;
            grupo.MesesGraficosDashboard = grupoDadosModel.MesesGraficosDashboard;

            return grupo;
        }

        public async Task<GrupoDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return GrupoDadosModel.Novo();
            }
            else
            {
                var grupo = await _grupoRepository.ObterGrupoAsync(id.Value);

                if (grupo == null)
                    return null;

                return new GrupoDadosModel(
                    id: grupo.Id,
                    descricao: grupo.Descricao,
                    mesesAntecedenciaGerarLancamento: grupo.MesesAntecedenciaGerarLancamento,
                    mesesAntecedenciaGerarLancamentoCartao: grupo.MesesAntecedenciaGerarLancamentoCartao,
                    mesesGraficosDashboard: grupo.MesesGraficosDashboard
                );
            }
        }

        public Task AtualizarAsync(GrupoDadosModel model)
        {
            return Task.CompletedTask;
        }
    }
}