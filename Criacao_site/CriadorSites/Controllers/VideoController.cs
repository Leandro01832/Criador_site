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
using Ecommerce.Classes;

namespace CriadorSites.Controllers
{
    public class VideoController : Controller
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

        // GET: Video
        public ActionResult Index()
        {
            return View(db.Video.ToList());
        }

        [Authorize]
        public ActionResult Galeria(int id)
        {
            var videos = db.Video.Where(p => p.div_ == id).ToList();

            return View(videos);
        }

        // GET: Video/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Video.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // GET: Video/Create
        [Authorize]
        public ActionResult Create()
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_ = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return View();
        }

        // POST: Video/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdVideo,Nome,ArquivoVideo,div_,videoFile")] Video video)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (ModelState.IsValid)
            {          

                db.Video.Add(video);
                db.SaveChanges();                

                if (video.videoFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/VideosGaleria";
                    var file = string.Format("{0}.mp4", video.IdVideo);
                    pic = string.Format("{0}/{1}", folder, file);

                    video.videoFile.SaveAs(Server.MapPath(pic));
                    video.ArquivoVideo = pic;
                }
                video.Processado = false;
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Galeria", new { id = video.div_ });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_ = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return View(video);
        }

        // GET: Video/Create
        [Authorize]
        public ActionResult CreateModal()
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_ = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return PartialView();
        }

        // POST: Video/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateModal([Bind(Include = "IdVideo,Nome,ArquivoVideo,div_,videoFile")] Video video)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (ModelState.IsValid)
            {

                db.Video.Add(video);
                db.SaveChanges();

                if (video.videoFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/VideosGaleria";
                    var file = string.Format("{0}.mp4", video.IdVideo);
                    pic = string.Format("{0}/{1}", folder, file);

                    video.videoFile.SaveAs(Server.MapPath(pic));
                    video.ArquivoVideo = pic;
                }

                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();

                video = db.Video.Include(v => v.div).First(vi => vi.IdVideo == video.IdVideo);

                return RedirectToAction("Renderizar_Dinamico", "Pagina", new { id = video.div.pagina_ });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.div_ = new SelectList(new List<Div>(), "IdDiv", "Nome");
            return View(video);
        }

        // GET: Video/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Video.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", video.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == video.div.pagina_), "IdPagina", "Titulo", video.div.pagina_);
            ViewBag.div_ = new SelectList(db.Div.Where(d => d.IdDiv == video.div_), "IdDiv", "Nome", video.div_);
            return View(video);
        }

        // POST: Video/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdVideo,Nome,ArquivoVideo,div_,videoFile")] Video video)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                if (video.videoFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/VideosGaleria";
                    var file = string.Format("{0}.jpg", video.IdVideo);
                    pic = string.Format("{0}/{1}", folder, file);

                    video.videoFile.SaveAs(Server.MapPath(pic));
                    video.ArquivoVideo = pic;
                }

                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Galeria", new { id = video.div_ });
            }
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", video.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == video.div.pagina_), "IdPagina", "Titulo", video.div.pagina_);
            ViewBag.div_ = new SelectList(db.Div.Where(d => d.IdDiv == video.div_), "IdDiv", "Nome", video.div_);
            return View(video);
        }


        [Authorize]
        public ActionResult EditModal(int? id)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Video.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", video.div.Pagina.pedido_);
            ViewBag.pagina = new SelectList(db.Pagina.Where(p => p.IdPagina == video.div.pagina_), "IdPagina", "Titulo", video.div.pagina_);
            ViewBag.div_ = new SelectList(db.Div.Where(d => d.IdDiv == video.div_), "IdDiv", "Nome", video.div_);
            return PartialView(video);
        }

        // GET: Video/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Video.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Video/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = db.Video.Find(id);
            db.Video.Remove(video);
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
