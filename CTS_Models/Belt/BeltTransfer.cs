using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class BeltTransfer : ITransfer, IHaveAnalysis
	{
		[Key]
		public string ID { get; set; }
		[MaxLength(255)]
		public string LotName { get; set; }
		public float? LotQuantity { get; set; }
		public double? TotalQuantity { get; set; }
		[MaxLength(255)]
		public string Comment { get; set; }
		[MaxLength(255)]
		public string OperatorName { get; set; }
		public DateTime LasEditDateTime { get; set; }
		[Required]
		[ValidateDate]
		public DateTime TransferTimeStamp { get; set; }
		[Column("BeltScaleID")]
		[ForeignKey("Equip")]
		public int? EquipID { get; set; }
		public virtual BeltScale Equip { get; set; }
		[Required]
		public bool IsValid { get; set; }
		public int? ItemID { get; set; }
		public virtual Item Item { get; set; }
		[Column("BeltAnalysisID")]
		[ForeignKey("Analysis")]
		public int? AnalysisID { get; set; }
		public virtual BeltAnalysis Analysis { get; set; }
		[Required]
		public int Status { get; set; }
		public string ApprovedBy { get; set; }
		public string InheritedFrom { get; set; }
	}
}