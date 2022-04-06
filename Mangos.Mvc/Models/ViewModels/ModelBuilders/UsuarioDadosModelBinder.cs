using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class UsuarioDadosModelBinder : IModelBuilder<UsuarioDadosModel, Usuario>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IGrupoRepository _grupoRepository;

        public UsuarioDadosModelBinder(IUsuarioRepository usuarioRepository, IGrupoRepository grupoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _grupoRepository = grupoRepository;
        }

        public async Task<Usuario?> CriarEntidadeAsync(UsuarioDadosModel usuarioModel)
        {
            Usuario? usuario;

            if (usuarioModel.Id == 0)
            {
                usuario = new Usuario()
                {
                    Id = 0,
                    GrupoId = usuarioModel.GrupoId!.Value,
                    TipoAlertaVencimentos = TipoAvisoVencimentosUsuario.Ambos,
                    DiasAlertaVencimentos = 7,
                    TipoEmailVencimentos = TipoAvisoVencimentosUsuario.Ambos,
                    DiasEmailVencimentos = 3,
                };
            }
            else
            {
                usuario = await _usuarioRepository.ObterUsuarioAsync(usuarioModel.Id);
            }

            if (usuario is null)
                return null;

            usuario.Nome = usuarioModel.Nome;
            usuario.Email = usuarioModel.Email;

            if (!string.IsNullOrEmpty(usuarioModel.Senha))
                usuario.AlterarSenha(usuarioModel.Senha);

            usuario.Admin = usuarioModel.Admin;
            usuario.Ativo = usuarioModel.Ativo;

            return usuario;
        }

        public async Task<UsuarioDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return UsuarioDadosModel.Novo(
                    await _grupoRepository.ListarGruposAsync(null)
                );
            }
            else
            {
                var usuario = await _usuarioRepository.ObterUsuarioAsync(id.Value);

                if (usuario == null)
                    return null;

                return new UsuarioDadosModel(
                    id: usuario.Id,
                    grupoId: usuario.GrupoId,
                    nome: usuario.Nome,
                    email: usuario.Email,
                    admin: usuario.Admin,
                    ativo: usuario.Ativo,
                    grupos: await _grupoRepository.ListarGruposAsync(null)
                );
            }
        }

        public async Task AtualizarAsync(UsuarioDadosModel model)
        {
            model.AtualizarGrupos(
                await _grupoRepository.ListarGruposAsync(null)
            );
        }
    }
}