using Mangos.Dominio.Constants;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Models.Result;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly MangosDb Db;

        public PessoaRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<Pessoa?> ObterPessoaAsync(int id)
        {
            return Db.Pessoas
                        .Where(p => p.Id == id)
                        .FirstOrDefaultAsync();
        }

        public Task<List<Pessoa>> ListarPessoasAsync(int grupoId, string nome, string tipo, bool buscarInativos, int? idInativo = null)
        {
            return Db.Pessoas
                    .Where(GerarExpressionBuscaPessoa(grupoId, nome, tipo, buscarInativos, idInativo))
                    .OrderBy(p => p.Nome)
                    .ToListAsync();
        }

        public async Task<PaginatedResult<Pessoa>> ListarPessoasPaginatedAsync(int grupoId, string? nome, string tipo, int pagina, int itensPorPagina, bool buscarInativos, int? idInativo = null)
        {
            int skip = (pagina - 1) * itensPorPagina;

            var pessoas = await Db.Pessoas
                    .Where(GerarExpressionBuscaPessoa(grupoId, nome, tipo, buscarInativos, idInativo))
                    .OrderBy(p => p.Nome)
                    .Skip(skip)
                    .Take(itensPorPagina)
                    .ToListAsync();

            var count = await Db.Pessoas
                    .Where(GerarExpressionBuscaPessoa(grupoId, nome, tipo, buscarInativos, idInativo))
                    .CountAsync();

            return new PaginatedResult<Pessoa>(
                count,
                pessoas,
                pagina,
                itensPorPagina
            );
        }

        private Expression<Func<Pessoa, bool>> GerarExpressionBuscaPessoa(int grupoId, string? nome, string tipo, bool buscarInativos, int? idInativo = null)
        {
            return p =>
                    p.GrupoId == grupoId
                    && (string.IsNullOrEmpty(nome) || p.Nome.Contains(nome))
                    && (
                        tipo == TipoPessoaPesquisa.Todos
                        || p.Tipo == TipoPessoa.Ambos
                        || (tipo == TipoPessoaPesquisa.Cliente && p.Tipo == TipoPessoa.Cliente)
                        || (tipo == TipoPessoaPesquisa.Fornecedor && p.Tipo == TipoPessoa.Fornecedor)
                    )
                    && (buscarInativos || p.Ativo || p.Id == idInativo);
        }

        public async Task IncluirAsync(Pessoa pessoa)
        {
            await Db.Pessoas.AddAsync(pessoa);
        }

        public Task AlterarAsync(Pessoa pessoa)
        {
            return Task.Run(() => Db.Pessoas.Update(pessoa));
        }

        public Task RemoverAsync(Pessoa pessoa)
        {
            return Task.Run(() => Db.Pessoas.Remove(pessoa));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var pessoas = await Db.Pessoas.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.Pessoas.RemoveRange(pessoas);
        }
    }
}