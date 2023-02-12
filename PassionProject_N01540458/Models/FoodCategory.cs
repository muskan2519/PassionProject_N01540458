using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject_N01540458.Models
{
    public class FoodCategory
    {
        // field for unique food category id
        [Key]
        public int CategoryId { get; set; }

        // field for food category name
        public string CategoryName { get; set; }
    }

    public class FoodCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}