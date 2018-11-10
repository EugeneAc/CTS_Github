using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTS_Models;
using CTS_Models.DBContext;
using System.DirectoryServices.AccountManagement;

namespace CTS_RoleAdmin.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult RoleList()
		{

			return View(GetCtsUsers());
		}

		public ActionResult FindUser()
		{

			return View();
		}

		[HttpPost]
		public ActionResult FindUser(string domainUser)
		{
			using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "yourdomain.com")
			
				using (UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, domainUser)
				{
				if (user != null)
				{
					//user will contain information such as name, email, phone etc..
				}
			}
		



			return View();
		}




		public ActionResult Add()
		{

			return View();
		}

		public ActionResult RolesForm(string userName)
		{

			return PartialView();
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

		private List<CtsUser> GetCtsUsers()
		{
			var users = new List<CtsUser>();
			var miRoles = GetMIRoles();
			var analyticsRoles = GetAnalyticsRoles();

			var ctx = new PrincipalContext(ContextType.Machine);
			foreach (var r in miRoles.Concat(analyticsRoles))
			{
				var foundGroup = GroupPrincipal.FindByIdentity(ctx, r.GroupName);
				if (foundGroup != null)
				{
					foreach (Principal p in foundGroup.GetMembers())
					{
						var ctsUser = users.Find(x => x.UserName == p.Name);
						if (ctsUser == null)
						{
							var newUser = new CtsUser()
							{
								UserName = p.Name,
								AnalyticsRoles = new List<CtsRole>(analyticsRoles.Select(item => (CtsRole)item.Clone()).ToList()),
								ManualInputRoles = new List<CtsRole>(miRoles.Select(item => (CtsRole)item.Clone()).ToList())
							};
							users.Add(newUser);
							ctsUser = newUser;
						}

						var group = ctsUser.ManualInputRoles.Concat(ctsUser.AnalyticsRoles).Where(x => x.GroupName == r.GroupName).FirstOrDefault();
						group.HasRole = true;
					}
				}
			}

			return users;
		}



	}
}