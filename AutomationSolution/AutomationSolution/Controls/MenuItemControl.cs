using System;
using System.Net.Configuration;
using AutomationSolution.PageObjects.LoginPage;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace AutomationSolution.Controls
{
    public class MenuItemControl
    {
        public IWebDriver driver;

        public MenuItemControl(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        [FindsBy(How = How.CssSelector, Using = "")]
        private IWebElement BtnHome { get; set; }
    }

    public class LoggedOutMenuItemControl: MenuItemControl
    {
        [FindsBy(How = How.Id, Using = "sign-in")]
        private IWebElement BtnSignIn { get; set; }

        public LoggedOutMenuItemControl(IWebDriver browser) : base(browser)
        {
        }

        public LoginPage NavigateToLoginPage()
        {
            BtnSignIn.Click();
            return new LoginPage(driver);
        }
    }

    public class LoggedInMenuItemControl : MenuItemControl
    {
        [FindsBy(How = How.CssSelector, Using = "[data-test=addresses]")]
        private IWebElement BtnAddresses { get; set; }

        [FindsBy(How = How.CssSelector, Using = "")]
        private IWebElement BtnSignOut { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span[data-test='current-user']")]
        private IWebElement LblUserEmail { get; set; }

        public LoggedInMenuItemControl(IWebDriver browser) : base(browser)
        {

        }
        public PageObjects.AddressesPage.AddressesPage NavigateToAddressesPage()
        {
            BtnAddresses.Click();
            return new PageObjects.AddressesPage.AddressesPage(driver);
        }

        public string UserEmailMessage => LblUserEmail.Text;
        public bool UserEmailDislyed => LblUserEmail.Displayed;
    }
}