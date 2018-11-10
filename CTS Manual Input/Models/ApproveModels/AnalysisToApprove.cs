using CTS_Models;
using System.Collections.Generic;

namespace CTS_Manual_Input.Models.ApproveModels
{
	public class AnalysisToApprove<T> where T : class, IAnalysis
	{
		public List<T> CreatedAnalysis { get; set; }
		public List<T> EditedAnalysis { get; set; }
		public List<T> ObsoleteAnalysis { get; set; }
		public List<T> DeletedAnalysis { get; set; }
	}
}