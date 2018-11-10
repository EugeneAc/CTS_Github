using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class SkipWeight
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ID { get; set; }
		[Required]
		public int SkipID { get; set; }
		public virtual Skip Skip { get; set; }
		[Required]
		public float Weight { get; set; }
		[Required]
		public DateTime ValidFrom { get; set; }
		[MaxLength(255)]
		public string OrderNo { get; set; }
		[Required]
		public DateTime LasEditDateTime { get; set; }
		[MaxLength(255)]
		public string OperatorName { get; set; }
	}
}