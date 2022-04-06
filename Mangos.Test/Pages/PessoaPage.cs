using Mangos.Test.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Mangos.Test.Pages
{
    public class PessoaPage : Page
    {
        public PessoaPage(IWebDriver driver) : base(driver) { }

        public PessoaPage AbreCadastro()
        {
            this.Driver.Navigate().GoToUrl($"{URL_MANGOS}/Pessoa");

            return this;
        }

        public PessoaPage IncluiNovoSomenteComTipo()
        {
            var btnIncluir = this.Driver.FindElement(By.Id("btn-incluir"));
            btnIncluir.Click();
            Driver.WaitForAjax();

            var campoTipo = new SelectElement(this.Driver.FindElement(By.Id("Tipo")));
            campoTipo.SelectByValue("70");

            var btnGravar = this.Driver.FindElement(By.Id("btn-gravar"));
            btnGravar.Click();
            Driver.WaitForAjax();

            return this;
        }

        public PessoaPage IncluiNovoComNome101Caracteres()
        {
            var btnIncluir = this.Driver.FindElement(By.Id("btn-incluir"));
            btnIncluir.Click();
            Driver.WaitForAjax();

            var campoNome = this.Driver.FindElement(By.Id("Nome"));
            var campoTipo = new SelectElement(this.Driver.FindElement(By.Id("Tipo")));

            string nome = "Nome da Silva".PadRight(101, '!');

            campoNome.SendKeys(nome);
            campoTipo.SelectByValue("70");

            var btnGravar = this.Driver.FindElement(By.Id("btn-gravar"));
            btnGravar.Click();
            Driver.WaitForAjax();

            return this;
        }
    }
}