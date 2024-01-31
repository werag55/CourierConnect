using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
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
