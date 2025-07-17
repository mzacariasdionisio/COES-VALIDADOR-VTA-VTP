using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Combustibles.Controllers;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using DevExpress.XtraRichEdit.Drawing;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTReporteController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        FichaTecnicaAppServicio servicioFT = new FichaTecnicaAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ReporteGasController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        #endregion
               
        #region Reporte Ampliación plazo

        

        /// <summary>
        /// Lista los registros del reporte de ampliaciones
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="etapas"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarReporteAmpliacion(string empresas, string etapas, string finicios, string ffins)
        {
            FTAdministradorModel model = new FTAdministradorModel();
            try
            {
                base.ValidarSesionJsonResult();
                
                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                if (fechaInicio > fechaFin)
                    throw new ArgumentException("La fecha inicio no puede ser mayor a la final.");

                model.ListadoEnvios = servicioFT.ObtenerListadoReporteAmpliacionPlazo(empresas, fechaInicio, fechaFin, etapas);

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
        /// /Genera el archivo excel de listado de envíos
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="etapas"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporte(string empresas, string etapas, string finicios, string ffins)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                DateTime hoy = DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                string nameFile = "Reporte_AmpliacionPlazo_FichaTécnica_" + 
                    fechaInicio.ToString(ConstantesAppServicio.FormatoFechaDMY) + "_" + fechaFin.ToString(ConstantesAppServicio.FormatoFechaDMY) + ".xlsx";

                servicioFT.GenerarExportacionEnviosEtapa(ruta, pathLogo, empresas, fechaInicio, fechaFin, etapas, nameFile);
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
        public virtual FileResult Exportar(string file_name)
        {
            base.ValidarSesionUsuario();

            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes + file_name;

            //eliminar archivo temporal
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, file_name);
        }


        #endregion

        #region Reporte histórico de Envíos

        public ActionResult IndexHistoricoEnvio()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAdministradorModel model = new FTAdministradorModel();
            model.IdEstado = 1;// carpeta.GetValueOrDefault(0) <= 0 ? ConstantesFichaTecnica.EstadoSolicitud : carpeta.Value;

            model.ListaEmpresas = servicioFT.ListarEmpresasExtranetFT(); //comentar para probar CP104
            //model.ListaEmpresas = new List<EmpresaCoes>();  //descomentar para probar CP104
            model.NumeroEmpresas = model.ListaEmpresas.Count();
            model.ListaEtapas = servicioFT.ListFtExtEtapas();

            model.ListaEstados = servicioFT.ListExtEstadoEnvio();

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (hoy.AddYears(-1)).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = (hoy).ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        [HttpPost]
        public JsonResult GenerarFormatoConexIntegModifHistorico(int codigoEnvio)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                bool esHistorico = true;

                //obtener valores necesarios para exportación                
                FtExtEnvioDTO objEnvio = servicioFT.GetByIdFtExtEnvio(codigoEnvio);
                model.IdEnvio = codigoEnvio;
                model.IdVersion = servicioFT.GetVersionSegunAmbiente(objEnvio, ConstantesFichaTecnica.INTRANET);
                model.IdEstado = objEnvio.Estenvcodi;

                model.ListaEnvioEq = servicioFT.ListFtExtEnvioEqsXEnvio(model.IdVersion);
                model.LstFteeqcodis = string.Join(",", model.ListaEnvioEq.Select(x => x.Fteeqcodi));

                string strAreasUsuario = servicioFT.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
                //model.StrIdsAreaDelUsuario = strAreasUsuario;
                model.StrIdsAreaTotales = servicioFT.ObtenerIdAreaTotales();

                bool esAdmin = this.TieneRolAdministradorFicha();
                //esAdmin = false;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                servicioFT.GenerarReporteConexIntegModifXEnvioHistorico(ruta, pathLogo, codigoEnvio, model.LstFteeqcodis, model.IdEstado, ConstantesFichaTecnica.INTRANET, model.IdVersion, model.StrIdsAreaTotales, esHistorico, esAdmin, out string fileName); //SOS

                model.Resultado = fileName;
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

        [HttpPost]
        public JsonResult GenerarFormatoRevisionContenidoHistorico(int codigoEnvio)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                bool esHistorico = true;
                model.StrIdsAreaTotales = servicioFT.ObtenerIdAreaTotales();
                bool esAdmin = this.TieneRolAdministradorFicha();
                esAdmin = true;

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                var objEnvioAct = servicioFT.GetByIdFtExtEnvio(codigoEnvio); //tipo formato: operación comercial o dar de baja
                servicioFT.GenerarFormatoRevisionContenidoHistorico(ruta, pathLogo, codigoEnvio, objEnvioAct.Ftenvtipoformato, model.StrIdsAreaTotales, 0, esHistorico, esAdmin, out string fileName);

                model.Resultado = fileName;
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

        #region Reporte de cumplimiento 

        public ActionResult IndexCumplimientoEnvio()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAdministradorModel model = new FTAdministradorModel();
            
            model.ListaEmpresas = servicioFT.ListarEmpresasExtranetFT(); //comentar para probar CP104            
            model.ListaEtapas = servicioFT.ListFtExtEtapas();
            model.ListaProyectos = servicioFT.GetProyectosByEstado(ConstantesFichaTecnica.EstadoStrActivo).OrderBy(x=>x.Ftprynombre).ToList();
            model.ListaAreas = servicioFT.ListFtExtCorreoareas().Where(x => x.Faremestado == ConstantesFichaTecnica.EstadoStrActivo && x.Faremcodi != ConstantesFichaTecnica.IdAreaAdminFT).ToList();

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (hoy.AddYears(-1)).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = (hoy).ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Lista los registos del reporte de cumplimiento de la administracion FT
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="idEstado"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarReporteCumplimientoAdminFT(string empresas, int ftetcodi, int idProyecto, string finicios, string ffins)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                model.ListaReporteCumplimientoAdminFT = servicioFT.ObtenerListadoReporteCumplimientoAdmin(empresas, ftetcodi, idProyecto, fechaInicio, fechaFin );

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
        /// /Genera el archivo excel de listado de envíos
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="idEstado"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteCumplimientoAdminFT(string empresas, int ftetcodi, int idProyecto, string finicios, string ffins)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                DateTime hoy = DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "rptCumplimientoPlazos_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) +
                                                string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                servicioFT.GenerarArchivoCumplimientoAdminFT(ruta, pathLogo, empresas, fechaInicio, fechaFin, idProyecto, ftetcodi, nameFile);

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
        public virtual FileResult ExportarCumplimiento(string file_name)
        {
            base.ValidarSesionUsuario();

            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes + file_name;

            //eliminar archivo temporal
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, file_name);
        }

        /// <summary>
        /// Lista los registos del reporte de cumplimiento de la revision de las areas
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="idEstado"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarReporteCumplimientoAreas(string empresas, int ftetcodi, int idProyecto, int idArea, string finicios, string ffins)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                model.ListaReporteCumplimientoRevAreas = servicioFT.ObtenerListadoReporteCumplimientoRevisionAreas(empresas, ftetcodi, idProyecto, idArea, fechaInicio, fechaFin);

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
        /// Genera el archivo excel de listado de reporte cumplimiento para las areas
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="idProyecto"></param>
        /// <param name="idArea"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteCumplimientoAreas(string empresas, int ftetcodi, int idProyecto, int idArea, string finicios, string ffins)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                DateTime hoy = DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "rptSeguimientoRptaAreas_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) +
                                                string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                servicioFT.GenerarArchivoCumplimientoAreas(ruta, pathLogo, empresas, fechaInicio, fechaFin, idProyecto, idArea, ftetcodi, nameFile);

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
        
        #endregion
    }
}