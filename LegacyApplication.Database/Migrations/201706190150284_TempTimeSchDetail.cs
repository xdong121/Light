namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempTimeSchDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "li.TempTimeSchDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(),
                        IsUse = c.Int(nullable: false),
                        Light = c.Int(nullable: false),
                        MakeTime = c.DateTime(),
                        Result = c.Int(nullable: false),
                        Remark = c.String(maxLength: 50),
                        GroupInfoId = c.Int(),
                        TempTimeSchMainId = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        UpdateUser = c.String(nullable: false, maxLength: 50),
                        LastAction = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("li.GroupInfo", t => t.GroupInfoId)
                .ForeignKey("li.TempTimeSchMain", t => t.TempTimeSchMainId)
                .Index(t => t.GroupInfoId)
                .Index(t => t.TempTimeSchMainId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("li.TempTimeSchDetail", "TempTimeSchMainId", "li.TempTimeSchMain");
            DropForeignKey("li.TempTimeSchDetail", "GroupInfoId", "li.GroupInfo");
            DropIndex("li.TempTimeSchDetail", new[] { "TempTimeSchMainId" });
            DropIndex("li.TempTimeSchDetail", new[] { "GroupInfoId" });
            DropTable("li.TempTimeSchDetail");
        }
    }
}
