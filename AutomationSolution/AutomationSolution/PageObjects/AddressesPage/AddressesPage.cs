using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace AutomationSolution.PageObjects.AddressesPage
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

        [FindsBy(How = How.CssSelector, Using = "tbody tr")]
        private IList<IWebElement> LstAddresses { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[data-test=notice]")]
        private IWebElement LblSuccessfullyDestroyed { get; set; }

        public string SuccessfullyDestroyedMessage => LblSuccessfullyDestroyed.Text;

        private By delete = By.CssSelector("[data-method=delete]");
        private IWebElement BtnDestroy(string address) =>
            LstAddresses.FirstOrDefault(element => element.Text.Contains(address))?.FindElement(delete);

        public void DeleteAddress(string addressName)
        {
            BtnDestroy(addressName).Click();
            driver.SwitchTo().Alert().Accept();
        }

        public AddAddressPage.AddAddressPage NavigateToAddAddressPage()
        {
            BtnAddNewAddress.Click();
            return new AddAddressPage.AddAddressPage(driver);
        }
    }
}