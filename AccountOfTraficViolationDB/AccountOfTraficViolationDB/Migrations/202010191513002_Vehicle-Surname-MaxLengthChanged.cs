namespace AccountOfTraficViolationDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleSurnameMaxLengthChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vehicles", "Surname", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vehicles", "Surname", c => c.String(nullable: false, maxLength: 15));
        }
    }
}
