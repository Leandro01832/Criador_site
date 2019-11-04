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
using System.Web.Helpers;

namespace CriadorSites.Controllers
{
    public class PaginaController : Controller
    {
        private BD db = new BD();

        [HttpPost]
        public JsonResult Alterar(int Id, string Titulo, int pedido_, bool ModalDireita, bool Layout)
        {
            db.Configuration.ProxyCreationEnabled = false;
           
            Pagina pagina = db.Pagina.First(di => di.IdPagina == Id);
            pagina.Titulo = Titulo;
            pagina.pedido_ = pedido_;
            pagina.ModalDireita = ModalDireita;
            pagina.Layout = Layout;

            db.Entry(pagina).State = EntityState.Modified;
            db.SaveChanges();
            return Json("");
        }

        // GET: Pagina
        public ActionResult Index()
        {
            var pagina = db.Pagina.Include(p => p.Pedido);
            return View(pagina.ToList());
        }


        [Authorize]
        public ActionResult Galeria(int id)
        {
            var paginas = db.Pagina.Where(p => p.pedido_ == id).ToList();

            return View(paginas);
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
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create()
        {
            var email = User.Identity.GetUserName();           

            var Cliente = db.Cliente.First(c => c.UserName == email);

            //ViewBag.copia = new SelectList(Cliente.GaleriaPagina, "IdPagina", "Titulo");
            ViewBag.pedido_ = new SelectList(Cliente.Servicos, "IdPedido", "Nome");
            return View();
        }

        // POST: Pagina/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "IdPagina,Titulo,Codigo,pedido_,Facebook,Twiter,Instagram")] Pagina pagina)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                    if(pagina.Facebook == null)
                    {
                    pagina.Facebook = "vazio";
                    }
                if (pagina.Twiter == null)
                {
                    pagina.Twiter = "vazio";
                }
                if (pagina.Instagram == null)
                {
                    pagina.Instagram = "vazio";
                }
                pagina.CodigoCss = db.Pagina.Find(1).CodigoCss;
                pagina.CodigoHtml = db.Pagina.Find(1).CodigoHtml;
                pagina.Layout = false;
                db.Pagina.Add(pagina);
                db.SaveChanges();

                Pagina paginaLayout = db.Pagina.Include(pa => pa.Background).FirstOrDefault(p => pagina.Layout == true);
                pagina = db.Pagina.Include(p => p.Pedido).Include(p => p.Pedido.Paginas).First(p => p.IdPagina == pagina.IdPagina);

                List<Background> b = pagina.CriarBackgrounds(pagina, paginaLayout, db.Imagem.ToList());
                db.Background.AddRange(b);
                db.SaveChanges();

                pagina.Background = new List<Background>();
                pagina.Background.AddRange(b);
                db.SaveChanges();

                return RedirectToAction("Galeria", new { id = pagina.pedido_ });               
            }

           
            ViewBag.pedido_ = new SelectList(cli.Servicos, "IdPedido", "Nome", pagina.pedido_);
            return View(pagina);
        }

        // GET: Pagina/Edit/5
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = db.Pagina.Find(id);
            if (pagina.Pedido.Cliente != cli)
            {
                return RedirectToAction("IndexCliente", "CLiente");
            }

