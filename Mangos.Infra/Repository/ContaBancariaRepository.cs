using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class ContaBancariaRepository : IContaBancariaRepository
    {
        private readonly MangosDb Db;

        public ContaBancariaRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<ContaBancaria?> ObterContaBancariaAsync(int id)
        {
            return Db.ContasBancarias
                        .Where(c => c.Id == id)
                        .FirstOrDefaultAsync();
        }

        public Task<List<ContaBancaria>> ListaContasBancariasAsync(int grupoId, string? descricao, bool buscarInativos, int? idInativo = null)
        {
            return Db.ContasBancarias
                        .Where(c =>
                            c.GrupoId == grupoId
                            && (string.IsNullOrEmpty(descricao) || c.Descricao.Contains(descricao))
                            && (buscarInativos || c.Ativo || c.Id == idInativo)
                        )
                        .OrderBy(c => c.Descricao)
                        .ToListAsync();
        }

        public Task<List<ContaBancaria>> ListaContasBancariasAtivasTodosAsync()
        {
            return Db.ContasBancarias
                        .Where(c => c.Ativo)
                        .OrderBy(c => c.Descricao)
                        .ToListAsync();
        }

        public async Task IncluirAsync(ContaBancaria contaBancaria)
        {
            await Db.ContasBancarias.AddAsync(contaBancaria);
        }

        public Task AlterarAsync(ContaBancaria contaBancaria)
        {
            return Task.Run(() => Db.ContasBancarias.Update(contaBancaria));
        }

        public Task RemoverAsync(ContaBancaria contaBancaria)
        {
            return Task.Run(() => Db.ContasBancarias.Remove(contaBancaria));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var contasBancarias = await Db.ContasBancarias.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.ContasBancarias.RemoveRange(contasBancarias);
        }
    }
}