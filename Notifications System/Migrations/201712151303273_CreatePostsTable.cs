namespace Notifications_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePostsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(nullable: false),
                        Answer = c.String(),
                        SenderId = c.String(maxLength: 128),
                        RecieverId = c.String(maxLength: 128),
                        DateAsked = c.DateTime(nullable: false),
                        DateAnswerd = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RecieverId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.RecieverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "RecieverId", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "RecieverId" });
            DropIndex("dbo.Posts", new[] { "SenderId" });
            DropTable("dbo.Posts");
        }
    }
}
