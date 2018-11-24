using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CTS_Core
{
	public class CtsAuthorizeAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			bool disableAuthentication = false;
			var rtrtrrt = base.Roles;
#if DEBUG
			disableAuthentication = true;
#endif

			if (disableAuthentication)
				return true;

			return base.AuthorizeCore(httpContext);
		}

	}
}
