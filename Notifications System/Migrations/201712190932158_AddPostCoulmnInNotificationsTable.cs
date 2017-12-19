namespace Notifications_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostCoulmnInNotificationsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "PostId", c => c.Int(nullable: false));
            CreateIndex("dbo.Notifications", "PostId");
            AddForeignKey("dbo.Notifications", "PostId", "dbo.Posts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "PostId", "dbo.Posts");
            DropIndex("dbo.Notifications", new[] { "PostId" });
            DropColumn("dbo.Notifications", "PostId");
        }
    }
}
