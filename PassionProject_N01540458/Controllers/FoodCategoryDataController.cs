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

namespace PassionProject_N01540458.Controllers
{
    public class FoodCategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FoodCategoryData/ListFoodCategories
        [HttpGet]
        public IEnumerable<FoodCategoryDto> ListFoodCategories()
        {
            List<FoodCategory> FoodCategories = db.FoodCategories.ToList();
            List<FoodCategoryDto> FoodCategoriesDto = new List<FoodCategoryDto>();

            FoodCategories.ForEach(f => FoodCategoriesDto.Add(new FoodCategoryDto()
            {
                CategoryId = f.CategoryId,
                CategoryName = f.CategoryName
            }));

            return FoodCategoriesDto;
        }

        // GET: api/FoodCategoryData/FindFoodCategory/5
        [ResponseType(typeof(FoodCategory))]
        [HttpGet]
        public IHttpActionResult FindFoodCategory(int id)
        {
            FoodCategory foodCategory = db.FoodCategories.Find(id);
            FoodCategoryDto foodCategoryDto = new FoodCategoryDto()
            {
                CategoryId = foodCategory.CategoryId,
                CategoryName = foodCategory.CategoryName
            };
            if (foodCategory == null)
            {
                return NotFound();
            }

            return Ok(foodCategoryDto);
        }

        // POST: api/FoodCategoryData/UpdateFoodCategory/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateFoodCategory(int id, FoodCategory foodCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != foodCategory.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(foodCategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodCategoryExists(id))
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

        // POST: api/FoodCategoryData/AddFoodCategory
        [ResponseType(typeof(FoodCategory))]
        [HttpPost]
        public IHttpActionResult AddFoodCategory(FoodCategory foodCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FoodCategories.Add(foodCategory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = foodCategory.CategoryId }, foodCategory);
        }

        // DELETE: api/FoodCategoryData/DeleteFoodCategory/5
        [ResponseType(typeof(FoodCategory))]
        [HttpPost]
        public IHttpActionResult DeleteFoodCategory(int id)
        {
            FoodCategory foodCategory = db.FoodCategories.Find(id);
            if (foodCategory == null)
            {
                return NotFound();
            }

            db.FoodCategories.Remove(foodCategory);
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

        private bool FoodCategoryExists(int id)
        {
            return db.FoodCategories.Count(e => e.CategoryId == id) > 0;
        }
    }
}