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
    public class SensorsController : ApiController
    {
        private StationWebContext db = new StationWebContext();

        // GET: api/Sensors
        public IQueryable<Sensor> GetSensors()
        {
            return db.Sensors;
        }

        // GET: api/Sensors/5
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult GetSensor(int id)
        {
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        // PUT: api/Sensors/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSensor(int id, Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensor.ID)
            {
                return BadRequest();
            }

            db.Entry(sensor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorExists(id))
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

        // POST: api/Sensors
        [Authorize]
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult PostSensor(Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sensors.Add(sensor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sensor.ID }, sensor);
        }

        // DELETE: api/Sensors/5
        [Authorize]
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult DeleteSensor(int id)
        {
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            db.Sensors.Remove(sensor);
            db.SaveChanges();

            return Ok(sensor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SensorExists(int id)
        {
            return db.Sensors.Count(e => e.ID == id) > 0;
        }
    }
}