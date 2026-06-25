namespace PawesomePalace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeOwnerIdOptional : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PetModels", "OwnerId", "dbo.PetOwnerModels");
            DropIndex("dbo.PetModels", new[] { "OwnerId" });
            AlterColumn("dbo.PetModels", "OwnerId", c => c.Guid());
            CreateIndex("dbo.PetModels", "OwnerId");
            AddForeignKey("dbo.PetModels", "OwnerId", "dbo.PetOwnerModels", "OwnerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetModels", "OwnerId", "dbo.PetOwnerModels");
            DropIndex("dbo.PetModels", new[] { "OwnerId" });
            AlterColumn("dbo.PetModels", "OwnerId", c => c.Guid(nullable: false));
            CreateIndex("dbo.PetModels", "OwnerId");
            AddForeignKey("dbo.PetModels", "OwnerId", "dbo.PetOwnerModels", "OwnerId", cascadeDelete: true);
        }
    }
}
