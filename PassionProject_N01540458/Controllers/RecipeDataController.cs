using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject_N01540458.Models;

namespace PassionProject_N01540458.Controllers
{
    public class RecipeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RecipeData/ListRecipes
        [HttpGet]
        public IEnumerable<RecipeDto> ListRecipes()
        {
            List<Recipe> Recipes = db.Recipes.ToList();
            List<RecipeDto> RecipesDto = new List<RecipeDto>();

            Recipes.ForEach(f => RecipesDto.Add(new RecipeDto()
            {
                RecipeId = f.RecipeId,
                RecipeName = f.RecipeName,
                RecipeDescription = f.RecipeDescription
            }));

            return RecipesDto;
        }

        /// <summary>
        /// Gathers information about all food items related to a particular recipe ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all food items in the database matched with a particular recipe ID
        /// </returns>
        /// <param name="id">Food item id</param>
        /// <example>
        /// GET: api/RecipeData/ListFoodItemsForRecipe/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(RecipeDto))]
        public IHttpActionResult ListFoodItemsForRecipe(int id)
        {
            // all recipes that have food items which match with the id 
            List<Recipe> Recipes = db.Recipes.Where(a => a.FoodItems.Any(k=>k.FoodItemId == id)).ToList();
            List<RecipeDto> RecipeDtos = new List<RecipeDto>();

            Recipes.ForEach(a => RecipeDtos.Add(new RecipeDto()
            {
                RecipeId = a.RecipeId,
                RecipeName = a.RecipeName,
                RecipeDescription = a.RecipeDescription,
            }));

            return Ok(RecipeDtos);
        }

        /// <summary>
        /// Associates a particular food item with a particular recipe
        /// </summary>
        /// <param name="recipeid">The recipe ID primary key</param>
        /// <param name="fooditemid">The food item ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/RecipeData/AssociateRecipeWithFoodItem/1/1
        /// </example>
        [HttpPost]
        [Route("api/RecipeData/AssociateRecipeWithFoodItem/{recipeid}/{fooditemid}")]
        public IHttpActionResult AssociateRecipeWithFoodItem(int recipeid, int fooditemid)
        {

            Recipe SelectedRecipe = db.Recipes.Include(a => a.FoodItems).Where(a => a.RecipeId == recipeid).FirstOrDefault();
            FoodItem SelectedFoodItem = db.FoodItems.Find(fooditemid);

            if (SelectedRecipe == null || SelectedFoodItem == null)
            {
                return NotFound();
            }

            SelectedRecipe.FoodItems.Add(SelectedFoodItem);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association a particular food item with a particular recipe
        /// </summary>
        /// <param name="recipeid">The recipe ID primary key</param>
        /// <param name="fooditemid">The food item ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/RecipeData/UnAssociateRecipeWithFoodItem/1/1
        /// </example>
        [HttpPost]
        [Route("api/RecipeData/UnAssociateRecipeWithFoodItem/{recipeid}/{fooditemid}")]
        public IHttpActionResult UnAssociateRecipeWithFoodItem(int recipeid, int fooditemid)
        {

            Recipe SelectedRecipe = db.Recipes.Include(a => a.FoodItems).Where(a => a.RecipeId == recipeid).FirstOrDefault();
            FoodItem SelectedFoodItem = db.FoodItems.Find(fooditemid);

            if (SelectedRecipe == null || SelectedFoodItem == null)
            {
                return NotFound();
            }

            SelectedRecipe.FoodItems.Remove(SelectedFoodItem);
            db.SaveChanges();

            return Ok();
        }

        // GET: api/RecipeData/FindRecipe/5
        [ResponseType(typeof(Recipe))]
        [HttpGet]
        public IHttpActionResult FindRecipe(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            RecipeDto recipeDto = new RecipeDto()
            {
                RecipeId = recipe.RecipeId,
                RecipeName = recipe.RecipeName,
                RecipeDescription= recipe.RecipeDescription
            };
            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipeDto);
        }

        // POST: api/RecipeData/UpdateRecipe/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRecipe(int id, Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipe.RecipeId)
            {
                return BadRequest();
            }

            db.Entry(recipe).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
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

        // POST: api/RecipeData/AddRecipe
        [ResponseType(typeof(Recipe))]
        public IHttpActionResult AddRecipe(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Recipes.Add(recipe);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recipe.RecipeId }, recipe);
        }

        // DELETE: api/RecipeData/DeleteRecipe/5
        [ResponseType(typeof(Recipe))]
        [HttpPost]
        public IHttpActionResult DeleteRecipe(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            db.Recipes.Remove(recipe);
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

        private bool RecipeExists(int id)
        {
            return db.Recipes.Count(e => e.RecipeId == id) > 0;
        }
    }
}