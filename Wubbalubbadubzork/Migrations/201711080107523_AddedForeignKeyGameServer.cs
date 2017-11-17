namespace Wubbalubbadubzork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKeyGameServer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Server_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Games", "Server_Id");
            AddForeignKey("dbo.Games", "Server_Id", "dbo.Servers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "Server_Id", "dbo.Servers");
            DropIndex("dbo.Games", new[] { "Server_Id" });
            DropColumn("dbo.Games", "Server_Id");
        }
    }
}
