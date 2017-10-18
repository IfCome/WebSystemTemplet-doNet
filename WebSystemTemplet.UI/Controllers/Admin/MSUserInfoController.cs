using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSystemTemplet.UI.Controllers.Admin
{
    public class MSUserInfoController : PCBaseController
    {
        // GET: MSUserInfo
        public ActionResult TeacherList()
        {
            return View("~/Views/Admin/MSUserInfo/TeacherList.cshtml");
        }
    }
}