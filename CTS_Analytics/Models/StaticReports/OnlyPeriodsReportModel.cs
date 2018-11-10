using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Analytics.Models.StaticReports
{
    public class OnlyPeriodsReportModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public IEnumerable<SelectListItem> Periods { get; set; }

        public string[] SelectedReportPeriod { get; set; }
    }
}