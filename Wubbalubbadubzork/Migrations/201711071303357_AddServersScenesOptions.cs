namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServersScenesOptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false),
                        number = c.Int(nullable: false),
                        next_scene_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Scenes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Servers");
            DropTable("dbo.Scenes");
            DropTable("dbo.Options");
        }
    }
}
