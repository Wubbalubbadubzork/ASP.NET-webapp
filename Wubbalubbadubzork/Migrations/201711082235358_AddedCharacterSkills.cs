namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCharacterSkills : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Role = c.String(nullable: false),
                        Max_HP = c.Int(nullable: false),
                        Max_Mana = c.Int(nullable: false),
                        Armor = c.Int(nullable: false),
                        Damage = c.Int(nullable: false),
                        Power = c.Int(nullable: false),
                        Playable = c.Boolean(nullable: false),
                        Health = c.Int(nullable: false),
                        Mana = c.Int(nullable: false),
                        Is_Turn = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Base_Power = c.Int(nullable: false),
                        Character_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id, cascadeDelete: true)
                .Index(t => t.Character_Id);
            
            AddColumn("dbo.AspNetUsers", "Score", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Skills", "Character_Id", "dbo.Characters");
            DropIndex("dbo.Skills", new[] { "Character_Id" });
            DropColumn("dbo.AspNetUsers", "Score");
            DropTable("dbo.Skills");
            DropTable("dbo.Characters");
        }
    }
}
