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
using NVelocity;
using System.Text;
using System.IO;
using Microsoft.AspNet.Identity;


namespace CriadorSites.Controllers
{
    public class BackgroundController : Controller
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

        [HttpPost]
        public JsonResult AlterarBack(int Id, string Nome, bool backgroundImage, bool backgroundTransparente, string cores, string Background_Repeat, string Background_Position, int imagem_)
        {
            db.Configuration.ProxyCreationEnabled = false;

            Background back = db.Background.First(di => di.IdBackground == Id);
            back.Nome = Nome;
            back.backgroundImage = backgroundImage;
            back.Cor = cores;
            back.Background_Repeat = Background_Repeat;
            back.Background_Position = Background_Position;
            back.imagem_ = imagem_;
            back.backgroundTransparente = backgroundTransparente;

            


            db.Entry(back).State = EntityState.Modified;
            db.SaveChanges();
            //var html = back.Pagina.Codigo;

            return Json("");
        }

        // GET: Background
        public ActionResult Index()
        {
            return View(db.Background.ToList());
        }

        [Authorize]
        public ActionResult Galeria(int id)
        {
            var backgrounds = db.Background.Where(p => p.pagina_2 == id).ToList();

            return View(backgrounds);
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
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create()
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_2 = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            return View();
        }

        // POST: Background/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdBackground,Nome,Codigo,backgroundImage,backgroundTransparente,Background_Repeat,Background_Position,imagem_,pagina_2")] Background background)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {

                background.Cor = Request["cores"];

                if (background.backgroundTransparente)
                {
                    background.Cor = "transparent";
                }
                
                db.Background.Add(background);
                db.SaveChanges();
                return RedirectToAction("Galeria", new { id = background.pagina_2 });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_2 = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.imagem_ = new SelectList(new List<Pagina>(), "IdImagem", "IdImagem");
            return View(background);
        }


        [ValidateInput(false)]
        [Authorize]
        public ActionResult CreateModal()
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_2 = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.imagem_ = new SelectList(new List<Imagem>(), "IdImagem", "IdImagem");
            return PartialView();
        }

        // POST: Background/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult CreateModal([Bind(Include = "IdBackground,Nome,Codigo,backgroundImage,backgroundTransparente,Background_Repeat,Background_Position,imagem_,pagina_2")] Background background)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {

                background.Cor = Request["cores"];

                if (background.backgroundTransparente)
                {
                    background.Cor = "transparent";
                }

                db.Background.Add(background);
                db.SaveChanges();
                return RedirectToAction("Renderizar_Dinamico", "Pagina", new { id = background.pagina_2 });
            }

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_2 = new SelectList(new List<Pagina>(), "IdPagina", "Titulo");
            ViewBag.imagem_ = new SelectList(new List<Pagina>(), "IdImagem", "IdImagem");
            return View(background);
        }


        // GET: Background/Edit/5
        [ValidateInput(false)]
        [Authorize]
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

            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", background.Pagina.pedido_);
            ViewBag.pagina_2 = new SelectList(db.Pagina.Where(p => p.IdPagina == background.pagina_2), "IdPagina", "Titulo", background.pagina_2);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == background.imagem_), "IdImagem", "IdImagem", background.imagem_);
            return View(background);
        }

        // POST: Background/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdBackground,Nome,Codigo,backgroundImage,backgroundTransparente,Background_Repeat,Background_Position,imagem_,pagina_2")] Background background)
        {
            if (ModelState.IsValid)
            {
                background.Cor = Request["cores"];

                if (background.backgroundTransparente)
                {
                    background.Cor = "transparent";
                }                

                db.Entry(background).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Galeria", new { id = background.pagina_2 });
            }
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", background.Pagina.pedido_);
            ViewBag.pagina_2 = new SelectList(db.Pagina.Where(p => p.IdPagina == background.pagina_2), "IdPagina", "Titulo", background.pagina_2);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == background.imagem_), "IdImagem", "IdImagem", background.imagem_);
            return View(background);
        }

        [ValidateInput(false)]
        [Authorize]
        public ActionResult EditModal(int? id)
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

            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome", background.Pagina.pedido_);
            ViewBag.pagina_2 = new SelectList(db.Pagina.Where(p => p.IdPagina == background.pagina_2), "IdPagina", "Titulo", background.pagina_2);
            ViewBag.imagem_ = new SelectList(db.Imagem.Where(i => i.IdImagem == background.imagem_), "IdImagem", "IdImagem", background.imagem_);
            return PartialView(background);
        }

        public ActionResult Renderizar_Dinamico(int? id)
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

            Velocity.Init();

            var Modelo = new
            {
                background_color = background.Cor,
                background_image = background.backgroundImage,
                background_url = background.imagem.Arquivo.Replace("~", "../.."),
                background_repeat = background.Background_Repeat,
                background_position = background.Background_Position,                
            };

            var velocitycontext = new VelocityContext();
            velocitycontext.Put("model", Modelo);

            var html = new StringBuilder();
            bool result = Velocity.Evaluate(velocitycontext, new StringWriter(html), "NomeParaCapturarLogError", new StringReader(background.Codigo));

            ViewBag.html = html.ToString();

          
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
