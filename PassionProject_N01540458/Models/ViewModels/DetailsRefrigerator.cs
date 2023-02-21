using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_N01540458.Models.ViewModels
{
    public class DetailsRefrigerator
    {
        public RefrigeratorDto SelectedRefrigerator { get; set; }
        public IEnumerable<FoodItemDto> ContainedFoodItems { get; set; }

        public IEnumerable<FoodItemDto> AvailableFoodItems { get; set; }
    }
}