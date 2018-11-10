using System;
using System.ComponentModel.DataAnnotations;

namespace CTS_Models
{
	public class WarehouseMeasure
	{
		[Key]
		public int ID { get; set; }
		public int? WarehouseID { get; set; }
		public virtual Warehouse Warehouse { get; set; }
		public double? TotalMeasured { get; set; }
		public double? TotalCalculated { get; set; }
		public DateTime MeasureDate { get; set; }
		public DateTime LasEditDateTime { get; set; }
		[MaxLength(255)]
		public string OperatorName { get; set; }
		[MaxLength(255)]
		public string Comment { get; set; }
	}
}
