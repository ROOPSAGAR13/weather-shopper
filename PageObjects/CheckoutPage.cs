using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WeatherShopper.PageObjects
{
    public static class CheckoutPage
    {

        public static List<PurchaseItem> GetPurchaseItems(IWebDriver driver)
        {
            var items = new List<PurchaseItem>();

            var tbodyElement = driver.FindElement(By.TagName("tbody"));

            var trElements = tbodyElement.FindElements(By.TagName("tr"));

            foreach (var trElement in trElements)
            {
                var tdElements = trElement.FindElements(By.TagName("td"));

                items.Add(new PurchaseItem()
                {
                    Name = tdElements[0].Text,
                    Price = double.Parse(tdElements[1].Text)
                });
            }

            return items;
        }


        public static double GetTotalPrice(IWebDriver driver)
        {
            return double.Parse(driver.FindElement(By.Id("total")).Text.Where(char.IsNumber).ToArray());
        }

        public static void ClickPayByCard(IWebDriver driver)
        {
            driver.FindElement(By.ClassName("stripe-button-el")).Click();
        }

        public static void AddDummyCardDetails(IWebDriver driver)
        {
            driver = driver.SwitchTo().Frame("stripe_checkout_app");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            wait.Until(d => driver.FindElement(By.Id("email")).Displayed);

            var emailElement = driver.FindElement(By.Id("email"));

            emailElement.SendKeys("example@email.com");

            var cardNumberElement = driver.FindElement(By.Id("card_number"));
            cardNumberElement.Clear();
            cardNumberElement.SendKeys("4242");
            cardNumberElement.SendKeys("4242");
            cardNumberElement.SendKeys("4242");
            cardNumberElement.SendKeys("4242");

            driver.FindElement(By.Id("cc-exp")).SendKeys("12");
            driver.FindElement(By.Id("cc-exp")).SendKeys("34");

            driver.FindElement(By.Id("cc-csc")).SendKeys("567");
            driver.FindElement(By.Id("billing-zip")).SendKeys("12345");
            driver.FindElement(By.Id("submitButton")).Click();
        }

        public class PurchaseItem
        {
            public string Name { get; set; }
            public double Price { get; set; }
        }
    }
}
