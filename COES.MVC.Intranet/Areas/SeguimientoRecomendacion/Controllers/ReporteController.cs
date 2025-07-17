using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Models;
using COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Helper;
using COES.Servicios.Aplicacion.Recomendacion;
using COES.Servicios.Aplicacion.Auditoria;
using log4net;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Controllers
{
    public class ReporteController : BaseController
    {
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
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicio
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionFechaInicioRepAseg] != null) ?
                    (DateTime?)(Session[ConstanteSeguimientoRecomendacion.SesionFechaInicioRepAseg]) : null;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionFechaInicioRepAseg] = value;
            }
        }

        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFin
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionFechaFinRepAseg] != null) ?
                  (DateTime?)(Session[ConstanteSeguimientoRecomendacion.SesionFechaFinRepAseg]) : null;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionFechaFinRepAseg] = value;
            }
        }
        

        /// <summary>
        /// Cambio familia de la consulta
        /// </summary>
        public int Familia
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionFamiliaRepAseg] != null) ?
                  (int)(Session[ConstanteSeguimientoRecomendacion.SesionFamiliaRepAseg]) : 0;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionFamiliaRepAseg] = value;
            }
        }


        /// <summary>
        /// Cambio tipo empresa de la consulta
        /// </summary>
        public int TipoEmpresa
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionTipoEmpresaRepAseg] != null) ?
                  (int)(Session[ConstanteSeguimientoRecomendacion.SesionTipoEmpresaRepAseg]) : 0;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionTipoEmpresaRepAseg] = value;
            }
        }


        /// <summary>
        /// Cambio empresa de la consulta
        /// </summary>
        public int Empresa
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionEmpresaRepAseg] != null) ?
                  (int)(Session[ConstanteSeguimientoRecomendacion.SesionEmpresaRepAseg]) : 0;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionEmpresaRepAseg] = value;
            }
        }

        
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        EventoAppServicio servEvento = new EventoAppServicio();        
        SeguimientoRecomendacionAppServicio servSegRecomend = new SeguimientoRecomendacionAppServicio();
        AuditoriaAppServicio servAuditoria = new AuditoriaAppServicio();


        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? ejecutivo)
        {
            BusquedaReporteModel model = new BusquedaReporteModel();
            model.ListaTipoEvento = this.servEvento.ListarTipoEvento();
            model.ListaEmpresas = this.servEvento.ListarEmpresas();
            model.ListaFamilias = this.servEvento.ListarFamilias();
            model.ListaTipoEmpresas = this.servEvento.ListarTipoEmpresas();
            model.ListaCausaEvento = this.servEvento.ListarCausasEventos();
            model.FechaInicio = (this.FechaInicio != null) ? ((DateTime)this.FechaInicio).ToString(Constantes.FormatoFecha) :
                DateTime.Now.AddDays(ConstanteSeguimientoRecomendacion.DesfaseFecha).ToString(Constantes.FormatoFecha);
            
            model.FechaFin = (this.FechaFin != null) ? ((DateTime)this.FechaFin).ToString(Constantes.FormatoFecha) :
                DateTime.Now.ToString(Constantes.FormatoFecha);

            model.IdFamilia = this.Familia;
            model.IdTipoEmpresa = this.TipoEmpresa;
            model.IdEmpresa = this.Empresa;

            model.ListaFwUser = servAuditoria.ListUserRol(ConstanteSeguimientoRecomendacion.RolAseguramiento);

            model.ParametroDefecto = 0;
            model.AccionNuevo = true;

            model.ListaCriticidad = servSegRecomend.ListSrmCriticidads();
            model.ListaEstado = servSegRecomend.ListSrmEstados();

            model.AccionConsultar = ejecutivo == null ? false : ((int)ejecutivo == 1);//base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);


            return View(model);
        }


        /// <summary>
        /// Permite mostrar la lista de reporte
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(BusquedaReporteModel modelo)
        {


            if (modelo.EquiAbrev == null)
            {
                modelo.EquiAbrev = "x1x2x3";
            }

            if (modelo.Recomendacion == null)
            {
                modelo.Recomendacion = "x1x2x3";
            }

            ReporteModel model = new ReporteModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (modelo.FechaInicio != null)
            {
                fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (modelo.FechaFin != null)
            {
                fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFin;


            this.Familia = modelo.IdFamilia;
            this.TipoEmpresa = modelo.IdTipoEmpresa;
            this.Empresa = modelo.IdEmpresa;


            model.ListaRecomendacion = servSegRecomend.BuscarOperacionesReporteListado(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                     modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable, modelo.NroPagina, Constantes.PageSizeEvento).ToList();



            return PartialView(model);

        }


        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(BusquedaReporteModel modelo)
        {
            BusquedaReporteModel model = new BusquedaReporteModel();
            model.IndicadorPagina = false;


            if (modelo.EquiAbrev == null)
            {
                modelo.EquiAbrev = "x1x2x3";
            }

            if (modelo.Recomendacion == null)
            {
                modelo.Recomendacion = "x1x2x3";
            }

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (modelo.FechaInicio != null)
            {
                fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (modelo.FechaFin != null)
            {
                fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFin;


            this.Familia = modelo.IdFamilia;
            this.TipoEmpresa = modelo.IdTipoEmpresa;
            this.Empresa = modelo.IdEmpresa;

            
            int nroRegistros = 1;


            nroRegistros = servSegRecomend.ObtenerNroFilas(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                     modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable);

                      

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }
        

        /// <summary>
        /// Permite cargar los puntos de la empresa seleccionada
        /// </summary>
        /// <param name="idTipoEmpresa">id de tipo de empresa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(int idTipoEmpresa)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();

            if (idTipoEmpresa != 0)
            {
                entitys = this.servEvento.ListarEmpresasPorTipo(idTipoEmpresa);
            }
            else
            {
                entitys = this.servEvento.ListarEmpresas();
            }

            SelectList list = new SelectList(entitys, EntidadPropiedad.EmprCodi, EntidadPropiedad.EmprNomb);

            return Json(list);
        }


        /// <summary>
        /// Permite generar el reporte eventos
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporte(BusquedaReporteModel modelo)
        {
            int result = 1;
            try
            {

                if (modelo.EquiAbrev == null)
                {
                    modelo.EquiAbrev = "x1x2x3";
                }

                if (modelo.Recomendacion == null)
                {
                    modelo.Recomendacion = "x1x2x3";
                }
                
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (modelo.FechaInicio != null)
                {
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (modelo.FechaFin != null)
                {
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;


                this.Familia = modelo.IdFamilia;
                this.TipoEmpresa = modelo.IdTipoEmpresa;
                this.Empresa = modelo.IdEmpresa;


                List<SrmRecomendacionDTO> list = servSegRecomend.BuscarOperacionesReporteListado(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable, modelo.NroPagina, Constantes.PageSizeEvento).ToList();



                ExcelDocument.GenerarReporteSeguimientoListado(list, fechaInicio, fechaFin, modelo.Indicador);

                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }


        /// <summary>
        /// Permite exportar los datos del evento
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarReporte()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteSeguimientoRecReporte;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteSeguimientoRecReporte);
        }


        /// <summary>
        /// Permite obtener los registros de empresa por criticidad
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>        
        public JsonResult ListadoEmpresaCriticidad(BusquedaReporteModel modelo)
        {
            BusquedaReporteModel brm = new BusquedaReporteModel();

            var list = new List<SrmRecomendacionDTO>();
            try
            {

                if (modelo.EquiAbrev == null)
                {
                    modelo.EquiAbrev = "x1x2x3";
                }

                if (modelo.Recomendacion == null)
                {
                    modelo.Recomendacion = "x1x2x3";
                }

                ReporteModel model = new ReporteModel();

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (modelo.FechaInicio != null)
                {
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (modelo.FechaFin != null)
                {
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;


                this.Familia = modelo.IdFamilia;
                this.TipoEmpresa = modelo.IdTipoEmpresa;
                this.Empresa = modelo.IdEmpresa;

                
                list = servSegRecomend.BuscarOperacionesEmpresaCriticidad(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable).ToList();
                
                brm.listaRecomendacion = list;
                brm.ListaCriticidad = servSegRecomend.ListSrmCriticidads();
            }
            catch
            {

            }

            return Json(brm);
        }


        /// <summary>
        /// Permite obtener los registros de empresa por estado
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>        
        public JsonResult ListadoEmpresaEstado(BusquedaReporteModel modelo)
        {
            BusquedaReporteModel brm = new BusquedaReporteModel();

            var list = new List<SrmRecomendacionDTO>();
            try
            {

                if (modelo.EquiAbrev == null)
                {
                    modelo.EquiAbrev = "x1x2x3";
                }

                if (modelo.Recomendacion == null)
                {
                    modelo.Recomendacion = "x1x2x3";
                }

                ReporteModel model = new ReporteModel();

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (modelo.FechaInicio != null)
                {
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (modelo.FechaFin != null)
                {
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;


                this.Familia = modelo.IdFamilia;
                this.TipoEmpresa = modelo.IdTipoEmpresa;
                this.Empresa = modelo.IdEmpresa;


                list = servSegRecomend.BuscarOperacionesEmpresaEstado(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable).ToList();
                
                brm.listaRecomendacion = list;
                brm.ListaEstado = servSegRecomend.ListSrmEstados();
            }
            catch
            {

            }

            return Json(brm);
        }


        /// <summary>
        /// Permite obtener los registros de tipo de equipo por criticidad
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>        
        public JsonResult ListadoTipoEquipoCriticidad(BusquedaReporteModel modelo)
        {
            BusquedaReporteModel brm = new BusquedaReporteModel();

            var list = new List<SrmRecomendacionDTO>();
            try
            {

                if (modelo.EquiAbrev == null)
                {
                    modelo.EquiAbrev = "x1x2x3";
                }

                if (modelo.Recomendacion == null)
                {
                    modelo.Recomendacion = "x1x2x3";
                }

                ReporteModel model = new ReporteModel();

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (modelo.FechaInicio != null)
                {
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (modelo.FechaFin != null)
                {
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;


                this.Familia = modelo.IdFamilia;
                this.TipoEmpresa = modelo.IdTipoEmpresa;
                this.Empresa = modelo.IdEmpresa;


                list = servSegRecomend.BuscarOperacionesTipoEquipoCriticidad(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable).ToList();

                brm.listaRecomendacion = list;
                brm.ListaCriticidad = servSegRecomend.ListSrmCriticidads();
            }
            catch
            {

            }

            return Json(brm);
        }


        /// <summary>
        /// Permite obtener los registros de tipo de equipo por estado
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>        
        public JsonResult ListadoTipoEquipoEstado(BusquedaReporteModel modelo)
        {
            BusquedaReporteModel brm = new BusquedaReporteModel();

            var list = new List<SrmRecomendacionDTO>();
            try
            {

                if (modelo.EquiAbrev == null)
                {
                    modelo.EquiAbrev = "x1x2x3";
                }

                if (modelo.Recomendacion == null)
                {
                    modelo.Recomendacion = "x1x2x3";
                }

                ReporteModel model = new ReporteModel();

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (modelo.FechaInicio != null)
                {
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (modelo.FechaFin != null)
                {
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;


                this.Familia = modelo.IdFamilia;
                this.TipoEmpresa = modelo.IdTipoEmpresa;
                this.Empresa = modelo.IdEmpresa;


                list = servSegRecomend.BuscarOperacionesTipoEquipoEstado(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable).ToList();

                brm.listaRecomendacion = list;
                brm.ListaEstado = servSegRecomend.ListSrmEstados();
            }
            catch
            {

            }

            return Json(brm);
        }


        /// <summary>
        /// Permite obtener los registros por estado
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>        
        public JsonResult ListadoEstado(BusquedaReporteModel modelo)
        {
            BusquedaReporteModel brm = new BusquedaReporteModel();

            var list = new List<SrmRecomendacionDTO>();
            try
            {

                if (modelo.EquiAbrev == null)
                {
                    modelo.EquiAbrev = "x1x2x3";
                }

                if (modelo.Recomendacion == null)
                {
                    modelo.Recomendacion = "x1x2x3";
                }

                ReporteModel model = new ReporteModel();

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (modelo.FechaInicio != null)
                {
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (modelo.FechaFin != null)
                {
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;


                this.Familia = modelo.IdFamilia;
                this.TipoEmpresa = modelo.IdTipoEmpresa;
                this.Empresa = modelo.IdEmpresa;


                list = servSegRecomend.BuscarOperacionesEstado(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable).ToList();


            }
            catch
            {

            }

            return Json(list);
        }
        
        
        /// <summary>
        /// Permite obtener los registros de estado y criticidad
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>        
        public JsonResult ListadoEstadoCriticidad(BusquedaReporteModel modelo)
        {
            BusquedaReporteModel brm = new BusquedaReporteModel();

            var list = new List<SrmRecomendacionDTO>();
            try
            {

                if (modelo.EquiAbrev == null)
                {
                    modelo.EquiAbrev = "x1x2x3";
                }

                if (modelo.Recomendacion == null)
                {
                    modelo.Recomendacion = "x1x2x3";
                }

                ReporteModel model = new ReporteModel();

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (modelo.FechaInicio != null)
                {
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (modelo.FechaFin != null)
                {
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;


                this.Familia = modelo.IdFamilia;
                this.TipoEmpresa = modelo.IdTipoEmpresa;
                this.Empresa = modelo.IdEmpresa;


                list = servSegRecomend.BuscarOperacionesEstadoCriticidad(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable).ToList();

                brm.listaRecomendacion = list;
                brm.ListaCriticidad = servSegRecomend.ListSrmCriticidads();
            }
            catch
            {

            }

            return Json(brm);
        }
        
        
        /// <summary>
        /// Permite obtener los registros por criticidad
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaReporte</param>
        /// <returns></returns>        
        public JsonResult ListadoCriticidad(BusquedaReporteModel modelo)
        {
            BusquedaReporteModel brm = new BusquedaReporteModel();

            var list = new List<SrmRecomendacionDTO>();
            try
            {

                if (modelo.EquiAbrev == null)
                {
                    modelo.EquiAbrev = "x1x2x3";
                }

                if (modelo.Recomendacion == null)
                {
                    modelo.Recomendacion = "x1x2x3";
                }

                ReporteModel model = new ReporteModel();

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (modelo.FechaInicio != null)
                {
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (modelo.FechaFin != null)
                {
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;


                this.Familia = modelo.IdFamilia;
                this.TipoEmpresa = modelo.IdTipoEmpresa;
                this.Empresa = modelo.IdEmpresa;


                list = servSegRecomend.BuscarOperacionesCriticidad(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.IdCriticidad, modelo.IdEstado, modelo.Recomendacion, modelo.IdResponsable).ToList();


            }
            catch
            {

            }

            return Json(list);
        }

        
    }
}