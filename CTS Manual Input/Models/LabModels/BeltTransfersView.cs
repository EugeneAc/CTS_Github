using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input.Models.LabModels
{
	public class BeltTransfersView
	{
		public PagedList.IPagedList<BeltTransfer> BeltTransfers { get; set; }
		public bool CanEdit { get; set; }
		public bool CanDelete { get; set; }
	}
}