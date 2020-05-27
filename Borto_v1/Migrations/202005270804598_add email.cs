namespace Borto_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addemail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String(maxLength: 70));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Email");
        }
    }
}
