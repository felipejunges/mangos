using System;
using OpenQA.Selenium;

namespace Mangos.Test.Pages
{
    public class LoginPage : Page
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        public LoginPage FazLogin()
        {
            Driver.Navigate().GoToUrl($"{URL_MANGOS}/Login/Login");

            var linkLogout = Driver.FindElements(By.Id("linkLogout"));
            if (linkLogout.Count == 0)
            {
                var campoEmail = Driver.FindElement(By.Id("Email"));
                var campoSenha = Driver.FindElement(By.Id("Senha"));
                var btnLogin = Driver.FindElement(By.ClassName("btn-primary"));

                campoEmail.SendKeys("felipejunges@yahoo.com.br");
                campoSenha.SendKeys("felipe123");

                btnLogin.Click();
            }

            return this;
        }
    }
}