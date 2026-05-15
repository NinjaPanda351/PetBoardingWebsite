namespace WebAppTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToDos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToDoModels",
                c => new
                    {
                        ToDoID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(maxLength: 100),
                        IsComplete = c.Boolean(nullable: false),
                        dateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ToDoID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ToDoModels");
        }
    }
}
