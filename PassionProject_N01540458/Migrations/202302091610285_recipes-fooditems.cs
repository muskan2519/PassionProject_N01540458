namespace PassionProject_N01540458.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipesfooditems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        RecipeId = c.Int(nullable: false, identity: true),
                        RecipeName = c.String(),
                        RecipeDescription = c.String(),
                    })
                .PrimaryKey(t => t.RecipeId);
            
            CreateTable(
                "dbo.RecipeFoodItems",
                c => new
                    {
                        Recipe_RecipeId = c.Int(nullable: false),
                        FoodItem_FoodItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipe_RecipeId, t.FoodItem_FoodItemId })
                .ForeignKey("dbo.Recipes", t => t.Recipe_RecipeId, cascadeDelete: true)
                .ForeignKey("dbo.FoodItems", t => t.FoodItem_FoodItemId, cascadeDelete: true)
                .Index(t => t.Recipe_RecipeId)
                .Index(t => t.FoodItem_FoodItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecipeFoodItems", "FoodItem_FoodItemId", "dbo.FoodItems");
            DropForeignKey("dbo.RecipeFoodItems", "Recipe_RecipeId", "dbo.Recipes");
            DropIndex("dbo.RecipeFoodItems", new[] { "FoodItem_FoodItemId" });
            DropIndex("dbo.RecipeFoodItems", new[] { "Recipe_RecipeId" });
            DropTable("dbo.RecipeFoodItems");
            DropTable("dbo.Recipes");
        }
    }
}
