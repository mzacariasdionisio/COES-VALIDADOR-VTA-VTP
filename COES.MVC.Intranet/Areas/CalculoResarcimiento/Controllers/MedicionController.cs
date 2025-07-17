using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class MedicionController : BaseController
    {
        /// <summary>
        /// Instancia de clase de servicios
        /// </summary>
        CalidadProductoAppServicio servicio = new CalidadProductoAppServicio();

        /// <summary>
        /// Muestra la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            MedicionModel model = new MedicionModel();
            model.Entidad = this.servicio.GetByIdReEventoProducto(id);
            model.FechaInicial = (((DateTime)model.Entidad.Reevprfecinicio)).ToString(ConstantesAppServicio.FormatoFechaFull);
            model.FechaFinal = (((DateTime)model.Entidad.Reevprfecfin)).ToString(ConstantesAppServicio.FormatoFechaFull);
            model.ListaEmpresa = this.servicio.ObtenerSuministradorPorEvento(id);
            model.AnioMes = Tools.ObtenerNombreMes((int)model.Entidad.Reevprmes) + " " + model.Entidad.Reevpranio;
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return View(model);
        }

        /// <summary>
        /// Permite realizar la consulta de interrupciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Consultar(int empresa, int evento)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(this.servicio.ObteenerEstructuraMedicion(empresa, evento));
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int empresa, int evento)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
            string template = ConstantesCalculoResarcimiento.PlantillaMedicionEvento;
            string file = ConstantesCalculoResarcimiento.FormatoCargaMedicionEvento;

            return Json(this.servicio.GenerarFormatoCargaMedicion(path, template, file, empresa, evento));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                ConstantesCalculoResarcimiento.FormatoCargaMedicionEvento;
            return File(fullPath, Constantes.AppExcel, ConstantesCalculoResarcimiento.FormatoCargaMedicionEvento);
        }

        /// <summary>
        /// Permite cargar el archivo de puntos de entrega
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesCalculoResarcimiento.ArchivoImportacionMedicion;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite realizar la importacion de suministros
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarMedicion(int empresa, int evento)
        {
            try
            {                
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoImportacionMedicion;
                                
                EstructuraMedicion data = this.servicio.CargarMedicionesExcel(path, file, empresa, evento);

                return Json(data);
            }
            catch
            {
                return Json(new { Result = -1});
            }
        }

        /// <summary>
        /// Permite grabar los datos de interrupciones
        /// </summary>
        /// <param name="data"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarMedicion(string[][] data, int empresa, int evento, string comentario)
        {            
            return Json(this.servicio.GrabarMedicion(data, empresa, evento, base.UserName, comentario));
        }

        /// <summary>
        /// Permite obtener los envios de interrpciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="evento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Envios(int empresa, int evento)
        {
            return Json(this.servicio.ObtenerEnviosMediciones(empresa, evento));
        }
    }
}
