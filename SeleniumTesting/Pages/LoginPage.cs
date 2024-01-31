using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTesting.Pages
{
    public class LoginPage : DriverHelper
    {
        IWebElement txtEmail => driver.FindElement(By.Name("Input.Email"));
        IWebElement txtPasswd => driver.FindElement(By.Name("Input.Password"));
        IWebElement btnLogin => driver.FindElement(By.Id("login-submit"));

        IWebElement ForgotPassword => driver.FindElement(By.Id("forgot-password"));
        IWebElement resendConfirmation => driver.FindElement(By.Id("resend-confirmation"));
        public void EnterUserEmailAndPassword(string email, string password)
        {
            txtEmail.Clear();
            txtPasswd.Clear();
            txtEmail.SendKeys(email);
            txtPasswd.SendKeys(password);
        }
        public void ClickForgotPassword()
        {
            ForgotPassword.Click();
        }
        public void ClickResentConfirmation()
        {
            resendConfirmation.Click();
        }

        public void ClickLogin()
        {
            btnLogin.Click();
        }
    }
}
