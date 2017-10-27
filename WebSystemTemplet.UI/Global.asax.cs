using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebSystemTemplet.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("Log4Net.config")));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            ILog log = LogManager.GetLogger("log4net");
            log.Error(ex); //记录日志信息 

            var httpStatusCode = (ex is HttpException) ? (ex as HttpException).GetHttpCode() : 500; //这里仅仅区分两种错误  
            var httpContext = ((MvcApplication)sender).Context;
            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = httpStatusCode;

            var errorPage = "_404.html";
            switch (httpStatusCode)
            {
                case 404:
                    errorPage = "_404.html"; break;

                default:
                    errorPage = "_500.html"; break;
            }

            // 判断是site还是admin
            bool isAdmin = Request.Url.LocalPath.ToLower().StartsWith("/admin");

            if (isAdmin)
            {
                Response.Redirect("~/Admin/ErrorPages/" + errorPage);
            }
            else
            {
                // 返回首页
                Response.Redirect("~/");
            }
        }
    }
}

