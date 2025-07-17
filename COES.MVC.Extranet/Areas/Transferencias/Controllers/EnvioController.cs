using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.DemandaMaxima.Helper;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.DemandaMaxima;
using COES.Servicios.Aplicacion.DemandaMaxima.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.MVC.Extranet.Models;
using log4net;
using System.Reflection;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Extranet.App_Start;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.Titularidad;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    [ValidarSesion]
    public class EnvioController : BaseController
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }
        public EnvioController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        #region Propiedades

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesDemandaMaxima.SesionNombreArchivo] != null) ?
                    Session[ConstantesDemandaMaxima.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstantesDemandaMaxima.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[ConstantesDemandaMaxima.SesionFileName] != null) ?
                    Session[ConstantesDemandaMaxima.SesionFileName].ToString() : null;
            }
            set { Session[ConstantesDemandaMaxima.SesionFileName] = value; }
        }

        /// <summary>
        /// Codigo del envio
        /// </summary>
        public int IdEnvio
        {
            get
            {
                return (Session[ConstantesDemandaMaxima.SesionIdEnvio] != null) ?
                    (int)Session[ConstantesDemandaMaxima.SesionIdEnvio] : 0;
            }
            set { Session[ConstantesDemandaMaxima.SesionIdEnvio] = value; }
        }

        public int IdEmpresa
        {
            get
            {
                return (Session["sIdEmpresa"] != null) ?
                    (int)Session["sIdEmpresa"] : 0;
            }
            set { Session["sIdEmpresa"] = value; }
        }

        public String ValMes
        {
            get
            {
                return (Session["sMes"] != null) ?
                    Session["sMes"].ToString() : null;
            }
            set { Session["sMes"] = value; }
        }

        /// <summary>
        /// Nombre del formato
        /// </summary>
        public MeFormatoDTO Formato
        {
            get
            {
                return (Session[ConstantesDemandaMaxima.SesionFormato] != null) ?
                    (MeFormatoDTO)Session[ConstantesDemandaMaxima.SesionFormato] : new MeFormatoDTO();
            }
            set { Session[ConstantesDemandaMaxima.SesionFormato] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[ConstantesDemandaMaxima.SesionMatrizExcel] != null) ?
                    (string[][])Session[ConstantesDemandaMaxima.SesionMatrizExcel] : new string[1][];
            }
            set { Session[ConstantesDemandaMaxima.SesionMatrizExcel] = value; }
        }

        public int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoTR"]);
        public int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaTR"]);

        #endregion

        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        DemandaMaximaAppServicio servicio = new DemandaMaximaAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();


        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int idEmpresa, string mes)
        {
            //base.IdModulo
            int indicador = 0;
            int idEnvio = 0;
            try
            {
                FormatoModel model = BuildHojaExcel(idEmpresa, idEnvio, mes);

                SiEmpresaDTO siEmpresaDto = this.logic.GetByIdSiEmpresa(idEmpresa);

                model.Formato.Formatnombre = siEmpresaDto.Emprabrev + "_MercadoLibre_" + mes;

                FormatoDemandaHelper.GenerarFileExcel(model);
                indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error("GenerarFormato", ex);
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras] + this.Formato.Formatnombre + ".xlsx";
            return File(fullPath, Constantes.AppExcel, this.Formato.Formatnombre + ".xlsx");
        }

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            MeArchivoDTO archivo = new MeArchivoDTO();
            MeEnvioDTO envio = new MeEnvioDTO();
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.FileName = fileRandom + "." + ConstantesDemandaMaxima.ExtensionFileUploadHidrologia;
                    string fileName = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.FileName;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lee datos desde excel
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult LeerFileUpExcel(int idEmpresa, string mes)
        {
            try
            {
                int retorno = FormatoDemandaHelper.VerificarIdsFormato(this.NombreFile, idEmpresa, this.IdFormato);

                if (retorno > 0)
                {
                    MeFormatoDTO formato = logic.GetByIdMeFormato(this.IdFormato);
                    DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
                    formato.FechaProceso = fechaProceso;
                    var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();

                    //- remision-pr16.JDEL - Inicio 13/05/2016: Cambio para atender el requerimiento.
                    //var listaPtos = this.logic.GetByCriteria2MeHojaptomeds(idEmpresa, this.IdFormato, cabercera.Cabquery);

                    //var listaPtos = this.servicio.GetByCriteria4MeHojaptomeds(idEmpresa, this.IdFormato, cabercera.Cabquery);
                    //- JDEL Fin

                    //- remision-pr16.JDEL - Inicio 25/04/2016: Cambio para atender el requerimiento.
                    string fecha = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha).ToString(Constantes.FormatoFecha);
                    List<MeFormatoEmpresaDTO> listPeriodoEnvio = this.servicio.ObtenerListaPeriodoEnvio(fecha, formato.Formatcodi, idEmpresa);
                    MeFormatoEmpresaDTO per = new MeFormatoEmpresaDTO();
                    per = listPeriodoEnvio[0];
                    formato.FechaInicio = per.PeriodoFechaIni.Value;
                    formato.FechaFin = per.PeriodoFechaFin.Value;

                    TitularidadAppServicio servTitEmp = new TitularidadAppServicio();
                    List<SiMigracionDTO> listTiee = servTitEmp.ListarTransferenciasXEmpresaOrigenXEmpresaDestino(-2, idEmpresa, "", 0);
                    DateTime fechaTiee = fechaProceso;
                    if (listTiee.Count > 0 && listTiee.First().Migrafeccorte != null && listTiee.First().Migrafeccorte > formato.FechaInicio && listTiee.First().Migrafeccorte <= formato.FechaFin)
                    {
                        fechaTiee = listTiee.First().Migrafeccorte;
                    }

                    var listaPtos = this.servicio.GetPtoMedicionPR16(idEmpresa, this.IdFormato, fechaTiee.ToString(Constantes.FormatoFecha), cabercera.Cabquery);
                    //- JDEL Fin

                    int nCol = listaPtos.Count;
                    int horizonte = formato.Formathorizonte;
                    FormatoDemandaHelper.GetSizeFormato2(formato);
                    int nBloques = formato.RowPorDia * formato.Formathorizonte;
                    this.MatrizExcel = FormatoDemandaHelper.InicializaMatrizExcel(cabercera.Cabfilas, nBloques, cabercera.Cabcolumnas, nCol);
                    Boolean isValido = FormatoDemandaHelper.LeerExcelFile(this.MatrizExcel, this.NombreFile, cabercera.Cabfilas, nBloques, cabercera.Cabcolumnas, nCol);
                }
                FormatoDemandaHelper.BorrarArchivo(this.NombreFile);
                return Json(retorno);
            }
            catch (Exception ex)
            {
                Log.Error("LeerFileUpExcel", ex);
                return Json(ex.Message);
            }
        }


        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuyera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        protected bool ValidarPlazo(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Metodo para valida la fecha para el procedimiento PR16
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <param name="enPlazo"></param>
        /// <returns></returns>
        protected bool ValidarFechaPr16(MeFormatoDTO formato, int idEmpresa, out int horaini, out int horafin, out bool enPlazo)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;

            DateTime fechaMinimaEnvio = DateTime.Now.AddMonths(-24);
            DateTime fechamaximaEnvio = DateTime.Now.AddMonths(12);

            if (fechaActual >= fechaMinimaEnvio && fechaActual <= fechamaximaEnvio)
            {
                //- En Plazo
                //- La fecha actual de remisión se encuentra dentro del plazo.
                enPlazo = true;
                resultado = true;
            }
            else if (fechaActual > fechamaximaEnvio)
            {
                //- Fuera de plazo
                //- La fecha actual de remisión se encuentra después de la fecha máxima permitida.
                var regfechaPlazo = this.logic.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= fechaMinimaEnvio) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
                enPlazo = false;
            }
            else
            {
                //- Envío inconsistente porque la fecha de envío es anterior a la toma de medición.
                resultado = false;
                enPlazo = false;
            }
            if ((formato.Formatdiaplazo == 0) && (resultado)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;
                if (((hora - 1) % 3) == 0)
                {
                    horaini = hora - 1 - 1 * 3;
                    horafin = hora - 1;
                }
                else
                {
                    horafin = -1;//indica que formato tiempo real no tiene filas habilitadas
                    resultado = false;
                }
            }
            return resultado;
        }
        /// <summary>
        /// Metodo para validar el periodo de remisión en el formato
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        protected bool ValidarFecha(MeFormatoDTO formato, int idEmpresa, out int horaini, out int horafin)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;

            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            else
            {
                var regfechaPlazo = this.logic.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
            }
            if ((formato.Formatdiaplazo == 0) && (resultado)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;
                if (((hora - 1) % 3) == 0)
                {
                    horaini = hora - 1 - 1 * 3;
                    horafin = hora - 1;
                }
                else
                {
                    horafin = -1;//indica que formato tiempo real no tiene filas habilitadas
                    resultado = false;
                }
            }
            //- remision-pr16.JDEL - Inicio 22/03/2016: Cambio para verificar el periodo de ampliación.
            //return true;
            return resultado;
            //- JDEL Fin            
            //return resultado;
        }


        /// <summary>
        /// Graba los datos del archivo Excel Web
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string dataExcel, int idEmpresa, string mes)
        {
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            int exito = 0;
            List<string> celdas = new List<string>();
            celdas = dataExcel.Split(',').ToList();
            string empresa = string.Empty;
            var regEmp = this.logic.GetByIdSiEmpresa(idEmpresa); ;
            //////////////////////////////////////////////////
            if (regEmp != null)
                empresa = regEmp.Emprnomb;

            MeFormatoDTO formato = this.logic.GetByIdMeFormato(this.IdFormato);

            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();

            int filaHead = cabercera.Cabfilas;
            int colHead = cabercera.Cabcolumnas;
            //formato.ListaHoja = this.logic.GetByCriteriaMeFormatohojas(this.IdFormato);



            //- remision-pr16.JDEL - Inicio 25/04/2016: Cambio para atender el requerimiento.
            string fecha = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha).ToString(Constantes.FormatoFecha);
            List<MeFormatoEmpresaDTO> listPeriodoEnvio = this.servicio.ObtenerListaPeriodoEnvio(fecha, formato.Formatcodi, idEmpresa);
            MeFormatoEmpresaDTO per = new MeFormatoEmpresaDTO();
            per = listPeriodoEnvio[0];
            formato.FechaInicio = per.PeriodoFechaIni.Value;
            formato.FechaFin = per.PeriodoFechaFin.Value;
            //- JDEL Fin

            //- remision-pr16.JDEL - Inicio 26/04/2016: Cambio para atender el requerimiento.
            //var listaPto = this.servicio.ObtenerListaPuntosPR16(idEmpresa, this.IdFormato, cabercera.Cabquery);

            TitularidadAppServicio servTitEmp = new TitularidadAppServicio();
            List<SiMigracionDTO> listTiee = servTitEmp.ListarTransferenciasXEmpresaOrigenXEmpresaDestino(-2, idEmpresa, "", 0);
            DateTime fechaTiee = formato.FechaInicio;
            if (listTiee.Count > 0 && listTiee.First().Migrafeccorte != null && listTiee.First().Migrafeccorte > formato.FechaInicio && listTiee.First().Migrafeccorte <= formato.FechaFin)
            {
                fechaTiee = listTiee.First().Migrafeccorte;
            }
            
            var listaPto = this.servicio.GetPtoMedicionPR16(idEmpresa, this.IdFormato, fechaTiee.ToString(Constantes.FormatoFecha), cabercera.Cabquery);
            //- JDEL Fin
            int nPtos = listaPto.Count();

            /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
            formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            FormatoDemandaHelper.GetSizeFormato2(formato);
            //////////////////////////////////////////////////////////////////////////

            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = this.IdFormato;
            config.Emprcodi = idEmpresa;
            config.FechaInicio = formato.FechaFin;
            //int idConfig = this.servicio.GrabarConfigFormatEnvio3(config, formato.FechaInicio.ToString(Constantes.FormatoFecha), formato.FechaFin.ToString(Constantes.FormatoFecha), cabercera.Cabquery);
            int idConfig = logic.GrabarConfigFormatEnvio(config);

            int horaini = 0;//Para Formato Tiempo Real
            int horafin = 0;//Para Formato Tiempo Real
            Boolean enPlazoReal = false;
            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            //Boolean enPlazo = ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);
            Boolean enPlazo = this.ValidarFechaPr16(formato, idEmpresa, out horaini, out horafin, out enPlazoReal);


            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = idEmpresa;
            envio.Enviofecha = DateTime.Now;
            envio.Enviofechaperiodo = formato.FechaProceso;
            envio.Envioplazo = (enPlazoReal) ? "P" : "F";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = DateTime.Now;
            envio.Lastuser = User.Identity.Name;
            envio.Userlogin = User.Identity.Name;
            envio.Formatcodi = this.IdFormato;
            //- remision-pr16.JDEL - Inicio 25/04/2016: Cambio para atender el requerimiento.
            envio.Enviofechaini = formato.FechaInicio;
            envio.Enviofechafin = formato.FechaFin.AddDays(-1);
            envio.Modcodi = (int)base.IdModulo;
            //- JDEL Fin

            this.IdEnvio = logic.SaveMeEnvio(envio);
            model.IdEnvio = this.IdEnvio;
            ///////////////////////////////////////////////////////
            int horizonte = formato.Formathorizonte;
            switch (formato.Formatresolucion)
            {
                case ConstantesDemandaMaxima.ResolucionCuartoHora:
                    int total = (nPtos + formato.Formatcols) * (filaHead + 96 * formato.Formathorizonte);
                    int totalRecibido = celdas.Count;

                    var lista96 = FormatoDemandaHelper.LeerExcelWeb96(celdas, listaPto, (int)formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 4 * formato.Formathorizonte, formato.Formatcheckblanco);

                    Log.Info("Info - " + "GrabarExcelWeb");
                    if (lista96.Count > 0)
                    {
                        Log.Info("Info - " + "lista96.Count: " + lista96.Count);
                        try
                        {
                            Log.Info("Info - " + "Valores: " + "User.Identity.Name: " + User.Identity.Name + " | this.IdEnvio: " + this.IdEnvio + " | idEmpresa: " + idEmpresa + " | formato: " + formato);
                            this.servicio.GrabarValoresCargados96DML(lista96, User.Identity.Name, this.IdEnvio, idEmpresa, formato, ConstantesDemandaMaxima.TptoMediCodiDML);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            envio.Cfgenvcodi = idConfig;
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Mensaje = ConstantesDemandaMaxima.MensajeEnvioExito;
                        }
                        catch (Exception ex)
                        {
                            Log.Info("Info - " + "Resultado = -1");
                            Log.Error(ex);
                            exito = -1;
                            model.Resultado = -1;
                        }
                    }
                    else
                    {
                        exito = -2;
                        model.Resultado = -2;
                    }
                    break;
            }

            model.Resultado = exito;

            if (exito == 1)
            {
                string strPlazo = "Fuera del Plazo";
                if (enPlazoReal)
                {
                    strPlazo = "Dentro del Plazo";
                }

                //int idPlantilla = ConstantesDemandaMaxima.IdInformacionEnviada;
                //SiPlantillacorreoDTO siPlantillaCorreo = this.servicio.GetByIdSiPlantillaCorreo(idPlantilla);

                //SiPlantillacorreoDTO siPlantillaCorreo = (new COES.Servicios.Aplicacion.Correo.CorreoAppServicio()).ObtenerPlantillaPorModulo(
                //    COES.Servicios.Aplicacion.Helper.TipoPlantillaCorreo.NotificacionEnvioExtranet, (int)this.IdModulo);

                //string contenidoEmpresa = empresa;
                //int contenidoIdEnvio = this.IdEnvio;
                //string contenidoPeriodo = mes;
                //ModuloDTO modulo = this.seguridad.ObtenerModulo((int)base.IdModulo);
                //List<string> emails = modulo.ListaAdministradores.Select(x => x.UserEmail).ToList();

                //string mail_ope = "";
                //foreach (var item in emails)
                //{
                //    mail_ope = item;
                //}

                //string mailOperador = mail_ope;
                //string asunto = string.Format(siPlantillaCorreo.Plantasunto, contenidoPeriodo);
                //string contenido = string.Format(siPlantillaCorreo.Plantcontenido, contenidoEmpresa, contenidoPeriodo, contenidoIdEnvio, strPlazo);
                //SeguridadServicio.UserDTO usuario = (SeguridadServicio.UserDTO)Session[DatosSesion.SesionUsuario];
                //string mailTo = string.Empty;
                //mailTo = usuario.UserEmail;
                //Log.Info("mailto: " + mailTo);
                //Log.Info("mailOperador: " + mailOperador);
                //Log.Info("asunto: " + asunto);
                //Log.Info("contenido: " + contenido);
                //COES.Base.Tools.Util.SendEmail(mailTo, mailOperador, asunto, contenido);

                //SiCorreoDTO correo = new SiCorreoDTO();
                //correo.Enviocodi = contenidoIdEnvio;
                //correo.Corrasunto = asunto;
                //correo.Corrcontenido = contenido;
                //correo.Corrfechaenvio = envio.Enviofecha;
                //correo.Corrfrom = HelperApp.ObtenerEmailRemitente();
                //correo.Corrto = mailTo;
                //correo.Corrbcc = mailOperador;
                //correo.Emprcodi = idEmpresa;
                //correo.Corrfechaperiodo = formato.FechaProceso;
                //correo.Plantcodi = siPlantillaCorreo.Plantcodi;
                //this.servicio.SaveSiCorreo(correo);
            }
            return Json(model);
        }

        /// <summary>
        /// Muestra El formato Excel en la Web 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="desEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarHojaExcelWeb(int idEmpresa, int idEnvio, string mes)
        {
            List<MeFormatoDTO> entitys = this.logic.GetByModuloLecturaMeFormatos(Modulos.AppDemandaMaxTransferencia, this.IdLectura, idEmpresa);

            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcel(idEmpresa, idEnvio, mes);

                int imes = Int32.Parse(mes.Substring(0, 2));
                int ianho = Int32.Parse(mes.Substring(3));

                DateTime fec_ini = new DateTime(ianho, imes, 1);
                DateTime fec_fin = new DateTime(ianho, imes, 1).AddMonths(1).AddDays(-1);
                DemandadiaDTO entity = this.servicio.ObtenerDatosMaximaDemanda(fec_ini, fec_fin);

                //- pr16.JDEL - Inicio 24/11/2016: Cambio para atender el requerimiento.
                //DateTime enteredDate = DateTime.Parse(entity.FechaMD);
                //string hora = TimeSpan.FromMinutes(entity.IndiceMDHP * 15).ToString("hh':'mm");
                //jsModel.fechaMaximaDemanda = enteredDate.ToString("dd/MM/yyyy") + " " + hora;

                string fecha = "";
                string hora = "";

                if (entity.FechaMD != null)
                {
                    DateTime enteredDate = DateTime.ParseExact(entity.FechaMD, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    if (entity.IndiceMDHP != null)
                    {
                        hora = TimeSpan.FromMinutes(entity.IndiceMDHP * 15).ToString("hh':'mm");
                    }
                    fecha = enteredDate.ToString(Constantes.FormatoFecha) + " " + hora;
                }

                jsModel.fechaMaximaDemanda = fecha;
                //- JDEL Fin


                SiEmpresaDTO siEmpresaDto = this.logic.GetByIdSiEmpresa(idEmpresa);

                //La empresa se encuentra inactiva
                //if (siEmpresaDto.Emprestado != "A")
                //{
                //    Session["DatosJSON"] = null;
                //    return Json(-2);
                //}

                if (jsModel.Handson.ListaExcelData == null)
                {
                    //La empresa no se encuentra Activa
                    Session["DatosJSON"] = null;
                    return Json(-1);
                }
                else
                {
                    Session["DatosJSON"] = jsModel.Handson.ListaExcelData;
                    jsModel.Handson.ListaExcelData = new string[0][];
                    return Json(jsModel);
                }
            }
            else
            {
                //La empresa no cuenta con puntos de medición
                Session["DatosJSON"] = null;
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DescargarDatos()
        {
            string[][] list = (string[][])Session["DatosJSON"];

            var data = list;
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(data);
            result.ContentType = "application/json";

            return result;
        }


        /// <summary>
        /// Carga principal de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            FormatoModel model = new FormatoModel();

            //model.IdModulo = Modulos.AppMedidoresDistribucion;
            model.IdModulo = (int)base.IdModulo;
            //- JDEL Fin

            int empresa = 0;
            string mes = "";

            if (IdEmpresa != 0)
            {
                empresa = IdEmpresa;
            }

            if (ValMes != null)
            {
                mes = (string)Session["sMes"];
            }

            List<SiEmpresaDTO> listEmpresas = (new FormatoMedicionAppServicio()).GetListaEmpresaFormato(this.IdFormato);

            bool permisoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);

            if (permisoEmpresas)
            {
                //model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
                //    OrderBy(x => x.EMPRNOMB).Select(x => new SiEmpresaDTO
                //    {
                //        Emprcodi = x.EMPRCODI,
                //        Emprnomb = x.EMPRNOMB,
                //        Emprrazsocial = x.EMPRRAZSOCIAL
                //    }).ToList();
                model.ListaEmpresas = listEmpresas.
                    OrderBy(x => x.Emprnomb).Select(x => new SiEmpresaDTO
                    {
                        Emprcodi = x.Emprcodi,
                        Emprnomb = x.Emprnomb,
                        Emprrazsocial = x.Emprrazsocial
                    }).ToList();
            }
            else
            {
                model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
                    Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).OrderBy(x => x.EMPRRAZSOCIAL).Select(x => new SiEmpresaDTO
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB,
                        Emprrazsocial = x.EMPRRAZSOCIAL
                    }).ToList();
            }

            if (empresa != null && empresa != 0)
            {
                model.IdEmpresa = empresa;
            }
            else
            {
                //if (model.ListaEmpresas.Count != 1 && model.ListaEmpresas.Count != 0)
                if (model.ListaEmpresas.Count > 0)
                {
                    model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
                }
                else //La empresa no cuenta con puntos de medición
                {
                    model.MensajeError = "La empresa no tiene asignados puntos de medición.";
                }
            }

            if (mes != null && mes != "")
            {
                model.Mes = mes;

                //Datos MaximaDemanda
                DateTime fec_ini = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                DateTime fec_fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                DemandadiaDTO entity = this.servicio.ObtenerDatosMaximaDemanda(fec_ini, fec_fin);
                model.fechaMaximaDemanda = entity.FechaMD;
            }
            else
            {
                model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

                //Datos MaximaDemanda
                DateTime fec_ini = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                DateTime fec_fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                DemandadiaDTO entity = this.servicio.ObtenerDatosMaximaDemanda(fec_ini, fec_fin);
                model.fechaMaximaDemanda = entity.FechaMD;
            }

            return View(model);

        }


        /// <summary>
        /// Permite obtener el último envio
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerUltimoEnvio(int idEmpresa, string mes)
        {
            MeFormatoDTO formato = logic.GetByIdMeFormato(this.IdFormato);
            DateTime fecha = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            List<MeEnvioDTO> list = this.logic.GetByCriteriaMeEnvios(idEmpresa, this.IdFormato, fecha);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar

            if (list.Count > 0)
            {
                idUltEnvio = list[list.Count - 1].Enviocodi;
            }

            return Json(idUltEnvio);
        }

        /// <summary>
        /// Grabar los cambios del editor de puntos de Suministro
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEditor(string datos, int idEmpresa, string mes)
        {
            int imes = Int32.Parse(mes.Substring(0, 2));
            int ianho = Int32.Parse(mes.Substring(3, 4));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string fecha = fechaProceso.ToString(Constantes.FormatoFecha);

            Char delimiter = '|';
            String[] empresas = datos.Split(delimiter);
            var a = empresas[0];

            List<MePtosuministradorDTO> lista = this.servicio.ListaEditorPtoSuministro(fecha, idEmpresa, this.IdFormato);

            int i = 0;
            foreach (MePtosuministradorDTO obj in lista)
            {
                MePtosuministradorDTO pto = this.servicio.GetByPtoPeriodo(obj.Ptomedicodi, fecha);
                if (pto == null)
                {
                    if (obj.VigEmprcodi != Int32.Parse(empresas[i]))
                    {
                        //Creamos un nuevo registro
                        obj.Emprcodi = Int32.Parse(empresas[i]);
                        obj.Ptosufechainicio = fechaProceso;
                        obj.Ptosuusucreacion = User.Identity.Name;
                        obj.Ptosufeccreacion = DateTime.Now;
                        this.servicio.SaveMePtosuministro(obj);
                    }
                }
                else
                {
                    //Actualizamos la empresa seleccionada es diferente a la vigente
                    if (pto.Emprcodi != Int32.Parse(empresas[i]))
                    {
                        //Actualizamos el registro
                        pto.Emprcodi = Int32.Parse(empresas[i]);
                        pto.Ptosuusumodificacion = User.Identity.Name;
                        pto.Ptosufecmodificacion = DateTime.Now;
                        this.servicio.UpdateMePtosuministro(pto);
                    }
                }
                i++;
            }

            return Json(0);
        }

        /// <summary>
        /// SaveSessionEmpPeriodo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSessionEmpPeriodo(int idEmpresa, string mes)
        {
            int imes = Int32.Parse(mes.Substring(0, 2));
            int ianho = Int32.Parse(mes.Substring(3, 4));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string fecha = fechaProceso.ToString(Constantes.FormatoFecha);

            List<MeEnvioDTO> list = this.servicio.ObtenerListaEnvioActual(idEmpresa, fecha);
            int val = 0;//0: No se remitido el periodo actual
            if (list.Count() == 1)
            {
                if (list[0].EnvioCodiAnt == 0 || list[0].EnvioCodiAnt == null)
                {
                    val = 1;//1:No se ha encontrado remision anterior
                }
                else
                {
                    val = 2;//2: Se ha encontrado remision actual y anterior - Se puede validar la información
                }
            }

            Session["sIdEmpresa"] = idEmpresa;
            Session["sMes"] = mes;
            return Json(val);
        }

        /// <summary>
        ///Devuelve el model necesario para mostrar en la web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcel(int idEmpresa, int idEnvio, string mes)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();
            model.IdEnvio = idEnvio;
            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = logic.GetByIdMeFormato(this.IdFormato);
            this.Formato = model.Formato;

            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            model.Formato.Formatcols = cabercera.Cabcolumnas;
            model.Formato.Formatrows = cabercera.Cabfilas;
            model.Formato.Formatheaderrow = cabercera.Cabcampodef;

            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;

            //- remision-pr16.JDEL - Inicio 25/04/2016: Cambio para atender el requerimiento.
            string fecha = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha).ToString(Constantes.FormatoFecha);
            List<MeFormatoEmpresaDTO> listPeriodoEnvio = this.servicio.ObtenerListaPeriodoEnvio(fecha, model.Formato.Formatcodi, idEmpresa);
            MeFormatoEmpresaDTO periodoEnvio = new MeFormatoEmpresaDTO();
            periodoEnvio = listPeriodoEnvio[0];
            model.Formato.FechaInicio = periodoEnvio.PeriodoFechaIni.Value;
            model.Formato.FechaFin = periodoEnvio.PeriodoFechaFin.Value;
            //- JDEL Fin


            int idCfgFormato = 0;
            if (idEnvio <= 0)
            {
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                var envioAnt = logic.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
            }

            //model.ListaHojaPto = this.servicio.ObtenerListaPuntosPR16(idEmpresa, this.IdFormato, cabercera.Cabquery);
            model.ListaHojaPto = this.servicio.GetPtoMedicionPR16(idEmpresa, this.IdFormato, model.Formato.FechaInicio.ToString(Constantes.FormatoFecha), cabercera.Cabquery);

            TitularidadAppServicio servTitEmp = new TitularidadAppServicio();
            List<SiMigracionDTO> listTiee = servTitEmp.ListarTransferenciasXEmpresaOrigenXEmpresaDestino(-2, idEmpresa, "", 0);
            bool migracion = false;
            DateTime fechaTiee = new DateTime();
            if (listTiee.Count > 0 && listTiee.First().Migrafeccorte != null && listTiee.First().Migrafeccorte > model.Formato.FechaInicio && listTiee.First().Migrafeccorte <= model.Formato.FechaFin)
            {
                migracion = true;
                fechaTiee = listTiee.First().Migrafeccorte;
            }

            if (migracion)
            {
                if (model.ListaHojaPto.Count == 0 || model.ListaHojaPto == null)
                {
                    model.ListaHojaPto = this.servicio.GetPtoMedicionPR16(idEmpresa, this.IdFormato, fechaTiee.ToString(Constantes.FormatoFecha), cabercera.Cabquery);
                }
            }

            if (model.ListaHojaPto.Count > 0)
            {
                var cabecerasRow = model.Formato.Formatheaderrow.Split(QueryParametros.SeparadorFila);
                List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
                for (var x = 0; x < cabecerasRow.Length; x++)
                {
                    var reg = new CabeceraRow();
                    var fila = cabecerasRow[x].Split(QueryParametros.SeparadorCol);
                    reg.NombreRow = fila[0];
                    reg.TituloRow = fila[1];
                    reg.IsMerge = int.Parse(fila[2]);
                    listaCabeceraRow.Add(reg);
                }

                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);

                FormatoDemandaHelper.GetSizeFormato2(model.Formato);

                //- remision-pr16.JDEL - Inicio 26/04/2016: Cambio para atender el requerimiento.
                model.ListaEnvios = this.logic.GetByCriteriaMeEnvios(idEmpresa, this.IdFormato, model.Formato.FechaProceso);
                //model.ListaEnvios = this.logic.GetByCriteriaMeEnvios(idEmpresa, this.IdFormato, model.Formato.FechaInicio);
                //- JDEL Fin


                int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
                if (model.ListaEnvios.Count > 0)
                {
                    idUltEnvio = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                    var reg = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                    if (reg != null)
                        model.FechaEnvio = ((DateTime)reg.Enviofecha).ToString(Constantes.FormatoFechaHora);
                }
                /// Verifica si Formato esta en Plaz0
                string mensaje = string.Empty;
                int horaIni = 0;//Para Formato Tiempo Real
                int horaFin = 0;//Para Formato Tiempo Real
                Boolean valorEnPlazo = false;
                //model.EnPlazo = ValidarPlazo(model.Formato);
                model.EnPlazo = this.ValidarFechaPr16(model.Formato, idEmpresa, out horaIni, out horaFin, out valorEnPlazo);
                model.EnPlazo = valorEnPlazo;

                if ((idEnvio <= 0) || (idEnvio == idUltEnvio)) // id envio < 0 => es una carga de archivo excel, id envio = 0 => envio nuevo 
                {
                    //- remision-pr16.JDEL - Inicio 22/04/2016: Cambio para atender el requerimiento.
                    model.Handson.ReadOnly = !this.ValidarFechaPr16(model.Formato, idEmpresa, out horaIni, out horaFin, out valorEnPlazo);
                    //- JDEL Fin
                }
                else
                { /// Es solo para visualizar envios anteriores
                    model.Handson.ReadOnly = true;
                }
                var entEmpresa = this.logic.GetByIdSiEmpresa(idEmpresa);
                if (entEmpresa != null)
                    model.Empresa = entEmpresa.Emprnomb;
                model.IdEmpresa = idEmpresa;
                model.Anho = model.Formato.FechaInicio.Year.ToString();
                model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);

                model.Dia = model.Formato.FechaInicio.Day.ToString();
                model.Handson.Width = HandsonConstantes.ColWidth * ((model.ListaHojaPto.Count > HandsonConstantes.ColPorHoja) ? HandsonConstantes.ColPorHoja :
                    (model.ListaHojaPto.Count + model.ColumnasCabecera));
                //Genera La vista html complementaria a la grilla Handson, nombre de formato, area coes, fecha de formato, etc.
                model.ViewHtml = FormatoDemandaHelper.GenerarFormatoHtml(model, idEnvio, model.EnPlazo);

                List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
                List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.
                int nCol = model.ListaHojaPto.Count;
                int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia;
                model.Handson.ListaFilaReadOnly = FormatoDemandaHelper.InicializaListaFilaReadOnly(model.Formato.Formatrows, nBloques, !model.Handson.ReadOnly, horaIni, horaFin);
                model.ListaCambios = new List<CeldaCambios>();
                model.Handson.ListaExcelData = FormatoDemandaHelper.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);

                //model.Formato.FechaInicio = model.Formato.FechaInicio.AddDays(-2);
                if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
                {
                    lista = this.logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                    if (idEnvio > 0) //Si se esta consultando un envio anterior se obtienen los cambios de ese envio.
                        listaCambios = this.logic.GetAllCambioEnvio(this.IdFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList(); ;
                    /// Cargar Datos en Arreglo para Web
                    FormatoDemandaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
                }
                else
                { // los datos para visualizar en el excel web provienen de un archivo excel cargado por el usuario
                    //Carga de archivo Excel               
                    model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                    FormatoDemandaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
                }

                model.ListaPtosMME = this.ListarPuntosActivosMME(idEmpresa, model.ListaHojaPto, fecha);

                #region Filas Cabeceras

                for (var ind = 0; ind < model.ColumnasCabecera; ind++)
                {
                    model.Handson.ListaColWidth.Add(160);
                }
                string sTitulo = string.Empty;
                string sTituloAnt = string.Empty;
                int column = model.ColumnasCabecera;
                var cellMerge = new CeldaMerge();
                foreach (var reg in model.ListaHojaPto)
                {
                    model.Handson.ListaColWidth.Add(100);
                    for (var w = 0; w < model.FilasCabecera; w++)
                    {
                        if (column == model.ColumnasCabecera)
                        {
                            model.Handson.ListaExcelData[w] = new string[model.ListaHojaPto.Count + model.ColumnasCabecera];
                            model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                        }
                        var valor = reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null);

                        if (valor != null)
                            model.Handson.ListaExcelData[w][column] = valor.ToString();
                        else
                            model.Handson.ListaExcelData[w][column] = string.Empty;

                        if (listaCabeceraRow[w].IsMerge == 1)
                        {
                            if (listaCabeceraRow[w].TituloRowAnt != model.Handson.ListaExcelData[w][column])
                            {
                                if (column != model.ColumnasCabecera)
                                {
                                    if ((column - listaCabeceraRow[w].ColumnIni) > 1)
                                    {
                                        cellMerge = new CeldaMerge();
                                        cellMerge.col = listaCabeceraRow[w].ColumnIni;
                                        cellMerge.row = w;
                                        cellMerge.colspan = (column - listaCabeceraRow[w].ColumnIni);
                                        cellMerge.rowspan = 1;
                                        model.Handson.ListaMerge.Add(cellMerge);
                                    }
                                }
                                listaCabeceraRow[w].TituloRowAnt = model.Handson.ListaExcelData[w][column];
                                listaCabeceraRow[w].ColumnIni = column;
                            }
                        }
                    }
                    column++;
                }
            }
            else
            {
                //La empresa no se encuentra activa
                Session["DatosJSON"] = null;
                model.Handson.ListaExcelData = null;
            }

            //- pr16.JDEL - Inicio 21/10/2016: Cambio para atender el requerimiento.
            model.ListaPtoSuministrador = this.servicio.ListaEditorPtoSuministro(model.Formato.FechaInicio.ToString(Constantes.FormatoFecha), idEmpresa, this.IdFormato);
            //- JDEL Fin

            model.ListaEmpresasSuministradoras = this.servicio.ListaEmpresasSuministrador();

            #endregion

            return model;
        }

        public List<MePtomedicionDTO> ListarPuntosActivosMME(int idEmpresa, List<MeHojaptomedDTO> ListaHojaPto, string fechainicio)
        {
            List<MePtomedicionDTO> lstPtosMME = new List<MePtomedicionDTO>();
            List<TrnConfiguracionPmmeDTO> lstConfiguraciones = new List<TrnConfiguracionPmmeDTO>();


            DateTime fInicio = DateTime.ParseExact(fechainicio, Constantes.FormatoFecha, null);
            //int messiguiente = primerdia.Month + 1;
            //DateTime ultimodia = DateTime.ParseExact("01/" + messiguiente.ToString().PadLeft(2,Constantes.CaracterCero) + "/" + primerdia.Year, Constantes.FormatoFecha, null).AddDays(-1);

            lstConfiguraciones = servicioTransferencia.ListaConfPtosMMExEmpresa(idEmpresa).ToList();

            foreach (var pto in ListaHojaPto)
            {
                TrnConfiguracionPmmeDTO ptoNoVigente = lstConfiguraciones.Where(x => x.Ptomedicodi == pto.Ptomedicodi && x.Vigencia == "N").OrderByDescending(x => x.Fechavigencia).FirstOrDefault();
                TrnConfiguracionPmmeDTO ptoVigente = lstConfiguraciones.Where(x => x.Ptomedicodi == pto.Ptomedicodi && x.Vigencia == "S").OrderByDescending(x => x.Fechavigencia).FirstOrDefault();

                if (ptoVigente != null)
                {
                    if(ptoNoVigente != null)
                    {
                        if (ptoVigente.Fechavigencia < ptoNoVigente.Fechavigencia)
                        {
                            MePtomedicionDTO ptolista = new MePtomedicionDTO();
                            ptolista.Ptomedicodi = pto.Ptomedicodi;
                            lstPtosMME.Add(ptolista);
                        }
                    }
                    else
                    {
                        MePtomedicionDTO ptolista = new MePtomedicionDTO();
                        ptolista.Ptomedicodi = pto.Ptomedicodi;
                        lstPtosMME.Add(ptolista);
                    }                  
                }
            }

            return lstPtosMME;
        }
    }
}
