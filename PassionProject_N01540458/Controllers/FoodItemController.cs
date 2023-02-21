using PassionProject_N01540458.Migrations;
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
    public class FoodItemController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static FoodItemController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44357/api/");
        }
        // GET: FoodItem
        public ActionResult List()
        {
            // communicate with fooditem api to retrieve a list of food items
            // curl https://localhost:44357/api/FoodItemData/ListFoodItems

            string url = "FoodItemData/ListFoodItems";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<FoodItemDto> fooditems = response.Content.ReadAsAsync<IEnumerable<FoodItemDto>>().Result;
            
            return View(fooditems);
        }

        // GET: FoodItem/Details/5
        public ActionResult Details(int id)
        {
            DetailsFoodItem ViewModel = new DetailsFoodItem();
            // communicate with fooditem api to retrieve details of the food item
            // curl https://localhost:44357/api/FoodItemData/FindFoodItem/{id}

            string url = "FoodItemData/FindFoodItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            FoodItemDto fooditem = response.Content.ReadAsAsync<FoodItemDto>().Result;

            ViewModel.SelectedFoodItem = fooditem;

            //show all recipes related to this food item
            url = "recipedata/ListFoodItemsForRecipe/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<RecipeDto> ContainedRecipes = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;

            ViewModel.ContainedRecipes = ContainedRecipes;

            return View(ViewModel);
        }

        // GET: FoodItem/New
        public ActionResult New()
        {
            // information about all food categories in the system
            // GET/foodcategoriesdata/listcategories

            string url = "FoodCategoryData/ListFoodCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<FoodCategoryDto> FoodCategoryOptions = response.Content.ReadAsAsync<IEnumerable<FoodCategoryDto>>().Result;

            return View(FoodCategoryOptions);
        }

        // POST: FoodItem/Create
        [HttpPost]
        public ActionResult Create(FoodItem fooditem)
        {
            // communicate with fooditem api to add details of the new food item
            // curl -H "Content-Type:application/json" -d @fooditem.json https://localhost:44357/api/FoodItemData/AddFoodItem
            string url = "FoodItemData/AddFoodItem";

            // converting payload to json to send to api
            
            string jsonpayload = jss.Serialize(fooditem);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType= "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
            

        }

        // GET: FoodItem/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateFoodItem ViewModel = new UpdateFoodItem();

            // existing food item information
            string url = "FoodItemData/FindFoodItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FoodItemDto fooditem = response.Content.ReadAsAsync<FoodItemDto>().Result;
            ViewModel.SelectedFoodItem = fooditem;

            // all categories to choose from when updating this food item
            url = "FoodCategoryData/ListFoodCategories";
            response = client.GetAsync(url).Result;
            IEnumerable<FoodCategoryDto> FoodCategoryOptions = response.Content.ReadAsAsync<IEnumerable<FoodCategoryDto>>().Result;
            ViewModel.FoodCategoriesOptions = FoodCategoryOptions;

            return View(ViewModel);
        }

        // POST: FoodItem/Update/5
        [HttpPost]
        public ActionResult Update(int id, FoodItem foodItem)
        {
            string url = "FoodItemData/UpdateFoodItem/" + id;

            // converting payload to json to send to api

            string jsonpayload = jss.Serialize(foodItem);

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

        // GET: FoodItem/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FoodItemData/FindFoodItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FoodItemDto fooditem = response.Content.ReadAsAsync<FoodItemDto>().Result;
            return View(fooditem);
        }

        // POST: FoodItem/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "FoodItemData/DeleteFoodItem/" + id;

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
