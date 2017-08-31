namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLightNo : DbMigration
    {
        public override void Up()
        {
            DropIndex("li.LightInfo", new[] { "LightNo" });
            AlterColumn("li.LightInfo", "LightNo", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("li.LightInfo", "LightNo", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("li.LightInfo", "LightNo", unique: true);
        }
    }
}
