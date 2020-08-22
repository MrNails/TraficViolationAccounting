namespace AccountingOfTraficViolation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRowVersionMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccidentOnHighways", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
            AddColumn("dbo.CaseAccidentPlace", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
            AddColumn("dbo.AccidentOnVillages", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
            AddColumn("dbo.Cases", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
            AddColumn("dbo.GeneralInfos", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
            AddColumn("dbo.ParticipantsInformations", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
            AddColumn("dbo.RoadConditions", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
            AddColumn("dbo.Vehicles", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
            AddColumn("dbo.Victims", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
            AddColumn("dbo.Users", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true,storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccidentOnHighways", "RowVersion");
            DropColumn("dbo.CaseAccidentPlace", "RowVersion");
            DropColumn("dbo.AccidentOnVillages", "RowVersion");
            DropColumn("dbo.Cases", "RowVersion");
            DropColumn("dbo.GeneralInfos", "RowVersion");
            DropColumn("dbo.ParticipantsInformations", "RowVersion");
            DropColumn("dbo.RoadConditions", "RowVersion");
            DropColumn("dbo.Vehicles", "RowVersion");
            DropColumn("dbo.Victims", "RowVersion");
            DropColumn("dbo.Users", "RowVersion");
        }
    }
}
