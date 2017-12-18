namespace Notifications_System.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateNotificationsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DateCreated = c.DateTime(nullable: false),
                    Message = c.String(nullable: false, maxLength: 255),
                    IsRead = c.Boolean(nullable: false),
                    SenderId = c.String(maxLength: 128),
                    RecieverId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RecieverId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.RecieverId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "RecieverId", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "RecieverId" });
            DropIndex("dbo.Notifications", new[] { "SenderId" });
            DropTable("dbo.Notifications");
        }
    }
}
