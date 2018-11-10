using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Analytics.Models
{
  public class BeltScalesDropDownModel
  {
    public IEnumerable<SelectListItem> BeltScales { get; set; }
  }
}