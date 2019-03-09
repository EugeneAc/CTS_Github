using CTS_Models;
using System.Collections.Generic;

namespace CTS_Analytics.Models.Mnemonic
{
    public class cofvModel : Station
    {
        public cofvModel(string id)
        {
            LocationID = id;
        }

        public int ConcShippedToSrasp { get; set; }
        public int PrpodShippedToSrasp { get; set; }

        public int ConcShippedToSdub { get; set; }
        public int PrpodShippedToSdub { get; set; }

        public int ConcShippedToSabay { get; set; }
        public int PrpodShippedToSabay { get; set; }

        public FromStationData FromShah_arrived { get; set; }
        public FromStationData FromTent_arrived { get; set; }
        public FromStationData FromKaz_arrived { get; set; }
        public FromStationData FromLen_arrived { get; set; }
        public FromStationData FromKuz_arrived { get; set; }
        public FromStationData FromKost_arrived { get; set; }
        public FromStationData FromAbay_arrived { get; set; }
        public FromStationData FromSar_arrived { get; set; }
    }
}