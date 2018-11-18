﻿using System.Data.Entity;
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;

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
