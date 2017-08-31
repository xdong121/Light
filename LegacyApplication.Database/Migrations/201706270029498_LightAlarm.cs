namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LightAlarm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "li.LightAlarm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControlNo = c.String(nullable: false, maxLength: 10),
                        LightNo = c.String(nullable: false, maxLength: 10),
                        LightSonNo = c.String(nullable: false, maxLength: 10),
                        State = c.Int(nullable: false),
                        IsRepair = c.Int(nullable: false),
                        ReMark = c.String(maxLength: 100),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        UpdateUser = c.String(nullable: false, maxLength: 50),
                        LastAction = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.ControlNo, t.LightNo, t.LightSonNo }, unique: true, name: "IX_Internal_LightAlarm_ControlNoAndLightNo");
            
        }
        
        public override void Down()
        {
            DropIndex("li.LightAlarm", "IX_Internal_LightAlarm_ControlNoAndLightNo");
            DropTable("li.LightAlarm");
        }
    }
}
