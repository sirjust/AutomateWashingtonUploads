using AutomateWashingtonUploads.Helpers;
using AutomateWashingtonUploads.StaticData;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomateWashingtonUploads
{
    public class Uploader : IUploader
    {
        IWebDriver _driver;
        ILoginInfo _loginInfo;
        WebDriverWait _wait;
        IErrorHelper _errorHelper;
        ILogger _logger;
        IValidationHelper _validationHelper;

        public Uploader(IWebDriver driver, ILoginInfo loginInfo, IErrorHelper errorHelper, IValidationHelper validationHelper, ILogger logger)
        {
            _driver = driver;
            _loginInfo = loginInfo;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _errorHelper = errorHelper;
            _validationHelper = validationHelper;
            _logger = logger;
        }

        public void InputCompletions(IEnumerable<Completion> completions)
        {
            string errorMessage = "";
            _logger.WriteToLog("Commencing uploads.\n", _logger.GetWriter());
            LoginToWebsite();

            foreach(Completion completion in completions)
            {
                // from here we loop through each completion
                string courseNumber = completion.Course;
                DateTime.TryParse(completion.Date, out DateTime completionDate);

                // check length of license
                try
                {
                    if (!ValidationHelper.IsLicenseTwelveCharacters(completion.License))
                    {
                        throw new Exception("", new Exception("The license is an incorrect length."));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex, completion);
                    // we are on the correct page so we can simply continue
                    continue;
                }
                // check if the user has put in a false value for the second to last character
                completion.License = _validationHelper.CheckForZero(completion.License);

                if (PlumbingCourses.Old_New_Courses.ContainsKey(courseNumber))
                {
                    courseNumber = PlumbingCourses.Old_New_Courses[courseNumber];
                }

                // if the plumbing courses array has a value that matches completion.course, click down 10 times, otherwise it is an electrical course, and the variable is 2
                int numberOfDownClicks = PlumbingCourses.WAPlumbingCourses.Contains(courseNumber) ? 10 : 2;

                // if the course isn't in the plumbing array or an electrical course, it will be handled by the catch block
                try
                {
                    IWebElement tradeContainer = _wait.Until(d => d.FindElement(By.Id("ddlCourseType")));
                    while (numberOfDownClicks > 0)
                    {
                        tradeContainer.SendKeys(Keys.Down);
                        numberOfDownClicks--;
                    }

                    _wait.Until(d=>d.FindElement(By.Id("txtClassID"))).SendKeys(courseNumber);
                    _wait.Until(d=>d.FindElement(By.Id("btnNext"))).Click();
                    _wait.Until(d => d.FindElement(By.PartialLinkText("HVAC"))).Click();
                }
                catch(Exception ex)
                {
                    var text = _driver.FindElement(By.Id("lblError")).GetAttribute("innerText");
                    if (_errorHelper.CourseNumberNotFound(text))
                    {
                        errorMessage = $"The following course was not found: {completion.Course}";
                    }
                    _logger.LogException(ex, completion, errorMessage);
                    _driver.Navigate().GoToUrl(_driver.Url);
                    continue;
                }

                // if the course is not found the program will log it and go to the next completion
                try
                {
                    _wait.Until(d=> d.FindElement(By.Id("txtComplDt"))).SendKeys(string.Format("{0:MM/dd/yyyy}", completionDate));
                }
                catch(Exception ex)
                {
                    _logger.LogException(ex, completion, errorMessage);
                    //then go back to the previous page
                    _driver.FindElement(By.Id("btnPrev")).Click();
                    continue;
                }

                try
                {
                    // here we create a roster and find the license
                    _wait.Until(d => d.FindElement(By.Id("btnGetRoster"))).Click();
                    _wait.Until(d=> d.FindElement(By.Id("txtLicense"))).SendKeys(completion.License);
                    _wait.Until(d=> d.FindElement(By.Id("btnPeople"))).Click();

                    // next we have to submit the roster
                    _wait.Until(d=> d.FindElement(By.Id("btnTransferToRoster"))).Click();

                    // _logger.LogSuccess(completion);
                }
                catch(Exception ex)
                {
                    try
                    {
                        var text = _driver.FindElement(By.Id("lblError")).GetAttribute("innerText");
                        if (_errorHelper.CourseOutOfDateRange(text)) errorMessage = "The Completion Date is out of the class range.";
                        if (_errorHelper.HasInvalidLicense(text)) errorMessage = $"License number {completion.License} is invalid.";
                        if (_errorHelper.LienseAlreadyOnRoster(text)) errorMessage = $"License number {completion.License} is already on the roster.";
                        if (_errorHelper.HasAlreadyUsedCourse(text)) errorMessage = $"License number {completion.License} has already used course {completion.Course}.";
                    }
                    finally
                    {
                        _logger.LogException(ex, completion, errorMessage);
                    }
                }
                finally
                {
                    try
                    {
                        _wait.Until(d => d.FindElement(By.Id("btnPrev"))).Click();
                    }
                    catch(Exception ex)
                    {
                        _logger.LogException(ex, completion, errorMessage);
                        Console.WriteLine("The program has stopped for an unknown reason. Please contact support.");
                    }
                    //then go back to the previous page
                }
                //loop again until the end
            }
            // I initially had the driver close, but found it more useful to have it remain open so we can fix any errors without loading a new page
            //driver.Close();
        }

        public void LoginToWebsite()
        {
            _driver.Url = _loginInfo.LoginUrl;
            // _driver.Manage().Window.Maximize();

            //_driver.FindElement(By.Id("username")).SendKeys(_loginInfo.Id);
            //_driver.FindElement(By.Id("password")).SendKeys(_loginInfo.Password);

            try
            {
                //_wait.Until(d => d.FindElement(By.XPath("//input[@value='SUBMIT']"))).Click();
                _driver.Url = @"https://secureaccess.wa.gov/myAccess/saw/leaving/display.do?agency=LNI&service=terrs";
                _wait.Until(d => d.FindElement(By.XPath("//input[@value='CONTINUE']"))).Click();
            }
            catch(Exception ex)
            {
                _logger.LogException(ex, new Completion(), "There was an error logging in");
                throw;
            }
        }
    }
}
