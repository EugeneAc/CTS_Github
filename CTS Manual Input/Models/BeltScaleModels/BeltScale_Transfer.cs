using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input.Models
{
	public class BeltScale_Transfer
	{
		public List<BeltScale> BeltScales { get; set; }
		public PagedList.IPagedList<BeltTransfer> InternalTransfers { get; set; }
		public bool CanEdit { get; set; }
		public bool CanDelete { get; set; }
	}
}