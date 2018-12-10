using CTS_Models;
using CTS_Models.DBContext;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace CTS_Core
{
	public static class CtsAuthorizeProvider
	{
		public static ICollection<CtsRole> GetUserRolesFromDb(IIdentity user)
		{
			var cdb = new CtsDbContext();
			var userLogin = user.Name.Split(new char[] { '\\' }).Last();
			var domain = user.Name.Split(new char[] { '\\' }).First();

			if (!(Cacher.Instance.TryRead(domain + userLogin + "Roles") is CtsUser ctsUser))
			{
				ctsUser = cdb.CtsUser.Where(x => x.Login == userLogin).FirstOrDefault(x => x.Domain == domain);
				if (ctsUser != null)
				{
					cdb.Entry(ctsUser).Collection(x => x.CtsRoles).Load();
					Cacher.Instance.Write(domain + userLogin + "Roles", ctsUser);
				}
				else
				{
					ctsUser = new CtsUser();
				}
			}

			return ctsUser.CtsRoles;
		}

		public static bool CheckIsInRole(IIdentity user, params string[] groups)
		{
#if DEBUG
			return true;
#endif
			var roles = GetUserRolesFromDb(user);
			return roles.Any(x => groups.Contains(x.RoleName));
		}

		public static bool CanEditUser(IIdentity user)
		{
			return CheckIsInRole(user, Roles.EditUserRoleName);
		}

		public static bool CanDeleteUser(IIdentity user)
		{
			return CheckIsInRole(user, Roles.DeleteUserRoleName);
		}

		public static bool CanAddUser(IIdentity user)
		{
			return CheckIsInRole(user, Roles.AddUserRoleName);
		}

		public static bool CanApproveUser(IIdentity user)
		{
			return CheckIsInRole(user, Roles.ApproveUserRoleName);
		}
	}
}
