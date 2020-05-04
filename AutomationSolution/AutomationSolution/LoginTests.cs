using System;
using System.Threading;
using AutomationSolution.PageObjects;
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
            driver.FindElement(By.Id("sign-in")).Click();
            //BAD BAD BAD PRACTICE
            Thread.Sleep(1000);
            loginPage = new LoginPage(driver);
        }

        [TestMethod]
        public void Should_Login_Successfully()
        {
            loginPage.LoginApplication("asd@asd.asd", "asd");

            Assert.IsTrue(driver.FindElement(By.CssSelector("span[data-test='current-user']"))
                .Text.Equals("asd@asd.asd"));

            Assert.AreEqual("asd@asd.asd",
                driver.FindElement(By.CssSelector("span[data-test='current-user']")).Text);

            Assert.IsTrue(driver.FindElement(By.CssSelector("span[data-test='current-user']")).Displayed);
            Assert.IsTrue(driver.FindElement(By.CssSelector("span[data-test='current-user']")).Displayed);
        }

        [TestMethod]
        public void Should_Not_Login_With_Incorrect_Email()
        {
            loginPage.LoginApplication("incorrectEmail@asd.asd", "asd");

            Assert.IsTrue(loginPage.FailLoginMessageText.Equals("Bad email or password."));
        }

        [TestMethod]
        public void Should_Not_Login_With_Incorrect_Password()
        {
            loginPage.LoginApplication("asd@asd.asd", "incorrectPassword");

            Assert.IsTrue(loginPage.FailLoginMessageText.Equals("Bad email or password."));
        }

        [TestMethod]
        public void Should_Not_Login_With_Invalid_Credentials()
        {
            loginPage.LoginApplication("incorrectEmail@asd.asd", "incorrectPassword");

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
