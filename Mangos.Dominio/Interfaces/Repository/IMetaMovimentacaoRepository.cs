using Mangos.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface IMetaMovimentacaoRepository
    {
        Task<MetaMovimentacao?> ObterMetaMovimentacaoAsync(int id);
        Task<List<MetaMovimentacao>> ListarMetasMovimentacaoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal);
        Task IncluirAsync(MetaMovimentacao metaMovimentacao);
        Task AlterarAsync(MetaMovimentacao metaMovimentacao);
        Task RemoverAsync(MetaMovimentacao metaMovimentacao);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}