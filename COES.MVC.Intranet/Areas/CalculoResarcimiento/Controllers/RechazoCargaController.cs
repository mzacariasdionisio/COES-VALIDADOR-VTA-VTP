using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Model;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Framework.Base.Tools;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class RechazoCargaController : BaseController
    {
        /// <summary>
        /// Instancia del servicio de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        CalculoResarcimientoAppServicio servicio = new CalculoResarcimientoAppServicio();

        /// <summary>
        /// Permite mostrar la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RechazoCargaModel model = new RechazoCargaModel();
            model.Anio = DateTime.Now.Year;
            model.ListaPeriodo = this.servicio.ObtenerPeriodosPorAnio(model.Anio);
            model.ListaEmpresas = this.servicio.ObtenerEmpresasSuministradorasTotal();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
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
            EstructuraRechazoCarga estructura = this.servicio.ObtenerEstructuraRechazoCarga(empresa, periodo, true);
            estructura.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            result.Content = serializer.Serialize(estructura);
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
            return Json(this.servicio.GrabarRechazosCarga(data, empresa, periodo, base.UserName, comentario, ConstantesCalculoResarcimiento.FuenteIngresoIntranet));
        }

        /// <summary>
        /// Permite anular una interrupcion
        /// </summary>
        /// <param name="idInterrupcion"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AnularInterrupcion(int idPeriodo, int idInterrupcion, string comentario)
        {
            return Json(this.servicio.AnularRechazoCarga(idPeriodo, idInterrupcion, comentario, base.UserName,
                ConstantesCalculoResarcimiento.FuenteIngresoIntranet, string.Empty, null));
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
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesCalculoResarcimiento.ModuloManualUsuario;
            string nombreArchivo = ConstantesCalculoResarcimiento.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesCalculoResarcimiento.FolderRaizResarcimientosModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }
    }
}
