using System;
using System.IO;

namespace AutomateWashingtonUploads
{
    public static class Logger
    {
        public static string StreamLocation { get; set; } = @"..\..\..\Logs\log_" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString() + "-" + DateTime.Today.Year.ToString() + ".txt";
        private static void WriteErrorsToLog(string logMessage, TextWriter sw)
        {
            sw.Write("\r\nLog Entry : ");
            sw.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            sw.WriteLine($"{logMessage}");
        }

        public static void LogException(Exception ex, Completion completion, string message = "")
        {
            string errorInfo = ($"The following completion encountered a(n) {ex.GetType()} error:\r\n{completion.Course} | {completion.Date} | {completion.License} | {completion.Name}\r\nDetails: {ex.InnerException.Message}\r\n{message}\r\n-------------------------------");
            var sw = GetWriter();
            WriteErrorsToLog(errorInfo, sw);
            Console.WriteLine($"\n{errorInfo}");
            sw.Close();
        }

        public static void LogLicenseChange(string oldLicense, string newLicense)
        {
            string licenseChange = $"The program found license number {oldLicense} and changed it to {newLicense}";
            var sw = GetWriter();
            WriteErrorsToLog(licenseChange, sw);
            Console.WriteLine($"\n{licenseChange}\r\n------------------------");
            sw.Close();
        }

        public static StreamWriter GetWriter()
        {
            return new StreamWriter(StreamLocation, true);
        }

        public static StreamReader GetReader()
        {
            return new StreamReader(StreamLocation, true);
        }
    }
}
