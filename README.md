# ACAutomationCourse 
 ACAutomationCourse 
 
### **Session 1**

We have discussed about automation:
  1. What is it
  2. Why is important
  3. Types of automation testing

The presentation will be sent via email at the end of this course.

### **Session 2:  Let's write our first UI automation tests**

**Scope:** This session scope was to create a unit test project, install all the dependencies and write some simple tests

How to create a unit test project:
  1. Open Visual studio
  2. Click File > New > Project
  3. Search for unit test: Unit Test Project(.NET Framework)
  4. Add project Name
  5. Click OK button
  
At this moment, we have created a unit test project and should have a default test class: UnitTest1. 
The class has the following particularities: 
1. It has [TestClass] annotation that identifies the class to be a test one. Without this annotation, the test under this class cannot     be recognized and there for cannot run the tests within it.
2. The class contains a test method that has a [TestMethod] annotation. This help the method to be recognized as an test method and run it accordingly.
  
Find more info on: https://docs.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2019
  
Let's import the needed packages in order to open a chrome browser using our test. Do a right click on the created project and 
click Manage Nuget Packages. On the opened window, select Browse tab and search for Selenium.Webdriver and install it in your project. Then search for Selenium.Webdriver.ChromeDriver and install the latest version(avoid the ones that are in betha). 

Be aware that if the local installed Chrome is not the same with the installed package, it will trigger a compatibility error when running the test.
  
Let's write some code :) 
  
**REMEMBER: THE TEST DOES WHAT YOU TELL IT TO DO.**
Thats why, selenium manipulates the elements in DOM as a human would do. For now, we will use click and sendkeys.
  
Our first automated test case will try to login into application. For this, we will need a write code for the next steps:
  1. Open the browser
  2. Maximize the page
  3. Open the application URL
  4. Click Sign in button
  5. Fill user email
  6. Fill user password
  7. Click Sign in button 
  8. Assert that the current user label contains our email
    
Open the UnitTest1 class and let instantiate our driver to open the Chrome browser.

```csharp  
      var driver = new ChromeDriver(); //open chrome browser
      driver.Manage().Window.Maximize(); //maximize the window
      driver.Navigate().GoToUrl("OUR URL"); //access the SUT(System Under Test) url. In our case http://a.testaddressbook.com/
```

Until now, we have covered the first 3 steps of our test. Let's complete our test:

```csharp  
      driver.FindElement(By.Id("sign-in")).Click();
      Thread.Sleep(1000);
      driver.FindElement(By.Id("session_email")).SendKeys("email that was used for creating the account");
      driver.FindElement(By.Name("session[password]")).SendKeys("password used to create the account");
      driver.FindElement(By.CssSelector("input[value='Sign in']")).Click();
      Thread.Sleep(2000);
      Assert.AreEqual("asd@asd.asd", driver.FindElement(By.XPath("//span[@data-test='current-user']")).Text);
```

Clarification :)
  1. WebElement represents a DOM element. WebElements can be found by searching from the document root using a WebDriver instance. WebDriver API provides built-in methods to find the WebElements which are based on different properties like ID, Name, Class, XPath, CSS Selectors, link Text, etc.
  2. There are some ways of optimizing our selectors used to identify the elements in page.  
      a. For the CssSelector, the simplest way is to use the following format: tagname[attribute='attributeValue'].  
      b. For the XPath, the simplest way is to use the following format: //tagname[@attribute='attributeValue'].  
    Let's take for example the Sign in button:
    
```csharp    
    <input type="submit" name="commit" value="Sign in" class="btn btn-primary" data-test="submit" data-disable-with="Sign in">
```

    Explained: 
```csharp    
    <input(this is the tagname) 
        type(this is the attribute)="submit"(this is the value) --> The CssSelector would be: input[type='submit']
        name(this is the attribute)="commit"(this is the value) --> The XPath would be: //input[@name='commit']
        value(this is the attribute)="Sign in"(this is the value) --> The CssSelector would be: input[value='Sign in']
        class(this is the attribute)="btn btn-primary"(this is the value) --> The XPath would be: //input[@class='btn btn-primary']
        data-test(this is the attribute)="submit"(this is the value) --> The CssSelector would be: input[data-test='submit']
        data-disable-with(this is the attribute)="Sign in"(this is the value) --> The Xpath would be: //input[data-disabled-with='Sign in']
    >
```   

### **Session 3: Let's refine/refactor our code**

**Scope:** This session scope was to use locators strategy, MSTest methods to initialize/clean up our test data and to get rid of our duplicate code

