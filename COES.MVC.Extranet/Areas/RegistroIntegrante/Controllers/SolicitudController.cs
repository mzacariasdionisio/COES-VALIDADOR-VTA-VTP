using COES.MVC.Extranet.Areas.RegistroIntegrante.Models;
using COES.MVC.Extranet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using log4net;
using System.IO;
using COES.Framework.Base.Tools;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Controllers
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
        /// Permite retornar el model correspondiente
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SolicitudPendienteModel model = new SolicitudPendienteModel();

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
            UserDTO userlogin = (UserDTO)Session[DatosSesion.SesionUsuario];
            int EmprCodi = userlogin.EmprCodi.Value;

            PaginadoModel model = new PaginadoModel();
            model.IndicadorPagina = false;

            int nroRegistros = this.appSolicitud.ObtenerTotalRegListPendporEmpresa(soliestado, EmprCodi);

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
            UserDTO userlogin = (UserDTO)Session[DatosSesion.SesionUsuario];
            int EmprCodi = userlogin.EmprCodi.Value;

            SolicitudPendienteModel model = new SolicitudPendienteModel();
            model.ListaSolicitudes = this.appSolicitud.ListarSolicitudesPendientesporEmpresa(
                                        soliestado,
                                        nroPagina,
                                        Constantes.PageSize,
                                        EmprCodi);

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
        /// Permite verificar el tipo de una solicitud
        /// </summary>
        /// <param name="solicodi">codigo de solicitud</param>
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
        /// Permite pintar la lista de registros de representantes legales de la solicitud cambio de representante legal
        /// </summary>
        /// <param name="solicodi">codigo de solictud</param> 
        /// <returns></returns>
        public PartialViewResult ListadoRepresentante(int solicodi)
        {
            int idEmpresa = base.EmpresaId;

            List<SiRepresentanteDTO> DatosRepresentante;
            RepresentanteModel modelRepresentante = new RepresentanteModel();

            try
            {
                if (solicodi == 0)
                    DatosRepresentante = this.appRepresentante.GetByEmpresaSiRepresentante(idEmpresa)
                        .Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoLegal)
                        .Where(x => x.Rptebaja.ToString() == ConstantesRegistroIntegrantes.RpteBajaNo).ToList();
                else
                    DatosRepresentante = ObtenerDatosRepresentante(solicodi);

                modelRepresentante.ListaRepresentantes = DatosRepresentante;

                return PartialView(modelRepresentante);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return PartialView(modelRepresentante);
            }
        }

        /// <summary>
        /// Permite pintar la lista de registros de tipo comportamiento de la solicitud de cambio de tipo
        /// </summary>
        /// <param name="solicodi">codigo de solicitud</param> 
        /// <returns></returns>
        public PartialViewResult ListadoTipo(int solicodi)
        {
            int idEmpresa = base.EmpresaId;

            List<SiTipoComportamientoDTO> DatosTipo;
            TipoModel modelTipo = new TipoModel();

            try
            {
                if (solicodi == 0)
                    DatosTipo = this.appTipo.GetByEmpresaSiTipo(idEmpresa)
                        .Where(x => x.Tipobaja.ToString() == ConstantesRegistroIntegrantes.RpteBajaNo).ToList();
                else
                    DatosTipo = ObtenerDatosTipo(solicodi);

                modelTipo.ListaTipos = DatosTipo;
                return PartialView(modelTipo);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return PartialView(modelTipo);
            }
        }


        /// <summary>
        /// Permite preparar los datos del un nuevo registro para una solicitud
        /// </summary>
        /// <param name="view">nombre de la vista para entregar la vista y el model de la solicitud</param>
        /// <returns></returns>
        public ActionResult Nuevo(string view)
        {
            int emprcodi;
            UserDTO userlogin = (UserDTO)Session[DatosSesion.SesionUsuario];
            emprcodi = userlogin.EmprCodi.Value;

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
                          ConstantesRegistroIntegrantes.RpteDescripcionTipoDocumentoDNI)
                                                                );
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

            objSolicitud.Emprcodi = userLogin.EmprCodi;
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

                    this.appSolicitud.EnviarCorreoSolicitudAgente((int)userLogin.EmprCodi, ConstantesRegistroIntegrantes.SoliCambioTipo);                                       
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

            objSolicitud.Emprcodi = userLogin.EmprCodi;
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
                        //Sdetvaloradjunto = modelSolicitud.DocumentoSustentatorio.FileName + "*" +  FileNameCartaAdjunto
                        Sdetvaloradjunto = modelSolicitud.DocumentoNombre + "*" + modelSolicitud.DocumentoAdjunto
                    });

                    int CodigoSolicitud = this.appSolicitud.Save(objSolicitud);
                    foreach (RiSolicituddetalleDTO item in listaDetalles)
                    {
                        item.Solicodi = CodigoSolicitud;
                        item.Sdetfeccreacion = DateTime.Now;
                        item.Sdetusucreacion = userLogin.UserCode.ToString();
                    }
                    id = this.appSolicitud.SaveDetails(listaDetalles);
                    this.appSolicitud.EnviarCorreoSolicitudAgente((int)userLogin.EmprCodi, ConstantesRegistroIntegrantes.SoliCambioRepresentante);
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

            objSolicitud.Emprcodi = userLogin.EmprCodi;
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
                }
                id = this.appSolicitud.SaveDetails(listaDetalles);

                this.appSolicitud.EnviarCorreoSolicitudAgente((int)userLogin.EmprCodi, ConstantesRegistroIntegrantes.SoliCambioDenominacion);

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

            objSolicitud.Emprcodi = userLogin.EmprCodi;
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
                string CondicionBajaValor = "";
                switch (modelSolicitud.CondicionBaja)
                {
                    case ConstantesRegistroIntegrantes.CondicionBajaVoluntarioCodi:
                        CondicionBajaValor = ConstantesRegistroIntegrantes.CondicionBajaVoluntario; break;
                    case ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionAgenteCodi:
                        CondicionBajaValor = ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionAgente; break;
                    case ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionObligatorioCodi:
                        CondicionBajaValor = ConstantesRegistroIntegrantes.CondicionBajaPerdidaCondicionObligatorio; break;
                    case ConstantesRegistroIntegrantes.CondicionBajaConclusionCodi:
                        CondicionBajaValor = ConstantesRegistroIntegrantes.CondicionBajaConclusionCodi; break;
                }

                listaDetalles.Add(new RiSolicituddetalleDTO()
                {
                    Sdetcampo = ConstantesRegistroIntegrantes.CondicionBajaCampo,
                    Sdetvalor = CondicionBajaValor
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

                this.appSolicitud.EnviarCorreoSolicitudAgente((int)userLogin.EmprCodi, ConstantesRegistroIntegrantes.SoliBajaEmpresa);

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

            objSolicitud.Emprcodi = userLogin.EmprCodi;
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
                }

                id = this.appSolicitud.SaveDetails(listaDetalles);


                this.appSolicitud.EnviarCorreoSolicitudAgente((int)userLogin.EmprCodi, ConstantesRegistroIntegrantes.SoliFusionEmpresa);

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
                        else if (item.Sdetadjunto.Trim() == ConstantesRegistroIntegrantes.EsAdjunto)
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
                            //RN: En Extranet solo se permite una sola condicion de baja, la voluntaria
                            model.CondicionBaja = item.Sdetvalor;
                            switch (model.CondicionBaja)
                            {
                                case ConstantesRegistroIntegrantes.CondicionBajaVoluntarioCodi:
                                    model.CondicionBajaDescripcion = ConstantesRegistroIntegrantes.CondicionBajaVoluntario;
                                    break;
                            }
                        }
                        else if (item.Sdetadjunto.Trim() == ConstantesRegistroIntegrantes.EsAdjunto)
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
                                                                                        ConstantesRegistroIntegrantes.RpteDescripcionTipoDocumentoDNI,
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
                            Tipoprincipal = camposRL[1].ToString() == ConstantesRegistroIntegrantes.TipoAgenteDescripcionPrincipal ?
                                                                        ConstantesRegistroIntegrantes.TipoAgenteCodigoPrincipal :
                                                                        ConstantesRegistroIntegrantes.TipoAgenteCodigoSecundario,
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
