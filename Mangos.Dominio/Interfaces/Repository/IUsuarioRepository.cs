using Mangos.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterUsuarioAsync(int id);
        Task<Usuario?> ObterUsuarioAtivoPeloEmailAsync(string email);
        Task<Usuario?> ObterUsuarioPeloTokenAsync(string token);
        Task<bool> ValidarEmailJaRegistradoAsync(string email, int idUsuarioExceto);
        Task<List<Usuario>> ListarUsuariosAsync(int? grupoId, string? nome);
        Task<List<Usuario>> ListarUsuariosEmailAvisoVencimentosAsync();
        Task IncluirAsync(Usuario usuario);
        Task AlterarAsync(Usuario usuario);
        Task RemoverAsync(Usuario usuario);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}