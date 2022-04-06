using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class GrupoRepository : IGrupoRepository
    {
        private readonly MangosDb Db;

        public GrupoRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<Grupo?> ObterGrupoAsync(int id)
        {
            return Db.Grupos
                        .Where(g => g.Id == id)
                        .FirstOrDefaultAsync();
        }

        public async Task<GrupoRelacionamentos?> ObterGrupoComRelacionamentosAsync(int id)
        {
            var grupo = await Db.Grupos
                        .Where(g => g.Id == id)
                        .FirstOrDefaultAsync();

            if (grupo is null)
                return default;

            return new GrupoRelacionamentos(
                    id: grupo.Id,
                    descricao: grupo.Descricao,
                    pessoas: Db.Pessoas.Where(p => p.GrupoId == id).Count(),
                    usuarios: Db.Usuarios.Where(p => p.GrupoId == id).Count(),
                    categorias: Db.Categorias.Where(p => p.GrupoId == id).Count(),
                    contasBancarias: Db.ContasBancarias.Where(p => p.GrupoId == id).Count(),
                    cartoesCredito: Db.CartoesCredito.Where(p => p.GrupoId == id).Count(),
                    receitas: Db.Lancamentos.Where(p => p.GrupoId == id && p.Tipo == TipoLancamento.Receita).Count(),
                    despesas: Db.Lancamentos.Where(p => p.GrupoId == id && p.Tipo == TipoLancamento.Despesa).Count(),
                    lancamentosCartao: Db.LancamentosCartao.Where(p => p.GrupoId == id).Count(),
                    transferenciasConta: Db.TransferenciasContas.Where(p => p.GrupoId == id).Count(),
                    lancamentosFixos: Db.LancamentosFixos.Where(p => p.GrupoId == id).Count()
                );
        }

        public async Task<IEnumerable<Grupo>> ListarGruposAsync(string? descricao)
        {
            return await Db.Grupos
                        .Where(g =>
                            (string.IsNullOrEmpty(descricao) || g.Descricao.Contains(descricao))
                        )
                        .OrderBy(g => g.Descricao)
                        .ToListAsync();
        }

        public async Task IncluirAsync(Grupo grupo)
        {
            await Db.Grupos.AddAsync(grupo);
        }

        public Task AlterarAsync(Grupo grupo)
        {
            return Task.Run(() => Db.Grupos.Update(grupo));
        }

        public Task RemoverAsync(Grupo grupo)
        {
            return Task.Run(() => Db.Grupos.Remove(grupo));
        }
    }
}