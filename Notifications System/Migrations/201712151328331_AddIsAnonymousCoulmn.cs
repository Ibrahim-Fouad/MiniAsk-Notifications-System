namespace Notifications_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAnonymousCoulmn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "IsAnonymously", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "IsAnonymously");
        }
    }
}
