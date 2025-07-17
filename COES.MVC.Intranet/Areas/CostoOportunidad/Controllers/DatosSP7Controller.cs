using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CostoOportunidad.Helper;
using COES.MVC.Intranet.Areas.CostoOportunidad.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Controllers
{
    public class DatosSP7Controller : BaseController
    {
        private readonly CostoOportunidadAppServicio costoOpServicio = new CostoOportunidadAppServicio();
        private readonly ServiceReferenceCostoOportunidad.CostoOportunidadServicioClient coService = new ServiceReferenceCostoOportunidad.CostoOportunidadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(DatosSP7Controller));
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
        /// Index Datos SP7
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            DatosSP7Model model = new DatosSP7Model();

            model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

            List<CoPeriodoDTO> lstPeriodos = costoOpServicio.ListCoPeriodos().Where(x=>x.Coperestado == "A").OrderByDescending(x => x.Coperanio).ToList();
            model.ListaAnios = lstPeriodos.GroupBy(x => x.Coperanio.Value).Select(m => m.Key).ToList();
            model.Anio = model.ListaAnios.First();

            model.ListaPeriodos = lstPeriodos.Where(x => x.Coperanio == model.Anio).OrderBy(x => x.Copermes).ToList();
            int idPeriodo = model.ListaPeriodos.Any() ? model.ListaPeriodos.First().Copercodi : 0;

            model.ListaVersiones = costoOpServicio.GetByCriteriaCoVersions(idPeriodo).Where(x => x.Coverestado == "Abierto").ToList();

            return View(model);
        }

        /// <summary>
        /// Devuelve las versiones para cierto periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerVersiones(int idPeriodo)
        {
            try
            {
                return Json(this.costoOpServicio.GetByCriteriaCoVersions(idPeriodo).Where(x=>x.Coverestado == "Abierto").ToList());
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Develve el listado de la importacion en formato html
        /// </summary>
        /// <param name="copercodi"></param>
        /// <param name="covercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarImportados(int copercodi, int covercodi)
        {
            DatosSP7Model model = new DatosSP7Model();
            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                string url = Url.Content("~/");

                model.Resultado = costoOpServicio.GenerarHtmlImportados(url, copercodi, covercodi, out bool hayDiasConErrres);
                model.MostrarBtnDF = hayDiasConErrres;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Devuelve los periodos para cierto año
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            DatosSP7Model model = new DatosSP7Model();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodos = costoOpServicio.ListCoPeriodos().Where(x=>x.Coperanio == anio && x.Coperestado == "A").OrderBy(x=>x.Copermes).ToList();
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Genera reporte de errores
        /// </summary>
        /// <param name="prodiacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteError(int prodiacodi)
        {
            DatosSP7Model model = new DatosSP7Model();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                CoProcesoDiarioDTO procDiario = costoOpServicio.GetByIdCoProcesoDiario(prodiacodi);
                string nameFile = string.Format("ReporteErrores_DatosSP7_{0}{1}{2}.xlsx", string.Format("{0:D2}", procDiario.Prodiafecha.Value.Day), string.Format("{0:D2}", procDiario.Prodiafecha.Value.Month), string.Format("{0:D2}", procDiario.Prodiafecha.Value.Year));

                costoOpServicio.GenerarReporteDeErrores(ruta, prodiacodi, nameFile);
                model.Resultado = nameFile;
                
                
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Exporta informacion a un archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        /// <summary>
        /// Realiza la funcion de importar las señales SP7 para ciertoperiodo y version
        /// </summary>
        /// <param name="tipoImportacion"></param>
        /// <param name="copercodi"></param>
        /// <param name="covercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarSenialesSP7(int tipoImportacion, int copercodi, int covercodi)
        {
            DatosSP7Model model = new DatosSP7Model();
            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                //costoOpServicio.ImportarTodoSeñalesSP7(tipoImportacion, null, copercodi, covercodi, base.UserName, "");
                coService.ImportarTodoSeñalesSP7(tipoImportacion, null, copercodi, covercodi, base.UserName, "");


            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
        #endregion

        public ActionResult Carga()
        {

            CargaModel model = new CargaModel();
            model.ListaURS = costoOpServicio.ObtenerListaURS();
            model.FechaInicio = model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ConsultaSenial(string fechaInicio, string fechaFin, int urs, int canalcodi)
        {
            CargaModel model = new CargaModel();
            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListaConsulta = costoOpServicio.ObtenerConsultaDatosSeniales(fecInicio, fecFin, urs, canalcodi);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ObtenerSeniales(int idUrs) 
        {
            return Json(this.costoOpServicio.ObtenerSenialesPorURS(idUrs));
        }
   
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesCOportunidad.NombreArchivoSenial;

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
        /// Permite cargar la seniales desde excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarSeniales()
        {
            try
            {
                int result = 1;
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion + ConstantesCOportunidad.NombreArchivoSenial;
                List<string> validaciones = new List<string>();
                List<CoDatoSenialDTO> list = (new ToolsCostoOport()).LeerSenialesDesdeFormato(path, out validaciones);

                if(validaciones.Count == 0)
                {                     
                    result = costoOpServicio.AlmacenarDatoSenialManual(list, base.UserName, out validaciones);
                }
                
                return Json(new { Result = result, Validaciones = validaciones});
            }
            catch
            {
                return Json(new { Result = -1, Validaciones = new List<String>() });
            }
        }



    }
}