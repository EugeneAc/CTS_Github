using System;
using System.Collections.Generic;

namespace CTS_Analytics.Models.Mnemonic
{
    public class RaspoznModel
    {
        public List<RaspoznItem> RaspoznList { get; set; }
        public string DivID { get; set; }
        public int RaspoznID { get; set; }
    }

    public class RaspoznItem
    {
        public DateTime Date { get; set; }
        public string WagonNumber { get; set; }
        public string GallleryName { get; set; }
        public List<string> PictureGallery { get; set; }
        public bool ManualRecogn { get; set; }
        public int IdSostav { get; set; }
    }
}