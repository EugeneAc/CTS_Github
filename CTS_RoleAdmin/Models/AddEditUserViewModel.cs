using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTS_Models;

namespace CTS_RoleAdmin.Models
{
    public class AddEditUserViewModel
    {
        public Dictionary<string, bool> CtsRoles { get; set; }
        public string UserLogin { get; set; }
        public string UserDomain { get; set; }

        public string ReturnView { get; set; }
        public bool AllRoles { get; set; }

        public AddEditUserViewModel() { }

        public AddEditUserViewModel(string userName, string userDomain, List<CtsRole> ctsRoles)
        {
            UserLogin = userName;
            UserDomain = userDomain;
            CtsRoles = ctsRoles.ToDictionary(x => x.RoleName, x => false);
        }
        public AddEditUserViewModel(CtsUser user, List<CtsRole> ctsRoles)
        {
            UserLogin = user.Login;
            UserDomain = user.Domain;
            CtsRoles = ctsRoles.ToDictionary(x => x.RoleName, x => user.CtsRoles.Contains(x));
        }
    }
}