using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Remision;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.IntercambioOsinergmin;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using COES.MVC.Intranet.Areas.IntercambioOsinergmin.Helper;
using log4net;
using System.Reflection;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Controllers
{
    public class RemisionesController : BaseController
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

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly RemisionAppServicio remisionAppServicio;

        //- alpha.HDT - Inicio 02/11/2017: Cambio para atender el requerimiento.
        //RemisionCoesEndpointService service = new RemisionCoesEndpointService();
        RemisionCoesEndpointService service = null;
        //- HDT Fin

        public RemisionesController()
        {
            log4net.Config.XmlConfigurator.Configure();
            remisionAppServicio = new RemisionAppServicio();

            //- alpha.HDT - Inicio 02/11/2017: Cambio para atender el requerimiento.
            //service = new RemisionCoesEndpointService();
            string url = ConfigurationManager.AppSettings["UrlRemisionAOsi"].ToString();
            if (string.IsNullOrEmpty(url))
            {
                throw new ApplicationException("No se ha encontrado la definición de la dirección URL del servicio de remisión hacia el Osinergmin");
            }
            service = new RemisionCoesEndpointService(url);
            service.Timeout = 1800 * 1000;  //- Media hora.
            //- HDT Fin
        }

        public static string LocalDirectory = ConfigurationManager.AppSettings["LocalDirectory"];

        /// <summary>
        /// Clase Inicial
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public ActionResult Index(int anio = 0)
        {
            //busqueda de modelo usando año
            var anios = remisionAppServicio.PeriodoListAnios();
            var anioFormateado = (anio == 0 ? DateTime.Today.Year : anio).ToString();
            var modelo = new FiltroListadoPeriodosRemisionModel(anios, anioFormateado);

            return View("Index", modelo);
        }

        /// <summary>
        /// Muestra la lista de periodos que corresponden al año entregado
        /// </summary>
        /// <param name="anio">Año con el cual filtrar los periodos</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarPeriodos(string anio)
        {
            //busqueda de modelo usando año
            var periodos = remisionAppServicio.PeriodoGetByCriteria(anio);

            //RemisionCoesWS.riObtenerCamposTablaRemision s = new RemisionCoesWS.riObtenerCamposTablaRemision { codigoTabla = "TMP_SEIN_POTENCIA_FIRME" };
            //var campos = service.obtenerCamposTablaRemision(s);

            return PartialView("ListarPeriodos", periodos.Select(PeriodoRemisionModel.Create).ToList());
        }

        /// <summary>
        /// Obtiene la vista de edición de un periodo
        /// </summary>
        /// <param name="periodo">Periodo a editar</param>
        /// <returns>HTML con el partial view de edición</returns>
        [HttpGet]
        public ActionResult Edit(string periodo)
        {
            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fech = new DateTime(anho, mes, 1);
            string per = fech.ToString("yyyyMM");
            // Obtenemos el Periodo según el Id proporcionado
            var periodoDto = remisionAppServicio.PeriodoGetById(per);
            var periodos = remisionAppServicio.PeriodoGetByCriteria(periodo.Substring(0, 4));
            var modelo = new DetallePeriodoRemisionModel(PeriodoRemisionModel.Create(periodoDto), periodos.Select(dto => PeriodoRemisionModel.Create(dto).Periodo).ToList());
            return View("Edit", modelo);
        }

        /// <summary>
        /// Obtiene la vista de creación de un periodo
        /// </summary>
        /// <returns>HTML con el Partial view</returns>
        [HttpGet]
        public ActionResult Create()
        {
            var modelo = new DetallePeriodoRemisionModel
            {
                PeriodoRemisionModel = new PeriodoRemisionModel
                {
                    //PrimerEnvioDate = DateTime.MinValue,
                    //UltimoEnvioDate = DateTime.MinValue
                }
            };
            return View("Create", modelo);
        }

        /// <summary>
        /// Valida si el periodo existe o no
        /// </summary>
        /// <param name="periodo"> Periodo a validar</param>
        /// <returns> 0 si existe, 1 si no existe</returns>
        [HttpPost]
        public JsonResult ValidarPeriodo(string periodo)
        {
            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fech = new DateTime(anho, mes, 1);
            string per = fech.ToString("yyyyMM");

            var modelo = remisionAppServicio.PeriodoGetById(per);

            if (fech > DateTime.Today)
            {
                return Json(2);
            }

            return Json(modelo == null ? 1 : 0);
        }

        /// <summary>
        /// Crea el periodo
        /// </summary>
        /// <param name="periodo"> Periodo a crear</param>
        /// <returns> 1 si lo logra crear</returns>
        [HttpPost]
        public JsonResult CrearPeriodo(string periodo)
        {
            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fech = new DateTime(anho, mes, 1);
            string per = fech.ToString("yyyyMM");


            remisionAppServicio.PeriodoSave(new IioPeriodoSeinDTO
            {
                PseinAnioMesPerrem = per,
                PseinFecUltEnvio = DateTime.MinValue,
                PseinEstRegistro = "1",
                PseinConfirmado = "1",
                PseinEstado = "1",
                PseinFecPriEnvio = DateTime.MinValue,
                PseinUsuCreacion = UserName,
                PseinUsuModificacion = UserName
            });

            return Json(1);
        }
        


        /// <summary>
        /// Lista las tablas a procesar
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarEntidades(string periodo)
        {

            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);

            string date = fechaProceso.ToString("yyyyMM");

            // Obtenemos el Periodo según el Id proporcionado
            var periodoDto = remisionAppServicio.PeriodoGetById(date);

            EntidadRemisionModel model = new EntidadRemisionModel();
            
            model.ListarEntidades = remisionAppServicio.List(periodoDto.PseinCodi);
            return View(model);
        }

        /// <summary>
        /// Remitir información
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="entidades"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Remitir(string periodo, List<int> entidades)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método para remitir todas las tablas
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public JsonResult RemitirTodo(string periodo, string cadena)
        {
            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string date = fechaProceso.ToString("yyyyMM");

            EntidadRemisionModel model = new EntidadRemisionModel();

            var periodoDto = remisionAppServicio.PeriodoGetById(date);

            List<IioTablaSyncDTO> lstTablas = remisionAppServicio.List(periodoDto.PseinCodi);

            Char delimiter = ',';
            String[] tablas = cadena.Split(delimiter);

            int tieneError = 0;
            int nroTablasError = 0;
            EntidadRemisionModel model1 = new EntidadRemisionModel();
            foreach (var tab in tablas)
            {
                tieneError = 0;
                RemitirRegistro(periodo, tab, ref tieneError, ref model1);
                if (tieneError == 1)
                {
                    nroTablasError += 1;
                }
            }
            if (nroTablasError > 0)
            {
                model.mensaje = "Algunas tablas presentaron errores al enviar la información, por favor consulte la lista para conocer el detalle. Nro. de Tablas con error : " + nroTablasError;
                model.resultado = 1;
            }
            else
            {
                model.mensaje = "Todas las tablas fueron remitidas al Osinergmin";
                model.resultado = 0;
            }
            //- HDT Fin

            return Json(model);
        }


        /// <summary>
        /// Valida si existen las carpetas y si no existen las crea
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        protected string validarFolder(string periodo) 
        {
            string ruta = "";

            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fecha = new DateTime(ianho, imes, 1);

            FileServer.CreateFolder(Constantes.RutaCargaIntranet, "IntercambioOsinergmin", string.Empty);
            FileServer.CreateFolder(Constantes.RutaCargaIntranet + Constantes.RutaCargaIntercambio, "Remision", string.Empty);
            FileServer.CreateFolder(Constantes.RutaCargaIntranet + Constantes.RutaCargaIntercambio + "Remision/", fecha.Year.ToString(), string.Empty);
            FileServer.CreateFolder(Constantes.RutaCargaIntranet + Constantes.RutaCargaIntercambio + "Remision/" + fecha.Year.ToString() + "/", fecha.Month.ToString("d2"), string.Empty);
            return ruta = Constantes.RutaCargaIntranet + Constantes.RutaCargaIntercambio + "Remision/" + fecha.Year + "/" + fecha.Month.ToString("d2") + "/";

        }

        /// <summary>
        /// Permite mostrar los archivos de un directorio
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Archivos(string periodo)
        {
            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fecha = new DateTime(ianho, imes, 1);

            validarFolder(periodo);

            //Validamos la creación de la carpeta
            string rutaFolder = Constantes.RutaCargaIntranet + Constantes.RutaCargaIntercambio + "Remision/" + fecha.Year + "/" + fecha.Month.ToString("d2") + "/";

            EntidadRemisionModel model = new EntidadRemisionModel();
            List<FileData> list =  FileServer.ListarArhivos(rutaFolder, string.Empty).OrderByDescending(x => x.FileName).ToList();


            List<FileDataCargados> ListaDocumentos = new List<FileDataCargados>();

            //Obtener el Icono del folder
            foreach (var item in list)
            {
                FileDataCargados file = new FileDataCargados();
                file.FileName = item.FileName;
                file.Extension = item.Extension;
                file.FileSize = item.FileSize;
                file.FileUrl = item.FileUrl;
                file.Icono = item.Icono;
                file.FileType = item.FileType;
                file.LastWriteTime = item.LastWriteTime;

                string[] strings = item.FileName.Split('.');
                string[] subStrings = strings[0].Split('_');

                file.idEnvio = Int32.Parse(subStrings[2]);
                file.tabla = subStrings[1];

                ListaDocumentos.Add(file);
            }

            model.ListaDocumentos = ListaDocumentos;

            return PartialView(model);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url)
        {
            Stream stream = FileServer.DownloadToStream(url, string.Empty);

            int indexOf = url.LastIndexOf('/');
            string fileName = url;
            if (indexOf >= 0)
            {
                fileName = url.Substring(indexOf + 1, url.Length - indexOf - 1);
            }
            indexOf = fileName.LastIndexOf('.');
            string extension = fileName.Substring(indexOf + 1, fileName.Length - indexOf - 1);

            return File(stream, extension, fileName);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DownloadIntercambio(string url)
        {

            Stream stream = new MemoryStream();
            string path = url;

            if (System.IO.File.Exists(path))
            {
                stream = System.IO.File.OpenRead(path);
            }

            int indexOf = url.LastIndexOf('/');
            string fileName = url;
            if (indexOf >= 0)
            {
                fileName = url.Substring(indexOf + 1, url.Length - indexOf - 1);
            }
            indexOf = fileName.LastIndexOf('.');
            string extension = fileName.Substring(indexOf + 1, fileName.Length - indexOf - 1);

            return File(stream, extension, fileName);
        }

        /// <summary>
        /// Procesar la tabla de remisión
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tabla"></param>
        /// <param name="tieneError"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemitirRegistro(string periodo, string tabla, ref int tieneError, ref EntidadRemisionModel model)
        {
            //Validamos la creación de la carpeta
            string rutaFolder = validarFolder(periodo);

            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);

            //Guardamos el Envio
            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = 1;
            envio.Enviofecha = DateTime.Now;
            envio.Enviofechaperiodo = fechaProceso;
            envio.Envioplazo = "P";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = DateTime.Now;
            envio.Lastuser = User.Identity.Name;
            envio.Userlogin = User.Identity.Name;
            envio.Formatcodi = 53;//Formato que enviara el COES
            envio.Modcodi = 14;//(int)base.IdModulo
            int idEnvio = remisionAppServicio.SaveMeEnvio(envio);

            string date = fechaProceso.ToString("yyyyMM");

            periodo = date;

            // Obtenemos el Periodo según el Id proporcionado
            var periodoDto = remisionAppServicio.PeriodoGetById(periodo);
            var tab = remisionAppServicio.List(periodoDto.PseinCodi).Where(x => x.RtabCodi.Trim() == tabla).FirstOrDefault();

            string rut = remisionAppServicio.GenerateDataTable(periodo, tab.RtabQuery, ConstantesIntercambioOsinergmin.Delimitador, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga, tab.RtabCodTablaOsig.Trim(), tab.RtabCodi.Trim(), idEnvio);

            string[] substrings = rut.Split('|');
            int val = 0;
            string ruta = "";
            string cont = "";
            foreach (var item in substrings)
            {
                if (val == 0)
                {
                    ruta = item.Trim();
                    val++;
                }
                else
                {
                    cont = item.Trim();
                    val++;
                }
            }
            string empresa = "COES";
            string host = "127.0.0.1";
            string user = "usuariocoes";
            if (!cont.Equals("0"))
            {
                System.Diagnostics.Debug.WriteLine("tabla : " + tabla);

                FileStream fs = System.IO.File.OpenRead(ruta);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();

                riProcesarRemisionDatos ri = new riProcesarRemisionDatos();
                ri.codigoEmpresa = empresa;
                ri.codigoPeriodoRemision = periodo;
                ri.codigoTabla = tab.RtabCodTablaOsig.Trim();
                ri.delimitador = ConstantesIntercambioOsinergmin.Delimitador;
                ri.informacionARemitir = data;
                ri.terminal = host;
                ri.usuario = user;

                roProcesarRemisionDatos ro = service.procesarRemisionDatos(ri);

                string estadoEnvio = "1";
                int cantErrores = 0;
                if (ro.listaErrores != null)
                {
                    estadoEnvio = "0";
                    cantErrores = ro.listaErrores.Count();
                    cont = "0";
                }

                //- HDT Inicio 12/10/2016
                string mensajeResultante = string.Empty;
                if (ro.valorResultante == 0)
                {
                    estadoEnvio = "0";
                    cantErrores = 1;
                    mensajeResultante = ro.mensajeResultante;
                    cont = "0";
                }
                if (ro.listaErrores != null || ro.valorResultante == 0)
                {
                    tieneError = 1;
                }
                else
                {
                    tieneError = 0;
                }
                //- HDT Fin

                IioControlCargaDTO iioControlCargaDTO = new IioControlCargaDTO();
                iioControlCargaDTO.PseinCodi = periodoDto.PseinCodi;
                iioControlCargaDTO.RtabCodi = tab.RtabCodi.Trim();
                iioControlCargaDTO.Enviocodi = idEnvio;
                IioControlCargaDTO controlCarga = remisionAppServicio.ControlCargaGetByCriteria(iioControlCargaDTO);

                int idControl = 0;

                if (controlCarga.RccaCodi == 0)
                {
                    //Creamos el control de carga
                    IioControlCargaDTO iioControlCargaDto = new IioControlCargaDTO();
                    iioControlCargaDto.RccaCodi = 0;
                    iioControlCargaDto.PseinCodi = periodoDto.PseinCodi;
                    iioControlCargaDto.RtabCodi = tab.RtabCodi.Trim();
                    iioControlCargaDto.RccaNroRegistros = Int32.Parse(cont);
                    iioControlCargaDto.RccaEstadoEnvio = estadoEnvio;
                    iioControlCargaDto.RccaEstRegistro = "1";
                    iioControlCargaDto.RccaUsuCreacion = User.Identity.Name;
                    iioControlCargaDto.Enviocodi = idEnvio;
                    idControl = remisionAppServicio.ControlCargaSave(iioControlCargaDto);
                }
                else
                {
                    controlCarga.RccaUsuModificacion = User.Identity.Name;
                    controlCarga.RccaEstadoEnvio = estadoEnvio;
                    controlCarga.RccaNroRegistros = Int32.Parse(cont);
                    controlCarga.Enviocodi = idEnvio;
                    idControl = remisionAppServicio.ControlCargaSave(controlCarga);                    
                }

                IioLogRemisionDTO scoLogRemisionDto = new IioLogRemisionDTO();
                //Si existen errores Guardamos los errores en la tabla de Logs

                if (cantErrores > 0)
                {
                    
                    if (ro.listaErrores != null)
                    {
                        foreach (var item in ro.listaErrores)
                        {
                            scoLogRemisionDto = new IioLogRemisionDTO();
                            scoLogRemisionDto.RccaCodi = idControl;
                            scoLogRemisionDto.RlogCodi = 0;
                            scoLogRemisionDto.RlogNroLinea = item.nroError;
                            scoLogRemisionDto.RlogDescripcionError = item.mensajeError;
                            scoLogRemisionDto.Enviocodi = idEnvio;
                            remisionAppServicio.SaveLogRemision(scoLogRemisionDto);
                        }
                    }

                    if (mensajeResultante != string.Empty)
                    {
                        scoLogRemisionDto = new IioLogRemisionDTO();
                        scoLogRemisionDto.RccaCodi = idControl;
                        scoLogRemisionDto.RlogCodi = 0;
                        scoLogRemisionDto.RlogNroLinea = 1;
                        scoLogRemisionDto.RlogDescripcionError = mensajeResultante;
                        scoLogRemisionDto.Enviocodi = idEnvio;
                        remisionAppServicio.SaveLogRemision(scoLogRemisionDto);
                    }

                    model.mensaje = "Se remitió la información pero se encontraron " + cantErrores + " errores.";
                    model.resultado = 1;
                    
                    String url = Constantes.RutaCargaIntranet + Constantes.RutaCargaIntercambio + "Remision/" + fechaProceso.Year + "/" + fechaProceso.Month.ToString("d2") + "/";
                    String lastName = tab.RtabCodi.Trim() + "_" + idEnvio + ".zip";
                    String newName = tab.RtabNombreTabla.Trim() + "_" + tab.RtabCodi.Trim() + "_" + idEnvio + "_BAD.zip";

                    //Movemos los archivos al repositorio del COES  
                    FileServer.UploadFromFileDirectory(ruta, url, newName, string.Empty);
                }
                else
                {
                    model.mensaje = "Envío exitoso.";
                    model.resultado = 0;

                    String url = Constantes.RutaCargaIntranet + Constantes.RutaCargaIntercambio + "Remision/" + fechaProceso.Year + "/" + fechaProceso.Month.ToString("d2") + "/";
                    String lastName = tab.RtabCodi.Trim() + "_" + idEnvio + ".zip";
                    String newName = tab.RtabNombreTabla.Trim() + "_" + tab.RtabCodi.Trim() + "_" + idEnvio + "_OK.zip";

                    //Movemos los archivos al repositorio del COES  
                    FileServer.UploadFromFileDirectory(ruta, url, newName, string.Empty);
                }

                String codMensaje = ro.codigoMensaje;
                String mensaje = ro.mensajeResultante;

                IioPeriodoSeinDTO periodoSeinDto = remisionAppServicio.PeriodoGetById(periodo);

                DateTime fecPrimerEnvio = DateTime.Now;

                //Verificamos si es la primera carga
                if (periodoSeinDto.PseinFecPriEnvio != DateTime.MinValue)
                {
                    fecPrimerEnvio = periodoSeinDto.PseinFecPriEnvio;
                }

                remisionAppServicio.PeriodoUpdate(new IioPeriodoSeinDTO
                {
                    PseinCodi = periodoSeinDto.PseinCodi,
                    PseinAnioMesPerrem = periodo,
                    PseinFecPriEnvio = fecPrimerEnvio,
                    PseinFecUltEnvio = DateTime.Now,
                    PseinEstRegistro = "1",
                    PseinConfirmado = "1",
                    PseinEstado = "1",
                    PseinUsuModificacion = UserName
                });
            }
            else
            {
                System.IO.File.Delete(ruta);

                String url = Constantes.RutaCargaIntranet + Constantes.RutaCargaIntercambio + "Remision/" + fechaProceso.Year + "/" + fechaProceso.Month.ToString("d2");
                String lastName = tab.RtabCodi.Trim() + "_" + idEnvio + ".zip";

                FileServer.DeleteBlob(url + lastName, string.Empty);

                model.mensaje = "No se encontraron registros en la tabla '" + tab.RtabDescripcionTabla + "'";
                model.resultado = 1;

                IioControlCargaDTO iioControlCargaDTO = new IioControlCargaDTO();
                iioControlCargaDTO.PseinCodi = periodoDto.PseinCodi;
                iioControlCargaDTO.RtabCodi = tab.RtabCodi.Trim();
                IioControlCargaDTO controlCarga = remisionAppServicio.ControlCargaGetByCriteria(iioControlCargaDTO);

                int idControl = 0;

                if (controlCarga.RccaCodi == 0)
                {
                    //Creamos el control de carga
                    IioControlCargaDTO iioControlCargaDto = new IioControlCargaDTO();
                    iioControlCargaDto.RccaCodi = 0;
                    iioControlCargaDto.PseinCodi = periodoDto.PseinCodi;
                    iioControlCargaDto.RtabCodi = tab.RtabCodi.Trim();
                    iioControlCargaDto.RccaNroRegistros = Int32.Parse(cont);
                    iioControlCargaDto.RccaEstadoEnvio = "2";
                    iioControlCargaDto.RccaEstRegistro = "1";
                    iioControlCargaDto.RccaUsuCreacion = user;
                    idControl = remisionAppServicio.ControlCargaSave(iioControlCargaDto);
                }
                else
                {
                    controlCarga.RccaUsuModificacion = user;
                    controlCarga.RccaEstadoEnvio = "2";
                    controlCarga.RccaNroRegistros = Int32.Parse(cont);
                    idControl = remisionAppServicio.ControlCargaSave(controlCarga);
                    //Eliminamos los logs anteriores si es que se actualiza el control
                    remisionAppServicio.ControlCargaDelete(idControl);
                }
            }

            System.IO.File.Delete(ruta);

            return Json(model);
        }

        /// <summary>
        /// Procesar la tabla de remisión
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tabla"></param>
        /// <param name="tieneError"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemitirRegistroIndividual(string periodo, string tabla)
        {
            int tieneErrores = 0;
            EntidadRemisionModel model = new EntidadRemisionModel();
            this.RemitirRegistro(periodo, tabla, ref tieneErrores, ref model);            

            return Json(model);
        }
     
        /// <summary>
        /// Permite generar el archivo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string periodo, string tabla, int idEnvio)
        {
            try
            {
                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3));
                DateTime fechaProceso = new DateTime(ianho, imes, 1);
                string date = fechaProceso.ToString("yyyyMM");
                // Obtenemos el Periodo según el Id proporcionado
                var periodoDto = remisionAppServicio.PeriodoGetById(date);
                var tab = remisionAppServicio.List(periodoDto.PseinCodi).Where(x => x.RtabCodi.Trim() == tabla).FirstOrDefault();
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                int formato = 1;//Formato excel
                string file = remisionAppServicio.GenerarFormato(formato, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga, pathLogo, periodoDto.PseinCodi, tab.RtabCodi.Trim(), idEnvio);
                return Json(file);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Descarga el formato
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int formato, string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, file);
        }


        /// <summary>
        /// Obtener el archivo para descargar
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tabla"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarArchivo(string periodo, string tabla, string tipo)
        {
            try
            {
                if (tipo == null) {
                    tipo = "x";
                }
                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3));
                DateTime fechaProceso = new DateTime(ianho, imes, 1);
                string date = fechaProceso.ToString("yyyyMM");
                // Obtenemos el Periodo según el Id proporcionado
                var periodoDto = remisionAppServicio.PeriodoGetById(date);
                var tab = remisionAppServicio.List(periodoDto.PseinCodi).Where(x => x.RtabCodi.Trim() == tabla).FirstOrDefault();
                string file = remisionAppServicio.GenerateDataReader(date, tab.RtabQuery, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga, tab.RtabCodi.Trim(), tipo);
                return Json(file);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Descargar el archivo
        /// </summary>
        /// <param name="file"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivo(string file, string tipo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + file;
            string app = Constantes.AppCSV;
            return File(path, app, file);
        }

    }
}
