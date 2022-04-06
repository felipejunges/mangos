using Mangos.Dominio.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoFixoListaModel
    {
        public int Id { get; set; }

        public TipoLancamentoFixo Tipo { get; set; }

        public PeriodicidadeLancamentoFixo Periodicidade { get; set; }

        public string Vencimento { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public string Pessoa { get; set; }

        [Display(Name = "Último mês gerado")]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}")]
        public DateTime? DataUltimoMesGerado { get; set; }

        [Display(Name = "Últ. geração")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? DataHoraUltimaGeracao { get; set; }

        public bool Ativo { get; set; }

        public LancamentoFixoListaModel(int id, TipoLancamentoFixo tipo, PeriodicidadeLancamentoFixo periodicidade, string vencimento, decimal valor, string descricao, string pessoa, DateTime? dataUltimoMesGerado, DateTime? dataHoraUltimaGeracao, bool ativo)
        {
            Id = id;
            Tipo = tipo;
            Periodicidade = periodicidade;
            Vencimento = vencimento;
            Valor = valor;
            Descricao = descricao;
            Pessoa = pessoa;
            DataUltimoMesGerado = dataUltimoMesGerado;
            DataHoraUltimaGeracao = dataHoraUltimaGeracao;
            Ativo = ativo;
        }
    }
}