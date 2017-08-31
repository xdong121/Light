namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTeleInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("li.TeleInfo", "ReadTime", c => c.DateTime(nullable: false));
            AlterColumn("li.TeleInfo", "Type", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("li.TeleInfo", "Content", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("li.TeleInfo", "Result", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("li.TeleInfo", "Result", c => c.Int(nullable: false));
            AlterColumn("li.TeleInfo", "Content", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("li.TeleInfo", "Type", c => c.Int(nullable: false));
            DropColumn("li.TeleInfo", "ReadTime");
        }
    }
}
