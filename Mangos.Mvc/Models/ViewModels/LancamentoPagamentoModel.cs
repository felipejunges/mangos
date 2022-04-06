using Mangos.Mvc.Models.ViewModels.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoPagamentoModel : HashedModel
    {
        [Display(Name = "Data pagamento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Text)]
        public DateTime? DataPagamento { get; set; }

        [Display(Name = "Conta bancária")]
        public int? ContaBancariaId { get; set; }
    }
}