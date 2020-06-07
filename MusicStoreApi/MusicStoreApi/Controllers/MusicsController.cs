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
using MusicStoreApi.Models;

namespace MusicStoreApi.Controllers
{
    public class MusicsController : ApiController
    {
        private MusicInventoryDbEntities db = new MusicInventoryDbEntities();

        // GET: api/Musics
        //IQueryable<Music>
        public IQueryable<Music> GetMusics()
        {

            return db.Musics;
        }
        [HttpGet]
        public IQueryable<Music> GetMusicsByGenre(string id)
        {
            return db.Musics.Where((x) => x.Genre == id);
        }
        [HttpGet]
        [Route("api/musics/getallgenres")]

        public IEnumerable<string> GetAllGenres()
        {
            List<string> genres = new List<string>();
            foreach (var item in db.Musics)
                genres.Add(item.Genre);
            return genres.Distinct();
        }

        // GET: api/Musics/5

        [ResponseType(typeof(Music))]
        [Route("api/musics/getmusic/{id}")]
        public IHttpActionResult GetMusic(int id)
        {
            Music music = db.Musics.Find(id);
            if (music == null)
            {
                return NotFound();
            }

            return Ok(music);
        }

        // PUT: api/Musics/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMusic(int id, Music music)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != music.Id)
            {
                return BadRequest();
            }

            db.Entry(music).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicExists(id))
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

        // POST: api/Musics
        [ResponseType(typeof(Music))]
        public IHttpActionResult PostMusic(Music music)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Musics.Add(music);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MusicExists(music.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = music.Id }, music);
        }

        // DELETE: api/Musics/5
        [ResponseType(typeof(Music))]
        public IHttpActionResult DeleteMusic(int id)
        {
            Music music = db.Musics.Find(id);
            if (music == null)
            {
                return NotFound();
            }

            db.Musics.Remove(music);
            db.SaveChanges();

            return Ok(music);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MusicExists(int id)
        {
            return db.Musics.Count(e => e.Id == id) > 0;
        }
    }
}