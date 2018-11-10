using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS_Models
{
	public interface IAnalysis
	{
		int ID { get; set; }
		string AnalysisID { get; set; }
		DateTime AnalysisTimeStamp { get; set; }
		DateTime LasEditDateTime { get; set; }
		string Comment { get; set; }
		string OperatorName { get; set; }
		bool IsValid { get; set; }
		int? InheritedFrom { get; set; }
	}
}
