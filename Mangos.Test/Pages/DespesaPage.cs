using Mangos.Test.Utils;
using OpenQA.Selenium;

namespace Mangos.Test.Pages
{
    public class DespesaPage : Page
    {
        public DespesaPage(IWebDriver driver) : base(driver) { }

        public DespesaPage AbreCadastro()
        {
            this.Driver.Navigate().GoToUrl($"{URL_MANGOS}/Despesa");

            return this;
        }

        public DespesaPage EditaPrimeiraDespesa(int mes, int ano)
        {
            var txtMes = Driver.FindElement(By.Name("mes"));
            txtMes.Clear();
            txtMes.SendKeys($"{mes.ToString().PadLeft(2, '0')}/{ano.ToString()}");
            txtMes.Submit();
            Driver.WaitForAjax();

            var btnsEditar = Driver.FindElements(By.ClassName("glyphicon-edit"));
            if (btnsEditar.Count > 0)
            {
                var btnEditar = btnsEditar[0];

                btnEditar.Click();
                Driver.WaitForAjax();

                var btnGravar = Driver.FindElement(By.ClassName("btn-primary"));
                btnGravar.Click();
                Driver.WaitForAjax();
            }

            return this;
        }
    }
}