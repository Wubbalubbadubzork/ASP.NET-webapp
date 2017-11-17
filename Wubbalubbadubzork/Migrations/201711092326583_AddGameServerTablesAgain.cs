namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGameServerTablesAgain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Server_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servers", t => t.Server_Id, cascadeDelete: true)
                .Index(t => t.Server_Id);
            
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Scene_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Scenes", t => t.Scene_id, cascadeDelete: true)
                .Index(t => t.Scene_id);
            
            AddColumn("dbo.AspNetUsers", "Game_Id", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "Game_Id");
            AddForeignKey("dbo.AspNetUsers", "Game_Id", "dbo.Games", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Games", "Server_Id", "dbo.Servers");
            DropForeignKey("dbo.Servers", "Scene_id", "dbo.Scenes");
            DropIndex("dbo.AspNetUsers", new[] { "Game_Id" });
            DropIndex("dbo.Servers", new[] { "Scene_id" });
            DropIndex("dbo.Games", new[] { "Server_Id" });
            DropColumn("dbo.AspNetUsers", "Game_Id");
            DropTable("dbo.Servers");
            DropTable("dbo.Games");
        }
    }
}
