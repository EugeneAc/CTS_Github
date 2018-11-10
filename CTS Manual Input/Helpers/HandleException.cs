using System;
using System.IO;
using System.Text;
using System.Web;

namespace CTS_Manual_Input.Helpers
{
    public static class HandleException
    {
    public static object AppDomaiHttpContext { get; private set; }

    public static void LogException(Exception ex)
        {
            // Путь .\\Log
            string pathToLog = Path.Combine(HttpContext.Current.Request.MapPath(@"~/"), "Log");

            if (!Directory.Exists(pathToLog))
               Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно

            string filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log",
            "CTS", DateTime.Now));

            string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}.{2}()] {3}\r\n {4}\r\n",
            DateTime.Now, ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message,ex.StackTrace);

            File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
        }

        public static string GetErrorMessage(Exception ex, bool includeStackTrace)
        {
            StringBuilder msg = new StringBuilder();
            BuildErrorMessage(ex, ref msg);
            if (includeStackTrace)
            {
                msg.Append("\n");
                msg.Append(ex.StackTrace);
            }
            return msg.ToString();
        }

        private static void BuildErrorMessage(Exception ex, ref StringBuilder msg)
        {
            if (ex != null)
            {
                msg.Append(ex.Message);
                msg.Append("\n");
                if (ex.InnerException != null)
                {
                    BuildErrorMessage(ex.InnerException, ref msg);
                }
            }
        }
  }
}