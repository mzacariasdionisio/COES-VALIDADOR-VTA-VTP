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
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.ServicioRPF;
using System.Globalization;
using COES.Servicios.Aplicacion.ServicioRPF.Helper;

namespace COES.MVC.Intranet.Areas.ServicioRPF.Controllers
{
    public class InformacionAGCController : BaseController
    {
        
        private readonly DatosAGCAppServicio datosAGCServicio = new DatosAGCAppServicio();
        private readonly RpfAppServicio rpfServicio = new RpfAppServicio();
        

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(InformacionAGCController));
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
        /// Index Factor de Utilizacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            InformacionAGCModel model = new InformacionAGCModel();

            model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

            DateTime fechaDefecto = DateTime.Now.AddDays(-1);
            model.Fecha = fechaDefecto.ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaExportacion = DateTime.Now.AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);

            List<EveRsfdetalleDTO> lstUrs = datosAGCServicio.ObtenerListadoURSPorFecha(fechaDefecto).OrderBy(x => x.Ursnomb).ToList();

            model.ListaEmpresas = lstUrs.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(y => new SiEmpresaDTO() { Emprcodi = y.Key.Emprcodi, Emprnomb = y.Key.Emprnomb }).OrderBy(x=>x.Emprnomb).ToList();
            model.ListaUrs = new List<EveRsfdetalleDTO>();
            model.ListaUrsPopup = lstUrs;
            model.ListaEquipos = new List<EqEquipoDTO>();
            model.HtmlListado = datosAGCServicio.GenerarHtmlListadoComparativo(new List<ComparativoExtranetConSP7>());




            return View(model);
        }

        /// <summary>
        /// Devuelve las empresas para cierta fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerListadoEmpresas(string fecha)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<SiEmpresaDTO> lstEmpresas = datosAGCServicio.ObtenerListadoURSPorFecha(fechaConsulta).GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(y => new SiEmpresaDTO() { Emprcodi = y.Key.Emprcodi, Emprnomb = y.Key.Emprnomb }).OrderBy(x => x.Emprnomb).ToList();
            return Json(lstEmpresas);
        }

        

        /// <summary>
        /// Devuelve las URS para cierta empresa y fecha
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerListadoUrs(int emprcodi, string fecha)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            
            List<EveRsfdetalleDTO> lstUrs = datosAGCServicio.ObtenerListadoURSPorFecha(fechaConsulta);
            List<EveRsfdetalleDTO> lstUrsPorEmpresa = lstUrs.Where(x => x.Emprcodi == emprcodi).OrderBy(m => m.Ursnomb).ToList();
            
            return Json(lstUrsPorEmpresa);
        }

        /// <summary>
        /// Devuelve los equipos (central o unidad) para cierta urs y fecha
        /// </summary>
        /// <param name="idurs"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerListadoEquipos(int idurs, string fecha)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<EveRsfdetalleDTO> lstUrs = datosAGCServicio.ObtenerListadoURSConEquiposPorFecha(fechaConsulta);
            List<EveRsfdetalleDTO> lstUrsCentralYEquipos = lstUrs.Where(x => x.Grupocodi == idurs).OrderBy(x => x.Ursnomb).ToList();

            return Json(lstUrsCentralYEquipos);
        }

        /// <summary>
        /// Realiza la comparacion de la informacion de extranet vs los datos sp7
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idUrs"></param>
        /// <param name="idEquipo"></param>
        /// <param name="senial"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CompararInformacion(string fecha, int idEmpresa, int idUrs, int idEquipo, int senial, int resolucion)
        {
            InformacionAGCModel model = new InformacionAGCModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<EveRsfdetalleDTO> lstUrs = datosAGCServicio.ObtenerListadoURSConEquiposPorFecha(fechaConsulta);
                List<ComparativoExtranetConSP7> listadoComparativo = datosAGCServicio.ObtenerListadoComparativoExtranetConSP7(fechaConsulta, idEmpresa, idUrs, idEquipo, senial, resolucion);
                model.HtmlListado = datosAGCServicio.GenerarHtmlListadoComparativo(listadoComparativo);                
                model.Grafico = datosAGCServicio.ObtenerGraficoComparativo(listadoComparativo, lstUrs, idUrs);
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
        /// Exporta la informacion de la tabla web (comparativo)
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idUrs"></param>
        /// <param name="idEquipo"></param>
        /// <param name="senial"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult exportarTabla(string fecha, int idEmpresa, int idUrs, int idEquipo, int senial, int resolucion)
        {
            InformacionAGCModel model = new InformacionAGCModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<ComparativoExtranetConSP7> listadoComparativo = datosAGCServicio.ObtenerListadoComparativoExtranetConSP7(fechaConsulta, idEmpresa, idUrs, idEquipo, senial, resolucion);
                string nameFile = "ComparativoExtranetVsHistoricoSP7_" + fecha.Replace("/", "") + ".xlsx";

                datosAGCServicio.GenerarReporteTabla(ruta, listadoComparativo, nameFile);
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
        /// Exporta informacion reportada por agentes desde la extranet
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="strIdUrs"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult exportarExtranet(string fecha, string strIdUrs)
        {
            InformacionAGCModel model = new InformacionAGCModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);                
                string nameFile = "DataAGC_URS_" + fecha.Replace("/", "") + ".csv";

                datosAGCServicio.GenerarCSVReporteExtranet(ruta, fechaConsulta, strIdUrs,  nameFile, out string strUrsSinPtomedicion);
                model.Resultado = nameFile;
                model.LstUrsSinPtoMedicion = strUrsSinPtomedicion != "" ? strUrsSinPtomedicion : "";
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
        /// exporta archivo kumpliy
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="strIdUrs"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult exportarKumpliy(string fecha, string strIdUrs)
        {
            InformacionAGCModel model = new InformacionAGCModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);                
                string nameFile = "Data_AGC_" + fechaConsulta.Day.ToString("00") + fechaConsulta.Month.ToString("00") + ".csv";

                datosAGCServicio.GenerarCSVReporteKumpliy(ruta, fechaConsulta, strIdUrs, nameFile, out string strUrsSinPtomedicion);
                model.Resultado = nameFile;
                model.LstUrsSinPtoMedicion = strUrsSinPtomedicion != "" ? strUrsSinPtomedicion : "";
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
        /// Exporta archivo excel, csv, ...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            System.IO.File.Delete(ruta + nombreArchivo);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        #endregion
    }
}