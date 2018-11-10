using System.Web;
using System.Web.Mvc;

namespace CTS_Manual_Input
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
