using OpenQA.Selenium;
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
        IWebElement lnkLogOut => driver.FindElement(By.LinkText("Logout"));
        public void ClickLogin() => lnkLogin.Click();

        public bool IsLogOutExist() => lnkLogOut.Displayed;
    }
}
