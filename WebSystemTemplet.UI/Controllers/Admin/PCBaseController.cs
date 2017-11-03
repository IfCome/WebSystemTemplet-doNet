using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSystemTemplet.Utility;
using WebSystemTemplet.Model;

namespace WebSystemTemplet.UI.Controllers.Admin
{
    public class PCBaseController : Controller
    {
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
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
                    if (Identity.LoginUserInfo != null)
                    {
                        Security.SetUserLoginCookies(userId, this.Response);
                        return;
                    }
                }
            }
            if (Request.IsAjaxRequest())
            {
                Response.StatusCode = 401; //未登录  
                Response.End();
                filterContext.Result = new EmptyResult();
            }
            else
            {
                // 没有登录成功者返回登录页面
                filterContext.Result = Redirect(Url.Action("Index", "Login"));
            }
            return;
        }
    }
}
