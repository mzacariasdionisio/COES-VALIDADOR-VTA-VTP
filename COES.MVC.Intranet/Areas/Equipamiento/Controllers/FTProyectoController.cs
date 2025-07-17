using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTProyectoController : BaseController
    {
        FichaTecnicaAppServicio servicioFT = new FichaTecnicaAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTProyectoController));
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

        #region Listado Proyectos
        /// <summary>
        /// Pantalla Inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTProyectoModel model = new FTProyectoModel();
           
            model.ListaEmpresas = servicioFT.ListarEmpresasActivas();
            model.ListaTipoEmpresas = servicioFT.ListarTipoEmpresas();
            model.RangoIni = DateTime.Now.AddYears(-1).ToString(ConstantesAppServicio.FormatoFecha);
            model.RangoFin = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Lista los proyectos segun filtro ingresado
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarProyectos(string empresa, string rangoIni, string rangoFin)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaInicio = DateTime.ParseExact(rangoIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(rangoFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //fechaFin = fechaFin.AddDays(1);

                model.ListadoProyectos = servicioFT.ListarProyectos(empresa, fechaInicio, fechaFin);  
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
        /// Devuelve el listado de empresas segun su tipo
        /// </summary>
        /// <param name="tipoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresasXTipo(int tipoEmpresa)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {                
                model.ListaEmpresas = servicioFT.ObtenerEmpresasPorTipo(tipoEmpresa);
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
        /// Devuelve el listado de codigos de estudio eo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEstudiosEo(int idEmpresa)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListadoEstudiosEo = servicioFT.ListarEstudiosEo(idEmpresa);
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
        /// Obtiene los datos del Estudio EO
        /// </summary>
        /// <param name="esteocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatoEstudioEO(int esteocodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.EstudioEO = servicioFT.ObtenerEstudioEO(esteocodi);
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
        /// Devuelve datos de cierta empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatoEmpresa(int emprcodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Empresa = servicioFT.ObtenerDatosNuevoEmpresa(emprcodi);
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
        /// Guarda o actualiza proyectos
        /// </summary>
        /// <param name="empresaNomb"></param>
        /// <param name="codigo"></param>
        /// <param name="proyNomb"></param>
        /// <param name="proyExtNomb"></param>
        /// <param name="esteocodi"></param>
        /// <param name="empresaId"></param>
        /// <param name="conEstudio"></param>
        /// <param name="accion"></param>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarProyecto(string empresaNomb, string codigo, string proyNomb, string proyExtNomb, int? esteocodi, int empresaId, string conEstudio, int accion, int? ftprycodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FtExtProyectoDTO objProyecto = new FtExtProyectoDTO();
                objProyecto.Emprcodi = empresaId;
                objProyecto.Emprnomb = empresaNomb;
                objProyecto.Esteocodi = esteocodi;
                objProyecto.Ftpryeocodigo = codigo;
                objProyecto.Ftpryeonombre = proyNomb;
                objProyecto.Ftprynombre = proyExtNomb;

                string usuario = base.UserName;
               
                servicioFT.GuardarDatosProyecto(objProyecto, conEstudio, accion, usuario, ftprycodi);
                
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
        /// Genera el archivo a exportar el listado de proyectos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarProyectos(string empresa, string rangoIni, string rangoFin)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                DateTime hoy = DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "Reporte_ProyectosFichaTécnica_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) + string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                DateTime fechaInicio = DateTime.ParseExact(rangoIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(rangoFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //fechaFin = fechaFin.AddDays(1);

                servicioFT.GenerarExportacionProyectos(ruta, pathLogo, nameFile, empresa, fechaInicio, fechaFin);
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
        /// Exporta archivo pdf, excel, csv, ...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string strArchivoTemporal = ruta + nombreArchivo;
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        /// <summary>
        /// Obtiene los detalles de cierto proyecto
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DetallarProyecto(int ftprycodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Proyecto = servicioFT.GetByIdFtExtProyecto(ftprycodi);
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
        /// Elimina cierto proyecto
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarProyecto(int ftprycodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;
                servicioFT.DarBajaProyecto(ftprycodi, usuario);
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
        /// Activa cierto proyecto
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActivarProyecto(int ftprycodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;

                servicioFT.ActivarProyecto(ftprycodi, usuario);
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
        


        #endregion

    }
}