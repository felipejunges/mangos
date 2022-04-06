using Mangos.Dominio.Enums;
using Mangos.Dominio.ValueObjects;
using System;
using System.Collections.Generic;

namespace Mangos.Dominio.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public UsuarioSenhaVO Senha { get; set; }

        public TipoAvisoVencimentosUsuario TipoAlertaVencimentos { get; set; }

        public int DiasAlertaVencimentos { get; set; }

        public TipoAvisoVencimentosUsuario TipoEmailVencimentos { get; set; }

        public int DiasEmailVencimentos { get; set; }

        public bool Admin { get; set; }

        public bool Ativo { get; set; }

        public string? TokenSenha { get; set; }

        public DateTime? ValidadeTokenSenha { get; set; }

        public virtual Grupo? Grupo { get; set; }

        public virtual ICollection<DispositivoConectado> DispositivosConectados { get; set; }

        public virtual ICollection<SessaoAcesso> SessoesAcesso { get; set; }

        public Usuario()
        {
            Nome = string.Empty;
            Email = string.Empty;
            Senha = string.Empty;

            DispositivosConectados = new HashSet<DispositivoConectado>();
            SessoesAcesso = new HashSet<SessaoAcesso>();
        }

        public static Usuario NovoUsuario(string email, string nome, string senha)
        {
            return new Usuario()
            {
                Id = 0,
                Grupo = new Grupo()
                {
                    Id = 0,
                    Descricao = email + " / " + nome,
                    MesesAntecedenciaGerarLancamento = 6,
                    MesesAntecedenciaGerarLancamentoCartao = 0,
                    MesesGraficosDashboard = 12
                },
                Nome = nome,
                Email = email,
                Senha = senha,
                TipoAlertaVencimentos = TipoAvisoVencimentosUsuario.Ambos,
                DiasAlertaVencimentos = 7,
                TipoEmailVencimentos = TipoAvisoVencimentosUsuario.Ambos,
                DiasEmailVencimentos = 3,
                Admin = false,
                Ativo = true
            };
        }

        public void AlterarSenha(string senha)
        {
            Senha = senha;
            TokenSenha = null;
            ValidadeTokenSenha = null;
        }

        public void SetarToken(string token)
        {
            TokenSenha = token;
            ValidadeTokenSenha = DateTime.Now.AddDays(1);
        }

        public void LimparToken()
        {
            TokenSenha = null;
            ValidadeTokenSenha = null;
        }
    }
}