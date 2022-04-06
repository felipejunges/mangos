using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class SessaoAcessoRepository : ISessaoAcessoRepository
    {
        private readonly MangosDb Db;

        public SessaoAcessoRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<SessaoAcesso?> ObterSessaoAcessoAsync(int id)
        {
            return Db.SessoesAcesso
                            .Include(s => s.Usuario)
                            .Where(s => s.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<SessaoAcesso?> ObterSessaoAcessoUsuarioSessaoAsync(int usuarioId, string chaveSessao)
        {
            return Db.SessoesAcesso
                            .Include(s => s.Usuario)
                            .Where(s => s.UsuarioId == usuarioId && s.Chave == chaveSessao)
                            .FirstOrDefaultAsync();
        }

        public Task<List<SessaoAcesso>> ListarSessoesAcessoAsync(DateTime dataInicial, DateTime dataFinal, bool buscarSessoesDeslogadas)
        {
            return Db.SessoesAcesso
                            .Include(s => s.Usuario)
                            .Where(o =>
                                o.DataHoraAtualizacao.Date >= dataInicial
                                && o.DataHoraAtualizacao.Date <= dataFinal
                                && (buscarSessoesDeslogadas || (!o.Logout && o.DataHoraExpiracao >= DateTime.Now))
                            )
                            .OrderBy(o => o.DataHoraAtualizacao)
                            .ToListAsync();
        }

        public async Task IncluirAsync(SessaoAcesso sessaoAcesso)
        {
            await Db.SessoesAcesso.AddAsync(sessaoAcesso);
        }

        public Task AlterarAsync(SessaoAcesso sessaoAcesso)
        {
            return Task.Run(() => Db.SessoesAcesso.Update(sessaoAcesso));
        }
    }
}