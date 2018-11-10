using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTS_Models;
using System.Web.Mvc;

namespace CTS_Analytics.Models
{
  public class StaticConsolidated
  {
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public IEnumerable<SelectListItem> Locations { get; set; }
    public IEnumerable<SelectListItem> WagonScales { get; set; }
    public IEnumerable<SelectListItem> BeltScales { get; set; }
    public IEnumerable<SelectListItem> Skips { get; set; }

    public string[] locationsInForm { get; set; }
    public string[] beltScalesInForm { get; set; }
    public string[] wagonScalesInForm { get; set; }
    public string[] skipsInForm { get; set; }
  }
}