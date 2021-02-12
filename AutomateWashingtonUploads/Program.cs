using AutomateWashingtonUploads.Data;
using AutomateWashingtonUploads.Dependency;
using AutomateWashingtonUploads.Helpers;
using Ninject;
using System;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads
{
    class Program
    {
        static async Task Main()
        {
            var kernel = new StandardKernel(new DependencyContainer());
            var completionRepository = kernel.Get<ICompletionRepository>();

            // take user input and convert to a string list
            Console.WriteLine("Please input completion data, then press Enter twice: ");
            var convertedList = DataHelper.ConvertDataToStringList();

            // convert string list to completion list which can be used by the upload task
            completionRepository.Completions = DataHelper.ListToCompletionList(convertedList);

            // send sanitized data to uploader, iterate and upload each entry
            kernel.Get<IUploader>().InputCompletions(completionRepository.Completions);

            // now we will send an email with the log file
            await kernel.Get<IEmailHelper>().SendEmail();

            // the log file is located in the bin/debug folder, it is called log.txt
            Console.WriteLine("\nYour uploads are complete. Please check the log file for any errors.");
            Console.Read();
        }
    }
}
