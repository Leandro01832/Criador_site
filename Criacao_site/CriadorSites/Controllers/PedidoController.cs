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
    public class PedidoController : Controller
    {
        private BD db = new BD();

        // GET: Pedido
        public ActionResult Index()
        {
            var pedido = db.Pedido.Include(p => p.Cliente).Include(p => p.Servico);
            return View(pedido.ToList());
        }

        [Authorize]
        public ActionResult Galeria()
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            
            return View(cli.Servicos);
        }

        // GET: Pedido/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedido/Create
        [Authorize]
        public ActionResult Create()
        {
            var email = User.Identity.GetUserName();
            CLiente cliente = null;

            try
            {
                cliente = db.Cliente.First(e => e.UserName == email);
            }
            catch (Exception)
            {
                return RedirectToAction("Create", "Cliente");
            }

            ViewBag.cliente_ = new SelectList(db.Cliente, "IdCliente", "IdCliente", cliente.IdCliente);
            ViewBag.IdPedido = new SelectList(db.Servico, "IdServico", "Descricao");
            return View();
        }

        // POST: Pedido/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdPedido,Datapedido,cliente_,Status,Nome,Servico")] Pedido pedido)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (ModelState.IsValid)
            {
                pedido.Datapedido = DateTime.Now;
                pedido.Status = "Nao realizado";

                db.Pedido.Add(pedido);
                db.SaveChanges();

                cli.Servicos.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("IndexCliente", "CLiente");
            }

            
            ViewBag.IdPedido = new SelectList(db.Servico, "IdServico", "Descricao", pedido.IdPedido);
            return View(pedido);
        }

        // GET: Pedido/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.IdPedido = new SelectList(db.Servico, "IdServico", "Descricao", pedido.IdPedido);
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdPedido,Datapedido,cliente_,Status,Nome,Servico")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexCliente", "CLiente");
            }
            
            ViewBag.IdPedido = new SelectList(db.Servico, "IdServico", "Descricao", pedido.IdPedido);
            return View(pedido);
        }

        // GET: Pedido/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            db.Pedido.Remove(pedido);
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
