using Mangos.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface IContaBancariaRepository
    {
        Task<ContaBancaria?> ObterContaBancariaAsync(int id);
        Task<List<ContaBancaria>> ListaContasBancariasAsync(int grupoId, string? descricao, bool buscarInativos, int? idInativo = null);
        Task<List<ContaBancaria>> ListaContasBancariasAtivasTodosAsync();
        Task IncluirAsync(ContaBancaria contaBancaria);
        Task AlterarAsync(ContaBancaria contaBancaria);
        Task RemoverAsync(ContaBancaria contaBancaria);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}