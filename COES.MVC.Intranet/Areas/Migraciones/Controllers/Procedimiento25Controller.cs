using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.Migraciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using Newtonsoft.Json;
using Novacode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Migraciones.Controllers
{
    public class Procedimiento25Controller : BaseController
    {
        MigracionesAppServicio servicio = new MigracionesAppServicio();
        PR5ReportesAppServicio servicioPR5 = new PR5ReportesAppServicio();

        #region Declaracion de variables

        private FontFamily fontDoc = new FontFamily("Arial");
        private double FactorWidth = 20.0;
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ConfiguracionesController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #endregion

        /// <summary>
        /// //Descarga el archivo excel exportado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["fi"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppXML, nombreArchivo);
        }

        #region Informes para OSINERGMIN (Mantenimientos y Horas de Operación)

        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            MigracionesModel model = new MigracionesModel();

            List<UserModel> list_ = new List<UserModel>();
            list_.Add(new UserModel() { IdArea = ConstantesMigraciones.TipoInformeEstadoOperativo90, Roles = "Estado Operativo 90 días calendario" });
            list_.Add(new UserModel() { IdArea = ConstantesMigraciones.TipoInformeMantenimientoProg, Roles = "Mantenimientos Programados 7d" });
            list_.Add(new UserModel() { IdArea = ConstantesMigraciones.TipoInformeEstadoOperativo, Roles = "Estado Operativo 30 dias" });
            model.ListaSelect = list_;
            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            model.Mesanio = DateTime.Now.ToString(ConstantesAppServicio.FormatoMesanio);

            return View(model);
        }

        /// <summary>
        /// Reporte web
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public JsonResult CargarInformacionP25(int tipo, string fecha)
        {
            MigracionesModel model = new MigracionesModel();
            DateTime f1 = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            if (ConstantesMigraciones.TipoInformeMantenimientoProg == tipo)
            {
                var lista = servicio.GetMantenimientos(f1);
                model.Resultado = servicio.InformacionP25Html(lista);
                model.nRegistros = lista.Count();
            }


            if (ConstantesMigraciones.TipoInformeEstadoOperativo90 == tipo)
            {
                //90 días hábiles
                //90/5 = 18 => 36 días no calendaríos
                //total a evaluar 90 + 36 => 126
                DateTime fechaIni = f1.Date.AddDays(-90), fechaFin = f1.AddHours(16);
                var lista = servicio.GetEstadoOpe90(fechaIni, fechaFin);
                model.Resultado = servicio.EstadoOperativoHtml(lista);
                model.nRegistros = lista.Count();
            }

            if (ConstantesMigraciones.TipoInformeEstadoOperativo == tipo)
            {
                DateTime fechaIni = f1.Date.AddDays(-30), fechaFin = f1.AddHours(16);
                var lista = servicio.GetEstadoOpe(fechaIni, fechaFin);
                model.Resultado = servicio.EstadoOperativoHtml(lista);
                model.nRegistros = lista.Count();
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(model);
        }

        /// <summary>
        /// Exportacion a archivo Excel
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GenerarReporteXls(int tipo, string fecha)
        {
            MigracionesModel model = new MigracionesModel();
            string nameFile = string.Empty;
            DateTime f1 = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                if (ConstantesMigraciones.TipoInformeMantenimientoProg == tipo)
                {
                    var lista = servicio.GetMantenimientos(f1);
                    nameFile = ConstantesMigraciones.RptInfoP25 + "_" + f1.ToString("yyyyMMdd");
                    this.servicio.GenerarArchivoExcelInfoP25(lista, f1, ruta + nameFile + ConstantesAppServicio.ExtensionExcel);
                }

                if (ConstantesMigraciones.TipoInformeEstadoOperativo == tipo)
                {
                    DateTime fechaIni = f1.Date.AddDays(-30), fechaFin = f1.AddHours(16);
                    var lista = servicio.GetEstadoOpe(fechaIni, fechaFin);
                    nameFile = ConstantesMigraciones.RptEstOpe + "_" + fechaIni.ToString("yyyyMMdd") + "_hasta_" + fechaFin.ToString("yyyyMMdd");
                    this.servicio.GenerarArchivoExcelEstOpe(lista, f1, ruta + nameFile + ConstantesAppServicio.ExtensionExcel);
                }

                if (ConstantesMigraciones.TipoInformeEstadoOperativo90 == tipo)
                {
                    //90 días hábiles
                    //90/5 = 18 => 36 días no calendaríos
                    //total a evaluar 90 + 36 => 126
                    DateTime fechaIni = f1.Date.AddDays(-90), fechaFin = f1.AddHours(16);
                    var lista = servicio.GetEstadoOpe90(fechaIni, fechaFin);
                    nameFile = ConstantesMigraciones.RptEstOpe + "_" + fechaIni.ToString("yyyyMMdd") + "_hasta_" + fechaFin.ToString("yyyyMMdd");
                    this.servicio.GenerarArchivoExcelEstOpe(lista, f1, ruta + nameFile + ConstantesAppServicio.ExtensionExcel);
                }

                model.Resultado = nameFile + ConstantesAppServicio.ExtensionExcel;
                model.nRegistros = 1;
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        #endregion

        #region GenerarReporteIDCOS

        public ActionResult CrearIDCOS()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            MigracionesModel model = new MigracionesModel();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(ConstantesBase.FormatoFechaPE);
            model.ListaGPS = servicioPR5.ListarGpsxFiltro(ConstantesAppServicio.ParametroDefecto).Where(x=>x.Gpsindieod == "S").ToList();

            return View(model);
        }

        /// <summary>
        /// Generación de Word IDCOS
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public async Task<JsonResult> GenerarIDCOS(string fecha, int gpscodi, int? checkProc25 = 0)
        {
            MigracionesModel model = new MigracionesModel();
            DateTime fecIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            //Procesar
            servicioPR5.GenerarIndicadores(fecIni);

            int nroColumn = 0;
            int nroRowData = 0;
            string strHead = string.Empty;
            List<string> listaHeadColumn;
            List<double> listaWidthColumn = null;
            string nombreDoc = string.Empty;

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                nombreDoc = ConstantesMigraciones.ReporteWordIDCOS + "_" + this.servicio.GetNumeroIeod(fecIni) + ConstantesAppServicio.ExtensionWord;
                using (DocX document = DocX.Create(ruta + nombreDoc))
                {
                    #region Header y Footer

                    document.MarginLeft = 114.0f;
                    document.MarginRight = 114.0f;

                    //Cabecera
                    document.AddHeaders();

                    Novacode.Image logo = document.AddImage(AppDomain.CurrentDomain.BaseDirectory + "Content/Images/" + Constantes.NombreLogoCoes);
                    document.DifferentFirstPage = false;
                    document.DifferentOddAndEvenPages = false;

                    Header header_first = document.Headers.odd;

                    Table header_first_table = header_first.InsertTable(2, 2);
                    header_first_table.Design = TableDesign.None;
                    header_first_table.AutoFit = AutoFit.Window;

                    //primera fila
                    Paragraph upperRightParagraph = header_first.Tables[0].Rows[0].Cells[0].Paragraphs[0];
                    upperRightParagraph.AppendPicture(logo.CreatePicture(48, 92));
                    upperRightParagraph.Alignment = Alignment.left;
                    header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Top;

                    Paragraph numIDCOS = header_first.Tables[0].Rows[0].Cells[1].Paragraphs[0];
                    numIDCOS.Append("IDCOS n.°  " + this.servicio.GetNumeroIeod(fecIni) + "/" + fecIni.Year);
                    numIDCOS.Alignment = Alignment.right;
                    numIDCOS.Font(fontDoc).FontSize(9).Bold();
                    header_first_table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Bottom;

                    //segunda fila
                    Paragraph subDireccion = header_first.Tables[0].Rows[1].Cells[0].Paragraphs[0];
                    subDireccion.Append("Sub Dirección de Coordinación");
                    subDireccion.Alignment = Alignment.left;
                    subDireccion.Font(fontDoc).FontSize(9);
                    header_first_table.Rows[1].Cells[0].VerticalAlignment = VerticalAlignment.Top;

                    Paragraph version = header_first.Tables[0].Rows[1].Cells[1].Paragraphs[0];
                    version.Append("SCO/COES-SINAC\n Versión 1");
                    version.Alignment = Alignment.right;
                    version.Font(fontDoc).FontSize(9).Bold();
                    header_first_table.Rows[1].Cells[1].VerticalAlignment = VerticalAlignment.Top;

                    header_first.InsertParagraph().AppendLine().Append(" ").Font(fontDoc);

                    //Pie de página
                    document.AddFooters();

                    Footer footer_main = document.Footers.odd;

                    Paragraph pFooter = footer_main.Paragraphs.First();
                    pFooter.Alignment = Alignment.left;
                    pFooter.Append("Página ");
                    pFooter.AppendPageNumber(PageNumberFormat.normal);
                    pFooter.Append(" de ");
                    pFooter.AppendPageCount(PageNumberFormat.normal);
                    //pFooter.Font(fontDoc).FontSize(10).Bold();

                    #endregion

                    #region Título IDCOS

                    Paragraph p = document.InsertParagraph().Font(fontDoc);
                    p.AppendLine().Font(fontDoc).Append("INFORME DIARIO DE COORDINACIÓN DE LA OPERACIÓN DEL SISTEMA").FontSize(13).Font(fontDoc).Bold();
                    p.Alignment = Alignment.center;

                    string dayname = fecIni.ToString("dddd", new CultureInfo("es-PE"));
                    string nombreMes = Base.Tools.Util.ObtenerNombreMes(fecIni.Month).ToLower();
                    nombreMes = nombreMes.Length > 1 ? nombreMes.Substring(0, 1).ToUpper() + nombreMes.Substring(1, nombreMes.Length - 1) : nombreMes;
                    string fechaTitulo = dayname.Substring(0, 1).ToUpper() + dayname.Substring(1, dayname.Length - 1) + " " + fecIni.Day + " de " + nombreMes + " del " + fecIni.Year;

                    p = document.InsertParagraph().Font(fontDoc);
                    p.AppendLine().Font(fontDoc).Append(fechaTitulo).FontSize(12).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc); ;
                    p.Alignment = Alignment.center;

                    #endregion

                    #region "1. Eventos importantes"

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "1. Eventos importantes");
                    var ListaEventosImportantes = servicio.GetListaEventosImportantes(fecIni);
                    if (ListaEventosImportantes.Count > 0)
                    {
                        nroRowData = ListaEventosImportantes.Count;
                        nroColumn = 3;
                        strHead = "Hora,Empresa,Descripción";
                        Table secuencia = document.InsertTable(ListaEventosImportantes.Count + 1, nroColumn);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), null, nroColumn);
                        this.BodyListaEventosImportantesTablaWord(ref secuencia, ListaEventosImportantes);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }
                    #endregion

                    #region "2. Resumen de la generación de las Centrales del SEIN"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "2. Resumen de la generación de las Centrales del SEIN");

                    //cuadro resumen
                    var ListaGeneCentralSein = servicio.GetListaGeneCentralSein(fecIni);

                    if (ListaGeneCentralSein.Count > 0)
                    {
                        var ListaGeneCentralSein_ = this.servicio.ResumenGeneracionCentralesSein(ListaGeneCentralSein);

                        nroRowData = ListaGeneCentralSein_.Count;
                        nroColumn = 2;
                        strHead = "Empresa,Descripción";
                        Table secuencia = document.InsertTable(ListaGeneCentralSein_.Count + 1, nroColumn);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), null, nroColumn);
                        this.BodyListaGeneCentralSeinTablaWord(ref secuencia, ListaGeneCentralSein_);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }

                    decimal valorMaximaDemanda; DateTime fechaMaxDemanda; decimal? porcentajeEjecProg; decimal factorCarga;
                    this.servicio.GetFactorCargaYMaximaDemanda(fecIni, out valorMaximaDemanda, out fechaMaxDemanda, out porcentajeEjecProg, out factorCarga);

                    //Factor de carga
                    p = document.InsertParagraph().Font(fontDoc);
                    p.AppendLine().Font(fontDoc).Append("El factor de carga ejecutado fue " + factorCarga).FontSize(11).Font(fontDoc);

                    //Máxima Demanda
                    decimal porcentajePrevisto = Math.Round(porcentajeEjecProg.GetValueOrDefault(0) * 100, 3);
                    string strMaxDemanda = string.Format("La máxima demanda del COES fue {0} MW, ", Math.Round(valorMaximaDemanda, 2));
                    string strPorcPrevisto = porcentajePrevisto < 0 ? (Math.Abs(porcentajePrevisto)).ToString() + " % menos de lo previsto "
                        : porcentajePrevisto > 0 ? (Math.Abs(porcentajePrevisto)).ToString() + " % más de lo previsto " : " similar a lo previsto ";
                    string strHora = "y ocurrió a las " + fechaMaxDemanda.ToString(ConstantesAppServicio.FormatoOnlyHora) + " h.";

                    p = document.InsertParagraph().Font(fontDoc);
                    p.AppendLine().Font(fontDoc).Append(strMaxDemanda + strPorcPrevisto + strHora).FontSize(11).Font(fontDoc);

                    #endregion

                    #region "2.1. Producción por tipo de combustible y empresa"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "2.1. Producción por tipo de combustible y empresa");

                    var listaPie = new List<MeMedicion48DTO>();
                    var js = new JavaScriptSerializer();
                    List<MeMedicion48DTO> listArea, listAreaCtg, listaProdTipCombRER, listaProdsinRER, listAreaCtgHidro, listAreaCtgGas;
                    var heighImagePX = 356;
                    var widthImagePX = 534;


                    var idempresa = "";
                    var idtipocentral = "";
                    var idtiporecurso = "";

                    //Area
                    this.servicioPR5.ListaReportePotenciaXTipoRecursoTotal48PorLectcodi(idempresa, idtipocentral, idtiporecurso, fecIni, fecIni, out listArea, out listAreaCtg, out listaProdsinRER, out listaProdTipCombRER, out listAreaCtgHidro, out listAreaCtgGas, out List<string> listaMensaje, Int32.Parse(ConstantesAppServicio.LectcodiEjecutadoHisto), false, false);
                    var dataGraficoArea = this.servicio.GraficoRecursosEnergeticosDiagramaCarga(listArea);

                    //Pie
                    listaPie = this.servicio.GetListaProduccionXEmpresa(fecIni);
                    listaPie = this.servicio.GetListaProduccionXEmpresaDataGrafico(listaPie);

                    var jsonDataArea = ObtenerJsonStringArea(dataGraficoArea);
                    var jsonDataPie = ObtenerJsonStringPie(listaPie);

                    var areaObject = js.Deserialize<dynamic>(jsonDataArea);
                    var pieObject = js.Deserialize<dynamic>(jsonDataPie);

                    var rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    var linkGraficoArea = await ObtenerLinkImagenDesdeJsonStringAsync(areaObject, rutaBase + "area.png");
                    var linkGraficoPie = await ObtenerLinkImagenDesdeJsonStringAsync(pieObject, rutaBase + "pie.png");

                    var imageArea = document.AddImage(linkGraficoArea);
                    var imagePie = document.AddImage(linkGraficoPie);

                    var tablaGraficos = document.InsertTable(2, 1);
                    tablaGraficos.AutoFit = AutoFit.Contents;
                    tablaGraficos.Design = TableDesign.TableGrid;

                    tablaGraficos.Rows[0].Cells[0].Paragraphs[0].AppendPicture(imageArea.CreatePicture(heighImagePX, widthImagePX));
                    tablaGraficos.Rows[1].Cells[0].Paragraphs[0].AppendPicture(imagePie.CreatePicture(heighImagePX, widthImagePX));

                    #endregion

                    #region "3. Costo total de la operación del día"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "3. Costo total de la operación del día");
                    var ListaCostoTotalOpe = servicio.GetListaCostoTotalOpe(fecIni, ConstantesAppServicio.LectcodiEjecutadoHisto, ConstantesAppServicio.TipoinfocodiSoles, ConstantesAppServicio.PtomedicodiCostoOpeDia);//03
                    if (ListaCostoTotalOpe.Count > 0)
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        decimal monto_ = (ListaCostoTotalOpe.First().H1 != null ? (decimal)ListaCostoTotalOpe.First().H1 : 0);
                        p.Append(string.Format("El costo de la operación del día fue S/. {0:# ### ###.##}", +monto_)).FontSize(11).Font(fontDoc);
                    }
                    #endregion

                    #region "4. Interconexiones Internacionales"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "4. Interconexiones Internacionales");
                    var ListaInterconexInterna = servicio.GetListaBitacora(fecIni, ConstantesAppServicio.SubcausacodiInterconexInterna, ConstantesAppServicio.FamcodiLinea);//04_B
                    if (ListaInterconexInterna.Count > 0)
                    {
                        nroRowData = ListaInterconexInterna.Count;
                        nroColumn = 6;
                        strHead = "País Importador,País Exportador,Equipo,Inicio,Final,Observación";
                        Table secuencia = document.InsertTable(ListaInterconexInterna.Count + 1, nroColumn);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), null, nroColumn);
                        this.BodyListaInterconexInternaTablaWord(ref secuencia, ListaInterconexInterna);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("Ninguna").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }
                    #endregion

                    #region "5. Regulación primaria (RPF) y secundaria (RSF) de frecuencia"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "5. Regulación primaria (RPF) y secundaria (RSF) de frecuencia");

                    decimal? valor = this.servicioPR5.GetMagnitudRPF(fecIni);
                    string magnitudRPF = valor != null ? String.Format("{0:0.00}", valor * 100) + "%" : string.Empty;

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("El porcentaje de magnitud de Reserva Rotante para la RPF a todas las centrales del SEIN según el PR-21 es: " + magnitudRPF).FontSize(11).Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append(" ").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Ver Anexo 11: Reporte de Regulación Secundaria de Frecuencia").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    /*
                    List<string> listaCabeceraFinal;
                    List<List<string>> listaDataFinal;

                    this.servicio.GetRPFyRSF(fecIni, out listaCabeceraFinal, out listaDataFinal);
                    if (listaDataFinal.Count > 0)
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.AppendLine().Font(fontDoc).Append("Las centrales que realizaron la Regulación Secundaria de Frecuencia fueron:").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                        nroRowData = listaDataFinal.Count;
                        nroColumn = listaCabeceraFinal.Count;
                        Table secuencia = document.InsertTable(listaDataFinal.Count + 1, listaCabeceraFinal.Count);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, listaCabeceraFinal, null, listaCabeceraFinal.Count);
                        this.BodyListaFrecuenciaTablaWord(ref secuencia, listaDataFinal);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);

                        p = document.InsertParagraph().Font(fontDoc);
                        p.AppendLine().Font(fontDoc).Append("Nota: Las celdas de color gris pertenecen a sistemas aislados.").FontSize(11).Font(fontDoc);
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("Ninguna").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }
                    */

                    #endregion

                    #region "6. Operación por regulación de tensión"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "6. Operación por regulación de tensión");

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("6.1. Las centrales que operaron por regulación de tensión fueron:").FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);

                    var ListaOperacionTension = servicio.GetListaOperacionTension(fecIni);//05
                    if (ListaOperacionTension.Count > 0)
                    {
                        nroRowData = ListaOperacionTension.Count;
                        nroColumn = 5;
                        strHead = "Empresa,Equipo,Inicio,Final,Motivo";
                        Table secuencia = document.InsertTable(ListaOperacionTension.Count + 1, nroColumn);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), null, nroColumn);
                        this.BodyListaOperacionTensionTablaWord(ref secuencia, ListaOperacionTension);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);

                        p = document.InsertParagraph().Font(fontDoc);
                        p.AppendLine().Append(" ").Font(fontDoc);
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("Ninguna").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("6.2. Las líneas que se desconectaron por regulación de tensión son las siguientes:").FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);
                    List<EveIeodcuadroDTO> ListaLineasDesconectadas = servicio.GetListaBitacora(fecIni, ConstantesAppServicio.SubcausacodiRegulacionTension, ConstantesAppServicio.FamcodiLinea);//05
                    ListaLineasDesconectadas = ListaLineasDesconectadas.OrderBy(x => x.Ichorini).ToList();
                    if (ListaLineasDesconectadas.Count > 0)
                    {
                        nroRowData = ListaLineasDesconectadas.Count;
                        nroColumn = 6;
                        strHead = "Empresa,Línea,Código,Inicio,Final,Motivo";
                        Table secuencia = document.InsertTable(ListaLineasDesconectadas.Count + 1, nroColumn);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), null, nroColumn);
                        this.BodyListaLineasDesconectadasTablaWord(ref secuencia, ListaLineasDesconectadas);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("Ninguna").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }

                    #endregion

                    #region "7. Operación de calderos"
                    /*
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "7. Operación de calderos");
                var ListaCalderosSinOperar = servicio.GetListaBitacora(fecIni, ConstantesAppServicio.SubcausacodiOperacionCalderos, ConstantesAppServicio.FamcodiLinea);//06
                if (ListaCalderosSinOperar.Count > 0)
                {
                    nroColumn = 6;
                    strHead = "Empresa,Central,Equipo,Inicio,Final,Motivo";
                    Table secuencia = document.InsertTable(ListaCalderosSinOperar.Count + 1, nroColumn);
                    secuencia.AutoFit = AutoFit.Contents;
                    secuencia.Design = TableDesign.TableGrid;

                    this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), null, nroColumn);
                    this.BodyListaCalderosSinOperarTablaWord(ref secuencia, ListaCalderosSinOperar);
                }
                else
                {
                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("No operaron los calderos.").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                }
                */
                    #endregion

                    #region "7. Equipos que operaron al máximo de su capacidad de transmisión"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "7. Equipos que operaron al máximo de su capacidad de transmisión");
                    var ListaEquiposOperaron = servicio.GetListaBitacora(fecIni, ConstantesAppServicio.SubcausacodiCongestion, ConstantesAppServicio.ParametroDefecto);
                    if (ListaEquiposOperaron.Count > 0)
                    {
                        nroRowData = ListaEquiposOperaron.Count;
                        nroColumn = 5;
                        strHead = "Empresa,Código,Ubicación,Inicio,Final";
                        Table secuencia = document.InsertTable(ListaEquiposOperaron.Count + 1, nroColumn);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), null, nroColumn);
                        this.BodyListaEquiposOperaronTablaWord(ref secuencia, ListaEquiposOperaron);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("No se presentó.").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }
                    #endregion

                    #region "8. Pruebas"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "8. Pruebas");

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("8.1. Pruebas por requerimientos propios").FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Se realizaron las siguientes pruebas:").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    var ListaReqPropios = servicio.GetListaReqPropios(fecIni, fecIni.AddDays(1));//09
                    if (ListaReqPropios.Count > 0)
                    {
                        nroRowData = ListaReqPropios.Count;
                        nroColumn = 6;
                        strHead = "Empresa,Central,Unidad,Inicio,Final,Descripción";
                        listaWidthColumn = null;

                        Table secuencia = document.InsertTable(ListaReqPropios.Count + 1, nroColumn);
                        secuencia.Design = TableDesign.TableGrid;
                        secuencia.AutoFit = AutoFit.Window;

                        this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), listaWidthColumn, nroColumn);
                        this.BodyListaReqPropiosTablaWord(ref secuencia, ListaReqPropios);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }

                    p = document.InsertParagraph().Font(fontDoc);
                    p.AppendLine().Font(fontDoc).Append("8.2. Pruebas aleatorias").FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Verificación de disponibilidades de las unidades térmicas mediante pruebas aleatorias. \n(Procedimiento n°25, numeral 7.4)").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    var ListaLogSorteo = servicio.GetListaLogSorteo(fecIni);//09
                    if (ListaLogSorteo.Count > 0)
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("Unidades que ingresaron al sorteo para las pruebas:").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                        var ListaPruebasAleatorias = servicio.GetListaPruebasAleatorias(fecIni);//09
                        if (ListaPruebasAleatorias.Count > 0)
                        {
                            listaHeadColumn = new List<string>() { "Empresa", "Equipo" };
                            listaWidthColumn = new List<double>() { 3, 4 };
                            nroRowData = ListaPruebasAleatorias.Count;
                            nroColumn = listaHeadColumn.Count;
                            Table secuencia = document.InsertTable(ListaPruebasAleatorias.Count + 1, listaHeadColumn.Count);
                            secuencia.Design = TableDesign.TableGrid;

                            this.HeadTablaWord(ref secuencia, listaHeadColumn, listaWidthColumn, listaHeadColumn.Count);
                            this.BodyListaPruebasAleatoriasTablaWord(ref secuencia, listaWidthColumn, ListaPruebasAleatorias);
                            this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);

                            int TotalSorteo = servicio.GetTotalConteoTipo(fecIni, "XEQ");//09
                            if (TotalSorteo > 0)
                            {
                                p = document.InsertParagraph().Font(fontDoc);
                                p.AppendLine().AppendLine().Append("Resultado del sorteo: Balota Negra - Día de Prueba").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                                ListaPruebasAleatorias = ListaPruebasAleatorias.Where(x => x.Ecodigo == "S").ToList();
                                if (ListaPruebasAleatorias.Count > 0)
                                {
                                    listaHeadColumn = new List<string>() { "Empresa", "Equipo" };
                                    listaWidthColumn = new List<double>() { 1, 4 };
                                    nroRowData = ListaPruebasAleatorias.Count;
                                    nroColumn = listaHeadColumn.Count;
                                    secuencia = document.InsertTable(ListaPruebasAleatorias.Count + 1, listaHeadColumn.Count);
                                    secuencia.Design = TableDesign.TableGrid;

                                    this.HeadTablaWord(ref secuencia, listaHeadColumn, listaWidthColumn, listaHeadColumn.Count);
                                    this.BodyListaPruebasAleatoriasTablaWord(ref secuencia, listaWidthColumn, ListaPruebasAleatorias);
                                    this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                                }
                                else
                                {
                                    p = document.InsertParagraph().Font(fontDoc);
                                    p.Append("No hay equipos disponibles para prueba").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                                }
                            }
                            else
                            {
                                p = document.InsertParagraph().Font(fontDoc);
                                p.AppendLine().Append(" ").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc).Append("Resultado del sorteo: Balota Blanca - Día de no Prueba").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                            }
                        }
                        else
                        {
                            p = document.InsertParagraph().Font(fontDoc);
                            p.Append("Ninguna.").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                        }
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("NO SE REALIZO SORTEO...").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }

                    #endregion

                    #region "9. Sistemas aislados"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "9. Sistemas aislados");
                    var ListaSistemasAislados = servicio.GetListaBitacora(fecIni, ConstantesAppServicio.SubcausacodiSistemasAislados, ConstantesAppServicio.ParametroDefecto);//10
                    if (ListaSistemasAislados.Count > 0)
                    {
                        strHead = "Equipo causante,Motivo,Sistema aislado,Inicio,Final,Operación del centrales";
                        nroColumn = 6;
                        nroRowData = ListaSistemasAislados.Count;
                        Table secuencia = document.InsertTable(ListaSistemasAislados.Count + 1, nroColumn);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), null, nroColumn);
                        this.BodyListaSistemasAisladosTablaWord(ref secuencia, ListaSistemasAislados);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("No se presentó.").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }
                    #endregion

                    #region "10. Vertimiento de presas, disponibilidad de gas y quema de gas informado por los integrantes"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "10. Vertimiento de presas, disponibilidad de gas y quema de gas informado por los integrantes");

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("10.1. Vertimiento de presas y embalses").FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Ver Anexo 6: Reporte de Vertimiento de Embalses").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("10.2. Disponibilidad de gas").FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Ver Anexo 7: Reporte de Disponibilidad de Gas").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("10.3. Quema de gas").FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);
                    //var ListaDisponibilidadGas = servicio.GetListaBitacora(fecIni, ConstantesAppServicio.SubcausacodiDisponibilidadGas, ConstantesAppServicio.ParametroDefecto);//11
                    var ListaQuemaGas = servicio.GetListaBitacora(fecIni, ConstantesAppServicio.SubcausacodiVenteoGas, ConstantesAppServicio.ParametroDefecto);//11
                    if (ListaQuemaGas.Count > 0)
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("Se muestran los volúmenes:").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                        nroColumn = 5;
                        nroRowData = ListaQuemaGas.Count;
                        strHead = "Empresa,Central,Inicio,Final,MMPC";
                        Table secuencia = document.InsertTable(ListaQuemaGas.Count + 1, nroColumn);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, strHead.Split(',').ToList(), null, nroColumn);
                        this.BodyListaQuemaGasTablaWord(ref secuencia, ListaQuemaGas);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("No se presentó.").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }
                    #endregion

                    #region "11. Descarga de Lagunas"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "11. Descarga de Lagunas");

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Ver Anexo 8: Reporte de Descarga de Lagunas").FontSize(11).Font(fontDoc);
                    #endregion

                    #region "12. Calidad del servicio eléctrico"
                    //insumos
                    List<FIndicadorDTO> listaDataPeriodoVariacion = servicioPR5.ReporteVariacionesFrecuenciaSEINDataReporte(gpscodi.ToString(), fecIni, fecIni, out List<MeGpsDTO> listaGPS);

                    //tabla frecuencia mínima máxima
                    string gpsNombre = "GPS: " + servicioPR5.GetNombreGPS(gpscodi);

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "12. Calidad del servicio eléctrico");

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("12.1. Calidad de producto del SEIN (*)").FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);
                    nroColumn = 7;

                    List<WordCelda> listaColumna1 = new List<WordCelda>
                    {
                        new WordCelda("Indicador de calidad", 100, 11,11, Alignment.left),
                        new WordCelda("Periodo/ Hora", 150, 11,11, Alignment.center),
                        new WordCelda("Valor (Hz)", 60, 11,11, Alignment.center),
                        new WordCelda("N° Transgresiones acumuladas en el mes", 100 , 11,11, Alignment.center),
                        new WordCelda("N° de veces en el mes",60 , 11,11, Alignment.center),
                        new WordCelda("Tolerancia NTCSE (Hz)",60 , 11,11, Alignment.center),
                        new WordCelda("", 60, 11,11, Alignment.center),
                    };
                    List<WordCelda> listaColumna2 = new List<WordCelda>
                    {
                        new WordCelda("", 100, 11,11, Alignment.left),
                        new WordCelda("", 150, 11,11, Alignment.center),
                        new WordCelda("", 60, 11,11, Alignment.center),
                        new WordCelda("",100 , 11,11, Alignment.center),
                        new WordCelda("",60 , 11,11, Alignment.center),
                        new WordCelda("Máx.",60 , 11,11, Alignment.center),
                        new WordCelda("Min.", 60, 11,11, Alignment.center),
                    };

                    Table secuencia_ = document.InsertTable(4, nroColumn);
                    secuencia_.AutoFit = AutoFit.ColumnWidth;
                    secuencia_.Design = TableDesign.TableGrid;
                    secuencia_.Alignment = Alignment.center;

                    var data = listaDataPeriodoVariacion.Where(x => x.Gps == gpscodi).ToList();
                    var sostenida = data.Find(x => x.Indiccodi == ConstantesIndicador.VariacionSostenida);
                    var subita = data.Find(x => x.Indiccodi == ConstantesIndicador.VariacionSubita);

                    this.AddRowsTablaWord(ref secuencia_, 2, 0, "Variaciones sostenidas de Frecuencia");
                    this.AddRowsTablaWord(ref secuencia_, 2, 1, sostenida.HoraTransgr.Trim());
                    this.AddRowsTablaWord(ref secuencia_, 2, 2, sostenida.IndicValorTransgr.Trim());
                    this.AddRowsTablaWord(ref secuencia_, 2, 3, sostenida.AcumuladoTransgr.ToString());
                    this.AddRowsTablaWord(ref secuencia_, 2, 4, "28.8");
                    this.AddRowsTablaWord(ref secuencia_, 2, 5, "60.36");
                    this.AddRowsTablaWord(ref secuencia_, 2, 6, "59.64");

                    this.AddRowsTablaWord(ref secuencia_, 3, 0, "Variaciones súbitas de Frecuencia");
                    this.AddRowsTablaWord(ref secuencia_, 3, 1, subita.HoraTransgr.Trim());
                    this.AddRowsTablaWord(ref secuencia_, 3, 2, subita.IndicValorTransgr.Trim());
                    this.AddRowsTablaWord(ref secuencia_, 3, 3, subita.AcumuladoTransgr.ToString());
                    this.AddRowsTablaWord(ref secuencia_, 3, 4, "1");
                    this.AddRowsTablaWord(ref secuencia_, 3, 5, "61");
                    this.AddRowsTablaWord(ref secuencia_, 3, 6, "59");

                    //formatear cuerpo
                    UtilWord.BodyTablaWord(ref secuencia_, 0, 0, 3, nroColumn - 1, listaColumna1, fontDoc);

                    //formatear cabecera
                    UtilWord.FormatearFilaTablaWord(ref secuencia_, 0, listaColumna1, fontDoc);
                    UtilWord.FormatearFilaTablaWord(ref secuencia_, 1, listaColumna2, fontDoc);

                    secuencia_.Rows[0].MergeCells(5, 6);
                    secuencia_.MergeCellsInColumn(0, 0, 1);
                    secuencia_.MergeCellsInColumn(1, 0, 1);
                    secuencia_.MergeCellsInColumn(2, 0, 1);
                    secuencia_.MergeCellsInColumn(3, 0, 1);
                    secuencia_.MergeCellsInColumn(4, 0, 1);

                    secuencia_.Rows[0].Cells[5].RemoveParagraphAt(1); //Hz

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("(*) " + gpsNombre).FontSize(8).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    //12.1
                    var frecMinimo = listaDataPeriodoVariacion.Find(x => x.Indiccodi == ConstantesIndicador.FrecuenciaMinima);
                    var frecMaximo = listaDataPeriodoVariacion.Find(x => x.Indiccodi == ConstantesIndicador.FrecuenciaMaxima);

                    listaHeadColumn = new List<string>() { "Frecuencia Mínima (SEIN)", "", "Frecuencia Máxima (SEIN)", "" };
                    Table secuenciaFrec = document.InsertTable(1 + 1, listaHeadColumn.Count);
                    secuenciaFrec.AutoFit = AutoFit.Contents;
                    secuenciaFrec.Design = TableDesign.TableGrid;

                    this.HeadTablaWord(ref secuenciaFrec, listaHeadColumn, null, listaHeadColumn.Count);
                    secuenciaFrec.Rows[0].MergeCells(0, 1);
                    secuenciaFrec.Rows[0].MergeCells(1, 2);

                    this.AddRowsTablaWord(ref secuenciaFrec, 1, 0, frecMinimo.HoraFrecMin.Trim());
                    this.AddRowsTablaWord(ref secuenciaFrec, 1, 1, frecMinimo.ValorFrecMinDesc.Trim());

                    this.AddRowsTablaWord(ref secuenciaFrec, 1, 2, frecMaximo.HoraFrecMax.Trim());
                    this.AddRowsTablaWord(ref secuenciaFrec, 1, 3, frecMaximo.ValorFrecMaxDesc.Trim());

                    //12.2
                    p = document.InsertParagraph().Font(fontDoc);
                    p.AppendLine().Font(fontDoc).Append("12.2. Calidad de suministro del SEIN").FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);
                    List<EveInterrupcionDTO> ListaCalidadSuministro = servicio.GetListaCalidadSuministro(fecIni);//13
                    if (ListaCalidadSuministro.Count > 0)
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("Las interrupciones de suministros son las siguientes:").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                        listaHeadColumn = new List<string>() { "Pto. Interrupción", "Cliente", "Pto. Entrega", "MW", "Hora ini.", "Horas", "Observ.", "racMF", "etapaMF", "RM" };

                        nroRowData = ListaCalidadSuministro.Count;
                        nroColumn = listaHeadColumn.Count;
                        Table secuencia = document.InsertTable(ListaCalidadSuministro.Count + 1, listaHeadColumn.Count);
                        secuencia.AutoFit = AutoFit.Contents;
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, listaHeadColumn, null, listaHeadColumn.Count);
                        this.BodyListaCalidadSuministroTablaWord(ref secuencia, ListaCalidadSuministro);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("No se reportó.").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }

                    #endregion

                    #region "13. Reprogramaciones del día"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "13. Reprogramaciones del día");
                    var ListaReprogramado = servicio.GetListaReprogramado(fecIni);//14
                    if (ListaReprogramado.Count > 0)
                    {
                        listaHeadColumn = new List<string>() { "Hora", "Reprograma", "Motivo" };
                        listaWidthColumn = new List<double>() { 10, 50, 100 };
                        nroRowData = ListaReprogramado.Count;
                        nroColumn = listaHeadColumn.Count;
                        Table secuencia = document.InsertTable(ListaReprogramado.Count + 1, listaHeadColumn.Count);
                        secuencia.Design = TableDesign.TableGrid;

                        this.HeadTablaWord(ref secuencia, listaHeadColumn, listaWidthColumn, listaHeadColumn.Count);
                        this.BodyListaReprogramadoTablaWord(ref secuencia, listaWidthColumn, ListaReprogramado);
                        this.BodyFontIDCOS(ref secuencia, nroRowData + 1, nroColumn);
                    }
                    #endregion

                    #region "14. Observaciones"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "14. Observaciones");
                    var ListaEveObservaciones = servicio.GetListaEveObservaciones(fecIni, ConstantesAppServicio.SubcausacodiObservacionesIDCOS, ConstantesAppServicio.CausaevencodiProgramado);//15
                    if (ListaEveObservaciones.Count > 0)
                    {
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("Ninguna.").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }
                    #endregion

                    #region "15. Coordinadores, Especialistas y Analistas de turno"
                    List<List<string>> dataItem16Rolturno = new List<List<string>>(); // this.servicio.ListarItem16RolTurno(fecIni);
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "15. Coordinadores, Especialistas y Analistas de turno");

                    listaHeadColumn = new List<string>() { "Turno", "Coordinador de la Operación", "Especialista de la Operación", "Analista de la Operación" };
                    listaWidthColumn = new List<double>() { 30, 40, 40, 40 };
                    nroRowData = dataItem16Rolturno.Count;
                    nroColumn = listaHeadColumn.Count;
                    Table tablaRolTurno = document.InsertTable(dataItem16Rolturno.Count + 1, listaHeadColumn.Count);
                    tablaRolTurno.Design = TableDesign.TableGrid;

                    this.HeadTablaWord(ref tablaRolTurno, listaHeadColumn, listaWidthColumn, listaHeadColumn.Count);
                    this.BodyRolTurnoTablaWord(ref tablaRolTurno, dataItem16Rolturno);
                    this.BodyFontIDCOS(ref tablaRolTurno, nroRowData + 1, nroColumn);

                    #endregion

                    #region "16. Anexos"
                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtitulo(ref p, "16. Anexos");

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 1: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Restricciones operativas del SEIN").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 2: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Resumen de la operación").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("2.1 Potencia activa ejecutada de las unidades de generación del SEIN").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("2.2 Potencia reactiva ejecutada de las unidades de generación del SEIN").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("2.3 Reprograma de la operación del SEIN").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("2.4 Caudales y volúmenes de las cuencas").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("2.5 Flujos de potencia activa de los principales equipos de transmisión del SEIN y demandas").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("2.6 Resumen RER").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 3: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Horas de operación de las unidades térmicas del SEIN").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 4: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Mantenimientos ejecutados del SEIN").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 5: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Restricción de suministro").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 6: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Vertimiento de embalses").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 7: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Disponibilidad de gas").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 8: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Descarga de lagunas").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 9: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Hidrología en tiempo real").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 10: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Resumen de pruebas aleatorias").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Anexo 11: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Regulación Secundaria de Frecuencia").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    if (ConstantesMigraciones.TieneCheckProc25 == checkProc25)
                    {
                        p = document.InsertParagraph().Font(fontDoc);
                        p.Append("Anexo 12: ").FontSize(11).Font(fontDoc).Italic().Bold().Append("Resumen de la prueba de verificación de disponibilidad de unidades térmicas mediante pruebas aleatorias (Procedimiento Nº 25 - COES-SINAC)").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    }

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append(" ").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Hora de emisión del informe: " + DateTime.Now.ToString("HH:mm") + " h").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Fecha: " + DateTime.Now.ToString(ConstantesBase.FormatoFechaBase)).FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Difusión: SCO, SPR, SEV, SPL, SIP, STR, DO, D, CC-INTEGRANTES.").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append(" ").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Nota1: Las siglas utilizadas en el presente documento están de acuerdo a la \"Base Metodológica para la Aplicación de la Norma Técnica de Calidad de los Servicios Eléctricos\".").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append(" ").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Nota2: Para el cálculo de la demanda del COES, se considera solo la generación de los Integrantes del COES.").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append(" ").Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);
                    p = document.InsertParagraph().Font(fontDoc);
                    p.Append("Nota 3: En caso hubiera observaciones al presente informe, favor de enviarlo al correo electrónico revisionieod@coes.org.pe").FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc);

                    #endregion

                    document.Save();
                }

                model.Resultado = nombreDoc;
                model.nRegistros = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Mensaje = ex.Message;
                model.Resultado = nombreDoc;
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// Get imagen desde Highcharts
        /// </summary>
        /// <param name="options"></param>
        /// <param name="rutaImagen"></param> 
        /// <returns></returns>
        private async Task<string> ObtenerLinkImagenDesdeJsonStringAsync(object options, string rutaImagen)
        {

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://export.highcharts.com/");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                httpWebRequest.UserAgent = "PR25";
                httpWebRequest.Referer = "https://coes.org.pe";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        infile = JsonConvert.SerializeObject(options)
                    });
                    streamWriter.Write(json);
                }

                if (System.IO.File.Exists(rutaImagen))
                    System.IO.File.Delete(rutaImagen);

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.Stream responseStream = httpResponse.GetResponseStream();
                Bitmap chartPNG = new Bitmap(responseStream);
                chartPNG.Save(rutaImagen);
            }
            catch(Exception ex)
            {
                rutaImagen = rutaImagen.Replace("area.png", "default.png");
                rutaImagen = rutaImagen.Replace("pie.png", "default.png");
            }

            //var linkWeb = await clientHighchartsClient.GetChartImageLinkFromOptionsAsync(JsonConvert.SerializeObject(options));




            return rutaImagen;
        }

        /// <summary>
        /// Objeto Json grafico pie
        /// </summary>
        /// <param name="listaPie"></param>
        /// <returns></returns>
        private string ObtenerJsonStringPie(List<MeMedicion48DTO> listaPie)
        {
            var jsonDataPie =
                "{ chart: {plotBackgroundColor: null,plotBorderWidth: null,plotShadow: false,type: 'pie'},title: {text: 'Producción por Empresa'}," +
                "plotOptions: {pie: {             showInLegend: true, allowPointSelect: true,cursor: 'pointer'," +
                "dataLabels: { enabled: true,                    format: '{point.percentage:.1f} %'}}}," +
                "series: [{name: 'Producción',colorByPoint: true,data: [dataPie]}]}";

            var dataPie = "";
            listaPie.ForEach(x => { dataPie += string.Format("{{ name: '{0}', y: {1}}},", x.Emprnomb, x.Meditotal); });


            jsonDataPie = jsonDataPie.Replace("dataPie", dataPie.TrimEnd(','));
            return jsonDataPie;
        }

        /// <summary>
        /// Objeto Json grafico area
        /// </summary>
        /// <param name="dataGraficoArea"></param>
        /// <returns></returns>
        private string ObtenerJsonStringArea(GraficoWeb dataGraficoArea)
        {
            string seriesArea = string.Empty;
            string jsonDataArea = @"{ 
                    chart: {type: 'area',height: 550},
                    title: {text: 'Producción por Tipo de Combustible'},
                    xAxis: {
                        title: { text: 'Horas (hh:mm)'},
                        labels: { rotation: -90 },
                        gridLineWidth: 1, 
                        categories: ['00:30', '01:00', '01:30', '02:00', '02:30', '03:00', '03:30', '04:00', '04:30', '05:00', '05:30', '06:00', '06:30', '07:00', '07:30','08:00', '08:30', '09:00', '09:30', '10.00', '10:30', '11:00', '11:30', '12:00', '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00', '16:30', '17:00', '17:30', '18:00', '18:30', '19:00', '19:30', '20:00', '20:30', '21:00', '21:30', '22:00', '22:30', '23:00', '23:30', '00:00']
                    },
                    yAxis: {
                        title: {text: 'Potencia (MW)'},
                        labels: { format:'{value:,.0f}'},min: 0 },
                        plotOptions: {area: {stacking: 'normal', marker: {enabled: false}},
                        lineWidth: 1
                    },
                    legend: {
                    	reversed: true
                    },
                    series: [seriesArea]}
            ";

            foreach (var area in dataGraficoArea.Series)
            {
                List<decimal> series = new List<decimal>();
                for (var i = 0; i < 48; i++)
                {
                    var val = area.Data[i].Y ?? 0;
                    series.Add(val);
                }
                string porcentaje = Math.Round(area.Porcentaje.GetValueOrDefault(0) * 100, 2) + "%";
                seriesArea += string.Format("{{name: '{0}', color: '{1}',data: [{2}]}},", area.Name + " " + porcentaje, area.Color, string.Join(",", series));
            }

            jsonDataArea = jsonDataArea.Replace("seriesArea", seriesArea.TrimEnd(','));

            return jsonDataArea;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lista"></param>
        /// <returns></returns>
        private string f_get_fallaacumulada(List<FIndicadorDTO> Lista)
        {
            return Lista.Count().ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pval1"></param>
        /// <param name="punidad"></param>
        /// <param name="psinregistros"></param>
        /// <param name="Lista"></param>
        /// <returns></returns>
        private string f_get_querytocadenatransg(int pval1, string punidad, string psinregistros, List<FIndicadorDTO> Lista)
        {
            int num_ = 0;
            string descrip = "";

            for (int i = 0; i < Lista.Count; i++)
            {
                num_++;
                if (pval1 == 0)
                {
                    string ls_hora = Lista[i].Fechahora.ToString("yyyy-MM-dd HH:mm");
                    ls_hora = Lista[i].Fechahora.ToString("HH:mm:ss") + " h.";
                    descrip += ls_hora;
                }
                else
                {
                    descrip += Lista[i].Indicvalor + " " + punidad;
                }

                if (i != (Lista.Count - 1)) { descrip += "\r\n"; }
            }

            if (num_ <= 0) { return psinregistros; }
            else { return descrip; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia_"></param>
        /// <param name="row"></param>
        /// <param name="cel"></param>
        /// <param name="name"></param>
        private void AddRowsTablaWord(ref Table secuencia_, int row, int cel, string name)
        {
            secuencia_.Rows[row].Cells[cel].Paragraphs[0].Append(name);
            secuencia_.Rows[row].Cells[cel].Paragraphs[0].Alignment = Alignment.center;
            secuencia_.Rows[row].Cells[cel].VerticalAlignment = VerticalAlignment.Center;
            secuencia_.Rows[row].Cells[cel].Paragraphs[0].Font(fontDoc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="descripcion"></param>
        private void AddSubtitulo(ref Paragraph p, string descripcion)
        {
            p.AppendLine().Font(fontDoc).AppendLine().Font(fontDoc).Append(descripcion).FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="nroRow"></param>
        /// <param name="nroColumn"></param>
        private void BodyFontIDCOS(ref Table secuencia, int nroRow, int nroColumn)
        {
            for (int index = 0; index < nroRow; index++)
            {
                for (int x = 0; x < nroColumn; x++)
                {
                    secuencia.Rows[index].Cells[x].Paragraphs[0].Font(fontDoc);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="namesColumn"></param>
        /// <param name="nroColumn"></param>
        private void HeadTablaWord(ref Table secuencia, List<string> namesColumn, List<double> widthColumn, int nroColumn)
        {
            for (int x = 0; x < nroColumn; x++)
            {
                secuencia.Rows[0].Cells[x].Paragraphs[0].Append(namesColumn[x]);
                secuencia.Rows[0].Cells[x].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                secuencia.Rows[0].Cells[x].Paragraphs[0].Alignment = Alignment.center;
                secuencia.Rows[0].Cells[x].Paragraphs[0].Bold();
                secuencia.Rows[0].Cells[x].Paragraphs[0].Font(fontDoc);
                this.CentrarCellTableWord(secuencia.Rows[0].Cells[x]);
                this.SetWidthColumCellTableWord(secuencia.Rows[0], x, widthColumn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaEventosImportantesTablaWord(ref Table secuencia, List<EveEventoDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Evenini.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                secuencia.Rows[index].Cells[0].Paragraphs[0].Alignment = Alignment.center;

                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Emprabrev);
                secuencia.Rows[index].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                secuencia.Rows[index].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                if (entity.Evendesc != null)
                    secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Evendesc.Trim());
                else
                    secuencia.Rows[index].Cells[2].Paragraphs[0].Append("*");
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="ListaGeneCentralSein"></param>
        private void BodyListaGeneCentralSeinTablaWord(ref Table secuencia, List<SiEmpresaDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Emprabrev);
                secuencia.Rows[index].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                secuencia.Rows[index].Cells[0].Paragraphs[0].Alignment = Alignment.center;

                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Descripcion);
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaInterconexInternaTablaWord(ref Table secuencia, List<EveIeodcuadroDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Icdescrip1);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Icdescrip2);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equiabrev);
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity.Ichorini.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[4].Paragraphs[0].Append(entity.Ichorfin.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[5].Paragraphs[0].Append(entity.Icdescrip3);

                this.CentrarCellTableWord(secuencia.Rows[index].Cells[0]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[1]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[2]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[3]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[4]);

                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="datos"></param>
        private void BodyListaFrecuenciaTablaWord(ref Table secuencia, List<List<string>> Lista)
        {
            //int index = 1;
            for (int i = 0; i < Lista.Count; i++)
            {
                secuencia.Rows[i + 1].Cells[0].Paragraphs[0].Append(Lista[i][0]);
                CentrarCellTableWord(secuencia.Rows[i + 1].Cells[0]);
                secuencia.Rows[i + 1].Cells[1].Paragraphs[0].Append(Lista[i][1]);
                CentrarCellTableWord(secuencia.Rows[i + 1].Cells[1]);


                for (int y = 2; y < Lista[0].Count; y++)
                {
                    secuencia.Rows[i + 1].Cells[y].Paragraphs[0].Append(Lista[i][y]);
                    CentrarCellTableWord(secuencia.Rows[i + 1].Cells[y]);
                }

                if (i == Lista.Count - 1)
                {
                    for (int y = 0; y < Lista[0].Count; y++)
                    {
                        secuencia.Rows[i + 1].Cells[y].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                        secuencia.Rows[i + 1].Cells[y].Paragraphs[0].Bold();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaOperacionTensionTablaWord(ref Table secuencia, List<EveHoraoperacionDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Emprabrev);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Gruponomb);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Hophorini.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity.Hophorfin.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[4].Paragraphs[0].Append(entity.Hopdesc);

                this.CentrarCellTableWord(secuencia.Rows[index].Cells[0]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[1]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[2]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[3]);

                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaLineasDesconectadasTablaWord(ref Table secuencia, List<EveIeodcuadroDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Emprabrev);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Areanomb);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equiabrev);
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity.Ichorini.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[4].Paragraphs[0].Append(entity.Ichorfin.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[5].Paragraphs[0].Append(entity.Icdescrip1);

                this.CentrarCellTableWord(secuencia.Rows[index].Cells[0]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[1]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[2]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[3]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[4]);

                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaCalderosSinOperarTablaWord(ref Table secuencia, List<EveIeodcuadroDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Emprabrev);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Areanomb);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equiabrev);
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity.Ichorini.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[4].Paragraphs[0].Append(entity.Ichorfin.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[5].Paragraphs[0].Append(entity.Icdescrip1);
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaEquiposOperaronTablaWord(ref Table secuencia, List<EveIeodcuadroDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Emprabrev);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Equiabrev);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Areanomb);
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity.Ichorini.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[4].Paragraphs[0].Append(entity.Ichorfin.Value.ToString("HH:mm"));
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="ListaReqPropios"></param>
        private void BodyListaReqPropiosTablaWord(ref Table secuencia, List<EveIeodcuadroDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                string horaIni = entity.Ichorini.Value.ToString("HH:mm");
                string horaFin = entity.Ichorfin.Value.ToString("HH:mm");
                string descripcion = entity.Icdescrip1 != null ? entity.Icdescrip1.Trim() : string.Empty;
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Emprabrev);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[0]);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Areanomb);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[1]);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equiabrev);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[2]);
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(horaIni);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[3]);
                secuencia.Rows[index].Cells[4].Paragraphs[0].Append(horaFin == "00:00" ? "24:00" : horaFin);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[4]);
                secuencia.Rows[index].Cells[5].Paragraphs[0].Append(descripcion);
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaSistemasAisladosTablaWord(ref Table secuencia, List<EveIeodcuadroDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Areanomb);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Icdescrip2);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Icdescrip3);
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity.Ichorini.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[4].Paragraphs[0].Append(entity.Ichorfin.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[5].Paragraphs[0].Append(entity.Icdescrip1);

                this.CentrarCellTableWord(secuencia.Rows[index].Cells[0]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[3]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[4]);

                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaQuemaGasTablaWord(ref Table secuencia, List<EveIeodcuadroDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Emprabrev);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Areanomb);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Ichorini.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity.Ichorfin.Value.ToString("HH:mm"));
                secuencia.Rows[index].Cells[4].Paragraphs[0].Append(entity.Icvalor1.ToString());

                this.CentrarCellTableWord(secuencia.Rows[index].Cells[0]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[1]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[2]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[3]);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[4]);
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaCalidadSuministroTablaWord(ref Table secuencia, List<EveInterrupcionDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.PtoInterrupNomb);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.EmprNomb);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.PtoEntreNomb);
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity.Interrmw.ToString());
                secuencia.Rows[index].Cells[4].Paragraphs[0].Append(entity.Evenini.ToString("HH:mm"));
                secuencia.Rows[index].Cells[5].Paragraphs[0].Append(string.Format("{0:0.00}", (entity.Interrminu / 60), 2).ToString());
                secuencia.Rows[index].Cells[6].Paragraphs[0].Append(entity.Interrdesc);
                secuencia.Rows[index].Cells[7].Paragraphs[0].Append(entity.Interrracmf);
                secuencia.Rows[index].Cells[8].Paragraphs[0].Append(entity.InterrmfetapaDesc);
                secuencia.Rows[index].Cells[9].Paragraphs[0].Append(entity.Interrmanualr);

                for (int y = 0; y < 10; y++)
                {
                    CentrarCellTableWord(secuencia.Rows[index].Cells[y]);
                }

                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaReprogramadoTablaWord(ref Table secuencia, List<double> widthColumn, List<EveMailsDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Mailfecha.ToString(ConstantesAppServicio.FormatoOnlyHora));
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[0]);
                this.SetWidthColumCellTableWord(secuencia.Rows[index], 0, widthColumn);

                secuencia.Rows[index].Cells[1].Paragraphs[0].Append("SCO - " + this.servicio.GetNumeroIeod(entity.Mailfecha)
                    + "-" + entity.Mailhoja
                    + " / Sem " + Base.Tools.Util.GenerarNroSemana(entity.Mailfecha, (int)DayOfWeek.Saturday).ToString());
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[1]);
                this.SetWidthColumCellTableWord(secuencia.Rows[index], 1, widthColumn);

                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.Mailreprogcausa.Trim());
                this.SetWidthColumCellTableWord(secuencia.Rows[index], 2, widthColumn);
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="Lista"></param>
        private void BodyListaPruebasAleatoriasTablaWord(ref Table secuencia, List<double> widthColumn, List<EqEquipoDTO> Lista)
        {
            int index = 1;
            foreach (var entity in Lista)
            {

                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.EMPRNOMB);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[0]);
                this.SetWidthColumCellTableWord(secuencia.Rows[index], 0, widthColumn);

                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Equinomb + " - " + entity.Equiabrev);
                this.CentrarCellTableWord(secuencia.Rows[index].Cells[1]);
                this.SetWidthColumCellTableWord(secuencia.Rows[index], 1, widthColumn);

                index++;
            }
        }

        /// <summary>
        /// Tabla rol de turnos
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="lista"></param>
        private void BodyRolTurnoTablaWord(ref Table secuencia, List<List<string>> lista)
        {
            int index = 1;
            foreach (var entity in lista)
            {
                secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity[0]);
                CentrarCellTableWord(secuencia.Rows[index].Cells[0]);
                secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity[1]);
                CentrarCellTableWord(secuencia.Rows[index].Cells[1]);
                secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity[2]);
                CentrarCellTableWord(secuencia.Rows[index].Cells[2]);
                secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity[3]);
                CentrarCellTableWord(secuencia.Rows[index].Cells[3]);
                index++;
            }
        }

        /// <summary>
        /// Centrar celda table Word
        /// </summary>
        /// <param name="cell"></param>
        private void CentrarCellTableWord(Cell cell)
        {
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.Paragraphs[0].Alignment = Alignment.center;
        }

        private void SetWidthColumCellTableWord(Row row, int index, List<double> widthColumn)
        {
            double width = 0;
            width = widthColumn != null && widthColumn.Count > 0 ? widthColumn[index] : 0;
            if (width > 0) { row.Cells[index].Width = width * this.FactorWidth; }
        }

        #endregion

        #region Restricciones Operativas

        /// <summary>
        /// Exportacion a archivo Excel de Restricciones Operativas
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GenerarReporteXlsRestricionesOperativas(string fecha)
        {
            MigracionesModel model = new MigracionesModel();
            string nameFile = string.Empty;
            DateTime f1 = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                nameFile = ConstantesMigraciones.RptRestricOp + "_" + f1.ToString("yyyyMMdd");
                this.servicio.GenerarArchivoExcelRptRestricOp(f1, ruta + nameFile + ConstantesAppServicio.ExtensionExcel);

                model.Resultado = nameFile + ConstantesAppServicio.ExtensionExcel;
                model.nRegistros = 1;
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        #endregion

    }
}