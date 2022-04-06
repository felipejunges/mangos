using Mangos.Dominio.Constants;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class ContaBancariaDadosModelBuilder : IModelBuilder<ContaBancariaDadosModel, ContaBancaria>
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUserResolverService _userResolverService;

        public ContaBancariaDadosModelBuilder(IContaBancariaRepository contaBancariaRepository, ICategoriaRepository categoriaRepository, IUserResolverService userResolverService)
        {
            _contaBancariaRepository = contaBancariaRepository;
            _categoriaRepository = categoriaRepository;
            _userResolverService = userResolverService;
        }

        public async Task<ContaBancaria?> CriarEntidadeAsync(ContaBancariaDadosModel contaBancariaDadosModel)
        {
            if (!contaBancariaDadosModel.CheckValidationHash())
                return null;

            ContaBancaria? contaBancaria;

            if (contaBancariaDadosModel.Id == 0)
                contaBancaria = new ContaBancaria() { Id = 0, GrupoId = contaBancariaDadosModel.GrupoId };
            else
                contaBancaria = await _contaBancariaRepository.ObterContaBancariaAsync(contaBancariaDadosModel.Id);

            if (contaBancaria is null)
                return null;

            if (contaBancaria.GrupoId != _userResolverService.GrupoId)
                return null;

            contaBancaria.Descricao = contaBancariaDadosModel.Descricao!;
            contaBancaria.NumeroBanco = contaBancariaDadosModel.NumeroBanco;
            contaBancaria.Agencia = contaBancariaDadosModel.Agencia;
            contaBancaria.NumeroConta = contaBancariaDadosModel.NumeroConta;
            contaBancaria.SaldoInicial = contaBancariaDadosModel.SaldoInicial;
            contaBancaria.CategoriaRendimentoMensalId = contaBancariaDadosModel.CategoriaRendimentoMensalId;
            contaBancaria.RelatorioProjecaoSaldo = contaBancariaDadosModel.RelatorioProjecaoSaldo;
            contaBancaria.PularFinaisSemanaLancamentoRapido = contaBancariaDadosModel.PularFinaisSemanaLancamentoRapido;
            contaBancaria.Ativo = contaBancariaDadosModel.Ativo;

            return contaBancaria;
        }

        public async Task<ContaBancariaDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return ContaBancariaDadosModel.Novo(
                    _userResolverService.GrupoId,
                    await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, null, TipoCategoriaPesquisa.Todos, false)
                );
            }
            else
            {
                var contaBancaria = await _contaBancariaRepository.ObterContaBancariaAsync(id.Value);

                if (contaBancaria == null)
                    return null;

                if (contaBancaria.GrupoId != _userResolverService.GrupoId)
                    return null;

                return new ContaBancariaDadosModel(
                    id: contaBancaria.Id,
                    grupoId: contaBancaria.GrupoId,
                    descricao: contaBancaria.Descricao,
                    numeroBanco: contaBancaria.NumeroBanco,
                    agencia: contaBancaria.Agencia,
                    numeroConta: contaBancaria.NumeroConta,
                    saldoInicial: contaBancaria.SaldoInicial,
                    categoriaRendimentoMensalId: contaBancaria.CategoriaRendimentoMensalId,
                    relatorioProjecaoSaldo: contaBancaria.RelatorioProjecaoSaldo,
                    pularFinaisSemanaLancamentoRapido: contaBancaria.PularFinaisSemanaLancamentoRapido,
                    ativo: contaBancaria.Ativo,
                    categoriasRendimentoMensal: await _categoriaRepository.ListarCategoriasAsync(contaBancaria.GrupoId, null, TipoCategoriaPesquisa.Todos, false, contaBancaria.CategoriaRendimentoMensalId)
                );
            }
        }

        public async Task AtualizarAsync(ContaBancariaDadosModel model)
        {
            model.AtualizarCategorias(
                await _categoriaRepository.ListarCategoriasAsync(model.GrupoId, null, TipoCategoriaPesquisa.Todos, false, model.CategoriaRendimentoMensalId)
            );
        }
    }
}