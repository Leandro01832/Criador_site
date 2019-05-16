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
    public class LetraController : Controller
    {
        private BD db = new BD();

        // GET: Letra
        public ActionResult Index()
        {
            var letra = db.Letra.Include(l => l.Div);
            return View(letra.ToList());
        }

        // GET: Letra/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Letra letra = db.Letra.Find(id);
            if (letra == null)
            {
                return HttpNotFound();
            }
            return View(letra);
        }

        // GET: Letra/Create
        public ActionResult Create()
        {
            ViewBag.div_ = new SelectList(db.Div, "IdDiv", "IdDiv");
            return View();
        }

        // POST: Letra/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdLetra,Tipo,Tamanho,Cor,div_")] Letra letra)
        {
            if (ModelState.IsValid)
            {
                db.Letra.Add(letra);
                db.SaveChanges();
                return RedirectToAction("Create", "Imagem", null);
            }

           // ViewBag.div_ = new SelectList(db.Div, "IdDiv", "IdDiv", letra.div_);
            return View(letra);
        }

        // GET: Letra/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Letra letra = db.Letra.Find(id);
            if (letra == null)
            {
                return HttpNotFound();
            }
          //  ViewBag.div_ = new SelectList(db.Div, "IdDiv", "IdDiv", letra.div_);
            return View(letra);
        }

        // POST: Letra/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLetra,Tipo,Tamanho,Cor,div_")] Letra letra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(letra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          //  ViewBag.div_ = new SelectList(db.Div, "IdDiv", "IdDiv", letra.div_);
            return View(letra);
        }

        // GET: Letra/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Letra letra = db.Letra.Find(id);
            if (letra == null)
            {
                return HttpNotFound();
            }
            return View(letra);
        }

        // POST: Letra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Letra letra = db.Letra.Find(id);
            db.Letra.Remove(letra);
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
