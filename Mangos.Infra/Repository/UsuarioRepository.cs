using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly MangosDb Db;

        public UsuarioRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<Usuario?> ObterUsuarioAsync(int id)
        {
            return Db.Usuarios
                        .Where(u => u.Id == id)
                        .FirstOrDefaultAsync();
        }

        public Task<Usuario?> ObterUsuarioPeloTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return Task.FromResult<Usuario?>(null);

            return Db.Usuarios
                        .Where(u =>
                            u.TokenSenha == token
                            && u.ValidadeTokenSenha >= DateTime.Now
                            && u.Ativo
                        )
                        .FirstOrDefaultAsync();
        }

        public Task<Usuario?> ObterUsuarioAtivoPeloEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Task.FromResult<Usuario?>(null);

            return Db.Usuarios
                        .Where(u =>
                            u.Email == email
                            && u.Ativo
                        )
                        .FirstOrDefaultAsync();
        }

        public Task<bool> ValidarEmailJaRegistradoAsync(string email, int idUsuarioExceto)
        {
            return Db.Usuarios.Where(o => o.Email == email && o.Id != idUsuarioExceto).AnyAsync();
        }

        public Task<List<Usuario>> ListarUsuariosAsync(int? grupoId, string? nome)
        {
            return Db.Usuarios
                        .Where(u =>
                            (grupoId == null || u.GrupoId == grupoId.Value)
                            && (string.IsNullOrEmpty(nome) || u.Nome.Contains(nome))
                        )
                        .OrderBy(o => o.Email)
                        .ToListAsync();
        }

        public Task<List<Usuario>> ListarUsuariosEmailAvisoVencimentosAsync()
        {
            return Db.Usuarios.Where(o => o.TipoEmailVencimentos != TipoAvisoVencimentosUsuario.Nenhum).OrderBy(o => o.Nome).ToListAsync();
        }

        public async Task IncluirAsync(Usuario usuario)
        {
            await Db.Usuarios.AddAsync(usuario);
        }

        public Task AlterarAsync(Usuario usuario)
        {
            return Task.Run(() => Db.Usuarios.Update(usuario));
        }

        public Task RemoverAsync(Usuario usuario)
        {
            return Task.Run(() => Db.Usuarios.Remove(usuario));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var usuarios = await Db.Usuarios.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.Usuarios.RemoveRange(usuarios);
        }
    }
}