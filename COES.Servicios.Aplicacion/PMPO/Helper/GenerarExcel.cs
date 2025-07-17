using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.PMPO.Helper
{
    public class GenerarExcel
    {
        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
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

            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));
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



            return rango;
        }

        public static byte[] Exportar(List<PmoDatPmhiTrDTO> lista, string tipo)
        {
            string titulo = string.Empty;
            if (tipo.Equals("H"))// NET 20190307 - Correción de impresión de título del reporte
                titulo = "Archivos de Mantenimiento - Hidráulico";
            else
                titulo = "Archivos de Mantenimiento - Térmico";

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws.Protection.IsProtected = true;
                if (ws != null)
                {
                    //TITULO
                    int fila = 2;
                    int colum = 5;
                    ws.Cells[fila, colum].Value = titulo;
                    ExcelRange rg = ws.Cells[fila, colum, fila + 1, colum + 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //CABECERA DE TABLA
                    fila = 5;
                    ws.Cells[fila, 7].Value = "SEMANA";
                    ws.Cells[fila, 7, fila, 59].Merge = true;
                    ws.Cells[fila + 1, 2].Value = "";
                    ws.Cells[fila + 1, 3].Value = "";
                    ws.Cells[fila + 1, 4].Value = "Código SDDP";
                    ws.Cells[fila + 1, 5].Value = "Grupo";
                    ws.Cells[fila + 1, 6].Value = "Año";
                    //ws.Cells[fila + 1, 5].Value = "1";

                    for (var x = 1; x < 54; x++)
                        ws.Cells[fila + 1, 6 + x].Value = "" + x + "";

                    rg = ws.Cells[fila, 2, fila + 1, 59];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    fila = 7;
                    foreach (var item in lista)
                    {
                        ws.Cells[fila, 2].Value = item.PmPmhtCodi;
                        ws.Cells[fila, 3].Value = item.Sddpcodi;
                        ws.Cells[fila, 4].Value = item.Sddpnum;
                        ws.Cells[fila, 5].Value = item.Sddpnomb;
                        ws.Cells[fila, 6].Value = item.PmPmhtAnhio;
                        ws.Cells[fila, 7].Value = item.PmPmhtDisp01;
                        ws.Cells[fila, 8].Value = item.PmPmhtDisp02;
                        ws.Cells[fila, 9].Value = item.PmPmhtDisp03;
                        ws.Cells[fila, 10].Value = item.PmPmhtDisp04;
                        ws.Cells[fila, 11].Value = item.PmPmhtDisp05;
                        ws.Cells[fila, 12].Value = item.PmPmhtDisp06;
                        ws.Cells[fila, 13].Value = item.PmPmhtDisp07;
                        ws.Cells[fila, 14].Value = item.PmPmhtDisp08;
                        ws.Cells[fila, 15].Value = item.PmPmhtDisp09;
                        ws.Cells[fila, 16].Value = item.PmPmhtDisp10;
                        ws.Cells[fila, 17].Value = item.PmPmhtDisp11;
                        ws.Cells[fila, 18].Value = item.PmPmhtDisp12;
                        ws.Cells[fila, 19].Value = item.PmPmhtDisp13;
                        ws.Cells[fila, 20].Value = item.PmPmhtDisp14;
                        ws.Cells[fila, 21].Value = item.PmPmhtDisp15;
                        ws.Cells[fila, 22].Value = item.PmPmhtDisp16;
                        ws.Cells[fila, 23].Value = item.PmPmhtDisp17;
                        ws.Cells[fila, 24].Value = item.PmPmhtDisp18;
                        ws.Cells[fila, 25].Value = item.PmPmhtDisp19;
                        ws.Cells[fila, 26].Value = item.PmPmhtDisp20;
                        ws.Cells[fila, 27].Value = item.PmPmhtDisp21;
                        ws.Cells[fila, 28].Value = item.PmPmhtDisp22;
                        ws.Cells[fila, 29].Value = item.PmPmhtDisp23;
                        ws.Cells[fila, 30].Value = item.PmPmhtDisp24;
                        ws.Cells[fila, 31].Value = item.PmPmhtDisp25;
                        ws.Cells[fila, 32].Value = item.PmPmhtDisp26;
                        ws.Cells[fila, 33].Value = item.PmPmhtDisp27;
                        ws.Cells[fila, 34].Value = item.PmPmhtDisp28;
                        ws.Cells[fila, 35].Value = item.PmPmhtDisp29;
                        ws.Cells[fila, 36].Value = item.PmPmhtDisp30;
                        ws.Cells[fila, 37].Value = item.PmPmhtDisp31;
                        ws.Cells[fila, 38].Value = item.PmPmhtDisp32;
                        ws.Cells[fila, 39].Value = item.PmPmhtDisp33;
                        ws.Cells[fila, 40].Value = item.PmPmhtDisp34;
                        ws.Cells[fila, 41].Value = item.PmPmhtDisp35;
                        ws.Cells[fila, 42].Value = item.PmPmhtDisp36;
                        ws.Cells[fila, 43].Value = item.PmPmhtDisp37;
                        ws.Cells[fila, 44].Value = item.PmPmhtDisp38;
                        ws.Cells[fila, 45].Value = item.PmPmhtDisp39;
                        ws.Cells[fila, 46].Value = item.PmPmhtDisp40;
                        ws.Cells[fila, 47].Value = item.PmPmhtDisp41;
                        ws.Cells[fila, 48].Value = item.PmPmhtDisp42;
                        ws.Cells[fila, 49].Value = item.PmPmhtDisp43;
                        ws.Cells[fila, 50].Value = item.PmPmhtDisp44;
                        ws.Cells[fila, 51].Value = item.PmPmhtDisp45;
                        ws.Cells[fila, 52].Value = item.PmPmhtDisp46;
                        ws.Cells[fila, 53].Value = item.PmPmhtDisp47;
                        ws.Cells[fila, 54].Value = item.PmPmhtDisp48;
                        ws.Cells[fila, 55].Value = item.PmPmhtDisp49;
                        ws.Cells[fila, 56].Value = item.PmPmhtDisp50;
                        ws.Cells[fila, 57].Value = item.PmPmhtDisp51;
                        ws.Cells[fila, 58].Value = item.PmPmhtDisp52;
                        ws.Cells[fila, 59].Value = item.PmPmhtDisp53;

                        //Border por celda
                        rg = ws.Cells[fila, 2, fila, 59];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[fila, 5, fila, 59];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg = ws.Cells[fila, 2, fila, 6];
                        rg = ObtenerEstiloCelda(rg, 2);
                        fila++;
                    }


                    ws.Column(2).Width = 0;
                    ws.Column(3).Width = 0;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;

                    for (var row = 7; row < lista.Count + 7; row++)
                    {
                        for (var column = 7; column <= 59; column++)
                        {
                            ws.Cells[row, column].Style.Locked = false;
                        }
                    }


                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                return xlPackage.GetAsByteArray();
            }
            //return excel;
        }

        public static byte[] ExportarDbf(List<PmoDatDbfDTO> lista)
        {
            string titulo = "Demanda por Barra SDDP";

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws.Protection.IsProtected = true;//NET 20190307 - Corrección de exportación Excel
                if (ws != null)
                {
                    //TITULO
                    int fila = 2;
                    int colum = 3;
                    ws.Cells[fila, colum].Value = titulo;
                    ExcelRange rg = ws.Cells[fila, colum, fila, colum + 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //CABECERA DE TABLA
                    fila = 5;
                    ws.Cells[fila, 2].Value = "BCod - ..Bus.Name..";
                    //ws.Cells[fila, 7, fila, 59].Merge = true;
                    ws.Cells[fila, 3].Value = "LCod";
                    ws.Cells[fila, 4].Value = "dd/mm/yyyy";
                    ws.Cells[fila, 5].Value = "Llev";
                    ws.Cells[fila, 6].Value = "..Load..";
                    ws.Cells[fila, 7].Value = "Icca";
                    ws.Cells[fila, 8].Value = "Nro. Semanas";
                    //ws.Cells[fila + 1, 5].Value = "1";

                    //for (var x = 1; x < 54; x++)
                    //    ws.Cells[fila + 1, 6 + x].Value = "" + x + "";

                    rg = ws.Cells[fila, 2, fila, 8];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    fila = 6;
                    foreach (var item in lista)
                    {
                        ws.Cells[fila, 2].Value = item.CodBarra + "-" + item.NomBarra;
                        ws.Cells[fila, 3].Value = item.PmDbf5LCod;
                        ws.Cells[fila, 4].Value = item.PmDbf5FecIni.Value.ToString("dd/MM/yyyy");
                        ws.Cells[fila, 5].Value = item.PmBloqCodi;
                        ws.Cells[fila, 6].Value = item.PmDbf5Carga;
                        ws.Cells[fila, 7].Value = item.PmDbf5ICCO;
                        ws.Cells[fila, 8].Value = item.NroSemana;
                        ws.Cells[fila, 9].Value = item.GrupoCodi;//NET 20190307 - Corrección de exportación Excel
                        ws.Cells[fila, 10].Value = item.PmDbf5Codi;//NET 20190307 - Corrección de exportación Excel

                        //Border por celda
                        //rg = ws.Cells[fila, 2, fila, 59];
                        //rg.Style.WrapText = true;
                        //rg = ObtenerEstiloCelda(rg, 0);
                        //rg = ws.Cells[fila, 5, fila, 59];
                        //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg = ws.Cells[fila, 2, fila, 8];
                        rg = ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }


                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 20;

                    //NET 20190307 - Corrección de exportación Excel - Inicio
                    ws.Column(9).Width = 0;
                    ws.Column(10).Width = 0;

                    for (var row = 6; row < lista.Count + 6; row++)
                    {
                        for (var column = 3; column <= 8; column++)
                        {
                            ws.Cells[row, column].Style.Locked = false;
                        }
                    }
                    //NET 20190307 - Corrección de exportación Excel - Fin


                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                return xlPackage.GetAsByteArray();
            }
            //return excel;
        }

        public static byte[] ExportarGnde(List<PmoDatGndseDTO> lista)
        {
            string titulo = "Definición de barras de modelo";

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                //ws.Protection.IsProtected = true;
                if (ws != null)
                {
                    //TITULO
                    int fila = 2;
                    int colum = 3;
                    ws.Cells[fila, colum].Value = titulo;
                    ExcelRange rg = ws.Cells[fila, colum, fila, colum + 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //CABECERA DE TABLA
                    fila = 5;
                    ws.Cells[fila, 2].Value = "Cod. Tabla";
                    //ws.Cells[fila, 7, fila, 59].Merge = true;
                    ws.Cells[fila, 3].Value = "Cod. COES";
                    ws.Cells[fila, 4].Value = "Cod. SDDP";
                    ws.Cells[fila, 5].Value = "Central";
                    ws.Cells[fila, 6].Value = "Stg";
                    ws.Cells[fila, 7].Value = "NScn";
                    ws.Cells[fila, 8].Value = "LBlk";
                    ws.Cells[fila, 9].Value = "PU";                    
                    //ws.Cells[fila + 1, 5].Value = "1";

                    //for (var x = 1; x < 54; x++)
                    //    ws.Cells[fila + 1, 6 + x].Value = "" + x + "";

                    rg = ws.Cells[fila, 2, fila, 9];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    fila = 6;
                    foreach (var item in lista)
                    {
                        ws.Cells[fila, 2].Value = item.PmGnd5Codi;
                        ws.Cells[fila, 3].Value = item.GrupoCodi;
                        ws.Cells[fila, 4].Value = item.GrupoCodiSDDP;
                        ws.Cells[fila, 5].Value = item.GrupoNomb;
                        ws.Cells[fila, 6].Value = item.PmGnd5STG;
                        ws.Cells[fila, 7].Value = item.PmGnd5SCN;
                        ws.Cells[fila, 8].Value = item.PmBloqCodi;
                        ws.Cells[fila, 9].Value = item.PmGnd5PU;
                        //Border por celda
                        //rg = ws.Cells[fila, 2, fila, 59];
                        //rg.Style.WrapText = true;
                        //rg = ObtenerEstiloCelda(rg, 0);
                        //rg = ws.Cells[fila, 5, fila, 59];
                        //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg = ws.Cells[fila, 2, fila, 9];
                        rg = ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }


                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 20;
                    ws.Column(9).Width = 20;
                    //for (var row = 7; row < lista.Count + 7; row++)
                    //{
                    //    for (var column = 7; column <= 59; column++)
                    //    {
                    //        ws.Cells[row, column].Style.Locked = false;
                    //    }
                    //}


                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                return xlPackage.GetAsByteArray();
            }
            //return excel;
        }
        public static List<PmoDatPmhiTrDTO> Importar(Stream file)
        {
            //ExcelPackage xlPackage = new ExcelPackage(file);
            List<PmoDatPmhiTrDTO> lista = new List<PmoDatPmhiTrDTO>();
            using (var package = new ExcelPackage(file))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var totalColumnas = workSheet.Dimension.End.Column;
                var totalFilas = workSheet.Dimension.End.Row;

                //if (workSheet.Cells[0, 4].Value.ToString().Equals())
                for (int fila = 7; fila <= totalFilas; fila++)
                {
                    var entity = new PmoDatPmhiTrDTO();
                    entity.PmPmhtCodi = Convert.ToInt32(workSheet.Cells[fila, 2].Value);
                    entity.Sddpcodi = Convert.ToInt32(workSheet.Cells[fila, 3].Value);
                    entity.Sddpnum = Convert.ToInt32(workSheet.Cells[fila, 4].Value);
                    entity.Sddpnomb = workSheet.Cells[fila, 5].Value.ToString();
                    entity.PmPmhtAnhio = Convert.ToInt32(workSheet.Cells[fila, 6].Value);
                    entity.PmPmhtDisp01 = Convert.ToDecimal(workSheet.Cells[fila, 7].Value);
                    entity.PmPmhtDisp02 = Convert.ToDecimal(workSheet.Cells[fila, 8].Value);
                    entity.PmPmhtDisp03 = Convert.ToDecimal(workSheet.Cells[fila, 9].Value);
                    entity.PmPmhtDisp04 = Convert.ToDecimal(workSheet.Cells[fila, 10].Value);
                    entity.PmPmhtDisp05 = Convert.ToDecimal(workSheet.Cells[fila, 11].Value);
                    entity.PmPmhtDisp06 = Convert.ToDecimal(workSheet.Cells[fila, 12].Value);
                    entity.PmPmhtDisp07 = Convert.ToDecimal(workSheet.Cells[fila, 13].Value);
                    entity.PmPmhtDisp08 = Convert.ToDecimal(workSheet.Cells[fila, 14].Value);
                    entity.PmPmhtDisp09 = Convert.ToDecimal(workSheet.Cells[fila, 15].Value);
                    entity.PmPmhtDisp10 = Convert.ToDecimal(workSheet.Cells[fila, 16].Value);
                    entity.PmPmhtDisp11 = Convert.ToDecimal(workSheet.Cells[fila, 17].Value);
                    entity.PmPmhtDisp12 = Convert.ToDecimal(workSheet.Cells[fila, 18].Value);
                    entity.PmPmhtDisp13 = Convert.ToDecimal(workSheet.Cells[fila, 19].Value);
                    entity.PmPmhtDisp14 = Convert.ToDecimal(workSheet.Cells[fila, 20].Value);
                    entity.PmPmhtDisp15 = Convert.ToDecimal(workSheet.Cells[fila, 21].Value);
                    entity.PmPmhtDisp16 = Convert.ToDecimal(workSheet.Cells[fila, 22].Value);
                    entity.PmPmhtDisp17 = Convert.ToDecimal(workSheet.Cells[fila, 23].Value);
                    entity.PmPmhtDisp18 = Convert.ToDecimal(workSheet.Cells[fila, 24].Value);
                    entity.PmPmhtDisp19 = Convert.ToDecimal(workSheet.Cells[fila, 25].Value);
                    entity.PmPmhtDisp20 = Convert.ToDecimal(workSheet.Cells[fila, 26].Value);
                    entity.PmPmhtDisp21 = Convert.ToDecimal(workSheet.Cells[fila, 27].Value);
                    entity.PmPmhtDisp22 = Convert.ToDecimal(workSheet.Cells[fila, 28].Value);
                    entity.PmPmhtDisp23 = Convert.ToDecimal(workSheet.Cells[fila, 29].Value);
                    entity.PmPmhtDisp24 = Convert.ToDecimal(workSheet.Cells[fila, 30].Value);
                    entity.PmPmhtDisp25 = Convert.ToDecimal(workSheet.Cells[fila, 31].Value);
                    entity.PmPmhtDisp26 = Convert.ToDecimal(workSheet.Cells[fila, 32].Value);
                    entity.PmPmhtDisp27 = Convert.ToDecimal(workSheet.Cells[fila, 33].Value);
                    entity.PmPmhtDisp28 = Convert.ToDecimal(workSheet.Cells[fila, 34].Value);
                    entity.PmPmhtDisp29 = Convert.ToDecimal(workSheet.Cells[fila, 35].Value);
                    entity.PmPmhtDisp30 = Convert.ToDecimal(workSheet.Cells[fila, 36].Value);
                    entity.PmPmhtDisp31 = Convert.ToDecimal(workSheet.Cells[fila, 37].Value);
                    entity.PmPmhtDisp32 = Convert.ToDecimal(workSheet.Cells[fila, 38].Value);
                    entity.PmPmhtDisp33 = Convert.ToDecimal(workSheet.Cells[fila, 39].Value);
                    entity.PmPmhtDisp34 = Convert.ToDecimal(workSheet.Cells[fila, 40].Value);
                    entity.PmPmhtDisp35 = Convert.ToDecimal(workSheet.Cells[fila, 41].Value);
                    entity.PmPmhtDisp36 = Convert.ToDecimal(workSheet.Cells[fila, 42].Value);
                    entity.PmPmhtDisp37 = Convert.ToDecimal(workSheet.Cells[fila, 43].Value);
                    entity.PmPmhtDisp38 = Convert.ToDecimal(workSheet.Cells[fila, 44].Value);
                    entity.PmPmhtDisp39 = Convert.ToDecimal(workSheet.Cells[fila, 45].Value);
                    entity.PmPmhtDisp40 = Convert.ToDecimal(workSheet.Cells[fila, 46].Value);
                    entity.PmPmhtDisp41 = Convert.ToDecimal(workSheet.Cells[fila, 47].Value);
                    entity.PmPmhtDisp42 = Convert.ToDecimal(workSheet.Cells[fila, 48].Value);
                    entity.PmPmhtDisp43 = Convert.ToDecimal(workSheet.Cells[fila, 49].Value);
                    entity.PmPmhtDisp44 = Convert.ToDecimal(workSheet.Cells[fila, 50].Value);
                    entity.PmPmhtDisp45 = Convert.ToDecimal(workSheet.Cells[fila, 51].Value);
                    entity.PmPmhtDisp46 = Convert.ToDecimal(workSheet.Cells[fila, 52].Value);
                    entity.PmPmhtDisp47 = Convert.ToDecimal(workSheet.Cells[fila, 53].Value);
                    entity.PmPmhtDisp48 = Convert.ToDecimal(workSheet.Cells[fila, 54].Value);
                    entity.PmPmhtDisp49 = Convert.ToDecimal(workSheet.Cells[fila, 55].Value);
                    entity.PmPmhtDisp50 = Convert.ToDecimal(workSheet.Cells[fila, 56].Value);
                    entity.PmPmhtDisp51 = Convert.ToDecimal(workSheet.Cells[fila, 57].Value);
                    entity.PmPmhtDisp52 = Convert.ToDecimal(workSheet.Cells[fila, 58].Value);
                    entity.PmPmhtDisp53 = Convert.ToDecimal(workSheet.Cells[fila, 59].Value);
                    //entity.LastName = workSheet.Cells[columna, 2].Value.ToString();

                    lista.Add(entity);
                }
            }

            return lista;
        }

        public static List<PmoDatDbfDTO> ImportarDbf(Stream file)
        {
            try
            {
                //ExcelPackage xlPackage = new ExcelPackage(file);
                List<PmoDatDbfDTO> lista = new List<PmoDatDbfDTO>();
                using (var package = new ExcelPackage(file))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var totalColumnas = workSheet.Dimension.End.Column;
                    var totalFilas = workSheet.Dimension.End.Row;

                    //if (workSheet.Cells[0, 4].Value.ToString().Equals())
                    for (int fila = 6; fila <= totalFilas; fila++)
                    {
                        var entity = new PmoDatDbfDTO();

                        var compuesto = Convert.ToString(workSheet.Cells[fila, 2].Value);

                        entity.CodBarra = compuesto.Split('-')[0] == string.Empty ? 0 : Convert.ToInt32(compuesto.Split('-')[0]);
                        entity.NomBarra = compuesto.Split('-')[1];
                        entity.PmDbf5LCod = workSheet.Cells[fila, 3].Value.ToString();
                        var fecha = workSheet.Cells[fila, 4].Value.ToString();
                        entity.PmDbf5FecIni = DateTime.ParseExact(fecha, "dd/MM/yyyy", null);
                        entity.PmBloqCodi = Convert.ToInt32(workSheet.Cells[fila, 5].Value);
                        entity.PmDbf5Carga = Convert.ToDecimal(workSheet.Cells[fila, 6].Value);
                        entity.PmDbf5ICCO = Convert.ToInt32(workSheet.Cells[fila, 7].Value);
                        entity.NroSemana = workSheet.Cells[fila, 8].Value == null ? "" : workSheet.Cells[fila, 8].Value.ToString();                            
                        entity.GrupoCodi = Convert.ToInt32(workSheet.Cells[fila, 9].Value); //NET 20190307 - corrección importación de arhivo Excel
                        entity.PmDbf5Codi = Convert.ToInt32(workSheet.Cells[fila, 10].Value); //NET 20190307 - corrección importación de arhivo Excel

                        lista.Add(entity);
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static List<PmoDatGndseDTO> ImportarGnde(Stream file)
        {
            try
            {
                //ExcelPackage xlPackage = new ExcelPackage(file);
                List<PmoDatGndseDTO> lista = new List<PmoDatGndseDTO>();
                using (var package = new ExcelPackage(file))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var totalColumnas = workSheet.Dimension.End.Column;
                    var totalFilas = workSheet.Dimension.End.Row;

                    //if (workSheet.Cells[0, 4].Value.ToString().Equals())
                    for (int fila = 6; fila <= totalFilas; fila++)
                    {
                        var entity = new PmoDatGndseDTO();

                        entity.PmGnd5Codi = Convert.ToInt32(workSheet.Cells[fila, 2].Value);
                        entity.GrupoCodi = Convert.ToInt32(workSheet.Cells[fila, 3].Value);
                        entity.GrupoCodiSDDP = Convert.ToInt32(workSheet.Cells[fila, 4].Value);
                        entity.GrupoNomb = workSheet.Cells[fila, 5].Value.ToString();
                        entity.PmGnd5STG = workSheet.Cells[fila, 6].Value.ToString();
                        entity.PmGnd5SCN = workSheet.Cells[fila, 7].Value.ToString();
                        entity.PmBloqCodi = Convert.ToInt32(workSheet.Cells[fila, 8].Value);
                        entity.PmGnd5PU = Convert.ToInt32(workSheet.Cells[fila, 9].Value);

                        lista.Add(entity);
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
