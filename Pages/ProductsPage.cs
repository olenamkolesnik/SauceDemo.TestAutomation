using Microsoft.Playwright;

namespace SauceDemo.TestAutomation.Pages
{
    public class ProductsPage(IPage page)
    {
        private readonly IPage _page = page;

        public ILocator InventoryList => _page.Locator(".inventory_list");
        public ILocator InventoryItemName => _page.Locator(".inventory_item_name");
        private ILocator SortDropdown => _page.Locator(".product_sort_container");
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
    }
}
