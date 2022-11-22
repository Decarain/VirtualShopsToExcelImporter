using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace VirtualShopsToExcelImporter
{
    public class VirtualShopReader : IDisposable
    {

        IWebDriver driver;

        public VirtualShopReader(string path)
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(path);
        }

        public void Dispose()
        {
            driver.Dispose();
        }
    }
}