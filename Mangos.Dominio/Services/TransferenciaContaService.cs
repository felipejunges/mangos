using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using System;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class TransferenciaContaService
    {
        private readonly SaldoContaBancariaService _saldoContaBancariaService;
        private readonly ITransferenciaContaRepository _transferenciaContaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferenciaContaService(SaldoContaBancariaService saldoContaBancariaService, ITransferenciaContaRepository transferenciaContaRepository, IUnitOfWork unitOfWork)
        {
            _saldoContaBancariaService = saldoContaBancariaService;
            _transferenciaContaRepository = transferenciaContaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PersistirAsync(TransferenciaConta transferenciaConta)
        {
            TransferenciaConta? transferenciaContaAtual = null;
            if (transferenciaConta.Id != 0)
                transferenciaContaAtual = await _transferenciaContaRepository.ObterTransferenciaContaNoTrackingAsync(transferenciaConta.Id);

            await _unitOfWork.BeginTransactionAsync();

            if (transferenciaConta.Id == 0)
                await _transferenciaContaRepository.IncluirAsync(transferenciaConta);
            else
                await _transferenciaContaRepository.AlterarAsync(transferenciaConta);

            await _unitOfWork.SaveChangesAsync();

            // seta o saldo das contas correntes
            await _saldoContaBancariaService.SetarSaldo(
                transferenciaContaAtual?.ContaBancariaOrigemId,
                transferenciaConta.ContaBancariaOrigemId,
                transferenciaContaAtual?.DataDebito,
                transferenciaConta.DataDebito);
            
            await _saldoContaBancariaService.SetarSaldo(
                transferenciaContaAtual?.ContaBancariaDestinoId,
                transferenciaConta.ContaBancariaDestinoId,
                transferenciaContaAtual?.DataCredito,
                transferenciaConta.DataCredito);

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task RemoverAsync(TransferenciaConta transferenciaConta)
        {
            // busca as informações, já que depois do Remove, o E.F. deixa os campos Nullables sem valor
            int? contaBancariaOrigemId = transferenciaConta.ContaBancariaOrigemId;
            int? contaBancariaDestinoId = transferenciaConta.ContaBancariaDestinoId;

            DateTime? dataDebitoAtual = transferenciaConta.DataDebito;
            DateTime? dataCreditoAtual = transferenciaConta.DataCredito;

            await _unitOfWork.BeginTransactionAsync();

            await _transferenciaContaRepository.RemoverAsync(transferenciaConta);

            await _saldoContaBancariaService.SetarSaldo(contaBancariaOrigemId, transferenciaConta.ContaBancariaOrigemId, dataDebitoAtual, transferenciaConta.DataDebito);
            await _saldoContaBancariaService.SetarSaldo(contaBancariaDestinoId, transferenciaConta.ContaBancariaDestinoId, dataCreditoAtual, transferenciaConta.DataCredito);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
    }
}