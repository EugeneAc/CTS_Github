using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTS_Analytics.Models;
using CTS_Models;
using System.IO;
using CTS_Analytics.Filters;
using System.IO.Compression;
using System.Text.RegularExpressions;
using CTS_Models.DBContext;
using CTS_Analytics.Models.StaticReports;
using System.Globalization;
using CTS_Core;

namespace CTS_Analytics.Controllers
{
    [Culture]
	[CtsAuthorize(Roles = Roles.AnalyticsRoleName)]
	public class StaticController : CtsAnalController
    {
        readonly string _directorypath = ProjectConstants.ReportFolderPath;
        readonly string _reportDateTimeRegex = ProjectConstants.ReportDateTimeRegex;
        readonly string _reportDateTimeParseFormat = ProjectConstants.ReportDateTimeParseFormat;
        private CtsDbContext _cdb = new CtsDbContext();
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        // GET: Static
        public ActionResult Index()
        {
            return View();
        }

        [CtsAuthorize(Roles = Roles.GlobalSafetyReportRole)]
        public ActionResult GlobalSafety()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        [CtsAuthorize(Roles = Roles.SiteSafetyReportRole)]
        public ActionResult SiteSafety()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        [CtsAuthorize(Roles = Roles.StockpilesMonthReportRole)]
        public ActionResult StockpilesMonth()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        [CtsAuthorize(Roles = Roles.OverallMineProductionReportRole)]
        public ActionResult OverallProduction()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        [CtsAuthorize(Roles = Roles.ComparisonInfoMiningsReportRole)]
        public ActionResult ComparisonInfoMiningsReport()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        [CtsAuthorize(Roles = Roles.OverallMineProductionReportRole)]
        public ActionResult OverallMineProduction()
        {
            return View(GetPeriodsAndMinesReportModel());
        }

        [CtsAuthorize(Roles = Roles.AlarmReportRole)]
        public ActionResult AlarmReport()
        {
            return View(GetPeriodsAndMinesReportModel());
        }

        [CtsAuthorize(Roles = Roles.AdvancedOverallMineProductionReportRole)]
        public ActionResult AdvancedOverallMineProduction()
        {
            return View(GetPeriodsAndMinesReportModel());
        }

        [CtsAuthorize(Roles = Roles.OperCoalProductionReportRole)]
        public ActionResult OperCoalProductionReport()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        [CtsAuthorize(Roles = Roles.CoalProductionByMineReportRole)]
        public ActionResult CoalProductionByMine()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        [CtsAuthorize(Roles = Roles.QualityControlReportRole)]
        public ActionResult QualityControlReport()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        [CtsAuthorize(Roles = Roles.WagonScalesReportRole)]
        public ActionResult WagonScalesReport()
        {
            return View(GetEquipmentReportModel<WagonScale>());
        }

        [CtsAuthorize(Roles = Roles.BeltScalesReportRole)]
        public ActionResult BeltScalesReport()
        {
            return View(GetEquipmentReportModel<BeltScale>());
        }

        [CtsAuthorize(Roles = Roles.SkipReportRole)]
        public ActionResult SkipReport()
        {
            return View(GetEquipmentReportModel<Skip>());
        }

        public ActionResult OpenLatestReport(string reportCode, string reportAuthor)
        {
            var file = GetFilesByFilter(null, reportAuthor, null, reportCode, null, new DateTime(), DateTime.Now)
                .OrderByDescending(f =>
                   DateTime.ParseExact(
                        new Regex("(" + _reportDateTimeRegex + ")$", RegexOptions.IgnoreCase)
                        .Match(Path.GetFileNameWithoutExtension(f))
                        .Value, _reportDateTimeParseFormat, CultureInfo.InvariantCulture))
                        .ThenByDescending(d => new FileInfo(d).CreationTime).FirstOrDefault();
            if (file != null)
                return OpenFile(file);
            else
                return RedirectToAction("Index");
        }

