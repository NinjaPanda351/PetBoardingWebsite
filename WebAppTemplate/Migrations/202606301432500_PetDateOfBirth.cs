namespace PawesomePalace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetDateOfBirth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PetModels", "DateOfBirth", c => c.DateTime());
            DropColumn("dbo.PetModels", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PetModels", "Age", c => c.Int(nullable: false));
            DropColumn("dbo.PetModels", "DateOfBirth");
        }
    }
}
