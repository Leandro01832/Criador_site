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
    public class TelefoneController : Controller
    {
        private BD db = new BD();

        // GET: Telefone
        public ActionResult Index()
        {
            var telefone = db.Telefone.Include(t => t.Cliente);
            return View(telefone.ToList());
        }

        // GET: Telefone/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefone telefone = db.Telefone.Find(id);
            if (telefone == null)
            {
                return HttpNotFound();
            }
            return View(telefone);
        }

        // GET: Telefone/Create
        public ActionResult Create()
        {
            ViewBag.IdTelefone = new SelectList(db.Cliente, "IdCliente", "FirstName");
            return View();
        }

        // POST: Telefone/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTelefone,DDD_Celular,Celular,DDD_Telefone,Fone")] Telefone telefone)
        {
            if (ModelState.IsValid)
            {
                db.Telefone.Add(telefone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTelefone = new SelectList(db.Cliente, "IdCliente", "FirstName", telefone.IdTelefone);
            return View(telefone);
        }

        // GET: Telefone/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefone telefone = db.Telefone.Find(id);
            if (telefone == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTelefone = new SelectList(db.Cliente, "IdCliente", "FirstName", telefone.IdTelefone);
            return View(telefone);
        }

        // POST: Telefone/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTelefone,DDD_Celular,Celular,DDD_Telefone,Fone")] Telefone telefone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(telefone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTelefone = new SelectList(db.Cliente, "IdCliente", "FirstName", telefone.IdTelefone);
            return View(telefone);
        }

        // GET: Telefone/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefone telefone = db.Telefone.Find(id);
            if (telefone == null)
            {
                return HttpNotFound();
            }
            return View(telefone);
        }

        // POST: Telefone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Telefone telefone = db.Telefone.Find(id);
            db.Telefone.Remove(telefone);
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
