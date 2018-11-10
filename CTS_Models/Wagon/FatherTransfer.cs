using System;
using System.ComponentModel.DataAnnotations;

namespace CTS_Models
{
	public class FatherTransfer
	{
		[Key]
		public string ID { get; set; }
		public string FromDestID { get; set; }
		public virtual Location FromDest { get; set; }
		[MaxLength(255)]
		public string ToDest { get; set; }
		[Required]
		[MaxLength(255)]
		public string LotName { get; set; }
		[ValidateLotQuantity]
		public float Brutto { get; set; }
		[MaxLength(255)]
		public string Comment { get; set; }
		[MaxLength(255)]
		public string OperatorName { get; set; }
		[ValidateWeight]
		public float Tare { get; set; }
		public int? ItemID { get; set; }
		public virtual Item Item { get; set; }
		public DateTime LasEditDateTime { get; set; }
		[Required]
		[Display(Name = "Дата отгрузки")]
		[ValidateDate]
		public DateTime TransferTimeStamp { get; set; }

		[Required]
		public bool IsValid { get; set; }
		[Required]
		public int Status { get; set; }
		public string ApprovedBy { get; set; }
		public string InheritedFrom { get; set; }
	}
}