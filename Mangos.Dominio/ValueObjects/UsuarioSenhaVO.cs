using Mangos.Dominio.Utils;

namespace Mangos.Dominio.ValueObjects
{
    public class UsuarioSenhaVO
    {
        private const int PASSWORD_SALT = 12;

        public string Senha { get; private set; }

        private UsuarioSenhaVO()
        {
            Senha = string.Empty;
        }

        private UsuarioSenhaVO(string value)
        {
            Senha = Cripto.BCryptGenerate(value, PASSWORD_SALT);
        }

        public static implicit operator UsuarioSenhaVO(string value)
        {
            return new UsuarioSenhaVO(value);
        }

        public bool Check(string senha)
        {
            return Cripto.BCryptCheck(senha, Senha);
        }

        public override string ToString()
        {
            return Senha;
        }

        public static UsuarioSenhaVO FromHashed(string senhaHashed)
        {
            return new UsuarioSenhaVO() { Senha = senhaHashed };
        }
    }
}