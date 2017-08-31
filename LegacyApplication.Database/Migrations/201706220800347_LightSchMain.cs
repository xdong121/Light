namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LightSchMain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "li.LightSchMain",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControlNo = c.String(maxLength: 10),
                        MakeTime = c.DateTime(),
                        Result = c.Int(nullable: false),
                        Remark = c.String(maxLength: 50),
                        GroupLightInfoId = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        UpdateUser = c.String(nullable: false, maxLength: 50),
                        LastAction = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("li.GroupLightInfo", t => t.GroupLightInfoId)
                .Index(t => t.GroupLightInfoId);
            
            CreateTable(
                "li.LightSchDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.Int(nullable: false),
                        End = c.Int(nullable: false),
                        Power = c.Int(nullable: false),
                        Remark = c.String(maxLength: 50),
                        LightSchMainId = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        UpdateUser = c.String(nullable: false, maxLength: 50),
                        LastAction = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("li.LightSchMain", t => t.LightSchMainId)
                .Index(t => t.LightSchMainId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("li.LightSchDetail", "LightSchMainId", "li.LightSchMain");
            DropForeignKey("li.LightSchMain", "GroupLightInfoId", "li.GroupLightInfo");
            DropIndex("li.LightSchDetail", new[] { "LightSchMainId" });
            DropIndex("li.LightSchMain", new[] { "GroupLightInfoId" });
            DropTable("li.LightSchDetail");
            DropTable("li.LightSchMain");
        }
    }
}
