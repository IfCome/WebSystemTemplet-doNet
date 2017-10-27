using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSystemTemplet.UI.Models.Admin;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.UI.Controllers.Site
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult IndexPage()
        {
            return View("~/Views/Site/Home/IndexPage.cshtml");
        }

    }
}
