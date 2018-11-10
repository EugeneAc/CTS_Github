using CTS_Models;
using System.Collections.Generic;

namespace CTS_Manual_Input.Models
{
	public class VehiScales_Transfers
	{
		public List<VehiScale> VehiScales { get; set; }
		public PagedList.IPagedList<VehiTransfer> Transfers { get; set; }
		public bool CanEdit { get; set; }
		public bool CanDelete { get; set; }
	}
}