namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRelations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Game_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Character_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Game_Id");
            CreateIndex("dbo.AspNetUsers", "Character_Id");
            AddForeignKey("dbo.AspNetUsers", "Character_Id", "dbo.Characters", "Id");
            AddForeignKey("dbo.AspNetUsers", "Game_Id", "dbo.Games", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.AspNetUsers", "Character_Id", "dbo.Characters");
            DropIndex("dbo.AspNetUsers", new[] { "Character_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Game_Id" });
            DropColumn("dbo.AspNetUsers", "Character_Id");
            DropColumn("dbo.AspNetUsers", "Game_Id");
        }
    }
}
