using System;
using System.IO;

namespace AutomateWashingtonUploads
{
    public interface ILogger
    {
        string StreamLocation { get; set; }

        StreamReader GetReader();
        StreamWriter GetWriter();
        void WriteToLog(string logMessage, TextWriter sw);
        void LogException(Exception ex, Completion completion, string message = "");
        void LogException(string message = "");
        void LogLicenseChange(string oldLicense, string newLicense);
        void LogSuccess(Completion completion);
    }
}