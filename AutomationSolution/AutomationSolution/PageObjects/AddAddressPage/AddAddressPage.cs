using System;
using System.Collections.Generic;
using AutomationSolution.PageObjects.AddAddressPage.InputData;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace AutomationSolution.PageObjects.AddAddressPage
{
    public class AddAddressPage
    {
        private IWebDriver driver;

        public AddAddressPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        [FindsBy(How = How.Id, Using = "address_first_name")]
        private IWebElement TxtFirstName { get; set; }

        [FindsBy(How = How.Id, Using = "address_last_name")]
        private IWebElement TxtLastName { get; set; }

        [FindsBy(How = How.Id, Using = "address_street_address")]
        private IWebElement TxtAddress1 { get; set; }

        [FindsBy(How = How.Id, Using = "address_city")]
        private IWebElement TxtCity { get; set; }

        [FindsBy(How = How.Id, Using = "address_state")]
        private IWebElement DdlState { get; set; }

        [FindsBy(How = How.Id, Using = "address_zip_code")]
        private IWebElement TxtZipCode { get; set; }

        [FindsBy(How = How.CssSelector, Using = "input[type=radio]")]
        private IList<IWebElement> LstCountry { get; set; }

        [FindsBy(How = How.Id, Using = "address_color")]
        private IWebElement BtnColor { get; set; }

        [FindsBy(How = How.Name, Using = "commit")]
        private IWebElement BtnCreateAddress { get; set; }

        public AddressDetailsPage.AddressDetailsPage AddAddress(AddAddressBO addAddressBo)
        {
            TxtFirstName.SendKeys(addAddressBo.FirstName);
            TxtLastName.SendKeys(addAddressBo.LastName);
            TxtAddress1.SendKeys(addAddressBo.Address1);
            TxtCity.SendKeys(addAddressBo.City);
            var selectState = new SelectElement(DdlState);
            selectState.SelectByText(addAddressBo.State);
            TxtZipCode.SendKeys(addAddressBo.ZipCode);
            LstCountry[addAddressBo.Country].Click();

            var js = (IJavaScriptExecutor) driver;
            js.ExecuteScript("arguments[0].setAttribute('value', arguments[1])", BtnColor, addAddressBo.Color);

            BtnCreateAddress.Click();
            return new AddressDetailsPage.AddressDetailsPage(driver);
        }


    }
}