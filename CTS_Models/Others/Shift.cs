using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class Shift
	{
		[Key, Column("Shift", Order = 0)]
		public int ShiftNum { get; set; }
		[Key, Column(Order = 1), ForeignKey("Location")]
		public string LocationID { get; set; }
		public virtual Location Location { get; set; }
		public TimeSpan TimeStart { get; set; }
	}
}