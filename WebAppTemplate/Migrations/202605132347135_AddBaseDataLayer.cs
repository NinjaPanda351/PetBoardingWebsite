namespace WebAppTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBaseDataLayer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingEventModels",
                c => new
                    {
                        EventId = c.Guid(nullable: false),
                        BookingId = c.Guid(nullable: false),
                        EmployeeId = c.Guid(nullable: false),
                        EventType = c.String(nullable: false, maxLength: 100),
                        EventTime = c.DateTime(nullable: false),
                        Notes = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.BookingModels", t => t.BookingId, cascadeDelete: true)
                .ForeignKey("dbo.EmployeeModels", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.BookingId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.BookingModels",
                c => new
                    {
                        BookingId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        BookingStartTime = c.DateTime(nullable: false),
                        BookingEndTime = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 50),
                        Notes = c.String(maxLength: 1000),
                        CreatedAt = c.DateTime(nullable: false),
                        CancelledAt = c.DateTime(),
                        CancellationReason = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.PetModels", t => t.PetId, cascadeDelete: true)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.PetModels",
                c => new
                    {
                        PetId = c.Guid(nullable: false),
                        OwnerId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Breed = c.String(maxLength: 100),
                        Age = c.Int(nullable: false),
                        Color = c.String(maxLength: 50),
                        SecondaryColor = c.String(maxLength: 50),
                        VetName = c.String(maxLength: 100),
                        VetPhone = c.String(maxLength: 20),
                        MedicalNotes = c.String(maxLength: 1000),
                        Medication = c.String(maxLength: 500),
                        FeedingsPerDay = c.Int(nullable: false),
                        FeedAmount = c.String(maxLength: 100),
                        FeedingInstructions = c.String(maxLength: 500),
                        SpecialInstructions = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.PetId)
                .ForeignKey("dbo.PetOwnerModels", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.PetOwnerModels",
                c => new
                    {
                        OwnerId = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Address = c.String(maxLength: 200),
                        City = c.String(maxLength: 100),
                        State = c.String(maxLength: 50),
                        ZipCode = c.String(maxLength: 10),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OwnerId);
            
            CreateTable(
                "dbo.PetMedicationModels",
                c => new
                    {
                        MedicationId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        MedicationName = c.String(nullable: false, maxLength: 100),
                        Dosage = c.String(nullable: false, maxLength: 100),
                        Frequency = c.String(nullable: false, maxLength: 100),
                        Notes = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.MedicationId)
                .ForeignKey("dbo.PetModels", t => t.PetId, cascadeDelete: true)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.EmployeeModels",
                c => new
                    {
                        EmployeeId = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Role = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.ContactUsModels",
                c => new
                    {
                        SubmissionId = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Subject = c.String(nullable: false, maxLength: 200),
                        Message = c.String(nullable: false, maxLength: 2000),
                        SubmittedAt = c.DateTime(nullable: false),
                        ResolvedAt = c.DateTime(),
                        IsResolved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SubmissionId);
            
            AlterColumn("dbo.EmergencyContactModels", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.EmergencyContactModels", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.EmergencyContactModels", "Phone", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.EmergencyContactModels", "Relationship", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.EmergencyContactModels", "OwnerId");
            AddForeignKey("dbo.EmergencyContactModels", "OwnerId", "dbo.PetOwnerModels", "OwnerId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingEventModels", "EmployeeId", "dbo.EmployeeModels");
            DropForeignKey("dbo.BookingEventModels", "BookingId", "dbo.BookingModels");
            DropForeignKey("dbo.PetMedicationModels", "PetId", "dbo.PetModels");
            DropForeignKey("dbo.PetModels", "OwnerId", "dbo.PetOwnerModels");
            DropForeignKey("dbo.EmergencyContactModels", "OwnerId", "dbo.PetOwnerModels");
            DropForeignKey("dbo.BookingModels", "PetId", "dbo.PetModels");
            DropIndex("dbo.PetMedicationModels", new[] { "PetId" });
            DropIndex("dbo.EmergencyContactModels", new[] { "OwnerId" });
            DropIndex("dbo.PetModels", new[] { "OwnerId" });
            DropIndex("dbo.BookingModels", new[] { "PetId" });
            DropIndex("dbo.BookingEventModels", new[] { "EmployeeId" });
            DropIndex("dbo.BookingEventModels", new[] { "BookingId" });
            AlterColumn("dbo.EmergencyContactModels", "Relationship", c => c.String());
            AlterColumn("dbo.EmergencyContactModels", "Phone", c => c.String());
            AlterColumn("dbo.EmergencyContactModels", "LastName", c => c.String());
            AlterColumn("dbo.EmergencyContactModels", "FirstName", c => c.String());
            DropTable("dbo.ContactUsModels");
            DropTable("dbo.EmployeeModels");
            DropTable("dbo.PetMedicationModels");
            DropTable("dbo.PetOwnerModels");
            DropTable("dbo.PetModels");
            DropTable("dbo.BookingModels");
            DropTable("dbo.BookingEventModels");
        }
    }
}
