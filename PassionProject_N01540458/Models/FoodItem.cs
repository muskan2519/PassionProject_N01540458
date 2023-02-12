using PassionProject_N01540458.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProject_N01540458.Models
{
    public class FoodItem
    {
        // field for unique food item id
        [Key]
        public int FoodItemId { get; set; }

        // field for food item name
        public string FoodItemName { get; set; }

        // field to get the food category to which the food item belong
        // A food item can belong to one food category
        // A food category can have multiple food items
        [ForeignKey("FoodCategory")]
        public int FoodCategoryId { get; set; }
        public virtual FoodCategory FoodCategory { get; set; }

        // A food item can belong to multiple food recipes
        public ICollection<Recipe> Recipes { get; set; }
        // A food item can belong to multiple refrigerators
        public ICollection<Refrigerator> Refrigerators { get; set; }
    }

    public class FoodItemDto
    {
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; }
        public int FoodCategoryId { get; set; }
        public string FoodCategoryName { get; set; }
    }
}