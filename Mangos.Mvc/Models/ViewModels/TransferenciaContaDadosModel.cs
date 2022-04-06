using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class TransferenciaContaDadosModel : HashedModel
    {
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        [Display(Name = "Data débito")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Text)]
        public DateTime? DataDebito { get; set; }

        [Display(Name = "Data crédito")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Text)]
        public DateTime? DataCredito { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Conta origem")]
        public int? ContaBancariaOrigemId { get; set; }

        [Display(Name = "Conta destino")]
        public int? ContaBancariaDestinoId { get; set; }

        public IEnumerable<ContaBancaria> ContasBancariasOrigem { get; set; }

        public IEnumerable<ContaBancaria> ContasBancariasDestino { get; set; }

        public TransferenciaContaDadosModel()
        {
            ContasBancariasOrigem = Enumerable.Empty<ContaBancaria>();
            ContasBancariasDestino = Enumerable.Empty<ContaBancaria>();
        }

        public TransferenciaContaDadosModel(int id, int grupoId, decimal valor, DateTime? dataDebito, DateTime? dataCredito, string? descricao, int? contaBancariaOrigemId, int? contaBancariaDestinoId, IEnumerable<ContaBancaria> contasBancariasOrigem, IEnumerable<ContaBancaria> contasBancariasDestino)
        {
            Id = id;
            GrupoId = grupoId;
            Valor = valor;
            DataDebito = dataDebito;
            DataCredito = dataCredito;
            Descricao = descricao;
            ContaBancariaOrigemId = contaBancariaOrigemId;
            ContaBancariaDestinoId = contaBancariaDestinoId;
            ContasBancariasOrigem = contasBancariasOrigem;
            ContasBancariasDestino = contasBancariasDestino;

            SetValidationHash();
        }

        public static TransferenciaContaDadosModel Novo(int grupoId, IEnumerable<ContaBancaria> contasBancariasOrigem, IEnumerable<ContaBancaria> contasBancariasDestino)
        {
            return new TransferenciaContaDadosModel(
                id: 0,
                grupoId: grupoId,
                valor: 0,
                dataDebito: null,
                dataCredito: null,
                descricao: null,
                contaBancariaOrigemId: null,
                contaBancariaDestinoId: null,
                contasBancariasOrigem: contasBancariasOrigem,
                contasBancariasDestino: contasBancariasDestino
            );
        }

        public void AtualizarContasBancarias(IEnumerable<ContaBancaria> contasBancariasOrigem, IEnumerable<ContaBancaria> contasBancariasDestino)
        {
            ContasBancariasOrigem = contasBancariasOrigem;
            ContasBancariasDestino = contasBancariasDestino;
        }
    }
}