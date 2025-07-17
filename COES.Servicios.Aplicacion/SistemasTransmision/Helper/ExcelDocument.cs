using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
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

namespace COES.Servicios.Aplicacion.SistemasTransmision.Helper
{
    class ExcelDocument
    {

        /// <summary>
        /// Estilo del excel 
        /// 0: Celdas
        /// 1: Titulos        
        /// </summary>
        /// <param name="rango"></param>
        /// <param name="seccion"></param>
        /// <returns></returns>
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

            return rango;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla ST_GENERADOR
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaEnergiaNetas">Lista de registros de StEnergiaDTO</param>
        /// <returns></returns>
        public static void ListaEmpresasGeneradoras(string fileName, StRecalculoDTO EntidadRecalculo, List<StGeneradorDTO> ListaGeneradores)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "LISTA DE EMPRESAS GENERADORAS";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "EMPRESA DE GENERACIÓN";
                    ws.Cells[index, 3].Value = "CENTARL";
                    ws.Cells[index, 4].Value = "BARRA";

                    rg = ws.Cells[index, 2, index, 4];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaGeneradores)
                    {
                        ws.Cells[index, 2].Value = item.Emprnomb.ToString();
                        ws.Cells[index, 3].Value = item.Equinomb.ToString();
                        ws.Cells[index, 4].Value = item.Barrnombre.ToString();

                        //Border por celda
                        rg = ws.Cells[index, 2, index, 4];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    ws.Column(2).Width = 60;
                    ws.Column(3).Width = 50;
                    ws.Column(4).Width = 50;
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla ST_SISTEMATRANS
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaEnergiaNetas">Lista de registros de StEnergiaDTO</param>
        /// <returns></returns>
        public static void ListaSistemasTransmision(string fileName, StRecalculoDTO EntidadRecalculo, List<StSistematransDTO> ListaSistemas)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "LISTA DE SISTEMAS DE TRANSMISIÓN";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "NOMBRE DEL TITULAR";
                    ws.Cells[index, 3].Value = "NOMBRE DEL SISTEMA";
                    ws.Cells[index, 4].Value = "CÓDIGO ELEMENTO";
                    ws.Cells[index, 5].Value = "NOMBRE ELEMENTO";
                    ws.Cells[index, 6].Value = "IMPORTE";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.0000";
                    ws.Cells[index, 7].Value = "BARRA1";
                    ws.Cells[index, 8].Value = "BARRA2";

                    rg = ws.Cells[index, 2, index, 8];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaSistemas)
                    {
                        ws.Cells[index, 2].Value = item.Emprnomb.ToString();
                        ws.Cells[index, 3].Value = item.Sistrnnombre.ToString();
                        ws.Cells[index, 4].Value = item.Stcompcodelemento.ToString();
                        ws.Cells[index, 5].Value = item.Stcompnomelemento.ToString();
                        ws.Cells[index, 6].Value = (item.Stcompimpcompensacion != null) ? item.Stcompimpcompensacion : Decimal.Zero;
                        ws.Cells[index, 7].Value = item.Barrnombre1.ToString();
                        ws.Cells[index, 8].Value = item.Barrnombre2.ToString();

                        //Border por celda
                        rg = ws.Cells[index, 2, index, 8];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 6, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 60;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 30;
                    ws.Column(8).Width = 30;
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla ST_ENERGIA
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaEnergiaNetas">Lista de registros de StEnergiaDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoStEnergiasNetas(string fileName, StRecalculoDTO EntidadRecalculo, List<StEnergiaDTO> ListaEnergiaNetas)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "ENERGIAS NETAS";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "CENTRAL GENERACIÓN";
                    ws.Cells[index, 3].Value = "ENERGÍA";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.000000000000";

                    rg = ws.Cells[index, 2, index, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaEnergiaNetas)
                    {
                        ws.Cells[index, 2].Value = item.Equinomb.ToString();
                        ws.Cells[index, 3].Value = (item.Stenrgrgia != null) ? item.Stenrgrgia : Decimal.Zero;

                        //Border por celda
                        rg = ws.Cells[index, 2, index, 3];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 3, index, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 30;
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla ST_DISTANCIASELECTRICAS
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaDistelectrica">Lista de registros de StDistelectricaDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoStDistelectrica(string fileName, StRecalculoDTO EntidadRecalculo, List<StDistelectricaDTO> ListaDistelectrica, List<StBarraDTO> ListaBarra)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "DISTANCIAS ELECTRICAS";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    int col = 3;
                    ws.Cells[index, 2].Value = "BARRAS";
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    ws.Column(2).Width = 40;
                    foreach (var Barra in ListaBarra)
                    {
                        ws.Cells[index, col].Value = Barra.Barrnomb;
                        ws.Cells[index, col, index, col + 1].Merge = true;
                        ws.Cells[index + 1, col].Value = "R(pu)";
                        ws.Cells[index + 1, col + 1].Value = "X(pu)";
                        ws.Column(col).Style.Numberformat.Format = "#,##0.00000000000000000000";
                        ws.Column(col + 1).Style.Numberformat.Format = "#,##0.00000000000000000000";
                        ws.Column(col).Width = 15;
                        ws.Column(col + 1).Width = 15;
                        col = col + 2;
                    }
                    rg = ws.Cells[index, 2, index + 1, col - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index = index + 2;
                    foreach (var item in ListaDistelectrica)
                    {
                        ws.Cells[index, 2].Value = item.Barrnombre.ToString();
                        col = 3;
                        foreach (StBarraDTO dto in ListaBarra)
                        {
                            StDsteleBarraDTO EntidadDistelecBarra = (new SistemasTransmisionAppServicio()).GetByIdStDsteleBarra(item.Dstelecodi, dto.Barrcodi);
                            ws.Cells[index, col].Value = (EntidadDistelecBarra.Delbarrpu != null) ? EntidadDistelecBarra.Delbarrpu : Decimal.Zero;
                            ws.Cells[index, col + 1].Value = (EntidadDistelecBarra.Delbarxpu != null) ? EntidadDistelecBarra.Delbarxpu : Decimal.Zero;
                            col = col + 2;
                        }
                        //Border por celda
                        rg = ws.Cells[index, 2, index, col - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 3, index, col - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla ST_FACTOR
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaDistelectrica">Lista de registros de StFactorDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoStFactor(string fileName, StRecalculoDTO EntidadRecalculo, List<StFactorDTO> ListaFactorActualizacion)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "FACTORES DE ACTUALIZACIÓN";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "SISTEMA";
                    ws.Cells[index, 3].Value = "FACTOR";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.000000000000";

                    rg = ws.Cells[index, 2, index, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaFactorActualizacion)
                    {
                        ws.Cells[index, 2].Value = item.SisTrnnombre.ToString();
                        ws.Cells[index, 3].Value = (item.Stfacttor != null) ? item.Stfacttor : Decimal.Zero;

                        //Border por celda
                        rg = ws.Cells[index, 2, index, 3];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 3, index, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 30;
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        /// <summary> 
        /// Permite generar el archivo de exportacion de la tabla ST_RESPAGO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaStRespago">Lista de registros de StRespagoDTO</param>
        /// <param name="ListaCompensacion">Lista de registros de StCompensacionDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoStRespago(string fileName, StRecalculoDTO EntidadRecalculo, List<StRespagoDTO> ListaStRespago, List<StCompensacionDTO> ListaCompensacion)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "CENTRALES RESPONSABILIDAD";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    int col = 3;
                    ws.Cells[index, 2].Value = "CENTRAL GENERACIÓN";
                    //ws.Cells[index, 3].Value = "SISTEMA TRANSMISION";
                    foreach (var ele in ListaCompensacion)
                    {
                        ws.Cells[index, col].Value = ele.Stcompcodelemento;
                        ws.Column(col).Style.Numberformat.Format = "#,##0";
                        ws.Column(col).Width = 15;
                        col++;
                    }

                    rg = ws.Cells[index, 2, index, col - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaStRespago)
                    {
                        ws.Cells[index, 2].Value = item.Equinomb.ToString();
                        //ws.Cells[index, 3].Value = item.Sistrnnombre.ToString();
                        col = 3;
                        foreach (StCompensacionDTO dto in ListaCompensacion)
                        {
                            StRespagoeleDTO EntidadRespagoEle = (new SistemasTransmisionAppServicio()).GetByIdStRespagoele(item.Respagcodi, dto.Stcompcodi);
                            ws.Cells[index, col].Value = (EntidadRespagoEle.Respaevalor != null) ? EntidadRespagoEle.Respaevalor : Decimal.Zero;
                            col++;
                        }
                        //Border por celda
                        rg = ws.Cells[index, 2, index, col - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 3, index, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        index++;
                    }

                    ws.Column(2).Width = 40;
                    //ws.Column(3).Width = 30;
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        /// <summary> 
        /// Permite generar el archivo de exportacion de la tabla ST_COMPMENSUAL
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaStCompmensual">Lista de registros de StCompmensualDTO</param>
        /// <param name="ListaCompensacion">Lista de registros de StCompensacionDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoStCompmensual(string fileName, StRecalculoDTO EntidadRecalculo, List<StCompmensualDTO> ListaStCompmensual, List<StCompensacionDTO> ListaCompensacion)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "COMPENSACIÓN MENSUAL";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    int col = 3;
                    ws.Cells[index, 2].Value = "CENTRAL GENERACIÓN";
                    //ws.Cells[index, 3].Value = "SISTEMA TRANSMISION";
                    foreach (var ele in ListaCompensacion)
                    {
                        ws.Cells[index, col].Value = ele.Stcompcodelemento;
                        ws.Column(col).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(col).Width = 15;
                        col++;
                    }

                    rg = ws.Cells[index, 2, index, col - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaStCompmensual)
                    {
                        ws.Cells[index, 2].Value = item.Equinomb.ToString();
                        //ws.Cells[index, 3].Value = item.Sistrnnombre.ToString();
                        col = 3;
                        foreach (StCompensacionDTO dto in ListaCompensacion)
                        {
                            StCompmensualeleDTO EntidadCompMenualEle = (new SistemasTransmisionAppServicio()).GetByIdStCompmensualele(item.Cmpmencodi, dto.Stcompcodi);
                            if (EntidadCompMenualEle != null)
                            {
                                ws.Cells[index, col].Value = EntidadCompMenualEle.Cmpmelvalor;
                            }
                            else
                                ws.Cells[index, col].Value = Decimal.Zero;
                            col++;
                        }
                        //Border por celda
                        rg = ws.Cells[index, 2, index, col - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 3, index, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        index++;
                    }

                    ws.Column(2).Width = 40;
                    //ws.Column(3).Width = 30;
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        // REPORTES 
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        //
        /// <summary> 
        /// CU06 Reportes 301 – GWh/OHMIOS Mensuales de Generadores Relevantes 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaReporteGeneradores">Lista de registros de StCentralgenDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoReporte301(string fileName, StRecalculoDTO EntidadRecalculo, List<StCentralgenDTO> ListaReporteGeneradores)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            GenerarFormatoReporteExcel301(EntidadRecalculo, ListaReporteGeneradores, newFile, "REPORTE");
        }

        /// <summary>
        /// Genera reporte excel para el formato 301 - GWh/OHMIOS Mensuales de Generadores Relevantes
        /// </summary>
        /// <param name="EntidadRecalculo"></param>
        /// <param name="ListaReporteGeneradores"></param>
        /// <param name="file"></param>
        /// <param name="nombreHoja"></param>
        public static void GenerarFormatoReporteExcel301(StRecalculoDTO EntidadRecalculo, List<StCentralgenDTO> ListaReporteGeneradores, FileInfo file, string nombreHoja)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(file))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                GenerarFormatoReporteExcel301Cuerpo(EntidadRecalculo, ListaReporteGeneradores, ws);
                xlPackage.Save();
            }
        }

        /// <summary>
        ///  Genera cuerpo de reporte excel para el formato 301 - GWh/OHMIOS Mensuales de Generadores Relevantes
        /// </summary>
        /// <param name="EntidadRecalculo"></param>
        /// <param name="ListaReporteGeneradores"></param>
        /// <param name="ws"></param>
        public static void GenerarFormatoReporteExcel301Cuerpo(StRecalculoDTO EntidadRecalculo, List<StCentralgenDTO> ListaReporteGeneradores, ExcelWorksheet ws)
        {
            if (ws != null)
            {
                int index = 2;
                //TITULO
                ws.Cells[index, 3].Value = "Reportes 301 – GWh/OHMIOS Mensuales de Generadores Relevantes";
                ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                rg.Style.Font.Size = 16;
                rg.Style.Font.Bold = true;
                //CABECERA DE TABLA
                index += 4;
                ws.Cells[index, 2].Value = "CENTRAL";
                ws.Cells[index, 3].Value = "TITULAR";
                ws.Cells[index, 4].Value = "ELEMENTO";
                ws.Cells[index, 5].Value = "DISTANCIA ELECTRICA Zu (OHMIOS)";
                ws.Cells[index, 6].Value = "GWH POR MES O AÑO";
                ws.Cells[index, 7].Value = "GWH / Z";
                ws.Column(5).Style.Numberformat.Format = "#,##0.000000000000";
                ws.Column(6).Style.Numberformat.Format = "#,##0.000000000000";
                ws.Column(7).Style.Numberformat.Format = "#,##0.000000000000";

                rg = ws.Cells[index, 2, index, 7];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);

                rg = ws.Cells[index - 1, 2];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);
                index++;

                int col = 6;
                int col2 = 7;
                int count = 5;
                int colTotal = 4;
                decimal[] TotalColum = new decimal[1000];
                decimal[] TotalColum2 = new decimal[1000];

                var codelement = "";
                foreach (var item in ListaReporteGeneradores)
                {
                    if (codelement == "") { codelement = item.Stcompcodelemento; ws.Cells[index - 2, 2].Value = item.Stcompcodelemento.ToString(); }
                    else if (codelement != item.Stcompcodelemento)
                    {
                        for (int i = 6; i < 7; i++)
                        {
                            ws.Cells[index, i].Value = TotalColum[i];
                        }
                        for (int i = 7; i < 8; i++)
                        {
                            ws.Cells[index, i].Value = TotalColum2[i];
                        }
                        ws.Cells[index, colTotal - 1].Value = "TOTAL";
                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index, 3, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        count = 5;
                        codelement = item.Stcompcodelemento;
                        index += 3;

                        ws.Cells[index - 1, 2].Value = item.Stcompcodelemento.ToString();
                        ws.Cells[index, 2].Value = "CENTRAL";
                        ws.Cells[index, 3].Value = "TITULAR";
                        ws.Cells[index, 4].Value = "ELEMENTO";
                        ws.Cells[index, 5].Value = "DISTANCIA ELECTRICA Zu (OHMIOS)";
                        ws.Cells[index, 6].Value = "GWH POR MES O AÑO";
                        ws.Cells[index, 7].Value = "GWH / Z";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.000000000000";
                        ws.Column(6).Style.Numberformat.Format = "#,##0.000000000000";
                        ws.Column(7).Style.Numberformat.Format = "#,##0.000000000000";

                        rg = ws.Cells[index, 2, index, 7];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        rg = ws.Cells[index - 1, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        index++;

                        TotalColum = new decimal[1000];
                        TotalColum2 = new decimal[1000];
                    }
                    //ws.Cells[index - 2, 2].Value = item.Stcompcodelemento.ToString();
                    ws.Cells[index, 2].Value = item.Equinomb.ToString();
                    ws.Cells[index, 3].Value = item.Emprnomb.ToString();
                    ws.Cells[index, 4].Value = item.Stcompcodelemento;
                    ws.Cells[index, 5].Value = item.Degeledistancia;
                    ws.Cells[index, 6].Value = item.Stenrgrgia;
                    ws.Cells[index, 7].Value = item.Gwhz;

                    TotalColum[col] += item.Stenrgrgia;
                    TotalColum2[col2] += item.Gwhz;
                    count++;
                    //Border por celda
                    rg = ws.Cells[index, 2, index, 7];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);
                    rg = ws.Cells[index, 4, index, 7];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    index++;
                }
                for (int i = 6; i < 7; i++)
                {
                    ws.Cells[index, i].Value = TotalColum[i];
                }
                for (int i = 7; i < 8; i++)
                {
                    ws.Cells[index, i].Value = TotalColum2[i];
                }
                ws.Cells[index, colTotal - 1].Value = "TOTAL";
                rg = ws.Cells[index, 2, index, 7];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);
                rg = ws.Cells[index, 3, index, 7];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                index++;

                ws.Column(2).Width = 40;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 30;
                ws.Column(5).Width = 30;
                ws.Column(6).Width = 30;
                ws.Column(7).Width = 30;
                //Insertar el logo
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                picture.SetPosition(10, 10);
                picture.SetSize(135, 45);
            }
        }

        /// <summary> 
        /// CU07 Reportes 302 – Cálculo del Factor de Participación Mensual o Anual 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaReporteFactorParticipacion">Lista de registros de StFactorpagoDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoReporte302(string fileName, StRecalculoDTO EntidadRecalculo, List<StFactorpagoDTO> ListaReporteFactorParticipacion)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            GenerarFormatoReporteExcel302(EntidadRecalculo, ListaReporteFactorParticipacion, newFile, "REPORTE");
        } // Falta este Reporte 

