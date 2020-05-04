using OpenQA.Selenium;

namespace AutomationSolution.PageObjects
{
    public class LoginPage
    {
        private IWebDriver driver;

        public LoginPage(IWebDriver browser)
        {
            driver = browser;
        }

        private By email = By.CssSelector("input[data-test='email']");
        private IWebElement TxtEmail => driver.FindElement(email);

        private By password = By.Name("session[password]");
        private IWebElement TxtPassword => driver.FindElement(password);

        private By signIn = By.CssSelector("input[value='Sign in']");
        private IWebElement BtnSignIn => driver.FindElement(signIn);

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