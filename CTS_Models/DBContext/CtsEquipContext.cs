using System.Data.Entity;
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;

namespace CTS_Models.DBContext
{
	public class CtsEquipContext<T> : DbContext where T : class, IEquip
	{
		public CtsEquipContext()
			: base("CentralDbConnection")
		{ }

		public CtsEquipContext(string connectionString)
			: base(connectionString)
		{ }

		public DbSet<T> DbSet { get; set; }
	}
}
