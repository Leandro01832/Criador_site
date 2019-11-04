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
using CriadorSites.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace CriadorSites.Controllers
{
    public class CLienteController : Controller
    {
        private BD db = new BD();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

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
        public async Task<ActionResult> Create([Bind(Include = "IdCliente,FirstName,LastName,UserName,Cpf,Endereco,Telefone,Password,ConfirmPassword")] CLiente cLiente)
        {
            if (ModelState.IsValid)
            {
                db.Cliente.Add(cLiente);
                db.SaveChanges();
                UserHelper.UsersHelper.CreateUserASP(cLiente.UserName, "User", cLiente.Password);

                var user = new ApplicationUser { UserName = cLiente.UserName, Email = cLiente.UserName };
                var result = await UserManager.CreateAsync(user, cLiente.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Por favor confirme sua conta clicando <a href=\"" + callbackUrl + "\">AQUI</a>");

                    return RedirectToAction("Index", "Home");
                }

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
