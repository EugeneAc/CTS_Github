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

		public static readonly string WagonDirection_ToObject = "на объект";

		public static readonly string WagonDirection_FromObject = "с объекта";

		public static readonly string WagonDirection_UnknownDirection = "Неопределенно";
	}
}
