using CriadorSites.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using projeto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CriadorSites.Controllers
{
    [Authorize(Users ="leandroleanleo@gmail.com")]
    public class UsersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            
            var usermaneger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = usermaneger.Users.ToList();
            var usersview = new List<userview>();
            foreach (var user in users)
            {
                var userview = new userview
                {
                    Email = user.Email,
                    Nome = user.UserName,
                    UserId = user.Id
                };
                usersview.Add(userview);
            }
            return View(usersview);
        }

        public ActionResult Roles(string id)
        {
            var usermaneger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = usermaneger.Users.ToList();
            var user = users.Find(u => u.Id == id);

            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = rolemanager.Roles.ToList();
            var rolesview = new List<roleview>();

            foreach (var item in user.Roles)
            {
                var role = roles.Find(r => r.Id == item.RoleId);
                var roleview = new roleview()
                {
                    RoleId = role.Id,
                    Nome = role.Name
                };

                rolesview.Add(roleview);
            }

            var userview = new userview
            {
                Email = user.Email,
                Nome = user.UserName,
                UserId = user.Id,
                Roles = rolesview
            };
            return View(userview);
        }


        public ActionResult AddRole(string id)
        {
            var usermaneger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = usermaneger.Users.ToList();
            var user = users.Find(u => u.Id == id);

            var userview = new userview
            {
                Email = user.Email,
                Nome = user.UserName,
                UserId = user.Id
            };

            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var list = rolemanager.Roles.ToList();
            list.Add(new IdentityRole { Id = "", Name = "[Selecione uma Permissão!]" });
            list = list.OrderBy(c => c.Name).ToList();
            ViewBag.RoleId = new SelectList(list, "Id", "Name");
            return View(userview);
        }

        [HttpPost]
        public ActionResult AddRole(string id, FormCollection form)
        {
            var RoleId = Request["RoleId"];
            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var usermaneger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = usermaneger.Users.ToList();
            var user = users.Find(u => u.Id == id);
            var roles = rolemanager.Roles.ToList();
            var role = rolemanager.Roles.ToList().Find(r => r.Id == RoleId);
            var rolesview = new List<roleview>();

            var userview = new userview
            {
                Email = user.Email,
                Nome = user.UserName,
                UserId = user.Id,
                Roles = rolesview
            };

            if (string.IsNullOrEmpty(RoleId))
            {
                var list = rolemanager.Roles.ToList();
                list.Add(new IdentityRole { Id = "", Name = "[Selecione uma Permissão!]" });
                list = list.OrderBy(c => c.Name).ToList();
                ViewBag.RoleId = new SelectList(list, "Id", "Name");
                ViewBag.error = "Voce precisa selecionar uma permissão!!!";
                return View(userview);
            }


            if (!usermaneger.IsInRole(user.Id, role.Name))
            {
                usermaneger.AddToRole(user.Id, role.Name);
            }



            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
                var roleview = new roleview()
                {
                    RoleId = role.Id,
                    Nome = role.Name
                };

                rolesview.Add(roleview);
            }

            return View("Roles", userview);
        }

        public ActionResult Delete(string id, string roleId)
        {
            var usermaneger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = usermaneger.Users.ToList();
            var user = users.Find(u => u.Id == id);
            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var role = rolemanager.Roles.ToList().Find(r => r.Id == roleId);

            if (usermaneger.IsInRole(user.Id, role.Name))
            {
                usermaneger.RemoveFromRole(user.Id, role.Name);
            }

            var rolesview = new List<roleview>();
            var roles = rolemanager.Roles.ToList();

            var userview = new userview
            {
                Email = user.Email,
                Nome = user.UserName,
                UserId = user.Id,
                Roles = rolesview
            };

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
                var roleview = new roleview()
                {
                    RoleId = role.Id,
                    Nome = role.Name
                };

                rolesview.Add(roleview);
            }

            return View("Roles", userview);
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