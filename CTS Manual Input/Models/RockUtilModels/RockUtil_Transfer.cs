using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input.Models
{
	public class RockUtil_Transfer
	{
		public List<RockUtil> RockUtils { get; set; }
		public PagedList.IPagedList<RockUtilTransfer> RockUtilTranfers { get; set; }
		public bool CanEdit { get; set; }
		public bool CanDelete { get; set; }
	}
}