using Microsoft.Playwright;
using NUnit.Framework;
using System.Globalization;

namespace EcommerceTests.PageObjects
{
    public class CheckoutPage
    {
        private readonly IPage _page;

        private ILocator PromoCodeInput => _page.Locator("#promo-code");
        private ILocator ApplyPromoButton => _page.Locator("#apply-promo");
        private ILocator OriginalPrice => _page.Locator(".original-price");
        private ILocator DiscountAmount => _page.Locator(".discount-amount");
        private ILocator FinalPrice => _page.Locator(".final-price");
        private ILocator PlaceOrderButton => _page.Locator("#place-order");
        private ILocator OrderNumber => _page.Locator(".order-number");
        private ILocator SuccessMessage => _page.Locator(".success-message");
        private ILocator ErrorMessage => _page.Locator(".error-message");

        public CheckoutPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateAsync()
        {
            await _page.GotoAsync("http://localhost:8080");
        }

        public async Task ApplyPromoCodeAsync(string code)
        {
            await PromoCodeInput.FillAsync(code);
            await ApplyPromoButton.ClickAsync();
        }

        public async Task<decimal> GetOriginalPriceAsync()
        {
            var text = await OriginalPrice.TextContentAsync();

            return decimal.Parse(
                text.Replace("$", "")
                    .Replace(",", "")
                    .Trim(),
                CultureInfo.InvariantCulture);
        }

        public async Task<decimal> GetDiscountAmountAsync()
        {
            var text = await DiscountAmount.TextContentAsync();

            return decimal.Parse(
                text.Replace("$", "")
                    .Replace(",", "")
                    .Trim(),
                CultureInfo.InvariantCulture);
        }

        public async Task<decimal> GetFinalPriceAsync()
        {
            var text = await FinalPrice.TextContentAsync();

            return decimal.Parse(
                text.Replace("$", "")
                    .Replace(",", "")
                    .Trim(),
                CultureInfo.InvariantCulture);
        }

        public async Task VerifyDiscountApplied(decimal expectedDiscount)
        {
            var actualDiscount = await GetDiscountAmountAsync();

            Assert.That(actualDiscount, Is.EqualTo(expectedDiscount));
        }

        public async Task<string> PlaceOrderAsync()
        {
            await PlaceOrderButton.ClickAsync();

            await SuccessMessage.WaitForAsync(
                new LocatorWaitForOptions
                {
                    Timeout = 10000
                });

            return await OrderNumber.TextContentAsync();
        }

        public async Task<bool> IsErrorDisplayedAsync()
        {
            return await ErrorMessage.IsVisibleAsync();
        }

        public async Task<string> GetErrorMessageAsync()
        {
            return await ErrorMessage.TextContentAsync();
        }
    }
}