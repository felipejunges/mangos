using FluentValidation;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Mvc.Models.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Mangos.Dominio.Validators
{
    public class UsuarioDadosValidator : AbstractValidator<UsuarioDadosModel>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioDadosValidator(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;

            RuleFor(x => x.GrupoId).NotEmpty().WithMessage("O grupo é obrigatório.");

            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome é obrigatório.");
            RuleFor(x => x.Nome).MaximumLength(100).WithMessage("O tamanho máximo do nome é de 100 caracteres.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("O e-mail é obrigatório.");
            RuleFor(x => x.Email).MaximumLength(255).WithMessage("O tamanho máximo do e-mail é de 255 caracteres.");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Email).MustAsync(UniqueEmailAsync).When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("O e-mail informado já está cadastrado no sistema.");

            RuleFor(x => x.Senha).NotEmpty().When(x => x.Id == 0).WithMessage("A senha deve ser informada.");
        }

        private async Task<bool> UniqueEmailAsync(UsuarioDadosModel usuario, string email, CancellationToken cancellationToken)
        {
            return !await _usuarioRepository.ValidarEmailJaRegistradoAsync(email, usuario.Id);
        }
    }
}