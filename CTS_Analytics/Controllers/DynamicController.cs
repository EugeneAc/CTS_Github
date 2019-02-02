using CTS_Analytics.Filters;
using CTS_Analytics.Models;
using CTS_Core;
using CTS_Models.DBContext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CTS_Analytics.Controllers
{
	[Culture]
	[CtsAuthorize(Roles = Roles.AnalyticsRoleName)]
	public class DynamicController : CtsAnalController
	{
		CtsDbContext cdb = new CtsDbContext();
		private List<String> fileFormats = new List<String> { "web", "pdf" };

        [CtsAuthorize(Roles = Roles.DynamicReportRole)]
        public ActionResult Index()
		{
			ViewBag.Url = "";

			return View(GetModel());
		}

		private DynamicModel GetModel()
		{
			var model = new DynamicModel();
			model.FromDate = System.DateTime.Today.AddDays(-7);
			model.ToDate = System.DateTime.Today.AddDays(1);
			model.FileFormats = fileFormats.Select(x => new SelectListItem { Text = x, Value = x });
			List<String> reportTypes = new List<String>();
			reportTypes.Add(Resources.ResourceDynamic.ReportBelt);
			reportTypes.Add(Resources.ResourceDynamic.ReportSkip);
			reportTypes.Add(Resources.ResourceDynamic.ReportComparisonMining);
			model.ReportTypes = reportTypes.Select(x => new SelectListItem { Text = x, Value = x });

			string lang = getUserLang(Request.Cookies["lang"]);

			if (lang == "en")
			{
				model.Locations = cdb.Locations.ToList().Select(N => new SelectListItem { Text = N.LocationNameEng, Value = N.ID.ToString() });
			}
			else
			{
				model.Locations = cdb.Locations.ToList().Select(N => new SelectListItem { Text = N.LocationName, Value = N.ID.ToString() });
			}

			return model;
		}

		[HttpPost]
		public ActionResult GetReport(DynamicModel model)
		{
			string DRip = ConfigurationManager.AppSettings["DRip"];
			string reportName = "";
			var url = new StringBuilder("http://" + DRip + "/DRWeb/WebApi/GenReport.aspx?report=reportName" + "&format=" + model.fileFormatsInForm +
				"&start=" + model.FromDate.ToString("yyyy-MM-dd") + "%20" + model.FromDate.ToString("HH:mm:ss") +
				"&stop=" + model.ToDate.ToString("yyyy-MM-dd") + "%20" + model.ToDate.ToString("HH:mm:ss") +
				"&ptype=abs&utctime=true"); 

			//Генерация url для динамического отчета по конв.весам
			if (model.reportTypesInForm == Resources.ResourceDynamic.ReportBelt)
			{
				reportName = "Dyn_belt";
				url.Replace("reportName", reportName);

				for (int i = 0; i < 30; i++)
				{
					url.Append("&ipp").Append((i + 1).ToString()).Append("=");

					if (i < model.beltScalesInForm.Length)
					{
						url.Append(model.beltScalesInForm[i].ToString().Substring(1));
					}
					else
					{
						url.Append("0");
					}
				}	
			}
			//Генерация url для динамического отчета по скипам
			else if (model.reportTypesInForm == Resources.ResourceDynamic.ReportSkip)
			{
				reportName = "Dyn_skip";
				url.Replace("reportName", reportName);

				for (int i = 0; i < 30; i++)
				{
					url.Append("&ipp").Append((i + 1).ToString()).Append("=");

					if (i < model.skipsInForm.Length)
					{
						url.Append(model.skipsInForm[i].ToString().Substring(1));
					}
					else
					{
						url.Append("0");
					}
				}
			}
			//Генерация url для динамического сравнения по добыче УД и CTS
			else if (model.reportTypesInForm == Resources.ResourceDynamic.ReportComparisonMining)
			{
				reportName = "ComparisonMiningInfo_d30004_all";
				url.Replace("reportName", reportName);
			}

			return Json(new { url = url.ToString() });
		}
	}
}