using OpenQA.Selenium.Chrome;
using SeleniumTesting.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTesting.LoginTests
{
    public class LoginTest : DriverHelper
    {
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }
        [Test]
        public void Login() 
        {
            driver.Navigate().GoToUrl("https://connectcourier.azurewebsites.net/");
            HomePage homePage = new HomePage();
            LoginPage loginPage = new LoginPage();

            homePage.ClickLogin();
            loginPage.EnterUserEmailAndPassword("alinkamalinka@gmail.com", "Alinka12!");
            loginPage.ClickLogin();

            Assert.Pass();
        }
    }
}
