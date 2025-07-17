using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ServicioRPF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Linq;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.ServicioRPF;
using COES.Servicios.Aplicacion.ServicioRPF.Helper;
using System.Globalization;

namespace COES.MVC.Intranet.Areas.ServicioRPF.Controllers
{
    public class EnvioAGCController : BaseController
    {
       

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioAGCController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        #region Métodos 


        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        DatosAGCAppServicio servicio = new DatosAGCAppServicio();


        /// <summary>
        /// Constante para almacenar el nombre original del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesDatosAGC.NombreArchivoAGC] != null) ?
                    Session[ConstantesDatosAGC.NombreArchivoAGC].ToString() : null;
            }
            set { Session[ConstantesDatosAGC.NombreArchivoAGC] = value; }
        }

        /// <summary>
        /// Pagina inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            EnvioAGCModel model = new EnvioAGCModel();
            model.ListaEmpresa = this.servicio.ObtenerEmpresasPropietariasURS();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenearFormatoCarga(int idEmpresa, string fecha)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionIntranet;

                DateTime fechaDato = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                string fileName = string.Empty;
                List<ResultFormatoAGC> mensajes = new List<ResultFormatoAGC>();
                int result = this.servicio.GenerarArchivoCargaAGC(idEmpresa, fechaDato, path, out fileName, out mensajes);

                return Json(new { Result = result, FileName = fileName, Mensajes = mensajes });
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Result = -1, FileName = string.Empty });
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato(string fileName)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionIntranet + fileName;
            return File(fullPath, Constantes.AppZip, fileName);
        }

        /// <summary>
        /// Action para carga del archivo
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(string fileName)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionIntranet;

                if (System.IO.File.Exists(path + fileName))
                {
                    System.IO.File.Delete(path + fileName);
                }

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    this.NombreFile = file.FileName;

                    file.SaveAs(path + fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Action para procesamiento del archivo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(int idEmpresa, string fecha)
        {
            int result = 0;
            List<string> errores = new List<string>();
            try
            {
                if (this.NombreFile != null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionIntranet;
                    string fileName = this.NombreFile;

                    if (System.IO.File.Exists(path + fileName))
                    {
                        result = this.servicio.GrabarDatos(path + fileName, idEmpresa, DateTime.ParseExact(fecha,
                            Constantes.FormatoFecha, CultureInfo.InvariantCulture), ConstantesDatosAGC.FuenteIntranet,
                            base.UserName, out errores);

                        if (result == 1) System.IO.File.Delete(path+fileName);
                    }
                    else
                    {
                        result = 6; //- Archivo no existe
                    }
                }
                else
                {
                    result = 6; //- Archivo no existe
                }

                return Json(new { Result = result, ListaError = errores });
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                result = -1;
                return Json(new { Result = result, ListaError = errores });
            }
        }

        /// <summary>
        /// Permite visualizar la ventana de reporte de cumplimiento
        /// </summary>
        /// <returns></returns>
        public ActionResult Consulta()
        {
            EnvioAGCModel model = new EnvioAGCModel();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permite obtener el reporte de cumplimiento
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(string fecha)
        {
            EnvioAGCModel model = new EnvioAGCModel();
            model.ListaReporte = this.servicio.ObtenerReporteCumplimiento(DateTime.ParseExact(fecha,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture));
            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el reporte
        /// </summary>
        /// <param name="puntos"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporte(string puntos, string fecha)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionIntranet;
                string fileName = string.Empty;

                this.servicio.GenerarReporteCumplimiento(puntos, DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                    path, out fileName);

                return Json(new { Result = 1, FileName = fileName });
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Result = -1, FileName = string.Empty });
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarReporte(string fileName)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionIntranet + fileName;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppZip, fileName);
        }

        #endregion
    }
}