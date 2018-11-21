using CTS_Analytics.Models.Mnemonic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Helpers
{
    public class AlarmCountModel
    {
        public int AlarmCount { get; set; }
        public int AlarmPriorityCount { get; set; }
    }

    public class AlarmHepler
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static AlarmCountModel GetMineAlarmCount(string mineId, DateTime fromDate, DateTime toDate)
        {
            string queryString = "select am.AlarmId,am.TagName,c.Comment,am.Priority " +
                "from [WWALMDB].[dbo].[AlarmMaster] am " +
                "join[WWALMDB].[dbo].[AlarmDetail] ad on am.AlarmId = ad.AlarmId " +
                "join[WWALMDB].[dbo].[Comment] c on ad.CommentId = c.CommentId " +
                "where am.AlarmId in " +
                "(select max(am.AlarmId) AlarmId from[WWALMDB].[dbo].[AlarmMaster] " +
                "am " +
                "inner join " +
                "(select distinct am.tagname, am.time time from[WWALMDB].[dbo].[AlarmMaster] am " +
                "inner join( " +
                "select am.AlarmId, am.TagName, c.Comment, am.Priority, am.Time AlarmStart, ad.EventStamp AlarmReturn from[WWALMDB].[dbo].[AlarmMaster] am " +
                "join [WWALMDB].[dbo].[AlarmDetail] ad on am.AlarmId= ad.AlarmId " +
                "join[WWALMDB].[dbo].[Comment] c on ad.CommentId= c.CommentId " +
                "where am.Time<ad.EventStamp " +
                $"and am.GroupName like '%{mineId}%')t1 on t1.AlarmId=am.AlarmId) t2 on t2.TagName=am.TagName " +
                "and t2.time=am.Time " +
                "group by am.time, am.tagname) " +
                "and ad.AlarmState = 'UNACK_RTN' " +
                "and am.Priority<500 " +
                "and am.Priority>200 " +
                $"and am.Time >= '{fromDate.ToString("yyyy-MM-dd")} {fromDate.ToString("HH:mm:ss")}' " +
                $"and am.Time <= '{toDate.ToString("yyyy-MM-dd")} {toDate.ToString("HH:mm:ss")}'" +
                "order by Time desc";

            var alarmCountModel = new AlarmCountModel();
            using (SqlConnection connection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["WWALMDB"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        alarmCountModel.AlarmCount++;
                        alarmCountModel.AlarmPriorityCount = alarmCountModel.AlarmPriorityCount + reader.GetInt16(3);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Exception from Alarm DB for GetMineAlarmCount");
                }
            }
            return alarmCountModel;
        }

        public static void GetMineAlarmLevel (MineGeneral mine, DateTime fromDate, DateTime toDate)
        {
            string queryString = "select am.Priority " +
                "from [WWALMDB].[dbo].[AlarmMaster] am " +
                "join[WWALMDB].[dbo].[AlarmDetail] ad on am.AlarmId = ad.AlarmId " +
                "join[WWALMDB].[dbo].[Comment] c on ad.CommentId = c.CommentId " +
                "where am.AlarmId in " +
                "(select max(am.AlarmId) AlarmId from[WWALMDB].[dbo].[AlarmMaster] " +
                "am " +
                "inner join " +
                "(select distinct am.tagname, am.time time from[WWALMDB].[dbo].[AlarmMaster] am " +
                "inner join( " +
                "select am.AlarmId, am.TagName, c.Comment, am.Priority, am.Time AlarmStart, ad.EventStamp AlarmReturn from[WWALMDB].[dbo].[AlarmMaster] am " +
                "join [WWALMDB].[dbo].[AlarmDetail] ad on am.AlarmId= ad.AlarmId " +
                "join[WWALMDB].[dbo].[Comment] c on ad.CommentId= c.CommentId " +
                "where am.Time<ad.EventStamp " +
                $"and am.GroupName like '%{mine.LocationID}%')t1 on t1.AlarmId=am.AlarmId) t2 on t2.TagName=am.TagName " +
                "and t2.time=am.Time " +
                "group by am.time, am.tagname) " +
                "and ad.AlarmState = 'UNACK_RTN' " +
                $"and am.Priority<500 " +
                $"and am.Priority>200" +
                $"and am.Time >= '{fromDate.ToString("yyyy-MM-dd")} {fromDate.ToString("HH:mm:ss")}' " +
                $"and am.Time <= '{toDate.ToString("yyyy-MM-dd")} {toDate.ToString("HH:mm:ss")}'" +
                "order by Time desc";

            using (SqlConnection connection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["WWALMDB"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetInt16(0) == ProjectConstants.CoalWarningPrio)
                            mine.CoalWarning = true;
                        if (reader.GetInt16(0) == ProjectConstants.CoalAlarmPrio)
                            mine.CoalAlarm = true;
                    }
                    reader.Close();
            }
        }
    }
}