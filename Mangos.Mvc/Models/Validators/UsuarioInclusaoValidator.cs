using FluentValidation;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Mvc.Models.ViewModels;
using System.Threading;
using System.Threading.Tasks;


namespace Mangos.Dominio.Validators
{
    public class UsuarioInclusaoValidator : AbstractValidator<UsuarioInclusaoModel>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioInclusaoValidator(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;

            RuleFor(x => x.Email).NotEmpty().WithMessage("O e-mail é obrigatório.");
            RuleFor(x => x.Email).MaximumLength(255).WithMessage("O tamanho máximo do e-mail é de 255 caracteres.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("O e-mail deve ser informado corretamente.");

            RuleFor(x => x.ConfirmeEmail).NotEmpty().WithMessage("A confirmação do e-mail é obrigatório.");
            RuleFor(x => x.ConfirmeEmail).EmailAddress().WithMessage("A confirmação do e-mail deve ser informado corretamente.");

            RuleFor(x => x.ConfirmeEmail).Equal(x => x.Email).WithMessage("O e-mail e a confirmação devem ser iguais.");

            RuleFor(x => x.Email).MustAsync(UniqueEmailAsync).When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("O e-mail informado já está cadastrado no sistema.");

            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome é obrigatório.");
            RuleFor(x => x.Nome).MaximumLength(100).WithMessage("O tamanho máximo do nome é de 100 caracteres.");

            RuleFor(x => x.Senha).NotEmpty().WithMessage("A senha deve ser informada.");
            RuleFor(x => x.ConfirmeSenha).Equal(x => x.Senha).WithMessage("A senha e a confirmação devem ser iguais.");
        }

        private async Task<bool> UniqueEmailAsync(UsuarioInclusaoModel usuario, string? email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            return !await _usuarioRepository.ValidarEmailJaRegistradoAsync(email, 0);
        }
    }
}