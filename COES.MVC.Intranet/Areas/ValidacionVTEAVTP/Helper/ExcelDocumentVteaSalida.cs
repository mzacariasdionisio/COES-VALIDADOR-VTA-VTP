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
    public static class ExcelDocumentVteaSalida
    {
        public const string ExcelExtension = ".xlsx";
        public const string FontNameCalibri = "Calibri";
        public const string CabeceraMesValorizacion = "Mes de Valorización";
        public const string CabeceraVersionValorVtp= "Versión de valorización VTEA";
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

        private static void CabeceraRetirosNegativos(ExcelWorksheet ws, string periodo, string version) {

            ws.Cells[5, 4].Value = "Análisis de Datos de Salida VTEA - RETIROS NEGATIVOS";
            ws.Cells[5, 4].Style.Font.Bold = true;
            ws.Cells[5, 4, 5, 9].Merge = true;
            ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                      

            ws.Cells[7, 2].Value = CabeceraMesValorizacion;
            ws.Cells[7, 3].Value = periodo;
            ws.Cells[8, 2].Value = CabeceraVersionValorVtp;
            ws.Cells[8, 3].Value = version;
                     
            ws.Cells[11, 2].Value = CabeceraCodigo;
            ws.Cells[11, 3].Value = CabeceraEmpresa;
            ws.Cells[11, 4].Value = CabeceraCliente;
            ws.Cells[11, 5].Value = CabeceraBarra;           


        }

        private static void EstiloColumnasRetirosNegativos(ExcelWorksheet ws) {
            ConfigurarColumnasBase(ws);
        }
        private static void DatoRetirosNegativos(ExcelWorksheet ws, RetirosNegativos item, int contFila)
        {
            ws.Cells[contFila, 2].Value = item.Codigo;
            ws.Cells[contFila, 3].Value = item.Empresa;
            ws.Cells[contFila, 4].Value = item.Cliente;
            ws.Cells[contFila, 5].Value = item.Barra;          
           
        }

        private static void CabeceraOtrasHojas(ExcelWorksheet ws, string periodo, string version, string titulo)
        {

            ws.Cells[5, 4].Value = string.Format("Análisis de Datos de Salida VTEA  - {0}", titulo.ToUpper());
            ws.Cells[5, 4].Style.Font.Bold = true;
            ws.Cells[5, 4, 5, 9].Merge = true;
            ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;                      

            ws.Cells[7, 2].Value = CabeceraMesValorizacion;
            ws.Cells[7, 3].Value = periodo;
            ws.Cells[8, 2].Value = CabeceraVersionValorVtp;
            ws.Cells[8, 3].Value = version;         

            ws.Cells[11, 2].Value = CabeceraCodigo;
            ws.Cells[11, 3].Value = CabeceraEmpresa;
            ws.Cells[11, 4].Value = CabeceraCliente;
            ws.Cells[11, 5].Value = CabeceraBarra;
            ws.Cells[11, 6].Value = "Inicio de Contrato";
            ws.Cells[11, 7].Value = "Fin de Contrato";
            ws.Cells[11, 8].Value = "Descripción";         

        }

        private static void DatoSinDeclaracion(ExcelWorksheet ws, TableHE item, int contFila)
        {
            ws.Cells[contFila, 2].Value = item.Codigo;
            ws.Cells[contFila, 3].Value = item.Empresa;
            ws.Cells[contFila, 4].Value = item.Cliente;
            ws.Cells[contFila, 5].Value = item.Barra;
            ws.Cells[contFila, 6].Value = item.InicioContrato;
            ws.Cells[contFila, 7].Value = item.FinContrato;
            ws.Cells[contFila, 8].Value = item.Descripcion;          
        }

        private static void DeclaracionesNuevas(ExcelWorksheet ws, TableEH item, int contFila)
        {
            ws.Cells[contFila, 2].Value = item.Codigo;
            ws.Cells[contFila, 3].Value = item.Empresa;
            ws.Cells[contFila, 4].Value = item.Cliente;
            ws.Cells[contFila, 5].Value = item.Barra;
            ws.Cells[contFila, 6].Value = item.InicioContrato;
            ws.Cells[contFila, 7].Value = item.FinContrato;
            ws.Cells[contFila, 8].Value = item.Descripcion;
        }

        private static void FinContrato(ExcelWorksheet ws, TableFC item, int contFila)
        {
            ws.Cells[contFila, 2].Value = item.Codigo;
            ws.Cells[contFila, 3].Value = item.Empresa;
            ws.Cells[contFila, 4].Value = item.Cliente;
            ws.Cells[contFila, 5].Value = item.Barra;
            ws.Cells[contFila, 6].Value = item.InicioContrato;
            ws.Cells[contFila, 7].Value = item.FinContrato;
            ws.Cells[contFila, 8].Value = item.Descripcion;
        }

        private static void EstiloColumnasOtrasHojas(ExcelWorksheet ws) {
            ConfigurarColumnasBase(ws);
            ws.Column(6).Width = 20;
            ws.Column(7).Width = 20;
            ws.Column(8).Width = 30;
        }

        private static void ConfigurarColumnasBase(ExcelWorksheet ws)
        {
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 30;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
        }

        public static string GenerarReporte(VteaDTO DatosVTEA, string periodo, string version, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = string.Format("Reporte_Vtea_Salida_{0}_{1}", periodo, version) + ExcelExtension;

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Retiros Negativos");
                ws.Cells.Style.Font.Name = FontNameCalibri;

                var contFila = 12;

                #region Retiros Negativos
                InsertarLogo(ws, rutaLogo);
                CabeceraRetirosNegativos(ws, periodo, version);

                ExcelRange rg1 = ws.Cells[11, 2, 11, 5];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTEA.RetirosNegativos)
                {

                    DatoRetirosNegativos(ws, item, contFila);
                    contFila++;
                }


                if (DatosVTEA.RetirosNegativos.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 5];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;

                EstiloColumnasRetirosNegativos(ws);

                #endregion


                ws = xlPackage.Workbook.Worksheets.Add("Sin Declaración");
                
                contFila = 12;

                #region Sin Declaracion

                InsertarLogo(ws, rutaLogo);

                CabeceraOtrasHojas(ws, periodo, version, "Sin Declaración");


                rg1 = ws.Cells[11, 2, 11, 8];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];

                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTEA.TableHE)
                {
                    DatoSinDeclaracion(ws, item, contFila);

                    contFila++;
                }


                if (DatosVTEA.TableHE.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 8];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;

                EstiloColumnasOtrasHojas(ws);

                #endregion


                ws = xlPackage.Workbook.Worksheets.Add("Declaraciones Nuevas");

                contFila = 12;

                #region Declaraciones Nuevas

                InsertarLogo(ws, rutaLogo);

                CabeceraOtrasHojas(ws, periodo, version, "Declaraciones Nuevas");


                rg1 = ws.Cells[11, 2, 11, 8];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];

                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTEA.TableEH)
                {
                    DeclaracionesNuevas(ws, item, contFila);

                    contFila++;
                }


                if (DatosVTEA.TableEH.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 8];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;

                EstiloColumnasOtrasHojas(ws);

                #endregion

                ws = xlPackage.Workbook.Worksheets.Add("Fin Contrato");

                contFila = 12;

                #region Fin Contrato

                InsertarLogo(ws, rutaLogo);

                CabeceraOtrasHojas(ws, periodo, version, "Fin Contrato");


                rg1 = ws.Cells[11, 2, 11, 8];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];

                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTEA.TableFC)
                {
                    FinContrato(ws, item, contFila);

                    contFila++;
                }


                if (DatosVTEA.TableHE.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 8];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;

                EstiloColumnasOtrasHojas(ws);

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