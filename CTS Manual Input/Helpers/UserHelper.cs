using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using CTS_Manual_Input.Helpers;
using System.DirectoryServices.AccountManagement;

namespace CTS_Manual_Input.Helpers
{
	public static class UserHelper
	{
		public static List<string> GetUserDomainGroups(string userName)
		{
            var uName = userName.Split(new char[] { '\\' }).Last();

            var result = Cacher.Instance.TryRead(userName) as List<string>;
            if (result != null) return result;
            result = new List<string>();
            try
			{
				WindowsIdentity wi = new WindowsIdentity(uName);
				foreach (IdentityReference group in wi.Groups)
				{
					if (group.Translate(typeof(NTAccount)).ToString().Split(new char[] { '\\' }).Count() > 1)
						result.Add(group.Translate(typeof(NTAccount)).ToString().Split(new char[] { '\\' })[1]);
				}
				result.Sort();
			}
			catch (Exception)
			{
				// Get loacal machine groups when domain is not available
				PrincipalContext context = new PrincipalContext(ContextType.Machine);
				UserPrincipal user = UserPrincipal.FindByIdentity(context, userName);
				if (user != null)
				{
					var groups = user.GetAuthorizationGroups().Select(g => g.ToString());
					result.AddRange(groups);
				}
			}

#if DEBUG
			result.Add(@"test");
			result.Add(@"ш. Кузембаева");
			result.Add(@"ш. Костенко");
#endif
            Cacher.Instance.Write(userName, result);
            return result;
		}

		public static bool CanEditUser(string userName)
		{
			return MatchUserGoupWithRoleName(userName, ProjectConstants.EditUserRoleName);
		}

		public static bool CanDeleteUser(string userName)
		{
			return MatchUserGoupWithRoleName(userName, ProjectConstants.DeleteUserRoleName);
		}

        public static bool CanAddUser(string userName)
        {
            return MatchUserGoupWithRoleName(userName, ProjectConstants.AddUserRoleName);
        }

		public static bool CanApproveUser(string userName)
		{
			return MatchUserGoupWithRoleName(userName, ProjectConstants.ApproveUserRoleName);
		}

		public static bool SkipUser(string userName)
        {
            return MatchUserGoupWithRoleName(userName, ProjectConstants.SkipUserRoleName);
        }

        public static bool BeltUser(string userName)
        {
            return MatchUserGoupWithRoleName(userName, ProjectConstants.BeltUserRoleName);
        }

		internal static string GetOperatorName4DBInsertion(object userHostName, object name)
		{
			throw new NotImplementedException();
		}

		public static bool VehiUser(string userName)
        {
            return MatchUserGoupWithRoleName(userName, ProjectConstants.VehiUserRoleName);
        }

        public static bool LabUser(string userName)
        {
            return MatchUserGoupWithRoleName(userName, ProjectConstants.LabUserRoleName);
        }

        public static bool WagonUser(string userName)
        {
            return MatchUserGoupWithRoleName(userName, ProjectConstants.WagonUserRoleName);
        }

		public static bool DictUser(string userName)
		{
			return MatchUserGoupWithRoleName(userName, ProjectConstants.DictUserRoleName);
		}
		public static bool RockUser(string userName)
		{
			return MatchUserGoupWithRoleName(userName, ProjectConstants.RockUserRoleName);
		}

		public static bool WarehouseUser(string userName)
		{
			return MatchUserGoupWithRoleName(userName, ProjectConstants.WarehouseUserRoleName);
		}

		public static bool WarehouseSetUser(string userName)
		{
			return MatchUserGoupWithRoleName(userName, ProjectConstants.WarehouseSetUserRoleName);
		}

		public static string GetOperatorName4DBInsertion(string userHostName, string userIdentityName)
		{
			return Dns.GetHostEntry(userHostName).HostName + " (" + userHostName + ") \\ " + userIdentityName;
		}

		private static bool MatchUserGoupWithRoleName (string userName, string roleName)
		{
			List<string> UserDomainGroups = GetUserDomainGroups(userName);
				foreach (var group in UserDomainGroups)
				{
                  if (String.Equals(group, roleName, StringComparison.OrdinalIgnoreCase))
						return true;
				}

			if (UserDomainGroups.Contains("Administrators") || UserDomainGroups.Contains("Администраторы")) return true;

			return false;
		}
	}
}