using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class LancamentoFixoRepository : ILancamentoFixoRepository
    {
        private readonly MangosDb Db;

        public LancamentoFixoRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<LancamentoFixo?> ObterLancamentoFixoAsync(int id)
        {
            return Db.LancamentosFixos
                        .Include(l => l.Pessoa)
                        .Where(l => l.Id == id)
                        .FirstOrDefaultAsync();
        }

        public async Task<List<LancamentoFixo>> ListarLancamentosFixosAsync(int grupoId, string? descricao, bool buscarInativos)
        {
            var lancamentosFixos = await Db.LancamentosFixos
                        .Include(l => l.Pessoa)
                        .Where(l =>
                            l.GrupoId == grupoId
                            && (string.IsNullOrEmpty(descricao) || l.Descricao.Contains(descricao))
                            && (buscarInativos || l.Ativo)
                        )
                        .ToListAsync();

            lancamentosFixos.Sort();

            return lancamentosFixos;
        }

        public Task<List<LancamentoFixo>> ListarLancamentosFixosRelatorioAsync(int grupoId)
        {
            return Db.LancamentosFixos
                    .Include(l => l.Pessoa)
                    .Where(l => l.GrupoId == grupoId)
                    .OrderByDescending(l => l.DataHoraUltimaGeracao)
                    .ToListAsync();
        }

        public async Task<List<LancamentoFixo>> ListarLancamentosFixosGerarAsync(int? grupoId, int? id)
        {
            var lancamentosFixos =
                    await Db.LancamentosFixos
                        .Where(l =>
                            l.Ativo
                            && (grupoId == null || l.GrupoId == grupoId.Value)
                            && (id == null || l.Id == id)
                        )
                        .ToListAsync();

            lancamentosFixos.Sort();

            return lancamentosFixos;
        }

        public async Task IncluirAsync(LancamentoFixo lancamentoFixo)
        {
            await Db.LancamentosFixos.AddAsync(lancamentoFixo);
        }

        public Task AlterarAsync(LancamentoFixo lancamentoFixo)
        {
            return Task.Run(() => Db.LancamentosFixos.Update(lancamentoFixo));
        }

        public Task RemoverAsync(LancamentoFixo lancamentoFixo)
        {
            return Task.Run(() => Db.LancamentosFixos.Remove(lancamentoFixo));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var lancamentosFixos = await Db.LancamentosFixos.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.LancamentosFixos.RemoveRange(lancamentosFixos);
        }
    }
}