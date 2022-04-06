using Mangos.Dominio.Constants;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly MangosDb Db;

        public CategoriaRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<Categoria?> ObterCartaoCreditoAsync(int id)
        {
            return Db.Categorias
                        .Where(c => c.Id == id)
                        .FirstOrDefaultAsync();
        }

        public async Task<List<Categoria>> ListarCategoriasAsync(int grupoId, string? descricao, string tipo, bool buscarInativas, int? idInativo = null)
        {
            var categorias = await Db.Categorias
                    .Include(c => c.CategoriaSuperior)
                    .Where(c =>
                        c.GrupoId == grupoId
                        && (
                            tipo == TipoCategoriaPesquisa.Todos
                            || (tipo == TipoCategoriaPesquisa.Receita && c.Receita)
                            || (tipo == TipoCategoriaPesquisa.Despesa && c.Despesa)
                        )
                        && (buscarInativas || c.Ativo || c.Id == idInativo)
                    )
                    .Where(c => string.IsNullOrEmpty(descricao) || (c.Descricao != null && c.Descricao.Contains(descricao)))
                    .ToListAsync();

            categorias.Sort((a,b) => a.DescricaoComSuperior.CompareTo(b.DescricaoComSuperior));

            return categorias;
        }

        public Task<List<Categoria>> ListarCategoriasSuperioresAsync(int grupoId, bool buscarInativas, int? idInativo = null)
        {
            return Db.Categorias
                        .Where(c =>
                            c.CategoriaSuperiorId == null
                            && c.GrupoId == grupoId
                            && (buscarInativas || c.Ativo || c.Id == idInativo)
                        )
                        .OrderBy(o => o.Descricao)
                        .ToListAsync();
        }

        public async Task<bool> ValidarCategoriasTemFilhasAsync(int id)
        {
            var quantidade = await Db.Categorias
                        .Where(c => c.CategoriaSuperiorId == id)
                        .CountAsync();

            return quantidade > 0;
        }

        public async Task IncluirAsync(Categoria categoria)
        {
            await Db.Categorias.AddAsync(categoria);
        }

        public Task AlterarAsync(Categoria categoria)
        {
            return Task.Run(() => Db.Categorias.Update(categoria));
        }

        public Task RemoverAsync(Categoria categoria)
        {
            return Task.Run(() => Db.Categorias.Remove(categoria));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var categorias = await Db.Categorias.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.Categorias.RemoveRange(categorias);
        }
    }
}