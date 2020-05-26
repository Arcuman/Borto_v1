namespace Borto_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Videos", "HasConvertation", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Videos", "HasConvertation", c => c.Int(nullable: false));
        }
    }
}
