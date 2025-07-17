using COES.MVC.Intranet.Areas.RegistroIntegrante.Models;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using COES.MVC.Intranet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using log4net;
using System.Configuration;
using System.IO;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Controllers
{
    public class SolicitudController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(SolicitudController));

        /// <summary>
        /// Instancia de la clase de aplicación
        /// </summary>
        EmpresaAppServicio servicio = new EmpresaAppServicio();

        public SolicitudController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("SolicitudController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("SolicitudController", ex);
                throw;
            }
        }


        /// <summary>
        /// Instancia de la clase GestionSolicitudesAppServicio
        /// </summary>
        GestionSolicitudesAppServicio appSolicitud = new GestionSolicitudesAppServicio();

        /// <summary>
        /// Instancia de la clase RepresentanteAppServicio
        /// </summary>
        RepresentanteAppServicio appRepresentante = new RepresentanteAppServicio();

        /// <summary>
        /// Instancia de la clase TipoAppServicio
        /// </summary>
        TipoAppServicio appTipo = new TipoAppServicio();

        /// <summary>
        /// Instancia de la clase EmpresaAppServicio
        /// </summary>
        private EmpresaAppServicio appEmpresa = new EmpresaAppServicio();

        /// <summary>
        /// Permite retornar el model correspondiente
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SolicitudPendienteModel model = new SolicitudPendienteModel();
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                //Consultar usuario directivo
                model.esUsuarioDirectivo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            }
            else
            {
                model.esUsuarioDirectivo = false;
            }

            model.ListaEstadoSolicitud.Add(new EstadoSolicitudDTO()
            {
                Soliestado = ConstantesRegistroIntegrantes.SoliPendiente,
                Soliestadocodi = ConstantesRegistroIntegrantes.SoliPendienteCodi
            });
            model.ListaEstadoSolicitud.Add(new EstadoSolicitudDTO()
            {
                Soliestado = ConstantesRegistroIntegrantes.SoliAprobadoDigital,
                Soliestadocodi = ConstantesRegistroIntegrantes.SoliAprobadoDigitalCodigo
            });
            model.ListaEstadoSolicitud.Add(new EstadoSolicitudDTO()
            {
                Soliestado = ConstantesRegistroIntegrantes.SoliAprobadoFisica,
                Soliestadocodi = ConstantesRegistroIntegrantes.SoliAprobadoFisicaCodigo
            });
            model.ListaEstadoSolicitud.Add(new EstadoSolicitudDTO()
            {
                Soliestado = ConstantesRegistroIntegrantes.SoliDenegada,
                Soliestadocodi = ConstantesRegistroIntegrantes.SoliDenegadaCodi
            });
            model.ListaEstadoSolicitud.Add(new EstadoSolicitudDTO()
            {
                Soliestado = ConstantesRegistroIntegrantes.SoliTodos,
                Soliestadocodi = ConstantesRegistroIntegrantes.SoliTodosCodi
            });
            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="soliestado">estado de la solicitud</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string soliestado)
        {
            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            int nroRegistros = this.appSolicitud.ObtenerTotalRegListPend(soliestado);

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
        /// Permite pintar la lista de registros
        /// </summary>
        /// <param name="soliestado">estado de la solicitud</param>
        /// <param name="nroPagina">nro de pagina</param>
        /// <returns></returns>
        public ActionResult Listado(string soliestado, int nroPagina)
        {
            List<RiSolicituddetalleDTO> ListDetalles = new List<RiSolicituddetalleDTO>();
            SolicitudPendienteModel model = new SolicitudPendienteModel();
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                //Consultar usuario directivo
                model.esUsuarioDirectivo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            }
            else
            {
                model.esUsuarioDirectivo = false;
            }

            model.ListaSolicitudes = this.appSolicitud.ListarSolicitudesPendientes(soliestado, nroPagina,
                Constantes.PageSize);

            foreach (RiSolicitudDTO item in model.ListaSolicitudes)
            {
                if (item.Tisocodi == 1)
                {
                    ListDetalles = this.appSolicitud.ListDetalleBySolicodi(item.Solicodi);
                    foreach (RiSolicituddetalleDTO det in ListDetalles)
                    {
                        if (det.Sdetcampo == ConstantesRegistroIntegrantes.Emprrazsocial)
                        {
                            item.Emprrazsocial = det.Sdetvalor;
                        }
                        else if (det.Sdetcampo == ConstantesRegistroIntegrantes.Emprnombcomercial)
                        {
                            item.EmprnombComercial = det.Sdetvalor;
                        }
                        else if (det.Sdetcampo == ConstantesRegistroIntegrantes.Emprsigla)
                        {
                            item.Emprsigla = det.Sdetvalor;
                        }
                    }
                }
            }

            return View(model);
        }


        /// <summary>
        /// Permite dar Notificar a una solicitud
        /// </summary>
        /// <param name="solicodi">codigo de solicitud</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DarNotificar(int solicodi)
        {
            try
            {
                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

                RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
                objSolicitud = this.appSolicitud.GetById(solicodi);

                //Dar notiifcar -> cambia a estado PRE-APROBADO (aprobado digitalmente)
                this.appSolicitud.DarNotificar(solicodi);
                
                ///Envio de Correo
                string toEmail = string.Empty;
                string ccEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];
                string msg = string.Empty;
                string solicitud = string.Empty;
                string representanteLegal = string.Empty;
                string empresa = string.Empty;
                string observacion = string.Empty;
                bool aceptado = false;

                var appEmpresa = new EmpresaAppServicio();
                var model = appEmpresa.GetByIdSiEmpresa((int)objSolicitud.Emprcodi);

                //Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
                empresa = (model.Emprnombrecomercial.Length > 3) ? model.Emprnombrecomercial : model.Emprrazsocial;
                //fin Garantia 9Jul18 

                var ListaTipoSolicitud = appSolicitud.ListarTipoSolicitud();
                solicitud = ListaTipoSolicitud.Where(x => x.Tisocodi == objSolicitud.Tisocodi).FirstOrDefault().Tisonombre.ToString();

                aceptado = (objSolicitud.SoliestadoInterno == ConstantesRegistroIntegrantes.SoliAprobadoDigitalCodigo) ? true : false;
                observacion = (objSolicitud.Soliobservacion == null) ? string.Empty : objSolicitud.Soliobservacion;

                List<SiRepresentanteDTO> listacorreos = new List<SiRepresentanteDTO>();

                if (objSolicitud.Tisocodi == ConstantesRegistroIntegrantes.SoliCambioRepresentante) //REPRESENTANTE LEGAL
                {
                    List<SiRepresentanteDTO> DatosRepresentante;
                    DatosRepresentante = ObtenerDatosRepresentante(solicodi);
                    foreach (SiRepresentanteDTO item in DatosRepresentante)
                    {
                        if (item.Accion == ConstantesRegistroIntegrantes.DetalleSolicitudAgregado) listacorreos.Add(item);
                    }

                }
                else
                {
                    listacorreos = appRepresentante.GetByEmpresaSiRepresentante((int)objSolicitud.Emprcodi).
                        Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoLegal).
                        Where(x => x.Rptebaja.ToString() == ConstantesRegistroIntegrantes.RpteBajaNo).ToList();
                }


                foreach (var item in listacorreos)
                {
                    representanteLegal = item.Rpteapellidos + ", " + item.Rptenombres;
                    toEmail = item.Rptecorreoelectronico;
                    try
                    {
                        if (aceptado)
                            msg = RegistroIntegrantesHelper.Solicitud_BodyMailAceptado(representanteLegal, empresa, solicitud);
                        else
                            msg = RegistroIntegrantesHelper.Solicitud_BodyMailDenegado(representanteLegal, empresa, solicitud, observacion);

                        log.Info("Solicitud - Dar Notificar - Envío de correo a representante legal");
                        COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, "Notificacion de Solicitud: " + solicitud + " - Registro de Integrantes", msg);

                    }
                    catch (Exception ex)
                    {
                        log.Error("Error", ex);
                    }

                }

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite dar Conformidad a una solicitud
        /// </summary>
        /// <param name="solicodi">codigo de solicitud</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DarConformidad(int solicodi)
        {
            try
            {
                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

                RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
                objSolicitud = this.appSolicitud.GetById(solicodi);
                String view = String.Empty;

                //Dar conformidad -> cambia a estado APROBADO (aprobado fisicamente)

                switch (objSolicitud.Tisocodi)
                {
                    case ConstantesRegistroIntegrantes.SoliCambioDenominacion:
                        this.appSolicitud.DarConformidad(solicodi);
                        break;
                    case ConstantesRegistroIntegrantes.SoliCambioTipo:
                        {
                            List<SiTipoComportamientoDTO> DatosTipo;
                            DatosTipo = ObtenerDatosTipo(solicodi);
                            foreach (SiTipoComportamientoDTO item in DatosTipo)
                            {
                                SiTipoComportamientoDTO tipo = new SiTipoComportamientoDTO();

                                if (item.Accion == ConstantesRegistroIntegrantes.DetalleSolicitudAgregado)
                                {
                                    tipo = item;
                                    tipo.Emprcodi = objSolicitud.Emprcodi;
                                    tipo.Tipolineatrans138 = tipo.Tipolineatrans138 == "true" ? "S" : "N";
                                    tipo.Tipolineatrans220 = tipo.Tipolineatrans220 == "true" ? "S" : "N";
                                    tipo.Tipolineatrans500 = tipo.Tipolineatrans500 == "true" ? "S" : "N";
                                    tipo.Tipousucreacion = userLogin.UserCode.ToString();
                                    //tipo.Tipousucreacion = "fit"; //test
                                    tipo.Tipofeccreacion = DateTime.Now;
                                    tipo.Tipoinicial = ConstantesRegistroIntegrantes.TipoAgenteInicialNo;
                                    tipo.Tipobaja = ConstantesRegistroIntegrantes.RpteBajaNo;
                                    appTipo.InsertSiTipo(tipo);
                                }
                                else if (item.Accion == ConstantesRegistroIntegrantes.DetalleSolicitudBaja)
                                {
                                    tipo = this.appTipo.GetByIdSiTipo(item.Tipocodi);
                                    tipo.Tipousumodificacion = userLogin.UserCode.ToString();
                                    //tipo.Tipousumodificacion = "fit"; //test
                                    tipo.Tipofecmodificacion = DateTime.Now;

                                    tipo.Tipobaja = ConstantesRegistroIntegrantes.RpteBajaSi;

                                    appTipo.UpdateSiTipo(tipo);
                                }
                            }
                            this.appSolicitud.DarConformidad(solicodi);
                        }
                        break;
                    case ConstantesRegistroIntegrantes.SoliCambioRepresentante:
                        {
                            List<SiRepresentanteDTO> DatosRepresentante;
                            DatosRepresentante = ObtenerDatosRepresentante(solicodi);
                            foreach (SiRepresentanteDTO item in DatosRepresentante)
                            {

                                SiRepresentanteDTO representante = new SiRepresentanteDTO();

                                if (item.Accion == ConstantesRegistroIntegrantes.DetalleSolicitudAgregado)
                                {
                                    representante = item;
                                    representante.Emprcodi = objSolicitud.Emprcodi;
                                    //representante.Rptetiprepresentantelegal = ConstantesRegistroIntegrantes.RepresentanteLegalTipoAlterno;
                                    representante.Rpteusucreacion = userLogin.UserCode.ToString();
                                    //representante.Rpteusucreacion = "fit"; //test
                                    representante.Rptefeccreacion = DateTime.Now;
                                    representante.Rptetipo = ConstantesRegistroIntegrantes.RepresentanteTipoLegal;
                                    representante.Rptebaja = ConstantesRegistroIntegrantes.RpteBajaNo;
                                    representante.Rpteinicial = ConstantesRegistroIntegrantes.RepresentanteInicialNo;

                                    appRepresentante.InsertSiRepresentante(representante);
                                }
                                else if (item.Accion == ConstantesRegistroIntegrantes.DetalleSolicitudBaja)
                                {
                                    representante = this.appRepresentante.GetByIdSiRepresentante(item.Rptecodi);

                                    representante.Rpteusumodificacion = userLogin.UserCode.ToString();
                                    //representante.Rpteusumodificacion = "fit"; //test
                                    representante.Rptefecmodificacion = DateTime.Now;

                                    representante.Rptebaja = ConstantesRegistroIntegrantes.RpteBajaSi;

                                    appRepresentante.UpdateSiRepresentante(representante);
                                }
                            }
                            this.appSolicitud.DarConformidad(solicodi);

                            //Actualizar Fecha Proceso, porque se ha hecho efectivo, es un proceso automático
                            int usuario = (int)userLogin.UserCode;
                            DateTime fecha = DateTime.Now;
                            this.appSolicitud.ActualizarFechaProceso(solicodi, fecha, usuario);
                        }
                        break;
                    case ConstantesRegistroIntegrantes.SoliBajaEmpresa:
                        this.appSolicitud.DarConformidad(solicodi);
                        break;
                    case ConstantesRegistroIntegrantes.SoliFusionEmpresa:
                        this.appSolicitud.DarConformidad(solicodi);
                        break;
                }

                /////Envio de Correo
                //string toEmail = string.Empty;                
                //string ccEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];
                //string msg = string.Empty;
                //string solicitud = string.Empty;
                //string representanteLegal = string.Empty;
                //string empresa = string.Empty;
                //string observacion = string.Empty;
                //bool aceptado = false;

                //var appEmpresa = new EmpresaAppServicio();
                //var model = appEmpresa.GetByIdSiEmpresa((int)objSolicitud.Emprcodi);

                ////Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
                //empresa = (model.Emprnombrecomercial.Length > 3) ? model.Emprnombrecomercial : model.Emprrazsocial;
                ////fin Garantia 9Jul18 

                //var ListaTipoSolicitud = appSolicitud.ListarTipoSolicitud();                
                //solicitud = ListaTipoSolicitud.Where(x => x.Tisocodi == objSolicitud.Tisocodi).FirstOrDefault().Tisonombre.ToString();

                //aceptado = (objSolicitud.SoliestadoInterno == ConstantesRegistroIntegrantes.SoliAceptada) ? true: false;
                //observacion = (objSolicitud.Soliobservacion==null)?string.Empty: objSolicitud.Soliobservacion;

                //var lista = appRepresentante.GetByEmpresaSiRepresentante((int)objSolicitud.Emprcodi).
                //    Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoLegal).
                //    Where(x => x.Rptebaja.ToString() == ConstantesRegistroIntegrantes.RpteBajaNo).ToList();

                //foreach (var item in lista)
                //{
                //    representanteLegal = item.Rpteapellidos + ", " + item.Rptenombres;
                //    toEmail = item.Rptecorreoelectronico;
                //    try
                //    {                        
                //        if (aceptado)
                //            msg = RegistroIntegrantesHelper.Solicitud_BodyMailAceptado(representanteLegal, empresa, solicitud);
                //        else
                //            msg = RegistroIntegrantesHelper.Solicitud_BodyMailDenegado(representanteLegal, empresa, solicitud, observacion);

                //        log.Info("Solicitud - Dar conformidad - Envío de correo a representante legal");
                //        COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, "Notificacion de Solicitud: " + solicitud + " - Registro de Integrantes", msg);

                //    }
                //    catch (Exception ex)
                //    {
                //        log.Error("Error", ex);
                //    }

                //}      

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite verificar el tipo de una solicitud
        /// </summary>
        /// <param name="solicodi">codigo de solictud</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult verificarTipoSoli(int solicodi)
        {
            RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
            try
            {
                objSolicitud = this.appSolicitud.GetById(solicodi);
                String view = String.Empty;
                switch (objSolicitud.Tisocodi)
                {
                    case ConstantesRegistroIntegrantes.SoliCambioDenominacion:
                        view = ConstantesRegistroIntegrantes.SolCambioDenominacionView;
                        break;
                    case ConstantesRegistroIntegrantes.SoliCambioRepresentante:
                        view = ConstantesRegistroIntegrantes.SolCambioRepresentanteView;
                        break;
                    case ConstantesRegistroIntegrantes.SoliBajaEmpresa:
                        view = ConstantesRegistroIntegrantes.SolBajaEmpresaView;
                        break;
                    case ConstantesRegistroIntegrantes.SoliFusionEmpresa:
                        view = ConstantesRegistroIntegrantes.SolFusionEmpresaView;
                        break;
                    case ConstantesRegistroIntegrantes.SoliCambioTipo:
                        view = ConstantesRegistroIntegrantes.SolCambioTipoView;
                        break;
                }

                return Json(new
                {
                    view = view,
                    codisoli = objSolicitud.Solicodi
                });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite pintar la lista de registros
        /// </summary>
        /// <param name="solicodi">codigo de solictud</param> 
        /// <param name="emprcodi">codigo de empresa</param> 
        /// <returns></returns>
        public PartialViewResult ListadoRepresentante(int solicodi, int emprcodi)
        {
            List<SiRepresentanteDTO> DatosRepresentante;
            if (solicodi == 0)
                DatosRepresentante = this.appRepresentante.GetByEmpresaSiRepresentante(emprcodi).Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoLegal).Where(x => x.Rptebaja.ToString() == ConstantesRegistroIntegrantes.RpteBajaNo).ToList();
            else
                DatosRepresentante = ObtenerDatosRepresentante(solicodi);

            RepresentanteModel modelRepresentante = new RepresentanteModel();
            modelRepresentante.ListaRepresentantes = DatosRepresentante;

            return PartialView(modelRepresentante);
        }

        /// <summary>
        /// Permite pintar la lista de registros de tipo comportamiento
        /// </summary>
        /// <param name="solicodi"></param> 
        /// <param name="emprcodi">codigo de empresa</param> 
        /// <returns></returns>
        public PartialViewResult ListadoTipo(int solicodi, int emprcodi)
        {
            List<SiTipoComportamientoDTO> DatosTipo;
            if (solicodi == 0)
                DatosTipo = this.appTipo.GetByEmpresaSiTipo(emprcodi).Where(x => x.Tipobaja.ToString() == ConstantesRegistroIntegrantes.RpteBajaNo).ToList();
            else
                DatosTipo = ObtenerDatosTipo(solicodi);

            TipoModel modelTipo = new TipoModel();
            modelTipo.ListaTipos = DatosTipo;

            return PartialView(modelTipo);
        }

        /// <summary>
        /// Permite preparar los datos del un nuevo registro para una solicitud
        /// </summary>
        /// <param name="view">nombre de la vista para entregar la vista y el model de la solicitud</param>   
        /// <param name="emprcodi">codigo de empresa</param> 
        /// <returns></returns>
        public ActionResult Nuevo(string view, int emprcodi)
        {
            ViewBag.emprcodi = emprcodi;

            SiEmpresaDTO empresa = this.appSolicitud.GetCabeceraSolicitudById(emprcodi);
            try
            {
                if (view == ConstantesRegistroIntegrantes.SolCambioDenominacionView)
                {
                    SolCambioDenominacionModel model = new SolCambioDenominacionModel();

                    if (appSolicitud.SolicitudEnCurso(emprcodi, ConstantesRegistroIntegrantes.SoliCambioDenominacion))
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoSI;
                    else
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoNO;

                    model.EmprRUC = empresa.Emprruc;
                    model.EmprRazSocial = empresa.Emprrazsocial;
                    model.EmprNombreComercial = empresa.Emprnombrecomercial;
                    model.EmprSigla = empresa.Emprsigla;
                    model.EmprDomLegal = empresa.Emprdomiciliolegal;
                    model.EmprTipoAgente = empresa.Tipoemprdesc;

                    return View(view, model);
                }
                else if (view == ConstantesRegistroIntegrantes.SolCambioRepresentanteView)
                {
                    SolCambioRepresentanteModel model = new SolCambioRepresentanteModel();

                    if (appSolicitud.SolicitudEnCurso(emprcodi, ConstantesRegistroIntegrantes.SoliCambioRepresentante))
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoSI;
                    else
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoNO;

                    model.EmprRUC = empresa.Emprruc;
                    model.EmprRazSocial = empresa.Emprrazsocial;
                    model.EmprSigla = empresa.Emprsigla;
                    model.EmprNombreComercial = empresa.Emprnombrecomercial;
                    model.EmprDomLegal = empresa.Emprdomiciliolegal;
                    model.EmprTipoAgente = empresa.Tipoemprdesc;

                    //Tipo de Documento
                    model.ListaTipoDocumento.Add(new TipoDocumentoModel(
                        ConstantesRegistroIntegrantes.RpteCodigoTipoDocumentoDNI,
                        ConstantesRegistroIntegrantes.RpteDescripcionTipoDocumentoDNI));
                    model.ListaTipoDocumento.Add(new TipoDocumentoModel(
                        ConstantesRegistroIntegrantes.RpteCodigoTipoDocumentoCarnetExtrangeria,
                        ConstantesRegistroIntegrantes.RpteDescripcionTipoDocumentoCarnetExtrangeria));

                    return View(view, model);
                }
                else if (view == ConstantesRegistroIntegrantes.SolBajaEmpresaView)
                {
                    SolBajaEmpresaModel model = new SolBajaEmpresaModel();

                    if (appSolicitud.SolicitudEnCurso(emprcodi, ConstantesRegistroIntegrantes.SoliBajaEmpresa))
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoSI;
                    else
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoNO;

                    model.EmprRUC = empresa.Emprruc;
                    model.EmprRazSocial = empresa.Emprrazsocial;
                    model.EmprNombreComercial = empresa.Emprnombrecomercial;
                    model.EmprSigla = empresa.Emprsigla;
                    model.EmprDomLegal = empresa.Emprdomiciliolegal;
                    model.EmprTipoAgente = empresa.Tipoemprdesc;


                    model.ListaCondicionBaja.Add(new CondicionBaja()
                    {
                        Codigo = ConstantesRegistroIntegrantes.CondicionBajaVoluntarioCodi,
                        Descripcion = ConstantesRegistroIntegrantes.CondicionBajaVoluntario
                    });

                    model.ListaCondicionBaja.Add(new CondicionBaja()
                    {
                        Codigo = ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionAgenteCodi,
                        Descripcion = ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionAgente
                    });

                    model.ListaCondicionBaja.Add(new CondicionBaja()
                    {
                        Codigo = ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionObligatorioCodi,
                        Descripcion = ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionObligatorio
                    });

                    model.ListaCondicionBaja.Add(new CondicionBaja()
                    {
                        Codigo = ConstantesRegistroIntegrantes.CondicionBajaConclusionCodi,
                        Descripcion = ConstantesRegistroIntegrantes.CondicionBajaConclusion
                    });
                    return View(view, model);
                }
                else if (view == ConstantesRegistroIntegrantes.SolFusionEmpresaView)
                {
                    SolFusionEmpresaModel model = new SolFusionEmpresaModel();

                    if (appSolicitud.SolicitudEnCurso(emprcodi, ConstantesRegistroIntegrantes.SoliFusionEmpresa))
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoSI;
                    else
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoNO;

                    model.EmprRUC = empresa.Emprruc;
                    model.EmprRazSocial = empresa.Emprrazsocial;
                    model.EmprNombreComercial = empresa.Emprnombrecomercial;
                    model.EmprSigla = empresa.Emprsigla;
                    model.EmprDomLegal = empresa.Emprdomiciliolegal;
                    model.EmprTipoAgente = empresa.Tipoemprdesc;
                    return View(view, model);
                }
                else if (view == ConstantesRegistroIntegrantes.SolCambioTipoView)
                {
                    SolCambioTipoModel model = new SolCambioTipoModel();

                    if (appSolicitud.SolicitudEnCurso(emprcodi, ConstantesRegistroIntegrantes.SoliCambioTipo))
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoSI;
                    else
                        model.SolicitudenCurso = ConstantesRegistroIntegrantes.SolicitudenCursoNO;

                    model.EmpresaCondicionVarianteGenerador = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteGenerador;
                    model.EmpresaCondicionVarianteTransmisor = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteTransmisor;
                    model.EmpresaCondicionVarianteDistribuidor = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteDistribuidor;
                    model.EmpresaCondicionVarianteUsuarioLibre = ConstantesRegistroIntegrantes.EmpresaCondicionVarianteUsuarioLibre;

                    model.EmprRUC = empresa.Emprruc;
                    model.EmprRazSocial = empresa.Emprrazsocial;
                    model.EmprSigla = empresa.Emprsigla;
                    model.EmprNombreComercial = empresa.Emprnombrecomercial;
                    model.EmprDomLegal = empresa.Emprdomiciliolegal;
                    model.EmprTipoAgente = empresa.Tipoemprdesc;

                    model.ListaPrincipalSecundario.Add(new TipoPrincipalSecundarioModel(
                        ConstantesRegistroIntegrantes.TipoAgenteCodigoPrincipal,
                        ConstantesRegistroIntegrantes.TipoAgenteDescripcionPrincipal));

                    model.ListaPrincipalSecundario.Add(new TipoPrincipalSecundarioModel(
                        ConstantesRegistroIntegrantes.TipoAgenteCodigoSecundario,
                        ConstantesRegistroIntegrantes.TipoAgenteDescripcionSecundario));

                    RegistroIntegrantesAppServicio appRegistroIntegrante = new RegistroIntegrantesAppServicio();

                    var TipoEmpresa = appRegistroIntegrante.ListTipoEmpresa();
                    model.TipoEmpresa = TipoEmpresa;

                    List<TipoDocumentoSustentarioModel> TipoDocumentoSustentario = new List<TipoDocumentoSustentarioModel>();

                    TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                    {
                        Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoAutorizacion,
                        Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionAutorizacion
                    });

                    TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                    {
                        Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoConcesion,
                        Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionConcesion
                    });

                    TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                    {
                        Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoDeclaracionJurada,
                        Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionDeclaracionJurada
                    });

                    model.TipoDocumentoSustentario = TipoDocumentoSustentario;

                    return View(view, model);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return View();
            }
        }

        /// <summary>
        /// Permite solicitar el cambio de tipo
        /// </summary>
        /// <param name="modelSolicitud">model InsertarSolCambioTipoModel</param>
        /// <returns></returns>
        public JsonResult SolicitarCambioTipo(InsertarSolCambioTipoModel modelSolicitud)
        {
            JsonResult jRespuesta;
            int id = 0;

            RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
            UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

            objSolicitud.Emprcodi = modelSolicitud.EmprCodi;
            objSolicitud.Soliusucreacion = userLogin.UserCode.ToString();
            objSolicitud.Soliestado = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.SoliestadoInterno = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.Solienviado = ConstantesRegistroIntegrantes.EnviadoSolicitudFalso;
            objSolicitud.Solifecsolicitud = DateTime.Now;
            objSolicitud.Solifeccreacion = DateTime.Now;
            objSolicitud.Tisocodi = ConstantesRegistroIntegrantes.SoliCambioTipo;


            List<RiSolicituddetalleDTO> listaDetalles = new List<RiSolicituddetalleDTO>();
            try
            {
                if (Request.Files.Count > 0)
                {
                    var Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;
                    listaDetalles.Add(new RiSolicituddetalleDTO()
                    {
                        Sdetcampo = ConstantesRegistroIntegrantes.DetalleSolicitudTipoCadenaTipo,
                        Sdetvalor = modelSolicitud.strTipo
                    });

                    Random oAleatorio = new Random();
                    string cExtension = "";

                    string[] Files = Request.Files.AllKeys;
                    for (int i = 0; i < Files.Length; i++)
                    {
                        string item = Files[i];
                        if (item.Contains("flArchivoDigitalizadoTI"))
                        {
                            cExtension = System.IO.Path.GetExtension(Request.Files[i].FileName);
                            string FileNameDNI = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);

                            //Request.Files[i].SaveAs(Path + FileNameDNI);
                            FileServer.UploadFromStream(Request.Files[i].InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameDNI, Path);

                            if (item.Replace("flArchivoDigitalizadoTI", "") != "")
                            {
                                int idDNI = int.Parse(item.Replace("flArchivoDigitalizadoTI", ""));
                                listaDetalles.Add(new RiSolicituddetalleDTO()
                                {
                                    Sdetcampo = ConstantesRegistroIntegrantes.DetalleSolicitudTipoAdjuntoArchivoDigitalizado,
                                    Sdetvalor = idDNI.ToString(),
                                    Sdetadjunto = ConstantesRegistroIntegrantes.EsAdjunto,
                                    Sdetvaloradjunto = Request.Files[i].FileName + "*" + FileNameDNI
                                });
                            }
                        }
                    }

                    //Documento sustentatorio
                    //cExtension = System.IO.Path.GetExtension(modelSolicitud.DocumentoSustentatorio.FileName);
                    //string FileNameCartaAdjunto = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                    //modelSolicitud.DocumentoSustentatorio.SaveAs(Path + FileNameCartaAdjunto);

                    modelSolicitud.DocumentoAdjunto = System.Web.HttpContext.Current.Session["sNombreArchivo"] as String;

                    listaDetalles.Add(new RiSolicituddetalleDTO()
                    {
                        Sdetcampo = ConstantesRegistroIntegrantes.DetalleSolicitudDocumentoSustentatorio,
                        Sdetadjunto = ConstantesRegistroIntegrantes.EsAdjunto,
                        // Sdetvaloradjunto = modelSolicitud.DocumentoSustentatorio.FileName + "*" + FileNameCartaAdjunto
                        Sdetvaloradjunto = modelSolicitud.DocumentoNombre + "*" + modelSolicitud.DocumentoAdjunto
                    });

                    int CodigoSolicitud = this.appSolicitud.Save(objSolicitud);
                    foreach (RiSolicituddetalleDTO item in listaDetalles)
                    {
                        item.Solicodi = CodigoSolicitud;
                        item.Sdetfeccreacion = DateTime.Now;
                        item.Sdetusucreacion = userLogin.UserCode.ToString();
                        //item.Sdetusucreacion = "Fit";
                    }

                    id = this.appSolicitud.SaveDetails(listaDetalles);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                id = -1;
            }
            jRespuesta = Json(id, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }


        /// <summary>
        /// Permite solicitar el cambio de representante
        /// </summary>
        /// <param name="modelSolicitud">model InsertarSolCambioRepresentanteModel</param>
        /// <returns></returns>
        public JsonResult SolicitarCambioRepresentante(InsertarSolCambioRepresentanteModel modelSolicitud)
        {
            JsonResult jRespuesta;
            int id = 0;

            RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
            UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

            objSolicitud.Emprcodi = modelSolicitud.EmprCodi;
            objSolicitud.Soliusucreacion = userLogin.UserCode.ToString();
            objSolicitud.Soliestado = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.SoliestadoInterno = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.Solienviado = ConstantesRegistroIntegrantes.EnviadoSolicitudFalso;
            objSolicitud.Solifecsolicitud = DateTime.Now;
            objSolicitud.Solifeccreacion = DateTime.Now;
            objSolicitud.Tisocodi = ConstantesRegistroIntegrantes.SoliCambioRepresentante;


            List<RiSolicituddetalleDTO> listaDetalles = new List<RiSolicituddetalleDTO>();
            try
            {
                if (Request.Files.Count > 0)
                {
                    var Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;

                    listaDetalles.Add(new RiSolicituddetalleDTO()
                    {
                        Sdetcampo = ConstantesRegistroIntegrantes.DetalleSolicitudRepresentanteLegalCadenaRepresentante,
                        Sdetvalor = modelSolicitud.strRepresentateLegal
                    });

                    Random oAleatorio = new Random();
                    string cExtension = "";

                    string[] Files = Request.Files.AllKeys;
                    for (int i = 0; i < Files.Length; i++)
                    {
                        string item = Files[i];

                        if (item.Contains("flDNIRL"))
                        {
                            cExtension = System.IO.Path.GetExtension(Request.Files[i].FileName);
                            string FileNameDNI = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);

                            //Request.Files[i].SaveAs(Path + FileNameDNI);
                            FileServer.UploadFromStream(Request.Files[i].InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameDNI, Path);
                            if (item.Replace("flDNIRL", "") != "")
                            {
                                int idDNI = int.Parse(item.Replace("flDNIRL", ""));
                                listaDetalles.Add(new RiSolicituddetalleDTO()
                                {
                                    Sdetcampo = ConstantesRegistroIntegrantes.DetalleSolicitudRepresentanteLegalAdjuntoDNI,
                                    Sdetvalor = idDNI.ToString(),
                                    Sdetadjunto = ConstantesRegistroIntegrantes.EsAdjunto,
                                    Sdetvaloradjunto = Request.Files[i].FileName + "*" + FileNameDNI
                                });
                            }
                        }
                        else if (item.Contains("flVigenciaPoderRL"))
                        {
                            cExtension = System.IO.Path.GetExtension(Request.Files[i].FileName);
                            string FileNameVP = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);

                            //Request.Files[i].SaveAs(Path + FileNameVP);
                            FileServer.UploadFromStream(Request.Files[i].InputStream, ConstantesRegistroIntegrantes.FolderRI, FileNameVP, Path);
                            if (item.Replace("flVigenciaPoderRL", "") != "")
                            {
                                int idVP = int.Parse(item.Replace("flVigenciaPoderRL", ""));
                                listaDetalles.Add(new RiSolicituddetalleDTO()
                                {
                                    Sdetcampo = ConstantesRegistroIntegrantes.DetalleSolicitudRepresentanteLegalAdjuntoVigenciaPoder,
                                    Sdetvalor = idVP.ToString(),
                                    Sdetadjunto = ConstantesRegistroIntegrantes.EsAdjunto,
                                    Sdetvaloradjunto = Request.Files[i].FileName + "*" + FileNameVP
                                });
                            }
                        }
                    }


                    //Documento sustentatorio
                    //cExtension = System.IO.Path.GetExtension(modelSolicitud.DocumentoSustentatorio.FileName);
                    //string FileNameCartaAdjunto = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);
                    //modelSolicitud.DocumentoSustentatorio.SaveAs(Path + FileNameCartaAdjunto);

                    modelSolicitud.DocumentoAdjunto = System.Web.HttpContext.Current.Session["sNombreArchivo"] as String;

                    listaDetalles.Add(new RiSolicituddetalleDTO()
                    {
                        Sdetcampo = ConstantesRegistroIntegrantes.DetalleSolicitudDocumentoSustentatorio,
                        Sdetadjunto = ConstantesRegistroIntegrantes.EsAdjunto,
                        //Sdetvaloradjunto = modelSolicitud.DocumentoSustentatorio.FileName + "*" + FileNameCartaAdjunto
                        Sdetvaloradjunto = modelSolicitud.DocumentoNombre + "*" + modelSolicitud.DocumentoAdjunto
                    });

                    int CodigoSolicitud = this.appSolicitud.Save(objSolicitud);

                    foreach (RiSolicituddetalleDTO item in listaDetalles)
                    {
                        item.Solicodi = CodigoSolicitud;
                        item.Sdetfeccreacion = DateTime.Now;
                        item.Sdetusucreacion = userLogin.UserCode.ToString();
                        //item.Sdetusucreacion = "Fit";
                    }

                    id = this.appSolicitud.SaveDetails(listaDetalles);
                }
                else
                {
                    throw new Exception("No se ha logrado obtener los archivos.");
                }
            }
            catch (Exception ex)
            {
                id = -1;
                log.Error(ex);
            }
            jRespuesta = Json(id, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }

        /// <summary>
        /// Permite subir los archivos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload()
        {
            string sNombreArchivo = "";
            string sNombreArchivoEnvio = "";
            string path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string extension = file.FileName.Split('.').Last().ToUpper();
                    sNombreArchivoEnvio = file.FileName;
                    extension = "." + extension;

                    Random oAleatorio = new Random();
                    sNombreArchivo = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), extension);

                    System.Web.HttpContext.Current.Session["sNombreArchivo"] = sNombreArchivo;

                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    //file.SaveAs(path + sNombreArchivo);
                    FileServer.UploadFromStream(file.InputStream, ConstantesRegistroIntegrantes.FolderRI, sNombreArchivo, path);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Permite solicitar el cambio de denominación
        /// </summary>
        /// <param name="modelSolicitud">model InsertarSolCambioDenominacionModel</param>
        /// <returns></returns>
        public JsonResult SolicitarCambioDenominacion(InsertarSolCambioDenominacionModel modelSolicitud)
        {

            RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
            UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

            objSolicitud.Emprcodi = modelSolicitud.EmprCodi;
            objSolicitud.Soliusucreacion = userLogin.UserCode.ToString();
            objSolicitud.Soliestado = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.SoliestadoInterno = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.Solienviado = ConstantesRegistroIntegrantes.EnviadoSolicitudFalso;
            objSolicitud.Solifecsolicitud = DateTime.Now;
            objSolicitud.Solifeccreacion = DateTime.Now;
            objSolicitud.Tisocodi = ConstantesRegistroIntegrantes.SoliCambioDenominacion;


            List<RiSolicituddetalleDTO> listaDetalles = new List<RiSolicituddetalleDTO>();
            try
            {
                listaDetalles.Add(new RiSolicituddetalleDTO()
                {
                    Sdetcampo = ConstantesRegistroIntegrantes.Emprnombcomercial,
                    Sdetvalor = modelSolicitud.EmprNombreComercialCambio,
                });
                listaDetalles.Add(new RiSolicituddetalleDTO()
                {
                    Sdetcampo = ConstantesRegistroIntegrantes.Emprrazsocial,
                    Sdetvalor = modelSolicitud.EmprRazSocialCambio,
                });
                listaDetalles.Add(new RiSolicituddetalleDTO()
                {
                    Sdetcampo = ConstantesRegistroIntegrantes.Emprruc,
                    Sdetvalor = modelSolicitud.EmprRUCCambio,
                });
                listaDetalles.Add(new RiSolicituddetalleDTO()
                {
                    Sdetcampo = ConstantesRegistroIntegrantes.Emprsigla,
                    Sdetvalor = modelSolicitud.EmprSiglaCambio,
                });

                //if (Request.Files.Count > 0)
                //{
                //    var Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;
                //    Random oAleatorio = new Random();
                //    string cExtension = "";
                //    cExtension = System.IO.Path.GetExtension(modelSolicitud.DocumentoSustentatorio.FileName);
                //    string FileNameCartaAdjunto = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);

                //    modelSolicitud.DocumentoSustentatorio.SaveAs(Path + FileNameCartaAdjunto);

                modelSolicitud.DocumentoAdjunto = System.Web.HttpContext.Current.Session["sNombreArchivo"] as String;

                listaDetalles.Add(new RiSolicituddetalleDTO()
                {
                    Sdetadjunto = ConstantesRegistroIntegrantes.EsAdjunto,
                    //Sdetvaloradjunto = modelSolicitud.DocumentoSustentatorio.FileName + "*" + FileNameCartaAdjunto
                    Sdetvaloradjunto = modelSolicitud.DocumentoNombre + "*" + modelSolicitud.DocumentoAdjunto
                });
                //}
                //else
                //{
                //    throw new Exception("No se ha logrado obtener los archivos.");
                //}

                int id = this.appSolicitud.Save(objSolicitud);
                foreach (RiSolicituddetalleDTO item in listaDetalles)
                {
                    item.Solicodi = id;
                    item.Sdetfeccreacion = DateTime.Now;
                    item.Sdetusucreacion = userLogin.UserCode.ToString();
                    //item.Sdetusucreacion = "Fit";
                }

                id = this.appSolicitud.SaveDetails(listaDetalles);
                return Json(1);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite solicitar la baja de empresa
        /// </summary>
        /// <param name="modelSolicitud">model InsertarSolBajaEmpresaModel</param>
        /// <returns></returns>
        public JsonResult SolicitarBajaEmpresa(InsertarSolBajaEmpresaModel modelSolicitud)
        {

            RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
            UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

            objSolicitud.Emprcodi = modelSolicitud.EmprCodi;
            objSolicitud.Soliusucreacion = userLogin.UserCode.ToString();
            objSolicitud.Soliestado = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.SoliestadoInterno = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.Solienviado = ConstantesRegistroIntegrantes.EnviadoSolicitudFalso;
            objSolicitud.Solifecsolicitud = DateTime.Now;
            objSolicitud.Solifeccreacion = DateTime.Now;
            objSolicitud.Tisocodi = ConstantesRegistroIntegrantes.SoliBajaEmpresa;

            List<RiSolicituddetalleDTO> listaDetalles = new List<RiSolicituddetalleDTO>();
            try
            {
                listaDetalles.Add(new RiSolicituddetalleDTO()
                {
                    Sdetcampo = ConstantesRegistroIntegrantes.CondicionBajaCampo,
                    Sdetvalor = modelSolicitud.CondicionBaja
                });

                //if (Request.Files.Count > 0)
                //{
                //    var Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;
                //    Random oAleatorio = new Random();
                //    string cExtension = "";
                //    cExtension = System.IO.Path.GetExtension(modelSolicitud.DocumentoSustentatorio.FileName);
                //    string FileNameCartaAdjunto = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);

                //modelSolicitud.DocumentoSustentatorio.SaveAs(Path + FileNameCartaAdjunto);

                modelSolicitud.DocumentoAdjunto = System.Web.HttpContext.Current.Session["sNombreArchivo"] as String;

                listaDetalles.Add(new RiSolicituddetalleDTO()
                {
                    Sdetadjunto = ConstantesRegistroIntegrantes.EsAdjunto,
                    //Sdetvaloradjunto = modelSolicitud.DocumentoSustentatorio.FileName + "*" + FileNameCartaAdjunto
                    Sdetvaloradjunto = modelSolicitud.DocumentoNombre + "*" + modelSolicitud.DocumentoAdjunto
                });
                //}
                //else
                //{
                //    throw new Exception("No se ha logrado obtener los archivos.");
                //}

                int id = this.appSolicitud.Save(objSolicitud);
                foreach (RiSolicituddetalleDTO item in listaDetalles)
                {
                    item.Solicodi = id;
                    item.Sdetfeccreacion = DateTime.Now;
                    item.Sdetusucreacion = userLogin.UserCode.ToString();
                    //item.Sdetusucreacion = "Fit";
                }

                id = this.appSolicitud.SaveDetails(listaDetalles);
                return Json(1);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite solicitar la fusion de empresa
        /// </summary>
        /// <param name="modelSolicitud">model InsertarSolFusionEmpresaModel</param>
        /// <returns></returns>
        public JsonResult SolicitarFusionEmpresa(InsertarSolFusionEmpresaModel modelSolicitud)
        {

            RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
            UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

            objSolicitud.Emprcodi = modelSolicitud.EmprCodi;
            objSolicitud.Soliusucreacion = userLogin.UserCode.ToString();
            objSolicitud.Soliestado = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.SoliestadoInterno = ConstantesRegistroIntegrantes.SoliPendiente;
            objSolicitud.Solienviado = ConstantesRegistroIntegrantes.EnviadoSolicitudFalso;
            objSolicitud.Solifecsolicitud = DateTime.Now;
            objSolicitud.Solifeccreacion = DateTime.Now;
            objSolicitud.Tisocodi = ConstantesRegistroIntegrantes.SoliFusionEmpresa;

            List<RiSolicituddetalleDTO> listaDetalles = new List<RiSolicituddetalleDTO>();
            try
            {
                //if (Request.Files.Count > 0)
                //{
                //    var Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;
                //    Random oAleatorio = new Random();
                //    string cExtension = "";
                //    cExtension = System.IO.Path.GetExtension(modelSolicitud.DocumentoSustentatorio.FileName);
                //    string FileNameCartaAdjunto = string.Format("{0:yyyyMMddHHmmss}{1}{2}", DateTime.Now, oAleatorio.Next(10, 100), cExtension);

                //    modelSolicitud.DocumentoSustentatorio.SaveAs(Path + FileNameCartaAdjunto);
                modelSolicitud.DocumentoAdjunto = System.Web.HttpContext.Current.Session["sNombreArchivo"] as String;

                listaDetalles.Add(new RiSolicituddetalleDTO()
                {
                    Sdetadjunto = ConstantesRegistroIntegrantes.EsAdjunto,
                    //Sdetvaloradjunto = modelSolicitud.DocumentoSustentatorio.FileName + "*" + FileNameCartaAdjunto
                    Sdetvaloradjunto = modelSolicitud.DocumentoNombre + "*" + modelSolicitud.DocumentoAdjunto
                });
                //}
                //else
                //{
                //    throw new Exception("No se ha logrado obtener los archivos.");
                //}


                int id = this.appSolicitud.Save(objSolicitud);
                foreach (RiSolicituddetalleDTO item in listaDetalles)
                {
                    item.Solicodi = id;
                    item.Sdetfeccreacion = DateTime.Now;
                    item.Sdetusucreacion = userLogin.UserCode.ToString();
                    //item.Sdetusucreacion = "Fit";
                }

                id = this.appSolicitud.SaveDetails(listaDetalles);
                return Json(1);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite ver el detalle de la solicitud según su tipo
        /// </summary>
        /// <param name="view">vista que represente el tipo de solicitud a visualizar</param>
        /// <param name="solicodi">codigo de solictud</param>
        /// <returns></returns>
        public ActionResult VerDetalle(string view, int solicodi)
        {
            RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
            SiEmpresaDTO objEmpresa = new SiEmpresaDTO();
            List<RiSolicituddetalleDTO> ListDetalles = new List<RiSolicituddetalleDTO>();

            try
            {

                objSolicitud = this.appSolicitud.GetById(solicodi);
                objEmpresa = this.appSolicitud.GetCabeceraSolicitudById(objSolicitud.Emprcodi.Value);
                ListDetalles = this.appSolicitud.ListDetalleBySolicodi(objSolicitud.Solicodi);

                if (view == ConstantesRegistroIntegrantes.SolCambioDenominacionView)
                {
                    SolCambioDenominacionModel model = new SolCambioDenominacionModel();
                    model.EmprDomLegal = objEmpresa.Emprdomiciliolegal;
                    model.EmprNombreComercial = objEmpresa.Emprnombrecomercial;
                    model.EmprRazSocial = objEmpresa.Emprrazsocial;
                    model.EmprRUC = objEmpresa.Emprruc;
                    model.EmprSigla = objEmpresa.Emprsigla;
                    model.EmprTipoAgente = objEmpresa.Tipoemprdesc;
                    model.objSolicitud = objSolicitud;
                    model.ListDetalleSolicitud = ListDetalles;
                    foreach (RiSolicituddetalleDTO item in ListDetalles)
                    {
                        if (item.Sdetcampo == ConstantesRegistroIntegrantes.Emprrazsocial)
                        {
                            model.EmprRazSocialCambio = item.Sdetvalor;
                        }
                        else if (item.Sdetcampo == ConstantesRegistroIntegrantes.Emprruc)
                        {
                            model.EmprRUCCambio = item.Sdetvalor;
                        }
                        else if (item.Sdetcampo == ConstantesRegistroIntegrantes.Emprnombcomercial)
                        {
                            model.EmprNombreComercialCambio = item.Sdetvalor;
                        }
                        else if (item.Sdetcampo == ConstantesRegistroIntegrantes.Emprsigla)
                        {
                            model.EmprSiglaCambio = item.Sdetvalor;
                        }
                        else if (item.Sdetadjunto == ConstantesRegistroIntegrantes.EsAdjunto)
                        {
                            string[] filesNames = item.Sdetvaloradjunto.Split('*');

                            model.NombAdjunto = filesNames[0];
                            model.DetAdjunto = filesNames[1];
                        }
                    }

                    return View(view, model);
                }
                if (view == ConstantesRegistroIntegrantes.SolCambioRepresentanteView)
                {
                    ViewBag.emprcodi = objSolicitud.Emprcodi.Value;

                    SolCambioRepresentanteModel model = new SolCambioRepresentanteModel();
                    model.EmprDomLegal = objEmpresa.Emprdomiciliolegal;
                    model.EmprNombreComercial = objEmpresa.Emprnombrecomercial;
                    model.EmprRazSocial = objEmpresa.Emprrazsocial;
                    model.EmprRUC = objEmpresa.Emprruc;
                    model.EmprSigla = objEmpresa.Emprsigla;
                    model.EmprTipoAgente = objEmpresa.Tipoemprdesc;
                    model.objSolicitud = objSolicitud;
                    model.ListDetalleSolicitud = ListDetalles;

                    model.ListaTipoDocumento.Add(new TipoDocumentoModel(
                        ConstantesRegistroIntegrantes.RpteCodigoTipoDocumentoDNI,
                        ConstantesRegistroIntegrantes.RpteDescripcionTipoDocumentoDNI));

                    model.ListaTipoDocumento.Add(new TipoDocumentoModel(
                        ConstantesRegistroIntegrantes.RpteCodigoTipoDocumentoCarnetExtrangeria,
                        ConstantesRegistroIntegrantes.RpteDescripcionTipoDocumentoCarnetExtrangeria));

                    foreach (RiSolicituddetalleDTO item in ListDetalles)
                    {
                        if (item.Sdetadjunto == ConstantesRegistroIntegrantes.EsAdjunto
                            && item.Sdetcampo == ConstantesRegistroIntegrantes.DetalleSolicitudDocumentoSustentatorio)
                        {
                            string[] filesNames = item.Sdetvaloradjunto.Split('*');

                            model.NombAdjunto = filesNames[0];
                            model.DetAdjunto = filesNames[1];
                        }
                    }

                    return View(view, model);
                }
                if (view == ConstantesRegistroIntegrantes.SolCambioTipoView)
                {
                    ViewBag.emprcodi = objSolicitud.Emprcodi.Value;

                    SolCambioTipoModel model = new SolCambioTipoModel();
                    model.EmprDomLegal = objEmpresa.Emprdomiciliolegal;
                    model.EmprNombreComercial = objEmpresa.Emprnombrecomercial;
                    model.EmprRazSocial = objEmpresa.Emprrazsocial;
                    model.EmprRUC = objEmpresa.Emprruc;
                    model.EmprSigla = objEmpresa.Emprsigla;
                    model.EmprTipoAgente = objEmpresa.Tipoemprdesc;
                    model.objSolicitud = objSolicitud;
                    model.ListDetalleSolicitud = ListDetalles;

                    model.ListaPrincipalSecundario.Add(new TipoPrincipalSecundarioModel(
                        ConstantesRegistroIntegrantes.TipoAgenteCodigoPrincipal,
                        ConstantesRegistroIntegrantes.TipoAgenteDescripcionPrincipal));

                    model.ListaPrincipalSecundario.Add(new TipoPrincipalSecundarioModel(
                        ConstantesRegistroIntegrantes.TipoAgenteCodigoSecundario,
                        ConstantesRegistroIntegrantes.TipoAgenteDescripcionSecundario));

                    RegistroIntegrantesAppServicio appRegistroIntegrante = new RegistroIntegrantesAppServicio();

                    var TipoEmpresa = appRegistroIntegrante.ListTipoEmpresa();
                    model.TipoEmpresa = TipoEmpresa;

                    List<TipoDocumentoSustentarioModel> TipoDocumentoSustentario = new List<TipoDocumentoSustentarioModel>();

                    TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                    {
                        Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoAutorizacion,
                        Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionAutorizacion
                    });

                    TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                    {
                        Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoConcesion,
                        Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionConcesion
                    });

                    TipoDocumentoSustentario.Add(new TipoDocumentoSustentarioModel()
                    {
                        Id = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioCodigoDeclaracionJurada,
                        Nombre = ConstantesRegistroIntegrantes.TipoDocumentoSustentatorioDescripcionDeclaracionJurada
                    });

                    model.TipoDocumentoSustentario = TipoDocumentoSustentario;

                    foreach (RiSolicituddetalleDTO item in ListDetalles)
                    {
                        if (item.Sdetadjunto == ConstantesRegistroIntegrantes.EsAdjunto
                            && item.Sdetcampo == ConstantesRegistroIntegrantes.DetalleSolicitudDocumentoSustentatorio)
                        {
                            string[] filesNames = item.Sdetvaloradjunto.Split('*');

                            model.NombAdjunto = filesNames[0];
                            model.DetAdjunto = filesNames[1];
                        }
                    }

                    return View(view, model);
                }
                if (view == ConstantesRegistroIntegrantes.SolBajaEmpresaView)
                {
                    SolBajaEmpresaModel model = new SolBajaEmpresaModel();
                    model.EmprDomLegal = objEmpresa.Emprdomiciliolegal;
                    model.EmprNombreComercial = objEmpresa.Emprnombrecomercial;
                    model.EmprRazSocial = objEmpresa.Emprrazsocial;
                    model.EmprRUC = objEmpresa.Emprruc;
                    model.EmprSigla = objEmpresa.Emprsigla;
                    model.EmprTipoAgente = objEmpresa.Tipoemprdesc;
                    model.objSolicitud = objSolicitud;
                    model.ListDetalleSolicitud = ListDetalles;


                    foreach (RiSolicituddetalleDTO item in ListDetalles)
                    {
                        if (item.Sdetcampo == ConstantesRegistroIntegrantes.CondicionBajaCampo)
                        {
                            model.CondicionBaja = item.Sdetvalor;
                            switch (model.CondicionBaja)
                            {
                                case ConstantesRegistroIntegrantes.CondicionBajaVoluntarioCodi:
                                    model.CondicionBajaDescripcion = ConstantesRegistroIntegrantes.CondicionBajaVoluntario;
                                    break;
                                case ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionAgenteCodi:
                                    model.CondicionBajaDescripcion = ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionAgente;
                                    break;
                                case ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionObligatorioCodi:
                                    model.CondicionBajaDescripcion = ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionObligatorio;
                                    break;
                                case ConstantesRegistroIntegrantes.CondicionBajaConclusionCodi:
                                    model.CondicionBajaDescripcion = ConstantesRegistroIntegrantes.CondicionBajaConclusion;
                                    break;
                            }
                        }
                        else if (item.Sdetadjunto == ConstantesRegistroIntegrantes.EsAdjunto)
                        {
                            string[] filesNames = item.Sdetvaloradjunto.Split('*');

                            model.NombAdjunto = filesNames[0];
                            model.DetAdjunto = filesNames[1];
                        }
                    }

                    return View(view, model);
                }
                if (view == ConstantesRegistroIntegrantes.SolFusionEmpresaView)
                {
                    SolFusionEmpresaModel model = new SolFusionEmpresaModel();
                    model.EmprDomLegal = objEmpresa.Emprdomiciliolegal;
                    model.EmprNombreComercial = objEmpresa.Emprnombrecomercial;
                    model.EmprRazSocial = objEmpresa.Emprrazsocial;
                    model.EmprRUC = objEmpresa.Emprruc;
                    model.EmprSigla = objEmpresa.Emprsigla;
                    model.EmprTipoAgente = objEmpresa.Tipoemprdesc;
                    model.objSolicitud = objSolicitud;
                    model.ListDetalleSolicitud = ListDetalles;

                    foreach (RiSolicituddetalleDTO item in ListDetalles)
                    {
                        if (item.Sdetadjunto == ConstantesRegistroIntegrantes.EsAdjunto)
                        {
                            string[] filesNames = item.Sdetvaloradjunto.Split('*');

                            model.NombAdjunto = filesNames[0];
                            model.DetAdjunto = filesNames[1];
                        }
                    }

                    return View(view, model);
                }
                else
                {
                    return View(view);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite finalizar una solicitud
        /// </summary>
        /// <param name="solicodi">codigo de solicitud</param>
        /// <param name="observacion">Observacion</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult finalizar(int solicodi, string observacion)
        {
            try
            {
                string estado = observacion.Length > 0 ?
                    ConstantesRegistroIntegrantes.SoliDenegada :
                    ConstantesRegistroIntegrantes.SoliAprobadoDigitalCodigo;
                this.appSolicitud.FinalizarSolicitud(solicodi, estado, observacion);
                
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite finalizar una solicitud
        /// </summary>
        /// <param name="solicodi">codigo de solicitud</param>
        /// <param name="observacion">Observacion</param>
        /// <param name="emprcodi">código de empresa</param>
        /// <param name="razonsocial">Razon Social Cambio</param>
        /// <param name="comercial">Nombre Comercial Cambio</param>
        /// <param name="sigla">Sigla Cambio</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult finalizarDenominacion(int solicodi, string observacion, int emprcodi, string razonsocial, string comercial, string sigla)
        {
            try
            {
                string estado = observacion.Length > 0 ?
                    ConstantesRegistroIntegrantes.SoliDenegada :
                    ConstantesRegistroIntegrantes.SoliAprobadoDigitalCodigo;
                this.appSolicitud.FinalizarSolicitud(solicodi, estado, observacion);

                if (estado != ConstantesRegistroIntegrantes.SoliDenegada)
                {
                    this.appEmpresa.ActualizarEmpresaCambioDenom(emprcodi, comercial, razonsocial, sigla);
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar la fecha de proceso de la solicitud
        /// </summary>
        /// <param name="solicodi">codigo de solicitud</param>
        /// <param name="fecha">fecha proceso</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult grabar(int solicodi, DateTime fecha)
        {
            try
            {
                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);
                int usuario = Convert.ToInt32(userLogin.UserCode.ToString());
                //int usuario = 1;//fit temporal

                int resultado = this.appSolicitud.ActualizarFechaProceso(solicodi, fecha, usuario);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite cargar los registros de SGDOC - flujos
        /// </summary>
        /// <param name="emprcodi">Codigo empresa</param>
        /// <param name="solicodi">Codigo de solicitud</param>
        [HttpPost]
        public PartialViewResult VerFlujo(int emprcodi, int solicodi)
        {
            FlujoModel model = new FlujoModel();

            var flujo = appEmpresa.ListarFlujoEmpresaSolicitud(emprcodi, solicodi);
            model.Emprcodi = emprcodi;
            model.Solicodi = solicodi;

            foreach (var item in flujo)
            {

                model.ListaFlujo.Add(new FlujoDetalleModel()
                {
                    Emflcodi = item.Emflcodi,
                    Fljcodi = item.Fljcodi,
                    Estado = item.FLJESTADO,
                    FechaOriginal = item.FLJFECHAORIG,
                    FechaProceso = item.FLJFECHAPROCE,
                    FechaRecepcion = item.FLJFECHARECEP,
                    Observacion = item.observacion,
                    //DocumentoAdjuntoFileName = item.DocumentoAdjuntoFileName,
                    //DocumentoAdjunto = item.DocumentoAdjunto,
                    corrnumproc = item.corrnumproc,
                    filecodi = item.filecodi,
                    EnlaceArchivo = "http://sicoes.coes.org.pe/sgdocenc/show.aspx?at=" + Encriptacion.Encripta(item.filecodi.ToString()).Replace("+", "%2b")
                });
            }

            return PartialView(model);
        }



        #region FUNCIONES

        /// <summary>
        /// Permite Obtener los Datos de la solicitud cambio de representante legal
        /// </summary>
        /// <param name="soliestado">estado de la solicitud</param>
        /// <returns></returns>
        public List<SiRepresentanteDTO> ObtenerDatosRepresentante(int solicodi)
        {
            List<SiRepresentanteDTO> DatosRepresentante = new List<SiRepresentanteDTO>();

            RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
            List<RiSolicituddetalleDTO> ListDetalles = new List<RiSolicituddetalleDTO>();

            objSolicitud = this.appSolicitud.GetById(solicodi);
            ListDetalles = this.appSolicitud.ListDetalleBySolicodi(objSolicitud.Solicodi);

            foreach (RiSolicituddetalleDTO item in ListDetalles)
            {
                if (item.Sdetcampo == ConstantesRegistroIntegrantes.DetalleSolicitudRepresentanteLegalCadenaRepresentante)
                {
                    string cadena = item.Sdetvalor;
                    string[] registrosRL = cadena.Split('|');
                    string[] camposRL;
                    for (int i = 0; i < registrosRL.Length; i++)
                    {
                        camposRL = registrosRL[i].Split('*');
                        DatosRepresentante.Add(new SiRepresentanteDTO()
                        {
                            SoloLectura = ConstantesRegistroIntegrantes.SoloLecturaSI,
                            Rptetipdocidentidad = camposRL[3].ToString().Length == 8 ? ConstantesRegistroIntegrantes.RpteCodigoTipoDocumentoDNI :
                                                                                    ConstantesRegistroIntegrantes.RpteCodigoTipoDocumentoCarnetExtrangeria,
                            Rptecodi = camposRL[0].ToString() == "" ? 0 : Convert.ToInt32(camposRL[0].ToString()),
                            Rptetiprepresentantelegal = camposRL[1].ToString() == ConstantesRegistroIntegrantes.RepresentanteLegalTitular ?
                                                                                    ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular :
                                                                                    ConstantesRegistroIntegrantes.RepresentanteLegalTipoAlterno,
                            Accion = camposRL[2].ToString(),
                            Rptedocidentidad = camposRL[3].ToString(),
                            Rptenombres = camposRL[4].ToString(),
                            Rpteapellidos = camposRL[5].ToString(),
                            Rptecargoempresa = camposRL[6].ToString(),
                            Rptetelefono = camposRL[7].ToString(),
                            Rptetelfmovil = camposRL[8].ToString(),
                            Rptecorreoelectronico = camposRL[9].ToString(),
                            Id = camposRL[10].ToString() == "" ? 0 : Convert.ToInt32(camposRL[10].ToString())
                        }
                        );
                    }
                }
            }

            foreach (RiSolicituddetalleDTO item in ListDetalles)
            {
                if (item.Sdetcampo == ConstantesRegistroIntegrantes.DetalleSolicitudRepresentanteLegalAdjuntoDNI)
                {
                    int idDNI = Convert.ToInt32(item.Sdetvalor);
                    string[] filesNames = item.Sdetvaloradjunto.Split('*');

                    DatosRepresentante.Find(x => x.Id == idDNI).Rptedocidentidadadj = filesNames[0];
                    DatosRepresentante.Find(x => x.Id == idDNI).Rptedocidentidadadjfilename = filesNames[1];
                }

                if (item.Sdetcampo == ConstantesRegistroIntegrantes.DetalleSolicitudRepresentanteLegalAdjuntoVigenciaPoder)
                {
                    int idVigenciaPoder = Convert.ToInt32(item.Sdetvalor);
                    string[] filesNames = item.Sdetvaloradjunto.Split('*');

                    DatosRepresentante.Find(x => x.Id == idVigenciaPoder).Rptevigenciapoder = filesNames[0];
                    DatosRepresentante.Find(x => x.Id == idVigenciaPoder).Rptevigenciapoderfilename = filesNames[1];
                }
            }

            return DatosRepresentante;
        }

        /// <summary>
        /// Permite Obtener los Datos de la solicitud cambio de Tipo
        /// </summary>
        /// <param name="soliestado">estado de la solicitud</param>
        /// <returns></returns>
        public List<SiTipoComportamientoDTO> ObtenerDatosTipo(int solicodi)
        {
            List<SiTipoComportamientoDTO> DatosRepresentante = new List<SiTipoComportamientoDTO>();

            RiSolicitudDTO objSolicitud = new RiSolicitudDTO();
            List<RiSolicituddetalleDTO> ListDetalles = new List<RiSolicituddetalleDTO>();

            objSolicitud = this.appSolicitud.GetById(solicodi);
            ListDetalles = this.appSolicitud.ListDetalleBySolicodi(objSolicitud.Solicodi);

            foreach (RiSolicituddetalleDTO item in ListDetalles)
            {
                if (item.Sdetcampo == ConstantesRegistroIntegrantes.DetalleSolicitudTipoCadenaTipo)
                {
                    string cadena = item.Sdetvalor;
                    string[] registrosRL = cadena.Split('|');
                    string[] camposRL;
                    for (int i = 0; i < registrosRL.Length; i++)
                    {
                        camposRL = registrosRL[i].Split('*');
                        DatosRepresentante.Add(new SiTipoComportamientoDTO()
                        {
                            SoloLectura = ConstantesRegistroIntegrantes.SoloLecturaSI,
                            Tipocodi = camposRL[0].ToString() == "" ? 0 : Convert.ToInt32(camposRL[0].ToString()),
                            Tipoprincipal = camposRL[1].ToString() == "Principal" ? "S" : "N",
                            Accion = camposRL[2].ToString(),
                            Tipoemprcodi = camposRL[3].ToString() == "" ? 0 : Convert.ToInt32(camposRL[3].ToString()),
                            Tipotipagente = camposRL[4].ToString(),
                            IdTipodocsustentatorio = camposRL[5].ToString() == "" ? 0 : Convert.ToInt32(camposRL[5].ToString()),
                            Tipodocsustentatorio = camposRL[6].ToString(),
                            Tipopotenciainstalada = camposRL[9].ToString(),
                            Tiponrocentrales = camposRL[10].ToString(),
                            Tipolineatrans500 = camposRL[11].ToString(),
                            Tipolineatrans220 = camposRL[12].ToString(),
                            Tipolineatrans138 = camposRL[13].ToString(),
                            Tipolineatrans500km = camposRL[14].ToString(),
                            Tipolineatrans220km = camposRL[15].ToString(),
                            Tipolineatrans138km = camposRL[16].ToString(),
                            Tipototallineastransmision = camposRL[17].ToString(),
                            Tipomaxdemandacoincidente = camposRL[18].ToString(),
                            Tipomaxdemandacontratada = camposRL[19].ToString(),
                            Tiponumsuministrador = camposRL[20].ToString(),
                            Id = camposRL[21].ToString() == "" ? 0 : Convert.ToInt32(camposRL[21].ToString())
                        }
                        );
                    }
                }
            }

            foreach (RiSolicituddetalleDTO item in ListDetalles)
            {
                if (item.Sdetcampo == ConstantesRegistroIntegrantes.DetalleSolicitudTipoAdjuntoArchivoDigitalizado)
                {
                    int idArchivoDigitalizado = Convert.ToInt32(item.Sdetvalor);
                    string[] filesNames = item.Sdetvaloradjunto.Split('*');

                    DatosRepresentante.Find(x => x.Id == idArchivoDigitalizado).Tipoarcdigitalizado = filesNames[0];
                    DatosRepresentante.Find(x => x.Id == idArchivoDigitalizado).Tipoarcdigitalizadofilename = filesNames[1];
                }
            }

            return DatosRepresentante;
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url, string nombre)
        {
            try
            {
                Stream stream = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + url, ConstantesRegistroIntegrantes.FolderUploadRutaCompleta);
                int indexOf = nombre.LastIndexOf('.');
                string extension = nombre.Substring(indexOf + 1, nombre.Length - indexOf - 1);

                if (stream != null)
                    return File(stream, extension, nombre);
                else
                {
                    log.Info("Download - No se encontro el archivo: " + url);
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Permite visualizar el archivo generado
        /// </summary>
        /// <returns></returns>
        public FileStreamResult ver(string url)
        {
            try
            {
                Stream stream = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + url, ConstantesRegistroIntegrantes.FolderUploadRutaCompleta);
                FileStream fs = stream as FileStream;

                if (stream != null)
                    return File(fs, "application/pdf");
                else
                {
                    log.Info("Ver - No se encontro el archivo: " + url);
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }


        /// <summary>
        /// Permite obtener los datos de SUNAT
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult obtenerdatos(string ruc)
        {
            try
            {
                BeanEmpresa empresa = this.servicio.ConsultarPorRUC(ruc);

                if (empresa == null)
                {
                    return Json(-2); //- RUC no Existe            
                }
                else
                {
                    if (string.IsNullOrEmpty(empresa.RUC))
                    {
                        return Json(-2); //- RUC no Existe            
                    }
                    else
                    {

                        if (empresa.Estado.ToUpper().Trim() != "ACTIVO")
                            return Json(-3); //- RUC de Baja            
                        else
                            return Json(empresa);
                    }
                }
            }
            catch
            {
                return Json(-1); //- Error en el proceso
            }
        }

        #endregion
    }

}
