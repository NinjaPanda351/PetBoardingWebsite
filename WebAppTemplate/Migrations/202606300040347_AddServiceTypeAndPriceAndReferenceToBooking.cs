namespace PawesomePalace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceTypeAndPriceAndReferenceToBooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingModels", "ServiceType", c => c.String(nullable: false));
            AddColumn("dbo.BookingModels", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BookingModels", "BookingReference", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingModels", "BookingReference");
            DropColumn("dbo.BookingModels", "Price");
            DropColumn("dbo.BookingModels", "ServiceType");
        }
    }
}
