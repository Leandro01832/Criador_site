using CriadorSites.Models;
using DataContextCriacaoSite;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CriadorSites
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BD, DataContextCriacaoSite.Migrations.Configuration>());
            ApplicationDbContext db = new ApplicationDbContext();
            criaroles(db);
            criarsuperuser(db);
            AddPermissoesSuperUser(db);
            db.Dispose();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void AddPermissoesSuperUser(ApplicationDbContext db)
        {
            var usermaneger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = usermaneger.FindByName("leandroleanleo@gmail.com");
            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));


            if (!usermaneger.IsInRole(user.Id, "Index"))
            {
                usermaneger.AddToRole(user.Id, "Index");
            }

            if (!usermaneger.IsInRole(user.Id, "Edit"))
            {
                usermaneger.AddToRole(user.Id, "Edit");
            }

            if (!usermaneger.IsInRole(user.Id, "Create"))
            {
                usermaneger.AddToRole(user.Id, "Create");
            }

            if (!usermaneger.IsInRole(user.Id, "Delete"))
            {
                usermaneger.AddToRole(user.Id, "Delete");
            }
        }

        private void criarsuperuser(ApplicationDbContext db)
        {
            var usermaneger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = usermaneger.FindByName("leandroleanleo@gmail.com");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "leandroleanleo@gmail.com",
                    Email = "leandroleanleo@gmail.com"
                };

                usermaneger.Create(user, "Manequim 1991");
            }

        }

        private void criaroles(ApplicationDbContext db)
        {
            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));


            if (!rolemanager.RoleExists("Index"))
            {
                rolemanager.Create(new IdentityRole("Index"));
            }

            if (!rolemanager.RoleExists("Create"))
            {
                rolemanager.Create(new IdentityRole("Create"));
            }

            if (!rolemanager.RoleExists("Edit"))
            {
                rolemanager.Create(new IdentityRole("Edit"));
            }

            if (!rolemanager.RoleExists("Delete"))
            {
                rolemanager.Create(new IdentityRole("Delete"));
            }
        }
    }
}
