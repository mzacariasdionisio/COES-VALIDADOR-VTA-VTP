using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.App_Start;
using COES.MVC.Extranet.Areas.Ensayo.Helper;
using COES.MVC.Extranet.Areas.Ensayo.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.MVC.Extranet.ServiceReferenceMail;
using COES.Servicios.Aplicacion.ensayo;
using COES.Servicios.Aplicacion.Ensayo.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Extranet.Areas.Ensayo.Controllers
{
    [ValidarSesion]
    public class GeneraController : BaseController
    {
        EnsayoAppServicio servicio = new EnsayoAppServicio();
        public HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();
        private MailClient appMail = new MailClient();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(GeneraController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #endregion

        #region Variables de Sesión
        int IdAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);
        public int CodigoEnsayo
        {
            get
            {
                return (Session[SesionEnsayo.SesionIdEnsayo] != null) ?
                   int.Parse(Session[SesionEnsayo.SesionIdEnsayo].ToString()) : 0;
            }
            set { Session[SesionEnsayo.SesionIdEnsayo] = value; }
        }
        public int IdNroFormatoAnt
        {
            get
            {
                return (Session[SesionEnsayo.SesionIdFormAnt] != null) ?
                   int.Parse(Session[SesionEnsayo.SesionIdFormAnt].ToString()) : 0;
            }
            set { Session[SesionEnsayo.SesionIdFormAnt] = value; }
        }
        public int IdNroOrdenEnsayo
        {
            get
            {
                return (Session[SesionEnsayo.SesionNroOrdenEnsayo] != null) ?
                   int.Parse(Session[SesionEnsayo.SesionNroOrdenEnsayo].ToString()) : 0;
            }
            set { Session[SesionEnsayo.SesionNroOrdenEnsayo] = value; }
        }
        public string Empresas
        {
            get
            {
                return (Session["Empresas"] != null) ?
                   Session["Empresas"].ToString() : "-1";
            }
            set { Session["Empresas"] = value; }
        }

        #endregion

        #region Index

        /// <summary>
        /// Página de inicio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();

            int idAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);
            BusquedaEnsayoModel model = new BusquedaEnsayoModel();

            int idOpcion = (Session[DatosSesion.SesionIdOpcion] == null) ? 0 : (int)Session[DatosSesion.SesionIdOpcion];
            model.OpNuevo = this.seguridad.ValidarPermisoOpcion(idAplicacion, idOpcion, Acciones.Nuevo, User.Identity.Name);
            model.OpAccesoEmpresa = this.seguridad.ValidarPermisoOpcion(idAplicacion, idOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
            model.OpEditar = this.seguridad.ValidarPermisoOpcion(idAplicacion, idOpcion, Acciones.Editar, User.Identity.Name);

            model.FechaInicio = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            model.ListaEstado = servicio.ListEnEstadoss().Where(x => x.Estadocodi <= EstadoEnsayo.Aprobado).ToList();
            model.ListaEmpresas = ListarEmpresasEnsayoPotencia();
            string strEmpresas = string.Join(",", model.ListaEmpresas.Select(x => x.Emprcodi).ToList());
            this.Empresas = strEmpresas;
            model.ListaEquipo = servicio.ListEqEquipoEmpresaGEN2(strEmpresas).ToList();

            return View(model);
        }

        /// <summary>
        /// Obtiene las centrales de la empresa seleccionada en el View para ajax con PartialView
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarCentrales(string idEmpresa)
        {
            BusquedaEnsayoModel model = new BusquedaEnsayoModel();
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            if (idEmpresa != "-1")
            {
                entitys = servicio.ListEqEquipoEmpresaGEN2(idEmpresa).ToList();
            }
            else
            {
                entitys = servicio.ListEqEquipoEmpresaGEN2("-1").ToList();
            }
            model.ListaEquipo = entitys;
            return PartialView(model);
        }

        #endregion

        #region Listado - Búsqueda

        /// <summary>
        /// Obtiene los ensayos segun el filtro seleccionado
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="centrales"></param>
        /// <param name="estados"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaEnsayo(string empresas, int nroPaginas, string centrales, int estado, string finicios,
            string ffins)
        {
            DateTime fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = fechaFin.AddDays(1);
            EnsayoModel model = new EnsayoModel();

            //model.IdEstado = int.Parse(estado);
            model.IdEstado = estado;
            string estados = "-1";
            if (empresas == "-1")
                empresas = this.Empresas;
            int idOpcion = (Session[DatosSesion.SesionIdOpcion] == null) ? 0 : (int)Session[DatosSesion.SesionIdOpcion];
            model.OpAprobar = this.seguridad.ValidarPermisoOpcion(this.IdAplicacion, idOpcion, Acciones.Aprobar, User.Identity.Name);
            model.OpEditar = this.seguridad.ValidarPermisoOpcion(this.IdAplicacion, idOpcion, Acciones.Editar, User.Identity.Name);
            model.ListaEnsayo = servicio.ListaDetalleCbEnsayosFiltro(empresas, centrales, estados, fechaInicio, fechaFin, nroPaginas, Constantes.PageSize);

            foreach (var reg in model.ListaEnsayo)
            {
                int codiensayo = reg.Ensayocodi;
                int nroEnviados = servicio.ListFormatoXEnsayo(reg.Ensayocodi).Where(x => x.Ensformtestado == EstadoFormato.Enviado).Count();
                int nroCorregidos = servicio.ListFormatoXEnsayo(reg.Ensayocodi).Where(x => x.Ensformtestado == EstadoFormato.Corregido).Count();
                int nroObservados = servicio.ListFormatoXEnsayo(reg.Ensayocodi).Where(x => x.Ensformtestado == EstadoFormato.Observado).Count();
                int nroAprobados = servicio.ListFormatoXEnsayo(reg.Ensayocodi).Where(x => x.Ensformtestado == EstadoFormato.Aprobado).Count();
                reg.NroAprobados = nroAprobados;
                reg.NroRevisar = nroEnviados + nroCorregidos;
                reg.NroObservar = nroObservados;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Maneja el paginado para la lista de ensayos
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="estados"></param>
        /// <param name="centrales"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string empresas, string estados, string centrales, string finicios,
            string ffins)
        {
            BusquedaEnsayoModel model = new BusquedaEnsayoModel();
            model.IndicadorPagina = false;
            DateTime fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = fechaFin.AddDays(1);
            estados = "-1";  //para que muestre el total de paginas
            if (empresas == "-1")
                empresas = this.Empresas;
            int nroRegistros = servicio.GetTotalEnsayo(empresas, centrales, estados, fechaInicio, fechaFin);
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
        /// Popup Modos de operación según Ensayo
        /// </summary>
        /// <param name="idEnsayo"></param>
        /// <returns></returns>
        public PartialViewResult ViewPopUpModosOperacion(int idEnsayo)
        {
            EnsayoModel model = new EnsayoModel();
            model.ListaModosOperacion = this.servicio.ListarMOXensayo(idEnsayo);

            return PartialView(model);
        }

        #endregion

        #region Exportación Excel

        /// <summary>
        /// Genera el reporte excel 
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="estados"></param>
        /// <param name="centrales"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteXls(string empresas, string estados, string centrales, string finicios,
            string ffins)
        {
            EnsayoModel model = new EnsayoModel();
            int indicador = 1;
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddDays(1);
                if (empresas == "-1")
                    empresas = this.Empresas;
                model.ListaEnsayo = servicio.ListaDetalleCbEnsayosFiltroXls(empresas, centrales, estados, fechaInicio, fechaFin);
                var list = model.ListaEnsayo;
                ExcelDocument.GernerarArchivoEnsayos(list.ToList());
                indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                indicador = -1;
                model.Mensaje = ex.Message;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Descarga el reporte excel del servidor
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = FormatoEnsayo.NombreReporte;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnsayo.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        #region Ensayo (Modos de Operación y Unidades)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult ViewPopUpNuevoEnsayo(int idEmpresa)
        {
            EnsayoModel model = new EnsayoModel();
            model.ListaEmpresas = ListarEmpresasEnsayoPotencia();

            //Metodo para listar CT al abrir el popup
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string idEmpresa2 = idEmpresa + "";
            if (idEmpresa2 != "-1")
            {
                entitys = servicio.ListEqEquipoEmpresaGEN2(idEmpresa2).ToList();
            }
            else
            {
                entitys = servicio.ListEqEquipoEmpresaGEN2("-1").ToList();
            }
            model.ListaEquipo = entitys;

            //Metodo para listar los modos de operacion al ingresar al popup
            if (model.ListaEquipo.Count > 0)
            {
                int idCentral = model.ListaEquipo[0].Equicodi;
                model.ListaModosOperacion = new List<PrGrupoDTO>();
                model.ListaModosOperacion = this.servicio.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa); //idEmpresa es entero, sino iria idEmpresa2
                model.ListaModosOperacion = model.ListaModosOperacion.OrderBy(x => x.Gruponomb).ToList();
            }

            return PartialView(model);
        }

        /// <summary>
        /// Genera la lista de centrales cada vez que cambiamos de item(empresas)
        /// </summary>
        /// <param name="idsAgente"></param>
        /// <returns></returns>
        /// 
        public JsonResult CargarCentral2(int idEmpresa)
        {
            //cada vez que cambia la empresa cambiará los items de centrales termicas
            BusquedaEnsayoModel model = new BusquedaEnsayoModel();
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string idEmpresa2 = idEmpresa + "";
            if (idEmpresa2 != "-1")
            {
                entitys = servicio.ListEqEquipoEmpresaGEN2(idEmpresa2).ToList();
            }
            else
            {
                entitys = servicio.ListEqEquipoEmpresaGEN2("-1").ToList();
            }
            model.ListaEquipo = entitys;
            return Json(model);
        }

        /// <summary>
        /// Carga el listado de modos de operacion o Grupos de operacion según el tipo de central seleccionada
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaGrupoModo(int idCentral, int idEmpresa)
        {
            EnsayoModel model = new EnsayoModel();
            model.ListaModosOperacion = this.servicio.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa);
            model.ListaModosOperacion = model.ListaModosOperacion.OrderBy(x => x.Gruponomb).ToList();
            model.ListaEspecial = model.ListaModosOperacion.Where(x => x.FlagModoEspecial == ConstantesHorasOperacion.FlagModoEspecial).ToList();

            return Json(model);
        }

        /// <summary>
        /// Permite abrir popup para agregar una unidad al ensayo
        /// </summary>
        /// <param name="iequicodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AgregarUnidadesEnsayo(string iequicodi)
        {
            EnsayoModel model = new EnsayoModel();
            model.ListaEquipo = servicio.ListEqEquipoEnsayo(iequicodi);
            return PartialView(model);
        }

        /// <summary>
        /// Carga el listado de unidades generadoras
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarUnidadesGeneradoras(int padre, int idEmpresa, int idCentral)
        {
            EnsayoModel model = new EnsayoModel();
            model.ListaEquipo = this.servHO.ListarGruposxCentralGEN(idEmpresa, idCentral, ConstantesHorasOperacion.IdGeneradorTemoelectrico); //trae todas las unidades ESPECIALES Y NO ESPEACIALES
            return Json(model);
        }

        /// <summary>
        /// Carga el listado de unidades generadoras especiales
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarUnidadesGeneradorasEspeciales(int padre)
        {
            EnsayoModel model = new EnsayoModel();
            model.ListaEquipo = this.servHO.ListarGruposxCentralGENEspecial(padre, ConstantesHorasOperacion.IdGeneradorTemoelectrico);  //Trae todas las unidades ESPECIALES
            return Json(model);
        }

        /// <summary>
        /// Carga un array con las unidades generadoras especiales de cierto padre
        /// </summary>
        /// <param name="padre"></param>
        /// <returns></returns>
        public JsonResult CargarArrayUnidadesGeneradorasEspeciales(int padre)
        {

            EnsayoModel model = new EnsayoModel();
            model.listitaUnidades = new List<int>();
            model.ListaEquipo = this.servHO.ListarGruposxCentralGENEspecial(padre, ConstantesHorasOperacion.IdGeneradorTemoelectrico);  //Trae todas las unidades ESPECIALES
            foreach (var item in model.ListaEquipo)
            {
                model.listitaUnidades.Add(item.Equicodi);
            }
            return Json(model);
        }


        /// <summary>
        /// Permite guardar un ensayo con el nuevo formato (2019)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarEnsayoNuevo(int idEmpresa, int idCentral, string dataListaMoOpYUnid)
        {
            EnsayoModel model = new EnsayoModel();
            try
            {
                base.ValidarSesionJsonResult();

                List<EnEnsayomodoDTO> listaMO = new List<EnEnsayomodoDTO>();

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                List<ModoOperacionConSusUnidadesModel> objData = serialize.Deserialize<List<ModoOperacionConSusUnidadesModel>>(dataListaMoOpYUnid);

                foreach (var regenmod in objData)
                {
                    EnEnsayomodoDTO ensayoModo = new EnEnsayomodoDTO();
                    ensayoModo.Grupocodi = regenmod.CodigoMO;
                    ensayoModo.ListaUnidades = new List<EnEnsayomodequiDTO>();
                    if (regenmod.ListaUnidades.Count > 0)  // si es especial
                    {
                        var unidades = regenmod.ListaUnidades;
                        foreach (var unidad in unidades)
                        {
                            EnEnsayomodequiDTO ensayoModoEqui = new EnEnsayomodequiDTO();
                            ensayoModoEqui.Equicodi = unidad;
                            ensayoModo.ListaUnidades.Add(ensayoModoEqui);
                        }
                    }
                    else  // si no es especial
                    {
                        EnEnsayomodequiDTO ensayoModoEqui = new EnEnsayomodequiDTO();
                        ensayoModoEqui.Equicodi = -1;
                        ensayoModo.ListaUnidades.Add(ensayoModoEqui);
                    }

                    listaMO.Add(ensayoModo);
                }

                EnEnsayoDTO ensayo = new EnEnsayoDTO();
                model.Resultado = servicio.GuardarEnsayoNuevo(ref ensayo, idEmpresa, idCentral, listaMO, User.Identity.Name);

                if (ensayo.Usercodi.IndexOf(Constantes.CaracterArroba) != -1)
                {
                    //Envia correo de solicitud de ensayo
                    string subjectEmail = "Notificaión de solicitud de un Ensayo de Potencia";
                    string toEmail = ConfigurationManager.AppSettings[EnsayoEnvio.AdminEmail];
                    string ccEmail = User.Identity.Name;
                    string msg = string.Empty;
                    msg = EnsayoHelper.BodyMailNuevoEnsayo(User.Identity.Name);
                    try
                    {
                        COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, subjectEmail, msg);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(NameController, ex);
                        model.Resultado = "2";
                        model.Mensaje = ex.Message;

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        #endregion

        #region Ensayo (Formatos)

        /// <summary>
        /// Action para visualizar todos los formato para su envio. 
        /// </summary>
        /// <returns></returns>
        public ActionResult EnvioFormato()
        {
            base.ValidarSesionUsuario();
            string codigo = "0";
            if (Request["id"] != null)
                codigo = Request["id"];

            int idEnsayo = int.Parse(codigo);
            EnEnsayoDTO ensayo = servicio.GetByIdEnEnsayo(idEnsayo);
            List<EnFormatoDTO> listaFormato = this.servicio.ListaFormatosConSusHijos();
            List<EqEquipoDTO> listaEquipo = this.servicio.ListarUnidadesxEnsayo(idEnsayo);

            EnsayoModel model = new EnsayoModel();
            model.Emprnomb = ensayo.Emprnomb;
            model.Equinomb = ensayo.Equinomb;
            model.Ensayocodi = idEnsayo;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            StringBuilder json = new StringBuilder();
            serializer.Serialize(listaFormato, json);
            model.ListaFormatoJson = json.ToString();

            json = new StringBuilder();
            serializer.Serialize(listaEquipo, json);
            model.ListaEquipoJson = json.ToString();

            return View(model);
        }

        /// <summary>
        /// Recibe los formatos de los ensayos
        /// </summary>
        /// <param name="iEnsayoCodi"></param>
        /// <param name="nroFormato"></param>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(int ensayocodi, int formatocodi, int equicodi)
        {
            int idensayo = ensayocodi;
            EnEnsayounidadDTO ensayoUnidad = new EnEnsayounidadDTO();
            ensayoUnidad = servicio.ObtenerEnsayoUnidad(ensayocodi, equicodi);
            int enunidad = ensayoUnidad.Enunidadcodi;

            string sNombreArchivo = "";
            string sNombreArchivoEnsayo = "";
            string path = "";
            string pathTemporal = "";
            string user = "";

            List<EnEnsayoformatoDTO> lstArchEnvio = new List<EnEnsayoformatoDTO>();
            EnEnsayoformatoDTO ArchEnvio = new EnEnsayoformatoDTO();
            int iEstadoFormatoHistorial = EstadoFormato.Enviado;

            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                if (Request.Files.Count == 1)
                {

                    var file = Request.Files[0];
                    string extension = file.FileName.Split('.').Last().ToUpper();
                    sNombreArchivo = servicio.GetNombreArchivoFormato(idensayo, formatocodi, extension);
                    sNombreArchivoEnsayo = file.FileName;
                    user = User.Identity.Name;

                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                    string fileName = ruta + file.FileName;
                    string ruta2 = string.Format("{0}\\{1}\\", base.PathFiles, ConstantesEnsayo.FolderFormato);
                    path = FileServer.GetDirectory() + ruta2;

                    this.servicio.ActualizarFormato(ArchEnvio, formatocodi, enunidad, sNombreArchivo, sNombreArchivoEnsayo, user, path, iEstadoFormatoHistorial, idensayo);  //verifica en raiz                    
                    file.SaveAs(fileName); //Guarda en temporal con nombfisico

                    pathTemporal = Server.MapPath("~/Uploads/") + file.FileName;
                    FileServer.CreateFolder(base.PathFiles, ConstantesEnsayo.FolderFormato, "");
                    FileServer.UploadFromFileDirectory(pathTemporal, ruta2, sNombreArchivo, string.Empty); //graba en raiz

                    //Elimina el archivo temporal
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  Descarga el formato enviado del servidor 
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoEnvio(string archivo)
        {
            string ruta2 = string.Format("{0}\\{1}\\", base.PathFiles, ConstantesEnsayo.FolderFormato);
            string path = ruta2 + "//" + archivo;
            byte[] buffer = FileServer.DownloadToArrayByte(path, "");
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, archivo);

        }

        /// <summary>
        /// Permite mostrar la lista de formato del ensayo
        /// </summary>
        /// <param name="Ensayocodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="formatocodi"></param>
        /// <returns></returns>
        public PartialViewResult ListaFormato(int ensayocodi, int equicodi, int formatocodi)
        {
            EnsayoModel model = new EnsayoModel();
            model.LstArchEnvioEnsayo = servicio.ListFormatoXEnsayo(ensayocodi, equicodi).OrderBy(x => x.Formatodesc).ToList(); ;
            model.Equicodi = equicodi;
            model.Formatocodi = formatocodi;
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el historial de estado de un ensayo
        /// </summary>
        /// <param name="enunidadcodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="iformatocodi"></param>
        /// <returns></returns>
        public PartialViewResult HistorialFormato(int enunidadcodi, int equicodi, int formatocodi)
        {
            EnsayoModel model = new EnsayoModel();
            model.Equicodi = equicodi;
            model.Equinomb = servicio.GetByIdEquipo(equicodi).Equinomb;
            List<EnEstformatoDTO> formatos = servicio.ListEnEstformatosEnsayo(enunidadcodi, formatocodi);
            model.LstEstFormatosEnsayo = formatos;
            return PartialView(model);
        }

        #endregion

        #region Útil

        /// <summary>
        /// Listar empresas de ensayo de potencia
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasEnsayoPotencia()
        {
            List<SiEmpresaDTO> ListaEmpresas = new List<SiEmpresaDTO>();
            string strEmpresas = string.Empty;
            var empresas = this.servicio.ListarEmpresasxTipoEquipos(ConstantesEnsayo.CodFamilias);   //termoelectricas

            int idAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);
            int idOpcion = (Session[DatosSesion.SesionIdOpcion] == null) ? 0 : (int)Session[DatosSesion.SesionIdOpcion];
            bool OpAccesoEmpresa = this.seguridad.ValidarPermisoOpcion(idAplicacion, idOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
            if (OpAccesoEmpresa)
            {
                ListaEmpresas = empresas;
            }
            else
            {
                var empresasUsuario = servicio.ObtenerEmpresasXUsuario(User.Identity.Name);
                foreach (var p in empresasUsuario)
                {
                    var find = empresas.Find(x => x.Emprcodi == p.Emprcodi);
                    if (find != null)
                    {
                        ListaEmpresas.Add(p);
                    }
                }
            }
            return ListaEmpresas;
        }

        #endregion

    }
}
