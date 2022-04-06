using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class RendimentoMensalContaDadosModelBinder : IModelBuilder<RendimentoMensalContaDadosModel, RendimentoMensalConta>
    {
        private readonly IUserResolverService _userResolverService;
        private readonly IRendimentoMensalContaRepository _rendimentoMensalContaRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public RendimentoMensalContaDadosModelBinder(IUserResolverService userResolverService, IRendimentoMensalContaRepository rendimentoMensalContaRepository, IContaBancariaRepository contaBancariaRepository)
        {
            _userResolverService = userResolverService;
            _rendimentoMensalContaRepository = rendimentoMensalContaRepository;
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<RendimentoMensalConta?> CriarEntidadeAsync(RendimentoMensalContaDadosModel model)
        {
            if (!model.CheckValidationHash())
                return null;

            RendimentoMensalConta? rendimentoMensalConta;

            if (model.Id == 0)
                rendimentoMensalConta = new RendimentoMensalConta() { Id = 0, DataHoraCadastro = DateTime.Now };
            else
                rendimentoMensalConta = await _rendimentoMensalContaRepository.ObterRendimentoMensalContaAsync(model.Id);

            if (rendimentoMensalConta is null)
                return null;

            if (rendimentoMensalConta.Id != 0 && rendimentoMensalConta.ContaBancaria!.GrupoId != _userResolverService.GrupoId)
                return null;

            rendimentoMensalConta.ContaBancariaId = model.ContaBancariaId!.Value;
            rendimentoMensalConta.MesReferencia = model.MesReferencia!.Value;
            rendimentoMensalConta.Valor = model.Valor!.Value;

            return rendimentoMensalConta;
        }

        public async Task<RendimentoMensalContaDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return RendimentoMensalContaDadosModel.Novo(
                    _userResolverService.GrupoId,
                    await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false));
            }
            else
            {
                var rendimentoMensalConta = await _rendimentoMensalContaRepository.ObterRendimentoMensalContaAsync(id.Value);

                if (rendimentoMensalConta == null)
                    return null;

                if (rendimentoMensalConta.ContaBancaria!.GrupoId != _userResolverService.GrupoId)
                    return null;

                return new RendimentoMensalContaDadosModel(
                    id: rendimentoMensalConta.Id,
                    grupoId: rendimentoMensalConta.ContaBancaria.GrupoId,
                    contaBancariaId: rendimentoMensalConta.ContaBancariaId,
                    mesReferencia: rendimentoMensalConta.MesReferencia,
                    valor: rendimentoMensalConta.Valor,
                    contasBancarias: await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, rendimentoMensalConta.ContaBancariaId)
                );
            }
        }

        public async Task AtualizarAsync(RendimentoMensalContaDadosModel model)
        {
            model.AtualizarContasBancarias(
                await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, model.ContaBancariaId)
            );
        }
    }
}