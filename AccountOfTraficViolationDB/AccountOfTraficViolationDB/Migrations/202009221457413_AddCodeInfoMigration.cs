namespace AccountOfTraficViolationDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodeInfoMigration : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.CodeInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false, maxLength: 15),
                        Description = c.String(maxLength: 100),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Users", "Phone", c => c.String(nullable: true, maxLength: 10));
        }
        
        public override void Down()
        {
            DropTable("dbo.CodeInformations");
            DropColumn("dbo.Users", "Phone");
        }
    }
}
