using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTesting.Pages
{
    public class HomePage : DriverHelper
    {
        IWebElement lnkLogin => driver.FindElement(By.LinkText("Login"));
        IWebElement lnkLogout => driver.FindElement(By.CssSelector("button[class^='nav-link btn btn-link text-dark border-0'][type='submit']"));
        IWebElement newInquiryBtn => driver.FindElement(By.LinkText("Create New Inquiry"));
        IWebElement RegisterBtn => driver.FindElement(By.LinkText("Register"));

        public void ClickNewInquiry() => newInquiryBtn.Click();
        public void ClickLogin() => lnkLogin.Click();

        public void ClickLogout()
        {
            //var lnkLogout = driver.FindElement(By.CssSelector("button[class^='nav-link btn btn-link text-dark border-0'][type='submit']"));
            lnkLogout.Click();
        }

        public void ClickRegister() => RegisterBtn.Click();
        public bool IsLogOutExist()
        {
            //var element = driver.FindElement(By.CssSelector("button[class^='nav-link btn btn-link text-dark border-0'][type='submit']"));
            return lnkLogout.Displayed;
        }
        public bool IsLogInExist()
        {
            var element = driver.FindElement(By.CssSelector("#login"));
            return element.Displayed;
        }
    }
}
