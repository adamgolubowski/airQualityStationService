using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using StationWeb.DAL;
using StationWeb.Models;
using StationWeb.DTOs;
using System.Collections.Generic;
using System.Web.Http.Cors;

namespace StationWeb.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "GET,POST")]
    public class StationsController : ApiController
    {
        private StationWebContext db = new StationWebContext();

        // GET: api/Stations
        public IQueryable<StationPublic> GetStations()
        {
            var stations = from s in db.Stations
                           select new StationPublic
                           {
                               ID = s.ID,
                               Name = s.Name
                           };
            return stations;
        }

        // GET: api/Stations/5
        [ResponseType(typeof(StationPublicDetails))]
        public IHttpActionResult GetStation(int id)
        {
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            var sensorsList = from equipment in db.Equipments
                              .Where(e => e.StationID == id)
                              from sensors in db.Sensors
                                .Where(s => s.ID == equipment.SensorID)
                              from measures in db.Measure
                                .Where(m => m.ID == sensors.measureID)
                              select new SensorDetails
                              {
                                  equipmentID = equipment.ID,
                                  Model = sensors.Model,
                                  measureType = measures.Type
                              };

            StationPublicDetails stationDetails = new StationPublicDetails
            {
                ID = station.ID,
                Name = station.Name,
                LocLattitude = station.LocLattitude,
                LocLongitude = station.LocLongitude,
                Sensors = sensorsList.ToList()
        };

            return Ok(stationDetails);
        }

        [Authorize]
        [Route("api/stations/{id:int}/key")]
        public IHttpActionResult GetStationKey(int id)
        {
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            return Ok(station.Key);
        }

        // PUT: api/Stations/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStation(int id, Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != station.ID)
            {
                return BadRequest();
            }

            db.Entry(station).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationExists(id))
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

        // POST: api/Stations
        [Authorize]
        [ResponseType(typeof(Station))]
        public IHttpActionResult PostStation(Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //generate key and assign to a station
            String generatedKey = RandomString(32);
            station.Key = generatedKey;

            db.Stations.Add(station);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = station.ID }, station);
        }

        // DELETE: api/Stations/5
        [Authorize]
        [ResponseType(typeof(Station))]
        public IHttpActionResult DeleteStation(int id)
        {
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            db.Stations.Remove(station);
            db.SaveChanges();

            return Ok(station);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StationExists(int id)
        {
            return db.Stations.Count(e => e.ID == id) > 0;
        }


        // generate random string for API key
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}