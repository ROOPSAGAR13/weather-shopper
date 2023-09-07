using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using WeatherShopper.PageObjects;

namespace WeatherShopper
{
    [TestClass]
    public class WeatherShopperTest
    {
        readonly String URL = "https://weathershopper.pythonanywhere.com/";

        [TestMethod]
        public void TestMethod1()
        {
            var chromeOptions = new ChromeOptions();
            IWebDriver driver = new RemoteWebDriver(new Uri("http://host.docker.internal:4444/wd/hub"), chromeOptions.ToCapabilities());
            // IWebDriver driver = new ChromeDriver();

            try
            {
                Console.WriteLine($"Navigating to {URL}");
                driver.Navigate().GoToUrl(URL);
                driver.Manage().Window.Maximize();

                int currentTemperature = CurrentTemperaturePage.GetCurrentTemperature(driver);
                Console.WriteLine($"Temperature: {currentTemperature}");

                bool isMoisturizer = true;
                if (currentTemperature < 19)
                {
                    Console.WriteLine("Buying Moisturizers");
                    CurrentTemperaturePage.ClickBuyMoisturizers(driver);
                    isMoisturizer = true;
                }
                else if (currentTemperature > 34)
                {
                    Console.WriteLine("Buying Sunscreens");
                    CurrentTemperaturePage.ClickBuySunscreens(driver);
                    isMoisturizer = false;
                }
                else
                {
                    // Do not buy anything
                    Console.WriteLine("Temperature is normal, no need to buy anything");
                    return;
                }

                var items = ItemListPage.GetItems(driver);
                if (isMoisturizer)
                {
                    // Least expensive mositurizer that contains Aloe
                    ItemListPage.AddLeastExpensiveItemWhichContains(items, "Aloe");

                    // Least expensive moisturizer that contains almond
                    ItemListPage.AddLeastExpensiveItemWhichContains(items, "almond");
                }
                else
                {
                    // Least expensive sunscreen that contains SPF-50
                    ItemListPage.AddLeastExpensiveItemWhichContains(items, "SPF-50");

                    // Least expensive sunscreen that contains SPF-30
                    ItemListPage.AddLeastExpensiveItemWhichContains(items, "SPF-30");
                }

                ItemListPage.OpenCartPage(driver);

                var totalPrice = CheckoutPage.GetTotalPrice(driver);
                var purchaseItems = CheckoutPage.GetPurchaseItems(driver);

                Console.WriteLine($"Total amount is {totalPrice}");

                Assert.IsTrue(purchaseItems.Count <= 2);
                Assert.IsTrue(purchaseItems.Aggregate(0.0, (acc, x) => acc + x.Price) == totalPrice);

                Console.WriteLine("Paying by card");
                CheckoutPage.ClickPayByCard(driver);

                Console.WriteLine("Adding card details");
                CheckoutPage.AddDummyCardDetails(driver);

                var confirmationMessage = ConfirmationPage.GetConfirmationMessage(driver);

                Console.WriteLine(confirmationMessage);

                Assert.AreEqual("PAYMENT SUCCESS", confirmationMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                driver.Close();
            }
        }
    }

}