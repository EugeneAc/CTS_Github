namespace CTS_Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Shifts",
            //    c => new
            //        {
            //            Shift = c.Int(nullable: false),
            //            LocationID = c.String(nullable: false, maxLength: 255),
            //            TimeStart = c.Time(nullable: false, precision: 7),
            //        })
            //    .PrimaryKey(t => new { t.Shift, t.LocationID })
            //    .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
            //    .Index(t => t.LocationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shifts", "LocationID", "dbo.Locations");
            DropIndex("dbo.Shifts", new[] { "LocationID" });
            DropTable("dbo.Shifts");
        }
    }
}
