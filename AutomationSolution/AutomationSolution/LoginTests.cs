using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationSolution
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public void Should_Login_Successfully()
        {
            var driver = new ChromeDriver();
            try
            {

                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("http://a.testaddressbook.com/");
                driver.FindElement(By.Id("sign-in")).Click();
                //BAD BAD BAD PRACTICE
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("input[data-test='email']")).SendKeys("asd@asd.asd");
                driver.FindElement(By.Name("session[password]")).SendKeys("asd");
                driver.FindElement(By.CssSelector("input[value='Sign in']")).Click();

                Assert.IsTrue(driver.FindElement(By.CssSelector("span[data-test='current-user']"))
                    .Text.Equals("asd@asd.asd"));

                Assert.AreEqual("asd@asd.asd",
                    driver.FindElement(By.CssSelector("span[data-test='current-user']")).Text);

                Assert.IsTrue(driver.FindElement(By.CssSelector("span[data-test='current-user']")).Displayed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
