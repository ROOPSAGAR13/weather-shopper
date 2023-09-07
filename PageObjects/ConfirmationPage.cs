using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WeatherShopper.PageObjects
{
    public static class ConfirmationPage
    {
        public static string GetConfirmationMessage(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            wait.Until(d => driver.Title == "Confirmation");

            return driver.FindElement(By.TagName("h2")).Text;
        }
    }
}
