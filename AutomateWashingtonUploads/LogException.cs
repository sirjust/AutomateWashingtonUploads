using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads
{
    public class LogException
    {
        public void logException(Exception ex, Completion completion)
        {
            string errorInfo = String.Format("The following completion encountered an{0} error:\r\n{1} | {2} | {3} | {4}", ex.GetType().ToString(), completion.course, completion.date, completion.license, completion.name);
            Logger logger = new Logger();
            StreamWriter sw = new StreamWriter(@"..\..\..\Logs\log_" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString() + "-" + DateTime.Today.Year.ToString() + ".txt", true);
            logger.writeErrorsToLog(errorInfo, sw);
            Console.WriteLine(errorInfo);
            sw.Close();
        }
    }
}
