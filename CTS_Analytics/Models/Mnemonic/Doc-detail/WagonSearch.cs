using CTS_Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CTS_Analytics.Models.Mnemonic.Doc_detail
{
    public class WagonSearchModel
    {
        public IEnumerable<WagonTransfer> WagonTransfers { get; set; }
        public PagedList.StaticPagedList<WagonTransfersAndPhoto> PagedWagonTrasnfersAndPhotos { get; set; }
        public List<RaspoznItem> WagonPictureList { get; set; }
        public string WagonNumberFilter { get; set; }
        public IEnumerable<SelectListItem> Mines { get; set; }
        public bool FilterManualInput { get; set; }
        public bool OrderByTransferTimeStampAsc { get; set; }
    }
}