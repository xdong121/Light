namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateGroupInfo20170602 : DbMigration
    {
        public override void Up()
        {
            DropIndex("li.GroupInfo", new[] { "GroupNo" });
            AlterColumn("li.GroupInfo", "GroupNo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("li.GroupInfo", "GroupNo", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("li.GroupInfo", "GroupNo", unique: true);
        }
    }
}
