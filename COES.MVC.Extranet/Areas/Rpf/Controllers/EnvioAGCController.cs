using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Rpf.Model;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.ServicioRPF;
using COES.Servicios.Aplicacion.ServicioRPF.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace COES.MVC.Extranet.Areas.Rpf.controllers
{
    public class EnvioAGCController : BaseController
    {
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
            model.ListaEmpresa = (new SeguridadServicio.SeguridadServicioClient()).ObtenerEmpresasPorUsuario(User.Identity.Name).
                Where(x=> this.servicio.ObtenerEmpresasURS().Any(y=> x.EMPRCODI == y)).ToList();
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
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionExtranet;

                DateTime fechaDato = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);               
                string fileName = string.Empty;
                List<ResultFormatoAGC> mensajes = new List<ResultFormatoAGC>();
                int result = this.servicio.GenerarArchivoCargaAGC(idEmpresa, fechaDato, path, out fileName, out mensajes);

                return Json(new { Result = result, FileName = fileName, Mensajes = mensajes });
            }
            catch
            {
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
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionExtranet + fileName;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppZip, fileName);
        }

        /// <summary>
        /// Action para carga del archivo
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(string fileName)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionExtranet;
                
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
            catch (Exception)
            {             
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
                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDatosAGC.RutaExportacionExtranet;
                    string fileName = this.NombreFile;

                    if (System.IO.File.Exists(path + fileName))
                    {                       
                        result = this.servicio.GrabarDatos(path + fileName, idEmpresa, DateTime.ParseExact(fecha,
                            Constantes.FormatoFecha, CultureInfo.InvariantCulture), ConstantesDatosAGC.FuenteExtranet,
                            base.UserName, out errores);

                        if (result == 1) System.IO.File.Delete(path + fileName);
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
            catch
            {
                result = -1;
                return Json(new { Result = result, ListaError = errores });
            }            
        }


        /// <summary>
        /// Permite visualizar la página de consulta
        /// </summary>
        /// <returns></returns>
        public ActionResult Consulta()
        {
            EnvioAGCModel model = new EnvioAGCModel();
            model.ListaEmpresa = (new SeguridadServicio.SeguridadServicioClient()).ObtenerEmpresasPorUsuario(User.Identity.Name).
               Where(x => this.servicio.ObtenerEmpresasURS().Any(y => x.EMPRCODI == y)).ToList();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite obtener las urs de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerUrsPorEmpresa(int idEmpresa)
        {
            return Json(this.servicio.ObtenerUrsPorEmpresa(idEmpresa));
        }

        /// <summary>
        /// Permite obtener el listado de centrales o unidades por urs que reportaton por extranet
        /// </summary>
        /// <param name="idUrs"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerUnidadesURS(int idUrs, string fecha)
        {
            return Json(this.servicio.ObtenerConfiguracionGenerador(idUrs, DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Permite obtener la consulta de datos
        /// </summary>
        /// <param name="idUrs"></param>
        /// <param name="idEquipo"></param>
        /// <param name="fecha"></param>
        /// <param name="tipoDato"></param>
        /// <param name="minutoInicio"></param>
        /// <param name="minutoFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerConsultaDatos(int idUrs, int idEquipo, string fecha, int tipoDato, int minutoInicio, int minutoFin)
        {
            return Json(this.servicio.ObtenerConsultaDatos(idUrs, idEquipo, DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                tipoDato, minutoInicio, minutoFin));
        }
    }
}
