using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using COES.MVC.Intranet.Controllers;
using log4net;
using System.Reflection;
using COES.Dominio.DTO.Enum;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class CalculoRentaCongestionController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CalculoRentaCongestionController));
        //
        // GET: /Transferencias/CalculoRentaCongestion/

        TransferenciaInformacionBaseAppServicio servicio = new TransferenciaInformacionBaseAppServicio();
        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();
        public CalculoRentaCongestionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("CalculoRentaCongestionController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("CalculoRentaCongestionController", ex);
                throw;
            }
        }

        public ActionResult CalculoRentaCongestion()
        {

            TransferenciaRentaCongestionModel transferenciaRentaCongestion = new TransferenciaRentaCongestionModel();
            transferenciaRentaCongestion.PeriodoRentaCongestion = new TransferenciaRecalculoDTO();

            transferenciaRentaCongestion.ListaPeriodos = (new PeriodoAppServicio()).ListPeriodo();

            //var pericodi = String.IsNullOrEmpty(Request["pericodi"]) ? 0 : Convert.ToInt32(Request["pericodi"]);
            //var recacodi = String.IsNullOrEmpty(Request["recacodi"]) ? 0 : Convert.ToInt32(Request["recacodi"]);

            //var periodoRentaCongestion = servicio.ListPeriodosRentaCongestion(pericodi, recacodi, 1, Funcion.PageSizePeriodoRentaCongestion);
            //if (periodoRentaCongestion.Count > 0)
            //{
            //    transferenciaRentaCongestion.PeriodoRentaCongestion = periodoRentaCongestion.First();
            //    ViewBag.Estado = periodoRentaCongestion.First().RecaEstado;
            //}

            //ViewBag.Pericodi = pericodi;
            //ViewBag.Recacodi = recacodi;

            return View(transferenciaRentaCongestion);            
        }

        public ActionResult ProcesarCalculo(int pericodi, int recacodi, string estado)
        {
            base.ValidarSesionUsuario();

            TransferenciaRentaCongestionModel transferenciaRentaCongestion = new TransferenciaRentaCongestionModel();

            if (estado.Equals("Abierto"))
            {

                servicio.CalcularRentasCongestionPeriodo(pericodi, recacodi, User.Identity.Name);
                PeriodoDTO dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo2 = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, recacodi);
                //---------------------------------------------------------------------------------------------
                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.ProcesarVentasCongestion;
                objAuditoria.Estdcodi = (int)EVtpEstados.GenerarReporteRentasCongestion;
                objAuditoria.Audproproceso = "Procesar rentas de congestión";
                objAuditoria.Audprodescripcion = "Se procesó rentas de congestión del periodo " + dtoPeriodo.PeriNombre + " / " + dtoRecalculo2.RecaNombre;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                _ = this.servicioAuditoria.save(objAuditoria);

                #endregion
            }

            transferenciaRentaCongestion.ListRentaCongestion = servicio.ListRentaCongestion(pericodi, recacodi);

            return PartialView("ListarRentaCongestionEmpresa", transferenciaRentaCongestion);

        }

        public JsonResult ObtenerMontosTotales(int pericodi, int recacodi)
        {
            string indicador = "0.00/0.00";
            try
            {
                var totalRentaCongestion = servicio.GetTotalRentaCongestion(pericodi, recacodi);
                var totalRentaNoAsignada = servicio.GetTotalRentaNoAsignada(pericodi, recacodi);

                indicador = totalRentaCongestion.ToString("N2") + "/" + totalRentaNoAsignada.ToString("N2");
            }
            catch(Exception ex)
            {
                log.Error("CalculoRentaCongestionController", ex);
                return Json(indicador, JsonRequestBehavior.AllowGet);
            }

            return Json(indicador, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ConsultarPeriodoEmpresas(int pericodi, int recacodi)
        {
            base.ValidarSesionUsuario();

            TransferenciaRentaCongestionModel transferenciaRentaCongestion = new TransferenciaRentaCongestionModel();                       

            transferenciaRentaCongestion.ListRentaCongestion = servicio.ListRentaCongestion(pericodi, recacodi);

            return PartialView("ListarRentaCongestionEmpresa", transferenciaRentaCongestion);

        }

        public JsonResult GenerarReporte(int pericodi, int recacodi)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcel(pericodi, recacodi);
                //indicador = 1;
            }
            catch(Exception ex)
            {
                log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }

        private string GenerarArchivoExcel(int pericodi, int recacodi)
        {
            //var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            var preNombre = "Rentas_Congestion_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            TransferenciaRentaCongestionModel transferenciaRentaCongestion = new TransferenciaRentaCongestionModel();

            transferenciaRentaCongestion.ListRentaCongestion = servicio.ListRentaCongestion(pericodi, recacodi);
            transferenciaRentaCongestion.ListRentaCongestionDetalle = servicio.ListRentaCongestionDetalle(pericodi, recacodi);
           

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Obtenemos los cuadros asociados al programa
                //var cuadroProgramas = servicio.GetByCriteriaRcaCuadroProgs(codigoPrograma.ToString(""), "");

                var contFila = 7;
                //var contHojas = 0;
                var nombreHojaAgentes = "Reporte_Principal";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);

                contFila = 2;
                //var contItem = 3;

                ws.Cells[contFila, 2].Value = "Empresa";
                ws.Cells[contFila, 3].Value = "Renta S/.";
                //ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                ws.Cells[contFila, 4].Value = "Reparto RND";
                //ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                ws.Cells[contFila, 5].Value = "Renta Total S/.";
                //ws.Column(5).Style.Numberformat.Format = "#,##0.00";


                ExcelRange rg1 = ws.Cells[contFila, 2, contFila, 5];
                ObtenerEstiloCelda(rg1, 1);

                contFila++;
                foreach (var registro in transferenciaRentaCongestion.ListRentaCongestion)
                {
                    ws.Cells[contFila, 2].Value = string.IsNullOrEmpty(registro.EmprNombre) ? string.Empty : registro.EmprNombre;
                    ws.Cells[contFila, 3].Value = registro.Rcrencrenta;
                    ws.Cells[contFila, 4].Value = registro.Reparto;
                    ws.Cells[contFila, 5].Value = registro.RentaTotal;                    

                    contFila++;
                }

                ws.Column(2).Width = 50;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                

                var nombreHojaOsinergmin = "Detalle_Reporte";
                ws = xlPackage.Workbook.Worksheets.Add(nombreHojaOsinergmin);

                contFila = 2;
                //contItem = 3;

                ws.Cells[contFila, 2].Value = "Generador";
                ws.Cells[contFila, 3].Value = "Cliente";
                ws.Cells[contFila, 4].Value = "Barra";
                ws.Cells[contFila, 5].Value = "Código";
                ws.Cells[contFila, 6].Value = "Rentas Licitación S/";
                ws.Cells[contFila, 7].Value = "Rentas Bilateral S/";

                rg1 = ws.Cells[contFila, 2, contFila, 7];
                ObtenerEstiloCelda(rg1, 1);

                contFila++;
                foreach (var registro in transferenciaRentaCongestion.ListRentaCongestionDetalle)
                {
                    ws.Cells[contFila, 2].Value = string.IsNullOrEmpty(registro.EmprNombre) ? string.Empty : registro.EmprNombre;
                    ws.Cells[contFila, 3].Value = string.IsNullOrEmpty(registro.EmprNombreCliente) ? string.Empty : registro.EmprNombreCliente;
                    ws.Cells[contFila, 4].Value = registro.BarrBarraTransferencia;
                    ws.Cells[contFila, 5].Value = registro.TretCodigo;
                    ws.Cells[contFila, 6].Value = registro.Licitacion;
                    ws.Cells[contFila, 7].Value = registro.Bilateral;                    

                    contFila++;
                }

                ws.Column(2).Width = 50;
                ws.Column(3).Width = 50;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;                

                xlPackage.Save();
            }

            return fileName;
        }
        private string GenerarArchivoExcelErrores(int pericodi, int recacodi)
        {
            //var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            var preNombre = "Errores_Barras" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            TransferenciaRentaCongestionModel transferenciaRentaCongestion = new TransferenciaRentaCongestionModel();

            var perianiomes = servicio.GetPeriodoMes(pericodi);

            var registrosErrores = servicio.ListErroresBarras(pericodi, recacodi, perianiomes);


            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {               

                var contFila = 7;
                //var contHojas = 0;
                var nombreHojaAgentes = "Reporte_Errores";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);

                contFila = 2;
                //var contItem = 3;

                ws.Cells[contFila, 2].Value = "Barra";
                ws.Cells[contFila, 3].Value = "Observación";
                ws.Cells[contFila, 4].Value = "Fecha";              


                ExcelRange rg1 = ws.Cells[contFila, 2, contFila, 4];
                ObtenerEstiloCelda(rg1, 1);

                contFila++;
                foreach (var registro in registrosErrores)
                {
                    ws.Cells[contFila, 2].Value = string.IsNullOrEmpty(registro.BarrBarraTransferencia) ? string.Empty : registro.BarrBarraTransferencia;
                    ws.Cells[contFila, 3].Value = registro.Observacion;
                    ws.Cells[contFila, 4].Value = registro.Fechaobservacion;                    

                    contFila++;
                }

                ws.Column(2).Width = 50;
                ws.Column(3).Width = 50;
                ws.Column(4).Width = 20;               
                                              

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

        public JsonResult ValidarCalculo(int pericodi, int recacodi)
        {
            string indicador = "0";
            try
            {
                var validarPorcentaje = servicio.GetTotalPorcentajes(pericodi, recacodi);

                if (validarPorcentaje.Equals(0))
                {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }

                //var perianiomes = servicio.GetPeriodoMes(pericodi);

                //var registrosErrores = servicio.ListErroresBarras(pericodi, recacodi, perianiomes);

                //if (registrosErrores.Count > 0)
                //{
                //    return Json("-2", JsonRequestBehavior.AllowGet);
                //}

                var existeCostoMarginal = servicio.ValidaCostoMarginal(pericodi, recacodi);

                if (existeCostoMarginal.Equals(0))
                {
                    return Json("-2", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                log.Error("CalculoRentaCongestionController", ex);
                return Json(indicador, JsonRequestBehavior.AllowGet);
            }

            return Json(indicador, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerarReporteErrores(int pericodi, int recacodi)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcelErrores(pericodi, recacodi);
                //indicador = 1;
            }
            catch(Exception ex)
            {
                log.Error("GenerarReporteErrores", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }

        /// <summary>
        /// Permite cargar versiones deacuerdo al periodo
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVersiones(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculo = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            modelRecalculo.ListaRecalculo.Insert(0, new RecalculoDTO { RecaCodi = 0, RecaNombre = "-- Seleccione --" });
            modelRecalculo.bEjecutar = true;
            //Consultamos por el estado del periodo
            //PeriodoDTO entidad = new PeriodoDTO();
            //entidad = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
            //if (entidad.PeriEstado.Equals("Cerrado"))
            //{ modelRecalculo.bEjecutar = false; }
            return Json(modelRecalculo);
        }

        public JsonResult GetVersion(int pericodi, int recacodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.Entidad = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, recacodi);
            modelRecalculo.bEjecutar = false;
            //Consultamos por el estado del periodo            
            if (modelRecalculo.Entidad.RecaEstado.Equals("Abierto"))
            { modelRecalculo.bEjecutar = true; }

            var listTotalRegistros = servicio.ListTotalRegistrosCostosMarginales(pericodi, recacodi);

            using (listTotalRegistros)
            {
                while (listTotalRegistros.Read()) 
                {
                    modelRecalculo.NumeroRegistros = Convert.ToInt32(listTotalRegistros["NROREGS"]);

                    if (listTotalRegistros["FECULTACTUALIZACION"] != null && !listTotalRegistros["FECULTACTUALIZACION"].ToString().Equals(string.Empty))
                    {
                        modelRecalculo.UltimaFechaActualizacion = Convert.ToDateTime(listTotalRegistros["FECULTACTUALIZACION"]).ToString("dd/MM/yyyy HH:mm:ss");
                    }else
                    {
                        modelRecalculo.UltimaFechaActualizacion = string.Empty;
                    }
                }
 
            }

            return Json(modelRecalculo);
        }

        public JsonResult GenerarReporteCostosMarginales(int pericodi, int recacodi)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoCostosMarginales(pericodi, recacodi);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporteCostosMarginales", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }

        private string GenerarArchivoCostosMarginales(int pericodi, int recacodi)
        {
            //var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            var preNombre = "Reporte_Costos_Marginales_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            TransferenciaRentaCongestionModel transferenciaRentaCongestion = new TransferenciaRentaCongestionModel();

            var perianiomes = servicio.GetPeriodoMes(pericodi);

            var listCostosMarginales = servicio.ListCostosMarginales(pericodi, recacodi, perianiomes);

            var anio = perianiomes.ToString().Substring(0, 4);
            var mes = perianiomes.ToString().Substring(4, 2);
            var fechaHoraInicio = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);
            fechaHoraInicio = fechaHoraInicio.AddMinutes(15);

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var contFila = 11;
                var contColumnas = 2;
                var nombreHojaAgentes = "Costos Marginales";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);

                //contFila = 6;
                //var contItem = 3;

                ws.Cells[6, 1].Value = "EMPRESA";
                ws.Cells[7, 1].Value = "Cliente / Unidad";
                ws.Cells[8, 1].Value = "Barra";
                ws.Cells[9, 1].Value = "Codigo";
                ws.Cells[10, 1].Value = "Tipo";

                ws.Cells[2, 2].Value = "HISTÓRICO DE COSTOS MARGINALES (MW.h) EN INTERVALOS DE 15 min.";
                

                //var culture = new System.CurrCultureInfo( "en-GB" );
                ws.Cells[3, 2].Value = fechaHoraInicio.ToString("MM/yyyy");       
                //var columnaInicio = 1;
                //var cambioFecha = false;
                using (listCostosMarginales)
                {
                    while (listCostosMarginales.Read())
                    {
                        //incremetar numero de fila
                        var fecha = Convert.ToDateTime(listCostosMarginales["INTERVALO"]);

                        if (fecha > fechaHoraInicio)
                        {
                            contFila++;
                            fechaHoraInicio = fechaHoraInicio.AddMinutes(15);
                            contColumnas = 2;
                        }

                        ws.Cells[contFila, 1].Value = fecha.ToString("dd/MM/yyyy HH:mm");

                        if (contFila == 11)
                        {
                            ws.Cells[6, contColumnas].Value = listCostosMarginales["EMPRNOMB"].ToString().Trim();
                            ws.Cells[7, contColumnas].Value = listCostosMarginales["EQUINOMB"].ToString().Trim();
                            ws.Cells[8, contColumnas].Value = listCostosMarginales["BARRNOMBRE"].ToString().Trim();
                            ws.Cells[9, contColumnas].Value = listCostosMarginales["TENTCODIGO"].ToString().Trim();
                            ws.Cells[10, contColumnas].Value = listCostosMarginales["TIPO"].ToString();
                        }

                        if (listCostosMarginales["CMGRCONGESTION"] != null && !listCostosMarginales["CMGRCONGESTION"].ToString().Equals(string.Empty))
                        {
                             ws.Cells[contFila, contColumnas].Value =  Convert.ToDecimal(listCostosMarginales["CMGRCONGESTION"]);
                             //ws.Cells[contFila, contColumnas].Style.Numberformat.Format = "#,##0.000000000000;-#,##0.000000000000;0";
                        }
                       
                        contColumnas++;    
                    }

                }

                ws.Column(1).Width = 20;

                for (int i = 2; i < contColumnas;i++)
                {
                    ws.Column(i).Width = 30;
                    ws.Column(i).Style.Numberformat.Format = "#,##0.000000000000;-#,##0.000000000000";
                }
                ExcelRange rg1 = ws.Cells[6, 1, 10, contColumnas-1];
                ObtenerEstiloCelda(rg1, 1);

                //Costos Marginales Por Barra
                var nombreCostosMarginalesPorBarra = "Costos Marginales Por Barra";
                ws = xlPackage.Workbook.Worksheets.Add(nombreCostosMarginalesPorBarra);

                var listCostosMarginalesPorBarra = servicio.ListCostosMarginalesPorBarra(pericodi, recacodi, perianiomes);

                fechaHoraInicio = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);
                fechaHoraInicio = fechaHoraInicio.AddMinutes(30);

                ws.Cells[2, 2].Value = "HISTÓRICO DE COSTO MARGINAL EN INTERVALOS DE 30 min.";
                //var culture = new System.CurrCultureInfo( "en-GB" );
                ws.Cells[3, 2].Value = fechaHoraInicio.ToString("MM/yyyy");

                ws.Cells[6, 1].Value = "BARRA";

                contFila = 7;
                contColumnas = 2;

                using (listCostosMarginalesPorBarra)
                {
                    while (listCostosMarginalesPorBarra.Read())
                    {
                        //incremetar numero de fila
                        var fecha = Convert.ToDateTime(listCostosMarginalesPorBarra["INTERVALO"]);

                        if (fecha > fechaHoraInicio)
                        {
                            contFila++;
                            fechaHoraInicio = fechaHoraInicio.AddMinutes(30);
                            contColumnas = 2;
                        }

                        ws.Cells[contFila, 1].Value = fecha.ToString("dd/MM/yyyy HH:mm");

                        if (contFila == 7)
                        {
                            ws.Cells[6, contColumnas].Value = listCostosMarginalesPorBarra["BARRNOMBRE"].ToString().Trim();                           
                        }

                        if (listCostosMarginalesPorBarra["CMGRCONGESTION"] != null && !listCostosMarginalesPorBarra["CMGRCONGESTION"].ToString().Equals(string.Empty))
                        {
                            ws.Cells[contFila, contColumnas].Value = Convert.ToDecimal(listCostosMarginalesPorBarra["CMGRCONGESTION"]);
                            //ws.Cells[contFila, contColumnas].Style.Numberformat.Format = "#,##0.000000000000;-#,##0.000000000000;0";
                        }

                        contColumnas++;
                    }

                }

                ws.Column(1).Width = 20;

                for (int i = 2; i < contColumnas; i++)
                {
                    ws.Column(i).Width = 30;
                    ws.Column(i).Style.Numberformat.Format = "#,##0.000000000000;-#,##0.000000000000";
                }
                rg1 = ws.Cells[6, 1, 6, contColumnas - 1];
                ObtenerEstiloCelda(rg1, 1);

                xlPackage.Save();
            }

            return fileName;
        }

        public JsonResult GenerarReporteEntregasRetiros(int pericodi, int recacodi)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoEntregasRetiros(pericodi, recacodi);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporteEntregasRetiros", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }

        private string GenerarArchivoEntregasRetiros(int pericodi, int recacodi)
        {
            //var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            var preNombre = "Reporte_Entregas_Retiros_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            TransferenciaRentaCongestionModel transferenciaRentaCongestion = new TransferenciaRentaCongestionModel();

            var perianiomes = servicio.GetPeriodoMes(pericodi);
            var anio = perianiomes.ToString().Substring(0, 4);
            var mes = perianiomes.ToString().Substring(4, 2);
            var fechaHoraInicio = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);
            var ultimoDiaMes = fechaHoraInicio.AddMonths(1).AddDays(-1).Day;

            var listCostosMarginales = servicio.ListEntregasRetiros(pericodi,recacodi, perianiomes, ultimoDiaMes);

            //var anio = perianiomes.ToString().Substring(0, 4);
            //var mes = perianiomes.ToString().Substring(4, 2);
            fechaHoraInicio = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);
            fechaHoraInicio = fechaHoraInicio.AddMinutes(15);

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var contFila = 11;
                var contColumnas = 2;
                var nombreHojaAgentes = "Entregas y Retiros";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);

                //contFila = 6;
                //var contItem = 3;

                ws.Cells[6, 1].Value = "EMPRESA";
                ws.Cells[7, 1].Value = "Cliente / Unidad";
                ws.Cells[8, 1].Value = "Barra";
                ws.Cells[9, 1].Value = "Codigo";
                ws.Cells[10, 1].Value = "Tipo";

                ws.Cells[2, 2].Value = "HISTÓRICO DE ENERGÍA (MW.h) EN INTERVALOS DE 15 min.";


                //var culture = new System.CurrCultureInfo( "en-GB" );
                ws.Cells[3, 2].Value = fechaHoraInicio.ToString("MM/yyyy");
                //var columnaInicio = 1;
                //var cambioFecha = false;
                using (listCostosMarginales)
                {
                    while (listCostosMarginales.Read())
                    {
                        //incremetar numero de fila
                        var fecha = Convert.ToDateTime(listCostosMarginales["INTERVALO"]);

                        if (fecha > fechaHoraInicio)
                        {
                            contFila++;
                            fechaHoraInicio = fechaHoraInicio.AddMinutes(15);
                            contColumnas = 2;
                        }

                        ws.Cells[contFila, 1].Value = fecha.ToString("dd/MM/yyyy HH:mm");

                        if (contFila == 11)
                        {
                            ws.Cells[6, contColumnas].Value = listCostosMarginales["EMPRNOMB"].ToString().Trim();
                            ws.Cells[7, contColumnas].Value = listCostosMarginales["EQUINOMB"].ToString().Trim();
                            ws.Cells[8, contColumnas].Value = listCostosMarginales["BARRNOMBRE"].ToString().Trim();
                            ws.Cells[9, contColumnas].Value = listCostosMarginales["TENTCODIGO"].ToString().Trim();
                            ws.Cells[10, contColumnas].Value = listCostosMarginales["TIPO"].ToString();
                        }

                        if (listCostosMarginales["TENTDEENERGIA"] != null && !listCostosMarginales["TENTDEENERGIA"].ToString().Equals(string.Empty))
                        {
                            ws.Cells[contFila, contColumnas].Value = Convert.ToDecimal(listCostosMarginales["TENTDEENERGIA"]);
                            //ws.Cells[contFila, contColumnas].Style.Numberformat.Format = "#,##0.000000000000;-#,##0.000000000000;0";
                        }
                        //ws.Cells[contFila, contColumnas].Value = listCostosMarginales["TENTDEENERGIA"] != null ? listCostosMarginales["TENTDEENERGIA"].ToString() : string.Empty;

                        contColumnas++;

                        //ws.Cells[nroFila, 1].Value = (list["DIA"] != null) ? Convert.ToDateTime(list["DIA"]).ToString("dd/MM/yyyy") : string.Empty;
                        //ws.Cells[nroFila, 2].Value = list["CALIFICACION"].ToString();
                        //indiceIni = 2;
                        //foreach (var grupo in listaGrupo)
                        //{
                        //    indiceIni++;
                        //    ws.Cells[nroFila, indiceIni].Value = list["C" + grupo.Grupocodi];
                        //}
                        ////ws.Cells[nroFila, indiceIni + 1].Value = list["TOTAL"];

                        //// Asigna formato numerico
                        //ws.Cells[nroFila, 3, nroFila, indiceIni].Style.Numberformat.Format = "#,##0.00;-#,##0.00;0";

                        //rg = ws.Cells[nroFila, 1, nroFila, indiceIni];
                        //rg = ObtenerEstiloCelda(rg, 0);
                    }

                }

                ws.Column(1).Width = 20;

                for (int i = 2; i < contColumnas; i++)
                {
                    ws.Column(i).Width = 30;
                    ws.Column(i).Style.Numberformat.Format = "#,##0.000000000000;-#,##0.000000000000";
                }
                ExcelRange rg1 = ws.Cells[6, 1, 10, contColumnas - 1];
                ObtenerEstiloCelda(rg1, 1);

                xlPackage.Save();
            }

            return fileName;
        }

        public JsonResult copiarCostosMarginales(int pericodi, int recacodi, int tipoCopia)
        {
            string indicador = "0";
            try
            {
                base.ValidarSesionUsuario();
                //Validamos si existe registros en la cabecera
                var rcgCostoMarginalCab = new RcgCostoMarginalCabDTO();
                var listRcgCostoMarginalCab = servicio.ListRcgCostoMarginalCab(pericodi, recacodi).ToList();

                
                if (listRcgCostoMarginalCab.Count > 0)
                {
                    //Actualizamos el nombre de usuario y fecha
                    rcgCostoMarginalCab.Rccmgccodi = listRcgCostoMarginalCab.First().Rccmgccodi;
                    rcgCostoMarginalCab.Pericodi = pericodi;
                    rcgCostoMarginalCab.Recacodi = recacodi;
                    rcgCostoMarginalCab.Rccmgcusumodificacion = User.Identity.Name;
                    rcgCostoMarginalCab.Rccmgcfecmodificacion = DateTime.Now;

                    //servicio.UpdateRcgCostoMarginalCab(rcgCostoMarginalCab);

                }
                else
                {
                    //Insertamos el registro en la tabla
                    rcgCostoMarginalCab.Pericodi = pericodi;
                    rcgCostoMarginalCab.Recacodi = recacodi;
                    rcgCostoMarginalCab.Rccmgcusucreacion = User.Identity.Name;
                    rcgCostoMarginalCab.Rccmgcfeccreacion = DateTime.Now;

                    rcgCostoMarginalCab.Rccmgccodi = servicio.SaveRcgCostoMarginalCab(rcgCostoMarginalCab);
                }

                
                //Eliminamos los datos de detalle

                servicio.DeleteDatosRcgCostoMarginalDet(rcgCostoMarginalCab.Rccmgccodi);

                var maximoCostoMarginalDetalleId = servicio.GetMaximoCostoMarginalDetalleId();
                // Insercion de Datos
                if (tipoCopia.Equals(1))//Datos SEV
                {
                    var perianiomes = servicio.GetPeriodoMes(pericodi);

                    servicio.SaveCostoMarginalDetalleSEV(maximoCostoMarginalDetalleId, rcgCostoMarginalCab.Rccmgccodi, perianiomes);
                    
                }else
                {
                    recacodi = recacodi - 1;
                    servicio.SaveCostoMarginalDetalleCalculoAnterior(maximoCostoMarginalDetalleId, rcgCostoMarginalCab.Rccmgccodi, pericodi, recacodi);
                }
                
            }
            catch (Exception ex)
            {
                indicador = "-1";
                log.Error("CalculoRentaCongestionController", ex);
                return Json(indicador, JsonRequestBehavior.AllowGet);
            }

            return Json(indicador, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarCopiaCostosMarginales(int pericodi, int recacodi)
        {
            string indicador = "0";
            try
            {                
                var perianiomes = servicio.GetPeriodoMes(pericodi);

                var registrosErrores = servicio.ListErroresBarras(pericodi, recacodi, perianiomes);

                if (registrosErrores.Count > 0)
                {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }

                

            }
            catch (Exception ex)
            {
                log.Error("CalculoRentaCongestionController", ex);
                return Json(indicador, JsonRequestBehavior.AllowGet);
            }

            return Json(indicador, JsonRequestBehavior.AllowGet);
        }
    }
}
