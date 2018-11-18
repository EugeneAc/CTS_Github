using System.Data.Entity;
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices; // do not delete https://stackoverflow.com/questions/14033193/entity-framework-provider-type-could-not-be-loaded

namespace CTS_Models.DBContext
{
	public class CtsUniversalContext<T> : DbContext where T : class
	{
		public CtsUniversalContext()
			: base("CentralDbConnection")
		{ }

		public CtsUniversalContext(string connectionString)
			: base(connectionString)
		{ }

		public DbSet<T> DbSet { get; set; }
	}
}
