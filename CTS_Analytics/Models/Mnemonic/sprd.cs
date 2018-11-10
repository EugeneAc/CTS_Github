using CTS_Models;
using System.Collections.Generic;

namespace CTS_Analytics.Models.Mnemonic
{
    public class sprdModel : Station
    {
        public sprdModel(string id)
        {
            LocationID = id;
        }

        public Mine_sklad Sklad { get; set; }
    }
}