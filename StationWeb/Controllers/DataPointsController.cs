using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using StationWeb.DAL;
using StationWeb.Models;
using StationWeb.DTOs;
using System.Web.Http.Cors;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace StationWeb.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "GET,POST")]
    public class DataPointsController : ApiController
    {
        private StationWebContext db = new StationWebContext();

        // GET: api/DataPoints
        public IQueryable<DataPoint> GetDataPoints()
        {
            return db.DataPoints;
        }

        // GET: api/DataPoints/5
        [ResponseType(typeof(DataPoint))]
        public IHttpActionResult GetDataPoint(int id)
        {
            DataPoint dataPoint = db.DataPoints.Find(id);
            if (dataPoint == null)
            {
                return NotFound();
            }

            return Ok(dataPoint);
        }

        // GET: datapoints by date range
        [Route("api/datapoints/{start:datetime}/{end:datetime}")]
        [HttpGet]
        [ResponseType(typeof(DatapointsQuery))]
        public IHttpActionResult GetDatapointsDates(DateTime start, DateTime end)
        {
            var dataPointsResult = from datapoint in db.DataPoints
                                    .Where(d => d.TimeStamp >= start && d.TimeStamp <= end)
                                   from equipment in db.Equipments
                                       .Where(e => e.ID == datapoint.equipmentID)
                                   from station in db.Stations
                                       .Where(s => s.ID == equipment.StationID)
                                   from sensor in db.Sensors
                                       .Where(s => s.ID == equipment.SensorID)
                                   from measure in db.Measure
                                        .Where(m=>m.ID==sensor.measureID)
                                   select new DatapointsQuery
                                   {
                                       ID = datapoint.ID,
                                       TimeStamp = datapoint.TimeStamp,
                                       Value = datapoint.Value,
                                       equipmentID=equipment.ID,
                                       Model=sensor.Model,
                                       measureType=measure.Type
                                       
                                   };

            return Ok(dataPointsResult);
            
        }


        // GET: datapoints from a station and date range
        [Route("api/stations/{stationid:int}/datapoints/{start:datetime}/{end:datetime}")]
        [HttpGet]
        [ResponseType(typeof(Station))]
        public IHttpActionResult StationReadingsTest(int stationid, DateTime start, DateTime end)
        {
            var result = from s in db.Stations
                         select new
                         {
                             ID=s.ID,
                             Name=s.Name,
                             LocLattitude=s.LocLattitude,
                             LocLongitude=s.LocLongitude,
                             Key="",
                             equipment = (from e in db.Equipments select new {
                                 ID=e.ID,
                                 StationID=e.StationID,
                                 SensorID=e.SensorID,
                                 sensor=(from sensors in db.Sensors 
                                             .Where(sens=>sens.ID == e.SensorID)
                                         select sensors).FirstOrDefault(),
                                 datapoint=(from datapoints in db.DataPoints
                                                .Where(d => d.TimeStamp >= start && d.TimeStamp <= end && d.equipmentID==e.ID)
                                            select datapoints).ToList()
                             }).ToList()

                         };

            return Ok(result.ToList());
        }

    
        // POST: api/DataPoints
        [ResponseType(typeof(DataPoint))]
        public IHttpActionResult PostDataPoint(Reading reading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Equipment equipment = db.Equipments.Find(reading.equipmentID);

            if(equipment == null)
            {
                return NotFound();
            }

            int stationId = equipment.StationID;
            String stationKey = db.Stations.Find(stationId).Key;

            if (stationKey.Equals(reading.Key))
            {
                var dataPoint = new DataPoint
                {
                    TimeStamp = reading.TimeStamp,
                    Value = reading.Value,
                    equipmentID = reading.equipmentID
                };

                db.DataPoints.Add(dataPoint);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = dataPoint.ID }, dataPoint);
            }
            else
            {
                return Unauthorized();
            }
   
        }

        // DELETE: api/DataPoints/5
        [Authorize]
        [ResponseType(typeof(DataPoint))]
        public IHttpActionResult DeleteDataPoint(int id)
        {
            DataPoint dataPoint = db.DataPoints.Find(id);
            if (dataPoint == null)
            {
                return NotFound();
            }

            db.DataPoints.Remove(dataPoint);
            db.SaveChanges();

            return Ok(dataPoint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DataPointExists(int id)
        {
            return db.DataPoints.Count(e => e.ID == id) > 0;
        }
    }
}