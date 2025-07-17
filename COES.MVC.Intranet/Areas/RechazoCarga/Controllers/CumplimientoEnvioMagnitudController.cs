using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.Servicios.Aplicacion.RechazoCarga;
using COES.Dominio.DTO.Sic;
using log4net;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class CumplimientoEnvioMagnitudController : BaseController
    {
        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CumplimientoEnvioMagnitudController));

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("CumplimientoEnvioMagnitudController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("CumplimientoEnvioMagnitudController", ex);
                throw;
            }
        }

        public CumplimientoEnvioMagnitudController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public ViewResult Consulta()
        {
            CumplimientoEnvioMagnitudModel model = new CumplimientoEnvioMagnitudModel();

            List<RcaSuministradorDTO> suministradores = new List<RcaSuministradorDTO>();
            suministradores.Add(new RcaSuministradorDTO { Emprcodi = 0, Emprrazsocial = "-- TODOS --" });
            
            List<RcaProgramaDTO> programas = new List<RcaProgramaDTO>();
            programas.Add(new RcaProgramaDTO { Rcprogcodi = 0, Rcprognombre = "-- SELECCIONE --" });
            var listaProgramas = servicio.ListProgramasRechazoCarga(false);
            if (listaProgramas.Any())
            {
                programas.AddRange(listaProgramas);
            }

            model.Programas = programas;
            model.Suministradores = suministradores;
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListarCumplimientoEnvioMagnitud(string programa, string cuadro, string suministrador, string cumplio)
        {
            CumplimientoEnvioMagnitudModel model = new CumplimientoEnvioMagnitudModel();

            var codigoCuadro = String.IsNullOrEmpty(cuadro) ? 0 : Convert.ToInt32(cuadro);
            var codigoSuministrador = String.IsNullOrEmpty(suministrador) ? 0 : Convert.ToInt32(suministrador);
            model.Reporte = servicio.ListEnvioArchivoMagnitud(0, codigoCuadro, codigoSuministrador, cumplio);
            return PartialView("ListarCumplimientoEnvioMagnitud", model);
        }

        /// <summary>
        /// Devuelve los cuadros asociados a un programa
        /// </summary>
        /// <param name="programa"></param>
        /// <returns></returns>
        public JsonResult ObtenerCuadrosPorPrograma(string programa)
        {
            List<RcaCuadroProgDTO> cuadros = new List<RcaCuadroProgDTO>();
            cuadros.Add(new RcaCuadroProgDTO { Rccuadcodi = 0, Rccuadmotivo = "-- SELECCIONE --" });
            if (programa == "0") return Json(new SelectList(cuadros, "Rccuadcodi", "Rccuadmotivo"));
            var cuadrosFiltrados = servicio.ListCuadroEnvioArchivoPorPrograma(Convert.ToInt32(programa));
            if (cuadrosFiltrados.Any())
            {
                cuadros.AddRange(cuadrosFiltrados);
            }
            return Json(new SelectList(cuadros, "Rccuadcodi", "Rccuadmotivo"));
        }

        /// <summary>
        /// Devuelve los suministradores asociados a un cuadro
        /// </summary>
        /// <param name="codigoCuadro"></param>
        /// <returns></returns>
        public JsonResult ObtenerSuministrador(int codigoCuadro)
        {
            List<SiEmpresaDTO> cuadros = new List<SiEmpresaDTO>();
            cuadros.Add(new SiEmpresaDTO { Emprcodi = 0, Emprrazsocial = "-- TODOS --" });
            if (codigoCuadro == 0) return Json(new SelectList(cuadros, "Emprcodi", "Emprrazsocial"));
            var cuadrosFiltrados = servicio.ListSuministrador(codigoCuadro);
            if (cuadrosFiltrados.Any())
            {
                cuadros.AddRange(cuadrosFiltrados);
            }
            return Json(new SelectList(cuadros, "Emprcodi", "Emprrazsocial"));
        }

        //Nuevos Metodos 10/02/2021
        public JsonResult GenerarReporte(string programa, string cuadro, string suministrador, string cumplio)
        {
            string fileName = "";
            try
            {                
                fileName = GenerarArchivoExcel(programa, cuadro, suministrador, cumplio);               
            }
            catch (Exception ex)
            {
                Log.Error("CumplimientoEnvioMagnitudController", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }
        private string GenerarArchivoExcel(string programa, string cuadro, string suministrador, string cumplio)
        {

            var preNombre = "Cumplimiento_Envio_" + DateTime.Now.ToString("yyyyMMddhhmmss");

            var codigoCuadro = String.IsNullOrEmpty(cuadro) ? 0 : Convert.ToInt32(cuadro);
            var codigoSuministrador = String.IsNullOrEmpty(suministrador) ? 0 : Convert.ToInt32(suministrador);
            var listReporteInformacion = servicio.ListEnvioArchivoMagnitud(0, codigoCuadro, codigoSuministrador, cumplio);           

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var nombreHoja = "REPORTE";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);

                var contFila = 3;

                ws.Cells[1, 6].Value = "Programado";
                ws.Cells[1, 7].Value = "Preliminar";
                ws.Cells[1, 10].Value = "Final";

                ws.Cells[2, 1].Value = "Suministrador";
                ws.Cells[2, 2].Value = "Cliente Libre";
                ws.Cells[2, 3].Value = "Subestación";
                ws.Cells[2, 4].Value = "Nombre Punto medición";
                ws.Cells[2, 5].Value = "Demanda en HFP (MW)";
                ws.Cells[2, 6].Value = "Rechazo de carga";
                ws.Cells[2, 7].Value = "Rechazo de carga Ejecutado";
                ws.Cells[2, 8].Value = "Fecha Hora Inicio";
                ws.Cells[2, 9].Value = "Fecha Hora Fin";
                ws.Cells[2, 10].Value = "Rechazo de carga Ejecutado";
                ws.Cells[2, 11].Value = "Fecha Hora Inicio";
                ws.Cells[2, 12].Value = "Fecha Hora Fin";
                ws.Cells[2, 13].Value = "Cumplió";

                ExcelRange rg1 = ws.Cells[2, 1, 2, 13];
                ObtenerEstiloCelda(rg1, 1);

                rg1 = ws.Cells[1, 7, 1, 9];
                rg1.Merge = true;
                //ObtenerEstiloCelda(rg1, 1);

                rg1 = ws.Cells[1, 10, 1, 12];
                rg1.Merge = true;

                rg1 = ws.Cells[1, 6, 1, 12];
                ObtenerEstiloCelda(rg1, 1);

                foreach (var registro in listReporteInformacion)
                {

                    ws.Cells[contFila, 1].Value = registro.Suministrador;
                    ws.Cells[contFila, 2].Value = registro.Empresa;
                    ws.Cells[contFila, 3].Value = registro.Subestacion;
                    ws.Cells[contFila, 4].Value = registro.Puntomedicion;
                    ws.Cells[contFila, 5].Value = registro.Rcproudemanda;
                    ws.Cells[contFila, 6].Value = registro.Rcproudemandaracionar;
                    ws.Cells[contFila, 7].Value = registro.Rcejeucargarechazadapreliminar;
                    ws.Cells[contFila, 8].Value = registro.Rcejeufechoriniciopreliminar == DateTime.MinValue ? String.Empty : registro.Rcejeufechoriniciopreliminar.ToString("dd/MM/yyyy HH:mm");
                    ws.Cells[contFila, 9].Value = registro.Rcejeufechorfinpreliminar == DateTime.MinValue ? String.Empty : registro.Rcejeufechorfinpreliminar.ToString("dd/MM/yyyy HH:mm");
                    ws.Cells[contFila, 10].Value = registro.Rcejeucargarechazada;
                    ws.Cells[contFila, 11].Value = registro.Rcejeufechorinicio == DateTime.MinValue ? String.Empty : registro.Rcejeufechorinicio.ToString("dd/MM/yyyy HH:mm");
                    ws.Cells[contFila, 12].Value = registro.Rcejeufechorfin == DateTime.MinValue ? String.Empty : registro.Rcejeufechorfin.ToString("dd/MM/yyyy HH:mm");
                    ws.Cells[contFila, 13].Value = registro.Cumplio.Equals("S") ? "Si" : "No";

                    contFila++;
                }

                ws.Column(1).Width = 50;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 30;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
                ws.Column(10).Width = 20;
                ws.Column(11).Width = 20;
                ws.Column(12).Width = 20;
                ws.Column(13).Width = 20;

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
