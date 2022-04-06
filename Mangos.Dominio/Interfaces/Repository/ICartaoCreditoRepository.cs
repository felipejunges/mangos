using Mangos.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface ICartaoCreditoRepository
    {
        Task<CartaoCredito?> ObterCartaoCreditoAsync(int id);
        Task<List<CartaoCredito>> ListarCartoesCreditoAsync(int grupoId, string? descricao, bool buscarInativos, int? idInativo = null);
        Task IncluirAsync(CartaoCredito cartaoCredito);
        Task AlterarAsync(CartaoCredito cartaoCredito);
        Task RemoverAsync(CartaoCredito cartaoCredito);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}