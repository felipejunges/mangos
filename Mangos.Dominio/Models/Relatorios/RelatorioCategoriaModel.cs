using Mangos.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Dominio.Models.Relatorios
{
    public class RelatorioCategoriaModel
    {
        public int CategoriaId { get; set; }

        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        public TipoLancamento Tipo { get; set; }

        public Dictionary<DateTime, decimal> Valores { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal ValorTotal { get; set; }

        public RelatorioCategoriaModel(int categoriaId, string categoria, TipoLancamento tipo, Dictionary<DateTime, decimal> valores, decimal valorTotal)
        {
            CategoriaId = categoriaId;
            Categoria = categoria;
            Tipo = tipo;
            Valores = valores;
            ValorTotal = valorTotal;
        }
    }
}