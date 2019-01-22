using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTS_Models;
using CTS_Models.DBContext;
using System.DirectoryServices.AccountManagement;
using CTS_Core;
using RoleAdmin.Handlers;
using System.Security.Principal;
using CTS_RoleAdmin.Models;
using System.Configuration;

namespace CTS_RoleAdmin.Controllers
{
	[CtsAuthorize(Roles = Roles.RoleAdminRoleName)]
	public class HomeController : Controller
	{
	    private CtsDbContext _cdb = new CtsDbContext();

        public ActionResult Index()
		{

			return View();
		}

		public ActionResult RoleList()
		{
            var model = new RolesIndexViewModel
            {
                AllCtsRoles = _cdb.CtsRole.ToList(),
                AllCtsUsers = _cdb.CtsUser.Include(m => m.CtsRoles).ToList()
            };

            return View(model);
		}

		public ActionResult AddEditUser(string userLogin, string userDomain)
		{
		    AddEditUserViewModel model;
		    var ctsRoles = _cdb.CtsRole;
            var user = _cdb.CtsUser.Find(userLogin, userDomain);
		    if (user == null)
		    {
		        model = new AddEditUserViewModel(userLogin, userDomain, ctsRoles.ToList());
		    }
		    else
		    {
		        _cdb.Entry(user).Collection(x => x.CtsRoles).Load();
                model = new AddEditUserViewModel(user, ctsRoles.ToList());
		    }

			return View(model);
		}

		[HttpPost]
		public ActionResult AddEditUser(AddEditUserViewModel model)
		{
            var user = _cdb.CtsUser.Include(s => s.CtsRoles)
                .Where(x => x.Login == model.UserLogin)
                .FirstOrDefault(s => s.Domain == model.UserDomain);
            if (user == null)
		    {
		        user = new CtsUser() { Login = model.UserLogin, Domain = model.UserDomain };
		        _cdb.CtsUser.Add(user);
		    }

            user.CtsRoles.Clear();
            user.CtsRoles = model.CtsRoles?
                .Where(x => x.Value)
                .Select(x => _cdb.CtsRole.Find(x.Key))
                .ToList();
            _cdb.SaveChanges();

            return RedirectToAction("RoleList");
		}

	    public ActionResult DeleteUser(string userLogin, string userDomain)
	    {
	        var user = _cdb.CtsUser.Find(userLogin, userDomain);
	        if (user == null)
	        {
	            return HttpNotFound("Пользователь не найден");
	        }

	        foreach (var role in user.CtsRoles)
	        {
	            user.CtsRoles.Remove(role);
	        }

	        _cdb.CtsUser.Remove(user);
	        _cdb.SaveChanges();

	        return RedirectToAction("RoleList");
        }

	    public ActionResult CheckIfUserExists(string userLogin)
	    {
	       UserPrincipal user;
            var domains = ConfigurationManager.AppSettings["DomainNames"].Split(',', ';').Select(s => s.Trim());

           foreach (var domain in domains)
		   {
		       user = DomainUsersHandler.FindUser(userLogin, domain.ToString());
               if (user != null)
		       {
		           return Json(new { userFound = true, domain = user.Context.Name});
		       }
            }

			return Json(new { userFound = false });
		}

		//private CtsUserAdmin GetUserGroup(string userLogin)
		//{
		//	var user = new CtsUserAdmin();
		//	user.ManualInputRoles = GetMIRoles();
		//	user.AnalyticsRoles = GetAnalyticsRoles();
		//	user.RoleAdminRoles = GetRoleAdminRoles();

		//	var ctx = new PrincipalContext(ContextType.Machine);

		//	foreach (var r in user.ManualInputRoles.Concat(user.AnalyticsRoles).Concat(user.RoleAdminRoles))
		//	{
		//		var foundGroup = GroupPrincipal.FindByIdentity(ctx, r.GroupName);
		//		var members = foundGroup.GetMembers().ToList();
		//		foreach (var p in foundGroup.GetMembers())
		//		{
		//			if (p.Name == userLogin)
		//			{
		//				r.HasRole = true;
		//			}
		//		}
		//	}

		//	return user;
		//}

