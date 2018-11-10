using CTS_Manual_Input.Models;
using CTS_Models;
using System.Collections.Generic;

namespace CTS_Manual_Input.Models
{
    public class WagonScales_Transfers
    {
        public List<WagonScale> WagonScales { get; set; }
        public PagedList.IPagedList<WagonTransfer> Transfers { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
  }
}