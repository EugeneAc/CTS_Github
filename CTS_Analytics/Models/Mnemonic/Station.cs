using CTS_Models;
using System;
using System.Collections.Generic;

namespace CTS_Analytics.Models.Mnemonic
{
     public abstract class Station
    {
        public class Station_wagon
        {
            public int TotalWagons { get; set; }
            public int TotalTonns { get; set; }
            public string LastTrainNumber { get; set; }
            public DateTime LastTrainTime { get; set; }
            public string LastTrainDirection { get; set; }
            public string LastTrainItem { get; set; }
            public int LastTrainWagonCount { get; set; }
            public string DivID { get; set; }
            public int WagonScalesID { get; set; }
        }

        public class FromStationData
        {
            public List<WagonTransfer> WagonTransfers { get; set; }
            public string ViewId { get; set; }
            public string MineName { get; set; }
            public string MineID { get; set; }
        }

        public string StationName { get; set; }
        public string LocationID { get; set; }
       

        public float TotalIncome { get; set; }
        public float TotalOutcome { get; set; }

        public FromStationData FromShah { get; set; }
        public FromStationData FromTent { get; set; }
        public FromStationData FromKaz { get; set; }
        public FromStationData FromLen { get; set; }
        public FromStationData FromKuz { get; set; }
        public FromStationData FromKost { get; set; }
        public FromStationData FromAbay { get; set; }
        public FromStationData FromSar { get; set; }
        public FromStationData FromCof { get; set; }

        public Station_wagon WagonScales { get; set; }
    }
}