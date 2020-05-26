namespace Borto_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        IdComment = c.Int(nullable: false, identity: true),
                        CommentMessage = c.String(nullable: false, maxLength: 210),
                        PostDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        VideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdComment)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.VideoId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IdUser = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40),
                        NickName = c.String(maxLength: 50),
                        Login = c.String(maxLength: 40),
                        Password = c.String(maxLength: 50),
                        Image = c.Binary(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdUser);
            
            CreateTable(
                "dbo.FavoriteVideos",
                c => new
                    {
                        IdFavoriteVideo = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        VideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFavoriteVideo)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.VideoId);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        IdVideo = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 200),
                        Image = c.Binary(),
                        Path = c.String(),
                        MaxDuration = c.Double(nullable: false),
                        HasConvertation = c.Boolean(nullable: false),
                        UploadDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdVideo)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Marks",
                c => new
                    {
                        IdMark = c.Int(nullable: false, identity: true),
                        TypeMark = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        VideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdMark)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.VideoId);
            
            CreateTable(
                "dbo.PlaylistVideos",
                c => new
                    {
                        IdPlaylistVideo = c.Int(nullable: false, identity: true),
                        PlaylistId = c.Int(nullable: false),
                        VideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlaylistVideo)
                .ForeignKey("dbo.Playlists", t => t.PlaylistId, cascadeDelete: true)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.PlaylistId)
                .Index(t => t.VideoId);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        IdPlaylist = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        Image = c.Binary(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlaylist)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "UserId", "dbo.Users");
            DropForeignKey("dbo.PlaylistVideos", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.Playlists", "UserId", "dbo.Users");
            DropForeignKey("dbo.PlaylistVideos", "PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.Marks", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.Marks", "UserId", "dbo.Users");
            DropForeignKey("dbo.FavoriteVideos", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.Comments", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.FavoriteVideos", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropIndex("dbo.Playlists", new[] { "UserId" });
            DropIndex("dbo.PlaylistVideos", new[] { "VideoId" });
            DropIndex("dbo.PlaylistVideos", new[] { "PlaylistId" });
            DropIndex("dbo.Marks", new[] { "VideoId" });
            DropIndex("dbo.Marks", new[] { "UserId" });
            DropIndex("dbo.Videos", new[] { "UserId" });
            DropIndex("dbo.FavoriteVideos", new[] { "VideoId" });
            DropIndex("dbo.FavoriteVideos", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "VideoId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.Playlists");
            DropTable("dbo.PlaylistVideos");
            DropTable("dbo.Marks");
            DropTable("dbo.Videos");
            DropTable("dbo.FavoriteVideos");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
        }
    }
}
