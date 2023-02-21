using PassionProject_N01540458.Models;
using PassionProject_N01540458.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProject_N01540458.Controllers
{
    public class RefrigeratorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static RefrigeratorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44357/api/");
        }
        // GET: Refrigerator
        public ActionResult List()
        {
            // communicate with refrigerator api to retrieve a list of refrigerators
            // curl https://localhost:44357/api/RefrigeratorData/ListRefrigerators

            string url = "RefrigeratorData/ListRefrigerators";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RefrigeratorDto> refrigerators = response.Content.ReadAsAsync<IEnumerable<RefrigeratorDto>>().Result;

            return View(refrigerators);
        }

        // GET: Refrigerator/Details/5
        public ActionResult Details(int id)
        {
            DetailsRefrigerator ViewModel = new DetailsRefrigerator();
            // communicate with refrigerator api to retrieve details of the refrigerators
            // curl https://localhost:44357/api/RefrigeratorData/FindRefrigerator/{id}

            string url = "RefrigeratorData/FindRefrigerator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RefrigeratorDto refrigerator = response.Content.ReadAsAsync<RefrigeratorDto>().Result;
            ViewModel.SelectedRefrigerator = refrigerator;

            //show all food items related to this refrigerator
            url = "fooditemdata/ListRefrigeratorsForItem/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<FoodItemDto> ContainedFoodItems = response.Content.ReadAsAsync<IEnumerable<FoodItemDto>>().Result;
            ViewModel.ContainedFoodItems = ContainedFoodItems;

            //show all food items not related to this refrigerator
            url = "fooditemdata/ListFoodItemsNotForRefrigerator/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<FoodItemDto> AvailableFoodItems = response.Content.ReadAsAsync<IEnumerable<FoodItemDto>>().Result;
            ViewModel.AvailableFoodItems = AvailableFoodItems;

            return View(ViewModel);
        }

        //POST: Refrigerator/Associate/{refrigeratorid}
        [HttpPost]
        public ActionResult Associate(int id, int FoodItemId)
        {
            //call our api to associate refrigerator with food item
            string url = "RefrigeratorData/AssociateRefrigeratorWithFoodItem/" + id + "/" + FoodItemId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //GET: Refrigerator/UnAssociate/{refrigeratorid}
        [HttpGet]
        public ActionResult UnAssociate(int id, int FoodItemId)
        {

            //call our api to unassociate refrigerator with food item
            string url = "RefrigeratorData/UnAssociateRefrigeratorWithFoodItem/" + id + "/" + FoodItemId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        // GET: Refrigerator/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Refrigerator/Create
        [HttpPost]
        public ActionResult Create(Refrigerator refrigerator)
        {
            // communicate with refrigerator api to add details of the new refrigerator
            // curl -H "Content-Type:application/json" -d @refrigerator.json https://localhost:44357/api/RefrigeratorData/AddRefrigerator
            string url = "RefrigeratorData/AddRefrigerator";

            // converting payload to json to send to api

            string jsonpayload = jss.Serialize(refrigerator);

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

        // GET: Refrigerator/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "RefrigeratorData/FindRefrigerator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RefrigeratorDto refrigerator = response.Content.ReadAsAsync<RefrigeratorDto>().Result;
            return View(refrigerator);
        }

        // POST: Refrigerator/Update/5
        [HttpPost]
        public ActionResult Update(int id, Refrigerator refrigerator)
        {
            string url = "RefrigeratorData/UpdateRefrigerator/" + id;

            // converting payload to json to send to api

            string jsonpayload = jss.Serialize(refrigerator);

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

        // GET: Refrigerator/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "RefrigeratorData/FindRefrigerator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RefrigeratorDto refrigerator = response.Content.ReadAsAsync<RefrigeratorDto>().Result;
            return View(refrigerator);
        }

        // POST: Refrigerator/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "RefrigeratorData/DeleteRefrigerator/" + id;

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
