using Mangos.Dominio.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Dominio.Models.Relatorios
{
    public class RelatorioProjecaoSaldoModel
    {
        public int Ordem { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        public TipoLancamento TipoValor { get; set; }

        public RelatorioProjecaoSaldoModel(int ordem, DateTime data, string descricao, decimal valor, TipoLancamento tipoValor)
        {
            Ordem = ordem;
            Data = data;
            Descricao = descricao;
            Valor = valor;
            TipoValor = tipoValor;
        }
    }
}