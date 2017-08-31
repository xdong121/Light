namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updtaeLightAlarm : DbMigration
    {
        public override void Up()
        {
            AddColumn("li.LightAlarm", "Back1", c => c.String(maxLength: 100));
            AddColumn("li.LightAlarm", "Back2", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("li.LightAlarm", "Back2");
            DropColumn("li.LightAlarm", "Back1");
        }
    }
}
