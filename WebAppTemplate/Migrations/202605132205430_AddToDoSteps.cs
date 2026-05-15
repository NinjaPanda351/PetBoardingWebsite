namespace WebAppTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToDoSteps : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToDoStepsModels",
                c => new
                    {
                        ToDoStepID = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Instruction = c.String(),
                    })
                .PrimaryKey(t => t.ToDoStepID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ToDoStepsModels");
        }
    }
}
