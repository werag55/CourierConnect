using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
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

            ChromeOptions options = new ChromeOptions();
            //options.EnableMobileEmulation(deviceName);
            options.AddArgument("no-sandbox");
            driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
            driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));
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
            Assert.That(homePage.IsLogOutExist(), Is.True);
        }

        [Test]
        public void Logout()
        {
            driver.Navigate().GoToUrl("https://connectcourier.azurewebsites.net/");
            HomePage homePage = new HomePage();
            LoginPage loginPage = new LoginPage();

            homePage.ClickLogin();
            loginPage.EnterUserEmailAndPassword("alinkamalinka@gmail.com", "Alinka12!");
            loginPage.ClickLogin();
            homePage.ClickLogout();
            Assert.That(homePage.IsLogInExist(), Is.True);
        }

        //[Test]
        //public void RegisterNewUser()
        //{
        //    driver.Navigate().GoToUrl("https://connectcourier.azurewebsites.net/");
        //    HomePage homePage = new HomePage();
        //    RegisterPage registerPage = new RegisterPage();

        //    homePage.ClickRegister();
        //    registerPage.EnterUserEmailAndPassword(GenerateRandomEmail("user"), "User1234!", true);
        //    registerPage.SelectRole("Client");
        //    registerPage.ClickRegister();
        //    Assert.That(homePage.IsLogOutExist(), Is.True);
        //}

        //static string GenerateRandomEmail(string prefix)
        //{
        //    string randomString = Guid.NewGuid().ToString("N").Substring(0, 8);
        //    string email = $"{prefix}_{randomString}@gmail.com";

        //    return email;
        //}
    }
}
