namespace Borto_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IdUser = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.IdUser);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        IdVideo = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        Path = c.String(),
                        UserId = c.Int(nullable: false),
                        User_IdUser = c.Int(),
                    })
                .PrimaryKey(t => t.IdVideo)
                .ForeignKey("dbo.Users", t => t.User_IdUser)
                .Index(t => t.User_IdUser);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "User_IdUser", "dbo.Users");
            DropIndex("dbo.Videos", new[] { "User_IdUser" });
            DropTable("dbo.Videos");
            DropTable("dbo.Users");
        }
    }
}
