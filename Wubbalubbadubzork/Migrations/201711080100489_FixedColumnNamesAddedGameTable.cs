namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedColumnNamesAddedGameTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Options", new[] { "scene_id" });
            DropIndex("dbo.Servers", new[] { "scene_id" });
            CreateIndex("dbo.Options", "Scene_id");
            CreateIndex("dbo.Servers", "Scene_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Servers", new[] { "Scene_id" });
            DropIndex("dbo.Options", new[] { "Scene_id" });
            CreateIndex("dbo.Servers", "scene_id");
            CreateIndex("dbo.Options", "scene_id");
        }
    }
}
