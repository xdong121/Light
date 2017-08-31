namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLightInfo : DbMigration
    {
        public override void Up()
        {
            DropIndex("li.LightInfo", "IX_Internal_LightInfo_ControlNoAndLightNo");
            AddColumn("li.LightInfo", "LightSonNo", c => c.String(maxLength: 10));
            AddColumn("li.LightInfo", "Back1", c => c.String(maxLength: 100));
            AddColumn("li.LightInfo", "Back2", c => c.String(maxLength: 100));
            AlterColumn("li.LightInfo", "ControlNo", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("li.LightInfo", "LightNo", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("li.LightInfo", "Describe", c => c.String(maxLength: 50));
            AlterColumn("li.LightInfo", "Longitude", c => c.String(maxLength: 20));
            AlterColumn("li.LightInfo", "Latitude", c => c.String(maxLength: 20));
            AlterColumn("li.LightInfo", "ReMark", c => c.String(maxLength: 100));
            CreateIndex("li.LightInfo", new[] { "ControlNo", "LightNo" }, unique: true, name: "IX_Internal_LightInfo_ControlNoAndLightNo");
        }
        
        public override void Down()
        {
            DropIndex("li.LightInfo", "IX_Internal_LightInfo_ControlNoAndLightNo");
            AlterColumn("li.LightInfo", "ReMark", c => c.String(maxLength: 500));
            AlterColumn("li.LightInfo", "Latitude", c => c.String(maxLength: 50));
            AlterColumn("li.LightInfo", "Longitude", c => c.String(maxLength: 50));
            AlterColumn("li.LightInfo", "Describe", c => c.String(maxLength: 200));
            AlterColumn("li.LightInfo", "LightNo", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("li.LightInfo", "ControlNo", c => c.String(nullable: false, maxLength: 20));
            DropColumn("li.LightInfo", "Back2");
            DropColumn("li.LightInfo", "Back1");
            DropColumn("li.LightInfo", "LightSonNo");
            CreateIndex("li.LightInfo", new[] { "ControlNo", "LightNo" }, unique: true, name: "IX_Internal_LightInfo_ControlNoAndLightNo");
        }
    }
}
