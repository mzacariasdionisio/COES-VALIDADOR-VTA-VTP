using COES.MVC.Extranet.Areas.RegistroIntegrante.Models;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using COES.MVC.Extranet.Helper;
using System.Web.Mvc;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System;
using System.Globalization;
using COES.MVC.Extranet.Areas.RegistroIntegrante.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using log4net;
using COES.Servicios.Aplicacion.Eventos;
using COES.MVC.Extranet.Controllers;
using System.Linq;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Controllers
{
    public class HistoricoController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(HistoricoController));

        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        EventoAppServicio servicio = new EventoAppServicio();


        public HistoricoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("HistoricoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("HistoricoController", ex);
                throw;
            }
        }

        /// <summary>
        /// Instancia de la clase ReportesAppServicio
        /// </summary>
        ReportesAppServicio appReporte = new ReportesAppServicio();

        #region Historico de Solicitudes

        /// <summary>
        /// Reporte Historico de Solicitudes
        /// </summary>
        /// <returns></returns>
        public ActionResult Solicitudes()
        {
            RegistroIntegrantesAppServicio appRegistroIntegrante = new RegistroIntegrantesAppServicio();
            GestionSolicitudesAppServicio appSolicitud = new GestionSolicitudesAppServicio();

            ReporteModel model = new ReporteModel();

            model.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            model.ListaTipoSolicitudes = appSolicitud.ListarTipoSolicitud();
            model.ListaTipoSolicitudes.Add(new RiTiposolicitudDTO()
            {
                Tisocodi = ConstantesRegistroIntegrantes.TodosCodigoEntero,
                Tisonombre = ConstantesRegistroIntegrantes.Todos
            });

            model.ListaEmpresas = base.ListaEmpresas.Select(x => new Dominio.DTO.Sic.EmpresaDTO { EMPRCODI = x.EMPRCODI, EMPRNOMB = x.EMPRNOMB }).ToList();

            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado del reporte Historico de Solicitudes
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>          
        /// <param name="tiposolicitud">tipo de solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoHistoricoSolicitudes(string finicios,
            string ffins,
            string tiposolicitud,
            string empresa)
        {

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (finicios != null)
            {
                fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (ffins != null)
            {
                fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFin = fechaFin.AddDays(1);

            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            string tipoempresa = "0";
            if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(empresa)) empresa = "0";

            int nroRegistros = this.appReporte.ObtenerTotalRegListarHistoricoSolicitudes(fechaInicio, fechaFin, tipoempresa, tiposolicitud, empresa);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite pintar la lista de registros del reporte de Historico de Solicitudes
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>          
        /// <param name="tiposolicitud">tipo de solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <param name="nroPagina">numero de pagina</param>
        /// <returns></returns>
        public ActionResult ListadoHistoricoSolicitudes(string finicios,
            string ffins,
            string tiposolicitud,
            string empresa,
            int nroPagina)
        {

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (finicios != null)
            {
                fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (ffins != null)
            {
                fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFin = fechaFin.AddDays(1);

            ReporteModel model = new ReporteModel();

            string tipoempresa = "0";
            if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(empresa)) empresa = "0";

            List<SiEmpresaDTO> reporte = this.appReporte.ListarHistoricoSolicitudes(fechaInicio, fechaFin, tipoempresa, tiposolicitud, empresa, nroPagina,
                Constantes.PageSize);

            model.ListaIntegrantes = reporte;

            return View(model);
        }


        /// <summary>
        /// Permite generar los datos del reporte de Historico de Solicitudes para exportar
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>        
        /// <param name="tiposolicitud">tipo solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteHistoricoSolicitudes(string finicios,
            string ffins,
            string tiposolicitud,
            string empresa)
        {
            int indicador = 1;
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (finicios != null)
                {
                    fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (ffins != null)
                {
                    fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                fechaFin = fechaFin.AddDays(1);

                ReporteModel model = new ReporteModel();

                string tipoempresa = "0";
                if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = ConstantesRegistroIntegrantes.ParametroDefecto;
                if (string.IsNullOrEmpty(empresa)) empresa = "0";

                List<SiEmpresaDTO> reporte = this.appReporte.ListarHistoricoSolicitudesFiltroXls(fechaInicio, fechaFin, tipoempresa, tiposolicitud, empresa);

                model.ListaIntegrantes = reporte;


                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
                string titulo = "HISTÓRICO DE SOLICITUDES";
                ExcelDocumentHistoricoSolicitudes.GernerarArchivoEnvios(reporte, ruta, titulo);
                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
                log.Error(ex);
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar los datos del reporte de Historico de Solicitudes
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarExcelHistoricoSolicitudes()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesRegistroIntegrantes.NombreReporteEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }


        #endregion

        #region Historico de Revisiones

        /// <summary>
        /// Reporte Historico de Revisiones
        /// </summary>
        /// <returns></returns>
        public ActionResult Revisiones()
        {
            RegistroIntegrantesAppServicio appRegistroIntegrante = new RegistroIntegrantesAppServicio();
            GestionSolicitudesAppServicio appSolicitud = new GestionSolicitudesAppServicio();

            ReporteModel model = new ReporteModel();

            model.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            //Lista Tipo Revisión
            model.ListaTipoRevision.Add(new TipoRevisionModel()
            {
                TipoRevisionCodigo = ConstantesRegistroIntegrantes.TodosCodigoEntero,
                TipoRevisionDescripcion = ConstantesRegistroIntegrantes.Todos
            });
            model.ListaTipoRevision.Add(new TipoRevisionModel()
            {
                TipoRevisionCodigo = ConstantesRegistroIntegrantes.EtrvSGI,
                TipoRevisionDescripcion = ConstantesRegistroIntegrantes.EtapaSGI
            });
            model.ListaTipoRevision.Add(new TipoRevisionModel()
            {
                TipoRevisionCodigo = ConstantesRegistroIntegrantes.EtrvDJR,
                TipoRevisionDescripcion = ConstantesRegistroIntegrantes.EtapaDJR
            });


            model.ListaEmpresas = base.ListaEmpresas.Select(x => new Dominio.DTO.Sic.EmpresaDTO { EMPRCODI = x.EMPRCODI, EMPRNOMB = x.EMPRNOMB }).ToList();

            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado del reporte de Historico de Revisiones
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>              
        /// <param name="tiporevision">tipo de revision</param>        
        /// <param name="empresa">empresa</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoHistoricoRevisiones(string finicios,
            string ffins,
            string tiporevision,
            string empresa)
        {

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (finicios != null)
            {
                fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (ffins != null)
            {
                fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFin = fechaFin.AddDays(1);

            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            string tipoempresa = "0";
            if (string.IsNullOrEmpty(tiporevision)) tiporevision = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(empresa)) empresa = "0";

            int nroRegistros = this.appReporte.ObtenerTotalRegListarHistoricoRevisiones(fechaInicio, fechaFin, tipoempresa, tiporevision, empresa);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite pintar la lista de registros del reporte de Historico de Revisiones
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>        
        /// <param name="tiporevision">tipo de revision</param>  
        /// <param name="empresa">empresa</param>
        /// <param name="nroPagina">numero de pagina</param>
        /// <returns></returns>
        public ActionResult ListadoHistoricoRevisiones(string finicios,
            string ffins,
            string tiporevision,
            string empresa,
            int nroPagina)
        {

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (finicios != null)
            {
                fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (ffins != null)
            {
                fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFin = fechaFin.AddDays(1);

            ReporteModel model = new ReporteModel();

            string tipoempresa = "0";
            if (string.IsNullOrEmpty(tiporevision)) tiporevision = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(empresa)) empresa = "0";

            List<SiEmpresaDTO> reporte = this.appReporte.ListarHistoricoRevisiones(fechaInicio, fechaFin, tipoempresa, tiporevision, empresa, nroPagina,
                Constantes.PageSize);

            model.ListaIntegrantes = reporte;

            return View(model);
        }


        /// <summary>
        /// Permite generar los datos del reporte de Historico de Revisiones para exportar
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>        
        /// <param name="tiporevision">tipo de revision</param>      
        /// <param name="empresa">empresa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteHistoricoRevisiones(string finicios,
            string ffins,
            string tiporevision,
            string empresa)
        {
            int indicador = 1;
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (finicios != null)
                {
                    fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (ffins != null)
                {
                    fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                fechaFin = fechaFin.AddDays(1);

                ReporteModel model = new ReporteModel();

                string tipoempresa = "0";
                if (string.IsNullOrEmpty(tiporevision)) tiporevision = ConstantesRegistroIntegrantes.ParametroDefecto;
                if (string.IsNullOrEmpty(empresa)) empresa = "0";

                List<SiEmpresaDTO> reporte = this.appReporte.ListarHistoricoRevisionesFiltroXls(fechaInicio, fechaFin, tipoempresa, tiporevision, empresa);

                model.ListaIntegrantes = reporte;


                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
                string titulo = "HISTÓRICO DE REVISIONES";
                ExcelDocumentHistoricoRevisiones.GernerarArchivoEnvios(reporte, ruta, titulo);
                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
                log.Error(ex);
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar los datos del reporte de Historico de Revisiones
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarExcelHistoricoRevisiones()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesRegistroIntegrantes.NombreReporteEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }


        #endregion

        #region Historico de Modificaciones

        /// <summary>
        /// Reporte Historico de Modificaciones
        /// </summary>
        /// <returns></returns>
        public ActionResult Modificaciones()
        {
            RegistroIntegrantesAppServicio appRegistroIntegrante = new RegistroIntegrantesAppServicio();
            GestionSolicitudesAppServicio appSolicitud = new GestionSolicitudesAppServicio();

            ReporteModel model = new ReporteModel();

            model.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            model.ListaTipoSolicitudes = appSolicitud.ListarTipoSolicitud();
            model.ListaEmpresas = base.ListaEmpresas.Select(x => new Dominio.DTO.Sic.EmpresaDTO { EMPRCODI = x.EMPRCODI, EMPRNOMB = x.EMPRNOMB }).ToList();

            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado del reporte de Historico de Modificaciones
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>             
        /// <param name="tiposolicitud">tipo de solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoHistoricoModificaciones(string finicios,
            string ffins,
            string tiposolicitud,
            string empresa)
        {

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (finicios != null)
            {
                fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (ffins != null)
            {
                fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFin = fechaFin.AddDays(1);

            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            string tipoempresa = "0";
            if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = "0";
            if (string.IsNullOrEmpty(empresa)) empresa = "0";

            int nroRegistros = this.appReporte.ObtenerTotalRegListarHistoricoModificaciones(fechaInicio, fechaFin, tipoempresa, tiposolicitud, empresa);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite pintar la lista de registros del reporte de Historico de Modificaciones
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>          
        /// <param name="tiposolicitud">tipo de solicitud</param>
        /// <param name="empresa">empresa</param>        
        /// <param name="nroPagina">numero de pagina</param>
        /// <returns></returns>
        public ActionResult ListadoHistoricoModificaciones(string finicios,
            string ffins,
            string tiposolicitud,
            string empresa,
            int nroPagina)
        {

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (finicios != null)
            {
                fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (ffins != null)
            {
                fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFin = fechaFin.AddDays(1);

            ReporteModel model = new ReporteModel();

            string tipoempresa = "0";
            if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = "0";
            if (string.IsNullOrEmpty(empresa)) empresa = "0";

            List<SiEmpresaDTO> reporte = this.appReporte.ListarHistoricoModificaciones(fechaInicio, fechaFin, tipoempresa, tiposolicitud, empresa, nroPagina,
                Constantes.PageSize);

            model.ListaIntegrantes = reporte;

            return View(model);
        }


        /// <summary>
        /// Permite generar los datos del reporte de Historico de Modificaciones para exportar
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>                
        /// <param name="tiposolicitud">tipo solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteHistoricoModificaciones(string finicios,
            string ffins,
            string tiposolicitud,
            string empresa)
        {
            int indicador = 1;
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (finicios != null)
                {
                    fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (ffins != null)
                {
                    fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                fechaFin = fechaFin.AddDays(1);

                ReporteModel model = new ReporteModel();

                string tipoempresa = "0";
                if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = "0";
                if (string.IsNullOrEmpty(empresa)) empresa = "0";

                List<SiEmpresaDTO> reporte = this.appReporte.ListarHistoricoModificacionesFiltroXls(fechaInicio, fechaFin, tipoempresa, tiposolicitud, empresa);

                model.ListaIntegrantes = reporte;


                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
                string titulo = "HISTORICO DE MODIFICACIONES";
                ExcelDocumentHistoricoModificaciones.GernerarArchivoEnvios(reporte, ruta, titulo);
                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
                log.Error(ex);
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar los datos del reporte de Historico de Modificaciones
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarExcelHistoricoModificaciones()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesRegistroIntegrantes.NombreReporteEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }


        #endregion
    }
}