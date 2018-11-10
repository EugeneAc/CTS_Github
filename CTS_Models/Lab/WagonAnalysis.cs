using System;
using System.ComponentModel.DataAnnotations;

namespace CTS_Models
{
	public class WagonAnalysis : IAnalysis
	{
		[Key]
		public int ID { get; set; }
		[Required]
		[MaxLength(255)]
		public string AnalysisID { get; set; }
		[Required]
		[ValidateDate]
		public DateTime AnalysisTimeStamp { get; set; }
		public DateTime LasEditDateTime { get; set; }
		[MaxLength(255)]
		public string Comment { get; set; }
		[MaxLength(255)]
		public string OperatorName { get; set; }
		public bool IsValid { get; set; }

		[ValidatePercent]
		public float? AshAvg { get; set; }
		[ValidatePercent]
		public float? AshMax { get; set; }
		[ValidatePercent]
		public float? SulfurAvg { get; set; }
		[ValidatePercent]
		public float? SulfurMax { get; set; }
		[ValidatePercent]
		public float? MoistureAvg { get; set; }
		[ValidatePercent]
		public float? MoistureMax { get; set; }
		[ValidatePercent]
		public float? VolatileMatter { get; set; }
		[ValidateAnalysis]
		public float? Caloricity { get; set; }
		[ValidateAnalysis]
		public float? CaloricityMax { get; set; }
		[ValidatePercent]
		public float? RockPerc { get; set; }
		[ValidatePercent]
		public float? SmallFractPerc { get; set; }
		[ValidateAnalysis]
		public float? xValue { get; set; }
		[ValidateAnalysis]
		public float? yValue { get; set; }
		public int? InheritedFrom { get; set; }
	}
}
