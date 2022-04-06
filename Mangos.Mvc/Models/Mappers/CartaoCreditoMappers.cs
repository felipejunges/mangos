using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class CartaoCreditoMappers
    {
        public static IEnumerable<CartaoCreditoListaModel> ToListaModel(IEnumerable<CartaoCredito> cartoesCredito)
            => cartoesCredito.Select(ToListaModel);

        public static CartaoCreditoListaModel ToListaModel(CartaoCredito cartaoCredito)
        {
            return new CartaoCreditoListaModel(
                id: cartaoCredito.Id,
                descricao: cartaoCredito.Descricao,
                categoria: cartaoCredito.Categoria?.Descricao ?? string.Empty,
                ativo: cartaoCredito.Ativo
            );
        }
    }
}