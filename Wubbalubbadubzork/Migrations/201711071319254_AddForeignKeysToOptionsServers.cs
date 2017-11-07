namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeysToOptionsServers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Options", "scene_id", c => c.Int(nullable: false));
            AddColumn("dbo.Servers", "scene_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Options", "scene_id");
            CreateIndex("dbo.Servers", "scene_id");
            AddForeignKey("dbo.Options", "scene_id", "dbo.Scenes", "id", cascadeDelete: true);
            AddForeignKey("dbo.Servers", "scene_id", "dbo.Scenes", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servers", "scene_id", "dbo.Scenes");
            DropForeignKey("dbo.Options", "scene_id", "dbo.Scenes");
            DropIndex("dbo.Servers", new[] { "scene_id" });
            DropIndex("dbo.Options", new[] { "scene_id" });
            DropColumn("dbo.Servers", "scene_id");
            DropColumn("dbo.Options", "scene_id");
        }
    }
}
