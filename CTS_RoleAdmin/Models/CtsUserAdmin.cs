using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_RoleAdmin
{
	public class CtsUserAdmin
	{
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public List<CtsRoleAdmin> ManualInputRoles { get; set; }
		public List<CtsRoleAdmin> AnalyticsRoles { get; set; }
		public List<CtsRoleAdmin> RoleAdminRoles { get; set; }
	}
}