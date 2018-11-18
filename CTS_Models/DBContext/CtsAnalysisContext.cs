﻿using System.Data.Entity;
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;

namespace CTS_Models.DBContext
{
	public class CtsAnalysisContext<T> : DbContext where T : class, IAnalysis
	{
		public CtsAnalysisContext()
			: base("CentralDbConnection")
		{ }

		public CtsAnalysisContext(string connectionString)
			: base(connectionString)
		{ }

		public DbSet<T> DbSet { get; set; }
	}
}
