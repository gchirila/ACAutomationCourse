using System;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

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

        public string SuccessfullyCreatedMessage => LblSuccessfullyCreated.Text;
    }
}