namespace PawesomePalace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBookingModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingModels", "AdminNotes", c => c.String(maxLength: 2000));
            AddColumn("dbo.BookingModels", "RefundStatus", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingModels", "RefundStatus");
            DropColumn("dbo.BookingModels", "AdminNotes");
        }
    }
}
