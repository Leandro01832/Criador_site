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
using Ecommerce.Classes;

namespace CriadorSites.Controllers
{
    public class DivController : Controller
    {
        private BD db = new BD();

        // GET: Div
        public ActionResult Index()
        {
            var div = db.Div.Include(d => d.Background).Include(d => d.Carousel).Include(d => d.Letra).Include(d => d.Pagina);
            return View(div.ToList());
        }

        // GET: Div/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Div div = db.Div.Find(id);
            if (div == null)
            {
                return HttpNotFound();
            }
            return View(div);
        }

        // GET: Div/Create
        public ActionResult Create()
        {
            ViewBag.background_ = new SelectList(db.Background, "IdBackground", "Cor");
            ViewBag.IdDiv = new SelectList(db.Carousel, "IdCarousel", "IdCarousel");
            ViewBag.IdDiv = new SelectList(db.Letra, "IdLetra", "Tipo");
            ViewBag.pagina_ = new SelectList(db.Pagina, "IdPagina", "Titulo");
            return View();
        }

        // POST: Div/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDiv,Texto,background_,AdicionarImagem,ImagemDiv,pagina_,ImagemDivFile")] Div div)
        {
            if (ModelState.IsValid)
            {
                db.Div.Add(div);
                db.SaveChanges();

                if (div.ImagemDivFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/ImagensDiv";
                    var file = string.Format("{0}.jpg", div.IdDiv);

                    var response = FileHelpers.UploadPhoto(div.ImagemDivFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        div.ImagemDiv = pic;
                    }
                }

                db.Entry(div).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Create", "Letra", null);
            }

            ViewBag.background_ = new SelectList(db.Background, "IdBackground", "Cor", div.background_);
            ViewBag.IdDiv = new SelectList(db.Carousel, "IdCarousel", "IdCarousel", div.IdDiv);
            ViewBag.IdDiv = new SelectList(db.Letra, "IdLetra", "Tipo", div.IdDiv);
            ViewBag.pagina_ = new SelectList(db.Pagina, "IdPagina", "Titulo", div.pagina_);
            return View(div);
        }

        // GET: Div/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Div div = db.Div.Find(id);
            if (div == null)
            {
                return HttpNotFound();
            }
            ViewBag.background_ = new SelectList(db.Background, "IdBackground", "Cor", div.background_);
            ViewBag.IdDiv = new SelectList(db.Carousel, "IdCarousel", "IdCarousel", div.IdDiv);
            ViewBag.IdDiv = new SelectList(db.Letra, "IdLetra", "Tipo", div.IdDiv);
            ViewBag.pagina_ = new SelectList(db.Pagina, "IdPagina", "Titulo", div.pagina_);
            return View(div);
        }

        // POST: Div/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDiv,Texto,background_,AdicionarImagem,ImagemDiv,pagina_,ImagemDivFile")] Div div)
        {
            if (ModelState.IsValid)
            {

                if (div.ImagemDivFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/ImagensDiv";
                    var file = string.Format("{0}.jpg", div.ImagemDivFile);

                    var response = FileHelpers.UploadPhoto(div.ImagemDivFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        div.ImagemDiv = pic;
                    }
                }

                db.Entry(div).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.background_ = new SelectList(db.Background, "IdBackground", "Cor", div.background_);
            ViewBag.IdDiv = new SelectList(db.Carousel, "IdCarousel", "IdCarousel", div.IdDiv);
            ViewBag.IdDiv = new SelectList(db.Letra, "IdLetra", "Tipo", div.IdDiv);
            ViewBag.pagina_ = new SelectList(db.Pagina, "IdPagina", "Titulo", div.pagina_);
            return View(div);
        }

        // GET: Div/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Div div = db.Div.Find(id);
            if (div == null)
            {
                return HttpNotFound();
            }
            return View(div);
        }

        // POST: Div/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Div div = db.Div.Find(id);
            db.Div.Remove(div);
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
