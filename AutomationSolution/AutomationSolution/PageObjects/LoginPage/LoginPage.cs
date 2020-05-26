using System;
using AutomationSolution.PageObjects.InputData;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace AutomationSolution.PageObjects.LoginPage
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
        
        [FindsBy(How = How.CssSelector, Using = "div[data-test='notice']")]
        private  IWebElement LblFailLoginMessage { get; set; }

        public string FailLoginMessageText => LblFailLoginMessage.Text;

        public void LoginApplication(LoginBO loginBo)
        {
            TxtEmail.SendKeys(loginBo.UserEmail);
            TxtPassword.SendKeys(loginBo.Password);
            BtnSignIn.Click();
        }

        public void FillEmailAndSignIn(string email)
        {
            TxtEmail.SendKeys(email);
            BtnSignIn.Click();
        }
    }
}