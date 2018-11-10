using CTS_Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CTS_Analytics.Models.StaticReports
{
    public class EqupmentReportModel<T> where T : IEquip
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public IEnumerable<SelectListItem> Periods { get; set; }
        public IEnumerable<SelectListItem> Mines { get; set; }
        public IEnumerable<T> Equipment { get; set; }

        public string[] SelectedReportPeriod { get; set; }
        public string[] SelectedMines { get; set; }
        public string[] wagonScalesInForm { get; set; }
        public string[] skipsInForm { get; set; }
        public string[] beltScalesInForm { get; set; }
    }
}