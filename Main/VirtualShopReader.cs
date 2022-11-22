using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Main.Utilities;
using OpenQA.Selenium.Support.UI;

namespace Main
{
    public class ShopReader : IDisposable
    {

        IWebDriver driver;

        public ShopReader(string path)
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Navigate().GoToUrl(path);
        }

        public void GetTitleModels(string title)
        {
            var searchInput = driver.FindElement(By.XPath($"//*[contains(@class,'nav-element__search')]"));
            searchInput.Click();

            //TODO Add waiter.
            searchInput = driver.FindElement(By.Id("mobileSearchInput"));
            searchInput.SendKeys(title);
            searchInput.SendKeys(Keys.Enter);
            driver.Navigate().Refresh();

            var nonPromotionalProductsLinks = driver.FindElements(
                By.XPath($"//div[@class ='catalog-page__content']/div/div/div[not(contains(@class ,'j-advert-card-item'))]/div/a"))
                .Select(pr => pr.GetAttribute("href")).ToList();


            var models = new List<ShopModel>();
            foreach (var link in nonPromotionalProductsLinks)
            {
                models.Add(GetModel(link));
            }
        }

        private ShopModel GetModel(string path)
        {
            driver.Navigate().GoToUrl(path);
            var title = driver.FindElement(By.ClassName("product-page__header")).Text;

            var brand = driver.FindElement(By.XPath($"//*[@class='product-page__grid']//*[@class='seller-info']/div/div/div/a")).Text;

            var id = driver.FindElement(By.Id("productNmId")).Text;

            var feedbacks = driver.FindElement(By.ClassName("product-review__count-review")).Text;

            var priceBlock = driver.FindElements(By.XPath($"//*[@class='price-block__content']"))[0];
            string price;
            try
            {
                price = priceBlock.FindElement(By.XPath($".//*[contains(@class, 'price-block__old-price')]")).Text;
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                price = priceBlock.FindElement(By.ClassName($"price-block__final-price")).Text;
            }
            var model = new ShopModel() { title = title, brand = brand, id = id, feedbacks = feedbacks, price = price };

            FixFormat(model);
            return model;
        }

        public void FixFormat(ShopModel model)
        {
            model.title = new string(model.title.Where(ch => !char.IsControl(ch)).ToArray());
            model.feedbacks = model.feedbacks.Split(" ")[0];
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}