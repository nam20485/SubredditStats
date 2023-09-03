using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Frontend.ConsoleClient
{
    internal class Logger
    {
        public const string DefaultLogFile = "log.txt";

        public string LogFile { get; }

        public Logger(string logFile)
        {
            LogFile = logFile;
        }

        public Logger()
            : this(DefaultLogFile)
        { }

        public void LogError(string message, params object[] formatParams)
        {
            var s = string.Format(message, formatParams);
            Console.Out.WriteLine(s);
            Console.Error.WriteLine(s);
        }

        public void LogException(Exception e, string message, params object[] formatParams)
        {
            message = message.Replace("{", "{{").Replace("}", "}}");
            LogError($"Exception: {e}:{Environment.NewLine}{message}", formatParams);
        }

        public void LogException(Exception e)
        {
            //LogError($"Exception: {e}:{Environment.NewLine}");
            LogException(e, "");
        }

        public void LogInfo(string message, params object[] formatParams)
        {
            Console.Out.WriteLine(string.Format(message, formatParams));            
        }
    }
}
