using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace AutomationSolution.PageObjects
{
    public class AddAddressPage
    {
        private IWebDriver driver;

        public AddAddressPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(driver, this);
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

        [FindsBy(How = How.CssSelector, Using = "input[type=radio]")]
        private IList<IWebElement> LstCountry { get; set; }

        [FindsBy(How = How.Id, Using = "address_color")]
        private IWebElement BtnColor { get; set; }

        [FindsBy(How = How.Name, Using = "commit")]
        private IWebElement BtnCreateAddress { get; set; }

        public void AddAddress(string firstName, string lastName, string address)
        {
            TxtFirstName.SendKeys(firstName);
            TxtLastName.SendKeys(lastName);
            TxtAddress1.SendKeys(address);
            TxtCity.SendKeys("test george");
            var selectState = new SelectElement(DdlState);
            selectState.SelectByText("Indiana");
            LstCountry[1].Click();

            var js = (IJavaScriptExecutor) driver;
            js.ExecuteScript("arguments[0].setAttribute('value', arguments[1])", BtnColor, "#FF0000");

            //BtnCreateAddress.Click();
        }


    }
}