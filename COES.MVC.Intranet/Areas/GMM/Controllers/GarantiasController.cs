using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.GMM.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.GMM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Collections;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Globalization;
using COES.Servicios.Aplicacion.Mediciones;
using log4net;
using System.Web.Script.Serialization;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.General;
using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.Helper;
using System.Reflection;
using COES.Servicios.Aplicacion.PMPO;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.GMM.Controllers
{
    public class GarantiasController : BaseController
    {

        GarantiaAppServicio garantiaAppServicio = new GarantiaAppServicio();
        ProgramacionAppServicio programacionAppServicio = new ProgramacionAppServicio();
        CalculoPorcentajesAppServicio servicioCalculoPorcentaje = new CalculoPorcentajesAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(GarantiasController));

        public GarantiasController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            //base.OnException(filterContext);

            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("GarantiaController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("GarantiaController", ex);
                throw;
            }
        }

        // GET: /GMM/Garantia/

        public ActionResult Index()
        {
            GarantiaModel model = new GarantiaModel();
            DateTime fechaActual = DateTime.Today;
            int anio = fechaActual.Year;
            int mes = fechaActual.Month;
            model.mensajeProcesamiento = garantiaAppServicio.mensajeProcesamientoParticipante(anio, mes.ToString()).FirstOrDefault();
            model.mensajeProcesamiento = model.mensajeProcesamiento ?? new GmmGarantiaDTO();
            model.anho = anio;
            model.mes = mes;
            return View(model);
        }

        [HttpPost]
        public ActionResult MensajeProcesamiento(int anio, string mes)
        {
            GmmGarantiaDTO entitie = new GmmGarantiaDTO();
            entitie = garantiaAppServicio.mensajeProcesamientoParticipante(anio, mes).FirstOrDefault();
            //Mensaje con resultados 
            int imes = 0;
            if (!Int32.TryParse(mes, out imes))
            {
                imes = -1;
            }
            List<GmmDatCalculoDTO> datosRPT1 = garantiaAppServicio.listarRpt1(anio, imes);

            entitie.Mensaje2 = "Resultados:\n";
            foreach (var item in datosRPT1)
            {
                entitie.EMPRESA = item.EMPRESA.ToString().Trim();
                entitie.RENERGIA = item.RENERGIA.ToString("#,##0.00");//"POR ENERGÍA ACTIVA";
                entitie.RCAPACIDAD = item.RCAPACIDAD.ToString("#,##0.00");//"POR CAPACIDAD";
                entitie.RPEAJE = item.RPEAJE.ToString("#,##0.00");//"POR PEAJE DE CONEXIÓN";
                entitie.RSCOMPLE = item.RSCOMPLE.ToString("#,##0.00");//"POR SERVICIOS COMPLEMENTARIOS";
                entitie.RINFLEXOP = item.RINFLEXOP.ToString("#,##0.00");//"POR INFLEXIBILIDADES OPERATIVAS";
                entitie.REREACTIVA = item.REREACTIVA.ToString("#,##0.00");//"POR EXCESO DE CONSUMO DE ENERGÍA REACTIVA";
                entitie.TOTALGARANTIA = item.TOTALGARANTIA.ToString("#,##0.00");// "MONTO TOTAL GARANTÍA S/";               
            }

            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(entitie));
        }

        public JsonResult ListarGarantias(int anio, string mes)
        {
            GarantiaModel model = new GarantiaModel();
            GmmGarantiaDTO entitie = new GmmGarantiaDTO();
            entitie = garantiaAppServicio.mensajeProcesamientoParticipante(anio, mes).FirstOrDefault();
            //Mensaje con resultados 
            int imes = 0;
            if (!Int32.TryParse(mes, out imes))
            {
                imes = -1;
            }
            List<GmmDatCalculoDTO> datosRPT1 = garantiaAppServicio.listarRpt1(anio, imes);
            List<GmmGarantiaDTO> entitys = new List<GmmGarantiaDTO>();
            entitie = entitie ?? new GmmGarantiaDTO();
            entitie.Mensaje2 = "Resultados:\n";
            foreach (var item in datosRPT1)
            {
                entitie = new GmmGarantiaDTO();
                entitie.EMPRESA = item.EMPRESA.ToString().Trim();
                entitie.RENERGIA = item.RENERGIA.ToString("#,##0.00");//"POR ENERGÍA ACTIVA";
                entitie.RCAPACIDAD = item.RCAPACIDAD.ToString("#,##0.00");//"POR CAPACIDAD";
                entitie.RPEAJE = item.RPEAJE.ToString("#,##0.00");//"POR PEAJE DE CONEXIÓN";
                entitie.RSCOMPLE = item.RSCOMPLE.ToString("#,##0.00");//"POR SERVICIOS COMPLEMENTARIOS";
                entitie.RINFLEXOP = item.RINFLEXOP.ToString("#,##0.00");//"POR INFLEXIBILIDADES OPERATIVAS";
                entitie.REREACTIVA = item.REREACTIVA.ToString("#,##0.00");//"POR EXCESO DE CONSUMO DE ENERGÍA REACTIVA";
                entitie.TOTALGARANTIA = item.TOTALGARANTIA.ToString("#,##0.00");// "MONTO TOTAL GARANTÍA S/";
                                                                                // 
                entitys.Add(entitie);
            }

            model.listaGarantias = entitys;
            model.Mensaje2 = "Resultados:\n";

            return Json(model, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult exportarReporte(int anio, int mes, string tipo)
        {
            string indicador = "1";
            string mesCadenaAct = "";
            string mesCadenaAnt = "";
            string mesCadenaSig = "";
            string mesNombre = "";
            int anioMesAnt = 0;
            int anioMesSig = 0;

            // tipos rpt1 =Monto de Garantias para respaldar obligaciones de pago en el Mercado Mayorista
            // tipos rpt2 = Montos de Garantías por energía activa
            // tipos rpt3 = Montos de Garantías por capacidad y peaje SPT - SGT
            // tipos rpt4 = Montos de Garantías por servicios complementarios
            // tipos rpt5 = Montos de Garantías por recaudación de energía reactiva
            // tipos rpt6 = Montos de Garantías por Inflexiblidades operativas


            switch (mes)
            {
                case 1:
                    mesCadenaAct = "Ene";
                    anioMesAnt = anio - 1;
                    mesCadenaAnt = "Dic";
                    mesCadenaSig = "Feb";
                    anioMesSig = anio;
                    mesNombre = "Enero";
                    break;
                case 2:
                    mesCadenaAct = "Feb";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Ene";
                    mesCadenaSig = "Mar";
                    mesNombre = "Febrero";
                    anioMesSig = anio;
                    break;
                case 3:
                    mesCadenaAct = "Mar";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Feb";
                    mesCadenaSig = "Abr";
                    mesNombre = "Marzo";
                    anioMesSig = anio;
                    break;
                case 4:
                    mesCadenaAct = "Abr";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Mar";
                    mesCadenaSig = "May";
                    mesNombre = "Abril";
                    anioMesSig = anio;
                    break;
                case 5:
                    mesCadenaAct = "May";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Abr";
                    mesCadenaSig = "Jun";
                    mesNombre = "Mayo";
                    anioMesSig = anio;
                    break;
                case 6:
                    mesCadenaAct = "Jun";
                    anioMesAnt = anio;
                    mesCadenaAnt = "May";
                    mesNombre = "Junio";
                    mesCadenaSig = "Jul";
                    anioMesSig = anio;
                    break;
                case 7:
                    mesCadenaAct = "Jul";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Jun";
                    mesCadenaSig = "Ago";
                    mesNombre = "Julio";
                    anioMesSig = anio;
                    break;
                case 8:
                    mesCadenaAct = "Ago";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Jul";
                    mesCadenaSig = "Set";
                    mesNombre = "Agosto";
                    anioMesSig = anio;
                    break;
                case 9:
                    mesCadenaAct = "Set";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Ago";
                    mesCadenaSig = "Oct";
                    mesNombre = "Setiembre";
                    anioMesSig = anio;
                    break;
                case 10:
                    mesCadenaAct = "Oct";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Set";
                    mesCadenaSig = "Nov";
                    mesNombre = "Octubre";
                    anioMesSig = anio;
                    break;
                case 11:
                    mesCadenaAct = "Nov";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Oct";
                    mesCadenaSig = "Dic";
                    mesNombre = "Noviembre";
                    anioMesSig = anio;
                    break;
                case 12:
                    mesCadenaAct = "Dic";
                    anioMesAnt = anio;
                    mesCadenaAnt = "Nov";
                    mesCadenaSig = "Ene";
                    mesNombre = "Diciembre";
                    anioMesSig = anio + 1;
                    break;
                default:

                    break;
            }

            GarantiaModel model = new GarantiaModel();
            GarantiaAppServicio garantiaAppServicio = new GarantiaAppServicio();

            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
            FileInfo newFile = new FileInfo(path + "");
            int nroReg = 0;

            try
            {

                switch (tipo)
                {
                    #region Listado de Insumos
                    case "rptInsumos":
                        List<GmmDatCalculoDTO> datosRptInsumo = garantiaAppServicio.listarRptInsumo(anio, mes);
                        //nroReg = 0;

                        path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                        newFile = new FileInfo(path + Funcion.NombreRptInsumo);

                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(path + Funcion.NombreRptInsumo);
                        }

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Insumos");

                            if (ws != null)
                            {
                                //TITULO
                                int row = 2; //fila donde inicia la data
                                ws.Cells[row++, 4].Value = "INSUMOS PARA EL PROCESAMIENTO DEL CÁLCULO" + anio + "-" + mes;
                                row++;
                                ExcelRange rg = ws.Cells[2, 3, row, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;
                                row++;
                                //CABECERA DE TABLA
                                ws.Cells[row, 2].Value = "AÑO";
                                ws.Cells[row, 3].Value = "MES";
                                ws.Cells[row, 4].Value = "PERIODO";
                                ws.Cells[row, 5].Value = "EMPRESA";
                                ws.Cells[row, 6].Value = "INSUMO";
                                ws.Cells[row, 7].Value = "VALOR";
                                ws.Cells[row, 8].Value = "ÚLTIMA FECHA DE ACTUALIZACIÓN";
                                ws.Cells[row, 9].Value = "USUARIO";

                                rg = ws.Cells[row, 2, row, 9];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;
                                rg.Style.WrapText = true;
                                //Border por celda
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                foreach (var item in datosRptInsumo)
                                {
                                    //if (item.RPMES == "1")
                                    //{
                                    DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                                    string nombreMes = formatoFecha.GetMonthName(Convert.ToInt32(item.mes));

                                    nroReg++;
                                    ws.Cells[row, 2].Value = item.anio.ToString().Trim();
                                    ws.Cells[row, 3].Value = item.mes.ToString().Trim();
                                    ws.Cells[row, 4].Value = item.periodo.ToString().Trim();
                                    ws.Cells[row, 5].Value = item.empresa.ToString().Trim();
                                    ws.Cells[row, 6].Value = item.insumo.ToString().Trim() + " " + item.anio.ToString().Trim() + "." + nombreMes.ToString().Trim();

                                    if (item.valor == 0) ws.Cells[row, 7].Value = string.Empty;
                                    else ws.Cells[row, 7].Value = item.valor;

                                    ws.Cells[row, 8].Value = item.fecha.ToString().Trim();
                                    ws.Cells[row, 9].Value = item.usuario;



                                    //Border por celda
                                    rg = ws.Cells[row, 2, row, 9];
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                    rg.Style.Font.Size = 10;

                                    ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";


                                    row++;
                                    //}

                                }

                                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                picture.From.Column = 1;
                                picture.From.Row = 1;
                                picture.To.Column = 2;
                                picture.To.Row = 2;
                                picture.SetSize(120, 60);

                                ws.Column(1).Width = 3.14;
                                ws.Column(2).Width = 8.71;
                                ws.Column(3).Width = 8.14;
                                ws.Column(4).Width = 9.87;
                                ws.Column(5).Width = 26.43;
                                ws.Column(6).Width = 77.43;
                                ws.Column(7).Width = 21.87;
                                ws.Column(8).Width = 30.00;
                                ws.Column(9).Width = 25.00;

                                row = row + 2;

                            }

                            ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("Energia_CM");
                            if (ws2 != null)
                            {
                                List<GmmDatCalculoDTO> datosRptEnergia = garantiaAppServicio.listarRptEnergia(anio, mes);

                                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                                ExcelPicture picture = ws2.Drawings.AddPicture("ff", img);
                                picture.From.Column = 1;
                                picture.From.Row = 1;
                                picture.To.Column = 2;
                                picture.To.Row = 2;
                                picture.SetSize(120, 60);

                                //TITULO
                                int row = 2; //fila donde inicia la data
                                ws2.Cells[row++, 4].Value = "ENERGÍA PREVISTA A RETIRAR Y COSTO MARGINAL" + anio + "-" + mes;
                                row++;
                                ExcelRange rg = ws2.Cells[2, 3, row, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;
                                row++;
                                //CABECERA DE TABLA

                                row++;

                                ws2.Cells[row, 2].Value = "anio";
                                ws2.Cells[row, 3].Value = "mes";
                                ws2.Cells[row, 4].Value = "empresa";
                                ws2.Cells[row, 5].Value = "barra";
                                ws2.Cells[row, 6].Value = "fecha";
                                ws2.Cells[row, 7].Value = "energia_t1";
                                ws2.Cells[row, 8].Value = "energia_t2";
                                ws2.Cells[row, 9].Value = "energia_t3";
                                ws2.Cells[row, 10].Value = "energia_t4";
                                ws2.Cells[row, 11].Value = "energia_t5";
                                ws2.Cells[row, 12].Value = "energia_t6";
                                ws2.Cells[row, 13].Value = "energia_t7";
                                ws2.Cells[row, 14].Value = "energia_t8";
                                ws2.Cells[row, 15].Value = "energia_t9";
                                ws2.Cells[row, 16].Value = "energia_t10";
                                ws2.Cells[row, 17].Value = "energia_t11";
                                ws2.Cells[row, 18].Value = "energia_t12";
                                ws2.Cells[row, 19].Value = "energia_t13";
                                ws2.Cells[row, 20].Value = "energia_t14";
                                ws2.Cells[row, 21].Value = "energia_t15";
                                ws2.Cells[row, 22].Value = "energia_t16";
                                ws2.Cells[row, 23].Value = "energia_t17";
                                ws2.Cells[row, 24].Value = "energia_t18";
                                ws2.Cells[row, 25].Value = "energia_t19";
                                ws2.Cells[row, 26].Value = "energia_t20";
                                ws2.Cells[row, 27].Value = "energia_t21";
                                ws2.Cells[row, 28].Value = "energia_t22";
                                ws2.Cells[row, 29].Value = "energia_t23";
                                ws2.Cells[row, 30].Value = "energia_t24";
                                ws2.Cells[row, 31].Value = "energia_t25";
                                ws2.Cells[row, 32].Value = "energia_t26";
                                ws2.Cells[row, 33].Value = "energia_t27";
                                ws2.Cells[row, 34].Value = "energia_t28";
                                ws2.Cells[row, 35].Value = "energia_t29";
                                ws2.Cells[row, 36].Value = "energia_t30";
                                ws2.Cells[row, 37].Value = "energia_t31";
                                ws2.Cells[row, 38].Value = "energia_t32";
                                ws2.Cells[row, 39].Value = "energia_t33";
                                ws2.Cells[row, 40].Value = "energia_t34";
                                ws2.Cells[row, 41].Value = "energia_t35";
                                ws2.Cells[row, 42].Value = "energia_t36";
                                ws2.Cells[row, 43].Value = "energia_t37";
                                ws2.Cells[row, 44].Value = "energia_t38";
                                ws2.Cells[row, 45].Value = "energia_t39";
                                ws2.Cells[row, 46].Value = "energia_t40";
                                ws2.Cells[row, 47].Value = "energia_t41";
                                ws2.Cells[row, 48].Value = "energia_t42";
                                ws2.Cells[row, 49].Value = "energia_t43";
                                ws2.Cells[row, 50].Value = "energia_t44";
                                ws2.Cells[row, 51].Value = "energia_t45";
                                ws2.Cells[row, 52].Value = "energia_t46";
                                ws2.Cells[row, 53].Value = "energia_t47";
                                ws2.Cells[row, 54].Value = "energia_t48";
                                ws2.Cells[row, 55].Value = "energia_t49";
                                ws2.Cells[row, 56].Value = "energia_t50";
                                ws2.Cells[row, 57].Value = "energia_t51";
                                ws2.Cells[row, 58].Value = "energia_t52";
                                ws2.Cells[row, 59].Value = "energia_t53";
                                ws2.Cells[row, 60].Value = "energia_t54";
                                ws2.Cells[row, 61].Value = "energia_t55";
                                ws2.Cells[row, 62].Value = "energia_t56";
                                ws2.Cells[row, 63].Value = "energia_t57";
                                ws2.Cells[row, 64].Value = "energia_t58";
                                ws2.Cells[row, 65].Value = "energia_t59";
                                ws2.Cells[row, 66].Value = "energia_t60";
                                ws2.Cells[row, 67].Value = "energia_t61";
                                ws2.Cells[row, 68].Value = "energia_t62";
                                ws2.Cells[row, 69].Value = "energia_t63";
                                ws2.Cells[row, 70].Value = "energia_t64";
                                ws2.Cells[row, 71].Value = "energia_t65";
                                ws2.Cells[row, 72].Value = "energia_t66";
                                ws2.Cells[row, 73].Value = "energia_t67";
                                ws2.Cells[row, 74].Value = "energia_t68";
                                ws2.Cells[row, 75].Value = "energia_t69";
                                ws2.Cells[row, 76].Value = "energia_t70";
                                ws2.Cells[row, 77].Value = "energia_t71";
                                ws2.Cells[row, 78].Value = "energia_t72";
                                ws2.Cells[row, 79].Value = "energia_t73";
                                ws2.Cells[row, 80].Value = "energia_t74";
                                ws2.Cells[row, 81].Value = "energia_t75";
                                ws2.Cells[row, 82].Value = "energia_t76";
                                ws2.Cells[row, 83].Value = "energia_t77";
                                ws2.Cells[row, 84].Value = "energia_t78";
                                ws2.Cells[row, 85].Value = "energia_t79";
                                ws2.Cells[row, 86].Value = "energia_t80";
                                ws2.Cells[row, 87].Value = "energia_t81";
                                ws2.Cells[row, 88].Value = "energia_t82";
                                ws2.Cells[row, 89].Value = "energia_t83";
                                ws2.Cells[row, 90].Value = "energia_t84";
                                ws2.Cells[row, 91].Value = "energia_t85";
                                ws2.Cells[row, 92].Value = "energia_t86";
                                ws2.Cells[row, 93].Value = "energia_t87";
                                ws2.Cells[row, 94].Value = "energia_t88";
                                ws2.Cells[row, 95].Value = "energia_t89";
                                ws2.Cells[row, 96].Value = "energia_t90";
                                ws2.Cells[row, 97].Value = "energia_t91";
                                ws2.Cells[row, 98].Value = "energia_t92";
                                ws2.Cells[row, 99].Value = "energia_t93";
                                ws2.Cells[row, 100].Value = "energia_t94";
                                ws2.Cells[row, 101].Value = "energia_t95";
                                ws2.Cells[row, 102].Value = "energia_t96";
                                ws2.Cells[row, 103].Value = "cm_t1";
                                ws2.Cells[row, 104].Value = "cm_t2";
                                ws2.Cells[row, 105].Value = "cm_t3";
                                ws2.Cells[row, 106].Value = "cm_t4";
                                ws2.Cells[row, 107].Value = "cm_t5";
                                ws2.Cells[row, 108].Value = "cm_t6";
                                ws2.Cells[row, 109].Value = "cm_t7";
                                ws2.Cells[row, 110].Value = "cm_t8";
                                ws2.Cells[row, 111].Value = "cm_t9";
                                ws2.Cells[row, 112].Value = "cm_t10";
                                ws2.Cells[row, 113].Value = "cm_t11";
                                ws2.Cells[row, 114].Value = "cm_t12";
                                ws2.Cells[row, 115].Value = "cm_t13";
                                ws2.Cells[row, 116].Value = "cm_t14";
                                ws2.Cells[row, 117].Value = "cm_t15";
                                ws2.Cells[row, 118].Value = "cm_t16";
                                ws2.Cells[row, 119].Value = "cm_t17";
                                ws2.Cells[row, 120].Value = "cm_t18";
                                ws2.Cells[row, 121].Value = "cm_t19";
                                ws2.Cells[row, 122].Value = "cm_t20";
                                ws2.Cells[row, 123].Value = "cm_t21";
                                ws2.Cells[row, 124].Value = "cm_t22";
                                ws2.Cells[row, 125].Value = "cm_t23";
                                ws2.Cells[row, 126].Value = "cm_t24";
                                ws2.Cells[row, 127].Value = "cm_t25";
                                ws2.Cells[row, 128].Value = "cm_t26";
                                ws2.Cells[row, 129].Value = "cm_t27";
                                ws2.Cells[row, 130].Value = "cm_t28";
                                ws2.Cells[row, 131].Value = "cm_t29";
                                ws2.Cells[row, 132].Value = "cm_t30";
                                ws2.Cells[row, 133].Value = "cm_t31";
                                ws2.Cells[row, 134].Value = "cm_t32";
                                ws2.Cells[row, 135].Value = "cm_t33";
                                ws2.Cells[row, 136].Value = "cm_t34";
                                ws2.Cells[row, 137].Value = "cm_t35";
                                ws2.Cells[row, 138].Value = "cm_t36";
                                ws2.Cells[row, 139].Value = "cm_t37";
                                ws2.Cells[row, 140].Value = "cm_t38";
                                ws2.Cells[row, 141].Value = "cm_t39";
                                ws2.Cells[row, 142].Value = "cm_t40";
                                ws2.Cells[row, 143].Value = "cm_t41";
                                ws2.Cells[row, 144].Value = "cm_t42";
                                ws2.Cells[row, 145].Value = "cm_t43";
                                ws2.Cells[row, 146].Value = "cm_t44";
                                ws2.Cells[row, 147].Value = "cm_t45";
                                ws2.Cells[row, 148].Value = "cm_t46";
                                ws2.Cells[row, 149].Value = "cm_t47";
                                ws2.Cells[row, 150].Value = "cm_t48";
                                ws2.Cells[row, 151].Value = "cm_t49";
                                ws2.Cells[row, 152].Value = "cm_t50";
                                ws2.Cells[row, 153].Value = "cm_t51";
                                ws2.Cells[row, 154].Value = "cm_t52";
                                ws2.Cells[row, 155].Value = "cm_t53";
                                ws2.Cells[row, 156].Value = "cm_t54";
                                ws2.Cells[row, 157].Value = "cm_t55";
                                ws2.Cells[row, 158].Value = "cm_t56";
                                ws2.Cells[row, 159].Value = "cm_t57";
                                ws2.Cells[row, 160].Value = "cm_t58";
                                ws2.Cells[row, 161].Value = "cm_t59";
                                ws2.Cells[row, 162].Value = "cm_t60";
                                ws2.Cells[row, 163].Value = "cm_t61";
                                ws2.Cells[row, 164].Value = "cm_t62";
                                ws2.Cells[row, 165].Value = "cm_t63";
                                ws2.Cells[row, 166].Value = "cm_t64";
                                ws2.Cells[row, 167].Value = "cm_t65";
                                ws2.Cells[row, 168].Value = "cm_t66";
                                ws2.Cells[row, 169].Value = "cm_t67";
                                ws2.Cells[row, 170].Value = "cm_t68";
                                ws2.Cells[row, 171].Value = "cm_t69";
                                ws2.Cells[row, 172].Value = "cm_t70";
                                ws2.Cells[row, 173].Value = "cm_t71";
                                ws2.Cells[row, 174].Value = "cm_t72";
                                ws2.Cells[row, 175].Value = "cm_t73";
                                ws2.Cells[row, 176].Value = "cm_t74";
                                ws2.Cells[row, 177].Value = "cm_t75";
                                ws2.Cells[row, 178].Value = "cm_t76";
                                ws2.Cells[row, 179].Value = "cm_t77";
                                ws2.Cells[row, 180].Value = "cm_t78";
                                ws2.Cells[row, 181].Value = "cm_t79";
                                ws2.Cells[row, 182].Value = "cm_t80";
                                ws2.Cells[row, 183].Value = "cm_t81";
                                ws2.Cells[row, 184].Value = "cm_t82";
                                ws2.Cells[row, 185].Value = "cm_t83";
                                ws2.Cells[row, 186].Value = "cm_t84";
                                ws2.Cells[row, 187].Value = "cm_t85";
                                ws2.Cells[row, 188].Value = "cm_t86";
                                ws2.Cells[row, 189].Value = "cm_t87";
                                ws2.Cells[row, 190].Value = "cm_t88";
                                ws2.Cells[row, 191].Value = "cm_t89";
                                ws2.Cells[row, 192].Value = "cm_t90";
                                ws2.Cells[row, 193].Value = "cm_t91";
                                ws2.Cells[row, 194].Value = "cm_t92";
                                ws2.Cells[row, 195].Value = "cm_t93";
                                ws2.Cells[row, 196].Value = "cm_t94";
                                ws2.Cells[row, 197].Value = "cm_t95";
                                ws2.Cells[row, 198].Value = "cm_t96";

                                rg = ws2.Cells[row, 2, row, 198];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 9;
                                rg.Style.Font.Bold = true;
                                rg.Style.WrapText = true;
                                //Border por celda
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                foreach (var item in datosRptEnergia)
                                {
                                    ws2.Cells[row, 2].Value = item.anio.ToString().Trim();
                                    ws2.Cells[row, 3].Value = item.mes.ToString().Trim();
                                    ws2.Cells[row, 4].Value = item.empresa.ToString().Trim();
                                    ws2.Cells[row, 5].Value = item.barra.ToString().Trim();
                                    ws2.Cells[row, 6].Value = item.fecha.ToString().Trim();

                                    ws2.Cells[row, 7].Value = item.energia_t1;
                                    ws2.Cells[row, 8].Value = item.energia_t2;
                                    ws2.Cells[row, 9].Value = item.energia_t3;
                                    ws2.Cells[row, 10].Value = item.energia_t4;
                                    ws2.Cells[row, 11].Value = item.energia_t5;
                                    ws2.Cells[row, 12].Value = item.energia_t6;
                                    ws2.Cells[row, 13].Value = item.energia_t7;
                                    ws2.Cells[row, 14].Value = item.energia_t8;
                                    ws2.Cells[row, 15].Value = item.energia_t9;
                                    ws2.Cells[row, 16].Value = item.energia_t10;
                                    ws2.Cells[row, 17].Value = item.energia_t11;
                                    ws2.Cells[row, 18].Value = item.energia_t12;
                                    ws2.Cells[row, 19].Value = item.energia_t13;
                                    ws2.Cells[row, 20].Value = item.energia_t14;
                                    ws2.Cells[row, 21].Value = item.energia_t15;
                                    ws2.Cells[row, 22].Value = item.energia_t16;
                                    ws2.Cells[row, 23].Value = item.energia_t17;
                                    ws2.Cells[row, 24].Value = item.energia_t18;
                                    ws2.Cells[row, 25].Value = item.energia_t19;
                                    ws2.Cells[row, 26].Value = item.energia_t20;
                                    ws2.Cells[row, 27].Value = item.energia_t21;
                                    ws2.Cells[row, 28].Value = item.energia_t22;
                                    ws2.Cells[row, 29].Value = item.energia_t23;
                                    ws2.Cells[row, 30].Value = item.energia_t24;
                                    ws2.Cells[row, 31].Value = item.energia_t25;
                                    ws2.Cells[row, 32].Value = item.energia_t26;
                                    ws2.Cells[row, 33].Value = item.energia_t27;
                                    ws2.Cells[row, 34].Value = item.energia_t28;
                                    ws2.Cells[row, 35].Value = item.energia_t29;
                                    ws2.Cells[row, 36].Value = item.energia_t30;
                                    ws2.Cells[row, 37].Value = item.energia_t31;
                                    ws2.Cells[row, 38].Value = item.energia_t32;
                                    ws2.Cells[row, 39].Value = item.energia_t33;
                                    ws2.Cells[row, 40].Value = item.energia_t34;
                                    ws2.Cells[row, 41].Value = item.energia_t35;
                                    ws2.Cells[row, 42].Value = item.energia_t36;
                                    ws2.Cells[row, 43].Value = item.energia_t37;
                                    ws2.Cells[row, 44].Value = item.energia_t38;
                                    ws2.Cells[row, 45].Value = item.energia_t39;
                                    ws2.Cells[row, 46].Value = item.energia_t40;
                                    ws2.Cells[row, 47].Value = item.energia_t41;
                                    ws2.Cells[row, 48].Value = item.energia_t42;
                                    ws2.Cells[row, 49].Value = item.energia_t43;
                                    ws2.Cells[row, 50].Value = item.energia_t44;
                                    ws2.Cells[row, 51].Value = item.energia_t45;
                                    ws2.Cells[row, 52].Value = item.energia_t46;
                                    ws2.Cells[row, 53].Value = item.energia_t47;
                                    ws2.Cells[row, 54].Value = item.energia_t48;
                                    ws2.Cells[row, 55].Value = item.energia_t49;
                                    ws2.Cells[row, 56].Value = item.energia_t50;
                                    ws2.Cells[row, 57].Value = item.energia_t51;
                                    ws2.Cells[row, 58].Value = item.energia_t52;
                                    ws2.Cells[row, 59].Value = item.energia_t53;
                                    ws2.Cells[row, 60].Value = item.energia_t54;
                                    ws2.Cells[row, 61].Value = item.energia_t55;
                                    ws2.Cells[row, 62].Value = item.energia_t56;
                                    ws2.Cells[row, 63].Value = item.energia_t57;
                                    ws2.Cells[row, 64].Value = item.energia_t58;
                                    ws2.Cells[row, 65].Value = item.energia_t59;
                                    ws2.Cells[row, 66].Value = item.energia_t60;
                                    ws2.Cells[row, 67].Value = item.energia_t61;
                                    ws2.Cells[row, 68].Value = item.energia_t62;
                                    ws2.Cells[row, 69].Value = item.energia_t63;
                                    ws2.Cells[row, 70].Value = item.energia_t64;
                                    ws2.Cells[row, 71].Value = item.energia_t65;
                                    ws2.Cells[row, 72].Value = item.energia_t66;
                                    ws2.Cells[row, 73].Value = item.energia_t67;
                                    ws2.Cells[row, 74].Value = item.energia_t68;
                                    ws2.Cells[row, 75].Value = item.energia_t69;
                                    ws2.Cells[row, 76].Value = item.energia_t70;
                                    ws2.Cells[row, 77].Value = item.energia_t71;
                                    ws2.Cells[row, 78].Value = item.energia_t72;
                                    ws2.Cells[row, 79].Value = item.energia_t73;
                                    ws2.Cells[row, 80].Value = item.energia_t74;
                                    ws2.Cells[row, 81].Value = item.energia_t75;
                                    ws2.Cells[row, 82].Value = item.energia_t76;
                                    ws2.Cells[row, 83].Value = item.energia_t77;
                                    ws2.Cells[row, 84].Value = item.energia_t78;
                                    ws2.Cells[row, 85].Value = item.energia_t79;
                                    ws2.Cells[row, 86].Value = item.energia_t80;
                                    ws2.Cells[row, 87].Value = item.energia_t81;
                                    ws2.Cells[row, 88].Value = item.energia_t82;
                                    ws2.Cells[row, 89].Value = item.energia_t83;
                                    ws2.Cells[row, 90].Value = item.energia_t84;
                                    ws2.Cells[row, 91].Value = item.energia_t85;
                                    ws2.Cells[row, 92].Value = item.energia_t86;
                                    ws2.Cells[row, 93].Value = item.energia_t87;
                                    ws2.Cells[row, 94].Value = item.energia_t88;
                                    ws2.Cells[row, 95].Value = item.energia_t89;
                                    ws2.Cells[row, 96].Value = item.energia_t90;
                                    ws2.Cells[row, 97].Value = item.energia_t91;
                                    ws2.Cells[row, 98].Value = item.energia_t92;
                                    ws2.Cells[row, 99].Value = item.energia_t93;
                                    ws2.Cells[row, 100].Value = item.energia_t94;
                                    ws2.Cells[row, 101].Value = item.energia_t95;
                                    ws2.Cells[row, 102].Value = item.energia_t96;
                                    ws2.Cells[row, 103].Value = item.cm_t1;
                                    ws2.Cells[row, 104].Value = item.cm_t2;
                                    ws2.Cells[row, 105].Value = item.cm_t3;
                                    ws2.Cells[row, 106].Value = item.cm_t4;
                                    ws2.Cells[row, 107].Value = item.cm_t5;
                                    ws2.Cells[row, 108].Value = item.cm_t6;
                                    ws2.Cells[row, 109].Value = item.cm_t7;
                                    ws2.Cells[row, 110].Value = item.cm_t8;
                                    ws2.Cells[row, 111].Value = item.cm_t9;
                                    ws2.Cells[row, 112].Value = item.cm_t10;
                                    ws2.Cells[row, 113].Value = item.cm_t11;
                                    ws2.Cells[row, 114].Value = item.cm_t12;
                                    ws2.Cells[row, 115].Value = item.cm_t13;
                                    ws2.Cells[row, 116].Value = item.cm_t14;
                                    ws2.Cells[row, 117].Value = item.cm_t15;
                                    ws2.Cells[row, 118].Value = item.cm_t16;
                                    ws2.Cells[row, 119].Value = item.cm_t17;
                                    ws2.Cells[row, 120].Value = item.cm_t18;
                                    ws2.Cells[row, 121].Value = item.cm_t19;
                                    ws2.Cells[row, 122].Value = item.cm_t20;
                                    ws2.Cells[row, 123].Value = item.cm_t21;
                                    ws2.Cells[row, 124].Value = item.cm_t22;
                                    ws2.Cells[row, 125].Value = item.cm_t23;
                                    ws2.Cells[row, 126].Value = item.cm_t24;
                                    ws2.Cells[row, 127].Value = item.cm_t25;
                                    ws2.Cells[row, 128].Value = item.cm_t26;
                                    ws2.Cells[row, 129].Value = item.cm_t27;
                                    ws2.Cells[row, 130].Value = item.cm_t28;
                                    ws2.Cells[row, 131].Value = item.cm_t29;
                                    ws2.Cells[row, 132].Value = item.cm_t30;
                                    ws2.Cells[row, 133].Value = item.cm_t31;
                                    ws2.Cells[row, 134].Value = item.cm_t32;
                                    ws2.Cells[row, 135].Value = item.cm_t33;
                                    ws2.Cells[row, 136].Value = item.cm_t34;
                                    ws2.Cells[row, 137].Value = item.cm_t35;
                                    ws2.Cells[row, 138].Value = item.cm_t36;
                                    ws2.Cells[row, 139].Value = item.cm_t37;
                                    ws2.Cells[row, 140].Value = item.cm_t38;
                                    ws2.Cells[row, 141].Value = item.cm_t39;
                                    ws2.Cells[row, 142].Value = item.cm_t40;
                                    ws2.Cells[row, 143].Value = item.cm_t41;
                                    ws2.Cells[row, 144].Value = item.cm_t42;
                                    ws2.Cells[row, 145].Value = item.cm_t43;
                                    ws2.Cells[row, 146].Value = item.cm_t44;
                                    ws2.Cells[row, 147].Value = item.cm_t45;
                                    ws2.Cells[row, 148].Value = item.cm_t46;
                                    ws2.Cells[row, 149].Value = item.cm_t47;
                                    ws2.Cells[row, 150].Value = item.cm_t48;
                                    ws2.Cells[row, 151].Value = item.cm_t49;
                                    ws2.Cells[row, 152].Value = item.cm_t50;
                                    ws2.Cells[row, 153].Value = item.cm_t51;
                                    ws2.Cells[row, 154].Value = item.cm_t52;
                                    ws2.Cells[row, 155].Value = item.cm_t53;
                                    ws2.Cells[row, 156].Value = item.cm_t54;
                                    ws2.Cells[row, 157].Value = item.cm_t55;
                                    ws2.Cells[row, 158].Value = item.cm_t56;
                                    ws2.Cells[row, 159].Value = item.cm_t57;
                                    ws2.Cells[row, 160].Value = item.cm_t58;
                                    ws2.Cells[row, 161].Value = item.cm_t59;
                                    ws2.Cells[row, 162].Value = item.cm_t60;
                                    ws2.Cells[row, 163].Value = item.cm_t61;
                                    ws2.Cells[row, 164].Value = item.cm_t62;
                                    ws2.Cells[row, 165].Value = item.cm_t63;
                                    ws2.Cells[row, 166].Value = item.cm_t64;
                                    ws2.Cells[row, 167].Value = item.cm_t65;
                                    ws2.Cells[row, 168].Value = item.cm_t66;
                                    ws2.Cells[row, 169].Value = item.cm_t67;
                                    ws2.Cells[row, 170].Value = item.cm_t68;
                                    ws2.Cells[row, 171].Value = item.cm_t69;
                                    ws2.Cells[row, 172].Value = item.cm_t70;
                                    ws2.Cells[row, 173].Value = item.cm_t71;
                                    ws2.Cells[row, 174].Value = item.cm_t72;
                                    ws2.Cells[row, 175].Value = item.cm_t73;
                                    ws2.Cells[row, 176].Value = item.cm_t74;
                                    ws2.Cells[row, 177].Value = item.cm_t75;
                                    ws2.Cells[row, 178].Value = item.cm_t76;
                                    ws2.Cells[row, 179].Value = item.cm_t77;
                                    ws2.Cells[row, 180].Value = item.cm_t78;
                                    ws2.Cells[row, 181].Value = item.cm_t79;
                                    ws2.Cells[row, 182].Value = item.cm_t80;
                                    ws2.Cells[row, 183].Value = item.cm_t81;
                                    ws2.Cells[row, 184].Value = item.cm_t82;
                                    ws2.Cells[row, 185].Value = item.cm_t83;
                                    ws2.Cells[row, 186].Value = item.cm_t84;
                                    ws2.Cells[row, 187].Value = item.cm_t85;
                                    ws2.Cells[row, 188].Value = item.cm_t86;
                                    ws2.Cells[row, 189].Value = item.cm_t87;
                                    ws2.Cells[row, 190].Value = item.cm_t88;
                                    ws2.Cells[row, 191].Value = item.cm_t89;
                                    ws2.Cells[row, 192].Value = item.cm_t90;
                                    ws2.Cells[row, 193].Value = item.cm_t91;
                                    ws2.Cells[row, 194].Value = item.cm_t92;
                                    ws2.Cells[row, 195].Value = item.cm_t93;
                                    ws2.Cells[row, 196].Value = item.cm_t94;
                                    ws2.Cells[row, 197].Value = item.cm_t95;
                                    ws2.Cells[row, 198].Value = item.cm_t96;

                                    //Border por celda
                                    rg = ws2.Cells[row, 2, row, 198];
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                    rg.Style.Font.Size = 10;

                                    ExcelRange rgval = ws2.Cells[row, 7, row, 198];
                                    rgval.Style.Numberformat.Format = "0.00";

                                    row++;

                                }

                            }

                            xlPackage.Save();
                        }
                        indicador = "1";

                        break;
                    #endregion
                    #region Lista de descargas disponibles Excel Monto de Garantias para respaldar obligaciones de pago en el Mercado Mayorista de Electricidad (Soles):
                    case "rpt1":
                        List<GmmDatCalculoDTO> datosRPT1 = garantiaAppServicio.listarRpt1(anio, mes);
                        nroReg = 0;

                        path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                        newFile = new FileInfo(path + Funcion.NombreRpt1);

                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(path + Funcion.NombreRpt1);
                        }

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                            if (ws != null)
                            {
                                //TITULO
                                int row = 4; //fila donde inicia la data
                                ws.Cells[row, 3].Value = "Monto de Garantias para cobertura de obligaciones de pago en el MME (Soles)";

                                ExcelRange rg = ws.Cells[2, 3, row, 3];
                                rg.Style.Font.Size = 16;

                                row = row + 2;

                                //CABECERA DE TABLA
                                ws.Cells[row, 2].Value = "MES DE CÁLCULO";
                                ws.Cells[row, 3].Value = "PARTICIPANTE";
                                ws.Cells[row, 4].Value = "POR ENERGÍA ACTIVA";
                                ws.Cells[row, 5].Value = "POR CAPACIDAD";
                                ws.Cells[row, 6].Value = "POR PEAJE DE CONEXIÓN";
                                ws.Cells[row, 7].Value = "POR SERVICIOS COMPLEMENTARIOS";
                                ws.Cells[row, 8].Value = "POR INFLEXIBILIDADES OPERATIVAS";
                                ws.Cells[row, 9].Value = "POR EXCESO DE CONSUMO DE ENERGÍA REACTIVA";
                                ws.Cells[row, 10].Value = "MONTO TOTAL GARANTÍA S/";
                                ws.Cells[row, 11].Value = "MONTO DE GARANTÍA DEPOSITADO";
                                ws.Cells[row, 12].Value = "FACTOR DE ACTUALIZACIÓN";
                                ws.Cells[row, 13].Value = "OBSERVACIÓN";

                                rg = ws.Cells[row, 2, row, 13];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;
                                rg.Style.WrapText = true;
                                //Border por celda
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                foreach (var item in datosRPT1)
                                {
                                    //if (item.RPMES == "1")
                                    //{
                                    nroReg++;
                                    ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                    ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();//item.EMPRESA.ToString().Trim();
                                    ws.Cells[row, 4].Value = item.RENERGIA;//"POR ENERGÍA ACTIVA";
                                    ws.Cells[row, 5].Value = item.RCAPACIDAD;//"POR CAPACIDAD";
                                    ws.Cells[row, 6].Value = item.RPEAJE;//"POR PEAJE DE CONEXIÓN";
                                    ws.Cells[row, 7].Value = item.RSCOMPLE;//"POR SERVICIOS COMPLEMENTARIOS";
                                    ws.Cells[row, 8].Value = item.RINFLEXOP;//"POR INFLEXIBILIDADES OPERATIVAS";
                                    ws.Cells[row, 9].Value = item.REREACTIVA;//"POR EXCESO DE CONSUMO DE ENERGÍA REACTIVA";
                                    ws.Cells[row, 10].Value = item.TOTALGARANTIA;// "MONTO TOTAL GARANTÍA S/";
                                    ws.Cells[row, 11].Value = item.GARANTIADEP;// "MONTO DE GARANTÍA DEPOSITADO ";
                                    if (item.COMENTARIO.ToString().Trim().Equals("PRIMER CALCULO"))
                                        ws.Cells[row, 12].Value = " -- ";
                                    else
                                        ws.Cells[row, 12].Value = item.FACTOR;// "FACTOR DE ACTUALIZACIÓN";
                                    decimal calculoPorcentaje = 20 * item.TOTALGARANTIA / 100;

                                    if (item.GARANTIADEP > 0 && item.TOTALGARANTIA / item.GARANTIADEP > calculoPorcentaje)
                                    {
                                        if (item.COMENTARIO.ToString().Trim() == "--")
                                            item.COMENTARIO = "Actualizar el monto de garantía";
                                        else
                                            item.COMENTARIO = item.COMENTARIO + " - Actualizar el monto de garantía";
                                    }
                                    ws.Cells[row, 13].Value = item.COMENTARIO.ToString().Trim();// "OBSERVACIÓN";
                                    //item.EMPRESA.ToString().Trim();
                                    //Border por celda
                                    rg = ws.Cells[row, 2, row, 13];
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                    rg.Style.Font.Size = 10;

                                    ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 8].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 9].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 10].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 11].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 12].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 13].Style.Numberformat.Format = "#,##0.00";

                                    row++;
                                    //}

                                }

                                row++;

                                if (nroReg == 0)
                                    ws.Cells[row, 2].Value = "Sin Registros";

                                nroReg = 0;
                                //Insertar el logo
                                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                picture.From.Column = 1;
                                picture.From.Row = 1;
                                picture.To.Column = 2;
                                picture.To.Row = 2;
                                picture.SetSize(120, 60);

                                ws.Column(1).Width = 3.14;
                                ws.Column(2).Width = 20.71;
                                ws.Column(3).Width = 102.14;
                                ws.Column(4).Width = 16.57;
                                ws.Column(5).Width = 14.86;
                                ws.Column(6).Width = 16.43;
                                ws.Column(7).Width = 24.57;
                                ws.Column(8).Width = 27.43;
                                ws.Column(9).Width = 32;
                                ws.Column(10).Width = 17.71;
                                ws.Column(11).Width = 26;
                                ws.Column(12).Width = 19.2;
                                ws.Column(13).Width = 49.43;

                            }
                            xlPackage.Save();
                        }
                        indicador = "1";

                        break;
                    #endregion
                    #region Montos de Garantías por energía activa:
                    case "rpt2":
                        List<GmmDatCalculoDTO> datosRPT2 = garantiaAppServicio.listarRpt2(anio, mes);

                        nroReg = 0;

                        path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                        newFile = new FileInfo(path + Funcion.NombreRpt2);

                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(path + Funcion.NombreRpt2);
                        }

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                            if (ws != null)
                            {
                                //TITULO
                                int row = 2; //fila donde inicia la data
                                ws.Cells[row++, 3].Value = "CUADRO N° 1";
                                ws.Cells[row++, 3].Value = "Montos de Garantías por Energía Activa";
                                row++;
                                ws.Cells[row++, 2].Value = "Primer cálculo";
                                ExcelRange rg = ws.Cells[2, 3, row, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;

                                //CABECERA DE TABLA
                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";
                                ws.Cells[row, 4].Value = "Montos de Retiros (S/)";
                                ws.Cells[row, 5].Value = "Montos de Entregas (S/)";
                                ws.Cells[row, 6].Value = "Montos por Energia Retiros - Entregas(S /)";

                                rg = ws.Cells[row, 2, row, 6];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                foreach (var item in datosRPT2)
                                {
                                    if (item.RPMES == "1")
                                    {
                                        nroReg++;
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                        ws.Cells[row, 4].Value = item.RRETIRO;
                                        ws.Cells[row, 5].Value = item.RENTREGA;
                                        ws.Cells[row, 6].Value = item.RENERGIA;

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 6];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;

                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";

                                        row++;
                                    }

                                }

                                row++;

                                if (nroReg == 0)
                                    ws.Cells[row, 2].Value = "Sin Registros";

                                nroReg = 0;
                                //Insertar el logo
                                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                picture.From.Column = 1;
                                picture.From.Row = 1;
                                picture.To.Column = 2;
                                picture.To.Row = 2;
                                picture.SetSize(120, 60);

                                /*2do logo*/
                                /*
                                picture.From.Column = 0;
                                picture.From.Row = row;
                                picture.To.Column = 1;
                                picture.To.Row = row+1;
                                picture.SetSize(120, 60);
                                */

                                row = row + 1;
                                rg = ws.Cells[row, 3, row + 1, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;
                                ws.Cells[row++, 3].Value = "CUADRO N° 1";
                                ws.Cells[row++, 3].Value = "Montos de Garantías por Energía Activa";
                                row = row + 1;

                                //2DA-CABECERA DE TABLA
                                ws.Cells[row++, 2].Value = "A partir del segundo mes";
                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";
                                ws.Cells[row, 4].Value = "Valorización LVTEA (" + mesCadenaAnt + "-" + anio + ")";
                                ws.Cells[row, 5].Value = "Valorizaciones Diarias 10 dias (" + mesCadenaAct + "-" + anio + ")";
                                ws.Cells[row, 6].Value = "Valorización 11-m dias (" + mesCadenaAct + "-" + anio + ")";
                                ws.Cells[row, 7].Value = "Valorización proyectada (" + mesCadenaSig + "-" + anio + ")";
                                ws.Cells[row, 8].Value = "Montos por Energia Retiros - Entregas(S /)";

                                rg = ws.Cells[row, 2, row, 8];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg = ws.Cells[row, 2, row + 1, 8];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                ws.Cells[row, 4].Value = "VME m-1";
                                ws.Cells[row, 5].Value = "VMEm";
                                ws.Cells[row, 7].Value = "VEMm+1";
                                ws.Cells[row, 5, row, 6].Merge = true;

                                rg = ws.Cells[row, 4, row, 8];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                row++;
                                foreach (var item in datosRPT2)
                                {
                                    if (item.RPMES == "0")
                                    {
                                        nroReg++;
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                        ws.Cells[row, 4].Value = item.RLVTEA;

                                        ws.Cells[row, 5].Value = item.RVD10;
                                        ws.Cells[row, 6].Value = item.RVD11;
                                        ws.Cells[row, 7].Value = item.RVPROY;
                                        ws.Cells[row, 8].Value = item.RENERGIA;

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 8];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;

                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 8].Style.Numberformat.Format = "#,##0.00";

                                        row++;
                                    }

                                }

                                row++;
                                if (nroReg == 0)
                                    ws.Cells[row, 2].Value = "Sin Registros";

                                ws.Column(1).Width = 3.71;
                                ws.Column(2).Width = 23.29;
                                ws.Column(3).Width = 40.57;
                                ws.Column(4).Width = 24.71;
                                ws.Column(5).Width = 20.14;
                                ws.Column(6).Width = 23.71;
                                ws.Column(7).Width = 20;
                                ws.Column(8).Width = 24.43;
                                ws.Column(4).Style.WrapText = true;
                                ws.Column(5).Style.WrapText = true;
                                ws.Column(6).Style.WrapText = true;
                                ws.Column(7).Style.WrapText = true;
                                ws.Column(8).Style.WrapText = true;

                            }
                            xlPackage.Save();
                        }
                        indicador = "1";


                        break;
                    #endregion
                    #region Montos de Garantías por capacidad y peaje SPT - SGT:
                    case "rpt3":

                        List<GmmDatCalculoDTO> datosRPT3 = garantiaAppServicio.listarRpt3(anio, mes);
                        int cont = 0;
                        path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                        newFile = new FileInfo(path + Funcion.NombreRpt3);
                        nroReg = 0;

                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(path + Funcion.NombreRpt3);
                        }

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                            if (ws != null)
                            {
                                //TITULO
                                int row = 2; //fila donde inicia la data
                                ExcelRange rg = ws.Cells[2, 3, row, 4];


                                if (datosRPT3.Count() == 0)
                                {
                                    ws.Cells[row++, 3].Value = "CUADRO N° 2";
                                    ws.Cells[row++, 3].Value = "Montos de Garantías por Capacidad y Peaje SPT - SGT";
                                    row++;
                                    ws.Cells[row++, 2].Value = "Primer cálculo";
                                    rg = ws.Cells[row - 4, 3, row - 2, 4];
                                    rg.Style.Font.Size = 16;
                                    rg.Style.Font.Bold = true;

                                    //CABECERA DE TABLA
                                    ws.Cells[row, 2].Value = "Mes de Cálculo";
                                    ws.Cells[row, 3].Value = "Participante (*)";
                                    ws.Cells[row, 4].Value = "Monto por capacidad(S/)";
                                    ws.Cells[row, 5].Value = "Monto por Peaje (S/)";

                                    //ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                                    //ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                                    //ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                                    //ws.Column(7).Style.Numberformat.Format = "#,##0.00";

                                    rg = ws.Cells[row, 2, row, 5];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                    rg.Style.Font.Color.SetColor(Color.White);
                                    rg.Style.Font.Size = 10;
                                    rg.Style.Font.Bold = true;

                                    //Border por celda
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                    row++;

                                    //nroReg++;

                                    //ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                    //ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                    //ws.Cells[row, 4].Value = item.RCAPACIDAD.ToString().Trim();
                                    //ws.Cells[row, 5].Value = item.RPEAJE;

                                    //Formateo de celdas
                                    ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";

                                    //Border por celda
                                    rg = ws.Cells[row, 2, row, 5];
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                    rg.Style.Font.Size = 10;
                                    row++;
                                    row++;

                                    ws.Cells[row, 3].Value = "Mes 1";
                                    ws.Cells[row, 4].Value = "Mes 2";
                                    ws.Cells[row, 5].Value = "Mes 3";

                                    rg = ws.Cells[row, 3, row, 5];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                    rg.Style.Font.Color.SetColor(Color.White);
                                    rg.Style.Font.Size = 10;
                                    rg.Style.Font.Bold = true;

                                    //Border por celda
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                    row++;

                                    ws.Cells[row, 2].Value = "Demanda Coincidente (kW)";
                                    rg = ws.Cells[row, 2, row, 2];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                    rg.Style.Font.Color.SetColor(Color.White);
                                    rg.Style.Font.Size = 10;
                                    rg.Style.Font.Bold = true;

                                    //ws.Cells[row, 3].Value = item.RDMES1;
                                    //ws.Cells[row, 4].Value = item.RDMES2;
                                    //ws.Cells[row, 5].Value = item.RDMES3;

                                    //Formateo de celdas
                                    ws.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                    ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";

                                    //Border por celda
                                    rg = ws.Cells[row, 2, row, 5];
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                    rg.Style.Font.Size = 10;
                                    row++;
                                    row++;

                                    ////ws.Cells[row++, 3].Value = item.RPREPOT;
                                    //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                    ////ws.Cells[row++, 3].Value = item.RPEAJEU;
                                    //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                    ////ws.Cells[row++, 3].Value = item.RMARGENR;
                                    //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                    ////ws.Cells[row++, 3].Value = item.RPFIRME;
                                    //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                    ////ws.Cells[row++, 3].Value = item.RPFIRME1MR;
                                    //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";

                                    //Border por celda
                                    //rg = ws.Cells[row - 5, 2, row - 1, 3];
                                    //rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    //rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    //rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    //rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    //rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    //rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    //rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    //rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                    //rg.Style.Font.Size = 10;
                                    //row++;

                                    ws.Cells[row, 2].Value = "Precio Potencia (**)";
                                    ws.Cells[row + 1, 2].Value = "Peaje Unitario (**)";
                                    ws.Cells[row + 2, 2].Value = "Margen de reserva";
                                    ws.Cells[row + 3, 2].Value = "PFirmeRemun (KW)";
                                    ws.Cells[row + 4, 2].Value = "PFirmeRemun/(1+mr) (KW)";


                                    rg = ws.Cells[row, 2, row + 4, 2];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                    rg.Style.Font.Color.SetColor(Color.White);
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                    rg.Style.Font.Size = 10;
                                    rg.Style.Font.Bold = true;

                                    row = row + 5;

                                }

                                foreach (var item in datosRPT3)
                                {

                                    if (item.RPMES == "1")
                                    {

                                        ws.Cells[row++, 3].Value = "CUADRO N° 2";
                                        ws.Cells[row++, 3].Value = "Montos de Garantías por Capacidad y Peaje SPT - SGT";
                                        row++;
                                        ws.Cells[row++, 2].Value = "Primer cálculo";
                                        rg = ws.Cells[row - 4, 3, row - 2, 4];
                                        rg.Style.Font.Size = 16;
                                        rg.Style.Font.Bold = true;

                                        //CABECERA DE TABLA
                                        ws.Cells[row, 2].Value = "Mes de Cálculo";
                                        ws.Cells[row, 3].Value = "Participante (*)";
                                        ws.Cells[row, 4].Value = "Monto por capacidad(S/)";
                                        ws.Cells[row, 5].Value = "Monto por Peaje (S/)";

                                        //ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                                        //ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                                        //ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                                        //ws.Column(7).Style.Numberformat.Format = "#,##0.00";

                                        rg = ws.Cells[row, 2, row, 5];
                                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                        rg.Style.Font.Color.SetColor(Color.White);
                                        rg.Style.Font.Size = 10;
                                        rg.Style.Font.Bold = true;

                                        //Border por celda
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                        row++;

                                        nroReg++;

                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                        ws.Cells[row, 4].Value = item.RCAPACIDAD; //.ToString().Trim();
                                        ws.Cells[row, 5].Value = item.RPEAJE;

                                        //Formateo de celdas
                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 5];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                        row++;
                                        row++;

                                        ws.Cells[row, 3].Value = "Mes 1";
                                        ws.Cells[row, 4].Value = "Mes 2";
                                        ws.Cells[row, 5].Value = "Mes 3";

                                        rg = ws.Cells[row, 3, row, 5];
                                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                        rg.Style.Font.Color.SetColor(Color.White);
                                        rg.Style.Font.Size = 10;
                                        rg.Style.Font.Bold = true;

                                        //Border por celda
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                        row++;

                                        ws.Cells[row, 2].Value = "Demanda Coincidente (kW)";
                                        rg = ws.Cells[row, 2, row, 2];
                                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                        rg.Style.Font.Color.SetColor(Color.White);
                                        rg.Style.Font.Size = 10;
                                        rg.Style.Font.Bold = true;

                                        ws.Cells[row, 3].Value = item.RDMES1;
                                        ws.Cells[row, 4].Value = item.RDMES2;
                                        ws.Cells[row, 5].Value = item.RDMES3;

                                        //Formateo de celdas
                                        ws.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 5];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                        row++;
                                        row++;

                                        ws.Cells[row++, 3].Value = item.RPREPOT;
                                        ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row++, 3].Value = item.RPEAJEU;
                                        ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row++, 3].Value = item.RMARGENR;
                                        ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row++, 3].Value = item.RPFIRME;
                                        ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row++, 3].Value = item.RPFIRME1MR;
                                        ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";

                                        //Border por celda
                                        rg = ws.Cells[row - 5, 2, row - 1, 3];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                        row++;

                                        ws.Cells[row - 6, 2].Value = "Precio Potencia (**)";
                                        ws.Cells[row - 5, 2].Value = "Peaje Unitario (**)";
                                        ws.Cells[row - 4, 2].Value = "Margen de reserva";
                                        ws.Cells[row - 3, 2].Value = "PFirmeRemun (KW)";
                                        ws.Cells[row - 2, 2].Value = "PFirmeRemun/(1+mr) (KW)";


                                        rg = ws.Cells[row - 6, 2, row - 2, 2];
                                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                        rg.Style.Font.Color.SetColor(Color.White);
                                        rg.Style.Font.Size = 10;
                                        rg.Style.Font.Bold = true;
                                    }

                                    else
                                    {
                                        /*
                                       ws.Cells[row++, 3].Value = "CUADRO N° 2";
                                       ws.Cells[row++, 3].Value = "Montos de Garantías por Capacidad y Peaje SPT - SGT";
                                       row++;
                                       ws.Cells[row++, 2].Value = "Primer cálculo";
                                       rg = ws.Cells[row - 4, 3, row - 2, 4];
                                       rg.Style.Font.Size = 16;
                                       rg.Style.Font.Bold = true;

                                       //CABECERA DE TABLA
                                       ws.Cells[row, 2].Value = "Mes de Cálculo";
                                       ws.Cells[row, 3].Value = "Participante (*)";
                                       ws.Cells[row, 4].Value = "Monto por capacidad(S/)";
                                       ws.Cells[row, 5].Value = "Monto por Peaje (S/)";

                                       //ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                                       //ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                                       //ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                                       //ws.Column(7).Style.Numberformat.Format = "#,##0.00";

                                       rg = ws.Cells[row, 2, row, 5];
                                       rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                       rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                       rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                       rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                       rg.Style.Font.Color.SetColor(Color.White);
                                       rg.Style.Font.Size = 10;
                                       rg.Style.Font.Bold = true;

                                       //Border por celda
                                       rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Left.Color.SetColor(Color.Black);
                                       rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Right.Color.SetColor(Color.Black);
                                       rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Top.Color.SetColor(Color.Black);
                                       rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                       row++;

                                       //nroReg++;

                                       //ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                       //ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                       //ws.Cells[row, 4].Value = item.RCAPACIDAD.ToString().Trim();
                                       //ws.Cells[row, 5].Value = item.RPEAJE;

                                       //Formateo de celdas
                                       ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                       ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";

                                       //Border por celda
                                       rg = ws.Cells[row, 2, row, 5];
                                       rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Left.Color.SetColor(Color.Black);
                                       rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Right.Color.SetColor(Color.Black);
                                       rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Top.Color.SetColor(Color.Black);
                                       rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                       rg.Style.Font.Size = 10;
                                       row++;
                                       row++;

                                       ws.Cells[row, 3].Value = "Mes 1";
                                       ws.Cells[row, 4].Value = "Mes 2";
                                       ws.Cells[row, 5].Value = "Mes 3";

                                       rg = ws.Cells[row, 3, row, 5];
                                       rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                       rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                       rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                       rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                       rg.Style.Font.Color.SetColor(Color.White);
                                       rg.Style.Font.Size = 10;
                                       rg.Style.Font.Bold = true;

                                       //Border por celda
                                       rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Left.Color.SetColor(Color.Black);
                                       rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Right.Color.SetColor(Color.Black);
                                       rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Top.Color.SetColor(Color.Black);
                                       rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                       row++;

                                       ws.Cells[row, 2].Value = "Demanda Coincidente (kW)";
                                       rg = ws.Cells[row, 2, row, 2];
                                       rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                       rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                       rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                       rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                       rg.Style.Font.Color.SetColor(Color.White);
                                       rg.Style.Font.Size = 10;
                                       rg.Style.Font.Bold = true;

                                       //ws.Cells[row, 3].Value = item.RDMES1;
                                       //ws.Cells[row, 4].Value = item.RDMES2;
                                       //ws.Cells[row, 5].Value = item.RDMES3;

                                       //Formateo de celdas
                                       ws.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";
                                       ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                       ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";

                                       //Border por celda
                                       rg = ws.Cells[row, 2, row, 5];
                                       rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Left.Color.SetColor(Color.Black);
                                       rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Right.Color.SetColor(Color.Black);
                                       rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Top.Color.SetColor(Color.Black);
                                       rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                       rg.Style.Font.Size = 10;
                                       row++;
                                       row++;

                                       ////ws.Cells[row++, 3].Value = item.RPREPOT;
                                       //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                       ////ws.Cells[row++, 3].Value = item.RPEAJEU;
                                       //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                       ////ws.Cells[row++, 3].Value = item.RMARGENR;
                                       //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                       ////ws.Cells[row++, 3].Value = item.RPFIRME;
                                       //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";
                                       ////ws.Cells[row++, 3].Value = item.RPFIRME1MR;
                                       //ws.Cells[row - 1, 3].Style.Numberformat.Format = "#,##0.00";

                                       //Border por celda
                                       //rg = ws.Cells[row - 5, 2, row - 1, 3];
                                       //rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                       //rg.Style.Border.Left.Color.SetColor(Color.Black);
                                       //rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                       //rg.Style.Border.Right.Color.SetColor(Color.Black);
                                       //rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                       //rg.Style.Border.Top.Color.SetColor(Color.Black);
                                       //rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                       //rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                       //rg.Style.Font.Size = 10;
                                       //row++;

                                       ws.Cells[row, 2].Value = "Precio Potencia (**)";
                                       ws.Cells[row + 1, 2].Value = "Peaje Unitario (**)";
                                       ws.Cells[row + 2, 2].Value = "Margen de reserva";
                                       ws.Cells[row + 3, 2].Value = "PFirmeRemun (KW)";
                                       ws.Cells[row + 4, 2].Value = "PFirmeRemun/(1+mr) (KW)";


                                       rg = ws.Cells[row, 2, row + 4, 2];
                                       rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                       rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                       rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                       rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                       rg.Style.Font.Color.SetColor(Color.White);
                                       rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Left.Color.SetColor(Color.Black);
                                       rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Right.Color.SetColor(Color.Black);
                                       rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Top.Color.SetColor(Color.Black);
                                       rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                       rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                       rg.Style.Font.Size = 10;
                                       rg.Style.Font.Bold = true;

                                       row = row + 5;

                                

                               row++;

                               if (nroReg == 0)
                                   ws.Cells[row, 2].Value = "Sin Registros";

                               //CABECERA DE TABLA
                               //ws.Cells[row+1, 2].Value = "Demanda Coincidente (kW)";
                               //nroReg = 0;




                               ////Border por celda
                               //rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                               //rg.Style.Border.Left.Color.SetColor(Color.Black);
                               //rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                               //rg.Style.Border.Right.Color.SetColor(Color.Black);
                               //rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                               //rg.Style.Border.Top.Color.SetColor(Color.Black);
                               //rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                               //rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                               row++;
                               //foreach (var item in datosRPT3)
                               //{
                               //    if (item.RPMES == "0")
                               //    {
                               //        nroReg++;
                               //        cont++;

                               //    }

                               //}

                               if (cont != 0)
                               {
                                   rg = ws.Cells[row - cont, 2, row - 1, 2];
                                   rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                   rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                   rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                   rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                   rg.Style.Font.Color.SetColor(Color.White);
                                   rg.Style.Font.Size = 10;
                                   rg.Style.Font.Bold = true;
                               }

                               //if (nroReg == 0)
                               //{
                               //    row++;
                               //    ws.Cells[row, 2].Value = "Sin Registros";
                               //}

                               //nroReg = 0;

                               ///
                                        //cont =0;
                                        //List<GmmDatCalculoDTO> OtroQuery = garantiaAppServicio.listarRpt3(anio, mes);
                                        //row++;
                                        //foreach (var item in OtroQuery)
                                        //{
                                        //    nroReg++;
                                        //    if (cont == 0)Contador para simular 1 solo registro no vale
                                        //    {


                                        //        cont++;
                                        //    }

                                        //}

                                        //if (nroReg != 0)
                                        //{

                                        //}
                                        
                                        row = row + 2;

                                        //if (nroReg == 0)
                                        //    ws.Cells[row, 2].Value = "Sin Registros";
                                      */
                                    }

                                }
                                //Insertar el logo
                                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                picture.From.Column = 0;
                                picture.From.Row = 0;
                                picture.To.Column = 1;
                                picture.To.Row = 1;
                                picture.SetSize(120, 60);



                                /*2do logo*/
                                /*
                                picture.From.Column = 0;
                                picture.From.Row = row;
                                picture.To.Column = 1;
                                picture.To.Row = row+1;
                                picture.SetSize(120, 60);
                                */
                                nroReg = 0;

                                row = row + 1;
                                rg = ws.Cells[row, 3, row + 1, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;
                                ws.Cells[row++, 3].Value = "CUADRO N° 2";
                                ws.Cells[row++, 3].Value = "Montos de Garantías por Capacidad y Peaje SPT - SGT";
                                row = row + 1;

                                //2DA-CABECERA DE TABLA
                                ws.Cells[row++, 2].Value = "A partir del segundo mes";
                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";
                                ws.Cells[row, 4].Value = "Pago de Capacidad -Ingreso por Potencia LVTP(" + mesCadenaAnt + " - " + anioMesAnt + ")";
                                ws.Cells[row, 5].Value = "Monto por Capacidad Mes 2";
                                ws.Cells[row, 6].Value = "Monto por Capacidad Mes 3";
                                ws.Cells[row, 7].Value = "Monto por capacidad (S/)";

                                ws.Column(4).Width = 25.5;
                                ws.Column(4).Style.WrapText = true;

                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                foreach (var item in datosRPT3)
                                {
                                    if (item.RPMES == "0")
                                    {
                                        nroReg++;
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                        ws.Cells[row, 4].Value = item.RCLVTP;
                                        ws.Cells[row, 5].Value = item.RMCM2;
                                        ws.Cells[row, 6].Value = item.RMCM3;
                                        ws.Cells[row, 7].Value = item.RCAPACIDAD;

                                        //Formateo de celdas
                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 7];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                        row++;
                                    }

                                }

                                row++;

                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";
                                ws.Cells[row, 4].Value = "Valorización Peajes LVTP (" + mesCadenaAnt + "-" + anioMesAnt + ")";
                                ws.Cells[row, 5].Value = "Monto por Peaje Mes 2";
                                ws.Cells[row, 6].Value = "Monto por Peaje Mes 3";
                                ws.Cells[row, 7].Value = "Monto por Peaje (S/)";

                                ws.Column(4).Width = 25.5;
                                ws.Column(4).Style.WrapText = true;

                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                //if (nroReg == 0)
                                //{
                                //    row++;
                                //    ws.Cells[row, 2].Value = "Sin Registros";
                                //}
                                nroReg = 0;
                                row++;
                                foreach (var item in datosRPT3)
                                {
                                    if (item.RPMES == "0")
                                    {
                                        nroReg++;
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                        ws.Cells[row, 4].Value = item.RPLVTP;
                                        ws.Cells[row, 5].Value = item.RMPM2;
                                        ws.Cells[row, 6].Value = item.RMPM3;
                                        ws.Cells[row, 7].Value = item.RPEAJE;

                                        //Formateo de celdas
                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";


                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 7];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                        row++;
                                    }

                                }

                                row++;

                                if (nroReg == 0)
                                    ws.Cells[row, 2].Value = "Sin Registros";

                                ws.Column(1).Width = 3.71;
                                ws.Column(2).Width = 22.14;
                                ws.Column(3).Width = 34.14;
                                ws.Column(4).Width = 27.29;
                                ws.Column(5).Width = 24.14;
                                ws.Column(6).Width = 23.83;
                                ws.Column(7).Width = 26.57;
                                ws.Column(4).Style.WrapText = true;
                                ws.Column(5).Style.WrapText = true;
                                ws.Column(6).Style.WrapText = true;
                                ws.Column(7).Style.WrapText = true;

                            }
                            xlPackage.Save();
                        }
                        indicador = "1";
                        break;
                    #endregion
                    #region Montos de Garantías por servicios complementarios:
                    case "rpt4":
                        List<GmmDatCalculoDTO> datosRPT4 = garantiaAppServicio.listarRpt4(anio, mes);

                        path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                        newFile = new FileInfo(path + Funcion.NombreRpt4);

                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(path + Funcion.NombreRpt4);
                        }

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                            nroReg = 0;
                            if (ws != null)
                            {   //TITULO
                                int row = 2; //fila donde inicia la data
                                ws.Cells[row++, 3].Value = "CUADRO N° 3";
                                ws.Cells[row++, 3].Value = "Montos de Garantías por Servicios Complementarios";
                                row++;
                                ExcelRange rg = ws.Cells[2, 3, row, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;

                                //CABECERA DE TABLA
                                ws.Cells[row++, 2].Value = "Primer cálculo";
                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";
                                ws.Cells[row, 4].Value = "Mes 1";
                                ws.Cells[row, 5].Value = "Mes 2";
                                ws.Cells[row, 6].Value = "Mes 3";
                                ws.Cells[row, 7].Value = "Monto por Servicios Complementarios(S/)";

                                //ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                                //ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                                //ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                                //ws.Column(7).Style.Numberformat.Format = "#,##0.00";

                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                foreach (var item in datosRPT4)
                                {
                                    if (item.RPMES == "1")
                                    {
                                        nroReg++;
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                        ws.Cells[row, 4].Value = item.RMES1;
                                        ws.Cells[row, 5].Value = item.RMES2;
                                        ws.Cells[row, 6].Value = item.RMES3;
                                        ws.Cells[row, 7].Value = item.RSCOMPLE;

                                        //Formateo de celdas
                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 7];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                        row++;
                                    }

                                }

                                //Insertar el logo
                                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                picture.From.Column = 0;
                                picture.From.Row = 0;
                                picture.To.Column = 1;
                                picture.To.Row = 1;
                                picture.SetSize(120, 60);

                                if (nroReg == 0)
                                {
                                    //row++;
                                    ws.Cells[row, 2, row, 7].Value = "Sin Registros";
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                }
                                nroReg = 0;
                                row++;





                                if (datosRPT4.Count() == 0)
                                {
                                    /*2do logo*/
                                    /*
                                    picture.From.Column = 0;
                                    picture.From.Row = row;
                                    picture.To.Column = 1;
                                    picture.To.Row = row+1;
                                    picture.SetSize(120, 60);
                                    */

                                    row = row + 1;
                                    rg = ws.Cells[row, 3, row + 1, 4];
                                    rg.Style.Font.Size = 16;
                                    rg.Style.Font.Bold = true;
                                    ws.Cells[row++, 3].Value = "CUADRO N° 3";
                                    ws.Cells[row++, 3].Value = "Montos de Garantías por Servicios Complementarios";
                                    row = row + 1;


                                    //2DA-CABECERA DE TABLA
                                    ws.Cells[row++, 2].Value = "A partir del segundo mes";
                                    ws.Cells[row, 2].Value = "Mes de Cálculo";
                                    ws.Cells[row, 3].Value = "Participante (*)";
                                    ws.Cells[row, 4].Value = "VMSCm-1";
                                    ws.Cells[row, 5].Value = "A= (J/10)*Valorizaciones Diarias";
                                    ws.Cells[row, 6].Value = "A*(Epm+1/Epm)";
                                    ws.Cells[row, 7].Value = "Monto por Servicios Complementarios(S/)";
                                    ws.Column(7).Width = 21.5;
                                    ws.Column(7).Style.WrapText = true;

                                    rg = ws.Cells[row, 2, row, 7];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                    rg.Style.Font.Color.SetColor(Color.White);
                                    rg.Style.Font.Size = 10;
                                    rg.Style.Font.Bold = true;

                                    //Border por celda
                                    rg = ws.Cells[row, 2, row, 7];
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);


                                    row++;

                                    row = row + 2;
                                    ws.Cells[row++, 2].Value = "Energia Retirada 10 dias + Energia Prevista (11-m) " + mesCadenaAct + "-" + anio;
                                    ws.Cells[row - 1, 2].Style.WrapText = true;
                                    ws.Cells[row - 1, 3].Value = "Epm";
                                    //ws.Cells[row - 1, 4].Value = item.ENRGN.ToString().Trim();
                                    ws.Cells[row++, 2].Value = "Energía Prevista a Retirar " + mesCadenaSig + "-" + anioMesSig;
                                    ws.Cells[row - 1, 2].Style.WrapText = true;
                                    ws.Cells[row - 1, 3].Value = "Epm+1";
                                    //ws.Cells[row - 1, 4].Value = item.ENRGN1.ToString().Trim();

                                    //ws.Column(4).Width = 25.5;
                                    //ws.Column(4).Style.WrapText = true;

                                    rg = ws.Cells[row - 2, 2, row - 1, 3];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                    rg.Style.Font.Color.SetColor(Color.White);
                                    rg.Style.Font.Size = 10;
                                    rg.Style.Font.Bold = true;

                                    //Border por celda
                                    rg = ws.Cells[row - 2, 2, row - 1, 4];
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                    row++;



                                }


                                row = row + 1;
                                rg = ws.Cells[row, 3, row + 1, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;
                                ws.Cells[row++, 3].Value = "CUADRO N° 3";
                                ws.Cells[row++, 3].Value = "Montos de Garantías por Servicios Complementarios";
                                row = row + 1;

                                //2DA-CABECERA DE TABLA
                                ws.Cells[row++, 2].Value = "A partir del segundo mes";
                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";
                                ws.Cells[row, 4].Value = "VMSCm-1";
                                ws.Cells[row, 5].Value = "A= (J/10)*Valorizaciones Diarias";
                                ws.Cells[row, 6].Value = "A*(Epm+1/Epm)";
                                ws.Cells[row, 7].Value = "Monto por Servicios Complementarios(S/)";
                               
                                ws.Cells[row - 1, 9].Value = "Energia Retirada 10 dias + Energia Prevista (11-m) " + mesCadenaAct + "-" + anio;
                                ws.Cells[row, 9].Value = "Epm";
                                ws.Cells[row - 1, 10].Value = "Energía Prevista a Retirar " + mesCadenaSig + "-" + anioMesSig;
                                ws.Cells[row, 10].Value = "Epm + 1";
                                ws.Column(7).Width = 21.5;
                                ws.Column(7).Style.WrapText = true;
                                ws.Column(9).Width = 22.5;
                                ws.Column(9).Style.WrapText = true;
                                ws.Column(10).Width = 21.5;
                                ws.Column(10).Style.WrapText = true;

                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                rg = ws.Cells[row - 1, 9, row, 10];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                rg = ws.Cells[row - 1, 9, row, 10];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                               
                                row++;
                                foreach (var item in datosRPT4)
                                {

                                    if (item.RPMES == "0")
                                    {
                                        nroReg++;                                      

                                        //row = row + 4;    
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                        ws.Cells[row, 4].Value = item.RMES1;
                                        ws.Cells[row, 5].Value = item.RMES2;
                                        ws.Cells[row, 6].Value = item.RMES3;
                                        ws.Cells[row, 7].Value = item.RSCOMPLE;
                                        ws.Cells[row, 9].Value = item.ENRGN;
                                        ws.Cells[row, 10].Value = item.ENRGN1;

                                        //Formateo de celdas
                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 9].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 10].Style.Numberformat.Format = "#,##0.00";

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 7];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;

                                        rg = ws.Cells[row, 9, row, 10];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;

                                        //row = row + 2;
                                        //ws.Cells[row++, 2].Value = "Energia Retirada 10 dias + Energia Prevista (11-m) " + mesCadenaAct + "-" + anio;
                                        //ws.Cells[row - 1, 2].Style.WrapText = true;
                                        //ws.Cells[row - 1, 3].Value = "Epm";
                                        //ws.Cells[row - 1, 4].Value = item.ENRGN;
                                        //ws.Cells[row - 1, 4].Style.Numberformat.Format = "#,##0.00";
                                        //ws.Cells[row++, 2].Value = "Energía Prevista a Retirar " + mesCadenaSig + "-" + anioMesSig;
                                        //ws.Cells[row - 1, 2].Style.WrapText = true;
                                        //ws.Cells[row - 1, 3].Value = "Epm+1";
                                        //ws.Cells[row - 1, 4].Value = item.ENRGN1;
                                        //ws.Cells[row - 1, 4].Style.Numberformat.Format = "#,##0.00";


                                        //rg = ws.Cells[row - 2, 2, row - 1, 3];
                                        //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        //rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        //rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                        //rg.Style.Font.Color.SetColor(Color.White);
                                        //rg.Style.Font.Size = 10;
                                        //rg.Style.Font.Bold = true;

                                        ////Border por celda
                                        //rg = ws.Cells[row - 2, 2, row - 1, 4];
                                        //rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        //rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        //rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        //rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        //rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        //rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        //rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        //rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                        row++;

                                    }
                                    /*else
                                    {


                                        row = row + 1;
                                        rg = ws.Cells[row, 3, row + 1, 4];
                                        rg.Style.Font.Size = 16;
                                        rg.Style.Font.Bold = true;
                                        ws.Cells[row++, 3].Value = "CUADRO N° 3";
                                        ws.Cells[row++, 3].Value = "Montos de Garantías por Servicios Complementarios";
                                        row = row + 1;

                                        //row = row + 4;

                                        //2DA-CABECERA DE TABLA
                                        ws.Cells[row++, 2].Value = "A partir del segundo mes";
                                        ws.Cells[row, 2].Value = "Mes de Cálculo";
                                        ws.Cells[row, 3].Value = "Participante (*)";
                                        ws.Cells[row, 4].Value = "VMSCm-1";
                                        ws.Cells[row, 5].Value = "A= (J/10)*Valorizaciones Diarias";
                                        ws.Cells[row, 6].Value = "A*(Epm+1/Epm)";
                                        ws.Cells[row, 7].Value = "Monto por Servicios Complementarios(S/)";
                                        ws.Column(7).Width = 21.5;
                                        ws.Column(7).Style.WrapText = true;

                                        rg = ws.Cells[row, 2, row, 7];
                                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                        rg.Style.Font.Color.SetColor(Color.White);
                                        rg.Style.Font.Size = 10;
                                        rg.Style.Font.Bold = true;

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 7];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                        row++;

                                        row = row + 2;
                                        ws.Cells[row++, 2].Value = "Energia Retirada 10 dias + Energia Prevista (11-m) " + mesCadenaAct + "-" + anio;
                                        ws.Cells[row - 1, 2].Style.WrapText = true;
                                        ws.Cells[row - 1, 3].Value = "Epm";
                                        //ws.Cells[row - 1, 4].Value = item.ENRGN.ToString().Trim();
                                        ws.Cells[row++, 2].Value = "Energía Prevista a Retirar " + mesCadenaSig + "-" + anioMesSig;
                                        ws.Cells[row - 1, 2].Style.WrapText = true;
                                        ws.Cells[row - 1, 3].Value = "Epm+1";

                                        rg = ws.Cells[row - 2, 2, row - 1, 3];
                                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                        rg.Style.Font.Color.SetColor(Color.White);
                                        rg.Style.Font.Size = 10;
                                        rg.Style.Font.Bold = true;

                                        //Border por celda
                                        rg = ws.Cells[row - 2, 2, row - 1, 4];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                        row++;
                                    }
*/
                                }

                                if (nroReg == 0)
                                {
                                    ws.Cells[row, 2].Value = "Sin Registros";
                                }

                                ws.Column(1).Width = 3.71;
                                ws.Column(2).Width = 29.43;
                                ws.Column(3).Width = 40.86;
                                ws.Column(4).Width = 20.86;
                                ws.Column(5).Width = 20.14;
                                ws.Column(6).Width = 20.86;
                                ws.Column(7).Width = 20.86;
                                ws.Column(4).Style.WrapText = true;
                                ws.Column(5).Style.WrapText = true;
                                ws.Column(6).Style.WrapText = true;
                                ws.Column(7).Style.WrapText = true;


                            }
                            xlPackage.Save();
                        }
                        indicador = "1";
                        break;
                    #endregion
                    #region Montos de Garantías por recaudación de energía reactiva:
                    case "rpt6":
                        List<GmmDatCalculoDTO> datosRPT6 = garantiaAppServicio.listarRpt6(anio, mes);

                        path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                        newFile = new FileInfo(path + Funcion.NombreRpt5);
                        nroReg = 0;
                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(path + Funcion.NombreRpt5);
                        }

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                            if (ws != null)
                            {   //TITULO
                                int row = 2; //fila donde inicia la data
                                ws.Cells[row++, 3].Value = "CUADRO N° 4";
                                ws.Cells[row++, 3].Value = "Montos de Garantías por Inflexiblidades operativas";
                                row++;
                                ExcelRange rg = ws.Cells[2, 3, row - 2, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;

                                //CABECERA DE TABLA
                                ws.Cells[row, 2].Value = "Primer cálculo de nuevos participantes";

                                ws.Cells[row, 4].Value = "Mes 1";
                                ws.Cells[row, 7].Value = "Mes 2";
                                ws.Cells[row, 10].Value = "Mes 3";
                                ws.Cells[row, 13].Value = "Pagos por inflexibilidades operativas mensual S/";
                                ws.Cells[row, 14].Value = "Monto por Inflexibilidad Operativa (S/)";
                                ws.Cells[row, 4, row, 6].Merge = true;
                                ws.Cells[row, 7, row, 9].Merge = true;
                                ws.Cells[row, 10, row, 12].Merge = true;
                                ws.Cells[row, 13, row + 1, 13].Merge = true;
                                ws.Cells[row, 14, row + 1, 14].Merge = true;

                                rg = ws.Cells[row, 4, row, 14];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";
                                ws.Cells[row, 4].Value = "Demanda Total Prevista, obtenida del PMPO";
                                ws.Cells[row, 5].Value = "Energía Prevista a Retirar";
                                ws.Cells[row, 6].Value = "Participación";
                                ws.Cells[row, 7].Value = "Demanda Total Prevista, obtenida del PMPO";
                                ws.Cells[row, 8].Value = "Energía Prevista a Retirar";
                                ws.Cells[row, 9].Value = "Participación";
                                ws.Cells[row, 10].Value = "Demanda Total Prevista, obtenida del PMPO";
                                ws.Cells[row, 11].Value = "Energía Prevista a Retirar";
                                ws.Cells[row, 12].Value = "Participación";

                                rg = ws.Cells[row, 2, row, 14];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                foreach (var item in datosRPT6)
                                {
                                    if (item.RPMES == "1")
                                    {
                                        nroReg++;
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();

                                        ws.Cells[row, 4].Value = item.RDEMM1; //mes1-Demanda Total Prevista, obtenida del PMPO
                                        ws.Cells[row, 5].Value = item.REPRM1;
                                        ws.Cells[row, 6].Value = item.RPARM1;
                                        ws.Cells[row, 7].Value = item.RDEMM2; //mes2-Demanda Total Prevista, obtenida del PMPO
                                        ws.Cells[row, 8].Value = item.REPRM2;
                                        ws.Cells[row, 9].Value = item.RPARM2;
                                        ws.Cells[row, 10].Value = item.RDEMM3; //mes3-Demanda Total Prevista, obtenida del PMPO
                                        ws.Cells[row, 11].Value = item.REPRM3;
                                        ws.Cells[row, 12].Value = item.RPARM3;
                                        ws.Cells[row, 13].Value = item.RINFLEX;//Pagos por inflexibilidades operativas mensual S/
                                        ws.Cells[row, 14].Value = item.RINFLEXOP;

                                        //Formateo de celdas
                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 8].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 9].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 10].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 11].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 12].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 13].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 14].Style.Numberformat.Format = "#,##0.00";

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 14];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                        row++;
                                    }

                                }

                                //Insertar el logo
                                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                picture.From.Column = 0;
                                picture.From.Row = 0;
                                picture.To.Column = 1;
                                picture.To.Row = 1;
                                picture.SetSize(120, 60);

                                if (nroReg == 0)
                                {
                                    //row++;
                                    ws.Cells[row, 2].Value = "No existen Registros";
                                    rg = ws.Cells[row, 2, row, 14];
                                    ws.Cells[row, 2, row, 14].Merge = true;
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                }

                                nroReg = 0;
                                row++;

                                row = row + 1;
                                rg = ws.Cells[row, 3, row + 1, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;
                                ws.Cells[row++, 3].Value = "CUADRO N° 4";
                                ws.Cells[row++, 3].Value = "Montos de Garantías por Inflexiblidades operativas";


                                //2DA-CABECERA DE TABLA
                                row++;
                                ws.Cells[row, 2].Value = "A partir del segundo mes";
                                ws.Cells[row, 9].Value = "Energia Retirada 10 dias + Energia Prevista (11-m) " + mesCadenaAct + "-" + anio;
                                ws.Cells[row, 10].Value = "Energía Prevista a Retirar " + mesCadenaSig + "-" + anioMesSig;


                                //COLOR
                                rg = ws.Cells[row, 9, row + 1, 10];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //BORDER
                                rg = ws.Cells[row, 9, row + 1, 10];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);


                                row++;
                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";                               
                                ws.Cells[row, 4].Value = "VMIOm-1";
                                ws.Cells[row, 5].Value = "A= (J/10)*Valorizaciones Diarias";
                                ws.Cells[row, 6].Value = "A*(Epm+1/Epm)";
                                ws.Cells[row, 7].Value = "Monto por Inflexibilidad Operativa (S/)";

                                ws.Cells[row, 9].Value = "Epm";
                                ws.Cells[row, 10].Value = "Epm+1";

                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);                               

                                foreach (var item in datosRPT6)
                                {
                                    if (item.RPMES == "0")
                                    {
                                        nroReg++;

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 7];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                        rg = ws.Cells[row, 9, row, 10];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                        row++;
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                        ws.Cells[row, 4].Value = item.RLSCIO;
                                        ws.Cells[row, 5].Value = item.RCOL1;
                                        ws.Cells[row, 6].Value = item.RCOL2;
                                        ws.Cells[row, 7].Value = item.RINFLEXOP;
                                        ws.Cells[row, 9].Value = item.RER10;
                                        ws.Cells[row, 10].Value = item.REPR11;

                                        //Formateo de celdas
                                        ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 9].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[row, 10].Style.Numberformat.Format = "#,##0.00";

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 7];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;

                                        rg = ws.Cells[row, 9, row, 10];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                    }

                                }

                                if (nroReg == 0)
                                {
                                    row++;
                                    ws.Cells[row, 2].Value = "Sin Registros";
                                }

                                ws.Column(1).Width = 3.71;
                                ws.Column(2).Width = 30.86;
                                ws.Column(3).Width = 30.43;
                                ws.Column(4).Width = 14.43;
                                ws.Column(5).Width = 17.29;
                                ws.Column(6).Width = 13.86;
                                ws.Column(7).Width = 18.29;
                                ws.Column(8).Width = 15.43;
                                ws.Column(9).Width = 24.57;
                                ws.Column(10).Width = 24.14;
                                ws.Column(11).Width = 14.14;
                                ws.Column(12).Width = 13.29;
                                ws.Column(13).Width = 13.57;
                                ws.Column(14).Width = 14.14;
                                ws.Column(4).Style.WrapText = true;
                                ws.Column(5).Style.WrapText = true;
                                ws.Column(6).Style.WrapText = true;
                                ws.Column(7).Style.WrapText = true;
                                ws.Column(8).Style.WrapText = true;
                                ws.Column(9).Style.WrapText = true;
                                ws.Column(10).Style.WrapText = true;
                                ws.Column(11).Style.WrapText = true;
                                ws.Column(12).Style.WrapText = true;
                                ws.Column(13).Style.WrapText = true;
                                ws.Column(14).Style.WrapText = true;

                            }
                            xlPackage.Save();
                        }
                        indicador = "1";

                        break;
                    #endregion
                    #region Montos de Garantías por Inflexiblidades operativas:
                    case "rpt5":
                        List<GmmDatCalculoDTO> datosRPT5 = garantiaAppServicio.listarRpt5(anio, mes);

                        path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                        newFile = new FileInfo(path + Funcion.NombreRpt6);
                        nroReg = 0;
                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(path + Funcion.NombreRpt6);
                        }

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                            if (ws != null)
                            {   //TITULO
                                int row = 2; //fila donde inicia la data
                                int colum = 2; //columna donde inicia la data
                                ws.Cells[row++, 3].Value = "CUADRO N° 5";
                                ws.Cells[row++, 3].Value = "Montos de Garantías por Recaudación de Energía Reactiva";
                                row++;
                                ExcelRange rg = ws.Cells[2, 3, row - 2, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;

                                //CABECERA DE TABLA
                                row++;
                                ws.Cells[row, 2].Value = "Primer cálculo de nuevos participantes";
                                ws.Cells[row, 4].Value = "Mes 1";
                                ws.Cells[row, 6].Value = "Mes 2";
                                ws.Cells[row, 8].Value = "Mes 3";
                                ws.Cells[row, 10].Value = "Monto por Recaudación Energia Reactiva (S/)";

                                ws.Cells[row, 4, row, 5].Merge = true;
                                ws.Cells[row, 6, row, 7].Merge = true;
                                ws.Cells[row, 8, row, 9].Merge = true;
                                ws.Cells[row, 10, row + 1, 10].Merge = true;


                                rg = ws.Cells[row, 4, row, 10];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";

                                ws.Cells[row, 4].Value = "Energía Prevista a Retirar";
                                ws.Cells[row, 5].Value = "Participación";
                                ws.Cells[row, 6].Value = "Energía Prevista a Retirar";
                                ws.Cells[row, 7].Value = "Participación";
                                ws.Cells[row, 8].Value = "Energía Prevista a Retirar";
                                ws.Cells[row, 9].Value = "Participación";


                                ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                                ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                                ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                                ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                                ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                                ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                                ws.Column(10).Style.Numberformat.Format = "#,##0.00";


                                rg = ws.Cells[row, 2, row, 10];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                row++;
                                foreach (var item in datosRPT5)
                                {
                                    if (item.RPMES == "1")
                                    {
                                        nroReg++;
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();

                                        ws.Cells[row, 4].Value = item.REPRM1;
                                        ws.Cells[row, 5].Value = item.RPARM1;
                                        ws.Cells[row, 6].Value = item.REPRM2;
                                        ws.Cells[row, 7].Value = item.RPARM2;
                                        ws.Cells[row, 8].Value = item.REPRM3;
                                        ws.Cells[row, 9].Value = item.RPARM3;
                                        ws.Cells[row, 10].Value = item.REREACTIVA;

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 10];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                        row++;
                                    }

                                }

                                //Insertar el logo
                                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                picture.From.Column = 0;
                                picture.From.Row = 0;
                                picture.To.Column = 1;
                                picture.To.Row = 1;
                                picture.SetSize(120, 60);

                                if (nroReg == 0)
                                {
                                    //row++;
                                    ws.Cells[row, 2].Value = "No existen Registros";
                                    rg = ws.Cells[row, 2, row, 10];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                    ws.Cells[row, 2, row, 10].Merge = true;
                                }
                                nroReg = 0;
                                row++;

                                //inicio de modificacion - assetec

                                row = row + 1;
                                rg = ws.Cells[row, 3, row + 1, 4];
                                rg.Style.Font.Size = 16;
                                rg.Style.Font.Bold = true;
                                ws.Cells[row++, 3].Value = "CUADRO N° 5";
                                ws.Cells[row++, 3].Value = "Montos de Garantías por Recaudación de Energía Reactiva";
                                row = row + 1;

                                //2DA-CABECERA DE TABLA
                                row++;
                                ws.Cells[row, 2].Value = "A partir del segundo mes";
                                ws.Cells[row, 9].Value = "Energia Retirada 10 dias + Energia Prevista (10-m) " + mesCadenaAct + "-" + anio;
                                ws.Cells[row, 10].Value = "Energía Prevista a Retirar " + mesCadenaSig + "-" + anioMesSig;

                                rg = ws.Cells[row, 9, row , 10];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg = ws.Cells[row, 9, row, 10];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                //

                                row++;
                                ws.Cells[row, 2].Value = "Mes de Cálculo";
                                ws.Cells[row, 3].Value = "Participante (*)";
                                ws.Cells[row, 4].Value = "VMERm-1";
                                ws.Cells[row, 5].Value = "A= (J/10)*Valorizaciones Diarias";
                                ws.Cells[row, 6].Value = "A*(Epm+1/Epm)";
                                ws.Cells[row, 7].Value = "Monto por Recaudación Energia Reactiva (S/)";                               
                                ws.Cells[row, 9].Value = "Epm";
                                ws.Cells[row, 10].Value = "Epm+1";
                               

                        

                                ws.Column(5).Width = 21.5;
                                ws.Column(5).Style.WrapText = true;
                                ws.Column(9).Width = 21.5;
                                ws.Column(9).Style.WrapText = true;
                                ws.Column(10).Width = 21.5;
                                ws.Column(10).Style.WrapText = true;


                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg = ws.Cells[row, 2, row, 7];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                                rg = ws.Cells[row, 9, row, 10];
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Bold = true;

                                //Border por celda
                                rg = ws.Cells[row, 9, row, 10];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);


                                row++;

                                foreach (var item in datosRPT5)
                                {
                                    if (item.RPMES == "0")
                                    {
                                        nroReg++;
                                        ws.Cells[row, 2].Value = mesCadenaAct + " - " + anio;
                                        ws.Cells[row, 3].Value = item.EMPRESA.ToString().Trim();
                                        ws.Cells[row, 4].Value = item.RVMERM1;
                                        ws.Cells[row, 5].Value = item.RCOL1;
                                        ws.Cells[row, 6].Value = item.RCOL2;
                                        ws.Cells[row, 7].Value = item.REREACTIVA;
                                        ws.Cells[row, 9].Value = item.RER10;
                                        ws.Cells[row, 10].Value = item.REPR11;

                                        //Border por celda
                                        rg = ws.Cells[row, 2, row, 7];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;

                                        rg = ws.Cells[row, 9, row, 10];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;

                                        row++;
                                    }
                                }


                                if (nroReg == 0)
                                {
                                    row++;
                                    ws.Cells[row, 2].Value = "Sin Registros";
                                }


                                ws.Column(1).Width = 3.71;
                                ws.Column(2).Width = 32;
                                ws.Column(3).Width = 40;
                                ws.Column(4).Width = 24.57;
                                ws.Column(5).Width = 24.86;
                                ws.Column(6).Width = 24.29;
                                ws.Column(7).Width = 24.29;
                                ws.Column(8).Width = 16.43;
                                ws.Column(9).Width = 26.43;
                                ws.Column(10).Width = 28.43;
                                ws.Column(11).Width = 86.43;

                                ws.Column(4).Style.WrapText = true;
                                ws.Column(5).Style.WrapText = true;
                                ws.Column(6).Style.WrapText = true;
                                ws.Column(7).Style.WrapText = true;
                                ws.Column(8).Style.WrapText = true;
                                ws.Column(9).Style.WrapText = true;
                                ws.Column(10).Style.WrapText = true;


                            }

                            xlPackage.Save();
                        }
                        


                        indicador = "1";

                        break;
                    #endregion
                    default:
                        break;
                }

            }
            catch (Exception e)
            {
                Log.Error("exportarReporte", e);
                indicador = e.Message;//"-1";
            }
            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult rptInsumos()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreRptInsumo;
            return File(path, Funcion.AppExcel, sFecha + "_" + Funcion.NombreRptInsumo);
        }

        [HttpGet]
        public virtual ActionResult rpt1()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreRpt1;
            return File(path, Funcion.AppExcel, sFecha + "_" + Funcion.NombreRpt1);
        }

        [HttpGet]
        public virtual ActionResult rpt2()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreRpt2;
            return File(path, Funcion.AppExcel, sFecha + "_" + Funcion.NombreRpt2);
        }

        [HttpGet]
        public virtual ActionResult rpt3()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreRpt3;
            return File(path, Funcion.AppExcel, sFecha + "_" + Funcion.NombreRpt3);
        }

        [HttpGet]
        public virtual ActionResult rpt4()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreRpt4;
            return File(path, Funcion.AppExcel, sFecha + "_" + Funcion.NombreRpt4);
        }

        [HttpGet]
        public virtual ActionResult rpt5()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreRpt6;
            return File(path, Funcion.AppExcel, sFecha + "_" + Funcion.NombreRpt6);
        }

        [HttpGet]
        public virtual ActionResult rpt6()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreRpt5;
            return File(path, Funcion.AppExcel, sFecha + "_" + Funcion.NombreRpt5);
        }

        [HttpPost]
        public JsonResult Respaldar(int anio, int mes)
        {
            string indicador = "1";

            try {
                // 1. Obtener las empresas agentes que son parte del procesamiento
                // Debe tener empgcodi, emprcodi, flagCalculo para saber si tiene un cálculo previo

                // TODO: Validar que el periodo no esté cerrado

                AgenteAppServicio agenteAppServicio = new AgenteAppServicio();

                List<GmmEmpresaDTO> agentes = agenteAppServicio.listarAgentesCalculo(anio, mes, Constantes.formatoEnergiaTrimestral, Constantes.formatoEnergiaMensual, Constantes.tipoEnergia);

                List<GmmEmpresaDTO> agentesEntrega = agenteAppServicio.listarAgentesEntregaCalculo(anio, mes, Constantes.formatoEnergiaEntregaTrimestral, Constantes.formatoEnergiaEntregaMensual, Constantes.tipoEnergia);

                int pericodi = agentes.First().PericodiCal;

                string periestado = agentes.First().PERIESTADO;
                if (periestado.Equals("C"))
                {
                    return Json("2");
                }

                //Inicio Nueva implementacion - Assetec
                //Realiza los calculos para Entrega Prevista
                foreach (GmmEmpresaDTO agente in agentesEntrega)
                {
                    // Validar si la empresa puedes ser procesada o ya está cerrada
                    //System.Threading.Thread.Sleep(5000);
                    // borrar toda la data de la empresa
                    garantiaAppServicio.DeletevaloresEntrega(agente);

                    // Ver si es el primer mes
                    agente.EmpgPrimerMes = false;
                    if (agente.EMPGFASECAL.Equals("1")) agente.EmpgPrimerMes = true;

                    foreach (GmmValEnergiaEntregaDTO valEnergia in agente.gmmValEnergiaEntregaDTOs)
                    {
                        decimal dFactor = 1;
                        //ASSETEC 2022.02 - CONSULTAMOS POR EL VALOR DEL FACTOR DE LA TABLA cai_equisddpbarr
                        CaiEquisddpbarrDTO dtoCaiEquisddpbarr = this.servicioCalculoPorcentaje.GetByBarraNombreSddp(valEnergia.BARRCODI, valEnergia.CASDDBBARRA);
                        if (dtoCaiEquisddpbarr != null)
                        {
                            dFactor = dtoCaiEquisddpbarr.Casddbfactbarrpmpo;
                        }
                        else
                        {
                            //Salir, falta el costo marginal PMPO
                            return Json("La Barra PMPO " + valEnergia.CASDDBBARRA + " no existe. Registrar su relación en Equivalencia entre barras de costo marginal ejecutado y barras del SDDP");
                        }
                        //FIN ASSETEC
                        // Leer la Energía Actual para n, n+1, n+2
                        valEnergia.formatoEnergiaMensual = Constantes.formatoEnergiaEntregaMensual;
                        valEnergia.formatoEnergiaTrimestral = Constantes.formatoEnergiaEntregaTrimestral;
                        valEnergia.VALANIO = anio; valEnergia.VALMES = mes; valEnergia.EmpgPrimerMes = agente.EmpgPrimerMes;
                        valEnergia.EMPRCODI = agente.EmprcodiCal; valEnergia.PTOMEDICODI = valEnergia.PTOMEDICODI;

                        List<GmmValEnergiaEntregaDTO> valEnergiaOrigen = garantiaAppServicio.ListarValores96OriginalesEntrega(valEnergia);
                        // Grabar la Energía Actual para n, n+1, n+2
                        List<GmmValEnergiaEntregaDTO> vCostoMarginal1 = null; List<GmmValEnergiaEntregaDTO> vCostoMarginal2 = null;

                        foreach (GmmValEnergiaEntregaDTO avalEnergiaOrigen in valEnergiaOrigen)
                        {
                            //break;

                            avalEnergiaOrigen.CASDDBBARRA = valEnergia.CASDDBBARRA;
                            // Mecanismo para registro de Costo Marginal
                            Double vhorasSemanaNoEjec = 0;
                            // Total horas
                            Double vthorasB1 = 0; Double vthorasB2 = 0; Double vthorasB3 = 0;
                            Double vthorasB4 = 0; Double vthorasB0 = 0; Double vdifHoras = 0;
                            // Horas para el día actual de mediciones
                            Double vdhorasB1 = 0; Double vdhorasB2 = 0; Double vdhorasB3 = 0;
                            Double vdhorasB4 = 0; Double vdhorasB0 = 0;
                            Double vhorasDiaEjec = 0;
                            decimal vcostoMargB1 = 0; decimal vcostoMargB2 = 0; decimal vcostoMargB3 = 0;
                            decimal vcostoMargB4 = 0; decimal vcostoMargB0 = 0;
                            Double[] vConfHoras = null;
                            // Control del costor marginal *LHBN\NET-CONSULTORES*


                            vCostoMarginal1 = garantiaAppServicio.ListarValoresEntregaCostoMarginal(avalEnergiaOrigen, anio, mes);
                            // Si el dia es menor que 11 puede darse que no haya datos
                            if (vCostoMarginal1.Count == 0)
                            {
                                //continue;   //agregar codigo para que devuelva nombre de la barra qiue no ha encontrado avalEnergiaOrigen.CASDDBBARRA
                                //Salir, falta el costo marginal PMPO
                                return Json("La Barra PMPO " + valEnergia.CASDDBBARRA + " no existe. Verificar si la Barra PMPO existe en la tabla ME_MEDICIONXINTERVALO");
                            }
                            vConfHoras = programacionAppServicio.ObtenerHoras(vCostoMarginal1.First().MEDINTFECHAINI);
                            vthorasB0 = vConfHoras.ElementAt(0); vcostoMargB0 = vCostoMarginal1.ElementAt(0).MEDINTH1 * dFactor;
                            vthorasB1 = vConfHoras.ElementAt(1); vcostoMargB1 = vCostoMarginal1.ElementAt(1).MEDINTH1 * dFactor;
                            vthorasB2 = vConfHoras.ElementAt(2); vcostoMargB2 = vCostoMarginal1.ElementAt(2).MEDINTH1 * dFactor;
                            vthorasB3 = vConfHoras.ElementAt(3); vcostoMargB3 = vCostoMarginal1.ElementAt(3).MEDINTH1 * dFactor;
                            vthorasB4 = vConfHoras.ElementAt(4); vcostoMargB4 = vCostoMarginal1.ElementAt(4).MEDINTH1 * dFactor;
                            // determinar horas transcurridas desde el inicio de la semana 
                            // hasta las 00:15 del día actual en el registro de avalEnergiaOrigen
                            vdifHoras = avalEnergiaOrigen.MEDIFECHA.Value.Subtract(vCostoMarginal1.First().MEDINTFECHAINI).TotalHours;
                            vhorasSemanaNoEjec = 168 - vdifHoras;
                            // determinar intervalos que serán empleados por cada Bloque
                            // los intervalos son cada uno 15 minutos (0.25 de horas)
                            // traducir los intervalos en número entero maximo de 96 
                            // Por ejemplo debe quedar vInterB1 = 10, vInterB2 = 50, vInterB3= 36
                            // Siempre suma 96 todos los intervalos y en base a esto es que asignamos valores
                            vhorasDiaEjec = 24;
                            // hallar las distribución de horas que se van a emplear
                            #region
                            //retirar seccion
                            #endregion
                            // Asignar el costo para el registro de cada intervalo (96)
                            // p.Name;
                            // g.GetType().GetProperty(coorde1).GetValue(_g, null).ToString();

                            // Determinar si el día es feriado
                            bool esFerido = false;
                            esFerido = (new GeneralAppServicio()).EsFeriado(avalEnergiaOrigen.MEDIFECHA.Value);

                            if (avalEnergiaOrigen.MEDIFECHA.Value.DayOfWeek == DayOfWeek.Saturday
                                || avalEnergiaOrigen.MEDIFECHA.Value.DayOfWeek == DayOfWeek.Sunday)
                            {
                                esFerido = true;
                            }

                            int j = 0;
                            for (int i = 1; i <= 96; i++)
                            {
                                j = i;

                                // Primer grupo de 00:00 hasta las 08:00
                                if (i <= 32)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB4);
                                }
                                if (i > 32 && i <= 44)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB3);
                                }
                                if (i > 44 && i <= 48)
                                {
                                    if (esFerido)
                                        avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB3);
                                    else
                                        avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB1);
                                }
                                if (i > 48 && i <= 72)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB3);
                                }
                                if (i > 72 && i <= 76)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB2);
                                }
                                if (i > 76 && i <= 78)
                                {
                                    if (esFerido)
                                        avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB2);
                                    else
                                        avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB0);
                                }
                                if (i > 78 && i <= 92)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB2);
                                }
                                if (i > 92)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALENEVALENEVALCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB4);
                                }

                            }

                            // Grabar los resultados
                            avalEnergiaOrigen.PERICODI = agente.PericodiCal;
                            avalEnergiaOrigen.EMPRCODI = agente.EmprcodiCal;
                            avalEnergiaOrigen.EMPGCODI = agente.EmpgcodiCal;

                            avalEnergiaOrigen.BARRCODI = valEnergia.BARRCODI;
                            avalEnergiaOrigen.PTOMEDICODI = valEnergia.PTOMEDICODI;
                            avalEnergiaOrigen.TPTOMEDICODI = valEnergia.TPTOMEDICODI;
                            avalEnergiaOrigen.TIPOINFOCODI = Constantes.tipoEnergia;

                            avalEnergiaOrigen.VALUSUCREACION = User.Identity.Name;

                            // avalEnergiaOrigen ya tiene el valor de MEDIFECHA y LECTCODI
                            garantiaAppServicio.SaveValEnergiaEntregaOrigen(avalEnergiaOrigen);
                        }
                    }
                }
                // fin implementacion - Assetec

                // Recorro las empresas si no tienen valores de energía las omito

                // 2 Obtener los insumos porbarra
                // Por barra
                // *********
                // 2.1 Monto de Garantías por Energía
                // Si flagCalculo = 1 se calcula para el mes
                // Si flagCalculo = 0 se calcula para el trimestre 
                // La información de energia obtenida es puesta en el periodo actual
                foreach (GmmEmpresaDTO agente in agentes)
                {
                    // Validar si la empresa puedes ser procesada o ya está cerrada
                    //System.Threading.Thread.Sleep(5000);
                    // borrar toda la data de la empresa
                    garantiaAppServicio.Deletevalores(agente);

                    // Ver si es el primer mes
                    agente.EmpgPrimerMes = false;
                    if (agente.EMPGFASECAL.Equals("1")) agente.EmpgPrimerMes = true;

                    foreach (GmmValEnergiaDTO valEnergia in agente.gmmValEnergiaDTOs)
                    {
                        decimal dFactor = 1;

                        //ASSETEC 2022.02 - CONSULTAMOS POR EL VALOR DEL FACTOR DE LA TABLA cai_equisddpbarr
                        CaiEquisddpbarrDTO dtoCaiEquisddpbarr = this.servicioCalculoPorcentaje.GetByBarraNombreSddp(valEnergia.BARRCODI, valEnergia.CASDDBBARRA);
                        if (dtoCaiEquisddpbarr != null)
                        {
                            dFactor = dtoCaiEquisddpbarr.Casddbfactbarrpmpo;
                        }
                        else
                        {
                            //Salir, falta el costo marginal PMPO
                            return Json("La Barra PMPO " + valEnergia.CASDDBBARRA + " no existe. Registrar su relación en Equivalencia entre barras de costo marginal ejecutado y barras del SDDP");
                        }
                        //FIN ASSETEC
                        // Leer la Energía Actual para n, n+1, n+2
                        valEnergia.formatoEnergiaMensual = Constantes.formatoEnergiaMensual;
                        valEnergia.formatoEnergiaTrimestral = Constantes.formatoEnergiaTrimestral;
                        valEnergia.VALANIO = anio; valEnergia.VALMES = mes; valEnergia.EmpgPrimerMes = agente.EmpgPrimerMes;
                        valEnergia.EMPRCODI = agente.EmprcodiCal; valEnergia.PTOMEDICODI = valEnergia.PTOMEDICODI;

                        List<GmmValEnergiaDTO> valEnergiaOrigen = garantiaAppServicio.ListarValores96Originales(valEnergia);
                        // Grabar la Energía Actual para n, n+1, n+2
                        List<GmmValEnergiaDTO> vCostoMarginal1 = null; List<GmmValEnergiaDTO> vCostoMarginal2 = null;

                        foreach (GmmValEnergiaDTO avalEnergiaOrigen in valEnergiaOrigen)
                        {
                            //break;

                            avalEnergiaOrigen.CASDDBBARRA = valEnergia.CASDDBBARRA;
                            // Mecanismo para registro de Costo Marginal
                            Double vhorasSemanaNoEjec = 0;
                            // Total horas
                            Double vthorasB1 = 0; Double vthorasB2 = 0; Double vthorasB3 = 0;
                            Double vthorasB4 = 0; Double vthorasB0 = 0; Double vdifHoras = 0;
                            // Horas para el día actual de mediciones
                            Double vdhorasB1 = 0; Double vdhorasB2 = 0; Double vdhorasB3 = 0;
                            Double vdhorasB4 = 0; Double vdhorasB0 = 0;
                            Double vhorasDiaEjec = 0;
                            decimal vcostoMargB1 = 0; decimal vcostoMargB2 = 0; decimal vcostoMargB3 = 0;
                            decimal vcostoMargB4 = 0; decimal vcostoMargB0 = 0;
                            Double[] vConfHoras = null;
                            // Control del costor marginal *LHBN\NET-CONSULTORES*


                            vCostoMarginal1 = garantiaAppServicio.ListarValoresCostoMarginal(avalEnergiaOrigen, anio, mes);
                            // Si el dia es menor que 11 puede darse que no haya datos
                            if (vCostoMarginal1.Count == 0)
                            {
                                //continue;   //agregar codigo para que devuelva nombre de la barra qiue no ha encontrado avalEnergiaOrigen.CASDDBBARRA
                                //Salir, falta el costo marginal PMPO
                                return Json("La Barra PMPO " + valEnergia.CASDDBBARRA + " no existe. Verificar si la Barra PMPO existe en la tabla ME_MEDICIONXINTERVALO");
                            }
                            vConfHoras = programacionAppServicio.ObtenerHoras(vCostoMarginal1.First().MEDINTFECHAINI);
                            vthorasB0 = vConfHoras.ElementAt(0); vcostoMargB0 = vCostoMarginal1.ElementAt(0).MEDINTH1 * dFactor;
                            vthorasB1 = vConfHoras.ElementAt(1); vcostoMargB1 = vCostoMarginal1.ElementAt(1).MEDINTH1 * dFactor;
                            vthorasB2 = vConfHoras.ElementAt(2); vcostoMargB2 = vCostoMarginal1.ElementAt(2).MEDINTH1 * dFactor;
                            vthorasB3 = vConfHoras.ElementAt(3); vcostoMargB3 = vCostoMarginal1.ElementAt(3).MEDINTH1 * dFactor;
                            vthorasB4 = vConfHoras.ElementAt(4); vcostoMargB4 = vCostoMarginal1.ElementAt(4).MEDINTH1 * dFactor;

                            // determinar horas transcurridas desde el inicio de la semana 
                            // hasta las 00:15 del día actual en el registro de avalEnergiaOrigen
                            vdifHoras = avalEnergiaOrigen.MEDIFECHA.Value.Subtract(vCostoMarginal1.First().MEDINTFECHAINI).TotalHours;
                            vhorasSemanaNoEjec = 168 - vdifHoras;
                            // determinar intervalos que serán empleados por cada Bloque
                            // los intervalos son cada uno 15 minutos (0.25 de horas)
                            // traducir los intervalos en número entero maximo de 96 
                            // Por ejemplo debe quedar vInterB1 = 10, vInterB2 = 50, vInterB3= 36
                            // Siempre suma 96 todos los intervalos y en base a esto es que asignamos valores
                            vhorasDiaEjec = 24;
                            // hallar las distribución de horas que se van a emplear
                            #region
                            //retirar seccion
                            #endregion
                            // Asignar el costo para el registro de cada intervalo (96)
                            // p.Name;
                            // g.GetType().GetProperty(coorde1).GetValue(_g, null).ToString();

                            // Determinar si el día es feriado
                            bool esFerido = false;
                            esFerido = (new GeneralAppServicio()).EsFeriado(avalEnergiaOrigen.MEDIFECHA.Value);

                            if (avalEnergiaOrigen.MEDIFECHA.Value.DayOfWeek == DayOfWeek.Saturday
                                || avalEnergiaOrigen.MEDIFECHA.Value.DayOfWeek == DayOfWeek.Sunday)
                            {
                                esFerido = true;
                            }

                            int j = 0;
                            for (int i = 1; i <= 96; i++)
                            {
                                j = i;

                                // Primer grupo de 00:00 hasta las 08:00
                                if (i <= 32)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB4);
                                }
                                if (i > 32 && i <= 44)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB3);
                                }
                                if (i > 44 && i <= 48)
                                {
                                    if (esFerido)
                                        avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB3);
                                    else
                                        avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB1);
                                }
                                if (i > 48 && i <= 72)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB3);
                                }
                                if (i > 72 && i <= 76)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB2);
                                }
                                if (i > 76 && i <= 78)
                                {
                                    if (esFerido)
                                        avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB2);
                                    else
                                        avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB0);
                                }
                                if (i > 78 && i <= 92)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB2);
                                }
                                if (i > 92)
                                {
                                    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB4);
                                }

                                //if (vdhorasB0 > 0)
                                //{
                                //    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB0);
                                //    if (i != 0) vdhorasB0 -= 0.25;
                                //}
                                //else if (vdhorasB1 > 0)
                                //{
                                //    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB1);
                                //    if (i != 0) vdhorasB1 -= 0.25;
                                //}
                                //else if (vdhorasB2 > 0)
                                //{
                                //    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB2);
                                //    if (i != 0) vdhorasB2 -= 0.25;
                                //}
                                //else if (vdhorasB3 > 0)
                                //{
                                //    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB3);
                                //    if (i != 0) vdhorasB3 -= 0.25;
                                //}
                                //else if (vdhorasB4 > 0)
                                //{
                                //    avalEnergiaOrigen.GetType().GetProperty("VALOVALORCM" + j).SetValue(avalEnergiaOrigen, vcostoMargB4);
                                //    if (i != 0) vdhorasB4 -= 0.25;
                                //}



                            }

                            // Grabar los resultados
                            avalEnergiaOrigen.PERICODI = agente.PericodiCal;
                            avalEnergiaOrigen.EMPRCODI = agente.EmprcodiCal;
                            avalEnergiaOrigen.EMPGCODI = agente.EmpgcodiCal;

                            avalEnergiaOrigen.BARRCODI = valEnergia.BARRCODI;
                            avalEnergiaOrigen.PTOMEDICODI = valEnergia.PTOMEDICODI;
                            avalEnergiaOrigen.TPTOMEDICODI = valEnergia.TPTOMEDICODI;
                            avalEnergiaOrigen.TIPOINFOCODI = Constantes.tipoEnergia;

                            avalEnergiaOrigen.VALUSUCREACION = User.Identity.Name;

                            // avalEnergiaOrigen ya tiene el valor de MEDIFECHA y LECTCODI
                            garantiaAppServicio.SaveValEnergiaOrigen(avalEnergiaOrigen);
                        }

                        // Leer Demanda Coincidente para n, n + 1, n + 2
                        GmmDatCalculoDTO datDC = new GmmDatCalculoDTO();
                        datDC.EMPRCODI = agente.EmprcodiCal; datDC.EMPGCODI = agente.EmpgcodiCal;
                        datDC.EmpgPrimerMes = agente.EmpgPrimerMes; datDC.DCALANIO = anio; datDC.DCALMES = mes;
                        datDC.formatoDemandaMensual = Constantes.formatoDemandaMensual; datDC.formatoDemandaTrimestral = Constantes.formatoDemandaTrimestral;
                        datDC.PTOMEDICODI = valEnergia.PTOMEDICODI;
                        //datDC.TPTOMEDICODI = valEnergia.TPTOMEDICODI;
                        //datDC.TIPOINFOCODI = Constantes.tipoDemanda; 
                        List<GmmDatCalculoDTO> valDC = garantiaAppServicio.ListarValores1Originales(datDC);
                        // Grabar la  Demanda Coincidente para n, n+1, n+2
                        foreach (GmmDatCalculoDTO avalDC in valDC)
                        {
                            avalDC.TINSCODI = "DCP";
                            avalDC.PERICODI = agente.PericodiCal;
                            avalDC.EMPGCODI = agente.EmpgcodiCal;
                            avalDC.EMPRCODI = agente.EmprcodiCal;
                            avalDC.PERICODI = agente.PericodiCal;
                            avalDC.DCALUSUCREACION = User.Identity.Name;
                            garantiaAppServicio.UpsertDatCalculo(avalDC);

                        }
                        //System.Threading.Thread.Sleep(500);

                    }

                    // Datos del Calculo que son por empresa
                    // Valorización LVTEA 
                    garantiaAppServicio.SetLVTEA(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                    // Valorización MEEN10 
                    garantiaAppServicio.SetMEEN10(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                    // Potencia Firme Remunerable PFR (1 mes Pot)
                    garantiaAppServicio.SetPFR(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                    // Saldo Neto Mensual o Saldos VTP (2 mes Pot)
                    garantiaAppServicio.SetVTP(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                    // Saldo Peaje Regulado PREG
                    garantiaAppServicio.SetPREG(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                    // Valoración del Peaje MPE
                    garantiaAppServicio.SetMPE(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                    // Energía Retirada Diaria 10 primeros Días 
                    garantiaAppServicio.SetENRG10(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                    // Energía Prevista a Retirar del día 11 al último día del mes
                    garantiaAppServicio.SetENRG11(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                    // Valoración Diaria Por Servicios Complementarios e Inflexibilidad Operativa (10 primeros días) del mes VDIO10
                    garantiaAppServicio.SetVDIO10(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                    // Valorizaciones diarias por recaudación de energía reactiva (10 primeros días) del mes VDER 
                    garantiaAppServicio.SetVDER10(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);

                    //ASSETEC: Grabar valor entrega para empresas generadoras ENTREGAS
                    garantiaAppServicio.SetENTPRE(anio, mes, pericodi, User.Identity.Name, agente.EmprcodiCal);
                }

                // 2.2 Grabar datos del cálculo para todas las empresas
                // Carga de valores adicionales ingresado manualmente

                //garantiaAppServicio.SetValoresAdicionales(anio, mes, pericodi, User.Identity.Name,
                //    tipoCambio, margenReserva, totalInflex, totalExceso);
                garantiaAppServicio.SetValorTipoCambio(anio, mes, pericodi, User.Identity.Name);
                // PPO Precio Básico de Potencia
                garantiaAppServicio.SetPPO(anio, mes, pericodi, User.Identity.Name);
                // Energía total de demanda DEMANDACOES
                garantiaAppServicio.SetDemandaCOES(anio, mes, pericodi, User.Identity.Name);
                // Grabar la ejecución del respaldo
                garantiaAppServicio.SetPerimsjpaso1(pericodi, User.Identity.Name);
            }
            catch (Exception e) {
                Log.Error("GarantiasController.cs / Respaldar", e);
            }
            
            return Json(indicador);
        }

        [HttpPost]
        public JsonResult calcular(int anio, int mes)
        {
            string indicador = "1";

            try {
                // 1. Obtener las empresas agentes que son parte del procesamiento
                // Debe tener empgcodi, emprcodi, flagCalculo para saber si tiene un cálculo previo

                AgenteAppServicio agenteAppServicio = new AgenteAppServicio();

                List<GmmEmpresaDTO> agentes = agenteAppServicio.listarAgentesCalculo(anio, mes, Constantes.formatoEnergiaTrimestral, Constantes.formatoEnergiaMensual, Constantes.tipoEnergia);

                int pericodi = agentes.First().PericodiCal;

                // TODO: Validar que el periodo no esté cerrado
                string periestado = agentes.First().PERIESTADO;

                if (periestado.Equals("C"))
                {
                    return Json("2");
                }

                // Carga de valores adicionales ingresado manualmente
                //garantiaAppServicio.SetValoresAdicionales(anio, mes, pericodi, User.Identity.Name,
                //    tipoCambio, margenReserva, totalInflex, totalExceso);
                // Clean Pericodi

                // Recorro las empresas si no tienen valores de energía las omito
                foreach (GmmEmpresaDTO agente in agentes)
                {
                    // Ver si es el primer mes
                    agente.EmpgPrimerMes = false;
                    if (agente.EMPGFASECAL.Equals("1")) agente.EmpgPrimerMes = true;

                    // Cálculo de la garantía por energía "Siempre empezar por este paso"
                    garantiaAppServicio.SetGarantiaEnergia(anio, mes, pericodi, agente.EmpgPrimerMes, agente.EmprcodiCal, agente.EmpgcodiCal, User.Identity.Name);
                    // Cálculo de la garantía Potencia-Peaje
                    garantiaAppServicio.SetGarantiaPotenciaPeaje(anio, mes, pericodi, agente.EmpgPrimerMes, agente.EmprcodiCal, agente.EmpgcodiCal, User.Identity.Name);
                    // Cálculo de la garantía Servicios Complementarios
                    garantiaAppServicio.SetGarantiaServiciosComp(anio, mes, pericodi, agente.EmpgPrimerMes, agente.EmprcodiCal, agente.EmpgcodiCal, User.Identity.Name);
                    // Cálculo de la garantía Energía Reactiva
                    garantiaAppServicio.SetGarantiaEnergiaReactiva(anio, mes, pericodi, agente.EmpgPrimerMes, agente.EmprcodiCal, agente.EmpgcodiCal, User.Identity.Name);
                    // Cálculo de la garantía Inflexibilidad Operativa
                    garantiaAppServicio.SetGarantiainflexibilidadOpe(anio, mes, pericodi, agente.EmpgPrimerMes, agente.EmprcodiCal, agente.EmpgcodiCal, User.Identity.Name);
                    //  break;
                }
                // Grabar la ejecución del cálculo
                garantiaAppServicio.SetPerimsjpaso2(pericodi, User.Identity.Name);
            }
            catch (Exception e)
            {
                Log.Error("GarantiasController.cs / calcular", e);
            }
            
            return Json(indicador);
        }

        [HttpPost]
        public JsonResult publicar(int anio, int mes)
        {
            string indicador = "1";
            // TODO: Validar que el periodo no esté cerrado

            AgenteAppServicio agenteAppServicio = new AgenteAppServicio();

            List<GmmEmpresaDTO> agentes = agenteAppServicio.listarAgentesCalculo(anio, mes, Constantes.formatoEnergiaTrimestral, Constantes.formatoEnergiaMensual, Constantes.tipoEnergia);
            int pericodi = agentes.First().PericodiCal;
            string periestado = agentes.First().PERIESTADO;

            if (periestado.Equals("A"))
                periestado = "C";
            else periestado = "A";

            // Grabar la ejecución del cálculo
            garantiaAppServicio.SetPeriEstado(pericodi, periestado, User.Identity.Name);
            return Json(indicador);
        }
    }
}
