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
    public class MeasuresController : ApiController
    {
        private StationWebContext db = new StationWebContext();

        // GET: api/Measures
        public IQueryable<Measure> GetMeasure()
        {
            return db.Measure;
        }

        // GET: api/Measures/5
        [ResponseType(typeof(Measure))]
        public IHttpActionResult GetMeasure(int id)
        {
            Measure measure = db.Measure.Find(id);
            if (measure == null)
            {
                return NotFound();
            }

            return Ok(measure);
        }

        // PUT: api/Measures/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMeasure(int id, Measure measure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != measure.ID)
            {
                return BadRequest();
            }

            db.Entry(measure).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasureExists(id))
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

        // POST: api/Measures
        [Authorize]
        [ResponseType(typeof(Measure))]
        public IHttpActionResult PostMeasure(Measure measure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Measure.Add(measure);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = measure.ID }, measure);
        }

        // DELETE: api/Measures/5
        [Authorize]
        [ResponseType(typeof(Measure))]
        public IHttpActionResult DeleteMeasure(int id)
        {
            Measure measure = db.Measure.Find(id);
            if (measure == null)
            {
                return NotFound();
            }

            db.Measure.Remove(measure);
            db.SaveChanges();

            return Ok(measure);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MeasureExists(int id)
        {
            return db.Measure.Count(e => e.ID == id) > 0;
        }
    }
}