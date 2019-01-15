using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTS_Models;

namespace CTS_RoleAdmin.Models
{
    public class AddEditUserViewModel
    {
        public Dictionary<CtsRole, bool> CtsRoles { get; set; }
        public string UserLogin { get; set; }
        public string UserDomain { get; set; }

        public AddEditUserViewModel() { }

        public AddEditUserViewModel(string userName, string userDomain, List<CtsRole> ctsRoles)
        {
            UserLogin = userName;
            UserDomain = userDomain;
            CtsRoles = ctsRoles.ToDictionary(x => x, x => false);
        }
        public AddEditUserViewModel(CtsUser user, List<CtsRole> ctsRoles)
        {
            UserLogin = user.Login;
            UserDomain = user.Domain;
            CtsRoles = ctsRoles.ToDictionary(x => x, x => user.CtsRoles.Contains(x));
        }
    }
}