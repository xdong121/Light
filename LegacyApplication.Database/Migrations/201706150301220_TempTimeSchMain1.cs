namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempTimeSchMain1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("li.TeleInfo", "ReadTime", c => c.DateTime());
            AlterColumn("li.TempTimeSchMain", "StartDate", c => c.DateTime());
            AlterColumn("li.TempTimeSchMain", "EndDate", c => c.DateTime());
            AlterColumn("li.TempTimeSchMain", "MakeTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("li.TempTimeSchMain", "MakeTime", c => c.DateTime(nullable: false));
            AlterColumn("li.TempTimeSchMain", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("li.TempTimeSchMain", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("li.TeleInfo", "ReadTime", c => c.DateTime(nullable: false));
        }
    }
}
