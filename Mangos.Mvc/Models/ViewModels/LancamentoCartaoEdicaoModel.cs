using Mangos.Dominio.Enums;
using Mangos.Mvc.Models.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoCartaoEdicaoModel : HashedModel
    {
        [Display(Name = "Cartão de crédito")]
        public int CartaoCreditoId { get; set; }

        [Display(Name = "Tipo de lançamento")]
        public TipoLancamentoCartao TipoLancamento { get; set; }

        public decimal Valor { get; set; }

        [Display(Name = "Mês referência")]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime MesReferencia { get; set; }

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
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Display(Name = "Fechado")]
        public bool GeradoLancamento { get; set; }

        public IEnumerable<CategoriaListaModel> Categorias { get; set; }

        public IEnumerable<CartaoCreditoListaModel> CartoesCredito { get; set; }

        public LancamentoCartaoEdicaoModel()
        {
            Categorias = Enumerable.Empty<CategoriaListaModel>();
            CartoesCredito = Enumerable.Empty<CartaoCreditoListaModel>();
        }

        public LancamentoCartaoEdicaoModel(int id, int grupoId, int cartaoCreditoId, TipoLancamentoCartao tipoLancamento, decimal valor, DateTime mesReferencia, string? descricao, int numeroParcela, int totalParcelas, int? pessoaId, string? pessoa, int? categoriaId, string? observacoes, bool geradoLancamento, IEnumerable<CategoriaListaModel> categorias, IEnumerable<CartaoCreditoListaModel> cartoesCredito)
        {
            Id = id;
            GrupoId = grupoId;
            CartaoCreditoId = cartaoCreditoId;
            TipoLancamento = tipoLancamento;
            Valor = valor;
            MesReferencia = mesReferencia;
            Descricao = descricao;
            NumeroParcela = numeroParcela;
            TotalParcelas = totalParcelas;
            PessoaId = pessoaId;
            Pessoa = pessoa;
            CategoriaId = categoriaId;
            Observacoes = observacoes;
            GeradoLancamento = geradoLancamento;
            Categorias = categorias;
            CartoesCredito = cartoesCredito;

            SetValidationHash();
        }

        public void AtualizarCategoriasCartoesCredito(IEnumerable<CategoriaListaModel> categorias, IEnumerable<CartaoCreditoListaModel> cartoesCredito)
        {
            Categorias = categorias;
            CartoesCredito = cartoesCredito;
        }
    }
}