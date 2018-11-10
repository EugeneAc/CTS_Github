using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS_Models
{
	public interface IEquip
	{
		int ID { get; set; }
		string NameEng { get; set; }
		string Name { get; set; }
		string LocationID { get; set; }
		string NameKZ { get; set; }
	}
}
