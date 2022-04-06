using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mangos.Dominio.Commands.DespesasRapidas
{
    public class IncluirDespesaRapidaCommandHandler : IRequestHandler<IncluirDespesaRapidaCommand>
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly IPessoaRepository _pessoaRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly LancamentoService _lancamentoService;
        private readonly PessoaCoordenadaService _pessoaCoordenadaService;
        private readonly SaldoContaBancariaService _saldoContaBancariaService;

        private readonly IUserResolverService _userResolverService;

        public IncluirDespesaRapidaCommandHandler(
            ILancamentoRepository lancamentoRepository,
            ILancamentoCartaoRepository lancamentoCartaoRepository,
            IPessoaRepository pessoaRepository,
            IUnitOfWork unitOfWork,
            LancamentoService lancamentoService,
            PessoaCoordenadaService pessoaCoordenadaService,
            SaldoContaBancariaService saldoContaBancariaService,
            IUserResolverService userResolverService)
        {
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _pessoaRepository = pessoaRepository;
            _unitOfWork = unitOfWork;
            _lancamentoService = lancamentoService;
            _pessoaCoordenadaService = pessoaCoordenadaService;
            _saldoContaBancariaService = saldoContaBancariaService;
            _userResolverService = userResolverService;
        }

        public async Task<Unit> Handle(IncluirDespesaRapidaCommand request, CancellationToken cancellationToken)
        {
            int? categoriaId = request.PessoaId == null ? null : (await _pessoaRepository.ObterPessoaAsync(request.PessoaId.Value))!.CategoriaPadraoDespesaId;

            await _unitOfWork.BeginTransactionAsync();

            if (request.CartaoCreditoId == null)
            {
                DateTime? dataContaBancaria = await _lancamentoService.CalcularDataContaBancariaAsync(request.ContaBancariaId, DateTime.Now.Date);

                var lancamento = new Lancamento()
                {
                    Id = 0,
                    GrupoId = _userResolverService.GrupoId,
                    DataHoraCadastro = DateTime.Now,
                    Tipo = TipoLancamento.Despesa,
                    Valor = request.Valor!.Value,
                    DataVencimento = DateTime.Now.Date,
                    Descricao = request.Descricao,
                    Agrupador = null,
                    NumeroParcela = 1,
                    TotalParcelas = 1,
                    PessoaId = request.PessoaId,
                    CategoriaId = categoriaId,
                    Observacoes = null,
                    Pago = true,
                    DataPagamento = DateTime.Now.Date,
                    ValorPago = request.Valor,
                    ContaBancariaId = request.ContaBancariaId,
                    DataContaBancaria = dataContaBancaria
                };

                await _lancamentoRepository.IncluirAsync(lancamento);
                await _unitOfWork.SaveChangesAsync();

                // seta o saldo da conta corrente
                await _saldoContaBancariaService.SetarSaldo(request.ContaBancariaId, dataContaBancaria);
            }
            else
            {
                var mesReferencia = await _lancamentoCartaoRepository.ObterMesReferenciaGerarLancamentoCartaoAsync(request.CartaoCreditoId.Value);

                var lancamentoCartao = new LancamentoCartao()
                {
                    Id = 0,
                    GrupoId = _userResolverService.GrupoId,
                    DataHoraCadastro = DateTime.Now,
                    CartaoCreditoId = request.CartaoCreditoId.Value,
                    TipoLancamento = TipoLancamentoCartao.Despesa,
                    Valor = request.Valor!.Value,
                    MesReferencia = mesReferencia,
                    Descricao = request.Descricao,
                    NumeroParcela = 1,
                    TotalParcelas = 1,
                    PessoaId = request.PessoaId,
                    CategoriaId = categoriaId,
                    GeradoLancamento = false
                };

                await _lancamentoCartaoRepository.IncluirAsync(lancamentoCartao);
                await _unitOfWork.SaveChangesAsync();
            }

            // atualiza as coordenadas da pessoa (se for o caso)
            if (request.AtualizarCoordenadas)
            {
                await _pessoaCoordenadaService.PersistirAsync(request.PessoaCoordenadaIdAtualizar,
                                                          request.PessoaId!.Value,
                                                          request.Latitude!.Value,
                                                          request.Longitude!.Value);
            }

            await _unitOfWork.CommitTransactionAsync();

            return default;
        }
    }
}