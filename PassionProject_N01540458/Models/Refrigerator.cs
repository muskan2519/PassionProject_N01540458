using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject_N01540458.Models
{
    public class Refrigerator
    {
        [Key]
        public int RefrigeratorId { get; set; }
        public string UserName { get; set; }
        public string RefrigeratorName { get; set; }

        // A refrigerator can take multiple food items
        public ICollection<FoodItem> FoodItems { get; set; }
    }
}