using OpenQA.Selenium.Chrome;
using SeleniumTesting.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SeleniumTesting.Specflow.Steps
{
    [Binding]
    public class LoginSteps : DriverHelper
    {
        HomePage homePage;
        LoginPage loginPage;

        [BeforeScenario]
        public void BeforeScenario()
        {
            driver = new ChromeDriver();
            homePage = new HomePage();
            loginPage = new LoginPage();
        }

        [Given(@"I navigate to application")]
        public void GivenINavigateToApplication()
        {
            driver.Navigate().GoToUrl("https://connectcourier.azurewebsites.net/");
        }

        [Given(@"I click the Login link")]
        public void GivenIClickTheLoginLink()
        {
            homePage.ClickLogin();
        }

        [Given(@"I enter username and password")]
        public void GivenIEnterUsernameAndPassword(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            loginPage.EnterUserEmailAndPassword(data.UserName, data.Password);
        }

        [Given(@"I click login")]
        public void GivenIClickLogin()
        {
            loginPage.ClickLogin();
        }

        [Then((@"I should see user logged in to the application"))]
        public void ThenIShouldSeeUserLoggedInToTheApplication()
        {
            Assert.That(homePage.IsLogOutExist(), Is.True);
        }
    }
}
