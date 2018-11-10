using System;
using System.Configuration;
using System.IO;

namespace CTS_Analytics
{
    public class ProjectConstants
    {
        public static string ReportFolderPath
        {
            get
            {
#if DEBUG
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"/App_data/TestReports");
#endif
                return ConfigurationManager.AppSettings["RepotrsFolderPath"];
            }
        }

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

        public static readonly string ReportDateTimeRegex = @"[0-9]{2}_[0-9]{2}_[0-9]{4} [0-9]{2}_[0-9]{2}_[0-9]{2}";

        public static readonly string ReportDateTimeParseFormat = @"dd_MM_yyyy HH_mm_ss";

        public static readonly string SystemPlarformOperatorName = "System Platform";

        public static readonly string DbSyncOperatorName = "DBSync";
    }
}