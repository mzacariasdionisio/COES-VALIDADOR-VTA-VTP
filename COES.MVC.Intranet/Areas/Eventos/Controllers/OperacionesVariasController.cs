using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.Evento.Helper;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.OperacionesVarias;
using DevExpress.Office.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class OperacionesVariasController : BaseController
    {
        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(OperacionesVariasController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Protected de log de errores page
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

        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicio
        {
            get
            {
                return (Session[DatosSesion.FechaConsultaInicio] != null) ?
                    (DateTime?)(Session[DatosSesion.FechaConsultaInicio]) : null;
            }
            set
            {
                Session[DatosSesion.FechaConsultaInicio] = value;
            }
        }

        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFinal
        {
            get
            {
                return (Session[DatosSesion.FechaConsultaFin] != null) ?
                  (DateTime?)(Session[DatosSesion.FechaConsultaFin]) : null;
            }
            set
            {
                Session[DatosSesion.FechaConsultaFin] = value;
            }
        }

        /// <summary>
        /// Horizonte
        /// </summary>
        public string Horizonte
        {
            get
            {
                return (Session[ConstantesOperacionesVarias.Horizonte] != null) ?
                  (string)Session[ConstantesOperacionesVarias.Horizonte] : null;
            }
            set
            {
                Session[ConstantesOperacionesVarias.Horizonte] = value;
            }
        }

        /// <summary>
        /// Tipo Operacion
        /// </summary>
        public string TipoOperacion
        {
            get
            {
                return (Session[ConstantesOperacionesVarias.TipoOperacion] != null) ?
                  (string)Session[ConstantesOperacionesVarias.TipoOperacion] : null;
            }
            set
            {
                Session[ConstantesOperacionesVarias.TipoOperacion] = value;
            }
        }


        OperacionesVariasAppServicio servicio = new OperacionesVariasAppServicio();


        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaOperacionesVariasModel model = new BusquedaOperacionesVariasModel();

            model.ListaEvenclase = this.servicio.ListarEvenclase();
            model.ListaEvensubcausa = this.servicio.ListarSubcausaeventoByAreausuaria(ConstantesOperacionesVarias.EvenSubcausa, base.IdArea);

            model.Fechaini = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Fechafin = DateTime.Now.ToString(Constantes.FormatoFecha);

            model.Fechaini = (this.FechaInicio != null) ? ((DateTime)this.FechaInicio).ToString(Constantes.FormatoFecha) :
               DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Fechafin = (this.FechaFinal != null) ? ((DateTime)this.FechaFinal).ToString(Constantes.FormatoFecha) :
                DateTime.Now.ToString(Constantes.FormatoFecha);

            model.Horizonte = (this.Horizonte != null) ? this.Horizonte : ConstantesOperacionesVarias.EventoEjecutado;
            model.TipoOperacion = (this.TipoOperacion != null) ? this.TipoOperacion : ConstantesOperacionesVarias.ParametroTodos;

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }



        /// <summary>
        /// Editar registro con parámetro
        /// </summary>
        /// <param name="id">id de operaciones varias</param>
        /// <param name="accion">0: ver, 1: editar. 2: copiar a nuevo</param>
        /// <returns></returns>
        //public ActionResult Editar_accion(int id,int accion)
        //public OperacionesVariasModel Editar_accion(int id, int accion)
        public ActionResult Editar(int id, int accion, int horz, int top)
        {

            int accion2 = 0;
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            OperacionesVariasModel model = new OperacionesVariasModel();

            //habilitar en producción de acuerdo a perfil
            model.GrabarEvento = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            model.ListaEvenclase = servicio.ListarEvenclase();
            model.ListaEvensubcausa = this.servicio.ListarSubcausaeventoByAreausuaria(ConstantesOperacionesVarias.EvenSubcausa, base.IdArea);

            EveIeodcuadroDTO ieodcuadro = servicio.ObtenerIeodCuadro(id);

            model.EquipoInvolucrado = "";
            if (id != 0)
            {
                model.IeodCuadroDet = servicio.GetByCriteria(id);

                foreach (EveIeodcuadroDetDTO ieoddet in model.IeodCuadroDet)
                {
                    model.EquipoInvolucrado = ieoddet.Iccodi + "," + ieoddet.Equicodi + "," + ieoddet.Icdetcheck1 + ",";
                }

            }
            else
                model.IeodCuadroDet = null;


            if (ieodcuadro != null)
            {
                model.IeodCuadro = ieodcuadro;

                if (accion == 2)
                    model.IeodCuadro.Iccodi = 0;

            }
            else
            {
                ieodcuadro = new EveIeodcuadroDTO();
                ieodcuadro.Evenclasecodi = horz;//;
                //ieodcuadro.Subcausacodi = (top == 0 ? 214 : top);//214;
                ieodcuadro.Subcausacodi = (top == 0 ? model.ListaEvensubcausa.First().Subcausacodi : top);

                ieodcuadro.Emprnomb = "( TODOS )";
                ieodcuadro.Areanomb = "(TODOS)";
                ieodcuadro.Equiabrev = "_SEIN";
                ieodcuadro.Famabrev = "( TODOS )";
                ieodcuadro.Equicodi = 0;
                ieodcuadro.Iccodi = 0;

                DateTime ldt1 = DateTime.Now;
                ieodcuadro.Ichorini = ldt1;
                ieodcuadro.Ichorfin = ldt1;

                model.IeodCuadro = ieodcuadro;

            }

            if (accion != 2)
                model.Accion = accion;
            else
                model.Accion = 1;

            if (base.IdArea == 8)
                model.Accioncuadro = 1;
            else
                model.Accioncuadro = 0;


            return View(model);

        }


        [HttpPost]
        public JsonResult Grabar(OperacionesVariasModel model)
        {
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

                entity.Iccodi = model.Iccodi;
                entity.Equicodi = model.Equicodi;
                entity.Subcausacodi = model.Subcausacodi;
                entity.Evenclasecodi = model.Evenclasecodi;

                entity.Ictipcuadro = model.Ictipcuadro;

                if (model.Ichorinicarga != null) entity.Ichorinicarga = DateTime.ParseExact(model.Ichorinicarga, Constantes.FormatoFechaFull,
                    CultureInfo.InvariantCulture);

                entity.Ichorini = DateTime.ParseExact(model.Ichorini, Constantes.FormatoFechaFull,
                    CultureInfo.InvariantCulture);
                entity.Ichorfin = DateTime.ParseExact(model.Ichorfin, Constantes.FormatoFechaFull,
                    CultureInfo.InvariantCulture);

                entity.Icdescrip1 = model.Icdescrip1;
                entity.Icdescrip2 = model.Icdescrip2;
                entity.Icdescrip3 = model.Icdescrip3;
                entity.Iccheck1 = model.Iccheck1;

                entity.Icvalor1 = model.Icvalor1;

                entity.Lastuser = base.UserName;
                entity.Lastdate = DateTime.Now;
                entity.Iccheck2 = model.Iccheck2;
                entity.Evenclasecodi = model.Evenclasecodi;

                if (model.Ichor3 != null)
                    entity.Ichor3 = DateTime.ParseExact(model.Ichor3, Constantes.FormatoFechaFull,
                        CultureInfo.InvariantCulture);

                if (model.Ichor4 != null)
                    entity.Ichor4 = DateTime.ParseExact(model.Ichor4, Constantes.FormatoFechaFull,
                        CultureInfo.InvariantCulture);

                entity.Iccheck3 = model.Iccheck3;
                entity.Iccheck4 = model.Iccheck4;
                entity.Icvalor2 = model.Icvalor2;

                //guardar detalle de ieod_cuadro: ieod_cuadro_det
                int id = servicio.Save(entity);

                servicio.DeleteEquipoInvolucrado(id);
                servicio.DeleteEveCongesgdespacho(id);
                servicio.DeleteEveGpsaisladoByIccodi(id);

                //inserta equipos involucrados
                if (model.EquipoInvolucrado != null)
                    servicio.InsertEquipoInvolucrado(id, model.EquipoInvolucrado);

                if (model.GrupoInvolucrado != null)
                    servicio.InsertGrupoInvolucrado(id, model.GrupoInvolucrado, entity, User.Identity.Name);

                if (model.StrAislado != null && model.StrAisladoFlagPpal != null)
                    servicio.InsertListaGpsAislado(id, model.StrAislado, model.StrAisladoFlagPpal, entity, User.Identity.Name);

                return Json(id);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite seleccionar un equipo de acuerdo a su identificador
        /// </summary>
        /// <param name="idEquipo">Código de equipo</param>
        /// <returns>Abrev. equipo, Demanda Hora Punta y Fuera de Punta</returns>
        [HttpPost]
        public JsonResult SeleccEquipo(int idEquipo)
        {
            EveIeodcuadroDTO equipo = this.servicio.ObtenerDatosEquipo(idEquipo);
            return Json(equipo.Famabrev + "," + equipo.DemHP + "," + equipo.DemFP);
        }


        /// <summary>
        /// Permite obtener la información de un equipo
        /// </summary>
        /// <param name="iccodi">Código de equipo</param>
        /// <returns>Datos del equipo</returns>
        [HttpPost]
        public PartialViewResult Equipos(int iccodi)
        {
            OperacionesVariasModel model = new OperacionesVariasModel();
            List<EveIeodcuadroDetDTO> list = this.servicio.GetByCriteria(iccodi);
            model.IeodCuadroDet = list;

            return PartialView(model);
        }

        /// <summary>
        /// Lista de grupos por codigo iccodi
        /// </summary>
        /// <param name="iccodi"></param>
        /// <returns></returns>
        public JsonResult GruposByIccodi(int iccodi)
        {
            OperacionesVariasModel model = new OperacionesVariasModel();

            List<EveCongesgdespachoDTO> listCongestion = this.servicio.ListEveCongesgdespachos();
            model.ListaCongestion = servicio.ListEveCongesgdespachosActivos(iccodi);

            model.ListaGrupoDespachoEdit = this.servicio.ListarGrupo();

            return Json(model);
        }

        /// <summary>
        /// Lista de grupo por empresa y central
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public JsonResult GrupoListado(string idEmpresa, string idCentral)
        {
            BusquedaOperacionesVariasModel model = new BusquedaOperacionesVariasModel();
            model.ListaGrupoDespacho = this.servicio.ListarGrupo();
            return Json(model);
        }

        /// <summary>
        /// Buscar grupo
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public JsonResult BuscarGrupo(int codigo)
        {
            BusquedaOperacionesVariasModel model = new BusquedaOperacionesVariasModel();
            PrGrupoDTO equipo = this.servicio.ObtenerDatosGrupo(codigo);
            model.ObjGrupoDespacho = equipo;
            return Json(model);
        }

        #region Gps

        /// <summary>
        /// Lista de gps por codigo iccodi
        /// </summary>
        /// <param name="iccodi"></param>
        /// <returns></returns>
        public JsonResult GpsByIccodi(int iccodi)
        {
            OperacionesVariasModel model = new OperacionesVariasModel();

            List<EveCongesgdespachoDTO> listCongestion = this.servicio.ListEveCongesgdespachos();
            model.ListaGpsAislado = servicio.GetByCriteriaEveGpsaislados(iccodi);
            model.ListaGps = servicio.ListarGps();

            return Json(model);
        }

        #endregion

        /// <summary>
        /// Permite crear un evento asociado a Operaciones Varias. Debe tener perfil correcto para realizarlo
        /// </summary>
        /// <param name="id">Código de Operaciones Varias</param>
        /// <param name="genIni">Hora inicial</param>
        /// <param name="genFin">Hora final</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEvento(int id, bool genIni, bool genFin)
        {
            if (genIni || genFin)
            {

                bool grabarEvento = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                if (!grabarEvento)
                    return Json(1);

                EveIeodcuadroDTO entity = this.servicio.ObtenerIeodCuadro(id);

                try
                {
                    var horaIni = Convert.ToDateTime(entity.Ichorini).ToString("HH:mm");
                    var horaFin = Convert.ToDateTime(entity.Ichorfin).ToString("HH:mm");
                    var asunto = "";
                    var detalle = "";
                    bool generarDBevento = false;

                    switch (entity.Subcausacodi)
                    {
                        case 202: //OPERACION DE CALDEROS
                            if (genIni)
                                asunto = "A  las " + horaIni + " h entró en servicio.";

                            if (genFin)
                                asunto += " A las " + horaFin + " h salió de servicio.";

                            detalle = entity.Icdescrip1;
                            generarDBevento = true;
                            break;
                        case 208: //POR PRUEBAS (no termoelectrico)
                            asunto = "A las " + horaIni + " h entró en servicio por " + entity.Icdescrip1;
                            detalle = "A las " + horaFin + " h salió de servicio luego de su prueba.";
                            generarDBevento = true;
                            break;
                        case 203: //REGULACION DE TENSION
                                  //asunto = "A las " + horaIni + " h entró en servicio por " + entity.Icdescrip1;
                                  //detalle = "A las " + horaFin + " h salió de servicio luego de operar por regulación de tensión";

                            if (genIni)
                            {
                                asunto = "A las " + horaIni + " h salió de servicio " + entity.Icdescrip1;
                                detalle = "";
                            }
                            if (genFin)
                            {
                                asunto = "A las " + horaFin + " h en servicio luego de operar por regulación de tensión";
                                detalle = "";
                            }
                            //asunto = "A las " + horaIni + " h salió de servicio " + entity.Icdescrip1;
                            //detalle = "A las " + horaFin + " h en servicio luego de operar por regulación de tensión";
                            generarDBevento = true;
                            break;
                        case 205: //RESTRICCIONES OPERATIVAS
                            asunto = "De " + horaIni + " h a " + horaFin + " h, " + entity.Icdescrip1;
                            generarDBevento = true;
                            break;
                        case 206: //SISTEMAS AISLADOS
                            asunto = "De " + horaIni + " h a " + horaFin + " h las centrales " + entity.Icdescrip1;
                            detalle = " operaron en sistema aislado debido a " + entity.Icdescrip2;
                            generarDBevento = true;
                            break;
                    }

                    if (!generarDBevento)
                        return Json(1);

                    try
                    {
                        EquipamientoAppServicio equipoServicio = new EquipamientoAppServicio();
                        EqEquipoDTO equipoEvento = equipoServicio.GetByIdEqEquipo(Convert.ToInt32(entity.Equicodi));

                        EveEventoDTO entity2 = new EveEventoDTO();

                        entity2.Evencodi = 0;
                        entity2.Emprcodirespon = equipoEvento.Emprcodi;
                        entity2.Equicodi = entity.Equicodi;
                        entity2.Evenclasecodi = 1;
                        entity2.Emprcodi = equipoEvento.Emprcodi;
                        entity2.Tipoevencodi = 4;
                        entity2.Evenini = entity.Ichorini;
                        entity2.Evenmwindisp = 0;
                        entity2.Evenfin = entity.Ichorfin;
                        entity2.Subcausacodi = entity.Subcausacodi;
                        entity2.Evenasunto = asunto;
                        entity2.Evenpadre = -1;
                        entity2.Eveninterrup = "N";
                        entity2.Lastuser = base.UserName;
                        entity2.Lastdate = DateTime.Now;
                        entity2.Evendesc = detalle;
                        entity2.Eventension = 0;
                        entity2.Evenaopera = "G";
                        entity2.Evenpreliminar = "S";
                        entity2.Evencomentarios = "";
                        entity2.Evenperturbacion = "N";
                        entity2.Twitterenviado = "N";
                        entity2.Evenpreini = null;
                        entity2.Evenpostfin = null;
                        entity2.Evenrelevante = 0;
                        entity2.Evenctaf = "N";
                        entity2.Eveninffalla = "N";
                        entity2.Eveninffallan2 = "N";
                        entity2.Deleted = "N";
                        entity2.Eventipofalla = null;
                        entity2.Eventipofallafase = null;
                        entity2.Smsenviado = "N";
                        entity2.Smsenviar = "N";
                        entity2.Evenactuacion = "N";
                        entity2.Tiporegistro = null;
                        entity2.Valtiporegistro = null;

                        //crear evento
                        (new EventosAppServicio()).GrabarBitacora(entity2, base.UserName);

                        return Json(1);
                    }
                    catch
                    {
                        return Json(-1);
                    }
                }
                catch
                {
                    return Json(-1);
                }

            }
            else return Json(1);
            
        }

        /// <summary>
        /// Permite eliminar la operacion y todos los registros relacionados
        /// </summary>
        /// <param name="idIccodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int idIccodi)
        {
            try
            {
                (new OperacionesVariasAppServicio()).DeleteOperacion(idIccodi);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        public JsonResult EliminarMasivo(string idIccodis)
        {
            try
            {
                string[] iccodiCopia = idIccodis.Split(new char[] { ',' });

                foreach (string iccodi1 in iccodiCopia)
                {
                    (new OperacionesVariasAppServicio()).DeleteOperacion(Int32.Parse(iccodi1));
                }
                return Json(1);

            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite copiar una fecha al modelo
        /// </summary>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Copiar(string fechaFin)
        {
            BusquedaOperacionesVariasModel model = new BusquedaOperacionesVariasModel();
            model.Fechafin = fechaFin;
            return PartialView(model);
        }

        /// <summary>
        /// Retorna PartialView
        /// </summary>
        /// <param name="id"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CopiarHorizonte(int id, string txt)
        {
            return PartialView();
        }

        static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                yield return date;
            }
        }


        /// <summary>
        /// Listado de Operaciones Varias según Clase, SubCausa, Fecha inicial, Fecha final y número de página
        /// </summary>
        /// <param name="evenClase">Clase</param>
        /// <param name="subCausacodi">Subclase</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="nroPage">Nro. de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int evenClase, int subCausacodi, string fechaIni, string fechaFin, int nroPage)
        {
            BusquedaOperacionesVariasModel model = new BusquedaOperacionesVariasModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;
            int tipoOpera;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            CortoPlazoAppServicio servCortoPlazo = new CortoPlazoAppServicio();

            //List < PrCongestionDTO > temp = servCortoPlazo.ObtenerCapacidadNominal(fechaInicio, "1");
            List<PrCongestionDTO> totalList = new List<PrCongestionDTO>();




            this.FechaInicio = fechaInicio;
            this.FechaFinal = fechaFinal;
            this.Horizonte = evenClase.ToString();
            this.TipoOperacion = subCausacodi.ToString();

            model.ListaIeodcuadro = servicio.BuscarOperaciones(evenClase, subCausacodi, fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento).ToList();

            
            if((evenClase == 2 || evenClase == 3) && (subCausacodi == 201|| subCausacodi == 0))
            {
                string horizonte = evenClase == 2 ? ConstantesCortoPlazo.TopologiaDiario : ConstantesCortoPlazo.TopologiaSemanal;
                foreach (DateTime date in GetDateRange(fechaInicio, fechaFinal))
                {

                    List<PrCongestionDTO> temp = servCortoPlazo.ObtenerCapacidadNominal(date, horizonte);
                    foreach (PrCongestionDTO congestion in temp)
                    {
                        totalList.Add(congestion);
                        EveIeodcuadroDTO tempEve = new EveIeodcuadroDTO();
                        tempEve.Equicodi = congestion.Equicodi;
                        tempEve.Ichorini = congestion.Congesfecinicio;
                        tempEve.Ichorfin = congestion.Congesfecfin;
                        tempEve.Areanomb = congestion.Areanomb;
                        tempEve.Famabrev = "LINEA";
                        tempEve.Equiabrev = congestion.Equinomb;
                        tempEve.Emprnomb = congestion.Emprnomb;
                        tempEve.Subcausacodi = 201;
                        tempEve.Subcausadesc = "CONGESTIÓN";
                        tempEve.Lastuser = "AUTO";
                        tempEve.Evenclasecodi = evenClase;
                        
                        

                        model.ListaIeodcuadro.Add(tempEve);
                    }
                }
            }


            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);

        }


        /// <summary>
        /// Listado de Operaciones Varias según Clase, SubCausa, Fecha inicial, Fecha final
        /// </summary>
        /// <param name="evenClase">Clase</param>
        /// <param name="subCausacodi">Subclase</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="nombreOperacion">Nombre de la Operación</param>
        /// <param name="evenClaseDescripcion">Descripción de la clase</param>
        /// <returns>1: ok, -1: error</returns>
        [HttpPost]
        public JsonResult ListaSinPaginado(int evenClase, int subCausacodi, string fechaIni, string fechaFin,
            string nombreOperacion, string evenClaseDescripcion)
        {
            BusquedaOperacionesVariasModel model = new BusquedaOperacionesVariasModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;
            int result = 1;

            try
            {
                if (fechaIni != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFin != null)
                {
                    fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }


                model.ListaIeodcuadro =
                    servicio.BuscarOperacionesSinPaginado(evenClase, subCausacodi, fechaInicio, fechaFinal).ToList();
                ExcelDocument.GenerarArchivoOperacionesVarias(model.ListaIeodcuadro, fechaIni, fechaFin,
                    evenClaseDescripcion, subCausacodi, nombreOperacion);

                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns>Archivo descargado</returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento + NombreArchivo.ReporteOperaciones;
            return File(file, Constantes.AppExcel, NombreArchivo.ReporteOperaciones);
        }


        /// <summary>
        /// Permite realizar el paginado de un listado
        /// </summary>
        /// <param name="evenClase">Clase</param>
        /// <param name="subCausacodi">Subcausa</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int evenClase, int subCausacodi, string fechaIni, string fechaFin)
        {
            Paginacion model = new Paginacion();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fechaFinal = fechaFinal.AddDays(1);

            int nroRegistros = servicio.ObtenerNroFilas(evenClase, subCausacodi, fechaInicio, fechaFinal, Constantes.NroPageShow, Constantes.PageSizeEvento);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }



        /// <summary>
        /// Permite realizar el copiado de bloque hasta para una semana
        /// </summary>
        /// <param name="model">Modelo del tipo Operaciones Varias</param>
        /// <returns>Número de registros creados. -1 si hay error</returns>
        [HttpPost]
        public JsonResult CopiarBloqueSemana(BusquedaOperacionesVariasModel model)
        {
            EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

            string fechas = "";
            string horainis = "";
            string horafins = "";

            if (model.Copiar1 == "S")
            {
                fechas += model.Fecha1 + ",";
                horainis += model.Horaini1 + ",";
                horafins += model.Horafin1 + ",";
            }
            if (model.Copiar2 == "S")
            {
                fechas += model.Fecha2 + ",";
                horainis += model.Horaini2 + ",";
                horafins += model.Horafin2 + ",";
            }
            if (model.Copiar3 == "S")
            {
                fechas += model.Fecha3 + ",";
                horainis += model.Horaini3 + ",";
                horafins += model.Horafin3 + ",";
            }
            if (model.Copiar4 == "S")
            {
                fechas += model.Fecha4 + ",";
                horainis += model.Horaini4 + ",";
                horafins += model.Horafin4 + ",";
            }
            if (model.Copiar5 == "S")
            {
                fechas += model.Fecha5 + ",";
                horainis += model.Horaini5 + ",";
                horafins += model.Horafin5 + ",";
            }
            if (model.Copiar6 == "S")
            {
                fechas += model.Fecha6 + ",";
                horainis += model.Horaini6 + ",";
                horafins += model.Horafin6 + ",";
            }
            if (model.Copiar7 == "S")
            {
                fechas += model.Fecha7 + ",";
                horainis += model.Horaini7 + ",";
                horafins += model.Horafin7 + ",";
            }

            int registros = 0;

            try
            {

                string[] iccodiCopia = model.Iccodis.Split(new char[] { ',' });
                string[] fechasSeleccion = fechas.Split(new char[] { ',' });
                string[] horainisSeleccion = horainis.Split(new char[] { ',' });
                string[] horafinsSeleccion = horafins.Split(new char[] { ',' });


                foreach (string iccodi1 in iccodiCopia)
                {

                    int iccodi;

                    try
                    {
                        iccodi = Convert.ToInt32(iccodi1);
                    }
                    catch
                    {
                        iccodi = 0;
                    }

                    if (iccodi == 0)
                        continue;


                    int indice = 0;

                    OperacionesVariasModel modelACopiar = new OperacionesVariasModel();
                    modelACopiar.IeodCuadro = this.servicio.ObtenerIeodCuadro(iccodi);
                    modelACopiar.IeodCuadroDet = this.servicio.GetByCriteria(iccodi);

                    foreach (string fecha in fechasSeleccion)
                    {

                        if (fecha != "")
                        {

                            entity = new EveIeodcuadroDTO();

                            entity.Iccodi = 0;
                            entity.Equicodi = modelACopiar.IeodCuadro.Equicodi;
                            entity.Subcausacodi = modelACopiar.IeodCuadro.Subcausacodi;

                            entity.Ichorini = DateTime.ParseExact(fecha + " " + horainisSeleccion[indice] + ":00",
                                Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                            if (horafinsSeleccion[indice] == "24:00")
                                entity.Ichorfin =
                                    DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture)
                                        .AddDays(1);
                            else
                                entity.Ichorfin = DateTime.ParseExact(fecha + " " + horafinsSeleccion[indice] + ":00",
                                    Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);


                            entity.Iccheck1 = modelACopiar.IeodCuadro.Iccheck1;
                            entity.Icvalor1 = modelACopiar.IeodCuadro.Icvalor1;
                            entity.Iccheck2 = modelACopiar.IeodCuadro.Iccheck2;
                            entity.Evenclasecodi = modelACopiar.IeodCuadro.Evenclasecodi;
                            entity.Ichor3 = modelACopiar.IeodCuadro.Ichor3;
                            entity.Ichor4 = modelACopiar.IeodCuadro.Ichor4;
                            entity.Iccheck3 = modelACopiar.IeodCuadro.Iccheck3;
                            entity.Iccheck4 = modelACopiar.IeodCuadro.Iccheck4;
                            entity.Icvalor2 = modelACopiar.IeodCuadro.Icvalor2;


                            if (model.Copiartexto == "S")
                            {
                                entity.Icdescrip1 = modelACopiar.IeodCuadro.Icdescrip1;
                                entity.Icdescrip2 = modelACopiar.IeodCuadro.Icdescrip2;
                                entity.Icdescrip3 = modelACopiar.IeodCuadro.Icdescrip3;
                            }

                            entity.Lastuser = base.UserName;
                            entity.Lastdate = DateTime.Now;

                            int id2 = this.servicio.Save(entity);

                            //copia detalle de equipo (eve_ieodcuadro_det)
                            foreach (EveIeodcuadroDetDTO entityDetalle in modelACopiar.IeodCuadroDet)
                            {
                                EveIeodcuadroDetDTO entityCopiar = new EveIeodcuadroDetDTO();

                                entityCopiar.Iccodi = id2;
                                entityCopiar.Equicodi = entityDetalle.Equicodi;
                                entityCopiar.Icdetcheck1 = entityDetalle.Icdetcheck1;
                                this.servicio.SaveDetalle(entityCopiar);
                            }

                            registros++;
                        }

                        indice++;
                    }
                }

                return Json(registros);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite copiar Operaciones Varias para el día siguiente
        /// </summary>
        /// <param name="model">Modelo</param>
        /// <returns>Número de registros creados. -1 si hay error</returns>
        [HttpPost]
        public JsonResult CopiarBloqueDia(BusquedaOperacionesVariasModel model)
        {
            EveIeodcuadroDTO entity = new EveIeodcuadroDTO();
            int registros = 0;

            try
            {

                string[] iccodiCopia = model.Iccodis.Split(new char[] { ',' });
                foreach (string iccodi1 in iccodiCopia)
                {
                    int iccodi;

                    try
                    {
                        iccodi = Convert.ToInt32(iccodi1);
                    }
                    catch
                    {
                        iccodi = 0;
                    }

                    if (iccodi == 0)
                        continue;

                    int indice = 0;

                    OperacionesVariasModel modelACopiar = new OperacionesVariasModel();
                    modelACopiar.IeodCuadro = this.servicio.ObtenerIeodCuadro(iccodi);
                    modelACopiar.IeodCuadroDet = this.servicio.GetByCriteria(iccodi);

                    entity = new EveIeodcuadroDTO();

                    entity.Iccodi = 0;
                    entity.Equicodi = modelACopiar.IeodCuadro.Equicodi;
                    entity.Subcausacodi = modelACopiar.IeodCuadro.Subcausacodi;

                    entity.Ichorini = ((DateTime)modelACopiar.IeodCuadro.Ichorini).AddDays(1);
                    entity.Ichorfin = ((DateTime)modelACopiar.IeodCuadro.Ichorfin).AddDays(1);

                    entity.Iccheck1 = modelACopiar.IeodCuadro.Iccheck1;
                    entity.Icvalor1 = modelACopiar.IeodCuadro.Icvalor1;
                    entity.Iccheck2 = modelACopiar.IeodCuadro.Iccheck2;
                    entity.Evenclasecodi = modelACopiar.IeodCuadro.Evenclasecodi;
                    entity.Ichor3 = modelACopiar.IeodCuadro.Ichor3;
                    entity.Ichor4 = modelACopiar.IeodCuadro.Ichor4;
                    entity.Iccheck3 = modelACopiar.IeodCuadro.Iccheck3;
                    entity.Iccheck4 = modelACopiar.IeodCuadro.Iccheck4;
                    entity.Icvalor2 = modelACopiar.IeodCuadro.Icvalor2;

                    if (model.Copiartexto == "S")
                    {
                        entity.Icdescrip1 = modelACopiar.IeodCuadro.Icdescrip1;
                        entity.Icdescrip2 = modelACopiar.IeodCuadro.Icdescrip2;
                        entity.Icdescrip3 = modelACopiar.IeodCuadro.Icdescrip3;
                    }

                    entity.Lastuser = base.UserName;
                    entity.Lastdate = DateTime.Now;

                    int id2 = this.servicio.Save(entity);

                    //copia detalle de equipo (eve_ieodcuadro_det)
                    foreach (EveIeodcuadroDetDTO entityDetalle in modelACopiar.IeodCuadroDet)
                    {
                        EveIeodcuadroDetDTO entityCopiar = new EveIeodcuadroDetDTO();
                        entityCopiar.Iccodi = id2;
                        entityCopiar.Equicodi = entityDetalle.Equicodi;
                        entityCopiar.Icdetcheck1 = entityDetalle.Icdetcheck1;
                        this.servicio.SaveDetalle(entityCopiar);

                    }

                    registros++;
                    indice++;
                }

                return Json(registros);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Copiado de Operaciones Varias
        /// </summary>
        /// <param name="evenClasecodiOrigen">Clase origen a copiar</param>
        /// <param name="evenClasecodiDestino">Clase destino</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha inicial</param>
        /// <param name="subCausacodi">Subcausa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarRegistrosHorizonteHorizonte(int evenClasecodiOrigen, int evenClasecodiDestino,
            string fechaIni, string fechaFin, int subCausacodi)
        {
            BusquedaOperacionesVariasModel model = new BusquedaOperacionesVariasModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;
            int result = 1;

            int registros = 0;
            try
            {

                if (fechaIni != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFin != null)
                {
                    fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }


                model.ListaIeodcuadro =
                    servicio.BuscarOperacionesSinPaginado(evenClasecodiOrigen, subCausacodi, fechaInicio, fechaFinal)
                        .ToList();

                foreach (EveIeodcuadroDTO modelACopiar in model.ListaIeodcuadro)
                {

                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

                    entity.Iccodi = 0;
                    entity.Equicodi = modelACopiar.Equicodi;
                    entity.Subcausacodi = modelACopiar.Subcausacodi;
                    entity.Ichorini = ((DateTime)modelACopiar.Ichorini);
                    entity.Ichorfin = ((DateTime)modelACopiar.Ichorfin);
                    entity.Iccheck1 = modelACopiar.Iccheck1;
                    entity.Icvalor1 = modelACopiar.Icvalor1;
                    entity.Iccheck2 = modelACopiar.Iccheck2;
                    entity.Evenclasecodi = evenClasecodiDestino;
                    entity.Ichor3 = modelACopiar.Ichor3;
                    entity.Ichor4 = modelACopiar.Ichor4;
                    entity.Iccheck3 = modelACopiar.Iccheck3;
                    entity.Iccheck4 = modelACopiar.Iccheck4;
                    entity.Icvalor2 = modelACopiar.Icvalor2;
                    entity.Icdescrip1 = modelACopiar.Icdescrip1;
                    entity.Icdescrip2 = modelACopiar.Icdescrip2;
                    entity.Lastuser = base.UserName;
                    entity.Lastdate = DateTime.Now;

                    int id = this.servicio.Save(entity);
                    registros++;
                }

                result = registros;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }


        /// <summary>
        /// Copiado de Operaciones Varias de Fecha origen a Fecha destino
        /// </summary>
        /// <param name="evenClasecodiOrigen">Clase origen</param>
        /// <param name="evenClasecodiDestino">Clase destino</param>
        /// <param name="fechaOrigen">Fecha original</param>
        /// <param name="fechaDestino">Fecha de copiado</param>
        /// <param name="subCausacodi">Especificación de la subcausa</param>
        /// <returns>Número de registros creados.  -1 si hay error</returns>
        [HttpPost]
        public JsonResult CopiarRegistrosHorizonteyFecha(int evenClasecodiOrigen, int evenClasecodiDestino,
            string fechaOrigen, string fechaDestino, int subCausacodi)
        {

            BusquedaOperacionesVariasModel model = new BusquedaOperacionesVariasModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;
            DateTime fechaFinal2 = DateTime.Now;
            DateTime Ichorini = DateTime.Now;
            DateTime Ichorfin = DateTime.Now;
            int result = 1;

            int registros = 0;
            try
            {

                if (fechaOrigen != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaOrigen, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaDestino != null)
                {
                    fechaFinal = DateTime.ParseExact(fechaDestino, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }


                model.ListaIeodcuadro =
                    servicio.BuscarOperacionesSinPaginado(evenClasecodiOrigen, subCausacodi, fechaInicio,
                        fechaInicio.AddDays(1)).ToList();

                foreach (EveIeodcuadroDTO modelACopiar in model.ListaIeodcuadro)
                {
                    Ichorini = (DateTime) modelACopiar.Ichorini;
                    Ichorfin = (DateTime) modelACopiar.Ichorfin;

                    if (Ichorfin.ToLongDateString() != Ichorini.ToLongDateString()) fechaFinal2 = fechaFinal.AddDays(1);
                    else fechaFinal2 = fechaFinal;

                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

                    entity.Iccodi = 0;
                    entity.Equicodi = modelACopiar.Equicodi;
                    entity.Subcausacodi = modelACopiar.Subcausacodi;
                    entity.Ichorini =
                        DateTime.ParseExact(
                            fechaFinal.ToString(Constantes.FormatoFecha) + " " +
                            ((DateTime)modelACopiar.Ichorini).ToString(Constantes.FormatoHora),
                            Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    entity.Ichorfin =
                        DateTime.ParseExact(
                            fechaFinal2.ToString(Constantes.FormatoFecha) + " " +
                            ((DateTime)modelACopiar.Ichorfin).ToString(Constantes.FormatoHora),
                            Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    entity.Iccheck1 = modelACopiar.Iccheck1;
                    entity.Icvalor1 = modelACopiar.Icvalor1;
                    entity.Iccheck2 = modelACopiar.Iccheck2;
                    entity.Evenclasecodi = evenClasecodiDestino;
                    entity.Ichor3 = modelACopiar.Ichor3;
                    entity.Ichor4 = modelACopiar.Ichor4;
                    entity.Iccheck3 = modelACopiar.Iccheck3;
                    entity.Iccheck4 = modelACopiar.Iccheck4;
                    entity.Icvalor2 = modelACopiar.Icvalor2;
                    entity.Lastuser = base.UserName;
                    entity.Lastdate = DateTime.Now;
                    entity.Icdescrip1 = modelACopiar.Icdescrip1;
                    int id = this.servicio.Save(entity);
                    registros++;
                }

                result = registros;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        #region Relación de Area Usuario y SubcausaEvento

        /// <summary>
        /// Index Relacion
        /// </summary>
        /// <returns></returns>
        public ActionResult RelacionAreaYTipoOperacion()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            bool accesoNuevo = true; // Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool accesoEditar = true; // Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);

            RelacionAreaSubcausaModel model = new RelacionAreaSubcausaModel();
            model.AccesoEditar = accesoNuevo;
            model.AccesoNuevo = accesoEditar;

            List<int> areasCoes = ConstantesEvento.AreacoesParaVisualizacion.Split(',').Select(x => int.Parse(x)).ToList();
            model.ListaArea = this.servicio.ListarAreaByListacodi(areasCoes);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            StringBuilder json2 = new StringBuilder();
            serializer.Serialize(model.ListaArea, json2);
            model.ListaAreaJson = json2.ToString();

            return View(model);
        }

        /// <summary>
        /// Listar conceptos
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoSubcausa()
        {
            if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

            bool accesoNuevo = true; // Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool accesoEditar = true; // Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);

            RelacionAreaSubcausaModel model = new RelacionAreaSubcausaModel();
            model.AccesoEditar = accesoNuevo;
            model.AccesoNuevo = accesoEditar;

            model.ListaEvensubcausa = this.servicio.ListarSubcausaeventoConfigurado();

            return PartialView(model);
        }

        /// <summary>
        /// Listar las area relacionadas por subcausa
        /// </summary>
        /// <param name="subcausacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaConfiguracionXSubcausa(int subcausacodi)
        {
            RelacionAreaSubcausaModel model = new RelacionAreaSubcausaModel();
            try
            {
                if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

                model.ListaRelacionAreaSubcausa = this.servicio.GetByCriteriaEveAreaSubcausaeventos(subcausacodi, ConstantesEvento.RelacionActivo.ToString());

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Registro / Edicion de Relacion Area usuario y Subcausa
        /// </summary>
        /// <param name="subcausacodi"></param>
        /// <param name="areacodis"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConfiguracionAreaSubcausaGuardar(int subcausacodi, List<int> listaArea)
        {
            RelacionAreaSubcausaModel model = new RelacionAreaSubcausaModel();
            try
            {
                if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

                //Validaciones
                if (subcausacodi <= 0)
                {
                    throw new Exception("Debe seleccionar un Tipo de operación.");
                }

                listaArea = listaArea == null ? new List<int>() : listaArea;

                this.servicio.GuardarRelacionAreaSubcausa(subcausacodi, listaArea, User.Identity.Name);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        #endregion
    }
}
