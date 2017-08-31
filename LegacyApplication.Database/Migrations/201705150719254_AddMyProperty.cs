namespace LegacyApplication.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMyProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("hr.Employee", "MyProperty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("hr.Employee", "MyProperty");
        }
    }
}
