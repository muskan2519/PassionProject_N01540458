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

        // Here two functions will be defined - Associate and Unassociate.
        // These functions will be given for many-to-many relation between food items and recipes.

        // GET: Recipe/Details/5
        public ActionResult Details(int id)
        {
            // communicate with recipe api to retrieve details of the recipes
            // curl https://localhost:44357/api/RecipeData/FindRecipe/{id}

            string url = "RecipeData/FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            // here a viewmodel will be called to show associated food items with the recipe


            return View(recipe);
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
