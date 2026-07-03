namespace PawesomePalace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceModel12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceModels",
                c => new
                    {
                        ServiceId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        PricePerNight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ServiceModels");
        }
    }
}
