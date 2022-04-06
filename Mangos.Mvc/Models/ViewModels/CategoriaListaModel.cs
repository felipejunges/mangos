using Mangos.Dominio.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class CategoriaListaModel
    {
        public int Id { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public bool Receita { get; set; }
        public bool Despesa { get; set; }

        [Display(Name = "Relatório total")]
        public bool RelatorioTotal { get; set; }

        public bool Ativo { get; set; }

        public CategoriaListaModel(int id, string? descricao, bool receita, bool despesa, bool relatorioTotal, bool ativo)
        {
            Id = id;
            Descricao = descricao;
            Receita = receita;
            Despesa = despesa;
            RelatorioTotal = relatorioTotal;
            Ativo = ativo;
        }
    }
}