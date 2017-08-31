namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IX_Internal_LightInfo_ControlNoAndLightNo : DbMigration
    {
        public override void Up()
        {
            DropIndex("li.LightInfo", new[] { "ControlNo" });
            AlterColumn("li.LightInfo", "LightNo", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("li.LightInfo", new[] { "ControlNo", "LightNo" }, unique: true, name: "IX_Internal_LightInfo_ControlNoAndLightNo");
        }
        
        public override void Down()
        {
            DropIndex("li.LightInfo", "IX_Internal_LightInfo_ControlNoAndLightNo");
            AlterColumn("li.LightInfo", "LightNo", c => c.String(maxLength: 20));
            CreateIndex("li.LightInfo", "ControlNo", unique: true);
        }
    }
}
