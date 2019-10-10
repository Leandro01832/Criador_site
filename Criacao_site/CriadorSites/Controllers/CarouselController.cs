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
    public class CarouselController : Controller
    {
        private BD db = new BD();

        public JsonResult GetImagens(int PaginaId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var imagens = db.Imagem.Where(b => b.pagina_ == PaginaId);

            return Json(imagens);
        }

        public JsonResult GetPaginas(int PedidoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var paginas = db.Pagina.Where(m => m.pedido_ == PedidoId);

            return Json(paginas);
        }

        public JsonResult GetDivs(int PaginaId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var backgrounds = db.Div.Where(m => m.pagina_ == PaginaId);

            return Json(backgrounds);
        }

        [HttpPost]
        public JsonResult AlterarCarousel(int Id, string Nome, int div_2)
        {
            db.Configuration.ProxyCreationEnabled = false;

            Carousel carousel = db.Carousel.First(di => di.IdCarousel == Id);
            carousel.Nome = Nome;
            carousel.div_2 = div_2;
            
            db.Entry(carousel).State = EntityState.Modified;
            db.SaveChanges();

            return Json("");
        }

        // GET: Carousel
        public ActionResult Index()
        {
            var carousel = db.Carousel.Include(c => c.Div);
            return View(carousel.ToList());
        }

        [Authorize]
        public ActionResult Galeria(int id)
        {
            var textos = db.Carousel.Where(p => p.div_2 == id).ToList();

            return View(textos);
        }

        // GET: Carousel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = db.Carousel.Find(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            return View(carousel);
        }

        // GET: Carousel/Create
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create()
        {
            var email = User.Identity.GetUserName();           
            var Cliente = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(Cliente.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return View();
        }

        // POST: Carousel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "IdCarousel,div_2,Codigo,Nome")] Carousel carousel)
        {
            
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {

               // carousel.Codigo = "";

                db.Carousel.Add(carousel);
                db.SaveChanges();

                ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
                ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");

                return RedirectToAction("Galeria", new { id = carousel.div_2 });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", carousel.Div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == carousel.Div.pagina_), "IdPagina", "Titulo", carousel.Div.pagina_);
            ViewBag.div_2 = new SelectList(db.Div.Where(d => d.IdDiv == carousel.div_2), "IdDiv", "Nome", carousel.div_2);
            return View(carousel);
        }


        // GET: Carousel/Create
        [Authorize]
        [ValidateInput(false)]
        public ActionResult CreateModal()
        {
            var email = User.Identity.GetUserName();
            var Cliente = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(Cliente.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return PartialView();
        }

        // POST: Carousel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult CreateModal([Bind(Include = "IdCarousel,div_2,Codigo,Nome")] Carousel carousel)
        {

            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                db.Carousel.Add(carousel);
                db.SaveChanges();

                ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
                ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");

                carousel = db.Carousel.Include(ca => ca.Div).First(c => c.IdCarousel == carousel.IdCarousel);

                return RedirectToAction("Renderizar_Dinamico", "Pagina", new { id = carousel.Div.pagina_ });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", carousel.Div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == carousel.Div.pagina_), "IdPagina", "Titulo", carousel.Div.pagina_);
            ViewBag.div_2 = new SelectList(db.Div.Where(d => d.IdDiv == carousel.div_2), "IdDiv", "Nome", carousel.div_2);
            return View(carousel);
        }



        // GET: Carousel/Edit/5
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = db.Carousel.Find(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", carousel.Div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == carousel.Div.pagina_), "IdPagina", "Titulo", carousel.Div.pagina_);
            ViewBag.div_2 = new SelectList(db.Div.Where(d => d.IdDiv == carousel.div_2), "IdDiv", "Nome", carousel.div_2);
            return View(carousel);
        }

        // POST: Carousel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdCarousel,div_2,Codigo,Nome")] Carousel carousel)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (ModelState.IsValid)
            {

               // carousel.Codigo = "";

                db.Entry(carousel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Galeria", new { id = carousel.div_2 });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", carousel.Div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == carousel.Div.pagina_), "IdPagina", "Titulo", carousel.Div.pagina_);
            ViewBag.div_2 = new SelectList(db.Div.Where(d => d.IdDiv == carousel.div_2), "IdDiv", "Nome", carousel.div_2);
            return View(carousel);
        }

        // GET: Carousel/Edit/5
        [ValidateInput(false)]
        [Authorize]
        public ActionResult EditModal(int? id)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = db.Carousel.Find(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", carousel.Div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == carousel.Div.pagina_), "IdPagina", "Titulo", carousel.Div.pagina_);
            ViewBag.div_2 = new SelectList(db.Div.Where(d => d.IdDiv == carousel.div_2), "IdDiv", "Nome", carousel.div_2);
            return PartialView(carousel);
        }

        [Authorize]
        public ActionResult AdicionarImagem(int? id)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = db.Carousel.Find(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.Imagem = new SelectList(new List<Imagem>(), "IdImagem", "Arquivo");
            return PartialView(carousel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult AdicionarImagem([Bind(Include = "IdCarousel,div_2,Codigo,Nome,imagens")] Carousel carousel)
        {

            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (ModelState.IsValid)
            {
                Imagem img = db.Imagem.Find(int.Parse(Request["Imagem"].ToString()));
                carousel.imagens = db.Carousel.First(c => c.IdCarousel == carousel.IdCarousel).imagens;
                carousel.imagens.Add(img);
                db.SaveChanges();

                carousel = db.Carousel.Include(c => c.Div).First(ca => ca.IdCarousel == carousel.IdCarousel);
                return RedirectToAction("Renderizar_Dinamico", "Pagina", new { id = carousel.Div.pagina_ });
            }
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.Imagem = new SelectList(new List<Imagem>(), "IdImagem", "Arquivo");
            return View(carousel);
        }


        public ActionResult Renderizar_Dinamico(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = db.Carousel.Find(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }

            Velocity.Init();

            var Modelo = new
            {
                ListaImagens = carousel.imagens
            };

            var velocitycontext = new VelocityContext();
            velocitycontext.Put("model", Modelo);

            var html = new StringBuilder();
            bool result = Velocity.Evaluate(velocitycontext, new StringWriter(html), "NomeParaCapturarLogError", new StringReader(carousel.Codigo));

            ViewBag.html = html.ToString();


            return View(carousel);
        }

        // GET: Carousel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = db.Carousel.Find(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            return View(carousel);
        }

        // POST: Carousel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carousel carousel = db.Carousel.Find(id);
            db.Carousel.Remove(carousel);
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
