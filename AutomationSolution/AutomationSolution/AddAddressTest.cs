using System.Threading;
using AutomationSolution.PageObjects;
using AutomationSolution.PageObjects.AddAddressPage;
using AutomationSolution.PageObjects.AddAddressPage.InputData;
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
            var loginPage = new LoginPage(driver);
            loginPage.LoginApplication("asd@asd.asd", "asd");
            var homePage = new HomePage(driver);
            var addressesPage = homePage.NavigateToAddressesPage();
            addAddressPage = addressesPage.NavigateToAddAddressPage();
        }

        [TestMethod]
        public void Should_Add_Address_Successfully()
        {
            var addAddressBo = new AddAddressBO
            {
                FirstName = "Changed AC George",
                ZipCode = "Changed Zip Code"
            };
            var addressDetailsPage = addAddressPage.AddAddress(addAddressBo);
            Assert.AreEqual("Address was successfully created.", addressDetailsPage.SuccessfullyCreatedMessage);
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}