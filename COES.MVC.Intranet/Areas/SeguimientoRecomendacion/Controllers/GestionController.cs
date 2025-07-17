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
using COES.MVC.Intranet.Areas.Eventos.Helper;
using log4net;
using COES.Servicios.Aplicacion.Evento;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Controllers
{
    public class GestionController : BaseController
    {

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(GestionController));
        public GestionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("GestionController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("GestionController", ex);
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
                return (Session[ConstanteSeguimientoRecomendacion.SesionFechaInicioAseg] != null) ?
                    (DateTime?)(Session[ConstanteSeguimientoRecomendacion.SesionFechaInicioAseg]) : null;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionFechaInicioAseg] = value;
            }
        }

        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFin
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionFechaFinAseg] != null) ?
                  (DateTime?)(Session[ConstanteSeguimientoRecomendacion.SesionFechaFinAseg]) : null;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionFechaFinAseg] = value;
            }
        }


        /// <summary>
        /// Cambio familia de la consulta
        /// </summary>
        public int Familia
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionFamiliaAseg] != null) ?
                  (int)(Session[ConstanteSeguimientoRecomendacion.SesionFamiliaAseg]) : 0;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionFamiliaAseg] = value;
            }
        }


        /// <summary>
        /// Cambio tipo empresa de la consulta
        /// </summary>
        public int TipoEmpresa
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionTipoEmpresaAseg] != null) ?
                  (int)(Session[ConstanteSeguimientoRecomendacion.SesionTipoEmpresaAseg]) : 0;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionTipoEmpresaAseg] = value;
            }
        }


        /// <summary>
        /// Cambio empresa de la consulta
        /// </summary>
        public int Empresa
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionEmpresaAseg] != null) ?
                  (int)(Session[ConstanteSeguimientoRecomendacion.SesionEmpresaAseg]) : 0;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionEmpresaAseg] = value;
            }
        }


        /// <summary>
        /// Con Recomendacion
        /// </summary>
        public string ConRecomendacion
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionConRecomendacionAseg] != null) ?
                  (string)(Session[ConstanteSeguimientoRecomendacion.SesionConRecomendacionAseg]) : "S";
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionConRecomendacionAseg] = value;
            }
        }


        /// <summary>
        /// Detalle Recomendacion
        /// </summary>
        public string DetRecomendacion
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionDetRecomendacionAseg] != null) ?
                  (string)(Session[ConstanteSeguimientoRecomendacion.SesionDetRecomendacionAseg]) : "N";
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionDetRecomendacionAseg] = value;
            }
        }


        /// <summary>
        /// Cambio estado
        /// </summary>
        public int IdEstado
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionIdEstado] != null) ?
                  (int)(Session[ConstanteSeguimientoRecomendacion.SesionIdEstado]) : 0;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionIdEstado] = value;
            }
        }


        /// <summary>
        /// Cambio criticidad
        /// </summary>
        public int IdCriticidad
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionIdCriticidad] != null) ?
                  (int)(Session[ConstanteSeguimientoRecomendacion.SesionIdCriticidad]) : 0;
            }
            set
            {
                Session[ConstanteSeguimientoRecomendacion.SesionIdCriticidad] = value;
            }
        }


        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        EventoAppServicio servEvento = new EventoAppServicio();
        SeguimientoRecomendacionAppServicio servSegRecomend = new SeguimientoRecomendacionAppServicio();



        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BusquedaGestionModel model = new BusquedaGestionModel();
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


            model.ParametroDefecto = 0;
            model.AccionNuevo = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);

            model.ListaCriticidad = servSegRecomend.ListSrmCriticidads();
            model.ListaEstado = servSegRecomend.ListSrmEstados();
            model.IdConRecomendacion = ConRecomendacion;
            model.IdDetRecomendacion = DetRecomendacion;
            model.IdEstado = IdEstado;
            model.IdCriticidad = IdCriticidad;
            
            return View(model);
        }


        /// <summary>
        /// Permite mostrar la lista de gestión
        /// </summary>
        /// <param name="modelo">modelo tipo BúsquedaGestion</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(BusquedaGestionModel modelo)
        {

            if(modelo.IdConRecomendacion==null)
            {
                modelo.IdConRecomendacion = "S";
            }

            if(modelo.IdDetRecomendacion==null)
            {
                modelo.IdDetRecomendacion = "N";
            }

            //if (modelo.IdConRecomendacion == "T")
            {
                //modelo.IdDetRecomendacion = "S";
            }

            if(modelo.EquiAbrev==null)
            {
                modelo.EquiAbrev="x1x2x3";
            }

            ConRecomendacion = modelo.IdConRecomendacion;
            DetRecomendacion = modelo.IdDetRecomendacion;
            IdEstado = modelo.IdEstado;
            IdCriticidad = modelo.IdCriticidad;

            
            GestionModel model = new GestionModel();

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


            if (DetRecomendacion == "S")
            {
                fechaInicio = DateTime.Now.AddYears(-5);
                fechaFin = DateTime.Now.AddYears(5);                  
            }


            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFin;
            this.Familia = modelo.IdFamilia;
            this.TipoEmpresa = modelo.IdTipoEmpresa;
            this.Empresa = modelo.IdEmpresa;


            //reemplazar listado
            model.ListaRecomendacion = new List<SrmRecomendacionDTO>();
            List<SrmRecomendacionDTO> lstPreviaRec = new List<SrmRecomendacionDTO>();
            List <SrmRecomendacionDTO> listaCon = new List<SrmRecomendacionDTO>();
            List<SrmRecomendacionDTO> listaSin = new List<SrmRecomendacionDTO>();

            if (modelo.IdTipoEventoRec == 0 || modelo.IdTipoEventoRec == 2)
            {
                //Con recomendaciones ctaf
                lstPreviaRec = servSegRecomend.BuscarOperacionesCtaf(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                     modelo.NroPagina, Constantes.PageSizeRecomendacion).ToList();

            }


            if (modelo.IdConRecomendacion == "N" || modelo.IdConRecomendacion == "T" && (modelo.IdTipoEventoRec == 0 || modelo.IdTipoEventoRec == 1))
            {
                //sin recomendacion
                listaSin = servSegRecomend.BuscarOperaciones(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                     modelo.NroPagina, Constantes.PageSizeRecomendacion).ToList();
                lstPreviaRec.AddRange(listaSin);
            }

            if (modelo.IdConRecomendacion == "S" || modelo.IdConRecomendacion == "T" && (modelo.IdTipoEventoRec == 0 || modelo.IdTipoEventoRec == 1))
            {
                //con recomendacion
                if (modelo.IdDetRecomendacion == "N" || modelo.IdConRecomendacion == "T")
                {
                    modelo.IdEstado = 0;
                    modelo.IdCriticidad = 0;
                }                

                listaCon = servSegRecomend.BuscarOperaciones(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                     modelo.IdDetRecomendacion, modelo.IdEstado, modelo.IdCriticidad, modelo.NroPagina, Constantes.PageSizeRecomendacion).ToList();

                lstPreviaRec.AddRange(listaCon);
            }

            if (DetRecomendacion == "S")
            {
                lstPreviaRec = model.ListaRecomendacion.Where(x => x.Srmstdcodi != 3).ToList();
            }

            if(IdEstado != 0)
            {
                foreach (var lstRecomendacion in lstPreviaRec)
                {
                    if(lstRecomendacion.Srmstdcodi == IdEstado)
                        model.ListaRecomendacion.Add(lstRecomendacion);
                }
            }
            else
                model.ListaRecomendacion.AddRange(lstPreviaRec);

            model.AccionNuevo = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);

            
            return PartialView(model);
        }


        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaGestion</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(BusquedaGestionModel modelo)
        {
            BusquedaGestionModel model = new BusquedaGestionModel();
            model.IndicadorPagina = false;

            if (modelo.IdConRecomendacion == null)
            {
                modelo.IdConRecomendacion = "S";
            }

            if (modelo.IdDetRecomendacion == null)
            {
                modelo.IdDetRecomendacion = "N";
            }

            if (modelo.EquiAbrev == null)
            {
                modelo.EquiAbrev = "x1x2x3";
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

            if (DetRecomendacion == "S")
            {
                fechaInicio = DateTime.Now.AddYears(-5);
                fechaFin = DateTime.Now.AddYears(5);
            }

            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFin;
            this.Familia = modelo.IdFamilia;
            this.TipoEmpresa = modelo.IdTipoEmpresa;
            this.Empresa = modelo.IdEmpresa;

            #region Inicio aplicativo Seg. Recomendaciones

            int nroRegistros = 0;

            //if (modelo.IdConRecomendacion == "S")
            //{
            //    //con recomendacion
            //    if (modelo.IdDetRecomendacion == "N")
            //    {
            //        modelo.IdEstado = 0;
            //        modelo.IdCriticidad = 0;
            //    }

            //    nroRegistros += servSegRecomend.ObtenerNroFilas(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
            //         modelo.IdDetRecomendacion, modelo.IdEstado, modelo.IdCriticidad);

            //}
            //else
            //{
            //    //sin recomendacion
            //    nroRegistros += servSegRecomend.ObtenerNroFilas(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa);

            //}

            //reemplazar listado
            GestionModel modelog = new GestionModel();
            modelog.ListaRecomendacion = new List<SrmRecomendacionDTO>();
            List<SrmRecomendacionDTO> lstPreviaRec = new List<SrmRecomendacionDTO>();
            List<SrmRecomendacionDTO> listaCon = new List<SrmRecomendacionDTO>();
            List<SrmRecomendacionDTO> listaSin = new List<SrmRecomendacionDTO>();

            if (modelo.IdTipoEventoRec == 0 || modelo.IdTipoEventoRec == 2)
            {
                //Con recomendaciones ctaf
                lstPreviaRec = servSegRecomend.BuscarOperacionesCtaf(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                     modelo.NroPagina, Constantes.PageSizeEvento).ToList();
            }


            if (modelo.IdConRecomendacion == "N" || modelo.IdConRecomendacion == "T" && (modelo.IdTipoEventoRec == 0 || modelo.IdTipoEventoRec == 1))
            {
                //sin recomendacion
                listaSin = servSegRecomend.BuscarOperaciones(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                     modelo.NroPagina, Constantes.PageSizeEvento).ToList();
                lstPreviaRec.AddRange(listaSin);
            }

            if (modelo.IdConRecomendacion == "S" || modelo.IdConRecomendacion == "T" && (modelo.IdTipoEventoRec == 0 || modelo.IdTipoEventoRec == 1))
            {
                //con recomendacion
                if (modelo.IdDetRecomendacion == "N" || modelo.IdConRecomendacion == "T")
                {
                    modelo.IdEstado = 0;
                    modelo.IdCriticidad = 0;
                }

                listaCon = servSegRecomend.BuscarOperaciones(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                     modelo.IdDetRecomendacion, modelo.IdEstado, modelo.IdCriticidad, modelo.NroPagina, Constantes.PageSizeEvento).ToList();

                lstPreviaRec.AddRange(listaCon);                
            }

            nroRegistros += lstPreviaRec.Count;

            #endregion Fin aplicativo Seg. Recomendaciones


            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = 1000;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
                string[] cantidadRegistros = { "200"};

                model.CantidadRegistros = cantidadRegistros;
            }

            return PartialView(model);
        }


        /// <summary>
        /// Permite visualizar el detalle del evento
        /// </summary>
        /// <returns></returns>
        public ActionResult Detalle()
        {
            string codigo = Request[RequestParameter.EventoId];
            int id = int.Parse(codigo);

            GestionModel model = new GestionModel();
            EventoDTO evento = this.servEvento.ObtenerEvento(id);
            model.ListaTipoEvento = this.servEvento.ListarTipoEvento().ToList();
            model.ListaEmpresas = this.servEvento.ListarEmpresas().ToList();
            model.ListaCausaEvento = this.servEvento.ObtenerCausaEvento(evento.TIPOEVENCODI).ToList();
            model.Evento = evento;
            model.IdEvento = id;
            model.ListaInterrupciones = (new DetalleEventoAppServicio()).GetByCriteriaEveInterrupcions(id);
            model.ListaSubEventos = (new DetalleEventoAppServicio()).GetByCriteriaEveSubeventoss(id);

            return View(model);
        }


        /// <summary>
        /// Permite abrir el archivo generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Exportar()
        {
            string tipo = Request[RequestParameter.Indicador];
            string file = string.Empty;
            string app = string.Empty;

            if (tipo == Constantes.FormatoWord)
            {
                file = Constantes.NombreReportePerturbacionWord;
                app = Constantes.AppWord;
            }
            if (tipo == Constantes.FormatoPDF)
            {
                file = Constantes.NombreReportePerturbacionPdf;
                app = Constantes.AppPdf;
            }

            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + file;
            return File(fullPath, app, file);
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
        /// Permite generar el reporte de seguimiento
        /// </summary>
        /// <param name="modelo">modelo tipo BusquedaGestion</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarSeguimiento(BusquedaGestionModel modelo)
        {

            int result = 1;
            try
            {
                if (modelo.IdConRecomendacion == null)
                {
                    modelo.IdConRecomendacion = "S";
                }

                if (modelo.IdDetRecomendacion == null)
                {
                    modelo.IdDetRecomendacion = "N";
                }

                if (modelo.EquiAbrev == null)
                {
                    modelo.EquiAbrev = "x1x2x3";
                }

                GestionModel model = new GestionModel();
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

                if (DetRecomendacion == "S")
                {
                    fechaInicio = DateTime.Now.AddYears(-5);
                    fechaFin = DateTime.Now.AddYears(5);
                }

                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;
                this.Familia = modelo.IdFamilia;
                this.TipoEmpresa = modelo.IdTipoEmpresa;
                this.Empresa = modelo.IdEmpresa;

                //reemplazar listado
                model.ListaRecomendacion = new List<SrmRecomendacionDTO>();
                List<SrmRecomendacionDTO> lstPreviaRec = new List<SrmRecomendacionDTO>();
                List<SrmRecomendacionDTO> listaCon = new List<SrmRecomendacionDTO>();
                List<SrmRecomendacionDTO> listaSin = new List<SrmRecomendacionDTO>();

                if (modelo.IdTipoEventoRec == 0 || modelo.IdTipoEventoRec == 2)
                {
                    //Con recomendaciones ctaf
                    lstPreviaRec = servSegRecomend.BuscarOperacionesCtaf(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.NroPagina, Constantes.PageSizeRecomendacion).ToList();
                }

                if (modelo.IdConRecomendacion == "N" || modelo.IdConRecomendacion == "T" && (modelo.IdTipoEventoRec == 0 || modelo.IdTipoEventoRec == 1))
                {
                    if (modelo.IdConRecomendacion == "N")
                    {
                        modelo.Indicador = 0;
                    }
                    //sin recomendacion
                    listaSin = servSegRecomend.BuscarOperaciones(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.NroPagina, Constantes.PageSizeRecomendacion).ToList();
                    lstPreviaRec.AddRange(listaSin);
                }

                if (modelo.IdConRecomendacion == "S" || modelo.IdConRecomendacion == "T" && (modelo.IdTipoEventoRec == 0 || modelo.IdTipoEventoRec == 1))
                {
                    //con recomendacion
                    modelo.Indicador = 1;
                    if (modelo.IdDetRecomendacion == "N")
                    {
                        modelo.IdEstado = 0;
                        modelo.IdCriticidad = 0;
                    }

                    listaCon = servSegRecomend.BuscarOperaciones(fechaInicio, fechaFin, modelo.IdFamilia, modelo.EquiAbrev, modelo.IdTipoEmpresa, modelo.IdEmpresa,
                         modelo.IdDetRecomendacion, modelo.IdEstado, modelo.IdCriticidad, modelo.NroPagina, Constantes.PageSizeRecomendacion).ToList();

                    lstPreviaRec.AddRange(listaCon);
                }

                if (DetRecomendacion == "S")
                {
                    lstPreviaRec = model.ListaRecomendacion.Where(x => x.Srmstdcodi != 3).ToList();
                }

                if (IdEstado != 0)
                {
                    foreach (var lstRecomendacion in lstPreviaRec)
                    {
                        if (lstRecomendacion.Srmstdcodi == IdEstado)
                            model.ListaRecomendacion.Add(lstRecomendacion);
                    }
                }
                else
                    model.ListaRecomendacion.AddRange(lstPreviaRec);

                ExcelDocument.GenerarReporteSeguimiento(model.ListaRecomendacion, fechaInicio, fechaFin, modelo.Indicador);

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
        public virtual ActionResult DescargarSeguimiento()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteSeguimientoRec;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteSeguimientoRec);
        }

        [HttpPost]
        public JsonResult EliminarEvento(int id)
        {
            try
            {
                EveEventoDTO evento = (new EventosAppServicio()).GetByIdEveEvento((int)id);

                if (evento.Evenpreliminar == "N")
                {
                    (new EventosAppServicio()).ActualizarEventoAseguramiento(id);
                }
                if (evento.Evenpreliminar == "A")
                {
                    (new EventosAppServicio()).DeleteEveEvento(id, User.Identity.Name);
                }

                return Json(1);
            }
            catch 
            {
                return Json(-11);
            }
        }


    }
}