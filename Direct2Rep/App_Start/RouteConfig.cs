using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Direct2Rep
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Test",                                           // Route name
               "d2r/{Company}/{cam}",                            // URL with parameters
               new
               {
                   controller = "Campaign",
                   action = "Index",
                   company = "{Company}",
                   cam = "{cam}"


               }  // Parameter defaults
           );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
