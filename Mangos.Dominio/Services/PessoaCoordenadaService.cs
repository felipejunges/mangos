using Mangos.Dominio.Entities;
using Mangos.Dominio.Models;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Settings;
using Mangos.Dominio.Utils;
using System.Linq;
using System.Threading.Tasks;
using Mangos.Dominio.Interfaces;
using System;

namespace Mangos.Dominio.Services
{
    public class PessoaCoordenadaService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly IPessoaCoordenadaRepository _pessoaCoordenadaRepository;
        private readonly GeoLocationSettings _geoLocationSettings;
        private readonly IUnitOfWork _unitOfWork;

        public PessoaCoordenadaService(ILancamentoRepository lancamentoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository, IPessoaCoordenadaRepository pessoaCoordenadaRepository, GeoLocationSettings geoLocationSettings, IUnitOfWork unitOfWork)
        {
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _pessoaCoordenadaRepository = pessoaCoordenadaRepository;
            _geoLocationSettings = geoLocationSettings;
            _unitOfWork = unitOfWork;
        }

        public async Task<FornecedorDespesaRapidaGeoModel?> ObterFornecedorDespesaRapidaGeoAsync(int grupoId, double latitude, double longitude)
        {
            var dataMinimaLancamentos = DateTime.Now.AddYears(-5);

            int distanciaMaximaFornecedorDespesaRapida = _geoLocationSettings.DistanciaMaximaFornecedorDespesaRapida;
            int quantidadeRegistrosBuscar = _geoLocationSettings.QuantidadeRegistrosBuscarGeo;
            int quantidadeMinimaResultados = 2;

            var pessoa = await _pessoaCoordenadaRepository.ListarPessoasCoordenadasFornecedoresNoTrackingAsync(grupoId);

            var pessoaModel = pessoa
                    .Select(p => new FornecedorDespesaRapidaGeoModel()
                    {
                        Id = p.Id,
                        PessoaId = p.PessoaId,
                        PessoaNome = p.Pessoa!.Nome,
                        Latitude = p.Latitude,
                        Longitude = p.Longitude,
                        Distancia = GeoLocation.GetDistance(latitude, longitude, p.Latitude, p.Longitude)
                    })
                    .Where(o => o.Distancia <= distanciaMaximaFornecedorDespesaRapida)
                    .OrderBy(o => o.Distancia)
                    .FirstOrDefault();

            if (pessoaModel != null)
            {
                var lancamentos = await _lancamentoRepository.ListarDespesasPagasDaPessoaNoTrackingAsync(grupoId, pessoaModel.PessoaId, dataMinimaLancamentos);
                var lancamentosCartao = await _lancamentoCartaoRepository.ListarDespesasPagasDaPessoaNoTrackingAsync(grupoId, pessoaModel.PessoaId, dataMinimaLancamentos);

                var lancamentosAgrupados = lancamentos
                    .Select(o => new
                    {
                        Data = o.DataPagamento!.Value,
                        o.Descricao,
                        o.ContaBancariaId,
                        CartaoCreditoId = (int?)null
                    })
                    .Union(lancamentosCartao
                        .Select(o => new
                        {
                            Data = o.MesReferencia,
                            o.Descricao,
                            ContaBancariaId = (int?)null,
                            CartaoCreditoId = (int?)o.CartaoCreditoId
                        })
                    )
                    .OrderByDescending(o => o.Data)
                    .Take(quantidadeRegistrosBuscar)
                    .GroupBy(o => new
                    {
                        o.Descricao,
                        o.ContaBancariaId,
                        o.CartaoCreditoId
                    })
                    .Select(o => new
                    {
                        o.Key.Descricao,
                        o.Key.ContaBancariaId,
                        o.Key.CartaoCreditoId,
                        Quantidade = o.Count()
                    })
                    .OrderByDescending(o => o.Quantidade)
                    .Where(o => o.Quantidade >= quantidadeMinimaResultados)
                    .ToList();

                if (lancamentosAgrupados.Count > 0)
                {
                    var primeiroLancamento = lancamentosAgrupados.First();
                    pessoaModel.UltimaDescricaoDespesa = primeiroLancamento.Descricao;
                    pessoaModel.ContaBancariaId = primeiroLancamento.ContaBancariaId;
                    pessoaModel.CartaoCreditoId = primeiroLancamento.CartaoCreditoId;
                }
            }

            return pessoaModel;
        }

        public async Task PersistirAsync(int? pessoaCoordenadaId, int pessoaId, double latitude, double longitude)
        {
            PessoaCoordenada? pessoaCoordenada = pessoaCoordenadaId == null
                                                ? null
                                                : await _pessoaCoordenadaRepository.ObterPessoaCoordenadaAsync(pessoaCoordenadaId.Value);

            if (pessoaCoordenada == null)
            {
                pessoaCoordenada = new PessoaCoordenada()
                {
                    Id = 0,
                    PessoaId = pessoaId
                };
            }

            pessoaCoordenada.Latitude = latitude;
            pessoaCoordenada.Longitude = longitude;

            if (pessoaCoordenada.Id == 0)
                await _pessoaCoordenadaRepository.IncluirAsync(pessoaCoordenada);
            else
                await _pessoaCoordenadaRepository.AlterarAsync(pessoaCoordenada);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task PersistirAsync(PessoaCoordenada pessoaCoordenada)
        {
            if (pessoaCoordenada.Id == 0)
                await _pessoaCoordenadaRepository.IncluirAsync(pessoaCoordenada);
            else
                await _pessoaCoordenadaRepository.AlterarAsync(pessoaCoordenada);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoverAsync(PessoaCoordenada pessoaCoordenada)
        {
            await _pessoaCoordenadaRepository.RemoverAsync(pessoaCoordenada);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}