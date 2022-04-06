using OpenQA.Selenium;
using System;
using System.Threading;

namespace Mangos.Test.Utils
{
    public static class WebDriverExtensions
    {
        public static void WaitForAjax(this IWebDriver driver, int timeoutSecs = 10, bool throwException = false)
        {
            for (var i = 0; i < timeoutSecs * 2; i++)
            {
                var executor = driver as IJavaScriptExecutor;
                var ajaxIsComplete = (bool)executor!.ExecuteScript("return jQuery.active == 0");
                if (ajaxIsComplete)
                    return;

                Thread.Sleep(500);
            }

            if (throwException)
            {
                throw new Exception("WebDriver timed out waiting for AJAX call to complete");
            }
        }
    }
}