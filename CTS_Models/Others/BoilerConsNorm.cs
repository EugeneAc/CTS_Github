using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTS_Models
{
	public class BoilerConsNorm
	{
		[Key, ForeignKey("Location")]
		public string ID { get; set; }
		public virtual Location Location { get; set; }
		public float ConsNorm { get; set; }
	}
}