using COES.Servicios.Aplicacion.CortoPlazo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ValorizacionDiaria.Controllers
{
    public class CostosMarginalesController : Controller
    {

        /// <summary>
        /// Llama a la página principal del listado de revisiones
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

     

        public JsonResult EjecutarCostosMarginales(string fecha, bool flajMdcoes = false)
        {
            int indicador = 1;
            try
            {

                CostoMarginalAppServicio servicio = new CostoMarginalAppServicio();
                //servicio.Procesar(fecha, indicadorPSSE, reproceso, indicadorNCP, flagWeb);
                servicio.Procesar(DateTime.Now, 1, false, false, true, string.Empty, false, 0, string.Empty, 1);

            }
            catch
            {
                indicador = 0;
            }

            return Json(indicador);
        }
    }
}
