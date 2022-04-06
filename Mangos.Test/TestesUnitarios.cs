using Mangos.Dominio.Enums;
using Mangos.Dominio.Models;
using Mangos.Dominio.Services;
using Mangos.Test.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mangos.Test
{
    public class TestesUnitarios : IClassFixture<ServiceFixture>, IDisposable
    {
        private const int GRUPO_ID = 1;

        private readonly IServiceScope _serviceScope;

        private readonly ContaBancariaService _contaBancariaService;
        private readonly LancamentoService _lancamentoService;
        private readonly LancamentoCartaoService _lancamentoCartaoService;
        private readonly PessoaCoordenadaService _pessoaCoordenadaService;
        private readonly SaldoContaBancariaService _saldoContaBancariaService;

        public TestesUnitarios(ServiceFixture fixture)
        {
            _serviceScope = fixture.ServiceProvider.CreateScope();

            _contaBancariaService = _serviceScope.ServiceProvider.GetRequiredService<ContaBancariaService>();
            _lancamentoService = _serviceScope.ServiceProvider.GetRequiredService<LancamentoService>();
            _lancamentoCartaoService = _serviceScope.ServiceProvider.GetRequiredService<LancamentoCartaoService>();
            _pessoaCoordenadaService = _serviceScope.ServiceProvider.GetRequiredService<PessoaCoordenadaService>();
            _saldoContaBancariaService = _serviceScope.ServiceProvider.GetRequiredService<SaldoContaBancariaService>();
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        // [Fact]
        // public async Task TestarObterLancamentosRapidosCoordenadas()
        // {
        //     var pessoaCoordenada1 = await _pessoaCoordenadaService.GetById(60); // Restaurante Bom Apetite
        //     //var pessoaCoordenada2 = await _pessoaCoordenadaService.GetById(7); // Posto Centenário

        //     var result1 = _pessoaCoordenadaService.GetFornecedorDespesaRapidaGeo(GRUPO_ID, pessoaCoordenada1.Latitude, pessoaCoordenada1.Longitude);
        //     //var result2 = _pessoaCoordenadaService.GetFornecedorDespesaRapidaGeo(GRUPO_ID, pessoaCoordenada2.Latitude, pessoaCoordenada2.Longitude);

        //     Assert.NotNull(result1?.UltimaDescricaoDespesa);
        //     Assert.NotNull(result1?.ContaBancariaId);
        //     Assert.Null(result1?.CartaoCreditoId);

        //     //Assert.NotNull(result2.UltimaDescricaoDespesa);
        //     //Assert.Null(result2.ContaBancariaId); // vai falhar, pois o posto centenário agora tem mais lançamentos em conta agora
        //     //Assert.NotNull(result2.CartaoCreditoId);
        // }

        // [Fact]
        // public async Task Grava100LancamentosEComparaSaldoContaBancaria()
        // {
        //     //
        //     var random = new Random();
        //     decimal contador = 0;

        //     var contaBancaria = await _contaBancariaService.AddOrUpdate(new ContaBancariaDadosModel() { Id = 0, GrupoId = GRUPO_ID, Descricao = "Teste", SaldoInicial = 0, RelatorioProjecaoSaldo = true, Ativo = true });

        //     var lancamentosGerados = new List<LancamentoPagoInclusaoModel>();

        //     //
        //     for (int i = 1; i <= 100; i++)
        //     {
        //         var dias = random.Next(0, 3650) * -1;
        //         var valor = random.Next(0, 1000000) / 100M;
        //         var tipoLancamento = random.Next(0, 1) == 0 ? TipoLancamento.Receita : TipoLancamento.Despesa;

        //         var lancamentoModel = new LancamentoPagoInclusaoModel()
        //         {
        //             CategoriaId = 0,
        //             GrupoId = GRUPO_ID,
        //             Descricao = "Teste inclusão",
        //             Valor = valor,
        //             DataPagamento = DateTime.Now.Date.AddDays(dias),
        //             ContaBancariaId = contaBancaria.Id
        //         };
        //         await _lancamentoService.AddPaga(lancamentoModel, tipoLancamento);

        //         lancamentosGerados.Add(lancamentoModel);

        //         contador += tipoLancamento == TipoLancamento.Receita ? valor : valor * -1;
        //     }

        //     //
        //     var data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        //     var saldoContaBancaria = _saldoContaBancariaService.GetSaldoContaBancaria(GRUPO_ID, contaBancaria.Id, data);

        //     //
        //     Assert.Equal(contador, saldoContaBancaria?.ValorSaldo);

        //     //
        //     // 
        //     var menorDataPossivel = DateTime.Now.Date.AddDays(-3650);
        //     var saldos = _saldoContaBancariaService.GetSaldosContasBancarias(GRUPO_ID, contaBancaria.Id, new DateTime(menorDataPossivel.Year, menorDataPossivel.Month, 1), DateTime.Now.Date);
        //     var somaAntesFechamento = saldos.Sum(o => o.ValorSaldo);

        //     Assert.True(saldos.Count > 0);

        //     foreach (var saldo in saldos)
        //     {
        //         saldo.ValorSaldo -= 0.01M; // somente para validar se ele vai recalcular
        //         saldo.Fechado = true;
        //     }

        //     await _saldoContaBancariaService.Gerar(GRUPO_ID, contaBancaria.Id, new DateTime(menorDataPossivel.Year, menorDataPossivel.Month, 1));

        //     var saldosDepois = _saldoContaBancariaService.GetSaldosContasBancarias(GRUPO_ID, contaBancaria.Id, new DateTime(menorDataPossivel.Year, menorDataPossivel.Month, 1), DateTime.Now.Date);
        //     var somaDepoisFechamento = saldosDepois.Sum(o => o.ValorSaldo);

        //     Assert.Equal(somaAntesFechamento, somaDepoisFechamento);
        // }

        // [Fact]
        // public void BuscarMinAndMaxMesesVencimentosCartaoCredito()
        // {
        //     var mesesVencimento1 = _lancamentoCartaoService.GetDatasMenorMaiorVencimentosNaoFechados(GRUPO_ID);
        //     var mesesVencimento2 = _lancamentoCartaoService.GetDatasMenorMaiorVencimentosNaoFechados(-1);

        //     DateTime? mesInicial1 = mesesVencimento1.DataMinima ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //     DateTime? mesFinal1 = mesesVencimento1.DataMaxima ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        //     DateTime? mesInicial2 = mesesVencimento2.DataMinima ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //     DateTime? mesFinal2 = mesesVencimento2.DataMaxima ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        //     Assert.NotNull(mesInicial1);
        //     Assert.NotNull(mesFinal1);
        //     Assert.NotNull(mesInicial2);
        //     Assert.NotNull(mesFinal2);
        // }
    }
}