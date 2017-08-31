namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGroupLightInfo : DbMigration
    {
        public override void Up()
        {
            DropIndex("li.GroupLightInfo", "IX_Internal_LightInfo_ControlNoAndLightNo");
            AlterColumn("li.GroupLightInfo", "No", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("li.GroupLightInfo", "No", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("li.GroupLightInfo", "No", unique: true, name: "IX_Internal_LightInfo_ControlNoAndLightNo");
        }
    }
}
