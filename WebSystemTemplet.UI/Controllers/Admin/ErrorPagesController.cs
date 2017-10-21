using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSystemTemplet.UI.Controllers.Admin
{
    public class ErrorPagesController : Controller
    {
        // GET: ErrorPages
        public ActionResult _404()
        {
            return View("~/Views/Admin/ErrorPages/_404.cshtml");
        }
        public ActionResult _500()
        {
            return View("~/Views/Admin/ErrorPages/_500.cshtml");
        }
    }
}