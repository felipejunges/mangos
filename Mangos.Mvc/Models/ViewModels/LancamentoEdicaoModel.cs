using Mangos.Dominio.Enums;
using Mangos.Mvc.Models.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoEdicaoModel : HashedModel
    {
        public TipoLancamento Tipo { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        [Display(Name = "Data vencimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Text)]
        public DateTime DataVencimento { get; set; }

        [MaxLength(255)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public int NumeroParcela { get; set; }

        public int TotalParcelas { get; set; }

        [Display(Name = "Pessoa")]
        public int? PessoaId { get; set; }

        public string? Pessoa { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength]
        [Column(TypeName = "varchar(MAX)")]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        public bool Pago { get; set; }

        [Display(Name = "Data pagamento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Text)]
        public DateTime? DataPagamento { get; set; }

        [Display(Name = "Valor pago")]
        public decimal? ValorPago { get; set; }

        [Display(Name = "Conta bancária")]
        public int? ContaBancariaId { get; set; }

        [Display(Name = "Data débito conta")]
        [DataType(DataType.Text)]
        public DateTime? DataContaBancaria { get; set; }

        public IEnumerable<CategoriaListaModel> Categorias { get; set; }

        public IEnumerable<ContaBancariaListaModel> ContasBancarias { get; set; }

        public LancamentoEdicaoModel()
        {
            Categorias = Enumerable.Empty<CategoriaListaModel>();
            ContasBancarias = Enumerable.Empty<ContaBancariaListaModel>();
        }

        public LancamentoEdicaoModel(int id, int grupoId, TipoLancamento tipo, decimal valor, DateTime dataVencimento, string? descricao, int numeroParcela, int totalParcelas, int? pessoaId, string? pessoa, int? categoriaId, string? observacoes, bool pago, DateTime? dataPagamento, decimal? valorPago, int? contaBancariaId, DateTime? dataContaBancaria, IEnumerable<CategoriaListaModel> categorias, IEnumerable<ContaBancariaListaModel> contasBancarias)
        {
            Id = id;
            GrupoId = grupoId;
            Tipo = tipo;
            Valor = valor;
            DataVencimento = dataVencimento;
            Descricao = descricao;
            NumeroParcela = numeroParcela;
            TotalParcelas = totalParcelas;
            PessoaId = pessoaId;
            Pessoa = pessoa;
            CategoriaId = categoriaId;
            Observacoes = observacoes;
            Pago = pago;
            DataPagamento = dataPagamento;
            ValorPago = valorPago;
            ContaBancariaId = contaBancariaId;
            DataContaBancaria = dataContaBancaria;
            Categorias = categorias;
            ContasBancarias = contasBancarias;

            SetValidationHash();
        }

        public void AtualizarCategoriasCartoesCredito(IEnumerable<CategoriaListaModel> categorias, IEnumerable<ContaBancariaListaModel> contasBancarias)
        {
            Categorias = categorias;
            ContasBancarias = contasBancarias;
        }
    }
}