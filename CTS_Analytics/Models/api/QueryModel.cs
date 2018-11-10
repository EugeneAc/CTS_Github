using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Models
{
  public class QueryModel
  {
    public int  panelID { get; set; }
    public range range { get; set; }
    public raw rangeRaw { get; set; }
    public string interval { get; set; }
    public ulong intervalMs { get; set; }
    public target_[] targets { get; set; }
    public string format { get; set; }
    public int maxDataPoints { get; set; }
  }

  public class range
  {
    public string from { get; set; }
    public string to { get; set; }
    public raw raw { get; set; }
  }

  public class raw
  {
    public string from { get; set; }
    public string to { get; set; }
  }

  public class target_
  { 
    public string target { get; set; }
    public string refID { get; set; }
    public string type { get; set; }
  }

  public class QueryResponseTableModel : Iresponsemodel
  {
    public column[] columns { get; set; }
    public object[][] rows { get; set; }
    public string type { get; set; }

  }

  public class column
  {
    public string text { get; set; }
    public string type { get; set; }
  }
  public class QueryResponseTimeSerieModel : Iresponsemodel
  {
    public string target { get; set; }
    public double[][] datapoints { get; set; }
  }

  public interface Iresponsemodel
  {

  }
}