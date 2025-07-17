using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;
using AjaxControlToolkit;
//using WSIC2010.WScoes;
using WScoes;

namespace WSIC2010
{
    /// <summary>
    /// Summary description for WebServiceEquipos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[System.Web.Script.Services.ScriptService]
    public class WebServiceEquipos : System.Web.Services.WebService
    {
        
        [WebMethod]
        public CascadingDropDownNameValue[] GetEmpresas(string knownCategoryValues, string category, string contextKey)
        {
            //ManttoServiceClient service = new ManttoServiceClient();
            //string ls_empresas="0";
            //if (Session["in_app"] != null)
            //{
            //    n_app in_app;
            //    in_app = (n_app)Session["in_app"];
            //    ls_empresas = in_app.is_Empresas;
            //}
            
            ManttoService service = new ManttoService();
            return service.GetEmpresas(knownCategoryValues, category, contextKey);
        }
        [WebMethod]
        public CascadingDropDownNameValue[] GetAreasPorEmpresa(string knownCategoryValues, string category)
        {
            //ManttoServiceClient service = new ManttoServiceClient();
            ManttoService service = new ManttoService();
            return service.GetAreasPorEmpresa(knownCategoryValues, category);
        }
        [WebMethod]
        public CascadingDropDownNameValue[] GetEquiposPorArea(string knownCategoryValues, string category)
        {
            //ManttoServiceClient service = new ManttoServiceClient();
            ManttoService service = new ManttoService();
            return service.GetEquiposPorArea(knownCategoryValues, category);
        }
    }
}
