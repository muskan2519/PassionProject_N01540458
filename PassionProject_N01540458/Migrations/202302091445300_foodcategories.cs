namespace PassionProject_N01540458.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foodcategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FoodCategories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FoodCategories");
        }
    }
}
