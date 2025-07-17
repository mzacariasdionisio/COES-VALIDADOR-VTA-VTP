using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.CalculoResarcimiento.Model;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.CalculoResarcimiento.Controllers
{
    public class RechazoCargaController : BaseController
    {
        /// <summary>
        /// Instancia del servicio de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        CalculoResarcimientoAppServicio servicio = new CalculoResarcimientoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(RechazoCargaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Permite mostrar la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RechazoCargaModel model = new RechazoCargaModel();
            model.Anio = DateTime.Now.Year;
            model.ListaPeriodo = this.servicio.ObtenerPeriodosPorAnio(model.Anio);
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
               Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            model.IndicadorEmpresa = (model.ListaEmpresas.Count > 1) ? Constantes.SI : Constantes.NO;
            if (model.ListaEmpresas.Count > 0)
            {
                model.Emprcodi = model.ListaEmpresas[0].Emprcodi;
                model.Emprnombre = model.ListaEmpresas[0].Emprnomb;
            }            

            return View(model);
        }

        /// <summary>
        /// Permite mostrar el dialogo de cambio de empresa
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresa()
        {
            RechazoCargaModel model = new RechazoCargaModel();
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
               Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar los periodos por anio
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPeriodos(int anio)
        {
            return Json(this.servicio.ObtenerPeriodosPorAnio(anio));
        }

        /// <summary>
        /// Permite realizar la consulta de interrupciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Consultar(int empresa, int periodo)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(this.servicio.ObtenerEstructuraRechazoCarga(empresa, periodo, true));
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
        public JsonResult GenerarFormato(int empresa, int periodo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
            string template = ConstantesCalculoResarcimiento.PlantillaRechazoCarga;
            string file = ConstantesCalculoResarcimiento.FormatoCargaRechazoCarga;

            return Json(this.servicio.GenerarFormatoRechazoCarga(path, template, file, empresa, periodo));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                ConstantesCalculoResarcimiento.FormatoCargaRechazoCarga;
            return File(fullPath, Constantes.AppExcel, ConstantesCalculoResarcimiento.FormatoCargaRechazoCarga);
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
                    string fileName = path + ConstantesCalculoResarcimiento.ArchivoImportacionRechazoCarga;

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
        public JsonResult ImportarInterrupciones(int empresa, int periodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoImportacionRechazoCarga;
               
                List<string> validaciones = new List<string>();
                string[][] data = this.servicio.CargarRechazoCargaExcel(path, file, empresa, periodo, out validaciones);

                if (validaciones.Count == 0)
                    return Json(new { Result = 1, Data = data, Errores = new List<string>() });
                else
                    return Json(new { Result = 2, Data = new string[1][], Errores = validaciones });
            }
            catch
            {
                return Json(new { Result = -1, Data = new string[1][], Errores = new List<string>() });
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
        public JsonResult GrabarInterrupciones(string[][] data, int empresa, int periodo, string comentario) 
        {            
            return Json(this.servicio.GrabarRechazosCarga(data, empresa, periodo, base.UserName, comentario, ConstantesCalculoResarcimiento.FuenteIngresoExtranet));
        }

        /// <summary>
        /// Permite anular una interrupcion
        /// </summary>
        /// <param name="idInterrupcion"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AnularInterrupcion(int idPeriodo, int idInterrupcion, string comentario, string recalculo, string[][] data)
        {
            return Json(this.servicio.AnularRechazoCarga(idPeriodo, idInterrupcion, comentario, base.UserName, 
                ConstantesCalculoResarcimiento.FuenteIngresoExtranet, recalculo, data));
        }

        /// <summary>
        /// Permite obtener los datos de anulacion de una interrupcion
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosAnulacion(string[][] data, int periodo)
        {
            return Json(this.servicio.ObtenerDatosAnulacionRC(data, periodo));
        }

        /// <summary>
        /// Permite obtener los envios de interrpciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Envios(int idEmpresa, int idPeriodo)
        {
            return Json(this.servicio.ObtenerEnviosInterrupciones(idEmpresa, idPeriodo, 
                ConstantesCalculoResarcimiento.EnvioTipoRechazoCarga));
        }

        /// <summary>
        /// Valida si una empresa tiene habilitado el permiso de carga de datos, todo para cierto periodo
        /// </summary>
        /// <param name="repercodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarHabilitacionCarga(int repercodi, int emprcodi, int tipo)
        {
            RechazoCargaModel model = new RechazoCargaModel();

            try
            {
                //base.ValidarSesionJsonResult();
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.Mensaje = servicio.ValidarHabilitacionCargaDatosParaEmpresaPeriodo(repercodi, emprcodi, tipo);
              
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
    }
}
