using System;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AutomationSolution.PageObjects.AddressesPage;
    

namespace AutomationSolution.PageObjects.AddressDetailsPage
{
    public class AddressDetailsPage
    {
        private IWebDriver driver;

        public AddressDetailsPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        [FindsBy(How = How.CssSelector, Using = "[data-test=notice]")]
        private IWebElement LblSuccessfullyCreated { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[data-test=list]")]
        private IWebElement BtnListAddresses { get; set; }

        public string SuccessfullyCreatedMessage => LblSuccessfullyCreated.Text;

        public AddressesPage.AddressesPage NavigateToAddressesPage()
        {
            BtnListAddresses.Click();
            return new AddressesPage.AddressesPage(driver);
        }
    }
}