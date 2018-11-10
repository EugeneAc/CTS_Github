using CTS_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataEmulator
{
  public class CentralDBContext : DbContext
  {
    public CentralDBContext()
        : base("CentralDbConnection")
    { }

    static CentralDBContext()
    {
      //Database.SetInitializer<WeightContext>(new ContextInitializer());
    }

    public DbSet<WagonScale> WagonScales { get; set; }
    public DbSet<WagonTransfer> WagonTransfers { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<VehiScale> VehiScales { get; set; }
    public DbSet<BeltScale> BeltScales { get; set; }
    public DbSet<BeltTransfer> BeltTransfers { get; set; }
    public DbSet<InnerDestination> InnerDestinations { get; set; }
    public DbSet<Skip> Skips { get; set; }
    public DbSet<SkipTransfer> SkipTransfers { get; set; }
    public DbSet<VehiTransfer> VehiTransfers { get; set; }
  }

  class ContextInitializer : DropCreateDatabaseIfModelChanges<CentralDBContext>
  {
    protected override void Seed(CentralDBContext db)
    {

    }

  }
}