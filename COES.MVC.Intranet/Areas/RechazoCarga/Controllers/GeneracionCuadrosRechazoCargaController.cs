using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.RechazoCarga;

using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;

using System.Configuration;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class GeneracionCuadrosRechazoCargaController : BaseController
    {
        //
        // GET: /RechazoCarga/CuadrorRechazoCarga/

        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        AnalisisFallasAppServicio servAF = new AnalisisFallasAppServicio();
        private const string estadoRegistroNoEliminado = "1";
        private const string nombreReporteDescarga = "ReporteCuadroRechazoCarga.xlsx";
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(GeneracionCuadrosRechazoCargaController));

        public GeneracionCuadrosRechazoCargaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("GeneracionCuadrosRechazoCargaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("GeneracionCuadrosRechazoCargaController", ex);
                throw;
            }
        }
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            GeneracionCuadrosRechazoCargaModel model = new GeneracionCuadrosRechazoCargaModel();
            model.ListConfiguracionProg = servicio.ListConfiguracionProg();
            var listaHorizonte = servicio.ListHorizonteProg();
            //model.ListHorizonteProg = servicio.ListHorizonteProg();
            model.ListEstadoCuadroProg = servicio.ListEstadoCuadroProg().OrderBy(p=>p.Rcestacodi).ToList();

            var perfil = 1; //Por defecto Permiso es SPR
            //bool AccesoAdicional = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Adicional);
            bool permisoSCO = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.PermisoSCO);
            bool permisoSEV = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.PermisoSEV);
            if (permisoSCO)
            {
                perfil = 2;
            }else if (permisoSEV)
            {
                perfil = 3;
            }

            model.ListHorizonteProg = listaHorizonte;
            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            //model.bNuevo = AccesoNuevo;

            List<RcaProgramaDTO> programas = new List<RcaProgramaDTO>();
            programas.Add(new RcaProgramaDTO { Rcprogcodi = 0, Rcprognombre = "-- SELECCIONE --" });
            var listaProgramas = servicio.ListProgramasRechazoCarga(true);
            if (listaProgramas.Any())
            {
                programas.AddRange(listaProgramas);
            }            

            model.Programas = programas;

            model.bNuevo = true;
            //model.bAdicional = permisoSCO;

            var semanaInicial = COES.Base.Tools.Util.GenerarNroSemana(DateTime.Now, FirstDayOfWeek.Saturday);

            model.SemanaActual = semanaInicial;

            model.Perfil = perfil;

            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListarGeneracionCuadroPrograma(string horizonte, string configuracion, string estado, string fecIni, 
            string fecFin, string energiaRechazadaInicio, string energiaRechazadaFin, int perfil, int sinPrograma)
        {
            GeneracionCuadrosRechazoCargaModel model = new GeneracionCuadrosRechazoCargaModel();

            model.ListCuadroProgramaDetalle = servicio.ListRcaCuadroProgFiltro(horizonte, configuracion, estado, fecIni, fecFin, energiaRechazadaInicio, energiaRechazadaFin, sinPrograma);
            //model.ListCuadroProgramaCabecera = model.ListCuadroProgramaDetalle.GroupBy(zz => zz.Rcprogcodi).Select(p=>p.FirstOrDefault()).ToList();

            model.bAdicional = (perfil > 0);
            model.Perfil = perfil;

            return PartialView("ListarGeneracionCuadroPrograma", model);
        }

        public ActionResult EliminarCuadroProgramacion(int rccuadCodi)
        {
            this.servicio.DeleteRcaCuadroProg(rccuadCodi);

            return Json(new { success = true, message = "Ok" });
        }
        public ActionResult NoEjecutarCuadroProgramacion(int rccuadCodi)
        {
            var cuadroProg = new RcaCuadroProgDTO();

            cuadroProg.Rccuadcodi = rccuadCodi;
            cuadroProg.Rcestacodi = ConstantesRechazoCarga.EstadoCuadroNoEjecutado;
            cuadroProg.Rccuadfecmodificacion = DateTime.Now;
            cuadroProg.Rccuadusumodificacion = User.Identity.Name;

            this.servicio.UpdateRcaCuadroProgEstado(cuadroProg);

            return Json(new { success = true, message = "Ok" });
        }

        public ActionResult GenerarCopiaCuadroPrograma(int codigoCuadroPrograma, int tipoPrograma,int codigoProgramaDuplicar, int horizonte, string fechaMensual, string fechaDiaria, int semana,
            string fechaHoraInicio, string fechaHoraFin) 
        {
            try
            {
                if (tipoPrograma.Equals(1))
                {
                    #region Programa Existente

                    var cuadroPrograma = servicio.GetByIdRcaCuadroProg(codigoCuadroPrograma);
                    var listaCuadroProgamaUsuario = servicio.ListProgramaRechazoCarga("", codigoCuadroPrograma.ToString());

                    //1. Creamos el nuevo cuadro asociado al programa seleccionado
                    cuadroPrograma.Rcprogcodi = codigoProgramaDuplicar;
                    cuadroPrograma.Rccuadfechorinicio = DateTime.ParseExact(fechaHoraInicio, "dd/MM/yyyy HH:mm", null);
                    cuadroPrograma.Rccuadfechorfin = DateTime.ParseExact(fechaHoraFin, "dd/MM/yyyy HH:mm", null);
                    cuadroPrograma.Rcestacodi = cuadroPrograma.Rcestacodi == ConstantesRechazoCarga.EstadoCuadroReprogramado ?
                        ConstantesRechazoCarga.EstadoCuadroReprogramado : ConstantesRechazoCarga.EstadoCuadroProgramado;
                    cuadroPrograma.Rccuadmotivo = "Cuadro Duplicado - " + cuadroPrograma.Rccuadmotivo;
                    codigoCuadroPrograma = servicio.SaveRcaCuadroProg(cuadroPrograma);

                    //2. Creamos los cuadros de Programa de Usuario asociado al nuevo programa y cuadro de programa

                    foreach(var registro in listaCuadroProgamaUsuario)
                    {
                        registro.Rccuadcodi = codigoCuadroPrograma;
                        registro.Rcprouusucreacion = User.Identity.Name;
                        registro.Rcproufeccreacion = DateTime.Now;
                        registro.Rcprouestregistro = "1";

                        servicio.SaveRcaCuadroProgUsuario(registro);
                    }

                    #endregion

                }else if (tipoPrograma.Equals(2))
                {
                    #region Nuevo Programa

                    //1. Validacion si el programa ya existe
                    var programaAbrev = GenerarCodigoPrograma(horizonte, fechaMensual, semana, fechaDiaria);

                    if (!string.IsNullOrEmpty(programaAbrev))
                    {
                        var programaExistente = servicio.GetByCriteriaRcaProgramas(programaAbrev);
                        if (programaExistente.Count > 0)
                        {
                            return Json(new { success = false, message = "El codigo de Programa ya Existe. Revisar." });
                        }
                    }

                    //2. Creacion del programa
                    RcaProgramaDTO rcaProgramaDTO = new RcaProgramaDTO();

                    rcaProgramaDTO.Rchorpcodi = horizonte;
                    rcaProgramaDTO.Rcprogabrev = programaAbrev;
                    //rcaProgramaDTO.Rcprognombre = nombrePrograma;
                    rcaProgramaDTO.Rcprogestado = "1";
                    rcaProgramaDTO.Rcprogestregistro = estadoRegistroNoEliminado;
                    rcaProgramaDTO.Rcprogusucreacion = User.Identity.Name;
                    rcaProgramaDTO.Rcprogfeccreacion = DateTime.Now;
                    rcaProgramaDTO.Rcprogusumodificacion = User.Identity.Name;
                    rcaProgramaDTO.Rcprogfecmodificacion = DateTime.Now;
                    rcaProgramaDTO.Rcprogcodipadre = 0;

                    if (horizonte.Equals(ConstantesRechazoCarga.HorizonteMensual) && !string.IsNullOrEmpty(fechaMensual))
                    {
                        var datos = fechaMensual.Split('-');
                        var fechaInicio = new DateTime(Convert.ToInt32(datos[0]), Convert.ToInt16(datos[1]), 1);
                        var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                        rcaProgramaDTO.Rcprogfecinicio = fechaInicio;
                        rcaProgramaDTO.Rcprogfecfin = fechaFin;
                    }
                    else if (horizonte.Equals(ConstantesRechazoCarga.HorizonteSemanal) && semana > 0)
                    {
                        var fechaSemanaInicial = COES.Base.Tools.Util.GenerarFecha(DateTime.Now.Year, semana, 0);
                        var fechaSemanaFin = fechaSemanaInicial.AddDays(6);

                        rcaProgramaDTO.Rcprogfecinicio = fechaSemanaInicial;
                        rcaProgramaDTO.Rcprogfecfin = fechaSemanaFin;
                    }
                    else if ((horizonte.Equals(ConstantesRechazoCarga.HorizonteDiario) || horizonte.Equals(ConstantesRechazoCarga.HorizonteReprograma)) && !string.IsNullOrEmpty(fechaDiaria))
                    {
                        DateTime fecha = DateTime.ParseExact(fechaDiaria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        rcaProgramaDTO.Rcprogfecinicio = fecha;
                        rcaProgramaDTO.Rcprogfecfin = fecha;
                    }

                    codigoProgramaDuplicar = servicio.SaveRcaPrograma(rcaProgramaDTO);

                    
                    var cuadroPrograma = servicio.GetByIdRcaCuadroProg(codigoCuadroPrograma);
                    var listaCuadroProgamaUsuario = servicio.ListProgramaRechazoCarga("", codigoCuadroPrograma.ToString());
                    
                    //3. Creamos el nuevo cuadro asociado al programa creado
                    cuadroPrograma.Rcprogcodi = codigoProgramaDuplicar;
                    cuadroPrograma.Rccuadfechorinicio = DateTime.ParseExact(fechaHoraInicio, "dd/MM/yyyy HH:mm", null);
                    cuadroPrograma.Rccuadfechorfin = DateTime.ParseExact(fechaHoraFin, "dd/MM/yyyy HH:mm", null);
                    cuadroPrograma.Rcestacodi = ConstantesRechazoCarga.EstadoCuadroProgramado;
                    cuadroPrograma.Rccuadmotivo = "Cuadro Duplicado - " + cuadroPrograma.Rccuadmotivo;
                    codigoCuadroPrograma = servicio.SaveRcaCuadroProg(cuadroPrograma);

                    //4. Creamos los cuadros de Programa de Usuario asociado al nuevo programa y cuadro de programa

                    foreach (var registro in listaCuadroProgamaUsuario)
                    {
                        registro.Rccuadcodi = codigoCuadroPrograma;
                        registro.Rcprouusucreacion = User.Identity.Name;
                        registro.Rcproufeccreacion = DateTime.Now;
                        registro.Rcprouestregistro = "1";

                        servicio.SaveRcaCuadroProgUsuario(registro);
                    }


                    #endregion
                }
                else
                {
                    #region Sin Programa 
                    var cuadroPrograma = servicio.GetByIdRcaCuadroProg(codigoCuadroPrograma);
                    var listaCuadroProgamaUsuario = servicio.ListProgramaRechazoCarga("", codigoCuadroPrograma.ToString());

                    //1. Creamos el nuevo cuadro sin asociar a algun programa -- consultar con Miguel deshabilitar foreign key tabla cuadro programa
                    cuadroPrograma.Rcprogcodi = 0;
                    cuadroPrograma.Rccuadfechorinicio = DateTime.ParseExact(fechaHoraInicio, "dd/MM/yyyy HH:mm", null);
                    cuadroPrograma.Rccuadfechorfin = DateTime.ParseExact(fechaHoraFin, "dd/MM/yyyy HH:mm", null);
                    cuadroPrograma.Rcestacodi = ConstantesRechazoCarga.EstadoCuadroProgramado;
                    cuadroPrograma.Rccuadmotivo = "Cuadro Duplicado - " + cuadroPrograma.Rccuadmotivo;

                    codigoCuadroPrograma = servicio.SaveRcaCuadroProg(cuadroPrograma);

                    //2. Creamos los cuadros de Programa de Usuario asociado al nuevo programa y cuadro de programa

                    foreach (var registro in listaCuadroProgamaUsuario)
                    {
                        registro.Rccuadcodi = codigoCuadroPrograma;
                        registro.Rcprouusucreacion = User.Identity.Name;
                        registro.Rcproufeccreacion = DateTime.Now;
                        registro.Rcprouestregistro = "1";

                        servicio.SaveRcaCuadroProgUsuario(registro);
                    }

                    #endregion
                }               


                return Json(new { success = true, message = "Ok" });
            }
            catch(Exception ex)
            {
                log.Error("GeneracionCuadrosRechazoCargaController", ex);

                return Json(new { success = false, message = "Ocurrio un error al duplicar el cuadro." });
            }
            
        }

        private string GenerarCodigoPrograma(int horizonte, string fechaMensual, int semana, string fechaDiaria)
        {
            var codigoPrograma = string.Empty;

            if (horizonte.Equals(ConstantesRechazoCarga.HorizonteMensual) && !string.IsNullOrEmpty(fechaMensual))
            {
                var datos = fechaMensual.Split('-');
                var fechaInicio = new DateTime(Convert.ToInt32(datos[0]), Convert.ToInt16(datos[1]), 1);

                codigoPrograma = "PRC-PMI-" + fechaInicio.ToString("yyyy-MM");
            }
            else if (horizonte.Equals(ConstantesRechazoCarga.HorizonteSemanal) && semana > 0)
            {
                codigoPrograma = "PRC-PSI-" + DateTime.Now.Year + "-" + semana.ToString("");
            }
            else if ((horizonte.Equals(ConstantesRechazoCarga.HorizonteDiario) || horizonte.Equals(ConstantesRechazoCarga.HorizonteReprograma)) && !string.IsNullOrEmpty(fechaDiaria))
            {
                DateTime fecha = DateTime.ParseExact(fechaDiaria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (horizonte.Equals(ConstantesRechazoCarga.HorizonteDiario))
                {
                    codigoPrograma = "PRC-PDI-" + fecha.ToString("yyyy-MM-dd");
                }
                else
                {
                    codigoPrograma = "PRC-RDO-" + fecha.ToString("yyyy-MM-dd");
                }
            }

            return codigoPrograma;
        }

        public JsonResult GenerarReporte(string horizonte, string configuracion, string estado, string fecIni,
            string fecFin, string energiaRechazadaInicio, string energiaRechazadaFin, int perfilUsuario, int sinPrograma)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcel(horizonte, configuracion, estado, fecIni, fecFin, energiaRechazadaInicio, energiaRechazadaFin, perfilUsuario, sinPrograma);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }
        private string GenerarArchivoExcel(string horizonte, string configuracion, string estado, string fecIni,
            string fecFin, string energiaRechazadaInicio, string energiaRechazadaFin, int perfilUsuario, int sinPrograma)
        {
           
            var preNombre = "Cuadros_Programa_";
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + DateTime.Now.ToString("yyyyMMddhhmm") + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Obtenemos los cuadros
                var cuadroProgramas = servicio.ListRcaCuadroProgFiltro(horizonte, configuracion, estado, fecIni, fecFin, energiaRechazadaInicio, energiaRechazadaFin, sinPrograma);

                var contFila = 3;
                //var contHojas = 0;
                var muestraCTAF = (perfilUsuario.Equals(ConstantesRechazoCarga.PerfilSEV)) ? true : false;
                var columnaAdicional = 0;
                var nombreHoja = "REPORTE";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);

                ws.Cells[2, 1].Value = "CODIGO";
                ws.Cells[2, 2].Value = "HORIZONTE";
                ws.Cells[2, 3].Value = "MOTIVO";
                ws.Cells[2, 4].Value = "HORA INICIO";
                ws.Cells[2, 5].Value = "HORA FIN";
                ws.Cells[2, 6].Value = "CONFIGURACION";
                ws.Cells[2, 7].Value = "U. REGULADOS";
                ws.Cells[2, 8].Value = "ESTADO";

                if (muestraCTAF)
                {
                    ws.Cells[2, 9].Value = "EVENTO CTAF";
                    columnaAdicional++;
                    ws.Cells[2, 10].Value = "CREADO POR";
                    columnaAdicional++;
                }
                else
                {
                    ws.Cells[2, 9].Value = "CREADO POR";
                    columnaAdicional++;

                }

                ExcelRange rg1 = ws.Cells[2, 1, 2, 8 + columnaAdicional];
                ObtenerEstiloCelda(rg1, 1);

                foreach (var cuadroPrograma in cuadroProgramas)
                {

                    ws.Cells[contFila, 1].Value = cuadroPrograma.Rcprogabrev;
                    ws.Cells[contFila, 2].Value = cuadroPrograma.Rchorpnombre;
                    ws.Cells[contFila, 3].Value = cuadroPrograma.Rccuadmotivo;
                    ws.Cells[contFila, 4].Value = ((DateTime)cuadroPrograma.Rccuadfechorinicio).ToString("dd/MM/yyyy HH:mm");
                    ws.Cells[contFila, 5].Value = ((DateTime)cuadroPrograma.Rccuadfechorfin).ToString("dd/MM/yyyy HH:mm");
                    ws.Cells[contFila, 6].Value = cuadroPrograma.Rcconpnombre;
                    ws.Cells[contFila, 7].Value = cuadroPrograma.Rccuadflagregulado.Equals("1") ? "Si" : "No";
                    ws.Cells[contFila, 8].Value = cuadroPrograma.Rcestanombre;

                    if (muestraCTAF)
                    {
                        ws.Cells[contFila, 9].Value = cuadroPrograma.Rccuadcodeventoctaf;
                        ws.Cells[contFila, 10].Value = cuadroPrograma.Rccuadusucreacion;
                    }
                    else
                    {
                        ws.Cells[contFila, 9].Value = cuadroPrograma.Rccuadusucreacion;
                    }

                    contFila++;                  

                }

                ws.Column(1).Width = 20;
                ws.Column(2).Width = 20;
                ws.Column(3).Width = 50;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;

                if (muestraCTAF)
                {
                    ws.Column(9).Width = 20;
                    ws.Column(10).Width = 30;
                }
                else
                {
                    ws.Column(9).Width = 30;
                }

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


        [HttpPost]
        public PartialViewResult ListarEventos(string codigoEvento, string nombreEvento, int codigoCuadroPrograma)
        {
            GeneracionCuadrosRechazoCargaModel model = new GeneracionCuadrosRechazoCargaModel();

            var cuadroPrograma = servicio.GetByIdRcaCuadroProg(codigoCuadroPrograma);

            var fechaInicio = new DateTime(cuadroPrograma.Rccuadfechorinicio.Value.Year, 1, 1);

            EventoDTO oEventoDTO = new EventoDTO();
            oEventoDTO.EmpresaPropietaria = "0";
            oEventoDTO.EmpresaInvolucrada = "0";
            oEventoDTO.TipoEquipo = "0";
            oEventoDTO.Estado = "N";
            oEventoDTO.RNC = "N";
            oEventoDTO.ERACMF = "N";
            oEventoDTO.ERACMT = "N";
            oEventoDTO.EDAGSF = "N";
            //oEventoDTO.DI = DateTime.Now.AddYears(-1).ToString("dd/MM/yyyy");
            oEventoDTO.DI = fechaInicio.ToString("dd/MM/yyyy");
            oEventoDTO.DF = DateTime.Now.ToString("dd/MM/yyyy");
            oEventoDTO.EveSinDatosReportados = "T";
            oEventoDTO.ListaEmprcodi = ConstantesAppServicio.ParametroDefecto;

            List<EventoDTO> listaInterrupciones = servAF.ConsultarInterrupcionesSuministros(oEventoDTO);

            if (!string.IsNullOrEmpty(codigoEvento))
            {
                var nuevaLista = from pe in listaInterrupciones
                                 where pe.CODIGO.Contains(codigoEvento.ToUpper())
                                 select pe;

                listaInterrupciones = nuevaLista.ToList();
            }

            if (!string.IsNullOrEmpty(nombreEvento))
            {
                var nuevaLista = from pe in listaInterrupciones
                                 where pe.NOMBRE_EVENTO.Contains(nombreEvento.ToUpper())
                                 select pe;

                listaInterrupciones = nuevaLista.ToList();
            }


            model.ListEventos = listaInterrupciones;

            return PartialView("ListarEventos", model);
        }
        /// <summary>
        /// Metodo que permite asociar el evento CTAF a un cuadro de programa
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="evento"></param>
        /// <returns></returns>
        public ActionResult ActualizarCuadroProgramaEvento(int codigoCuadroPrograma, string evento)
        {
            try
            {
                RcaCuadroProgDTO cuadroPrograma = new RcaCuadroProgDTO();

                cuadroPrograma.Rccuadcodi = codigoCuadroPrograma;
                cuadroPrograma.Rccuadcodeventoctaf = evento;
                cuadroPrograma.Rccuadfecmodificacion = DateTime.Now;
                cuadroPrograma.Rccuadusumodificacion = User.Identity.Name;

                servicio.UpdateRcaCuadroProgEvento(cuadroPrograma);

                return Json(new { success = true, message = "Ok" });
            }
            catch(Exception ex)
            {
                log.Error("GeneracionCuadrosRechazoCargaController", ex);

                return Json(new { success = false, message = "Ocurrio un error al actualizar el cuadro." });
            }
        }
    }
}
