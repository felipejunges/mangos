using Mangos.Dominio.Entities;
using Mangos.Dominio.Models.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface IPessoaRepository
    {
        Task<Pessoa?> ObterPessoaAsync(int id);
        Task<List<Pessoa>> ListarPessoasAsync(int grupoId, string nome, string tipo, bool buscarInativos, int? idInativo = null);
        Task<PaginatedResult<Pessoa>> ListarPessoasPaginatedAsync(int grupoId, string? nome, string tipo, int pagina, int itensPorPagina, bool buscarInativos, int? idInativo = null);
        Task IncluirAsync(Pessoa pessoa);
        Task AlterarAsync(Pessoa pessoa);
        Task RemoverAsync(Pessoa pessoa);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}