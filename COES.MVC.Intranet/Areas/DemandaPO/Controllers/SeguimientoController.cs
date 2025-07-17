using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.DemandaPO.Models;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class SeguimientoController : Controller
    {
        DemandaPOAppServicio servicio = new DemandaPOAppServicio();
        public ActionResult Index()
        {
            SeguimientoModel model = new SeguimientoModel();
            model.FechaMes = DateTime.Now.ToString(ConstantesDpo.FormatoMesAnio);
            model.FechaDia = DateTime.Now.ToString(ConstantesDpo.FormatoFecha);
            model.ListaPuntos = this.servicio.ListPtomedicionSco();
            return View(model);
        }

        public JsonResult ConsultarEstado(string fecha)
        {            
            List<int> res = this.servicio.RepEstProcesoDpoDemandaSco(
                ConstantesDpo.DporawfuenteIeod, fecha);

            return Json(res);
        }

        public JsonResult ConsultarFiltrado(int punto, string fecha)
        {
            string pathFile = ConfigurationManager.AppSettings[ConstantesDpo.RutaReporte].ToString();
            object res;
            try
            {
                int valid = 1;
                string response = this.servicio.PruebaFiltradoInfSco(punto, fecha, pathFile);
                res = new {valid, response };
            }
            catch (Exception ex)
            {
                int valid = -1;
                //la tabla correspondiente a la fecha de consulta no existe
                string str = (ex.Message.Contains("ORA-00942"))
                    ? "No existe información TNA (fuente) para la fecha seleccionada"
                    : ex.Message;
                string response = str;
                res = new { valid, response};
                return Json(res);
            }

            return Json(res);
        }

        /// <summary>
        /// Ejecuta el proceso de filtrado de información para el mes solicitado
        /// </summary>
        /// <param name="tipo">Tipo de fecha a ejecutar (día o mes)</param>
        /// <param name="fecha">Fecha de ejecución del proceso</param>
        /// <param name="idPunto">Punto de medición a ejecutar (-1 = todos)</param>
        /// <returns></returns>
        public JsonResult EjecutarFiltrado(string tipo, string fecha,
            int idPunto)
        {
            DateTime parseFecha = new DateTime();
            string res = "Se realizó el proceso correctamente!";

            if (tipo == "dia")
            {
                parseFecha = DateTime.ParseExact(fecha,
                    ConstantesDpo.FormatoFecha,
                    CultureInfo.InvariantCulture);
            }
            else if (tipo == "mes")
            {
                parseFecha = DateTime.ParseExact(fecha,
                    ConstantesDpo.FormatoMesAnio,
                    CultureInfo.InvariantCulture);
            }

            try
            {
                this.servicio.FiltradoInformacionTNA(parseFecha, tipo, idPunto);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return Json(res);
        }

        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesDpo.RutaReporte].ToString() + file;
            string app = Constantes.AppExcel;
            return File(path, app, sFecha + "_" + file);
        }
    }
}