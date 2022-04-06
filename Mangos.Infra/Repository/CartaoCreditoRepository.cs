using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class CartaoCreditoRepository : ICartaoCreditoRepository
    {
        private readonly MangosDb Db;

        public CartaoCreditoRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<CartaoCredito?> ObterCartaoCreditoAsync(int id)
        {
            return Db.CartoesCredito
                        .Where(c => c.Id == id)
                        .FirstOrDefaultAsync();
        }

        public Task<List<CartaoCredito>> ListarCartoesCreditoAsync(int grupoId, string? descricao, bool buscarInativos, int? idInativo = null)
        {
            return Db.CartoesCredito
                        .Include(c => c.Categoria)
                        .Where(c =>
                            c.GrupoId == grupoId
                            && (string.IsNullOrEmpty(descricao) || c.Descricao.Contains(descricao))
                            && (buscarInativos || c.Ativo || c.Id == idInativo)
                        )
                        .OrderBy(c => c.Descricao)
                        .ToListAsync();
        }

        public async Task IncluirAsync(CartaoCredito cartaoCredito)
        {
            await Db.CartoesCredito.AddAsync(cartaoCredito);
        }

        public Task AlterarAsync(CartaoCredito cartaoCredito)
        {
            return Task.Run(() => Db.CartoesCredito.Update(cartaoCredito));
        }

        public Task RemoverAsync(CartaoCredito cartaoCredito)
        {
            return Task.Run(() => Db.CartoesCredito.Remove(cartaoCredito));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var cartoesCredito = await Db.CartoesCredito.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.CartoesCredito.RemoveRange(cartoesCredito);
        }
    }
}