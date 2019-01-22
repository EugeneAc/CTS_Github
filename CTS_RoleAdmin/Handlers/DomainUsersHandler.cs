using System;
using System.DirectoryServices.AccountManagement;
using System.Web.Hosting;

namespace RoleAdmin.Handlers
{
	public static class DomainUsersHandler
	{
		public static UserPrincipal FindUser(string userLogin, string domain)
		{
            UserPrincipal user = null;

            //using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain, "y.aniskina", "rty-561"))
            //{
            //	user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, userLogin);
            //}
            try
            {
                using (HostingEnvironment.Impersonate())
                {
                    var context = new PrincipalContext(ContextType.Domain, domain.ToLower(), null, ContextOptions.Negotiate | ContextOptions.SecureSocketLayer);
                    user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userLogin);
                }
            }
            catch (Exception e)
            {

            }
			
			return user;
		}
	}
}
