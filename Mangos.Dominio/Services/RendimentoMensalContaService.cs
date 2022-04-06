using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class RendimentoMensalContaService
    {
        private readonly SaldoContaBancariaService _saldoContaBancariaService;
        private readonly IRendimentoMensalContaRepository _rendimentoMensalContaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RendimentoMensalContaService(SaldoContaBancariaService saldoContaBancariaService, IRendimentoMensalContaRepository rendimentoMensalContaRepository, IUnitOfWork unitOfWork)
        {
            _saldoContaBancariaService = saldoContaBancariaService;
            _rendimentoMensalContaRepository = rendimentoMensalContaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PersistirAsync(RendimentoMensalConta rendimentoMensalConta)
        {
            RendimentoMensalConta? rendimentoMensalContaAtual = null;
            if (rendimentoMensalConta.Id != 0)
                rendimentoMensalContaAtual = await _rendimentoMensalContaRepository.ObterRendimentoMensalContaNoTrackingAsync(rendimentoMensalConta.Id);

            await _unitOfWork.BeginTransactionAsync();

            if (rendimentoMensalConta.Id == 0)
                await _rendimentoMensalContaRepository.IncluirAsync(rendimentoMensalConta);
            else
                await _rendimentoMensalContaRepository.AtualizarAsync(rendimentoMensalConta);

            await _unitOfWork.SaveChangesAsync();

            // seta o saldo da conta corrente
            await _saldoContaBancariaService.SetarSaldo(
                rendimentoMensalContaAtual?.ContaBancariaId,
                rendimentoMensalConta.ContaBancariaId,
                rendimentoMensalContaAtual?.MesReferencia,
                rendimentoMensalConta.MesReferencia);

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task RemoverAsync(RendimentoMensalConta rendimentoMensalConta)
        {
            var contaBancariaId = rendimentoMensalConta.ContaBancariaId;
            var mesReferencia = rendimentoMensalConta.MesReferencia;

            await _unitOfWork.BeginTransactionAsync();

            await _rendimentoMensalContaRepository.RemoverAsync(rendimentoMensalConta);

            await _unitOfWork.SaveChangesAsync();

            await _saldoContaBancariaService.SetarSaldo(contaBancariaId, mesReferencia);

            await _unitOfWork.CommitTransactionAsync();
        }
    }
}