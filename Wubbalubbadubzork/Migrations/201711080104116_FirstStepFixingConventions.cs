namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstStepFixingConventions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Options", "Scene_id", "dbo.Scenes");
            DropForeignKey("dbo.Servers", "Scene_id", "dbo.Scenes");
            DropIndex("dbo.Options", new[] { "Scene_id" });
            DropIndex("dbo.Servers", new[] { "Scene_id" });
            DropTable("dbo.Options");
            DropTable("dbo.Scenes");
            DropTable("dbo.Servers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Scene_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Scenes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Number = c.Int(nullable: false),
                        Next_scene_id = c.Int(nullable: false),
                        Scene_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Servers", "Scene_id");
            CreateIndex("dbo.Options", "Scene_id");
            AddForeignKey("dbo.Servers", "Scene_id", "dbo.Scenes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Options", "Scene_id", "dbo.Scenes", "Id", cascadeDelete: true);
        }
    }
}
