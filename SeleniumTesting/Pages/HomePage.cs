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
        //IWebElement lnkLogout => driver.FindElement(By.LinkText("Logout"));
        //IWebElement tokenInput = driver.FindElement(By.Name("__RequestVerificationToken"));

        IWebElement newInquiryBtn => driver.FindElement(By.LinkText("Create New Inquiry"));

        IWebElement RegisterBtn => driver.FindElement(By.LinkText("Register"));
        //public void ClickNewInquiry() => newInquiryBtn.Click();
        public void ClickLogin() => lnkLogin.Click();

        public void ClickLogout()
        {
            try
            {
                var lnkLogout = driver.FindElement(By.CssSelector("button[class^='nav-link btn btn-link text-dark border-0'][type='submit']"));
                lnkLogout.Click();
            }
            catch (NoSuchElementException)
            {
                return;  // "Logout" link not found, indicating not logged in
            }
        }

        public void ClickRegister() => RegisterBtn.Click();
        public bool IsLogOutExist()
        {
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            //var div = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"logout\"]")));
            //string tokenValue = tokenInput.GetAttribute("value");

            var element = driver.FindElement(By.CssSelector("button[class^='nav-link btn btn-link text-dark border-0'][type='submit']"));
            return element.Displayed;
            //IWebElement lnkLogout = driver.FindElement(By.LinkText("Logout"));
            //return lnkLogout != null && lnkLogout.Displayed;
        }
        public bool IsLogInExist()
        {
            var element = driver.FindElement(By.CssSelector("#login"));
            return element.Displayed;
        }
        //public bool IsLoggedIn()
        //{
        //    try
        //    {
        //        string tokenValue = tokenInput.GetAttribute("value");
        //        return lnkLogout.Displayed;
        //    }
        //    catch (NoSuchElementException)
        //    {
        //        return false;
        //    }
        //}
    }
}
