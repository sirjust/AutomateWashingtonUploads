using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads.Helpers
{
    public class ErrorHelper : IErrorHelper
    {
        IWebDriver _driver;
        public ErrorHelper(IWebDriver driver)
        {
            _driver = driver;
        }
        public bool HasInvalidLicense()
        {
            try
            {
                var text = _driver.FindElement(By.Id("lblError")).GetAttribute("innerText");
                if (text == "No Licenses Found.") return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool CourseOutOfDateRange()
        {
            try
            {
                var text = _driver.FindElement(By.Id("lblError")).GetAttribute("innerText");
                if (text == "The Completion Date is out of the class range.") return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool HasAlreadyUsedCourse()
        {
            throw new NotImplementedException();
        }

        public bool LienseAlreadyOnRoster()
        {
            throw new NotImplementedException();
        }

        public bool CourseNumberNotFound()
        {
            try
            {
                var text = _driver.FindElement(By.Id("lblError")).GetAttribute("innerText");
                if (text == "Invalid course identifier specified") return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
