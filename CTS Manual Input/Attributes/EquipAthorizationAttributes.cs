using CTS_Manual_Input.Helpers;
using System.Web.Mvc;

namespace CTS_Manual_Input.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class SkipUserAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool Match = UserHelper.SkipUser(filterContext.HttpContext.User.Identity.Name);
            if (!Match)
            {
                filterContext.Result = new RedirectResult("Index");
            }

            base.OnAuthorization(filterContext);
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class LabUserAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool Match = UserHelper.LabUser(filterContext.HttpContext.User.Identity.Name);
            if (!Match)
            {
                filterContext.Result = new RedirectResult("Index");
            }

            base.OnAuthorization(filterContext);
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class BeltUserAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool Match = UserHelper.BeltUser(filterContext.HttpContext.User.Identity.Name);
            if (!Match)
            {
                filterContext.Result = new RedirectResult("Index");
            }

            base.OnAuthorization(filterContext);
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class WagonUserAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool Match = UserHelper.WagonUser(filterContext.HttpContext.User.Identity.Name);
            if (!Match)
            {
                filterContext.Result = new RedirectResult("Index");
            }

            base.OnAuthorization(filterContext);
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class VehiUserAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool Match = UserHelper.VehiUser(filterContext.HttpContext.User.Identity.Name);
            if (!Match)
            {
                filterContext.Result = new RedirectResult("Index");
            }

            base.OnAuthorization(filterContext);
        }
    }

	[System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class DictUserAuthorization : AuthorizeAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{

			bool Match = UserHelper.DictUser(filterContext.HttpContext.User.Identity.Name);
			if (!Match)
			{
				filterContext.Result = new RedirectResult("Index");
			}

			base.OnAuthorization(filterContext);
		}
	}

	[System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class RockUserAuthorization : AuthorizeAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{

			bool Match = UserHelper.RockUser(filterContext.HttpContext.User.Identity.Name);
			if (!Match)
			{
				filterContext.Result = new RedirectResult("Index");
			}

			base.OnAuthorization(filterContext);
		}
	}

	[System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class WarehouseUserAuthorization : AuthorizeAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{

			bool Match = UserHelper.WarehouseUser(filterContext.HttpContext.User.Identity.Name);
			if (!Match)
			{
				filterContext.Result = new RedirectResult("Index");
			}

			base.OnAuthorization(filterContext);
		}
	}

	[System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class WarehouseSetUserAuthorization : AuthorizeAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{

			bool Match = UserHelper.WarehouseSetUser(filterContext.HttpContext.User.Identity.Name);
			if (!Match)
			{
				filterContext.Result = new RedirectResult("Index");
			}

			base.OnAuthorization(filterContext);
		}
	}
}