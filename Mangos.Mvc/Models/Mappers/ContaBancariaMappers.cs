using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class ContaBancariaMappers
    {
        public static IEnumerable<ContaBancariaListaModel> ToListaModel(IEnumerable<ContaBancaria> contasBancarias)
            => contasBancarias.Select(ToListaModel);

        public static ContaBancariaListaModel ToListaModel(ContaBancaria contaBancaria)
        {
            return new ContaBancariaListaModel(
                id: contaBancaria.Id,
                descricao: contaBancaria.Descricao,
                numeroBanco: contaBancaria.NumeroBanco,
                agencia: contaBancaria.Agencia,
                numeroConta: contaBancaria.NumeroConta,
                ativo: contaBancaria.Ativo,
                saldoInicial: contaBancaria.SaldoInicial
            );
        }
    }
}