        // POST: Static
        [HttpPost]
        public ActionResult GetWagonScalesReports(EqupmentReportModel<WagonScale> model)
        {
            model.wagonScalesInForm = model.wagonScalesInForm == null ?
                 _cdb.WagonScales.Select(s => s.ID).ToArray().Select(f => f.ToString("00")).ToArray()
                 : TrimEquipmentArray(model.wagonScalesInForm);
            model.SelectedMines = model.SelectedMines ?? _cdb.Locations.Select(i => i.ID).ToArray();
            var selectedfiles = GetFilesByFilter(model.SelectedReportPeriod, "1", model.wagonScalesInForm, "03", model.SelectedMines, model.FromDate, model.ToDate);
            var viewmodel = new FileTableModel();
            viewmodel.FileList = BuildFileModel(selectedfiles, getUserLang(Request.Cookies["lang"]))?.ToList();
			ViewBag.FileCount = viewmodel.FileList.Count();
			return PartialView("_FileTable", viewmodel);
        }

        [HttpPost]
        public ActionResult GetSkipReports(EqupmentReportModel<Skip> model)
        {
            model.skipsInForm = model.skipsInForm == null ? 
                _cdb.Skips.Select(s => s.ID).ToArray().Select(f => f.ToString("00")).ToArray()
                : TrimEquipmentArray(model.skipsInForm);
            model.SelectedMines = model.SelectedMines ?? _cdb.Locations.Select(i => i.ID).ToArray();
            var selectedfiles = GetFilesByFilter(model.SelectedReportPeriod, "1", model.skipsInForm, "02", model.SelectedMines, model.FromDate, model.ToDate);
            var viewmodel = new FileTableModel();
            viewmodel.FileList = BuildFileModel(selectedfiles, getUserLang(Request.Cookies["lang"]))?.ToList();
			ViewBag.FileCount = viewmodel.FileList.Count();
			return PartialView("_FileTable", viewmodel);
        }

        [HttpPost]
        public ActionResult GetBeltScalesReports(EqupmentReportModel<BeltScale> model)
        {
            model.beltScalesInForm = model.beltScalesInForm == null ?
                _cdb.BeltScales.Select(s => s.ID).ToArray().Select(f => f.ToString("00")).ToArray()
                : TrimEquipmentArray(model.beltScalesInForm);
            model.SelectedMines = model.SelectedMines ?? _cdb.Locations.Select(i => i.ID).ToArray();
            var selectedfiles = GetFilesByFilter(model.SelectedReportPeriod, "1", model.beltScalesInForm, "01", model.SelectedMines, model.FromDate, model.ToDate);
            var viewmodel = new FileTableModel();
            viewmodel.FileList = BuildFileModel(selectedfiles, getUserLang(Request.Cookies["lang"]))?.ToList();
			ViewBag.FileCount = viewmodel.FileList.Count();
			return PartialView("_FileTable", viewmodel);
        }

        [HttpPost]
        public ActionResult GetStockpilesMonthReport(OnlyPeriodsReportModel model)
        {
            return PartialView("_FileTable", GetOnlyPeriodReportFiles("3", "05", model));
        }

        [HttpPost]
        public ActionResult GetGlobalSafetyReport(OnlyPeriodsReportModel model)
        {
            return PartialView("_FileTable", GetOnlyPeriodReportFiles("3", "06", model));
        }

        [HttpPost]
        public ActionResult GetSiteSafetyReport(OnlyPeriodsReportModel model)
        {
            return PartialView("_FileTable", GetOnlyPeriodReportFiles("3", "07", model));
        }

        [HttpPost]
        public ActionResult GetOperCoalProductionReport(OnlyPeriodsReportModel model)
        {
            return PartialView("_FileTable", GetOnlyPeriodReportFiles("2", "01", model));
        }

        [HttpPost]
        public ActionResult GetComparisonInfoMiningsReport(OnlyPeriodsReportModel model)
        {
            return PartialView("_FileTable", GetOnlyPeriodReportFiles("3", "04", model));
        }

        [HttpPost]
        public ActionResult GetCoalProductionByMineReports(OnlyPeriodsReportModel model)
        {
            return PartialView("_FileTable", GetOnlyPeriodReportFiles("2", "02", model));
        }

        [HttpPost]
        public ActionResult GetOverallProductionReports(OnlyPeriodsReportModel model)
        {
            return PartialView("_FileTable", GetOnlyPeriodReportFiles("3", "01", model));
        }

