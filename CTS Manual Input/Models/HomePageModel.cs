using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input.Models
{
  public class HomePageModel
  {
    public List<Location> Locations { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
  }
}