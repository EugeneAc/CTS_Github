namespace CTS_Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CtsRoles",
                c => new
                    {
                        RoleName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.RoleName);
            
            CreateTable(
                "dbo.CtsUsers",
                c => new
                    {
                        Login = c.String(nullable: false, maxLength: 128),
                        Domain = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Login, t.Domain });
            
            CreateTable(
                "dbo.CtsUserCtsRoles",
                c => new
                    {
                        CtsUser_Login = c.String(nullable: false, maxLength: 128),
                        CtsUser_Domain = c.String(nullable: false, maxLength: 128),
                        CtsRole_RoleName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CtsUser_Login, t.CtsUser_Domain, t.CtsRole_RoleName })
                .ForeignKey("dbo.CtsUsers", t => new { t.CtsUser_Login, t.CtsUser_Domain }, cascadeDelete: true)
                .ForeignKey("dbo.CtsRoles", t => t.CtsRole_RoleName, cascadeDelete: true)
                .Index(t => new { t.CtsUser_Login, t.CtsUser_Domain })
                .Index(t => t.CtsRole_RoleName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CtsUserCtsRoles", "CtsRole_RoleName", "dbo.CtsRoles");
            DropForeignKey("dbo.CtsUserCtsRoles", new[] { "CtsUser_Login", "CtsUser_Domain" }, "dbo.CtsUsers");
            DropIndex("dbo.CtsUserCtsRoles", new[] { "CtsRole_RoleName" });
            DropIndex("dbo.CtsUserCtsRoles", new[] { "CtsUser_Login", "CtsUser_Domain" });
            DropTable("dbo.CtsUserCtsRoles");
            DropTable("dbo.CtsUsers");
            DropTable("dbo.CtsRoles");
        }
    }
}
