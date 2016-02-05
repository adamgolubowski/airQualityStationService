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
using StationWeb.DAL;
using StationWeb.Models;

namespace StationWeb.Controllers
{
    public class EquipmentsController : ApiController
    {
        private StationWebContext db = new StationWebContext();

        // GET: api/Equipments
        public IQueryable<Equipment> GetEquipments()
        {
            return db.Equipments;
        }

        // GET: api/Equipments/5
        [ResponseType(typeof(Equipment))]
        public IHttpActionResult GetEquipment(int id)
        {
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }

        // PUT: api/Equipments/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEquipment(int id, Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != equipment.ID)
            {
                return BadRequest();
            }

            db.Entry(equipment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
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

        // POST: api/Equipments
        [Authorize]
        [ResponseType(typeof(Equipment))]
        public IHttpActionResult PostEquipment(Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Equipments.Add(equipment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = equipment.ID }, equipment);
        }

        // DELETE: api/Equipments/5
        [Authorize]
        [ResponseType(typeof(Equipment))]
        public IHttpActionResult DeleteEquipment(int id)
        {
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return NotFound();
            }

            db.Equipments.Remove(equipment);
            db.SaveChanges();

            return Ok(equipment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EquipmentExists(int id)
        {
            return db.Equipments.Count(e => e.ID == id) > 0;
        }
    }
}