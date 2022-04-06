using Mangos.Dominio.Entities;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface IDispositivoConectadoRepository
    {
        Task<DispositivoConectado?> ObterDispositivoConectadoAsync(string identificador, int usuarioId);
        Task IncluirAsync(DispositivoConectado dispositivoConectado);
        Task AlterarAsync(DispositivoConectado dispositivoConectado);
    }
}