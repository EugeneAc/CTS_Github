using CTS_Analytics.Filters;
using CTS_Core;
using System.Web.Mvc;

namespace CTS_Analytics.Controllers
{
	[Culture]
	[CtsAuthorize(Roles = Roles.AnalyticsDashboardRoleName)]
	public class DashBoardController : CtsAnalController
	{

		// GET: DashBoard
		public ActionResult Index()
		{
			
#if DEBUG
			ViewBag.HostName = "192.168.0.68";
#else
            ViewBag.HostName = Request.Url.Host;
#endif
			return View("Index");
		}

		public ActionResult Mine(string ID)
		{
			string viewID;
			switch (ID.ToLower())
			{
				case "kuz":
					{
						viewID = "kuz";
                        ViewBag.LocationName = "Шахта им. Кузембаева / Kuzembayeva Mine";
                        break;
					}
				case "kost":
					{
						viewID = "kost";
                        ViewBag.LocationName = "Шахта им. Костенко / Kostenko Mine";
                        break;
					}
                case "abay":
                    {
                        viewID = "abay";
                        ViewBag.LocationName = "Шахта Абайская / Abay Mine";
                        break;
                    }
                case "len":
                    {
                        viewID = "len";
                        ViewBag.LocationName = "Шахта им. Ленина / Lenina Mine";
                        break;
                    }
                case "sar1":
					{
						viewID = "sar1";
                        ViewBag.LocationName = "Шахта Саранская - 1 / Saranskaya - 1 Mine";
                        break;
					}
                case "sar3":
                    {
                        viewID = "sar3";
                        ViewBag.LocationName = "Шахта Саранская - 3 / Saranskaya - 3 Mine";
                        break;
                    }
                case "sar":
                    {
                        viewID = "sar";
                        ViewBag.LocationName = "Шахта Саранская / Saranskaya - Mine";
                        break;
                    }
                case "kaz":
                    {
                        viewID = "kaz";
                        ViewBag.LocationName = "Шахта Казахстанская / Kazakhstanskaya Mine";
                        break;
                    }
                case "shah":
					{
						viewID = "shah";
                        ViewBag.LocationName = "Шахта Шахтинская / Shahtinskaya Mine";
                        break;
					}
                case "tent":
                    {
                        viewID = "tent";
                        ViewBag.LocationName = "Шахта Тентекская / Tentekstaya Mine";
                        break;
                    }
                case "alarms_kuz":
                    {
                        viewID = "alarms_kuz";
                        ViewBag.LocationName = "Шахта им. Кузембаева / Kuzembayeva Mine";
                        break;
                    }
                case "alarms_kost":
                    {
                        viewID = "alarms_kost";
                        ViewBag.LocationName = "Шахта им. Костенко / Kostenko Mine";
                        break;
                    }
                case "alarms_abay":
                    {
                        viewID = "alarms_abay";
                        ViewBag.LocationName = "Шахта Абайская / Abay Mine";
                        break;
                    }
                case "alarms_len":
                    {
                        viewID = "alarms_len";
                        ViewBag.LocationName = "Шахта им. Ленина / Lenina Mine";
                        break;
                    }
                case "alarms_sar1":
                    {
                        viewID = "alarms_sar1";
                        ViewBag.LocationName = "Шахта Саранская - 1 / Saranskaya - 1 Mine";
                        break;
                    }
                case "alarms_sar3":
                    {
                        viewID = "alarms_sar3";
                        ViewBag.LocationName = "Шахта Саранская - 3 / Saranskaya - 3 Mine";
                        break;
                    }
                case "alarms_sar":
                    {
                        viewID = "alarms_sar";
                        ViewBag.LocationName = "Шахта Саранская / Saranskaya - Mine";
                        break;
                    }
                case "alarms_kaz":
                    {
                        viewID = "alarms_kaz";
                        ViewBag.LocationName = "Шахта Казахстанская / Kazakhstanskaya Mine";
                        break;
                    }
                case "alarms_shah":
                    {
                        viewID = "alarms_shah";
                        ViewBag.LocationName = "Шахта Шахтинская / Shahtinskaya Mine";
                        break;
                    }
                case "alarms_tent":
                    {
                        viewID = "alarms_tent";
                        ViewBag.LocationName = "Шахта Тентекская / Tentekstaya Mine";
                        break;
                    }
                default:
					viewID = "kuz";
					break;
			}
			ViewBag.LocationID = viewID;
			return View("Mine");
		}

        [CtsAuthorize(Roles = Roles.MineAbayRoleName)]
        public ActionResult Abay()
        {
            return Mine("abay");
        }

        [CtsAuthorize(Roles = Roles.MineKuzRoleName)]
        public ActionResult Kuz()
        {
            return Mine("kuz");
        }

        [CtsAuthorize(Roles = Roles.MineKostRoleName)]
        public ActionResult Kost()
        {
            return Mine("Kost");
        }

        [CtsAuthorize(Roles = Roles.MineLenRoleName)]
        public ActionResult Len()
        {
            return Mine("Len");
        }

        [CtsAuthorize(Roles = Roles.MineSar1RoleName)]
        public ActionResult Sar1()
        {
            return Mine("sar1");
        }

        [CtsAuthorize(Roles = Roles.MineSar3RoleName)]
        public ActionResult Sar3()
        {
            return Mine("sar3");
        }

        [CtsAuthorize(Roles = Roles.MineKazRoleName)]
        public ActionResult Kaz()
        {
            return Mine("kaz");
        }

        [CtsAuthorize(Roles = Roles.MineShahRoleName)]
        public ActionResult Shah()
        {
            return Mine("shah");
        }

        [CtsAuthorize(Roles = Roles.MineTentRoleName)]
        public ActionResult Tent()
        {
            return Mine("tent");
        }



        [CtsAuthorize(Roles = Roles.MineAbayRoleName)]
        public ActionResult Alarms_Abay()
        {
            return Mine("Alarms_abay");
        }

        [CtsAuthorize(Roles = Roles.MineKuzRoleName)]
        public ActionResult Alarms_Kuz()
        {
            return Mine("Alarms_kuz");
        }

        [CtsAuthorize(Roles = Roles.MineKostRoleName)]
        public ActionResult Alarms_Kost()
        {
            return Mine("Alarms_Kost");
        }

        [CtsAuthorize(Roles = Roles.MineLenRoleName)]
        public ActionResult Alarms_Len()
        {
            return Mine("Alarms_Len");
        }

        [CtsAuthorize(Roles = Roles.MineSar1RoleName)]
        public ActionResult Alarms_Sar1()
        {
            return Mine("Alarms_sar1");
        }

        [CtsAuthorize(Roles = Roles.MineSar3RoleName)]
        public ActionResult Alarms_Sar3()
        {
            return Mine("Alarms_sar3");
        }

        [CtsAuthorize(Roles = Roles.MineKazRoleName)]
        public ActionResult Alarms_Kaz()
        {
            return Mine("Alarms_kaz");
        }

        [CtsAuthorize(Roles = Roles.MineShahRoleName)]
        public ActionResult Alarms_Shah()
        {
            return Mine("Alarms_shah");
        }

        [CtsAuthorize(Roles = Roles.MineTentRoleName)]
        public ActionResult Alarms_Tent()
        {
            return Mine("Alarms_tent");
        }


        public ActionResult Alarm()
        {
            return View();
        }

        public ActionResult Alarm_other()
        {
            return View();
        }
	}
}