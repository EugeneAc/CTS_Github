using CTS_Models;
using CTS_Models.DBContext;
using System.Collections.Generic;
using System.Linq;

namespace CTS_Manual_Input.Helpers
{
    public static class EquipmentProvider
    {
        public static List<T> GetUserAuthorizedEquipment<T>(CtsDbContext _cdb, string userName) where T : class, IEquip
        {
            string equipName = typeof(T).ToString();
            var equipment = Cacher.Instance.TryRead(userName + equipName) as List<T>;
			if (equipment == null)
			{
				equipment = new List<T>();
				var locations = GetUserLocations(_cdb, userName);
				if (locations != null)
				{
					var locationsArray = locations.Select(x => x.ID).ToList();
					var genericContext = new CtsEquipContext<T>();
					equipment.AddRange(genericContext.DbSet.Where(n => locationsArray.Contains(n.LocationID)).ToList());
				}

				if (equipment.Count > 0)
				{
					Cacher.Instance.Write(userName + equipName, equipment);
				}
			}

            return equipment;
        }

		public static List<Location> GetUserLocations(CtsDbContext cdb, string userName)
		{
			var locations = Cacher.Instance.TryRead(userName + "Locations") as List<Location>;
			if (locations == null)
			{
				locations = new List<Location>();
				var groups = UserHelper.GetUserDomainGroups(userName);
                var t = cdb.Locations.ToList();
                locations.AddRange(cdb.Locations.Where(n => groups.Contains(n.DomainName)).ToList());

				if (locations.Count > 0)
				{
					Cacher.Instance.Write(userName + "Locations", locations);
				}
			}

			return locations;
		}
		public static List<Item> GetItems(CtsDbContext _cdb, string userName)
		{
			var items = Cacher.Instance.TryRead(userName + "Items") as List<Item>;
			if (items == null)
			{
				items = new List<Item>();
				var groups = UserHelper.GetUserDomainGroups(userName);
				items.AddRange(_cdb.Items.Where(n => groups.Contains(n.Location.DomainName)).ToList());

				if (items.Count > 0)
				{
					Cacher.Instance.Write(userName + "Items", items);
				}
			}

			return items;
		}
	}
}


