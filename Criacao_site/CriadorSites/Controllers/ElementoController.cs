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
    public class ElementoController : Controller
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
            var blocos = db.Div.Where(m => m.pagina_ == PaginaId);

            return Json(blocos);
        }

        public JsonResult GetElementos(int DivId, string valor, int pagina)
        {
            if(valor == "Video")
            {
                db.Configuration.ProxyCreationEnabled = false;
                var v = db.Video.Where(m => m.div_ == DivId);
                return Json(v);
            }

            if (valor == "Imagem")
            {
                db.Configuration.ProxyCreationEnabled = false;
                var imagens = db.Imagem.Where(b => b.pagina_ == pagina);

                return Json(imagens);
            }

            if (valor == "Carrossel")
            {
                db.Configuration.ProxyCreationEnabled = false;
                var c = db.Carousel.Where(m => m.div_2 == DivId);
                return Json(c);
            }

            if (valor == "Texto")
            {
                db.Configuration.ProxyCreationEnabled = false;
                var t = db.Texto.Where(m => m.div_ == DivId);
                return Json(t);
            }

            return Json("");
        }

        // GET: Elemento
        public ActionResult Index()
        {
            var elemento = db.Elemento.Include(e => e.carousel).Include(e => e.div).Include(e => e.imagem).Include(e => e.texto).Include(e => e.video);
            return View(elemento.ToList());
        }

        // GET: Elemento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elemento elemento = db.Elemento.Find(id);
            if (elemento == null)
            {
                return HttpNotFound();
            }
            return View(elemento);
        }

        // GET: Elemento/Create
        [Authorize]
        public ActionResult Create()
        {
            var email = User.Identity.GetUserName();
            var cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.carousel_ = new SelectList(new List<Carousel>(), "IdCarousel", "Nome");
            ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            ViewBag.texto_ = new SelectList(new List<Texto>(), "IdTexto", "Nome");
            ViewBag.video_ = new SelectList(new List<Video>(), "IdVideo", "Nome");
            return View();
        }

        // POST: Elemento/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdElemento,texto_,carousel_,imagem_,video_,div_2")] Elemento elemento)
        {
            var email = User.Identity.GetUserName();
            var cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {

                if(elemento.imagem_ == null && elemento.video_ == null && elemento.texto_ == null && elemento.carousel_ == null)
                {
                    ViewBag.erro = "Escolha um elemento.";
                    ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                    ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
                    ViewBag.carousel_ = new SelectList(new List<Carousel>(), "IdCarousel", "Nome");
                    ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
                    ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
                    ViewBag.texto_ = new SelectList(new List<Texto>(), "IdTexto", "Nome");
                    ViewBag.video_ = new SelectList(new List<Video>(), "IdVideo", "Nome");
                    return View(elemento);
                }


                db.Elemento.Add(elemento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.carousel_ = new SelectList(new List<Carousel>(), "IdCarousel", "Nome");
            ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            ViewBag.texto_ = new SelectList(new List<Texto>(), "IdTexto", "Nome");
            ViewBag.video_ = new SelectList(new List<Video>(), "IdVideo", "Nome");
            return View(elemento);
        }

        [Authorize]
        public ActionResult CreateModal()
        {
            var email = User.Identity.GetUserName();
            var cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.carousel_ = new SelectList(new List<Carousel>(), "IdCarousel", "Nome");
            ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            ViewBag.texto_ = new SelectList(new List<Texto>(), "IdTexto", "Nome");
            ViewBag.video_ = new SelectList(new List<Video>(), "IdVideo", "Nome");
            return PartialView();
        }

        // POST: Elemento/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateModal([Bind(Include = "IdElemento,texto_,carousel_,imagem_,video_,div_2")] Elemento elemento)
        {
            var email = User.Identity.GetUserName();
            var cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {

                if (elemento.imagem_ == null && elemento.video_ == null && elemento.texto_ == null && elemento.carousel_ == null)
                {
                    ViewBag.erro = "Escolha um elemento.";
                    ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                    ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
                    ViewBag.carousel_ = new SelectList(new List<Carousel>(), "IdCarousel", "Nome");
                    ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
                    ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
                    ViewBag.texto_ = new SelectList(new List<Texto>(), "IdTexto", "Nome");
                    ViewBag.video_ = new SelectList(new List<Video>(), "IdVideo", "Nome");
                    return View(elemento);
                }


                db.Elemento.Add(elemento);
                db.SaveChanges();

                elemento = db.Elemento.Include(e => e.div).First(el => el.IdElemento == elemento.IdElemento);
                return RedirectToAction("Renderizar_Dinamico", "Pagina", new { id = elemento.div.pagina_ });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.carousel_ = new SelectList(new List<Carousel>(), "IdCarousel", "Nome");
            ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            ViewBag.texto_ = new SelectList(new List<Texto>(), "IdTexto", "Nome");
            ViewBag.video_ = new SelectList(new List<Video>(), "IdVideo", "Nome");
            return View(elemento);
        }

        // GET: Elemento/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            var email = User.Identity.GetUserName();
            var cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elemento elemento = db.Elemento.Find(id);

            if (elemento.div.Pagina.Pedido.Cliente != cli)
            {
                return RedirectToAction("IndexCliente", "CLiente");
            }

            if (elemento == null)
            {
                return HttpNotFound();
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", elemento.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == elemento.div.pagina_), "IdPagina", "Titulo", elemento.div.pagina_);
            ViewBag.carousel_ = new SelectList(elemento.div.Carousel, "IdCarousel", "Nome");
            ViewBag.div_2 = new SelectList(db.Div, "IdDiv", "Nome", elemento.div_2);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == elemento.imagem_), "IdImagem", "IdImagem", elemento.imagem_);
            ViewBag.texto_ = new SelectList(elemento.div.Textos, "IdTexto", "Nome");
            ViewBag.video_ = new SelectList(elemento.div.Video, "IdVideo", "Nome");
            return View(elemento);
        }

        // POST: Elemento/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdElemento,texto_,carousel_,imagem_,video_,div_2")] Elemento elemento)
        {
            var email = User.Identity.GetUserName();
            var cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                if (elemento.imagem_ == null && elemento.video_ == null && elemento.texto_ == null && elemento.carousel_ == null)
                {
                    ViewBag.erro = "Escolha um elemento.";
                    ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                    ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
                    ViewBag.carousel_ = new SelectList(new List<Carousel>(), "IdCarousel", "Nome");
                    ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
                    ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
                    ViewBag.texto_ = new SelectList(new List<Texto>(), "IdTexto", "Nome");
                    ViewBag.video_ = new SelectList(new List<Video>(), "IdVideo", "Nome");
                    return View(elemento);
                }

                db.Entry(elemento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", elemento.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == elemento.div.pagina_), "IdPagina", "Titulo", elemento.div.pagina_);
            ViewBag.carousel_ = new SelectList(elemento.div.Carousel, "IdCarousel", "Nome");
            ViewBag.div_2 = new SelectList(db.Div, "IdDiv", "Nome", elemento.div_2);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == elemento.imagem_), "IdImagem", "IdImagem", elemento.imagem_);
            ViewBag.texto_ = new SelectList(elemento.div.Textos, "IdTexto", "Nome");
            ViewBag.video_ = new SelectList(elemento.div.Video, "IdVideo", "Nome");
            return View(elemento);
        }


        [Authorize]
        public ActionResult EditModal(int? id)
        {
            var email = User.Identity.GetUserName();
            var cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elemento elemento = db.Elemento.Find(id);

            if (elemento.div.Pagina.Pedido.Cliente != cli)
            {
                return RedirectToAction("IndexCliente", "CLiente");
            }

            if (elemento == null)
            {
                return HttpNotFound();
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", elemento.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == elemento.div.pagina_), "IdPagina", "Titulo", elemento.div.pagina_);
            ViewBag.carousel_ = new SelectList(elemento.div.Carousel, "IdCarousel", "Nome");
            ViewBag.div_2 = new SelectList(db.Div, "IdDiv", "Nome", elemento.div_2);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == elemento.imagem_), "IdImagem", "IdImagem", elemento.imagem_);
            ViewBag.texto_ = new SelectList(elemento.div.Textos, "IdTexto", "Nome");
            ViewBag.video_ = new SelectList(elemento.div.Video, "IdVideo", "Nome");
            return PartialView(elemento);
        }

        // POST: Elemento/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditModal([Bind(Include = "IdElemento,texto_,carousel_,imagem_,video_,div_2")] Elemento elemento)
        {
            var email = User.Identity.GetUserName();
            var cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                if (elemento.imagem_ == null && elemento.video_ == null && elemento.texto_ == null && elemento.carousel_ == null)
                {
                    ViewBag.erro = "Escolha um elemento.";
                    ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                    ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
                    ViewBag.carousel_ = new SelectList(new List<Carousel>(), "IdCarousel", "Nome");
                    ViewBag.div_2 = new SelectList(new List<Div>(), "IdDiv", "Nome");
                    ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
                    ViewBag.texto_ = new SelectList(new List<Texto>(), "IdTexto", "Nome");
                    ViewBag.video_ = new SelectList(new List<Video>(), "IdVideo", "Nome");
                    return PartialView(elemento);
                }

                db.Entry(elemento).State = EntityState.Modified;
                db.SaveChanges();

                elemento = db.Elemento.Include(e => e.div).First(el => el.IdElemento == elemento.IdElemento);
                return RedirectToAction("Renderizar_Dinamico", "Pagina", new { id = elemento.div.pagina_ });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", elemento.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == elemento.div.pagina_), "IdPagina", "Titulo", elemento.div.pagina_);
            ViewBag.carousel_ = new SelectList(elemento.div.Carousel, "IdCarousel", "Nome");
            ViewBag.div_2 = new SelectList(db.Div, "IdDiv", "Nome", elemento.div_2);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == elemento.imagem_), "IdImagem", "IdImagem", elemento.imagem_);
            ViewBag.texto_ = new SelectList(elemento.div.Textos, "IdTexto", "Nome");
            ViewBag.video_ = new SelectList(elemento.div.Video, "IdVideo", "Nome");
            return View(elemento);
        }

        // GET: Elemento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elemento elemento = db.Elemento.Find(id);
            if (elemento == null)
            {
                return HttpNotFound();
            }
            return View(elemento);
        }

        // POST: Elemento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Elemento elemento = db.Elemento.Find(id);
            db.Elemento.Remove(elemento);
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
