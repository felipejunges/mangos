using Mangos.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface ILancamentoFixoRepository
    {
        Task<LancamentoFixo?> ObterLancamentoFixoAsync(int id);
        Task<List<LancamentoFixo>> ListarLancamentosFixosAsync(int grupoId, string? descricao, bool buscarInativos);
        Task<List<LancamentoFixo>> ListarLancamentosFixosRelatorioAsync(int grupoId);
        Task<List<LancamentoFixo>> ListarLancamentosFixosGerarAsync(int? grupoId, int? id);
        Task IncluirAsync(LancamentoFixo lancamentoFixo);
        Task AlterarAsync(LancamentoFixo lancamentoFixo);
        Task RemoverAsync(LancamentoFixo lancamentoFixo);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}