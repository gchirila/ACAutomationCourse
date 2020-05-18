using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace AutomationSolution.PageObjects
{
    public class LoginPage
    {
        private IWebDriver driver;

        public LoginPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        [FindsBy(How = How.CssSelector, Using = "input[data-test='email']")]
        private IWebElement TxtEmail { get; set; }
        
        [FindsBy(How = How.Name, Using = "session[password]")]
        private IWebElement TxtPassword { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "input[value='Sign in']")]
        private IWebElement BtnSignIn { get; set; }
        
        private By failLoginMessage = By.CssSelector("div[data-test='notice']");
        private IWebElement LblFailLoginMessage => driver.FindElement(failLoginMessage);

        public string FailLoginMessageText => LblFailLoginMessage.Text;

        public void LoginApplication(string email, string password)
        {
            TxtEmail.SendKeys(email);
            TxtPassword.SendKeys(password);
            BtnSignIn.Click();
        }

        public void FillEmailAndSignIn(string email)
        {
            TxtEmail.SendKeys(email);
            BtnSignIn.Click();
        }
    }
}