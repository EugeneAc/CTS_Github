using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTS_Models;

namespace CTS_RoleAdmin.Models
{
    public class RolesIndexViewModel
    {
        public List<CTS_Models.CtsRole> AllCtsRoles { get; set; }
        public List<CTS_Models.CtsUser> AllCtsUsers { get; set; }
    }
}