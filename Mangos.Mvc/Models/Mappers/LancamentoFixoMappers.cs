using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class LancamentoFixoMappers
    {
        public static IEnumerable<LancamentoFixoListaModel> ToListaModel(IEnumerable<LancamentoFixo> lancamentosFixos)
            => lancamentosFixos.Select(ToListaModel);

        public static LancamentoFixoListaModel ToListaModel(LancamentoFixo lancamentoFixo)
        {
            string dia = lancamentoFixo.DiaVencimento.ToString().PadLeft(2, '0');
            string mes = lancamentoFixo.MesVencimento.HasValue ? lancamentoFixo.MesVencimento.Value.ToString().PadLeft(2, '0') : "..";

            string vencimento = dia + "/" + mes;

            return new LancamentoFixoListaModel(
                id: lancamentoFixo.Id,
                tipo: lancamentoFixo.Tipo,
                periodicidade: lancamentoFixo.Periodicidade,
                vencimento: vencimento,
                valor: lancamentoFixo.Valor,
                descricao: lancamentoFixo.Descricao,
                pessoa: lancamentoFixo.Pessoa?.Nome ?? string.Empty,
                dataUltimoMesGerado: lancamentoFixo.DataUltimoMesGerado,
                dataHoraUltimaGeracao: lancamentoFixo.DataHoraUltimaGeracao,
                ativo: lancamentoFixo.Ativo
            );
        }
    }
}