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
using NVelocity.App;
using NVelocity;
using System.Text;
using System.IO;

namespace CriadorSites.Controllers
{
    public class TextoController : Controller
    {
        private BD db = new BD();

        public JsonResult GetPaginas(int PedidoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var paginas = db.Pagina.Where(m => m.pedido_ == PedidoId);

            return Json(paginas);
        }

        public JsonResult GetDivs(int PaginaId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var divs = db.Div.Where(m => m.pagina_ == PaginaId);

            return Json(divs);
        }

        // GET: Texto
        public ActionResult Index()
        {
            var texto = db.Texto.Include(t => t.div);
            return View(texto.ToList());
        }

        [Authorize]
        public ActionResult Galeria(int id)
        {
            var textos = db.Texto.Where(p => p.div_ == id).ToList();

            return View(textos);
        }

        // GET: Texto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Texto texto = db.Texto.Find(id);
            if (texto == null)
            {
                return HttpNotFound();
            }
            return View(texto);
        }

        // GET: Texto/Create
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create()
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_ = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return View();
        }

        // POST: Texto/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "IdTexto,Nome,Palavras,div_,Codigo")] Texto texto)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                db.Texto.Add(texto);
                db.SaveChanges();

                
                return RedirectToAction("Galeria", new { id = texto.div_ });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_ = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return View(texto);
        }


        // GET: Texto/Create
        [Authorize]
        [ValidateInput(false)]
        public ActionResult CreateModal()
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_ = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return PartialView();
        }

        // POST: Texto/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult CreateModal([Bind(Include = "IdTexto,Nome,Palavras,div_,Codigo")] Texto texto)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                db.Texto.Add(texto);
                db.SaveChanges();

                texto = db.Texto.Include(t => t.div).First(te => te.IdTexto == texto.IdTexto);

                return RedirectToAction("Renderizar_dinamico", "Pagina", new { id = texto.div.pagina_ });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_ = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return View(texto);
        }


        // GET: Texto/Edit/5
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Texto texto = db.Texto.Find(id);
            if (texto == null)
            {
                return HttpNotFound();
            }

            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", texto.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == texto.div.pagina_), "IdPagina", "Titulo", texto.div.pagina_);
            ViewBag.div_ = new SelectList(db.Div.Where(d => d.IdDiv == texto.div_), "IdDiv", "Nome", texto.div_);
            return View(texto);
        }

        // POST: Texto/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdTexto,Nome,Palavras,div_,Codigo")] Texto texto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(texto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Galeria", new { id = texto.div_ });
            }

            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", texto.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == texto.div.pagina_), "IdPagina", "Titulo", texto.div.pagina_);
            ViewBag.div_ = new SelectList(db.Div.Where(d => d.IdDiv == texto.div_), "IdDiv", "Nome", texto.div_);
            return View(texto);
        }

        // GET: Texto/Edit/5
        [ValidateInput(false)]
        [Authorize]
        public ActionResult EditModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Texto texto = db.Texto.Find(id);
            if (texto == null)
            {
                return HttpNotFound();
            }

            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", texto.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == texto.div.pagina_), "IdPagina", "Titulo", texto.div.pagina_);
            ViewBag.div_ = new SelectList(db.Div.Where(d => d.IdDiv == texto.div_), "IdDiv", "Nome", texto.div_);
            return PartialView(texto);
        }

        // POST: Texto/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditModal([Bind(Include = "IdTexto,Nome,Palavras,div_,Codigo")] Texto texto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(texto).State = EntityState.Modified;
                db.SaveChanges();

                texto = db.Texto.Include(t => t.div).First(te => te.IdTexto == texto.IdTexto);

                return RedirectToAction("Renderizar_dinamico", "Pagina", new { id = texto.div.pagina_ });
            }

            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", texto.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina, "IdPagina", "Titulo", texto.div.pagina_);
            ViewBag.div_ = new SelectList(db.Div, "IdDiv", "Nome", texto.div_);
            return View(texto);
        }

        public ActionResult Renderizar_Dinamico(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Texto texto = db.Texto.Find(id);
            if (texto == null)
            {
                return HttpNotFound();
            }

            Velocity.Init();

            var Modelo = new
            {

            };

            var velocitycontext = new VelocityContext();
            velocitycontext.Put("model", Modelo);

            var html = new StringBuilder();
            bool result = Velocity.Evaluate(velocitycontext, new StringWriter(html), "NomeParaCapturarLogError", new StringReader(texto.Codigo));

            ViewBag.html = html.ToString();


            return View(texto);
        }

        // GET: Texto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Texto texto = db.Texto.Find(id);
            if (texto == null)
            {
                return HttpNotFound();
            }
            return View(texto);
        }

        // POST: Texto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Texto texto = db.Texto.Find(id);
            db.Texto.Remove(texto);
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
