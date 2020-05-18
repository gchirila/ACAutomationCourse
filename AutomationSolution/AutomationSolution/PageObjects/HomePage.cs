using System;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace AutomationSolution.PageObjects
{
    public class HomePage
    {
        private IWebDriver driver;

        public HomePage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        [FindsBy(How = How.CssSelector, Using = "[data-test=addresses]")]
        private IWebElement BtnAddresses { get; set; }

        public AddressesPage NavigateToAddressesPage()
        {
            BtnAddresses.Click();
            return new AddressesPage(driver);
        }

    }
}