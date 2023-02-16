using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject_N01540458.Models;
using System.Diagnostics;

namespace PassionProject_N01540458.Controllers
{
    public class FoodItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FoodItemData/ListFoodItems
        [HttpGet]
        public IEnumerable<FoodItemDto> ListFoodItems()
        {
            List<FoodItem> FoodItems = db.FoodItems.ToList();
            List<FoodItemDto> FoodItemsDto = new List<FoodItemDto>();

            FoodItems.ForEach(f => FoodItemsDto.Add(new FoodItemDto()
            {
                FoodItemId = f.FoodItemId,
                FoodItemName = f.FoodItemName,
                FoodCategoryId= f.FoodCategoryId,
                FoodCategoryName = f.FoodCategory.CategoryName
            }));

            return FoodItemsDto;
        }

        /// <summary>
        /// Gathers information about all food items related to a particular category ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all food items in the database, including their associated categories matched with a particular category ID
        /// </returns>
        /// <param name="id">FOodCategory ID.</param>
        /// <example>
        /// GET: api/FoodItemData/ListFoodItemsForFoodCategory/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(FoodItemDto))]
        public IHttpActionResult ListFoodItemsForFoodCategory(int id)
        {
            List<FoodItem> FoodItems = db.FoodItems.Where(a => a.FoodCategoryId == id).ToList();
            List<FoodItemDto> FoodItemDtos = new List<FoodItemDto>();

            FoodItems.ForEach(a => FoodItemDtos.Add(new FoodItemDto()
            {
                FoodItemId = a.FoodItemId,
                FoodItemName = a.FoodItemName,
                FoodCategoryId = a.FoodCategory.CategoryId,
                FoodCategoryName = a.FoodCategory.CategoryName
            }));

            return Ok(FoodItemDtos);
        }

        // GET: api/FoodItemData/FindFoodItem/5
        [ResponseType(typeof(FoodItem))]
        [HttpGet]
        public IHttpActionResult FindFoodItem(int id)
        {
            FoodItem foodItem = db.FoodItems.Find(id);
            FoodItemDto foodItemDto = new FoodItemDto()
            {
                FoodItemId = foodItem.FoodItemId,
                FoodItemName = foodItem.FoodItemName,
                FoodCategoryId= foodItem.FoodCategoryId,
                FoodCategoryName = foodItem.FoodCategory.CategoryName
            };
            if (foodItem == null)
            {
                return NotFound();
            }

            return Ok(foodItemDto);
        }

        // POST: api/FoodItemData/UpdateFoodItem/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateFoodItem(int id, FoodItem foodItem)
        {
            Debug.WriteLine("reached update method");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != foodItem.FoodItemId)
            {
                return BadRequest();
            }

            db.Entry(foodItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/FoodItemData/AddFoodItem
        [ResponseType(typeof(FoodItem))]
        [HttpPost]
        public IHttpActionResult AddFoodItem(FoodItem foodItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FoodItems.Add(foodItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = foodItem.FoodItemId }, foodItem);
        }

        // POST: api/FoodItemData/DeleteFoodItem/5
        [ResponseType(typeof(FoodItem))]
        [HttpPost]
        public IHttpActionResult DeleteFoodItem(int id)
        {
            FoodItem foodItem = db.FoodItems.Find(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            db.FoodItems.Remove(foodItem);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FoodItemExists(int id)
        {
            return db.FoodItems.Count(e => e.FoodItemId == id) > 0;
        }
    }
}