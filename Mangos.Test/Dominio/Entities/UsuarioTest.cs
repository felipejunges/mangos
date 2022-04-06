using Mangos.Dominio.Entities;
using Mangos.Dominio.ValueObjects;
using Xunit;

namespace Mangos.Test.Dominio.Entities
{
    public class UsuarioTest
    {
        [Fact]
        public void ValidacaoSenhaDeveConferir()
        {
            string senhaTeste = "abc123!@#...";

            var usuario = new Usuario()
            {
                Id = 0,
                GrupoId = 1,
                Nome = "Teste",
                Email = "teste@teste.com",
                Senha = senhaTeste
            };

            Assert.True(usuario.Senha.Check(senhaTeste));

            string senhaCripto = usuario.Senha.Senha;

            var usuarioSenhaVO = UsuarioSenhaVO.FromHashed(senhaCripto);

            Assert.True(usuarioSenhaVO.Check(senhaTeste));
        }
    }
}