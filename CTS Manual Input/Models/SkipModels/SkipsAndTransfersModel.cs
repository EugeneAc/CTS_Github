using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Manual_Input.Models.SkipModels
{
	public class SkipsAndTransfersModel
	{
		public List<Skip> Skips { get; set; }
		public Dictionary<String, String> Counters { get; set; }
		public PagedList.IPagedList<SkipTransfer> SkipTransfers { get; set; }
		public bool CanEdit { get; set; }
		public bool CanDelete { get; set; }
	}
}