One of a test case component is the prerequisite: conditions that must be met before the test case can be run.
Our code test login scenarios and we need to see what are the prerequisites.
We have identified the following steps that need to be execute before running the test:

```csharp
            var driver = new ChromeDriver(); // open the browser
            driver.Manage().Window.Maximize(); // maximize the window 
            driver.Navigate().GoToUrl("http://a.testaddressbook.com/"); // access the SUT url
            driver.FindElement(By.Id("sign-in")).Click(); // click on sign in button in order to be redirected to the login page
            Thread.Sleep(1000); // THIS sleep is a bad practice AND NEEDS TO BE DELETED AND WE WILL BURN IT WITH FIRE IN THE NEAR FUTURE
```

Also, after the test has finished running, we need to clean up the operations that we made in our test in order to not impact further test. Remember, each test is independent and should not influence the result of other tests. In our test, the clean up would mean to close the browser.

```csharp
            driver.Quit();
```

MSTest provides a way to declare methods to be called by the test runner before or after running a test.

```csharp
            [TestInitialize]
            public void TestInitialize()
            {
            }

            [TestCleanup]
            public void TestCleanup()
            {
            }
```

The method decorated by [TestInitialize] is called before running each test of the class. The method decorated by [TestCleanup] is called after running each test of the class.

First, we need to remove the init/clean up steps and to move it the according method. At this point, our tests should look like this: 

```csharp
          namespace UnitTestProject1
          {
              [TestClass]
              public class LoginTests
              {
                  //declare IWebDriver instance variable
                  //use it outside of any methods so that we can use it in various methods
                  private IWebDriver driver;

                  [TestInitialize]
                  public void SetUp()
                  {
                      //initialize the needed driver. In our case is ChromeDriver
                      driver = new ChromeDriver();
                      loginPage = new LoginPage(driver);
                      driver.Manage().Window.Maximize();
                      driver.Navigate().GoToUrl("http://a.testaddressbook.com/");
                      driver.FindElement(By.Id("sign-in")).Click();
                      Thread.Sleep(1000);
                  }

                  [TestMethod]
                  public void Should_Login_Successfully()
                  {
                      driver.FindElement(By.Id("session_email")).SendKeys("asd@asd.asd");
                      driver.FindElement(By.Id("session_password")).SendKeys("asd");
                      driver.FindElement(By.Name("commit")).Click();

                      var expectedResult = "test@test.test";
                      var actualResults = driver.FindElement(By.CssSelector("span[data-test='current-user']")).Text;

                      Assert.AreEqual(expectedResult, actualResults);
                  }

                  [TestMethod]
                  public void Should_Not_Login_With_Invalid_Credentials()
                  {
                      driver.FindElement(By.Id("session_email")).SendKeys("wrong@wrong.wrong");
                      driver.FindElement(By.Id("session_password")).SendKeys("wrong");
                      driver.FindElement(By.Name("commit")).Click();

                      var expectedResult = "Bad email or password.";
                      var actualResults = driver.FindElement(By.XPath("//div[starts-with(@class, 'alert')]")).Text;

                      Assert.AreEqual(expectedResult, actualResults);
                  } 

                  [TestCleanup]
                  public void CleanUp()
                  {
                      driver.Quit();
                  }
              }
          }   
```

ONE OF THE COMMON MISTAKE IS TO DECLARE THE IWebDriver INSTANCE VARIABLE WITHIN THE TEST INIT:

```csharp
        private IWebDriver driver;

        [TestInitialize]
        public void SetUp()
        {
            var driver = new ChromeDriver();
          //this is a method local variable and cannot be used in other methods outside init. The tests will throw null refrence since it doesn't know about the ChromeDriver instance
        }
```

Our code starts to look cleaner :)

Lets add more login tests to the suite by doing a barbaric copy paste of one of our test methods and do the updates:

