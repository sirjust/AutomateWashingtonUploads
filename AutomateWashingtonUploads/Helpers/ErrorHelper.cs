using OpenQA.Selenium;

namespace AutomateWashingtonUploads.Helpers
{
    public class ErrorHelper : IErrorHelper
    {
        public bool HasInvalidLicense(string text)
        {
            if (text == "No Licenses Found.") return true;
            return false;
        }

        public bool CourseOutOfDateRange(string text)
        {
            if (text == "The Completion Date is out of the class range.") return true;
            return false;
        }

        public bool HasAlreadyUsedCourse(string text)
        {
            if (text.Contains("has already used this class for this renewal.")) return true;
            return false;
        }

        public bool LienseAlreadyOnRoster(string text)
        {
            if (text.Contains("is already on this roster.")) return true;
            return false;
        }

        public bool CourseNumberNotFound(string text)
        {
            if (text == "Invalid course identifier specified") return true;
            return false;
        }
    }
}
