using System;
using OpenQA.Selenium;

public class Page
{
    protected readonly string URL_MANGOS = "http://localhost:5000";
    protected readonly IWebDriver Driver;

    public Page(IWebDriver driver)
    {
        this.Driver = driver;
    }

    public bool ModalEstaAberta()
    {
        var modal = this.Driver.FindElement(By.Id("modal-principal"));
        
        return modal != null ? modal.Displayed : false;
    }

    public void FechaModal()
    {
        var btnFechar = this.Driver.FindElement(By.LinkText("Fechar"));
        btnFechar.Click();
    }
}