using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class CartaoCreditoListaModel
    {
        public int Id { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public string? Categoria { get; set; }

        public bool Ativo { get; set; }

        public CartaoCreditoListaModel(int id, string? descricao, string? categoria, bool ativo)
        {
            Id = id;
            Descricao = descricao;
            Categoria = categoria;
            Ativo = ativo;
        }
    }
}