        [HttpPost]
        public ActionResult GetQualityControlReports(OnlyPeriodsReportModel model)
        {
            return PartialView("_FileTable", GetOnlyPeriodReportFiles("3", "03", model));
        }

        [HttpPost]
        public ActionResult GetAlarmReports(PeriodsAndMinesReportModel model)
        {
            return PartialView("_FileTable", GetPeriodsAndMinesReportFiles("1","05",model));
        }

        [HttpPost]
        public ActionResult GetAdvancedOverallMineProductionReports(PeriodsAndMinesReportModel model)
        {
            return PartialView("_FileTable", GetPeriodsAndMinesReportFiles("3", "02", model));
        }

        [HttpPost]
        public ActionResult GetOverallMineProductionReports(PeriodsAndMinesReportModel model)
        {
            return PartialView("_FileTable", GetPeriodsAndMinesReportFiles("1", "04", model));
        }

        public FileResult OpenFile(string fileName)
        {
            //code to download file:
            //return File(FileName, "application/octet-stream", Path.GetFileName(FileName));

            //code to open file in browser:
            var fileStream = new FileStream(fileName,
                                 FileMode.Open,
                                 FileAccess.Read
                               );
            var extension = Path.GetExtension(fileName).TrimStart('.');
            var fsResult = new FileStreamResult(fileStream, MimeMapping.GetMimeMapping(fileName));
            return fsResult;
        }

        // private methods below

        /// <summary>
        /// Build file list model for view
        /// </summary>
        /// <param name="files"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        private IEnumerable<file> BuildFileModel(IEnumerable<string> files, string lang)
        {
            var filesit = new List<file>();
            Regex reportPeriodRegex = new Regex(@"^\w+_s[0-9]{1}", RegexOptions.IgnoreCase);
            Regex reportCreationDateRegex = new Regex("(" + _reportDateTimeRegex + ")$", RegexOptions.IgnoreCase);
            Regex locationRegex = new Regex("_([a-z]+[1,3]?)_", RegexOptions.IgnoreCase);
            Regex reportTypeRegex = new Regex("[0-9]{5}_", RegexOptions.IgnoreCase);
            foreach (var file in files)
            {
                string filename = Path.GetFileNameWithoutExtension(file);
                Match periodMatch = reportPeriodRegex.Match(filename);
                Match creationDateMatch = reportCreationDateRegex.Match(filename);
                Match locationMatch = locationRegex.Match(filename);
                Match reportTyperMatch = reportTypeRegex.Match(filename);
                RerportPeriod period = RerportPeriod.Day;
                DateTime creationDate = new DirectoryInfo(file).CreationTime;
                string equipmentName = "";
                if (periodMatch.Success)
                {
                    period = (RerportPeriod)Int32.Parse(periodMatch.Value.Substring(periodMatch.Value.Length - 1));
                }

                if (creationDateMatch.Success)
                {
                    creationDate = DateTime.ParseExact(creationDateMatch.Value, _reportDateTimeParseFormat, CultureInfo.InvariantCulture);
                }

                if (reportTyperMatch.Success)
                {
                    var reportType = reportTyperMatch.Value.TrimEnd('_');
                    if (reportType.StartsWith("1")) // if report is ours
                    {
                        var id = Int32.Parse(reportType[1].ToString() + reportType[2].ToString());
                        if (reportType.EndsWith("01")) //if report is beltscale report
                        {
                            if (lang == "en")
                                equipmentName = _cdb.BeltScales.Find(id).NameEng;
                            else if (lang == "kk")
                                equipmentName = _cdb.BeltScales.Find(id).NameKZ;
                            else
                                equipmentName = _cdb.BeltScales.Find(id).Name;
                        }

                        if (reportType.EndsWith("02")) //if report is skip report
                        {
                            if (lang == "en")
                                equipmentName = _cdb.Skips.Find(id).NameEng;
                            else if (lang == "kk")
                                equipmentName = _cdb.Skips.Find(id).NameKZ;
                            else
                                equipmentName = _cdb.Skips.Find(id).Name;
                        }

                        if (reportType.EndsWith("03")) //if report is wagon report
                        {
                            if (lang == "en")
                                equipmentName = _cdb.WagonScales.Find(id).NameEng;
                            else if (lang == "kk")
                                equipmentName = _cdb.WagonScales.Find(id).NameKZ;
                            else
                                equipmentName = _cdb.WagonScales.Find(id).Name;
                        }
                    }
                }

                string location = string.Empty;
                if (locationMatch.Success)
                {
                    if (locationMatch.Value.Trim('_') == "all")
                    {
                        if (lang == "en")
                            location = "all";
                        else if (lang == "kk")
                            location = "Все";
                        else
                            location = "Все";
                    }
                    else
                    {
                        if (lang == "en")
                            location = _cdb.Locations.Find(locationMatch.Value.Trim('_')).LocationNameEng;
                        else if (lang == "kk")
                            location = _cdb.Locations.Find(locationMatch.Value.Trim('_')).LocationNameKZ;
                        else
                            location = _cdb.Locations.Find(locationMatch.Value.Trim('_')).LocationName;
                    }
                }

                filesit.Add(new file()
                {
                    FileName = file,
                    Location = location,
                    Equipment = Resources.ResourceFilters.ResourceManager.GetString(period.ToString())
                    + " " + equipmentName,
                    CreationDate = creationDate
                });
            }
            return filesit;
        }


