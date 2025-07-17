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

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class DemandaMercadoLibreController : Controller
    {
        //
        // GET: /Transferencias/DemandaMercadoLibre/
        public int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoTR"]);
        public int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaTR"]);
        public int IdLecturaSICLI = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaAlphaPR16"]);
        TransferenciaInformacionBaseAppServicio servicio = new TransferenciaInformacionBaseAppServicio();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarDemandas(string periodoInicial, int tipoEmpresa, string nombreEmpresa)
        {
            var anioMes = periodoInicial.Split('/');
            var fechaFinal = new DateTime(Convert.ToInt32(anioMes[1]), Convert.ToInt16(anioMes[0]), 1);
            var fechaInicial = fechaFinal.AddMonths(-11);

            DemandaMercadoLibreModel modelDemandaMercadoLibre = new DemandaMercadoLibreModel();
            
            modelDemandaMercadoLibre.PeriodosEvaluados = new DateTime[12];
            modelDemandaMercadoLibre.ListaInformacionAgentes = new List<DemandaMercadoLibreDTO>();

            for (int i = 0; i < 12; i++)
            {
                modelDemandaMercadoLibre.PeriodosEvaluados[i] = fechaInicial.AddMonths(i);
            }

            modelDemandaMercadoLibre.ListaInformacionAgentes = servicio.ListDemandaMercadoLibre(modelDemandaMercadoLibre.PeriodosEvaluados, 
                fechaFinal, tipoEmpresa, nombreEmpresa, IdLectura, Funcion.CodigoOrigenLecturaML);            

            foreach (var item in modelDemandaMercadoLibre.ListaInformacionAgentes)
            {
                item.PotenciaMaximaRetirar = item.DemandaMaxima * Funcion.dPorcentajePotenciaMaxima;
            }           

            return PartialView("ListarDemandaMercadoLibre", modelDemandaMercadoLibre);

        }
        public ActionResult ListarDemandasOsinergmin(string periodoInicial, int tipoEmpresa, string nombreEmpresa)
        {
            var anioMes = periodoInicial.Split('/');
            var fechaFinal = new DateTime(Convert.ToInt32(anioMes[1]), Convert.ToInt16(anioMes[0]), 1);
            var fechaInicial = fechaFinal.AddMonths(-11);

            DemandaMercadoLibreModel modelDemandaMercadoLibre = new DemandaMercadoLibreModel();

            modelDemandaMercadoLibre.PeriodosEvaluados = new DateTime[12];
            modelDemandaMercadoLibre.ListaInformacionAgentes = new List<DemandaMercadoLibreDTO>();

            for (int i = 0; i < 12; i++)
            {
                modelDemandaMercadoLibre.PeriodosEvaluados[i] = fechaInicial.AddMonths(i);
            }
           
            modelDemandaMercadoLibre.ListaInformacionOsinergmin = servicio.ListDemandaMercadoLibre(modelDemandaMercadoLibre.PeriodosEvaluados,
               fechaFinal, tipoEmpresa, nombreEmpresa, IdLecturaSICLI, Funcion.CodigoOrigenLecturaSICLI);

            

            foreach (var item in modelDemandaMercadoLibre.ListaInformacionOsinergmin)
            {
                item.PotenciaMaximaRetirar = item.DemandaMaxima * Funcion.dPorcentajePotenciaMaxima;
            }

            return PartialView("ListarDemandaMercadoLibreOsinergmin", modelDemandaMercadoLibre);

        }

        public static bool TieneDemandaCompleta(DemandaMercadoLibreDTO entity)
        {
            bool demandaCompleta = true;

            for (int i = 1; i <= 12; i++)
            {
                var propertyName = string.Concat("DemandaMes", i);
                var property = entity.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                if (property != null)
                {
                    if (decimal.Parse(property.GetValue(entity).ToString()) <= 0)
                    {
                        demandaCompleta = false;
                        break;
                    }
                }
            }

            return demandaCompleta;
        }

        public JsonResult GenerarReporte(string periodoInicial, int tipoEmpresa, string nombreEmpresa)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcel(periodoInicial, tipoEmpresa, nombreEmpresa);
                //indicador = 1;
            }
            catch
            {
                fileName = "-1";
            }

            return Json(fileName);
        }

        private string GenerarArchivoExcel(string periodoInicial, int tipoEmpresa, string nombreEmpresa)
        {
            //var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            var preNombre = "Demanda_Mercado_Libre_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            var anioMes = periodoInicial.Split('/');
            var fechaFinal = new DateTime(Convert.ToInt32(anioMes[1]), Convert.ToInt16(anioMes[0]), 1);
            var fechaInicial = fechaFinal.AddMonths(-11);

            DemandaMercadoLibreModel modelDemandaMercadoLibre = new DemandaMercadoLibreModel();

            modelDemandaMercadoLibre.PeriodosEvaluados = new DateTime[12];
            modelDemandaMercadoLibre.ListaInformacionAgentes = new List<DemandaMercadoLibreDTO>();

            for (int i = 0; i < 12; i++)
            {
                modelDemandaMercadoLibre.PeriodosEvaluados[i] = fechaInicial.AddMonths(i);
            }

            modelDemandaMercadoLibre.ListaInformacionAgentes = servicio.ListDemandaMercadoLibre(modelDemandaMercadoLibre.PeriodosEvaluados,
                fechaFinal, tipoEmpresa, nombreEmpresa, IdLectura, Funcion.CodigoOrigenLecturaML);
            modelDemandaMercadoLibre.ListaInformacionOsinergmin = servicio.ListDemandaMercadoLibre(modelDemandaMercadoLibre.PeriodosEvaluados,
               fechaFinal, tipoEmpresa, nombreEmpresa, IdLecturaSICLI, Funcion.CodigoOrigenLecturaSICLI);

            foreach (var item in modelDemandaMercadoLibre.ListaInformacionAgentes)
            {
                item.PotenciaMaximaRetirar = item.DemandaMaxima * Funcion.dPorcentajePotenciaMaxima;
            }

            foreach (var item in modelDemandaMercadoLibre.ListaInformacionOsinergmin)
            {
                item.PotenciaMaximaRetirar = item.DemandaMaxima * Funcion.dPorcentajePotenciaMaxima;
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Obtenemos los cuadros asociados al programa
                //var cuadroProgramas = servicio.GetByCriteriaRcaCuadroProgs(codigoPrograma.ToString(""), "");

                var contFila = 7;
                //var contHojas = 0;
                var nombreHojaAgentes = "Datos_Agentes";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);

                contFila = 2;
                var contItem = 3;
                               
                ws.Cells[contFila, 2].Value = "Empresas";
                foreach (var periodo in modelDemandaMercadoLibre.PeriodosEvaluados)
                {
                    ws.Cells[contFila, contItem].Value = periodo.ToString("MMM yy", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"));
                    contItem++;
                }

                ws.Cells[contFila, contItem].Value = "Demanda Máxima";
                ws.Cells[contFila, contItem + 1].Value = "Potencia Máxima a Retirar";
               

                ExcelRange rg1 = ws.Cells[contFila, 2, contFila, 16];
                ObtenerEstiloCelda(rg1, 1);

                contFila++;
                foreach (var registro in modelDemandaMercadoLibre.ListaInformacionAgentes)
                {
                    ws.Cells[contFila, 2].Value = registro.EmprRazSocial;
                    ws.Cells[contFila, 3].Value = registro.DemandaMes1;
                    ws.Cells[contFila, 4].Value = registro.DemandaMes2;
                    ws.Cells[contFila, 5].Value = registro.DemandaMes3;
                    ws.Cells[contFila, 6].Value = registro.DemandaMes4;
                    ws.Cells[contFila, 7].Value = registro.DemandaMes5;
                    ws.Cells[contFila, 8].Value = registro.DemandaMes6;
                    ws.Cells[contFila, 9].Value = registro.DemandaMes7;
                    ws.Cells[contFila, 10].Value = registro.DemandaMes8;
                    ws.Cells[contFila, 11].Value = registro.DemandaMes9;
                    ws.Cells[contFila, 12].Value = registro.DemandaMes10;
                    ws.Cells[contFila, 13].Value = registro.DemandaMes11;
                    ws.Cells[contFila, 14].Value = registro.DemandaMes12;
                    ws.Cells[contFila, 15].Value = registro.DemandaMaxima;
                    ws.Cells[contFila, 16].Value = registro.PotenciaMaximaRetirar;

                    contFila++;
                }                

                ws.Column(2).Width = 50;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
                ws.Column(10).Width = 20;
                ws.Column(11).Width = 20;
                ws.Column(12).Width = 20;
                ws.Column(13).Width = 20;
                ws.Column(14).Width = 20;
                ws.Column(15).Width = 30;
                ws.Column(16).Width = 30;

                
                var nombreHojaOsinergmin = "Datos_Osinergmin";
                ws = xlPackage.Workbook.Worksheets.Add(nombreHojaOsinergmin);

                contFila = 2;
                contItem = 3;

                ws.Cells[contFila, 2].Value = "Empresas";
                foreach (var periodo in modelDemandaMercadoLibre.PeriodosEvaluados)
                {
                    ws.Cells[contFila, contItem].Value = periodo.ToString("MMM yy", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"));
                    contItem++;
                }

                ws.Cells[contFila, contItem].Value = "Demanda Máxima";
                ws.Cells[contFila, contItem + 1].Value = "Potencia Máxima a Retirar";


                rg1 = ws.Cells[contFila, 2, contFila, 16];
                ObtenerEstiloCelda(rg1, 1);

                contFila++;
                foreach (var registro in modelDemandaMercadoLibre.ListaInformacionOsinergmin)
                {
                    ws.Cells[contFila, 2].Value = registro.EmprRazSocial;
                    ws.Cells[contFila, 3].Value = registro.DemandaMes1;
                    ws.Cells[contFila, 4].Value = registro.DemandaMes2;
                    ws.Cells[contFila, 5].Value = registro.DemandaMes3;
                    ws.Cells[contFila, 6].Value = registro.DemandaMes4;
                    ws.Cells[contFila, 7].Value = registro.DemandaMes5;
                    ws.Cells[contFila, 8].Value = registro.DemandaMes6;
                    ws.Cells[contFila, 9].Value = registro.DemandaMes7;
                    ws.Cells[contFila, 10].Value = registro.DemandaMes8;
                    ws.Cells[contFila, 11].Value = registro.DemandaMes9;
                    ws.Cells[contFila, 12].Value = registro.DemandaMes10;
                    ws.Cells[contFila, 13].Value = registro.DemandaMes11;
                    ws.Cells[contFila, 14].Value = registro.DemandaMes12;
                    ws.Cells[contFila, 15].Value = registro.DemandaMaxima;
                    ws.Cells[contFila, 16].Value = registro.PotenciaMaximaRetirar;

                    contFila++;
                }

                ws.Column(2).Width = 50;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
                ws.Column(10).Width = 20;
                ws.Column(11).Width = 20;
                ws.Column(12).Width = 20;
                ws.Column(13).Width = 20;
                ws.Column(14).Width = 20;
                ws.Column(15).Width = 30;
                ws.Column(16).Width = 30;

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

    }
}
