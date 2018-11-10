namespace CTS_Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlarmComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlarmComments",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Comment = c.String(maxLength: 255),
                        CommentEng = c.String(maxLength: 255),
                        CommentKZ = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AlarmComments");
        }
    }
}
