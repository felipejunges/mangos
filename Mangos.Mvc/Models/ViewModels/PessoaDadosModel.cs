using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Mvc.Models.ViewModels.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class PessoaDadosModel : HashedModel
    {
        public string? Nome { get; set; }

        [Display(Name = "Telefone 1")]
        public string? Telefone1 { get; set; }

        [Display(Name = "Telefone 2")]
        public string? Telefone2 { get; set; }

        [Display(Name = "Telefone 3")]
        public string? Telefone3 { get; set; }

        public string? Site { get; set; }

        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        public TipoPessoa? Tipo { get; set; }

        [Display(Name = "Categoria padrão receita")]
        public int? CategoriaPadraoReceitaId { get; set; }

        [Display(Name = "Categoria padrão despesa")]
        public int? CategoriaPadraoDespesaId { get; set; }

        public bool Ativo { get; set; }

        public IEnumerable<Categoria> CategoriasReceita { get; set; }

        public IEnumerable<Categoria> CategoriasDespesa { get; set; }

        public PessoaDadosModel()
        {
            CategoriasReceita = Enumerable.Empty<Categoria>();
            CategoriasDespesa = Enumerable.Empty<Categoria>();
        }

        public PessoaDadosModel(int id, int grupoId, string? nome, string? telefone1, string? telefone2, string? telefone3, string? site, string? email, TipoPessoa? tipo, int? categoriaPadraoReceitaId, int? categoriaPadraoDespesaId, bool ativo, IEnumerable<Categoria> categoriasReceita, IEnumerable<Categoria> categoriasDespesa)
        {
            Id = id;
            GrupoId = grupoId;
            Nome = nome;
            Telefone1 = telefone1;
            Telefone2 = telefone2;
            Telefone3 = telefone3;
            Site = site;
            Email = email;
            Tipo = tipo;
            CategoriaPadraoReceitaId = categoriaPadraoReceitaId;
            CategoriaPadraoDespesaId = categoriaPadraoDespesaId;
            Ativo = ativo;
            CategoriasReceita = categoriasReceita;
            CategoriasDespesa = categoriasDespesa;

            SetValidationHash();
        }

        public static PessoaDadosModel Novo(int grupoId, IEnumerable<Categoria> categoriasReceita, IEnumerable<Categoria> categoriasDespesa)
        {
            return new PessoaDadosModel(
                id: 0,
                grupoId: grupoId,
                nome: null,
                telefone1: null,
                telefone2: null,
                telefone3: null,
                site: null,
                email: null,
                tipo: null,
                categoriaPadraoReceitaId: null,
                categoriaPadraoDespesaId: null,
                ativo: true,
                categoriasReceita: categoriasReceita,
                categoriasDespesa: categoriasDespesa
            );
        }

        public void AtualizarCategorias(IEnumerable<Categoria> categoriasReceita, IEnumerable<Categoria> categoriasDespesa)
        {
            CategoriasReceita = categoriasReceita;
            CategoriasDespesa = categoriasDespesa;
        }
    }
}