        /// <summary>
        /// search files with reports in fodler by given filter params
        /// </summary>
        /// <param name="periods"></param>
        /// <param name="author"></param>
        /// <param name="codePrefix"></param>
        /// <param name="code"></param>
        /// <param name="locations"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private IEnumerable<string> GetFilesByFilter(string[] periods, string author, string[] codePrefix, string code, string[] locations, DateTime fromDate, DateTime toDate)
        {
            var periodsRegex = "[0-9]";
            if (periods != null)
                periodsRegex = "[" + string.Join(",", periods) + "]";

            var authorsRegex = "[0-9]";
            if (author != null)
                authorsRegex = author;

            var locationsRegex = "all";
            if (locations != null)
                locationsRegex = "(" + string.Join("|", locations) + ")";

            var codePrefixRegex = "00";
            if (codePrefix != null)
                codePrefixRegex = "(" + string.Join("|", codePrefix) + ")";

            string regexp =
                @"\w+" +
                $"{periodsRegex}" + "{1}" +
                $"{authorsRegex}" + "{1}" +
                $"{codePrefixRegex}" + "{1}" +
                code +
                $"_{locationsRegex}_" +
                _reportDateTimeRegex +
                "\\w*";
            Regex reg = new Regex(regexp);

            return from o in Directory.GetFiles(_directorypath, "*.*",
                   SearchOption.AllDirectories)
                   let fileName = Path.GetFileNameWithoutExtension(o)
                   where Regex.IsMatch(fileName, regexp, RegexOptions.IgnoreCase)
                   let x = new Regex("(" + _reportDateTimeRegex + ")$", RegexOptions.IgnoreCase)
                   let creationDateMatch = x.Match(fileName)
                   let dt = DateTime.ParseExact(creationDateMatch.Value, _reportDateTimeParseFormat, CultureInfo.InvariantCulture)
                   where (dt >= fromDate && dt <= toDate)
                   select o.ToLower();
        }

        [HttpPost]
        public ActionResult BuildArchive(ICollection<string> files)
        {
            try
            {
                if (System.IO.File.Exists(Server.MapPath("~/App_Data/Reports.zip")))
                {
                    System.IO.File.Delete(Server.MapPath("~/App_Data/Reports.zip"));
                }
                using (ZipArchive archive = ZipFile.Open(Server.MapPath("~/App_Data/Reports.zip"), ZipArchiveMode.Create))
                {
                    foreach (var file in files)
                    {
                        archive.CreateEntryFromFile(file, Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exeption when building Reopts.zip");
            }

            return new JsonResult()
            {
                Data = new { FilePath = Server.MapPath("~/App_Data/Reports.zip") }
            };
        }

        [HttpGet]
        public FileResult Download(string filePath)
        {
            return File(Server.MapPath("~/App_Data/Reports.zip"), "application/octet-stream", "Reports.zip");
        }

        private List<SelectListItem> GetPeriods()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = Resources.ResourceFilters.Day,
                    Value = "1"
                },
                new SelectListItem()
                {
                    Text = Resources.ResourceFilters.Week,
                    Value = "2"
                },
                new SelectListItem()
                {
                    Text = Resources.ResourceFilters.Month,
                    Value = "3"
                },
                new SelectListItem()
                {
                    Text = Resources.ResourceFilters.Quater,
                    Value = "4"
                },
                new SelectListItem()
                {
                    Text = Resources.ResourceFilters.Year,
                    Value = "5"
                }
            };
        }

