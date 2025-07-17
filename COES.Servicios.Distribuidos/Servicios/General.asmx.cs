using COES.Servicios.Aplicacion.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Summary description for General
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class General : System.Web.Services.WebService
    {
        /// <summary>
        /// Obtiene la estructura para idcos del rsf
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [WebMethod]
        public string[][] ObtenerEstructuraRSF(DateTime fecha)
        {
            return (new RsfAppServicio()).ObtenerEstructura(fecha);
        }
    }
}
