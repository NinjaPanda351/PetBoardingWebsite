namespace WebAppTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveToDoModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ToDoToStepsModels", "Steps_ToDoStepID", "dbo.ToDoStepsModels");
            DropForeignKey("dbo.ToDoToStepsModels", "ToDo_ToDoID", "dbo.ToDoModels");
            DropIndex("dbo.ToDoToStepsModels", new[] { "Steps_ToDoStepID" });
            DropIndex("dbo.ToDoToStepsModels", new[] { "ToDo_ToDoID" });
            DropTable("dbo.ToDoToStepsModels");
            DropTable("dbo.ToDoStepsModels");
            DropTable("dbo.ToDoModels");
        }
        
        public override void Down()
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
            
            CreateTable(
                "dbo.ToDoStepsModels",
                c => new
                    {
                        ToDoStepID = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Instruction = c.String(),
                    })
                .PrimaryKey(t => t.ToDoStepID);
            
            CreateTable(
                "dbo.ToDoToStepsModels",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        DateTimeCreated = c.DateTime(nullable: false),
                        Steps_ToDoStepID = c.Guid(nullable: false),
                        ToDo_ToDoID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.ToDoToStepsModels", "ToDo_ToDoID");
            CreateIndex("dbo.ToDoToStepsModels", "Steps_ToDoStepID");
            AddForeignKey("dbo.ToDoToStepsModels", "ToDo_ToDoID", "dbo.ToDoModels", "ToDoID", cascadeDelete: true);
            AddForeignKey("dbo.ToDoToStepsModels", "Steps_ToDoStepID", "dbo.ToDoStepsModels", "ToDoStepID", cascadeDelete: true);
        }
    }
}
