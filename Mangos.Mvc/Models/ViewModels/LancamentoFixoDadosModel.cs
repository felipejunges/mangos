using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Mvc.Models.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoFixoDadosModel : HashedModel
    {
        public TipoLancamentoFixo? Tipo { get; set; }

        public PeriodicidadeLancamentoFixo? Periodicidade { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        [Display(Name = "Dia vencimento")]
        public int? DiaVencimento { get; set; }

        [Display(Name = "Mês vencimento")]
        public int? MesVencimento { get; set; }

        [Display(Name = "Cartão crédito")]
        public int? CartaoCreditoId { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Pessoa")]
        public int? PessoaId { get; set; }

        public string? Pessoa { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

        [Display(Name = "Último mês gerado")]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Text)]
        public DateTime? DataUltimoMesGerado { get; set; }

        public bool Ativo { get; set; }

        public IEnumerable<CartaoCredito> CartoesCredito { get; set; }

        public IEnumerable<Categoria> Categorias { get; set; }

        public LancamentoFixoDadosModel()
        {
            CartoesCredito = Enumerable.Empty<CartaoCredito>();
            Categorias = Enumerable.Empty<Categoria>();
        }

        public LancamentoFixoDadosModel(int id, int grupoId, TipoLancamentoFixo? tipo, PeriodicidadeLancamentoFixo? periodicidade, decimal valor, int? diaVencimento, int? mesVencimento, int? cartaoCreditoId, string? descricao, int? pessoaId, string? pessoa, int? categoriaId, DateTime? dataUltimoMesGerado, bool ativo, IEnumerable<CartaoCredito> cartoesCredito, IEnumerable<Categoria> categorias)
        {
            Id = id;
            GrupoId = grupoId;
            Tipo = tipo;
            Periodicidade = periodicidade;
            Valor = valor;
            DiaVencimento = diaVencimento;
            MesVencimento = mesVencimento;
            CartaoCreditoId = cartaoCreditoId;
            Descricao = descricao;
            PessoaId = pessoaId;
            Pessoa = pessoa;
            CategoriaId = categoriaId;
            DataUltimoMesGerado = dataUltimoMesGerado;
            Ativo = ativo;
            CartoesCredito = cartoesCredito;
            Categorias = categorias;

            SetValidationHash();
        }

        public static LancamentoFixoDadosModel Novo(int grupoId, IEnumerable<CartaoCredito> cartoesCredito, IEnumerable<Categoria> categorias)
        {
            return new LancamentoFixoDadosModel(
                id: 0,
                grupoId: grupoId,
                tipo: null,
                periodicidade: null,
                valor: 0,
                diaVencimento: null,
                mesVencimento: null,
                cartaoCreditoId: null,
                descricao: null,
                pessoaId: null,
                pessoa: null,
                categoriaId: null,
                dataUltimoMesGerado: null,
                ativo: true,
                cartoesCredito: cartoesCredito,
                categorias: categorias
            );
        }

        public void AtualizarCartoesCreditoCategorias(IEnumerable<CartaoCredito> cartoesCredito, IEnumerable<Categoria> categorias)
        {
            CartoesCredito = cartoesCredito;
            Categorias = categorias;
        }
    }
}