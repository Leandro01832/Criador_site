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
    public class CLienteController : Controller
    {
        private BD db = new BD();

        // GET: CLiente
        public ActionResult Index()
        {
            var cliente = db.Cliente.Include(c => c.Telefone);
            return View(cliente.ToList());
        }

        [Authorize]
        public ActionResult IndexCliente()
        {
            CLiente cliente = null;
            string email = User.Identity.GetUserName();
            try
            {
                cliente = db.Cliente.FirstOrDefault(c => c.UserName == email);
            }
            catch (Exception)
            {
                return RedirectToAction("Create", "Cliente");
            }
            CLiente cli = cliente;
            return View(cli);
        }

        // GET: CLiente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLiente cLiente = db.Cliente.Find(id);
            if (cLiente == null)
            {
                return HttpNotFound();
            }
            return View(cLiente);
        }

        // GET: CLiente/Create
        
        public ActionResult Create()
        {
            return View();
        }

        // POST: CLiente/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,FirstName,LastName,UserName,Cpf,Endereco,Telefone,Password,ConfirmPassword")] CLiente cLiente)
        {
            if (ModelState.IsValid)
            {
                db.Cliente.Add(cLiente);
                db.SaveChanges();
                UserHelper.UsersHelper.CreateUserASP(cLiente.UserName, "User", cLiente.Password);

                List<Imagem> imagens = db.Imagem.Where(img => img.IdImagem < 4).ToList();
                db.Imagem.AddRange(imagens);
                db.SaveChanges();                                

                return RedirectToAction("IndexCliente");
            }
            
            return View(cLiente);
        }

        // GET: CLiente/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLiente cLiente = db.Cliente.Find(id);
            if (cLiente == null)
            {
                return HttpNotFound();
            }

            var email = User.Identity.GetUserName();
            ViewBag.email = email;
            ViewBag.IdCliente = new SelectList(db.Telefone, "IdTelefone", "DDD_Celular");
            return View(cLiente);
        }

        // POST: CLiente/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdCliente,FirstName,LastName,UserName,Cpf,Endereco,Telefone")] CLiente cLiente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLiente.Endereco).State = EntityState.Modified;
                db.Entry(cLiente.Telefone).State = EntityState.Modified;
                db.Entry(cLiente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCliente = new SelectList(db.Telefone, "IdTelefone", "DDD_Celular", cLiente.IdCliente);
            return View(cLiente);
        }

        // GET: CLiente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLiente cLiente = db.Cliente.Find(id);
            if (cLiente == null)
            {
                return HttpNotFound();
            }
            return View(cLiente);
        }

        // POST: CLiente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CLiente cLiente = db.Cliente.Find(id);
            db.Cliente.Remove(cLiente);
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
