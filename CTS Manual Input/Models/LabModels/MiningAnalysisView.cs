using CTS_Models;
using System.Collections.Generic;

namespace CTS_Manual_Input.Models.LabModels
{
	public class MiningAnalysisView
	{
		public PagedList.IPagedList<MiningAnalysis> MiningAnalysis { get; set; }
		public List<Location> Locations { get; set; }
		public bool CanEdit { get; set; }
		public bool CanDelete { get; set; }
	}
}