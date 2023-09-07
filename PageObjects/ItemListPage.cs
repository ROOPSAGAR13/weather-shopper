using OpenQA.Selenium;

namespace WeatherShopper.PageObjects
{
    public static class ItemListPage
    {
        public static List<Item> GetItems(IWebDriver driver)
        {
            var itemElements = driver.FindElements(By.ClassName("col-4"));
            var items = new List<Item>();
            foreach (var itemElement in itemElements)
            {
                var pElements = itemElement.FindElements(By.TagName("p"));

                items.Add(new Item()
                {
                    Name = pElements[0].Text,
                    Price = double.Parse(pElements[1].Text.Where(char.IsNumber).ToArray()),
                    AddToCartButton = itemElement.FindElement(By.TagName("button")),
                });
            }

            return items;
        }

        public static void AddLeastExpensiveItemWhichContains(List<Item> items, string containing)
        {
            var item = items
                .OrderBy(item => item.Price)
                .ToList()
                .Find(item => item.Name.Contains(containing, StringComparison.InvariantCultureIgnoreCase));

            if (item != null)
            {
                Console.WriteLine($"Adding {item.Name} to cart");
                item.AddToCartButton.Click();
            }
        }

        public static void OpenCartPage(IWebDriver driver)
        {
            driver.FindElement(By.Id("cart")).Click();
        }

        public class Item
        {
            public string Name { get; set; }

            public double Price { get; set; }

            public IWebElement AddToCartButton { get; set; }
        }
    }
}
