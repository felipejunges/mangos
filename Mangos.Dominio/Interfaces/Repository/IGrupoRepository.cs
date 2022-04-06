using Mangos.Dominio.Entities;
using Mangos.Dominio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface IGrupoRepository
    {
        Task<Grupo?> ObterGrupoAsync(int grupoId);
        Task<GrupoRelacionamentos?> ObterGrupoComRelacionamentosAsync(int id);
        Task<IEnumerable<Grupo>> ListarGruposAsync(string? descricao);
        Task IncluirAsync(Grupo grupo);
        Task AlterarAsync(Grupo grupo);
        Task RemoverAsync(Grupo grupo);
    }
}