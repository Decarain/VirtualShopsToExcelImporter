using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Main.Utilities;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Net.Http.Headers;

namespace Main
{
    public class ShopReader : IDisposable
    {

        IWebDriver driver;

        public ShopReader(string path)
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl(path);
            driver.Manage().Window.Maximize();
            Thread.Sleep(TimeSpan.FromSeconds(5)); //Unfortunately, I haven't find selenium working waiter alternative.
        }

        public List<ShopModel> GetTitleModels(string title)
        {
            var searchInput = driver.FindElement(By.XPath($"//*[@class='search-catalog__block']/input"));
            searchInput.SendKeys(title);
            searchInput.SendKeys(Keys.Enter);
            driver.Navigate().Refresh();

            var nonPromotionalProductsLinks = driver.FindElements(
                By.XPath($"//div[@class ='catalog-page__content']/div/div/div[not(contains(@class ,'j-advert-card-item'))]/div/a"))
                .Select(pr => pr.GetAttribute("href")).ToList();


            var models = new List<ShopModel>();
            foreach (var link in nonPromotionalProductsLinks)
            {
                var model = GetModel(link);
                if (model != null)
                {
                    models.Add(model);
                }
            }

            return models;
        }

        private ShopModel GetModel(string path)
        {
            try
            {
                driver.Navigate().GoToUrl(path);
                var title = driver.FindElement(By.ClassName("product-page__header")).Text;

                var brand = driver.FindElement(By.XPath($"//*[@class='product-page__grid']//*[@class='seller-info']/div/div/div")).Text;

                var id = driver.FindElement(By.Id("productNmId")).Text;

                var feedbacks = driver.FindElement(By.ClassName("product-review__count-review")).Text;

                var priceBlock = driver.FindElements(By.XPath($"//*[@class='price-block__content']"));
                string price;
                try
                {
                    price = priceBlock
                        .Select(pr => pr.FindElement(By.XPath($".//*[contains(@class, 'price-block__old-price')]")).Text)
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .FirstOrDefault();
                }
                catch (OpenQA.Selenium.NoSuchElementException)
                {
                    price = priceBlock
                        .Select(pr => pr.FindElement(By.ClassName($"price-block__final-price")).Text)
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .FirstOrDefault();
                }
                var model = new ShopModel() { title = title, brand = brand, id = id, feedbacks = feedbacks, price = price };

                FixFormat(model);
                return model;
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                return null;
            }
        }

        public void FixFormat(ShopModel model)
        {
            model.title = new string(model.title.Where(ch => !char.IsControl(ch)).ToArray());
            model.price = new string(model.price.Where(ch => char.IsDigit(ch)).ToArray());
            model.feedbacks = new string(model.feedbacks.Where(ch => char.IsDigit(ch)).ToArray());
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}