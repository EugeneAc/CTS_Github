using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CTS_Models;
using CTS_Models.DBContext;

using System.Configuration;
using System.Security.Principal;
using CTS_Core;

namespace CTS_Analytics.Helpers
{
    public static class ReportRoleHelper
    {
        public static bool UserHasAnyReportRole(IIdentity user)
        {
            if (Cacher.Instance.TryRead(user.Name + "CheckUserReportRole") is bool)
                return (bool)Cacher.Instance.TryRead(user.Name + "CheckUserReportRole");

            var userHasReportRole = CtsAuthorizeProvider.CheckIsInRole(user, GetReprtRoles());
            Cacher.Instance.Write(user.Name + "CheckUserReportRole", userHasReportRole);
            return userHasReportRole;
        }

        private static string[] GetReprtRoles()
        {
            var cdb = new CtsDbContext();
            return cdb.CtsRole
                   .Where(r => r.RoleName.ToLower().StartsWith("rep"))
                   .Select(r => r.RoleName)
                  .ToArray();
        }

        public static bool UserHasLocationRole(IIdentity user, string locationID)
        {
            var cacheKey = user.Name + "UserHasLocationRole" + locationID;
            if (Cacher.Instance.TryRead(cacheKey) is bool)
                return (bool)Cacher.Instance.TryRead(cacheKey);

            var cdb = new CtsDbContext();
            var userHasLocationRole = CtsAuthorizeProvider.CheckIsInRole(user, cdb.Locations.Find(locationID).LocationName);
            Cacher.Instance.Write(cacheKey, userHasLocationRole);
            return userHasLocationRole;
        }
    }
}