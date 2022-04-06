using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.GerenciarConta
{
    public class GerenciarDadosCadastraisModel
    {
        public int Id { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Confirme o e-mail")]
        [DataType(DataType.EmailAddress)]
        public string ConfirmeEmail { get; set; }

        public string Nome { get; set; }

        [Display(Name = "Senha atual (opcional)")]
        [DataType(DataType.Password)]
        public string? SenhaAtual { get; set; }

        [Display(Name = "Nova senha (opcional)")]
        [DataType(DataType.Password)]
        public string? NovaSenha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha (opcional)")]
        public string? ConfirmeNovaSenha { get; set; }

        public bool Sucesso { get; set; }

        public GerenciarDadosCadastraisModel()
        {
            Email = string.Empty;
            ConfirmeEmail = string.Empty;
            Nome = string.Empty;
        }

        public GerenciarDadosCadastraisModel(int id, string email, string confirmeEmail, string nome)
        {
            Id = id;
            Email = email;
            ConfirmeEmail = confirmeEmail;
            Nome = nome;
            Sucesso = false;
        }
    }
}