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
using Ecommerce.Classes;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;

namespace CriadorSites.Controllers
{
    public class ImagemController : Controller
    {
        private BD db = new BD();

        public JsonResult GetPaginas(int PedidoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var paginas = db.Pagina.Where(m => m.pedido_ == PedidoId);

            return Json(paginas);
        }

        public JsonResult AlterarImagem(int Id, bool Recortar, int RecortarTop, int RecortarLeft, int RecortarRight, int RecortarBottom, bool Redimencionar, int RedimencionarLargura, int RedimencionarAltura, bool FlipHorizontal, bool FlipVertical, bool RotacaoEsquerda, bool RotacaoDireita, bool Texto, string TextoImagem)
        {
            Imagem img = db.Imagem.First(i => i.IdImagem == Id);

            img.Recortar = Recortar;
            img.RecortarTop = RecortarTop;
            img.RecortarRight = RecortarRight;
            img.RecortarLeft = RecortarLeft;
            img.RecortarBottom = RecortarBottom;
            img.Redimencionar = Redimencionar;
            img.RedimencionarAltura = RedimencionarAltura;
            img.RedimencionarLargura = RedimencionarLargura;
            img.FlipHorizontal = FlipHorizontal;
            img.FlipVertical = FlipVertical;
            img.RotacaoDireita = RotacaoDireita;
            img.RotacaoEsquerda = RotacaoEsquerda;
            img.Texto = Texto;
            img.TextoImagem = TextoImagem;

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

            db.Entry(img).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                var erro = "Preencha o formulario corretamente";
                return Json(erro);
            }           

            return Json("valor");
        }

        
        //public void GetImageWatermark()
        //{
        //    var watermarkPath = HttpContext.Server.MapPath(@"~/Imagens/marcadagua.jpg");
        //    var watermark = new WebImage(watermarkPath);

        //    new WebImage(ImagePath)
        //        .AddImageWatermark(watermark)
        //        .Write();
        //}

        // GET: Imagem
        public ActionResult Index()
        {
            var imagems = db.Imagem;
            return View(imagems.ToList());
        }

        [Authorize]
        public ActionResult Galeria(int id)
        {
            var pagina = db.Pagina.First(p => p.IdPagina == id);

            return View(pagina.Imagem);
        }

