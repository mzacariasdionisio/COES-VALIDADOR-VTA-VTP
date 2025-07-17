using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.RegistroIntegrante.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Configuration;
using System.Text;
using COES.MVC.Intranet.Areas.RegistroIntegrante.Helper;
using System.Globalization;
using System.Threading;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Controllers
{
    public class RevisionController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(RevisionController));

        /// <summary>
        /// Instancia de la clase RegistroIntegrantesAppServicio
        /// </summary>
        RegistroIntegrantesAppServicio servicio = new RegistroIntegrantesAppServicio();

        /// <summary>
        /// Instancia de la clase EmpresaAppServicio
        /// </summary>
        private EmpresaAppServicio appEmpresa = new EmpresaAppServicio();

        /// <summary>
        /// Instancia de la clase RepresentanteAppServicio
        /// </summary>
        private RepresentanteAppServicio appRepresentante = new RepresentanteAppServicio();

        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Instancia de la clase ReportesAppServicio
        /// </summary>
        private ReportesAppServicio appRegistroIntegranteReporte = new ReportesAppServicio();

        public RevisionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("RevisionController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("RevisionController", ex);
                throw;
            }
        }

        /// <summary>
        /// Llama a la página principal del listado de revisiones
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RevisionModel model = new RevisionModel();

            model.Estado.Add(new EstadoModel()
            {
                Codigo = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroPendienteCodigo,
                Nombre = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroPendiente
            });

            model.Estado.Add(new EstadoModel()
            {
                Codigo = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroAprobadoDigitalCodigo,
                Nombre = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroAprobadoDigital
            });

            model.Estado.Add(new EstadoModel()
            {
                Codigo = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroAprobadoFisicaCodigo,
                Nombre = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroAprobadoFisica
            });

            List<SiTipoempresaDTO> tipos = new List<SiTipoempresaDTO>();
            tipos = this.servicio.ListTipoEmpresa();
            model.ListaTipoEmpresa = tipos;
            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="estado">estado</param>
        /// <param name="tipemprcodi">codigo tipo empresa</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string estado, string tipemprcodi, string nombre)
        {
            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;
            int tip = tipemprcodi.Length == 0 ? 0 : int.Parse(tipemprcodi);
            if (string.IsNullOrEmpty(nombre)) nombre = ConstantesRegistroIntegrantes.ParametroDefecto;

            int nroRegistros = this.servicio.ObtenerTotalListByEstadoAndTipEmp(estado, tip, nombre);

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
        /// <param name="estado">estado</param>
        /// <param name="tipemprcodi">codigo tipo empresa</param>
        /// <param name="nroPagina">Nro de pagina</param>
        /// <returns></returns>
        public ActionResult Listado(string estado, string tipemprcodi, string nombre, int nroPagina)
        {
            RevisionModel model = new RevisionModel();
            int tip = tipemprcodi.Length == 0 ? 0 : int.Parse(tipemprcodi);
            if (string.IsNullOrEmpty(nombre)) nombre = ConstantesRegistroIntegrantes.ParametroDefecto;

            model.Lista = this.servicio.ListByEstadoAndTipEmpr(estado, tip, nombre, nroPagina,
                Constantes.PageSize);

            //Consultar usuario directivo
            model.esUsuarioDE = base.VerificarAccesoAccion(Acciones.AccionRiDE, base.UserName);
            model.esUsuarioSGI = base.VerificarAccesoAccion(Acciones.AccionRiSGI, base.UserName);
            model.esUsuarioDJR = base.VerificarAccesoAccion(Acciones.AccionRiDJR, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Permite dar Conformidad a una revisión
        /// </summary>
        /// <param name="revicodi">codigo de revsión</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DarConformidad(int emprcodi)
        {
            SiEmpresaDTO objEmpresa = new SiEmpresaDTO();
            int reviCodiDJR;
            int reviCodiSGI;

            try
            {
                objEmpresa = this.servicio.GetEmpresaByIdConRevision(emprcodi);

                if (objEmpresa != null)
                {
                    objEmpresa.Lastuser = base.UserName;
                    objEmpresa.Lastdate = DateTime.Now;
                    objEmpresa.Emprestadoregistro = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroAprobadoFisicaCodigo;

                    var revisionDJR = this.servicio.GetByIdRiRevision(objEmpresa.ReviCodiDJR);
                    reviCodiDJR = revisionDJR.Revicodi;
                    this.servicio.DarConformidad(reviCodiDJR, objEmpresa);

                    var revisionSGI = this.servicio.GetByIdRiRevision(objEmpresa.ReviCodiSGI);
                    reviCodiSGI = revisionSGI.Revicodi;
                    this.servicio.DarConformidad(reviCodiSGI, objEmpresa);

                    //Agregado Garantia nro 7: agregar correlativo registro integrante
                    var nroRegistro = this.servicio.UpdateSiEmpresaCorrelativoRI(emprcodi);
                    //fin                    

                    if (!EnviarCredenciales(objEmpresa))
                        return Json(-1);
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
        /// Permite bajar la carta
        /// </summary>
        /// <param name="id">codigo de empresa</param>
        /// <returns></returns>      
        public FileResult BajarCarta(int id)
        {
            byte[] bytes = new byte[0];
            CartaModel model = new CartaModel();

            SiEmpresaDTO flujo = appEmpresa.ListarFlujoEmpresa(id).FirstOrDefault();
            SiEmpresaDTO empresa = appEmpresa.GetByIdSiEmpresa(id);
            SiRepresentanteDTO Representante = this.appRepresentante.GetByEmpresaSiRepresentante(id)
                .Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoLegal)
                .Where(x => x.Rptetiprepresentantelegal.ToString() == ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular).FirstOrDefault();

            if (Representante != null)
            {
                try
                {

                    model.NroRegistro = empresa.Emprnroregistro.ToString();
                    model.FechaSolicitud = (flujo != null) ? flujo.FLJFECHARECEP.Value.ToShortDateString() : string.Empty;
                    model.NroDocumento = (flujo != null) ? "COES/D-" + flujo.corrnumproc + "-" + DateTime.Now.Year : "CODS/D-XXX-" + DateTime.Now.Year;
                    model.Direccion = (empresa.Emprdomiciliolegal != null) ? empresa.Emprdomiciliolegal : "";
                    model.Empresa = empresa.Emprrazsocial;


                    //DateTime fechaFormato = null;
                    //var format = "dd de MMMM de yyyy";


                    //model.FechaRegistro
                    DateTime fechaFormato  = (flujo != null) ? flujo.Emprfechainscripcion.Value : 
                        DateTime.Now;

                    model.FechaRegistro = fechaFormato.ToString("dd") + " de " + Tools.ObtenerNombreMes(fechaFormato.Month).ToLower() + " de " + fechaFormato.ToString("yyyy");

                    model.Condicion = (flujo != null) ? flujo.Emprcondicion : "";

                    model.Nombre = Representante.Rptenombres + ' ' + Representante.Rpteapellidos;
                    model.Cargo = Representante.Rptecargoempresa;

                    //model.Distrito = ""; // TODO: no hay distrito 
                    bytes = (new WordDocument()).GenerarCarta(model);

                }
                catch
                {
                    bytes = null;
                }
            }
            else
                bytes = null;

            var FileName = string.Format("carta_{0}.docx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", FileName);
        }

        /// <summary>
        /// Permite Reenviar las credenciales de la Extranet a los Rpt. Legales
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReEnviarCredenciales(int emprcodi)
        {
            SiEmpresaDTO objEmpresa = new SiEmpresaDTO();
            objEmpresa = this.servicio.GetEmpresaByIdConRevision(emprcodi);
            if (objEmpresa != null)
            {
                if (!EnviarCredenciales(objEmpresa))
                    return Json(-1);
                else
                    return Json(1);
            }
            else
                return Json(-1);
        }

        /// <summary>
        /// Envia las credenciales de la Extranet a los representantes legales
        /// </summary>
        /// <param name="objEmpresa"></param>
        /// <returns></returns>
        bool EnviarCredenciales(SiEmpresaDTO objEmpresa)
        {
            RepresentanteAppServicio appRepresentante = new RepresentanteAppServicio();

            string toEmail = string.Empty;
            string ccEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];
            string representanteLegal = string.Empty;
            string empresa = string.Empty;

            var appEmpresa = new EmpresaAppServicio();
            var model = appEmpresa.GetByIdSiEmpresa((int)objEmpresa.Emprcodi);

            //Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
            empresa = (model.Emprnombrecomercial.Length > 3) ? model.Emprnombrecomercial : model.Emprrazsocial;
            //fin Garantia 9Jul18 

            //Correo a todos los representantes legales con su usuario y contraseña para la Extranet
            var lista = appRepresentante.GetByEmpresaSiRepresentante((int)objEmpresa.Emprcodi).
                Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoLegal).
                Where(x => x.Rptebaja.ToString() == ConstantesRegistroIntegrantes.RpteBajaNo).ToList();

            foreach (var item in lista)
            {
                representanteLegal = item.Rpteapellidos + ", " + item.Rptenombres;
                toEmail = item.Rptecorreoelectronico;
                try
                {
                    string usuario;
                    string contraseña;

                    var usuarioDto = Grabar(item);

                    if (usuarioDto != null)
                    {
                        usuario = usuarioDto.UserLogin;
                        contraseña = usuarioDto.UserPass;

                        string msgExtranet = RegistroIntegrantesHelper.Revision_BodyMailExtranet(representanteLegal, empresa, usuario, contraseña);
                        log.Info("Revisión - Envío de correo a representante legal");
                        COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, "Notificacion - Registro de Integrantes", msgExtranet);
                    }
                    else
                    {
                        log.Warn("No se ha conseguido crear el usuario.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Crea el usuario en la Extranet
        /// </summary>
        /// <param name="model">datos para crear el usuario</param>
        /// <returns></returns>
        public UserDTO Grabar(SiRepresentanteDTO model)
        {
            try
            {
                UserDTO usuario = new UserDTO();
                usuario.UserCode = 0;
                usuario.UsernName = model.Rptenombres + ' ' + model.Rpteapellidos;
                usuario.UserLogin = model.Rptecorreoelectronico;
                usuario.UserEmail = model.Rptecorreoelectronico;
                usuario.UserState = "A";// model.Estado;
                usuario.EmprCodi = (short)model.Emprcodi;
                usuario.UserTlf = model.Rptetelefono;
                usuario.AreaLaboral = "";//model.AreaLaboral;
                usuario.UserCargo = model.Rptecargoempresa;
                usuario.MotivoContacto = (!string.IsNullOrEmpty(model.Rptetelfmovil)) ? model.Rptetelfmovil.Trim() : null;
                usuario.UserIndReprLeg = Constantes.SI;
                usuario.LastUser = base.UserName;
                usuario.LastDate = DateTime.Now;

                List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO> listEmpresa = new List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO>();
                listEmpresa.Add(new COES.MVC.Intranet.SeguridadServicio.EmpresaDTO { EMPRCODI = (short)model.Emprcodi });

                usuario.ListaEmpresas = listEmpresa.ToArray();
                usuario.AreaCode = -1;
                usuario.Roles = ConfigurationManager.AppSettings[RutaDirectorio.RolRepresentanteLegal];

                bool flag = true;

                usuario.UserPass = this.seguridad.GenerarPassword();
                usuario.UserFCreacion = DateTime.Now;
                usuario.UserUCreacion = base.UserName;

                if (this.seguridad.ValidarExistencia(usuario.UserLogin))
                {
                    flag = false;
                }

                if (flag)
                {
                    int resultado = this.seguridad.GrabarUsuario(usuario);

                    return usuario;
                }
                else
                {
                    usuario = this.seguridad.ObtenerUsuarioPorLogin(usuario.UserLogin);
                    this.seguridad.EnviarPasswordNuevo(usuario.UserCode, base.UserName);
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Permite dar Conformidad a una revisión
        /// </summary>
        /// <param name="revicodi">codigo de revsión</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DarNotificar(int emprcodi)
        {
            RepresentanteAppServicio appRepresentante = new RepresentanteAppServicio();
            SiEmpresaDTO objEmpresa = new SiEmpresaDTO();
            int reviCodiDJR;
            int reviCodiSGI;

            try
            {
                objEmpresa = this.servicio.GetEmpresaByIdConRevision(emprcodi);

                if (objEmpresa != null)
                {
                    var revisionDJR = this.servicio.GetByIdRiRevision(objEmpresa.ReviCodiDJR);
                    reviCodiDJR = revisionDJR.Revicodi;

                    var revisionSGI = this.servicio.GetByIdRiRevision(objEmpresa.ReviCodiSGI);
                    reviCodiSGI = revisionSGI.Revicodi;

                    bool aceptado = false;

                    aceptado = (revisionSGI.Reviestado.ToUpper() == ConstantesRegistroIntegrantes.RevisionEstadoRevisadoCodigo &&
                                revisionDJR.Reviestado.ToUpper() == ConstantesRegistroIntegrantes.RevisionEstadoRevisadoCodigo) ? true : false;

                    //Actualizar el estado registro de la empresa a aprobado digitalmente
                    if (aceptado)
                    {
                        objEmpresa.Lastuser = base.UserName;
                        objEmpresa.Lastdate = DateTime.Now;
                        objEmpresa.Emprestadoregistro = ConstantesRegistroIntegrantes.EmpresaEstadoRegistroAprobadoDigitalCodigo;

                        this.servicio.DarNotificar(reviCodiDJR, objEmpresa);
                        this.servicio.DarNotificar(reviCodiSGI, objEmpresa);

                    }
                    else
                    {
                        this.servicio.DarNotificar(reviCodiDJR);
                        this.servicio.DarNotificar(reviCodiSGI);
                    }


                    ///Envio de Correo
                    string toEmail = string.Empty;
                    string ccEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];

                    string msg = string.Empty;
                    string enlace = string.Empty;
                    string responsableTramite = string.Empty;
                    string empresa = string.Empty;

                    var appEmpresa = new EmpresaAppServicio();
                    var model = appEmpresa.GetByIdSiEmpresa((int)objEmpresa.Emprcodi);

                    //Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
                    empresa = (model.Emprnombrecomercial.Length > 3) ? model.Emprnombrecomercial : model.Emprrazsocial;
                    //fin Garantia 9Jul18 

                    //Generar Enlace
                    var site = ConfigurationManager.AppSettings["SiteRegistroIntegrantes"];
                    enlace = site + "RegistroIntegrante/Integrante/edicion?codigo=" + RegistroIntegrantesHelper.Codifica(objEmpresa.Emprcodi.ToString());

                    //correo al responsable del trámite
                    try
                    {
                        var lista = appRepresentante.GetByEmpresaSiRepresentante((int)objEmpresa.Emprcodi).
                                Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoResponsableTramite).
                                Where(x => x.Rptebaja.ToString() == ConstantesRegistroIntegrantes.RpteBajaNo).ToList();

                        foreach (var item in lista)
                        {
                            responsableTramite = item.Rpteapellidos + ", " + item.Rptenombres;

                            if (aceptado)
                                msg = RegistroIntegrantesHelper.Revision_BodyMailAceptado(responsableTramite, empresa);
                            else
                                msg = RegistroIntegrantesHelper.Revision_BodyMailDenegado(responsableTramite, empresa, enlace);

                            toEmail = item.Rptecorreoelectronico;
                            log.Info("Revisión - Notificar - Envío de correo al responsable del tramite");
                            COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, "Notificacion - Registro de Integrantes", msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error", ex);
                    }

                    //correo personas de contacto
                    try
                    {
                        var lista = appRepresentante.GetByEmpresaSiRepresentante((int)objEmpresa.Emprcodi).
                                Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoContacto).
                                Where(x => x.Rptebaja.ToString() == ConstantesRegistroIntegrantes.RpteBajaNo).ToList();

                        foreach (var item in lista)
                        {
                            responsableTramite = item.Rpteapellidos + ", " + item.Rptenombres;

                            if (aceptado)
                                msg = RegistroIntegrantesHelper.Revision_BodyMailAceptado(responsableTramite, empresa);
                            else
                                msg = RegistroIntegrantesHelper.Revision_BodyMailDenegado(responsableTramite, empresa, enlace);

                            toEmail = item.Rptecorreoelectronico;
                            log.Info("Revisión - Notificar - Envío de correo a personas de contacto");
                            COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, "Notificacion - Registro de Integrantes", msg);
                        }
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
        /// Permite terminar el proceso de revision fisico antes de dar conformidad por DE
        /// </summary>
        /// <param name="revicodi">codigo de revision</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DarTerminar(int emprcodi)
        {
            RepresentanteAppServicio appRepresentante = new RepresentanteAppServicio();
            SiEmpresaDTO objEmpresa = new SiEmpresaDTO();
            int reviCodiDJR;
            int reviCodiSGI;

            try
            {
                objEmpresa = this.servicio.GetEmpresaByIdConRevision(emprcodi);

                if (objEmpresa != null)
                {
                    var revisionDJR = this.servicio.GetByIdRiRevision(objEmpresa.ReviCodiDJR);
                    reviCodiDJR = revisionDJR.Revicodi;

                    var revisionSGI = this.servicio.GetByIdRiRevision(objEmpresa.ReviCodiSGI);
                    reviCodiSGI = revisionSGI.Revicodi;

                    bool aceptado = false;

                    aceptado = (revisionSGI.Reviestado.ToUpper() == ConstantesRegistroIntegrantes.RevisionEstadoRevisadoCodigo &&
                                revisionDJR.Reviestado.ToUpper() == ConstantesRegistroIntegrantes.RevisionEstadoRevisadoCodigo) ? true : false;

                    //Solo se permite terminar si SGI y DJR estan respectivamente aprobadoas (revisado)
                    if (aceptado)
                    {
                        this.servicio.DarTerminar(reviCodiDJR);
                        this.servicio.DarTerminar(reviCodiSGI);
                    }
                    else
                        return Json(-1);
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
        /// Permite devolver una revisión al asistente
        /// </summary>
        /// <param name="revicodi">codigo de revision</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RevisarAsistente(int revicodi)
        {
            try
            {
                this.servicio.RevAsistente(revicodi);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar la revisión DJR
        /// </summary>
        /// <param name="model">model RevisionDJRModel</param>
        /// <returns></returns>
        public bool GrabarDJR(RevisionDJRModel model)
        {
            int id = model.EmprCodi;
            bool resul = false;

            UserDTO userlogin = (UserDTO)Session[DatosSesion.SesionUsuario];
            RiRevisionDTO objRevision = new RiRevisionDTO();
            try
            {
                if (id != 0)
                {
                    string[] Data = model.Data.Split('|');

                    RiDetalleRevisionDTO objDetalleRevision = null;
                    List<RiDetalleRevisionDTO> DetalleRevision = new List<RiDetalleRevisionDTO>();
                    foreach (var item in Data)
                    {
                        objDetalleRevision = new RiDetalleRevisionDTO();
                        string[] Col = item.Split('*');
                        objDetalleRevision.Dervcampo = ConstantesRegistroIntegrantes.DerCampoRepresentante;
                        objDetalleRevision.Dervvalor = Col[0]; // Id Representante
                        objDetalleRevision.Dervobservacion = Col[1]; // Id Representante
                        objDetalleRevision.Dervadjunto = ConstantesRegistroIntegrantes.RevisionDetalleAdjuntoSi;
                        objDetalleRevision.Dervvaloradjunto = Col[2] + "," + Col[3]; // Adjunto + Fileadjunto
                        objDetalleRevision.Dervestadorevision = Col[4];
                        objDetalleRevision.Dervestado = ConstantesRegistroIntegrantes.RevisionDetalleEstadoActivo;
                        objDetalleRevision.Dervusucreacion = userlogin.UserCode.ToString();
                        objDetalleRevision.Dervusumoficicacion = userlogin.UserCode.ToString();
                        objDetalleRevision.Dervfeccreacion = DateTime.Now;
                        objDetalleRevision.Dervfecmodificacion = DateTime.Now;
                        DetalleRevision.Add(objDetalleRevision);
                    }

                    if (!string.IsNullOrEmpty(model.DataTA))
                    {
                        string[] DataTA = model.DataTA.Split('|');
                        foreach (var item in DataTA)
                        {
                            objDetalleRevision = new RiDetalleRevisionDTO();
                            string[] Col = item.Split('*');
                            objDetalleRevision.Dervcampo = ConstantesRegistroIntegrantes.DerCampoTA;
                            objDetalleRevision.Dervvalor = Col[0]; // Numero Titualo Adicional del a 1 al 5
                            objDetalleRevision.Dervobservacion = Col[1]; // 
                            objDetalleRevision.Dervadjunto = ConstantesRegistroIntegrantes.RevisionDetalleAdjuntoSi;
                            objDetalleRevision.Dervvaloradjunto = Col[2] + "," + Col[3]; // Adjunto + Fileadjunto
                            objDetalleRevision.Dervestadorevision = Col[4];
                            objDetalleRevision.Dervestado = ConstantesRegistroIntegrantes.RevisionDetalleEstadoActivo;
                            objDetalleRevision.Dervusucreacion = userlogin.UserCode.ToString();
                            objDetalleRevision.Dervusumoficicacion = userlogin.UserCode.ToString();
                            objDetalleRevision.Dervfeccreacion = DateTime.Now;
                            objDetalleRevision.Dervfecmodificacion = DateTime.Now;
                            DetalleRevision.Add(objDetalleRevision);
                        }
                    }

                    objRevision.Detalle = DetalleRevision;
                    objRevision.Revicodi = model.Revicodi;

                    objRevision.Reviusurevision = userlogin.UserCode;
                    objRevision.Revifecrevision = DateTime.Now;

                    objRevision.Revifinalizado = model.Revifinalizado;
                    if (model.Revifinalizado == ConstantesRegistroIntegrantes.RevisionFinalizadoSi)
                    {
                        objRevision.Revifecfinalizado = DateTime.Now;
                    }

                    objRevision.Reviterminado = ConstantesRegistroIntegrantes.RevisionTerminadoNo;
                    objRevision.Revifecterminado = null;

                    objRevision.Revinotificado = ConstantesRegistroIntegrantes.RevisionNotificadoNo;
                    objRevision.Revifecnotificado = null;

                    objRevision.Reviusucreacion = userlogin.UserCode.ToString();

                    objRevision.Reviusumodificacion = userlogin.UserCode.ToString();
                    objRevision.Revifecmodificacion = DateTime.Now;

                    objRevision.Revienviado = ConstantesRegistroIntegrantes.RevisionEnviadoNo;
                    objRevision.Revifecenviado = null;

                    if (DetalleRevision.Exists(x => x.Dervestadorevision.ToUpper() == ConstantesRegistroIntegrantes.RevisionEstadoObservado))
                    {
                        objRevision.Reviestado = ConstantesRegistroIntegrantes.RevisionEstadoObservado;
                    }
                    else
                    {
                        objRevision.Reviestado = ConstantesRegistroIntegrantes.RevisionEstadoRevisado;
                    }


                    resul = servicio.SaveRevisionDJR(objRevision);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return resul;
        }

        /// <summary>
        /// Permite grabar la revisión DJR
        /// </summary>
        /// <param name="model">model RevisionDJRModel</param>
        /// <returns></returns>
        public bool GrabarSGI(RevisionSGIModel model)
        {
            int id = model.EmprCodi;
            bool resul = false;

            UserDTO userlogin = (UserDTO)Session[DatosSesion.SesionUsuario];
            RiRevisionDTO objRevision = new RiRevisionDTO();
            try
            {
                if (id != 0)
                {
                    string[] Data = model.Data.Split('|');

                    RiDetalleRevisionDTO objDetalleRevision = null;
                    List<RiDetalleRevisionDTO> DetalleRevision = new List<RiDetalleRevisionDTO>();
                    foreach (var item in Data)
                    {
                        objDetalleRevision = new RiDetalleRevisionDTO();
                        string[] Col = item.Split('*');
                        objDetalleRevision.Dervcampo = Col[0];
                        objDetalleRevision.Dervvalor = Col[1]; // Id Representante
                        objDetalleRevision.Dervobservacion = Col[2]; // Id Representante
                        objDetalleRevision.Dervadjunto = ConstantesRegistroIntegrantes.RevisionDetalleAdjuntoSi;
                        objDetalleRevision.Dervvaloradjunto = Col[3] + "," + Col[4]; // Adjunto + Fileadjunto
                        objDetalleRevision.Dervestadorevision = Col[5];
                        objDetalleRevision.Dervestado = ConstantesRegistroIntegrantes.RevisionDetalleEstadoActivo;
                        objDetalleRevision.Dervusucreacion = userlogin.UserCode.ToString();
                        objDetalleRevision.Dervusumoficicacion = userlogin.UserCode.ToString();
                        objDetalleRevision.Dervfeccreacion = DateTime.Now;
                        objDetalleRevision.Dervfecmodificacion = DateTime.Now;
                        DetalleRevision.Add(objDetalleRevision);
                    }

                    objRevision.Detalle = DetalleRevision;
                    objRevision.Revicodi = model.Revicodi;

                    objRevision.Revifecrevision = DateTime.Now;
                    objRevision.Reviusurevision = userlogin.UserCode;

                    objRevision.Revifinalizado = model.Revifinalizado;
                    if (model.Revifinalizado == ConstantesRegistroIntegrantes.RevisionFinalizadoSi)
                    {
                        objRevision.Revifecfinalizado = DateTime.Now;
                    }

                    objRevision.Reviterminado = ConstantesRegistroIntegrantes.RevisionTerminadoNo;
                    objRevision.Revifecterminado = null;

                    objRevision.Revinotificado = ConstantesRegistroIntegrantes.RevisionNotificadoNo;
                    objRevision.Revifecnotificado = null;

                    objRevision.Revienviado = ConstantesRegistroIntegrantes.RevisionEnviadoNo;
                    objRevision.Revifecenviado = null;

                    objRevision.Reviusucreacion = userlogin.UserCode.ToString();

                    objRevision.Reviusumodificacion = userlogin.UserCode.ToString();
                    objRevision.Revifecmodificacion = DateTime.Now;


                    if (DetalleRevision.Exists(x => x.Dervestadorevision.ToUpper() == ConstantesRegistroIntegrantes.RevisionEstadoObservado))
                    {
                        objRevision.Reviestado = ConstantesRegistroIntegrantes.RevisionEstadoObservado;
                    }
                    else
                    {
                        objRevision.Reviestado = ConstantesRegistroIntegrantes.RevisionEstadoRevisado;
                    }

                    resul = servicio.SaveRevisionSGI(objRevision);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return resul;
        }

        /// <summary>
        /// Permite visualizar el detalle de revisión SGI
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RevisionSGI(int id)
        {
            RevisionSGIModel model = new RevisionSGIModel();
            RiRevisionDTO revision = new RiRevisionDTO();
            SiEmpresaDTO objEmpresa = new SiEmpresaDTO();
            List<SiRepresentanteDTO> ListRepresentates = new List<SiRepresentanteDTO>();
            List<RiDetalleRevisionDTO> ListDetalles = new List<RiDetalleRevisionDTO>();
            List<SiTipoComportamientoDTO> ListComportamiento = new List<SiTipoComportamientoDTO>();

            objEmpresa = this.servicio.GetEmpresaByIdConRevision(id);
            revision = this.servicio.GetByIdRiRevision(objEmpresa.ReviCodiSGI);
            ListDetalles = this.servicio.ListRiDetalleRevisionByRevicodi(revision.Revicodi);

            ListComportamiento = this.servicio.ListSiTipoComportamientoByEmprcodi(objEmpresa.Emprcodi);

            model.Reviterminado = revision.Reviterminado;
            model.Revinotificado = revision.Revinotificado;
            model.Revienviado = revision.Revienviado;
            model.Revifinalizado = revision.Revifinalizado;
            model.Reviestado = revision.Reviestado;
            model.UsuarioRevision = revision.Reviusucreacion;
            model.Correlativo = revision.Revicodi.ToString();
            model.Iteracion = revision.Reviiteracion.ToString();

            model.EmprCodi = objEmpresa.Emprcodi;
            model.EmprNombreComercial = objEmpresa.Emprnombrecomercial;
            model.EmprRazSocial = objEmpresa.Emprrazsocial;
            model.EmprRUC = objEmpresa.Emprruc;
            model.EmprSigla = objEmpresa.Emprsigla;
            model.EmprDomLegal = objEmpresa.Emprdomiciliolegal;


            List<SiTipoempresaDTO> tipos = new List<SiTipoempresaDTO>();
            tipos = this.servicio.ListTipoEmpresa();

            model.Revicodi = objEmpresa.ReviCodiSGI;

            model.EmprTipoAgente = tipos.Find(x => x.Tipoemprcodi == objEmpresa.Tipoemprcodi).Tipoemprdesc;
            model.EmprTipoAgenteCodigo = objEmpresa.Tipoemprcodi;
            model.TipoComportamiento = ListComportamiento.Find(x => x.Tipoinicial == ConstantesRegistroIntegrantes.TipoAgenteInicialSi);

            if (ListDetalles != null && ListDetalles.Count > 0)
            {
                model.TieneRevisionDetalle = true;
                foreach (var item in ListDetalles)
                {
                    switch (item.Dervcampo)
                    {
                        case ConstantesRegistroIntegrantes.DocumentoSustentatorio:
                            model.TipoComportamiento.TipodocsustentatorioComentario = item.Dervobservacion;
                            break;
                        case ConstantesRegistroIntegrantes.ArchivoDigitalizado:
                            model.TipoComportamiento.TipoarcdigitalizadoComentario = item.Dervobservacion;
                            break;
                        case ConstantesRegistroIntegrantes.PotenciaInstalada:
                            model.TipoComportamiento.TipopotenciainstaladaComentario = item.Dervobservacion;
                            break;
                        case ConstantesRegistroIntegrantes.NumeroCentrales:
                            model.TipoComportamiento.TiponrocentralesComentario = item.Dervobservacion;
                            break;
                        case ConstantesRegistroIntegrantes.TotalLineaTransmision:
                            model.TipoComportamiento.TipototallineastransmisionComentario = item.Dervobservacion;
                            break;
                        case ConstantesRegistroIntegrantes.MaximaDemandaCoincidente:
                            model.TipoComportamiento.TipomaxdemandacoincidenteComentario = item.Dervobservacion;
                            break;
                        case ConstantesRegistroIntegrantes.MaximaDemandaContratada:
                            model.TipoComportamiento.TipomaxdemandacontratadaComentario = item.Dervobservacion;
                            break;
                        case ConstantesRegistroIntegrantes.NumeroSuministrador:
                            model.TipoComportamiento.TiponumsuministradorComentario = item.Dervobservacion;
                            break;
                    }
                }
            }

            return View(model);
        }

        /// <summary>
        /// Permite visualizar el detalle de la revisión para DJR
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RevisionDJR(int id)
        {
            RevisionDJRModel model = new RevisionDJRModel();
            //RiRevisionDTO revision = new RiRevisionDTO();

            SiEmpresaDTO objEmpresa = new SiEmpresaDTO();
            List<SiRepresentanteDTO> ListRepresentates = new List<SiRepresentanteDTO>();
            List<RiDetalleRevisionDTO> ListDetalles = new List<RiDetalleRevisionDTO>();

            objEmpresa = this.servicio.GetEmpresaByIdConRevision(id);

            if (objEmpresa != null)
            {
                model.RiRevision = this.servicio.GetByIdRiRevision(objEmpresa.ReviCodiDJR);
                model.Revicodi = id;

                var TipoComportamiento = this.servicio.ListSiTipoComportamientoByEmprcodi(id);
                List<TitulosAdicionalesModel> ListTitulosAdicionales = new List<TitulosAdicionalesModel>();

                if (TipoComportamiento.Count > 0)
                {

                    var TipoPrincipal = TipoComportamiento.Find(x => x.Tipoinicial == ConstantesRegistroIntegrantes.TipoAgenteInicialSi);
                    model.ComentarioTitulosAdicionales = TipoPrincipal.Tipocomentario;

                    if (TipoPrincipal != null)
                    {
                        TitulosAdicionalesModel oTitulosAdicionalesModel = null;
                        if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname1) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename1))
                        {
                            oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                            oTitulosAdicionalesModel.Numero = 1;
                            oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname1;
                            oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename1;
                            ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                            if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname2) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename2))
                            {
                                oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                                oTitulosAdicionalesModel.Numero = 2;
                                oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname2;
                                oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename2;
                                ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                                if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname3) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename3))
                                {
                                    oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                                    oTitulosAdicionalesModel.Numero = 3;
                                    oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname3;
                                    oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename3;
                                    ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                                    if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname4) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename4))
                                    {
                                        oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                                        oTitulosAdicionalesModel.Numero = 4;
                                        oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname4;
                                        oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename4;
                                        ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                                        if (!string.IsNullOrEmpty(TipoPrincipal.Tipodocname5) && !string.IsNullOrEmpty(TipoPrincipal.Tipodocadjfilename5))
                                        {
                                            oTitulosAdicionalesModel = new TitulosAdicionalesModel();
                                            oTitulosAdicionalesModel.Numero = 5;
                                            oTitulosAdicionalesModel.Nombre = TipoPrincipal.Tipodocname5;
                                            oTitulosAdicionalesModel.FileName = TipoPrincipal.Tipodocadjfilename5;
                                            ListTitulosAdicionales.Add(oTitulosAdicionalesModel);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                ListDetalles = this.servicio.ListRiDetalleRevisionByRevicodi(model.RiRevision.Revicodi);
                ListRepresentates = this.servicio.GetRepresentantesByEmprcodi(id);
                ListRepresentates = ListRepresentates.FindAll(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoLegal
                                                                && x.Rpteinicial == ConstantesRegistroIntegrantes.RepresentanteInicialSi);

                foreach (SiRepresentanteDTO item in ListRepresentates)
                {
                    item.RpteObservacion = "";
                    if (item.Rptetiprepresentantelegal == ConstantesRegistroIntegrantes.Titular)
                    {
                        item.Rptetipodesc = ConstantesRegistroIntegrantes.TitularDesc;
                    }
                    else
                    {
                        item.Rptetipodesc = ConstantesRegistroIntegrantes.AlternoDesc;
                    }

                    int index = ListDetalles.FindIndex(x => x.Dervvalor == item.Rptecodi.ToString()
                                && x.Dervcampo == ConstantesRegistroIntegrantes.DerCampoRepresentante);

                    if (index >= 0)
                    {
                        item.RpteObservacion = ListDetalles[index].Dervobservacion;
                        item.Dervcodi = ListDetalles[index].Dervcodi;
                    }
                }

                foreach (TitulosAdicionalesModel item in ListTitulosAdicionales)
                {
                    item.RpteObservacion = "";

                    int index = ListDetalles.FindIndex(x => x.Dervvalor == item.Numero.ToString()
                                && x.Dervcampo == ConstantesRegistroIntegrantes.DerCampoTA);

                    if (index >= 0)
                    {
                        item.RpteObservacion = ListDetalles[index].Dervobservacion;
                        item.Dervcodi = ListDetalles[index].Dervcodi;
                    }
                }

                List<SiTipoempresaDTO> tipos = new List<SiTipoempresaDTO>();
                tipos = this.servicio.ListTipoEmpresa();

                model.Correlativo = model.RiRevision.Revicodi.ToString();
                model.Iteracion = model.RiRevision.Reviiteracion.ToString();

                model.Revicodi = objEmpresa.ReviCodiDJR;
                model.EmprTipoAgente = tipos.Find(x => x.Tipoemprcodi == objEmpresa.Tipoemprcodi).Tipoemprdesc;
                model.EmprTipoAgenteCodigo = objEmpresa.Tipoemprcodi;
                model.EmprCodi = objEmpresa.Emprcodi;
                model.EmprNombreComercial = objEmpresa.Emprnombrecomercial;
                model.EmprRazSocial = objEmpresa.Emprrazsocial;
                model.EmprRUC = objEmpresa.Emprruc;
                model.EmprSigla = objEmpresa.Emprsigla;
                model.EmprDomLegal = objEmpresa.Emprdomiciliolegal;
                model.ListaRepresentantes = ListRepresentates;
                model.TitulosAdicionales = ListTitulosAdicionales;
            }

            return View(model);
        }

        /// <summary>
        /// Permite bajar el archivo
        /// </summary>
        /// <param name="url">nombre de archivo</param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url)
        {
            string[] par = url.Split('*');
            //var Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;

            Stream str = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + par[1], ConstantesRegistroIntegrantes.FolderUploadRutaCompleta);

            return File(str, "application/pdf", par[0]);
        }

        /// <summary>
        /// Permite visualizar el archivo
        /// </summary>
        /// <param name="url">nombre de archivo</param>
        /// <returns></returns>
        public FileStreamResult ver(string url)
        {
            string[] par = url.Split('*');
            //var Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;
            //var pdfContent = new MemoryStream(System.IO.File.ReadAllBytes(Path + par[1]));
            //pdfContent.Position = 0;
            Stream str = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + par[1], ConstantesRegistroIntegrantes.FolderUploadRutaCompleta);
            return new FileStreamResult(str, "application/pdf");
        }

        /// <summary>
        /// Permite bajar el archivo de mesa de partes
        /// </summary>
        /// <param name="url">nombre de archivo generado</param>
        /// <param name="nombre">nombre del archivo original</param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DownloadDocumentoFlujo(string url, string nombre)
        {
            try
            {
                Stream stream = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + url, ConstantesRegistroIntegrantes.FolderUploadRutaCompleta + ConstantesRegistroIntegrantes.FolderRI);
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
        /// Permite visualizar el archivo de mesa de partes
        /// </summary>
        /// <param name="url">nombre de archivo</param>
        /// <returns></returns>
        public FileStreamResult verDocumentoFlujo(string url)
        {
            try
            {
                Stream stream = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + url, ConstantesRegistroIntegrantes.FolderUploadRutaCompleta + ConstantesRegistroIntegrantes.FolderRI);
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
        /// Permite cargar los registros de SGDOC - flujos
        /// </summary>
        /// <param name="emprcodi">Codigo empresa</param>
        [HttpPost]
        public PartialViewResult VerFlujo(int emprcodi)
        {
            FlujoModel model = new FlujoModel();

            var flujo = appEmpresa.ListarFlujoEmpresa(emprcodi);
            model.Emprcodi = emprcodi;


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

        /// <summary>
        /// Exporta a PDF la constancia de registro
        /// </summary>
        /// <param name="id">codigo de empresa</param>
        /// <returns></returns>
        public FileResult ExportarConstanciaPDF(int id)
        {
            byte[] bytes = new byte[0];
            string FileName = "Error.pdf";
            int nroConstancia = 0;
            try
            {
                bytes = appRegistroIntegranteReporte.ExportarPDF(id, out nroConstancia);
                FileName = string.Format("Empresa-{0}", nroConstancia);
                if (bytes == null)
                {
                    return null;
                }
                else
                {
                    return File(bytes, "application/pdf", FileName + ".pdf");
                }
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Exporta a PDF el registro de registro
        /// </summary>
        /// <param name="id">codigo de empresa</param>
        /// <returns></returns>
        public FileResult ExportarRegistroPDF(int id)
        {
            byte[] bytes = new byte[0];
            string FileName = "Error.pdf";
            int nroRegistro = 0;
            try
            {
                bytes = appRegistroIntegranteReporte.ExportarRegistroPDF(id, out nroRegistro);
                FileName = string.Format("Empresa-{0}", nroRegistro);
                if (bytes == null)
                {
                    return null;
                }
                else
                {
                    return File(bytes, "application/pdf", FileName + ".pdf");
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
