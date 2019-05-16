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
    public class BackgroundController : Controller
    {
        private BD db = new BD();

        // GET: Background
        public ActionResult Index()
        {
            return View(db.Background.ToList());
        }

        // GET: Background/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Background background = db.Background.Find(id);
            if (background == null)
            {
                return HttpNotFound();
            }
            return View(background);
        }

        // GET: Background/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Background/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdBackground,backgroundImage,Cor,ImagemFile")] Background background)
        {
            if (ModelState.IsValid)
            {

                db.Background.Add(background);
                db.SaveChanges();

                if (background.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagens";
                    var file = string.Format("{0}.jpg", background.IdBackground);

                    var response = FileHelpers.UploadPhoto(background.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        background.Imagem = pic;
                    }
                }

                db.Entry(background).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "Div", null);
            }

            return View(background);
        }

        // GET: Background/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Background background = db.Background.Find(id);
            if (background == null)
            {
                return HttpNotFound();
            }
            return View(background);
        }

        // POST: Background/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdBackground,backgroundImage,Cor,ImagemFile")] Background background)
        {
            if (ModelState.IsValid)
            {
                if (background.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagens";
                    var file = string.Format("{0}.jpg", background.ImagemFile);

                    var response = FileHelpers.UploadPhoto(background.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        background.Imagem = pic;
                    }
                }

                db.Entry(background).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(background);
        }

        // GET: Background/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Background background = db.Background.Find(id);
            if (background == null)
            {
                return HttpNotFound();
            }
            return View(background);
        }

        // POST: Background/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Background background = db.Background.Find(id);
            db.Background.Remove(background);
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
