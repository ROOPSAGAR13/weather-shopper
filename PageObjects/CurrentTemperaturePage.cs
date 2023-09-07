using OpenQA.Selenium;

namespace WeatherShopper.PageObjects
{
    public static class CurrentTemperaturePage
    {
        public static int GetCurrentTemperature(IWebDriver driver)
        {
            return int.Parse(driver.FindElement(By.Id("temperature")).Text.Split(" ")[0]);
        }

        public static void ClickBuyMoisturizers(IWebDriver driver)
        {
            driver.FindElement(By.LinkText("Buy moisturizers")).Click();
        }

        public static void ClickBuySunscreens(IWebDriver driver)
        {
            driver.FindElement(By.LinkText("Buy sunscreens")).Click();
        }
    }
}
