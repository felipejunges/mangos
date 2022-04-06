using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoPagoInclusaoModel
    {
        public int GrupoId { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public decimal Valor { get; set; }

        [Display(Name = "Data pagamento")]
        [DataType(DataType.Text)]
        public DateTime? DataPagamento { get; set; }

        [Display(Name = "Pessoa")]
        public int? PessoaId { get; set; }

        public string? Pessoa { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

        [Display(Name = "Conta bancária")]
        public int? ContaBancariaId { get; set; }

        public IEnumerable<CategoriaListaModel> Categorias { get; set; }

        public IEnumerable<ContaBancariaListaModel> ContasBancarias { get; set; }

        public LancamentoPagoInclusaoModel()
        {
            Categorias = Enumerable.Empty<CategoriaListaModel>();
            ContasBancarias = Enumerable.Empty<ContaBancariaListaModel>();
        }

        public LancamentoPagoInclusaoModel(int grupoId, IEnumerable<CategoriaListaModel> categorias, IEnumerable<ContaBancariaListaModel> contasBancarias)
        {
            GrupoId = grupoId;

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