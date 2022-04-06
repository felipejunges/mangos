using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class CategoriaMappers
    {
        public static IEnumerable<CategoriaListaModel> ToListaModel(IEnumerable<Categoria> categorias)
            => categorias.Select(ToListaModel);

        public static CategoriaListaModel ToListaModel(Categoria categoria)
        {
            return new CategoriaListaModel(
                id: categoria.Id,
                descricao: categoria.DescricaoComSuperior,
                receita: categoria.Receita,
                despesa: categoria.Despesa,
                relatorioTotal: categoria.RelatorioTotal,
                ativo: categoria.Ativo
            );
        }
    }
}