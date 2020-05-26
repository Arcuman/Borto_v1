namespace Borto_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlaylistVideo4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FavoriteVideos", "UserId", "dbo.Users");
            DropForeignKey("dbo.FavoriteVideos", "VideoId", "dbo.Videos");
            DropIndex("dbo.FavoriteVideos", new[] { "UserId" });
            DropIndex("dbo.FavoriteVideos", new[] { "VideoId" });
            CreateTable(
                "dbo.WatchLaters",
                c => new
                    {
                        IdWatchLater = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        VideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdWatchLater)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.VideoId);
            
            DropTable("dbo.FavoriteVideos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FavoriteVideos",
                c => new
                    {
                        IdFavoriteVideo = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        VideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFavoriteVideo);
            
            DropForeignKey("dbo.WatchLaters", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.WatchLaters", "UserId", "dbo.Users");
            DropIndex("dbo.WatchLaters", new[] { "VideoId" });
            DropIndex("dbo.WatchLaters", new[] { "UserId" });
            DropTable("dbo.WatchLaters");
            CreateIndex("dbo.FavoriteVideos", "VideoId");
            CreateIndex("dbo.FavoriteVideos", "UserId");
            AddForeignKey("dbo.FavoriteVideos", "VideoId", "dbo.Videos", "IdVideo", cascadeDelete: true);
            AddForeignKey("dbo.FavoriteVideos", "UserId", "dbo.Users", "IdUser", cascadeDelete: true);
        }
    }
}
