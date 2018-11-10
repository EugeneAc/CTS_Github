using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class RockUtilTransfer : ITransfer
	{
		[Key]
		public string ID { get; set; }
		public float? LotQuantity { get; set; }
		[MaxLength(255)]
		public string Comment { get; set; }
		[MaxLength(255)]
		public string OperatorName { get; set; }
		public DateTime LasEditDateTime { get; set; }
		[Required]
		[ValidateDate]
		public DateTime TransferTimeStamp { get; set; }
		[Column("RockUtilID")]
		[ForeignKey("Equip")]
		public int? EquipID { get; set; }
		public virtual RockUtil Equip { get; set; }
		[Required]
		public bool IsValid { get; set; }
		[Required]
		public int Status { get; set; }
		public string ApprovedBy { get; set; }
		public string InheritedFrom { get; set; }
	}
}
