namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLightInfo20170602 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("li.LightInfo", "GroupInfoId", "li.GroupInfo");
            DropIndex("li.LightInfo", new[] { "GroupInfoId" });
            AddColumn("li.LightInfo", "GroupNo", c => c.String());
            DropColumn("li.LightInfo", "GroupInfoId");
        }
        
        public override void Down()
        {
            AddColumn("li.LightInfo", "GroupInfoId", c => c.Int(nullable: false));
            DropColumn("li.LightInfo", "GroupNo");
            CreateIndex("li.LightInfo", "GroupInfoId");
            AddForeignKey("li.LightInfo", "GroupInfoId", "li.GroupInfo", "Id");
        }
    }
}