        // GET: Imagem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagem imagem = db.Imagem.Find(id);
            if (imagem == null)
            {
                return HttpNotFound();
            }
            return View(imagem);
        }

        // GET: Imagem/Create
        [Authorize]
        public ActionResult Create()
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome"); 
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
            return View();
        }

        // POST: Imagem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdImagem,Arquivo,FiguraFile,pagina_")] Imagem imagem)
        {
            Pagina pagina = db.Pagina.Find(int.Parse(Request["pagina_"].ToString()));
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (ModelState.IsValid)
            {
                if(Request["pagina_"].ToString() == "0")
                {
                    ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                    ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
                    ViewBag.erro = "Escolha uma pagina";
                    return View(imagem);
                }

                List<Imagem> image = new List<Imagem>();
                foreach (var arquivo in imagem.FiguraFile)
                {
                    Imagem img = new Imagem();
                    img.pagina_ = imagem.pagina_;                    
                    img.Arquivo = arquivo.FileName;
                    db.Imagem.Add(img);
                    db.SaveChanges();
                    image.Add(img);
                }

                var i = 0;
                foreach (var arquivo in imagem.FiguraFile)
                {
                    if (arquivo != null)
                    {
                        var pic = string.Empty;
                        var folder = "~/Content/ImagensGaleria";
                        var file = string.Format("{0}-{1}", image[i].IdImagem, arquivo.FileName);

                        var response = FileHelpers.UploadPhoto(arquivo, folder, file);
                        if (response)
                        {
                            pic = string.Format("{0}/{1}", folder, file);
                            image[i].Arquivo = pic;
                            
                        }
                    }

                    db.Entry(image[i]).State = EntityState.Modified;
                    db.SaveChanges();                    
                    pagina.Imagem.Add(image[i]);
                    db.SaveChanges();
                    i++;
                }            

                return RedirectToAction("Galeria", new { id = pagina.IdPagina });
            }
            
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
            return View(imagem);
        }

        [Authorize]
        public ActionResult CreateModal()
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            ViewBag.site3 = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
            return PartialView();
        }

        // POST: Imagem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateModal([Bind(Include = "IdImagem,Arquivo,FiguraFile,pagina_")] Imagem imagem)
        {
            Pagina pagina = db.Pagina.Find(int.Parse(Request["pagina_"].ToString()));
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (ModelState.IsValid)
            {
                if (Request["pagina_"].ToString() == "0")
                {
                    ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                    ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
                    ViewBag.erro = "Escolha uma pagina";
                    return View(imagem);
                }

                List<Imagem> image = new List<Imagem>();
                foreach (var arquivo in imagem.FiguraFile)
                {
                    Imagem img = new Imagem();
                    img.pagina_ = imagem.pagina_;
                    img.Arquivo = arquivo.FileName;
                    db.Imagem.Add(img);
                    db.SaveChanges();
                    image.Add(img);
                }

                var i = 0;
                foreach (var arquivo in imagem.FiguraFile)
                {
                    if (arquivo != null)
                    {
                        var pic = string.Empty;
                        var folder = "~/Content/ImagensGaleria";
                        var file = string.Format("{0}-{1}", image[i].IdImagem, arquivo.FileName);

                        var response = FileHelpers.UploadPhoto(arquivo, folder, file);
                        if (response)
                        {
                            pic = string.Format("{0}/{1}", folder, file);
                            image[i].Arquivo = pic;

                        }
                    }

                    db.Entry(image[i]).State = EntityState.Modified;
                    db.SaveChanges();
                    pagina.Imagem.Add(image[i]);
                    db.SaveChanges();
                    i++;
                }

                return RedirectToAction("Renderizar_Dinamico", "Pagina", new { id = pagina.IdPagina });
            }

            ViewBag.site3 = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
            return View(imagem);
        }

        // GET: Imagem/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagem imagem = db.Imagem.Find(id);
            if (imagem.pagina.Pedido.Cliente != cli)
            {
                return RedirectToAction("IndexCliente", "CLiente");
            }

            if (imagem == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
            return View(imagem);
        }

        // POST: Imagem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdImagem,Arquivo,FiguraFile,Pagina_")] Imagem imagem)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (ModelState.IsValid)
            {
                if (Request["pagina_"].ToString() == "0")
                {
                    ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                    ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
                    ViewBag.erro = "Escolha uma pagina";
                    return View(imagem);
                }

                if (imagem.FiguraFile.Count() > 0)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/ImagensGaleria";
                    var file = string.Format("{0}.jpg", imagem.IdImagem);

                    var response = FileHelpers.UploadPhoto(imagem.FiguraFile.First(), folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        imagem.Arquivo = pic;
                    }
                }

                db.Entry(imagem).State = EntityState.Modified;
                db.SaveChanges();

                Pagina pagina = db.Pagina.Find(int.Parse(Request["pagina_"].ToString()));
                
                return RedirectToAction("Galeria", new { id = pagina.IdPagina });
            }

            
            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
            return View(imagem);
        }

        [Authorize]
        public ActionResult EditModal(int? id)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Imagem imagem = db.Imagem.Find(id);
            if (imagem.pagina.Pedido.Cliente != cli)
            {
                return RedirectToAction("IndexCliente", "CLiente");
            }

            if (imagem == null)
            {
                return HttpNotFound();
            }


            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
            return PartialView(imagem);
        }

        // POST: Imagem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditModal([Bind(Include = "IdImagem,Arquivo,FiguraFile,Pagina_")] Imagem imagem)
        {
            string email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.UserName == email);
            if (ModelState.IsValid)
            {
                if (Request["pagina_"].ToString() == "0")
                {
                    ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
                    ViewBag.pagina = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
                    ViewBag.erro = "Escolha uma pagina";
                    return View(imagem);
                }

                if (imagem.FiguraFile.Count() > 0)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/ImagensGaleria";
                    var file = string.Format("{0}.jpg", imagem.IdImagem);

                    var response = FileHelpers.UploadPhoto(imagem.FiguraFile.First(), folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        imagem.Arquivo = pic;
                    }
                }

                db.Entry(imagem).State = EntityState.Modified;
                db.SaveChanges();

                Pagina pagina = db.Pagina.Find(int.Parse(Request["pagina_"].ToString()));

                return RedirectToAction("Renderizar_Dinamico", "Pagina", new { id = pagina.IdPagina });
            }


            ViewBag.site = new SelectList(cli.Servicos, "IdPedido", "Nome");
            ViewBag.pagina_ = new SelectList(new List<Pagina>(), "IdPagina", "Nome");
            return View(imagem);
        }

        // GET: Imagem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagem imagem = db.Imagem.Find(id);
            if (imagem == null)
            {
                return HttpNotFound();
            }
            return View(imagem);
        }

        // POST: Imagem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Imagem imagem = db.Imagem.Find(id);
            db.Imagem.Remove(imagem);
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
