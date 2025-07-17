using COES.MVC.Extranet.Areas.RegistroIntegrante.Models;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using COES.MVC.Extranet.Helper;
using System.Web.Mvc;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System;
using System.Globalization;
using COES.MVC.Extranet.Areas.RegistroIntegrante.Helper;
using log4net;
using System.Linq;
using COES.MVC.Extranet.SeguridadServicio;
using COES.MVC.Extranet.Controllers;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Controllers
{
    public class ReporteController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ReporteController));

        public ReporteController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ReporteController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ReporteController", ex);
                throw;
            }
        }

        /// <summary>
        /// Instancia de la clase ReportesAppServicio
        /// </summary>
        ReportesAppServicio appReporte = new ReportesAppServicio();

        #region Reporte Empresas

        /// <summary>
        /// Reporte de Empresas
        /// </summary>
        /// <returns></returns>
        public ActionResult Empresas()
        {
            RegistroIntegrantesAppServicio appRegistroIntegrante = new RegistroIntegrantesAppServicio();

            ReporteModel model = new ReporteModel();

            model.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            model.ListaModalidadVoluntarioObligatorio.Add(new ModalidadVoluntarioObligatorioModel(
                ConstantesRegistroIntegrantes.ModalidadVoluntarioObligatorioCodigoVoluntario,
                ConstantesRegistroIntegrantes.ModalidadVoluntarioObligatorioDescripcionVoluntario));

            model.ListaModalidadVoluntarioObligatorio.Add(new ModalidadVoluntarioObligatorioModel(
                ConstantesRegistroIntegrantes.ModalidadVoluntarioObligatorioCodigoObligatorio,
                ConstantesRegistroIntegrantes.ModalidadVoluntarioObligatorioDescripcionObligatorio));

            model.ListaModalidadVoluntarioObligatorio.Add(new ModalidadVoluntarioObligatorioModel(
                ConstantesRegistroIntegrantes.TodosCodigoString,
                ConstantesRegistroIntegrantes.Todos));

            model.ListaTipoEmpresa = appRegistroIntegrante.ListTipoEmpresa();
            model.ListaTipoEmpresa.Add(new SiTipoempresaDTO()
            {
                Tipoemprcodi = ConstantesRegistroIntegrantes.TodosCodigoEntero,
                Tipoemprabrev = ConstantesRegistroIntegrantes.Todos,
                Tipoemprdesc = ConstantesRegistroIntegrantes.Todos
            });

            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado del reporte de empresa
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">nombre</param>
        /// <param name="tipomodalidad">tipo modalidad</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoEmpresa(string finicios,
            string ffins,
            string tipoempresa,
            string nombre,
            string tipomodalidad)
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

            if (string.IsNullOrEmpty(tipoempresa)) tipoempresa = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(nombre)) nombre = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tipomodalidad)) tipomodalidad = ConstantesRegistroIntegrantes.ParametroDefecto;

            int nroRegistros = this.appReporte.ObtenerTotalRegListarEmpresas(fechaInicio, fechaFin, tipoempresa, nombre, tipomodalidad);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView("Paginado", model);
        }


        /// <summary>
        /// Permite pintar la lista de registros de empresas
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">nombre</param>
        /// <param name="tipomodalidad">tipo modalidad</param>
        /// <param name="nroPagina">nro de pagina</param>
        /// <returns></returns>
        public ActionResult ListadoEmpresas(string finicios,
            string ffins,
            string tipoempresa,
            string nombre,
            string tipomodalidad,
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

            if (string.IsNullOrEmpty(nombre)) nombre = ConstantesRegistroIntegrantes.ParametroDefecto;

            List<SiEmpresaDTO> reporte = this.appReporte.ListarEmpresas(fechaInicio, fechaFin, tipoempresa, nombre, tipomodalidad, nroPagina,
                Constantes.PageSize);

            model.ListaIntegrantes = reporte;

            return View(model);
        }

        /// <summary>
        /// Permite generar los datos del reporte de empresas para exportar
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">nombre</param>
        /// <param name="tipomodalidad">tipo modalidad</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteEmpresa(string finicios,
            string ffins,
            string tipoempresa,
            string nombre,
            string tipomodalidad)
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

                if (string.IsNullOrEmpty(nombre)) nombre = ConstantesRegistroIntegrantes.ParametroDefecto;

                List<SiEmpresaDTO> reporte = this.appReporte.ListarEmpresasFiltroXls(fechaInicio, fechaFin, tipoempresa, nombre, tipomodalidad);

                model.ListaIntegrantes = reporte;


                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
                string titulo = "REPORTE DE EMPRESAS";
                ExcelDocumentEmpresa.GernerarArchivoEnvios(reporte, ruta, titulo);
                indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar los datos del reporte de empresas
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarExcelEmpresa()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesRegistroIntegrantes.NombreReporteEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        #region Reporte Evolucion Empresas

        /// <summary>
        /// Reporte evolución de empresas
        /// </summary>
        /// <returns></returns>
        public ActionResult EvolucionEmpresa()
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

            model.ListaTipoEmpresa = appRegistroIntegrante.ListTipoEmpresa();
            model.ListaTipoEmpresa.Add(new SiTipoempresaDTO()
            {
                Tipoemprcodi = ConstantesRegistroIntegrantes.TodosCodigoEntero,
                Tipoemprabrev = ConstantesRegistroIntegrantes.Todos,
                Tipoemprdesc = ConstantesRegistroIntegrantes.Todos
            });


            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado del reporte de evolucion de empresa
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>        
        /// <param name="tiposolicitud">tipo de solicitud</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoEvolucionEmpresa(string finicios,
            string ffins,
            string tipoempresa,
            string tiposolicitud)
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

            if (string.IsNullOrEmpty(tipoempresa)) tipoempresa = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = ConstantesRegistroIntegrantes.ParametroDefecto;


            int nroRegistros = this.appReporte.ObtenerTotalRegListarEvolucionEmpresas(fechaInicio, fechaFin, tipoempresa, tiposolicitud);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView("Paginado", model);
        }

        /// <summary>
        /// Permite pintar la lista de registros del reporte de evolucion de empresa
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>        
        /// <param name="tiposolicitud">tipo de solicitud</param>
        /// <param name="nroPagina">numero de pagina</param>
        /// <returns></returns>
        public ActionResult ListadoEvolucionEmpresas(string finicios,
            string ffins,
            string tipoempresa,
            string tiposolicitud,
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

            if (string.IsNullOrEmpty(tipoempresa)) tipoempresa = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = ConstantesRegistroIntegrantes.ParametroDefecto;

            List<SiEmpresaDTO> reporte = this.appReporte.ListarEvolucionEmpresas(fechaInicio, fechaFin, tipoempresa, tiposolicitud, nroPagina,
                Constantes.PageSize);

            model.ListaIntegrantes = reporte;

            return View(model);
        }


        /// <summary>
        /// Permite generar los datos del reporte de evolucion de empresas para exportar
        /// </summary>
        /// <param name="finicios">fecha de inicio</param>
        /// <param name="ffins">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>       
        /// <param name="tiposolicitud">tipo solicitud</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteEvolucionEmpresa(string finicios,
            string ffins,
            string tipoempresa,
            string tiposolicitud)
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

                if (string.IsNullOrEmpty(tipoempresa)) tipoempresa = ConstantesRegistroIntegrantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposolicitud)) tiposolicitud = ConstantesRegistroIntegrantes.ParametroDefecto;

                List<SiEmpresaDTO> reporte = this.appReporte.ListarEvolucionEmpresasFiltroXls(fechaInicio, fechaFin, tipoempresa, tiposolicitud);

                model.ListaIntegrantes = reporte;


                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
                string titulo = "REPORTE DE EVOLUCION DE EMPRESAS";
                ExcelDocumentEvolucionEmpresa.GernerarArchivoEnvios(reporte, ruta, titulo);
                indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                indicador = -1;
            }
            return Json(indicador);
        }


        /// <summary>
        /// Permite exportar los datos del reporte de evolucion de empresas
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarExcelEvolucionEmpresa()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesRegistroIntegrantes.NombreReporteEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }


        #endregion

        #region Reporte Representantes Legales

        /// <summary>
        /// Reporte representante legal
        /// </summary>
        /// <returns></returns>
        public ActionResult RepresentantesLegales()
        {
            RegistroIntegrantesAppServicio appRegistroIntegrante = new RegistroIntegrantesAppServicio();

            ReporteModel model = new ReporteModel();
            model.ListaTipoEmpresa = appRegistroIntegrante.ListTipoEmpresa();
            model.ListaTipoEmpresa.Add(new SiTipoempresaDTO()
            {
                Tipoemprcodi = ConstantesRegistroIntegrantes.TodosCodigoEntero,
                Tipoemprabrev = ConstantesRegistroIntegrantes.Todos,
                Tipoemprdesc = ConstantesRegistroIntegrantes.Todos
            });
            //Tipo Representante
            model.ListaTipoRepresentante.Add(new TipoTitularAlternoModel(ConstantesRegistroIntegrantes.TodosCodigoString, ConstantesRegistroIntegrantes.Todos));
            model.ListaTipoRepresentante.Add(new TipoTitularAlternoModel(ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular, ConstantesRegistroIntegrantes.RepresentanteLegalTitular));
            model.ListaTipoRepresentante.Add(new TipoTitularAlternoModel(ConstantesRegistroIntegrantes.RepresentanteLegalTipoAlterno, ConstantesRegistroIntegrantes.RepresentanteLegalAlterno));


            //Estado
            //model.ListaEstado.Add(new EstadoModel() { Codigo = ConstantesRegistroIntegrantes.TodosCodigoString, Nombre = ConstantesRegistroIntegrantes.Todos });
            //model.ListaEstado.Add(new EstadoModel() { Codigo = ConstantesRegistroIntegrantes.RepresentanteBajaSi, Nombre = ConstantesRegistroIntegrantes.Si });
            model.ListaEstado.Add(new EstadoModel() { Codigo = ConstantesRegistroIntegrantes.RepresentanteBajaNo, Nombre = ConstantesRegistroIntegrantes.No });


            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado del reporte representantes legales
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="tiporepresentante">tipo de representante</param>
        /// <param name="tiporepresentantecontacto">tipo de representante, contacto</param>
        /// <param name="estado">estado</param>
        /// <param name="fecha">fecha Vigencia Poder</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoRepresentanteLegal(string tipoempresa,
            string nombre,
            string tiporepresentante,
            string tiporepresentantecontacto,
            string estado,
            string fecha)
        {
            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            if (string.IsNullOrEmpty(tipoempresa)) tipoempresa = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(nombre)) nombre = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiporepresentante)) tiporepresentante = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiporepresentantecontacto)) tiporepresentantecontacto = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(estado)) estado = ConstantesRegistroIntegrantes.ParametroDefecto;

            DateTime fechaVigenviaPoder = DateTime.MaxValue;
            if (fecha != "")
                fechaVigenviaPoder = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int nroRegistros = this.appReporte.ObtenerTotalRegListarRepresentanteLegal(tipoempresa, nombre, tiporepresentante, tiporepresentantecontacto, estado, fechaVigenviaPoder);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView("Paginado", model);
        }


        /// <summary>
        /// Permite pintar la lista de registros del reporte de representante legal
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="tiporepresentante">tipo de representante</param>
        /// <param name="tiporepresentantecontacto">tipo de representante, contacto</param>
        /// <param name="nroPagina">numero de pagina</param>
        /// <returns></returns>
        public ActionResult ListadoRepresentantesLegales(string tipoempresa,
            string nombre,
            string tiporepresentante,
            string tiporepresentantecontacto,
            string estado,
            string fecha,
            int nroPagina)
        {
            ReporteModel model = new ReporteModel();

            if (string.IsNullOrEmpty(tipoempresa)) tipoempresa = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(nombre)) nombre = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiporepresentante)) tiporepresentante = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiporepresentantecontacto)) tiporepresentantecontacto = ConstantesRegistroIntegrantes.ParametroDefecto;
            if (string.IsNullOrEmpty(estado)) estado = ConstantesRegistroIntegrantes.ParametroDefecto;


            DateTime fechaVigenviaPoder = DateTime.MaxValue;
            if (fecha != "")
                fechaVigenviaPoder = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<SiEmpresaDTO> reporte = this.appReporte.ListarRepresentanteLegal(tipoempresa, nombre, tiporepresentante, tiporepresentantecontacto, estado, fechaVigenviaPoder, nroPagina,
                Constantes.PageSize);

            foreach (var item in reporte)
            {
                switch (item.RpteTipo)
                {
                    case ConstantesRegistroIntegrantes.RepresentanteTipoLegal:
                        item.RpteTipo = ConstantesRegistroIntegrantes.RpteTipoLegalDescripcion;
                        break;
                    case ConstantesRegistroIntegrantes.RepresentanteTipoResponsableTramite:
                        item.RpteTipo = ConstantesRegistroIntegrantes.RpteTipoResponsableDescripcion;
                        break;
                    case ConstantesRegistroIntegrantes.RepresentanteTipoContacto:
                        item.RpteTipo = ConstantesRegistroIntegrantes.RpteTipoContactoDescripcion;
                        break;
                }

                switch (item.RpteTipRepresentanteLegal)
                {
                    case ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular:
                        item.RpteTipRepresentanteLegal = ConstantesRegistroIntegrantes.RepresentanteLegalTitular;
                        break;
                    case ConstantesRegistroIntegrantes.RepresentanteLegalTipoAlterno:
                        item.RpteTipRepresentanteLegal = ConstantesRegistroIntegrantes.RepresentanteLegalAlterno;
                        break;
                }
            }

            model.ListaIntegrantes = reporte;

            return View(model);
        }

        /// <summary>
        /// Permite pintar la lista de registros del reporte de representante legal para exportar
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="tiporepresentante">tipo de representante</param>
        /// <param name="tiporepresentantecontacto">tipo de representante, contacto</param>        
        /// <returns></returns>
        public JsonResult GenerarArchivoReporteRepresentantesLegales(string tipoempresa,
            string nombre,
            string tiporepresentante,
            string tiporepresentantecontacto,
            string estado,
            string fecha)
        {
            int indicador = 1;
            try
            {

                ReporteModel model = new ReporteModel();

                if (string.IsNullOrEmpty(tipoempresa)) tipoempresa = ConstantesRegistroIntegrantes.ParametroDefecto;
                if (string.IsNullOrEmpty(nombre)) nombre = ConstantesRegistroIntegrantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiporepresentante)) tiporepresentante = ConstantesRegistroIntegrantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiporepresentantecontacto)) tiporepresentantecontacto = ConstantesRegistroIntegrantes.ParametroDefecto;
                if (string.IsNullOrEmpty(estado)) estado = ConstantesRegistroIntegrantes.ParametroDefecto;

                DateTime fechaVigenviaPoder = DateTime.MaxValue;
                if (fecha != "")
                    fechaVigenviaPoder = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<SiEmpresaDTO> reporte = this.appReporte.ListarRepresentanteLegalFiltroXls(tipoempresa, nombre, tiporepresentante, tiporepresentantecontacto, estado, fechaVigenviaPoder);

                foreach (var item in reporte)
                {
                    switch (item.RpteTipo)
                    {
                        case ConstantesRegistroIntegrantes.RepresentanteTipoLegal:
                            item.RpteTipo = ConstantesRegistroIntegrantes.RpteTipoLegalDescripcion;
                            break;
                        case ConstantesRegistroIntegrantes.RepresentanteTipoResponsableTramite:
                            item.RpteTipo = ConstantesRegistroIntegrantes.RpteTipoResponsableDescripcion;
                            break;
                        case ConstantesRegistroIntegrantes.RepresentanteTipoContacto:
                            item.RpteTipo = ConstantesRegistroIntegrantes.RpteTipoContactoDescripcion;
                            break;
                    }

                    switch (item.RpteTipRepresentanteLegal)
                    {
                        case ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular:
                            item.RpteTipRepresentanteLegal = ConstantesRegistroIntegrantes.RepresentanteLegalTitular;
                            break;
                        case ConstantesRegistroIntegrantes.RepresentanteLegalTipoAlterno:
                            item.RpteTipRepresentanteLegal = ConstantesRegistroIntegrantes.RepresentanteLegalAlterno;
                            break;
                    }
                }

                model.ListaIntegrantes = reporte;


                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
                string titulo = "REPORTE DE REPRESENTANTES LEGALES";
                ExcelDocumentRepresentantesLegales.GernerarArchivoEnvios(reporte, ruta, titulo);
                indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                indicador = -1;
            }
            return Json(indicador);
        }

        [HttpPost]
        public JsonResult AlertaVigenciaVencida()
        {


            return Json(1);
        }

        [HttpPost]
        public JsonResult CorreoInformeRpteLegal()
        {


            return Json(1);
        }

        /// <summary>
        /// Permite exportar los datos del reporte de Representación Legal
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarExcelRepresentanteLegal()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesRegistroIntegrantes.NombreReporteEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesRegistroIntegrantes.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        [HttpPost]
        public JsonResult ExportarTextoRepresentanteLegal()
        {


            return Json(1);
        }

        [HttpPost]
        public JsonResult ExportarWordRepresentanteLegal()
        {


            return Json(1);
        }

        #endregion

    }
}
