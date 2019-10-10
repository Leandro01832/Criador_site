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
using Microsoft.AspNet.Identity;

namespace CriadorSites.Controllers
{
    public class ServicoController : Controller
    {
        private BD db = new BD();

        // GET: Servico
        public ActionResult Index()
        {
            var servico = db.Servico.Include(s => s.Pedido);
            return View(servico.ToList());
        }

        // GET: Servico/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servico servico = db.Servico.Find(id);
            if (servico == null)
            {
                return HttpNotFound();
            }
            return View(servico);
        }

        // GET: Servico/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.IdServico = new SelectList(db.Pedido, "IdPedido", "Status");
            return View();
        }

        // POST: Servico/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdServico,Descricao,Nome")] Servico servico)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                db.Servico.Add(servico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdServico = new SelectList(db.Pedido, "IdPedido", "Status", servico.IdServico);
            return View(servico);
        }

        // GET: Servico/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servico servico = db.Servico.Find(id);
            if (servico == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdServico = new SelectList(db.Pedido, "IdPedido", "Status", servico.IdServico);
            return View(servico);
        }

        // POST: Servico/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdServico,Descricao,Nome")] Servico servico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdServico = new SelectList(db.Pedido, "IdPedido", "Status", servico.IdServico);
            return View(servico);
        }

        // GET: Servico/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servico servico = db.Servico.Find(id);
            if (servico == null)
            {
                return HttpNotFound();
            }
            return View(servico);
        }

        // POST: Servico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Servico servico = db.Servico.Find(id);
            db.Servico.Remove(servico);
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
