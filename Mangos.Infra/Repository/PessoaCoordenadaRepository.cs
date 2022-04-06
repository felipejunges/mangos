using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class PessoaCoordenadaRepository : IPessoaCoordenadaRepository
    {
        private readonly MangosDb Db;

        public PessoaCoordenadaRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<PessoaCoordenada?> ObterPessoaCoordenadaAsync(int id)
        {
            return Db.PessoasCoordenadas
                            .Include(p => p.Pessoa)
                            .Where(p => p.Id == id)
                            .FirstOrDefaultAsync();

        }

        public Task<List<PessoaCoordenada>> ListarPessoasCoordenadasPorGrupoAsync(int grupoId)
        {
            return Db.PessoasCoordenadas
                        .Include(p => p.Pessoa)
                        .Where(p =>
                            p.Pessoa!.GrupoId == grupoId
                            && p.Pessoa.Ativo
                        )
                        .OrderBy(p => p.Pessoa!.Nome)
                        .ToListAsync();
        }

        public Task<List<PessoaCoordenada>> ListarPessoasCoordenadasPorPessoaAsync(int grupoId, int pessoaId)
        {
            return Db.PessoasCoordenadas
                        .Include(p => p.Pessoa)
                        .Where(p =>
                            p.PessoaId == pessoaId
                            && p.Pessoa!.GrupoId == grupoId
                        )
                        .OrderBy(p => p.Id)
                        .ToListAsync();
        }

        public Task<List<PessoaCoordenada>> ListarPessoasCoordenadasFornecedoresNoTrackingAsync(int grupoId)
        {
            return Db.PessoasCoordenadas
                        .Include(p => p.Pessoa)
                        .Where(p =>
                            p.Pessoa!.GrupoId == grupoId
                            && p.Pessoa.Ativo
                            && (p.Pessoa.Tipo == TipoPessoa.Fornecedor || p.Pessoa.Tipo == TipoPessoa.Ambos)
                        )
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task IncluirAsync(PessoaCoordenada pessoaCoordenada)
        {
            await Db.PessoasCoordenadas.AddAsync(pessoaCoordenada);
        }

        public Task AlterarAsync(PessoaCoordenada pessoaCoordenada)
        {
            return Task.Run(() => Db.PessoasCoordenadas.Update(pessoaCoordenada));
        }

        public Task RemoverAsync(PessoaCoordenada pessoaCoordenada)
        {
            return Task.Run(() => Db.PessoasCoordenadas.Remove(pessoaCoordenada));
        }
    }
}