namespace PawesomePalace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpeciesToPet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PetModels", "Species", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PetModels", "Species");
        }
    }
}
