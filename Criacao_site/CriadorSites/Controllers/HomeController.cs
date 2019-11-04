using business;
using DataContextCriacaoSite;
using Microsoft.AspNet.Identity;
using NVelocity;
using NVelocity.App;
using PagSeguro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CriadorSites.Controllers
{
    public class HomeController : Controller
    {
        private BD db = new BD();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Layout()
        {
            return View();
        }

        public ActionResult Blocos()
        {
            return View();
        }

        public ActionResult Planos_de_fundo()
        {
            return View();
        }

        public ActionResult WebSiteNow()
        {
            return View();
        }

        public ActionResult Carrossel()
        {
            return View();
        }

        public ActionResult MinhaPagina(int? id)
        {
           // var email = User.Identity.GetUserName();
            CLiente cli = db.Cliente.First(c => c.IdCliente == 1);
            Pagina pagina = db.Pagina.Find(id);

            ViewBag.html = renderizar(id);

            return View(pagina);            
        }

        public string renderizar(int? id)
        {
            Pagina pagina = db.Pagina.Find(id);
            if (pagina == null)
            {
                return "";
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
                background = pagina.Background.ToList()[0],
                background_url = pagina.Background.ToList()[0].imagem.Arquivo.Replace("~", "../.."),
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

            return html.ToString();
        }

        
        [Authorize]
        public ActionResult Compra()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult FazerCompra()
        {
            string email = User.Identity.GetUserName();

            var cliente = db.Cliente.First(c => c.UserName == email);

            Dados dados = new Dados();
            
            dados.Email = email;
            dados.data = DateTime.Now;
            
            var cli = db.Cliente.Find(cliente.IdCliente);
            dados.Nome = cli.FirstName + " " + cli.LastName;
            dados.DDD = db.Cliente.First(c => c.IdCliente == cliente.IdCliente).Telefone.DDD_Celular;
            dados.NumeroTelefone = db.Cliente.First(c => c.IdCliente == cliente.IdCliente).Telefone.Celular;            

            dados.Valor = "80.00";

            dados.MeuEmail = "leandro91luis@gmail.com";
            dados.MeuToken = "ffd81aee-ddac-42c9-b6ff-d1cc90f8eab42ee5fca7432694c690d3819c29f774714bd9-609f-4b91-9704-6df2d8c0e2e4";
            dados.TituloPagamento = "Pagamento";
            dados.Referencia = "Meu Projeto Agora - " + dados.Email;

            dados = sPagSeguro.GerarPagamento(dados);

            db.Dados.Add(dados);
            db.SaveChanges();


            return Redirect(dados.stringConexao);
        }
    }
}