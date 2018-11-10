using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class SkipTransfer : ITransfer, IHaveAnalysis
	{
		[Key]
		public string ID { get; set; }
		[Required(ErrorMessage = "Поле должно быть установлено")]
		[MaxLength(255)]
		[ValidateSkipLiftingQuantity]
		public string LiftingID { get; set; }
		[Required]
		public DateTime LasEditDateTime { get; set; }
		[Required]
		[ValidateDate]
		public DateTime TransferTimeStamp { get; set; }
		[MaxLength(255)]
		public string Comment { get; set; }
		[MaxLength(255)]
		public string OperatorName { get; set; }
		[Column("SkipID")]
		[ForeignKey("Equip")]
		public int? EquipID { get; set; }
		public virtual Skip Equip { get; set; }
		[Required]
		public bool IsValid { get; set; }
		[Column("SkipAnalysisID")]
		[ForeignKey("Analysis")]
		public int? AnalysisID { get; set; }
		public virtual SkipAnalysis Analysis { get; set; }
		[Required]
		public float SkipWeight { get; set; }
		[Required]
		public int Status { get; set; }
		public string ApprovedBy { get; set; }
		public string InheritedFrom { get; set; }
	}
}