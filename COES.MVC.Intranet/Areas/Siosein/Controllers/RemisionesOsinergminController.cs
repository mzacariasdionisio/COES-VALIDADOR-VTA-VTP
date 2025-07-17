using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Siosein.Helper;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IntercambioOsinergmin;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.Servicios.Aplicacion.SIOSEIN.Util; //SIOSEIN-PRIE-2021
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text; //SIOSEIN-PRIE-2021
using System.Threading.Tasks;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Siosein.Controllers
{
    public class RemisionesOsinergminController : BaseController
    {
        private readonly SIOSEINAppServicio servicio = new SIOSEINAppServicio();
        private readonly FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        private readonly RemisionAppServicio _remisionAppServicio;

        private readonly RemisionCoesEndpointService _wsRemisionCoes;
        private readonly string _urlRemisionAOsi = ConfigurationManager.AppSettings["UrlRemisionAOsi"].ToString();
        private readonly int _Timeout = 160000;  //- Un minuto(60000 milisegundos)

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(RemisionesOsinergminController));
        private static string NameController = "RemisionesOsinergminController";

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

        /// <summary>
        /// listado ventos del controller
        /// </summary>
        public RemisionesOsinergminController()
        {
            if (string.IsNullOrEmpty(this._urlRemisionAOsi))
            {
                throw new ApplicationException("No se ha encontrado la definición de la dirección URL del servicio de remisión hacia el Osinergmin");
            }
            _wsRemisionCoes = new RemisionCoesEndpointService(this._urlRemisionAOsi)
            {
                Timeout = this._Timeout
            };
            _remisionAppServicio = new RemisionAppServicio();
        }


        /// <summary>
        /// index de remisiones osinergmin
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SioseinModel model = new SioseinModel();
            model.Fecha = DateTime.Today.Year.ToString();

            return View(model);
        }

        #region Pantalla inicial

        /// <summary>
        /// partial html de los periodos
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public PartialViewResult ListadoPeriodo(string anio)
        {
            //busqueda de modelo usando año
            List<IioPeriodoSeinDTO> periodos = _remisionAppServicio.PeriodoGetByCriteria(anio);

            return PartialView(periodos);
        }

        /// <summary>
        /// partial html de un nuevo periodo
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CargarViewNuevoPeriodo()
        {
            return PartialView();
        }

        /// <summary>
        /// creamos la lista de tablas prie por periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="ultimaFecha"></param>
        /// <returns></returns>
        public JsonResult CrearPeriodo(string periodo)
        {
            try
            {
                int anho = Int32.Parse(periodo.Substring(3));
                int mes = Int32.Parse(periodo.Substring(0, 2));
                DateTime fecha = new DateTime(anho, mes, 1);
                string per = fecha.ToString("yyyyMM");

                _remisionAppServicio.PeriodoSave(new IioPeriodoSeinDTO
                {
                    PseinAnioMesPerrem = per,
                    PseinFecUltEnvio = DateTime.MinValue,
                    PseinEstRegistro = "1",
                    PseinConfirmado = "1",
                    PseinEstado = "1",
                    PseinFecPriEnvio = DateTime.MinValue,
                    PseinUsuCreacion = base.UserName,
                    PseinUsuModificacion = base.UserName
                });


                var List = servicio.ListSioTablapries();

                DateTime fechaRegistro = DateTime.Now;
                foreach (var item in List)
                {
                    SioCabeceradetDTO entity = new SioCabeceradetDTO();

                    entity.Cabpriusucreacion = base.UserName;
                    entity.Cabpriperiodo = fecha;
                    entity.Cabprifeccreacion = fechaRegistro;
                    entity.Tpriecodi = item.Tpriecodi;

                    servicio.SaveSioCabeceradet(entity);
                }
                return Json(true);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                return Json(false);
            }
        }

        /// <summary>
        /// validamos periodo existente
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult ValidarPeriodo(string periodo)
        {
            try
            {
                DateTime fechaInicioMes = DateTime.ParseExact(periodo, ConstantesSioSein.FormatAnioMes, CultureInfo.InvariantCulture);
                string periodoMes = fechaInicioMes.ToString(ConstantesSioSein.FormatAnioMes);
                var modelo = _remisionAppServicio.PeriodoGetById(periodoMes);

                if (fechaInicioMes > DateTime.Today)
                {
                    return Json(2);
                }
                return Json(modelo == null ? 1 : 0);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                return Json(1);
            }
        }

        #endregion

        /// <summary>
        /// mostramos lista de tablas prie por el periodo a consultar
        /// </summary>
        /// <param name="Cabpriperiodo"></param>
        /// <returns></returns>
        public ActionResult ListaTablasPrie(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            DateTime fechaPeriodo = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            SioseinModel model = new SioseinModel
            {
                PeriodoSelect = periodo,
                Formato = servFormato.GetByIdMeFormato(ConstantesSioSein.idFormato)
            };

            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Aprobar, base.UserName);
            model.TituloWeb = "Remisión de Información a Osinergmin - " + EPDate.f_NombreMes(fechaPeriodo.Month).ToUpper() + " " + fechaPeriodo.Year;

            //
            //Obtener el mes y año del periodo
            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string periodoYYYYMM = fechaProceso.ToString("yyyyMM");

            model.Formato.FechaInicio = fechaPeriodo;
            model.Formato.FechaFin = fechaPeriodo.AddMonths(1).AddDays(-1);

            //CONSULTAS las 35 Tablas
            model.ListaTablaPrie = servicio.GetListSioTablaprieByPeriodo(fechaPeriodo).OrderBy(x => x.Tpriecodi).ToList();

            //verificar si existe datos cargados en el excel web de la tabla 09
            bool existeExcel9 = servicio.GetByCriteriaSioPrieComps(fechaPeriodo).Any();
            model.CountCarga09 = existeExcel9 ? 1 : 0;

            //obtener datos de Procesamiento .txt
            List<SioCabeceradetDTO> listaProcXTabla = servicio.GetByCriteriaXPeriodo(fechaPeriodo);

            //obtener datos de remisiones
            IioPeriodoSeinDTO iioPeriodoSeinDTO = _remisionAppServicio.PeriodoGetById(periodoYYYYMM);
            List<IioControlCargaDTO> listaControlCarga = _remisionAppServicio.ControlCargaGetByPeriodo(iioPeriodoSeinDTO.PseinCodi);

            foreach (var regPrie in model.ListaTablaPrie)
            {
                regPrie.TituloTabla = string.Format("TABLA {0}: {1} ({2})", regPrie.Tpriecodi.ToString("D2"), regPrie.Tpriedscripcion, regPrie.Tprieabrev);
                regPrie.PseinCodi = iioPeriodoSeinDTO.PseinCodi;

                //datos de último procesamiento
                regPrie.FechaUltimaVerificacionDesc = string.Empty;
                regPrie.UsuarioUltimaVerificacionDesc = string.Empty;

                SioCabeceradetDTO objCab = listaProcXTabla.Find(x => x.Tpriecodi == regPrie.Tpriecodi);
                if (objCab != null)
                {
                    regPrie.FechaUltimaVerificacionDesc = objCab.Cabprifeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull);
                    regPrie.UsuarioUltimaVerificacionDesc = objCab.Cabpriusucreacion ?? "";
                }

                //datos de última remisión
                regPrie.FechaUltimaRemisionDesc = string.Empty;
                regPrie.UsuarioUltimaRemisionDesc = string.Empty;
                regPrie.EstadoUltimaRemisionDesc = string.Empty;

                IioControlCargaDTO controlCarga = listaControlCarga.Find(x => x.RtabCodi.Trim() == regPrie.Tprieabrev.Trim());
                if (controlCarga != null)
                {
                    regPrie.RccaCodi = controlCarga.RccaCodi;
                    regPrie.EstadoUltimaRemisionDesc = controlCarga.RccaEstadoEnvio;

                    DateTime fechaUltimamodif = controlCarga.RccaFecModificacion != null ? controlCarga.RccaFecModificacion.Value : controlCarga.RccaFecCreacion ?? DateTime.MinValue;
                    string usuarioUltimamodif = controlCarga.RccaFecModificacion != null ? controlCarga.RccaUsuModificacion : controlCarga.RccaUsuCreacion;

                    DateTime fechaMin = new DateTime(2010, 1, 1);
                    if (fechaUltimamodif > fechaMin)
                        regPrie.FechaUltimaRemisionDesc = fechaUltimamodif.ToString(ConstantesAppServicio.FormatoFechaFull);
                    if (usuarioUltimamodif != null) regPrie.UsuarioUltimaRemisionDesc = usuarioUltimamodif;
                }
            }

            return View(model);
        }

        #region Pantalla de periodo seleccionado

        /// <summary>
        /// Procesar la tabla de remisión
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tabla"></param>
        /// <param name="tieneError"></param>
        /// <returns></returns>
        public JsonResult RemitirRegistroIndividual(string periodo, string tabla)
        {
            base.ValidarSesionJsonResult();
            if (!base.VerificarAccesoAccion(Acciones.Aprobar, base.UserName)) throw new ArgumentException("Usted no tiene permisos para realizar esta acción.");

            int tieneErrores = 0;
            SioseinModel model = new SioseinModel();
            this.RemitirRegistro(periodo, tabla, ref tieneErrores, ref model, 1);

            return Json(model);
        }

        /// <summary>
        /// Método para remitir todas las tablas
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public JsonResult RemitirTodo(string periodo, string cadena)
        {
            base.ValidarSesionJsonResult();
            if (!base.VerificarAccesoAccion(Acciones.Aprobar, base.UserName)) throw new ArgumentException("Usted no tiene permisos para realizar esta acción.");

            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string date = fechaProceso.ToString("yyyyMM");

            SioseinModel model = new SioseinModel();

            //var periodoDto = servicio.PeriodoGetById(new IioPeriodoSeinDTO { PseinAnioMesPerrem = date });
            //List<IioTablaSyncDTO> lstTablas = servicio.List(periodoDto.PseinCodi);

            Char delimiter = ',';
            String[] tablas = cadena.Split(delimiter);

            int tieneError = 0;
            int nroTablasError = 0;
            SioseinModel model1 = new SioseinModel();
            foreach (var tab in tablas)
            {
                if (tab != null && tab.Trim() != "") //SIOSEIN-PRIE-2021
                {
                    tieneError = 0;
                    RemitirRegistro(periodo, tab, ref tieneError, ref model1, 0);
                    if (tieneError == 1)
                    {
                        nroTablasError += 1;
                    }
                }
            }
            if (nroTablasError > 0)
            {
                model.Mensaje = "Algunas tablas presentaron errores al enviar la información, por favor consulte la lista para conocer el detalle. Nro. de Tablas con error : " + nroTablasError;
                model.NRegistros = 1;
            }
            else
            {
                model.Mensaje = "Todas las tablas fueron remitidas al Osinergmin";
                model.NRegistros = 0;
            }
            //- HDT Fin

            return Json(model);
        }

        /// <summary>
        /// Valida si existen las carpetas y si no existen las crea
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        protected string ValidarFolder(string periodo, int tipRemi)
        {
            int imes = Int32.Parse((tipRemi == 0 ? periodo.Substring(0, 2) : periodo.Substring(0, 2)));
            int ianho = Int32.Parse((tipRemi == 0 ? periodo.Substring(3) : periodo.Substring(3)));
            DateTime fecha = new DateTime(ianho, imes, 1);

            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel;
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "Remision");
            Directory.CreateDirectory(path + "Remision" + "/" + fecha.Year.ToString());
            Directory.CreateDirectory(path + "Remision" + "/" + fecha.Year.ToString() + "/" + fecha.Month.ToString("d2"));

            return AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel + "Remision/" + fecha.Year + "/" + fecha.Month.ToString("d2") + "/";
        }

        /// <summary>
        /// Procesar la tabla de remisión
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tabla"></param>
        /// <param name="tieneError"></param>
        /// <param name="model"></param>
        /// <param name="tipRemi"></param>
        /// <returns></returns>
        private JsonResult RemitirRegistro(string periodo, string rtabCodi, ref int tieneError, ref SioseinModel model, int tipRemi)
        {
            DateTime fechaRegistro = DateTime.Now;

            //Obtener el mes y año del periodo
            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string periodoYYYYMM = fechaProceso.ToString("yyyyMM");

            //Guarda el Envio (cabecera)
            MeEnvioDTO envio = new MeEnvioDTO
            {
                Emprcodi = 1,
                Enviofecha = fechaRegistro,
                Enviofechaperiodo = fechaProceso,
                Envioplazo = "P",
                Estenvcodi = ConstantesSioSein.EnvioEnviado,
                Lastdate = fechaRegistro,
                Lastuser = base.UserName,
                Userlogin = base.UserName,
                Formatcodi = 53,//Formato que enviara el COES
                Modcodi = 14//(int)base.IdModulo
            };
            int idEnvio = _remisionAppServicio.SaveMeEnvio(envio);

            //Obtiene el PseinCodi de la tabla iio_periodo_sein según su periodo
            IioPeriodoSeinDTO iioPeriodoSeinDTO = _remisionAppServicio.PeriodoGetById(periodoYYYYMM);
            int pseinCodi = iioPeriodoSeinDTO.PseinCodi;

            //Obtiene los datos generales de la tabla sio_tablaprie
            IioTablaSyncDTO tab_ = _remisionAppServicio.List(pseinCodi).Where(x => x.RtabCodi.Trim() == rtabCodi).FirstOrDefault();

            //Valida la creación de la carpeta dentro del proyecto, si no existe la crea
            string rutaFolder = ValidarFolder(periodo, tipRemi);
            string zipPathFileName = string.Format("{0}\\{1}_{2}.zip", rutaFolder, tab_.RtabCodi.Trim(), idEnvio);

            //Obtiene los registros de la tabla sio_datoprie
            //Genera el archivo zip, con el contenido de los datos de la tabla sio_datoprie
            List<SioDatoprieDTO> ListDatosPrie = servicio.GetListaSioDatoprie(Convert.ToInt32(tab_.RtabNombreTabla), fechaProceso, out bool existeVersionProcesada);
            int countRows = servicio.GeneraArchivoZipPrie2(ListDatosPrie, Convert.ToInt32(tab_.RtabNombreTabla), tab_.RtabCodTablaOsig.Trim(), zipPathFileName.ToString());

            string estadoEnvio = "2"; //por defecto no existen registros
            bool esTieneError = false;
            int cantErrores = 0;
            List<IioLogRemisionDTO> listaLog = new List<IioLogRemisionDTO>();

            //En caso exista registros en el archivo zip para remitir, se envia dicha información a Osinergmin
            if (countRows > 0)
            {
                //Lee la información (es decir, su contenido se pasa a un array byte) que hay en el archivo zip
                FileStream fs = System.IO.File.OpenRead(zipPathFileName.ToString());
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();

                //Se declara e instancia la variable para procesar la remisión de los datos a Osinergmin
                string empresa = "COES";
                string host = "127.0.0.1";
                string user = "usuariocoes";

                riProcesarRemisionDatos ri = new riProcesarRemisionDatos
                {
                    codigoEmpresa = empresa,
                    codigoPeriodoRemision = periodoYYYYMM,
                    codigoTabla = tab_.RtabCodTablaOsig.Trim(),
                    delimitador = ConstantesSioSein.Delimitador,
                    informacionARemitir = data,
                    terminal = host,
                    usuario = user
                };

                //Envío de los datos de la tabla a Osinergmin
                roProcesarRemisionDatos ro = new roProcesarRemisionDatos();
                try
                {
                    ro = _wsRemisionCoes.procesarRemisionDatos(ri);
                    Task.Delay(2000);
                }
                catch (Exception ex)
                {
                    Log.Error(NameController, ex);

                    ro = new roProcesarRemisionDatos();
                    ro.mensajeResultante = ex.ToString();
                }

                string mensajeResultante = ro.mensajeResultante ?? string.Empty;
                esTieneError = ro.listaErrores != null || ro.valorResultante == 0;
                if (esTieneError)
                {
                    //ro.listaErrores != null. En caso de existir errores en los datos enviados de la tabla PRIE, se declaran estos valores para estas variables
                    if (ro.listaErrores != null && ro.listaErrores.Count() > 0)
                        cantErrores = ro.listaErrores.Count();

                    //ro.valorResultante == 0. En caso de existir error en el llamado al servicio web de Osinergmin, se declaran estos valores para estas variables
                    if (!string.IsNullOrEmpty(mensajeResultante))
                        cantErrores = 1;
                }

                //Consulta si existe control de carga
                estadoEnvio = !esTieneError ? "1" : "0"; //1: EXITO. 0: ERROR

                if (esTieneError)
                {
                    if (ro.listaErrores != null && ro.listaErrores.Count() > 0)
                    {
                        foreach (var item in ro.listaErrores)
                        {
                            IioLogRemisionDTO scoLogRemisionDto = new IioLogRemisionDTO();
                            scoLogRemisionDto.RlogCodi = 0;
                            scoLogRemisionDto.RlogNroLinea = item.nroError;
                            scoLogRemisionDto.RlogDescripcionError = item.mensajeError;
                            scoLogRemisionDto.Enviocodi = idEnvio;
                            listaLog.Add(scoLogRemisionDto);
                        }
                    }

                    if (!string.IsNullOrEmpty(mensajeResultante))
                    {
                        IioLogRemisionDTO scoLogRemisionDto = new IioLogRemisionDTO();
                        scoLogRemisionDto.RlogCodi = 0;
                        scoLogRemisionDto.RlogNroLinea = 0;
                        scoLogRemisionDto.RlogDescripcionError = mensajeResultante;
                        scoLogRemisionDto.Enviocodi = idEnvio;
                        listaLog.Add(scoLogRemisionDto);
                    }
                }

            }

            IioControlCargaDTO iioControlCargaDto = new IioControlCargaDTO();
            iioControlCargaDto.PseinCodi = pseinCodi;
            iioControlCargaDto.RtabCodi = tab_.RtabCodi.Trim();
            iioControlCargaDto.RccaNroRegistros = countRows;
            iioControlCargaDto.RccaEstadoEnvio = estadoEnvio;
            iioControlCargaDto.RccaFecHorEnvio = fechaRegistro;
            iioControlCargaDto.RccaFecCreacion = fechaRegistro; //fecha y hora antes de remitir a Osinergmin
            iioControlCargaDto.RccaUsuCreacion = base.UserName;
            iioControlCargaDto.Enviocodi = idEnvio;

            //guardar en BD
            _remisionAppServicio.GuardarRemisionOsinergmin(iioControlCargaDto, listaLog);

            _remisionAppServicio.ActualizarEstadoRemisionOsinergmin(iioControlCargaDto, iioPeriodoSeinDTO, base.UserName);

            //El nuevo PRIE ya no guarda el archivo en el FileServer
            if (countRows > 0)
            {
                if (esTieneError)
                {

                    model.Mensaje = "Se remitió la información pero se encontraron " + cantErrores + " errores.";
                    model.NRegistros = 1;
                    tieneError = 1;
                }
                else
                {
                    //Si no hay errores
                    model.Mensaje = "Envío exitoso.";
                    model.NRegistros = 0;
                }
            }
            else
            {
                model.Mensaje = "No se encontraron registros en la '" + tab_.RtabDescripcionTabla + "'";
                model.NRegistros = 1;
            }

            //eliminar archivo temporal
            System.IO.File.Delete(zipPathFileName.ToString());

            return Json(model);
        }

        /// <summary>
        /// Valida el formato Osinergmin sobre los valores de la columna dprievalor de la tabla sio_datoprie para una tabla PRIE específica
        /// </summary>
        /// <param name="tpriecodi">Número de la tabla prie</param>
        /// <param name="tprieabrev">Abreviatura de la tabla prie</param>
        /// <param name="periodo">Periodo de la consulta ('MM YYYY'). Ej: ('01 2021')</param>
        /// <returns>Retorna mensaje si encuentra errores</returns>
        public JsonResult ValidarFormatoOsinergmin(int tpriecodi, string tprieabrev, string periodo)
        {
            SioseinModel model = new SioseinModel();
            List<IioLogRemisionDTO> listaLogErrores = servicio.ValidarFormatoOsinergmin(tpriecodi, periodo);

            if (listaLogErrores.Count > 0)
            {
                //En caso de existir errores
                //Obtener el mes y año del periodo
                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3));
                DateTime fechaProceso = new DateTime(ianho, imes, 1);
                string periodoYYYYMM = fechaProceso.ToString("yyyyMM");

                StringBuilder fileName = new StringBuilder("LogErrores_");
                fileName.Append(tprieabrev);
                fileName.Append(periodoYYYYMM.Substring(2));
                fileName.Append(".xlsx");

                StringBuilder rutaNombreArchivo = new StringBuilder();
                rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesSioSein.RutaReportePRIE].ToString());
                rutaNombreArchivo.Append(fileName.ToString());

                ExcelDocumentSioSein.ExportarLogErrores(rutaNombreArchivo.ToString(), listaLogErrores, base.UserName, DateTime.Now);
                model.Resultado = "0";
                model.Resultado2 = "action-error";
                model.Mensaje = "Existen errores. Descargue el archivo: ";
                model.Mensaje2 = fileName.ToString();
            }
            else
            {
                //En caso de no existir errores
                model.Resultado = "1";
                model.Resultado2 = "action-exito";
                model.Mensaje = "Validación interna exitosa.";
            }

            return Json(model);
        }

        /// <summary>
        /// Permite generar el archivo excel con los errores
        /// </summary>
        /// <param name="periodo">Contenido del archivo a exportar, definido por hojas excel</param>
        /// <param name="tpriecodi">Número de la tabla prie</param>
        [HttpPost]
        public JsonResult ExportarLogErrores(string periodo, string tpriecodi, int rccacodi)
        {
            StringBuilder metodo = new StringBuilder();
            metodo.Append(NameController);
            metodo.Append(".ExportarLogErrores(string periodo, string tpriecodi) - periodo = ");
            metodo.Append(periodo);
            metodo.Append(" , tpriecodi = ");
            metodo.Append(tpriecodi);

            try
            {
                //Obtener el mes y año del periodo
                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3));
                DateTime fechaProceso = new DateTime(ianho, imes, 1);
                string periodoYYYYMM = fechaProceso.ToString("yyyyMM");

                //Obtiene el PseinCodi de la tabla iio_periodo_sein según su periodo
                IioPeriodoSeinDTO iioPeriodoSeinDTO = _remisionAppServicio.PeriodoGetById(periodoYYYYMM);

                if (iioPeriodoSeinDTO == null)
                {
                    //En caso no exista el periodo, devolvemos "-2"
                    return Json(-2);
                }

                SioTablaprieDTO tab_ = servicio.GetByIdSioTablaprie(int.Parse(tpriecodi));

                //Obtenenos los datos del control de carga
                IioControlCargaDTO controlCarga = _remisionAppServicio.ControlCargaGetById(rccacodi);

                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesSioSein.RutaReportePRIE].ToString();
                string reporte = servicio.ExportarReporteLogErrores(rutaArchivo, tab_, periodoYYYYMM, controlCarga);
                return Json(reporte);
            }
            catch (Exception e)
            {
                metodo.Append(" , e.Message: ");
                metodo.Append(e.Message);
                Log.Error(metodo.ToString());
                return Json(-1);
            }
        }

        /// <summary>
        /// Historial verificacion de tabla
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tpriecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarHistorialVerificacion(string periodo, int tpriecodi)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3));
                DateTime fechaProceso = new DateTime(ianho, imes, 1);

                model.ListaHistorialVerificacion = servicio.GetByCriteriaSioCabeceradet(fechaProceso, tpriecodi);
                model.Resultado = "1";
            }
            catch (Exception e)
            {
                model.Resultado = "-1";
                model.Mensaje = e.ToString();
                Log.Error(NameController, e);
            }

            return Json(model);
        }

        /// <summary>
        /// Historial remision de tabla
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tpriecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarHistorialRemision(int pseinCodi, string rtabcodi)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                model.ListaHistorialRemision = _remisionAppServicio.ControlCargaGetByPeriodoXTabla(pseinCodi, rtabcodi);
                model.Resultado = "1";
            }
            catch (Exception e)
            {
                model.Resultado = "-1";
                model.Mensaje = e.ToString();
                Log.Error(NameController, e);
            }

            return Json(model);
        }

        #endregion


    }
}