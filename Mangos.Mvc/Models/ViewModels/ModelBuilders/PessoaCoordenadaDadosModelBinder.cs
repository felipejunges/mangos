using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class PessoaCoordenadaDadosModelBinder : IModelBuilder<PessoaCoordenadaDadosModel, PessoaCoordenada>
    {
        private readonly IPessoaCoordenadaRepository _pessoaCoordenadaRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IUserResolverService _userResolverService;
        private readonly IHttpContextAccessor _contextAccessor;

        public PessoaCoordenadaDadosModelBinder(IPessoaCoordenadaRepository pessoaCoordenadaRepository, IPessoaRepository pessoaRepository, IUserResolverService userResolverService, IHttpContextAccessor contextAccessor)
        {
            _pessoaCoordenadaRepository = pessoaCoordenadaRepository;
            _pessoaRepository = pessoaRepository;
            _userResolverService = userResolverService;
            _contextAccessor = contextAccessor;
        }

        public async Task<PessoaCoordenada?> CriarEntidadeAsync(PessoaCoordenadaDadosModel pessoaCoordenadaDadosModel)
        {
            if (!pessoaCoordenadaDadosModel.CheckValidationHash())
                return null;

            PessoaCoordenada? pessoaCoordenada;

            if (pessoaCoordenadaDadosModel.Id == 0)
            {
                var pessoa = await _pessoaRepository.ObterPessoaAsync(pessoaCoordenadaDadosModel.PessoaId);

                if (pessoa is null)
                    return null;

                pessoaCoordenada = new PessoaCoordenada()
                {
                    Id = 0,
                    PessoaId = pessoaCoordenadaDadosModel.PessoaId,
                    Pessoa = pessoa
                };
            }
            else
            {
                pessoaCoordenada = await _pessoaCoordenadaRepository.ObterPessoaCoordenadaAsync(pessoaCoordenadaDadosModel.Id);
            }

            if (pessoaCoordenada is null)
                return null;

            if (pessoaCoordenada.Pessoa!.GrupoId != _userResolverService.GrupoId || pessoaCoordenada.PessoaId != pessoaCoordenadaDadosModel.PessoaId)
                return null;
            
            pessoaCoordenada.Latitude = pessoaCoordenadaDadosModel.Latitude!.Value;
            pessoaCoordenada.Longitude = pessoaCoordenadaDadosModel.Longitude!.Value;
            pessoaCoordenada.Observacao = pessoaCoordenadaDadosModel.Observacao;

            return pessoaCoordenada;
        }

        public async Task<PessoaCoordenadaDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            var pessoaParam = _contextAccessor?.HttpContext?.Request.Query["Pessoa"];
            if (string.IsNullOrEmpty(pessoaParam))
                throw new Exception("Parâmetro pessoa inválido");

            int pessoaId = Int32.Parse(pessoaParam);

            if (id == null)
            {
                return PessoaCoordenadaDadosModel.Novo(_userResolverService.GrupoId, pessoaId);
            }
            else
            {
                var pessoaCoordenada = await _pessoaCoordenadaRepository.ObterPessoaCoordenadaAsync(id.Value);

                if (pessoaCoordenada == null)
                    return null;

                if (pessoaCoordenada.Pessoa!.GrupoId != _userResolverService.GrupoId || pessoaCoordenada.PessoaId != pessoaId)
                    return null;

                return new PessoaCoordenadaDadosModel(
                    id: pessoaCoordenada.Id,
                    grupoId: pessoaCoordenada.Pessoa.GrupoId,
                    pessoaId: pessoaCoordenada.PessoaId,
                    latitude: pessoaCoordenada.Latitude,
                    longitude: pessoaCoordenada.Longitude,
                    observacao: pessoaCoordenada.Observacao
                );
            }
        }

        public Task AtualizarAsync(PessoaCoordenadaDadosModel model)
        {
            return Task.CompletedTask;
        }
    }
}