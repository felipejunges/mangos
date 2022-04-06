using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class CartaoCreditoDadosModel : HashedModel
    {
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Categoria padrão")]
        public int? CategoriaId { get; set; }

        [Display(Name = "Dia fechamento")]
        public int DiaFechamento { get; set; }

        [Display(Name = "Dia vencimento")]
        public int DiaVencimento { get; set; }

        [Display(Name = "Diferença ref. x venc.")]
        public bool OffsetReferenciaVencimento { get; set; }

        [Display(Name = "Limite")]
        public decimal ValorLimite { get; set; }

        [Display(Name = "Exibir dados no relatório")]
        public bool ExibirDadosRelatorio { get; set; }

        [Display(Name = "Gerar lançamento ao fechar mês")]
        public bool GerarLancamentoFecharMes { get; set; }

        public bool Ativo { get; set; }

        public IEnumerable<Categoria> Categorias { get; set; }

        public CartaoCreditoDadosModel()
        {
            Categorias = Enumerable.Empty<Categoria>();
        }

        public CartaoCreditoDadosModel(int id, int grupoId, string? descricao, int? categoriaId, int diaFechamento, int diaVencimento, bool offsetReferenciaVencimento, decimal valorLimite, bool exibirDadosRelatorio, bool gerarLancamentoFecharMes, bool ativo, IEnumerable<Categoria> categorias)
        {
            Id = id;
            GrupoId = grupoId;
            Descricao = descricao;
            CategoriaId = categoriaId;
            DiaFechamento = diaFechamento;
            DiaVencimento = diaVencimento;
            OffsetReferenciaVencimento = offsetReferenciaVencimento;
            ValorLimite = valorLimite;
            ExibirDadosRelatorio = exibirDadosRelatorio;
            GerarLancamentoFecharMes = gerarLancamentoFecharMes;
            Ativo = ativo;
            Categorias = categorias;

            SetValidationHash();
        }

        public static CartaoCreditoDadosModel Novo(int grupoId, IEnumerable<Categoria> categorias)
        {
            return new CartaoCreditoDadosModel(
                id: 0,
                grupoId: grupoId,
                descricao: null,
                categoriaId: null,
                diaFechamento: 0,
                diaVencimento: 0,
                offsetReferenciaVencimento: false,
                valorLimite: 0,
                exibirDadosRelatorio: true,
                gerarLancamentoFecharMes: false,
                ativo: true,
                categorias: categorias
            );
        }

        public void AtualizarCategorias(IEnumerable<Categoria> categorias)
        {
            Categorias = categorias;
        }
    }
}