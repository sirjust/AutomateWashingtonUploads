using OpenQA.Selenium;

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
            try
            {
                var text = _driver.FindElement(By.Id("lblError")).GetAttribute("innerText");
                if (text.Contains("has already used this class for this renewal.")) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool LienseAlreadyOnRoster()
        {
            try
            {
                var text = _driver.FindElement(By.Id("lblError")).GetAttribute("innerText");
                if (text.Contains("is already on this roster.")) return true;
                return false;
            }
            catch
            {
                return false;
            }
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
