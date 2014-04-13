using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dcavaletto.Models;

namespace dcavaletto.Controllers
{
    public class MoviesController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        //
        // GET: /Movies/

        public ActionResult Index()
        {
            return View(db.Movies.ToList());
        }

        //
        // GET: /Movies/Details/5

        public ViewResult Details(int id)
        {
            Movie moviedb = db.Movies.Find(id);
            return View(moviedb);
        }

        //
        // GET: /Movies/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Movies/Create

        [HttpPost]
        public ActionResult Create(Movie moviedb)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(moviedb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(moviedb);
        }

        //
        // GET: /Movies/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
                return HttpNotFound();

            return View(movie);
        }

        //
        // POST: /Movies/Edit/5

        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        //
        // GET: /Movies/Delete/5

        public ActionResult Delete(int id)
        {
            Movie moviedb = db.Movies.Find(id);
            return View(moviedb);
        }

        //
        // POST: /Movies/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie moviedb = db.Movies.Find(id);
            db.Movies.Remove(moviedb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /Movies/Search/query

        public ActionResult Search(string movieGenre, string searchString)
        {
            var genreLst = new List<string>();

            var genreQry = from d in db.Movies
                           orderby d.Genre
                           select d.Genre;

            ViewBag.movieGenre = (new SelectList(genreLst));


            var movies = from m in db.Movies select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (string.IsNullOrEmpty(movieGenre))
                return View(movies);
            else
            {
                return View(movies.Where(x => x.Genre == movieGenre));
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}