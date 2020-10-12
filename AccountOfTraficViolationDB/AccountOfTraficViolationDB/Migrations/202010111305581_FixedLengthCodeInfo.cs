namespace AccountOfTraficViolationDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedLengthCodeInfo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CodeInformations", "Description", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CodeInformations", "Description", c => c.String(maxLength: 100));
        }
    }
}
