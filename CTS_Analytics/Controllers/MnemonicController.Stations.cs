using CTS_Analytics.Models.Mnemonic;
using CTS_Core;
using CTS_Models.DBContext;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CTS_Analytics.Controllers
{
	[CtsAuthorize(Roles = Roles.AnalyticsRoleName)]
	public partial class MnemonicController : CtsAnalController
    {
        public ActionResult cofv()
        {
            var model = new cofvModel("cofv");
            Builder.GetGeneralStationData(model);
            model.FromAbay_arrived = Builder.GetFromMineData(model.LocationID, "abay", true);
            model.FromTent_arrived = Builder.GetFromMineData(model.LocationID, "tent", true);
            model.FromKaz_arrived = Builder.GetFromMineData(model.LocationID, "kaz", true);
            model.FromLen_arrived = Builder.GetFromMineData(model.LocationID, "len", true);
            model.FromKuz_arrived = Builder.GetFromMineData(model.LocationID, "kuz", true);
            model.FromKost_arrived = Builder.GetFromMineData(model.LocationID, "kost", true);
            model.FromShah_arrived = Builder.GetFromMineData(model.LocationID, "shah", true);
            model.FromSar_arrived = Builder.GetFromMineData(model.LocationID, "sar", true);
            return View("tsof2-more", model);
        }

        public ActionResult sdub()
        {
            var model = new sdubModel("sdub");
            Builder.GetGeneralStationData(model);

            return View(model);
        }

        public ActionResult sabay()
        {
            var model = new sabayModel("sabay");
            Builder.GetGeneralStationData(model);
            return View(model);
        }

        public ActionResult sprd()
        {
            var model = new sprdModel("sprd");
                Builder.GetGeneralStationData(model);
                model.Sklad = Builder.GetWarehouseModel(1); // TODO поменять ID когда заведем склад в базе

            return View(model);
        }

        public ActionResult sugl()
        {
            var model = new suglModel("sugl");
            Builder.GetGeneralStationData(model);

            return View(model);
        }

        public ActionResult srasp()
        {
            var model = new sraspModel("srasp");
                Builder.GetGeneralStationData(model);
                model.Sklad = Builder.GetWarehouseModel(10); 

            return View(model);
        }
    }
}