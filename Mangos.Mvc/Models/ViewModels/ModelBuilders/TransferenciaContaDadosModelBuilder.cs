using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class TransferenciaContaDadosModelBuilder : IModelBuilder<TransferenciaContaDadosModel, TransferenciaConta>
    {
        private readonly IUserResolverService _userResolverService;
        private readonly ITransferenciaContaRepository _transferenciaContaRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public TransferenciaContaDadosModelBuilder(IUserResolverService userResolverService, ITransferenciaContaRepository transferenciaContaRepository, IContaBancariaRepository contaBancariaRepository)
        {
            _userResolverService = userResolverService;
            _transferenciaContaRepository = transferenciaContaRepository;
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<TransferenciaConta?> CriarEntidadeAsync(TransferenciaContaDadosModel transferenciaContaDadosModel)
        {
            if (!transferenciaContaDadosModel.CheckValidationHash())
                return null;

            TransferenciaConta? transferenciaConta;

            if (transferenciaContaDadosModel.Id == 0)
                transferenciaConta = new TransferenciaConta() { Id = 0, GrupoId = transferenciaContaDadosModel.GrupoId, DataHoraCadastro = DateTime.Now };
            else
                transferenciaConta = await _transferenciaContaRepository.ObterTransferenciaContaAsync(transferenciaContaDadosModel.Id);

            if (transferenciaConta is null)
                return null;

            if (transferenciaConta.GrupoId != _userResolverService.GrupoId)
                return null;

            transferenciaConta.Valor = transferenciaContaDadosModel.Valor;
            transferenciaConta.DataDebito = transferenciaContaDadosModel.DataDebito;
            transferenciaConta.DataCredito = transferenciaContaDadosModel.DataCredito;
            transferenciaConta.Descricao = transferenciaContaDadosModel.Descricao!;
            transferenciaConta.ContaBancariaOrigemId = transferenciaContaDadosModel.ContaBancariaOrigemId;
            transferenciaConta.ContaBancariaDestinoId = transferenciaContaDadosModel.ContaBancariaDestinoId;
            
            return transferenciaConta;
        }

        public async Task<TransferenciaContaDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false);

                return TransferenciaContaDadosModel.Novo(
                    _userResolverService.GrupoId,
                    contasBancarias,
                    contasBancarias
                );
            }
            else
            {
                var transferenciaConta = await _transferenciaContaRepository.ObterTransferenciaContaAsync(id.Value);

                if (transferenciaConta == null)
                    return null;

                if (transferenciaConta.GrupoId != _userResolverService.GrupoId)
                    return null;

                return new TransferenciaContaDadosModel(
                    id: transferenciaConta.Id,
                    grupoId: transferenciaConta.GrupoId,
                    valor: transferenciaConta.Valor,
                    dataDebito: transferenciaConta.DataDebito,
                    dataCredito: transferenciaConta.DataCredito,
                    descricao: transferenciaConta.Descricao,
                    contaBancariaOrigemId: transferenciaConta.ContaBancariaOrigemId,
                    contaBancariaDestinoId: transferenciaConta.ContaBancariaDestinoId,
                    contasBancariasOrigem: await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, transferenciaConta.ContaBancariaOrigemId),
                    contasBancariasDestino: await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, transferenciaConta.ContaBancariaDestinoId)
                );
            }
        }

        public async Task AtualizarAsync(TransferenciaContaDadosModel model)
        {
            model.AtualizarContasBancarias(
                await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, model.ContaBancariaOrigemId),
                await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, model.ContaBancariaDestinoId)
            );
        }
    }
}