using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_N01540458.Models.ViewModels
{
    public class DetailsFoodCategory
    {
        //the food categories itself that we want to display
        public FoodCategoryDto SelectedFoodCategories { get; set; }

        //all of the related food items to that particular food category
        public IEnumerable<FoodItemDto> RelatedFoodItems { get; set; }
    }
}