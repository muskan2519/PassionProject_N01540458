namespace PassionProject_N01540458.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refrigeratorfooditems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Refrigerators",
                c => new
                    {
                        RefrigeratorId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        RefrigeratorName = c.String(),
                    })
                .PrimaryKey(t => t.RefrigeratorId);
            
            CreateTable(
                "dbo.RefrigeratorFoodItems",
                c => new
                    {
                        Refrigerator_RefrigeratorId = c.Int(nullable: false),
                        FoodItem_FoodItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Refrigerator_RefrigeratorId, t.FoodItem_FoodItemId })
                .ForeignKey("dbo.Refrigerators", t => t.Refrigerator_RefrigeratorId, cascadeDelete: true)
                .ForeignKey("dbo.FoodItems", t => t.FoodItem_FoodItemId, cascadeDelete: true)
                .Index(t => t.Refrigerator_RefrigeratorId)
                .Index(t => t.FoodItem_FoodItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefrigeratorFoodItems", "FoodItem_FoodItemId", "dbo.FoodItems");
            DropForeignKey("dbo.RefrigeratorFoodItems", "Refrigerator_RefrigeratorId", "dbo.Refrigerators");
            DropIndex("dbo.RefrigeratorFoodItems", new[] { "FoodItem_FoodItemId" });
            DropIndex("dbo.RefrigeratorFoodItems", new[] { "Refrigerator_RefrigeratorId" });
            DropTable("dbo.RefrigeratorFoodItems");
            DropTable("dbo.Refrigerators");
        }
    }
}
