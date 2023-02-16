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
    public class FoodCategoryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static FoodCategoryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44357/api/");
        }
        // GET: FoodCategory
        public ActionResult List()
        {
            // communicate with foodcategory api to retrieve a list of food categories
            // curl https://localhost:44357/api/FoodCategoryData/ListFoodCategories

            string url = "FoodCategoryData/ListFoodCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<FoodCategoryDto> foodcategories = response.Content.ReadAsAsync<IEnumerable<FoodCategoryDto>>().Result;

            return View(foodcategories);
        }

        // GET: FoodCategory/Details/5
        public ActionResult Details(int id)
        {
            DetailsFoodCategory ViewModel = new DetailsFoodCategory();
            // communicate with foodcategory api to retrieve details of the food categories
            // curl https://localhost:44357/api/FoodCategoryData/FindFoodCategory/{id}

            string url = "FoodCategoryData/FindFoodCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            FoodCategoryDto foodcategory = response.Content.ReadAsAsync<FoodCategoryDto>().Result;
            ViewModel.SelectedFoodCategories= foodcategory;

            //showcase information about food items related to this food category
            //send a request to gather information about food items related to a particular category ID
            url = "FoodItemData/ListFoodItemsForFoodCategory/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<FoodItemDto> RelatedFoodItems = response.Content.ReadAsAsync<IEnumerable<FoodItemDto>>().Result;

            ViewModel.RelatedFoodItems = RelatedFoodItems;


            return View(ViewModel);
        }

        // GET: FoodCategory/New
        public ActionResult New()
        {
            return View();
        }

        // POST: FoodCategory/Create
        [HttpPost]
        public ActionResult Create(FoodCategory foodcategory)
        {
            // communicate with foodcategory api to add details of the new food category
            // curl -H "Content-Type:application/json" -d @foodcategory.json https://localhost:44357/api/FoodCategoryData/AddFoodCategory
            string url = "FoodCategoryData/AddFoodCategory";

            // converting payload to json to send to api

            string jsonpayload = jss.Serialize(foodcategory);

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

        // GET: FoodCategory/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FoodCategoryData/FindFoodCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FoodCategoryDto foodcategory = response.Content.ReadAsAsync<FoodCategoryDto>().Result;
            return View(foodcategory);
        }

        // POST: FoodCategory/Update/5
        [HttpPost]
        public ActionResult Update(int id, FoodCategory foodcategory)
        {
            string url = "FoodCategoryData/UpdateFoodCategory/" + id;

            // converting payload to json to send to api

            string jsonpayload = jss.Serialize(foodcategory);

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

        // GET: FoodCategory/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FoodCategoryData/FindFoodCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FoodCategoryDto foodcategory = response.Content.ReadAsAsync<FoodCategoryDto>().Result;
            return View(foodcategory);
        }

        // POST: FoodCategory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Debug.WriteLine("The function enters");
            string url = "FoodCategoryData/DeleteFoodCategory/" + id;

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
