using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class DespesaRapidaViewModel
    {
        public DespesaRapidaViewModel()
        {
            ContasBancarias = new HashSet<ContaBancariaListaModel>();
            CartoesCredito = new HashSet<CartaoCreditoListaModel>();
        }

        public int? PessoaId { get; set; }

        public string? Pessoa { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public decimal? Valor { get; set; }

        [Display(Name = "Conta bancária")]
        public int? ContaBancariaId { get; set; }

        [Display(Name = "Cartão crédito")]
        public int? CartaoCreditoId { get; set; }

        [Display(Name = "Atualizar lat./long. do fornecedor")]
        public bool AtualizarCoordenadas { get; set; }

        public int? PessoaCoordenadaIdAtualizar { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public IEnumerable<ContaBancariaListaModel> ContasBancarias { get; set; }

        public IEnumerable<CartaoCreditoListaModel> CartoesCredito { get; set; }
    }
}