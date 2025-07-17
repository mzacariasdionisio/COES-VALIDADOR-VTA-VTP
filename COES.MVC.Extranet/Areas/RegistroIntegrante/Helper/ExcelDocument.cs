using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Extranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using COES.Servicios.Aplicacion.RegistroIntegrantes;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Helper
{
    public class ExcelDocumentEmpresa
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>

        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);
            }
        }

        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, string ruta)
        {
            //AddImage(ws,1, 0,ruta + Constantes.NombreLogoCoes);
            var border = ws.Cells[4, 2, 4, 7].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:K4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            ws.Cells[4, 2].Value = "CODIGO";
            ws.Cells[4, 3].Value = "ESTADO";
            ws.Cells[4, 4].Value = "TIPO";
            ws.Cells[4, 5].Value = "RUC";
            ws.Cells[4, 6].Value = "RAZON SOCIAL";
            ws.Cells[4, 7].Value = "NOMBRE EMPRESA COMERCIAL";
            ws.Cells[4, 8].Value = "REPRESENTANTE LEGAL";
            ws.Cells[4, 9].Value = "CORREO ELECTRÓNICO";
            ws.Cells[4, 10].Value = "TELÉFONO";
            ws.Cells[4, 11].Value = "CONDICIÓN";

            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        public static void GernerarArchivoEnvios(List<SiEmpresaDTO> list, string ruta, string titulo)
        {

            FileInfo newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);
            }
            int row = 5;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Cells[1, 3].Value = titulo;
                ws.Cells[2, 2].Value = "FECHA:";
                ws.Cells[2, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws, ruta);

                foreach (var reg in list)
                {
                    //RpteCorreoElectronico
                    //RpteTelefono

                    ws.Cells[row, column].Value = reg.Emprcodi;
                    ws.Cells[row, column + 1].Value = reg.Emprestado == null ? "" : reg.Emprestado.ToString();
                    ws.Cells[row, column + 1].AutoFitColumns();
                    ws.Cells[row, column + 2].Value = reg.Tipoemprdesc == null ? "" : reg.Tipoemprdesc.ToString();
                    ws.Cells[row, column + 2].AutoFitColumns();
                    ws.Cells[row, column + 3].Value = reg.Emprruc == null ? "" : reg.Emprruc.ToString();
                    ws.Cells[row, column + 3].AutoFitColumns();
                    ws.Cells[row, column + 4].Value = reg.Emprrazsocial == null ? "" : reg.Emprrazsocial.ToString();
                    ws.Cells[row, column + 4].AutoFitColumns();
                    ws.Cells[row, column + 5].Value = reg.Emprnombrecomercial == null ? "" : reg.Emprnombrecomercial.ToString();
                    ws.Cells[row, column + 5].AutoFitColumns();
                    ws.Cells[row, column + 6].Value = reg.RpteNombres == null ? "" : reg.RpteNombres.ToString();
                    ws.Cells[row, column + 6].AutoFitColumns();
                    ws.Cells[row, column + 7].Value = reg.RpteCorreoElectronico == null ? "" : reg.RpteCorreoElectronico.ToString();
                    ws.Cells[row, column + 7].AutoFitColumns();
                    ws.Cells[row, column + 8].Value = reg.RpteTelefono == null ? "" : reg.RpteTelefono.ToString();
                    ws.Cells[row, column + 8].AutoFitColumns();
                    ws.Cells[row, column + 9].Value = reg.Modalidad == null ? "" : reg.Modalidad.ToString();
                    ws.Cells[row, column + 9].AutoFitColumns();

                    var border = ws.Cells[row, 2, row, 11].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 11].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }

                xlPackage.Save();
            }
        }
    }

    public class ExcelDocumentEvolucionEmpresa
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>

        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);
            }
        }

        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, string ruta)
        {
            //AddImage(ws,1, 0,ruta + Constantes.NombreLogoCoes);
            var border = ws.Cells[4, 2, 4, 7].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:N4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            ws.Cells[4, 2].Value = "CÓDIGO";
            ws.Cells[4, 3].Value = "ESTADO";
            ws.Cells[4, 4].Value = "TIPO";
            ws.Cells[4, 5].Value = "RUC";
            ws.Cells[4, 6].Value = "RAZÓN SOCIAL";
            ws.Cells[4, 7].Value = "NOMBRE EMPRESA COMERCIAL";
            ws.Cells[4, 8].Value = "REPRESENTANTE LEGAL";
            ws.Cells[4, 9].Value = "CORREO ELECTRÓNICO";
            ws.Cells[4, 10].Value = "TELÉFONO";
            ws.Cells[4, 11].Value = "SOLICITUD";
            ws.Cells[4, 12].Value = "ESTADO SOLICITUD";
            ws.Cells[4, 13].Value = "FECHA SOLICITUD";
            ws.Cells[4, 14].Value = "FECHA ENVIADO";

            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        public static void GernerarArchivoEnvios(List<SiEmpresaDTO> list, string ruta, string titulo)
        {

            FileInfo newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);
            }
            int row = 5;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Cells[1, 3].Value = titulo;
                ws.Cells[2, 2].Value = "FECHA:";
                ws.Cells[2, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws, ruta);

                foreach (var reg in list)
                {
                    //RpteCorreoElectronico
                    //RpteTelefono

                    ws.Cells[row, column].Value = reg.Emprcodi;
                    ws.Cells[row, column + 1].Value = reg.Emprestado == null ? "" : reg.Emprestado.ToString();
                    ws.Cells[row, column + 1].AutoFitColumns();
                    ws.Cells[row, column + 2].Value = reg.Tipoemprdesc == null ? "" : reg.Tipoemprdesc.ToString();
                    ws.Cells[row, column + 2].AutoFitColumns();
                    ws.Cells[row, column + 3].Value = reg.Emprruc == null ? "" : reg.Emprruc.ToString();
                    ws.Cells[row, column + 3].AutoFitColumns();
                    ws.Cells[row, column + 4].Value = reg.Emprrazsocial == null ? "" : reg.Emprrazsocial.ToString();
                    ws.Cells[row, column + 4].AutoFitColumns();
                    ws.Cells[row, column + 5].Value = reg.Emprnombrecomercial == null ? "" : reg.Emprnombrecomercial.ToString();
                    ws.Cells[row, column + 5].AutoFitColumns();
                    ws.Cells[row, column + 6].Value = reg.RpteNombres == null ? "" : reg.RpteNombres.ToString();
                    ws.Cells[row, column + 6].AutoFitColumns();
                    ws.Cells[row, column + 7].Value = reg.RpteCorreoElectronico == null ? "" : reg.RpteCorreoElectronico.ToString();
                    ws.Cells[row, column + 7].AutoFitColumns();
                    ws.Cells[row, column + 8].Value = reg.RpteTelefono == null ? "" : reg.RpteTelefono.ToString();
                    ws.Cells[row, column + 8].AutoFitColumns();

                    ws.Cells[row, column + 9].Value = reg.TisoNombre == null ? "" : reg.TisoNombre.ToString();
                    ws.Cells[row, column + 9].AutoFitColumns();

                    ws.Cells[row, column + 10].Value = reg.SoliEstado == null ? "" : reg.SoliEstado.ToString();
                    ws.Cells[row, column + 10].AutoFitColumns();

                    ws.Cells[row, column + 11].Value = reg.SoliFecSolicitud == null ? "" : reg.SoliFecSolicitud.ToString();
                    ws.Cells[row, column + 11].AutoFitColumns();

                    ws.Cells[row, column + 12].Value = reg.SoliFecEnviado == null ? "" : reg.SoliFecEnviado.ToString();
                    ws.Cells[row, column + 12].AutoFitColumns();


                    var border = ws.Cells[row, 2, row, 14].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 14].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }

                xlPackage.Save();
            }
        }
    }

    public class ExcelDocumentRepresentantesLegales
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>

        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);
            }
        }

        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, string ruta)
        {
            //AddImage(ws,1, 0,ruta + Constantes.NombreLogoCoes);
            var border = ws.Cells[4, 2, 4, 7].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:H4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            //ws.Cells[4, 2].Value = "CODIGO";            
            //ws.Cells[4, 3].Value = "TIPO";
            ws.Cells[4, 2].Value = "RUC";
            ws.Cells[4, 3].Value = "RAZON SOCIAL";
            ws.Cells[4, 4].Value = "NOMBRE EMPRESA COMERCIAL";
            ws.Cells[4, 5].Value = "REPRESENTANTE LEGAL";
            ws.Cells[4, 6].Value = "CORREO ELECTRÓNICO";
            ws.Cells[4, 7].Value = "TELÉFONO";
            ws.Cells[4, 8].Value = "CELULAR";


            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        public static void GernerarArchivoEnvios(List<SiEmpresaDTO> list, string ruta, string titulo)
        {

            FileInfo newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);
            }
            int row = 5;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Cells[1, 3].Value = titulo;
                ws.Cells[2, 2].Value = "FECHA:";
                ws.Cells[2, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws, ruta);

                foreach (var reg in list)
                {
                    //RpteCorreoElectronico
                    //RpteTelefono

                    //ws.Cells[row, column].Value = reg.Emprcodi;   
                    //ws.Cells[row, column + 1].Value = reg.Tipoemprdesc == null ? "" : reg.Tipoemprdesc.ToString();
                    //ws.Cells[row, column + 1].AutoFitColumns();
                    ws.Cells[row, column].Value = reg.Emprruc == null ? "" : reg.Emprruc.ToString();
                    ws.Cells[row, column].AutoFitColumns();
                    ws.Cells[row, column + 1].Value = reg.Emprrazsocial == null ? "" : reg.Emprrazsocial.ToString();
                    ws.Cells[row, column + 1].AutoFitColumns();
                    ws.Cells[row, column + 2].Value = reg.Emprnombrecomercial == null ? "" : reg.Emprnombrecomercial.ToString();
                    ws.Cells[row, column + 2].AutoFitColumns();
                    ws.Cells[row, column + 3].Value = reg.RpteNombres == null ? "" : reg.RpteNombres.ToString();
                    ws.Cells[row, column + 3].AutoFitColumns();
                    ws.Cells[row, column + 4].Value = reg.RpteCorreoElectronico == null ? "" : reg.RpteCorreoElectronico.ToString();
                    ws.Cells[row, column + 4].AutoFitColumns();
                    ws.Cells[row, column + 5].Value = reg.RpteTelefono == null ? "" : reg.RpteTelefono.ToString();
                    ws.Cells[row, column + 5].AutoFitColumns();
                    ws.Cells[row, column + 6].Value = reg.RpteTelfMovil == null ? "" : reg.RpteTelfMovil.ToString();
                    ws.Cells[row, column + 6].AutoFitColumns();

                    var border = ws.Cells[row, 2, row, 8].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 8].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }

                xlPackage.Save();
            }
        }
    }

    public class ExcelDocumentHistoricoSolicitudes
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>

        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);
            }
        }

        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, string ruta)
        {
            //AddImage(ws,1, 0,ruta + Constantes.NombreLogoCoes);
            var border = ws.Cells[4, 2, 4, 7].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:N4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            ws.Cells[4, 2].Value = "CÓDIGO";
            ws.Cells[4, 3].Value = "ESTADO";
            ws.Cells[4, 4].Value = "TIPO";
            ws.Cells[4, 5].Value = "RUC";
            ws.Cells[4, 6].Value = "RAZÓN SOCIAL";
            ws.Cells[4, 7].Value = "NOMBRE EMPRESA COMERCIAL";
            ws.Cells[4, 8].Value = "REPRESENTANTE LEGAL";
            ws.Cells[4, 9].Value = "CORREO ELECTRÓNICO";
            ws.Cells[4, 10].Value = "TELÉFONO";
            ws.Cells[4, 11].Value = "SOLICITUD";
            ws.Cells[4, 12].Value = "ESTADO SOLICITUD";
            ws.Cells[4, 13].Value = "FECHA SOLICITUD";
            ws.Cells[4, 14].Value = "FECHA ENVIADO";

            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
        }


        public static void ConfiguracionHojaExcelSolicitud(ExcelWorksheet ws, string ruta)
        {
            //AddImage(ws,1, 0,ruta + Constantes.NombreLogoCoes);
            var border = ws.Cells[4, 2, 4, 7].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:L4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            ws.Cells[4, 2].Value = "CÓDIGO";
            ws.Cells[4, 3].Value = "ESTADO";
            ws.Cells[4, 4].Value = "TIPO";
            ws.Cells[4, 5].Value = "RUC";
            ws.Cells[4, 6].Value = "RAZÓN SOCIAL";
            ws.Cells[4, 7].Value = "NOMBRE EMPRESA COMERCIAL";
            ws.Cells[4, 8].Value = "RESPONSABLE";     
            ws.Cells[4, 9].Value = "SOLICITUD";
            ws.Cells[4, 10].Value = "ESTADO SOLICITUD";
            ws.Cells[4, 11].Value = "FECHA SOLICITUD";
            ws.Cells[4, 12].Value = "FECHA ENVIADO";

            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        public static void GernerarArchivoEnvios(List<SiEmpresaDTO> list, string ruta, string titulo)
        {

            FileInfo newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);
            }
            int row = 5;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Cells[1, 3].Value = titulo;
                ws.Cells[2, 2].Value = "FECHA:";
                ws.Cells[2, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcelSolicitud(ws, ruta);

                foreach (var reg in list)
                {
                    //RpteCorreoElectronico
                    //RpteTelefono

                    ws.Cells[row, column].Value = reg.Emprcodi;
                    ws.Cells[row, column + 1].Value = reg.Emprestado == null ? "" : reg.Emprestado.ToString();
                    ws.Cells[row, column + 1].AutoFitColumns();
                    ws.Cells[row, column + 2].Value = reg.Tipoemprdesc == null ? "" : reg.Tipoemprdesc.ToString();
                    ws.Cells[row, column + 2].AutoFitColumns();
                    ws.Cells[row, column + 3].Value = reg.Emprruc == null ? "" : reg.Emprruc.ToString();
                    ws.Cells[row, column + 3].AutoFitColumns();
                    ws.Cells[row, column + 4].Value = reg.Emprrazsocial == null ? "" : reg.Emprrazsocial.ToString();
                    ws.Cells[row, column + 4].AutoFitColumns();
                    ws.Cells[row, column + 5].Value = reg.Emprnombrecomercial == null ? "" : reg.Emprnombrecomercial.ToString();
                    ws.Cells[row, column + 5].AutoFitColumns();
                    ws.Cells[row, column + 6].Value = reg.RpteNombres == null ? "" : reg.RpteNombres.ToString();
                    ws.Cells[row, column + 6].AutoFitColumns();      

                    ws.Cells[row, column + 7].Value = reg.TisoNombre == null ? "" : reg.TisoNombre.ToString();
                    ws.Cells[row, column + 7].AutoFitColumns();

                    ws.Cells[row, column + 8].Value = reg.SoliEstado == null ? "" : reg.SoliEstado.ToString();
                    ws.Cells[row, column + 8].AutoFitColumns();

                    ws.Cells[row, column + 9].Value = reg.SoliFecSolicitud == null ? "" : reg.SoliFecSolicitud.ToString();
                    ws.Cells[row, column + 9].AutoFitColumns();

                    ws.Cells[row, column + 10].Value = reg.SoliFecEnviado == null ? "" : reg.SoliFecEnviado.ToString();
                    ws.Cells[row, column + 10].AutoFitColumns();


                    var border = ws.Cells[row, 2, row, 10].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 10].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }

                xlPackage.Save();
            }
        }
    }

    public class ExcelDocumentHistoricoRevisiones
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>

        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);
            }
        }

        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, string ruta)
        {
            //AddImage(ws,1, 0,ruta + Constantes.NombreLogoCoes);
            var border = ws.Cells[4, 2, 4, 7].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:M4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            ws.Cells[4, 2].Value = "CODIGO";
            ws.Cells[4, 3].Value = "ESTADO";
            ws.Cells[4, 4].Value = "TIPO";
            ws.Cells[4, 5].Value = "RUC";
            ws.Cells[4, 6].Value = "RAZÓN SOCIAL";
            ws.Cells[4, 7].Value = "NOMBRE EMPRESA COMERCIAL";
            ws.Cells[4, 8].Value = "ITERACIÓN";
            ws.Cells[4, 9].Value = "REVISIÓN";
            ws.Cells[4, 10].Value = "FECHA CREACIÓN";
            ws.Cells[4, 11].Value = "FECHA REVISIÓN";
            ws.Cells[4, 12].Value = "ESTADO REVISIÓN";
            ws.Cells[4, 13].Value = "DÍAS";
            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        public static void GernerarArchivoEnvios(List<SiEmpresaDTO> list, string ruta, string titulo)
        {

            FileInfo newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);
            }
            int row = 5;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Cells[1, 3].Value = titulo;
                ws.Cells[2, 2].Value = "FECHA:";
                ws.Cells[2, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws, ruta);

                foreach (var reg in list)
                {
                    //RpteCorreoElectronico
                    //RpteTelefono

                    ws.Cells[row, column].Value = reg.Emprcodi;
                    ws.Cells[row, column + 1].Value = reg.Emprestado == null ? "" : reg.Emprestado.ToString();
                    ws.Cells[row, column + 1].AutoFitColumns();
                    ws.Cells[row, column + 2].Value = reg.Tipoemprdesc == null ? "" : reg.Tipoemprdesc.ToString();
                    ws.Cells[row, column + 2].AutoFitColumns();
                    ws.Cells[row, column + 3].Value = reg.Emprruc == null ? "" : reg.Emprruc.ToString();
                    ws.Cells[row, column + 3].AutoFitColumns();
                    ws.Cells[row, column + 4].Value = reg.Emprrazsocial == null ? "" : reg.Emprrazsocial.ToString();
                    ws.Cells[row, column + 4].AutoFitColumns();
                    ws.Cells[row, column + 5].Value = reg.Emprnombrecomercial == null ? "" : reg.Emprnombrecomercial.ToString();
                    ws.Cells[row, column + 5].AutoFitColumns();
                    ws.Cells[row, column + 6].Value = reg.reviiteracion == null ? "" : reg.reviiteracion.ToString();
                    ws.Cells[row, column + 6].AutoFitColumns();
                    ws.Cells[row, column + 7].Value = reg.tiporevision == null ? "" : reg.tiporevision.ToString();
                    ws.Cells[row, column + 7].AutoFitColumns();
                    ws.Cells[row, column + 8].Value = reg.revifeccreacion == null ? "" : reg.revifeccreacion.ToString();
                    ws.Cells[row, column + 8].AutoFitColumns();
                    ws.Cells[row, column + 9].Value = reg.revifecrevision == null ? "" : reg.revifecrevision.ToString();
                    ws.Cells[row, column + 9].AutoFitColumns();
                    ws.Cells[row, column + 10].Value = reg.reviestado == null ? "" : reg.reviestado.ToString();
                    ws.Cells[row, column + 10].AutoFitColumns();
                    ws.Cells[row, column + 11].Value = reg.hora == null ? "" : reg.hora.ToString();
                    ws.Cells[row, column + 11].AutoFitColumns();


                    var border = ws.Cells[row, 2, row, 13].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 13].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }

                xlPackage.Save();
            }
        }
    }

    public class ExcelDocumentHistoricoModificaciones
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>

        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);
            }
        }

        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, string ruta)
        {
            //AddImage(ws,1, 0,ruta + Constantes.NombreLogoCoes);
            var border = ws.Cells[4, 2, 4, 7].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:J4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            ws.Cells[4, 2].Value = "CODIGO";
            ws.Cells[4, 3].Value = "ESTADO";
            ws.Cells[4, 4].Value = "TIPO";
            ws.Cells[4, 5].Value = "RUC";
            ws.Cells[4, 6].Value = "RAZON SOCIAL";
            ws.Cells[4, 7].Value = "NOMBRE EMPRESA COMERCIAL";
            ws.Cells[4, 8].Value = "REPRESENTANTE LEGAL";
            ws.Cells[4, 9].Value = "TEEFONO";
            ws.Cells[4, 10].Value = "CORREO ELCTRONICO";
            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        public static void GernerarArchivoEnvios(List<SiEmpresaDTO> list, string ruta, string titulo)
        {

            FileInfo newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);
            }
            int row = 5;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Cells[1, 3].Value = titulo;
                ws.Cells[2, 2].Value = "FECHA:";
                ws.Cells[2, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws, ruta);

                foreach (var reg in list)
                {
                    //RpteCorreoElectronico
                    //RpteTelefono

                    ws.Cells[row, column].Value = reg.Emprcodi;
                    ws.Cells[row, column + 1].Value = reg.Emprestado == null ? "" : reg.Emprestado.ToString();
                    ws.Cells[row, column + 1].AutoFitColumns();
                    ws.Cells[row, column + 2].Value = reg.Tipoemprdesc == null ? "" : reg.Tipoemprdesc.ToString();
                    ws.Cells[row, column + 2].AutoFitColumns();
                    ws.Cells[row, column + 3].Value = reg.Emprruc == null ? "" : reg.Emprruc.ToString();
                    ws.Cells[row, column + 3].AutoFitColumns();
                    ws.Cells[row, column + 4].Value = reg.Emprrazsocial == null ? "" : reg.Emprrazsocial.ToString();
                    ws.Cells[row, column + 4].AutoFitColumns();
                    ws.Cells[row, column + 5].Value = reg.Emprnombrecomercial == null ? "" : reg.Emprnombrecomercial.ToString();
                    ws.Cells[row, column + 5].AutoFitColumns();
                    ws.Cells[row, column + 6].Value = reg.RpteNombres == null ? "" : reg.RpteNombres.ToString();
                    ws.Cells[row, column + 6].AutoFitColumns();
                    ws.Cells[row, column + 7].Value = reg.RpteTelefono == null ? "" : reg.RpteTelefono.ToString();
                    ws.Cells[row, column + 7].AutoFitColumns();
                    ws.Cells[row, column + 8].Value = reg.RpteCorreoElectronico == null ? "" : reg.RpteCorreoElectronico.ToString();
                    ws.Cells[row, column + 8].AutoFitColumns();

                    var border = ws.Cells[row, 2, row, 10].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 10].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }

                xlPackage.Save();
            }
        }
    }

    public class ExcelDocumentTiemposProceso
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>

        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);
            }
        }

        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, string ruta)
        {
            //AddImage(ws,1, 0,ruta + Constantes.NombreLogoCoes);
            var border = ws.Cells[4, 2, 4, 7].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:M4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            ws.Cells[4, 2].Value = "CODIGO";
            ws.Cells[4, 3].Value = "ESTADO";
            ws.Cells[4, 4].Value = "TIPO";
            ws.Cells[4, 5].Value = "RUC";
            ws.Cells[4, 6].Value = "RAZÓN SOCIAL";
            ws.Cells[4, 7].Value = "NOMBRE EMPRESA COMERCIAL";
            ws.Cells[4, 8].Value = "ITERACIÓN";
            ws.Cells[4, 9].Value = "REVISIÓN";
            ws.Cells[4, 10].Value = "FECHA CREACIÓN";
            ws.Cells[4, 11].Value = "FECHA REVISIÓN";
            ws.Cells[4, 12].Value = "ESTADO REVISIÓN";
            ws.Cells[4, 13].Value = "DÍAS";
            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        public static void GernerarArchivoEnvios(List<SiEmpresaDTO> list, string ruta, string titulo)
        {

            FileInfo newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesRegistroIntegrantes.NombreReporteEnvios);
            }
            int row = 5;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Cells[1, 3].Value = titulo;
                ws.Cells[2, 2].Value = "FECHA:";
                ws.Cells[2, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws, ruta);

                foreach (var reg in list)
                {
                    //RpteCorreoElectronico
                    //RpteTelefono

                    ws.Cells[row, column].Value = reg.Emprcodi;
                    ws.Cells[row, column + 1].Value = reg.Emprestado == null ? "" : reg.Emprestado.ToString();
                    ws.Cells[row, column + 1].AutoFitColumns();
                    ws.Cells[row, column + 2].Value = reg.Tipoemprdesc == null ? "" : reg.Tipoemprdesc.ToString();
                    ws.Cells[row, column + 2].AutoFitColumns();
                    ws.Cells[row, column + 3].Value = reg.Emprruc == null ? "" : reg.Emprruc.ToString();
                    ws.Cells[row, column + 3].AutoFitColumns();
                    ws.Cells[row, column + 4].Value = reg.Emprrazsocial == null ? "" : reg.Emprrazsocial.ToString();
                    ws.Cells[row, column + 4].AutoFitColumns();
                    ws.Cells[row, column + 5].Value = reg.Emprnombrecomercial == null ? "" : reg.Emprnombrecomercial.ToString();
                    ws.Cells[row, column + 5].AutoFitColumns();
                    ws.Cells[row, column + 6].Value = reg.reviiteracion == null ? "" : reg.reviiteracion.ToString();
                    ws.Cells[row, column + 6].AutoFitColumns();
                    ws.Cells[row, column + 7].Value = reg.tiporevision == null ? "" : reg.tiporevision.ToString();
                    ws.Cells[row, column + 7].AutoFitColumns();
                    ws.Cells[row, column + 8].Value = reg.revifeccreacion == null ? "" : reg.revifeccreacion.ToString();
                    ws.Cells[row, column + 8].AutoFitColumns();
                    ws.Cells[row, column + 9].Value = reg.revifecrevision == null ? "" : reg.revifecrevision.ToString();
                    ws.Cells[row, column + 9].AutoFitColumns();
                    ws.Cells[row, column + 10].Value = reg.reviestado == null ? "" : reg.reviestado.ToString();
                    ws.Cells[row, column + 10].AutoFitColumns();
                    ws.Cells[row, column + 11].Value = reg.hora == null ? "" : reg.hora.ToString();
                    ws.Cells[row, column + 11].AutoFitColumns();


                    var border = ws.Cells[row, 2, row, 13].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 13].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }

                xlPackage.Save();
            }
        }
    }
}