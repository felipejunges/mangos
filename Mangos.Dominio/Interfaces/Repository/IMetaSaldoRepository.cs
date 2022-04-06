using Mangos.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface IMetaSaldoRepository
    {
        Task<MetaSaldo?> ObterMetaSaldoAsync(int id);
        Task<List<MetaSaldo>> ListarMetasSaldoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal);
        Task IncluirAsync(MetaSaldo metaSaldo);
        Task AlterarAsync(MetaSaldo metaSaldo);
        Task RemoverAsync(MetaSaldo metaSaldo);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}