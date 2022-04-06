using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class TransferenciaContaMappers
    {
        public static IEnumerable<TransferenciaContaListaModel> ToListaModel(IEnumerable<TransferenciaConta> transferenciasConta)
            => transferenciasConta.Select(ToListaModel);

        public static TransferenciaContaListaModel ToListaModel(TransferenciaConta transferenciaConta)
        {
            return new TransferenciaContaListaModel(
                id: transferenciaConta.Id,
                valor: transferenciaConta.Valor,
                dataDebito: transferenciaConta.DataDebito,
                dataCredito: transferenciaConta.DataCredito,
                descricao: transferenciaConta.Descricao,
                contaBancariaOrigem: transferenciaConta.ContaBancariaOrigem?.Descricao ?? string.Empty,
                contaBancariaDestino: transferenciaConta.ContaBancariaDestino?.Descricao ?? string.Empty
            );
        }
    }
}