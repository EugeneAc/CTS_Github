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
using System.Configuration;
using CTS_Models.DBContext;
using CTS_Analytics.Models.StaticReports;
using System.Globalization;
using CTS_Core;

namespace CTS_Analytics.Controllers
{
    [Culture]
	[CtsAuthorize(Roles = Roles.AnalyticsRoleName)]
	public class StaticController : Controller
    {
        readonly string _directorypath = ProjectConstants.ReportFolderPath;
        readonly string _reportDateTimeRegex = ProjectConstants.ReportDateTimeRegex;
        readonly string _reportDateTimeParseFormat = ProjectConstants.ReportDateTimeParseFormat;
        private CtsDbContext _cdb = new CtsDbContext();
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "kk" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        // GET: Static
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GlobalSafety()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        public ActionResult SiteSafety()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        public ActionResult StockpilesMonth()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        public ActionResult OverallProduction()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        public ActionResult ComparisonInfoMiningsReport()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        public ActionResult OverallMineProduction()
        {
            return View(GetPeriodsAndMinesReportModel());
        }

        public ActionResult AlarmReport()
        {
            return View(GetPeriodsAndMinesReportModel());
        }

        public ActionResult AdvancedOverallMineProduction()
        {
            return View(GetPeriodsAndMinesReportModel());
        }

        public ActionResult OperCoalProductionReport()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        public ActionResult CoalProductionByMine()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        public ActionResult QualityControlReport()
        {
            return View(GetOnlyPeriodsReportModel());
        }

        public ActionResult WagonScalesReport()
        {
            return View(GetEquipmentReportModel<WagonScale>());
        }

        public ActionResult BeltScalesReport()
        {
            return View(GetEquipmentReportModel<BeltScale>());
        }

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

        #region Old Report

        public ActionResult Consolid()
        {
            string lang = getUserLang(Request.Cookies["lang"]);
            var model = new StaticConsolidated();
            model.FromDate = System.DateTime.Today.AddDays(-7);
            model.ToDate = System.DateTime.Today.AddDays(1);
            if (lang == "en")
                model.Locations = _cdb.Locations.ToList().Select(N => new SelectListItem { Text = N.LocationNameEng, Value = N.ID.ToString() });
            else if (lang == "kk")
                model.Locations = _cdb.Locations.ToList().Select(N => new SelectListItem { Text = N.LocationNameKZ, Value = N.ID.ToString() });
            else
                model.Locations = _cdb.Locations.ToList().Select(N => new SelectListItem { Text = N.LocationName, Value = N.ID.ToString() });

            return View(model);
        }

