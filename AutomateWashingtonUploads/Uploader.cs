using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;

namespace AutomateWashingtonUploads
{
    public class Uploader
    {
        IWebDriver _driver;
        readonly string _location = @"../../../packages/Selenium.Firefox.WebDriver.0.24.0/driver/";
        readonly string _url = "https://secureaccess.wa.gov/myAccess/saw/select.do";
        readonly LoginInfo _loginInfo;

        public Uploader(IWebDriver driver, LoginInfo loginInfo)
        {
            _driver = driver;
            _loginInfo = loginInfo;
        }

        public void InputCompletions(List<Completion> completions)
        {
            LoginToWebsite();
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            try
            {
                IWebElement loginButton = wait.Until(d => d.FindElement(By.XPath("//input[@value='SUBMIT']")));
                loginButton.Click();
                IWebElement accessButton = wait.Until(d => d.FindElement(By.XPath("//div[@class='table-row table-row-odd']//input[@value='ACCESS']")));
                accessButton.Click();
            } catch
            {
                IWebElement tradeReporting = wait.Until(d => d.FindElement(By.XPath("//a[contains(text(),'Trades Education Roster Reporting System')]")));
                tradeReporting.Click();
            }

            IWebElement keepGoing = wait.Until(d=> d.FindElement(By.XPath("//input[@value='CONTINUE']")));
            keepGoing.Click();

            foreach(Completion completion in completions)
            {
                // from here we loop through each completion
                string courseNumber = completion.course;
                string license = completion.license;
                // check if the user has put in a false value for the second to last character
                if(license[10] == '0')
                {
                    license = Helper.ChangeSecondToLastCharacter(license);
                }
                string dateString = completion.date;
                string[] splitUpDate = dateString.Split('-');
                int year = int.Parse(splitUpDate[0]);
                int month = int.Parse(splitUpDate[1]);
                int day = int.Parse(splitUpDate[2]);
                int numberOfDownClicks = 0;
                DateTime completionDate = new DateTime(year, month, day);

                if (PlumbingCourses.Old_New_Courses.ContainsKey(courseNumber))
                {
                    courseNumber = PlumbingCourses.Old_New_Courses[courseNumber];
                }

                // if the plumbing courses array has a value that matches completion.course, click down 10 times, otherwise it is
                // an electrical course, and the variable is 2
                if (PlumbingCourses.WAPlumbingCourses.Contains(courseNumber))
                {
                    numberOfDownClicks = 10;
                }
                else
                {
                    numberOfDownClicks = 2;
                }

                // if the course isn't in the plumbing array or an electrical course, it will be handled by the catch block
                try
                {
                    IWebElement tradeContainer = wait.Until(d => d.FindElement(By.Id("ddlCourseType")));
                    while (numberOfDownClicks > 0)
                    {
                        tradeContainer.SendKeys(Keys.Down);
                        numberOfDownClicks--;
                    }

                    IWebElement courseField = wait.Until(d=>d.FindElement(By.Id("txtClassID")));
                    courseField.SendKeys(courseNumber);

                    IWebElement btnNext = wait.Until(d=>d.FindElement(By.Id("btnNext")));
                    btnNext.Click();

                    IWebElement anchor = wait.Until<IWebElement>(d => d.FindElement(By.PartialLinkText("HVAC")));
                    anchor.Click();
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
                // this occurs with older electrical courses, where the completion date is out of the range of course validity
                try
                {
                    IWebElement dateInput = wait.Until(d=> d.FindElement(By.Id("txtComplDt")));
                    dateInput.SendKeys(String.Format("{0:MM/dd/yyyy}", completionDate));
                }
                catch(Exception ex)
                {
                    Logger.LogException(ex, completion);
                    //then go back to the previous page
                    IWebElement goBack = _driver.FindElement(By.Id("btnPrev"));
                    goBack.Click();
                    continue;
                }

                try
                {
                    IWebElement createRoster = wait.Until(d => d.FindElement(By.Id("btnGetRoster")));
                    createRoster.Click();
                    IWebElement inputLicense = wait.Until(d=> d.FindElement(By.Id("txtLicense")));
                    inputLicense.SendKeys(license);
                    IWebElement findLicensee = wait.Until(d=> d.FindElement(By.Id("btnPeople")));
                    findLicensee.Click();

                    // next we have to submit the roster
                    IWebElement addToRoster = wait.Until<IWebElement>(d=> d.FindElement(By.Id("btnTransferToRoster")));
                    addToRoster.Click();
                }
                catch(Exception ex)
                {
                    Logger.LogException(ex, completion);
                }
                finally
                {
                    //then go back to the previous page
                    IWebElement goBack = wait.Until(d=> d.FindElement(By.Id("btnPrev")));
                    goBack.Click();
                }
                //loop again until the end
            }
            // I initially had the driver close, but found it more useful to have it remain open so we can fix any errors without loading a new page
            //driver.Close();
        }

        private void LoginToWebsite()
        {
            _driver = new FirefoxDriver(_location)
            {
                Url = _url
            };
            _driver.Manage().Window.Maximize();

            IWebElement usernameInput = _driver.FindElement(By.Id("username"));
            IWebElement passwordInput = _driver.FindElement(By.Id("password"));
            usernameInput.SendKeys(_loginInfo.id);
            passwordInput.SendKeys(_loginInfo.password);
        }
    }
}

