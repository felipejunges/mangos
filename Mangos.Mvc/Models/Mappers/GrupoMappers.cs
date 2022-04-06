using Mangos.Dominio.Models;
using Mangos.Mvc.Models.ViewModels;

namespace Mangos.Mvc.Models.Mappers
{
    public static class GrupoMappers
    {
        public static GrupoDetalhesModel ToGrupoDetalhes(GrupoRelacionamentos grupo)
        {
            return new GrupoDetalhesModel(
                            id: grupo.Id,
                            descricao: grupo.Descricao,
                            pessoas: grupo.Pessoas,
                            usuarios: grupo.Usuarios,
                            categorias: grupo.Categorias,
                            contasBancarias: grupo.ContasBancarias,
                            cartoesCredito: grupo.CartoesCredito,
                            receitas: grupo.Receitas,
                            despesas: grupo.Despesas,
                            lancamentosCartao: grupo.LancamentosCartao,
                            transferenciasConta: grupo.TransferenciasConta,
                            lancamentosFixos: grupo.LancamentosFixos
                        );
        }
    }
}