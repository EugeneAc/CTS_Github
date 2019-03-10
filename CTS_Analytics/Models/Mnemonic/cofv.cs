using CTS_Models;
using System.Collections.Generic;

namespace CTS_Analytics.Models.Mnemonic
{
    public class cofvModel_more : Station
    {
        public cofvModel_more(string id)
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

    public class cofvModel : iMine
    {
        public cofvModel(string locationID = "cofv")
        {
            Belt = new Mine_konv(locationID);
            VagonNorth = new Mine_vagon(locationID);
            VagonSouth1 = new Mine_vagon(locationID);
            VagonSouth2 = new Mine_vagon(locationID);
            Vagon = new Mine_vagon(locationID);
            Kotel = new Mine_Kotel(locationID);
            VehiScales = new Mine_rockUtil(locationID);
        }

        public Mine_konv Belt { get; set; }
        public Mine_vagon VagonNorth { get; set; }
        public Mine_vagon VagonSouth1 { get; set; }
        public Mine_vagon VagonSouth2 { get; set; }
        public Mine_vagon Vagon { get; set; }
        public Mine_Kotel Kotel { get; set; }
        public Mine_rockUtil VehiScales { get; set; }

        public string MineName { get; set; }
        public string DetailsViewName { get; set; }
    }
}