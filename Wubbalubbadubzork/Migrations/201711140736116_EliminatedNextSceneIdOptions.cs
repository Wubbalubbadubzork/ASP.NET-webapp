namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminatedNextSceneIdOptions : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Options", "Next_scene_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Options", "Next_scene_id", c => c.Int(nullable: false));
        }
    }
}
