using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS_BusinessProcesses
{
	public static class Logger
	{
		static private Object thisLock = new Object();

		static void AddSystemLog(string log)
		{
			try
			{
				if (!EventLog.SourceExists("DBSyncService"))
				{
					EventLog.CreateEventSource("DBSyncService", "DBSyncService");
				}

				EventLog myLog = new EventLog();
				myLog.Source = "DBSyncService";
				myLog.WriteEntry(log);
			}
			catch { }
		}

		static void WriteToFileLog(string text)
		{
			string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");

			lock (thisLock)
			{
				if (!Directory.Exists(pathToLog))
					Directory.CreateDirectory(pathToLog);
				string filename = Path.Combine(pathToLog, string.Format("LogFile_{0:dd.MM.yyy}.log",
				DateTime.Now));
				string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] {1} \r\n",
				DateTime.Now, text);

				File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
			}
		}

		public static void MakeLog(string text)
		{
			//AddSystemLog(text);
			WriteToFileLog(text);
		}
	}
}
