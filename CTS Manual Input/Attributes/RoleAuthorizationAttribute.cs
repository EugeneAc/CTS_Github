using CTS_Manual_Input.Helpers;
using System.Web.Mvc;

namespace CTS_Manual_Input.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class CanEditRoleAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool Match = UserHelper.CanEditUser(filterContext.HttpContext.User.Identity.Name);
            if (!Match)
            {
                filterContext.Result = new RedirectResult("Index");
            }

            base.OnAuthorization(filterContext);
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class CanDeleteRoleAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool Match = UserHelper.CanDeleteUser(filterContext.HttpContext.User.Identity.Name);
            if (!Match)
            {
                filterContext.Result = new RedirectResult("Index");
            }

            base.OnAuthorization(filterContext);
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class CanAddRoleAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool Match = UserHelper.CanAddUser(filterContext.HttpContext.User.Identity.Name);
            if (!Match)
            {
                filterContext.Result = new RedirectResult("Index");
            }

            base.OnAuthorization(filterContext);
        }
    }


	[System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class CanApproveRoleAuthorization : AuthorizeAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{

			bool Match = UserHelper.CanApproveUser(filterContext.HttpContext.User.Identity.Name);
			if (!Match)
			{
				filterContext.Result = new RedirectResult("Index");
			}

			base.OnAuthorization(filterContext);
		}
	}
}