```csharp
		//Should_Not_Login_With_Incorrect_Email
		[TestMethod]
		public void Should_Not_Login_With_Incorrect_Email()
		{
		    driver.FindElement(By.Id("session_email")).SendKeys("wrong@wrong.wrong");
		    driver.FindElement(By.Id("session_password")).SendKeys("asd");
		    driver.FindElement(By.Name("commit")).Click();
		
		    var expectedResult = "Bad email or password.";
		    var actualResults = driver.FindElement(By.XPath("//div[starts-with(@class, 'alert')]")).Text;
		
		    Assert.AreEqual(expectedResult, actualResults);
		} 	
	
		//Should_Not_Login_With_Incorrect_Password
		[TestMethod]
		public void Should_Not_Login_With_Incorrect_Password()
		{
		    driver.FindElement(By.Id("session_email")).SendKeys("asd@asd.asd");
		    driver.FindElement(By.Id("session_password")).SendKeys("worng");
		    driver.FindElement(By.Name("commit")).Click();
		
		    var expectedResult = "Bad email or password.";
		    var actualResults = driver.FindElement(By.XPath("//div[starts-with(@class, 'alert')]")).Text;
		
		    Assert.AreEqual(expectedResult, actualResults);
		} 	
	
		//Should_Not_Login_With_No_Password
		[TestMethod]
		public void Should_Not_Login_With_No_Password()
		{
		    driver.FindElement(By.Id("session_email")).SendKeys("wrong@wrong.wrong");
		    driver.FindElement(By.Name("commit")).Click();
		
		    var expectedResult = "Bad email or password.";
		    var actualResults = driver.FindElement(By.XPath("//div[starts-with(@class, 'alert')]")).Text;
		
		    Assert.AreEqual(expectedResult, actualResults);
		} 	
```

But wait, there is more work to do. Let's say that the login page layout needs to be changed. Our test will fail after this changes.
We have only two tests and will be easy to fix it. But imagine that we have 25 login tests. Is not so funny to update all the tests.

A better approach to script maintenance is to create a separate class file which would find web elements, fill them or verify them. This class can be reused in all the scripts using that element. In future, if there is a change in the web element, we need to make the change in just 1 class file and not 10 different scripts.

This approach is called Page Object Model(POM). It helps make the code more readable, maintainable, and reusable.

Page Object model is an object design pattern in Selenium, where web pages are represented as classes, and the various elements on the page are defined as variables on the class. All possible user interactions can then be implemented as methods on the class.

Let's create the the page object that contains the elements for the login page: LoginPage.cs

Right click on the project > Add > Folder and name it PageObjects

Right click on the PageObjects folder > Add > New Item... > Add a class with name: LoginPage.cs

We need to add the objects that we use in our script in this class: email input, password input, sign in button and create a method to login the user.

Our login page will look like this in the end:

```csharp
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
```

At this point, we need to update our tests:

```csharp
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
```

### **Session 4: Let's write an UI automation tests for adding a new address**

**Scope:** This session scope was to create a test for adding addresses and to keep the code clean
We will change the strategy and use Page Factory approach.
The PageFactory Class in Selenium is an extension to the Page Object design pattern. It is used to initialize the elements of the Page Object or instantiate the Page Objects itself. Annotations for elements can also be created (and recommended) as the describing properties may not always be descriptive enough to tell one object from the other.

It is used to initialize elements of a Page class without having to use ‘FindElement’ or ‘FindElements’. Annotations can be used to supply descriptive names of target objects to improve code readability. There is however a few differences between C# and Java implementation – Java provide greater flexibility with PageFactory.

In order to add a new address, we will need a write code for the next steps:
1.	Open the browser
2.	Maximize the page
3.	Open the application URL
4.	Click Sign in button (NavigateToLoginPage method)
5.	Fill user email and password, then click Sign in button (LoginApplication method)
6.	Navigate to addresses page
7.	Navigate to add address page
8.	Complete the form with mandatory fields and click Save button
9.	Assert that the success message is shown

First, we need to apply the Page Factory stratefy to our Login page object.

At step 5, another method can contain login actions (fill user email and password, click Sign in button) and should be added in page object for LoginPage.cs:

```csharp
    public class LoginPage
    {
        private IWebDriver driver;

        public LoginPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input[data-test='email']")]
        private IWebElement TxtEmail { get; set; }
        
        [FindsBy(How = How.Name, Using = "session[password]")]
        private IWebElement TxtPassword { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "input[value='Sign in']")]
        private IWebElement BtnSignIn { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "div[data-test='notice']")]
        private IWebElement LblFailLoginMessage { get; set; }
            
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
```
Notice that we instantiate our objects with the following line of code in the constructor: 
```csharp
    PageFactory.InitElements(this, driver);
```

In order to navigate to addresses page, we need to create a page object HomePage.cs that contains elements and method for this page:

```csharp
    public class HomePage
    {
        private IWebDriver driver;

        public HomePage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "[data-test=addresses]")]
        private IWebElement BtnAddresses { get; set; }

        public AddressesPage NavigateToAddressesPage()
        {
            BtnAddresses.Click();
            return new AddressesPage(driver);
        }

    }
```

