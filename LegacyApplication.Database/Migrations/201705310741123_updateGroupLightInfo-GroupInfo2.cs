namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateGroupLightInfoGroupInfo2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "li.GroupInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupNo = c.String(nullable: false, maxLength: 10),
                        GroupName = c.String(maxLength: 10),
                        Remark = c.String(maxLength: 50),
                        GroupLightInfoId = c.Int(nullable: false),
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
                "li.GroupLightInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupNo = c.String(maxLength: 10),
                        ControlNo = c.String(maxLength: 10),
                        LightNo = c.String(maxLength: 10),
                        LightSonNo = c.String(maxLength: 10),
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
            DropForeignKey("li.GroupInfo", "GroupLightInfoId", "li.GroupLightInfo");
            DropIndex("li.GroupInfo", new[] { "GroupLightInfoId" });
            DropTable("li.GroupLightInfo");
            DropTable("li.GroupInfo");
        }
    }
}
