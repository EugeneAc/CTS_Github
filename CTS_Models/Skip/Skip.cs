using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class Skip : IEquip
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ID { get; set; }
		[MaxLength(255)]
		public string NameEng { get; set; }
		public string Name { get; set; }
		public string LocationID { get; set; }
		public virtual Location Location { get; set; }
		[ValidateWeight]
		public float Weight { get; set; }
		public string NameKZ { get; set; }
	}
}