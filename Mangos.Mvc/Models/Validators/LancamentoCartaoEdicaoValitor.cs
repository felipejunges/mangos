using FluentValidation;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Mvc.Models.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.Validators
{
    public class LancamentoCartaoEdicaoValitor : AbstractValidator<LancamentoCartaoEdicaoModel>
    {
        private ILancamentoCartaoRepository _lancamentoCartaoRepository;

        public LancamentoCartaoEdicaoValitor(ILancamentoCartaoRepository lancamentoCartaoRepository)
        {
            _lancamentoCartaoRepository = lancamentoCartaoRepository;

            RuleFor(x => x.Descricao).NotEmpty().WithMessage("A descrição é obrigatória.");
            RuleFor(x => x.Descricao).MaximumLength(255).WithMessage("O tamanho máximo da descrição é de 255 caracteres.");

            RuleFor(x => x.CartaoCreditoId).NotEmpty().WithMessage("O cartão de crédito é obrigatório.");

            RuleFor(x => x.Valor).MustAsync(ValidarValor).WithMessage("O valor de um lançamento fechado não pode ser alterado.");

            RuleFor(x => x.MesReferencia).NotEmpty().WithMessage("O mês de referência é obrigatório.");
            RuleFor(x => x.MesReferencia).MustAsync(ValidarMesReferencia).WithMessage("O mês de referência de um lançamento fechado não pode ser alterado.");
        }

        private async Task<bool> ValidarValor(LancamentoCartaoEdicaoModel lancamento, decimal valor, CancellationToken cancellationToken)
        {
            var lancamentoCartao = await _lancamentoCartaoRepository.ObterLancamentoCartaoAsync(lancamento.Id);

            if (lancamentoCartao is null) return false;

            return !lancamentoCartao.GeradoLancamento || lancamentoCartao.Valor == valor;
        }

        private async Task<bool> ValidarMesReferencia(LancamentoCartaoEdicaoModel lancamento, DateTime mesReferencia, CancellationToken cancellationToken)
        {
            var lancamentoCartao = await _lancamentoCartaoRepository.ObterLancamentoCartaoAsync(lancamento.Id);

            if (lancamentoCartao is null) return false;

            return !lancamentoCartao.GeradoLancamento || lancamentoCartao.MesReferencia == mesReferencia;
        }
    }
}