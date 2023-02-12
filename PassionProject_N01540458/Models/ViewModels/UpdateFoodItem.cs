using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_N01540458.Models.ViewModels
{
    public class UpdateFoodItem
    {
        // this view model is a class that stores information that we need to present to /FoodItem/Update/{}

        // existing food item information
        public FoodItemDto SelectedFoodItem { get; set; }
        // all food categories to choose from
        public IEnumerable<FoodCategoryDto> FoodCategoriesOptions { get; set; }

    }
}