using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebSystemTemplet.Model;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.UI.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ActionLogFilter : FilterAttribute, IActionFilter
    {
        public LogType? InitLogType = null;
        public string InitLogMsg = null;
        public bool NeedLog = true;

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (NeedLog)
            {
                HttpRequest request = HttpContext.Current.Request;
                string _logMsg = InitLogMsg ?? request.Path;
                LogType _logType = InitLogType ?? (InitLogMsg == null ? LogType.其它 : (request.RequestType == "GET" ? LogType.访问 : LogType.操作));

                StringBuilder sbDetail = new StringBuilder();
                sbDetail.Append(string.Format("路径：{0}{1}", request.Url.AbsoluteUri, Environment.NewLine));
                string queryParams = request.RequestType == "GET" ? request.QueryString.ToString() : request.Form.ToString();
                sbDetail.Append(string.Format("参数：{0}{1}", HttpUtility.UrlDecode(queryParams.IsNullOrEmpty() ? "无参数" : queryParams), Environment.NewLine));

                BLL.Admin.MSSystemOperateLogBll.AddOperateLog(_logType, _logMsg, sbDetail.ToString(), request.UserHostAddress);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

    }
}