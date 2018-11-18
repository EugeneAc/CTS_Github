using System.Data.Entity;
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices; // do not delete https://stackoverflow.com/questions/14033193/entity-framework-provider-type-could-not-be-loaded

namespace CTS_Models.DBContext
{
	public class CtsTransferContext<T> : DbContext where T : class, ITransfer
	{
		public CtsTransferContext()
			: base("CentralDbConnection")
		{ }

		public CtsTransferContext(string connectionString)
			: base(connectionString)
		{ }

		public DbSet<T> DbSet { get; set; }
	}
}
