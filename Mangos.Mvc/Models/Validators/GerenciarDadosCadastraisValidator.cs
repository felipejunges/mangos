using FluentValidation;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Mvc.Models.GerenciarConta;
using System.Threading;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.Validators
{
    public class GerenciarDadosCadastraisValidator : AbstractValidator<GerenciarDadosCadastraisModel>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public GerenciarDadosCadastraisValidator(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;

            RuleFor(x => x.Email).NotEmpty().WithMessage("O e-mail é obrigatório.");
            RuleFor(x => x.Email).MaximumLength(100).WithMessage("O tamanho máximo do e-mail é de 255 caracteres.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("O e-mail deve ser informado corretamente.");

            RuleFor(x => x.ConfirmeEmail).NotEmpty().WithMessage("A confirmação do e-mail é obrigatório.");
            RuleFor(x => x.ConfirmeEmail).EmailAddress().WithMessage("A confirmação do e-mail deve ser informado corretamente.");

            RuleFor(x => x.ConfirmeEmail).Equal(x => x.Email).WithMessage("O e-mail e a confirmação devem ser iguais.");

            RuleFor(x => x.Email).MustAsync(UniqueEmailAsync).When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("O e-mail informado já está cadastrado no sistema.");

            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome é obrigatório.");
            RuleFor(x => x.Nome).MaximumLength(100).WithMessage("O tamanho máximo do nome é de 100 caracteres.");

            RuleFor(x => x.SenhaAtual).MustAsync(SenhaAtualValidaAsync).When(x => !string.IsNullOrEmpty(x.SenhaAtual))
                .WithMessage("A senha atual é inválida.");

            RuleFor(x => x.SenhaAtual).NotEmpty().When(x => !string.IsNullOrEmpty(x.NovaSenha) || !string.IsNullOrEmpty(x.ConfirmeNovaSenha))
                .WithMessage("A senha atual deve ser informada para alterar a senha.");

            RuleFor(x => x.ConfirmeNovaSenha).Equal(x => x.NovaSenha).When(x => !string.IsNullOrEmpty(x.NovaSenha) || !string.IsNullOrEmpty(x.ConfirmeNovaSenha))
                .WithMessage("A senha e a confirmação devem ser iguais.");
        }

        private async Task<bool> UniqueEmailAsync(GerenciarDadosCadastraisModel usuario, string email, CancellationToken cancellationToken)
        {
            return !await _usuarioRepository.ValidarEmailJaRegistradoAsync(email, usuario.Id);
        }

        private async Task<bool> SenhaAtualValidaAsync(GerenciarDadosCadastraisModel usuario, string? senha, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(senha))
                return false;

            var usuarioDb = await _usuarioRepository.ObterUsuarioAtivoPeloEmailAsync(usuario.Email);

            if (usuarioDb is null)
                return false;

            return usuarioDb.Senha.Check(senha);
        }
    }
}