using PassionProject_N01540458.Models;
using PassionProject_N01540458.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProject_N01540458.Controllers
{
    public class RecipeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static RecipeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44357/api/");
        }
        // GET: Recipe
        public ActionResult List()
        {
            // communicate with recipe api to retrieve a list of recipes
            // curl https://localhost:44357/api/RecipeData/ListRecipes

            string url = "RecipeData/ListRecipes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RecipeDto> recipes = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;

            return View(recipes);
        }

        // GET: Recipe/Details/5
        public ActionResult Details(int id)
        {
            DetailsRecipe ViewModel = new DetailsRecipe();
            // communicate with recipe api to retrieve details of the recipes
            // curl https://localhost:44357/api/RecipeData/FindRecipe/{id}

            string url = "RecipeData/FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto recipe = response.Content.ReadAsAsync<RecipeDto>().Result;
            ViewModel.SelectedRecipe = recipe;

            //show all food items related to this recipe
            url = "fooditemdata/ListRecipesForItem/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<FoodItemDto> ContainedFoodItems = response.Content.ReadAsAsync<IEnumerable<FoodItemDto>>().Result;
            ViewModel.ContainedFoodItems = ContainedFoodItems;

            //show all food items not related to this recipe
            url = "fooditemdata/ListFoodItemsNotForRecipe/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<FoodItemDto> AvailableFoodItems = response.Content.ReadAsAsync<IEnumerable<FoodItemDto>>().Result;
            ViewModel.AvailableFoodItems = AvailableFoodItems;

            return View(ViewModel);
        }

        //POST: Recipe/Associate/{recipeid}
        [HttpPost]
        public ActionResult Associate(int id, int FoodItemId)
        {
            //call our api to associate recipe with food item
            string url = "RecipeData/AssociateRecipeWithFoodItem/" + id + "/" + FoodItemId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //GET: Recipe/UnAssociate/{id}?FoodItemId={FoodItemId}
        [HttpGet]
        public ActionResult UnAssociate(int id, int FoodItemId)
        {

            //call our api to remove association recipe with food item
            string url = "RecipeData/UnAssociateRecipeWithFoodItem/" + id + "/" + FoodItemId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        // GET: Recipe/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        public ActionResult Create(Recipe recipe)
        {
            // communicate with recipe api to add details of the new recipe
            // curl -H "Content-Type:application/json" -d @foodcategory.json https://localhost:44357/api/RecipeData/AddRecipe
            string url = "RecipeData/AddRecipe";

            // converting payload to json to send to api

            string jsonpayload = jss.Serialize(recipe);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Recipe/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "RecipeData/FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RecipeDto recipe = response.Content.ReadAsAsync<RecipeDto>().Result;
            return View(recipe);
        }

        // POST: Recipe/Update/5
        [HttpPost]
        public ActionResult Update(int id, Recipe recipe)
        {
            string url = "RecipeData/UpdateRecipe/" + id;

            // converting payload to json to send to api

            string jsonpayload = jss.Serialize(recipe);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Recipe/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "RecipeData/FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RecipeDto recipe = response.Content.ReadAsAsync<RecipeDto>().Result;
            return View(recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "RecipeData/DeleteRecipe/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
