using AutomationSolution.Controls;
using AutomationSolution.PageObjects.HomePage;
using AutomationSolution.PageObjects.InputData;
using AutomationSolution.PageObjects.LoginPage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationSolution
{
    [TestClass]
    public class LoginTests
    {
        private IWebDriver driver;
        private LoginPage loginPage;

        [TestInitialize]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://a.testaddressbook.com/");
            var menuItemControl = new LoggedOutMenuItemControl(driver);
            menuItemControl.NavigateToLoginPage();
            loginPage = new LoginPage(driver);
        }

        [TestMethod]
        public void Should_Login_Successfully()
        {
            loginPage.LoginApplication(new LoginBO());
            var homePage = new HomePage(driver);

            Assert.IsTrue(homePage.menuItemControl.UserEmailMessage.Equals("asd@asd.asd"));

            Assert.AreEqual("asd@asd.asd", homePage.menuItemControl.UserEmailMessage);

            Assert.IsTrue(homePage.menuItemControl.UserEmailDislyed);
            Assert.IsTrue(homePage.menuItemControl.UserEmailDislyed);
        }

        [TestMethod]
        public void Should_Not_Login_With_Incorrect_Email()
        {
            var loginBo = new LoginBO
            {
                UserEmail = "incorrectEmail@asd.asd"
            };
            loginPage.LoginApplication(loginBo);

            Assert.IsTrue(loginPage.FailLoginMessageText.Equals("Bad email or password."));
        }

        [TestMethod]
        public void Should_Not_Login_With_Incorrect_Password()
        {
            var loginBo = new LoginBO
            {
                Password = "incorrectPassword"
            };
            loginPage.LoginApplication(loginBo);

            Assert.IsTrue(loginPage.FailLoginMessageText.Equals("Bad email or password."));
        }

        [TestMethod]
        public void Should_Not_Login_With_Invalid_Credentials()
        {
            var loginBo = new LoginBO
            {
                UserEmail = "incorrectEmail@asd.asd",
                Password = "incorrectPassword"
            };
            loginPage.LoginApplication(loginBo);

            Assert.IsTrue(loginPage.FailLoginMessageText.Equals("Bad email or password."));
        }

        [TestMethod]
        public void Should_Not_Login_With_No_Password()
        {
            loginPage.FillEmailAndSignIn("asd@asd.asd");

            Assert.IsTrue(loginPage.FailLoginMessageText.Equals("Bad email or password."));
        }

        [TestCleanup]
        public void CleanUp()
        {
            driver.Quit();
        }
    }
}
