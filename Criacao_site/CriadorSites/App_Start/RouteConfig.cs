using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CriadorSites
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
              name: "Preview_dinamico_div",
              url: "Div/Renderizar_Dinamico/{id}",
              defaults: new { controller = "Div", action = "Renderizar_Dinamico", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "Preview_dinamico_background",
              url: "Background/Renderizar_Dinamico/{id}",
              defaults: new { controller = "Background", action = "Renderizar_Dinamico", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "Preview_dinamico",
              url: "codigo/Renderizar_Dinamico/{id}",
              defaults: new { controller = "codigo", action = "Renderizar_Dinamico", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "Preview",
               url: "Codigo/Renderizar/{id}",
               defaults: new { controller = "Codigo", action = "Renderizar", id = UrlParameter.Optional }
           );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
