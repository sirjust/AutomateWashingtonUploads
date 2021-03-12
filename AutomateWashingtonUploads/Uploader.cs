using AutomateWashingtonUploads.Helpers;
using AutomateWashingtonUploads.StaticData;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            _logger.WriteToLog("Commencing uploads.\n", _logger.GetWriter());
            LoginToWebsite();

            foreach(Completion completion in completions)
            {
                string errorMessage = "";
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

                    bool submitted = RosterSubmitted();

                    if (submitted == false)
                    {
                        Task.Delay(3000).Wait();
                        // retry the submission button if the server doesn't respond immediately
                        submitted = RosterSubmitted();
                    }
                    if (submitted == false) throw new Exception("Roster submission button is unresponsive");
                }
                catch(Exception ex)
                {
                    try
                    {
                        var text = _driver.FindElement(By.Id("lblError")).GetAttribute("innerText");
                        if (_errorHelper.CourseOutOfDateRange(text)) errorMessage = "The Completion Date is out of the class range.";
                        else if (_errorHelper.HasInvalidLicense(text)) errorMessage = $"License number {completion.License} is invalid.";
                        else if (_errorHelper.LienseAlreadyOnRoster(text)) errorMessage = $"License number {completion.License} is already on the roster.";
                        else if (_errorHelper.HasAlreadyUsedCourse(text)) errorMessage = $"License number {completion.License} has already used course {completion.Course}.";
                        else errorMessage = "An unknown error occurred";
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
            _driver.Manage().Window.Maximize();

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

        public bool RosterSubmitted()
        {
            try
            {
                _wait.Until(d => d.GetElementAndScrollTo(By.Id("btnTransferToRoster"))).Click();

                try
                {
                    // This message will appear if clicking the transfer button failed to get a response
                    var pendingMessage = _wait.Until(d => d.FindElement(By.Id("MessageLabel")));
                    if (pendingMessage.Text.Contains("Pending Roster has not been submitted"))
                    {
                        return false;
                    }
                }
                catch
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
