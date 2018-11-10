using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class BeltScale : IEquip
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ID { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }
		public string NameEng { get; set; }
		public string LocationID { get; set; }
		public virtual Location Location { get; set; }
		public int? FromInnerDestID { get; set; }
		public virtual InnerDestination FromInnerDest { get; set; }
		public int? ToInnerDestID { get; set; }
		public virtual InnerDestination ToInnerDest { get; set; }
		public string NameKZ { get; set; }
	}
}
