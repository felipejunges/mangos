using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Usuario?> ObterUsuarioPeloLoginSenhaAsync(string email, string senha)
        {
            var usuario = await _usuarioRepository.ObterUsuarioAtivoPeloEmailAsync(email);

            // verifica se encontrou um usuário ativo pelo e-mail, e se a senha bate
            if (usuario != null && usuario.Senha.Check(senha))
                return usuario;
            else
                return null;
        }
        
        public async Task ZerarTokenSenha(Usuario usuario)
        {
            if (usuario.TokenSenha is not null)
            {
                usuario.LimparToken();

                await _usuarioRepository.AlterarAsync(usuario);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task PersistirAsync(Usuario usuario)
        {
            if (usuario.Id == 0)
                await _usuarioRepository.IncluirAsync(usuario);
            else
                await _usuarioRepository.AlterarAsync(usuario);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoverAsync(Usuario usuario)
        {
            await _usuarioRepository.RemoverAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}