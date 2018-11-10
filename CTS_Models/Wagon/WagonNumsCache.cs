using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class WagonNumsCache
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ID { get; set; }
		public DateTime Date_time { get; set; }
		public int RecognID { get; set; }
		public virtual Recogn Recogn { get; set; }
		public int? Id_sostav { get; set; }
		public string Number { get; set; }
		public string Number_operator { get; set; }
		public int? Id_operator { get; set; }
		public string Camera { get; set; }
	}
}