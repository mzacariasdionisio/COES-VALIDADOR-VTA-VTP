using COES.Dominio.DTO.ValidacionVTEAVTP;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.TiempoReal.Models;
using COES.Framework.Base.Tools;
using static iTextSharp.text.pdf.AcroFields;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Helper
{
    public static class ExcelDocumentVteaAnalisis
    {
        public const string ExcelExtension = ".xlsx";
        public const string FontNameCalibri = "Calibri";
        public const string CabeceraMesValorizacion = "Mes de Valorización";
        public const string CabeceraVersionValorVtea= "Versión de valorización";
        public const string CabeceraCodigo = "Código";
        public const string CabeceraEmpresa = "Empresa";
        public const string CabeceraCliente = "Cliente";
        public const string CabeceraBarra = "Barra";
      
        public const string ColorNegro = "#000000";


        private static void InsertarLogo(ExcelWorksheet ws, string rutaLogo)
        {
            if (File.Exists(rutaLogo))
            {
                Image logo = Image.FromFile(rutaLogo);
                var excelImage = ws.Drawings.AddPicture("Logo", logo);
                excelImage.SetPosition(2, 0, 1, 0);
                excelImage.SetSize(120, 60);
            }
        }

        private static void CabeceraRolEmpresa(ExcelWorksheet ws, string periodo, string version) {

            ws.Cells[5, 4].Value = "Análisis de Valorizaciones de VTEA - Rol de Empresa";
            ws.Cells[5, 4].Style.Font.Bold = true;
            ws.Cells[5, 4, 5, 9].Merge = true;
            ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                      

            ws.Cells[7, 2].Value = CabeceraMesValorizacion;
            ws.Cells[7, 3].Value = periodo;
            ws.Cells[8, 2].Value = CabeceraVersionValorVtea;
            ws.Cells[8, 3].Value = version;
                     
            ws.Cells[11, 2].Value = "Empresa";
            ws.Cells[11, 3].Value = "Cambio";
            ws.Cells[11, 4].Value = "Grupo Actual";
            ws.Cells[11, 5].Value = "Grupo Previo";
            ws.Cells[11, 6].Value = "Tasa";

        }

        private static void EstiloColumnasRolEmpresa(ExcelWorksheet ws) {
            //ConfigurarColumnasBase(ws);
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 15;
            ws.Column(4).Width = 20;
            ws.Column(5).Width = 20;
            ws.Column(6).Width = 15;
        }
        private static void DatoRolEmpresa(ExcelWorksheet ws, InfoEmpresaResumen item, int contFila)
        {
            ws.Cells[contFila, 2].Value = item.Company;
            ws.Cells[contFila, 3].Value = item.Change;
            ws.Cells[contFila, 4].Value = item.TeamNow;
            ws.Cells[contFila, 5].Value = item.TeamPrev;
            ws.Cells[contFila, 6].Value = item.Rate;
        }

        private static void CabeceraEnergiaCmg(ExcelWorksheet ws, string periodo, string version)
        {

            ws.Cells[5, 4].Value = "Análisis de Datos de Salida VTEA  - Energia y CMg";
            ws.Cells[5, 4].Style.Font.Bold = true;
            ws.Cells[5, 4, 5, 9].Merge = true;
            ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;                      

            ws.Cells[7, 2].Value = CabeceraMesValorizacion;
            ws.Cells[7, 3].Value = periodo;
            ws.Cells[8, 2].Value = CabeceraVersionValorVtea;
            ws.Cells[8, 3].Value = version;         

            ws.Cells[11, 2].Value = CabeceraCodigo;
            ws.Cells[11, 3].Value = CabeceraEmpresa;
            ws.Cells[11, 4].Value = CabeceraCliente;
            ws.Cells[11, 5].Value = CabeceraBarra;
            ws.Cells[11, 6].Value = "Tipo";
            ws.Cells[11, 7].Value = "Observado";            

        }

        private static void DatoEnergiaCmg(ExcelWorksheet ws, InfoDeclaracionResumen item, int contFila)
        {
            ws.Cells[contFila, 2].Value = item.Codigo;
            ws.Cells[contFila, 3].Value = item.Empresa;
            ws.Cells[contFila, 4].Value = item.Cliente;
            ws.Cells[contFila, 5].Value = item.Barra;
            ws.Cells[contFila, 6].Value = item.Tipo;
            ws.Cells[contFila, 7].Value = item.Observado;                   
        }

        private static void DatoEnergiaCmgDia(ExcelWorksheet ws, Tmp item, int contFila)
        {
            ws.Cells[contFila, 2].Value = item.codigo;
            ws.Cells[contFila, 3].Value = item.Empresa;
            ws.Cells[contFila, 4].Value = item.Cliente;
            ws.Cells[contFila, 5].Value = item.Barra;
            ws.Cells[contFila, 6].Value = item.Tipo;
            ws.Cells[contFila, 7].Value = item.cmg;
            ws.Cells[contFila, 8].Value = item.mwh;
        }


        private static void EstiloColumnasEnergiaCmg(ExcelWorksheet ws) {
            ConfigurarColumnasBase(ws);
            ws.Column(6).Width = 15;
            ws.Column(7).Width = 15;
            ws.Column(8).Width = 15;
        }

        private static void CabeceraEnergiaDiaCmg(ExcelWorksheet ws, string periodo, string version, string dia)
        {

            ws.Cells[5, 4].Value = string.Format("Análisis de Datos de Salida VTEA  - Energia y CMg - Dia {0}", dia);
            ws.Cells[5, 4].Style.Font.Bold = true;
            ws.Cells[5, 4, 5, 9].Merge = true;
            ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells[7, 2].Value = CabeceraMesValorizacion;
            ws.Cells[7, 3].Value = periodo;
            ws.Cells[8, 2].Value = CabeceraVersionValorVtea;
            ws.Cells[8, 3].Value = version;

            ws.Cells[11, 2].Value = CabeceraCodigo;
            ws.Cells[11, 3].Value = CabeceraEmpresa;
            ws.Cells[11, 4].Value = CabeceraCliente;
            ws.Cells[11, 5].Value = CabeceraBarra;
            ws.Cells[11, 6].Value = "Hora";
            ws.Cells[11, 7].Value = "CMg ($/MWh)";
            ws.Cells[11, 8].Value = "Energía (MWh)";
        }

        private static void ConfigurarColumnasBase(ExcelWorksheet ws)
        {
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 30;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
        }

        public static string GenerarReporteRolEmpresa(VteaValidadorDTO DatosVTEA, string periodo, string version, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = string.Format("Reporte_Vtea_RolEmpresa_{0}_{1}", periodo, version) + ExcelExtension;

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("VTEARolEmpresa");
                ws.Cells.Style.Font.Name = FontNameCalibri;

                var contFila = 12;

                #region Retiros Negativos
                InsertarLogo(ws, rutaLogo);
                CabeceraRolEmpresa(ws, periodo, version);

                ExcelRange rg1 = ws.Cells[11, 2, 11, 6];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTEA.InfoEmpresaResumen)
                {

                    DatoRolEmpresa(ws, item, contFila);
                    contFila++;
                }


                if (DatosVTEA.InfoEmpresaResumen.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 6];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;

                EstiloColumnasRolEmpresa(ws);

                #endregion
               

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteEnergia(VteaValidadorDTO DatosVTEA, string periodo, string version, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = string.Format("Reporte_Vtea_EnergiaCMg_{0}_{1}", periodo, version) + ExcelExtension;

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("VTEAEnergiaCMg");
                ws.Cells.Style.Font.Name = FontNameCalibri;

                var contFila = 12;

                #region Retiros Negativos
                InsertarLogo(ws, rutaLogo);
                CabeceraEnergiaCmg(ws, periodo, version);

                ExcelRange rg1 = ws.Cells[11, 2, 11, 7];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTEA.InfoDeclaracionResumen)
                {

                    DatoEnergiaCmg(ws, item, contFila);
                    contFila++;
                }


                if (DatosVTEA.InfoDeclaracionResumen.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 7];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;

                EstiloColumnasEnergiaCmg(ws);

                #endregion


                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteEnergiaDia(VteaDetailDTO DatosVTEA, string periodo, string version, string dia, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = string.Format("Reporte_Vtea_EnergiaCMgDia{0}_{1}_{2}", dia, periodo, version) + ExcelExtension;

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(string.Format("VTEAEnergiaCMgDia{0}", dia));
                ws.Cells.Style.Font.Name = FontNameCalibri;

                var contFila = 12;

                #region Retiros Negativos
                InsertarLogo(ws, rutaLogo);
                CabeceraEnergiaDiaCmg(ws, periodo, version, dia);

                ExcelRange rg1 = ws.Cells[11, 2, 11, 8];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTEA.Tmp)
                {

                    DatoEnergiaCmgDia(ws, item, contFila);
                    contFila++;
                }


                if (DatosVTEA.Tmp.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 8];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;

                EstiloColumnasEnergiaCmg(ws);

                #endregion


                xlPackage.Save();
            }

            return archivoExcel;
        }
        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml(ColorNegro));
                rango.Style.Font.Size = 8;
                rango.Style.Font.Bold = true;
                rango.Style.WrapText = true;
                string colorborder = ColorNegro;
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
                rango.Style.Font.Size = 10;
                string colorborder = ColorNegro;
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

            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                rango.Style.WrapText = true;
                string colorborder = ColorNegro;
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

            if (seccion == 3)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml(ColorNegro));
                rango.Style.Font.Size = 8;
                rango.Style.Font.Bold = true;
                rango.Style.WrapText = true;
                string colorborder = ColorNegro;
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }
    }
}