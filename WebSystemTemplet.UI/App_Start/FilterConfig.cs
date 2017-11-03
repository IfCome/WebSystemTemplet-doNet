using System.Web;
using System.Web.Mvc;
using WebSystemTemplet.UI.Filters;

namespace WebSystemTemplet.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new ActionLogFilter());
        }
    }
}