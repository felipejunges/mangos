using Mangos.Dominio.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoCartaoListaModel
    {
        public int Id { get; set; }

        [Display(Name = "Cartão crédito")]
        public string CartaoCredito { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public string Pessoa { get; set; }

        [Display(Name = "Mês referência")]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}")]
        public DateTime MesReferencia { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        [Display(Name = "Tipo")]
        public TipoLancamentoCartao TipoLancamento { get; set; }

        [Display(Name = "Fechado")]
        public bool GeradoLancamento { get; set; }

        public Guid? Agrupador { get; set; }

        public bool TemAgrupador => Agrupador.HasValue;

        public decimal ValorCalculado { get => TipoLancamento == TipoLancamentoCartao.Despesa ? Valor : Valor * -1; }

        public LancamentoCartaoListaModel(int id, string cartaoCredito, string descricao, string pessoa, DateTime mesReferencia, decimal valor, TipoLancamentoCartao tipoLancamento, bool geradoLancamento, Guid? agrupador)
        {
            Id = id;
            CartaoCredito = cartaoCredito;
            Descricao = descricao;
            Pessoa = pessoa;
            MesReferencia = mesReferencia;
            Valor = valor;
            TipoLancamento = tipoLancamento;
            GeradoLancamento = geradoLancamento;
            Agrupador = agrupador;
        }
    }
}