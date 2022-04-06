using Mangos.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> ObterCartaoCreditoAsync(int id);
        Task<List<Categoria>> ListarCategoriasAsync(int grupoId, string? descricao, string tipo, bool buscarInativas, int? idInativo = null);
        Task<List<Categoria>> ListarCategoriasSuperioresAsync(int grupoId, bool buscarInativas, int? idInativo = null);
        Task<bool> ValidarCategoriasTemFilhasAsync(int id);
        Task IncluirAsync(Categoria categoria);
        Task AlterarAsync(Categoria categoria);
        Task RemoverAsync(Categoria categoria);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}