using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_RoleAdmin
{
	public class CtsRole : ICloneable
	{
		public string GroupName { get; set; }
		public string GroupDescription { get; set; }
		public bool HasRole { get; set; }

		public object Clone()
		{
			return new CtsRole { GroupName = this.GroupName, GroupDescription = this.GroupDescription, HasRole = this.HasRole };
		}
	}
}