        /// <summary>
        /// Genera reporte excel para el formato 302 – Cálculo del Factor de Participación Mensual o Anual
        /// </summary>
        /// <param name="entidadRecalculo"></param>
        /// <param name="listaReporteFactorParticipacion"></param>
        /// <param name="file"></param>
        /// <param name="nombreHoja"></param>
        public static void GenerarFormatoReporteExcel302(StRecalculoDTO entidadRecalculo, List<StFactorpagoDTO> listaReporteFactorParticipacion, FileInfo file, string nombreHoja)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(file))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                GenerarFormatoReporteExcel302Cuerpo(entidadRecalculo, listaReporteFactorParticipacion, ws);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera cuerpo de reporte excel para el formato 302 – Cálculo del Factor de Participación Mensual o Anual
        /// </summary>
        /// <param name="entidadRecalculo"></param>
        /// <param name="listaReporteFactorParticipacion"></param>
        /// <param name="ws"></param>
        public static void GenerarFormatoReporteExcel302Cuerpo(StRecalculoDTO entidadRecalculo, List<StFactorpagoDTO> listaReporteFactorParticipacion, ExcelWorksheet ws)
        {
            if (ws != null)
            {
                int index = 2;
                //TITULO
                ws.Cells[index, 3].Value = "Reportes 302 – Cálculo del Factor de Participación Mensual o Anual";
                ws.Cells[index + 1, 3].Value = entidadRecalculo.Stpernombre + "/" + entidadRecalculo.Strecanombre + "";
                ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                rg.Style.Font.Size = 16;
                rg.Style.Font.Bold = true;
                //CABECERA DE TABLA
                index += 4;
                ws.Cells[index, 2].Value = "CENTRAL";
                ws.Cells[index, 3].Value = "FG INICIAL (B)";
                ws.Cells[index, 4].Value = "FG% INICIAL (C)";
                ws.Cells[index, 5].Value = "FG% > 1%  (D)";
                ws.Cells[index, 6].Value = "FG % (E)";
                ws.Column(3).Style.Numberformat.Format = "#,##0.000000000000";
                ws.Column(4).Style.Numberformat.Format = "#,##0.0000%";
                ws.Column(5).Style.Numberformat.Format = "#,##0.0000%";
                ws.Column(6).Style.Numberformat.Format = "#,##0.0000%";

                rg = ws.Cells[index, 2, index, 6];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);

                rg = ws.Cells[index - 1, 2];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);

                index++;
                int col = 3;
                int col2 = 4;
                int col3 = 5;
                int col4 = 6;
                int count = 5;
                int colTotal = 3;
                decimal[] TotalColum = new decimal[1000];
                decimal[] TotalColum2 = new decimal[1000];
                decimal[] TotalColum3 = new decimal[1000];
                decimal[] TotalColum4 = new decimal[1000];

                var codelement = "";
                foreach (var item in listaReporteFactorParticipacion)
                {
                    if (codelement == "") { codelement = item.Stcompcodelemento; ws.Cells[index - 2, 2].Value = item.Stcompcodelemento.ToString(); }
                    else if (codelement != item.Stcompcodelemento)
                    {
                        for (int i = 3; i < 4; i++)
                        {
                            ws.Cells[index, i].Value = TotalColum[i];
                        }
                        for (int i = 4; i < 5; i++)
                        {
                            ws.Cells[index, i].Value = TotalColum2[i];
                        }
                        for (int i = 5; i < 6; i++)
                        {
                            ws.Cells[index, i].Value = TotalColum3[i];
                        }
                        for (int i = 6; i < 7; i++)
                        {
                            ws.Cells[index, i].Value = TotalColum4[i];
                        }
                        ws.Cells[index, colTotal - 1].Value = "TOTAL";
                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index, 3, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        count = 3;
                        codelement = item.Stcompcodelemento;
                        index += 3;

                        ws.Cells[index - 1, 2].Value = item.Stcompcodelemento.ToString();
                        ws.Cells[index, 2].Value = "CENTRAL";
                        ws.Cells[index, 3].Value = "FG INICIAL (B)";
                        ws.Cells[index, 4].Value = "FG% INICIAL (C)";
                        ws.Cells[index, 5].Value = "FG% > 1%  (D)";
                        ws.Cells[index, 6].Value = "FG % (E)";
                        ws.Column(3).Style.Numberformat.Format = "#,##0.000000000000";
                        ws.Column(4).Style.Numberformat.Format = "#,##0.0000%";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.0000%";
                        ws.Column(6).Style.Numberformat.Format = "#,##0.0000%";

                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        rg = ws.Cells[index - 1, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        index++;

                        TotalColum = new decimal[1000];
                        TotalColum2 = new decimal[1000];
                        TotalColum3 = new decimal[1000];
                        TotalColum4 = new decimal[1000];
                    }
                    //ws.Cells[index - 2, 2].Value = item.Stcompcodelemento.ToString();
                    ws.Cells[index, 2].Value = item.Equinomb;
                    ws.Cells[index, 3].Value = item.Facpagfggl;
                    ws.Cells[index, 4].Value = item.Facpagfggl;
                    if (item.Facpagreajuste == 1)
                    {
                        ws.Cells[index, 5].Value = item.Facpagfggl;
                        TotalColum3[col3] += item.Facpagfggl;
                    }
                    else
                        ws.Cells[index, 5].Value = 0;
                    ws.Cells[index, 6].Value = item.Facpagfgglajuste;

                    TotalColum[col] += item.Facpagfggl;
                    TotalColum2[col2] += item.Facpagfggl;
                    TotalColum4[col4] += item.Facpagfgglajuste;
                    count++;
                    //Border por celda
                    rg = ws.Cells[index, 2, index, 6];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);
                    rg = ws.Cells[index, 3, index, 6];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    index++;
                }
                for (int i = 3; i < 4; i++)
                {
                    ws.Cells[index, i].Value = TotalColum[i];
                }
                for (int i = 4; i < 5; i++)
                {
                    ws.Cells[index, i].Value = TotalColum2[i];
                }
                for (int i = 5; i < 6; i++)
                {
                    ws.Cells[index, i].Value = TotalColum3[i];
                }
                for (int i = 6; i < 7; i++)
                {
                    ws.Cells[index, i].Value = TotalColum4[i];
                }
                ws.Cells[index, colTotal - 1].Value = "TOTAL";
                rg = ws.Cells[index, 2, index, 6];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);
                rg = ws.Cells[index, 3, index, 6];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                index++;

                ws.Column(2).Width = 40;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 30;
                ws.Column(5).Width = 30;
                ws.Column(6).Width = 30;
                //Insertar el logo
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                picture.SetPosition(10, 10);
                picture.SetSize(135, 45);
            }
        }

        /// <summary> 
        /// CU08 Reportes 303 – Compensación Mensual
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaReporteCompensacionMensual">Lista de registros de StFactorpagoDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoReporte303(string fileName, StRecalculoDTO EntidadRecalculo, List<StFactorpagoDTO> ListaReporteCompensacionMensual)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            GenerarFormatoReporteExcel303(EntidadRecalculo, ListaReporteCompensacionMensual, newFile, "REPORTE");
        }

        /// <summary>
        /// Genera reporte excel para el formato 303 – Compensación Mensual
        /// </summary>
        /// <param name="entidadRecalculo"></param>
        /// <param name="listaReporteCompensacionMensual"></param>
        /// <param name="file"></param>
        /// <param name="nombreHoja"></param>
        public static void GenerarFormatoReporteExcel303(StRecalculoDTO entidadRecalculo, List<StFactorpagoDTO> listaReporteCompensacionMensual, FileInfo file, string nombreHoja)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(file))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                GenerarFormatoReporteExcel303Cuerpo(entidadRecalculo, listaReporteCompensacionMensual, ws);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera cuerpo reporte excel para el formato 303 – Compensación Mensual
        /// </summary>
        /// <param name="entidadRecalculo"></param>
        /// <param name="listaReporteCompensacionMensual"></param>
        /// <param name="ws"></param>
        public static void GenerarFormatoReporteExcel303Cuerpo(StRecalculoDTO entidadRecalculo, List<StFactorpagoDTO> listaReporteCompensacionMensual, ExcelWorksheet ws)
        {
            if (ws != null)
            {
                int index = 2;
                //TITULO
                ws.Cells[index, 3].Value = "Reportes 303 – Compensación Mensual";
                ws.Cells[index + 1, 3].Value = entidadRecalculo.Stpernombre + "/" + entidadRecalculo.Strecanombre + "";
                ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                rg.Style.Font.Size = 16;
                rg.Style.Font.Bold = true;

                //CABECERA DE TABLA
                index += 4;
                ws.Cells[index, 2].Value = "CENTRAL";
                ws.Cells[index, 3].Value = "MI S/";
                ws.Cells[index, 4].Value = "FG Final %";
                ws.Cells[index, 5].Value = "CMG S/ asignado";
                ws.Column(3).Style.Numberformat.Format = "#,##0.0000";
                ws.Column(4).Style.Numberformat.Format = "#,##0.0000%";
                ws.Column(5).Style.Numberformat.Format = "#,##0.000000000000";

                rg = ws.Cells[index, 2, index, 5];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);

                rg = ws.Cells[index - 1, 2];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);

                index++;

                int col = 4;
                int col2 = 5;
                int count = 4;
                int colTotal = 3;
                decimal[] TotalColum = new decimal[1000];
                decimal[] TotalColum2 = new decimal[1000];

                var codelement = "";
                foreach (var item in listaReporteCompensacionMensual)
                {
                    if (codelement == "") { codelement = item.Stcompcodelemento; ws.Cells[index - 2, 2].Value = item.Stcompcodelemento.ToString(); }
                    else if (codelement != item.Stcompcodelemento)
                    {
                        for (int i = 4; i < 5; i++)
                        {
                            ws.Cells[index, i].Value = TotalColum[i];
                        }
                        for (int i = 5; i < 6; i++)
                        {
                            ws.Cells[index, i].Value = TotalColum2[i];
                        }
                        ws.Cells[index, colTotal - 1].Value = "TOTAL";
                        rg = ws.Cells[index, 2, index, 5];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index, 3, index, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        count = 4;
                        codelement = item.Stcompcodelemento;
                        index += 3;

                        ws.Cells[index - 1, 2].Value = item.Stcompcodelemento.ToString();
                        ws.Cells[index, 2].Value = "CENTRAL";
                        ws.Cells[index, 3].Value = "MI S/";
                        ws.Cells[index, 4].Value = "FG Final %";
                        ws.Cells[index, 5].Value = "CMG S/ asignado";
                        ws.Column(3).Style.Numberformat.Format = "#,##0.0000";
                        ws.Column(4).Style.Numberformat.Format = "#,##0.0000%";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.000000000000";

                        rg = ws.Cells[index, 2, index, 5];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        rg = ws.Cells[index - 1, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        index++;

                        TotalColum = new decimal[1000];
                        TotalColum2 = new decimal[1000];
                    }
                    //ws.Cells[index - 2, 2].Value = item.Stcompcodelemento.ToString();
                    ws.Cells[index, 2].Value = item.Equinomb.ToString();
                    ws.Cells[index, 3].Value = item.Elecmpmonto;
                    ws.Cells[index, 4].Value = item.Facpagfgglajuste;
                    ws.Cells[index, 5].Value = item.Pagasgcmggl;

                    TotalColum[col] += item.Facpagfgglajuste;
                    TotalColum2[col2] += item.Pagasgcmggl;
                    count++;
                    //Border por celda
                    rg = ws.Cells[index, 2, index, 5];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);
                    rg = ws.Cells[index, 3, index, 5];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    index++;
                }
                for (int i = 4; i < 5; i++)
                {
                    ws.Cells[index, i].Value = TotalColum[i];
                }
                for (int i = 5; i < 6; i++)
                {
                    ws.Cells[index, i].Value = TotalColum2[i];
                }
                ws.Cells[index, colTotal - 1].Value = "TOTAL";
                rg = ws.Cells[index, 2, index, 5];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);
                rg = ws.Cells[index, 3, index, 5];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                index++;

                ws.Column(2).Width = 40;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                //Insertar el logo
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                picture.SetPosition(10, 10);
                picture.SetSize(135, 45);
            }
        }

        /// <summary> 
        /// CU09 lo seReportes Distancias Eléctricas 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaReporteDistElectrGenele">Lista de registros de StDistelectricaGeneleDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoReporteDistElec(string fileName, StRecalculoDTO EntidadRecalculo, List<StDistelectricaGeneleDTO> ListaReporteDistElectrGenele)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    int columastotales;
                    //TITULO
                    ws.Cells[index, 3].Value = "Distancias Eléctricas";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    if (ListaReporteDistElectrGenele.Count != 0)
                    {
                        index += 3;
                        int col = 3;
                        var CodElem = "";
                        string[] Lista = new string[1000];
                        int valorenlista = 0;
                        ws.Cells[index, 2].Value = "Elemento";
                        ws.Cells[index + 1, 2].Value = "CODIGO";
                        foreach (var item in ListaReporteDistElectrGenele)
                        {
                            if (CodElem == "") CodElem = item.Cmpmelcodelemento;

                            else if (CodElem != item.Cmpmelcodelemento)
                            {
                                CodElem = item.Cmpmelcodelemento;
                            }
                            if (Lista.Contains(item.Equinomb))
                            {
                                continue;
                            }
                            Lista[valorenlista] = item.Equinomb.ToString();
                            valorenlista++;

                            ws.Cells[index + 1, col].Value = item.Equinomb;
                            ws.Column(col).Width = 15;
                            col = col + 1;
                        }
                        ws.Cells[index, 3, index, col - 1].Merge = true;
                        ws.Cells[index, 3, index, col - 1].Value = "GENERADOR";
                        rg = ws.Cells[index, 2, index + 1, col - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        columastotales = col - 1;
                        index = index + 2;
                        col = 3;
                        int colTotal = 3;
                        var CodElem2 = "";
                        decimal[] TotalColum = new decimal[1000];

                        foreach (var item2 in ListaReporteDistElectrGenele)
                        {
                            if (CodElem2 == "")
                            {
                                CodElem2 = item2.Cmpmelcodelemento;
                                ws.Cells[index, 2].Value = item2.Cmpmelcodelemento;
                            }
                            else if (CodElem2 != item2.Cmpmelcodelemento)
                            {
                                index++;
                                col = 3;
                                CodElem2 = item2.Cmpmelcodelemento;
                                ws.Cells[index, 2].Value = item2.Cmpmelcodelemento;                               
                            }
                            ws.Cells[index, col].Value = item2.Degeledistancia;

                            TotalColum[col] += item2.Degeledistancia;
                            ws.Column(col).Style.Numberformat.Format = "#,##0.0000000000000000";
                            //Border por celda
                            col++;
                            rg = ws.Cells[index, 2, index, columastotales];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, columastotales];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        for (int i = 3; i < columastotales + 1; i++)
                        {
                            ws.Cells[index + 1, i].Value = TotalColum[i];
                        }
                        ws.Cells[index + 1, colTotal - 1].Value = "TOTAL";
                        rg = ws.Cells[index + 1, 2, index + 1, columastotales];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index + 1, 3, index + 1, columastotales];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        /// <summary> 
        /// CU10 Reportes Factor de Participación
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaFactorPago">Lista de registros de StFactorpagoDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoFactorParticipacion(string fileName, StRecalculoDTO EntidadRecalculo, List<StFactorpagoDTO> ListaFactorPago)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "Factor de Participación Inicial";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    if (ListaFactorPago.Count != 0)
                    {
                        index += 3;
                        int col = 3;
                        int ColumnaTotal = 0;
                        var CodElem = "";
                        string[] Lista = new string[1000];// Central
                        string[] ListaId = new string[1000];// ID Central
                        int valorenlista = 0;
                        int valorenlistaId = 0;
                        ws.Cells[index + 1, 2].Value = "CODIGO";
                        ws.Cells[index, 2].Value = "Elementos";
                        ws.Column(2).Width = 20;
                        foreach (var item in ListaFactorPago)
                        {
                            if (CodElem == "") CodElem = item.Stcompcodelemento;

                            else if (CodElem != item.Stcompcodelemento)
                            {
                                CodElem = item.Stcompcodelemento;
                            }
                            if (Lista.Contains(item.Equinomb))
                            {
                                continue;
                            }
                            Lista[valorenlista] = item.Equinomb.ToString();
                            valorenlista++;
                            ListaId[valorenlistaId] = item.Stcntgcodi.ToString();
                            valorenlistaId++;

                            ws.Cells[index + 1, col].Value = item.Equinomb;
                            ws.Column(col).Style.Numberformat.Format = "##0.0000";
                            ws.Column(col).Width = 15;
                            col = col + 1;
                        }
                        ws.Cells[index, 3, index, col - 1].Merge = true;
                        ws.Cells[index, 3, index, col - 1].Value = "GENERADOR";
                        ws.Cells[index + 1, col].Value = "TOTAL";
                        ws.Column(col).Style.Numberformat.Format = "#,##0.00%";
                        rg = ws.Cells[index, 2, index + 1, col];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        ColumnaTotal = col;

                        index = index + 2;
                        col = 3;
                        int fila = index;
                        var CodElem2 = "";
                        decimal[] TotalColum = new decimal[1000];

                        foreach (var item2 in ListaFactorPago)
                        {
                            for (int i = 0; i < Lista.Count(); i++)
                            {
                                if (ListaId[i] == null)
                                {
                                    break;
                                }
                                if (item2.Stcntgcodi.ToString() == ListaId[i].ToString())
                                {
                                    if (CodElem2 == "")
                                    {
                                        CodElem2 = item2.Stcompcodelemento;
                                        ws.Cells[index, 2].Value = item2.Stcompcodelemento;
                                    }
                                    else if (CodElem2 != item2.Stcompcodelemento)
                                    {
                                        index++;
                                        col = 3;
                                        fila++;
                                        CodElem2 = item2.Stcompcodelemento;
                                        ws.Cells[index, 2].Value = item2.Stcompcodelemento;
                                    }
                                    ws.Cells[index, i + 3].Value = item2.Facpagfggl;
                                    TotalColum[fila] += item2.Facpagfggl;
                                    //Border por celda
                                    rg = ws.Cells[index, 2, index, ColumnaTotal];
                                    rg.Style.WrapText = true;
                                    rg = ObtenerEstiloCelda(rg, 0);
                                    rg = ws.Cells[index, 3, index, ColumnaTotal];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                }
                            }
                        }
                        for (int i = 7; i <= fila; i++)
                        {
                            ws.Cells[i, ColumnaTotal].Value = TotalColum[i];
                            rg = ws.Cells[i, ColumnaTotal];
                            rg = ObtenerEstiloCelda(rg, 1);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
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
                xlPackage.Save();
            }
        }

        /// <summary> 
        /// CU11 Reportes Factor de Participación Recalculado 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaFactorPago">Lista de registros de StFactorpagoDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoFactorParticipacionRecalculado(string fileName, StRecalculoDTO EntidadRecalculo, List<StFactorpagoDTO> ListaFactorPago)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "Factor de Participación Final";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    if (ListaFactorPago.Count != 0)
                    {
                        index += 3;
                        int col = 3;
                        int ColumnaTotal = 0;
                        var CodElem = "";
                        string[] Lista = new string[1000];// Central
                        string[] ListaId = new string[1000];// ID Central
                        int valorenlista = 0;
                        int valorenlistaId = 0;
                        ws.Cells[index + 1, 2].Value = "CODIGO";
                        ws.Cells[index, 2].Value = "Elementos";
                        ws.Column(2).Width = 20;
                        foreach (var item in ListaFactorPago)
                        {
                            if (CodElem == "") CodElem = item.Stcompcodelemento;

                            else if (CodElem != item.Stcompcodelemento)
                            {
                                CodElem = item.Stcompcodelemento;
                            }
                            if (Lista.Contains(item.Equinomb))
                            {
                                continue;
                            }
                            Lista[valorenlista] = item.Equinomb.ToString();
                            valorenlista++;
                            ListaId[valorenlistaId] = item.Stcntgcodi.ToString();
                            valorenlistaId++;

                            ws.Cells[index + 1, col].Value = item.Equinomb;
                            ws.Column(col).Style.Numberformat.Format = "##0.0000";
                            ws.Column(col).Width = 15;
                            col = col + 1;
                        }
                        ws.Cells[index, 3, index, col - 1].Merge = true;
                        ws.Cells[index, 3, index, col - 1].Value = "GENERADOR";
                        ws.Cells[index + 1, col].Value = "TOTAL";
                        ws.Column(col).Style.Numberformat.Format = "#,##0.00%";
                        rg = ws.Cells[index, 2, index + 1, col];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        ColumnaTotal = col;

                        index = index + 2;
                        col = 3;
                        int fila = index;
                        var CodElem2 = "";
                        decimal[] TotalColum = new decimal[1000];

                        foreach (var item2 in ListaFactorPago)
                        {
                            for (int i = 0; i < Lista.Count(); i++)
                            {
                                if (ListaId[i] == null)
                                {
                                    break;
                                }
                                if (item2.Stcntgcodi.ToString() == ListaId[i].ToString())
                                {
                                    if (CodElem2 == "") { CodElem2 = item2.Stcompcodelemento; ws.Cells[index, 2].Value = item2.Stcompcodelemento; }
                                    else if (CodElem2 != item2.Stcompcodelemento) { index++; col = 3; fila++; CodElem2 = item2.Stcompcodelemento; ws.Cells[index, 2].Value = item2.Stcompcodelemento; }
                                    ws.Cells[index, i + 3].Value = item2.Facpagfgglajuste;
                                    //Border por celda
                                    TotalColum[fila] += item2.Facpagfgglajuste;

                                    rg = ws.Cells[index, 2, index, ColumnaTotal];
                                    rg.Style.WrapText = true;
                                    rg = ObtenerEstiloCelda(rg, 0);
                                    rg = ws.Cells[index, 3, index, ColumnaTotal];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                }
                            }
                        }
                        for (int i = 7; i <= fila; i++)
                        {
                            ws.Cells[i, ColumnaTotal].Value = TotalColum[i];
                            rg = ws.Cells[i, ColumnaTotal];
                            rg = ObtenerEstiloCelda(rg, 1);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
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
                xlPackage.Save();
            }
        }

        /// <summary> 
        /// CU12 Reportes Compensación Mensual Asignada 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaPagoAsignadoReporte">Lista de registros de StPagoasignadoDTO</param>
        /// <returns></returns>
        public static void GenerarReporteCompensacionMensual(string fileName, StRecalculoDTO EntidadRecalculo, List<StPagoasignadoDTO> ListaPagoAsignadoReporte)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "Compensación Mensual Asignada";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    if (ListaPagoAsignadoReporte.Count != 0)
                    {
                        index += 3;
                        int col = 3;
                        var CodElem = "";
                        int ColumnasTotal = 0;
                        string[] Lista = new string[1000];// Central
                        string[] ListaId = new string[1000];// ID Central
                        int valorenlista = 0;
                        int valorenlistaId = 0;
                        ws.Cells[index + 1, 2].Value = "CODIGO";
                        ws.Cells[index, 2].Value = "Elementos";
                        ws.Column(2).Width = 20;
                        foreach (var item in ListaPagoAsignadoReporte)
                        {
                            if (CodElem == "") CodElem = item.Stcompcodelemento;

                            else if (CodElem != item.Stcompcodelemento)
                            {
                                CodElem = item.Stcompcodelemento;
                            }
                            if (Lista.Contains(item.Equinomb))
                            {
                                continue;
                            }
                            Lista[valorenlista] = item.Equinomb.ToString();
                            valorenlista++;
                            ListaId[valorenlistaId] = item.Stcntgcodi.ToString();
                            valorenlistaId++;

                            ws.Cells[index + 1, col].Value = item.Equinomb;
                            ws.Column(col).Width = 15;
                            col = col + 1;
                        }
                        ws.Cells[index, 3, index, col - 1].Merge = true;
                        ws.Cells[index, 3, index, col - 1].Value = "GENERADOR";
                        rg = ws.Cells[index, 2, index + 1, col - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        ColumnasTotal = col - 1;

                        index = index + 2;
                        col = 3;
                        int colTotal = 3;
                        var CodElem2 = "";
                        decimal[] TotalColum = new decimal[1000];

                        foreach (var item2 in ListaPagoAsignadoReporte)
                        {
                            for (int i = 0; i < Lista.Count(); i++)
                            {
                                if (ListaId[i] == null)
                                {
                                    break;
                                }
                                if (item2.Stcntgcodi.ToString() == ListaId[i].ToString())
                                {
                                    if (CodElem2 == "")
                                    {
                                        CodElem2 = item2.Stcompcodelemento;
                                        ws.Cells[index, 2].Value = item2.Stcompcodelemento;
                                    }
                                    else if (CodElem2 != item2.Stcompcodelemento)
                                    {
                                        index++; col = 3;
                                        CodElem2 = item2.Stcompcodelemento;
                                        ws.Cells[index, 2].Value = item2.Stcompcodelemento;
                                    }
                                    ws.Cells[index, i + 3].Value = item2.Pagasgcmggl;
                                    //Border por celda
                                    TotalColum[i + 3] += item2.Pagasgcmggl;

                                    rg = ws.Cells[index, 2, index, ColumnasTotal];
                                    rg.Style.WrapText = true;
                                    rg = ObtenerEstiloCelda(rg, 0);
                                    rg = ws.Cells[index, 2, index, ColumnasTotal];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                }
                            }
                        }
                        for (int i = 3; i < valorenlistaId + 3; i++)
                        {
                            ws.Cells[index + 1, i].Value = TotalColum[i];
                        }
                        ws.Cells[index + 1, colTotal - 1].Value = "TOTAL";
                        rg = ws.Cells[index + 1, 2, index + 1, ColumnasTotal];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index + 1, 3, index + 1, ColumnasTotal];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        /// <summary> 
        /// CU13 Reportes Compensación Mensual Filtrada CU13
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaPagoAsignadoReporteFiltrado">Lista de registros de StPagoasignadoDTO</param>
        /// <returns></returns>
        public static void GenerarReporteCompensacionMensualFiltrada(string fileName, StRecalculoDTO EntidadRecalculo, List<StPagoasignadoDTO> ListaPagoAsignadoReporteFiltrado)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "Compensación Mensual Filtrada";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    if (ListaPagoAsignadoReporteFiltrado.Count != 0)
                    {
                        index += 3;
                        int col = 3;
                        var CodElem = "";
                        int ColumnasTotal = 0;
                        string[] Lista = new string[1000];// Central
                        string[] ListaId = new string[1000];// ID Central
                        int valorenlista = 0;
                        int valorenlistaId = 0;
                        ws.Cells[index + 1, 2].Value = "CODIGO";
                        ws.Cells[index, 2].Value = "Elementos";
                        ws.Column(2).Width = 20;
                        foreach (var item in ListaPagoAsignadoReporteFiltrado)
                        {
                            if (CodElem == "") CodElem = item.Stcompcodelemento;

                            else if (CodElem != item.Stcompcodelemento)
                            {
                                CodElem = item.Stcompcodelemento;
                            }
                            if (Lista.Contains(item.Equinomb))
                            {
                                continue;
                            }
                            Lista[valorenlista] = item.Equinomb.ToString();
                            valorenlista++;
                            ListaId[valorenlistaId] = item.Stcntgcodi.ToString();
                            valorenlistaId++;

                            ws.Cells[index + 1, col].Value = item.Equinomb;
                            ws.Column(col).Width = 15;
                            col = col + 1;
                        }
                        ws.Cells[index, 3, index, col - 1].Merge = true;
                        ws.Cells[index, 3, index, col - 1].Value = "GENERADOR";
                        rg = ws.Cells[index, 2, index + 1, col - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        ColumnasTotal = col - 1;

                        index = index + 2;
                        col = 3;
                        int colTotal = 3;
                        var CodElem2 = "";
                        decimal[] TotalColum = new decimal[1000];

                        foreach (var item2 in ListaPagoAsignadoReporteFiltrado)
                        {
                            for (int i = 0; i < Lista.Count(); i++)
                            {
                                if (ListaId[i] == null)
                                {
                                    break;
                                }
                                if (item2.Stcntgcodi.ToString() == ListaId[i].ToString())
                                {
                                    if (CodElem2 == "") { CodElem2 = item2.Stcompcodelemento; ws.Cells[index, 2].Value = item2.Stcompcodelemento; }
                                    else if (CodElem2 != item2.Stcompcodelemento) { index++; col = 3; CodElem2 = item2.Stcompcodelemento; ws.Cells[index, 2].Value = item2.Stcompcodelemento; }
                                    ws.Cells[index, i + 3].Value = item2.Pagasgcmgglfinal;
                                    //Border por celda
                                    TotalColum[i + 3] += item2.Pagasgcmgglfinal;

                                    rg = ws.Cells[index, 2, index, ColumnasTotal];
                                    rg.Style.WrapText = true;
                                    rg = ObtenerEstiloCelda(rg, 0);
                                    rg = ws.Cells[index, 2, index, ColumnasTotal];
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                }
                            }
                        }
                        for (int i = 3; i < valorenlistaId + 3; i++)
                        {
                            ws.Cells[index + 1, i].Value = TotalColum[i];
                        }
                        ws.Cells[index + 1, colTotal - 1].Value = "TOTAL";
                        rg = ws.Cells[index + 1, 2, index + 1, ColumnasTotal];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index + 1, 3, index + 1, ColumnasTotal];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        /// <summary> 
        /// CU14 Reportes Asignación de Responsabilidad de Pago de Sistemas Secundarios de Transmisión y Sistemas Complementarios de Transmisión por Parte de los Generadores por el Criterio de Uso
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaEmpresasGeneradores">Lista de registros de StPagoasignadoDTO</param>
        /// <param name="ListaEmpresasSistemas">Lista de registros de StPagoasignadoDTO</param>
        /// <returns></returns>
        public static void GenerarReporteResponsabilidadPago(string fileName, StRecalculoDTO EntidadRecalculo, List<StPagoasignadoDTO> ListaEmpresasGeneradores, List<StPagoasignadoDTO> ListaEmpresasSistemas)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    int strecacodi = EntidadRecalculo.Strecacodi;
                    //TITULO
                    ws.Cells[index, 3].Value = "Asignación de Responsabilidad de Pago de Sistemas Secundarios de Transmisión y Sistemas Complementarios de Transmisión por Parte de los Generadores por el Criterio de Uso ";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Stpernombre + "/" + EntidadRecalculo.Strecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 3].Value = "RUC";
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    ws.Cells[index, 3, index + 1, 3].Merge = true;
                    ws.Column(3).Width = 13;
                    ws.Column(2).Width = 40;
                    int col = 4;
                    decimal[] dTotalSistema = new decimal[1000];
                    foreach (var EmpresasSistema in ListaEmpresasSistemas)
                    {
                        //if(EmpresasSistema.Emprcodi == 12708)
                        //{
                            ws.Cells[index, col].Value = EmpresasSistema.Emprnomb;
                            ws.Cells[index + 1, col].Value = EmpresasSistema.Emprruc;
                            ws.Cells[index + 2, col].Value = EmpresasSistema.Sistrnnombre;
                            ws.Column(col).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(col).Width = 15;
                            dTotalSistema[col] = 0;
                            col++;
                        //}
                        
                    }
                    ws.Cells[index, col].Value = "TOTAL";
                    ws.Column(col).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, col, index + 2, col].Merge = true;
                    rg = ws.Cells[index, 2, index + 1, col];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    ws.Column(col).Width = 15;
                    index = index + 3;
                    foreach (var EmpresasGenerador in ListaEmpresasGeneradores)
                    {
                        ws.Cells[index, 3].Value = EmpresasGenerador.Emprruc.ToString();
                        ws.Cells[index, 2].Value = EmpresasGenerador.Emprnomb.ToString();
                        col = 4;
                        decimal dTotalGenerador = 0;
                        foreach (var dto in ListaEmpresasSistemas)
                        {
                            //if (dto.Emprcodi == 12708)
                            //{
                                decimal dPago = (new SistemasTransmisionAppServicio()).GetPagoStPagoasignadosGeneradorXSistema(strecacodi, EmpresasGenerador.Emprcodi, dto.Emprcodi, dto.Sistrncodi);
                                ws.Cells[index, col].Value = dPago;
                                dTotalGenerador += dPago;
                                dTotalSistema[col] += dPago;
                                col++;
                            //}
                        }
                        ws.Cells[index, col].Value = dTotalGenerador;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, col];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 4, index, col];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    col = 4;
                    decimal dTotal = 0;
                    ws.Cells[index, 2].Value = "TOTAL";
                    foreach (var dto2 in ListaEmpresasSistemas)
                    {
                        //if (dto2.Emprcodi == 12708)
                        //{
                            ws.Cells[index, col].Value = dTotalSistema[col];
                            dTotal += dTotalSistema[col];
                            col++;
                        //}
                    }
                    ws.Cells[index, col].Value = dTotal;
                    rg = ws.Cells[index, 2, index, col];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

    }
}