Next step is to navigate to add address page. For this, we need another page object AddressesPage.cs which contains New Address button declaration and method to clicks on the element:

```csharp
    public class AddressesPage
    {
        private IWebDriver driver;

        public AddressesPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "[data-test=create]")]
        private IWebElement BtnAddNewAddress { get; set; }

        public AddAddressPage.AddAddressPage NavigateToAddAddressPage()
        {
            BtnAddNewAddress.Click();
            return new AddAddressPage.AddAddressPage(driver);
        }
    }
```

For step 8 (Complete the form with mandatory fields and click Save button), we will create a page object that contains the elements for the add address page: AddAdressPage.cs.
We need to add the objects that we use in our script in this class: first name, last name, address, city, zip code and color inputs, state dropdown, country select, save button and create a method to add the address.
Our add address page will look like this:

```csharp
    public class AddAddressPage
    {
        private IWebDriver driver;

        public AddAddressPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "address_first_name")]
        private IWebElement TxtFirstName { get; set; }

        [FindsBy(How = How.Id, Using = "address_last_name")]
        private IWebElement TxtLastName { get; set; }

        [FindsBy(How = How.Id, Using = "address_street_address")]
        private IWebElement TxtAddress1 { get; set; }

        [FindsBy(How = How.Id, Using = "address_city")]
        private IWebElement TxtCity { get; set; }

        [FindsBy(How = How.Id, Using = "address_state")]
        private IWebElement DdlState { get; set; }

        [FindsBy(How = How.Id, Using = "address_zip_code")]
        private IWebElement TxtZipCode { get; set; }

        [FindsBy(How = How.CssSelector, Using = "input[type=radio]")]
        private IList<IWebElement> LstCountry { get; set; }

        [FindsBy(How = How.Id, Using = "address_color")]
        private IWebElement BtnColor { get; set; }

        [FindsBy(How = How.Name, Using = "commit")]
        private IWebElement BtnCreateAddress { get; set; }

        public AddressDetailsPage.AddressDetailsPage AddAddress(string firstName, string lastName)
        {
            TxtFirstName.SendKeys(firstName);
            TxtLastName.SendKeys(lastName);
            TxtAddress1.SendKeys("hardcoded string");
            TxtCity.SendKeys("hardcoded string";
            var selectState = new SelectElement(DdlState);
            selectState.SelectByText("hardcoded string");
            TxtZipCode.SendKeys("hardcoded string");
            LstCountry[1].Click();

            var js = (IJavaScriptExecutor) driver;
            js.ExecuteScript("arguments[0].setAttribute('value', arguments[1])", BtnColor, "hardcoded string");

            BtnCreateAddress.Click();
            return new AddressDetailsPage.AddressDetailsPage(driver);
        }
    }
}
```

In order to finish the automated tests, we need to make an assertion. For this test, will be the success message shown in the address details page, after saving it. Another page object will be created: AddressDetailsPage.cs

```csharp
    public class AddressDetailsPage
    {
        private IWebDriver driver;

        public AddressDetailsPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(this, driver);
        }

        [FindsBy(How = How.CssSelector, Using = "[data-test=notice]")]
        private IWebElement LblSuccessfullyCreated { get; set; }

        public string SuccessfullyCreatedMessage => LblSuccessfullyCreated.Text;
    }
```

At this point, our automated tests will look like this:

```csharp
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
```

### **Session 5: Let’s continue to refactor our project**

**Scope:** This session scope was to refactored our code and replace Thread.Sleep with efficient waits

At this point, we have the AddAddress method that has 2 parameters and some hardcoded strings for filling the necesary data:
```csharp
        public AddressDetailsPage.AddressDetailsPage AddAddress(string firstName, string lastName)
        {
            TxtFirstName.SendKeys(firstName);
            TxtLastName.SendKeys(lastName);
            TxtAddress1.SendKeys("hardcoded string");
            TxtCity.SendKeys("hardcoded string";
            var selectState = new SelectElement(DdlState);
            selectState.SelectByText("hardcoded string");
            TxtZipCode.SendKeys("hardcoded string");
            LstCountry[1].Click();

            var js = (IJavaScriptExecutor) driver;
            js.ExecuteScript("arguments[0].setAttribute('value', arguments[1])", BtnColor, "hardcoded string");

            BtnCreateAddress.Click();
            return new AddressDetailsPage.AddressDetailsPage(driver);
        }
```
Having harcoded values in our methods is considered bad practice and not maintanable.
Also, adding to many parameters to the methods is considered flaky.

