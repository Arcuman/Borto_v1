namespace Borto_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Videos", "User_IdUser", "dbo.Users");
            DropIndex("dbo.Videos", new[] { "User_IdUser" });
            DropColumn("dbo.Videos", "UserId");
            RenameColumn(table: "dbo.Videos", name: "User_IdUser", newName: "UserId");
            AlterColumn("dbo.Videos", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Videos", "UserId");
            AddForeignKey("dbo.Videos", "UserId", "dbo.Users", "IdUser", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "UserId", "dbo.Users");
            DropIndex("dbo.Videos", new[] { "UserId" });
            AlterColumn("dbo.Videos", "UserId", c => c.Int());
            RenameColumn(table: "dbo.Videos", name: "UserId", newName: "User_IdUser");
            AddColumn("dbo.Videos", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Videos", "User_IdUser");
            AddForeignKey("dbo.Videos", "User_IdUser", "dbo.Users", "IdUser");
        }
    }
}
