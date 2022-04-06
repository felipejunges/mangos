using Mangos.Dominio.Services;
using Mangos.Test.Pages;
using Mangos.Test.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Mangos.Test
{
    public class TestesAutomatizados : IClassFixture<ServiceFixture>, IDisposable
    {
        private const int GRUPO_ID = 1;

        private readonly IServiceScope _serviceScope;

        private readonly SaldoContaBancariaService _saldoContaBancariaService;

        private readonly IWebDriver Driver;
        private readonly LoginPage LoginPage;
        private readonly DespesaPage DespesaPage;
        private readonly PessoaPage PessoaPage;

        public TestesAutomatizados(ServiceFixture fixture)
        {
            this.Driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            this.Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            this.Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);

            this.LoginPage = new LoginPage(this.Driver);
            this.DespesaPage = new DespesaPage(this.Driver);
            this.PessoaPage = new PessoaPage(this.Driver);

            _serviceScope = fixture.ServiceProvider.CreateScope();

            _saldoContaBancariaService = _serviceScope.ServiceProvider.GetRequiredService<SaldoContaBancariaService>();
        }

        public void Dispose()
        {
            this.Driver.Close();
            this.Driver.Dispose();
        }

        // [Fact]
        // public void AlteraDespesaDe2009EVerificaSaldo()
        // {
        //     var saldosAntes = _saldoContaBancariaService.GetSaldosContasBancariasTest(GRUPO_ID);

        //     this.LoginPage.FazLogin();
        //     this.DespesaPage.AbreCadastro().EditaPrimeiraDespesa(9, 2009);

        //     var saldosDepois = _saldoContaBancariaService.GetSaldosContasBancariasTest(GRUPO_ID);

        //     Assert.Equal(saldosAntes, saldosDepois);
        // }

        [Fact]
        public void TentaIncluirPessoaSemTodosOsDados()
        {
            this.LoginPage.FazLogin();
            this.PessoaPage.AbreCadastro().IncluiNovoSomenteComTipo();

            Assert.True(this.PessoaPage.ModalEstaAberta());
        }

        [Fact]
        public void TentaIncluirNomeCom101Caracteres()
        {
            // dotnet test --filter "FullyQualifiedName=Mangos.Test.TestesAutomatizados.TentaIncluirNomeCom101Caracteres"

            this.LoginPage.FazLogin();
            this.PessoaPage.AbreCadastro().IncluiNovoComNome101Caracteres();

            Assert.True(this.PessoaPage.ModalEstaAberta());
        }
    }
}