using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input.Models.Dictionary
{
	public class ShiftsConsolidated
	{
		public string LocationID { get; set; }
		public Location Location { get; set; }
		public TimeSpan TimeStartFirstShift { get; set; }
		public TimeSpan TimeStartSecondShift { get; set; }
		public TimeSpan TimeStartThirdShift { get; set; }
		public TimeSpan TimeStartFourthShift { get; set; }
	}
}