		//private List<CtsRoleAdmin> GetMIRoles()
		//{
		//	//Add hardcoded roles from CTS_Models.Roles:
		//	var ctsRoles = new List<CtsRoleAdmin>()
		//	{   new CtsRoleAdmin() { GroupName = Roles.AddUserRoleName, GroupDescription = "Добавление" },
		//		new CtsRoleAdmin() { GroupName = Roles.EditUserRoleName, GroupDescription = "Редактирование" },
		//		new CtsRoleAdmin() { GroupName = Roles.DeleteUserRoleName, GroupDescription = "Удаление" },
		//		new CtsRoleAdmin() { GroupName = Roles.ApproveUserRoleName, GroupDescription = "Подтверждение" },
		//		new CtsRoleAdmin() { GroupName = Roles.SkipUserRoleName, GroupDescription = "Скипы" },
		//		new CtsRoleAdmin() { GroupName = Roles.BeltUserRoleName, GroupDescription = "Конв. весы" },
		//		new CtsRoleAdmin() { GroupName = Roles.WagonUserRoleName, GroupDescription = "Вагон. весы" },
		//		new CtsRoleAdmin() { GroupName = Roles.LabUserRoleName, GroupDescription = "Лаборатория" },
		//		new CtsRoleAdmin() { GroupName = Roles.VehiUserRoleName, GroupDescription = "Авто весы" },
		//		new CtsRoleAdmin() { GroupName = Roles.DictUserRoleName, GroupDescription = "Словари" },
		//		new CtsRoleAdmin() { GroupName = Roles.RockUserRoleName, GroupDescription = "Утил.породы" },
		//		new CtsRoleAdmin() { GroupName = Roles.WarehouseUserRoleName, GroupDescription = "Замер склада" },
		//		new CtsRoleAdmin() { GroupName = Roles.WarehouseSetUserRoleName, GroupDescription = "Уст.баланс склада" },
		//	};

		//	//Add roles corresponding to CTS locations:
		//	var cdb = new CtsDbContext();
		//	var locations = cdb.Locations.Where(x => x.ID != "test").ToList();
		//	foreach (var loc in locations)
		//	{
		//		ctsRoles.Add(new CtsRoleAdmin { GroupName = loc.LocationName.Substring(loc.LocationName.LastIndexOf(' ') + 1), GroupDescription = loc.LocationName });
		//	}

		//	return ctsRoles;
		//}

		//private List<CtsRoleAdmin> GetAnalyticsRoles()
		//{
		//	//Add hardcoded roles from CTS_Models.Roles:
		//	var ctsRoles = new List<CtsRoleAdmin>()
		//	{
		//		new CtsRoleAdmin { GroupName = Roles.AnalyticsRoleName, GroupDescription = "Сайт Аналитики" },
		//	};

		//	return ctsRoles;
		//}

		//private List<CtsRoleAdmin> GetRoleAdminRoles()
		//{
		//	//Add hardcoded roles from CTS_Models.Roles:
		//	var ctsRoles = new List<CtsRoleAdmin>()
		//	{
		//		new CtsRoleAdmin { GroupName = Roles.RoleAdminRoleName, GroupDescription = "Упр-е ролями CTS" },
		//	};

		//	return ctsRoles;
		//}

		//private List<CtsUserAdmin> GetCtsUsers()
		//{
		//	var users = new List<CtsUserAdmin>();
		//	var miRoles = GetMIRoles();
		//	var analyticsRoles = GetAnalyticsRoles();
		//	var roleAdminRoles = GetRoleAdminRoles();

		//	var ctx = new PrincipalContext(ContextType.Machine);
		//	//check every declared group to be a cts role
		//	foreach (var r in miRoles.Concat(analyticsRoles).Concat(roleAdminRoles))
		//	{
		//		//try to find a local group with such a name
		//		var foundGroup = GroupPrincipal.FindByIdentity(ctx, r.GroupName);
		//		if (foundGroup != null)
		//		{
		//			//pick out every person from found group and add him to ctsUsers list
		//			foreach (Principal p in foundGroup.GetMembers())
		//			{
		//				var ctsUser = users.Find(x => x.UserLogin == p.Name);
		//				if (ctsUser == null)
		//				{
		//					//if there's still no such a user in ctsUser list, add him there
		//					var newUser = new CtsUserAdmin()
		//					{
		//						UserLogin = p.Name,
		//						AnalyticsRoles = new List<CtsRoleAdmin>(analyticsRoles.Select(item => (CtsRoleAdmin)item.Clone()).ToList()),
	
		//					RoleAdminRoles = new List<CtsRoleAdmin>(roleAdminRoles.Select(item => (CtsRoleAdmin)item.Clone()).ToList()),
		//						ManualInputRoles = new List<CtsRoleAdmin>(miRoles.Select(item => (CtsRoleAdmin)item.Clone()).ToList())
		//					};
		//					users.Add(newUser);
		//					ctsUser = newUser;
		//				}

		//				//mark user to have this role
		//				var group = ctsUser.ManualInputRoles.Concat(ctsUser.AnalyticsRoles).Concat(roleAdminRoles).Where(x => x.GroupName == r.GroupName).FirstOrDefault();
		//				group.HasRole = true;
		//			}
		//		}
		//	}

		//	return users;
		//}



	}
}