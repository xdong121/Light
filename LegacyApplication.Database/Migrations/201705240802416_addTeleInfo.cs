namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTeleInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "li.TeleInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Content = c.String(nullable: false, maxLength: 500),
                        Result = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        UpdateUser = c.String(nullable: false, maxLength: 50),
                        LastAction = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("li.TeleInfo");
        }
    }
}