        private PeriodsAndMinesReportModel GetPeriodsAndMinesReportModel()
        {
            var model = new PeriodsAndMinesReportModel();
            model.FromDate = DateTime.Today.AddDays(-7);
            model.ToDate = DateTime.Today.AddDays(1);
            model.Periods = GetPeriods();
            model.Mines = _cdb.Locations.Where(l => l.LocationName.ToLower().StartsWith("ш")) 
                                                     .Select(i => new SelectListItem()
                                                     {
                                                         Text = i.LocationName,
                                                         Value = i.ID
                                                     });
            string lang = getUserLang(Request.Cookies["lang"]);
            if (lang == "en")
                model.Mines.Select(m => m.Text = _cdb.Locations.Where(l => l.ID == m.Value).Select(s => s.LocationNameEng).FirstOrDefault());
            else if (lang == "kk")
                model.Mines.Select(m => m.Text = _cdb.Locations.Where(l => l.ID == m.Value).Select(s => s.LocationNameKZ).FirstOrDefault());

            return model;
        }

        private EqupmentReportModel<T> GetEquipmentReportModel<T>() where T : IEquip
        {
            var model = new EqupmentReportModel<T>();
            model.FromDate = DateTime.Today.AddDays(-7);
            model.ToDate = DateTime.Today.AddDays(1);
            model.Periods = GetPeriods();
            model.Mines = _cdb.Locations.Select(i => new SelectListItem()
            {
                Text = i.LocationName,
                Value = i.ID
            });
            string lang = getUserLang(Request.Cookies["lang"]);
            if (lang == "en")
                model.Mines.Select(m => m.Text = _cdb.Locations.Where(l => l.ID == m.Value).Select(s => s.LocationNameEng).FirstOrDefault());
            else if (lang == "kk")
                model.Mines.Select(m => m.Text = _cdb.Locations.Where(l => l.ID == m.Value).Select(s => s.LocationNameKZ).FirstOrDefault());

            return model;
        }

        private OnlyPeriodsReportModel GetOnlyPeriodsReportModel()
        {
            var model = new OnlyPeriodsReportModel();
            model.FromDate = DateTime.Today.AddDays(-7);
            model.ToDate = DateTime.Today.AddDays(1);
            model.Periods = GetPeriods();
            return model;
        }

        private FileTableModel GetOnlyPeriodReportFiles(string author, string reportCode ,OnlyPeriodsReportModel model)
        {
            var selectedfiles = GetFilesByFilter(model.SelectedReportPeriod, author, null, reportCode, null, model.FromDate, model.ToDate);
            var viewmodel = new FileTableModel();
            viewmodel.FileList = BuildFileModel(selectedfiles, getUserLang(Request.Cookies["lang"]))?.ToList();
            return viewmodel;
        }

        private FileTableModel GetPeriodsAndMinesReportFiles(string author, string reportCode, PeriodsAndMinesReportModel model)
        {
            var selectedfiles = GetFilesByFilter(model.SelectedReportPeriod, author, null, reportCode, model.SelectedMines, model.FromDate, model.ToDate);
            var viewmodel = new FileTableModel();
            viewmodel.FileList = BuildFileModel(selectedfiles, getUserLang(Request.Cookies["lang"]))?.ToList();
            return viewmodel;
        }

        private string[] TrimEquipmentArray(string[] selectedEquipment)
        {
            if (selectedEquipment != null)
            for (int i = 0; i < selectedEquipment.Count(); i++)
            {
                selectedEquipment[i] = selectedEquipment[i].Remove(0, 1);
                selectedEquipment[i] = Int32.Parse(selectedEquipment[i]).ToString("00");
            }
            return selectedEquipment;
        }
    }
}