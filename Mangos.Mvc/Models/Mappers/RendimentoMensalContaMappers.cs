using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class RendimentoMensalContaMappers
    {
        public static IEnumerable<RendimentoMensalContaListaModel> ToListaModel(IEnumerable<RendimentoMensalConta> rendimentosMensaisConta)
            => rendimentosMensaisConta.Select(ToListaModel);

        public static RendimentoMensalContaListaModel ToListaModel(RendimentoMensalConta rendimentoMensalConta)
        {
            return new RendimentoMensalContaListaModel(
                id: rendimentoMensalConta.Id,
                contaBancaria: rendimentoMensalConta.ContaBancaria?.Descricao ?? string.Empty,
                mesReferencia: rendimentoMensalConta.MesReferencia,
                valor: rendimentoMensalConta.Valor
            );
        }
    }
}