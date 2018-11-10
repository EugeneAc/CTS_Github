using CTS_Models;
using System;

namespace CTS_Manual_Input.Models.LabModels
{
  public class WagonBatch
  {
    public string FromDest { get; set; }
    public string ToDest { get; set; }
    public string LotName { get; set; }
    public int WagonQuantity { get; set; }
    public float Netto { get; set; }
    public string Item { get; set; }
    public DateTime TransferTimeStamp { get; set; }
    public int? WagonAnalysisID { get; set; }
    public virtual WagonAnalysis WagonAnalysis { get; set; }
  }
}