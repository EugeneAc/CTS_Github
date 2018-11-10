using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Analytics.Models
{
    public class DynamicModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public IEnumerable<SelectListItem> Locations { get; set; }
        public IEnumerable<SelectListItem> BeltScales { get; set; }
        public IEnumerable<SelectListItem> Skips { get; set; }
        public IEnumerable<SelectListItem> FileFormats { get; set; }
        public IEnumerable<SelectListItem> ReportTypes { get; set; }

        public string[] locationsInForm { get; set; }
        public string[] beltScalesInForm { get; set; }
		public string[] skipsInForm { get; set; }

		public string fileFormatsInForm { get; set; }
		public string reportTypesInForm { get; set; }
	}
}