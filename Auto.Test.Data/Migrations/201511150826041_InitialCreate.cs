namespace AutoClutch.Test.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        AuditLogId = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        EventDateUTC = c.DateTimeOffset(nullable: false, precision: 7),
                        EventType = c.Int(nullable: false),
                        TypeFullName = c.String(nullable: false, maxLength: 512),
                        RecordId = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.AuditLogId);
            
            CreateTable(
                "dbo.AuditLogDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PropertyName = c.String(nullable: false, maxLength: 256),
                        OriginalValue = c.String(),
                        NewValue = c.String(),
                        AuditLogId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuditLogs", t => t.AuditLogId, cascadeDelete: true)
                .Index(t => t.AuditLogId);
            
            CreateTable(
                "dbo.facility",
                c => new
                    {
                        facilityId = c.Int(nullable: false, identity: true),
                        locationId = c.Int(),
                        name = c.String(nullable: false),
                        facilityType = c.String(),
                    })
                .PrimaryKey(t => t.facilityId)
                .ForeignKey("dbo.location", t => t.locationId)
                .Index(t => t.locationId);
            
            CreateTable(
                "dbo.location",
                c => new
                    {
                        locationId = c.Int(nullable: false, identity: true),
                        contactUserId = c.Int(),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.locationId)
                .ForeignKey("dbo.user", t => t.contactUserId)
                .Index(t => t.contactUserId);
            
            CreateTable(
                "dbo.user",
                c => new
                    {
                        userId = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.location", "contactUserId", "dbo.user");
            DropForeignKey("dbo.facility", "locationId", "dbo.location");
            DropForeignKey("dbo.AuditLogDetails", "AuditLogId", "dbo.AuditLogs");
            DropIndex("dbo.location", new[] { "contactUserId" });
            DropIndex("dbo.facility", new[] { "locationId" });
            DropIndex("dbo.AuditLogDetails", new[] { "AuditLogId" });
            DropTable("dbo.user");
            DropTable("dbo.location");
            DropTable("dbo.facility");
            DropTable("dbo.AuditLogDetails");
            DropTable("dbo.AuditLogs");
        }
    }
}
