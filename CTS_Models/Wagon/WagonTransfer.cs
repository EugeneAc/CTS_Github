using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class WagonTransfer : FatherTransfer, ITransfer, IHaveAnalysis
	{
		[MaxLength(255)]
		public string SublotName { get; set; }
		[Column("WagonScaleID")]
		[ForeignKey("Equip")]
		public int? EquipID { get; set; }
		public virtual WagonScale Equip { get; set; }
		[Column("WagonAnalysisID")]
		[ForeignKey("Analysis")]
		public int? AnalysisID { get; set; }
		public virtual WagonAnalysis Analysis { get; set; }

		public float? Netto { get; set; }
		[MaxLength(255)]
		public string OrderNumber { get; set; }

		public float? NettoByOrder { get; set; }
		[MaxLength(255)]
		public string Direction { get; set; }
	}
}