using CTS_Models;
using System.Collections.Generic;

namespace CTS_Analytics.Models.Mnemonic
{
    public class sraspModel : Station
    {
        public sraspModel(string id)
        {
            LocationID = id;
        }

        public Mine_sklad Sklad { get; set; }
    }
}