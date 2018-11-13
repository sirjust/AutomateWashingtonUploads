using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads
{
    public class Logger
    {

        public void writeErrorsToLog(string logMessage, TextWriter sw)
        {
            sw.Write("\r\nLog Entry : ");
            sw.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            sw.WriteLine("  :{0}", logMessage);
            sw.WriteLine("-------------------------------");
        }
    }
}
