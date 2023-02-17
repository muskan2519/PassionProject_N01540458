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

        // Below three functions will be made to show many-to-many relationship between food items and recipes
        // 1. ListFoodItemsForRecipes(int id) - work similar to ListFoodItemsForFoodCategory
        // 2. AssociateFoodItemToRecipe(int ItemId, int RecipeId) - to give relation between selected recipe and food item
        // 3. UnassociateFoodItemToRecipe(int ItemId, int RecipeId) - to remove relation between selected recipe and food item

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