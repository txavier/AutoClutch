namespace AutoClutch.Test.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTrackerEnabledDb : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AuditLogs", "EventDateUTC", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AuditLogs", "EventDateUTC", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
