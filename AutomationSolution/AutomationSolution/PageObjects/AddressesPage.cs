using System;
using System.Reflection.Emit;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace AutomationSolution.PageObjects
{
    public class AddressesPage
    {
        private IWebDriver driver;

        public AddressesPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        [FindsBy(How = How.CssSelector, Using = "[data-test=create]")]
        private IWebElement BtnAddNewAddress { get; set; }

        public AddAddressPage.AddAddressPage NavigateToAddAddressPage()
        {
            BtnAddNewAddress.Click();
            return new AddAddressPage.AddAddressPage(driver);
        }
    }
}