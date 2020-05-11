using System.Threading;
using AutomationSolution.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationSolution
{
    [TestClass]
    public class AddAddressTest
    {
        private IWebDriver driver;
        private AddAddressPage addAddressPage;

        [TestInitialize]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://a.testaddressbook.com/");
            driver.FindElement(By.Id("sign-in")).Click();
            //BAD BAD BAD PRACTICE
            Thread.Sleep(1000);
            var loginPage = new LoginPage(driver);
            loginPage.LoginApplication("asd@asd.asd", "asd");
            var homePage = new HomePage(driver);
            var addressesPage = homePage.NavigateToAddressesPage();
            //BAD BAD BAD PRACTICE
            Thread.Sleep(1000);
            addAddressPage = addressesPage.NavigateToAddAddressPage();
        }

        [TestMethod]
        public void Should_Add_Address_Successfully()
        {
            addAddressPage.AddAddress("test george", "test george", "test george");
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}