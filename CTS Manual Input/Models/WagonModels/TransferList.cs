using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Models
{
    public class TransferList
    {
        public WagonTransfer Transfer { get; set; }
        public IList<WagonTransfer> Transfers { get; set; }
        public bool Incomming { get; set; }

        public TransferList()
        {
            Transfers = new List<WagonTransfer>();
        }
    }
}