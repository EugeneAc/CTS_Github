using CTS_Analytics.Filters;
using CTS_Analytics.Models;
using CTS_Models;
using CTS_Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Analytics.Controllers
{

    [Culture]
    public class FilterController : Controller
    {
		CtsDbContext cdb = new CtsDbContext();

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetWagonScales(string Locations)
        {
            string lang = getUserLang(Request.Cookies["lang"]);
            string[] locations = Locations.Split(Convert.ToChar(@","));
            var model = new WagonScalesDropDownModel();
            var scales = new List<WagonScale>();
            foreach (var l in locations)
            {
                scales.AddRange(cdb.WagonScales.Where(s => s.LocationID == l));
            }
            if (lang == "en")
                model.WagonScales = scales.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.NameEng, " / "), N.Location.LocationNameEng), Value = "W" + N.ID.ToString() });
            else if (lang == "kk")
                model.WagonScales = scales.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.NameKZ, " / "), N.Location.LocationNameKZ), Value = "W" + N.ID.ToString() });
            else
                model.WagonScales = scales.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.Name, " / "), N.Location.LocationName), Value = "W" + N.ID.ToString() });

            string mystring = Resources.ResourceFilters.allSelected;

            return PartialView("_WagonScalesDynDropDown", model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetBeltScales(string Locations)
        {
            string lang = getUserLang(Request.Cookies["lang"]);
            string[] locations = Locations.Split(Convert.ToChar(@","));
            var model = new BeltScalesDropDownModel();
            var scales = new List<BeltScale>();
            foreach (var l in locations)
            {
                scales.AddRange(cdb.BeltScales.Where(s => s.LocationID == l));
            }
            if (lang == "en")
                model.BeltScales = scales.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.NameEng, " / "), N.Location.LocationNameEng), Value = "B" + N.ID.ToString() });
            else if (lang == "kk")
                model.BeltScales = scales.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.NameKZ, " / "), N.Location.LocationNameKZ), Value = "B" + N.ID.ToString() });
            else
                model.BeltScales = scales.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.Name, " / "), N.Location.LocationName), Value = "B" + N.ID.ToString() }); ;

            return PartialView("_BeltScalesDynDropDown", model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetSkips(string Locations)
        {
            string lang = getUserLang(Request.Cookies["lang"]);
            string[] locations = Locations.Split(Convert.ToChar(@","));
            var model = new SkipsDropDownModel();
            var scales = new List<Skip>();
            foreach (var l in locations)
            {
                scales.AddRange(cdb.Skips.Where(s => s.LocationID == l));
            }
            if (lang == "en")
                model.Skips = scales.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.NameEng, " / "), N.Location.LocationNameEng), Value = "S" + N.ID.ToString() });
            else if (lang == "kk")
                model.Skips = scales.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.NameKZ, " / "), N.Location.LocationNameKZ), Value = "S" + N.ID.ToString() });
            else
                model.Skips = scales.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.Name, " / "), N.Location.LocationName), Value = "S" + N.ID.ToString() });

            return PartialView("_SkipsDynDropDown", model);
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
    }
}

