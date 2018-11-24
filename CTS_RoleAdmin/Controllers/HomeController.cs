using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTS_Models;
using CTS_Models.DBContext;
using System.DirectoryServices.AccountManagement;
using CTS_Core;
using RoleAdmin.Handlers;
using System.Security.Principal;

namespace CTS_RoleAdmin.Controllers
{
	[CtsAuthorize(Roles = Roles.RoleAdminRoleName)]
	[Authorize]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{

			return View();
		}

		public ActionResult RoleList()
		{
			List<CtsUser> users;
			users = GetCtsUsers();

			return View(users);
		}

		public ActionResult EditUser(string userLogin)
		{
			var userWithHisGroups = GetUserGroup(userLogin);

			return View(userWithHisGroups);
		}

		[HttpPost]
		public ActionResult EditUser(CtsUser model)
		{





			return View();
		}

		public ActionResult CheckIfUserExists(string userLogin)
		{
			if (DomainUsersHandler.FindUser(userLogin, "kazprom") != null)
			{
				return Json(new { userFound = true });
			}

			return Json(new { userFound = false });
		}


		public ActionResult Add()
		{

			return View();
		}

		public ActionResult RolesForm(string userName)
		{

			return PartialView();
		}

		private CtsUser GetUserGroup(string userLogin)
		{
			var user = new CtsUser();
			user.ManualInputRoles = GetMIRoles();
			user.AnalyticsRoles = GetAnalyticsRoles();
			user.RoleAdminRoles = GetRoleAdminRoles();

			var ctx = new PrincipalContext(ContextType.Machine);

			foreach (var r in user.ManualInputRoles.Concat(user.AnalyticsRoles).Concat(user.RoleAdminRoles))
			{
				var foundGroup = GroupPrincipal.FindByIdentity(ctx, r.GroupName);
				var members = foundGroup.GetMembers().ToList();
				foreach (var p in foundGroup.GetMembers())
				{
					if (p.Name == userLogin)
					{
						r.HasRole = true;
					}
				}
			}

			return user;
		}

		private List<CtsRole> GetMIRoles()
		{
			//Add hardcoded roles from CTS_Models.Roles:
			var ctsRoles = new List<CtsRole>()
			{   new CtsRole() { GroupName = Roles.AddUserRoleName, GroupDescription = "Добавление" },
				new CtsRole() { GroupName = Roles.EditUserRoleName, GroupDescription = "Редактирование" },
				new CtsRole() { GroupName = Roles.DeleteUserRoleName, GroupDescription = "Удаление" },
				new CtsRole() { GroupName = Roles.ApproveUserRoleName, GroupDescription = "Подтверждение" },
				new CtsRole() { GroupName = Roles.SkipUserRoleName, GroupDescription = "Скипы" },
				new CtsRole() { GroupName = Roles.BeltUserRoleName, GroupDescription = "Конв. весы" },
				new CtsRole() { GroupName = Roles.WagonUserRoleName, GroupDescription = "Вагон. весы" },
				new CtsRole() { GroupName = Roles.LabUserRoleName, GroupDescription = "Лаборатория" },
				new CtsRole() { GroupName = Roles.VehiUserRoleName, GroupDescription = "Авто весы" },
				new CtsRole() { GroupName = Roles.DictUserRoleName, GroupDescription = "Словари" },
				new CtsRole() { GroupName = Roles.RockUserRoleName, GroupDescription = "Утил.породы" },
				new CtsRole() { GroupName = Roles.WarehouseUserRoleName, GroupDescription = "Замер склада" },
				new CtsRole() { GroupName = Roles.WarehouseSetUserRoleName, GroupDescription = "Уст.баланс склада" },
			};

			//Add roles corresponding to CTS locations:
			var cdb = new CtsDbContext();
			var locations = cdb.Locations.Where(x => x.ID != "test").ToList();
			foreach (var loc in locations)
			{
				ctsRoles.Add(new CtsRole { GroupName = loc.LocationName.Substring(loc.LocationName.LastIndexOf(' ') + 1), GroupDescription = loc.LocationName });
			}

			return ctsRoles;
		}

		private List<CtsRole> GetAnalyticsRoles()
		{
			//Add hardcoded roles from CTS_Models.Roles:
			var ctsRoles = new List<CtsRole>()
			{
				new CtsRole { GroupName = Roles.AnalyticsRoleName, GroupDescription = "Сайт Аналитики" },
			};

			return ctsRoles;
		}

		private List<CtsRole> GetRoleAdminRoles()
		{
			//Add hardcoded roles from CTS_Models.Roles:
			var ctsRoles = new List<CtsRole>()
			{
				new CtsRole { GroupName = Roles.RoleAdminRoleName, GroupDescription = "Упр-е ролями CTS" },
			};

			return ctsRoles;
		}

		private List<CtsUser> GetCtsUsers()
		{
			var users = new List<CtsUser>();
			var miRoles = GetMIRoles();
			var analyticsRoles = GetAnalyticsRoles();
			var roleAdminRoles = GetRoleAdminRoles();

			var ctx = new PrincipalContext(ContextType.Machine);
			//check every declared group to be a cts role
			foreach (var r in miRoles.Concat(analyticsRoles).Concat(roleAdminRoles))
			{
				//try to find a local group with such a name
				var foundGroup = GroupPrincipal.FindByIdentity(ctx, r.GroupName);
				if (foundGroup != null)
				{
					//pick out every person from found group and add him to ctsUsers list
					foreach (Principal p in foundGroup.GetMembers())
					{
						var ctsUser = users.Find(x => x.UserName == p.Name);
						if (ctsUser == null)
						{
							//if there's still no such a user in ctsUser list, add him there
							var newUser = new CtsUser()
							{
								UserName = p.Name,
								AnalyticsRoles = new List<CtsRole>(analyticsRoles.Select(item => (CtsRole)item.Clone()).ToList()),
	
							RoleAdminRoles = new List<CtsRole>(roleAdminRoles.Select(item => (CtsRole)item.Clone()).ToList()),
								ManualInputRoles = new List<CtsRole>(miRoles.Select(item => (CtsRole)item.Clone()).ToList())
							};
							users.Add(newUser);
							ctsUser = newUser;
						}

						//mark user to have this role
						var group = ctsUser.ManualInputRoles.Concat(ctsUser.AnalyticsRoles).Concat(roleAdminRoles).Where(x => x.GroupName == r.GroupName).FirstOrDefault();
						group.HasRole = true;
					}
				}
			}

			return users;
		}



	}
}