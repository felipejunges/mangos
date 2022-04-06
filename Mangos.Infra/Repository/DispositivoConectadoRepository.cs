using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class DispositivoConectadoRepository : IDispositivoConectadoRepository
    {
        private readonly MangosDb Db;

        public DispositivoConectadoRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<DispositivoConectado?> ObterDispositivoConectadoAsync(string identificador, int usuarioId)
        {
            return Db.DispositivosConectados
                            .Where(o => o.UsuarioId == usuarioId && o.Identificador == identificador)
                            .FirstOrDefaultAsync();
        }

        public async Task IncluirAsync(DispositivoConectado dispositivoConectado)
        {
            await Db.DispositivosConectados.AddAsync(dispositivoConectado);
        }

        public Task AlterarAsync(DispositivoConectado dispositivoConectado)
        {
            return Task.Run(() => Db.DispositivosConectados.Update(dispositivoConectado));
        }
    }
}