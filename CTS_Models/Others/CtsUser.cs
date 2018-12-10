using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS_Models
{
	public class CtsUser
	{
		[Key, Column(Order = 0)]
		public string Login { get; set; }
		[Key, Column(Order = 1)]
		public string Domain { get; set; }

		public ICollection<CtsRole> CtsRoles { get; set; }
		public CtsUser()
		{
			CtsRoles = new List<CtsRole>();
		}
	}
}
