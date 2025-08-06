using Microsoft.Playwright;
using SauceDemo.TestAutomation.Models;

namespace SauceDemo.TestAutomation.Pages
{
    public class ProductsPage(IPage page)
    {
        private readonly IPage _page = page;

        public ILocator InventoryList => _page.Locator(".inventory_list");
        public ILocator InventoryItem => _page.Locator(".inventory_item");
        public ILocator InventoryItemName => _page.Locator(".inventory_item_name");
        private ILocator SortDropdown => _page.Locator(".product_sort_container");
        public ILocator InventoryItemPrice => _page.Locator(".inventory_item_price");
        public ILocator InventoryItemDesc => _page.Locator(".inventory_item_desc");
        public string CurrentUrl => _page.Url;

        public async Task SortProductsBy(string sortBy)
        {
            await SortDropdown.SelectOptionAsync([sortBy]);
        }

        public async Task<List<string>> GetAllProductNames()
        {
            var productElements = await InventoryItemName.AllAsync();
            var productNames = new List<string>();

            foreach (var element in productElements)
            {
                var name = await element.InnerTextAsync();
                productNames.Add(name.Trim());
            }
            return productNames;
        }

        public async Task<List<Product>> GetAllProductDetails()
        {
            var productElements = await InventoryItem.AllAsync();
            var products = new List<Product>();

            foreach (var element in productElements)
            {
                var name = await element.Locator(".inventory_item_name").InnerTextAsync();
                var price = await element.Locator(".inventory_item_price").InnerTextAsync();
                var description = await element.Locator(".inventory_item_desc").InnerTextAsync();

                products.Add(new Product
                {
                    Name = name.Trim(),
                    Price = price.Trim(),
                    Description = description.Trim()
                });
            }

            return products;
        }
    }
}
