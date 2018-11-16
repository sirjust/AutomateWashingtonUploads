using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Support.UI;

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
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement tradeReporting = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//a[contains(text(),'Trades Education Roster Reporting System')]")));
            tradeReporting.Click();

            IWebElement keepGoing = wait.Until<IWebElement>(d=> d.FindElement(By.XPath("//input[@value='CONTINUE']")));
            keepGoing.Click();

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

                IWebElement tradeContainer = wait.Until<IWebElement>(d=> d.FindElement(By.Id("ddlCourseType")));
                // for electricians this variable should be 2
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

                IWebElement anchor = wait.Until<IWebElement>(d=> d.FindElement(By.PartialLinkText("HVAC")));
                anchor.Click();

                // if the completion date is incorrect for the course the program will log it and go to the next completion
                try
                {
                    IWebElement dateInput = wait.Until<IWebElement>(d=> d.FindElement(By.Id("txtComplDt")));
                    dateInput.SendKeys(String.Format("{0:MM/dd/yyyy}", completionDate));
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

                IWebElement createRoster = wait.Until<IWebElement>(d=> d.FindElement(By.Id("btnGetRoster")));
                createRoster.Click();

                try
                {
                    IWebElement inputLicense = wait.Until<IWebElement>(d=> d.FindElement(By.Id("txtLicense")));
                    inputLicense.SendKeys(license);
                    IWebElement findLicensee = wait.Until<IWebElement>(d=> d.FindElement(By.Id("btnPeople")));
                    findLicensee.Click();

                    // next we have to submit the roster
                    IWebElement addToRoster = wait.Until<IWebElement>(d=> d.FindElement(By.Id("btnTransferToRoster")));
                    addToRoster.Click();
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
                    IWebElement goBack = wait.Until<IWebElement>(d=> d.FindElement(By.Id("btnPrev")));
                    goBack.Click();
                }
                //loop again until the end
            }
            driver.Close();
        }
    }
}

