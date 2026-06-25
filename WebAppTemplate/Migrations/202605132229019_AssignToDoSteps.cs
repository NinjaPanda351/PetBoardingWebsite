namespace PawesomePalace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssignToDoSteps : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToDoToStepsModels",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        DateTimeCreated = c.DateTime(nullable: false),
                        Steps_ToDoStepID = c.Guid(nullable: false),
                        ToDo_ToDoID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ToDoStepsModels", t => t.Steps_ToDoStepID, cascadeDelete: true)
                .ForeignKey("dbo.ToDoModels", t => t.ToDo_ToDoID, cascadeDelete: true)
                .Index(t => t.Steps_ToDoStepID)
                .Index(t => t.ToDo_ToDoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoToStepsModels", "ToDo_ToDoID", "dbo.ToDoModels");
            DropForeignKey("dbo.ToDoToStepsModels", "Steps_ToDoStepID", "dbo.ToDoStepsModels");
            DropIndex("dbo.ToDoToStepsModels", new[] { "ToDo_ToDoID" });
            DropIndex("dbo.ToDoToStepsModels", new[] { "Steps_ToDoStepID" });
            DropTable("dbo.ToDoToStepsModels");
        }
    }
}
