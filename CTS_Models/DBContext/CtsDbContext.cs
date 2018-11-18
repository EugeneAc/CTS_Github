using CTS_Models.DbViewModels;
using System.Data.Entity;
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices; // do not delete https://stackoverflow.com/questions/14033193/entity-framework-provider-type-could-not-be-loaded

namespace CTS_Models.DBContext
{
	public class CtsDbContext : DbContext
	{
		public CtsDbContext()
			: base("CentralDbConnection")
		{ }

		public CtsDbContext(string connectionString)
			: base(connectionString)
		{ }

		public virtual DbSet<WagonScale> WagonScales { get; set; }
		public virtual DbSet<WagonTransfer> WagonTransfers { get; set; }
		public virtual DbSet<Location> Locations { get; set; }
		public virtual DbSet<Item> Items { get; set; }
		public virtual DbSet<VehiScale> VehiScales { get; set; }
		public virtual DbSet<BeltScale> BeltScales { get; set; }
		public virtual DbSet<BeltTransfer> InternalTransfers { get; set; }
		public virtual DbSet<InnerDestination> InnerDestinations { get; set; }
		public virtual DbSet<Skip> Skips { get; set; }
		public virtual DbSet<SkipTransfer> SkipTransfers { get; set; }
		public virtual DbSet<VehiTransfer> VehiTransfers { get; set; }
		public virtual DbSet<WagonAnalysis> WagonAnalyzes { get; set; }
		public virtual DbSet<BeltAnalysis> BeltAnalyzes { get; set; }
		public virtual DbSet<SkipAnalysis> SkipAnalyzes { get; set; }
		public virtual DbSet<MiningAnalysis> MiningAnalyzes { get; set; }
		public virtual DbSet<RockUtil> RockUtils { get; set; }
		public virtual DbSet<RockUtilTransfer> RockUtilTransfers { get; set; }
		public virtual DbSet<SkipWeight> SkipWeights { get; set; }
		public virtual DbSet<BoilerConsNorm> BoilerConsNorms { get; set; }
		public virtual DbSet<Warehouse> Warehouses { get; set; }
		public virtual DbSet<WarehouseTransfer> WarehouseTransfers { get; set; }
		public virtual DbSet<WarehouseMeasure> WarehouseMeasures { get; set; }
		public virtual DbSet<Shift> Shifts { get; set; }
		public virtual DbSet<Recogn> Recogn { get; set; }
		public virtual DbSet<WagonNumsCache> WagonNumsCache { get; set; }
		public virtual DbSet<AlarmComment> AlarmComment { get; set; }


		public DbSet<OraclePlanWithShop_ID> OraclePlansWithShop_ID { get; set; }
		public DbSet<OralcleMiningPlanB> OralceMiningPlansB { get; set; }
		public DbSet<LocalPlanBWithLocationID> LocalPlansBWithLocationID { get; set; }
        public DbSet<LocalPlanWithLocationID> LocalPlansWithLocationID { get; set; }
        public DbSet<LocalMiningPlanB> LocalMiningPlansB { get; set; }
        public DbSet<LocalMiningPlan> LocalMiningPlans { get; set; }
        public DbSet<OracleStaff> OracleStaffs { get; set; }
        public DbSet<Staff> LocalStaffs { get; set; }
    }
}
