using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Models;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Extensions;
using Mangos.Mvc.Models;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Security;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UsuarioService _usuarioService;
        private readonly LoginService _loginService;
        private readonly TrocaSenhaUsuarioService _trocaSenhaUsuarioService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginController(IUsuarioRepository usuarioRepository, UsuarioService usuarioService, LoginService loginService, TrocaSenhaUsuarioService trocaSenhaUsuarioService, DataKeeperService dataKeeperService, IUserResolverService userResolverService, IUnitOfWork unitOfWork) : base(dataKeeperService, userResolverService)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _loginService = loginService;
            _trocaSenhaUsuarioService = trocaSenhaUsuarioService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Login(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl ?? string.Empty;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Throttle(Name = "LoginPost", TempoCache = 60)]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var resultLogin = await _loginService.LogarUsuario(model);

                if (!resultLogin.IsValid)
                {
                    ModelState.AddFromResult(resultLogin);
                }
                else
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _loginService.DeslogarUsuario();

            return RedirectToAction("Login", "Login");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult EsqueciSenha()
        {
            ViewData["FormLiberado"] = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Throttle(Name = "EsqueciSenhaPost", TempoCache = 60)]
        public async Task<IActionResult> EsqueciSenha(EsqueciSenhaModel esqueciSenhaModel)
        {
            if (ModelState.IsValid)
            {
                var trocaSenhaResult = await _trocaSenhaUsuarioService.SetarTokenRandomicoAsync(esqueciSenhaModel.Email);

                if (trocaSenhaResult.IsValid)
                {
                    ViewData["FormLiberado"] = false;
                    ViewData["MensagemRetorno"] = "E-mail enviado com sucesso! Verifique sua caixa de mensagens.";

                    return View();
                }
                else
                {
                    ModelState.AddFromResult(trocaSenhaResult);
                }
            }

            ViewData["FormLiberado"] = true;

            return View(esqueciSenhaModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> TrocarSenha(string id)
        {
            string token = id;

            var usuario = await _usuarioRepository.ObterUsuarioPeloTokenAsync(token);

            if (usuario == null)
            {
                ModelState.AddModelError("", "O link para troca de senha é inválido ou expirou.");

                ViewData["FormLiberado"] = false;

                return View();
            }

            //
            ViewData["FormLiberado"] = true;

            var trocarSenhaModel = new TrocarSenhaModel() { Token = token };

            return View(trocarSenhaModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Throttle(Name = "TrocarSenhaPost", TempoCache = 60)]
        public async Task<IActionResult> TrocarSenha(TrocarSenhaModel trocarSenhaModel)
        {
            Usuario? usuario = null;

            if (trocarSenhaModel.Token is not null)
                usuario = await _usuarioRepository.ObterUsuarioPeloTokenAsync(trocarSenhaModel.Token);

            if (usuario == null)
            {
                ModelState.AddModelError("", "O link para troca de senha é inválido ou expirou.");
                return View();
            }

            if (ModelState.IsValid)
            {
                usuario.AlterarSenha(trocarSenhaModel.Senha!);
                await _usuarioService.PersistirAsync(usuario);

                //
                ViewData["FormLiberado"] = false;
                ViewData["MensagemRetorno"] = "A senha foi alterada com sucesso! Você pode voltar ao login.";

                return View();
            }

            //
            ViewData["FormLiberado"] = true;

            return View(trocarSenhaModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Throttle(Name = "RegistroPost", TempoCache = 180, StepSegundos = 3)]
        public async Task<IActionResult> Registro(UsuarioInclusaoModel usuarioInclusaoModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = Usuario.NovoUsuario(usuarioInclusaoModel.Email!, usuarioInclusaoModel.Nome!, usuarioInclusaoModel.Senha!);

                await _usuarioRepository.IncluirAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                await _loginService.LogarUsuario(usuario, false);

                return RedirectToAction("Index", "Home");
            }

            return View(usuarioInclusaoModel);
        }
    }
}