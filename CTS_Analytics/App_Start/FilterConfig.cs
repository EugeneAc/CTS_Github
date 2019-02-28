using StackExchange.Profiling.Mvc;
using System.Web.Mvc;

namespace CTS_Analytics
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
            filters.Add(new ProfilingActionFilter());
        }
    }
}
