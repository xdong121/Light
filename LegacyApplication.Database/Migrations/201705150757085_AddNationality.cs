namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNationality : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "hr.Nationality",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        UpdateUser = c.String(nullable: false, maxLength: 50),
                        LastAction = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            DropColumn("hr.Employee", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("hr.Employee", "MyProperty", c => c.Int(nullable: false));
            DropIndex("hr.Nationality", new[] { "Name" });
            DropTable("hr.Nationality");
        }
    }
}
