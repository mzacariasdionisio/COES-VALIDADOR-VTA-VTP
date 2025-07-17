using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.DevolucionAportes;
using log4net;
using Novacode;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DevolucionAporte.Controllers
{
    [ValidarSesion]
    public class CuadroDevolucionesController : BaseController
    {
        DevolucionAportesAppServicio _svcDevolucionAporte = new DevolucionAportesAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CuadroDevolucionesController));
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("CuadroDevolucionesController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("CuadroDevolucionesController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            List<DaiAportanteDTO> listadoAniosCronograma = _svcDevolucionAporte.GetByCriteriaAniosCronograma(0, 0);
            ViewBag.ListadoAniosCronograma = listadoAniosCronograma;
            return View();
        }

        public ActionResult Listado()
        {
            List<DaiAportanteDTO> listadoAportantes = new List<DaiAportanteDTO>();
            try
            {
                DaiTablacodigoDetalleDTO tablaCodigoDetalle = _svcDevolucionAporte.GetByIdDaiTablacodigoDetalle(Convert.ToInt32(TablaCodigoDetalleDAI.IGV));
                decimal igv = 0;

                if (tablaCodigoDetalle != null)
                {
                    if (Convert.ToDecimal(tablaCodigoDetalle.Tabdvalor) > 0)
                    {
                        igv = Convert.ToDecimal(tablaCodigoDetalle.Tabdvalor) / 100;
                    }
                }

                listadoAportantes = _svcDevolucionAporte.ListCuadroDevolucion(0, DateTime.Now.Year, Convert.ToInt32(DaiEstadoCronogramaCuota.Pagado));
            }
            catch (Exception ex)
            {
                log.Error("Listado", ex);
            }
            
            return PartialView(listadoAportantes);
        }
        
        public decimal CalcularMonto(decimal number, decimal percent)
        {
            //return ((double) 80         *       25)/100;
            return ((decimal)number * percent) / 100;
        }

        private double CalcularCuota(double monto, int mesesPlazo, double taza)
        {      //PMT = -RATE * ( FV + PV * Math.pow(1+RATE,NPER)) / ((Math.pow(1+RATE,NPER)-1));//double Monto = 100000;      int Plazos = 60;      double taza = 0.02;
            double t = taza / 100;
            double b = Math.Pow((1 + t), mesesPlazo);
            return t * monto * b / (b - 1);
        }

        public FileContentResult GenerarReporte(int anio, string aports)
        {
            base.ValidarSesionUsuario();

            MemoryStream ms = new MemoryStream();

            DaiTablacodigoDetalleDTO tasadetalle = _svcDevolucionAporte.GetByIdDaiTablacodigoDetalle(Convert.ToInt32(TablaCodigoDetalleDAI.Tasa_Interes));
            DaiTablacodigoDetalleDTO igvdetalle = _svcDevolucionAporte.GetByIdDaiTablacodigoDetalle(Convert.ToInt32(TablaCodigoDetalleDAI.IGV));

            decimal tasa = 0;
            decimal igv = 0;

            if (tasadetalle != null)
            {
                tasa = Convert.ToDecimal(tasadetalle.Tabdvalor);
            }

            if (igvdetalle != null)
            {
                igv = Convert.ToDecimal(tasadetalle.Tabdvalor);
            }
            
            string nombre = "CuadroDevoluciones";

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("CuadroDevoluciones");
                ws.View.ShowGridLines = true;

                ws.Cells["A1:F3"].Merge = true;
                ws.Cells["A1:F3"].Style.Font.Size = 11;
                ws.Cells["A1:F3"].Style.Font.Bold = true;
                ws.Cells["A1:F3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A1:F3"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["A1:F3"].Value = "INTERESES Y AMORTIZACIONES " + anio.ToString();
                ws.Cells["A1:F3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells["A1:F3"].Style.WrapText = true;
                
                ws.Cells["A5:F5"].Merge = true;
                ws.Cells["A5:F5"].Style.Font.Size = 11;
                ws.Cells["A5:F5"].Style.Font.Bold = true;
                ws.Cells["A5:F5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A5:F5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A5:F5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["A5:F5"].Value = "(EXPRESADO EN DOLARES)";
                ws.Cells["A5:F5"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["A7:A7"].Style.Font.Size = 11;
                ws.Cells["A7:A7"].Style.Font.Bold = true;
                ws.Cells["A7:A7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A7:A7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A7:A7"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["A7:A7"].Value = "N°";
                ws.Cells["A7:A7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                ws.Column(2).Width = 50;
                ws.Column(3).Width = 25;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 25;
                ws.Cells["B7:B7"].Style.Font.Size = 11;
                ws.Cells["B7:B7"].Style.Font.Bold = true;
                ws.Cells["B7:B7"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["B7:B7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["B7:B7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B7:B7"].Value = "EMPRESAS";
                ws.Cells["B7:B7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["C7:C7"].Merge = true;
                ws.Cells["C7:C7"].Style.Font.Size = 11;
                ws.Cells["C7:C7"].Style.Font.Bold = true;
                ws.Cells["C7:C7"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["C7:C7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C7:C7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["C7:C7"].Value = "INTERES";
                ws.Cells["C7:C7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["D7:D7"].Merge = true;
                ws.Cells["D7:D7"].Style.Font.Size = 11;
                ws.Cells["D7:D7"].Style.Font.Bold = true;
                ws.Cells["D7:D7"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["D7:D7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["D7:D7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["D7:D7"].Value = "IGV (" + igvdetalle.Tabdvalor + "%)";
                ws.Cells["D7:D7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["E7:E7"].Merge = true;
                ws.Cells["E7:E7"].Style.Font.Size = 11;
                ws.Cells["E7:E7"].Style.Font.Bold = true;
                ws.Cells["E7:E7"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["E7:E7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["E7:E7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["E7:E7"].Value = "INTERESES CON IGV";
                ws.Cells["E7:E7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["F7:F7"].Merge = true;
                ws.Cells["F7:F7"].Style.Font.Size = 11;
                ws.Cells["F7:F7"].Style.Font.Bold = true;
                ws.Cells["F7:F7"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["F7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["F7:F7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["F7:F7"].Value = "AMORTIZACION";
                ws.Cells["F7:F7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["G7:G7"].Merge = true;
                ws.Cells["G7:G7"].Style.Font.Size = 11;
                ws.Cells["G7:G7"].Style.Font.Bold = true;
                ws.Cells["G7:G7"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["G7:G7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["G7:G7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["G7:G7"].Value = "TOTAL";
                ws.Cells["G7:G7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                int i = 8;
                int contador = 1;
                
                string[] aaports = aports.Split(',');
                decimal totalIntereses = 0;
                decimal totalIGV = 0;
                decimal sumTotalInteresesIGV = 0;
                decimal sumAmortizacion = 0;
                decimal sumTotal = 0;

                DaiTablacodigoDetalleDTO tablaCodigoDetalle = _svcDevolucionAporte.GetByIdDaiTablacodigoDetalle(Convert.ToInt32(TablaCodigoDetalleDAI.IGV));
                decimal Igvvalor = 0;

                if (tablaCodigoDetalle != null)
                {
                    if (Convert.ToDecimal(tablaCodigoDetalle.Tabdvalor) > 0)
                    {
                        Igvvalor = Convert.ToDecimal(tablaCodigoDetalle.Tabdvalor) / 100;
                    }
                }

                List<DaiAportanteDTO> listadoAportantes = _svcDevolucionAporte.ListCuadroDevolucion(Igvvalor, anio, Convert.ToInt32(DaiEstadoCronogramaCuota.Pendiente));
                for (int x = 0; x < aaports.Length; x++)
                {
                    DaiAportanteDTO aportanteMonto = listadoAportantes.FirstOrDefault(a => a.Emprcodi == Convert.ToInt32(aaports[x]));

                    List<DaiCalendariopagoDTO> calendarioPago = _svcDevolucionAporte.GetByCriteriaDaiCalendariopagos(Convert.ToInt32(aaports[x]));
                    //List<DaiCalendariopagoDTO> calendarioPagoAnio = calendarioPago.Where(c => Convert.ToInt32(c.Caleanio) <= anio).ToList();
                    
                    ws.Cells[string.Format("A{0}:A{0}", i)].Merge = true;
                    ws.Cells[string.Format("A{0}:A{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("A{0}:A{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("A{0}:A{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("A{0}:A{0}", i)].Value = x + 1;
                    ws.Cells[string.Format("A{0}:A{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("A{0}:A{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("B{0}:B{0}", i)].Merge = true;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Value = String.Format("{0:0,0.0}", aportanteMonto.Emprrazsocial);
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("C{0}:C{0}", i)].Merge = true;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Value = String.Format("{0:0,0.0}", aportanteMonto.Caleinteres);
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.WrapText = true;

                    totalIntereses += aportanteMonto.Caleinteres;

                    ws.Cells[string.Format("D{0}:D{0}", i)].Merge = true;
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("D{0}:D{0}", i)].Value = String.Format("{0:0,0.0}", aportanteMonto.Caleinteresigv);
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.WrapText = true;

                    totalIGV += aportanteMonto.Caleinteresigv;

                    ws.Cells[string.Format("E{0}:E{0}", i)].Merge = true;
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("E{0}:E{0}", i)].Value = String.Format("{0:0,0.0}", aportanteMonto.Totalinteresesigv);
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.WrapText = true;

                    sumTotalInteresesIGV += aportanteMonto.Totalinteresesigv;

                    ws.Cells[string.Format("F{0}:F{0}", i)].Merge = true;
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("F{0}:F{0}", i)].Value = String.Format("{0:0,0.0}", aportanteMonto.Amortizacion);
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.WrapText = true;

                    sumAmortizacion += aportanteMonto.Amortizacion;
                    
                    ws.Cells[string.Format("G{0}:G{0}", i)].Merge = true;
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("G{0}:G{0}", i)].Value = String.Format("{0:0,0.0}", aportanteMonto.Total);
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.WrapText = true;

                    sumTotal += aportanteMonto.Total;

                    i += 1;
                    contador++;
                }
                
                ws.Cells[string.Format("A{0}:B{0}", i)].Merge = true;
                ws.Cells[string.Format("A{0}:B{0}", i)].Style.Font.Size = 11;
                ws.Cells[string.Format("A{0}:B{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[string.Format("A{0}:B{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("A{0}:B{0}", i)].Value = "TOTAL";
                ws.Cells[string.Format("A{0}:B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("A{0}:B{0}", i)].Style.WrapText = true;

                ws.Cells[string.Format("C{0}:C{0}", i)].Merge = true;
                ws.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Size = 11;
                ws.Cells[string.Format("C{0}:C{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[string.Format("C{0}:C{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[string.Format("C{0}:C{0}", i)].Value = String.Format("{0:0,0.0}", totalIntereses);
                ws.Cells[string.Format("C{0}:C{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("C{0}:C{0}", i)].Style.WrapText = true;

                ws.Cells[string.Format("D{0}:D{0}", i)].Merge = true;
                ws.Cells[string.Format("D{0}:D{0}", i)].Style.Font.Size = 11;
                ws.Cells[string.Format("D{0}:D{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[string.Format("D{0}:D{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[string.Format("D{0}:D{0}", i)].Value = String.Format("{0:0,0.0}", totalIGV);
                ws.Cells[string.Format("D{0}:D{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("D{0}:D{0}", i)].Style.WrapText = true;

                ws.Cells[string.Format("E{0}:E{0}", i)].Merge = true;
                ws.Cells[string.Format("E{0}:E{0}", i)].Style.Font.Size = 11;
                ws.Cells[string.Format("E{0}:E{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[string.Format("E{0}:E{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[string.Format("E{0}:E{0}", i)].Value = String.Format("{0:0,0.0}", sumTotalInteresesIGV);
                ws.Cells[string.Format("E{0}:E{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("E{0}:E{0}", i)].Style.WrapText = true;

                ws.Cells[string.Format("F{0}:F{0}", i)].Merge = true;
                ws.Cells[string.Format("F{0}:F{0}", i)].Style.Font.Size = 11;
                ws.Cells[string.Format("F{0}:F{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[string.Format("F{0}:F{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[string.Format("F{0}:F{0}", i)].Value = String.Format("{0:0,0.0}", sumAmortizacion);
                ws.Cells[string.Format("F{0}:F{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("F{0}:F{0}", i)].Style.WrapText = true;

                ws.Cells[string.Format("G{0}:G{0}", i)].Merge = true;
                ws.Cells[string.Format("G{0}:G{0}", i)].Style.Font.Size = 11;
                ws.Cells[string.Format("G{0}:G{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[string.Format("G{0}:G{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[string.Format("G{0}:G{0}", i)].Value = String.Format("{0:0,0.0}", sumTotal);
                ws.Cells[string.Format("G{0}:G{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("G{0}:G{0}", i)].Style.WrapText = true;

                var FileBytesArray = package.GetAsByteArray();
                return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombre + ".xlsx");
            }
        }
    }
}
