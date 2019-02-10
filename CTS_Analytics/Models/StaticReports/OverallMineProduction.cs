
using System.Collections.Generic;
using System.Web.Mvc;

namespace CTS_Analytics.Models.StaticReports
{
    public class PeriodsAndMinesReportModel : OnlyPeriodsReportModel
    {
        public IEnumerable<SelectListItem> Mines { get; set; }

        public string[] SelectedMines { get; set; }
    }
}