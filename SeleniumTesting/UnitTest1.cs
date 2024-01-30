using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace SeleniumTesting
{
    public class Tests
    {
        public IWebDriver Driver;
        [SetUp]
        public void Setup()
        {
            Driver = new FirefoxDriver();
        }

        [Test]
        public void Test1()
        {
            Driver.Navigate().GoToUrl("https://connectcourier.azurewebsites.net/");
            Driver.FindElement(By.Id("login")).Click();

            Assert.Pass();
        }
    }
}