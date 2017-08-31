namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLightInfo : DbMigration
    {
        public override void Up()
        {
            DropIndex("hr.Nationality", new[] { "Name" });
            CreateTable(
                "li.LightInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControlNo = c.String(nullable: false, maxLength: 20),
                        LightNo = c.String(nullable: false, maxLength: 20),
                        Describe = c.String(maxLength: 200),
                        Address = c.String(maxLength: 50),
                        Longitude = c.String(maxLength: 50),
                        Latitude = c.String(maxLength: 50),
                        ReMark = c.String(maxLength: 500),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        UpdateUser = c.String(nullable: false, maxLength: 50),
                        LastAction = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ControlNo, unique: true)
                .Index(t => t.LightNo, unique: true);
            
            AlterColumn("hr.Nationality", "Name", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("hr.Nationality", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("hr.Nationality", new[] { "Name" });
            DropIndex("li.LightInfo", new[] { "LightNo" });
            DropIndex("li.LightInfo", new[] { "ControlNo" });
            AlterColumn("hr.Nationality", "Name", c => c.String(nullable: false, maxLength: 50));
            DropTable("li.LightInfo");
            CreateIndex("hr.Nationality", "Name", unique: true);
        }
    }
}
