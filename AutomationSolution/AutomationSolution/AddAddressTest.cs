﻿using System.Threading;
using AutomationSolution.Controls;
using AutomationSolution.PageObjects;
using AutomationSolution.PageObjects.AddAddressPage;
using AutomationSolution.PageObjects.AddAddressPage.InputData;
using AutomationSolution.PageObjects.HomePage;
using AutomationSolution.PageObjects.InputData;
using AutomationSolution.PageObjects.LoginPage;
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
            var menuItemControl = new LoggedOutMenuItemControl(driver);
            menuItemControl.NavigateToLoginPage();
            var loginPage = new LoginPage(driver);
            loginPage.LoginApplication(new LoginBO());
            var homePage = new HomePage(driver);
            var addressesPage = homePage.menuItemControl.NavigateToAddressesPage();
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