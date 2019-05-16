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
using NVelocity.App;
using System.Text;
using NVelocity;
using System.IO;

namespace CriadorSites.Controllers
{
    public class PaginaController : Controller
    {
        private BD db = new BD();

        // GET: Pagina
        public ActionResult Index()
        {
            var pagina = db.Pagina.Include(p => p.Servico);
            return View(pagina.ToList());
        }

        // GET: Pagina/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = db.Pagina.Find(id);
            if (pagina == null)
            {
                return HttpNotFound();
            }
            return View(pagina);
        }

        // GET: Pagina/Create
        [ValidateInput(false)]
        public ActionResult Create()
        {
            ViewBag.servico_ = new SelectList(db.Servico, "IdServico", "Descricao");
            return View();
        }

        // POST: Pagina/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "IdPagina,Titulo,CodigoHtml,servico_")] Pagina pagina)
        {
            if (ModelState.IsValid)
            {
                db.Pagina.Add(pagina);
                db.SaveChanges();
                return RedirectToAction("Create", "Background" , null);
            }

            ViewBag.servico_ = new SelectList(db.Servico, "IdServico", "Descricao", pagina.servico_);
            return View(pagina);
        }

        // GET: Pagina/Edit/5
        [ValidateInput(false)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = db.Pagina.Find(id);
            if (pagina == null)
            {
                return HttpNotFound();
            }
            ViewBag.servico_ = new SelectList(db.Servico, "IdServico", "Descricao", pagina.servico_);

            Velocity.Init();

            var Modelo = new
            {
                background = "green",
                cor = "Yellow",
                font_family = "Arial",
                Header = "Dados de tabela",
                Itens = new[]
                {
                    new {ID = 1, Nome = "texto1", Negrito = false},
                    new {ID = 2, Nome = "texto2", Negrito = false},
                    new {ID = 3, Nome = "texto3", Negrito = false},
                    new {ID = 4, Nome = "texto4", Negrito = false}
                },

            };

            var velocitycontext = new VelocityContext();
            velocitycontext.Put("model", Modelo);

            var html = new StringBuilder();
            bool result = Velocity.Evaluate(velocitycontext, new StringWriter(html), "NomeParaCapturarLogError", new StringReader(pagina.CodigoHtml));

            ViewBag.html = html.ToString();

            return View(pagina);
        }

        // POST: Pagina/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "IdPagina,Titulo,CodigoHtml,servico_")] Pagina pagina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pagina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.servico_ = new SelectList(db.Servico, "IdServico", "Descricao", pagina.servico_);
            return View(pagina);
        }

        // GET: Pagina/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = db.Pagina.Find(id);
            if (pagina == null)
            {
                return HttpNotFound();
            }
            return View(pagina);
        }

        // POST: Pagina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pagina pagina = db.Pagina.Find(id);
            db.Pagina.Remove(pagina);
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
