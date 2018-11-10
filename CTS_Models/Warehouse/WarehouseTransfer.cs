using System;
using System.ComponentModel.DataAnnotations;

namespace CTS_Models
{
	public class WarehouseTransfer
	{
		[Key]
		public string ID { get; set; }
		public int? WarehouseID { get; set; }
		public virtual Warehouse Warehouse { get; set; }
		[Required]
		public DateTime TransferTimeStamp { get; set; }
		public float? LotQuantity { get; set; }
		public double? TotalQuantity { get; set; }
		[MaxLength(255)]
		public string Comment { get; set; }
		public bool IsManual { get; set; }
	}
}
