using CTS_Core;
using CTS_Models;
using CTS_Models.DBContext;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace CTS_Manual_Input.Helpers
{
    public static class EquipmentProvider
    {
        public static List<T> GetUserAuthorizedEquipment<T>(CtsDbContext cdb, IIdentity user) where T : class, IEquip
        {
			var userLogin = user.Name.Split(new char[] { '\\' }).Last();
			var domain = user.Name.Split(new char[] { '\\' }).First();
			string equipName = typeof(T).ToString();
            var equipment = Cacher.Instance.TryRead(domain + userLogin + equipName) as List<T>;
			if (equipment == null)
			{
				equipment = new List<T>();
				var locations = GetUserLocations(cdb, user);
				if (locations != null)
				{
					var locationsArray = locations.Select(x => x.ID).ToList();
					var genericContext = new CtsEquipContext<T>();
					equipment.AddRange(genericContext.DbSet.Where(n => locationsArray.Contains(n.LocationID)).ToList());
				}
				if (equipment.Count > 0)
				{
					Cacher.Instance.Write(domain + userLogin + equipName, equipment);
				}
			}

            return equipment;
        }

		public static List<Location> GetUserLocations(CtsDbContext cdb, IIdentity user)
		{
			var userLogin = user.Name.Split(new char[] { '\\' }).Last();
			var domain = user.Name.Split(new char[] { '\\' }).First();
			if (!(Cacher.Instance.TryRead(domain + userLogin + "Locations") is List<Location> locations))
			{
				locations = new List<Location>();
				var groups = CtsAuthorizeProvider.GetUserRolesFromDb(user).Select(x => x.RoleName).ToArray();
				locations.AddRange(cdb.Locations.Where(n => groups.Contains(n.LocationName)).ToList());
				if (locations.Count > 0)
				{
					Cacher.Instance.Write(domain + userLogin + "Locations", locations);
				}
			}

			return locations;
		}
		public static List<Item> GetItems(CtsDbContext cdb, IIdentity user)
		{
			var userLogin = user.Name.Split(new char[] { '\\' }).Last();
			var domain = user.Name.Split(new char[] { '\\' }).First();
			if (!(Cacher.Instance.TryRead(domain + userLogin + "Items") is List<Item> items))
			{
				items = new List<Item>();
				var groups = CtsAuthorizeProvider.GetUserRolesFromDb(user).Select(x => x.RoleName).ToArray();
				items.AddRange(cdb.Items.Where(n => groups.Contains(n.Location.DomainName)).ToList());

				if (items.Count > 0)
				{
					Cacher.Instance.Write(domain + userLogin + "Items", items);
				}
			}

			return items;
		}
	}
}


