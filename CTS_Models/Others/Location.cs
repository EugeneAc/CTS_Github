using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class Location
	{
		[Key]
		[MaxLength(255)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string ID { get; set; }
		[MaxLength(255)]
		public string LocationName { get; set; }
		[MaxLength(255)]
		public string LocationNameEng { get; set; }
		[MaxLength(255)]
		public string DomainName { get; set; }
		[MaxLength(255)]
		public string LocationNameKZ { get; set; }
	}
}