To parametrize AddAddress method in an efficient way, we can create a business object class called AddAddressBO.cs which will contain the objects needed in the process of adding an address: 

```csharp
    public class AddAddressBO
    {
        public string TxtFirstName = "test";
        public string TxtLastName = "test";
        public string TxtAddress1 = "test";
        public string TxtCity = "test";
        public string TxtState = "Hawaii";
        public string TxtZipCode = "test";
        public string TxtBirthdayMonth = "03";
        public string TxtBirthdayYear = "2000";
        public string TxtColor = "#FF0000";
    }
```

Then, use it in the AddAddress method as a parameter and to access his properties:

```csharp
         public AddressDetailsPage.AddressDetailsPage AddAddress(AddAddressBO addAddressBo)
        {
            TxtFirstName.SendKeys(addAddressBo.FirstName);
            TxtLastName.SendKeys(addAddressBo.LastName);
            TxtAddress1.SendKeys(addAddressBo.Address1);
            TxtCity.SendKeys(addAddressBo.City);
            var selectState = new SelectElement(DdlState);
            selectState.SelectByText(addAddressBo.State);
            TxtZipCode.SendKeys(addAddressBo.ZipCode);
            LstCountry[addAddressBo.Country].Click();

            var js = (IJavaScriptExecutor) driver;
            js.ExecuteScript("arguments[0].setAttribute('value', arguments[1])", BtnColor, addAddressBo.Color);

            BtnCreateAddress.Click();
            return new AddressDetailsPage.AddressDetailsPage(driver);
        }
```

Our solution is growing and starts to have some classes. 
We need to structure our solution a bit in order to be readable.
Create a folder: **AddAddressPage** and move AddAddressPage.cs page object in this folder.
Alse, within **AddAddressPage**, create folder:  **InputData** and move AddAddressBo.cs class in this folder.
This can be done for every page object that we have in our application because in this way, we create clarity of what we have in our solution.

Now, we can move on to the **Wait Strategy** and how to use it.

There are explicit and implicit waits in Selenium Web Driver. Waiting is having the automated task execution elapse a certain amount of time before continuing with the next step. 

You should choose to use Explicit or Implicit Waits.

**•	Thread.Sleep**

In particular, this pattern of sleep is an example of explicit waits. So this isn’t actually a feature of Selenium WebDriver, it’s a common feature in most programming languages though.

Thread.Sleep() does exactly what you think it does, it sleeps the thread.

Example:

```csharp
            Thread.Sleep(2000);
```

Warning! Using Thread.Sleep() can leave to random failures (server is sometimes slow), you don't have full control of the test and the test could take longer than it should. It is a good practice to use other types of waits.

**•	Implicit Wait**

WebDriver will poll the DOM for a certain amount of time when trying to find an element or elements if they are not immediately available

Example:

```csharp
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
```

**•	Explicit Wait** - Wait for a certain condition to occur before proceeding further in the code

In practice, we recommend that you use Web Driver Wait in combination with methods of the Expected Conditions class that reduce the wait. If the element appeared earlier than the time specified during Web Driver wait initialization, Selenium will not wait but will continue the test execution.

Example 1:
This can be called within the method
```csharp
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementToBeClickable(TxtFirstName));
```

Example 2:
Adding the wait in the constuctor. In this way, we can create 
```csharp
        public AddAdressPage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(driver, this);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementToBeClickable(TxtFirstName));
        }

        [FindsBy(How = How.Id, Using = "address_first_name")]
        private IWebElement TxtFirstName { get; set; }
```

### **Session 6**
**Scope:** This session scope is to handle grids and menu in our automation script
At this point, we have added an address, but the application has also implemented the delete functionality. This can be done in the Addresses page.
Within this page we have a grid that contains a list of addresses(table row > tr) and each address contains it's details(table data > td) and the posibility to view/edit/delete it.
After clicking delete button, an alert appears. Selenium Webdriver manipulate(accept/dismiss/get text) the alert and accept it in our case:

```csharp
		driver.SwitchTo().Alert().Accept();
```

Selenium Webdriver gives us the posibility to chain elements and create a structure father child:
```csharp
		driver.FindElement("father").FindElement(By.CssSelector("child")).FindElement(By.CssSelector("grandchild"));
```