            if (pagina == null)
            {
                return HttpNotFound();
            }
            
           
            ViewBag.pedido_ = new SelectList(cli.Servicos, "IdPedido", "Nome", pagina.pedido_);
            return View(pagina);
        }

        // POST: Pagina/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdPagina,Titulo,Codigo,pedido_,Facebook,Twiter,Instagram")] Pagina pagina)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (ModelState.IsValid)
            {
                if (pagina.Facebook == null)
                {
                    pagina.Facebook = "vazio";
                }
                if (pagina.Twiter == null)
                {
                    pagina.Twiter = "vazio";
                }
                if (pagina.Instagram == null)
                {
                    pagina.Instagram = "vazio";
                }
                db.Entry(pagina).State = EntityState.Modified;
                db.SaveChanges();            

                return RedirectToAction("Galeria", new { id = pagina.pedido_ });
            }
            
            ViewBag.pedido_ = new SelectList(cli.Servicos, "IdPedido", "Nome", pagina.pedido_);
            return View(pagina);
        }

        [ValidateInput(false)]
        [Authorize]
        public ActionResult EditModal(int? id)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = db.Pagina.Find(id);
            if (pagina.Pedido.Cliente != cli)
            {
                return RedirectToAction("IndexCliente", "CLiente");
            }

            if (pagina == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.pedido_ = new SelectList(cli.Servicos, "IdPedido", "Nome", pagina.pedido_);
            return PartialView(pagina);
        }


        [Authorize]
        public ActionResult Renderizar_Dinamico(int? id)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = db.Pagina.Find(id);
            if (pagina.Pedido.Cliente != cli)
            {
                return RedirectToAction("IndexCliente", "CLiente");
            }

            if (pagina == null)
            {
                return HttpNotFound();
            }

            renderizacao(id);

             return View(pagina);         
        }

        [Authorize]
        public ActionResult GetView(int? id)
        {
            var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = db.Pagina.Find(id);
            if (pagina.Pedido.Cliente != cli)
            {
                return RedirectToAction("IndexCliente", "CLiente");
            }

            if (pagina == null)
            {
                return HttpNotFound();
            }

            renderizacao(id);

            return PartialView("GetView");            
        }

        public void renderizacao(int? id)
        {
            Pagina pagina = db.Pagina.Find(id);

            foreach (var img in pagina.Imagem.ToList())
            {
                if (img.Recortar && !img.Arquivo.Contains(".gif"))
                {
                    new WebImage(@"" + img.Arquivo)
                   .Crop(img.RecortarTop, img.RecortarLeft, img.RecortarBottom, img.RecortarRight).Save(@"" + img.Arquivo);
                }
                else if (!img.Arquivo.Contains(".gif"))
                {
                    new WebImage(@"" + img.Arquivo)
                   .Crop(0, 0, 0, 0).Save(@"" + img.Arquivo);
                }

                if (img.Redimencionar && !img.Arquivo.Contains(".gif"))
                {
                    new WebImage(@"" + img.Arquivo)
                    .Resize(img.RedimencionarLargura, img.RedimencionarAltura).Save(@"" + img.Arquivo);
                }
                else if (!img.Arquivo.Contains(".gif"))
                {
                    new WebImage(@"" + img.Arquivo)
                    .Resize(new WebImage(@"" + img.Arquivo).Width, new WebImage(@"" + img.Arquivo).Height).Save(@"" + img.Arquivo);
                }

                if (img.FlipHorizontal && !img.Arquivo.Contains(".gif"))
                {
                    new WebImage(@"" + img.Arquivo)
                   .FlipHorizontal().Save(@"" + img.Arquivo);
                }


                if (img.FlipVertical && !img.Arquivo.Contains(".gif"))
                {
                    new WebImage(@"" + img.Arquivo)
                    .FlipVertical().Save(@"" + img.Arquivo);
                }


                if (img.RotacaoEsquerda && !img.Arquivo.Contains(".gif"))
                {
                    new WebImage(@"" + img.Arquivo)
                    .RotateLeft().Save(@"" + img.Arquivo);
                }

                if (img.RotacaoDireita && !img.Arquivo.Contains(".gif"))
                {
                    new WebImage(@"" + img.Arquivo)
                    .RotateRight().Save(@"" + img.Arquivo);
                }

                if (img.Texto && !img.Arquivo.Contains(".gif"))
                {
                    new WebImage(@"" + img.Arquivo)
                    .AddTextWatermark(img.TextoImagem, "white", 12, "Regular").Save(@"" + img.Arquivo);
                }
            }

            pagina.CodigoCss = db.Pagina.Find(1).CodigoCss;
            pagina.CodigoHtml = db.Pagina.Find(1).CodigoHtml;

            int[] numero = pagina.CriarRows(pagina);            

            foreach (var fundo in pagina.Background)
            {
                if (fundo.backgroundTransparente)
                {
                    fundo.Cor = "transparent";
                }
            }

            Velocity.Init();

            var Modelo = new
            {
                Pagina = pagina,
                titulo = pagina.Titulo,
                facebook = pagina.Facebook,
                twiter = pagina.Twiter,
                instagram = pagina.Instagram,
                background = pagina.Background.ToList()[0],
                background_topo = pagina.Background.ToList()[1],
                background_menu = pagina.Background.ToList()[2],
                background_borda_esquerda = pagina.Background.ToList()[3],
                background_borda_direita = pagina.Background.ToList()[4],
                background_bloco = pagina.Background.ToList()[5],
                divs = pagina.Div,
                Rows = numero,
                espacamento = 0,
                indice = 1

            };

            var velocitycontext = new VelocityContext();
            velocitycontext.Put("model", Modelo);
            velocitycontext.Put("divs", pagina.Div);
            var html = new StringBuilder();
            bool result = Velocity.Evaluate(velocitycontext, new StringWriter(html), "NomeParaCapturarLogError", new StringReader(pagina.CodigoCss + pagina.CodigoHtml));

            ViewBag.html = html.ToString();
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
