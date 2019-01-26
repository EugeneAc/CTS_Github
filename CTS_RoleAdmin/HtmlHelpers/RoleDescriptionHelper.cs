using System.Configuration;
using System.Web.Mvc;

namespace CTS_RoleAdmin.HtmlHelpers
{
    public static class RoleDescriptionHelper
    {
        public static MvcHtmlString GetRoleDescription(this HtmlHelper html, string roleName)
        {
            TagBuilder span = new TagBuilder("span");
            span.SetInnerText(ConfigurationManager.AppSettings[roleName]);
            return new MvcHtmlString(span.ToString());
        }
    }
}