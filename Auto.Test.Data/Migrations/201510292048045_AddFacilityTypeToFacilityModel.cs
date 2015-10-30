namespace Auto.Test.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFacilityTypeToFacilityModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.facility", "facilityType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.facility", "facilityType");
        }
    }
}
