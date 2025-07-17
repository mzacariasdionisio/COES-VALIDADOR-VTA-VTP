using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.Eventos.Helper;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.MVC.Intranet.Helper;
using Novacode;
using COES.Servicios.Aplicacion.Eventos;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Evento.Helper;
using COES.Servicios.Aplicacion.Informe;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Controllers;
using System.Web.Script.Serialization;
using COES.Framework.Base.Tools;
using System.Text.RegularExpressions;
using System.Reflection;
using log4net;
using static COES.Servicios.Aplicacion.Migraciones.Helper.ConstantesMigraciones;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class EventoController : BaseController
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private readonly CriteriosEventoAppServicio serviciosCriteriosEvento = new CriteriosEventoAppServicio();
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

        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicio
        {
            get
            {
                return (Session[ConstantesEventos.SesionFechaInicio] != null) ?
                    (DateTime?)(Session[ConstantesEventos.SesionFechaInicio]) : null;
            }
            set
            {
                Session[ConstantesEventos.SesionFechaInicio] = value;
            }
        }

        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFin
        {
            get
            {
                return (Session[ConstantesEventos.SesionFechaFin] != null) ?
                  (DateTime?)(Session[ConstantesEventos.SesionFechaFin]) : null;
            }
            set
            {
                Session[ConstantesEventos.SesionFechaFin] = value;
            }
        }

        /// <summary>
        /// Campo version de la consulta
        /// </summary>
        public string Version
        {
            get
            {
                return (Session[ConstantesEventos.SesionVersion] != null) ?
                    Session[ConstantesEventos.SesionVersion].ToString() : (-1).ToString();
            }
            set
            {
                Session[ConstantesEventos.SesionVersion] = value;
            }
        }

        /// <summary>
        /// Campo turno de la consulta
        /// </summary>
        public string Turno
        {
            get
            {
                return (Session[ConstantesEventos.SesionTurno] != null) ?
                    Session[ConstantesEventos.SesionTurno].ToString() : string.Empty;
            }
            set
            {
                Session[ConstantesEventos.SesionTurno] = value;
            }
        }

        /// <summary>
        /// Cambio familia de la consulta
        /// </summary>
        public int Familia
        {
            get
            {
                return (Session[ConstantesEventos.SesionFamilia] != null) ?
                  (int)(Session[ConstantesEventos.SesionFamilia]) : 0;
            }
            set
            {
                Session[ConstantesEventos.SesionFamilia] = value;
            }
        }

        /// <summary>
        /// Cambio tipo empresa de la consulta
        /// </summary>
        public int TipoEmpresa
        {
            get
            {
                return (Session[ConstantesEventos.SesionTipoEmpresa] != null) ?
                  (int)(Session[ConstantesEventos.SesionTipoEmpresa]) : 0;
            }
            set
            {
                Session[ConstantesEventos.SesionTipoEmpresa] = value;
            }
        }

        /// <summary>
        /// Cambio empresa de la consulta
        /// </summary>
        public int Empresa
        {
            get
            {
                return (Session[ConstantesEventos.SesionEmpresa] != null) ?
                  (int)(Session[ConstantesEventos.SesionEmpresa]) : 0;
            }
            set
            {
                Session[ConstantesEventos.SesionEmpresa] = value;
            }
        }

        /// <summary>
        /// Campo interrupcion de la consulta
        /// </summary>
        public string Interrupcion
        {
            get
            {
                return (Session[ConstantesEventos.SesionInterrupcion] != null) ?
                    Session[ConstantesEventos.SesionInterrupcion].ToString() : (-1).ToString();
            }
            set
            {
                Session[ConstantesEventos.SesionInterrupcion] = value;
            }
        }

        /// <summary>
        /// Campo tipo evento de la consulta
        /// </summary>
        public int TipoEvento
        {
            get
            {
                return (Session[ConstantesEventos.SesionTipoEvento] != null) ?
                  (int)(Session[ConstantesEventos.SesionTipoEvento]) : 0;
            }
            set
            {
                Session[ConstantesEventos.SesionTipoEvento] = value;
            }
        }

        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        EventoAppServicio servicio = new EventoAppServicio();
        
        /// <summary>
        /// Maneja los datos de la pantalla de perturbacion
        /// </summary>
        public List<InformePerturbacionDTO> ListaItemReportePerturbacion
        {
            get
            {
                return (Session[DatosSesion.SesionPerturbacion] != null) ?
                    (List<InformePerturbacionDTO>)Session[DatosSesion.SesionPerturbacion] : new List<InformePerturbacionDTO>();
            }
            set { Session[DatosSesion.SesionPerturbacion] = value; }
        }

        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BusquedaEventoModel model = new BusquedaEventoModel();
            model.ListaTipoEvento = this.servicio.ListarTipoEvento();
            model.ListaEmpresas = this.servicio.ListarEmpresas();
            model.ListaFamilias = this.servicio.ListarFamilias();
            model.ListaTipoEmpresas = this.servicio.ListarTipoEmpresas();
            model.ListaCausaEvento = this.servicio.ListarCausasEventos();
            model.FechaInicio = (this.FechaInicio != null) ? ((DateTime)this.FechaInicio).ToString(Constantes.FormatoFecha) :
                DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = (this.FechaFin != null) ? ((DateTime)this.FechaFin).ToString(Constantes.FormatoFecha) :
                DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Version = this.Version;
            model.Turno = this.Turno;
            model.IdFamilia = this.Familia;
            model.IdTipoEmpresa = this.TipoEmpresa;
            model.IdEmpresa = this.Empresa;
            model.IndInterrupcion = this.Interrupcion;
            model.IdTipoEvento = this.TipoEvento;
            model.IdOpcion = (base.IdOpcion != null) ? (int)base.IdOpcion : 0;

            model.ParametroDefecto = 0;
            model.AccionNuevo = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                base.UserName, Acciones.Nuevo);

            model.IndicadorGrabarAseg = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], base.UserName, Acciones.PermisoSEVAseg);

            model.AccionEventoCtaf = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], base.UserName, Acciones.PermisoSEV);
            return View(model);
        }

        /// <summary>
        /// Permite mostrar la lista de eventos
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(BusquedaEventoModel modelo)
        {

            Log.Info(NameController + " - PARTE 1 - " + DateTime.Now, null);
            EventoModel model = new EventoModel();

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

            Log.Info(NameController + " - PARTE 2 - " + DateTime.Now, null);

            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFin;

            this.Version = modelo.Version;
            this.Turno = modelo.Turno;
            this.Familia = modelo.IdFamilia;
            this.TipoEmpresa = modelo.IdTipoEmpresa;
            this.Empresa = modelo.IdEmpresa;
            this.Interrupcion = modelo.IndInterrupcion;
            this.TipoEvento = modelo.IdTipoEvento;

            modelo.AreaOperativa = "-1";

            fechaFin = fechaFin.AddDays(1);

            int todosaseg = -1;
            if (modelo.Version == "-1")
            {
                //acceso del usuario de aseguramiento de la operación
                bool accesoAseguramiento = (new EventoHelper()).VerificarAcceso(modelo.IdOpcion, base.UserName, Acciones.PermisoSEVAseg);
                if (accesoAseguramiento)
                {
                    todosaseg = 1;
                }
            }

            Log.Info(NameController + " - PARTE 3 - " + DateTime.Now, null);

            model.ListaEventos = servicio.BuscarEventos(modelo.IdTipoEvento, fechaInicio, fechaFin, modelo.Version, modelo.Turno,
                modelo.IdTipoEmpresa, modelo.IdEmpresa, modelo.IdFamilia, modelo.IndInterrupcion, modelo.NroPagina,
                Constantes.PageSizeEvento, modelo.CampoOrden, modelo.TipoOrden, modelo.AreaOperativa, todosaseg).ToList();

            Log.Info(NameController + " - PARTE 4 - " + DateTime.Now, null);

            model.AccionEliminar = (new EventoHelper()).VerificarAcceso(modelo.IdOpcion,
                base.UserName, Acciones.Eliminar);

            model.AccionVerInforme = (new EventoHelper()).VerificarAcceso(modelo.IdOpcion,
                base.UserName, Acciones.Informe);

            model.AccionCopiar = (new EventoHelper()).VerificarAcceso(modelo.IdOpcion,
                base.UserName, Acciones.Copiar);

            model.AccionEventoCtaf = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], base.UserName, Acciones.PermisoSEV);

            Log.Info(NameController + " - PARTE 5 - " + DateTime.Now, null);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(BusquedaEventoModel modelo)
        {
            BusquedaEventoModel model = new BusquedaEventoModel();
            model.IndicadorPagina = false;

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
            fechaFin = fechaFin.AddDays(1);

            #region Inicio aplicativo Seg. Recomendaciones
            int todosaseg = -1;
            if (modelo.Version == "-1")
            {
                //acceso del usuario de aseguramiento de la operación
                bool accesoAseguramiento = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], base.UserName, Acciones.PermisoSEVAseg);
                if (accesoAseguramiento)
                {
                    todosaseg = 1;
                }
            }

            int nroRegistros = servicio.ObtenerNroFilasEvento(modelo.IdTipoEvento, fechaInicio, fechaFin, modelo.Version, modelo.Turno,
                modelo.IdTipoEmpresa, modelo.IdEmpresa, modelo.IdFamilia, modelo.IndInterrupcion, modelo.AreaOperativa, todosaseg);
            #endregion Fin aplicativo Seg. Recomendaciones


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
        /// Permite visualizar el detalle del evento
        /// </summary>
        /// <returns></returns>
        public ActionResult Detalle()
        {
            string codigo = Request[RequestParameter.EventoId];
            int id = int.Parse(codigo);

            EventoModel model = new EventoModel();
            EventoDTO evento = this.servicio.ObtenerEvento(id);
            model.ListaTipoEvento = this.servicio.ListarTipoEvento().ToList();
            model.ListaEmpresas = this.servicio.ListarEmpresas().ToList();
            model.ListaCausaEvento = this.servicio.ObtenerCausaEvento(evento.TIPOEVENCODI).ToList();
            model.Evento = evento;
            model.IdEvento = id;
            model.ListaInterrupciones = (new DetalleEventoAppServicio()).GetByCriteriaEveInterrupcions(id);
            model.ListaSubEventos = (new DetalleEventoAppServicio()).GetByCriteriaEveSubeventoss(id);

            return View(model);
        }

        /// <summary>
        /// Muestra la vista del informe
        /// </summary>
        /// <returns></returns>
        public ActionResult Perturbacion()
        {

            string codigo = Request[RequestParameter.EventoId];
            int id = int.Parse(codigo);

            string indicador = Constantes.NO;

            if (Request[RequestParameter.Indicador] != null)
            {
                indicador = Request[RequestParameter.Indicador];
            }

            PerturbacionModel model = new PerturbacionModel();
            EventoDTO evento = this.servicio.ObtenerResumenEvento(id);
            model.AsuntoEvento = evento.EVENDESC;
            model.FechaEvento = (evento.EVENINI != null) ? ((DateTime)evento.EVENINI).ToString(Constantes.FormatoFecha) : string.Empty;
            model.HoraEvento = (evento.EVENINI != null) ? ((DateTime)evento.EVENINI).ToString(Constantes.FormatoHora) : string.Empty;
            model.EmpresaEvento = evento.EMPRNOMB;
            model.EquipoEvento = evento.EQUIABREV;
            model.CodigoEvento = id;
            model.IndicadorGrabado = indicador;
            model.ListaCausaEvento = this.servicio.ObtenerCausaEvento(evento.TIPOEVENCODI).ToList();
            model.ListaInforme = this.servicio.ObtenerInformePorEvento(id).ToList();
            model.IndicadorExistencia = evento.EVENPERTURBACION;
            this.ListaItemReportePerturbacion = model.ListaInforme;

            return View(model);
        }

        /// <summary>
        /// Permite agregar un item al reporte de perturbación
        /// </summary>
        /// <param name="nroOrden"></param>
        /// <param name="tipo"></param>
        /// <param name="descripcion"></param>
        /// <param name="hora"></param>
        /// <param name="equipo"></param>
        /// <param name="interruptor"></param>
        /// <param name="senial"></param>
        /// <param name="ac"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddItemPerturbacion(int nroOrden, string tipo, string descripcion, string hora,
            string equipo, string interruptor, string senial, string ac)
        {
            List<InformePerturbacionDTO> list = this.ListaItemReportePerturbacion;
            list.Add(new InformePerturbacionDTO
            {
                ITEMORDEN = nroOrden,
                ITEMTIPO = tipo,
                ITEMDESCRIPCION = descripcion,
                ITEMTIME = hora,
                ITEMSENALIZACION = senial,
                ITEMAC = ac,
                EQUICODI = (!string.IsNullOrEmpty(equipo)) ? (int?)int.Parse(equipo) : null,
                INTERRUPTORCODI = (!string.IsNullOrEmpty(interruptor)) ? (int?)int.Parse(interruptor) : null
            });
            this.ListaItemReportePerturbacion = list;
            return Json(1);
        }

        /// <summary>
        /// Permite editar un item del reporte de perturbación
        /// </summary>
        /// <param name="nroOrden"></param>
        /// <param name="tipo"></param>
        /// <param name="descripcion"></param>
        /// <param name="hora"></param>
        /// <param name="equipo"></param>
        /// <param name="interruptor"></param>
        /// <param name="senial"></param>
        /// <param name="ac"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditItemPerturbacion(int nroOrden, string tipo, string descripcion, string hora,
            string equipo, string interruptor, string senial, string ac)
        {
            try
            {
                List<InformePerturbacionDTO> list = this.ListaItemReportePerturbacion;
                InformePerturbacionDTO item = list.Where(x => x.ITEMORDEN == nroOrden && x.ITEMTIPO == tipo).FirstOrDefault();
                if (item != null)
                {
                    item.ITEMDESCRIPCION = descripcion;
                    item.ITEMTIME = hora;
                    item.ITEMSENALIZACION = senial;
                    item.ITEMAC = ac;
                    item.EQUICODI = (!string.IsNullOrEmpty(equipo)) ? (int?)int.Parse(equipo) : null;
                    item.INTERRUPTORCODI = (!string.IsNullOrEmpty(interruptor)) ? (int?)int.Parse(interruptor) : null;
                    this.ListaItemReportePerturbacion = list;
                }
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Quita un item del reporte de perturbación
        /// </summary>
        /// <param name="nroOrden"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteItemPerturbacion(int nroOrden, string tipo)
        {
            try
            {
                List<InformePerturbacionDTO> list = this.ListaItemReportePerturbacion;
                InformePerturbacionDTO item = list.Where(x => x.ITEMORDEN == nroOrden && x.ITEMTIPO == tipo).FirstOrDefault();
                if (item != null)
                {
                    list.Remove(item);
                    this.ListaItemReportePerturbacion = list;
                }
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener el nro de orden
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerNroOrden(string tipo)
        {
            List<InformePerturbacionDTO> list = this.ListaItemReportePerturbacion;
            decimal? nro = list.Where(x => x.ITEMTIPO == tipo).Max(x => x.ITEMORDEN);
            if (nro != null)
            {
                return Json((int)nro + 1);
            }
            else
            {
                return Json(1);
            }
        }

        /// <summary>
        /// Permite generar el reporte de perturbación
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="idCausa"></param>
        /// <param name="asunto"></param>
        /// <param name="analisis"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarReporte(int idEvento, string idCausa, string asunto, string analisis)
        {
            try
            {
                List<InformePerturbacionDTO> list = this.ListaItemReportePerturbacion;

                if (!string.IsNullOrEmpty(idCausa))
                {
                    InformePerturbacionDTO itemCausa = list.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemCausa).FirstOrDefault();
                    if (itemCausa != null)
                    {
                        itemCausa.SUBCAUSACODI = int.Parse(idCausa);
                    }
                    else
                    {
                        list.Add(new InformePerturbacionDTO
                        {
                            ITEMORDEN = 1,
                            ITEMTIPO = TipoItemPerturbacion.ItemCausa,
                            SUBCAUSACODI = int.Parse(idCausa)
                        });
                    }
                }
                else
                {
                    InformePerturbacionDTO itemCausa = list.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemCausa).FirstOrDefault();
                    if (itemCausa != null)
                    {
                        list.Remove(itemCausa);
                    }
                }

                InformePerturbacionDTO itemAsunto = list.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemDescripcion).FirstOrDefault();

                if (itemAsunto != null)
                {
                    itemAsunto.ITEMDESCRIPCION = asunto;
                }
                else
                {
                    list.Add(new InformePerturbacionDTO
                    {
                        ITEMORDEN = 1,
                        ITEMTIPO = TipoItemPerturbacion.ItemDescripcion,
                        ITEMDESCRIPCION = asunto
                    });
                }

                InformePerturbacionDTO itemAnalisis = list.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemAnalisis).FirstOrDefault();

                if (itemAnalisis != null)
                {
                    itemAnalisis.ITEMDESCRIPCION = analisis;
                }
                else
                {
                    list.Add(new InformePerturbacionDTO
                    {
                        ITEMORDEN = 1,
                        ITEMTIPO = TipoItemPerturbacion.ItemAnalisis,
                        ITEMDESCRIPCION = analisis
                    });
                }

                this.ListaItemReportePerturbacion = list;
                int resultado = this.servicio.GrabarInformePerturbacion(list, idEvento, base.UserName);
                return Json(resultado);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar el reporte de perturbacion
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarReporte(int idEvento)
        {
            try
            {
                this.servicio.EliminarInformePerturbacion(idEvento);
                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite generar el archivo del reporte
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivo(int idEvento, string tipo)
        {
            try
            {
                List<InformePerturbacionDTO> Lista = this.servicio.ObtenerInformePorEvento(idEvento).ToList();
                EventoDTO evento = this.servicio.ObtenerResumenEvento(idEvento);
                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();

                if (tipo == Constantes.FormatoWord)
                {
                    (new WordDocument()).GenerarReportePerturbacion(Lista, evento, idEvento, path);

                }
                if (tipo == Constantes.FormatoPDF)
                {
                    (new PdfDocument()).GenerarReportePerturbacion(Lista, evento, idEvento, path);
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
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
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(int idTipoEmpresa)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();

            if (idTipoEmpresa != 0)
            {
                entitys = this.servicio.ListarEmpresasPorTipo(idTipoEmpresa);
            }
            else
            {
                entitys = this.servicio.ListarEmpresas();
            }

            SelectList list = new SelectList(entitys, EntidadPropiedad.EmprCodi, EntidadPropiedad.EmprNomb);

            return Json(list);
        }

        /// <summary>
        /// Permite generar el reporte eventos
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarEvento(BusquedaEventoModel modelo)
        {
            int result = 1;
            try
            {
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
                fechaFin = fechaFin.AddDays(1);

                List<EventoDTO> list = servicio.ExportarEventos(modelo.IdTipoEvento, fechaInicio, fechaFin, modelo.Version, modelo.Turno,
                    modelo.IdTipoEmpresa, modelo.IdEmpresa, modelo.IdFamilia, modelo.IndInterrupcion, modelo.Indicador, modelo.AreaOperativa).ToList();

                ExcelDocument.GenerarReporteEvento(list, fechaInicio, fechaFin, modelo.Indicador);

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
        public virtual ActionResult DescargarEvento()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteEvento;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteEvento);
        }

        /// <summary>
        /// Permite mostrar los estados de los informes cargados
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Informe(int idEvento)
        {
            DatoInformeModel model = new DatoInformeModel();
            List<EventoInformeReporte> reporte = (new InformeAppServicio()).ObtenerInformeResumenIntranet(idEvento);
            model.Reporte = reporte.Where(x => x.Emprcodi != -1 && x.Emprcodi != 0).ToList();
            model.ReporteSCO = reporte.Where(x => x.Emprcodi == -1).ToList();
            model.IdEvento = idEvento;

            if (reporte.Where(x => x.Emprcodi == 0).Count() > 0)
            {
                model.ExistenciaInformeConsolidado = Constantes.SI;
            }

            model.IndicadorConsolidado = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                base.UserName, Acciones.Consolidado);

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar el evento y todos los registros relacionados
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int idEvento)
        {
            try
            {
                (new EventosAppServicio()).DeleteEveEvento(idEvento,User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        #region Mejoras CTAF
        /// <summary>
        /// Permite generar evento CTAF
        /// </summary>
        /// <param name="objEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult generarEventoCtaf(List<int> objEvento)
        {
            //EventosAppServicio servEvento = new EventosAppServicio();
            //CriteriosEventoAppServicio servCriterio = new CriteriosEventoAppServicio();
            //List<AnalisisFallaDTO> lstEventosCtaf = new List<AnalisisFallaDTO>();
            //string fileSev = string.Empty;
            //string fileSevAgrupado = string.Empty;
            //int esMultipleEvento = 0, rpta = 1;
            int rpta = 1;
            string usuario = base.User.Identity.Name;
            try
            {
                //foreach (var item in objEvento)
                //{
                //    foreach (var x in objEvento)
                //    {
                //        (new EventosAppServicio()).insertarEventoEvento(item, x);
                //    }
                //    (new EventosAppServicio()).generarEventoCtaf(item);
                //}

                rpta = (new EventosAppServicio()).procesarEventoCtaf(objEvento, usuario);

                //Carpetas en Fs SEV
                //foreach (var y in objEvento)
                //{
                //    List<AnalisisFallaDTO> lstAnalisisFallaDTO = servEvento.ListarAnalisisFalla(y);
                    
                //    foreach (AnalisisFallaDTO itemAF in lstAnalisisFallaDTO)
                //    {
                //        if (itemAF.AFECODI > 0)
                //            lstEventosCtaf.Add(itemAF);
                //    }

                //}
                
                //if (lstEventosCtaf.Count > 0)
                //{
                //    esMultipleEvento = lstEventosCtaf.DistinctBy(x => x.EVENCODI).Count();
                //    //Obtener datos de evento más antiguo
                //    AnalisisFallaDTO _Evento = lstEventosCtaf.OrderBy(x => x.EVENINI).First();

                //    AnalisisFallaDTO Evento = servEvento.ObtenerAnalisisFalla(_Evento.EVENCODI);

                //    #region InsertarCriterios
                //    CrEventoDTO CrCriterio = new CrEventoDTO();
                //    int idcrevento = 0;
                //    CrCriterio.AFECODI = Evento.AFECODI;
                //    CrCriterio.CRESPECIALCODI = 0;
                //    CrCriterio.LASTDATE = DateTime.Now;
                //    CrCriterio.LASTUSER = base.User.Identity.Name;
                //    idcrevento = servCriterio.SaveCrEvento(CrCriterio);

                //    for (int x = 1; x <= 4; x++)
                //    {
                //        CrEtapaEventoDTO CrEtapa = new CrEtapaEventoDTO();
                //        CrEtapa.CREVENCODI = idcrevento;
                //        CrEtapa.CRETAPA = x;
                //        CrEtapa.LASTUSER = base.User.Identity.Name;
                //        CrEtapa.LASTDATE = DateTime.Now;
                //        serviciosCriteriosEvento.SaveCrEtapaEvento(CrEtapa);
                //    }
                //    #endregion

                //    string aaaa = Evento.EVENINI.Value.ToString("yyyy");
                //    string FsSev = Constantes.FileSystemSev;

                //    string asunto = (RemoveAccentsWithRegEx(Regex.Replace(Evento.EVENASUNTO, "[\t@,\\\"/:*?<>|\\\\]", string.Empty)).TrimEnd()).TrimStart();
                //    int maxcaracteres = Convert.ToInt32(ConfigurationManager.AppSettings["MaxCaractAF"]);
                //    if (asunto.Length > maxcaracteres)
                //        asunto = asunto.Substring(0, maxcaracteres).Trim();

                //    //string NombreEvento = Evento.CODIGO + "_" + asunto + "_" + Evento.EVENINI.Value.ToString("dd.MM.yyyy");
                //    string NombreEvento = Evento.CODIGO + "_" + Evento.EVENINI.Value.ToString("dd.MM.yyyy");
                //    DateTime FechaFinSem1 = DateTime.ParseExact(Constantes.FechaFinSem1 + aaaa , Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //    DateTime FechaInicioSem2 = DateTime.ParseExact(Constantes.FechaInicioSem2 + aaaa, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //    DateTime FechaFinSem2 = DateTime.ParseExact(Constantes.FechaFinSem2 + aaaa, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //    DateTime FechaEvento = DateTime.ParseExact(Evento.EVENINI.Value.ToString("dd/MM/yyyy"), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //    string semestre = string.Empty;

                //    if (FechaEvento <= FechaFinSem1)
                //    {
                //        semestre = "Semestre I";
                //    }
                //    else if (FechaEvento >= FechaInicioSem2 && FechaEvento <= FechaFinSem2)
                //    {
                //        semestre = "Semestre II";
                //    }

                //    fileSev = aaaa + "\\" + semestre + "\\" + NombreEvento + "\\";

                //    if (esMultipleEvento == 1)
                //    {
                //        string fileSevValidar = FsSev + fileSev;
                //        if (fileSevValidar.Length > 247)
                //            return Json(-2);

                //        FileServer.CreateFolder(null, fileSev, FsSev);
                //        rpta = UploadFileSev(lstEventosCtaf, fileSev,1, Evento.EVENCODI);
                //        rpta = UploadFileSev(lstEventosCtaf, fileSev, 2, Evento.EVENCODI);
                //    }
                //    else if (esMultipleEvento > 1)
                //    {

                //        foreach (var evento in lstEventosCtaf)
                //        {
                //            fileSevAgrupado = fileSev + evento.EVENINI.Value.ToString("dd.MM.yyyy HH.mm") + "\\";
                //            string fileSevValidar = FsSev + fileSevAgrupado;
                //            if (fileSevValidar.Length > 247)
                //                return Json(-2);
                //            else
                //            {
                //                FileServer.CreateFolder(null, fileSevAgrupado, FsSev);
                //                rpta = UploadFileSev(lstEventosCtaf, fileSevAgrupado, 1, evento.EVENCODI);
                //                rpta = UploadFileSev(lstEventosCtaf, fileSevAgrupado, 2, evento.EVENCODI);
                //            }
                //        }
                //    }
                //}

                return Json(rpta);
            }
            catch (Exception ex)
            {
                Log.Error(NameController,ex);
                return Json(-1);
            }
        }
        
        public int UploadFileSev(List<AnalisisFallaDTO> lstEventosCtaf, string rutaSev, int tipo, int evencodi)
        {
            EventosAppServicio servEvento = new EventosAppServicio();
            //Obtener lista de informes finales e informes preliminares
            List<EveInformesScoDTO> lstInfFinales = new List<EveInformesScoDTO>();
            List<EveInformesScoDTO> lstInfPreliminares = new List<EveInformesScoDTO>();
            foreach (var evento in lstEventosCtaf.DistinctBy(x=>x.EVENCODI))
            {
                if(tipo == 1 && evento.EVENCODI == evencodi)
                {
                    List<EveInformesScoDTO> lstInformesPreliminares = servEvento.ListEveInformesScoxEvento(evento.EVENCODI, 1).ToList(); //Lista de informes preliminares
                    lstInfPreliminares.AddRange(lstInformesPreliminares);
                }
                else if(tipo == 2 && evento.EVENCODI == evencodi)
                {
                    List<EveInformesScoDTO> lstInformesFinales = servEvento.ListEveInformesScoxEvento(evento.EVENCODI, 2).ToList(); //Lista de informes finales
                    lstInfFinales.AddRange(lstInformesFinales);
                }               
            }

            //Copiar IPIs de evento Sco a Sev si es que los hubiera.
            foreach (var informeP in lstInfPreliminares)
            {
                EveInformesScoDTO informe = servEvento.ObtenerInformeSco(informeP.Eveinfcodi);
                string etapa = informe.Afiversion == 1 ? "IPI" : "IF";
                string foldername = string.Empty;
                string filename = informe.Eveinfrutaarchivo;
                string fileserverSco = Constantes.FileSystemSco;
                string fileserverSev = Constantes.FileSystemSev;
                EveInformefallaDTO InformeFalla = new EveInformefallaDTO();
                EveInformefallaN2DTO InformeFallaN2 = new EveInformefallaN2DTO();
                if (informe.Eveninffalla == "S")
                {
                    InformeFalla = servEvento.MostrarEventoInformeFalla(informe.Evencodi);
                    foldername = ConstantesEnviarCorreo.CarpetaInformeFallaN1 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFalla.Evencorr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb.Trim() + "\\" + informe.Env_Evencodi.ToString() + "\\";
                }
                else if (informe.Eveninffallan2 == "S")
                {
                    InformeFallaN2 = servEvento.MostrarEventoInformeFallaN2(informe.Evencodi);
                    foldername = ConstantesEnviarCorreo.CarpetaInformeFallaN2 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb.Trim() + "\\" + informe.Env_Evencodi.ToString() + "\\";
                }
                string rfSev = rutaSev + informeP.Emprnomb + "\\IPI\\" + informe.Env_Evencodi.ToString() + "\\";
                string fileSevValida = fileserverSev + rfSev;
                if (fileSevValida.Length > 247)
                    return -2;

                FileServer.CreateFolder(null, rfSev, fileserverSev);
                FileServer.UploadFromFileDirectory(fileserverSco + foldername + filename, "", rfSev + filename, fileserverSev);
            }
            //Copiar IFs de evento Sco a Sev si es que los hubiera.
            foreach (var informeF in lstInfFinales)
            {
                EveInformesScoDTO informe = servEvento.ObtenerInformeSco(informeF.Eveinfcodi);
                string etapa = informe.Afiversion == 1 ? "IPI" : "IF";
                string foldername = string.Empty;
                string filename = informe.Eveinfrutaarchivo;
                string fileserverSco = Constantes.FileSystemSco;
                string fileserverSev = Constantes.FileSystemSev;
                EveInformefallaDTO InformeFalla = new EveInformefallaDTO();
                EveInformefallaN2DTO InformeFallaN2 = new EveInformefallaN2DTO();
                if (informe.Eveninffalla == "S")
                {
                    InformeFalla = servEvento.MostrarEventoInformeFalla(informe.Evencodi);
                    foldername = ConstantesEnviarCorreo.CarpetaInformeFallaN1 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFalla.Evencorr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb.Trim() + "\\" + informe.Env_Evencodi.ToString() + "\\";
                }
                else if (informe.Eveninffallan2 == "S")
                {
                    InformeFallaN2 = servEvento.MostrarEventoInformeFallaN2(informe.Evencodi);
                    foldername = ConstantesEnviarCorreo.CarpetaInformeFallaN2 + "\\" + informe.Anio + "\\" + informe.Semestre + "\\" + informe.Diames + "\\E" + InformeFallaN2.Evenn2corr.ToString() + "\\" + etapa + "\\" + informe.Emprnomb.Trim() + "\\" + informe.Env_Evencodi.ToString() + "\\";
                }
                string rfSev = rutaSev + informeF.Emprnomb + "\\IF\\" + informe.Env_Evencodi.ToString() + "\\";
                string fileSevValida = fileserverSev + rfSev;
                if (fileSevValida.Length > 247)
                    return -2;

                FileServer.CreateFolder(null, rfSev, fileserverSev);
                FileServer.UploadFromFileDirectory(fileserverSco + foldername + filename, "", rfSev + filename, fileserverSev);
            }

            return 1;
        }

        /// <summary>
        /// Permite validar evento CTAF
        /// </summary>
        /// <param name="objEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarEventoCtaf(List<int> objEvento)
        {
            EventosAppServicio servEvento = new EventosAppServicio();
            List<EventoDTO> lstEventos = new List<EventoDTO>();
            foreach (var y in objEvento)
            {
                EventoDTO evento = this.servicio.ObtenerEvento(y);
                lstEventos.Add(evento);
            }

            //Obtener datos de evento más antiguo
            EventoDTO Evento = lstEventos.OrderBy(x => x.EVENINI).First();
                if(Evento.EVENASUNTO != null)
                    return Json(Evento.EVENASUNTO);
                else
                    return Json("");
         
        }

        #endregion

        public static string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_A_Accents = new Regex("[Á|À|Ä|Â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_E_Accents = new Regex("[É|È|Ë|Ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_I_Accents = new Regex("[Í|Ì|Ï|Î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_O_Accents = new Regex("[Ó|Ò|Ö|Ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Regex replace_U_Accents = new Regex("[Ú|Ù|Ü|Û]", RegexOptions.Compiled);

            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_A_Accents.Replace(inputString, "A");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_E_Accents.Replace(inputString, "E");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_I_Accents.Replace(inputString, "I");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_O_Accents.Replace(inputString, "O");
            inputString = replace_u_Accents.Replace(inputString, "u");
            inputString = replace_U_Accents.Replace(inputString, "U");
            return inputString;
        }
    }  
}