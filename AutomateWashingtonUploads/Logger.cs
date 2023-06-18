using System;
using System.IO;

namespace AutomateWashingtonUploads
{
    public class Logger : ILogger
    {
        public string StreamLocation { get; set; } = @".\Logs\log_" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString() + "-" + DateTime.Today.Year.ToString() + ".txt";
        public void WriteToLog(string logMessage, TextWriter sw)
        {
            sw.Write("\r\nLog Entry : ");
            sw.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            sw.WriteLine($"{logMessage}");
            sw.Close();
        }

        public void LogException(Exception ex, Completion completion, string message = "")
        {
            string errorInfo = $"The following completion encountered a(n) {ex.GetType()} error:\r\n{completion.Course} | {completion.Date} | {completion.License} | {completion.Name}\r\nDetails: {ex.InnerException?.Message}\r\n{message}\r\n-------------------------------";
            var sw = GetWriter();
            WriteToLog(errorInfo, sw);
            Console.WriteLine($"\n{errorInfo}");
            sw.Close();
        }

        public void LogException(string message)
        {
            string errorInfo = $"{message}\n-------------------------------";
            var sw = GetWriter();
            WriteToLog(errorInfo, sw);
            Console.WriteLine($"\n{errorInfo}");
            sw.Close();
        }

        public void LogSuccess(Completion completion)
        {
            string successInfo = $"{completion.License} | {completion.Course} upload succeeded\r\n-------------------------------";
            var sw = GetWriter();
            WriteToLog(successInfo, sw);
            Console.WriteLine($"\n{successInfo}");
            sw.Close();
        }
        public void LogLicenseChange(string oldLicense, string newLicense)
        {
            string licenseChange = $"The program found license number {oldLicense} and changed it to {newLicense}\r\n------------------------";
            var sw = GetWriter();
            WriteToLog(licenseChange, sw);
            Console.WriteLine($"\n{licenseChange}");
            sw.Close();
        }

        public StreamWriter GetWriter()
        {
            return new StreamWriter(StreamLocation, true);
        }

        public StreamReader GetReader()
        {
            return new StreamReader(StreamLocation, true);
        }
    }
}
