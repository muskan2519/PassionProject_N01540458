namespace PassionProject_N01540458.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fooditems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FoodItems",
                c => new
                    {
                        FoodItemId = c.Int(nullable: false, identity: true),
                        FoodItemName = c.String(),
                        FoodCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FoodItemId)
                .ForeignKey("dbo.FoodCategories", t => t.FoodCategoryId, cascadeDelete: true)
                .Index(t => t.FoodCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FoodItems", "FoodCategoryId", "dbo.FoodCategories");
            DropIndex("dbo.FoodItems", new[] { "FoodCategoryId" });
            DropTable("dbo.FoodItems");
        }
    }
}
