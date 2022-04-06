using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class CategoriaDadosModel : HashedModel
    {
        [Display(Name = "Categoria superior")]
        public int? CategoriaSuperiorId { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public bool Receita { get; set; }

        public bool Despesa { get; set; }

        [Display(Name = "Relatório total")]
        public bool RelatorioTotal { get; set; }

        public bool Ativo { get; set; }

        public bool CategoriaTemFilhas { get; set; }

        public IEnumerable<Categoria> CategoriasSuperiores { get; set; }

        public CategoriaDadosModel()
        {
            CategoriasSuperiores = Enumerable.Empty<Categoria>();
        }

        public CategoriaDadosModel(int id, int grupoId, int? categoriaSuperiorId, string? descricao, bool receita, bool despesa, bool relatorioTotal, bool ativo, bool categoriaTemFilhas, IEnumerable<Categoria> categoriasSuperiores)
        {
            Id = id;
            GrupoId = grupoId;
            CategoriaSuperiorId = categoriaSuperiorId;
            Descricao = descricao;
            Receita = receita;
            Despesa = despesa;
            RelatorioTotal = relatorioTotal;
            Ativo = ativo;
            CategoriaTemFilhas = categoriaTemFilhas;
            CategoriasSuperiores = categoriasSuperiores;

            SetValidationHash();
        }

        public static CategoriaDadosModel Novo(int grupoId, IEnumerable<Categoria> categoriasSuperiores)
            => new CategoriaDadosModel(
                id: 0,
                grupoId: grupoId,
                categoriaSuperiorId: null,
                descricao: null,
                receita: false,
                despesa: false,
                relatorioTotal: true,
                ativo: true,
                categoriaTemFilhas: false,
                categoriasSuperiores: categoriasSuperiores
            );

        public void AtualizarCategorias(bool categoriaTemFilhas, IEnumerable<Categoria> categoriasSuperiores)
        {
            CategoriaTemFilhas = categoriaTemFilhas;
            CategoriasSuperiores = categoriasSuperiores;
        }
    }
}