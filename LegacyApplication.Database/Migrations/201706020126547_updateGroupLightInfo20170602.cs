namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateGroupLightInfo20170602 : DbMigration
    {
        public override void Up()
        {
            AddColumn("li.GroupLightInfo", "Describe", c => c.String(maxLength: 50));
            AddColumn("li.GroupLightInfo", "Address", c => c.String(maxLength: 50));
            AddColumn("li.GroupLightInfo", "Longitude", c => c.String(maxLength: 20));
            AddColumn("li.GroupLightInfo", "Latitude", c => c.String(maxLength: 20));
            AddColumn("li.GroupLightInfo", "ReMark", c => c.String(maxLength: 100));
            AddColumn("li.GroupLightInfo", "Back1", c => c.String(maxLength: 100));
            AddColumn("li.GroupLightInfo", "Back2", c => c.String(maxLength: 100));
            AddColumn("li.GroupLightInfo", "GroupInfoId", c => c.Int(nullable: false));
            AddColumn("li.GroupLightInfo", "ParentId", c => c.Int());
            AddColumn("li.GroupLightInfo", "AncestorIds", c => c.String(maxLength: 200));
            AddColumn("li.GroupLightInfo", "IsAbstract", c => c.Boolean(nullable: false));
            AlterColumn("li.GroupLightInfo", "ControlNo", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("li.GroupLightInfo", "LightNo", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("li.GroupLightInfo", "LightSonNo", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("li.GroupLightInfo", new[] { "ControlNo", "LightNo", "LightSonNo" }, unique: true, name: "IX_Internal_LightInfo_ControlNoAndLightNo");
            CreateIndex("li.GroupLightInfo", "GroupInfoId");
            CreateIndex("li.GroupLightInfo", "ParentId");
            AddForeignKey("li.GroupLightInfo", "GroupInfoId", "li.GroupInfo", "Id");
            AddForeignKey("li.GroupLightInfo", "ParentId", "li.GroupLightInfo", "Id");
            DropColumn("li.GroupLightInfo", "GroupNo");
        }
        
        public override void Down()
        {
            AddColumn("li.GroupLightInfo", "GroupNo", c => c.String());
            DropForeignKey("li.GroupLightInfo", "ParentId", "li.GroupLightInfo");
            DropForeignKey("li.GroupLightInfo", "GroupInfoId", "li.GroupInfo");
            DropIndex("li.GroupLightInfo", new[] { "ParentId" });
            DropIndex("li.GroupLightInfo", new[] { "GroupInfoId" });
            DropIndex("li.GroupLightInfo", "IX_Internal_LightInfo_ControlNoAndLightNo");
            AlterColumn("li.GroupLightInfo", "LightSonNo", c => c.String());
            AlterColumn("li.GroupLightInfo", "LightNo", c => c.String());
            AlterColumn("li.GroupLightInfo", "ControlNo", c => c.String());
            DropColumn("li.GroupLightInfo", "IsAbstract");
            DropColumn("li.GroupLightInfo", "AncestorIds");
            DropColumn("li.GroupLightInfo", "ParentId");
            DropColumn("li.GroupLightInfo", "GroupInfoId");
            DropColumn("li.GroupLightInfo", "Back2");
            DropColumn("li.GroupLightInfo", "Back1");
            DropColumn("li.GroupLightInfo", "ReMark");
            DropColumn("li.GroupLightInfo", "Latitude");
            DropColumn("li.GroupLightInfo", "Longitude");
            DropColumn("li.GroupLightInfo", "Address");
            DropColumn("li.GroupLightInfo", "Describe");
        }
    }
}
