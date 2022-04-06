using Mangos.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface ISessaoAcessoRepository
    {
        Task<SessaoAcesso?> ObterSessaoAcessoAsync(int id);
        Task<SessaoAcesso?> ObterSessaoAcessoUsuarioSessaoAsync(int usuarioId, string chaveSessao);
        Task<List<SessaoAcesso>> ListarSessoesAcessoAsync(DateTime dataInicial, DateTime dataFinal, bool buscarSessoesDeslogadas);
        Task IncluirAsync(SessaoAcesso sessaoAcesso);
        Task AlterarAsync(SessaoAcesso sessaoAcesso);
    }
}