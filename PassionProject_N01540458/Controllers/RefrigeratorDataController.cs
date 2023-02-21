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
    public class RefrigeratorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RefrigeratorData/ListRecipes
        [HttpGet]
        public IEnumerable<RefrigeratorDto> ListRefrigerators()
        {
            List<Refrigerator> Refrigerators = db.Refrigerators.ToList();
            List<RefrigeratorDto> RefrigeratorsDto = new List<RefrigeratorDto>();

            Refrigerators.ForEach(f => RefrigeratorsDto.Add(new RefrigeratorDto()
            {
                RefrigeratorId = f.RefrigeratorId,
                RefrigeratorName = f.RefrigeratorName,
                UserName = f.UserName
            }));

            return RefrigeratorsDto;
        }

        /// <summary>
        /// Gathers information about all food items related to a particular refrigerator ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all food items in the database matched with a particular refrigerator ID
        /// </returns>
        /// <param name="id">Food item id</param>
        /// <example>
        /// GET: api/RefrigeratoreData/ListFoodItemsForRefrigerator/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(RefrigeratorDto))]
        public IHttpActionResult ListFoodItemsForRefrigerator(int id)
        {
            // all refrigerators that have food items which match with the id 
            List<Refrigerator> Refrigerators = db.Refrigerators.Where(a => a.FoodItems.Any(k => k.FoodItemId == id)).ToList();
            List<RefrigeratorDto> RefrigeratorDtos = new List<RefrigeratorDto>();

            Refrigerators.ForEach(a => RefrigeratorDtos.Add(new RefrigeratorDto()
            {
                RefrigeratorId = a.RefrigeratorId,
                RefrigeratorName = a.RefrigeratorName,
                UserName = a.UserName
            }));

            return Ok(RefrigeratorDtos);
        }

        /// <summary>
        /// Associates a particular food item with a particular refrigerator
        /// </summary>
        /// <param name="refrigeratorid">The refrigerator ID primary key</param>
        /// <param name="fooditemid">The food item ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/RefrigeratorData/AssociateRefrigeratorWithFoodItem/1/1
        /// </example>
        [HttpPost]
        [Route("api/RefrigeratorData/AssociateRefrigeratorWithFoodItem/{refrigeratorid}/{fooditemid}")]
        public IHttpActionResult AssociateRefrigeratorWithFoodItem(int refrigeratorid, int fooditemid)
        {

            Refrigerator SelectedRefrigerator = db.Refrigerators.Include(a => a.FoodItems).Where(a => a.RefrigeratorId == refrigeratorid).FirstOrDefault();
            FoodItem SelectedFoodItem = db.FoodItems.Find(fooditemid);

            if (SelectedRefrigerator == null || SelectedFoodItem == null)
            {
                return NotFound();
            }

            SelectedRefrigerator.FoodItems.Add(SelectedFoodItem);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Remove an association of a particular food item with a particular refrigerator
        /// </summary>
        /// <param name="refrigeratorid">The refrigerator ID primary key</param>
        /// <param name="fooditemid">The food item ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/RefrigeratorData/UnAssociateRefrigeratorWithFoodItem/1/1
        /// </example>
        [HttpPost]
        [Route("api/RefrigeratorData/UnAssociateRefrigeratorWithFoodItem/{refrigeratorid}/{fooditemid}")]
        public IHttpActionResult UnAssociateRefrigeratorWithFoodItem(int refrigeratorid, int fooditemid)
        {

            Refrigerator SelectedRefrigerator = db.Refrigerators.Include(a => a.FoodItems).Where(a => a.RefrigeratorId == refrigeratorid).FirstOrDefault();
            FoodItem SelectedFoodItem = db.FoodItems.Find(fooditemid);

            if (SelectedRefrigerator == null || SelectedFoodItem == null)
            {
                return NotFound();
            }

            SelectedRefrigerator.FoodItems.Remove(SelectedFoodItem);
            db.SaveChanges();

            return Ok();
        }

        // GET: api/RefrigeratorData/FindRefrigerator/5
        [ResponseType(typeof(Refrigerator))]
        [HttpGet]
        public IHttpActionResult FindRefrigerator(int id)
        {
            Refrigerator refrigerator = db.Refrigerators.Find(id);
            RefrigeratorDto refrigeratorDto = new RefrigeratorDto()
            {
                RefrigeratorId = refrigerator.RefrigeratorId,
                RefrigeratorName = refrigerator.RefrigeratorName,
                UserName = refrigerator.UserName
            };
            if (refrigerator == null)
            {
                return NotFound();
            }

            return Ok(refrigeratorDto);
        }

        // POST: api/RefrigeratorData/UpdateRefrigerator/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRefrigerator(int id, Refrigerator refrigerator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != refrigerator.RefrigeratorId)
            {
                return BadRequest();
            }

            db.Entry(refrigerator).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RefrigeratorExists(id))
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

        // POST: api/RefrigeratorData/AddRefrigerator
        [ResponseType(typeof(Refrigerator))]
        public IHttpActionResult AddRefrigerator(Refrigerator refrigerator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Refrigerators.Add(refrigerator);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = refrigerator.RefrigeratorId }, refrigerator);
        }

        // DELETE: api/RefrigeratorData/DeleteRefrigerator/5
        [ResponseType(typeof(Refrigerator))]
        [HttpPost]
        public IHttpActionResult DeleteRefrigerator(int id)
        {
            Refrigerator refrigerator = db.Refrigerators.Find(id);
            if (refrigerator == null)
            {
                return NotFound();
            }

            db.Refrigerators.Remove(refrigerator);
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

        private bool RefrigeratorExists(int id)
        {
            return db.Refrigerators.Count(e => e.RefrigeratorId == id) > 0;
        }
    }
}