        [HttpPost]
        public ActionResult GetFileTable(StaticConsolidated model)
        {
            string regexp = "";
            if (model.locationsInForm != null)
            {
                regexp = regexp + Getregexp(model.locationsInForm);
            }

            List<string> idarray = new List<string>();
            if (model.beltScalesInForm == null && model.wagonScalesInForm == null && model.skipsInForm == null)
            {
                idarray.Add("c");
            }

            if (model.wagonScalesInForm != null)
            {
                idarray.AddRange(model.wagonScalesInForm);
                regexp = regexp + Getregexp(idarray.ToArray());
            }

            if (model.beltScalesInForm != null)
            {
                idarray.AddRange(model.beltScalesInForm);
                regexp = regexp + Getregexp(idarray.ToArray());
            }

            if (model.skipsInForm != null)
            {
                idarray.AddRange(model.skipsInForm);
                regexp = regexp + Getregexp(idarray.ToArray());
            }

            Regex reg = new Regex(regexp);

            var viewmodel = new FileTableModel();
            viewmodel.FileList = new List<file>();

            try
            {
                var selectedfiles = from o in Directory.GetFiles(_directorypath, "*.*",
                   SearchOption.AllDirectories)
                                    let x = new FileInfo(o)
                                    where (x.CreationTime >= model.FromDate && x.CreationTime <= model.ToDate)
                                    where Regex.IsMatch(Path.GetFileNameWithoutExtension(o), regexp, RegexOptions.IgnoreCase)
                                    select o.ToLower();

                foreach (var file in selectedfiles)
                {
                    Regex locationregex = new Regex(@"^([a-z]+)", RegexOptions.IgnoreCase);
                    Regex equipregex = new Regex(@"_(W|S|B|C)([0-9]+)", RegexOptions.IgnoreCase);
                    Regex equipregex1 = new Regex(@"([0-9]+)_(W|S|B)([0-9]+)", RegexOptions.IgnoreCase);
                    string filename = Path.GetFileNameWithoutExtension(file);
                    string lang = getUserLang(Request.Cookies["lang"]);

                    Match locationmatch = locationregex.Match(filename);
                    string lo = "";
                    if (locationmatch.Success)
                        if (lang == "en")
                            lo = _cdb.Locations.FirstOrDefault(l => l.ID.Contains(locationmatch.Value)).LocationNameEng;
                        else if (lang == "kk")
                            lo = _cdb.Locations.FirstOrDefault(l => l.ID.Contains(locationmatch.Value)).LocationNameKZ;

                    Match equipmatch = equipregex.Match(filename);
                    Match equipmatch1 = equipregex1.Match(filename);
                    bool res = equipmatch1.Success;
                    string sc = "";

                    if (equipmatch.Success)
                    {
                        string id = equipmatch.Value.Substring(2, equipmatch.Value.Length - 2);

                        if (equipmatch.Value[1] == Convert.ToChar("c"))
                        {
                            if (equipmatch1.Value.Length > 3)
                            {
                                id = equipmatch1.Value.Substring(3, equipmatch1.Value.Length - 3);
                            }
                            if (filename.Contains("c1"))
                            {
                                sc = Resources.ResourceStatic.ConsolidatedWeek + " ";
                            }
                            else if (filename.Contains("c2"))
                            {
                                sc = Resources.ResourceStatic.ConsolidatedMonth + " ";
                            }
                            else
                            {
                                sc = Resources.ResourceStatic.ConsolidatedDaily + " ";
                            }
                        }
                        else
                        {
                            sc = Resources.ResourceStatic.Daily + " ";
                        }

                        if ((equipmatch.Value[1] == Convert.ToChar("w")) || equipmatch1.Groups[2].Value.Contains("w"))
                        {
                            if (lang == "en")
                                sc = sc + _cdb.WagonScales.FirstOrDefault(s => s.ID.ToString() == id).NameEng;
                            else if (lang == "kk")
                                sc = sc + _cdb.WagonScales.FirstOrDefault(s => s.ID.ToString() == id).NameKZ;
                            else
                                sc = sc + _cdb.WagonScales.FirstOrDefault(s => s.ID.ToString() == id).Name;
                        }
                        else if ((equipmatch.Value[1] == Convert.ToChar("s")) || equipmatch1.Groups[2].Value.Contains("s"))
                        {
                            if (lang == "en")
                                sc = sc + _cdb.Skips.FirstOrDefault(s => s.ID.ToString() == id).NameEng;
                            else if (lang == "kk")
                                sc = sc + _cdb.Skips.FirstOrDefault(s => s.ID.ToString() == id).NameKZ;
                            else
                                sc = sc + _cdb.Skips.FirstOrDefault(s => s.ID.ToString() == id).Name;
                        }
                        else if ((equipmatch.Value[1] == Convert.ToChar("b")) || equipmatch1.Groups[2].Value.Contains("b"))
                        {
                            if (lang == "en")
                                sc = sc + _cdb.BeltScales.FirstOrDefault(s => s.ID.ToString() == id).NameEng;
                            else if (lang == "kk")
                                sc = sc + _cdb.BeltScales.FirstOrDefault(s => s.ID.ToString() == id).NameKZ;
                            else
                                sc = sc + _cdb.BeltScales.FirstOrDefault(s => s.ID.ToString() == id).Name;
                        }

                    }

                    viewmodel.FileList.Add(new file() { FileName = file, Location = lo, Equipment = sc, CreationDate = new DirectoryInfo(file).CreationTime });
                }
            }
            catch (Exception ex)
            {

            }

            ViewBag.FileCount = viewmodel.FileList.Count();
            return PartialView("_FileTable", viewmodel);
        }

        private string Getregexp(string[] idarray)
        {
            string regexp = "(";
            foreach (string id in idarray)
            {
                regexp = regexp + (id + @"|");
            }
            regexp = regexp.Remove(regexp.Length - 1, 1);
            regexp = regexp + @")\w*";
            return regexp;
        }
        #endregion

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

        private string getUserLang(HttpCookie cookie)
        {
            string lang = "";

            if (cookie != null)
                lang = cookie.Value;
            else
                lang = "ru";

            return lang;
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
            model.Mines = _cdb.Locations.Where(l => l.DomainName.ToLower().StartsWith("ш")) // Это плохо, может придумать фильтр получше?
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
