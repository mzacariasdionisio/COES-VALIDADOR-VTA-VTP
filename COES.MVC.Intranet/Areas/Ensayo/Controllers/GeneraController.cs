
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.Ensayo.Helper;
using COES.MVC.Intranet.Areas.Ensayo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ensayo;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Ensayo.Controllers
{
    [ValidarSesion]
    public class GeneraController : BaseController
    {
        //
        // GET: /Ensayo/Genera/
        EnsayoAppServicio servicio = new EnsayoAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();
        //private MailClient appMail = new MailClient(); 

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

        int IdAplicacion = Convert.ToInt32(Constantes.IdAplicacion);
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
            int idAplicacion = Constantes.IdAplicacion;

            BusquedaEnsayoModel model = new BusquedaEnsayoModel();
            int idOpcion = (Session[DatosSesion.SesionIdOpcion] == null) ? 0 : (int)Session[DatosSesion.SesionIdOpcion];
            model.OpNuevo = this.seguridad.ValidarPermisoOpcion(idAplicacion, idOpcion, Acciones.Nuevo, User.Identity.Name);
            model.OpAccesoEmpresa = this.seguridad.ValidarPermisoOpcion(idAplicacion, idOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
            model.OpEditar = this.seguridad.ValidarPermisoOpcion(idAplicacion, idOpcion, Acciones.Editar, User.Identity.Name);

            model.FechaInicio = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            model.ListaEstado = servicio.ListEnEstadoss().Where(x => x.Estadocodi <= EstadoEnsayo.Aprobado).ToList();
            model.ListaEmpresas = ListarEmpresasEnsayoPotencia();
            string strEmpresas = ConstantesAppServicio.ParametroDefecto;
            if (strEmpresas != ConstantesAppServicio.ParametroDefecto && !string.IsNullOrEmpty(strEmpresas))
            {
                strEmpresas = string.Join(",", model.ListaEmpresas.Select(x => x.Emprcodi).ToList());
            }
            this.Empresas = strEmpresas;
            model.ListaEquipo = servicio.ListEqEquipoEmpresaGEN2("-1").ToList();

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
            EnsayoModel model = new EnsayoModel();
            //model.IdEstado = int.Parse(estados);

            model.IdEstado = estado;
            string estados = "-1";
            //string estados = estado+"";
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
            estados = "-1";  //para que muestre el total de paginas
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
            int indicador = 1;
            EnsayoModel model = new EnsayoModel();
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
                if (empresas == "-1")
                    empresas = this.Empresas;
                model.ListaEnsayo = servicio.ListaDetalleCbEnsayosFiltroXls(empresas, centrales, estados, fechaInicio, fechaFin);
                var list = model.ListaEnsayo;
                ExcelDocument.GernerarArchivoEnsayos(list.ToList());
                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
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
        /// Permite crear un ensayo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEnsayo(EnsayoModel model)
        {
            string strVectUnid = model.StrVectorUnidad;
            List<EnEnsayoformatoDTO> lstformatos = new List<EnEnsayoformatoDTO>();
            List<string> codiUnidad = new List<string>();
            codiUnidad = strVectUnid.Split(',').ToList();

            int codensayo = model.Ensayocodi;
            int equicodi = model.Equicodi;
            int emprcodi = model.Emprcodi;
            string nomUser = User.Identity.Name;
            string mo = "";

            //lista de formatos si existen previos para una misma empresa y central
            lstformatos = servicio.ListEnEnsayoFormatosEmpresaCentral(emprcodi, equicodi);
            var apto = servicio.GrabarEnsayosYFormatos(codensayo, codiUnidad, emprcodi, nomUser, mo, lstformatos);


            //si se guardo el correctamente el ensayo y los formatos, enviamos email
            if (apto.Equals("aptoParaEnviarEmail"))
            {

                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();
                List<string> mailsBcc = new List<string>();

                string fromEmail = EnsayoEnvio.FromEmail;
                string subjectEmail = EnsayoEnvio.SubjetcEmail;
                string toEmail = ConfigurationManager.AppSettings[EnsayoEnvio.AdminEmail];
                string ccEmail = User.Identity.Name;
                toEmails.Add(toEmail);
                ccEmails.Add(ccEmail);
                string msg = string.Empty;
                msg = EnsayoHelper.BodyMailNuevoEnsayo(User.Identity.Name);
                try
                {
                    COES.Base.Tools.Util.SendEmail(toEmails, ccEmails, mailsBcc, subjectEmail, msg, fromEmail);
                }
                catch (Exception e)
                {

                }
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
            string codigo = "0";
            if (Request["id"] != null)
                codigo = Request["id"];
            int idEnsayo = int.Parse(codigo);
            EnsayoModel model = new EnsayoModel();
            var ensayo = servicio.GetByIdEnEnsayo(idEnsayo);
            model.Emprnomb = ensayo.Emprnomb;
            model.Equinomb = ensayo.Equinomb;
            model.Ensayocodi = idEnsayo;
            return View(model);
        }

        /// <summary>
        /// Sirve para  observar o aprobar formato. 
        /// </summary>
        /// <returns></returns>
        public ActionResult EditarFormato()
        {
            base.ValidarSesionUsuario();
            string codigo = "0"; // id : ensayocodi
            if (Request["id"] != null)
                codigo = Request["id"];
            int id = int.Parse(codigo);
            EnsayoModel model = new EnsayoModel();
            model.LstArchEnvioEnsayo = servicio.ListFormatoXEnsayoConColorFila(id);

            var ensayo = servicio.GetByIdEnEnsayo(id);
            model.Ensayocodi = id;
            model.Emprnomb = ensayo.Emprnomb;
            model.Equinomb = ensayo.Equinomb;
            model.IdEstado = (int)ensayo.Estadocodi;
            if (ensayo.Ensayofechaevento < DateTime.Now)
                model.IdEstado = EstadoEnsayo.Archivado;
            model.RowChange = -1;
            return View(model);
        }

        /// <summary>
        /// Recibe los formatos de los ensayos
        /// </summary>
        /// <param name="nroFormato"></param>
        /// <param name="iEnsayoCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(int nroFormato, int iEnsayoCodi)
        //public ActionResult Upload(int nroFormato, int iEnunidadCodi)
        {
            int idensayo = iEnsayoCodi;
            int nf = nroFormato;
            //int idenunidad = iEnunidadCodi;
            string sNombreArchivo = "";
            string sNombreArchivoEnsayo = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnsayo.FolderRepositorio;
            List<EnEnsayoformatoDTO> lstArchEnvio = new List<EnEnsayoformatoDTO>();
            EnEnsayoformatoDTO ArchEnvio = new EnEnsayoformatoDTO();
            string fecha = DateTime.Now.ToString("yyyyMMddhhmm");
            int iEstadoFormatoHistorial = EstadoFormato.Enviado; ;
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string extension = file.FileName.Split('.').Last().ToUpper();
                    sNombreArchivo = servicio.ObtenerNombreDelArchivo(nf, idensayo, fecha, extension);
                    ArchEnvio.Formatocodi = nroFormato;
                    sNombreArchivoEnsayo = file.FileName;
                    ArchEnvio.Ensayocodi = idensayo;
                    ArchEnvio.Ensformatnomblogico = sNombreArchivo;
                    ArchEnvio.Ensformatnombfisico = sNombreArchivoEnsayo;
                    ArchEnvio.Ensformtestado = EstadoFormato.Enviado;
                    ArchEnvio.Lastdate = DateTime.Now;
                    ArchEnvio.Lastuser = User.Identity.Name;

                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);

                    // verificamos si existe un archivo enviado para el ensayo
                    var ensayoFormato = servicio.GetByIdEnEnsayoformato(iEnsayoCodi, nroFormato);
                    if (ensayoFormato == null)
                        servicio.SaveEnEnsayoformato(ArchEnvio);
                    else
                    {
                        ArchEnvio.Ensformtestado = EstadoFormato.Corregido;
                        ArchEnvio.Lastdate = DateTime.Now;
                        ArchEnvio.Lastuser = User.Identity.Name;
                        servicio.UpdateEnEnsayoformato(ArchEnvio);
                        iEstadoFormatoHistorial = EstadoFormato.Corregido;

                        //actualizamos el estado del ensayo a "Autorizado" si esta en estado "Aprobado"
                        var ensayo = servicio.GetByIdEnEnsayo(idensayo);
                        if (ensayo.Estadocodi == EstadoEnsayo.Aprobado)
                        {
                            DateTime lastDate = DateTime.Now;
                            string lastUser = User.Identity.Name;
                            servicio.ActualizaEstadoEnsayo(idensayo, ensayo.Ensayofechaevento, EstadoEnsayo.Autorizado, lastDate, lastUser);
                        }

                    }
                    //guardamos en el historial de formatos estado table EN_ESTFORMATO
                    EnEstformatoDTO entity = new EnEstformatoDTO();

                    //entity.Ensayocodi = (int)iEnsayoCodi;
                    entity.Formatocodi = (int)nroFormato;
                    entity.Estadocodi = iEstadoFormatoHistorial;
                    entity.Estfmtlastdate = DateTime.Now;
                    entity.Estfmtlastuser = User.Identity.Name;
                    servicio.SaveEnEstformato(entity);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite mostrar la lista de formato del ensayo
        /// </summary>
        /// <param name="Ensayocodi"></param>
        /// <param name="iRowChange"></param>
        /// <returns></returns>
        public PartialViewResult ListaFormato(int Ensayocodi, int iRowChange)
        {
            EnsayoModel model = new EnsayoModel();
            model.LstArchEnvioEnsayo = servicio.ListFormatoXEnsayo(Ensayocodi);
            model.RowChange = iRowChange;
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el historial de estado de un ensayo
        /// </summary>
        /// <param name="ienunidadcodi"></param>
        /// <param name="iformatocodi"></param> 
        /// <returns></returns>
        public PartialViewResult HistorialFormato(int ienunidadcodi, int iformatocodi)
        {
            EnsayoModel model = new EnsayoModel();
            model.Enunidadcodi = ienunidadcodi;
            List<EnEstformatoDTO> formatos = new List<EnEstformatoDTO>();
            formatos = servicio.ListEnEstformatosEnsayo(ienunidadcodi, iformatocodi);
            model.LstEstFormatosEnsayo = formatos;
            return PartialView(model);
        }

        /// <summary>
        ///  Descarga del servidor el formato enviado
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoEnvio(string archivo)
        {
            //string ruta2 = string.Format("{0}\\{1}\\", base.PathFiles, ConstantesEnsayo.FolderFormato);
            string ruta2 = string.Format("{0}\\{1}\\", "Ensayos", ConstantesEnsayo.FolderFormato);
            string path = ruta2 + "//" + archivo;
            byte[] buffer = FileServer.DownloadToArrayByte(path, "");
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, archivo);
        }

        /// <summary>
        /// Descargar archivo del Anexo A
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteXls(string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnsayo.FolderReporte;
            string fullPath = ruta + nameFile;
            return File(fullPath, Constantes.AppExcel, nameFile);

        }


        #endregion

        #region Autorización


        /// <summary>
        /// graba en BD autorizacion de un ensayo.
        /// </summary>
        /// <param name="iensayocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarAutorizaEnsayo(int iensayocodi)
        {
            EnsayoModel model = new EnsayoModel();
            model.Ensayoaprob = 0;
            model.Ensayocodi = iensayocodi;
            model.Estadocodi = EstadoEnsayo.Autorizado;
            int icodiensayo = model.Ensayocodi;
            int iCodEstado = model.Estadocodi;
            DateTime lastDate = DateTime.Now;
            DateTime fechaEnsayo = DateTime.Now;
            string lastUser = User.Identity.Name;
            servicio.ActualizaEstadoEnsayo(icodiensayo, fechaEnsayo, iCodEstado, lastDate, lastUser);
            var ensayo = servicio.GetByIdEnEnsayo(iensayocodi);

            int nroAprobados = 0;
            int numeroFormatos = servicio.ObtenerNumeroFormatosActivos().Count();
            int numeroUnidadesDelEnsayo = servicio.ObtenerUnidadesXEnsayo(iensayocodi).Count();
            int totalFormatosXAprobar = numeroFormatos * numeroUnidadesDelEnsayo;
            // verifica si todos los formatos estan aprobados para aprobar el ensayo
            nroAprobados = nroAprobados + servicio.ListFormatoXEnsayo(iensayocodi).Where(x => x.Ensformtestado == EstadoFormato.Aprobado).Count();
            if (nroAprobados == totalFormatosXAprobar)
            {
                iCodEstado = EstadoEnsayo.Aprobado;
                servicio.ActualizaEstadoEnsayo(iensayocodi, fechaEnsayo, iCodEstado, lastDate, lastUser);
                model.Ensayoaprob = 1;
            }

            //Envia correo de solicitud de ensayo
            if (ensayo.Usercodi.IndexOf(Constantes.CaracterArroba) != -1)
            {

                string subjectEmail = "Estado de su solicitud de un Ensayo de Potencia";
                string toEmail = ensayo.Usercodi;
                string ccEmail = ConfigurationManager.AppSettings[EnsayoEnvio.AdminEmail];
                string msg = string.Empty;
                msg = EnsayoHelper.BodyMailAutorizaEnsayo(User.Identity.Name, fechaEnsayo.ToString("dd/MM/yyyy"));
                try
                {
                    COES.Base.Tools.Util.SendEmail(toEmail, ccEmail, subjectEmail, msg);
                }
                catch (Exception e)
                {

                }

            }
            return Json(model);
        }

        /// <summary>
        /// Muestra popup para autorizar ensayo
        /// </summary>
        /// <param name="iensayocodi"></param>
        /// <returns></returns>
        public PartialViewResult AutorizaEnsayo(int iensayocodi)
        {
            EnsayoModel model = new EnsayoModel();
            model.ListaModosOperacion = servicio.ListarMOXensayo(iensayocodi);  // Lista de modos de operacion
            model.ListaEquipo = servicio.ListarUnidadesxEnsayo(iensayocodi);  // Lista de Unidades generadoras            
            var ensayo = servicio.GetByIdEnEnsayo(iensayocodi);
            model.Emprnomb = ensayo.Emprnomb;
            model.Equinomb = ensayo.Equinomb;
            model.Ensayocodi = iensayocodi;
            return PartialView(model);
        }

        #endregion

        #region Observación

        /// <summary>
        /// Graba en BD la observacion del ensayo
        /// </summary>
        /// <param name="iEnunidcodi"></param>
        /// <param name="iFormatoCodi"></param>
        /// <param name="sObservacion"></param>
        /// <returns></returns>
        public JsonResult GuardarObservacionFormato(int iEnunidcodi, int iFormatoCodi, string sObservacion)
        {
            EnsayoModel model = new EnsayoModel();
            EnEstformatoDTO entity = new EnEstformatoDTO();
            int codiEstFormato;

            entity.Enunidadcodi = iEnunidcodi;
            entity.Formatocodi = iFormatoCodi;
            entity.Estadocodi = EstadoFormato.Observado;
            entity.Estfmtlastdate = DateTime.Now;
            entity.Estfmtlastuser = User.Identity.Name;
            entity.Estfmtdescrip = sObservacion;
            codiEstFormato = servicio.SaveEnEstformato(entity);
            entity.Estfmtcodi = codiEstFormato;
            //atualiza el estado del formato
            servicio.UpdateEnEnsayoformatoEstado(EstadoFormato.Observado, iEnunidcodi, iFormatoCodi);
            //actualiza el estado del ensayo si esta en estado aprobado
            var enunidadActual = servicio.GetByIdEnEnsayounidad(iEnunidcodi);
            var ensayoActual = enunidadActual.Ensayocodi;
            var ensayo = servicio.GetByIdEnEnsayo(ensayoActual);
            if (ensayo.Estadocodi == EstadoEnsayo.Aprobado)
            {
                DateTime fechaEvento = ensayo.Ensayofechaevento;
                DateTime lastDate = DateTime.Now;
                string lastUser = User.Identity.Name;
                servicio.ActualizaEstadoEnsayo(ensayoActual, fechaEvento, EstadoEnsayo.Autorizado, lastDate, lastUser);
            }
            return Json(model);
        }

        /// <summary>
        /// Guarda en BD la aprobacion del ensayo
        /// </summary>
        /// <param name="iEnunidadCodi"></param>
        /// <param name="iFormatoCodi"></param>
        /// <returns></returns>
        public JsonResult GuardarAprobacionFormato(int iEnunidadCodi, int iFormatoCodi)
        {
            EnsayoModel model = new EnsayoModel();
            int codiEstFmt;
            model.Ensayoaprob = 0;
            EnEstformatoDTO entity = new EnEstformatoDTO();
            entity.Enunidadcodi = iEnunidadCodi;
            entity.Formatocodi = iFormatoCodi;
            entity.Estadocodi = EstadoFormato.Aprobado;
            entity.Estfmtlastdate = DateTime.Now;
            entity.Estfmtlastuser = User.Identity.Name;
            codiEstFmt = servicio.SaveEnEstformato(entity);
            entity.Estfmtcodi = codiEstFmt;

            servicio.UpdateEnEnsayoformatoEstado(EstadoFormato.Aprobado, iEnunidadCodi, iFormatoCodi);
            var enunidadActual = servicio.GetByIdEnEnsayounidad(iEnunidadCodi);
            var ensayoActual = enunidadActual.Ensayocodi;
            int numeroFormatos = servicio.ObtenerNumeroFormatosActivos().Count();
            int numeroUnidadesDelEnsayo = servicio.ObtenerUnidadesXEnsayo(ensayoActual).Count();
            int totalFormatosXAprobar = numeroFormatos * numeroUnidadesDelEnsayo;

            int aprobados = 1;
            var listaEnsayoFormato = servicio.ListFormatoXEnsayo(ensayoActual);
            foreach (var objEnsFor in listaEnsayoFormato)
            {
                if (objEnsFor.Formatocodi == numeroFormatos)
                    aprobados = 0;
            }
            aprobados = aprobados + servicio.ListFormatoXEnsayo(ensayoActual).Where(x => x.Ensformtestado == EstadoFormato.Aprobado).Count();
            var objEnsayo = servicio.GetByIdEnEnsayo(ensayoActual);

            //if (aprobados == FormatoEnsayo.NroFormatos)
            if (aprobados == totalFormatosXAprobar)
            {
                //actualiza estado ensayo si esta en estado autorizado
                if (objEnsayo.Estadocodi == EstadoEnsayo.Autorizado)
                {
                    int iCodEstado = EstadoEnsayo.Aprobado;
                    DateTime dfechaEvento = objEnsayo.Ensayofechaevento;
                    DateTime lastDate = DateTime.Now;
                    string lastUser = User.Identity.Name;
                    servicio.ActualizaEstadoEnsayo(ensayoActual, dfechaEvento, iCodEstado, lastDate, lastUser);
                    model.Ensayoaprob = 1; // ensayo aprobado
                }
            }
            return Json(model);
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
            var empresas = this.servicio.ListarEmpresasxTipoEquipos(ConstantesEnsayo.CodFamilias);

            int idAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);
            int idOpcion = (Session[DatosSesion.SesionIdOpcion] == null) ? 0 : (int)Session[DatosSesion.SesionIdOpcion];
            bool OpAccesoEmpresa = true; //TODO comentado por pruebas this.seguridad.ValidarPermisoOpcion(idAplicacion, idOpcion, Acciones.AccesoEmpresa, User.Identity.Name);

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
