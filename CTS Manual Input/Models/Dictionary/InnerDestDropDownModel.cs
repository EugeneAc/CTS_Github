using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Manual_Input.Models.Dictionary
{
  public class InnerDestDropDownModel
  {
    public IEnumerable<SelectListItem> InnerDests { get; set; }
  }
}