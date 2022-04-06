using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class RendimentoMensalContaDadosModel : HashedModel
    {
        [Display(Name = "Conta bancária")]
        public int? ContaBancariaId { get; set; }

        [Display(Name = "Mês referência")]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? MesReferencia { get; set; }

        public decimal? Valor { get; set; }

        public IEnumerable<ContaBancaria> ContasBancarias { get; set; }

        public RendimentoMensalContaDadosModel()
        {
            ContasBancarias = Enumerable.Empty<ContaBancaria>();
        }

        public RendimentoMensalContaDadosModel(int id, int grupoId, int? contaBancariaId, DateTime? mesReferencia, decimal? valor, IEnumerable<ContaBancaria> contasBancarias)
        {
            Id = id;
            GrupoId = grupoId;
            ContaBancariaId = contaBancariaId;
            MesReferencia = mesReferencia;
            Valor = valor;
            ContasBancarias = contasBancarias;

            SetValidationHash();
        }

        public static RendimentoMensalContaDadosModel Novo(int grupoId, IEnumerable<ContaBancaria> contasBancarias)
        {
            return new RendimentoMensalContaDadosModel(
                id: 0,
                grupoId: grupoId,
                contaBancariaId: null,
                mesReferencia: null,
                valor: null,
                contasBancarias: contasBancarias
            );
        }

        public void AtualizarContasBancarias(IEnumerable<ContaBancaria> contasBancarias)
        {
            ContasBancarias = contasBancarias;
        }
    }
}