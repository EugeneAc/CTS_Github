namespace CTS_Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkipWeightsWagonNumsCacheIDNotGenerated : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.SkipWeights");
            DropPrimaryKey("dbo.WagonNumsCaches");
            AlterColumn("dbo.SkipWeights", "ID", c => c.Int(nullable: false));
            AlterColumn("dbo.WagonNumsCaches", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.SkipWeights", "ID");
            AddPrimaryKey("dbo.WagonNumsCaches", "ID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.WagonNumsCaches");
            DropPrimaryKey("dbo.SkipWeights");
            AlterColumn("dbo.WagonNumsCaches", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SkipWeights", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.WagonNumsCaches", "ID");
            AddPrimaryKey("dbo.SkipWeights", "ID");
        }
    }
}
