using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTesting.Pages
{
    public class RegisterPage : DriverHelper
    {
        IWebElement txtEmail => driver.FindElement(By.Name("Input.Email"));
        IWebElement txtPasswd => driver.FindElement(By.Id("Input_Password"));
        IWebElement txtPasswdConfirm => driver.FindElement(By.Id("Input_ConfirmPassword"));
        IWebElement btnRegister => driver.FindElement(By.Id("registerSubmit"));
        IWebElement selectElement => driver.FindElement(By.Id("Input_Role"));

        public void EnterUserEmailAndPassword(string email, string password, bool IsValisPassword)
        {
            txtEmail.Clear();
            txtPasswd.Clear();
            txtEmail.SendKeys(email);
            txtPasswd.SendKeys(password);
            if(IsValisPassword) txtPasswdConfirm.SendKeys(password);
            else txtPasswdConfirm.SendKeys(password + "A");
        }
        public void SelectRole(string role)
        {
            SelectElement select = new SelectElement(selectElement);
            select.SelectByText(role);
            //IWebElement selectedOption = select.SelectedOption;
        }
        public void ClickRegister() => btnRegister.Click();

    }
}
