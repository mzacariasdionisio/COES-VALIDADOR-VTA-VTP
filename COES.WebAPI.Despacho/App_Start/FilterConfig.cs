using System.Web;
using System.Web.Mvc;

namespace COES.WebAPI.Despacho
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
