namespace CTS_Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rmvuserRole : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.UserRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SystemName = c.String(nullable: false),
                        DomainRoleName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
