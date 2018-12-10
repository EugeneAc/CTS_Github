using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS_Models
{ 
	public class CtsRole
	{
		[Key]
		public string RoleName { get; set; }
		public ICollection<CtsUser> CtsUsers { get; set; }
		public CtsRole()
		{
			CtsUsers = new List<CtsUser>();
		}
	}
}
