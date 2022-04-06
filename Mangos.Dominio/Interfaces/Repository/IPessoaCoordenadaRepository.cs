using Mangos.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface IPessoaCoordenadaRepository
    {
        Task<PessoaCoordenada?> ObterPessoaCoordenadaAsync(int id);
        Task<List<PessoaCoordenada>> ListarPessoasCoordenadasPorGrupoAsync(int grupoId);
        Task<List<PessoaCoordenada>> ListarPessoasCoordenadasPorPessoaAsync(int grupoId, int pessoaId);
        Task<List<PessoaCoordenada>> ListarPessoasCoordenadasFornecedoresNoTrackingAsync(int grupoId);
        Task IncluirAsync(PessoaCoordenada pessoaCoordenada);
        Task AlterarAsync(PessoaCoordenada pessoaCoordenada);
        Task RemoverAsync(PessoaCoordenada pessoaCoordenada);
    }
}