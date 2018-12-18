using CTS_Analytics.Models.Mnemonic;
using CTS_Analytics.Models.Mnemonic.Doc_detail;
using CTS_Core;
using CTS_Models.DBContext;
using PagedList;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Analytics.Controllers
{
	[CtsAuthorize(Roles = Roles.AnalyticsRoleName)]
	public partial class MnemonicController : Controller
    {
        public ActionResult doc()
        {
            return View();
        }

        public ActionResult doc_detail_abay() => View("Doc-detail/doc_detail_abay");

        public ActionResult doc_detail_kaz() => View("Doc-detail/doc_detail_kaz");

        public ActionResult doc_detail_kost() => View("Doc-detail/doc_detail_kost");

        public ActionResult doc_detail_kuz() => View("Doc-detail/doc_detail_kuz");

        public ActionResult doc_detail_len() => View("Doc-detail/doc_detail_len");

        public ActionResult doc_detail_sar() => View("Doc-detail/doc_detail_sar");

        public ActionResult doc_detail_shah() => View("Doc-detail/doc_detail_shah");

        public ActionResult doc_detail_tent() => View("Doc-detail/doc_detail_tent");

        public ActionResult doc_detail_Skips()
        {
            using (var db = new CtsDbContext())
            {
                var skips = db.Skips;
                if (getUserLang(Request.Cookies["lang"]) == "en")
                {
                    return View("Doc-detail/doc_detail_Skips", skips.Select(s => new DocSkip()
                    {
                        Weight = s.Weight,
                        Name = s.NameEng,
                        Location = s.Location.LocationNameEng
                    }).ToArray());
                }

                if (getUserLang(Request.Cookies["lang"]) == "kk")
                {
                    return View("Doc-detail/doc_detail_Skips", skips.Select(s => new DocSkip()
                    {
                        Weight = s.Weight,
                        Name = s.NameKZ,
                        Location = s.Location.LocationNameKZ
                    }).ToArray());
                }
                return View("Doc-detail/doc_detail_Skips", skips.Select(s => new DocSkip()
                {
                    Weight = s.Weight,
                    Name = s.Name,
                    Location = s.Location.LocationName
                }).ToArray());

            }
        }

        public ActionResult doc_detail_Conveyors()
        {
            using (var db = new CtsDbContext())
            {
                var beltScales = db.BeltScales;
                if (getUserLang(Request.Cookies["lang"]) == "en")
                {
                    return View("Doc-detail/doc_detail_Conveyors", beltScales.Select(s => new DocBeltScale()
                    {
                        Name = s.NameEng,
                        Location = s.Location.LocationNameEng,
                        FromDest = s.FromInnerDest.NameEng,
                        ToDest = s.ToInnerDest.NameEng

                    }).ToArray());
                }

                if (getUserLang(Request.Cookies["lang"]) == "kk")
                {
                    return View("Doc-detail/doc_detail_Conveyors", beltScales.Select(s => new DocBeltScale()
                    {
                        Name = s.NameKZ,
                        Location = s.Location.LocationNameKZ,
                        FromDest = s.FromInnerDest.NameKZ,
                        ToDest = s.ToInnerDest.NameKZ
                    }).ToArray());
                }
                return View("Doc-detail/doc_detail_Conveyors", beltScales.Select(s => new DocBeltScale()
                {
                    Name = s.Name,
                    Location = s.Location.LocationName,
                    FromDest = s.FromInnerDest.Name,
                    ToDest = s.ToInnerDest.Name
                }).ToArray());
            }
        }

        public ActionResult doc_detail_Wagons()
        {
            using (var db = new CtsDbContext())
            {
                var beltScales = db.WagonScales;
                if (getUserLang(Request.Cookies["lang"]) == "en")
                {
                    return View("Doc-detail/doc_detail_Wagons", beltScales.Select(s => new DocWagonScales()
                    {
                        Name = s.NameEng,
                        Location = s.Location.LocationNameEng
                    }).ToArray());
                }

                if (getUserLang(Request.Cookies["lang"]) == "kk")
                {
                    return View("Doc-detail/doc_detail_Wagons", beltScales.Select(s => new DocWagonScales()
                    {
                        Name = s.NameKZ,
                        Location = s.Location.LocationNameKZ
                    }).ToArray());
                }
                return View("Doc-detail/doc_detail_Wagons", beltScales.Select(s => new DocWagonScales()
                {
                    Name = s.Name,
                    Location = s.Location.LocationName
                }).ToArray());
            }
        }

        public ActionResult doc_detail_SkipsWeights()
        {
            using (var db = new CtsDbContext())
            {
                var skipWeighs = db.SkipWeights;
                SkipWeights[] model;
                if (getUserLang(Request.Cookies["lang"]) == "en")
                {
                    model = skipWeighs.Select(s => new SkipWeights()
                    {
                        Name = s.Skip.NameEng,
                        Location = s.Skip.Location.LocationNameEng,
                        Weight = s.Weight.ToString(),
                        SkipWeight = s,
                    }).ToArray();
                }

                if (getUserLang(Request.Cookies["lang"]) == "kk")
                {
                    model = skipWeighs.Select(s => new SkipWeights()
                    {
                        Name = s.Skip.NameKZ,
                        Location = s.Skip.Location.LocationNameKZ,
                        Weight = s.Weight.ToString(),
                        SkipWeight = s,
                    }).ToArray();
                }
                model = skipWeighs.Select(s => new SkipWeights()
                {
                    Name = s.Skip.Name,
                    Location = s.Skip.Location.LocationName,
                    Weight = s.Weight.ToString(),
                    SkipWeight = s,
                }).ToArray();
                foreach (var s in model)
                {
                    s.ActFilePath = Directory.EnumerateFiles(CTS_Core.ProjectConstants.SkipActFolderPath, s.SkipWeight.ID + ".*").FirstOrDefault();
                }
                //model.Select(s => s.ActFilePath = Directory.EnumerateFiles(ProjectConstants.SkipActFolderPath, s.SkipWeight.ID.ToString() + ".*").FirstOrDefault());
                return View("Doc-detail/doc_detail_SkipWeights", model);
            }
        }

        public ActionResult doc_detail_BoilerUnitNorm()
        {
            using (var db = new CtsDbContext())
            {
                var skipWeighs = db.BoilerConsNorms;
                if (getUserLang(Request.Cookies["lang"]) == "en")
                {
                    return View("Doc-detail/doc_detail_BoilerUnitNorm", skipWeighs.Select(s => new BoilerNorm()
                    {
                        Location = s.Location.LocationNameEng,
                        ConsumptionNorm = s.ConsNorm.ToString()
                    }).ToArray());
                }

                if (getUserLang(Request.Cookies["lang"]) == "kk")
                {
                    return View("Doc-detail/doc_detail_BoilerUnitNorm", skipWeighs.Select(s => new BoilerNorm()
                    {
                        Location = s.Location.LocationNameKZ,
                        ConsumptionNorm = s.ConsNorm.ToString()
                    }).ToArray());
                }
                return View("Doc-detail/doc_detail_BoilerUnitNorm", skipWeighs.Select(s => new BoilerNorm()
                {
                    Location = s.Location.LocationName,
                    ConsumptionNorm = s.ConsNorm.ToString()
                }).ToArray());
            }
        }

        public FileResult OpenSkipAct(string filePath)
        {
            var fileStream = new FileStream(filePath,
                                 FileMode.Open,
                                 FileAccess.Read
                               );
            var fsResult = new FileStreamResult(fileStream, MimeMapping.GetMimeMapping(filePath));
            return fsResult;
        }

        public ActionResult doc_detail_Wagon_Search(string locationID, int? page, bool filterManualInput = false, bool orderByTransferTimeStampAsc = false, string wagonNumberFilter = "")
        {
            var model = new Mine_vagon(locationID);
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            using (var db = new CtsDbContext())
            {
                model.WagonTransfers = GetWagonTransfersFromCentralDb(null, db, fromDate, toDate)
              .ToList();
            }
            
            model.FilterManualInput = filterManualInput;
            if (filterManualInput)
            {
                model.WagonTransfers = model.WagonTransfers.Where(v => v.OperatorName != ProjectConstants.DbSyncOperatorName).ToList();
            }

            model.OrderByTransferTimeStampAsc = orderByTransferTimeStampAsc;
            if (orderByTransferTimeStampAsc)
            {
                model.WagonTransfers = model.WagonTransfers.OrderBy(t => t.TransferTimeStamp).ToList();
            }

            model.WagonNumberFilter = wagonNumberFilter;
            if (!string.IsNullOrEmpty(wagonNumberFilter))
            {
                model.WagonTransfers = model.WagonTransfers.Where(w => w.SublotName.Contains(wagonNumberFilter)).ToList();
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var transfersAndPictures = model.WagonTransfers.Select(w => new WagonTransfersAndPhoto()
            {
                WagonTransfer = w
            }).ToList();
            for (int i = (pageNumber - 1) * pageSize; i < pageNumber * pageSize; i++)
            {
                transfersAndPictures[i].Photo = GetWagonPhoto(transfersAndPictures[i].WagonTransfer.SublotName, transfersAndPictures[i].WagonTransfer.TransferTimeStamp);
            }

            model.PagedWagonTrasnfersAndPhotos = transfersAndPictures.ToPagedList(pageNumber, pageSize);

            return View("Doc-detail/doc_detail_Wagon_search", model);
        }
    }
}