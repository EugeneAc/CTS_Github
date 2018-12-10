using System.Web;
using System.Web.Mvc;


namespace CTS_Core
{
	public class CtsAuthorizeAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{

			return CtsAuthorizeProvider.CheckIsInRole(httpContext.User.Identity, base.Roles) || base.AuthorizeCore(httpContext);
		}
	}
}
