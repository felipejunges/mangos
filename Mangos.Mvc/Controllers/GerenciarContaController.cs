using Mangos.Dominio.Enums;
using Mangos.Dominio.Extensions;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.GerenciarConta;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class GerenciarContaController : BaseController
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UsuarioService _usuarioService;
        private readonly IGrupoRepository _grupoRepository;
        private readonly GrupoService _grupoService;

        public GerenciarContaController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, IUsuarioRepository usuarioRepository, UsuarioService usuarioService, IGrupoRepository grupoRepository, GrupoService grupoService) : base(dataKeeperService, userResolverService)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _grupoRepository = grupoRepository;
            _grupoService = grupoService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(DadosCadastrais));
        }

        public async Task<IActionResult> DadosCadastrais()
        {
            var usuario = await _usuarioRepository.ObterUsuarioAsync(_userResolverService.UsuarioId);

            if (usuario == null)
                return NotFound();

            var gerenciarDadosCadastraisModel = new GerenciarDadosCadastraisModel(
                id: usuario.Id,
                email: usuario.Email,
                confirmeEmail: usuario.Email,
                nome: usuario.Nome
            );

            //
            return View(gerenciarDadosCadastraisModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DadosCadastrais(GerenciarDadosCadastraisModel gerenciarDadosCadastraisModel)
        {
            if (ModelState.IsValid)
            {
                if (gerenciarDadosCadastraisModel.Id != _userResolverService.UsuarioId)
                    return BadRequest();

                var usuario = await _usuarioRepository.ObterUsuarioAsync(_userResolverService.UsuarioId);

                if (usuario == null)
                    return NotFound();

                usuario.Nome = gerenciarDadosCadastraisModel.Nome;
                usuario.Email = gerenciarDadosCadastraisModel.Email;

                if (!string.IsNullOrEmpty(gerenciarDadosCadastraisModel.NovaSenha))
                {
                    usuario.AlterarSenha(gerenciarDadosCadastraisModel.NovaSenha);
                }

                await _usuarioService.PersistirAsync(usuario);

                gerenciarDadosCadastraisModel.Sucesso = true;
            }
            else
            {
                gerenciarDadosCadastraisModel.Sucesso = false;
            }

            //
            return View(gerenciarDadosCadastraisModel);
        }

        public async Task<IActionResult> AlertasVencimentos()
        {
            var usuario = await _usuarioRepository.ObterUsuarioAsync(_userResolverService.UsuarioId);

            if (usuario == null)
                return NotFound();

            var gerenciarAlertasVencimentosModel = new GerenciarAlertasVencimentosModel(
                tipoAlertaVencimentos: usuario.TipoAlertaVencimentos,
                diasAlertaVencimentos: usuario.DiasAlertaVencimentos,
                tipoEmailVencimentos: usuario.TipoEmailVencimentos,
                diasEmailVencimentos: usuario.DiasEmailVencimentos
            );

            //
            AtualizarTiposGerenciarAlertasVencimentos(gerenciarAlertasVencimentosModel);

            return View(gerenciarAlertasVencimentosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlertasVencimentos(GerenciarAlertasVencimentosModel gerenciarAlertasVencimentosModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioRepository.ObterUsuarioAsync(_userResolverService.UsuarioId);

                if (usuario is null)
                    return BadRequest();

                usuario.TipoAlertaVencimentos = gerenciarAlertasVencimentosModel.TipoAlertaVencimentos;
                usuario.DiasAlertaVencimentos = gerenciarAlertasVencimentosModel.DiasAlertaVencimentos;
                usuario.TipoEmailVencimentos = gerenciarAlertasVencimentosModel.TipoEmailVencimentos;
                usuario.DiasEmailVencimentos = gerenciarAlertasVencimentosModel.DiasEmailVencimentos;

                await _usuarioService.PersistirAsync(usuario);

                gerenciarAlertasVencimentosModel.Sucesso = true;
            }
            else
            {
                gerenciarAlertasVencimentosModel.Sucesso = false;
            }

            //
            AtualizarTiposGerenciarAlertasVencimentos(gerenciarAlertasVencimentosModel);

            return View(gerenciarAlertasVencimentosModel);
        }

        public async Task<IActionResult> ConfiguracoesGrupo()
        {
            var grupo = await _grupoRepository.ObterGrupoAsync(_userResolverService.GrupoId);

            if (grupo == null)
                return NotFound();

            var gerenciarConfiguracoesGrupoModel = new GerenciarConfiguracoesGrupoModel(
                mesesAntecedenciaGerarLancamento: grupo.MesesAntecedenciaGerarLancamento,
                mesesAntecedenciaGerarLancamentoCartao: grupo.MesesAntecedenciaGerarLancamentoCartao,
                mesesGraficosDashboard: grupo.MesesGraficosDashboard
            );

            //
            return View(gerenciarConfiguracoesGrupoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfiguracoesGrupo(GerenciarConfiguracoesGrupoModel gerenciarConfiguracoesGrupoModel)
        {
            if (ModelState.IsValid)
            {
                var grupo = await _grupoRepository.ObterGrupoAsync(_userResolverService.GrupoId);

                if (grupo == null)
                    return NotFound();

                grupo.MesesAntecedenciaGerarLancamento = gerenciarConfiguracoesGrupoModel.MesesAntecedenciaGerarLancamento;
                grupo.MesesAntecedenciaGerarLancamentoCartao = gerenciarConfiguracoesGrupoModel.MesesAntecedenciaGerarLancamentoCartao;
                grupo.MesesGraficosDashboard = gerenciarConfiguracoesGrupoModel.MesesGraficosDashboard;

                await _grupoService.PersistirAsync(grupo);

                gerenciarConfiguracoesGrupoModel.Sucesso = true;
            }
            else
            {
                gerenciarConfiguracoesGrupoModel.Sucesso = false;
            }

            //
            return View(gerenciarConfiguracoesGrupoModel);
        }

        private void AtualizarTiposGerenciarAlertasVencimentos(GerenciarAlertasVencimentosModel model)
        {
            var tiposAvisoVencimentosUsuario = new Dictionary<string, string>()
            {
                { TipoAvisoVencimentosUsuario.Nenhum.ToString(), TipoAvisoVencimentosUsuario.Nenhum.GetDescription() },
                { TipoAvisoVencimentosUsuario.Ambos.ToString(), TipoAvisoVencimentosUsuario.Ambos.GetDescription() },
                { TipoAvisoVencimentosUsuario.Receitas.ToString(), TipoAvisoVencimentosUsuario.Receitas.GetDescription() },
                { TipoAvisoVencimentosUsuario.Despesas.ToString(), TipoAvisoVencimentosUsuario.Despesas.GetDescription() }
            };

            var tiposEmailVencimentosUsuario = new Dictionary<string, string>()
            {
                { TipoAvisoVencimentosUsuario.Nenhum.ToString(), TipoAvisoVencimentosUsuario.Nenhum.GetDescription() },
                { TipoAvisoVencimentosUsuario.Ambos.ToString(), TipoAvisoVencimentosUsuario.Ambos.GetDescription() },
                { TipoAvisoVencimentosUsuario.Receitas.ToString(), TipoAvisoVencimentosUsuario.Receitas.GetDescription() },
                { TipoAvisoVencimentosUsuario.Despesas.ToString(), TipoAvisoVencimentosUsuario.Despesas.GetDescription() }
            };

            model.AtualizarTipos(tiposAvisoVencimentosUsuario, tiposEmailVencimentosUsuario);
        }
    }
}