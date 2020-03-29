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

        public Uploader(IWebDriver driver, ILoginInfo loginInfo)
        {
            _driver = driver;
            _loginInfo = loginInfo;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void InputCompletions(List<Completion> completions)
        {
            LoginToWebsite();

            foreach(Completion completion in completions)
            {
                // from here we loop through each completion
                string courseNumber = completion.Course;
                string license = completion.License;
                DateTime.TryParse(completion.Date, out DateTime completionDate);

                // check length of license
                try
                {
                    if (!ValidationHelper.IsLicenseTwelveCharacters(license))
                    {
                        throw new Exception("", new Exception("The license is an incorrect length."));
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, completion);
                    // we are on the correct page so we can simply continue
                    continue;
                }
                // check if the user has put in a false value for the second to last character
                if (license[10] == '0')
                {
                    license = ValidationHelper.ChangeSecondToLastCharacter(license);
                }

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
                    Logger.LogException(ex, completion);
                    //then go back to the previous page
                    IWebElement goBack = _driver.FindElement(By.Id("btnPrev"));
                    goBack.Click();
                    continue;
                }

                // if the completion date is incorrect for the course the program will log it and go to the next completion
                try
                {
                    _wait.Until(d=> d.FindElement(By.Id("txtComplDt"))).SendKeys(string.Format("{0:MM/dd/yyyy}", completionDate));
                }
                catch(Exception ex)
                {
                    Logger.LogException(ex, completion);
                    //then go back to the previous page
                    _driver.FindElement(By.Id("btnPrev")).Click();
                    continue;
                }

                try
                {
                    // here we create a roster and find the license
                    _wait.Until(d => d.FindElement(By.Id("btnGetRoster"))).Click();
                    _wait.Until(d=> d.FindElement(By.Id("txtLicense"))).SendKeys(license);
                    _wait.Until(d=> d.FindElement(By.Id("btnPeople"))).Click();

                    // next we have to submit the roster
                    _wait.Until(d=> d.FindElement(By.Id("btnTransferToRoster"))).Click();
                }
                catch(Exception ex)
                {
                    Logger.LogException(ex, completion);
                }
                finally
                {
                    //then go back to the previous page
                    _wait.Until(d=> d.FindElement(By.Id("btnPrev"))).Click();
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

            _driver.FindElement(By.Id("username")).SendKeys(_loginInfo.Id);
            _driver.FindElement(By.Id("password")).SendKeys(_loginInfo.Password);

            try
            {
                _wait.Until(d => d.FindElement(By.XPath("//input[@value='SUBMIT']"))).Click();
                _wait.Until(d => d.FindElement(By.XPath("//div[@class='table-row table-row-odd']//input[@value='ACCESS']"))).Click();
            }
            catch
            {
                _wait.Until(d => d.FindElement(By.XPath("//a[contains(text(),'Trades Education Roster Reporting System')]"))).Click();
            }

            IWebElement keepGoing = _wait.Until(d => d.FindElement(By.XPath("//input[@value='CONTINUE']")));
            keepGoing.Click();
        }
    }
}

