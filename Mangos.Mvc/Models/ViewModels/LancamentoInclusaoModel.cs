using Mangos.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoInclusaoModel
    {
        public int GrupoId { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public decimal Valor { get; set; }

        [Display(Name = "Data vencimento")]
        [DataType(DataType.Text)]
        public DateTime? DataVencimento { get; set; }

        [Display(Name = "Pessoa")]
        public int? PessoaId { get; set; }

        public string? Pessoa { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        public bool Pago { get; set; }

        [Display(Name = "Data pagamento")]
        [DataType(DataType.Text)]
        public DateTime? DataPagamento { get; set; }

        [Display(Name = "Valor pago")]
        public decimal? ValorPago { get; set; }

        [Display(Name = "Conta bancária")]
        public int? ContaBancariaId { get; set; }

        [Display(Name = "Data débito conta")]
        [DataType(DataType.Text)]
        public DateTime? DataContaBancaria { get; set; }

        public bool Parcelado { get; set; }

        [Display(Name = "Número parcelas")]
        public int? NumeroParcelas { get; set; }

        public TipoParcelamentoLancamento TipoParcelamento { get; set; }

        public IEnumerable<CategoriaListaModel> Categorias { get; set; }

        public IEnumerable<ContaBancariaListaModel> ContasBancarias { get; set; }

        public LancamentoInclusaoModel()
        {
            Categorias = Enumerable.Empty<CategoriaListaModel>();
            ContasBancarias = Enumerable.Empty<ContaBancariaListaModel>();
        }

        public LancamentoInclusaoModel(int grupoId, TipoParcelamentoLancamento tipoParcelamento, IEnumerable<CategoriaListaModel> categorias, IEnumerable<ContaBancariaListaModel> contasBancarias)
        {
            GrupoId = grupoId;
            TipoParcelamento = tipoParcelamento;

            Categorias = categorias;
            ContasBancarias = contasBancarias;
        }

        public void AtualizarCategoriasCartoesCredito(IEnumerable<CategoriaListaModel> categorias, IEnumerable<ContaBancariaListaModel> contasBancarias)
        {
            Categorias = categorias;
            ContasBancarias = contasBancarias;
        }
    }
}