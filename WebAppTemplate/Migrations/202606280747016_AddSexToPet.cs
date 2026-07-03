namespace PawesomePalace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSexToPet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PetModels", "Sex", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PetModels", "Sex");
        }
    }
}
