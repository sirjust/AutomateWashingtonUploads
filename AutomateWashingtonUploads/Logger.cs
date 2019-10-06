using System;
using System.IO;

namespace AutomateWashingtonUploads
{
    public static class Logger
    {
        private static void WriteErrorsToLog(string logMessage, TextWriter sw)
        {
            sw.Write("\r\nLog Entry : ");
            sw.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            sw.WriteLine($"{logMessage}");
        }

        public static void LogException(Exception ex, Completion completion)
        {
            string errorInfo = ($"The following completion encountered an {ex.GetType().ToString()} error:\r\n{completion.Course} | {completion.Date} | {completion.License} | {completion.Name}\r\nDetails: {ex.InnerException.Message}\r\n-------------------------------");
            StreamWriter sw = new StreamWriter(@"..\..\..\Logs\log_" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString() + "-" + DateTime.Today.Year.ToString() + ".txt", true);
            WriteErrorsToLog(errorInfo, sw);
            Console.WriteLine($"\n{errorInfo}");
            sw.Close();
        }
    }
}
