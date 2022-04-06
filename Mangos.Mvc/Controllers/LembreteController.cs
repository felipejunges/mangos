using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.Interface;
using Mangos.Dominio.Services.User;
using Mangos.Dominio.Utils;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    public class LembreteController : BaseController
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;

        public LembreteController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, IEmailService emailService, ILancamentoRepository lancamentoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository, NotificacaoService notificacaoService, IUsuarioRepository usuarioRepository) : base(dataKeeperService, userResolverService)
        {
            _emailService = emailService;
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _usuarioRepository = usuarioRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> EmailVencimentos()
        {
            int quantidadeEmailsEnviados = 0;

            var usuarios = await _usuarioRepository.ListarUsuariosEmailAvisoVencimentosAsync();

            foreach (var usuario in usuarios)
            {
                //
                var receitas =
                    usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Ambos || usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Receitas
                        ? await _lancamentoRepository.ListarLancamentosAlertaAsync(usuario.GrupoId, usuario.DiasEmailVencimentos, TipoLancamento.Receita)
                        : null;

                var despesas =
                    usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Ambos || usuario.TipoEmailVencimentos == TipoAvisoVencimentosUsuario.Despesas
                        ? await _lancamentoRepository.ListarLancamentosAlertaAsync(usuario.GrupoId, usuario.DiasEmailVencimentos, TipoLancamento.Despesa)
                        : null;

                var lancamentosCartao =
                    usuario.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Ambos || usuario.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Despesas
                        ? await _lancamentoCartaoRepository.ListarLancamentosCartaoAlertaAsync(usuario.GrupoId)
                        : null;

                //
                if ((receitas != null && receitas.Count > 0) || (despesas != null && despesas.Count > 0))
                {
                    StringBuilder textoEmail = new StringBuilder();
                    textoEmail.Append($"Olá {usuario.Nome}!<br /><br />");
                    textoEmail.Append($"Este é um lembrete de vencimentos do Mangos - Sistema Financeiro dos próximos <b>{usuario.DiasEmailVencimentos.ToString()}</b> dias.<br /><br />");

                    textoEmail.Append("<table cellpadding=\"2\" cellspacing=\"4\" style=\"width: 100%;\"><tbody>");

                    if (receitas != null && receitas.Count > 0)
                    {
                        textoEmail.Append("<tr><th colspan=\"4\" style=\"text-align: center; font-size: 16px; background-color: #337ab7; color: #C9DEEF; line-height: 24px;\">Receitas</th></tr>");
                        textoEmail.Append("<tr><th>Descrição</th><th>Cliente</th><th>Vencimento</th><th style=\"text-align: right;\">Valor</th></tr>");

                        foreach (var receita in receitas)
                        {
                            textoEmail.Append($"<tr><td>{receita.Descricao}</td><td>{receita.Pessoa?.Nome}</td><td>{receita.DataVencimento.ToString("dd/MM/yyyy (ddd)")}</td><td style=\"text-align: right;\">{receita.Valor.ToString("c2")}</td></tr>");
                        }
                    }

                    if (despesas != null && despesas.Count > 0)
                    {
                        textoEmail.Append("<tr><th colspan=\"4\" style=\"text-align: center; font-size: 16px; background-color: #337ab7; color: #C9DEEF; line-height: 24px;\">Despesas</th></tr>");
                        textoEmail.Append("<tr><th>Descrição</th><th>Cliente</th><th>Vencimento</th><th style=\"text-align: right;\">Valor</th></tr>");

                        foreach (var despesa in despesas)
                        {
                            textoEmail.Append($"<tr><td>{despesa.Descricao}</td><td>{despesa.Pessoa?.Nome}</td><td>{despesa.DataVencimento.ToString("dd/MM/yyyy (ddd)")}</td><td style=\"text-align: right;\">{despesa.Valor.ToString("c2")}</td></tr>");
                        }
                    }

                    if (lancamentosCartao != null && lancamentosCartao.Count > 0)
                    {
                        textoEmail.Append("<tr><th colspan=\"4\" style=\"text-align: center; font-size: 16px; background-color: #337ab7; color: #C9DEEF; line-height: 24px;\">Lançamentos cartão</th></tr>");
                        textoEmail.Append("<tr><th>Descrição</th><th>Cliente</th><th>Vencimento</th><th style=\"text-align: right;\">Valor</th></tr>");

                        foreach (var lancamentoCartao in lancamentosCartao)
                        {
                            textoEmail.Append($"<tr><td>{lancamentoCartao.Descricao}</td><td>{lancamentoCartao.Pessoa?.Nome}</td><td>{lancamentoCartao.MesReferencia.ToString("MMM/yyyy")}</td><td style=\"text-align: right;\">{lancamentoCartao.Valor.ToString("c2")}</td></tr>");
                        }
                    }

                    textoEmail.Append("</tbody></table>");

                    try
                    {
                        var emailRetorno = MailMessageFactory.Create(usuario.Email, usuario.Nome, "Mangos | Lembrete de vencimentos", textoEmail.ToString(), null);

                        await _emailService.EnviarAsync(emailRetorno);

                        quantidadeEmailsEnviados++;
                    }
                    catch { }
                }
            }

            return Content($"{quantidadeEmailsEnviados} e-mails enviados");
        }
    }
}