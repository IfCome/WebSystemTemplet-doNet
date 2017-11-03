using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSystemTemplet.BLL;
using WebSystemTemplet.Model;
using WebSystemTemplet.UI.Filters;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.UI.Controllers.Admin
{
    [ActionLogFilter(NeedLog = false)]
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            // 如果登录成功，则直接跳转到Home页

            // 获取用户的 cookie 对象
            HttpCookie userIdCookie = Request.Cookies["ID"];
            HttpCookie pairACookie = Request.Cookies["PairA"];
            HttpCookie pairBCookie = Request.Cookies["PairB"];

            if (userIdCookie != null && pairACookie != null && pairBCookie != null)
            {
                // 解密获取用户ID
                string userId = Security.Decrypt(userIdCookie.Value);
                string pairA = pairACookie.Value;
                string pairB = pairBCookie.Value;

                // 通过pairA、pairB是否匹配判断用户是否登录
                if (!string.IsNullOrEmpty(userId) && Security.Encrypt(userId + pairA).Equals(pairB))
                {
                    // 登录成功直接跳转到home页
                    return Redirect(Url.Action("IndexPage", "Home"));
                }
            }
            return View("~/Views/Admin/Login/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Pwd)
        {
            string result = "用户名或密码错误";
            Model.Admin.MSUserInfo userInfo = BLL.Admin.MSUserInfoBll.Login(UserName, Pwd, Request.UserHostAddress);
            if (userInfo != null)
            {
                // 写Cookies
                Security.SetUserLoginCookies(userInfo.UserID.ToString(), this.Response);
                result = "ok";
            }
            return Content(result); ;
        }

        public ActionResult Logout()
        {
            BLL.Admin.MSSystemOperateLogBll.AddLoginLog("退出系统", Request.UserHostAddress);

            // 清除identity
            Identity.LoginUserInfo = null;

            // 设置新的Cookies
            HttpCookie userIdCookie = new HttpCookie("ID");
            userIdCookie.Expires = DateTime.Now.AddDays(-1);

            HttpCookie pairACookie = new HttpCookie("PairA");
            pairACookie.Expires = DateTime.Now.AddDays(-1);

            HttpCookie pairBCookie = new HttpCookie("PairB");
            pairBCookie.Expires = DateTime.Now.AddDays(-1);

            Response.Cookies.Add(userIdCookie);
            Response.Cookies.Add(pairACookie);
            Response.Cookies.Add(pairBCookie);



            return RedirectToAction("Index");
        }

    }
}
