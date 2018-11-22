using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_RoleAdmin
{
	public class CtsUser
	{
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public List<CtsRole> ManualInputRoles { get; set; }
		public List<CtsRole> AnalyticsRoles { get; set; }
		public List<CtsRole> RoleAdminRoles { get; set; }
	}
}