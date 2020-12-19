using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.IE;
using System.Drawing;

namespace HLTrader
{
    public static class WebBrowserFacade
    {
        private static IWebDriver webDriver1 = null;

        public static void init(string browser)
        {
            //if (webDriver1 != null) return;
            webDriver1 = new ChromeDriver("ChromeDriver\\");
            webDriver1.Manage().Window.Maximize();
        }
            
        public static void setWidth(int width)
        {
            //webDriver.Manage().Window.Size = new Size(width, 1000);
        }

        public static IWebDriver getDriver
        {
            get { return webDriver1; }
        }

        public static void Goto(string url)
        {
            webDriver1.Url = url;
        }

        public static void Close()
        {
            try
            {
                webDriver1.Quit();
            }
            catch (Exception ex)
            {

            }
        }

        //extensions
        public static bool ControlExists(this IWebDriver driver, By by)
        {
            return driver.FindElements(by).Count == 0 ? false : true;
        }
        public static bool ControlDisplayed(this IWebElement element, bool displayed = true, uint timeoutInSeconds = 60)
        {
            var wait = new WebDriverWait(webDriver1, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.IgnoreExceptionTypes(typeof(Exception));
            return wait.Until(drv =>
            {
                if (!displayed && !element.Displayed || displayed && element.Displayed)
                {
                    return true;
                }
                return false;
            });
        }
        public static IWebElement IsElementExists(this By Locator, uint timeoutInSeconds = 60)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(webDriver1, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(Locator));
            }
            catch
            {
                return null;
            }
        }
        public static bool ElementlIsClickable(this IWebElement element, uint timeoutInSeconds = 60, bool displayed = true)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(webDriver1, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv =>
                {
                    if (SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element) != null)
                        return true;
                    return false;
                });
            }
            catch
            {
                return false;
            }
        }
        public static void ClickWrapper(this IWebElement element, string elementName)
        {
            if (element.ElementlIsClickable())
            {
                element.Click();
            }
            else
            {
                throw new Exception(string.Format("[{0}] - Element [{1}] is not displayed", DateTime.Now.ToString("HH:mm:ss.fff"), elementName));
            }
        }
        public static void SendKeysWrapper(this IWebElement element, string value, string elementName)
        {
            Console.WriteLine(string.Format("[{0}] - SendKeys value [{1}] to  element [{2}]", DateTime.Now.ToString("HH:mm:ss.fff"), value, elementName));
            try
            {
                element.SendKeys(value);
            }
            catch
            {
                element = NullWebElement.NULL;
            }
            
        }

        public static void MoveToElementWrapper(this IWebElement element, string elementName)
        {
            Console.WriteLine(string.Format("[{0}] - Move to element [{1}]", DateTime.Now.ToString("HH:mm:ss.fff"), elementName));
            try
            {
                Actions action = new Actions(getDriver);
                action.MoveToElement(element).Perform();
            }
            catch
            {
                element = NullWebElement.NULL;
            }

        }

        public static void DoubleClickActionWrapper(this IWebElement element, string elementName)
        {
            Actions ClickButton = new Actions(webDriver1);
            ClickButton.MoveToElement(element).DoubleClick().Build().Perform();
            Console.WriteLine("[{0}] - Double Click on element [{1}]", DateTime.Now.ToString("HH:mm:ss.fff"), elementName);
        }
        public static void ClearWrapper(this IWebElement element, string elementName)
        {
            Console.WriteLine("[{0}] - Clear element [{1}] content", DateTime.Now.ToString("HH:mm:ss.fff"), elementName);
            element.Clear();
            Assert.AreEqual(element.Text, string.Empty, "Element is not cleared");
        }
        public static void CheckboxWrapper(this IWebElement element, bool value, string elementName)
        {
            Console.WriteLine("[{0}] - Set value of checkbox [{1}] to [{2}]", DateTime.Now.ToString("HH:mm:ss.fff"), elementName, value.ToString());
            if ((!element.Selected && value == true) || (element.Selected && value == false))
            {
                element.Click();
            }
        }

        public static string Title
        {
            get { return ""; }
        }
    }
}
