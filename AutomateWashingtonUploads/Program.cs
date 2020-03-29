using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using AutomateWashingtonUploads.StaticData;
using AutomateWashingtonUploads.Helpers;
using Ninject;
using AutomateWashingtonUploads.Dependency;

namespace AutomateWashingtonUploads
{
    class Program
    {
        static void Main()
        {
            var kernel = new StandardKernel(new DependencyContainer());
            var loginInfo = kernel.Get<ILoginInfo>();

            // take user input and convert to a string list
            Console.WriteLine("Please input completion data, then press Enter twice: ");
            List<string> convertedList = DataHelper.ConvertDataToStringList();

            // convert string list to completion list which can be used by the upload task
            var finishedList = DataHelper.ListToCompletionList(convertedList);

            // send sanitized data to uploader, iterate and upload each entry
            Uploader uploader = new Uploader(new FirefoxDriver(@"../../../packages/Selenium.Firefox.WebDriver.0.24.0/driver/"), loginInfo);
            uploader.InputCompletions(finishedList);

            // now we will send an email with the log file
            // Helper.SendEmail(Logger.GetReader(), loginInfo);

            // the log file is located in the bin/debug folder, it is called log.txt
            Console.WriteLine("\nYour uploads are complete. Please check the log file for any errors.");
            Console.Read();
        }
    }
}
