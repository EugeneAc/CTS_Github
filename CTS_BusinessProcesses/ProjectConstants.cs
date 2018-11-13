using System;
using System.Configuration;
using System.IO;

namespace CTS_Core
{
	public class ProjectConstants
	{
		public static string SkipActFolderPath
		{
			get
			{
#if DEBUG
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"/App_data/SkipData");
#endif
				return ConfigurationManager.AppSettings["SkipActPath"];
			}
		}
	}
}
