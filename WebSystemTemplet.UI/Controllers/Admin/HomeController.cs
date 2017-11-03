using System.Collections.Generic;
using System.Web.Mvc;
using WebSystemTemplet.UI.Filters;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.UI.Controllers.Admin
{
    public class HomeController : PCBaseController
    {
        [ActionLogFilter(InitLogMsg = "访问网站首页")]
        public ActionResult IndexPage()
        {
            List<Model.Admin.MSSystemOperateLog> logList = BLL.Admin.MSSystemOperateLogBll.GetSingleUserTop10Logs(Model.Identity.LoginUserInfo.UserID);
            ViewBag.LogList = logList;
            ViewBag.LastIpArea = IpAddressHelper.GetAreaByIp(Model.Identity.LoginUserInfo.LastIpAddress);

            return View("~/Views/Admin/Home/IndexPage.cshtml");
        }

    }
}
