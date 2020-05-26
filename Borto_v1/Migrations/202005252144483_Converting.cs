namespace Borto_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Converting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "HasConvertation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "HasConvertation");
        }
    }
}
