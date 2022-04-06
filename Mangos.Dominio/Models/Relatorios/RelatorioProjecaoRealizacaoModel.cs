using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Dominio.Models.Relatorios
{
    [Serializable]
    public class RelatorioProjecaoRealizacaoModel
    {
        [Display(Name = "Mês")]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}")]
        public DateTime Mes { get; set; }

        [Display(Name = "Receitas (projeção)")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal ValorReceitasProjecao { get; set; }

        [Display(Name = "Receitas (realização)")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal ValorReceitasRealizacao { get; set; }

        [Display(Name = "Despesas (projeção)")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal ValorDespesasProjecao { get; set; }

        [Display(Name = "Despesas (realização)")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal ValorDespesasRealizacao { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal ValorTotal => ValorReceitasProjecao + ValorReceitasRealizacao - ValorDespesasProjecao - ValorDespesasRealizacao;
    }
}