using AutomationSolution.Controls;
using AutomationSolution.PageObjects.AddAddressPage.InputData;
using AutomationSolution.PageObjects.AddressesPage;
using AutomationSolution.PageObjects.HomePage;
using AutomationSolution.PageObjects.InputData;
using AutomationSolution.PageObjects.LoginPage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationSolution
{
    [TestClass]
    public class DeleteAddressTests
    {
        private IWebDriver driver;
        private AddressesPage addressesPage;
        private AddAddressBO addAddressBo = new AddAddressBO();

        [TestInitialize]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://a.testaddressbook.com/");
            var menuItemControl = new LoggedOutMenuItemControl(driver);
            menuItemControl.NavigateToLoginPage();
            var loginPage = new LoginPage(driver);
            loginPage.LoginApplication(new LoginBO());
            var homePage = new HomePage(driver);
            addressesPage = homePage.menuItemControl.NavigateToAddressesPage();
            var addAddressPage = addressesPage.NavigateToAddAddressPage();
            var addressDetailsPage = addAddressPage.AddAddress(addAddressBo);
            addressesPage = addressDetailsPage.NavigateToAddressesPage();
        }

        [TestMethod]
        public void Should_Delete_Address_Successfully()
        { 
            addressesPage.DeleteAddress(addAddressBo.FirstName);
            Assert.IsTrue(addressesPage.SuccessfullyDestroyedMessage.Equals("Address was successfully destroyed."));
        }


        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}