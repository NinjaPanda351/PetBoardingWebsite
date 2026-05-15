namespace WebAppTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmergencyContact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmergencyContactModels",
                c => new
                    {
                        EmergencyContactId = c.Guid(nullable: false),
                        OwnerId = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        Relationship = c.String(),
                    })
                .PrimaryKey(t => t.EmergencyContactId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmergencyContactModels");
        }
    }
}
