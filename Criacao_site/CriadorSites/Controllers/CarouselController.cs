using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataContextCriacaoSite;
using business;

namespace CriadorSites.Controllers
{
    public class CarouselController : Controller
    {
        private BD db = new BD();

        // GET: Carousel
        public ActionResult Index()
        {
            var carousel = db.Carousel.Include(c => c.Div);
            return View(carousel.ToList());
        }

        // GET: Carousel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = db.Carousel.Find(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            return View(carousel);
        }

        // GET: Carousel/Create
        public ActionResult Create()
        {
            ViewBag.div_2 = new SelectList(db.Div, "IdDiv", "Texto");
            return View();
        }

        // POST: Carousel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCarousel,div_2")] Carousel carousel)
        {
            if (ModelState.IsValid)
            {
                db.Carousel.Add(carousel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.div_2 = new SelectList(db.Div, "IdDiv", "Texto", carousel.div_2);
            return View(carousel);
        }

        // GET: Carousel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = db.Carousel.Find(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            ViewBag.div_2 = new SelectList(db.Div, "IdDiv", "Texto", carousel.div_2);
            return View(carousel);
        }

        // POST: Carousel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCarousel,div_2")] Carousel carousel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carousel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.div_2 = new SelectList(db.Div, "IdDiv", "Texto", carousel.div_2);
            return View(carousel);
        }

        // GET: Carousel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = db.Carousel.Find(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            return View(carousel);
        }

        // POST: Carousel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carousel carousel = db.Carousel.Find(id);
            db.Carousel.Remove(carousel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