We need to iterate the list to identify the address that we have added and to delete it. There are multiple ways to do it, but the preferred one is documented below: 
```csharp
		 [FindsBy(How = How.CssSelector, Using = "tbody tr")]
        private IList<IWebElement> LstAddresses { get; set; }

        private By delete = By.CssSelector("[data-method=delete]");
        private IWebElement BtnDestroy(string address) =>
            LstAddresses.FirstOrDefault(element => element.Text.Contains(address))?.FindElement(delete);
														
		public void DeleteAddress(string addressName)
        {
            BtnDestroy(addressName).Click();
            driver.SwitchTo().Alert().Accept();
        }
```

Another way would be to create a method and iterate the list:
```csharp
        public void DeleteAddressV2(string address)
        {
            foreach (var address in LstAddresses)
            {
                if (address.Text.Contains(address))
                {
                    address.FindElement(delete).Click();
                    driver.SwitchTo().Alert().Accept();
                    break;
                }
            }
        }    
```


And the methods could be called in test classes:
```csharp

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
			
```

Now let's hanle the menu.
The menu is present in all the app pages and we need to create a single repository where the menu elements can be stored.
This is a shared component and we need to call it in all of our page objects.
The first step is to create a class named MenuItemControl. This class will containt all menu elements.

```csharp

		public class MenuItemControl
	    {
	        public IWebDriver driver;
			
			public MenuItemControl(IWebDriver browser)
	        {
	            driver = browser;
	        }
			
			[FindsBy(How = How.CssSelector, Using = "")]
            private IWebElement BtnHome { get; set; }
	    
	        [FindsBy(How = How.Id, Using = "sign-in")]
            private IWebElement BtnSignIn { get; set; }
	
	        [FindsBy(How = How.CssSelector, Using = "[data-test=addresses]")]
            private IWebElement BtnAddresses { get; set; }

            [FindsBy(How = How.CssSelector, Using = "")]
            private IWebElement BtnSignOut { get; set; }

            [FindsBy(How = How.CssSelector, Using = "span[data-test='current-user']")]
            private IWebElement LblUserEmail { get; set; }
		}
		
```

The application has 2 contexts, but this menu cannot be used from both perspectives: logged out and logged in.
Let's identify the elements used in this contextes:
			- logged out: Home, Sign in
			- logged in: Home, Addresses, Sign out and User email
The common webelement for both contextes is Home.
Now that we have identified what we have, we need to create 2 classes for out contextes: LoggedOutMenuItemControl and LoggedInMenuItemControl that will inhirit the MenuItemControlClass.
And we need to move the elements to the according classes. We also need to move the navigation logic to this classes.

```csharp

	public class MenuItemControl
    {
        public IWebDriver driver;

        public MenuItemControl(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        [FindsBy(How = How.CssSelector, Using = "")]
        private IWebElement BtnHome { get; set; }
    }
		
```

```csharp

	public class LoggedOutMenuItemControl: MenuItemControl
    {
        [FindsBy(How = How.Id, Using = "sign-in")]
        private IWebElement BtnSignIn { get; set; }

        public LoggedOutMenuItemControl(IWebDriver browser) : base(browser)
        {
        }

        public LoginPage NavigateToLoginPage()
        {
            BtnSignIn.Click();
            return new LoginPage(driver);
        }
    }
		
```

```csharp

	public class LoggedInMenuItemControl : MenuItemControl
    {
        [FindsBy(How = How.CssSelector, Using = "[data-test=addresses]")]
        private IWebElement BtnAddresses { get; set; }

        [FindsBy(How = How.CssSelector, Using = "")]
        private IWebElement BtnSignOut { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span[data-test='current-user']")]
        private IWebElement LblUserEmail { get; set; }

        public LoggedInMenuItemControl(IWebDriver browser) : base(browser)
        {

        }
        public PageObjects.AddressesPage.AddressesPage NavigateToAddressesPage()
        {
            BtnAddresses.Click();
            return new PageObjects.AddressesPage.AddressesPage(driver);
        }

        public string UserEmailMessage => LblUserEmail.Text;
        public bool UserEmailDislyed => LblUserEmail.Displayed;
    }
```

Let's used in the HomePage.cs, LoginTests.cs, AddAdreessTest.cs and DeleteAddressTests.cs

Home page
```csharp
	public class HomePage
    {
        private IWebDriver driver;

        public HomePage(IWebDriver browser)
        {
            driver = browser;
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        public LoggedInMenuItemControl menuItemControl => new LoggedInMenuItemControl(driver);
    }
```

Login tests
```csharp

        //LoginTests
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
```

Add address tests
```csharp
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
```
Delete address tests
```csharp
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
```
As said before, this can be put in every class, depending on the context.