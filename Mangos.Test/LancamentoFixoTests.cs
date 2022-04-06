using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Services;
using Mangos.Test.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace Mangos.Test
{
    public class LancamentoFixoTests : IClassFixture<ServiceFixture>, IDisposable
    {
        private const int GRUPO_ID = 1;

        private readonly IServiceScope _serviceScope;

        private readonly LancamentoFixoService _lancamentoFixoService;

        public LancamentoFixoTests(ServiceFixture fixture)
        {
            _serviceScope = fixture.ServiceProvider.CreateScope();

            _lancamentoFixoService = _serviceScope.ServiceProvider.GetRequiredService<LancamentoFixoService>();
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        [Theory]
        [InlineData(PeriodicidadeLancamentoFixo.Mensal, TipoLancamentoFixo.Despesa, 0, 1, 7, null, false)]
        [InlineData(PeriodicidadeLancamentoFixo.Mensal, TipoLancamentoFixo.Despesa, 0, 1, 7, null, true)]
        [InlineData(PeriodicidadeLancamentoFixo.Mensal, TipoLancamentoFixo.Despesa, 0, 1, 3, null, false)]
        [InlineData(PeriodicidadeLancamentoFixo.Mensal, TipoLancamentoFixo.Despesa, 0, 1, 3, null, true)]
        [InlineData(PeriodicidadeLancamentoFixo.Mensal, TipoLancamentoFixo.Despesa, 0, 6, 7, null, true)]
        [InlineData(PeriodicidadeLancamentoFixo.Mensal, TipoLancamentoFixo.Despesa, 0, 6, 7, null, false)]
        [InlineData(PeriodicidadeLancamentoFixo.Mensal, TipoLancamentoFixo.Despesa, 1, 6, 7, null, false)]
        [InlineData(PeriodicidadeLancamentoFixo.Mensal, TipoLancamentoFixo.Despesa, 1, 6, 27, null, false)]
        [InlineData(PeriodicidadeLancamentoFixo.Mensal, TipoLancamentoFixo.Despesa, 0, 6, 31, null, false)]
        [InlineData(PeriodicidadeLancamentoFixo.Anual, TipoLancamentoFixo.Despesa, 1, 6, 7, 12, false)]
        [InlineData(PeriodicidadeLancamentoFixo.Anual, TipoLancamentoFixo.Despesa, 0, 12, 7, 12, false)]
        [InlineData(PeriodicidadeLancamentoFixo.Anual, TipoLancamentoFixo.Despesa, 0, 12, 7, 5, false)]
        [InlineData(PeriodicidadeLancamentoFixo.Anual, TipoLancamentoFixo.Despesa, 1, 24, 7, 12, false)]
        public void TestarGeracaoLancamentosFixos(PeriodicidadeLancamentoFixo periodicidadeLancamentoFixo, TipoLancamentoFixo tipoLancamentoFixo, int mesesAnteriorUltimoGerado, int mesesAntecedenciaGerar, int diaVencimento, int? mesVencimento, bool gerarSomenteNoVencimento)
        {
            var dataUltimoMesGerado = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(mesesAnteriorUltimoGerado * -1);

            var datas = GerarDatasLancamentoFixo(periodicidadeLancamentoFixo, tipoLancamentoFixo, diaVencimento, mesVencimento, dataUltimoMesGerado, mesesAntecedenciaGerar, gerarSomenteNoVencimento);

            int decrementoDiaNaoChegou = gerarSomenteNoVencimento && DateTime.Now.Day < diaVencimento ? 1 : 0;

            if (periodicidadeLancamentoFixo == PeriodicidadeLancamentoFixo.Mensal)
            {
                Assert.True(datas.Count == mesesAntecedenciaGerar + mesesAnteriorUltimoGerado - decrementoDiaNaoChegou);
            }
            else if (periodicidadeLancamentoFixo == PeriodicidadeLancamentoFixo.Anual)
            {
                int mesesArredondado = (int)Math.Floor((mesesAntecedenciaGerar + mesesAnteriorUltimoGerado + decrementoDiaNaoChegou) / 12M);

                Assert.True(datas.Count == mesesArredondado);
            }
        }

        private List<DateTime> GerarDatasLancamentoFixo(PeriodicidadeLancamentoFixo periodicidadeLancamento, TipoLancamentoFixo tipoLancamentoFixo, int diaVencimento, int? mesVencimento, DateTime? dataUltimoMesGerado, int mesesAntecedenciaGerar, bool gerarSomenteNoVencimento)
        {
            var lancamentoFixo = new LancamentoFixo()
            {
                Id = 0,
                GrupoId = GRUPO_ID,
                Tipo = tipoLancamentoFixo,
                Periodicidade = periodicidadeLancamento,
                Valor = 100,
                DiaVencimento = diaVencimento,
                MesVencimento = mesVencimento,
                Descricao = "Teste",
                Ativo = true,
                DataUltimoMesGerado = dataUltimoMesGerado
            };

            return _lancamentoFixoService.GetDatasGerar(lancamentoFixo, mesesAntecedenciaGerar, gerarSomenteNoVencimento);
        }
    }
}