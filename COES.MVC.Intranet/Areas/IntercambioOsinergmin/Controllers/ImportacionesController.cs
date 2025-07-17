using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.IntercambioOsinergmin;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using COES.MVC.Intranet.Areas.IntercambioOsinergmin.Helper;
using COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Importacion;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using System.IO;
using System.Text;
using log4net;
using System.Configuration;
using System.Reflection;
using System.Web.Script.Serialization;
using FormatoHelperOsinergmin = COES.MVC.Intranet.Areas.IntercambioOsinergmin.Helper.FormatoHelper;

using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;


namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Controllers
{
    public class ImportacionesController : BaseController
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

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ImportacionAppServicio importacionAppServicio;

        //- alpha.HDT - 23/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Listado de barras.
        /// </summary>
        private List<barraDTO> lbarraDTO = null;

        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        ImportacionSicliEndpointService service = null;

        #region Propiedades
        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreArchivo
        {
            get
            {
                return (Session[DatosSesionIntercambioOsinergmin.SesionNombreArchivo] != null) ?
                    Session[DatosSesionIntercambioOsinergmin.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionIntercambioOsinergmin.SesionNombreArchivo] = value; }
        }
        public String RutaCompletaArchivo
        {
            get
            {
                return (Session[DatosSesionIntercambioOsinergmin.RutaCompletaArchivo] != null) ?
                    Session[DatosSesionIntercambioOsinergmin.RutaCompletaArchivo].ToString() : null;
            }
            set { Session[DatosSesionIntercambioOsinergmin.RutaCompletaArchivo] = value; }
        }

        #endregion

        public ImportacionesController()
        {
            log4net.Config.XmlConfigurator.Configure();
            importacionAppServicio = new ImportacionAppServicio();
            //- alpha.HDT - Inicio 09/07/2017: Cambio para atender el requerimiento.
            string url = ConfigurationManager.AppSettings["UrlImportacionSicli"].ToString();
            if (string.IsNullOrEmpty(url))
            {
                throw new ApplicationException("No se ha encontrado la definición de la dirección URL del servicio de importación de datos Sicli");
            }
            service = new ImportacionSicliEndpointService(url);
            //- HDT Fin
            //- alpha.HDT - Inicio 02/11/2017: Cambio para atender el requerimiento.
            service.Timeout = 1800 * 1000;  //- Media hora.
            //- HDT Fin
        }

        public int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaAlphaPR16"]);

        //- alpha.HDT - Inicio 09/07/2017: : Cambio para atender el requerimiento.
        //- Remover en proxima revision: SI
        //ImportacionSicliEndpointService service = new ImportacionSicliEndpointService();
        //- HDT Fin

        /// <summary>
        /// Clase Inicial
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public ActionResult Index(int anio = 0)
        {
            //busqueda de modelo usando año
            var anios = importacionAppServicio.PeriodoListAnios();
            var anioFormateado = (anio == 0 ? DateTime.Today.Year : anio).ToString();
            var modelo = new FiltroListadoPeriodosImportacionModel(anios, anioFormateado);

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
            //var periodos = importacionAppServicio.PeriodoGetByCriteria(anio);

            List<IioPeriodoSicliDTO> lstPeriodos = importacionAppServicio.PeriodoGetByCriteria(anio);

            List<IioPeriodoSicliDTO> lista = new List<IioPeriodoSicliDTO>();

            foreach (var item in lstPeriodos)
            {
                riConsultarUltFecActualizacion c = new riConsultarUltFecActualizacion
                {
                    codigoPeriodoRemision = item.PsicliAnioMesPerrem.Trim(),
                    codigoProcedimientoRemision = ConstantesIntercambioOsinergmin.ProcedimientoSicli
                };

                var resul = service.consultarUltFecActualizacion(c);
                DateTime fecUltActOsi = resul.fechaUltimaActualizacion;

                item.PsicliFecUltActOsi = fecUltActOsi;

                lista.Add(item);
            }

            var periodos = lista;

            return PartialView("ListarPeriodos", periodos.Select(PeriodoImportacionModel.Create).ToList());
        }

        /// <summary>
        /// Obtiene la vista de creación de un periodo
        /// </summary>
        /// <returns>HTML con el Partial view</returns>
        [HttpGet]
        public ActionResult Create()
        {
            var modelo = new DetallePeriodoImportacionModel
            {
                PeriodoImportacionModel = new PeriodoImportacionModel
                {
                    //PrimerEnvioDate = DateTime.MinValue,
                    //UltimoEnvioDate = DateTime.MinValue
                }
            };
            return View("Create", modelo);
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
            var periodoDto = importacionAppServicio.PeriodoGetById(new IioPeriodoSicliDTO { PsicliAnioMesPerrem = per });

            riConsultarUltFecActualizacion c = new riConsultarUltFecActualizacion
            {
                codigoPeriodoRemision = per.Trim(),
                codigoProcedimientoRemision = ConstantesIntercambioOsinergmin.ProcedimientoSicli
            };
            var resul = service.consultarUltFecActualizacion(c);
            periodoDto.PsicliFecUltActOsi = resul.fechaUltimaActualizacion;

            var periodos = importacionAppServicio.PeriodoGetByCriteria(periodo.Substring(0, 4));
            var modelo = new DetallePeriodoImportacionModel(PeriodoImportacionModel.Create(periodoDto), periodos.Select(dto => PeriodoImportacionModel.Create(dto).Periodo).ToList());

            //Agregado para validar la seleccion del dia
            var fechaFin = fech.AddMonths(1).AddDays(-1);
            ViewBag.fechaInicio = fech.ToString("dd/MM/yyyy");
            ViewBag.fechaFin = fechaFin.ToString("dd/MM/yyyy");
            //Consultar empresas remitentes con los campos actualizados
            riConsultarEmpresasRemitentes e = new riConsultarEmpresasRemitentes
            {
                codigoPeriodoRemision = per,
                codigoProcedimientoRemision = ConstantesIntercambioOsinergmin.ProcedimientoSicli
            };

            if (service.consultarEmpresasRemitentes(e).listaNuevasRemisiones == null)
            {
                return View("Edit", modelo);
            }

            int cant = service.consultarEmpresasRemitentes(e).listaNuevasRemisiones.Count();

            int cantRegOsinergmin = cant / 5 * 2;

            int cantRegCoes = importacionAppServicio.CantidadRegistrosImportacion(periodoDto.PsicliCodi);

            if (cantRegOsinergmin > cantRegCoes)
            {
                int id = importacionAppServicio.GetMaxIdControlImportacion();

                List<IioControlImportacionDTO> lista = new List<IioControlImportacionDTO>();
                foreach (var item in service.consultarEmpresasRemitentes(e).listaNuevasRemisiones)
                {
                    if (item.codigoTabla.Trim().Equals(ConstantesIntercambioOsinergmin.TablaSicli04) 
                        || item.codigoTabla.Trim().Equals(ConstantesIntercambioOsinergmin.TablaSicli05)
                        || item.codigoTabla.Trim().Equals(ConstantesIntercambioOsinergmin.TablaSicli02))
                    {
                        IioControlImportacionDTO entity = new IioControlImportacionDTO();
                        entity.Rcimcodi = id;
                        entity.Psiclicodi = periodoDto.PsicliCodi;
                        entity.Rcimempresa = item.codigoEmpresa.Trim();
                        entity.Rtabcodi = item.codigoTabla.Trim();
                        entity.Rcimnroregistros = item.nroRegistros;
                        entity.Rcimestregistro = "1";
                        entity.Rcimusucreacion = User.Identity.Name;
                        entity.Rcimfeccreacion = DateTime.Now;
                        entity.Rcimempresadesc = item.nombreEmpresa.Trim();
                        lista.Add(entity);
                        id++;
                    }
                }
                //Grabo la importación
                importacionAppServicio.GrabarMasivoControlImportacion(lista);
            }
            return View("Edit", modelo);
        }


        /// <summary>
        /// Lista las empresas de la Tabla04
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaControlTabla04(string periodo)
        {
            EntidadImportacionModel model = new EntidadImportacionModel();

            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string date = fechaProceso.ToString("yyyyMM");

            // Obtenemos el Periodo según el Id proporcionado
            var periodoDto = importacionAppServicio.PeriodoGetById(new IioPeriodoSicliDTO { PsicliAnioMesPerrem = date });

            model.ListaControlTabla04 = importacionAppServicio.ListByTabla(periodoDto.PsicliCodi, ConstantesIntercambioOsinergmin.TablaSicli04);

            return View(model);
        }

        /// <summary>
        /// Lista las empresas de la Tabla04
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaControlTabla05(string periodo)
        {
            EntidadImportacionModel model = new EntidadImportacionModel();

            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string date = fechaProceso.ToString("yyyyMM");

            // Obtenemos el Periodo según el Id proporcionado
            var periodoDto = importacionAppServicio.PeriodoGetById(new IioPeriodoSicliDTO { PsicliAnioMesPerrem = date });

            model.ListaControlTabla05 = importacionAppServicio.ListByTabla(periodoDto.PsicliCodi, ConstantesIntercambioOsinergmin.TablaSicli05);

            return View(model);
        }

        /// <summary>
        /// Lista las empresas de la Tabla02
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaControlTabla02(string periodo)
        {
            EntidadImportacionModel model = new EntidadImportacionModel();

            int imes = Int32.Parse(periodo.Substring(0, 2));
            int ianho = Int32.Parse(periodo.Substring(3));
            DateTime fechaProceso = new DateTime(ianho, imes, 1);
            string date = fechaProceso.ToString("yyyyMM");

            // Obtenemos el Periodo según el Id proporcionado
            var periodoDto = importacionAppServicio.PeriodoGetById(new IioPeriodoSicliDTO { PsicliAnioMesPerrem = date });

            model.ListaControlTabla02 = importacionAppServicio.ListByTabla(periodoDto.PsicliCodi, ConstantesIntercambioOsinergmin.TablaSicli02);

            return View(model);
        }

        /// <summary>
        /// Validar periodo nuevo sicli
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarPeriodo(string periodo)
        {
            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fech = new DateTime(anho, mes, 1);
            string per = fech.ToString("yyyyMM");

            var modelo = importacionAppServicio.PeriodoGetById(new IioPeriodoSicliDTO { PsicliAnioMesPerrem = per });

            if (fech > DateTime.Today)
            {
                return Json(2);
            }

            return Json(modelo == null ? 1 : 0);
        }

        /// <summary>
        /// Crear periodo nuevo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CrearPeriodo(string periodo)
        {
            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fech = new DateTime(anho, mes, 1);
            string per = fech.ToString("yyyyMM");

            importacionAppServicio.PeriodoSave(new IioPeriodoSicliDTO
            {
                PsicliAnioMesPerrem = per,
                PsicliEstado = "1",
                PsicliEstRegistro = "1",
                PsicliUsuCreacion = UserName
            });

            return Json(1);
        }


        //- alpha.HDT - 02/04/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Proceso para Importar los registros del SICLI
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tabla"></param>
        /// <param name="empresasIn"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarRegistros(string periodo, string tabla, string empresasIn)
        {
            EntidadImportacionModel model = new EntidadImportacionModel();

            //- HDT.Alpha Inicio 19/10/2017. Permite orientar al usuario sobre qué hacer de presentarse la excepción no controlada.
            try
            {
                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3));
                DateTime fechaProceso = new DateTime(ianho, imes, 1);
                string per = fechaProceso.ToString("yyyyMM");

                string[] empresas = empresasIn.Split(',');

                bool ocurrioUnProblema = false;
                List<IioLogImportacionIncidenteDTO> lIioLogImportacionIncidenteDTO = null;

                IioPeriodoSicliDTO iioPeriodoSicliDTO = importacionAppServicio.PeriodoGetById(new IioPeriodoSicliDTO { PsicliAnioMesPerrem = per });

                List<string> lstMensajes = new List<string>();

                //Validacion de Tab01
                var empresasConsulta = string.Empty;
                foreach (string emp in empresas)
                {
                    empresasConsulta = empresasConsulta + "'" + emp.Trim() + "',";
                }
                if (!tabla.Equals(ConstantesIntercambioOsinergmin.TablaSicli05))
                {
                    var resultado = importacionAppServicio.ValidarOsigFacturaTablaEmpresas(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','));
                    if (resultado > 0)
                    {
                        model.lstMensajes = lstMensajes;
                        model.mensaje = "Existe información incompleta en la sección de Facturación. Revisar.";
                        model.resultado = 0;
                        model.empresasPendientes = 1;
                        return Json(model);
                    }
                }
               

                //- Antes de iniciar el proceso se debe validar que no existan duplicados en la configuración del COES.            
                lIioLogImportacionIncidenteDTO = importacionAppServicio.GetDuplicadosConfiguracionCOES();
                if (lIioLogImportacionIncidenteDTO.Count > 0)
                {
                    //- Este error no es necesario registrarlo en el tabla log de importación.
                    //- Se configura el mensaje a retornar.
                    model.lstMensajes = new List<string>();
                    foreach (IioLogImportacionIncidenteDTO oIioLogImportacionIncidenteDTO in lIioLogImportacionIncidenteDTO)
                    {
                        model.lstMensajes.Add(oIioLogImportacionIncidenteDTO.Mensaje);
                    }

                    model.mensaje = "Se detectaron problemas en la validación de duplicados en la configuración del COES. Contáctese con el Administrador.";
                    model.resultado = 0;

                    return Json(model);
                }

                //Obtener tablas para la remisión total
                riConsultarEmpresasRemitentes e = new riConsultarEmpresasRemitentes
                {
                    codigoPeriodoRemision = per,
                    codigoProcedimientoRemision = ConstantesIntercambioOsinergmin.ProcedimientoSicli,
                    fechaReferente = DateTime.Now
                };

                String empresa = "";
                String mensaje = "";

                
                var match = "";
                
                IioControlImportacionDTO control = null;


                var respuestaOM = service.consultarEmpresasRemitentes(e); //
                List<IioControlImportacionDTO> list = new List<IioControlImportacionDTO>();
                foreach (var item in respuestaOM.listaNuevasRemisiones)
                {
                    if (item.codigoTabla.Trim().Equals(tabla))
                    {
                        try
                        {
                            empresa = item.codigoEmpresa.Trim();

                            if (!empresas.Contains(empresa))
                            {
                                continue;
                            }

                            //- Obtener el control de importación
                            control = importacionAppServicio.GetByEmpresaTabla(iioPeriodoSicliDTO.PsicliCodi, tabla, empresa);
                            importacionAppServicio.EliminarIncidenciasImportacionSicli(control.Rcimcodi, periodo.Replace(" ", ""));

                            riDescargarInformacionRemitida c = new riDescargarInformacionRemitida
                            {
                                codigoPeriodoRemision = per,
                                codigoProcedimientoRemision = ConstantesIntercambioOsinergmin.ProcedimientoSicli,
                                codigoTabla = tabla.Trim(),
                                codigosEmpresas = empresa.Trim()
                            };

                            //- alpha.HDT - Inicio 02/11/2017: Cambio para atender el requerimiento.
                            //var resul = service.descargarInformacionRemitida(c);
                            //byte[] data = resul.archivoInformacionRemitida;
                            byte[] data = null;
                            try
                            {
                                service.Timeout = 100000000;
                                var resul = service.descargarInformacionRemitida(c);
                                data = resul.archivoInformacionRemitida;
                            }
                            catch (System.Net.WebException exWeb)
                            {
                                if (exWeb.Status == System.Net.WebExceptionStatus.Timeout)
                                {
                                    throw new ApplicationException("El servicio de importación de información desde el Osinergmin está tomando más tiempo del esperado");
                                }
                                else
                                {
                                    throw exWeb;
                                }
                            }
                            //- HDT Fin

                            string linea = "";
                            int cont = 0;

                            if (tabla.Equals(ConstantesIntercambioOsinergmin.TablaSicli04)) //TABLA 04 - SICLI
                            {
                                //Eliminamos los registros en la tabla - Consultar
                                //importacionAppServicio.OsigConsumoDelete(iioPeriodoSicliDTO.PsicliCodi, empresa);

                                //Eliminamos los registros en la tabla
                                importacionAppServicio.TmpConsumoDelete();

                                System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataConsumo.txt", data);
                                var fechaCreacion = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                var usuarioCreacion = User.Identity.Name;

                                //- Construimos los registro y los insertamos de forma masiva
                                //List<IioOsigConsumoUlDTO> lista = new List<IioOsigConsumoUlDTO>();
                                //IioOsigConsumoUlDTO entity = new IioOsigConsumoUlDTO();
                                List<IioTmpConsumoDTO> lista = new List<IioTmpConsumoDTO>();
                                IioTmpConsumoDTO entity = new IioTmpConsumoDTO();
                                using (var f = System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataConsumo.txt"))
                                {
                                    using (var reader = new StreamReader(f, Encoding.UTF8))
                                    {
                                        while (!reader.EndOfStream)
                                        {
                                            linea = reader.ReadLine();
                                            if (cont > 0)
                                            {
                                                Char delimiter = '\t';
                                                String[] dat = linea.Split(delimiter);

                                                //entity = new IioOsigConsumoUlDTO();
                                                //entity.Psiclicodi = iioPeriodoSicliDTO.PsicliCodi;
                                                //entity.Ulconcodempresa = dat[0];
                                                //entity.Ulconcodsuministro = dat[1];
                                                //entity.Ulconcodbarra = dat[2];
                                                //entity.Ulconfecha = DateTime.ParseExact(dat[3], "yyyyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture);

                                                //if (dat[4] == null || dat[4] == "null")
                                                //{
                                                //    entity.Ulconenergactv = 0;
                                                //}
                                                //else
                                                //{
                                                //    entity.Ulconenergactv = decimal.Parse(dat[4]);
                                                //}

                                                //if (dat[5] == null || dat[5] == "null")
                                                //{
                                                //    entity.Ulconenergreac = 0;
                                                //}
                                                //else
                                                //{
                                                //    entity.Ulconenergreac = decimal.Parse(dat[5]);
                                                //}

                                                //entity.Ulconfeccreacion = fechaCreacion;
                                                //entity.Ulconusucreacion = usuarioCreacion;

                                                entity = new IioTmpConsumoDTO();
                                                entity.Psiclicodi = iioPeriodoSicliDTO.PsicliCodi;
                                                entity.Uconempcodi = dat[0];
                                                entity.Sumucodi = dat[1];
                                                entity.Uconptosumincodi = dat[2];
                                                entity.Uconfecha = DateTime.ParseExact(dat[3], "yyyyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture);

                                                if (dat[4] == null || dat[4] == "null")
                                                {
                                                    entity.Uconenergactv = 0;
                                                }
                                                else
                                                {
                                                    entity.Uconenergactv = decimal.Parse(dat[4]);
                                                }

                                                if (dat[5] == null || dat[5] == "null")
                                                {
                                                    entity.Uconenergreac = 0;
                                                }
                                                else
                                                {
                                                    entity.Uconenergreac = decimal.Parse(dat[5]);
                                                }

                                                lista.Add(entity);
                                            }
                                            cont++;
                                        }
                                    }
                                }

                                //Grabamos a la tabla temporal
                                importacionAppServicio.GrabarMasivoTmpConsumo(lista);
                                
                                //Eliminamos y grabamos los registros en la tabla final de consumo
                                importacionAppServicio.OsigConsumoDelete(iioPeriodoSicliDTO.PsicliCodi, empresa);
                                //importacionAppServicio.GrabarMasivoOsigConsumo(lista);
                                importacionAppServicio.SaveOsigConsumo(usuarioCreacion);


                                #region Proceso Antiguo Lectura TMP_CLI_TABLA04
                                //Eliminamos los registros en la tabla
                                //importacionAppServicio.TmpConsumoDelete();

                                //System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataConsumo.txt", data);

                                ////- Construimos los registro y los insertamos de forma masiva
                                //List<IioTmpConsumoDTO> lista = new List<IioTmpConsumoDTO>();
                                //IioTmpConsumoDTO entity = new IioTmpConsumoDTO();
                                //using (var f = System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataConsumo.txt"))
                                //{
                                //    using (var reader = new StreamReader(f, Encoding.UTF8))
                                //    {
                                //        while (!reader.EndOfStream)
                                //        {
                                //            linea = reader.ReadLine();
                                //            if (cont > 0)
                                //            {
                                //                Char delimiter = '\t';
                                //                String[] dat = linea.Split(delimiter);

                                //                entity = new IioTmpConsumoDTO();
                                //                entity.Psiclicodi = iioPeriodoSicliDTO.PsicliCodi;
                                //                entity.Uconempcodi = dat[0];
                                //                entity.Sumucodi = dat[1];
                                //                entity.Uconptosumincodi = dat[2];
                                //                entity.Uconfecha = DateTime.ParseExact(dat[3], "yyyyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture);

                                //                if (dat[4] == null || dat[4] == "null")
                                //                {
                                //                    entity.Uconenergactv = 0;
                                //                }
                                //                else
                                //                {
                                //                    entity.Uconenergactv = decimal.Parse(dat[4]);
                                //                }

                                //                if (dat[5] == null || dat[5] == "null")
                                //                {
                                //                    entity.Uconenergreac = 0;
                                //                }
                                //                else
                                //                {
                                //                    entity.Uconenergreac = decimal.Parse(dat[5]);
                                //                }

                                //                lista.Add(entity);
                                //            }
                                //            cont++;
                                //        }
                                //    }
                                //}
                                //importacionAppServicio.GrabarMasivoTmpConsumo(lista);

                                //int cantidadRegistrosImportados = 0;

                                //importacionAppServicio.UpdatePtoMediCodiTmpConsumo();

                                //- De existir registros en la tabla IIO_TMP_CONSUMO con PTOMEDICODI IS NULL no se debe permitir realizar la 
                                //- importación de la información del SICLI.
                                //lIioLogImportacionIncidenteDTO = importacionAppServicio.GetIncidentesSinPuntoMedicionCOES();
                                //if (lIioLogImportacionIncidenteDTO.Count > 0)
                                //{
                                //    ocurrioUnProblema = true;
                                //    //- Entonces tenemos un problema: se debe registrar en la tabla log de la importación.
                                //    this.RegistrarIncidenciasImportacionSicli(control.Rcimcodi
                                //        , lIioLogImportacionIncidenteDTO
                                //        , periodo
                                //        , empresa);
                                //    cantidadRegistrosImportados = 0;
                                //}
                                //else
                                //{
                                //    //Migramos la data de la tabla TMP_CONSUMO to ME_MEDICION96
                                //    String rpta = importacionAppServicio.MigrateMeMedicion96(IdLectura, ConstantesIntercambioOsinergmin.tipoInfoCodi, per);

                                //    if (!rpta.Equals("OK"))
                                //    {
                                //        match = lstMensajes.FirstOrDefault(stringToCheck => stringToCheck.Contains(rpta));

                                //        if (match == null)
                                //        {
                                //            lstMensajes.Add(rpta);
                                //            ocurrioUnProblema = true;
                                //            this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                //                , periodo
                                //                , empresa
                                //                , rpta
                                //                , "TMP_CONSUMO"
                                //                , string.Empty
                                //                , EnuTipoIncidenciaImportaSicli.Ninguno);
                                //        }
                                //    }

                                //    //Actualizamos el periodo
                                //    iioPeriodoSicliDTO.PsicliFecUltActCoes = DateTime.Now;
                                //    importacionAppServicio.PeriodoUpdate(iioPeriodoSicliDTO);

                                //    cantidadRegistrosImportados = lista.Count();
                                //}

                                //control.Rcimnroregistroscoes = cantidadRegistrosImportados;
                                //control.Rcimnroregistros = item.nroRegistros;
                                //control.Rcimfechorimportacion = DateTime.Now;
                                //control.Rcimusumodificacion = User.Identity.Name;
                                //control.Rcimfecmodificacion = DateTime.Now;

                                //if (item.nroRegistros != cantidadRegistrosImportados)
                                //{
                                //    control.Rcimestadoimportacion = "0";
                                //}
                                //else
                                //{
                                //    control.Rcimestadoimportacion = "1" + "";
                                //}

                                ////Actualizamos el control de importación
                                //importacionAppServicio.ControlImportacionSave(control);


                                #endregion
                            }
                            else if (tabla.Equals(ConstantesIntercambioOsinergmin.TablaSicli05))    //TMP_CLI_TABLA05
                            {
                                System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataFactura.txt", data);

                                //Eliminamos los registros en la tabla
                                //SiEmpresaDTO empresaDto = importacionAppServicio.GetSiEmpresaByCodOsinergmin(empresa.Trim());

                                importacionAppServicio.OsigFacturaDelete(iioPeriodoSicliDTO.PsicliCodi, empresa.Trim());

                                //if (empresaDto == null)                               
                                //{
                                //    ocurrioUnProblema = true;
                                //    this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                //        , periodo
                                //        , empresa
                                //        , "No se ha encontrado la empresa con código '" + empresa.Trim() + "'"
                                //        , "SI_EMPRESA"
                                //        , empresa.Trim()
                                //        , EnuTipoIncidenciaImportaSicli.EmpresaNoExiste);
                                //}

                                //Construimos los registro y los insertamos de forma masiva
                                List<IioOsigFacturaUlDTO> lista = new List<IioOsigFacturaUlDTO>();
                                IioOsigFacturaUlDTO entity = new IioOsigFacturaUlDTO();
                                int cantError = 0;
                                var fechaCreacion = DateTime.Now;
                                var usuarioCreacion = User.Identity.Name;
                                
                                using (var f = System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataFactura.txt"))
                                {
                                    using (var reader = new StreamReader(f, Encoding.UTF8))
                                    {
                                        cantError = 0;
                                        while (!reader.EndOfStream)
                                        {
                                            linea = reader.ReadLine();
                                            if (cont > 0)
                                            {
                                                Char delimiter = '\t';
                                                String[] dat = linea.Split(delimiter);

                                                entity = new IioOsigFacturaUlDTO();
                                                entity.Psiclicodi = iioPeriodoSicliDTO.PsicliCodi;
                                                entity.Ulfactmesfacturado = dat[1].Trim();
                                                entity.Ulfactcodsuministro = dat[2].Trim();
                                                entity.Ulfactcodbrg = dat[3].Trim();
                                                //3 - Código de la Barra de Referencia de Generación - UFACCODBRG
                                                PrBarraDTO prBarraDTO = importacionAppServicio.GetPrBarraByCodOsinergmin(dat[3].Trim());
                                                if (prBarraDTO != null)
                                                {
                                                    entity.Ulfactbarrcodibrg = prBarraDTO.Barrcodi;
                                                }
                                                else
                                                {
                                                    String barrCodiOsinergmin = dat[3].Trim();

                                                    barraDTO barraDTO = this.obtenerBarra(barrCodiOsinergmin);
                                                    if (barraDTO != null)
                                                    {
                                                        prBarraDTO = new PrBarraDTO();
                                                        prBarraDTO.Barrnombre = barraDTO.nomBarra.ToUpper();
                                                        prBarraDTO.Barrtension = barraDTO.tension.ToString();
                                                        prBarraDTO.Barrusername = User.Identity.Name;
                                                        prBarraDTO.Barrestado = ConstantesIntercambio.BarraCOESEstadoActivo;
                                                        prBarraDTO.Osinergcodi = barrCodiOsinergmin;

                                                        //- Valores por defecto.
                                                        prBarraDTO.Barrflagbarratransferencia = "NO";
                                                        prBarraDTO.Barrflagdesbalance = "NO";
                                                        prBarraDTO.Barrfecins = DateTime.Now;
                                                        prBarraDTO.Barrfecact = DateTime.Now;
                                                        prBarraDTO.Barrbarratransf = 0;
                                                        prBarraDTO.Barrflagbarracompensa = 0;

                                                        int barrCodi = importacionAppServicio.InsertarBarra(prBarraDTO);
                                                        entity.Ulfactbarrcodibrg = barrCodi;
                                                    }
                                                    else
                                                    {
                                                        //ocurrioUnProblema = true;

                                                        mensaje = "Empresa " + empresa.Trim() + ": no encontró la información de la barra  " + dat[3].Trim();
                                                        model.resultado = 0;

                                                        this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                                            , periodo
                                                            , empresa
                                                            , mensaje
                                                            , "PR_BARRA"
                                                            , string.Empty
                                                            , EnuTipoIncidenciaImportaSicli.BarraNoExiste);

                                                        match = lstMensajes.FirstOrDefault(stringToCheck => stringToCheck.Contains(mensaje));

                                                        if (match == null)
                                                        {
                                                            lstMensajes.Add(mensaje);
                                                        }
                                                        cantError++;
                                                    }
                                                }

                                                //4 - Código del Punto de Suministro - UFACCODPUNTOSUMINISTRO
                                                entity.Ulfactcodpuntosuministro = dat[4].Trim();
                                                PrBarraDTO bar2 = importacionAppServicio.GetPrBarraByCodOsinergmin(dat[4].Trim());
                                                if (bar2 != null)
                                                {
                                                    entity.Ulfactbarrcodiptosumin = bar2.Barrcodi;
                                                }
                                                else
                                                {
                                                    String barrCodiOsinergmin2 = dat[4].Trim();

                                                    barraDTO barraDTO = this.obtenerBarra(barrCodiOsinergmin2);
                                                    if (barraDTO != null)
                                                    {
                                                        bar2 = new PrBarraDTO();
                                                        bar2.Barrnombre = barraDTO.nomBarra.ToUpper();
                                                        bar2.Barrtension = barraDTO.tension.ToString();
                                                        bar2.Barrusername = User.Identity.Name;
                                                        bar2.Barrestado = ConstantesIntercambio.BarraCOESEstadoActivo;
                                                        bar2.Osinergcodi = barrCodiOsinergmin2;

                                                        //- Valores por defecto.
                                                        bar2.Barrflagbarratransferencia = "NO";
                                                        bar2.Barrflagdesbalance = "NO";
                                                        bar2.Barrfecins = DateTime.Now;
                                                        bar2.Barrfecact = DateTime.Now;
                                                        bar2.Barrbarratransf = 0;
                                                        bar2.Barrflagbarracompensa = 0;

                                                        int barrCodi2 = importacionAppServicio.InsertarBarra(bar2);
                                                        entity.Ulfactbarrcodiptosumin = barrCodi2;
                                                    }
                                                    else
                                                    {
                                                        //ocurrioUnProblema = true;

                                                        mensaje = "Empresa " + empresa.Trim() + ": no encontró la información del Punto de Suministro " + dat[4].Trim();
                                                        model.resultado = 0;

                                                        this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                                            , periodo
                                                            , empresa
                                                            , mensaje
                                                            , "PR_BARRA"
                                                            , string.Empty
                                                            , EnuTipoIncidenciaImportaSicli.BarraNoExiste);

                                                        match = lstMensajes.FirstOrDefault(stringToCheck => stringToCheck.Contains(mensaje));

                                                        if (match == null)
                                                        {
                                                            lstMensajes.Add(mensaje);
                                                        }
                                                        cantError++;
                                                    }

                                                }

                                                entity.Ulfactcodareademanda = Int32.Parse(dat[5]);
                                                entity.Ulfactpagavad = dat[6].Trim();
                                                entity.Ulfactprecenergbrghp = decimal.Parse(dat[7]);
                                                entity.Ulfactprecenergbrgfp = decimal.Parse(dat[8]);
                                                entity.Ulfactprecpotenbrg = decimal.Parse(dat[9]);
                                                entity.Ulfactconsenergactvhpps = decimal.Parse(dat[10]);
                                                entity.Ulfactconsenergactvfpps = decimal.Parse(dat[11]);
                                                entity.Ulfactconsenergreacps = decimal.Parse(dat[12]);
                                                entity.Ulfactmaxdemhpps = decimal.Parse(dat[13]);
                                                entity.Ulfactmaxdemfpps = decimal.Parse(dat[14]);
                                                entity.Ulfactpeajetransmprin = decimal.Parse(dat[15]);
                                                entity.Ulfactpeajetransmsec = decimal.Parse(dat[16]);
                                                entity.Ulfactvadhp = decimal.Parse(dat[17]);
                                                entity.Ulfactvadfp = decimal.Parse(dat[18]);
                                                entity.Ulfactfpmpoten = decimal.Parse(dat[19]);
                                                entity.Ulfactfpmenerg = decimal.Parse(dat[20]);
                                                entity.Ulfactppmt = decimal.Parse(dat[21]);
                                                entity.Ulfactpemt = decimal.Parse(dat[22]);
                                                entity.Ulfactfactptoref = dat[23];
                                                entity.Ulfactfactgeneracion = decimal.Parse(dat[24]);
                                                entity.Ulfactfacttransmprin = decimal.Parse(dat[25]);
                                                entity.Ulfactfacttransmsec = decimal.Parse(dat[26]);
                                                entity.Ulfactfactdistrib = decimal.Parse(dat[27]);
                                                entity.Ulfactfactexcesopoten = decimal.Parse(dat[28]);
                                                if (dat[29] != null && dat[29] != "" && dat[29].ToLowerInvariant().Trim() != "null")
                                                    entity.Ulfactcargoelectrificarural = decimal.Parse(dat[29]);
                                                if (dat[30] != null && dat[30] != "" && dat[30].ToLowerInvariant().Trim() != "null")
                                                    entity.Ulfactotrosconceptosnoafecigv = decimal.Parse(dat[30]);
                                                if (dat[31] != null && dat[31] != "" && dat[31].ToLowerInvariant().Trim() != "null")
                                                    entity.Ulfactotrosconceptosafectoigv = decimal.Parse(dat[31]);

                                                entity.Ulfactfacturaciontotal = decimal.Parse(dat[32]);
                                                //0 - Código de la Empresa Eléctrica Suministradora - EMPRCODI
                                                entity.Ulfactcodempresa = dat[0].Trim();
                                                entity.Emprcodi = 0;
                                                //SiEmpresaDTO emp = importacionAppServicio.GetSiEmpresaByCodOsinergmin(dat[0].Trim());
                                                //if (emp != null)
                                                //{
                                                //    entity.Emprcodi = emp.Emprcodi;
                                                //}

                                                entity.Ulfactusucreacion = usuarioCreacion;
                                                entity.Ulfactfeccreacion = fechaCreacion;                                              

                                                lista.Add(entity);
                                            }
                                            cont++;
                                            
                                        }

                                    }
                                }


                                //obtenemos el suministrador
                                //var suministradorDTO = importacionAppServicio.GetSiEmpresaByCodOsinergmin(empresa.Trim());

                                importacionAppServicio.GrabarMasivoOsigFactura(lista);
                                
                                #region Proceso Antiguo Carga Tabla05

                                //System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataFactura.txt", data);

                                ////Eliminamos los registros en la tabla
                                //SiEmpresaDTO empresaDto = importacionAppServicio.GetSiEmpresaByCodOsinergmin(empresa.Trim());

                                //if (empresaDto != null)
                                //{
                                //    importacionAppServicio.FacturaDelete(iioPeriodoSicliDTO.PsicliCodi, empresaDto.Emprcodi);
                                //}
                                //else
                                //{
                                //    ocurrioUnProblema = true;
                                //    this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                //        , periodo
                                //        , empresa
                                //        , "No se ha encontrado la empresa con código '" + empresa.Trim() + "'"
                                //        , "SI_EMPRESA"
                                //        , empresa.Trim()
                                //        , EnuTipoIncidenciaImportaSicli.EmpresaNoExiste);
                                //}

                                //Construimos los registro y los insertamos de forma masiva
                                //List<IioFacturaDTO> lista = new List<IioFacturaDTO>();
                                //IioFacturaDTO entity = new IioFacturaDTO();
                                //int cantError = 0;

                                //using (var f = System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataFactura.txt"))
                                //{
                                //    using (var reader = new StreamReader(f, Encoding.UTF8))
                                //    {
                                //        cantError = 0;
                                //        while (!reader.EndOfStream)
                                //        {
                                //            linea = reader.ReadLine();
                                //            if (cont > 0)
                                //            {
                                //                Char delimiter = '\t';
                                //                String[] dat = linea.Split(delimiter);

                                //                entity = new IioFacturaDTO();
                                //                entity.Psiclicodi = iioPeriodoSicliDTO.PsicliCodi;
                                //                entity.Ufacmesfacturado = dat[1].Trim();

                                //                //3 - Código de la Barra de Referencia de Generación - UFACCODBRG
                                //                PrBarraDTO prBarraDTO = importacionAppServicio.GetPrBarraByCodOsinergmin(dat[3].Trim());
                                //                if (prBarraDTO != null)
                                //                {
                                //                    entity.Ufaccodbrg = prBarraDTO.Barrcodi.ToString();
                                //                }
                                //                else
                                //                {
                                //                    String barrCodiOsinergmin = dat[3].Trim();

                                //                    barraDTO barraDTO = this.obtenerBarra(barrCodiOsinergmin);
                                //                    if (barraDTO != null)
                                //                    {
                                //                        prBarraDTO = new PrBarraDTO();
                                //                        prBarraDTO.Barrnombre = barraDTO.nomBarra.ToUpper();
                                //                        prBarraDTO.Barrtension = barraDTO.tension.ToString();
                                //                        prBarraDTO.Barrusername = User.Identity.Name;
                                //                        prBarraDTO.Barrestado = ConstantesIntercambio.BarraCOESEstadoActivo;
                                //                        prBarraDTO.Osinergcodi = barrCodiOsinergmin;

                                //                        //- Valores por defecto.
                                //                        prBarraDTO.Barrflagbarratransferencia = "NO";
                                //                        prBarraDTO.Barrflagdesbalance = "NO";
                                //                        prBarraDTO.Barrfecins = DateTime.Now;
                                //                        prBarraDTO.Barrfecact = DateTime.Now;
                                //                        prBarraDTO.Barrbarratransf = 0;
                                //                        prBarraDTO.Barrflagbarracompensa = 0;

                                //                        int barrCodi = importacionAppServicio.InsertarBarra(prBarraDTO);
                                //                        entity.Ufaccodbrg = barrCodi.ToString();
                                //                    }
                                //                    else
                                //                    {
                                //                        ocurrioUnProblema = true;

                                //                        mensaje = "Empresa " + empresa.Trim() + ": no encontró la información de la barra  " + dat[3].Trim();
                                //                        model.resultado = 0;

                                //                        this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                //                            , periodo
                                //                            , empresa
                                //                            , mensaje
                                //                            , "PR_BARRA"
                                //                            , string.Empty
                                //                            , EnuTipoIncidenciaImportaSicli.BarraNoExiste);

                                //                        match = lstMensajes.FirstOrDefault(stringToCheck => stringToCheck.Contains(mensaje));

                                //                        if (match == null)
                                //                        {
                                //                            lstMensajes.Add(mensaje);
                                //                        }
                                //                        cantError++;
                                //                    }
                                //                }

                                //                //4 - Código del Punto de Suministro - UFACCODPUNTOSUMINISTRO
                                //                PrBarraDTO bar2 = importacionAppServicio.GetPrBarraByCodOsinergmin(dat[4].Trim());
                                //                if (bar2 != null)
                                //                {
                                //                    entity.Ufaccodpuntosuministro = bar2.Barrcodi.ToString();
                                //                }
                                //                else
                                //                {
                                //                    String barrCodiOsinergmin2 = dat[4].Trim();

                                //                    barraDTO barraDTO = this.obtenerBarra(barrCodiOsinergmin2);
                                //                    if (barraDTO != null)
                                //                    {
                                //                        bar2 = new PrBarraDTO();
                                //                        bar2.Barrnombre = barraDTO.nomBarra.ToUpper();
                                //                        bar2.Barrtension = barraDTO.tension.ToString();
                                //                        bar2.Barrusername = User.Identity.Name;
                                //                        bar2.Barrestado = ConstantesIntercambio.BarraCOESEstadoActivo;
                                //                        bar2.Osinergcodi = barrCodiOsinergmin2;

                                //                        //- Valores por defecto.
                                //                        bar2.Barrflagbarratransferencia = "NO";
                                //                        bar2.Barrflagdesbalance = "NO";
                                //                        bar2.Barrfecins = DateTime.Now;
                                //                        bar2.Barrfecact = DateTime.Now;
                                //                        bar2.Barrbarratransf = 0;
                                //                        bar2.Barrflagbarracompensa = 0;

                                //                        int barrCodi2 = importacionAppServicio.InsertarBarra(bar2);
                                //                        entity.Ufaccodpuntosuministro = barrCodi2.ToString();
                                //                    }
                                //                    else
                                //                    {
                                //                        ocurrioUnProblema = true;

                                //                        mensaje = "Empresa " + empresa.Trim() + ": no encontró la información del Punto de Suministro " + dat[4].Trim();
                                //                        model.resultado = 0;

                                //                        this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                //                            , periodo
                                //                            , empresa
                                //                            , mensaje
                                //                            , "PR_BARRA"
                                //                            , string.Empty
                                //                            , EnuTipoIncidenciaImportaSicli.BarraNoExiste);

                                //                        match = lstMensajes.FirstOrDefault(stringToCheck => stringToCheck.Contains(mensaje));

                                //                        if (match == null)
                                //                        {
                                //                            lstMensajes.Add(mensaje);
                                //                        }
                                //                        cantError++;
                                //                    }

                                //                }

                                //                entity.Ufacidareademanda = Int32.Parse(dat[5]);
                                //                entity.Ufacpagavad = dat[6].Trim();
                                //                entity.Ufacprecenergbrghp = decimal.Parse(dat[7]);
                                //                entity.Ufacprecenergbrgfp = decimal.Parse(dat[8]);
                                //                entity.Ufacprecpotenbrg = decimal.Parse(dat[9]);
                                //                entity.Ufacconsenergactvhpps = decimal.Parse(dat[10]);
                                //                entity.Ufacconsenergactvfpps = decimal.Parse(dat[11]);
                                //                entity.Ufacconsenergreacps = decimal.Parse(dat[12]);
                                //                entity.Ufacmaxdemhpps = decimal.Parse(dat[13]);
                                //                entity.Ufacmaxdemfpps = decimal.Parse(dat[14]);
                                //                entity.Ufacpeajetransmprin = decimal.Parse(dat[15]);
                                //                entity.Ufacpeajetransmsec = decimal.Parse(dat[16]);
                                //                entity.Ufacvadhp = decimal.Parse(dat[17]);
                                //                entity.Ufacvadfp = decimal.Parse(dat[18]);
                                //                entity.Ufacfpmpoten = decimal.Parse(dat[19]);
                                //                entity.Ufacfpmenerg = decimal.Parse(dat[20]);
                                //                entity.Ufacppmt = decimal.Parse(dat[21]);
                                //                entity.Ufacpemt = decimal.Parse(dat[22]);
                                //                entity.Ufacfactptoref = dat[23];
                                //                entity.Ufacfactgeneracion = decimal.Parse(dat[24]);
                                //                entity.Ufacfacttransmprin = decimal.Parse(dat[25]);
                                //                entity.Ufacfacttransmsec = decimal.Parse(dat[26]);
                                //                entity.Ufacfactdistrib = decimal.Parse(dat[27]);
                                //                entity.Ufacfactexcesopoten = decimal.Parse(dat[28]);
                                //                if (dat[29] != null && dat[29] != "" && dat[29].ToLowerInvariant().Trim() != "null")
                                //                    entity.Ufaccargoelectrificacionrural = decimal.Parse(dat[29]);
                                //                if (dat[30] != null && dat[30] != "" && dat[30].ToLowerInvariant().Trim() != "null")
                                //                    entity.Ufacotrosconceptosnoafectoigv = decimal.Parse(dat[30]);
                                //                if (dat[31] != null && dat[31] != "" && dat[31].ToLowerInvariant().Trim() != "null")
                                //                    entity.Ufacotrosconceptosafectoigv = decimal.Parse(dat[31]);

                                //                entity.Ufacfacturaciontotal = decimal.Parse(dat[32]);
                                //                //0 - Código de la Empresa Eléctrica Suministradora - EMPRCODI
                                //                SiEmpresaDTO emp = importacionAppServicio.GetSiEmpresaByCodOsinergmin(dat[0].Trim());
                                //                if (emp != null)
                                //                {
                                //                    entity.Emprcodi = emp.Emprcodi;
                                //                }
                                //                else
                                //                {
                                //                    ocurrioUnProblema = true;

                                //                    //Mostramos mensaje
                                //                    mensaje = "Empresa " + empresa.Trim() + ": no encontró la información de la empresa  " + dat[0].Trim();
                                //                    model.resultado = 0;
                                //                    match = lstMensajes.FirstOrDefault(stringToCheck => stringToCheck.Contains(mensaje));

                                //                    //- alpha.HDT - Inicio 11/10/2017: Cambio para atender el requerimiento.
                                //                    //this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                //                    //                                       , periodo
                                //                    //                                       , empresa
                                //                    //                                       , mensaje
                                //                    //                                       , "SI_EMPRESA"
                                //                    //                                       , dat[0].Trim()
                                //                    //                                       , EnuTipoIncidenciaImportaSicli.EmpresaNoExiste);
                                //                    //- HDT Fin

                                //                    if (match == null)
                                //                    {
                                //                        //- alpha.HDT - Inicio 10/10/2017: Cambio para atender el requerimiento.
                                //                        this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                //                            , periodo
                                //                            , empresa
                                //                            , mensaje
                                //                            , "SI_EMPRESA"
                                //                            , dat[0].Trim()
                                //                            , EnuTipoIncidenciaImportaSicli.EmpresaNoExiste);
                                //                        //- HDT Fin                                                    
                                //                        lstMensajes.Add(mensaje);
                                //                    }
                                //                    cantError++;
                                //                }

                                //                //2 - Código del Suministro del Usuario Libre - EQUICODI
                                //                EqEquipoDTO eqp = importacionAppServicio.GetEqEquipoByCodOsinergmin(dat[2].Trim());
                                //                if (eqp != null)
                                //                {
                                //                    entity.Equicodi = eqp.Equicodi;
                                //                }
                                //                else
                                //                {
                                //                    ocurrioUnProblema = true;

                                //                    //Mostramos mensaje
                                //                    mensaje = "Empresa " + empresa.Trim() + ": no encontró la información del Suministro del Usuario Libre " + dat[2].Trim();
                                //                    model.resultado = 0;
                                //                    match = lstMensajes.FirstOrDefault(stringToCheck => stringToCheck.Contains(mensaje));

                                //                    this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                //                        , periodo
                                //                        , empresa
                                //                        , mensaje
                                //                        , "EQ_EQUIPO"
                                //                        , dat[2].Trim()
                                //                        , EnuTipoIncidenciaImportaSicli.SuministroUsuarioLibreNoExiste);

                                //                    if (match == null)
                                //                    {
                                //                        lstMensajes.Add(mensaje);
                                //                    }
                                //                    cantError++;
                                //                }
                                //                lista.Add(entity);
                                //            }
                                //            cont++;
                                //        }

                                //    }
                                //}

                                //if (cantError > 0)
                                //{
                                //    control.Rcimnroregistroscoes = 0;
                                //    control.Rcimestadoimportacion = "0";
                                //}
                                //else
                                //{
                                //    importacionAppServicio.GrabarMasivoFactura(lista);
                                //    control.Rcimnroregistroscoes = lista.Count();
                                //    if (item.nroRegistros != lista.Count())
                                //    {
                                //        control.Rcimestadoimportacion = "0";
                                //    }
                                //    else
                                //    {
                                //        control.Rcimestadoimportacion = "1";
                                //    }
                                //}
                                //control.Rcimnroregistros = item.nroRegistros;
                                //control.Rcimfechorimportacion = DateTime.Now;
                                //control.Rcimusumodificacion = User.Identity.Name;
                                //control.Rcimfecmodificacion = DateTime.Now;

                                ////Actualizamos el control de importación
                                //importacionAppServicio.ControlImportacionSave(control);

                                ////Actualizamos el periodo
                                //iioPeriodoSicliDTO.PsicliFecUltActCoes = DateTime.Now;
                                //importacionAppServicio.PeriodoUpdate(iioPeriodoSicliDTO);

                                #endregion
                            }
                            else
                            {
                                System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataSuministro.txt", data);
                                var fechaCreacion = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                var usuarioCreacion = User.Identity.Name;
                                //var cantError = 0;

                                //Eliminamos los registros en la tabla
                                //SiEmpresaDTO empresaDto = importacionAppServicio.GetSiEmpresaByCodOsinergmin(empresa.Trim());

                                importacionAppServicio.OsigSuministroDelete(iioPeriodoSicliDTO.PsicliCodi, empresa.Trim());

                                //- Construimos los registro y los insertamos de forma masiva
                                List<IioOsigSuministroUlDTO> lista = new List<IioOsigSuministroUlDTO>();
                                IioOsigSuministroUlDTO entity = new IioOsigSuministroUlDTO();

                                using (var f = System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + "dataSuministro.txt"))
                                {
                                    using (var reader = new StreamReader(f, Encoding.UTF8))
                                    {
                                        //cantError = 0;
                                        while (!reader.EndOfStream)
                                        {
                                            linea = reader.ReadLine();
                                            if (cont > 0)
                                            {
                                                Char delimiter = '\t';
                                                String[] dat = linea.Split(delimiter);

                                                entity = new IioOsigSuministroUlDTO();
                                                entity.Psiclicodi = iioPeriodoSicliDTO.PsicliCodi;
                                                entity.Ulsumicodempresa = dat[0].Trim();
                                                entity.Ulsumicodusuariolibre = dat[1].Trim();
                                                entity.Ulsumicodsuministro = dat[2].Trim();
                                                entity.Ulsuminombreusuariolibre = dat[3].Trim();
                                                entity.Ulsumidireccionsuministro = dat[4].Trim();

                                                entity.Ulsuminrosuministroemp = dat[5].Trim();
                                                entity.Ulsumiubigeo = dat[6].Trim();                                                
                                                entity.Ulsumicodciiu = dat[7].Trim();
                                                entity.Equicodi = 0;
                                                                                                
                                                entity.Ulsumiusucreacion = usuarioCreacion;
                                                entity.Ulsumifeccreacion = fechaCreacion;
                                                                                               

                                                lista.Add(entity);
                                            }
                                            cont++;

                                        }

                                    }
                                }

                                importacionAppServicio.GrabarMasivoOsigSuministro(lista);
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            ocurrioUnProblema = true;
                            mensaje = "Se presenta problemas con el web service de Osinergmin, por favor intentar más tarde.";
                            if (control != null)
                            {
                                if (importacionAppServicio != null)
                                {
                                    control.Rcimestadoimportacion = "0";
                                    importacionAppServicio.ControlImportacionSave(control);
                                }

                                this.RegistrarIncidenciaImportacionSicli(control.Rcimcodi
                                    , periodo
                                    , empresa
                                    , mensaje
                                    , string.Empty
                                    , string.Empty
                                    , EnuTipoIncidenciaImportaSicli.Ninguno);
                            }

                            Log.Error("ImportarRegistrosSICLI " + empresa, ex);
                            continue;
                        }
                        finally
                        {
                            this.lbarraDTO = null;
                        }
                    }

                }

                #region Actualizaciones masivas por tabla
                //var empresasConsulta = string.Empty;
                //foreach (var emp in empresas)
                //{
                //    empresasConsulta = empresasConsulta + "'" + emp.Trim() + "',";
                //}

                if (tabla.Equals(ConstantesIntercambioOsinergmin.TablaSicli04))
                {
                    importacionAppServicio.UpdatePtoMediCodiOsigConsumo(0, iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','));
                    //Registro Log                  

                    importacionAppServicio.GenerarOsigConsumoLogImportacionPtoMedicion(iioPeriodoSicliDTO.PsicliCodi, fechaProceso.ToString("MMyyyy"),
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli04, empresasConsulta.TrimEnd(','));


                    importacionAppServicio.ActualizarRegistrosImportacion(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','),
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli04);

                    //Actualizar tabla control                   
                    importacionAppServicio.ActualizarControlImportacionNoOK(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','), 
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli04);

                    importacionAppServicio.ActualizarControlImportacionOK(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','),
                       User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli04);
                }
                else if (tabla.Equals(ConstantesIntercambioOsinergmin.TablaSicli05))
                {
                    //Actualizacion de campo Emprcodi
                    importacionAppServicio.UpdateEmprcodiOsigFactura(0, iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','));

                    //Actualizacion de Puntos de medicion
                    importacionAppServicio.UpdatePtoMediCodiOsigFactura(0, iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','));                    

                    //Registro Log
                    importacionAppServicio.GenerarOsigFacturaLogImportacionEmpresa(iioPeriodoSicliDTO.PsicliCodi, fechaProceso.ToString("MMyyyy"),
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli05, empresasConsulta.TrimEnd(','));

                    importacionAppServicio.GenerarOsigFacturaLogImportacionPtoMedicion(iioPeriodoSicliDTO.PsicliCodi, fechaProceso.ToString("MMyyyy"),
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli05, empresasConsulta.TrimEnd(','));

                    importacionAppServicio.ActualizarRegistrosImportacion(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','),
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli05);

                    //Actualizar tabla control                   
                    importacionAppServicio.ActualizarControlImportacionNoOK(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','), 
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli05);

                    importacionAppServicio.ActualizarControlImportacionOK(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','),
                       User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli05);
                }
                else if (tabla.Equals(ConstantesIntercambioOsinergmin.TablaSicli02))
                {
                    importacionAppServicio.UpdateOsigSuministro(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','));
                    //Registro Log
                    importacionAppServicio.GenerarOsigSuministroLogImportacionEquipo(iioPeriodoSicliDTO.PsicliCodi, fechaProceso.ToString("MMyyyy"),
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli02, empresasConsulta.TrimEnd(','));

                    importacionAppServicio.ActualizarRegistrosImportacion(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','),
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli02);
                    //Actualizar tabla control                   
                    importacionAppServicio.ActualizarControlImportacionNoOK(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','),
                        User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli02);

                    importacionAppServicio.ActualizarControlImportacionOK(iioPeriodoSicliDTO.PsicliCodi, empresasConsulta.TrimEnd(','),
                       User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli02);
                }


                #endregion

                ////Actualizamos el periodo
                iioPeriodoSicliDTO.PsicliFecUltActCoes = DateTime.Now;
                //iioPeriodoSicliDTO.PSicliFecSincronizacion = DateTime.Now;
                importacionAppServicio.PeriodoUpdate(iioPeriodoSicliDTO);

                //Obtenemos el periodo para revisar si hay empresas pendientes
                //iioPeriodoSicliDTO = importacionAppServicio.PeriodoGetById(new IioPeriodoSicliDTO { PsicliAnioMesPerrem = per });
                //model.empresasPendientes = iioPeriodoSicliDTO.TablasEmpresasProcesar;

                if (!ocurrioUnProblema)
                {
                    model.mensaje = "Importación Correcta";
                    model.resultado = 1;
                }
                else
                {
                    model.mensaje = "Ocurrió al menos un problema en la importación, por favor revise el detalle en las Banderas Rojas de la lista";
                    model.resultado = 0;
                }

                model.lstMensajes = lstMensajes.OrderBy(x => x).ToList();
            }
            catch (Exception exGeneral)
            {
                Log.Error("ImportarRegistrosSICLI.Excepción General ", exGeneral);
                throw exGeneral;
            }

            //- HDT Fin

            return Json(model);
        }

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite registrar las incidencias de la importación de la información del Sicli para la Tabla 04.
        /// </summary>
        /// <param name="rcImCodi"></param>
        /// <param name="lIioLogImportacionIncidenteDTO"></param>
        /// <param name="periodo"></param>
        /// <param name="empresa"></param>
        private void RegistrarIncidenciasImportacionSicli(int rcImCodi
                                                        , List<IioLogImportacionIncidenteDTO> lIioLogImportacionIncidenteDTO
                                                        , string periodo
                                                        , string empresa)
        {
            IioLogImportacionDTO oIioLogImportacionDTO = null;
            int correlativoLog = importacionAppServicio.GetCorrelativoDisponibleLogImportacionSicli();

            foreach (IioLogImportacionIncidenteDTO oIioLogImportacionIncidenteDTO in lIioLogImportacionIncidenteDTO)
            {
                oIioLogImportacionDTO = new IioLogImportacionDTO();
                oIioLogImportacionDTO.UlogCodi = correlativoLog;
                oIioLogImportacionDTO.PsicliCodi = periodo.Replace(" ", "");
                oIioLogImportacionDTO.UlogUsuCreacion = User.Identity.Name;
                oIioLogImportacionDTO.UlogFecCreacion = DateTime.Now;
                oIioLogImportacionDTO.UlogProceso = "SICLI";
                oIioLogImportacionDTO.UlogTablaAfectada = empresa;
                oIioLogImportacionDTO.UlogNroRegistrosAfectados = 1;
                oIioLogImportacionDTO.UlogMensaje = oIioLogImportacionIncidenteDTO.Mensaje;
                oIioLogImportacionDTO.RcimCodi = rcImCodi;

                //- Realizando la inserción:
                importacionAppServicio.SaveIioLogImportacion(oIioLogImportacionDTO);

                correlativoLog++;    
            }

        }

        //- alpha.HDT - 12/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite registrar una incidencia del proceso de importación.
        /// </summary>
        /// <param name="rcImCodi"></param>
        /// <param name="periodo"></param>
        /// <param name="empresa"></param>
        /// <param name="mensaje"></param>
        /// <param name="tablaCOES"></param>
        /// <param name="idRegistroCOES"></param>
        /// <param name="tipoIncidenciaImportaSicli"></param>
        private void RegistrarIncidenciaImportacionSicli(int rcImCodi
                                                       , string periodo
                                                       , string empresa
                                                       , string mensaje
                                                       , string tablaCOES
                                                       , string idRegistroCOES
                                                       , EnuTipoIncidenciaImportaSicli tipoIncidenciaImportaSicli)
        {
            int correlativoLog = importacionAppServicio.GetCorrelativoDisponibleLogImportacionSicli();

            IioLogImportacionDTO oIioLogImportacionDTO = new IioLogImportacionDTO();
            oIioLogImportacionDTO.UlogCodi = correlativoLog;
            oIioLogImportacionDTO.PsicliCodi = periodo.Replace(" ", "");
            oIioLogImportacionDTO.UlogUsuCreacion = User.Identity.Name;
            oIioLogImportacionDTO.UlogFecCreacion = DateTime.Now;
            oIioLogImportacionDTO.UlogProceso = "SICLI";
            oIioLogImportacionDTO.UlogTablaAfectada = empresa;
            oIioLogImportacionDTO.UlogNroRegistrosAfectados = 1;
            oIioLogImportacionDTO.UlogMensaje = mensaje;
            oIioLogImportacionDTO.RcimCodi = rcImCodi;
            oIioLogImportacionDTO.UlogTablaCOES = tablaCOES;
            oIioLogImportacionDTO.UlogIdRegistroCOES = idRegistroCOES;
            oIioLogImportacionDTO.UlogTipoIncidencia = int.Parse(tipoIncidenciaImportaSicli.ToString("D"));

            //- Realizando la inserción:
            importacionAppServicio.SaveIioLogImportacion(oIioLogImportacionDTO);            
        }

        /// <summary>
        /// Permite listar las incidencias de la importación.
        /// </summary>
        /// <param name="rcImCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarIncidenciasImportacion(int rcImCodi)
        {
            List<IioLogImportacionDTO> lIioLogImportacionDTO = importacionAppServicio.GetIncidenciasImportacion(rcImCodi);

            List<string> incidencias = new List<string>();

            foreach (IioLogImportacionDTO item in lIioLogImportacionDTO)
            {
                incidencias.Add(item.UlogMensaje);
            }

            var jsonSerialiser = new JavaScriptSerializer();

            return Json(incidencias);
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite generar el reporte de los datos de la tabla 05 del Sicli importados desde el Osinergmin.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tabla"></param>
        /// <param name="empresasIn"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDatosTabla(string periodo, string tabla, string empresasIn, string fechaDia)
        {
            try
            {
                periodo = periodo.Substring(3, 4) + periodo.Substring(0, 2);

                if (!string.IsNullOrEmpty(fechaDia))
                {
                    var fecha = DateTime.ParseExact(fechaDia, "dd/MM/yyyy", null);
                    fechaDia = fecha.ToString("yyyyMMdd");
                }               
               

                var rutaLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                String nombreArchivo = importacionAppServicio.GenerarReporteTabla(periodo
                                                                                , tabla
                                                                                , empresasIn
                                                                                , AppDomain.CurrentDomain.BaseDirectory 
                                                                                  + Constantes.RutaCarga
                                                                                , rutaLogo
                                                                                , fechaDia);
                return Json(nombreArchivo);
            }
            catch
            {
                return Json(-1);
            }
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite recibir el requerimiento de descarga del archivo generado previamente. <see cref="ExportarDatosTabla"/>
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarTabla04(string file)
        {
            string path = file;
            string app = Constantes.AppExcel;
            return File(path, app, "ImportaciónTabla04.xls");
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite recibir el requerimiento de descarga del archivo generado previamente. <see cref="ExportarDatosTabla"/>
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarTabla05(string file)
        {
            string path = file;
            string app = Constantes.AppExcel;
            return File(path, app, "ImportaciónTabla05.xls");
        }

        //- alpha.HDT - 11/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite listar las incidencias de la importación que están relacionados a Suministros que pueden
        /// ser generados.
        /// </summary>
        /// <remarks>Se asume que los registros con incidentes de importación de suministros no superan los 500 registros.</remarks>
        /// <param name="periodo"></param>
        /// <param name="empresasIn"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult listarIncidenciasImportacionSuministros(string periodo, string empresasIn)
        {
            periodo = periodo.Replace(" ", "");
           
            List<EqAreaDTO> lEqAreaDTO = importacionAppServicio.GetAreasSubestacion();

            SincronizaMaestroAppServicio sincMaestroAppServicio = new SincronizaMaestroAppServicio();
            Dictionary<string, suministroUsuarioDTO> suministroXSuministroUsuario = sincMaestroAppServicio.ObtenerSuministroXSuministroUsuario();

            List<IioLogImportacionDTO> 
                lIioLogImportacionDTO = importacionAppServicio.GetIncidenciasImportacionSuministro(periodo, empresasIn);

            SiEmpresaDTO siEmpresaDTO = null;
            SiEmpresaDTO suministradorDTO = null;

            string ruc = string.Empty;
            foreach (IioLogImportacionDTO iioLogImportacionDTO in lIioLogImportacionDTO)
            {
                iioLogImportacionDTO.Cliente = "(*)";
                iioLogImportacionDTO.Suministrador = "(*)";
                iioLogImportacionDTO.Tension = string.Empty;
                iioLogImportacionDTO.Barra = string.Empty;                

                foreach (KeyValuePair<string, suministroUsuarioDTO> kvp in suministroXSuministroUsuario)
	            {
                    if (iioLogImportacionDTO.UlogIdRegistroCOES.Trim() == kvp.Key.Trim())
                    {
                        //- Entonces ya se encontró al RUC.
                        ruc = kvp.Value.codigoUsuarioLibre;
                        
                        //- Por cada RUC se debe buscar la razón social de ese RUC de acuerdo con la configuración del COES.
                        siEmpresaDTO = importacionAppServicio.GetUsuarioLibreByRuc(ruc);

                        suministradorDTO = importacionAppServicio.GetSiEmpresaByCodOsinergmin(iioLogImportacionDTO.UlogTablaAfectada.Trim());

                        if (siEmpresaDTO != null)
                        {
                            //- Se ha encontrado el registro.
                            iioLogImportacionDTO.Cliente = siEmpresaDTO.Emprruc + ": " + siEmpresaDTO.Emprnomb;
                            iioLogImportacionDTO.Tension = kvp.Value.tension;
                            iioLogImportacionDTO.Barra = kvp.Value.nomBarra;
                            iioLogImportacionDTO.EmprCodi = siEmpresaDTO.Emprcodi;
                        }
                        else
                        {
                            iioLogImportacionDTO.Cliente = ruc + ": (*)";
                            iioLogImportacionDTO.Barra = string.Empty;
                        }

                        if (suministradorDTO != null)
                        {
                            iioLogImportacionDTO.Suministrador = iioLogImportacionDTO.UlogTablaAfectada.Trim() + ": " + suministradorDTO.Emprnomb;
                            iioLogImportacionDTO.EmprCodiSumi = suministradorDTO.Emprcodi;
                        }
                        else
                        {
                            iioLogImportacionDTO.Suministrador = iioLogImportacionDTO.UlogTablaAfectada.Trim() + ": (*)";
                        }

                        break;
                    }
	            }
                
            }

            IncidenciaSuministroDTO incidenciaSuministroDTO = new IncidenciaSuministroDTO();
            incidenciaSuministroDTO.ListaEqAreaDTO = lEqAreaDTO;
            incidenciaSuministroDTO.ListaIioLogImportacionDTO = lIioLogImportacionDTO;

            var jsonSerialiser = new JavaScriptSerializer();

            return Json(incidenciaSuministroDTO);
        }

        //- alpha.HDT - 22/07/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Genera los suministros a partir de una lista de incidentes de importación.
        /// </summary>
        /// <param name="lIioLogImportacionDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarSuministrosDesdeIncidentes(List<IioLogImportacionDTO> lIioLogImportacionDTO)
        {
            EntidadImportacionModel model = new EntidadImportacionModel();
            //importacionAppServicio = new ImportacionAppServicio();
            bool ocurrioUnProblema = false;

            EqEquipoDTO eqEquipoDTO = null;
            MePtomedicionDTO mePtomedicionDTO = null;
            int equicodi = 0;
            List<MePtomedicionDTO> lMePtomedicionDTO = null;
            bool yaTienePuntoMedicion = false;

            IioPeriodoSicliDTO iioPeriodoSicliDTO = null;
            //int psiclicodi = 0;
            try
            {
                foreach (IioLogImportacionDTO iioLogImportacionDTO in lIioLogImportacionDTO)
                {
                    //- Obteniendo la Empresa Suministradora
                    SiEmpresaDTO siEmpresaDTOSuministradora = importacionAppServicio.GetSiEmpresaByCodOsinergmin(iioLogImportacionDTO.UlogTablaAfectada);

                    if (siEmpresaDTOSuministradora == null)
                    {
                        continue;
                    }

                    if(iioPeriodoSicliDTO == null)
                    {
                        int imes = Int32.Parse(iioLogImportacionDTO.PsicliCodi.ToString().Substring(0, 2));
                        int ianho = Int32.Parse(iioLogImportacionDTO.PsicliCodi.ToString().Substring(2));
                        DateTime fechaProceso = new DateTime(ianho, imes, 1);
                        string per = fechaProceso.ToString("yyyyMM");

                        iioPeriodoSicliDTO = importacionAppServicio.PeriodoGetById(new IioPeriodoSicliDTO { PsicliAnioMesPerrem = per });
                        //psiclicodi = iioPeriodoSicliDTO.PsicliCodi;

                    }
                    yaTienePuntoMedicion = false;
                    eqEquipoDTO = importacionAppServicio.GetEqEquipo(iioLogImportacionDTO.Suministro
                                                                   , int.Parse(FamiliaCodi.Suministro.ToString("D"))
                                                                     );

                    if (eqEquipoDTO == null)
                    {

                        //- Ahora se procede a insertar el equipo:
                        //- ======================================
                        eqEquipoDTO = new EqEquipoDTO();
                        eqEquipoDTO.Emprcodi = iioLogImportacionDTO.EmprCodi;
                        eqEquipoDTO.Elecodi = -1;
                        eqEquipoDTO.Areacodi = iioLogImportacionDTO.CodigoArea;
                        eqEquipoDTO.Famcodi = int.Parse(FamiliaCodi.Suministro.ToString("D"));
                        eqEquipoDTO.Equiabrev = ajustarTamanioTexto(25
                                                                  , iioLogImportacionDTO.Suministro
                                                                  , iioLogImportacionDTO.Barra.ToUpper()
                                                                  , iioLogImportacionDTO.Tension);
                        eqEquipoDTO.Equinomb = ajustarTamanioTexto(80
                                                                 , iioLogImportacionDTO.Suministro
                                                                 , iioLogImportacionDTO.Barra.ToUpper()
                                                                 , iioLogImportacionDTO.Tension);
                        eqEquipoDTO.Equitension = decimal.Parse(iioLogImportacionDTO.Tension);
                        eqEquipoDTO.Lastuser = User.Identity.Name;
                        eqEquipoDTO.Lastdate = DateTime.Now;
                        eqEquipoDTO.Equiestado = ConstantesIntercambio.EquipoCOESEstadoActivo.ToString();
                        eqEquipoDTO.Osinergcodi = iioLogImportacionDTO.Suministro;

                        equicodi = importacionAppServicio.InsertarEquipo(eqEquipoDTO);
                    }
                    else
                    {
                        equicodi = eqEquipoDTO.Equicodi;
                    }

                    //- Verificamos si el equipo ya tiene puntos de medición asociados
                    //lMePtomedicionDTO = importacionAppServicio.ObtenerPuntoMedicionPorEquipo(equicodi);
                    lMePtomedicionDTO = importacionAppServicio.ObtenerPuntoMedicionPorEquipoUsuarioLibre(equicodi, siEmpresaDTOSuministradora.Emprcodi);
                                       

                    foreach (MePtomedicionDTO puntoMedicion in lMePtomedicionDTO)
                    {
                        if (puntoMedicion.Ptomediestado != "X")
                        {
                            yaTienePuntoMedicion = true;
                            break;
                        }
                    }

                    if (!yaTienePuntoMedicion)
                    {
                        //- Ahora se procede a insertar el punto de medición:
                        //- =================================================
                        mePtomedicionDTO = new MePtomedicionDTO();
                        mePtomedicionDTO.Origlectcodi = 19;
                        mePtomedicionDTO.Ptomedielenomb = eqEquipoDTO.Equinomb;
                        mePtomedicionDTO.Ptomedidesc = mePtomedicionDTO.Ptomedielenomb;
                        mePtomedicionDTO.Equicodi = equicodi;
                        mePtomedicionDTO.Grupocodi = -1;
                        //mePtomedicionDTO.Emprcodi = eqEquipoDTO.Emprcodi;
                        mePtomedicionDTO.Emprcodi = siEmpresaDTOSuministradora.Emprcodi;//cambio
                        mePtomedicionDTO.Lastuser = User.Identity.Name;
                        mePtomedicionDTO.Lastdate = DateTime.Now;
                        mePtomedicionDTO.Tipoptomedicodi = 15;
                        mePtomedicionDTO.Ptomediestado = ConstantesIntercambio.PtoMedicionCOESEstadoActivo;

                        int ptoMediCodi = importacionAppServicio.InsertarPuntoMedicion(mePtomedicionDTO);

                        //- alpha.HDT - Inicio 01/10/2017: Cambio para completar la información para el PR-16.
                        //- Se incluye el registro de los datos en las tablas ME_PTOSUMINISTRADOR y ME_HOJAPTOMED.
                        this.CompletarInformacionPr16(ptoMediCodi
                                                    , mePtomedicionDTO.Emprcodi.Value
                                                    , iioLogImportacionDTO.UlogTablaAfectada
                                                    , iioLogImportacionDTO.PsicliCodi);
                        //- HDT Fin
                    }

                    //- Si todo está conforme entonces se procede a eliminar el registro de la Incidencia:
                    //- ==================================================================================
                    importacionAppServicio.EliminarIncidenteImportacion(iioLogImportacionDTO.UlogCodi);
                    
                }

                //Se procede a actualizar los ptomedicodi en las nuevas tablas
                var listaEmpresas = lIioLogImportacionDTO.Select(p => p.UlogTablaAfectada).Distinct();
                var empresas = "";
                foreach(var emp in listaEmpresas)
                {
                    empresas = empresas + "'" + emp + "',";
                }
                //var emp = "'" + iioLogImportacionDTO.UlogTablaAfectada + "'";
                //importacionAppServicio.UpdatePtoMediCodiOsigConsumo(0, iioPeriodoSicliDTO.PsicliCodi, empresas.TrimEnd(','));
                importacionAppServicio.UpdatePtoMediCodiOsigFactura(0, iioPeriodoSicliDTO.PsicliCodi, empresas.TrimEnd(','));
                //se procede a actualizar el equicodi en las nuevas tablas
                //importacionAppServicio.UpdateOsigSuministro(iioPeriodoSicliDTO.PsicliCodi, empresas.TrimEnd(','));

                //se procede a actualizar el flag en caso se resuelva todas las incidencias
                //Actualizar tabla control                   
                importacionAppServicio.ActualizarControlImportacionNoOK(iioPeriodoSicliDTO.PsicliCodi, empresas.TrimEnd(','),
                    User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli05);

                importacionAppServicio.ActualizarControlImportacionOK(iioPeriodoSicliDTO.PsicliCodi, empresas.TrimEnd(','),
                   User.Identity.Name, ConstantesIntercambioOsinergmin.TablaSicli05);

            }
            catch (Exception ex)
            {
                Log.Error("GenerarSuministrosDesdeIncidentes ", ex);
                ocurrioUnProblema = true;
            }

            if (!ocurrioUnProblema)
            {
                model.mensaje = "Importación Correcta";
                model.resultado = 1;
            }
            else
            {
                model.mensaje = "Ocurrió al menos un problema en la generación de los susministros, por favor contacte al Administrador del Sistema";
                model.resultado = 0;
            }

            return Json(model);
        }

        //- alpha.HDT - 10/10/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite completar la información en el PR16.
        /// Se insertan los registros correspondientes en ME_HOJAPTOMED y ME_PTOSUMINISTRADOR.
        /// </summary>
        /// <param name="ptoMediCodi"></param>
        /// <param name="emprCodi"></param>
        /// <param name="codigoEmpresaSuministradora"></param>
        /// <param name="pSicliCodi"></param>
        private void CompletarInformacionPr16(int ptoMediCodi, int emprCodi, string codigoEmpresaSuministradora, string pSicliCodi)
        {
            //- Insertando el punto en la hoja de medición
            //- ==========================================
            int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);

            MeHojaptomedDTO meHojaptomedDTO = new MeHojaptomedDTO();
            meHojaptomedDTO.Ptomedicodi = ptoMediCodi;
            meHojaptomedDTO.Tipoinfocodi = 1; //- Valor por defecto de acuerdo con la revisión de los datos.
            meHojaptomedDTO.Formatcodi = IdFormato; //- Se obtiene de la configuración de la aplicación.
            meHojaptomedDTO.Hojaptosigno = 1; //- Valor por defecto de acuerdo con la revisión de los datos.
            meHojaptomedDTO.Hojaptoliminf = 0; //- Valor por defecto de acuerdo con la revisión de los datos.
            meHojaptomedDTO.Hojaptolimsup = 10000; //- Valor por defecto de acuerdo con la revisión de los datos.
            meHojaptomedDTO.Hojaptoactivo = 1; //- Valor por defecto de acuerdo con la revisión de los datos.
            meHojaptomedDTO.Lastuser = User.Identity.Name;
            meHojaptomedDTO.Lastdate = DateTime.Now;

            importacionAppServicio.InsertarHojaPtoMed(meHojaptomedDTO, emprCodi);

            //- Insertando el punto en la tabla de Suministradores del Punto
            //- ============================================================

            //- Obteniendo la Empresa Suministradora
            SiEmpresaDTO siEmpresaDTOSuministradora = importacionAppServicio.GetSiEmpresaByCodOsinergmin(codigoEmpresaSuministradora);

            int mes = Int32.Parse(pSicliCodi.Substring(0, 2));
            int anio = Int32.Parse(pSicliCodi.Substring(2, 4));
            DateTime fechaProceso = new DateTime(anio, mes, 1);

            MePtosuministradorDTO mePtosuministradorDTO = new MePtosuministradorDTO();
            mePtosuministradorDTO.Ptomedicodi = ptoMediCodi;
            mePtosuministradorDTO.Emprcodi = siEmpresaDTOSuministradora.Emprcodi;
            mePtosuministradorDTO.Ptosufechainicio = fechaProceso;
            mePtosuministradorDTO.Ptosuusucreacion = User.Identity.Name;
            mePtosuministradorDTO.Ptosufeccreacion = DateTime.Now;

            importacionAppServicio.InsertarPtoSuministrador(mePtosuministradorDTO);
        }

        //- alpha.HDT - 24/08/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite ajustar la cadena al tamaño máximo posible.
        /// </summary>
        /// <param name="tamanioMaximo"></param>
        /// <param name="cadenaAjustar"></param>
        /// <returns></returns>
        static string ajustarTamanioTexto(int tamanioMaximo, string suministro, string barra, string tension)
        {
            string cadenaFormateada = suministro.Trim() + " - " + barra.Trim() + " - " + tension.Trim();
            int tamanioActual = cadenaFormateada.Length;

            if (tamanioActual > tamanioMaximo)
            {
                int diferencia = tamanioActual - tamanioMaximo;
                barra = barra.Substring(0, barra.Length - diferencia).Trim();
                cadenaFormateada = suministro + " - " + barra + " - " + tension;
            }

            return cadenaFormateada;
        }

        //- alpha.HDT - 23/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la Barra Osinergmin.
        /// </summary>
        /// <param name="barrCodiOsinergmin"></param>
        /// <returns></returns>
        private barraDTO obtenerBarra(string barrCodiOsinergmin)
        {
            barraDTO barraDTOEncontrado = null;

            if (this.lbarraDTO == null)
            {
                SincronizaMaestroAppServicio sincMaestroAppServicio = new SincronizaMaestroAppServicio();
                this.lbarraDTO = sincMaestroAppServicio.ObtenerBarrasOsinergmin();
            }

            var barrasOsiFiltradas = from barrasOsi in this.lbarraDTO
                                     where barrasOsi.codBarra.Trim() == barrCodiOsinergmin
                                     select barrasOsi;

            if (barrasOsiFiltradas.Count() > 0)
            {
                barraDTOEncontrado = barrasOsiFiltradas.First();
            }

            return barraDTOEncontrado;
        }


        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite abrir o cerrar el periodo.
        /// </summary>
        /// <param name="pSicliCodi"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult abrirCerrarPeriodo(int pSicliCodi, string accion)
        {
            IioPeriodoSicliDTO iioPeriodoSicliDTO = importacionAppServicio.PeriodoGetByCodigo(pSicliCodi);
            EntidadImportacionModel model = new EntidadImportacionModel();
            string mensajePostAccion = "";

            if (iioPeriodoSicliDTO == null)
            {
                model.resultado = 0;
                model.mensaje = "El Periodo no existe";

                return Json(model);
            }

            if (accion == "Cerrar")
            {
                if (iioPeriodoSicliDTO.PSicliCerrado == "1")
                {
                    model.resultado = 0;
                    model.mensaje = "El Periodo está cerrado";

                    return Json(model);
                }
                else
                {
                    iioPeriodoSicliDTO.PSicliCerrado = "1";
                    mensajePostAccion = "El Periodo ha sido cerrado";
                }
            }
            else if (accion == "Abrir")
            {
                if (iioPeriodoSicliDTO.PSicliCerrado == "0")
                {
                    model.resultado = 0;
                    model.mensaje = "El Periodo está abierto";

                    return Json(model);
                }
                else
                {
                    iioPeriodoSicliDTO.PSicliCerrado = "0";
                    mensajePostAccion = "El Periodo ha sido abierto";
                }
            }

            importacionAppServicio.PeriodoUpdate(iioPeriodoSicliDTO);

            model.resultado = 1;
            model.mensaje = mensajePostAccion;

            return Json(model);
        }

        //mejoras SICLI PR16 08/07/2019
        /// <summary>
        /// Carga archivo excel con un nombre temporal en la ruta configurada en el Archivo de Configuración
        /// </summary>
        /// <returns></returns>
        public ActionResult Subir()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var archivo = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.NombreArchivo = fileRandom + "." + NombreArchivoIntercambioOsinergmin.ExtensionFileUploadRechazoCarga;
                    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.NombreArchivo;
                    this.RutaCompletaArchivo = ruta;
                    archivo.SaveAs(ruta);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LeerExcelSubido(string periodo)
        {
            try
            {
                int anho = Int32.Parse(periodo.Substring(3));
                int mes = Int32.Parse(periodo.Substring(0, 2));
                DateTime fech = new DateTime(anho, mes, 1);
                string per = fech.ToString("yyyyMM");
                
                Respuesta respuesta;
                var listaInformacionSicli = FormatoHelperOsinergmin.LeerExcelInformacionBase(this.RutaCompletaArchivo, per, out respuesta);
                if (respuesta.Exito)
                {
                    //Se elimina los registros del periodo a cargar
                    this.importacionAppServicio.DeleteSicliOsigFactura(per);


                    //Se obtiene el MaxId de la tabla IIO_SICLI_OSIG_FACTURA 
                    var idSicliOsigFactura = this.importacionAppServicio.ObtenerSicliOsigFacturaMaximoId();

                    //Se procede a hacer la insercion de cada registro en la tabla IIO_SICLI_OSIG_FACTURA 
                    var usuario = User.Identity.Name;

                    foreach (var registroSicli in listaInformacionSicli)
                    {
                        //var equipo = listaEquipos.Where(p => p.Osinergcodi == demanda.Osinergcodi).ToList();
                        registroSicli.Clofaccodi = idSicliOsigFactura;

                        registroSicli.Clofacusucreacion = usuario;
                        registroSicli.Clofacfeccreacion = DateTime.Now;

                        this.importacionAppServicio.SaveSicliOsigFactura(registroSicli);
                        idSicliOsigFactura++;

                    }

                    FormatoHelperOsinergmin.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta { Exito = true });
                }
                else
                {
                    return Json(respuesta);
                }
            }
            catch (Exception ex)
            {
                Log.Error("LeerExcelSubido", ex);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        #region Reportes


        public JsonResult GenerarReporteComparativoCliente(string periodo)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoComparativoCliente(periodo);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error("GenerarReporteComparativoCliente", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }
        private string GenerarArchivoComparativoCliente(string periodo)
        {
            //var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            var preNombre = "Reporte_Comp_Cliente_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fech = new DateTime(anho, mes, 1);
            string per = fech.ToString("yyyyMM");

            //var perianiomes = servicio.GetPeriodoMes(pericodi);

            var totalOsinergmin = importacionAppServicio.GetCountTotal(per);
            var totalWS = importacionAppServicio.GetCountTotalFactura(per);

            var listRepCompCliente = importacionAppServicio.ListRepCompCliente(per);

            //var anio = perianiomes.ToString().Substring(0, 4);
            //var mes = perianiomes.ToString().Substring(4, 2);
            //var fechaHoraInicio = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);
            //fechaHoraInicio = fechaHoraInicio.AddMinutes(15);

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var contFila = 7;
                var item = 1;
                var nombreHojaAgentes = "Rep.Comp.Cliente";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);

                //contFila = 6;
                //var contItem = 3;

                ws.Cells[2, 2].Value = "EMPRESAS";
                ws.Cells[3, 2].Value = "EMPRESAS";
                ws.Cells[2, 3].Value = "ACCESS";
                ws.Cells[3, 3].Value = "WEB SERVICE";
                ws.Cells[2, 4].Value = totalOsinergmin;
                ws.Cells[3, 4].Value = totalWS;

                ws.Cells[5, 4].Value = "AnnioMes";
                ws.Cells[5, 6].Value = "Empresa";
                ws.Cells[5, 8].Value = "Ruc";
                ws.Cells[5, 10].Value = "NombreCliente";
                ws.Cells[5, 12].Value = "TensionEntrega";
                ws.Cells[5, 14].Value = "ConsumoPHP_BE";
                ws.Cells[5, 16].Value = "ConsumoPFP_BE";

                ws.Cells[6, 2].Value = "ITEM";
                ws.Cells[6, 3].Value = "CodCliente";
                ws.Cells[6, 4].Value = "ACCESS";
                ws.Cells[6, 5].Value = "WEB SERVICE";
                ws.Cells[6, 6].Value = "ACCESS";
                ws.Cells[6, 7].Value = "WEB SERVICE";
                ws.Cells[6, 8].Value = "ACCESS";
                ws.Cells[6, 9].Value = "WEB SERVICE";
                ws.Cells[6, 10].Value = "ACCESS";
                ws.Cells[6, 11].Value = "WEB SERVICE";
                ws.Cells[6, 12].Value = "ACCESS";
                ws.Cells[6, 13].Value = "WEB SERVICE";
                ws.Cells[6, 14].Value = "ACCESS";
                ws.Cells[6, 15].Value = "WEB SERVICE";
                ws.Cells[6, 16].Value = "ACCESS";
                ws.Cells[6, 17].Value = "WEB SERVICE";

                //ws.Cells[2, 2].Value = "HISTÓRICO DE COSTOS MARGINALES (MW.h) EN INTERVALOS DE 15 min.";


                //var culture = new System.CurrCultureInfo( "en-GB" );
                //ws.Cells[3, 2].Value = fechaHoraInicio.ToString("MM/yyyy");
                //var columnaInicio = 1;
                //var cambioFecha = false;
                using (listRepCompCliente)
                {
                    while (listRepCompCliente.Read())
                    {
                        //incremetar numero de fila
                       

                        ws.Cells[contFila, 2].Value = item;
                        ws.Cells[contFila, 3].Value = listRepCompCliente["CLOFACCODCLIENTE"].ToString().Trim();

                        ws.Cells[contFila, 4].Value = listRepCompCliente["CLOFACANHIOMES_OSIG"] != null ?
                            listRepCompCliente["CLOFACANHIOMES_OSIG"].ToString().Trim() : "";
                        ws.Cells[contFila, 5].Value = listRepCompCliente["CLOFACANHIOMES_WEBS"] != null ?
                            listRepCompCliente["CLOFACANHIOMES_WEBS"].ToString().Trim() : "";

                        ws.Cells[contFila, 6].Value = listRepCompCliente["CLOFACCODEMPRESA_OSIG"] != null ?
                           listRepCompCliente["CLOFACCODEMPRESA_OSIG"].ToString().Trim() : "";
                        ws.Cells[contFila, 7].Value = listRepCompCliente["CLOFACCODEMPRESA_WEBS"] != null ?
                            listRepCompCliente["CLOFACCODEMPRESA_WEBS"].ToString().Trim() : "";

                        ws.Cells[contFila, 8].Value = listRepCompCliente["CLOFACRUC_OSIG"] != null ?
                           listRepCompCliente["CLOFACRUC_OSIG"].ToString().Trim() : "";
                        ws.Cells[contFila, 9].Value = listRepCompCliente["CLOFACRUC_WEBS"] != null ?
                            listRepCompCliente["CLOFACRUC_WEBS"].ToString().Trim() : "";

                        ws.Cells[contFila, 10].Value = listRepCompCliente["CLOFACNOMCLIENTE_OSIG"] != null ?
                           listRepCompCliente["CLOFACNOMCLIENTE_OSIG"].ToString().Trim() : "";
                        ws.Cells[contFila, 11].Value = listRepCompCliente["CLOFACNOMCLIENTE_WEBS"] != null ?
                            listRepCompCliente["CLOFACNOMCLIENTE_WEBS"].ToString().Trim() : "";

                        ws.Cells[contFila, 12].Value = listRepCompCliente["CLOFACTENSIONENTREGA_OSIG"] != null
                             && !string.IsNullOrEmpty(listRepCompCliente["CLOFACTENSIONENTREGA_OSIG"].ToString()) ?
                           Convert.ToDecimal(listRepCompCliente["CLOFACTENSIONENTREGA_OSIG"].ToString()) : Decimal.Zero;

                        ws.Cells[contFila, 13].Value = listRepCompCliente["CLOFACTENSIONENTREGA_WEBS"] != null
                              && !string.IsNullOrEmpty(listRepCompCliente["CLOFACTENSIONENTREGA_WEBS"].ToString()) ?
                           Convert.ToDecimal(listRepCompCliente["CLOFACTENSIONENTREGA_WEBS"].ToString()) : Decimal.Zero;

                        ws.Cells[contFila, 14].Value = listRepCompCliente["CLOFACPHPBE_OSIG"] != null
                             && !string.IsNullOrEmpty(listRepCompCliente["CLOFACPHPBE_OSIG"].ToString()) ?
                           Convert.ToDecimal(listRepCompCliente["CLOFACPHPBE_OSIG"].ToString()) : Decimal.Zero;

                        ws.Cells[contFila, 15].Value = listRepCompCliente["CLOFACPHPBE_WEBS"] != null
                              && !string.IsNullOrEmpty(listRepCompCliente["CLOFACPHPBE_WEBS"].ToString()) ?
                           Convert.ToDecimal(listRepCompCliente["CLOFACPHPBE_WEBS"].ToString()) : Decimal.Zero;
                        
                        ws.Cells[contFila, 16].Value = listRepCompCliente["CLOFACPFPBE_OSIG"] != null
                            && !string.IsNullOrEmpty(listRepCompCliente["CLOFACPFPBE_OSIG"].ToString()) ?
                           Convert.ToDecimal(listRepCompCliente["CLOFACPFPBE_OSIG"].ToString()) : Decimal.Zero;

                        ws.Cells[contFila, 17].Value = listRepCompCliente["CLOFACPFPBE_WEBS"] != null
                              && !string.IsNullOrEmpty(listRepCompCliente["CLOFACPFPBE_WEBS"].ToString()) ?
                           Convert.ToDecimal(listRepCompCliente["CLOFACPFPBE_WEBS"].ToString()) : Decimal.Zero;

                        ws.Cells[contFila, 12, contFila, 17].Style.Numberformat.Format = "#,##0.0000;-#,##0.0000;0";

                        contFila++;
                        item++;
                    }

                }

                ws.Column(1).Width = 10;
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 25;
                ws.Column(11).Width = 25;
                ws.Column(12).Width = 15;
                ws.Column(13).Width = 15;
                ws.Column(14).Width = 15;
                ws.Column(15).Width = 15;
                ws.Column(16).Width = 15;
                ws.Column(17).Width = 15;

                              
                ws.Cells[5, 4, 5, 5].Merge = true;
                ws.Cells[5, 6, 5, 7].Merge = true;
                ws.Cells[5, 8, 5, 9].Merge = true;
                ws.Cells[5, 10, 5, 11].Merge = true;
                ws.Cells[5, 12, 5, 13].Merge = true;
                ws.Cells[5, 14, 5, 15].Merge = true;
                ws.Cells[5, 16, 5, 17].Merge = true; 
               
                ExcelRange rg1 = ws.Cells[6, 2, 6,17];
                ObtenerEstiloCelda(rg1, 1);

                rg1 = ws.Cells[5, 4, 5, 17];
                ObtenerEstiloCelda(rg1, 1);

                xlPackage.Save();
            }

            return fileName;
        }

        public JsonResult GenerarReporteComparativoEmpresa(string periodo)
        {
            string fileName = "";
            try
            {
                
                fileName = GenerarArchivoComparativoEmpresa(periodo);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error("GenerarReporteComparativoCliente", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }

        private string GenerarArchivoComparativoEmpresa(string periodo)
        {
            
            var preNombre = "Reporte_Comp_Empresa_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fech = new DateTime(anho, mes, 1);
            string per = fech.ToString("yyyyMM");

            

            var totalOsinergmin = importacionAppServicio.GetCountTotalRuc(per);
            var totalWS = importacionAppServicio.GetCountTotalFacturaRuc(per);

            var listRepCompCliente = importacionAppServicio.ListRepCompEmpresa(per);            

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var contFila = 7;
                var item = 1;
                var nombreHojaAgentes = "Rep.Comp.Empresa";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);
                               

                ws.Cells[2, 2].Value = "EMPRESAS";
                ws.Cells[3, 2].Value = "EMPRESAS";
                ws.Cells[2, 3].Value = "ACCESS";
                ws.Cells[3, 3].Value = "WEB SERVICE";
                ws.Cells[2, 4].Value = totalOsinergmin;
                ws.Cells[3, 4].Value = totalWS;               

                ws.Cells[6, 2].Value = "ITEM";
                ws.Cells[6, 3].Value = "RUC";
                ws.Cells[6, 4].Value = "Empresa";
                
                ws.Cells[6, 5].Value = "ACCESS";
                ws.Cells[6, 6].Value = "WEB SERVICE";                

               
                using (listRepCompCliente)
                {
                    while (listRepCompCliente.Read())
                    {
                        //incremetar numero de fila


                        ws.Cells[contFila, 2].Value = item;
                        ws.Cells[contFila, 3].Value = listRepCompCliente["CLOFACRUC"].ToString().Trim();
                        ws.Cells[contFila, 4].Value = listRepCompCliente["CLOFACNOMCLIENTE_WEBS"] != null ?
                            listRepCompCliente["CLOFACNOMCLIENTE_WEBS"].ToString().Trim() : "";

                        ws.Cells[contFila, 5].Value = listRepCompCliente["CANTIDAD_OSIG"] != null
                            && !string.IsNullOrEmpty(listRepCompCliente["CANTIDAD_OSIG"].ToString()) ?
                           Convert.ToInt32(listRepCompCliente["CANTIDAD_OSIG"].ToString()) : 0;

                        ws.Cells[contFila, 6].Value = listRepCompCliente["CANTIDAD_WEBS"] != null &&
                            !string.IsNullOrEmpty(listRepCompCliente["CANTIDAD_WEBS"].ToString()) ?
                           Convert.ToInt32(listRepCompCliente["CANTIDAD_WEBS"].ToString()) : 0;

                        ws.Cells[contFila, 5, contFila, 6].Style.Numberformat.Format = "#,##0;-#,##0;0";

                        contFila++;
                        item++;
                    }

                }

                ws.Column(1).Width = 10;
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 40;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                
                ExcelRange rg1 = ws.Cells[6, 2, 6, 6];
                ObtenerEstiloCelda(rg1, 1);

                xlPackage.Save();
            }

            return fileName;
        }
        public JsonResult GenerarReporteComparativoHistorico(string periodo)
        {
            string fileName = "";
            try
            {
                
                fileName = GenerarArchivoComparativoHistorico(periodo);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error("GenerarReporteComparativoCliente", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }

        private string GenerarArchivoComparativoHistorico(string periodo)
        {
            
            var preNombre = "Reporte_Comp_Historico_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fechaFin = new DateTime(anho, mes, 1);
            string per = fechaFin.ToString("yyyyMM");
            DateTime fechaInicio = fechaFin.AddMonths(-11);           

            var listRepCompCliente = importacionAppServicio.ListRepCompHistorico(fechaInicio, fechaFin);           

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var contFila = 7;
                var item = 1;
                var nombreHojaAgentes = "Rep.Comp.Historico";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);               

                ws.Cells[2, 2].Value = "EMPRESAS";
                ws.Cells[3, 2].Value = "EMPRESAS";
                ws.Cells[2, 3].Value = "ACCESS";
                ws.Cells[3, 3].Value = "WEB SERVICE";     
                
                ws.Cells[6, 2].Value = "ITEM";
                ws.Cells[6, 3].Value = "CodCliente";
                ws.Cells[6, 4].Value = "Suministrador";

                for (int k = 0; k <= 11; k++)
                {
                    ws.Cells[6, 5 + k].Value = "PS_" + fechaInicio.AddMonths(k).ToString("yyyyMM");                    
                }

                for (int k = 0; k <= 11; k++)
                {
                    ws.Cells[6, 17 + k].Value = "MD_" + fechaInicio.AddMonths(k).ToString("yyyyMM");
                }

                
                using (listRepCompCliente)
                {
                    while (listRepCompCliente.Read())
                    {                      

                        ws.Cells[contFila, 2].Value = item;
                        ws.Cells[contFila, 3].Value = listRepCompCliente["EMPRCODOSINERGMIN"].ToString().Trim();

                        ws.Cells[contFila, 4].Value = listRepCompCliente["NOMBRESUMINISTRADOR"] != null ?
                            listRepCompCliente["NOMBRESUMINISTRADOR"].ToString().Trim() : "";

                        for (int i = 0; i <= 11; i++)
                        {
                            var nombreColumna = "PS_" + fechaInicio.AddMonths(i).ToString("yyyyMM");
                            ws.Cells[contFila, 5 + i].Value = listRepCompCliente[nombreColumna] != null 
                                && !string.IsNullOrEmpty(listRepCompCliente[nombreColumna].ToString()) ?
                            Convert.ToDecimal(listRepCompCliente[nombreColumna].ToString()) : Decimal.Zero;

                            ws.Cells[contFila, 5 + i].Style.Numberformat.Format = "#,##0.0000;-#,##0.0000;0";
                        }

                        for (int i = 0; i <= 11; i++)
                        {
                            var nombreColumna = "MD_" + fechaInicio.AddMonths(i).ToString("yyyyMM");
                            ws.Cells[contFila, 17 + i].Value = listRepCompCliente[nombreColumna] != null 
                                && !string.IsNullOrEmpty(listRepCompCliente[nombreColumna].ToString()) ?
                            Convert.ToDecimal(listRepCompCliente[nombreColumna].ToString()) : Decimal.Zero;

                            ws.Cells[contFila, 17 + i].Style.Numberformat.Format = "#,##0.0000;-#,##0.0000;0";
                        }     

                        contFila++;
                        item++;
                    }

                }

                ws.Column(1).Width = 10;
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 40;

                for (int i = 5; i <= 28; i++)
                {
                    ws.Column(i).Width = 15;
                }    

                ExcelRange rg1 = ws.Cells[6, 2, 6, 28];
                ObtenerEstiloCelda(rg1, 1);

                xlPackage.Save();
            }

            return fileName;
        }
        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                //rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;
                string colorborder = "#245C86";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }

        [HttpGet]
        public virtual ActionResult DescargarFormato(string file)
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, file);
        }

        public JsonResult GenerarReporte(string periodo, int tipoReporte)
        {
            string fileName = "";
            try
            {
                switch (tipoReporte)
                {
                    case 1: fileName = GenerarArchivoComparativoCliente(periodo); break;
                    case 2: fileName = GenerarArchivoComparativoEmpresa(periodo); break;
                    case 3: fileName = GenerarArchivoComparativoHistorico(periodo); break;
                }
                
                //indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }

        #endregion

        public JsonResult ProcesarEnvioCoes(string periodo)
        {
            var mensaje = "";
            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fechaFin = new DateTime(anho, mes, 1);
            string per = fechaFin.ToString("yyyyMM");

            IioPeriodoSicliDTO iioPeriodoSicliDTO = importacionAppServicio.PeriodoGetById(new IioPeriodoSicliDTO { PsicliAnioMesPerrem = per });

            List<IioPeriodoSicliDTO> lstPeriodos = importacionAppServicio.PeriodoGetByCriteria(fechaFin.Year.ToString());

            if (lstPeriodos.Where(p => p.PsicliAnioMesPerrem == per).Count() > 0)
            {
                //iioPeriodoSicliDTO = lstPeriodos.Where(p => p.PsicliAnioMesPerrem == per).FirstOrDefault();

                var empresasProcesar = lstPeriodos.Where(p => p.PsicliAnioMesPerrem == per).FirstOrDefault().TablasEmpresasProcesar;

                if(empresasProcesar > 0)
                {
                    mensaje = "Existe información incompleta en el periodo. Revisar.";
                    return Json(mensaje);
                }
            }


            var res = string.Empty;
            try
            {                
                //realizamos el proceso de migracion
                
                importacionAppServicio.MigrateMeMedicion96OsigConsumo(IdLectura, ConstantesIntercambioOsinergmin.tipoInfoCodi, per, iioPeriodoSicliDTO.PsicliCodi, string.Empty);

                //grabacion en tabla IIO_Factura
                importacionAppServicio.SaveIioFactura(iioPeriodoSicliDTO.PsicliCodi, string.Empty);

                //actualizamos la fecha de sincronizacion                
                importacionAppServicio.ActualizarPeriodoFechaSincCoes(iioPeriodoSicliDTO.PsicliCodi);

                mensaje = "OK";
            }
            catch (Exception ex)
            {
                Log.Error("ProcesarEnvioCoes", ex);
                mensaje = "Error";
            }

            return Json(mensaje);
        }

        public JsonResult ObtenerEmpresasPendientes(string periodo)
        {
            var mensaje = "0";
            int anho = Int32.Parse(periodo.Substring(3));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fechaFin = new DateTime(anho, mes, 1);
            string per = fechaFin.ToString("yyyyMM");

            List<IioPeriodoSicliDTO> lstPeriodos = importacionAppServicio.PeriodoGetByCriteria(fechaFin.Year.ToString());
            IioPeriodoSicliDTO iioPeriodoSicliDTO = null;

            if(lstPeriodos.Where(p=>p.PsicliAnioMesPerrem == per).Count() > 0)
            {
                iioPeriodoSicliDTO = lstPeriodos.Where(p => p.PsicliAnioMesPerrem == per).FirstOrDefault();

                mensaje = iioPeriodoSicliDTO.TablasEmpresasProcesar.ToString()+"@";
                if (iioPeriodoSicliDTO.PSicliFecSincronizacion != DateTime.MinValue)
                {
                    mensaje = mensaje + iioPeriodoSicliDTO.PSicliFecSincronizacion.ToString("dd/MM/yyyy HH:mm");
                }
            }            

            return Json(mensaje);
        }

    }

}
