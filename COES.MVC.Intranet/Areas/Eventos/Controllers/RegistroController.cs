using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Evento.Helper;
using COES.MVC.Intranet.Areas.Eventos.Helper;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Informe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class RegistroController : BaseController
    {
        /// <summary>
        /// Instanicia de clase servicio
        /// </summary>
        EventosAppServicio servicioEvento = new EventosAppServicio();
        EquipamientoAppServicio servicioEquipo = new EquipamientoAppServicio();

        /// <summary>
        /// Tipo de evento por defecto
        /// </summary>
        private int IdTipoEvento = 4;

        /// <summary>
        /// Tipo evento bitacora
        /// </summary>
        private int IdTipoEventoBitacora = 2;

        /// <summary>
        /// Tipo de evento por defecto
        /// </summary>
        private int IdTipoOperacion = 100;

        /// <summary>
        /// Configuracion de Aseguramiento. Seguimiento Recomendaciones SEV
        /// </summary>
        private int IdTipoOpeAseguramiento = 11;
        private int IdSubcausaDefecto = 401;
        private int IdSubcausaDetalleDefecto = -1;
        private string TipoAseguramiento = "A";


        /// <summary>
        /// Lista de equipos
        /// </summary>
        public List<EqEquipoDTO> ListaEquipos
        {
            get
            {
                return (Session[DatosSesion.ListaEquipos] != null) ?
                    (List<EqEquipoDTO>)Session[DatosSesion.ListaEquipos] : new List<EqEquipoDTO>();
            }
            set { Session[DatosSesion.ListaEquipos] = value; }
        }
        
        /// <summary>
        /// Pagina inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Muestra la pantalla para el registro de eventos
        /// </summary>
        /// <returns></returns>
        public ActionResult Final(int? id, int? idCopia)
        {
            RegistroModel model = new RegistroModel();
            model.ListaTipoEvento = this.servicioEvento.ListarTipoEvento().Where(x => x.Tipoevencodi == 4 || x.Tipoevencodi == 5 || x.Tipoevencodi == 10).ToList();
           
            if (id == null && idCopia == null)
            {
                model.IdTipoEvento = this.IdTipoEvento;
                model.ListaSubCausaEvento = this.servicioEvento.ObtenerCausaEvento(this.IdTipoEvento);
                model.Entidad = new EveEventoDTO();
                model.Entidad.Evencodi = 0;
                //this.ListaEquipos = null;
                model.ListaEquipo = new List<EqEquipoDTO>();
                model.IndicadorInterrupcion = Constantes.NO;
                model.HoraInicial = DateTime.Now.ToString(Constantes.FormatoFechaFull);
                model.HoraFinal = DateTime.Now.ToString(Constantes.FormatoFechaFull);
                model.TipoRegistro = string.Empty;
                model.TipoMalaCalidad = string.Empty;
                model.DesconexionAutomatico = string.Empty;
                model.DesconexionDisminucion = string.Empty;
                model.DesconexionInterrupcion = string.Empty;
                model.DesconexionManual = string.Empty;
                model.ValDesconexionAutomatico = string.Empty;
                model.DesconexionDisminucion = string.Empty;
                model.DesconexionInterrupcion = string.Empty;
                model.DesconexionManual = string.Empty;
                model.IndAuditoria = Constantes.NO;
                model.IndRemitenteCorreo = Constantes.SI;
                model.IdTipoOperacion = Convert.ToInt32(Constantes.ParametroDefecto);

                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    model.RemitenteCorreo = ((COES.MVC.Intranet.SeguridadServicio.UserDTO)Session[DatosSesion.SesionUsuario]).UsernName;
                }
            }
            else if(id != null || idCopia!=null)
            {
                if (id == null) id = idCopia;

                model.Entidad = this.servicioEvento.GetByIdEveEvento((int)id);
                ViewBag.CTEvenctaf = model.Entidad.Evenctaf;
                model.IdTipoEvento = (int)model.Entidad.Tipoevencodi;
                model.ListaSubCausaEvento = this.servicioEvento.ObtenerCausaEvento(model.IdTipoEvento);
                if (model.Entidad.Eveninffalla == "S")
                {
                    var falla = this.servicioEvento.MostrarEventoInformeFalla((int)id);
                    model.DiaIpiAmpliacion = (int)falla.Eveninfplazodiasipi ;
                    model.DiaIfAmpliacion = (int)falla.Eveninfplazodiasif;
                    model.HorarioIpiAmpliacion = (falla.Eveninfplazohoraipi.ToString().Length == 2 ? falla.Eveninfplazohoraipi.ToString() : '0' + falla.Eveninfplazohoraipi.ToString()) + ':' +
                                                (falla.Eveninfplazominipi.ToString().Length == 2 ? falla.Eveninfplazominipi.ToString() : '0' + falla.Eveninfplazominipi.ToString());
                    model.HorarioIfAmpliacion = (falla.Eveninfplazohoraif.ToString().Length == 2 ? falla.Eveninfplazohoraif.ToString() : '0' + falla.Eveninfplazohoraif.ToString()) + ':' +
                                                 (falla.Eveninfplazominif.ToString().Length == 2 ? falla.Eveninfplazominif.ToString() : '0' + falla.Eveninfplazominif.ToString()); ;
                    model.EvenInfCodi = falla.Eveninfcodi;
                }

                if (model.Entidad.Eveninffallan2 == "S")
                {
                    var falla = this.servicioEvento.MostrarEventoInformeFallaN2((int)id);

                    if (falla != null)
                    {
                        if (falla.Eveninfplazodiasipi != null)
                            model.DiaIpiAmpliacion = (int)falla.Eveninfplazodiasipi;

                        if (falla.Eveninfplazohoraipi != null || falla.Eveninfplazominipi != null)
                            model.HorarioIpiAmpliacion = (falla.Eveninfplazohoraipi.ToString().Length == 2 ? falla.Eveninfplazohoraipi.ToString() : '0' + falla.Eveninfplazohoraipi.ToString()) + ':' +
                                                       (falla.Eveninfplazominipi.ToString().Length == 2 ? falla.Eveninfplazominipi.ToString() : '0' + falla.Eveninfplazominipi.ToString());

                        if (falla.Eveninfplazohoraif != null || falla.Eveninfplazominif != null)
                            model.HorarioIfAmpliacion = (falla.Eveninfplazohoraif.ToString().Length == 2 ? falla.Eveninfplazohoraif.ToString() : '0' + falla.Eveninfplazohoraif.ToString()) + ':' +
                                                     (falla.Eveninfplazominif.ToString().Length == 2 ? falla.Eveninfplazominif.ToString() : '0' + falla.Eveninfplazominif.ToString());

                        if (falla.Eveninfn2codi != null)
                            model.EvenInfn2Codi = falla.Eveninfn2codi;
                    }

                }



                model.ListaEquipo = this.servicioEvento.GetEquiposPorEvento((int)id);

                if (model.ListaEquipo.Count == 0)
                {
                    EveEvenequipoDTO eventoEquipo = new EveEvenequipoDTO();
                    eventoEquipo.Equicodi = (int)model.Entidad.Equicodi;
                    eventoEquipo.Evencodi = (int)id;
                    eventoEquipo.Lastuser = base.UserName;
                    eventoEquipo.Lastdate = DateTime.Now;
                    this.servicioEvento.GrabarEventoEquipo(eventoEquipo);
                    model.ListaEquipo = this.servicioEvento.GetEquiposPorEvento((int)id);
                }

                model.IndicadorInterrupcion = Constantes.SI;
                model.HoraInicial = (model.Entidad.Evenini != null) ?
                    ((DateTime)model.Entidad.Evenini).ToString(Constantes.FormatoFechaFull) : string.Empty;
                model.HoraFinal = (model.Entidad.Evenfin != null) ?
                    ((DateTime)model.Entidad.Evenfin).ToString(Constantes.FormatoFechaFull) : string.Empty;

                model.TipoRegistro = model.Entidad.Tiporegistro;
                string valTipoRegistro = model.Entidad.Valtiporegistro;
                model.IdTipoOperacion = Convert.ToInt32(Constantes.ParametroDefecto);

                if (model.TipoRegistro == ConstantesEvento.TipoRegistroDesconexion)
                {
                    if (!string.IsNullOrEmpty(valTipoRegistro))
                    {
                        string[] desconexion = valTipoRegistro.Split(Constantes.CaracterComa);

                        if (desconexion.Length == 8)
                        {
                            model.ValDesconexionAutomatico = string.Empty;
                            model.ValDesconexionDisminucion = string.Empty;
                            model.ValDesconexionInterrupcion = string.Empty;
                            model.ValDesconexionManual = string.Empty;

                            model.DesconexionInterrupcion = desconexion[0];
                            if (model.DesconexionInterrupcion == Constantes.SI)
                                model.ValDesconexionInterrupcion = desconexion[1];
                            
                            model.DesconexionDisminucion = desconexion[2];
                            if (model.DesconexionDisminucion == Constantes.SI)
                                model.ValDesconexionDisminucion = desconexion[3];

                            model.DesconexionManual = desconexion[4];
                            if (model.DesconexionManual == Constantes.SI)
                                model.ValDesconexionManual = desconexion[5];

                            model.DesconexionAutomatico = desconexion[6];
                            if (model.DesconexionAutomatico == Constantes.SI)
                                model.ValDesconexionAutomatico = desconexion[7];
                        }
                    }
                    else
                    {
                        model.DesconexionAutomatico = Constantes.NO;
                        model.DesconexionDisminucion = Constantes.NO;
                        model.DesconexionInterrupcion = Constantes.NO;
                        model.DesconexionManual = Constantes.NO;
                        model.ValDesconexionAutomatico = string.Empty;
                        model.ValDesconexionDisminucion = string.Empty;
                        model.ValDesconexionInterrupcion = string.Empty;
                        model.ValDesconexionManual = string.Empty;
                    }
                }

                if (model.TipoRegistro == ConstantesEvento.TipoRegistroMalaCalidad)
                {
                    model.TipoMalaCalidad = valTipoRegistro;
                }

                model.IndAuditoria = Constantes.SI;
                model.IndRemitenteCorreo = Constantes.NO;

                if (idCopia != null)
                {
                    model.IndAuditoria = Constantes.NO;
                    model.IndRemitenteCorreo = Constantes.SI;
                    model.IndicadorInterrupcion = Constantes.NO;
                    model.Entidad.Evencodi = 0;

                    if (Session[DatosSesion.SesionUsuario] != null)
                    {
                        model.RemitenteCorreo = ((COES.MVC.Intranet.SeguridadServicio.UserDTO)Session[DatosSesion.SesionUsuario]).UsernName;
                    }                    
                }
            }

            model.IndicadorGrabar = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                base.UserName, Acciones.Grabar);
            model.IndicadorAdicional = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                base.UserName, Acciones.Adicional);
            model.IndicadorInforme = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                base.UserName, Acciones.Informe);
            model.IndicadorImportar = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                base.UserName, Acciones.Importar);
            model.IndicadorGrabarAseg = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], 
                base.UserName, Acciones.PermisoSEVAseg);


            
            return View(model);
        }
        
        /// <summary>
        /// Muestra la pantalla para el registro de eventos tipo bitacora
        /// </summary>
        /// <returns></returns>
        public ActionResult Bitacora(int? id, int? idCopia)
        {
            RegistroModel model = new RegistroModel();
            model.ListaTipoEvento = this.servicioEvento.ListarTipoEvento();            
            model.ListaEmpresa = this.servicioEvento.ListarEmpresas();
            model.ListaTipoOperacion = this.servicioEvento.ObtenerCausaEvento(this.IdTipoOperacion);

            if (id == null && idCopia == null)
            {
                model.IdTipoEvento = this.IdTipoEventoBitacora;
                model.ListaSubCausaEvento = this.servicioEvento.ObtenerCausaEvento(this.IdTipoEventoBitacora);
                model.Entidad = new EveEventoDTO();
                model.Entidad.Evencodi = 0;
                model.HoraInicial = DateTime.Now.ToString(Constantes.FormatoFechaFull);
                model.HoraFinal = DateTime.Now.ToString(Constantes.FormatoFechaFull);
            }
            else if (id != null || idCopia != null)
            {
                if (id == null) id = idCopia;

                model.Entidad = this.servicioEvento.GetByIdEveEvento((int)id);

                if (model.Entidad.Equicodi != null)
                {
                    EqEquipoDTO equipo = servicioEquipo.ObtenerDetalleEquipo((int)model.Entidad.Equicodi);
                    model.Entidad.Equiabrev = equipo.TAREAABREV + " " + equipo.AREANOMB + " - " + equipo.Equiabrev; 
                }

                model.IdTipoEvento = (int)model.Entidad.Tipoevencodi;

                model.IdTipoOperacion = (model.Entidad.Subcausacodiop != null) ? (int)model.Entidad.Subcausacodiop : -1;
                model.ListaSubCausaEvento = this.servicioEvento.ObtenerCausaEvento(model.IdTipoEvento);
                model.HoraInicial = (model.Entidad.Evenini != null) ?
                    ((DateTime)model.Entidad.Evenini).ToString(Constantes.FormatoFechaFull) : string.Empty;
                model.HoraFinal = (model.Entidad.Evenfin != null) ?
                    ((DateTime)model.Entidad.Evenfin).ToString(Constantes.FormatoFechaFull) : string.Empty;

                if (idCopia != null)
                {
                    model.Entidad.Evencodi = 0;
                }
            }

            model.IndicadorGrabar = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
               base.UserName, Acciones.Grabar);

            return View(model);
        }

        /// <summary>
        /// Muestra la pantalla para el registro de eventos tipo aseguramiento
        /// </summary>
        /// <returns></returns>
        public ActionResult Aseguramiento(int? id, int? idCopia)
        {
            RegistroModel model = new RegistroModel();
            model.ListaTipoEvento = this.servicioEvento.ListarTipoEvento();
            model.ListaEmpresa = this.servicioEvento.ListarEmpresas();
            model.ListaTipoOperacion = this.servicioEvento.ObtenerCausaEvento(this.IdTipoOpeAseguramiento);

            if (id == null && idCopia == null)
            {
                model.IdTipoEvento = this.IdTipoOpeAseguramiento;
                model.ListaSubCausaEvento = this.servicioEvento.ObtenerCausaEvento(this.IdTipoEventoBitacora);
                model.Entidad = new EveEventoDTO();
                model.Entidad.Evencodi = 0;
                model.HoraInicial = DateTime.Now.ToString(Constantes.FormatoFechaFull);
                model.HoraFinal = DateTime.Now.ToString(Constantes.FormatoFechaFull);
                model.IdTipoOperacion = this.IdSubcausaDefecto; //29052019
                model.Entidad.Subcausacodi = IdSubcausaDetalleDefecto;
            }
            else if (id != null || idCopia != null)
            {
                if (id == null) id = idCopia;

                model.Entidad = this.servicioEvento.GetByIdEveEvento((int)id);

                if (model.Entidad.Equicodi != null)
                {
                    EqEquipoDTO equipo = servicioEquipo.ObtenerDetalleEquipo((int)model.Entidad.Equicodi);
                    model.Entidad.Equiabrev = equipo.TAREAABREV + " " + equipo.AREANOMB + " - " + equipo.Equiabrev;
                }

                model.IdTipoEvento = (int)model.Entidad.Tipoevencodi;

                model.IdTipoOperacion = (model.Entidad.Subcausacodiop != null) ? (int)model.Entidad.Subcausacodiop : -1;
                //model.IdTipoOperacion = (model.Entidad.Subcausacodiop != null) ? (int)model.Entidad.Subcausacodiop : IdTipoOpeAseguramiento;
                model.ListaSubCausaEvento = this.servicioEvento.ObtenerCausaEvento(model.IdTipoEvento);
                model.HoraInicial = (model.Entidad.Evenini != null) ?
                    ((DateTime)model.Entidad.Evenini).ToString(Constantes.FormatoFechaFull) : string.Empty;
                model.HoraFinal = (model.Entidad.Evenfin != null) ?
                    ((DateTime)model.Entidad.Evenfin).ToString(Constantes.FormatoFechaFull) : string.Empty;

                if (idCopia != null)
                {
                    model.Entidad.Evencodi = 0;
                }
            }

            model.IndicadorGrabarAseg = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion], base.UserName, Acciones.PermisoSEVAseg);
            
            return View(model);
        }
        
        /// <summary>
        /// Permite cargar los tipos de eventos
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarSubCausaEvento(int idTipoEvento)
        {
            List<EveSubcausaeventoDTO> list = this.servicioEvento.ObtenerCausaEvento(idTipoEvento);
            return Json(list);
        }

        /// <summary>
        /// Permite agregar un equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgregarEquipo(int idEquipo)
        {
            try
            {                             
                EqEquipoDTO equipo = servicioEquipo.ObtenerDetalleEquipo(idEquipo);
                return Json(equipo);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite mostrar el listado de equipo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Equipos()
        {
            RegistroModel model = new RegistroModel();
            model.ListaEquipo = this.ListaEquipos;
            model.IndicadorGrabar = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                base.UserName, Acciones.Grabar);

            return PartialView(model);
        }

        ///// <summary>
        ///// Permite eliminar el equipo
        ///// </summary>
        ///// <param name="idEquipo"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult EliminarEquipo(int idEquipo)
        //{
        //    try
        //    {
        //        List<EqEquipoDTO> list = this.ListaEquipos;
        //        EqEquipoDTO item = list.Where(x => x.Equicodi == idEquipo).FirstOrDefault();
        //        if (item != null)
        //        {
        //            list.Remove(item);
        //        }
        //        this.ListaEquipos = list;
        //        return Json(1);
        //    }
        //    catch
        //    {
        //        return Json(-1);
        //    }        
        //}

        /// <summary>
        /// Permite grabar los datos del evento
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(RegistroModel model)
        {
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(model.HoraInicial, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(model.HoraFinal, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                
                TimeSpan diferencia = fechaFin.Subtract(fechaInicio);

                bool permisoSCO = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                    base.UserName, Acciones.PermisoSCO);

                bool validacionFecha = true;

                if (permisoSCO)
                {
                    validacionFecha = this.servicioEvento.ValidarFechaRegistro(fechaInicio);
                }
                
                if (diferencia.TotalSeconds >= 0)
                {
                    if (true)
                    {
                        EveEventoDTO entity = new EveEventoDTO();
                        entity.Tipoevencodi = model.IdTipoEvento;
                        entity.Evenini = fechaInicio;
                        entity.Evenfin = fechaFin;
                        entity.Evenasunto = model.Descripcion;
                        entity.Evencomentarios = model.Comentarios;
                        entity.Evendesc = model.Detalle;
                        entity.Subcausacodi = model.IdSubCausaEvento;
                        entity.Eventipofalla = model.TipoFalla;
                        entity.Eventipofallafase = model.Fases;
                        entity.Smsenviar = model.MensajeSMS;
                        entity.Evenrelevante = (model.Relevante == Constantes.SI) ? 1 : 0;
                        entity.Evenctaf = model.CTAnalisis;
                        entity.Eveninffalla = model.InformeFalla;
                        entity.Eveninffallan2 = model.InformeFalla2;
                        entity.Evenasegoperacion = model.EvenAsegOperacion;
                        entity.Smsenviado = Constantes.NO;
                        entity.Twitterenviado = Constantes.NO;
                        entity.Evenclasecodi = 1;
                        entity.Deleted = Constantes.NO;
                        entity.Evencodi = model.IdEvento;
                        entity.Tiporegistro = model.TipoRegistro;
                        entity.Evenpreliminar = Constantes.NO;
                        entity.Subcausacodiop = Convert.ToInt32(Constantes.ParametroDefecto);
                        entity.Evenmwindisp = model.MWInterrumpidos;
                        entity.Eveninterrup = model.ProvocaInterrupcion;
                        entity.Evenaopera = model.AreaOperativa;
                        entity.Eventension = model.TensionFalla;
                        entity.Evenmwgendescon = model.MWGeneracionDesconectada;
                        entity.Evengendescon = model.DeconectaGeneracion;
                        entity.Eveninfplazodiasipi = model.DiaIpiAmpliacion;
                        entity.Eveninfplazodiasif = model.DiaIfAmpliacion;
                        entity.Eveninfplazohoraipi = Convert.ToInt32(model.HorarioIpiAmpliacion != null ? model.HorarioIpiAmpliacion.Substring(0,2) : "0");
                        entity.Eveninfplazominipi = Convert.ToInt32(model.HorarioIpiAmpliacion != null ? model.HorarioIpiAmpliacion.Substring(3, 2) : "0");
                        entity.Eveninfplazohoraif = Convert.ToInt32(model.HorarioIfAmpliacion != null ? model.HorarioIfAmpliacion.Substring(0, 2) : "0");
                        entity.Eveninfplazominif = Convert.ToInt32(model.HorarioIfAmpliacion != null ?  model.HorarioIfAmpliacion.Substring(3, 2) : "0");
                        entity.Eveninfcodi = model.EvenInfCodi;
                        entity.Eveninfn2codi = model.EvenInfn2Codi;
                        if (entity.Tiporegistro == ConstantesEvento.TipoRegistroDesconexion)
                        {
                            if (string.IsNullOrEmpty(model.ValDesconexionInterrupcion)) model.ValDesconexionInterrupcion = 0.ToString();
                            if (string.IsNullOrEmpty(model.ValDesconexionDisminucion)) model.ValDesconexionDisminucion = 0.ToString();
                            if (string.IsNullOrEmpty(model.ValDesconexionManual)) model.ValDesconexionManual = 0.ToString();
                            if (string.IsNullOrEmpty(model.ValDesconexionAutomatico)) model.ValDesconexionAutomatico = 0.ToString();

                            string valTipoRegistro =
                                model.DesconexionInterrupcion + Constantes.CaracterComa + model.ValDesconexionInterrupcion + Constantes.CaracterComa +
                                model.DesconexionDisminucion + Constantes.CaracterComa + model.ValDesconexionDisminucion + Constantes.CaracterComa +
                                model.DesconexionManual + Constantes.CaracterComa + model.ValDesconexionManual + Constantes.CaracterComa +
                                model.DesconexionAutomatico + Constantes.CaracterComa + model.ValDesconexionAutomatico;

                            entity.Valtiporegistro = valTipoRegistro;
                        }
                        if (entity.Tiporegistro == ConstantesEvento.TipoRegistroInterrupcion)
                        {
                            entity.Valtiporegistro = string.Empty;
                        }
                        if (entity.Tiporegistro == ConstantesEvento.TipoRegistroMalaCalidad)
                        {
                            entity.Valtiporegistro = model.TipoMalaCalidad;
                        }
                    
                        List<int> idsEquipos = new List<int>();
                        if (!string.IsNullOrEmpty(model.IdsEquipos))
                        {
                            idsEquipos = model.IdsEquipos.Split(Constantes.CaracterComa).Select(int.Parse).ToList();
                        }
                        int id = this.servicioEvento.SaveEveEvento(entity, idsEquipos, base.UserName, model.RemitenteCorreo);

                        return Json(id);
                    }
                    else 
                    {
                        return Json(-3);
                    }
                }
                else
                {
                    return Json(-2);
                }                
            }
            catch
            {
                return Json(-1);
            }        
        }

        [HttpPost]
        public ActionResult Upload()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEvento.RutaBitacora;
            try
            {
                var sNombreArchivo = "";
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public FilePathResult DownloadFileBitacora(string name)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEvento.RutaBitacora;
            string fileName = path + name;
            return File(fileName, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }

        /// <summary>
        /// Permite grabar el evento tipo bitacora
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarBitacora(RegistroModel model)
        {
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(model.HoraInicial, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(model.HoraFinal, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                TimeSpan diferencia = fechaFin.Subtract(fechaInicio);

                if (diferencia.TotalSeconds >= 0)
                {
                    EveEventoDTO entity = new EveEventoDTO();
                    entity.Tipoevencodi = model.IdTipoEvento;
                    entity.Evenini = fechaInicio;
                    entity.Evenfin = fechaFin;
                    entity.Evenasunto = model.Descripcion;                    
                    entity.Evendesc = model.Detalle;
                    entity.Evencomentarios = model.Comentarios;
                    entity.Subcausacodi = model.IdSubCausaEvento;
                    entity.Eventipofalla = Constantes.NO;
                    entity.Eventipofallafase = Constantes.NO;
                    entity.Smsenviar = Constantes.NO;
                    entity.Evenrelevante = 0;
                    entity.Evenctaf = Constantes.NO;
                    entity.Eveninffalla = Constantes.NO;
                    entity.Eveninffallan2 = Constantes.NO;
                    entity.Evenasegoperacion = Constantes.NO;
                    entity.Smsenviado = Constantes.NO;
                    entity.Twitterenviado = Constantes.NO;
                    entity.Evenclasecodi = 1;
                    entity.Deleted = Constantes.NO;
                    entity.Evencodi = model.IdEvento;
                    entity.Equicodi = model.IdEquipo;
                    entity.Emprcodi = model.IdEmpresa;
                    entity.Emprcodirespon = model.IdEmpresa;
                    entity.Evenpreliminar = Constantes.SI;
                    entity.Subcausacodiop = (int)model.IdTipoOperacion;
                    entity.EveAdjunto = model.ArchivoAdicional;

                    int id = this.servicioEvento.GrabarBitacora(entity, base.UserName);

                    return Json(id);
                }
                else
                {
                    return Json(-2);
                }                
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar el evento tipo bitacora
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarAseguramiento(RegistroModel model)
        {
            try
            {
                double diferenciaSegundo=0;
                bool horaFinalvacia = true;
                
                DateTime fechaInicio = DateTime.ParseExact(model.HoraInicial, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact("01/01/2000", Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                //valida fecha si se ingresó
                if (model.HoraFinal != "" && model.HoraFinal!=null)
                {
                    horaFinalvacia = false;
                    fechaFin = DateTime.ParseExact(model.HoraFinal, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    TimeSpan diferencia = fechaFin.Subtract(fechaInicio);
                    diferenciaSegundo = diferencia.TotalSeconds;
                }


                //if (diferencia.TotalSeconds >= 0)
                if ((diferenciaSegundo >= 0 && !horaFinalvacia) || horaFinalvacia)
                {
                    EveEventoDTO entity = new EveEventoDTO();
                    entity.Tipoevencodi = IdTipoOpeAseguramiento;
                    entity.Evenini = fechaInicio;

                    if(!horaFinalvacia){
                        entity.Evenfin = fechaFin;
                    }

                    entity.Evenasunto = model.Descripcion;
                    entity.Evendesc = model.Detalle;
                    entity.Evencomentarios = model.Comentarios;
                    entity.Subcausacodi = model.IdSubCausaEvento;
                    entity.Eventipofalla = Constantes.NO;
                    entity.Eventipofallafase = Constantes.NO;
                    entity.Smsenviar = Constantes.NO;
                    entity.Evenrelevante = 0;
                    entity.Evenctaf = Constantes.NO;
                    entity.Eveninffalla = Constantes.NO;
                    entity.Eveninffallan2 = Constantes.NO;
                    entity.Evenasegoperacion = Constantes.NO;
                    entity.Smsenviado = Constantes.NO;
                    entity.Twitterenviado = Constantes.NO;
                    entity.Evenclasecodi = 1;
                    entity.Deleted = Constantes.NO;
                    entity.Evencodi = model.IdEvento;
                    entity.Equicodi = model.IdEquipo;
                    entity.Emprcodi = model.IdEmpresa;
                    entity.Emprcodirespon = model.IdEmpresa;
                    entity.Evenpreliminar = this.TipoAseguramiento;
                    entity.Subcausacodiop = (int)model.IdTipoOperacion;

                    #region Inicio aplicativo Seg. Recomendaciones
                    entity.Lastuser = base.UserName;
                    entity.Lastdate = DateTime.Now;
                    #endregion Fin aplicativo Seg. Recomendaciones

                    int id = this.servicioEvento.GrabarBitacora(entity, base.UserName);

                    return Json(id);
                }
                else
                {
                    //if(diferenciaSegundo >= 0 && !horaFinalvacia)
                    {
                        return Json(-2);
                    }
                }
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Abre la ventana para el registro de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Interrupcion(int idEvento, int id, decimal tiempo, decimal total, int idItemInforme)
        {
            InterrupcionModel model = new InterrupcionModel();
            model.ListaPuntos = this.servicioEvento.ListaPuntosInterrupcion();

            if (id == 0)
            {
                model.Entidad = new EveInterrupcionDTO();
                model.Entidad.Evencodi = idEvento;               
                model.Entidad.Interrupcodi = 0;
                model.IdItemInforme = 0;

                if (idItemInforme > 0)
                {
                    model.Entidad.Interrmw = total;
                    model.Entidad.Interrminu = tiempo;
                    model.IdItemInforme = idItemInforme;
                }
            }
            else 
            {
                model.Entidad = this.servicioEvento.ObtenerInterrupcion(id);
            }
            
            return PartialView(model);
        }

        /// <summary>
        /// Permite listar las interrupciones de un evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaInterrupcion(int idEvento)
        {
            RegistroModel model = new RegistroModel();
            model.ListaInterrupciones = (new DetalleEventoAppServicio()).GetByCriteriaEveInterrupcions(idEvento);
            model.IndicadorGrabar = (new EventoHelper()).VerificarAcceso(Session[DatosSesion.SesionIdOpcion],
                base.UserName, Acciones.Grabar);
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar una interrupcion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteInterrupcion(int id, int idEvento)
        {
            try
            {
                this.servicioEvento.EliminarInterrupcion(id, idEvento, base.UserName);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar el punto de interrupción
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarInterrupcion(InterrupcionModel model)
        {
            try
            {
                EveInterrupcionDTO entity = new EveInterrupcionDTO();

                if (model.Interrnivel == Constantes.SI)
                {
                    if (model.Interrmw == null)
                        entity.Interrmw = model.InterrmwDe - model.InterrmwA;
                    else
                        entity.Interrmw = model.Interrmw;
                }
                else
                {
                    entity.Interrmw = model.Interrmw;
                }

                entity.Evencodi = model.Evencodi;
                entity.Interrdesc = model.Interrdesc;
                entity.Interrmanualr = model.Interrmanualr;
                entity.Interrmfetapa = model.Interrmfetapa;
                entity.Interrminu = model.Interrminu;
                entity.InterrmwA = model.InterrmwA;
                entity.InterrmwDe = model.InterrmwDe;
                entity.Interrnivel = model.Interrnivel;
                entity.Interrracmf = model.Interrracmf;
                entity.Interrupcodi = model.Interrupcodi;
                entity.Lastdate = DateTime.Now;
                entity.Lastuser = base.UserName;
                entity.Ptointerrcodi = model.Ptointerrcodi;

                int id = this.servicioEvento.GrabarInterrupcion(entity, model.IdItemInforme, base.UserName);

                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }
        
        /// <summary>
        /// Permite mostrar los registros del interrupciones
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Importacion(int idEvento)
        {
            InterrupcionModel model = new InterrupcionModel();
            model.ListaInterrupcionInforme = (new InformeAppServicio()).ObtenerInformeInterrupcion(idEvento);
            return PartialView(model);
        }

        /// <summary>
        /// Permite convertir una bitacora a un evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CambiarVersion(int idEvento)
        {
            try
            {
                int resultado = this.servicioEvento.CambiarVersion(idEvento, Constantes.NO, base.UserName);
                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite convertir un evento tipo final a bitárica
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CambiarBitacora(int idEvento)
        {
            try
            {
                this.servicioEvento.CambiarBitacora(idEvento, Constantes.SI, base.UserName);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite listar la auditoria del evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Auditoria(int idEvento)
        {
            AuditoriaModel model = new AuditoriaModel();
            model.ListaAuditoria = this.servicioEvento.ListEveEventoLogs(idEvento);

            return PartialView(model);            
        }



        /// <summary>
        /// Permite exportar las interrupciones de un evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarInterrupcion(int idEvento)
        {
            try
            {
                List<EveInterrupcionDTO> list = (new DetalleEventoAppServicio()).GetByCriteriaEveInterrupcions(idEvento);                
                ExcelDocument.GenerarArchivoInterrupcion(list);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
        
        /// <summary>
        /// Permite descargar el excel generado de interrupciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarInterrupcion()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteInterrupciones;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteInterrupciones);
        }

    }
}
