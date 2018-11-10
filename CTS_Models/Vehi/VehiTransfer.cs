
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class VehiTransfer : FatherTransfer, ITransfer
	{
		[Column("VehiScaleID")]
		[ForeignKey("Equip")]
		public int? EquipID { get; set; }
		public virtual VehiScale Equip { get; set; }
	}
}