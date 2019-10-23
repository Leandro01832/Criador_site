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
    public class PaginaController : Controller
    {
        private BD db = new BD();

        public JsonResult Alterar(int Id, string Titulo, int pedido_)
        {
            db.Configuration.ProxyCreationEnabled = false;
           
            Pagina pagina = db.Pagina.First(di => di.IdPagina == Id);
            pagina.Titulo = Titulo;
            pagina.pedido_ = pedido_;

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
                pagina.Codigo = db.Pagina.Find(1).Codigo;
                    db.Pagina.Add(pagina);
                    db.SaveChanges();

                Background back1 = new Background
                {
                    backgroundImage = true,
                    backgroundTransparente = false,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pagina.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = db.Imagem.ToList()[0],
                    imagem_ = db.Imagem.ToList()[0].IdImagem,
                    Nome = "plano de fundo da pagina"
                };

                Background back2 = new Background
                {
                    backgroundImage = true,
                    backgroundTransparente = false,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pagina.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = db.Imagem.ToList()[1],
                    imagem_ = db.Imagem.ToList()[1].IdImagem,
                    Nome = "topo"
                };


                Background back3 = new Background
                {
                    backgroundImage = true,
                    backgroundTransparente = false,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pagina.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = db.Imagem.ToList()[2],
                    imagem_ = db.Imagem.ToList()[2].IdImagem,
                    Nome = "menu"
                };

                Background back4 = new Background
                {
                    backgroundImage = false,
                    backgroundTransparente = true,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pagina.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = db.Imagem.ToList()[0],
                    imagem_ = db.Imagem.ToList()[0].IdImagem,
                    Nome = "borda esquerda"
                };

                Background back5 = new Background
                {
                    backgroundImage = false,
                    backgroundTransparente = true,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pagina.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = db.Imagem.ToList()[0],
                    imagem_ = db.Imagem.ToList()[0].IdImagem,
                    Nome = "borda direita"
                };

                Background back6 = new Background
                {
                    backgroundImage = false,
                    backgroundTransparente = true,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pagina.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = db.Imagem.ToList()[0],
                    imagem_ = db.Imagem.ToList()[0].IdImagem,
                    Nome = "blocos"
                };

                List<Background> Background = new List<Background>();
                Background.Add(back1);
                Background.Add(back2);
                Background.Add(back3);
                Background.Add(back4);
                Background.Add(back5);
                Background.Add(back6);

                db.Background.AddRange(Background);
                db.SaveChanges();

                pagina.Background.ToList().AddRange(Background);
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

            int espaco = 0;
            int rows = 0;

            foreach(var bloco in pagina.Div)
            {
                if(bloco.Divisao == "col-md-12" || bloco.Divisao == "col-sm-12")
                espaco += 12;                

                if(bloco.Divisao == "col-md-6" || bloco.Divisao == "col-sm-6")
                espaco += 6;

                if(bloco.Divisao == "col-md-4" || bloco.Divisao == "col-sm-4")
                espaco += 4;

                if(bloco.Divisao == "col-md-3" || bloco.Divisao == "col-sm-3")
                espaco += 3;

                if (bloco.Divisao == "col-md-2" || bloco.Divisao == "col-sm-2")
                espaco += 2;

                
            }

            rows = espaco / 12;

            rows += 1;

            int[] numero = new int[rows];

            for (int i = 0; i < numero.Length; i++)
            {
                numero[i] += i + 1;
                
            }

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
            bool result = Velocity.Evaluate(velocitycontext, new StringWriter(html), "NomeParaCapturarLogError", new StringReader(pagina.Codigo));

             ViewBag.html = html.ToString();

              return View(pagina);  
           // return html.ToString();
        }

        [Authorize]
        public ActionResult visualizacao(int? id)
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

            int espaco = 0;
            int rows = 0;

            foreach (var bloco in pagina.Div)
            {
                if (bloco.Divisao == "col-md-12" || bloco.Divisao == "col-sm-12")
                    espaco += 12;

                if (bloco.Divisao == "col-md-6" || bloco.Divisao == "col-sm-6")
                    espaco += 6;

                if (bloco.Divisao == "col-md-4" || bloco.Divisao == "col-sm-4")
                    espaco += 4;

                if (bloco.Divisao == "col-md-3" || bloco.Divisao == "col-sm-3")
                    espaco += 3;

                if (bloco.Divisao == "col-md-2" || bloco.Divisao == "col-sm-2")
                    espaco += 2;


            }

            rows = espaco / 12;

            rows += 1;

            int[] numero = new int[rows];

            for (int i = 0; i < numero.Length; i++)
            {
                numero[i] += i + 1;

            }

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
            bool result = Velocity.Evaluate(velocitycontext, new StringWriter(html), "NomeParaCapturarLogError", new StringReader(pagina.Codigo));

            ViewBag.html = html.ToString();

            return View(pagina);
            // return html.ToString();
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
