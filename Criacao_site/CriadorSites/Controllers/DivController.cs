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
using NVelocity;
using NVelocity.App;
using System.Text;
using System.IO;
using Microsoft.AspNet.Identity;

namespace CriadorSites.Controllers
{
    public class DivController : Controller
    {
        private BD db = new BD();

        public JsonResult GetPaginas(int PedidoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var paginas = db.Pagina.Where(m => m.pedido_ == PedidoId);

            return Json(paginas);
        }

        public JsonResult GetBackgrounds(int PaginaId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var backgrounds = db.Background.Where(m => m.pagina_2 == PaginaId);
            
            return Json(backgrounds);
        }

        [HttpPost]
        public JsonResult Alterar(int Id, string Nome, string Divisao, int Height, int background_, int? imagem_, int BorderRadius, int Padding)
        {
            db.Configuration.ProxyCreationEnabled = false;

            Div div = db.Div.Include(d => d.Pagina).First(di => di.IdDiv == Id);
            div.Nome = Nome;
            div.Divisao = Divisao;
            div.Height = Height;
            div.imagem_ = imagem_;
            div.background_ = background_;
            div.Desenhado = 0;
            div.BorderRadius = BorderRadius;
            div.Padding = Padding;
            


            db.Entry(div).State = EntityState.Modified;
            db.SaveChanges();
            

            return Json("");
        }

        // GET: Div
        public ActionResult Index()
        {
            var div = db.Div.Include(d => d.Background).Include(d => d.imagem).Include(d => d.Pagina);
            return View(div.ToList());
        }

        [Authorize]
        public ActionResult Galeria(int id)
        {
            var divs = db.Div.Where(p => p.pagina_ == id).ToList();

            return View(divs);
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
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create()
        {
            var email = User.Identity.GetUserName();
            var Cliente = db.Cliente.First(c => c.UserName == email);            

            ViewBag.site = new SelectList(Cliente.Servicos, "IdPedido", "Nome");
            ViewBag.background_ = new SelectList(new List<Background>(), "IdBackground", "Nome");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            return View();
        }

        // POST: Div/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdDiv,Nome,Codigo,background_,pagina_,imagem_")] Div div)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                div.Height = 200;
                div.Divisao = "col-md-12";
                div.Padding = 150;
                div.Desenhado = 0;
                db.Div.Add(div);
                db.SaveChanges();
                return RedirectToAction("Galeria", new { id = div.pagina_ });
            }


            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.background_ = new SelectList(new List<Background>(), "IdBackground", "Nome");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            return View(div);
        }


        [ValidateInput(false)]
        [Authorize]
        public ActionResult CreateModal()
        {
            var email = User.Identity.GetUserName();
            var Cliente = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(Cliente.Servicos, "IdPedido", "Nome");
            ViewBag.background_ = new SelectList(new List<Background>(), "IdBackground", "Nome");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            return PartialView();
        }

        // POST: Div/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult CreateModal([Bind(Include = "IdDiv,Nome,Codigo,background_,pagina_,imagem_")] Div div)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                div.Height = 200;
                div.Divisao = "col-md-12";
                div.Padding = 150;
                div.Desenhado = 0;
                db.Div.Add(div);
                db.SaveChanges();

                return RedirectToAction("Renderizar_Dinamico", "Pagina", new { id = div.pagina_ });
            }


            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.background_ = new SelectList(new List<Background>(), "IdBackground", "Nome");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            return View(div);
        }


        // GET: Div/Edit/5
        [ValidateInput(false)]
        [Authorize]
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
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", div.Pagina.pedido_);
            ViewBag.background_ = new SelectList(db.Background.Where(b => b.IdBackground == div.background_), "IdBackground", "Nome", div.background_);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == div.imagem_), "IdImagem", "IdImagem", div.imagem_);
            ViewBag.pagina_ = new SelectList(db.Pagina.Where(p => p.IdPagina == div.pagina_), "IdPagina", "Titulo", div.pagina_);
            return View(div);
        }

        // POST: Div/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdDiv,Nome,Codigo,background_,pagina_,imagem_")] Div div)
        {
            if (ModelState.IsValid)
            {
                div.Height = 200;
                div.Divisao = "col-md-12";
                div.Padding = 150;
                div.Desenhado = 0;
                db.Entry(div).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Galeria", new { id = div.pagina_ });
            }
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", div.Pagina.pedido_);
            ViewBag.background_ = new SelectList(db.Background.Where(b => b.IdBackground == div.background_), "IdBackground", "Nome", div.background_);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == div.imagem_), "IdImagem", "IdImagem", div.imagem_);
            ViewBag.pagina_ = new SelectList(db.Pagina.Where(p => p.IdPagina == div.pagina_), "IdPagina", "Titulo", div.pagina_);
            return View(div);
        }

        // GET: Div/Edit/5
        [ValidateInput(false)]
        [Authorize]
        public ActionResult EditModal(int? id)
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
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", div.Pagina.pedido_);
            ViewBag.background_ = new SelectList(db.Background.Where(b => b.IdBackground == div.background_), "IdBackground", "Nome", div.background_);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == div.imagem_), "IdImagem", "IdImagem", div.imagem_);
            ViewBag.pagina_ = new SelectList(db.Pagina.Where(p => p.IdPagina == div.pagina_), "IdPagina", "Titulo", div.pagina_);
            return PartialView(div);
        }

        public ActionResult Renderizar_Dinamico(int? id)
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

            Velocity.Init();

            var Modelo = new
            {
                background = div.Background
            };

            var velocitycontext = new VelocityContext();
            velocitycontext.Put("model", Modelo);

            var html = new StringBuilder();
            bool result = Velocity.Evaluate(velocitycontext, new StringWriter(html), "NomeParaCapturarLogError", new StringReader(div.Codigo));

            ViewBag.html = html.ToString();


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
