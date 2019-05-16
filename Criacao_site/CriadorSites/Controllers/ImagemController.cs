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
    public class ImagemController : Controller
    {
        private BD db = new BD();

        // GET: Imagem
        public ActionResult Index()
        {
            var imagems = db.Imagems;
            return View(imagems.ToList());
        }

        // GET: Imagem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagem imagem = db.Imagems.Find(id);
            if (imagem == null)
            {
                return HttpNotFound();
            }
            return View(imagem);
        }

        // GET: Imagem/Create
        public ActionResult Create()
        {
            ViewBag.carousel_ = new SelectList(db.Carousel, "IdCarousel", "IdCarousel");
            return View();
        }

        // POST: Imagem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdImagem,Figura,carousel_,FiguraFile")] Imagem imagem)
        {
            if (ModelState.IsValid)
            {
                db.Imagems.Add(imagem);
                db.SaveChanges();

                if (imagem.FiguraFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/ImagensGaleria";
                    var file = string.Format("{0}.jpg", imagem.IdImagem);

                    var response = FileHelpers.UploadPhoto(imagem.FiguraFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        imagem.Figura = pic;
                    }
                }

                db.Entry(imagem).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Create", "Imagem", null);
            }

            
            return View(imagem);
        }

        // GET: Imagem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagem imagem = db.Imagems.Find(id);
            if (imagem == null)
            {
                return HttpNotFound();
            }
            
            return View(imagem);
        }

        // POST: Imagem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdImagem,Figura,carousel_,FiguraFile")] Imagem imagem)
        {
            if (ModelState.IsValid)
            {

                if (imagem.FiguraFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/ImagensGaleria";
                    var file = string.Format("{0}.jpg", imagem.IdImagem);

                    var response = FileHelpers.UploadPhoto(imagem.FiguraFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        imagem.Figura = pic;
                    }
                }

                db.Entry(imagem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(imagem);
        }

        // GET: Imagem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagem imagem = db.Imagems.Find(id);
            if (imagem == null)
            {
                return HttpNotFound();
            }
            return View(imagem);
        }

        // POST: Imagem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Imagem imagem = db.Imagems.Find(id);
            db.Imagems.Remove(imagem);
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
