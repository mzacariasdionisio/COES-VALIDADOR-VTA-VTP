using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}