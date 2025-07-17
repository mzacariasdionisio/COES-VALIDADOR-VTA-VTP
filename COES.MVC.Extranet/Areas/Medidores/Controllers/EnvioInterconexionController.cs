using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Medidores.Models;
using COES.MVC.Extranet.Areas.Medidores.Utilities;
using COES.MVC.Extranet.Areas.Medidores.Helper;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Interconexiones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Extranet.ServiceReferenceMail;
using COES.MVC.Extranet.Controllers;

namespace COES.MVC.Extranet.Areas.Medidores.Controllers
{
    public class EnvioInterconexionController : BaseController
    {
        //
        // GET: /Medidores/EnvioInterconexion/
        InterconexionesAppServicio logic = new InterconexionesAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        private MailClient appMail;
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionInterconexiones.SesionNombreArchivo] != null) ?
                    Session[DatosSesionInterconexiones.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionInterconexiones.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[DatosSesionInterconexiones.SesionFileName] != null) ?
                    Session[DatosSesionInterconexiones.SesionFileName].ToString() : null;
            }
            set { Session[DatosSesionInterconexiones.SesionFileName] = value; }
        }

        public int IdEnvio
        {
            get
            {
                return (Session[DatosSesionInterconexiones.SesionIdEnvio] != null) ?
                    (int)Session[DatosSesionInterconexiones.SesionIdEnvio] : 0;
            }
            set { Session[DatosSesionInterconexiones.SesionIdEnvio] = value; }
        }

        public MeEnvioDTO Envio
        {
            get
            {
                return (Session["envio"] != null) ? (MeEnvioDTO)Session["envio"] : new MeEnvioDTO();
            }
            set
            {
                Session["envio"] = value;
            }
        }

        public int Empresa
        {
            get
            {
                return (Session[DatosSesionInterconexiones.SesionEmpresa] != null) ?
                    (int)Session[DatosSesionInterconexiones.SesionEmpresa] : 0;
            }
            set { Session[DatosSesionInterconexiones.SesionEmpresa] = value; }
        }

        public DateTime FechaProceso
        {
            get
            {
                return (Session[DatosSesionInterconexiones.SesionFechaProceso] != null) ?
                    (DateTime)Session[DatosSesionInterconexiones.SesionFechaProceso] : DateTime.Now;
            }
            set { Session[DatosSesionInterconexiones.SesionFechaProceso] = value; }
        }

        public Boolean EnPlazo
        {
            get
            {
                return (Session[DatosSesionInterconexiones.SesionEnPlazo] != null) ?
                    (Boolean)Session[DatosSesionInterconexiones.SesionEnPlazo] : false;
            }
            set { Session[DatosSesionInterconexiones.SesionEnPlazo] = value; }
        }

        public List<string> Medidores
        {
            get
            {
                return (Session[DatosSesionInterconexiones.SesionMedidores] != null) ?
                   (List<string>)Session[DatosSesionInterconexiones.SesionMedidores] : new List<string>();
            }
            set
            {
                Session[DatosSesionInterconexiones.SesionMedidores] = value;
            }

        }
        /// <summary>
        /// Estrcutura con los datos a grabar
        /// </summary>
        public List<MeMedicion96DTO> ListaProceso
        {
            get
            {
                return (Session[DatosSesion.SesionListaProcesar] != null) ?
                    (List<MeMedicion96DTO>)Session[DatosSesion.SesionListaProcesar] : new List<MeMedicion96DTO>();
            }
            set { Session[DatosSesion.SesionListaProcesar] = value; }
        }

        public List<MePeriodomedidorDTO> ListaPeriodo
        {
            get
            {
                return (Session[DatosSesionInterconexiones.SesionListaPeriodo] != null) ?
                    (List<MePeriodomedidorDTO>)Session[DatosSesionInterconexiones.SesionListaPeriodo] : new List<MePeriodomedidorDTO>();
            }
            set { Session[DatosSesionInterconexiones.SesionListaPeriodo] = value; }
        }

        public List<CeldaInfo> ListaErrores
        {
            get
            {
                return (Session[DatosSesion.SesionListaErrores] != null) ?
                    (List<CeldaInfo>)Session[DatosSesion.SesionListaErrores] : new List<CeldaInfo>();
            }
            set { Session[DatosSesion.SesionListaErrores] = value; }
        }

        //[COES.MVC.Extranet.Helper.CustomAuthorize]
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            InterconexionModel model = new InterconexionModel();
            model.ListaEmpresas = InterconexionHelper.ListarEmpresaInterconexion();
            model.Empresa = ConstantesMedidores.IdEmpresa;
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            string validacion = string.Empty;

            return View(model);
        }

        /// <summary>
        /// Recibe el archivo enviado por el agente
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            //ExtArchivoDTO archivoEnvio = new ExtArchivoDTO();
            MeArchivoDTO archivo = new MeArchivoDTO();
            MeEnvioDTO envio = new MeEnvioDTO();
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    this.FileName = NombreArchivoInterconexiones.NombreFileUploadInterconexion + IdEnvio.ToString() + "." + NombreArchivoInterconexiones.ExtensionFileUploadInterconexion;
                    archivo.Archsize = file.ContentLength / 1024;
                    archivo.Formatcodi = ConstantesMedidores.IdFormato;
                    archivo.Archnomborig = file.FileName;
                    archivo.Lastdate = DateTime.Now;
                    archivo.Lastuser = User.Identity.Name;
                    int idArchivo = logic.SaveMeArchivo(archivo);
                    archivo.Archcodi = idArchivo;
                    envio.Archcodi = idArchivo;
                    envio.Emprcodi = ConstantesMedidores.IdEmpresa;
                    envio.Enviofecha = DateTime.Now;
                    envio.Formatcodi = ConstantesMedidores.IdFormato;
                    envio.Enviofechaperiodo = this.FechaProceso;
                    envio.Envioplazo = (this.EnPlazo) ? "P" : "F";
                    envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                    envio.Lastdate = DateTime.Now;
                    envio.Lastuser = User.Identity.Name;
                    envio.Userlogin = User.Identity.Name;
                    this.IdEnvio = logic.SaveMeEnvio(envio);

                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMedidores.FolderUpload;
                    this.FileName = NombreArchivoInterconexiones.NombreFileUploadInterconexion + "-" + this.IdEnvio.ToString() + "." + NombreArchivoInterconexiones.ExtensionFileUploadInterconexion;
                    string fileName = ruta + this.FileName;

                    archivo.Archnombpatron = this.FileName;
                    this.NombreFile = fileName;
                    logic.UpdateMeArchivo(archivo);
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Recupera variable Session Iniciales
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarVariablesSesion(int idEmpresa, string fecha)
        {
            int resultado = 1;
            DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            this.FechaProceso = fechaProceso;
            this.Empresa = idEmpresa;
            this.EnPlazo = ValidarPlazo2(fechaProceso);
            return Json(resultado);
        }

        /// <summary>
        /// Action para procesamiento del archivo      
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarArchivo(int idEmpresa, string desEmpresa, string fecha, string periodos)
        {
            DatosExcel ps;
            short tipoDeError = 0;
            string mensaje = string.Empty;
            this.ListaProceso = null;
            this.Medidores = periodos.Split(',').ToList();
            MeEnvioDTO envio = logic.GetByIdMeEnvio(this.IdEnvio);
            List<TipoError> listaError = new List<TipoError>();
            try
            {
                if (Session[DatosSesion.SesionNombreArchivo] != null)
                {
                    string fileName = Session[DatosSesion.SesionNombreArchivo].ToString();

                    if (System.IO.File.Exists(fileName))
                    {
                        DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        var listaPuntos = logic.GetByCriteriaMeHojaptomeds(idEmpresa, ConstantesMedidores.IdFormato);
                        ps = new DatosExcel(fileName, (short)idEmpresa, 0, fechaProceso, listaPuntos);
                        tipoDeError = ps.ListaTipoObser.IndicarGrabarArchivo();
                        listaError = ps.ListaTipoObser.ListaTipoError;
                        if (ps.ListaMedidores != null)
                            this.ListaProceso = ps.ListaMedidores;
                        if (ps.ListaErrores != null)
                            this.ListaErrores = ps.ListaErrores.ToList();
                        switch (tipoDeError)
                        {
                            case 3:
                                envio.Lastdate = DateTime.Now;
                                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                                logic.UpdateMeEnvio(envio);
                                //this.ListaPeriodo = InterconexionHelper.GetListaPeriodoMedidor(medidores, this.IdEnvio, fechaProceso);
                                //logic.SaveListaMePeriodomedidor(this.ListaPeriodo);
                                mensaje = ValidacionArchivo.OK;
                                break;
                            case 2:
                            case 4:
                                envio.Lastdate = DateTime.Now;
                                envio.Estenvcodi = ParametrosEnvio.EnvioRechazado;
                                logic.UpdateMeEnvio(envio);
                                mensaje = "Error en Validación de Archivo";
                                break;
                            case 1:
                                envio.Lastdate = DateTime.Now;
                                envio.Estenvcodi = ParametrosEnvio.EnvioRechazado;
                                logic.UpdateMeEnvio(envio);
                                mensaje = "Error en Validación de Archivo";
                                break;
                        }
                        this.Envio = envio;
                    }
                    else
                    {
                        mensaje = ToolHtml.ObtenerValidacion(ValidacionArchivo.NoExiste);
                        tipoDeError = 1;
                    }
                }
                else
                {
                    mensaje = ToolHtml.ObtenerValidacion(ValidacionArchivo.NoExiste);
                    tipoDeError = 1;
                }

            }
            catch (Exception ex)
            {
                tipoDeError = 1;


            }
            var jsonData = new
            {
                ListaTipoError = (from q in listaError
                                  select new
                                  {
                                      q.TipoObservacion,
                                      q.TotalObservacion,
                                      q.Mensaje
                                  }).ToArray(),
                tipoError = tipoDeError
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Valida el plazo de envio del archivio
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarPlazo(string fecha)
        {
            InterconexionModel json = new InterconexionModel();
            try
            {
                string validacion = string.Empty;
                DateTime fechaProceso = (!string.IsNullOrEmpty(fecha)) ? DateTime.ParseExact(fecha,
                    Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

                json.EnPlazo = this.ValidarFecha(fechaProceso, out validacion);
                json.ValidacionPlazo = validacion;
            }
            catch
            {
                json.ValidacionPlazo = string.Empty;
            }

            return Json(json);
        }

        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        protected bool ValidarPlazo2(DateTime fecha)
        {
            bool resultado = false;
            int minutosDia = 540;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= fecha) && (fechaActual <= fecha.AddMinutes(minutosDia)))
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Permite validar una fecha para el ingreso
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="mensaje"></param>
        protected bool ValidarFecha(DateTime fecha, out string mensaje)
        {
            mensaje = string.Empty;
            int minutosDia = 540;
            DateTime fechaActual = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha),
                Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            var fechaPlazo = this.logic.GetByIdMeAmpliacionfecha(fecha, this.Empresa, 2);
            if (fechaPlazo != null)
            {
                if (fechaPlazo.Amplifechaplazo.ToString(Constantes.FormatoFecha) == DateTime.Now.ToString(Constantes.FormatoFecha))
                    minutosDia = fechaPlazo.Amplifechaplazo.Hour * 60 + fechaPlazo.Amplifechaplazo.Minute;
            }
            TimeSpan ts = fecha.Subtract(fechaActual.AddDays(-1));
            if (ts.Days == 0)
            {
                if (DateTime.Now.Hour * 60 + DateTime.Now.Minute > minutosDia)
                {
                    mensaje = ConstantesMedidores.ValidacionDiario;
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                if (fechaPlazo == null)
                {
                    mensaje = ConstantesMedidores.ValidacionDiaPasado;
                    return false;
                }
                else
                {
                    if (DateTime.Now.Hour * 60 + DateTime.Now.Minute > minutosDia)
                    {
                        mensaje = ConstantesMedidores.ValidacionDiario;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }


        }
        /// <summary>
        /// Action para grabar en db el archivo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(int idEmpresa, string fecha)
        {
            ResultadoOperacion resultado = new ResultadoOperacion();
            this.appMail = new MailClient();
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaInicio;
                this.Envio.Lastdate = DateTime.Now;
                this.Envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                logic.UpdateMeEnvio(Envio);

                if (this.ListaProceso.Count > 0)
                {
                    int estado = 1;
                    this.ListaPeriodo = InterconexionHelper.GetListaPeriodoMedidor(this.Medidores, this.IdEnvio, fechaInicio);
                    logic.SaveListaMePeriodomedidor(this.ListaPeriodo);
                    this.logic.EliminarValoresCargados(idEmpresa, fechaInicio, fechaFin);
                    this.logic.GrabarInterconexion(this.ListaProceso);
                }
                resultado.Mensaje = ConstantesMedidores.CargaInterconexionesCorrecto;

                string msg = InterconexionHelper.GenerarBodyMail(this.FechaProceso, (DateTime)this.Envio.Lastdate, this.IdEnvio,
                    (int)this.Envio.Estenvcodi, this.ListaPeriodo);
                string toEmail = ConfigurationManager.AppSettings[ParametrosEnvio.To_Mail];
                string ccEmail = string.Empty;
                if (User.Identity.Name.IndexOf("@") != -1)
                    ccEmail = User.Identity.Name;
                appMail.EnviaCorreoFull(ParametrosEnvio.From_Email, ParametrosEnvio.From_Name, toEmail,
                   ccEmail, "Notificacion de Envío de Medidores Interconexón", msg, true, "", null);
                resultado.Resultado = 1;
            }
            catch (Exception ex)
            {
                resultado.Resultado = -1;
                resultado.Mensaje = ex.Message;
            }
            return Json(resultado);
        }

        /// <summary>
        /// Recupera el detalle de los errores generados al cargar el archivo
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ListarDetalleErrorEnvio()
        {
            InterconexionModel model = new InterconexionModel();
            model.ListaErrores = this.ListaErrores;
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar los datos cargados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="horizonte"></param>
        /// <param name="fecha"></param>
        /// <param name="nroSemana"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarDatos(int idEmpresa, string fecha)
        {
            InterconexionModel model = new InterconexionModel();

            try
            {
                DateTime fechaInicial = DateTime.Now;
                DateTime fechaFinal = DateTime.Now;
                fechaInicial = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFinal = fechaInicial;
                string resultado = this.logic.ObtenerConsultaInterconexion(idEmpresa, fechaInicial, fechaFinal);

                model.Resultado = resultado;
            }
            catch (Exception ex)
            {
                model.Resultado = ex.Message;
            }


            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int idEmpresa, string desEmpresa, string fecha)
        {
            int indicador = 1;
            try
            {
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                List<MeHojaptomedDTO> list = this.logic.GetByCriteriaMeHojaptomeds(idEmpresa, ConstantesMedidores.IdFormato);
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMedidores.FolderReporte;
                InterconexionHelper.GenerarFormato(list, desEmpresa, fechaProceso, ruta);
                indicador = 1;

                return Json("1");
            }
            catch (Exception ex)
            {
                indicador = -1;
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMedidores.FolderReporte;
            string fullPath = ruta + NombreArchivoInterconexiones.FormatoInterconexion;
            return File(fullPath, Constantes.AppExcel, NombreArchivoInterconexiones.FormatoInterconexion);
        }
        /// <summary>
        /// Model para cargar el popup Periodo Medidor
        /// </summary>
        /// <param name="periodoIni"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult IndicarPeriodoMedidor(int periodoIni)
        {
            InterconexionModel model = new InterconexionModel();
            model.ListaMedidor = logic.ListMeMedidors();
            model.PeriodoIni = periodoIni;
            return PartialView(model);
        }

    }
}
