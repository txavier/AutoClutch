namespace AutoClutch.Test.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeFacilityISoftDeletable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.facility", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.facility", "IsDeleted");
        }
    }
}
