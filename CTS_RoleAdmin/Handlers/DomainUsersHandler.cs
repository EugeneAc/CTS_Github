using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;

namespace RoleAdmin.Handlers
{
	public static class DomainUsersHandler
	{
		public static UserPrincipal FindUser(string userLogin, string domain)
		{
			UserPrincipal user;

			//using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain, "y.aniskina", "rty-561"))
			//{
			//	user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, userLogin);
			//}

			using (HostingEnvironment.Impersonate())
			{
				var context = new PrincipalContext(ContextType.Domain, "kazprom", null, ContextOptions.Negotiate | ContextOptions.SecureSocketLayer);
				user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userLogin);
			}

			return user;
		}


	}
}
