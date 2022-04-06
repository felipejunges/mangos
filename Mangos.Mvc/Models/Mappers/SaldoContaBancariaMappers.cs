using Mangos.Dominio.Entities;
using Mangos.Dominio.Extensions;
using Mangos.Mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class SaldoContaBancariaMappers
    {
        public static IEnumerable<SaldoContaBancariaListaModel> ToListaModel(IEnumerable<SaldoContaBancaria> saldosContaBancarias)
            => saldosContaBancarias.Select(ToListaModel);

        public static SaldoContaBancariaListaModel ToListaModel(SaldoContaBancaria saldoContaBancaria)
        {
            return new SaldoContaBancariaListaModel(
                id: saldoContaBancaria.Id,
                contaBancariaId: saldoContaBancaria.ContaBancariaId,
                contaBancaria: saldoContaBancaria.ContaBancaria?.Descricao ?? string.Empty,
                data: saldoContaBancaria.Data,
                valorSaldo: saldoContaBancaria.ValorSaldo,
                valorMovimentacoes: saldoContaBancaria.ValorMovimentacoes,
                ultimaConferenciaSaldo: saldoContaBancaria.DataHoraConferenciaSaldo.ToDiaDiff(),
                valorConferenciaSaldo: saldoContaBancaria.ValorConferenciaSaldo,
                fechado: saldoContaBancaria.Fechado
            );
        }
    }
}