using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class EscenarioSDDPController : BaseController
    {
        private readonly ProgramacionAppServicio pmpoServicio = new ProgramacionAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ParametrosFechasController));
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
        /// Index Escenarios SDDP
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            EscenarioSDDPModel model = new EscenarioSDDPModel();

            try
            {
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.UsarLayoutModulo = false;
                
                model.MesAnioMensual = pmpoServicio.ultimoMesAnioOficializadoMasUnMes(ConstantesPMPO.ResolucionMensual);
                model.MesAnioSemanal = pmpoServicio.ultimoMesAnioOficializadoMasUnMes(ConstantesPMPO.ResolucionSemanal);                

                model.Mes = model.MesAnioMensual.Replace("*", "");
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return View(model);
        }

        /// <summary>
        /// Devuelve el html del listado de escenarios SDDP
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEscenariosSddp()
        {
            EscenarioSDDPModel model = new EscenarioSDDPModel();
            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.HtmlListadoEscenariosSddp = pmpoServicio.HtmlListadoEscenariosSddp(url, tienePermiso);
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
        /// Devuelve el html de la ventana de todos los escenario para un tipo de escenario SDDP
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoEscenarioTotales(int mtopcodi)
        {
            EscenarioSDDPModel model = new EscenarioSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = pmpoServicio.GenerarHtmlEscenariosTotales(url, mtopcodi, tienePermiso);
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
        /// Muestra html de la pestaña "Detalles"
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarDetalles(int accion, int topcodi)
        {
            EscenarioSDDPModel model = new EscenarioSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (accion == ConstantesPMPO.AccionVerDetalles)
                    model.TienePermisoNuevo = false; //Desactiva botones

                MpTopologiaDTO escenario = pmpoServicio.GetByIdMpTopologia(topcodi);
                string res = escenario.Mtopresolucion;
                model.VersionMostrada = escenario.Mtopversion.Value;
                model.Accion = accion;
                model.Resolucion = res != null ? (res != "" ? (res == "S" ? "Semanal" : (res == "M" ? "Mensual" : "")) : "") : "";
                model.Periodo = escenario.Mtopfecha.Value.Month.ToString("00") + " " + escenario.Mtopfecha.Value.Year;
                model.CodigoEscenario = escenario.Mtopcodi;
                //model.SoloLectura = accion == 3 ? true : false;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Valida y crea un escenario SDDP 
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CrearNuevoEscenario(int accion, string resolucion, int anio, int mes)
        {
            EscenarioSDDPModel model = new EscenarioSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime periodo = new DateTime(anio, mes, 1);

                string msgValidacion = pmpoServicio.ValidarNuevoEscenario(resolucion, periodo); 
                if (msgValidacion != "") throw new ArgumentException(msgValidacion);

                //modulo fechas
                PmoMesDTO mesOperativo = pmpoServicio.ListarSemanaMesDeAnho(anio, ConstantesPMPO.AccionEditar, null).Find(x => x.Pmmesfecinimes.Month == mes);
                DateTime fechaIniSemOperativa = mesOperativo.Pmmesfecini; //primer sabado del mes operativo 

                List<EqEquipoDTO> lstCH = pmpoServicio.ListarTodasCentralesHidroelectricas(fechaIniSemOperativa);
                string strListaCH = pmpoServicio.ObtenerCadenaDeCentralesHidro(lstCH);
                
                List<EqEquipoDTO> lstTotalCHBD = pmpoServicio.FormatoCentralesHidroBD(strListaCH);
                pmpoServicio.CrearNuevoEscenario(base.UserName, accion, resolucion, periodo, lstTotalCHBD); 
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
        /// Asigan oficial a un escenario
        /// </summary>
        /// <param name="mtopcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MarcarOficial(int mtopcodi)
        {
            EscenarioSDDPModel model = new EscenarioSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.AsignarOficialEscenario(mtopcodi, base.UserName);

                //actualizar fechas popup crear              
                model.MesAnioMensual = pmpoServicio.ultimoMesAnioOficializadoMasUnMes(ConstantesPMPO.ResolucionMensual);
                model.MesAnioSemanal = pmpoServicio.ultimoMesAnioOficializadoMasUnMes(ConstantesPMPO.ResolucionSemanal);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Desoficiliza un escenario
        /// </summary>
        /// <param name="mtopcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QuitarOficial(int mtopcodi)
        {
            EscenarioSDDPModel model = new EscenarioSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.DesoficializarEscenario(mtopcodi, base.UserName);

                //actualizar fechas popup crear              
                model.MesAnioMensual = pmpoServicio.ultimoMesAnioOficializadoMasUnMes(ConstantesPMPO.ResolucionMensual);
                model.MesAnioSemanal = pmpoServicio.ultimoMesAnioOficializadoMasUnMes(ConstantesPMPO.ResolucionSemanal);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Elimina un escenario
        /// </summary>
        /// <param name="mtopcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEscenario(int mtopcodi)
        {
            EscenarioSDDPModel model = new EscenarioSDDPModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.EliminarEscenarioSddp(mtopcodi);

                //actualizar fechas popup crear              
                model.MesAnioMensual = pmpoServicio.ultimoMesAnioOficializadoMasUnMes(ConstantesPMPO.ResolucionMensual);
                model.MesAnioSemanal = pmpoServicio.ultimoMesAnioOficializadoMasUnMes(ConstantesPMPO.ResolucionSemanal);

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
        /// Generar archivos .dat
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="recurcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoSalida(int topcodi)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                pmpoServicio.GenerarArchivosSalidaHidro(topcodi, ruta, out string nameFile);
                pmpoServicio.GenerarArchivosSalidaHidroZip(ruta, out string nameFile2);

                model.Resultado = nameFile2;
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
        /// Permite exportar archivos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult ExportarZip()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            string fullPath = ruta + "PMPO\\CENTRALSDDP\\" + nombreArchivo;

            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar extension
        }
        #endregion
    }
}