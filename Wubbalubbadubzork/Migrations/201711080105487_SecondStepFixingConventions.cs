namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondStepFixingConventions : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Scenes", t => t.Scene_id, cascadeDelete: true)
                .Index(t => t.Scene_id);
            
            CreateTable(
                "dbo.Scenes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Scene_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Scenes", t => t.Scene_id, cascadeDelete: true)
                .Index(t => t.Scene_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servers", "Scene_id", "dbo.Scenes");
            DropForeignKey("dbo.Options", "Scene_id", "dbo.Scenes");
            DropIndex("dbo.Servers", new[] { "Scene_id" });
            DropIndex("dbo.Options", new[] { "Scene_id" });
            DropTable("dbo.Servers");
            DropTable("dbo.Scenes");
            DropTable("dbo.Options");
        }
    }
}
