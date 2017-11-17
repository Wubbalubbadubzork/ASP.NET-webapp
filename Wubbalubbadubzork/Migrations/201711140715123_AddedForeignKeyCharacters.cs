namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKeyCharacters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Scene_Id", c => c.Int());
            CreateIndex("dbo.Characters", "Scene_Id");
            AddForeignKey("dbo.Characters", "Scene_Id", "dbo.Scenes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "Scene_Id", "dbo.Scenes");
            DropIndex("dbo.Characters", new[] { "Scene_Id" });
            DropColumn("dbo.Characters", "Scene_Id");
        }
    }
}
