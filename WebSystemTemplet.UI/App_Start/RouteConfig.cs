using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebSystemTemplet.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");




            // 后台管理系统路由
            // 先定义.html
            routes.MapRoute(
                name: "admin_html",
                url: "admin/{controller}/{action}.html",
                defaults: new { controller = "Home", action = "IndexPage" },
                namespaces: new[] { "WebSystemTemplet.UI.Controllers.Admin" }
            );
            routes.MapRoute(
                name: "admin",
                url: "admin/{controller}/{action}",
                defaults: new { controller = "Home", action = "IndexPage" },
                namespaces: new[] { "WebSystemTemplet.UI.Controllers.Admin" }
            );

            // 门户网站路由
            routes.MapRoute(
                name: "site_html",
                url: "{controller}/{action}.html",
                defaults: new { controller = "Home", action = "IndexPage" },
                namespaces: new[] { "WebSystemTemplet.UI.Controllers.Site" }
            );
            routes.MapRoute(
                name: "site",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "IndexPage" },
                namespaces: new[] { "WebSystemTemplet.UI.Controllers.Site" }
            );

            routes.MapRoute(
                name: "default",
                url: "{*extrastuff}"
            );

        }
    }
}