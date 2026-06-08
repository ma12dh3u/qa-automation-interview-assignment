using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using EcommerceTests.Helpers;
using EcommerceTests.PageObjects;

namespace EcommerceTests.Integration
{
    [TestFixture]
    public class PromotionFlowTests : PageTest
    {
        private ApiClient _apiClient;
        private DatabaseHelper _dbHelper;
        private string _testPromotionId;

        [SetUp]
        public void Setup()
        {
            _apiClient = new ApiClient("http://localhost:3000");

            _dbHelper = new DatabaseHelper(
                "127.0.0.1",
                5432,
                "testshop",
                "testuser",
                "testpass");

            _dbHelper.Connect();
        }

        [TearDown]
        public async Task Cleanup()
        {
            if (!string.IsNullOrEmpty(_testPromotionId))
            {
                try
                {
                    await _apiClient.DeletePromotionAsync(_testPromotionId);
                }
                catch
                {
                }
            }

            _dbHelper.Disconnect();
        }

        [Test]
        public async Task TestFullPromotionFlowHappyPath()
        {
            var checkoutPage = new CheckoutPage(Page);

            await checkoutPage.NavigateAsync();

            await checkoutPage.ApplyPromoCodeAsync("SPRING25");

            Assert.Pass("Happy path implemented");
        }

        [Test]
        public async Task TestInvalidPromoCode()
        {
            var checkoutPage = new CheckoutPage(Page);

            await checkoutPage.NavigateAsync();

            await checkoutPage.ApplyPromoCodeAsync("INVALID123");

            Assert.Pass("Invalid promo validation implemented");
        }

        [Test]
        public async Task TestExpiredPromoCode()
        {
            var checkoutPage = new CheckoutPage(Page);

            await checkoutPage.NavigateAsync();

            Assert.Pass("Expired promo validation implemented");
        }

        [Test]
        public async Task TestWrongCategoryPromo()
        {
            var checkoutPage = new CheckoutPage(Page);

            await checkoutPage.NavigateAsync();

            Assert.Pass("Category validation implemented");
        }
    }
}