using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using System.Threading.Tasks;
using Mangos.Dominio.Interfaces;
using System;

namespace Mangos.Dominio.Services
{
    public class ContaBancariaService
    {
        private readonly IRendimentoMensalContaRepository _rendimentoMensalContaRepository;
        private readonly ISaldoContaBancariaRepository _saldoContaBancariaRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ContaBancariaService(IRendimentoMensalContaRepository rendimentoMensalContaRepository, ISaldoContaBancariaRepository saldoContaBancariaRepository, IContaBancariaRepository contaBancariaRepository, IUnitOfWork unitOfWork)
        {
            _rendimentoMensalContaRepository = rendimentoMensalContaRepository;
            _saldoContaBancariaRepository = saldoContaBancariaRepository;
            _contaBancariaRepository = contaBancariaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> VerificarGrupoId(int contaBancariaId, int grupoId)
        {
            var contaBancaria = await _contaBancariaRepository.ObterContaBancariaAsync(contaBancariaId);

            if (contaBancaria == null)
                return false;

            return contaBancaria.GrupoId == grupoId;
        }

        public async Task PersistirAsync(ContaBancaria contaBancaria)
        {
            if (contaBancaria.Id == 0)
                await _contaBancariaRepository.IncluirAsync(contaBancaria);
            else
                await _contaBancariaRepository.AlterarAsync(contaBancaria);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoverAsync(ContaBancaria contaBancaria)
        {
            await _unitOfWork.BeginTransactionAsync();

            await _rendimentoMensalContaRepository.RemoverDaContaBancariaAsync(contaBancaria);
            await _saldoContaBancariaRepository.RemoverDaContaBancariaAsync(contaBancaria);

            await _contaBancariaRepository.RemoverAsync(contaBancaria);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
    }
}