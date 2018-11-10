using System.Collections.Generic;
using System.Linq;
//using ArchestrA.MxAccess;
using CTS_Analytics.Hubs;

namespace CTS_Analytics
{
	public class ArchestraHelper
	{
		public static int hLMX { get; set; }
		//public static LMXProxyServer LMX_Server { get; set; }
		public static List<Tag> tags;

		public static void init()
		{
			/// Define Archestra tags here
			tags = new List<Tag>();
			tags.Add(new Tag("Shah_Skip_0.teststring"));
		}

		
		private static void SendMessage(string tagname, string tagvalue)
		{
			// Получаем контекст хаба
			var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
			// отправляем сообщение
			context.Clients.All.displayMessage(tagname, tagvalue);
		}
	}
}