namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempTimeSchRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("li.TempTimeSchDetail", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("li.TempTimeSchMain", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("li.TempTimeSchMain", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("li.TempTimeSchMain", "EndDate", c => c.DateTime());
            AlterColumn("li.TempTimeSchMain", "StartDate", c => c.DateTime());
            AlterColumn("li.TempTimeSchDetail", "StartTime", c => c.DateTime());
        }
    }
}
