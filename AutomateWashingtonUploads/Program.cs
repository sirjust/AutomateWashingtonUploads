using AutomateWashingtonUploads.Dependency;
using AutomateWashingtonUploads.Helpers;
using AutomateWashingtonUploads.StaticData;
using Ninject;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AutomateWashingtonUploads
{
    class Program
    {
        static void Main()
        {
            var kernel = new StandardKernel(new DependencyContainer());

            // take user input and convert to a string list
            Console.WriteLine("Please input completion data, then press Enter twice: ");
            List<string> convertedList = DataHelper.ConvertDataToStringList();

            // convert string list to completion list which can be used by the upload task
            var finishedList = DataHelper.ListToCompletionList(convertedList);

            // send sanitized data to uploader, iterate and upload each entry
            kernel.Get<IUploader>().InputCompletions(finishedList);

            // now we will send an email with the log file
            EmailHelper.SendEmail(Logger.GetReader(), kernel.Get<ILoginInfo>());

            // the log file is located in the bin/debug folder, it is called log.txt
            Console.WriteLine("\nYour uploads are complete. Please check the log file for any errors.");
            Console.Read();
        }
    }
}
