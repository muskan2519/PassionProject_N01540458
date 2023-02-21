using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_N01540458.Models.ViewModels
{
    public class DetailsFoodItem
    {
        public FoodItemDto SelectedFoodItem { get; set; }
        public IEnumerable<RecipeDto> ContainedRecipes { get; set; }
        public IEnumerable<RefrigeratorDto> ContainedRefrigerators { get; set; }
    }
}