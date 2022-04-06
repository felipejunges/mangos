using Mangos.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoCartaoInclusaoModel
    {
        public int GrupoId { get; set; }

        [Display(Name = "Cartão de crédito")]
        public int CartaoCreditoId { get; set; }

        [Display(Name = "Tipo de lançamento")]
        public TipoLancamentoCartao TipoLancamento { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Pessoa")]
        public int? PessoaId { get; set; }

        public string? Pessoa { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

        [Display(Name = "Valor total")]
        public decimal ValorTotal { get; set; }

        public bool Parcelado { get; set; }

        [Display(Name = "Número de parcelas")]
        public int? NumeroParcelas { get; set; }

        [Display(Name = "Mês referência")]
        [DataType(DataType.Text)]
        public DateTime? DataReferencia { get; set; }

        public IEnumerable<CategoriaListaModel> Categorias { get; set; }

        public IEnumerable<CartaoCreditoListaModel> CartoesCredito { get; set; }

        public LancamentoCartaoInclusaoModel()
        {
            Categorias = Enumerable.Empty<CategoriaListaModel>();
            CartoesCredito = Enumerable.Empty<CartaoCreditoListaModel>();
        }

        public LancamentoCartaoInclusaoModel(int grupoId, TipoLancamentoCartao tipoLancamento, IEnumerable<CategoriaListaModel> categorias, IEnumerable<CartaoCreditoListaModel> cartoesCredito)
        {
            GrupoId = grupoId;
            TipoLancamento = tipoLancamento;
            Categorias = categorias;
            CartoesCredito = cartoesCredito;
        }

        public void AtualizarCategoriasCartoesCredito(IEnumerable<CategoriaListaModel> categorias, IEnumerable<CartaoCreditoListaModel> cartoesCredito)
        {
            Categorias = categorias;
            CartoesCredito = cartoesCredito;
        }
    }
}