using System.Data.Entity;

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
