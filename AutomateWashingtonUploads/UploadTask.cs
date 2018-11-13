﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace AutomateWashingtonUploads
{
    public class UploadTask
    {
        IWebDriver driver;

        public void inputCompletions(List<Completion> completions)
        {
            driver = new ChromeDriver(@"C:\Users\SirJUST\source\repos\AutomateWashingtonUploads\packages\Selenium.Chrome.WebDriver.2.43\driver");
            driver.Url = "https://secureaccess.wa.gov/myAccess/saw/select.do";
            driver.Manage().Window.Maximize();
            LoginInfo loginInfo = new LoginInfo();
            string myUserName = loginInfo.id;
            string myPassword = loginInfo.password;
            IWebElement usernameInput = driver.FindElement(By.Id("username"));
            IWebElement passwordInput = driver.FindElement(By.Id("password"));

            usernameInput.SendKeys(myUserName);
            passwordInput.SendKeys(myPassword);

            IWebElement loginButton = driver.FindElement(By.XPath("//input[@value='SUBMIT']"));
            loginButton.Click();
            Thread.Sleep(3000);

            IWebElement tradeReporting = driver.FindElement(By.XPath("//a[contains(text(),'Trades Education Roster Reporting System')]"));
            tradeReporting.Click();
            Thread.Sleep(3000);

            IWebElement keepGoing = driver.FindElement(By.XPath("//input[@value='CONTINUE']"));
            keepGoing.Click();
            Thread.Sleep(3000);

            foreach(Completion completion in completions)
            {
                // from here we loop through each completion
                string courseNumber = completion.course;
                string license = completion.license;
                string dateString = completion.date;
                string[] splitUpDate = dateString.Split('-');
                int year = int.Parse(splitUpDate[0]);
                int month = int.Parse(splitUpDate[1]);
                int day = int.Parse(splitUpDate[2]);
                DateTime completionDate = new DateTime(year, month, day);

                IWebElement tradeContainer = driver.FindElement(By.Id("ddlCourseType"));
                int numberOfDownClicks = 10;
                while (numberOfDownClicks > 0)
                {
                    tradeContainer.SendKeys(Keys.Down);
                    numberOfDownClicks--;
                }

                IWebElement courseField = driver.FindElement(By.Id("txtClassID"));
                courseField.SendKeys(courseNumber);

                IWebElement btnNext = driver.FindElement(By.Id("btnNext"));
                btnNext.Click();
                Thread.Sleep(3000);

                IWebElement anchor = driver.FindElement(By.PartialLinkText("HVAC"));
                anchor.Click();
                Thread.Sleep(3000);

                // if the completion date is incorrect for the course the program will log it and go to the next completion
                try
                {
                    IWebElement dateInput = driver.FindElement(By.Id("txtComplDt"));
                    dateInput.SendKeys(String.Format("{0:MM/dd/yyyy}", completionDate));
                    Thread.Sleep(3000);
                }
                catch
                {
                    string errorInfo = String.Format("The following completion encountered an error: {0} | {1} | {2} | {3}", completion.course, completion.date, completion.license, completion.name);
                    Logger logger = new Logger();
                    StreamWriter sw = new StreamWriter(@"log.txt", true);
                    logger.writeErrorsToLog(errorInfo, sw);
                    sw.Close();
                    //then go back to the previous page
                    IWebElement goBack = driver.FindElement(By.Id("btnPrev"));
                    goBack.Click();
                    Thread.Sleep(3000);
                    continue;
                }

                IWebElement createRoster = driver.FindElement(By.Id("btnGetRoster"));
                createRoster.Click();
                Thread.Sleep(3000);

                try
                {
                    IWebElement inputLicense = driver.FindElement(By.Id("txtLicense"));
                    inputLicense.SendKeys(license);
                    Thread.Sleep(3000);
                    IWebElement findLicensee = driver.FindElement(By.Id("btnPeople"));
                    findLicensee.Click();
                    Thread.Sleep(3000);

                    // next we have to submit roster
                    IWebElement addToRoster = driver.FindElement(By.Id("btnTransferToRoster"));
                    addToRoster.Click();
                    Thread.Sleep(3000);
                }
                catch
                {
                    string errorInfo = String.Format("The following completion encountered an error: {0} | {1} | {2} | {3}",completion.course, completion.date, completion.license, completion.name);
                    Logger logger = new Logger();
                    StreamWriter sw = new StreamWriter(@"log.txt", true);
                    logger.writeErrorsToLog(errorInfo, sw);
                    sw.Close();
                }
                finally
                {
                    //then go back to the previous page
                    IWebElement goBack = driver.FindElement(By.Id("btnPrev"));
                    goBack.Click();
                    Thread.Sleep(3000);
                }
                //loop again until the end
            }
        }
    }
}

