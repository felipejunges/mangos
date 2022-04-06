using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Models;
using Mangos.Dominio.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class NotificacaoService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;

        public NotificacaoService(ILancamentoRepository lancamentoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository)
        {
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
        }

        public async Task<List<ItemNotificacaoModel>> ObterNotificacoesAsync(Usuario usuario)
        {
            var notificacoes = new List<ItemNotificacaoModel>();

            if (usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Ambos || usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Receitas)
            {
                var receitas = await _lancamentoRepository.ListarLancamentosAlertaAsync(usuario.GrupoId, usuario.DiasAlertaVencimentos, TipoLancamento.Receita);

                if (receitas.Count > 0)
                    notificacoes.Add(new ItemNotificacaoModel($"{receitas.Count} receita{(receitas.Count > 1 ? "s" : string.Empty)} à vencer", "~/ConsultaVencimento"));
            }

            if (usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Ambos || usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Despesas)
            {
                var despesas = await _lancamentoRepository.ListarLancamentosAlertaAsync(usuario.GrupoId, usuario.DiasAlertaVencimentos, TipoLancamento.Despesa);

                if (despesas.Count > 0)
                    notificacoes.Add(new ItemNotificacaoModel($"{despesas.Count} despesa{(despesas.Count > 1 ? "s" : string.Empty)} à vencer", "~/ConsultaVencimento"));
            }

            if (usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Ambos || usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Despesas)
            {
                var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoAlertaAsync(usuario.GrupoId);

                if (lancamentosCartao.Count > 0)
                    notificacoes.Add(new ItemNotificacaoModel($"{lancamentosCartao.Count} lançamento{(lancamentosCartao.Count > 1 ? "s" : string.Empty)} cartão à fechar", "~/LancamentoCartao"));
            }

            return notificacoes;
        }
    }
}