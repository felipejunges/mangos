using Mangos.Dominio.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class UsuarioDadosModel
    {
        public int Id { get; set; }

        [Display(Name = "Grupo")]
        public int? GrupoId { get; set; }

        public string Nome { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string? Senha { get; set; }

        [Display(Name = "Administrador do sistema")]
        public bool Admin { get; set; }

        public bool Ativo { get; set; }

        public IEnumerable<Grupo> Grupos { get; set; }

        public UsuarioDadosModel()
        {
            Nome = string.Empty;
            Email = string.Empty;
            Grupos = Enumerable.Empty<Grupo>();
        }

        public UsuarioDadosModel(int id, int? grupoId, string nome, string email, bool admin, bool ativo, IEnumerable<Grupo> grupos)
        {
            Id = id;
            GrupoId = grupoId;
            Nome = nome;
            Email = email;
            Admin = admin;
            Ativo = ativo;
            Grupos = grupos;
        }

        public static UsuarioDadosModel Novo(IEnumerable<Grupo> grupos)
        {
            return new UsuarioDadosModel(
                id: 0,
                grupoId: null,
                nome: string.Empty,
                email: string.Empty,
                admin: false,
                ativo: true,
                grupos: grupos
            );
        }

        public void AtualizarGrupos(IEnumerable<Grupo> grupos)
        {
            Grupos = grupos;
        }
    }
}