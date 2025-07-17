using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.TransfPotencia.Helper
{
    class ExcelDocument
    {
        /// <summary>
        /// CU04 - Permite generar el archivo de exportación de la tabla VTP_REPA_RECA_PEAJE_DETALLE
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaRepaRecaPeaje">Lista de registros de VtpRepaRecaPeajeDTO</param>
        /// <param name="ListaRepaRecaPeajeDetalle">Lista de registros de VtpRepaRecaPeajeDetalleDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoRepaRecaPeajeDetalle(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpRepaRecaPeajeDTO> ListaRepaRecaPeaje, List<VtpRepaRecaPeajeDetalleDTO> ListaRepaRecaPeajeDetalle, int CantidadEmpresas)
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
                    ws.Cells[index, 3].Value = "DESARROLLO DE REPARTOS";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    int col = 0;
                    ws.Cells[index, 2].Value = "REPARTO";
                    ws.Cells[index, 3].Value = "TOTAL %";
                    ws.Column(3).Style.Numberformat.Format = "##0";
                    //LISTA DE COLUMNAS DE EMPRESAS
                    for (int i = 0; i < CantidadEmpresas; i++)
                    {
                        ws.Cells[index, (2 * i) + 4].Value = "EMPRESA";
                        ws.Cells[index, (2 * i) + 5].Value = "%";
                        ws.Column((2 * i) + 5).Style.Numberformat.Format = "##0";
                        col = (2 * i) + 5;
                    }
                    int iNroColumnas = col;
                    rg = ws.Cells[index, 2, index, iNroColumnas];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaRepaRecaPeaje)
                    {
                        ws.Cells[index, 2].Value = item.Rrpenombre.ToString();
                        int iPorcentajeTotal = 0;
                        col = 4;
                        foreach (VtpRepaRecaPeajeDetalleDTO itemD in ListaRepaRecaPeajeDetalle)
                        {
                            if (item.Rrpecodi == itemD.Rrpecodi)
                            {
                                EmpresaDTO dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(itemD.Emprcodi);
                                ws.Cells[index, col++].Value = dtoEmpresa.EmprNombre;
                                ws.Cells[index, col++].Value = itemD.Rrpdporcentaje;
                                iPorcentajeTotal += (int)itemD.Rrpdporcentaje;
                            }

                        }
                        ws.Cells[index, 3].Value = iPorcentajeTotal;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, iNroColumnas];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(6, 0);
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 10;
                    ws.Column(4).Width = 30;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 30;
                    ws.Column(7).Width = 10;
                    ws.Column(8).Width = 30;
                    ws.Column(9).Width = 10;
                    ws.Column(10).Width = 30;
                    ws.Column(11).Width = 10;
                    ws.Column(12).Width = 30;
                    ws.Column(13).Width = 10;
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
        /// CU05 - Permite generar el archivo de exportación de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeIngreso">Lista de registros de VtpPeajeIngresoDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoPeajeIngreso(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeIngresoDTO> ListaPeajeIngreso)
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
                    ws.Cells[index, 4].Value = "LISTA DE PEAJES E INGRESOS TARIFARIOS";
                    ws.Cells[index + 1, 4].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 4, index + 1, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "TIPO";
                    ws.Cells[index, 3].Value = "TITULAR";
                    ws.Cells[index, 4].Value = "NOMBRE";
                    ws.Cells[index, 5].Value = "PAGO";
                    ws.Cells[index, 6].Value = "TRANSMISIÓN";
                    ws.Cells[index, 7].Value = "REPARTO DE RECAUDACIÓN DE PEAJE";
                    ws.Cells[index, 8].Value = "CÓDIGO";
                    ws.Cells[index, 9].Value = "PEAJE MENSUAL S/";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.0000";
                    ws.Cells[index, 10].Value = "INGRESO TARIFARIO MENSUAL S/";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.0000";
                    ws.Cells[index, 11].Value = "REGULADO S/ / kW-mes";
                    ws.Column(11).Style.Numberformat.Format = "#,##0.000000000000";
                    ws.Cells[index, 12].Value = "LIBRE S/ / kW-mes";
                    ws.Column(12).Style.Numberformat.Format = "#,##0.000000000000";
                    ws.Cells[index, 13].Value = "GRAN USUARIO S/ / kW-mes";
                    ws.Column(13).Style.Numberformat.Format = "#,##0.000000000000";

                    rg = ws.Cells[index, 2, index, 13];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (VtpPeajeIngresoDTO item in ListaPeajeIngreso)
                    {
                        ws.Cells[index, 2].Value = item.Pingtipo;
                        ws.Cells[index, 3].Value = (item.Emprnomb != null) ? item.Emprnomb.ToString() : string.Empty;
                        ws.Cells[index, 4].Value = item.Pingnombre.ToString();
                        ws.Cells[index, 5].Value = item.Pingpago.ToString();
                        ws.Cells[index, 6].Value = item.Pingtransmision.ToString();
                        ws.Cells[index, 7].Value = (item.Rrpenombre != null) ? item.Rrpenombre.ToString() : string.Empty;
                        ws.Cells[index, 8].Value = (item.Pingcodigo != null) ? item.Pingcodigo : string.Empty;
                        ws.Cells[index, 9].Value = (item.Pingpeajemensual != null) ? item.Pingpeajemensual : Decimal.Zero;
                        ws.Cells[index, 10].Value = (item.Pingtarimensual != null) ? item.Pingtarimensual : Decimal.Zero;
                        ws.Cells[index, 11].Value = (item.Pingregulado != null) ? item.Pingregulado : Decimal.Zero;
                        ws.Cells[index, 12].Value = (item.Pinglibre != null) ? item.Pinglibre : Decimal.Zero;
                        ws.Cells[index, 13].Value = (item.Pinggranusuario != null) ? item.Pinggranusuario : Decimal.Zero;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 13];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 8, index, 13];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(6, 0);
                    ws.Column(2).Width = 8;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 40;
                    ws.Column(5).Width = 8;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 40;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 15;
                    ws.Column(12).Width = 15;
                    ws.Column(13).Width = 15;

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
        /// CU09 Información ingresada para VTP y peajes - Permite generar el archivo de exportación de la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeEgresoMinfo">Lista de registros de VtpIngresoPotefrDetalleDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoPeajeEgresoMinfoFormatoNuevo(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo, out ExcelWorksheet hoja)
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
                    ws.Cells[index, 3].Value = "INFORMACIÓN INGRESADA PARA VTP Y PEAJES";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[index + 2, 3].Value = "";
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 4;


                    index++;
                    ws.Cells[index, 2].Value = "CÓDIGO";
                    ws.Cells[index, 3].Value = "Empresa";

                    //ws.Cells[index, 7].Value = "PARA EGRESO DE POTENCIA";
                    //ws.Cells[index, 9].Value = "PARA PEAJE POR CONEXIÓN";
                    //ws.Cells[index, 12].Value = "PARA FLUJO DE CARGA OPTIMO";

                    //rg = ws.Cells[index, 7, index, 8];
                    //rg.Merge = true;
                    //rg.Style.WrapText = true;
                    //rg = ObtenerEstiloCelda(rg, 1);

                    //rg = ws.Cells[index, 9, index, 11];
                    //rg.Merge = true;
                    //rg.Style.WrapText = true;
                    //rg = ObtenerEstiloCelda(rg, 1);

                    //rg = ws.Cells[index, 12, index, 14];
                    //rg.Merge = true;
                    //rg.Style.WrapText = true;
                    //rg = ObtenerEstiloCelda(rg, 1);
                    //index++;


                    ws.Cells[index, 2].Value = "CODIGO VTP";
                    ws.Cells[index, 3].Value = "EMPRESA";

                    ws.Cells[index, 4].Value = "CLIENTE";
                    ws.Cells[index, 5].Value = "BARRA";
                    ws.Cells[index, 6].Value = "CONTRATO";
                    ws.Cells[index, 7].Value = "TIPO USUARIO";
                    ws.Cells[index, 8].Value = "PRECIO POTENCIA S/ /kW-mes";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 9].Value = "POTENCIA COINCIDENTE kW";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 10].Value = "POTENCIA DECLARADA (KW)";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 11].Value = "PEAJE UNITARIO S/ /kW-mes";
                    ws.Column(11).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 12].Value = "FACTOR PÉRDIDA";
                    ws.Column(12).Style.Numberformat.Format = "#,##0.000";

                    ws.Cells[index, 13].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, 13];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaPeajeEgresoMinfo)
                    {
                        ws.Cells[index, 2].Value = (item.Coregecodvtp != null) ? item.Coregecodvtp.ToString() : string.Empty;
                        ws.Cells[index, 3].Value = (item.Genemprnombre != null) ? item.Genemprnombre.ToString() : string.Empty;
                        ws.Cells[index, 4].Value = (item.Cliemprnombre != null) ? item.Cliemprnombre.ToString() : string.Empty;
                        ws.Cells[index, 5].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                        ws.Cells[index, 6].Value = (item.Pegrmilicitacion != null) ? item.Pegrmilicitacion.ToString() : string.Empty;
                        ws.Cells[index, 7].Value = (item.Pegrmitipousuario != null) ? item.Pegrmitipousuario.ToString() : string.Empty;
                        ws.Cells[index, 8].Value = (item.Pegrmipreciopote != null) ? item.Pegrmipreciopote : Decimal.Zero;
                        ws.Cells[index, 9].Value = (item.Pegrdpotecoincidente ?? 0) == 0 ? (item.Pegrmipoteegreso ?? Decimal.Zero) : item.Pegrdpotecoincidente ?? Decimal.Zero;
                        ws.Cells[index, 10].Value = (item.Pegrmipotedeclarada != null) ? item.Pegrmipotedeclarada : Decimal.Zero;
                        ws.Cells[index, 11].Value = (item.Pegrmipeajeunitario != null) ? item.Pegrmipeajeunitario : Decimal.Zero;
                        ws.Cells[index, 12].Value = (item.Pegrdfacperdida != null) ? item.Pegrdfacperdida : Decimal.Zero;
                        ws.Cells[index, 13].Value = (item.Pegrmicalidad != null) ? item.Pegrmicalidad.ToString() : string.Empty;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 13];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 30;
                    ws.Column(4).Width = 30;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 10;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 15;
                    ws.Column(12).Width = 20;
                    ws.Column(13).Width = 15;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                hoja = ws;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// CU09 Información ingresada para VTP y peajes - Permite generar el archivo de exportación de la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeEgresoMinfo">Lista de registros de VtpIngresoPotefrDetalleDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoPeajeEgresoMinfo(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo, out ExcelWorksheet hoja)
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
                    ws.Cells[index, 3].Value = "INFORMACIÓN INGRESADA PARA VTP Y PEAJES";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[index + 2, 3].Value = "";
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 4;
                    ws.Cells[index, 7].Value = "PARA EGRESO DE POTENCIA";
                    ws.Cells[index, 9].Value = "PARA PEAJE POR CONEXIÓN";
                    ws.Cells[index, 12].Value = "PARA FLUJO DE CARGA OPTIMO";

                    rg = ws.Cells[index, 7, index, 8];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    rg = ws.Cells[index, 9, index, 11];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    rg = ws.Cells[index, 12, index, 14];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    index++;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "CLIENTE";
                    ws.Cells[index, 4].Value = "BARRA";
                    ws.Cells[index, 5].Value = "TIPO USUARIO";
                    ws.Cells[index, 6].Value = "LICITACIÓN";
                    ws.Cells[index, 7].Value = "PRECIO POTENCIA S/ /kW-mes";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 8].Value = "POTENCIA EGRESO kW";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 9].Value = "POTENCIA CALCULADA (KW)";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 10].Value = "POTENCIA DECLARADA (KW)";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 11].Value = "PEAJE UNITARIO S/ /kW-mes";
                    ws.Column(11).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 12].Value = "FCO-BARRA";
                    ws.Cells[index, 13].Value = "FCO-POTENCIA ACTIVA kW";
                    ws.Column(13).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 14].Value = "FCO-POTENCIA REACTIVA kW";
                    ws.Column(14).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 15].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, 15];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaPeajeEgresoMinfo)
                    {
                        ws.Cells[index, 2].Value = (item.Genemprnombre != null) ? item.Genemprnombre.ToString() : string.Empty;
                        ws.Cells[index, 3].Value = (item.Cliemprnombre != null) ? item.Cliemprnombre.ToString() : string.Empty;
                        ws.Cells[index, 4].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                        ws.Cells[index, 5].Value = (item.Pegrmitipousuario != null) ? item.Pegrmitipousuario.ToString() : string.Empty;
                        ws.Cells[index, 6].Value = (item.Pegrmilicitacion != null) ? item.Pegrmilicitacion.ToString() : string.Empty;
                        ws.Cells[index, 7].Value = (item.Pegrmipreciopote != null) ? item.Pegrmipreciopote : Decimal.Zero;
                        ws.Cells[index, 8].Value = (item.Pegrmipoteegreso != null) ? item.Pegrmipoteegreso : Decimal.Zero;
                        ws.Cells[index, 9].Value = (item.Pegrmipotecalculada != null) ? item.Pegrmipotecalculada : Decimal.Zero;
                        ws.Cells[index, 10].Value = (item.Pegrmipotedeclarada != null) ? item.Pegrmipotedeclarada : Decimal.Zero;
                        ws.Cells[index, 11].Value = (item.Pegrmipeajeunitario != null) ? item.Pegrmipeajeunitario : Decimal.Zero;
                        ws.Cells[index, 12].Value = (item.Barrnombrefco != null) ? item.Barrnombrefco.ToString() : string.Empty;
                        ws.Cells[index, 13].Value = (item.Pegrmipoteactiva != null) ? item.Pegrmipoteactiva : Decimal.Zero;
                        ws.Cells[index, 14].Value = (item.Pegrmipotereactiva != null) ? item.Pegrmipotereactiva : Decimal.Zero;
                        ws.Cells[index, 15].Value = (item.Pegrmicalidad != null) ? item.Pegrmicalidad.ToString() : string.Empty;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 15];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 30;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 10;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 15;
                    ws.Column(12).Width = 20;
                    ws.Column(13).Width = 15;
                    ws.Column(14).Width = 15;
                    ws.Column(15).Width = 20;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                hoja = ws;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// CU10 Retiros de potencia sin contrato - Permite generar el archivo de exportación de la vista VTP_RETIRO_POTESC
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaRetiroPotenciaSC">Lista de registros de VtpRetiroPotescDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoRetiroPotenciaSC(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC)
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
                    ws.Cells[index, 3].Value = "LISTA DE RETIROS DE POTENCIA SIN CONTRATO";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 9].Value = "PARA FLUJO DE CARGA OPTIMO";
                    ws.Cells["I" + index.ToString() + ":K" + index.ToString() + ""].Merge = true; //(index,3,index,4)
                    rg = ws.Cells[index, 9, index, 11];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    index++;
                    ws.Cells[index, 2].Value = "CLIENTE";
                    ws.Cells[index, 3].Value = "BARRA";
                    ws.Cells[index, 4].Value = "TIPO USUARIO";
                    ws.Cells[index, 5].Value = "PRECIO PPB S/ /kW-mes";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 6].Value = "PRECIO POTENCIA S/ /kW-mes";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7].Value = "POTENCIA EGRESO kW";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 8].Value = "PEAJE UNITARIO S/ /kW-mes";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 9].Value = "FCO-BARRA";
                    ws.Cells[index, 10].Value = "FCO-POTENCIA ACTIVA kW";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 11].Value = "FCO-POTENCIA REACTIVA kW";
                    ws.Column(11).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 12].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, 12];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaRetiroPotenciaSC)
                    {
                        ws.Cells[index, 2].Value = item.Emprnomb.ToString();
                        ws.Cells[index, 3].Value = item.Barrnombre.ToString();
                        ws.Cells[index, 4].Value = item.Rpsctipousuario.ToString();
                        ws.Cells[index, 5].Value = (item.Rpscprecioppb != null) ? item.Rpscprecioppb : Decimal.Zero;
                        ws.Cells[index, 6].Value = (item.Rpscpreciopote != null) ? item.Rpscpreciopote : Decimal.Zero;
                        ws.Cells[index, 7].Value = (item.Rpscpoteegreso != null) ? item.Rpscpoteegreso : Decimal.Zero;
                        ws.Cells[index, 8].Value = (item.Rpscpeajeunitario != null) ? item.Rpscpeajeunitario : Decimal.Zero;
                        ws.Cells[index, 9].Value = (item.Barrnombrefco != null) ? item.Barrnombrefco : string.Empty;
                        ws.Cells[index, 10].Value = (item.Rpscpoteactiva != null) ? item.Rpscpoteactiva : Decimal.Zero;
                        ws.Cells[index, 11].Value = (item.Rpscpotereactiva != null) ? item.Rpscpotereactiva : Decimal.Zero;
                        ws.Cells[index, 12].Value = (item.Rpsccalidad != null) ? item.Rpsccalidad.ToString() : string.Empty;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 12];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5, index, 8];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg = ws.Cells[index, 10, index, 11];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 30;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 15;
                    ws.Column(12).Width = 20;

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
        /// CU11 - Permite generar el archivo de exportación de la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaIngresoPotefrDetalle">Lista de registros de VtpIngresoPotefrDetalleDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoIngresoPotefrDetalle(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpIngresoPotefrDetalleDTO> ListaIngresoPotefrDetalle, int intervalo)
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
                    ws.Cells[index, 3].Value = "DATOS DE POTENCIA EFECTIVA, FIRME Y FIRME REMUNERABLE";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre + "/ Intervalo" + intervalo;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "CENTRAL / UNIDAD GENERACIÓN";
                    ws.Cells[index, 4].Value = "POT. EFECTIVA kW";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 5].Value = "POT. FIRME kW";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 6].Value = "POT FIRME REMUNERABLE kW";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";

                    rg = ws.Cells[index, 2, index, 6];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaIngresoPotefrDetalle)
                    {
                        ws.Cells[index, 2].Value = item.Emprnomb.ToString();
                        ws.Cells[index, 3].Value = item.Cenequinomb.ToString();
                        ws.Cells[index, 4].Value = (item.Ipefrdpoteefectiva != null) ? item.Ipefrdpoteefectiva : Decimal.Zero;
                        ws.Cells[index, 5].Value = (item.Ipefrdpotefirme != null) ? item.Ipefrdpotefirme : Decimal.Zero;
                        ws.Cells[index, 6].Value = (item.Ipefrdpotefirmeremunerable != null) ? item.Ipefrdpotefirmeremunerable : Decimal.Zero;

                        //Border por celda
                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 20;
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
        /// Permite generar el archivo de exportación de la tabla VTP_INGRESO_POTEFR_DETALLE con datos de PFR 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaIngresoPotefrDetalle">Lista de registros de VtpIngresoPotefrDetalleDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoIngresoPotefrDetallePfr(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpIngresoPotefrDetalleDTO> ListaIngresoPotefrDetalle, int intervalo)
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
                    ws.Cells[index, 3].Value = "DATOS DE POTENCIA EFECTIVA, FIRME Y FIRME REMUNERABLE";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre + "/ Intervalo" + intervalo;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "CENTRAL";
                    ws.Cells[index, 4].Value = "UNIDAD GENERACIÓN";
                    ws.Cells[index, 5].Value = "POT. EFECTIVA kW";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 6].Value = "POT. FIRME kW";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7].Value = "POT FIRME REMUNERABLE kW";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";

                    rg = ws.Cells[index, 2, index, 7];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaIngresoPotefrDetalle)
                    {
                        ws.Cells[index, 2].Value = item.Emprnomb.ToString();
                        ws.Cells[index, 3].Value = item.Cenequinomb.ToString();
                        ws.Cells[index, 4].Value = item.Ipefrdunidadnomb ?? "";
                        ws.Cells[index, 5].Value = (item.Ipefrdpoteefectiva != null) ? item.Ipefrdpoteefectiva : Decimal.Zero;
                        ws.Cells[index, 6].Value = (item.Ipefrdpotefirme != null) ? item.Ipefrdpotefirme : Decimal.Zero;
                        ws.Cells[index, 7].Value = (item.Ipefrdpotefirmeremunerable != null) ? item.Ipefrdpotefirmeremunerable : Decimal.Zero;

                        //Border por celda
                        rg = ws.Cells[index, 2, index, 7];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 40;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 20;
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
        /// CU14 - Permite generar el archivo de exportación de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="EntidadPeajeEgreso">Entidad de VtpPeajeEgresoDTO</param>
        /// <param name="ListaPeajeEgresoDetalle">Lista de registros de VtpPeajeEgresoDetalleDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoPeajeEgreso(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<CodigoRetiroGeneradoDTO> ListaCodigoRetiroGenerado, List<EmpresaDTO> ListaEmpresas, List<BarraDTO> ListaBarras)
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
                    ws.Cells[index, 3].Value = "INGRESO PARA VTP Y PEAJES";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    if (EntidadRecalculoPotencia != null)
                    { ws.Cells[index + 2, 3].Value = "Código de envío: " + EntidadRecalculoPotencia.Perinombre.ToString() + ", Fecha envío: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); }
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 4;

                    ws.Cells[index, 2].Value = "CODIGO VTP";
                    ws.Cells[index, 3].Value = "CLIENTE";
                    ws.Cells[index, 4].Value = "BARRA";
                    ws.Cells[index, 5].Value = "CONTRATO";
                    ws.Cells[index, 6].Value = "USUARIO";
                    ws.Cells[index, 7].Value = "PRECIO POTENCIA S/ /kW-mes";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 8].Value = "POTENCIA COINCIDENTE kW";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 9].Value = "POTENCIA DECLARADA (KW)";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 10].Value = "PEAJE UNITARIO S/KW MES";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 11].Value = "FACTOR PÉRDIDA";
                    ws.Column(11).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 12].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, 12];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaCodigoRetiroGenerado)
                    {
                        ws.Cells[index, 2].Value = (item.CoregeCodVTP != null) ? item.CoregeCodVTP.ToString() : string.Empty;
                        ws.Cells[index, 3].Value = (item.Emprnomb != null) ? item.Emprnomb.ToString() : string.Empty;
                        ws.Cells[index, 4].Value = (item.BarrNombre != null) ? item.BarrNombre.ToString() : string.Empty;
                        ws.Cells[index, 5].Value = (item.Tipconnombre != null) ? item.Tipconnombre.ToString() : string.Empty;
                        ws.Cells[index, 6].Value = (item.Tipusunombre != null) ? item.Tipusunombre.ToString() : string.Empty;
                        ws.Cells[index, 7].Value = (item.Pegrdpotecalculada != null) ? item.Pegrdpotecalculada : Decimal.Zero;
                        ws.Cells[index, 8].Value = (item.Pegrdpotecoincidente != null) ? item.Pegrdpotecoincidente : Decimal.Zero;
                        ws.Cells[index, 9].Value = (item.Pegrdpotedeclarada != null) ? item.Pegrdpotedeclarada : Decimal.Zero;
                        ws.Cells[index, 10].Value = (item.Pegrdpeajeunitario != null) ? item.Pegrdpotedeclarada : Decimal.Zero;
                        ws.Cells[index, 11].Value = (item.Pegrdfacperdida != null) ? item.Pegrdpeajeunitario : Decimal.Zero;
                        ws.Cells[index, 12].Value = (item.Pegrdcalidad != null) ? item.Pegrdcalidad.ToString() : string.Empty;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 12];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 10;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 20;
                    ws.Column(12).Width = 15;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("EMPRESAS");
                if (ws3 != null)
                {   //TITULO
                    ws3.Cells[2, 4].Value = "LISTA DE EMPRESAS";
                    ExcelRange rg = ws3.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws3.Cells[5, 2].Value = "EMPRESA";
                    rg = ws3.Cells[5, 2, 5, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in ListaEmpresas)
                    {
                        ws3.Cells[row, 2].Value = (item.EmprNombre != null) ? item.EmprNombre : string.Empty;
                        //Border por celda
                        rg = ws3.Cells[row, 2, row, 2];
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

                    //Fijar panel
                    ws3.View.FreezePanes(6, 3);
                    rg = ws3.Cells[5, 2, row, 2];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws3.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }

                ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("BARRAS");
                if (ws4 != null)
                {   //TITULO
                    ws4.Cells[2, 4].Value = "LISTA DE BARRAS";
                    ExcelRange rg = ws4.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws4.Cells[5, 2].Value = "BARRA";
                    ws4.Cells[5, 3].Value = "TENSIÓN";
                    ws4.Column(3).Style.Numberformat.Format = "#.#";
                    ws4.Cells[5, 4].Value = "PUNTO DE SUMINISTRO";
                    ws4.Cells[5, 5].Value = "BARRA BGR";
                    ws4.Cells[5, 6].Value = "ÁREA";

                    rg = ws4.Cells[5, 2, 5, 6];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in ListaBarras)
                    {
                        ws4.Cells[row, 2].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws4.Cells[row, 3].Value = (item.BarrTension != null) ? item.BarrTension.ToString() : string.Empty;
                        ws4.Cells[row, 4].Value = (item.BarrPuntoSumirer != null) ? item.BarrPuntoSumirer.ToString() : string.Empty;
                        ws4.Cells[row, 5].Value = (item.BarrBarraBgr != null) ? item.BarrBarraBgr.ToString() : string.Empty;
                        ws4.Cells[row, 6].Value = (item.AreaNombre != null) ? item.AreaNombre.ToString() : string.Empty;
                        //Border por celda
                        rg = ws4.Cells[row, 2, row, 6];
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

                    //Fijar panel
                    ws4.View.FreezePanes(6, 7);
                    rg = ws4.Cells[5, 2, row, 6];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws4.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }
                xlPackage.Save();
            }
        }


        /// <summary>
        /// CU14 - Permite generar el archivo de exportación de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="EntidadPeajeEgreso">Entidad de VtpPeajeEgresoDTO</param>
        /// <param name="ListaPeajeEgresoDetalle">Lista de registros de VtpPeajeEgresoDetalleDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoPeajeEgresoExtranet(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<CodigoRetiroGeneradoDTO> ListaCodigoRetiroGenerado, List<EmpresaDTO> ListaEmpresas, List<BarraDTO> ListaBarras)
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
                    ws.Cells[index, 3].Value = "INGRESO PARA VTP Y PEAJES";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    if (EntidadRecalculoPotencia != null)
                    { ws.Cells[index + 2, 3].Value = "Código de envío: " + EntidadRecalculoPotencia.Perinombre.ToString() + ", Fecha envío: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); }
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 4;

                    ws.Cells[index, 2].Value = "CODIGO VTP";
                    ws.Cells[index, 3].Value = "CLIENTE";
                    ws.Cells[index, 4].Value = "BARRA";
                    ws.Cells[index, 5].Value = "CONTRATO";
                    ws.Cells[index, 6].Value = "USUARIO";
                    ws.Cells[index, 7].Value = "PRECIO POTENCIA S/ /kW-mes";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 8].Value = "POTENCIA COINCIDENTE kW";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 9].Value = "POTENCIA DECLARADA (KW)";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 10].Value = "PEAJE UNITARIO S/KW MES";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 11].Value = "FACTOR PÉRDIDA";
                    ws.Column(11).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 12].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, 12];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaCodigoRetiroGenerado)
                    {
                        ws.Cells[index, 2].Value = (item.CoregeCodVTP != null) ? item.CoregeCodVTP.ToString() : string.Empty;
                        ws.Cells[index, 3].Value = (item.Emprnomb != null) ? item.Emprnomb.ToString() : string.Empty;
                        ws.Cells[index, 4].Value = (item.BarrNombre != null) ? item.BarrNombre.ToString() : string.Empty;
                        ws.Cells[index, 5].Value = (item.Tipconnombre != null) ? item.Tipconnombre.ToString() : string.Empty;
                        ws.Cells[index, 6].Value = (item.Tipusunombre != null) ? item.Tipusunombre.ToString() : string.Empty;
                        ws.Cells[index, 7].Value = (item.Pegrdpotecalculada != null) ? item.Pegrdpotecalculada : Decimal.Zero;
                        ws.Cells[index, 8].Value = (item.Pegrdpotecoincidente != null) ? item.Pegrdpotecoincidente : Decimal.Zero;
                        ws.Cells[index, 9].Value = (item.Pegrdpotedeclarada != null) ? item.Pegrdpotedeclarada : Decimal.Zero;
                        ws.Cells[index, 10].Value = (item.Pegrdpeajeunitario != null) ? item.Pegrdpotedeclarada : Decimal.Zero;
                        ws.Cells[index, 11].Value = (item.Pegrdfacperdida != null) ? item.Pegrdpeajeunitario : Decimal.Zero;
                        ws.Cells[index, 12].Value = (item.Pegrdcalidad != null) ? item.Pegrdcalidad.ToString() : string.Empty;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 12];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 10;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 20;
                    ws.Column(12).Width = 15;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("EMPRESAS");
                if (ws3 != null)
                {   //TITULO
                    ws3.Cells[2, 4].Value = "LISTA DE EMPRESAS";
                    ExcelRange rg = ws3.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws3.Cells[5, 2].Value = "EMPRESA";
                    rg = ws3.Cells[5, 2, 5, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in ListaEmpresas)
                    {
                        ws3.Cells[row, 2].Value = (item.EmprNombre != null) ? item.EmprNombre : string.Empty;
                        //Border por celda
                        rg = ws3.Cells[row, 2, row, 2];
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

                    //Fijar panel
                    ws3.View.FreezePanes(6, 3);
                    rg = ws3.Cells[5, 2, row, 2];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws3.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }

                ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("BARRAS");
                if (ws4 != null)
                {   //TITULO
                    ws4.Cells[2, 4].Value = "LISTA DE BARRAS";
                    ExcelRange rg = ws4.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws4.Cells[5, 2].Value = "BARRA";
                    ws4.Cells[5, 3].Value = "TENSIÓN";
                    ws4.Column(3).Style.Numberformat.Format = "#.#";
                    ws4.Cells[5, 4].Value = "PUNTO DE SUMINISTRO";
                    ws4.Cells[5, 5].Value = "BARRA BGR";
                    ws4.Cells[5, 6].Value = "ÁREA";

                    rg = ws4.Cells[5, 2, 5, 6];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in ListaBarras)
                    {
                        ws4.Cells[row, 2].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws4.Cells[row, 3].Value = (item.BarrTension != null) ? item.BarrTension.ToString() : string.Empty;
                        ws4.Cells[row, 4].Value = (item.BarrPuntoSumirer != null) ? item.BarrPuntoSumirer.ToString() : string.Empty;
                        ws4.Cells[row, 5].Value = (item.BarrBarraBgr != null) ? item.BarrBarraBgr.ToString() : string.Empty;
                        ws4.Cells[row, 6].Value = (item.AreaNombre != null) ? item.AreaNombre.ToString() : string.Empty;
                        //Border por celda
                        rg = ws4.Cells[row, 2, row, 6];
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

                    //Fijar panel
                    ws4.View.FreezePanes(6, 7);
                    rg = ws4.Cells[5, 2, row, 6];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws4.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }
                xlPackage.Save();
            }
        }


        public static void GenerarFormatoPeajeEgresoAnterior(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, VtpPeajeEgresoDTO EntidadPeajeEgreso, List<VtpPeajeEgresoDetalleDTO> ListaPeajeEgresoDetalle, List<EmpresaDTO> ListaEmpresas, List<BarraDTO> ListaBarras)
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
                    ws.Cells[index, 3].Value = "INGRESO PARA VTP Y PEAJES";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    if (EntidadPeajeEgreso != null)
                    { ws.Cells[index + 2, 3].Value = "Código de envío: " + EntidadPeajeEgreso.Pegrcodi.ToString() + ", Fecha envío: " + EntidadPeajeEgreso.Pegrfeccreacion.ToString("dd/MM/yyyy HH:mm:ss"); }
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 4;
                    ws.Cells[index, 6].Value = "PARA EGRESO DE POTENCIA";
                    ws.Cells[index, 8].Value = "PARA PEAJE POR CONEXIÓN";
                    ws.Cells[index, 11].Value = "PARA FLUJO DE CARGA OPTIMO";

                    rg = ws.Cells[index, 6, index, 7];
                    ws.Cells[index, 6, index, 7].Merge = true; //(index,3,index,4)
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    rg = ws.Cells[index, 8, index, 10];
                    ws.Cells[index, 8, index, 10].Merge = true; //(index,3,index,4)                      
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    rg = ws.Cells[index, 11, index, 13];
                    ws.Cells[index, 11, index, 13].Merge = true; //(index,3,index,4)
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    index++;
                    ws.Cells[index, 2].Value = "CLIENTE";
                    ws.Cells[index, 3].Value = "BARRA";
                    ws.Cells[index, 4].Value = "TIPO USUARIO";
                    ws.Cells[index, 5].Value = "LICITACIÓN";
                    ws.Cells[index, 6].Value = "PRECIO POTENCIA S/ /kW-mes";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7].Value = "POTENCIA EGRESO kW";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 8].Value = "POTENCIA CALCULADA (KW)";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 9].Value = "POTENCIA DECLARADA (KW)";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 10].Value = "PEAJE UNITARIO S/ /kW-mes";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 11].Value = "FCO-BARRA";
                    ws.Cells[index, 12].Value = "FCO-POTENCIA ACTIVA kW";
                    ws.Column(12).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 13].Value = "FCO-POTENCIA REACTIVA kW";
                    ws.Column(13).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 14].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, 14];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaPeajeEgresoDetalle)
                    {
                        ws.Cells[index, 2].Value = (item.Emprnomb != null) ? item.Emprnomb.ToString() : string.Empty;
                        ws.Cells[index, 3].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                        ws.Cells[index, 4].Value = (item.Pegrdtipousuario != null) ? item.Pegrdtipousuario.ToString() : string.Empty;
                        ws.Cells[index, 5].Value = (item.Pegrdlicitacion != null) ? item.Pegrdlicitacion.ToString() : string.Empty;
                        ws.Cells[index, 6].Value = (item.Pegrdpreciopote != null) ? item.Pegrdpreciopote : Decimal.Zero;
                        ws.Cells[index, 7].Value = (item.Pegrdpoteegreso != null) ? item.Pegrdpoteegreso : Decimal.Zero;
                        ws.Cells[index, 8].Value = (item.Pegrdpotecalculada != null) ? item.Pegrdpotecalculada : Decimal.Zero;
                        ws.Cells[index, 9].Value = (item.Pegrdpotedeclarada != null) ? item.Pegrdpotedeclarada : Decimal.Zero;
                        ws.Cells[index, 10].Value = (item.Pegrdpeajeunitario != null) ? item.Pegrdpeajeunitario : Decimal.Zero;
                        ws.Cells[index, 11].Value = (item.Barrnombrefco != null) ? item.Barrnombrefco.ToString() : string.Empty;
                        ws.Cells[index, 12].Value = (item.Pegrdpoteactiva != null) ? item.Pegrdpotereactiva : Decimal.Zero;
                        ws.Cells[index, 13].Value = (item.Pegrdpotereactiva != null) ? item.Pegrdpotereactiva : Decimal.Zero;
                        ws.Cells[index, 14].Value = (item.Pegrdcalidad != null) ? item.Pegrdcalidad.ToString() : string.Empty;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 14];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 10;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 15;

                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 20;
                    ws.Column(12).Width = 15;
                    ws.Column(13).Width = 15;
                    ws.Column(14).Width = 10;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("EMPRESAS");
                if (ws3 != null)
                {   //TITULO
                    ws3.Cells[2, 4].Value = "LISTA DE EMPRESAS";
                    ExcelRange rg = ws3.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws3.Cells[5, 2].Value = "EMPRESA";
                    rg = ws3.Cells[5, 2, 5, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in ListaEmpresas)
                    {
                        ws3.Cells[row, 2].Value = (item.EmprNombre != null) ? item.EmprNombre : string.Empty;
                        //Border por celda
                        rg = ws3.Cells[row, 2, row, 2];
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

                    //Fijar panel
                    ws3.View.FreezePanes(6, 3);
                    rg = ws3.Cells[5, 2, row, 2];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws3.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }

                ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("BARRAS");
                if (ws4 != null)
                {   //TITULO
                    ws4.Cells[2, 4].Value = "LISTA DE BARRAS";
                    ExcelRange rg = ws4.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws4.Cells[5, 2].Value = "BARRA";
                    ws4.Cells[5, 3].Value = "TENSIÓN";
                    ws4.Column(3).Style.Numberformat.Format = "#.#";
                    ws4.Cells[5, 4].Value = "PUNTO DE SUMINISTRO";
                    ws4.Cells[5, 5].Value = "BARRA BGR";
                    ws4.Cells[5, 6].Value = "ÁREA";

                    rg = ws4.Cells[5, 2, 5, 6];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in ListaBarras)
                    {
                        ws4.Cells[row, 2].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws4.Cells[row, 3].Value = (item.BarrTension != null) ? item.BarrTension.ToString() : string.Empty;
                        ws4.Cells[row, 4].Value = (item.BarrPuntoSumirer != null) ? item.BarrPuntoSumirer.ToString() : string.Empty;
                        ws4.Cells[row, 5].Value = (item.BarrBarraBgr != null) ? item.BarrBarraBgr.ToString() : string.Empty;
                        ws4.Cells[row, 6].Value = (item.AreaNombre != null) ? item.AreaNombre.ToString() : string.Empty;
                        //Border por celda
                        rg = ws4.Cells[row, 2, row, 6];
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

                    //Fijar panel
                    ws4.View.FreezePanes(6, 7);
                    rg = ws4.Cells[5, 2, row, 6];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws4.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }
                xlPackage.Save();
            }
        }
        public static void GenerarFormatoPeajeEgreso(string fileName, VtpRecalculoPotenciaDTO entidadRecalculoPotencia, VtpPeajeEgresoDTO entidadPeajeEgreso, List<VtpPeajeEgresoDetalleDTO> listaPeajeEgresoDetalle, List<EmpresaDTO> listaEmpresas, List<BarraDTO> listaBarras)
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
                    ws.Cells[index, 3].Value = "INGRESO PARA VTP Y PEAJES";
                    ws.Cells[index + 1, 3].Value = entidadRecalculoPotencia.Perinombre + "/" + entidadRecalculoPotencia.Recpotnombre;
                    if (entidadPeajeEgreso != null)
                    { ws.Cells[index + 2, 3].Value = "Código de envío: " + entidadPeajeEgreso.Pegrcodi.ToString() + ", Fecha envío: " + entidadPeajeEgreso.Pegrfeccreacion.ToString("dd/MM/yyyy HH:mm:ss"); }
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 4;
                    ws.Cells[index, 6].Value = "PARA EGRESO DE POTENCIA";
                    ws.Cells[index, 8].Value = "PARA PEAJE POR CONEXIÓN";
                    ws.Cells[index, 11].Value = "PARA FLUJO DE CARGA OPTIMO";

                    rg = ws.Cells[index, 6, index, 7];
                    ws.Cells[index, 6, index, 7].Merge = true; //(index,3,index,4)
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    rg = ws.Cells[index, 8, index, 10];
                    ws.Cells[index, 8, index, 10].Merge = true; //(index,3,index,4)                      
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    rg = ws.Cells[index, 11, index, 13];
                    ws.Cells[index, 11, index, 13].Merge = true; //(index,3,index,4)
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    index++;
                    ws.Cells[index, 2].Value = "CLIENTE";
                    ws.Cells[index, 3].Value = "BARRA";
                    ws.Cells[index, 4].Value = "TIPO USUARIO";
                    ws.Cells[index, 5].Value = "LICITACIÓN";
                    ws.Cells[index, 6].Value = "PRECIO POTENCIA S/ /kW-mes";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7].Value = "POTENCIA EGRESO kW";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 8].Value = "POTENCIA CALCULADA (KW)";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 9].Value = "POTENCIA DECLARADA (KW)";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 10].Value = "PEAJE UNITARIO S/ /kW-mes";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 11].Value = "FCO-BARRA";
                    ws.Cells[index, 12].Value = "FCO-POTENCIA ACTIVA kW";
                    ws.Column(12).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 13].Value = "FCO-POTENCIA REACTIVA kW";
                    ws.Column(13).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 14].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, 14];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in listaPeajeEgresoDetalle)
                    {
                        ws.Cells[index, 2].Value = (item.Emprnomb != null) ? item.Emprnomb.ToString() : string.Empty;
                        ws.Cells[index, 3].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                        ws.Cells[index, 4].Value = (item.Pegrdtipousuario != null) ? item.Pegrdtipousuario.ToString() : string.Empty;
                        ws.Cells[index, 5].Value = (item.Pegrdlicitacion != null) ? item.Pegrdlicitacion.ToString() : string.Empty;
                        ws.Cells[index, 6].Value = (item.Pegrdpreciopote != null) ? item.Pegrdpreciopote : Decimal.Zero;
                        ws.Cells[index, 7].Value = (item.Pegrdpoteegreso != null) ? item.Pegrdpoteegreso : Decimal.Zero;
                        ws.Cells[index, 8].Value = (item.Pegrdpotecalculada != null) ? item.Pegrdpotecalculada : Decimal.Zero;
                        ws.Cells[index, 9].Value = (item.Pegrdpotedeclarada != null) ? item.Pegrdpotedeclarada : Decimal.Zero;
                        ws.Cells[index, 10].Value = (item.Pegrdpeajeunitario != null) ? item.Pegrdpeajeunitario : Decimal.Zero;
                        ws.Cells[index, 11].Value = (item.Barrnombrefco != null) ? item.Barrnombrefco.ToString() : string.Empty;
                        ws.Cells[index, 12].Value = (item.Pegrdpoteactiva != null) ? item.Pegrdpotereactiva : Decimal.Zero;
                        ws.Cells[index, 13].Value = (item.Pegrdpotereactiva != null) ? item.Pegrdpotereactiva : Decimal.Zero;
                        ws.Cells[index, 14].Value = (item.Pegrdcalidad != null) ? item.Pegrdcalidad.ToString() : string.Empty;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 14];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 10;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 15;

                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 20;
                    ws.Column(12).Width = 15;
                    ws.Column(13).Width = 15;
                    ws.Column(14).Width = 10;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("EMPRESAS");
                if (ws3 != null)
                {   //TITULO
                    ws3.Cells[2, 4].Value = "LISTA DE EMPRESAS";
                    ExcelRange rg = ws3.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws3.Cells[5, 2].Value = "EMPRESA";
                    rg = ws3.Cells[5, 2, 5, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in listaEmpresas)
                    {
                        ws3.Cells[row, 2].Value = (item.EmprNombre != null) ? item.EmprNombre : string.Empty;
                        //Border por celda
                        rg = ws3.Cells[row, 2, row, 2];
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

                    //Fijar panel
                    ws3.View.FreezePanes(6, 3);
                    rg = ws3.Cells[5, 2, row, 2];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws3.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }

                ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("BARRAS");
                if (ws4 != null)
                {   //TITULO
                    ws4.Cells[2, 4].Value = "LISTA DE BARRAS";
                    ExcelRange rg = ws4.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws4.Cells[5, 2].Value = "BARRA";
                    ws4.Cells[5, 3].Value = "TENSIÓN";
                    ws4.Column(3).Style.Numberformat.Format = "#.#";
                    ws4.Cells[5, 4].Value = "PUNTO DE SUMINISTRO";
                    ws4.Cells[5, 5].Value = "BARRA BGR";
                    ws4.Cells[5, 6].Value = "ÁREA";

                    rg = ws4.Cells[5, 2, 5, 6];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in listaBarras)
                    {
                        ws4.Cells[row, 2].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws4.Cells[row, 3].Value = (item.BarrTension != null) ? item.BarrTension.ToString() : string.Empty;
                        ws4.Cells[row, 4].Value = (item.BarrPuntoSumirer != null) ? item.BarrPuntoSumirer.ToString() : string.Empty;
                        ws4.Cells[row, 5].Value = (item.BarrBarraBgr != null) ? item.BarrBarraBgr.ToString() : string.Empty;
                        ws4.Cells[row, 6].Value = (item.AreaNombre != null) ? item.AreaNombre.ToString() : string.Empty;
                        //Border por celda
                        rg = ws4.Cells[row, 2, row, 6];
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

                    //Fijar panel
                    ws4.View.FreezePanes(6, 7);
                    rg = ws4.Cells[5, 2, row, 6];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws4.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }
                xlPackage.Save();
            }
        }

        public static void GenerarFormatoConsultaDatosHistoricos(string fileName, List<VtpIngresoPotUnidPromdDTO> lstData,
            List<VtpEmpresaPagoDTO> lstData2, List<VtpPeajeEmpresaPagoDTO> lstData3, List<String> lstPeriodos, int opt, int pegrtipinfo)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            int column = pegrtipinfo == 1 ? 3 : 4;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = opt == 1 ? "CONSULTA DE DATOS HISTÓRICOS" : "COMPARACIÓN DE DATOS HISTÓRICOS";

                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //CABECERA DE TABLA
                    index += 4;


                    index++;
                    ws.Cells[index, 2].Value = "Empresa";

                    if (pegrtipinfo == 0 || pegrtipinfo == 2)
                    {
                        ws.Cells[index, 3].Value = pegrtipinfo == 0 ? "Central/Unidad" : pegrtipinfo == 1 ? "" : "Cargo";
                    }

                    int i = column;
                    for (int x = 0; x < lstPeriodos.Count; x++)
                    {
                        ws.Cells[index, i].Value = lstPeriodos[x];
                        i++;
                    }

                    if (opt == 2)
                    {
                        ws.Cells[index, i].Value = "% Variación";
                        ws.Column(i).Style.Numberformat.Format = "#0\\.00%";
                    }
                    else
                    {
                        i--;
                    }

                    //ws.Cells[index, 4].Value = "BARRA";
                    //ws.Cells[index, 5].Value = "CONTRATO";
                    //ws.Cells[index, 6].Value = "TIPO USUARIO";

                    //ws.Cells[index, 7].Value = "PRECIO POTENCIA S/ /kW-mes";
                    //ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    //ws.Cells[index, 8].Value = "POTENCIA COINCIDENTE kW";
                    //ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    //ws.Cells[index, 9].Value = "POTENCIA DECLARADA (KW)";
                    //ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                    //ws.Cells[index, 10].Value = "PEAJE UNITARIO S/ /kW-mes";
                    //ws.Column(10).Style.Numberformat.Format = "#,##0.000";
                    //ws.Cells[index, 11].Value = "FACTOR PERDIDA";
                    //ws.Column(11).Style.Numberformat.Format = "#,##0.000";
                    //ws.Cells[index, 12].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, i];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;

                    if (pegrtipinfo == 0)
                    {
                        foreach (var item in lstData)
                        {
                            ws.Cells[index, 2].Value = (item.Emprnomb != null) ? item.Emprnomb.ToString() : string.Empty;
                            ws.Cells[index, 3].Value = (item.Equinomb != null) ? item.Equinomb.ToString() : string.Empty;
                            int col = 4;
                            for (int x = 0; x < item.lstImportesPromd.Count; x++)
                            {
                                ws.Cells[index, col].Value = (item.lstImportesPromd[x] > 0) ? item.lstImportesPromd[x] : Decimal.Zero;
                                ws.Column(col).Style.Numberformat.Format = "#,##0.00";
                                col++;
                            }
                            if (opt == 2)
                            {
                                ws.Cells[index, col].Value = item.PorcentajeVariacion > 0 ? item.PorcentajeVariacion : Decimal.Zero;
                            }
                            else
                            {
                                col--;
                            }
                            //Border por celda
                            rg = ws.Cells[index, 2, index, col];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            index++;
                        }
                    }
                    else if (pegrtipinfo == 1)
                    {
                        foreach (var item in lstData2)
                        {
                            ws.Cells[index, 2].Value = (item.Emprnombpago != null) ? item.Emprnombpago.ToString() : string.Empty;
                            int col = 3;
                            for (int x = 0; x < item.lstImportesPromd.Count; x++)
                            {
                                ws.Cells[index, col].Value = item.lstImportesPromd[x];
                                ws.Column(col).Style.Numberformat.Format = "#,##0.00";
                                col++;
                            }
                            if (opt == 2)
                            {
                                ws.Cells[index, col].Value = item.PorcentajeVariacion > 0 ? item.PorcentajeVariacion : Decimal.Zero;
                            }
                            else
                            {
                                col--;
                            }
                            //Border por celda
                            rg = ws.Cells[index, 2, index, col];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            index++;
                        }
                    }
                    else
                    {
                        foreach (var item in lstData3)
                        {
                            ws.Cells[index, 2].Value = (item.Emprnombcargo != null) ? item.Emprnombcargo.ToString() : string.Empty;
                            ws.Cells[index, 3].Value = (item.Pingnombre != null) ? item.Pingnombre.ToString() : string.Empty;
                            int col = 4;
                            for (int x = 0; x < item.lstImportesPromd.Count; x++)
                            {
                                ws.Cells[index, col].Value = (item.lstImportesPromd[x] > 0) ? item.lstImportesPromd[x] : Decimal.Zero;
                                ws.Column(col).Style.Numberformat.Format = "#,##0.00";
                                col++;
                            }
                            if (opt == 2)
                            {
                                ws.Cells[index, col].Value = item.PorcentajeVariacion > 0 ? item.PorcentajeVariacion : Decimal.Zero;
                            }
                            else
                            {
                                col--;
                            }
                            //Border por celda
                            rg = ws.Cells[index, 2, index, col];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            index++;
                        }
                    }



                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    int colG = column;
                    for (int x = 0; x < lstPeriodos.Count; x++)
                    {
                        ws.Column(colG).Width = 35;
                        colG++;
                    }

                    if (opt == 2)
                    {
                        ws.Column(colG).Width = 35;
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


        public static void GenerarFormatoPeajeEgresoNuevo(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, VtpPeajeEgresoDTO EntidadPeajeEgreso, List<VtpPeajeEgresoDetalleDTO> ListaPeajeEgresoDetalle, List<EmpresaDTO> ListaEmpresas, List<BarraDTO> ListaBarras)
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
                    ws.Cells[index, 3].Value = "INGRESO PARA VTP Y PEAJES";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    if (EntidadPeajeEgreso != null)
                    { ws.Cells[index + 2, 3].Value = "Código de envío: " + EntidadPeajeEgreso.Pegrcodi.ToString() + ", Fecha envío: " + EntidadPeajeEgreso.Pegrfeccreacion.ToString("dd/MM/yyyy HH:mm:ss"); }
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 4;


                    index++;
                    ws.Cells[index, 2].Value = "CÓDIGO";
                    ws.Cells[index, 3].Value = "CLIENTE";
                    ws.Cells[index, 4].Value = "BARRA";
                    ws.Cells[index, 5].Value = "CONTRATO";
                    ws.Cells[index, 6].Value = "TIPO USUARIO";

                    ws.Cells[index, 7].Value = "PRECIO POTENCIA S/ /kW-mes";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 8].Value = "POTENCIA COINCIDENTE kW";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 9].Value = "POTENCIA DECLARADA (KW)";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 10].Value = "PEAJE UNITARIO S/ /kW-mes";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 11].Value = "FACTOR PERDIDA";
                    ws.Column(11).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 12].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, 12];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaPeajeEgresoDetalle)
                    {
                        ws.Cells[index, 2].Value = (item.Coregecodvtp != null) ? item.Coregecodvtp.ToString() : string.Empty;
                        ws.Cells[index, 3].Value = (item.Emprnomb != null) ? item.Emprnomb.ToString() : string.Empty;
                        ws.Cells[index, 4].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                        ws.Cells[index, 5].Value = (item.TipConNombre != null) ? item.TipConNombre.ToString() : string.Empty;
                        ws.Cells[index, 6].Value = (item.Pegrdtipousuario != null) ? item.Pegrdtipousuario : string.Empty;
                        ws.Cells[index, 7].Value = item.Pegrdpreciopote;// item.Pegrdpreciopote != null ? item.Pegrdpreciopote.ToString() : string.Empty;
                        //ws.Cells[index, 8].Value = (item.PegrdPoteCoincidente != null) ? item.PegrdPoteCoincidente : Decimal.Zero;
                        ws.Cells[index, 8].Value = item.Pegrdpotecoincidente ?? item.Pegrdpoteegreso;// (item.Pegrdpotecoincidente ?? 0) == 0 ? (item.Pegrdpoteegreso.ToString() ?? string.Empty) : item.Pegrdpotecoincidente.ToString() ?? string.Empty;
                        ws.Cells[index, 9].Value = item.Pegrdpotedeclarada;// (item.Pegrdpotedeclarada != null) ? item.Pegrdpotedeclarada.ToString() : string.Empty;
                        ws.Cells[index, 10].Value = item.Pegrdpeajeunitario;// (item.Pegrdpeajeunitario != null) ? item.Pegrdpeajeunitario.ToString() : string.Empty;
                        ws.Cells[index, 11].Value = item.Pegrdfacperdida;// (item.Pegrdfacperdida != null) ? item.Pegrdfacperdida.ToString() : string.Empty;
                        ws.Cells[index, 12].Value = (item.Pegrdcalidad != null) ? item.Pegrdcalidad : string.Empty;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 12];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 10;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 15;

                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 20;
                    ws.Column(12).Width = 15;
                    ws.Column(13).Width = 15;
                    ws.Column(14).Width = 10;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("EMPRESAS");
                if (ws3 != null)
                {   //TITULO
                    ws3.Cells[2, 4].Value = "LISTA DE EMPRESAS";
                    ExcelRange rg = ws3.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws3.Cells[5, 2].Value = "EMPRESA";
                    rg = ws3.Cells[5, 2, 5, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in ListaEmpresas)
                    {
                        ws3.Cells[row, 2].Value = (item.EmprNombre != null) ? item.EmprNombre : string.Empty;
                        //Border por celda
                        rg = ws3.Cells[row, 2, row, 2];
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

                    //Fijar panel
                    ws3.View.FreezePanes(6, 3);
                    rg = ws3.Cells[5, 2, row, 2];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws3.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }

                ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("BARRAS");
                if (ws4 != null)
                {   //TITULO
                    ws4.Cells[2, 4].Value = "LISTA DE BARRAS";
                    ExcelRange rg = ws4.Cells[2, 4, 2, 4];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws4.Cells[5, 2].Value = "BARRA";
                    ws4.Cells[5, 3].Value = "TENSIÓN";
                    ws4.Column(3).Style.Numberformat.Format = "#.#";
                    ws4.Cells[5, 4].Value = "PUNTO DE SUMINISTRO";
                    ws4.Cells[5, 5].Value = "BARRA BGR";
                    ws4.Cells[5, 6].Value = "ÁREA";

                    rg = ws4.Cells[5, 2, 5, 6];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    int row = 6;
                    foreach (var item in ListaBarras)
                    {
                        ws4.Cells[row, 2].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws4.Cells[row, 3].Value = (item.BarrTension != null) ? item.BarrTension.ToString() : string.Empty;
                        ws4.Cells[row, 4].Value = (item.BarrPuntoSumirer != null) ? item.BarrPuntoSumirer.ToString() : string.Empty;
                        ws4.Cells[row, 5].Value = (item.BarrBarraBgr != null) ? item.BarrBarraBgr.ToString() : string.Empty;
                        ws4.Cells[row, 6].Value = (item.AreaNombre != null) ? item.AreaNombre.ToString() : string.Empty;
                        //Border por celda
                        rg = ws4.Cells[row, 2, row, 6];
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

                    //Fijar panel
                    ws4.View.FreezePanes(6, 7);
                    rg = ws4.Cells[5, 2, row, 6];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws4.Drawings.AddPicture("ff", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }
                xlPackage.Save();
            }
        }
        public static ResultadoDTO<DataSet> GeneraDatasetSolicitudCodigoRetiro(string Base64, int hoja, bool esintranet = false)
        {
            //Valida Formato
            Regex reg = new Regex(@"^(([0-9]+\.{0,1}[0-9]+)|[0-9])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Dictionary<string, string> colores = new Dictionary<string, string>();

            colores.Add("red", "red");
            colores.Add("yellow", "yellow");
            colores.Add("orange", "orange");

            var html =
                @"<table border= '1' cellspacing='0' style='width: 100%;'>
                    <thead><tr style = 'height: 17px;' >
                     <th style = 'background: #2980b9; color: white; width: 78.4px; height: 17px;' > Celda </ th >
                      <th style = 'background: #2980b9; color: white; width: 165.6px; height: 17px;' > Tipo Error </ th >
                    </tr>
                    </thead>
                    <tbody>
                      {0}
                     </tbody>
                    </table>";
            ResultadoDTO<DataSet> resultado = new ResultadoDTO<DataSet>();

            StringBuilder mensajeError = new StringBuilder();
            byte[] data = System.Convert.FromBase64String(Base64);
            MemoryStream ms = new MemoryStream(data);
            ExcelPackage xlPackage = new ExcelPackage(ms);
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            List<string> celldasMerges = ws.MergedCells.Where(x => x.Split(':').Length > 1).Select(x => x.Split(':')[1]).ToList();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            StringBuilder rows = new StringBuilder();
            for (int j = 1; j <= ws.Dimension.End.Column; j = j + 1)
            {
                string Columna = "";

                if (ws.Cells[1, j].Value != null)
                    Columna = ws.Cells[1, j].Value.ToString();

                dt.Columns.Add(Columna);
            }

            for (int i = 2; i <= ws.Dimension.End.Row; i = i + 1)
            {
                DataRow row = dt.NewRow();
                for (int j = 1; j <= ws.Dimension.End.Column; j = j + 1)
                {
                    string valor = null;
                    var cellda = ws.Cells[i, j];
                    if (cellda.Value == null)
                        row[j - 1] = "null";
                    else
                    {
                        row[j - 1] = cellda.Value.ToString().Trim();
                        valor = cellda.Value.ToString().Trim();
                    }

                    int inicio = 0;
                    int fin = 0;

                    if (!esintranet)
                    {
                        inicio = 12;
                        fin = 19;
                        // fin = 20;
                    }
                    else
                    {
                        inicio = 15;
                        fin = 22;
                        //fin = 23;
                    }
                    //if ((j > 10 && j < 17) && i >= 6)
                    if ((j > inicio && j < fin) && i >= 6)
                    {
                        int esMerge = celldasMerges.Where(x => x.Substring(0, 1) == cellda.Address.Substring(0, 1)
                        && Convert.ToInt32(cellda.Address.Substring(1, cellda.Address.Length - 1)) <= Convert.ToInt32(x.Substring(1, x.Length - 1))).Count();
                        if (esMerge == 0)
                        {
                            if (string.IsNullOrEmpty(valor))
                            {
                                row[j - 1] = "error";
                                rows.AppendLine($"<tr><td style='background-color:{colores["orange"]}'>{cellda.Address}</td><td>Columna sin datos.</td></tr>");
                            }
                            else
                            {
                                //valor = string.IsNullOrEmpty(valor) ? "0" : valor;
                                if (reg.IsMatch(valor))
                                {
                                    if (Convert.ToDecimal(valor) < 0 || Convert.ToDecimal(valor) > 1000)
                                    {
                                        row[j - 1] = "error";
                                        rows.AppendLine($"<tr><td style='background-color:{colores["yellow"]}'>{cellda.Address}</td><td>Los valores reportados en las columnas deben encontrarse entre 0 y 1000 MW.</td></tr>");
                                    }
                                }
                                else
                                {
                                    row[j - 1] = "error";
                                    rows.AppendLine($"<tr><td style='background-color:{colores["red"]}'>{cellda.Address}</td><td>Caracteres incorrectos.</td></tr>");
                                }
                            }
                        }

                    }
                }
                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            xlPackage.Dispose();
            ms.Dispose();

            if (rows.Length > 0)
                mensajeError.AppendLine(string.Format(html, rows.ToString()));

            if (mensajeError.Length == 0)
                resultado.Data = ds;
            else
            {
                resultado.EsCorrecto = -1;
                resultado.Data = ds;
                resultado.Mensaje = mensajeError.ToString();
            }
            return resultado;
        }

        public static string GenerarFormatoPotenciasContradas(string empresa, List<SolicitudCodigoDTO> parametro)
        {
            string resultadoBase64;
            //1. Instancia para crear un objeto que consuma en memoria RAM
            MemoryStream memoryStream = new MemoryStream();
            //2. Ambito
            // Create a workbook. 
            using (ExcelPackage xlPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                int fila = 2;
                int indice = 2;
                //TITULO
                ws.Cells[fila, 3].Value = "LISTADO DE CÓDIGOS DE RETIRO LVTEA – LVTP.";
                ws.Cells[fila + 1, 3].Value = "Empresa:" + empresa;
                ExcelRange rg = ws.Cells[fila, 3, fila + 2, 3];
                rg.Style.Font.Size = 16;
                rg.Style.Font.Bold = true;
                //CABECERA DE TABLA
                fila += 3;

                ws.Cells[fila, 2].Value = "CLIENTE";
                ws.Cells[fila, 3].Value = "CONTRATO";
                ws.Cells[fila, 4].Value = "USUARIO";
                ws.Cells[fila, 5].Value = "FECHA INICIO";
                ws.Cells[fila, 6].Value = "FECHA FIN";
                ws.Cells[fila, 7].Value = "BARRA TRANSFERENCIA";
                ws.Cells[fila, 8].Value = "CORESOCODI (NO MODIFICAR)";
                ws.Cells[fila, 9].Value = "CODIGO VTEA";
                ws.Cells[fila, 10].Value = "BARRA SUMINISTRO";
                ws.Cells[fila, 11].Value = "COREGECODI (NO MODIFICAR)";
                ws.Cells[fila, 12].Value = "CODIGO VTP";
                //ws.Cells[fila, 13].Value = "POTCN.FIJA TOTAL(MW)";
                //ws.Cells[fila, 14].Value = "POTCN.FIJA HP(MW)";
                //ws.Cells[fila, 15].Value = "POTCN.FIJA HFP(MW)";
                //ws.Cells[fila, 16].Value = "POTCN.VAR TOTAL(MW)";
                //ws.Cells[fila, 17].Value = "POTCN.VAR HP(MW)";
                //ws.Cells[fila, 18].Value = "POTCN.VAR HFP(MW)";
                ws.Cells[fila, 13].Value = "DESCRIPCIÓN";
                ws.Cells[fila, 14].Value = "POSICION";

                rg = ws.Cells[fila, 2, fila, 14];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);

                //Inicio: Color rojo               
                rg = ws.Cells[fila, 8];
                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FF3933"));

                rg = ws.Cells[fila, 11];
                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FF3933"));
                //Fin: Color rojo               

                rg = ws.Cells[fila, 13, fila, 13];
                rg.Style.WrapText = true;
                rg.Style.Locked = true;

                rg = ws.Cells[fila, 14, fila, 14];
                rg.Style.WrapText = true;
                rg.Style.Locked = true;

                fila++;
                foreach (var item in parametro)
                {
                    indice++;
                    int inicioFilaVTA = fila;
                    int indexDetalle = -1;
                    ws.Cells[fila, 2].Value = item.CliNombre ?? string.Empty;
                    ws.Cells[fila, 3].Value = item.TipoContNombre ?? string.Empty;
                    ws.Cells[fila, 4].Value = item.TipoUsuaNombre ?? string.Empty;
                    ws.Cells[fila, 5].Value = item.SoliCodiRetiFechaInicio.GetValueOrDefault().ToString("dd/MM/yyyy");
                    ws.Cells[fila, 6].Value = item.SoliCodiRetiFechaFin.GetValueOrDefault().ToString("dd/MM/yyyy");
                    ws.Cells[fila, 7].Value = item.BarrNombBarrTran ?? string.Empty;
                    //ws.Cells[fila, 8].Value = (item.PegrdPoteCoincidente != null) ? item.PegrdPoteCoincidente : Decimal.Zero;
                    ws.Cells[fila, 8].Value = item.SoliCodiRetiCodi;
                    ws.Cells[fila, 9].Value = item.SoliCodiRetiCodigo ?? string.Empty;

                    foreach (var det in item.ListaCodigoRetiroDetalle)
                    {
                        indexDetalle++;
                        ws.Cells[fila, 10].Value = det.Barranomsum ?? string.Empty;
                        ws.Cells[fila, 11].Value = det.Coregecodigo;
                        ws.Cells[fila, 12].Value = det.Codigovtp ?? string.Empty;


                        rg = ws.Cells[fila, 10, fila, 12];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 2);

                        if (item.OmitirFilaVTA == 0)
                        {
                            int rowsPan = det.PotenciaContratadaDTO.RowSpan ?? 0;

                            //ws.Cells[fila, 13].Value = det.PotenciaContratadaDTO.PotenciaContrTotalFija?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 14].Value = det.PotenciaContratadaDTO.PotenciaContrHPFija?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 15].Value = det.PotenciaContratadaDTO.PotenciaContrHFPFija?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 16].Value = det.PotenciaContratadaDTO.PotenciaContrTotalVar?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 17].Value = det.PotenciaContratadaDTO.PotenciaContrHPVar?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 18].Value = det.PotenciaContratadaDTO.PotenciaContrHFPVar?.ToString() ?? string.Empty;
                            ws.Cells[fila, 13].Value = det.PotenciaContratadaDTO.PotenciaContrObservacion?.ToString() ?? string.Empty;
                            ws.Cells[fila, 14].Value = indice;
                            rg = ws.Cells[fila, 13, fila, 14];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            if (rowsPan > 0)
                            {

                                rg = ws.Cells[fila, 13, (fila - 1) + rowsPan, 13];
                                rg.Merge = true;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg = ObtenerEstiloCelda(rg, 0);
                                rg = ws.Cells[fila, 14, (fila - 1) + rowsPan, 14];
                                rg.Merge = true;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 15, (fila - 1) + rowsPan, 15];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 16, (fila - 1) + rowsPan, 16];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 17, (fila - 1) + rowsPan, 17];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 18, (fila - 1) + rowsPan, 18];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 19, (fila - 1) + rowsPan, 19];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);


                                //rg = ws.Cells[fila, 20, (fila - 1) + rowsPan, 20];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                            }
                        }
                        if (indexDetalle < item.ListaCodigoRetiroDetalle.Count - 1)
                            fila++;

                    }

                    if (indexDetalle > 0)
                    {
                        rg = ws.Cells[inicioFilaVTA, 2, (fila), 2];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 3, (fila), 3];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 4, (fila), 4];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 5, (fila), 5];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 6, (fila), 6];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 7, (fila), 7];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 8, (fila), 8];
                        rg.Merge = true;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 9, (fila), 9];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);


                    }

                    //Border por celda
                    rg = ws.Cells[fila, 2, fila, 12];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 2);

                    rg = ws.Cells[fila, 13, fila, 14];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);

                    fila++;
                }
                ws.Column(2).Width = 30;
                //ws.Column(2).Hidden = true;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 10;
                ws.Column(5).Width = 10;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 40;
                ws.Column(8).Width = 15;
                ws.Column(8).Hidden = true;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Column(11).Width = 15;
                ws.Column(11).Hidden = true;
                ws.Column(12).Width = 20;

                ws.Column(13).Width = 50;
                ws.Column(13).Style.Locked = false;
                ws.Column(14).Width = 1;
                ws.Column(14).Hidden = true;
                //ws.Column(15).Width = 10;
                //ws.Column(15).Style.Locked = false;
                //ws.Column(16).Width = 15;
                //ws.Column(16).Style.Locked = false;
                //ws.Column(17).Width = 10;
                //ws.Column(17).Style.Locked = false;
                //ws.Column(18).Width = 10;
                //ws.Column(18).Style.Locked = false;
                //ws.Column(19).Width = 50;
                //ws.Column(19).Style.Locked = false;

                //ws.Column(20).Width = 1;
                //ws.Column(20).Hidden = true;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                picture.SetPosition(10, 10);
                picture.SetSize(135, 45);


                xlPackage.Save();
                xlPackage.Dispose();
            }

            resultadoBase64 = ConvertToBase64(memoryStream);

            return resultadoBase64;
        }


        public static string GenerarFormatoPotenciasContradasIntranet(string empresa, List<CodigoRetiroDTO> parametro)
        {
            string resultadoBase64;
            //1. Instancia para crear un objeto que consuma en memoria RAM
            MemoryStream memoryStream = new MemoryStream();
            //2. Ambito
            // Create a workbook. 


            using (ExcelPackage xlPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                //ws.Protection.IsProtected = true;
                int fila = 2;
                int indice = 2;
                //TITULO
                ws.Cells[fila, 3].Value = "LISTADO DE CÓDIGOS DE RETIRO LVTEA – LVTP.";
                ws.Cells[fila + 1, 3].Value = "Empresa:" + (string.IsNullOrEmpty(empresa) ? "Todos" : empresa);
                ExcelRange rg = ws.Cells[fila, 3, fila + 2, 3];
                rg.Style.Font.Size = 16;
                rg.Style.Font.Bold = true;
                //CABECERA DE TABLA
                fila += 3;

                ws.Cells[fila, 2].Value = "Empresa";
                ws.Cells[fila, 3].Value = "Cliente";
                ws.Cells[fila, 4].Value = "Ruc Cliente";
                ws.Cells[fila, 5].Value = "Contrato";
                ws.Cells[fila, 6].Value = "Usuario";
                ws.Cells[fila, 7].Value = "Inicio";
                ws.Cells[fila, 8].Value = "Fin";
                ws.Cells[fila, 9].Value = "Estado";
                ws.Cells[fila, 10].Value = "CORESOCODI (NO MODIFICAR)";
                ws.Cells[fila, 11].Value = "Barra Transferencia";
                ws.Cells[fila, 12].Value = "Codigo";
                ws.Cells[fila, 13].Value = "COREGECODI (NO MODFIICAR)";
                ws.Cells[fila, 14].Value = "Barra Suministro";
                ws.Cells[fila, 15].Value = "Codigo";
                ws.Cells[fila, 16].Value = "Estado";
                //ws.Cells[fila, 16].Value = "POTCN.FIJA TOTAL(MW)";
                //ws.Cells[fila, 17].Value = "POTCN.FIJA HP(MW)";
                //ws.Cells[fila, 18].Value = "POTCN.FIJA HFP(MW)";
                //ws.Cells[fila, 19].Value = "POTCN.VAR TOTAL(MW)";
                //ws.Cells[fila, 20].Value = "POTCN.VAR HP(MW)";
                //ws.Cells[fila, 21].Value = "POTCN.VAR HFP(MW)";
                ws.Cells[fila, 17].Value = "DESCRIPCION";
                ws.Cells[fila, 18].Value = "posicion";

                rg = ws.Cells[fila, 2, fila, 18];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);

                //Inicio Color rojo
                rg = ws.Cells[fila, 10];
                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FF3933"));

                rg = ws.Cells[fila, 13];
                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FF3933"));
                //Fin Color Rojo

                rg = ws.Cells[fila, 14, fila, 17];
                rg.Style.WrapText = true;
                rg.Style.Locked = true;

                rg = ws.Cells[fila, 18, fila, 18];
                rg.Style.WrapText = true;
                rg.Style.Locked = true;

                fila++;
                foreach (var item in parametro)
                {
                    indice++;
                    int inicioFilaVTA = fila;
                    int indexDetalle = -1;
                    ws.Cells[fila, 2].Value = item.EmprNombre ?? string.Empty;
                    ws.Cells[fila, 3].Value = item.CliNombre ?? string.Empty;
                    ws.Cells[fila, 4].Value = item.CliRuc ?? string.Empty;
                    ws.Cells[fila, 5].Value = item.TipoContNombre ?? string.Empty;
                    ws.Cells[fila, 6].Value = item.TipoUsuaNombre ?? string.Empty;
                    ws.Cells[fila, 7].Value = item.SoliCodiRetiFechaInicio.GetValueOrDefault().ToString("dd/MM/yyyy");
                    ws.Cells[fila, 8].Value = item.SoliCodiRetiFechaFin.GetValueOrDefault().ToString("dd/MM/yyyy");
                    ws.Cells[fila, 9].Value = item.EstDescripcion;
                    ws.Cells[fila, 10].Value = item.SoliCodiRetiCodi;
                    ws.Cells[fila, 11].Value = item.BarrNombBarrTran ?? string.Empty;
                    ws.Cells[fila, 12].Value = item.SoliCodiRetiCodigo ?? string.Empty;


                    foreach (var det in item.ListarBarraSuministro)
                    {
                        indexDetalle++;
                        ws.Cells[fila, 13].Value = det.CoregeCodi.ToString();
                        //aqui
                        ws.Cells[fila, 14].Value = det.BarrNombre ?? string.Empty;
                        ws.Cells[fila, 15].Value = det.CoregeCodVTP;
                        ws.Cells[fila, 16].Value = det.EstdDescripcion ?? string.Empty;

                        rg = ws.Cells[fila, 13, fila, 16];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 2);
                        if (item.OmitirFilaVTA == 0)
                        {
                            int rowsPan = det.PotenciaContratadaDTO.RowSpan ?? 0;

                            //ws.Cells[fila, 16].Value = det.PotenciaContratadaDTO.PotenciaContrTotalFija?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 17].Value = det.PotenciaContratadaDTO.PotenciaContrHPFija?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 18].Value = det.PotenciaContratadaDTO.PotenciaContrHFPFija?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 19].Value = det.PotenciaContratadaDTO.PotenciaContrTotalVar?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 20].Value = det.PotenciaContratadaDTO.PotenciaContrHPVar?.ToString() ?? string.Empty;
                            //ws.Cells[fila, 21].Value = det.PotenciaContratadaDTO.PotenciaContrHFPVar?.ToString() ?? string.Empty;
                            ws.Cells[fila, 17].Value = det.PotenciaContratadaDTO.PotenciaContrObservacion?.ToString() ?? string.Empty;
                            ws.Cells[fila, 18].Value = indice;
                            rg = ws.Cells[fila, 17, fila, 18];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            if (rowsPan > 0)
                            {

                                rg = ws.Cells[fila, 17, (fila - 1) + rowsPan, 17];
                                rg.Merge = true;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg = ObtenerEstiloCelda(rg, 0);
                                rg = ws.Cells[fila, 18, (fila - 1) + rowsPan, 18];
                                rg.Merge = true;
                                rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 18, (fila - 1) + rowsPan, 18];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 19, (fila - 1) + rowsPan, 19];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 20, (fila - 1) + rowsPan, 20];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 21, (fila - 1) + rowsPan, 21];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                                //rg = ws.Cells[fila, 22, (fila - 1) + rowsPan, 22];
                                //rg.Merge = true;
                                //rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rg = ObtenerEstiloCelda(rg, 0);
                            }

                        }
                        if (indexDetalle < item.ListarBarraSuministro.Count - 1)
                            fila++;

                    }

                    if (indexDetalle > 0)
                    {
                        rg = ws.Cells[inicioFilaVTA, 2, (fila), 2];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 3, (fila), 3];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 4, (fila), 4];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 5, (fila), 5];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 6, (fila), 6];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 7, (fila), 7];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 8, (fila), 8];
                        rg.Merge = true;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 9, (fila), 9];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 10, (fila), 10];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 11, (fila), 11];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);
                        rg = ws.Cells[inicioFilaVTA, 12, (fila), 12];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg = ObtenerEstiloCelda(rg, 2);


                    }

                    //Border por celda

                    rg = ws.Cells[fila, 2, fila, 16];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 2);

                    rg = ws.Cells[fila, 17, fila, 18];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);

                    fila++;



                }
                ws.Column(2).Width = 30;
                //ws.Column(2).Hidden = true;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 10;
                ws.Column(6).Width = 10;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Column(10).Hidden = true;
                ws.Column(11).Width = 15;
                ws.Column(12).Width = 15;
                ws.Column(13).Width = 20;
                ws.Column(13).Hidden = true;
                ws.Column(14).Width = 15;
                ws.Column(15).Width = 15;
                ws.Column(16).Width = 10;
                ws.Column(17).Width = 40;
                ws.Column(17).Style.Locked = false;

                //ws.Column(17).Width = 10;
                //ws.Column(17).Style.Locked = false;
                //ws.Column(18).Width = 10;
                //ws.Column(18).Style.Locked = false;
                //ws.Column(19).Width = 10;
                //ws.Column(19).Style.Locked = false;
                //ws.Column(20).Width = 10;
                //ws.Column(20).Style.Locked = false;
                //ws.Column(21).Width = 10;
                //ws.Column(21).Style.Locked = false;
                ws.Column(18).Width = 1;
                ws.Column(18).Hidden = true;

                //ws.Column(18).Width = 1;
                //ws.Column(18).Hidden = true;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                picture.SetPosition(10, 10);
                picture.SetSize(135, 45);

                xlPackage.Save();
                xlPackage.Dispose();
            }

            resultadoBase64 = ConvertToBase64(memoryStream);

            return resultadoBase64;
        }

        public static string ConvertToBase64(Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Close();
            stream.Dispose();
            return Convert.ToBase64String(bytes);
        }

        public static void GenerarFormatoValidacionesEnvio(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, VtpPeajeEgresoDTO EntidadPeajeEgreso, List<VtpValidacionEnvioDTO> lstValidacionEnvio)
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
                    ws.Cells[index, 3].Value = "VALIDACIONES DE ENVÍO DE PEAJE EGRESO";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    if (EntidadPeajeEgreso != null)
                    { ws.Cells[index + 2, 3].Value = "Código de envío: " + EntidadPeajeEgreso.Pegrcodi.ToString() + ", Fecha envío: " + EntidadPeajeEgreso.Pegrfeccreacion.ToString("dd/MM/yyyy HH:mm:ss"); }
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 4;

                    ws.Cells[index, 2].Value = "Existen diferencias significativas con respecto a los datos reportados de energía activa";
                    ExcelRange rg1 = ws.Cells[index, 2, index + 2, 3];
                    rg1.Style.Font.Size = 12;
                    rg1.Style.Font.Bold = true;

                    index++;
                    index++;
                    ws.Cells[index, 2].Value = "Código VTEA";
                    ws.Cells[index, 3].Value = "Valor VTEA";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00000";
                    ws.Cells[index, 4].Value = "Código VTP";
                    ws.Cells[index, 5].Value = "Valor VTP";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00000";
                    ws.Cells[index, 6].Value = "% Variación";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.0";

                    rg = ws.Cells[index, 2, index, 6];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    int indexinit = index;
                    int indexfin = index;
                    List<int> lstIndex = new List<int>();
                    List<VtpValidacionEnvioDTO> lstItem = new List<VtpValidacionEnvioDTO>();
                    lstValidacionEnvio = lstValidacionEnvio.OrderBy(x => x.VaenCodi).ToList();
                    foreach (var item in lstValidacionEnvio)
                    {
                        if (item.VaenTipoValidacion == "2")
                        {
                            if (lstItem.Count == 0)
                            {
                                lstItem.Add(item);
                                lstIndex.Add(index);
                            }
                            else
                            {
                                if (lstItem[0].PegrdCodi == item.PegrdCodi)
                                {
                                    lstItem.Add(item);
                                    lstIndex.Add(index);
                                    indexinit = lstIndex[0];
                                    indexfin = lstIndex[lstIndex.Count - 1];
                                }
                                else
                                {
                                    if (lstItem.Count > 1)
                                    {
                                        indexinit = lstIndex[0];
                                        indexfin = lstIndex[lstIndex.Count - 1];
                                    }
                                    else
                                    {
                                        lstItem.RemoveAt(0);
                                        lstIndex.RemoveAt(0);
                                        lstItem.Add(item);
                                        lstIndex.Add(index);
                                        indexinit = index;
                                        indexfin = index;
                                    }
                                }
                            }
                            ws.Cells[index, 2].Value = (item.VaenCodVtea != null) ? item.VaenCodVtea.ToString() : string.Empty;
                            ws.Cells[index, 3].Value = (item.VaenValorVtea != null) ? item.VaenValorVtea : Decimal.Zero;
                            ws.Cells[index, 4].Value = (item.VaenCodVtp != null) ? item.VaenCodVtp.ToString() : string.Empty;
                            ws.Cells[index, 5].Value = (item.VaenValorVtp != null) ? item.VaenValorVtp : Decimal.Zero;
                            ws.Cells[index, 6].Value = (item.VaenValorReportado != null) ? item.VaenValorReportado : Decimal.Zero;

                            //Border por celda
                            rg = ws.Cells[index, 2, index, 6];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);

                            ws.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            if (indexfin > indexinit && lstItem.Count == Convert.ToInt32(item.VaenValorCoes))
                            {
                                var listVTEA = lstValidacionEnvio.Where(m => m.PegrdCodi == item.PegrdCodi && m.VaenTipoValidacion == "2")
                                                                .Select(m => new VtpValidacionEnvioDTO
                                                                {
                                                                    VaenCodVtea = m.VaenCodVtea
                                                                }).ToList();

                                var distinct = listVTEA.Select(x => x.VaenCodVtea).Distinct().ToList();
                                if (distinct.Count > 1)
                                {
                                    rg = ws.Cells[indexinit, 6, indexfin, 6];
                                    rg.Merge = true;
                                }
                                else
                                {
                                    rg = ws.Cells[indexinit, 2, indexfin, 2];
                                    rg.Merge = true;

                                    rg = ws.Cells[indexinit, 3, indexfin, 3];
                                    rg.Merge = true;

                                    rg = ws.Cells[indexinit, 6, indexfin, 6];
                                    rg.Merge = true;
                                }
                                lstItem = new List<VtpValidacionEnvioDTO>();
                                lstIndex = new List<int>();
                                indexinit = index;
                                indexfin = index;
                            }

                            index++;
                        }
                    }

                    index++;
                    index++;

                    ws.Cells[index, 2].Value = "Existen diferencias significativas con respecto a los datos de la última revisión del mes anterior";
                    ExcelRange rg2 = ws.Cells[index, 2, index + 2, 3];
                    rg2.Style.Font.Size = 12;
                    rg2.Style.Font.Bold = true;

                    index++;
                    index++;

                    ws.Cells[index, 2].Value = "Cliente";
                    ws.Cells[index, 3].Value = "Barra";
                    ws.Cells[index, 4].Value = "Código";
                    ws.Cells[index, 5].Value = "Valor Reportado";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00000";
                    ws.Cells[index, 6].Value = "Última Revisión Anterior";
                    ws.Cells[index, 6].Style.Numberformat.Format = "#,##0.00000";
                    ws.Cells[index, 7].Value = "% Variación";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.0";

                    rg = ws.Cells[index, 2, index, 7];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in lstValidacionEnvio)
                    {
                        if (item.VaenTipoValidacion == "1")
                        {
                            ws.Cells[index, 2].Value = (item.VaenNomCliente != null) ? item.VaenNomCliente.ToString() : string.Empty;
                            ws.Cells[index, 3].Value = (item.VaenBarraSum != null) ? item.VaenBarraSum.ToString() : string.Empty;
                            ws.Cells[index, 4].Value = (item.VaenCodVtp != null) ? item.VaenCodVtp.ToString() : string.Empty;
                            ws.Cells[index, 5].Value = (item.VaenValorReportado != null) ? item.VaenValorReportado : Decimal.Zero;
                            ws.Cells[index, 6].Value = (item.VaenRevisionAnterior != null) ? item.VaenRevisionAnterior : Decimal.Zero;
                            ws.Cells[index, 7].Value = (item.VaenVariacion != null) ? (item.VaenVariacion).ToString() : string.Empty;


                            //Border por celda
                            rg = ws.Cells[index, 2, index, 7];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);

                            ws.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            index++;
                        }
                    }

                    index++;
                    index++;

                    ws.Cells[index, 2].Value = "Existen diferencias en los factores de pérdida reportados con respecto a lo registrado por el COES.";
                    ExcelRange rg3 = ws.Cells[index, 2, index + 2, 3];
                    rg3.Style.Font.Size = 12;
                    rg3.Style.Font.Bold = true;

                    index++;
                    index++;

                    ws.Cells[index, 2].Value = "BARRA TRANSFERENCIA/SUMINISTRO";
                    ws.Cells[index, 3].Value = "VALOR REPORTADO";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00000";
                    ws.Cells[index, 4].Value = "VALOR REGISTRADO POR EL COES";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00000";

                    rg = ws.Cells[index, 2, index, 4];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in lstValidacionEnvio)
                    {
                        if (item.VaenTipoValidacion == "3")
                        {
                            ws.Cells[index, 2].Value = (item.VaenBarraSum != null) ? item.VaenBarraSum.ToString() : string.Empty;
                            ws.Cells[index, 3].Value = (item.VaenValorReportado != null) ? item.VaenValorReportado : Decimal.Zero;
                            ws.Cells[index, 4].Value = (item.VaenValorCoes != null) ? item.VaenValorCoes : Decimal.Zero;

                            //Border por celda
                            rg = ws.Cells[index, 2, index, 4];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);

                            ws.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            index++;
                        }
                    }

                    index++;
                    index++;

                    List<VtpValidacionEnvioDTO> lstVal = lstValidacionEnvio.Where(x => x.VaenTipoValidacion == "4").ToList();
                    decimal precioPotenciaRevision = lstVal.Count > 0 ? Convert.ToDecimal(lstVal[0].VaenValorCoes) : 0;

                    ws.Cells[index, 2].Value = "Precios de Potencia menor a los calculados por el COES (" + precioPotenciaRevision + ")";
                    ExcelRange rg4 = ws.Cells[index, 2, index + 2, 3];
                    rg4.Style.Font.Size = 12;
                    rg4.Style.Font.Bold = true;

                    index++;
                    index++;

                    ws.Cells[index, 2].Value = "Cliente";
                    ws.Cells[index, 3].Value = "Barra";
                    ws.Cells[index, 4].Value = "Código";
                    ws.Cells[index, 5].Value = "Precio Potencia S/kW-mes";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00000";

                    rg = ws.Cells[index, 2, index, 5];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in lstValidacionEnvio)
                    {
                        if (item.VaenTipoValidacion == "4")
                        {
                            ws.Cells[index, 2].Value = (item.VaenNomCliente != null) ? item.VaenNomCliente.ToString() : string.Empty;
                            ws.Cells[index, 3].Value = (item.VaenBarraSum != null) ? item.VaenBarraSum.ToString() : string.Empty;
                            ws.Cells[index, 4].Value = (item.VaenCodVtp != null) ? item.VaenCodVtp.ToString() : string.Empty;
                            ws.Cells[index, 5].Value = (item.VaenPrecioPotencia != null) ? item.VaenPrecioPotencia : Decimal.Zero;

                            //Border por celda
                            rg = ws.Cells[index, 2, index, 5];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);

                            ws.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            index++;
                        }
                    }

                    index++;
                    index++;

                    List<VtpValidacionEnvioDTO> lstVal2 = lstValidacionEnvio.Where(x => x.VaenTipoValidacion == "5").ToList();
                    decimal peajeUnitarioRevision = lstVal2.Count > 0 ? Convert.ToDecimal(lstVal2[0].VaenValorCoes) : 0;

                    ws.Cells[index, 2].Value = "Precio de Peaje unitario menor a los calculados por el COES (" + peajeUnitarioRevision + ")";
                    ExcelRange rg5 = ws.Cells[index, 2, index + 2, 3];
                    rg5.Style.Font.Size = 12;
                    rg5.Style.Font.Bold = true;

                    index++;
                    index++;

                    ws.Cells[index, 2].Value = "Cliente";
                    ws.Cells[index, 3].Value = "Barra";
                    ws.Cells[index, 4].Value = "Código";
                    ws.Cells[index, 5].Value = "Peaje Unitario S/kW-mes";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00000";

                    rg = ws.Cells[index, 2, index, 5];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in lstValidacionEnvio)
                    {
                        if (item.VaenTipoValidacion == "5")
                        {
                            ws.Cells[index, 2].Value = (item.VaenNomCliente != null) ? item.VaenNomCliente.ToString() : string.Empty;
                            ws.Cells[index, 3].Value = (item.VaenBarraSum != null) ? item.VaenBarraSum.ToString() : string.Empty;
                            ws.Cells[index, 4].Value = (item.VaenCodVtp != null) ? item.VaenCodVtp.ToString() : string.Empty;
                            ws.Cells[index, 5].Value = (item.VaenPeajeUnitario != null) ? item.VaenPeajeUnitario : Decimal.Zero;

                            //Border por celda
                            rg = ws.Cells[index, 2, index, 5];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);

                            ws.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            index++;
                        }
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 40;
                    ws.Column(5).Width = 40;
                    ws.Column(6).Width = 40;
                    ws.Column(7).Width = 40;

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
        /// CU17 Retiros de potencia sin contrato - Permite generar el archivo de exportación de la vista VTP_RETIRO_POTESC
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaRetiroPotenciaSC">Lista de registros de VtpRetiroPotescDTO</param>
        /// <returns></returns>
        public static void GenerarReporteRetiroSC(int PeriFormNuevo, string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC, out ExcelWorksheet hoja)
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
                    int indexResta = PeriFormNuevo == 0 ? 1 : 0;
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "RETIRO NO DECLARADO";//ASSETEC 20200421
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    //ws.Cells[index, 2].Value = "CLIENTE";
                    //ws.Cells[index, 3].Value = "BARRA";
                    //ws.Cells[index, 4].Value = "PRECIO POTENCIA PPB ctm. S/KW-mes";
                    //ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    //ws.Cells[index, 5].Value = "POTENCIA CONSUMIDA kW";
                    //ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                    //ws.Cells[index, 6].Value = "VALORIZACIÓN S/";
                    //ws.Column(6).Style.Numberformat.Format = "#,##0.000";

                    if (PeriFormNuevo == 1)
                        ws.Cells[index, 2].Value = "CODIGO";
                    ws.Cells[index, 3 - indexResta].Value = "CLIENTE";
                    ws.Cells[index, 4 - indexResta].Value = "BARRA";
                    ws.Cells[index, 5 - indexResta].Value = "PRECIO POTENCIA PPB ctm. S/KW-mes";
                    ws.Column(5 - indexResta).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 6 - indexResta].Value = PeriFormNuevo == 1 ? "POTENCIA COINCIDENTE KW" : "POTENCIA CONSUMIDA kW";
                    ws.Column(6 - indexResta).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7 - indexResta].Value = "VALORIZACIÓN S/";
                    ws.Column(7 - indexResta).Style.Numberformat.Format = "#,##0.000";
                    rg = ws.Cells[index, 2, index, 7 - indexResta];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    decimal dTotalRpscpoteegreso = 0;
                    decimal dTotalValorizacion = 0;
                    index++;
                    foreach (var item in ListaRetiroPotenciaSC)
                    {
                        //ws.Cells[index, 2].Value = item.EmprNomb.ToString();
                        //ws.Cells[index, 3].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : "";
                        //ws.Cells[index, 4].Value = (item.Rpscprecioppb != null) ? item.Rpscprecioppb : Decimal.Zero;
                        //ws.Cells[index, 5].Value = (item.Rpscpoteegreso != null) ? item.Rpscpoteegreso : Decimal.Zero;
                        //ws.Cells[index, 6].Value = item.Rpscprecioppb * item.Rpscpoteegreso;
                        //dTotalRpscpoteegreso += Convert.ToDecimal(item.Rpscpoteegreso);
                        //dTotalValorizacion += Convert.ToDecimal(item.Rpscprecioppb) * Convert.ToDecimal(item.Rpscpoteegreso);
                        ////Border por celda
                        //rg = ws.Cells[index, 2, index, 6];
                        //rg.Style.WrapText = true;
                        //rg = ObtenerEstiloCelda(rg, 0);
                        //rg = ws.Cells[index, 4, index, 6];
                        //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        //index++;
                        if (PeriFormNuevo == 1)
                            ws.Cells[index, 2].Value = item.RpsCodiVTP?.ToString();
                        ws.Cells[index, 3 - indexResta].Value = item.Emprnomb.ToString();
                        ws.Cells[index, 4 - indexResta].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : "";
                        ws.Cells[index, 5 - indexResta].Value = (item.Rpscprecioppb != null) ? item.Rpscprecioppb : Decimal.Zero;
                        ws.Cells[index, 6 - indexResta].Value = (item.Rpscpoteegreso != null) ? item.Rpscpoteegreso : Decimal.Zero;
                        ws.Cells[index, 7 - indexResta].Value = item.Rpscprecioppb * item.Rpscpoteegreso;
                        dTotalRpscpoteegreso += Convert.ToDecimal(item.Rpscpoteegreso);
                        dTotalValorizacion += Convert.ToDecimal(item.Rpscprecioppb) * Convert.ToDecimal(item.Rpscpoteegreso);
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 7 - indexResta];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5 - indexResta, index, 7 - indexResta];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    if (dTotalValorizacion > 0)
                    {
                        if (PeriFormNuevo == 1)
                            indexResta = 1;
                        else
                            indexResta = 0;

                        ws.Cells[index, 3 + indexResta].Value = "TOTAL";
                        ws.Cells[index, 4 + indexResta].Value = "";
                        ws.Cells[index, 5 + indexResta].Value = dTotalRpscpoteegreso;
                        ws.Cells[index, 6 + indexResta].Value = dTotalValorizacion;
                        //Border por celda
                        rg = ws.Cells[index, 3 + indexResta, index, 6 + indexResta];
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index, 4 + indexResta, index, 6 + indexResta];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    //Lista de empresas con FactorProporcion
                    decimal dFactorProporcion = 0;
                    //Listas complementarias
                    List<VtpRetiroPotescDTO> ListaRetiroSC = (new TransfPotenciaAppServicio()).ListVtpRetiroPotenciaSCByEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                    List<IngresoRetiroSCDTO> ListaFactorProporcion = (new IngresoRetiroSCAppServicio()).BuscarIngresoRetiroSC(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recacodi);
                    //Lista de Empresas
                    index += 6;
                    decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    int iColumna = 3;
                    foreach (IngresoRetiroSCDTO dtoFactoProporcion in ListaFactorProporcion)
                    {
                        if (dtoFactoProporcion.EmprCodi != 10582) //- Linea agregada egjunin
                        {//- Linea agregada egjunin
                            if (dtoFactoProporcion.EmprNombre == null) dtoFactoProporcion.EmprNombre = "No existe empresa";
                            ws.Cells[index, iColumna].Value = dtoFactoProporcion.EmprNombre.ToString();
                            ws.Column(iColumna).Width = 15;
                            dTotalColum[iColumna] = 0; //Inicializando los valores
                            ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                            iColumna++;
                        }//- Linea agregada egjunin
                    }
                    ws.Cells[index, iColumna].Value = "TOTAL";
                    ws.Column(iColumna).Width = 15;
                    ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 3, index, iColumna];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    index++;
                    //Por cada Factor de Proporción
                    foreach (VtpRetiroPotescDTO dtoRetiroPoteSC in ListaRetiroPotenciaSC)
                    {
                        ws.Cells[index, 2].Value = dtoRetiroPoteSC.Emprnomb.ToString();
                        rg = ws.Cells[index, 2, index, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        decimal dTotalRow = 0;
                        iColumna = 3;
                        foreach (IngresoRetiroSCDTO dtoFactoProporcion in ListaFactorProporcion)
                        {
                            if (dtoFactoProporcion.EmprCodi != 10582) //- Linea agregada egjunin
                            {//- Linea agregada egjunin

                                if (dtoFactoProporcion.EmprCodi == 11153) //- Linea modificada vtajunin
                                { //- Linea modificada vtajunin
                                    IngresoRetiroSCDTO anterior = ListaFactorProporcion.Where(x => x.EmprCodi == 10582).FirstOrDefault();//- Linea modificada vtajunin
                                    if (anterior != null)//- Linea modificada vtajunin
                                    {//- Linea modificada vtajunin                                        
                                        dtoFactoProporcion.IngrscImporteVtp = dtoFactoProporcion.IngrscImporteVtp + anterior.IngrscImporteVtp; //- Linea modificada vtajunin
                                    }//- Linea modificada vtajunin
                                }//- Linea modificada vtajunin


                                dFactorProporcion = dtoFactoProporcion.IngrscImporteVtp;
                                ws.Cells[index, iColumna].Value = dFactorProporcion * dtoRetiroPoteSC.Rpscprecioppb * dtoRetiroPoteSC.Rpscpoteegreso;
                                dTotalRow += dFactorProporcion * (decimal)dtoRetiroPoteSC.Rpscprecioppb * (decimal)dtoRetiroPoteSC.Rpscpoteegreso;
                                dTotalColum[iColumna] += dFactorProporcion * (decimal)dtoRetiroPoteSC.Rpscprecioppb * (decimal)dtoRetiroPoteSC.Rpscpoteegreso;
                                iColumna++;
                            }//- Linea agregada egjunin
                        }
                        ws.Cells[index, iColumna].Value = dTotalRow; //Pinta el total por Fila
                        dTotalColum[iColumna] += dTotalRow;
                        rg = ws.Cells[index, 3, index, iColumna];
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    ws.Cells[index, 2].Value = "TOTAL";
                    rg = ws.Cells[index, 2, index, 2];
                    rg = ObtenerEstiloCelda(rg, 1);
                    iColumna = 3;
                    //for (int i = 0; i <= ListaFactorProporcion.Count(); i++)
                    for (int i = 0; i <= ListaFactorProporcion.Where(x => x.EmprCodi != 10582).Count(); i++) //- Linea modificada vtajunin
                    {
                        ws.Cells[index, iColumna].Value = dTotalColum[iColumna];
                        iColumna++;
                    }
                    rg = ws.Cells[index, 3, index, iColumna - 1];
                    rg = ObtenerEstiloCelda(rg, 0);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
                hoja = ws;
            }
        }


        /// <summary>
        /// Permite generar el Reporte de Peajes a pagarse - CU18
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeEmpresaPago">Lista de registros de VtpPeajeEmpresaPagoDTO</param>
        /// <returns></returns>
        public static void GenerarReporteUnificadoNuevo(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaPago,
            List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioPago, List<VtpEmpresaPagoDTO> ListaEmpresaPago,
            List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo, bool formatoNuevo, List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgreso,
            List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC, PeriodoDTO EntidadPeriodo, List<VtpPagoEgresoDTO> ListaPagoEgreso,
            List<VtpIngresoPotUnidPromdDTO> ListaIngresoPotenciaEmpresa, List<VtpIngresoPotefrDTO> ListaIngresoPotEFR,
            List<VtpSaldoEmpresaDTO> ListaSaldoEmpresa, out ExcelWorksheet hoja)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                #region COMPENSACION TRANSMISORAS POR PEAJE DE CONEXIÓN

                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("C1");
                if (ws != null)
                {   //PAGO = SI & TRANSMISION = SI
                    int row = 2; //Fila donde inicia la data
                    //ws.Cells[row++, 4].Value = dtoRecalculo.RecaCuadro3;
                    //ws.Cells[row++, 4].Value = dtoRecalculo.RecaNroInforme;
                    ws.Cells[row++, 3].Value = "COMPENSACIÓN A TRANSMISORAS POR PEAJE DE CONEXIÓN Y TRANSMISIÓN";
                    ws.Cells[row++, 3].Value = EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[row++, 3].Value = "A) COMPENSACIÓN A TRANSMISORAS";
                    ws.Cells[row++, 3].Value = "PEAJE POR  CONEXIÓN Y TRANSMISIÓN QUE CORRESPONDE PAGAR (" + ConstantesTransfPotencia.MensajeSoles + ")";
                    ExcelRange rg = ws.Cells[2, 3, row++, 3];
                    rg.Style.Font.Size = 12;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws.Cells[row + 1, 2].Value = "EMPRESA";
                    rg = ws.Cells[row, 2, row + 2, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    int iNumEmpresaCobro = 0;
                    int colum = 3;
                    for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                    {
                        if (i == 0)
                        {
                            int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                            iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                            {
                                if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                {
                                    Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                    dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                }

                                ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                if (dtoPeajeIngreso != null)
                                {
                                    ws.Cells[row + 1, colum].Value = dtoPeajeIngreso.Pingnombre;
                                    ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingtipo;
                                }
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0; //Inicializando los valores
                                colum++;

                            }
                            ws.Cells[row + 1, colum].Value = "TOTAL";
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                            dTotalColum[colum] = 0;
                            rg = ws.Cells[row, 3, row + 2, colum];
                            rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Font.Bold = true;
                            rg.Style.Font.Size = 10;
                        }
                        break;
                    }
                    row = row + 3;
                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                    {
                        if (dtoEmpresaPago.Emprcodipeaje != 10582)
                        {
                            ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.Font.Bold = true;
                            ws.Cells[row, 2].Style.Font.Size = 10;

                            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                            //- Haciendo ajuste pgjunin
                            if (dtoEmpresaPago.Emprcodipeaje == 11153)
                            {
                                List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                {
                                    for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                    {
                                        ListaPeajeEmpresaCobro[i].Pempagpeajepago += ListaAnterior[i].Pempagpeajepago;
                                        ListaPeajeEmpresaCobro[i].Pempagsaldoanterior += ListaAnterior[i].Pempagsaldoanterior;
                                        ListaPeajeEmpresaCobro[i].Pempagajuste += ListaAnterior[i].Pempagajuste;
                                    }
                                }
                            }

                            //- Haciendo ajuste egjunin

                            colum = 3;
                            decimal dTotalRow = 0;
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                            {
                                decimal dPeajePago = Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago) + Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                                ws.Cells[row, colum].Value = dPeajePago;
                                dTotalRow += dPeajePago;
                                dTotalColum[colum] += dPeajePago;
                                colum++;
                            }
                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                            dTotalColum[colum] += dTotalRow;
                            //Border por celda en la Fila
                            rg = ws.Cells[row, 2, row, colum];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Bold = false;
                            rg.Style.Font.Size = 10;
                            row++;

                        }
                    }
                    ws.Cells[row, 2].Value = "TOTAL";
                    ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 2].Style.Font.Bold = true;
                    ws.Cells[row, 2].Style.Font.Size = 10;

                    colum = 3;
                    for (int i = 0; i <= iNumEmpresaCobro; i++)
                    {
                        ws.Cells[row, colum].Value = dTotalColum[colum];
                        colum++;
                    }
                    rg = ws.Cells[row, 2, row, colum - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //PAGO = SI & TRANSMISION = NO & CODIGO = ? -> Listar Códigos de Peaje de Ingreso...?
                    int rowaux = row += 2; //Fila donde inicia la data
                    ws.Cells[row++, 2].Value = "C) COMPENSACIONES CENTRALES DE GENERACIÓN DE ELECTRICIDAD CON ENERGÍA RENOVABLE (" + ConstantesTransfPotencia.MensajeSoles + ")";
                    rg = ws.Cells[rowaux, 2, row++, 2];
                    rg.Style.Font.Size = 12;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws.Cells[row + 1, 2].Value = "EMPRESA";
                    rg = ws.Cells[row, 2, row + 2, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    decimal[] dTotalColum2 = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    iNumEmpresaCobro = 0;
                    colum = 3;
                    for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                    {
                        if (i == 0)
                        {
                            int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                            iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                            {
                                if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                {
                                    Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                    dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                }

                                ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                if (dtoPeajeIngreso != null)
                                {
                                    ws.Cells[row + 1, colum].Value = dtoPeajeIngreso.Pingnombre;
                                    ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingtipo;
                                }
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum2[colum] = 0; //Inicializando los valores
                                colum++;

                            }
                            ws.Cells[row + 1, colum].Value = "TOTAL";
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                            dTotalColum2[colum] = 0;
                            rg = ws.Cells[row, 3, row + 2, colum];
                            rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Font.Bold = true;
                            rg.Style.Font.Size = 10;

                        }
                        break;
                    }
                    row = row + 3;
                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                    {
                        if (dtoEmpresaPago.Emprcodipeaje != 10582)
                        {

                            ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.Font.Bold = true;
                            ws.Cells[row, 2].Style.Font.Size = 10;

                            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                            //- Haciendo ajuste pgjunin
                            if (dtoEmpresaPago.Emprcodipeaje == 11153)
                            {
                                List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                {
                                    for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                    {
                                        ListaPeajeEmpresaCobro[i].Pempagpeajepago += ListaAnterior[i].Pempagpeajepago;
                                        ListaPeajeEmpresaCobro[i].Pempagsaldoanterior += ListaAnterior[i].Pempagsaldoanterior;
                                        ListaPeajeEmpresaCobro[i].Pempagajuste += ListaAnterior[i].Pempagajuste;
                                    }
                                }
                            }

                            //- Haciendo ajuste egjunin

                            colum = 3;
                            decimal dTotalRow = 0;
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                            {
                                decimal dPeajePago = Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago) + Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                                ws.Cells[row, colum].Value = dPeajePago;
                                dTotalRow += dPeajePago;
                                dTotalColum2[colum] += dPeajePago;
                                colum++;
                            }
                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                            dTotalColum2[colum] += dTotalRow;
                            //Border por celda en la Fila
                            rg = ws.Cells[row, 2, row, colum];
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
                    ws.Cells[row, 2].Value = "TOTAL";
                    ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 2].Style.Font.Bold = true;
                    ws.Cells[row, 2].Style.Font.Size = 10;

                    colum = 3;
                    for (int i = 0; i <= iNumEmpresaCobro; i++)
                    {
                        ws.Cells[row, colum].Value = dTotalColum2[colum];
                        ws.Column(colum).Width = 20;
                        colum++;
                    }
                    rg = ws.Cells[row, 2, row, colum - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //Grupos de repartos
                    List<VtpRepaRecaPeajeDTO> ListaRepaRecaPeaje = (new TransfPotenciaAppServicio()).GetByCriteriaVtpRepaRecaPeajes(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                    foreach (VtpRepaRecaPeajeDTO dtoRepaRecaPeaje in ListaRepaRecaPeaje)
                    {
                        rowaux = row += 2; //Fila donde inicia la data
                        ws.Cells[row++, 2].Value = dtoRepaRecaPeaje.Rrpenombre + " (" + ConstantesTransfPotencia.MensajeSoles + ")";
                        rg = ws.Cells[rowaux, 2, row++, 2];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 2, 2];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        iNumEmpresaCobro = 0;
                        colum = 3;
                        for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {

                                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                    {
                                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                    }


                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum2[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum2[colum] = 0;
                                rg = ws.Cells[row, 3, row, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;

                            }
                            break;
                        }
                        row = row + 1;

                        int indiceError = 0;
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                        {
                            indiceError++;

                            //try
                            //{
                            if (dtoEmpresaPago.Emprcodipeaje != 10582)
                            {
                                ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2].Style.Font.Bold = true;
                                ws.Cells[row, 2].Style.Font.Size = 10;

                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                {
                                    List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, 10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                    {
                                        if (ListaPeajeEmpresaCobro.Count() > 0)
                                        {
                                            for (int i = 0; i <= ListaPeajeEmpresaCobro.Count(); i++)
                                            {
                                                ListaPeajeEmpresaCobro[i].Pempagpeajepago += ListaAnterior[i].Pempagpeajepago;
                                                ListaPeajeEmpresaCobro[i].Pempagsaldoanterior += ListaAnterior[i].Pempagsaldoanterior;
                                                ListaPeajeEmpresaCobro[i].Pempagajuste += ListaAnterior[i].Pempagajuste;
                                            }
                                        }
                                    }
                                }

                                colum = 3;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    decimal dPeajePago = Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago) + Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                                    ws.Cells[row, colum].Value = dPeajePago;
                                    dTotalRow += dPeajePago;
                                    dTotalColum2[colum] += dPeajePago;
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum2[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                            //}
                            //catch (Exception ex)
                            //{
                            //    string s = indiceError.ToString();
                            //}
                        }
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2].Style.Font.Bold = true;
                        ws.Cells[row, 2].Style.Font.Size = 10;

                        colum = 3;
                        for (int i = 0; i <= iNumEmpresaCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum2[colum];
                            ws.Column(colum).Width = 20;
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
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

                    ws.Column(2).Width = 30;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                #endregion

                #region COMPENSACION TRANSMISORAS INGRESO TARIFARIO

                ws = xlPackage.Workbook.Worksheets.Add("C2");
                if (ws != null)
                {
                    int row = 2; //Fila donde inicia la data
                    //ws.Cells[row++, 3].Value = dtoRecalculo.RecaCuadro3;
                    //ws.Cells[row++, 3].Value = dtoRecalculo.RecaNroInforme;
                    ws.Cells[row++, 3].Value = "COMPENSACIÓN A TRANSMISORAS POR INGRESO TARIFARIO";
                    ws.Cells[row++, 3].Value = EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[row++, 3].Value = "INGRESO TARIFARIO QUE CORRESPONDE PAGAR (" + ConstantesTransfPotencia.MensajeSoles + ")";
                    ExcelRange rg = ws.Cells[2, 3, row++, 3];
                    rg.Style.Font.Size = 12;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws.Cells[row + 1, 2].Value = "EMPRESA";
                    rg = ws.Cells[row, 2, row + 2, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    int iNumEmpresasCobro = 0;
                    int colum = 3;
                    for (int i = 0; i < ListaIngresoTarifarioPago.Count(); i++)
                    {
                        if (i == 0)
                        {
                            int iEmprcodiPago = ListaIngresoTarifarioPago[0].Emprcodingpot;
                            List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                            iNumEmpresasCobro = ListaIngresoTarifarioCobro.Count();
                            foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                            {
                                ws.Cells[row, colum].Value = (dtoIngresoTarifarioCobro.Emprnombping != null) ? dtoIngresoTarifarioCobro.Emprnombping.ToString().Trim() : string.Empty;
                                int iPingcodi = dtoIngresoTarifarioCobro.Pingcodi;
                                VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                if (dtoPeajeIngreso != null)
                                {
                                    ws.Cells[row + 1, colum].Value = dtoPeajeIngreso.Pingnombre;
                                    ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingtipo;
                                }
                                rg = ws.Cells[row, colum, row + 2, colum];
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0; //Inicializando los valores
                                colum++;
                            }
                            ws.Cells[row + 1, colum].Value = "TOTAL";
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                            dTotalColum[colum] = 0;
                            rg = ws.Cells[row, 3, row + 2, colum];
                            rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Font.Bold = true;
                            rg.Style.Font.Size = 10;
                        }
                        break;
                    }
                    row = row + 3;
                    foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioPago in ListaIngresoTarifarioPago)
                    {
                        if (dtoIngresoTarifarioPago.Emprcodingpot != 10582) //- Linea agregada egjunin
                        {//- Linea agregada egjunin

                            ws.Cells[row, 2].Value = (dtoIngresoTarifarioPago.Emprnombingpot != null) ? dtoIngresoTarifarioPago.Emprnombingpot.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.Font.Bold = true;
                            ws.Cells[row, 2].Style.Font.Size = 10;

                            List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(dtoIngresoTarifarioPago.Emprcodingpot, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                            //- Linea agregada egjunin
                            if (dtoIngresoTarifarioPago.Emprcodingpot == 11153)
                            {
                                List<VtpIngresoTarifarioDTO> subList = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (ListaIngresoTarifarioCobro.Count() == subList.Count())
                                {
                                    for (int i = 0; i < ListaIngresoTarifarioCobro.Count(); i++)
                                    {
                                        ListaIngresoTarifarioCobro[i].Ingtarimporte += subList[i].Ingtarimporte;
                                        ListaIngresoTarifarioCobro[i].Ingtarsaldoanterior += subList[i].Ingtarsaldoanterior;
                                        ListaIngresoTarifarioCobro[i].Ingtarajuste += subList[i].Ingtarajuste;
                                    }
                                }
                            }
                            //- Linea agregada egjunin

                            colum = 3;
                            decimal dTotalRow = 0;
                            foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                            {
                                decimal dIngtarimporte = Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarimporte) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarsaldoanterior) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarajuste);
                                ws.Cells[row, colum].Value = dIngtarimporte;
                                dTotalRow += dIngtarimporte;
                                dTotalColum[colum] += dIngtarimporte;
                                colum++;
                            }
                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                            dTotalColum[colum] += dTotalRow;
                            //Border por celda en la Fila
                            rg = ws.Cells[row, 2, row, colum];
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

                        }//- Linea agregada egjunin

                    }
                    ws.Cells[row, 2].Value = "TOTAL";
                    ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 2].Style.Font.Bold = true;
                    ws.Cells[row, 2].Style.Font.Size = 10;

                    colum = 3;
                    for (int i = 0; i <= iNumEmpresasCobro; i++)
                    {
                        ws.Cells[row, colum].Value = dTotalColum[colum];
                        colum++;
                    }
                    rg = ws.Cells[row, 2, row, colum - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    //Fijar panel
                    //ws.View.FreezePanes(9, 3);//fijo hasta la Fila 7 y columna 2
                    rg = ws.Cells[7, 2, row, colum];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                #endregion

                #region MATRIZ DE PAGOS VTP

                ws = xlPackage.Workbook.Worksheets.Add("C3");

                if (ws != null)
                {   //TITULO
                    int row = 2; //Fila donde inicia la data
                    //ws.Cells[row++, 4].Value = dtoRecalculo.RecaCuadro3;
                    //ws.Cells[row++, 4].Value = dtoRecalculo.RecaNroInforme;
                    ws.Cells[row++, 3].Value = "VALORIZACIÓN DE LAS TRANSFERENCIA DE POTENCIA";
                    ws.Cells[row++, 3].Value = EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[row++, 3].Value = "MATRIZ DE PAGOS VTP";
                    ws.Cells[row++, 3].Value = ConstantesTransfPotencia.MensajeSoles;
                    ExcelRange rg = ws.Cells[2, 3, row++, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws.Cells[row, 2].Value = "EMPRESA";
                    rg = ws.Cells[row, 2, row, 2];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    int colum = 3;
                    int iNumEmpresasCobro = 0;
                    for (int i = 0; i < ListaEmpresaPago.Count(); i++)
                    {
                        if (i == 0)
                        {
                            int iEmprcodiPago = ListaEmpresaPago[0].Emprcodipago;
                            List<VtpEmpresaPagoDTO> ListaEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                            iNumEmpresasCobro = ListaEmpresaCobro.Where(x => x.Emprcodicobro != 10582).Count();
                            foreach (VtpEmpresaPagoDTO dtoEmpresaCobro in ListaEmpresaCobro)
                            {
                                if (dtoEmpresaCobro.Emprcodicobro != 10582) //- Linea modificada egjunin
                                {
                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcobro != null) ? dtoEmpresaCobro.Emprnombcobro.ToString().Trim() : string.Empty;
                                    ws.Cells[row, colum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    ws.Cells[row, colum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                    ws.Cells[row, colum].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                    ws.Cells[row, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[row, colum].Style.Font.Bold = true;
                                    ws.Cells[row, colum].Style.Font.Size = 10;
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                            }
                            ws.Cells[row, colum].Value = "TOTAL";
                            ws.Cells[row, colum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, colum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, colum].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[row, colum].Style.Font.Bold = true;
                            ws.Cells[row, colum].Style.Font.Size = 10;
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                            dTotalColum[colum] = 0;
                        }
                        break;
                    }
                    row++; //int row = 8;
                    foreach (VtpEmpresaPagoDTO dtoEmpresaPago in ListaEmpresaPago)
                    {
                        if (dtoEmpresaPago.Emprcodipago != 10582)
                        {
                            ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpago != null) ? dtoEmpresaPago.Emprnombpago.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.Font.Bold = true;
                            ws.Cells[row, 2].Style.Font.Size = 10;

                            List<VtpEmpresaPagoDTO> ListaEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(dtoEmpresaPago.Emprcodipago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                            //- Linea agregada egjunin

                            if (dtoEmpresaPago.Emprcodipago == 11153)
                            {
                                List<VtpEmpresaPagoDTO> ListaAdicional = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (ListaEmpresaCobro.Count() == ListaAdicional.Count())
                                {
                                    for (int i = 0; i < ListaEmpresaCobro.Count(); i++)
                                    {
                                        ListaEmpresaCobro[i].Potepmonto += ListaAdicional[i].Potepmonto;
                                    }
                                }
                            }

                            //- Fin linea agregada egjunin

                            colum = 3;
                            decimal dTotalRow = 0;
                            foreach (VtpEmpresaPagoDTO dtoEmpresaCobro in ListaEmpresaCobro)
                            {
                                if (dtoEmpresaCobro.Emprcodicobro != 10582)
                                {
                                    if (dtoEmpresaCobro.Emprcodicobro == 11153)
                                    {
                                        VtpEmpresaPagoDTO entidadAdicional = ListaEmpresaCobro.Where(x => x.Emprcodicobro == 10582).FirstOrDefault();

                                        if (entidadAdicional != null)
                                        {
                                            dtoEmpresaCobro.Potepmonto += entidadAdicional.Potepmonto;
                                        }
                                    }

                                    ws.Cells[row, colum].Value = dtoEmpresaCobro.Potepmonto;
                                    dTotalRow += Convert.ToDecimal(dtoEmpresaCobro.Potepmonto);
                                    dTotalColum[colum] += Convert.ToDecimal(dtoEmpresaCobro.Potepmonto);
                                    colum++;
                                }
                            }

                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                            dTotalColum[colum] += dTotalRow;
                            //Border por celda en la Fila
                            rg = ws.Cells[row, 2, row, colum];
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
                    ws.Cells[row, 2].Value = "TOTAL";
                    ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 2].Style.Font.Bold = true;
                    ws.Cells[row, 2].Style.Font.Size = 10;

                    colum = 3;
                    for (int i = 0; i <= iNumEmpresasCobro; i++)
                    {
                        ws.Cells[row, colum].Value = dTotalColum[colum];
                        colum++;
                    }
                    rg = ws.Cells[row, 2, row, colum - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    //Fijar panel
                    //ws.View.FreezePanes(8, 3);//fijo hasta la Fila 7 y columna 2
                    rg = ws.Cells[7, 2, row, colum];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                #endregion

                #region INFORMACION INGRESADA PARA VTP Y PEAJES

                ws = xlPackage.Workbook.Worksheets.Add("C5");

                if (ws != null)
                {
                    if (formatoNuevo)
                    {
                        int index = 2;
                        //TITULO
                        ws.Cells[index, 3].Value = "INFORMACIÓN INGRESADA PARA VTP Y PEAJES";
                        ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                        ws.Cells[index + 2, 3].Value = "";
                        ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        index += 4;

                        index++;
                        ws.Cells[index, 2].Value = "CODIGO VTP";
                        ws.Cells[index, 3].Value = "EMPRESA";

                        ws.Cells[index, 4].Value = "CLIENTE";
                        ws.Cells[index, 5].Value = "BARRA";
                        ws.Cells[index, 6].Value = "CONTRATO";
                        ws.Cells[index, 7].Value = "TIPO USUARIO";
                        ws.Cells[index, 8].Value = "PRECIO POTENCIA S/ /kW-mes";
                        ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, 9].Value = "POTENCIA COINCIDENTE kW";
                        ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, 10].Value = "POTENCIA DECLARADA (KW)";
                        ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, 11].Value = "PEAJE UNITARIO S/ /kW-mes";
                        ws.Column(11).Style.Numberformat.Format = "#,##0.000";
                        ws.Cells[index, 12].Value = "FACTOR PÉRDIDA";
                        ws.Column(12).Style.Numberformat.Format = "#,##0.00";

                        ws.Cells[index, 13].Value = "CALIDAD";

                        rg = ws.Cells[index, 2, index, 13];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        index++;
                        foreach (var item in ListaPeajeEgresoMinfo)
                        {
                            ws.Cells[index, 2].Value = (item.Coregecodvtp != null) ? item.Coregecodvtp.ToString() : string.Empty;
                            ws.Cells[index, 3].Value = (item.Genemprnombre != null) ? item.Genemprnombre.ToString() : string.Empty;
                            ws.Cells[index, 4].Value = (item.Cliemprnombre != null) ? item.Cliemprnombre.ToString() : string.Empty;
                            ws.Cells[index, 5].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                            ws.Cells[index, 6].Value = (item.Pegrmilicitacion != null) ? item.Pegrmilicitacion.ToString() : string.Empty;
                            ws.Cells[index, 7].Value = (item.Pegrmitipousuario != null) ? item.Pegrmitipousuario.ToString() : string.Empty;
                            ws.Cells[index, 8].Value = (item.Pegrmipreciopote != null) ? item.Pegrmipreciopote : Decimal.Zero;
                            ws.Cells[index, 9].Value = (item.Pegrdpotecoincidente ?? 0) == 0 ? (item.Pegrmipoteegreso ?? Decimal.Zero) : item.Pegrdpotecoincidente ?? Decimal.Zero;
                            ws.Cells[index, 10].Value = (item.Pegrmipotedeclarada != null) ? item.Pegrmipotedeclarada : Decimal.Zero;
                            ws.Cells[index, 11].Value = (item.Pegrmipeajeunitario != null) ? item.Pegrmipeajeunitario : Decimal.Zero;
                            ws.Cells[index, 12].Value = (item.Pegrdfacperdida != null) ? item.Pegrdfacperdida : Decimal.Zero;
                            ws.Cells[index, 13].Value = (item.Pegrmicalidad != null) ? item.Pegrmicalidad.ToString() : string.Empty;
                            //Border por celda
                            rg = ws.Cells[index, 2, index, 13];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            index++;
                        }

                        //Fijar panel
                        //ws.View.FreezePanes(7, 0);
                        ws.Column(2).Width = 20;
                        ws.Column(3).Width = 30;
                        ws.Column(4).Width = 30;
                        ws.Column(5).Width = 10;
                        ws.Column(6).Width = 10;
                        ws.Column(7).Width = 15;
                        ws.Column(8).Width = 15;
                        ws.Column(9).Width = 15;
                        ws.Column(10).Width = 15;
                        ws.Column(11).Width = 15;
                        ws.Column(12).Width = 20;
                        ws.Column(13).Width = 15;
                    }
                    else
                    {
                        int index = 2;
                        //TITULO
                        ws.Cells[index, 3].Value = "INFORMACIÓN INGRESADA PARA VTP Y PEAJES";
                        ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                        ws.Cells[index + 2, 3].Value = "";
                        ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        index += 4;
                        ws.Cells[index, 7].Value = "PARA EGRESO DE POTENCIA";
                        ws.Cells[index, 9].Value = "PARA PEAJE POR CONEXIÓN";
                        ws.Cells[index, 12].Value = "PARA FLUJO DE CARGA OPTIMO";

                        rg = ws.Cells[index, 7, index, 8];
                        rg.Merge = true;
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        rg = ws.Cells[index, 9, index, 11];
                        rg.Merge = true;
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        rg = ws.Cells[index, 12, index, 14];
                        rg.Merge = true;
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        index++;
                        ws.Cells[index, 2].Value = "EMPRESA";
                        ws.Cells[index, 3].Value = "CLIENTE";
                        ws.Cells[index, 4].Value = "BARRA";
                        ws.Cells[index, 5].Value = "TIPO USUARIO";
                        ws.Cells[index, 6].Value = "LICITACIÓN";
                        ws.Cells[index, 7].Value = "PRECIO POTENCIA S/ /kW-mes";
                        ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, 8].Value = "POTENCIA EGRESO kW";
                        ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, 9].Value = "POTENCIA CALCULADA (KW)";
                        ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, 10].Value = "POTENCIA DECLARADA (KW)";
                        ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, 11].Value = "PEAJE UNITARIO S/ /kW-mes";
                        ws.Column(11).Style.Numberformat.Format = "#,##0.000";
                        ws.Cells[index, 12].Value = "FCO-BARRA";
                        ws.Cells[index, 13].Value = "FCO-POTENCIA ACTIVA kW";
                        ws.Column(13).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, 14].Value = "FCO-POTENCIA REACTIVA kW";
                        ws.Column(14).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, 15].Value = "CALIDAD";

                        rg = ws.Cells[index, 2, index, 15];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        index++;
                        foreach (var item in ListaPeajeEgresoMinfo)
                        {
                            ws.Cells[index, 2].Value = (item.Genemprnombre != null) ? item.Genemprnombre.ToString() : string.Empty;
                            ws.Cells[index, 3].Value = (item.Cliemprnombre != null) ? item.Cliemprnombre.ToString() : string.Empty;
                            ws.Cells[index, 4].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                            ws.Cells[index, 5].Value = (item.Pegrmitipousuario != null) ? item.Pegrmitipousuario.ToString() : string.Empty;
                            ws.Cells[index, 6].Value = (item.Pegrmilicitacion != null) ? item.Pegrmilicitacion.ToString() : string.Empty;
                            ws.Cells[index, 7].Value = (item.Pegrmipreciopote != null) ? item.Pegrmipreciopote : Decimal.Zero;
                            ws.Cells[index, 8].Value = (item.Pegrmipoteegreso != null) ? item.Pegrmipoteegreso : Decimal.Zero;
                            ws.Cells[index, 9].Value = (item.Pegrmipotecalculada != null) ? item.Pegrmipotecalculada : Decimal.Zero;
                            ws.Cells[index, 10].Value = (item.Pegrmipotedeclarada != null) ? item.Pegrmipotedeclarada : Decimal.Zero;
                            ws.Cells[index, 11].Value = (item.Pegrmipeajeunitario != null) ? item.Pegrmipeajeunitario : Decimal.Zero;
                            ws.Cells[index, 12].Value = (item.Barrnombrefco != null) ? item.Barrnombrefco.ToString() : string.Empty;
                            ws.Cells[index, 13].Value = (item.Pegrmipoteactiva != null) ? item.Pegrmipoteactiva : Decimal.Zero;
                            ws.Cells[index, 14].Value = (item.Pegrmipotereactiva != null) ? item.Pegrmipotereactiva : Decimal.Zero;
                            ws.Cells[index, 15].Value = (item.Pegrmicalidad != null) ? item.Pegrmicalidad.ToString() : string.Empty;
                            //Border por celda
                            rg = ws.Cells[index, 2, index, 15];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            index++;
                        }

                        //Fijar panel
                        //ws.View.FreezePanes(7, 0);
                        ws.Column(2).Width = 30;
                        ws.Column(3).Width = 30;
                        ws.Column(4).Width = 20;
                        ws.Column(5).Width = 10;
                        ws.Column(6).Width = 10;
                        ws.Column(7).Width = 15;
                        ws.Column(8).Width = 15;
                        ws.Column(9).Width = 15;
                        ws.Column(10).Width = 15;
                        ws.Column(11).Width = 15;
                        ws.Column(12).Width = 20;
                        ws.Column(13).Width = 15;
                        ws.Column(14).Width = 15;
                        ws.Column(15).Width = 20;
                    }

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                #endregion

                #region RESUMEN DE INFORMACION VTP

                ws = xlPackage.Workbook.Worksheets.Add("C6.1");

                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "RESUMEN DE INFORMACIÓN VTP";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 3].Value = "POTENCIA CONSUMIDA (kW)";
                    rg = ws.Cells[index, 3, index, 6];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Cells[index, 7].Value = "VALORIZACIÓN DE CONSUMOS (S/)";
                    rg = ws.Cells[index, 7, index, 10];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    rg = ws.Cells[index, 3, index, 10];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "CLIENTES CONTRATO BILATERAL";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 4].Value = "CLIENTES CONTRATO LICITACIONES";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 5].Value = "RETIRO NO DECLARADO";  //ASSETEC 20200421
                    ws.Column(5).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 6].Value = "TOTAL";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 7].Value = "CLIENTES CONTRATO BILATERAL";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 8].Value = "CLIENTES CONTRATO LICITACIONES";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 9].Value = "RETIRO NO DECLARADO";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 10].Value = "TOTAL";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.000";

                    rg = ws.Cells[index, 2, index, 10];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    decimal dTotalPotenciaBilateral = 0;
                    decimal dTotalPotenciaLicitacion = 0;
                    decimal dTotalPotenciaSinContrato = 0;
                    decimal dTotalPotencia = 0;
                    decimal dTotalValorizacionBilateral = 0;
                    decimal dTotalValorizacionLicitacion = 0;
                    decimal dTotalValorizacionSinContrato = 0;
                    decimal dTotalValorizacion = 0;
                    index++;
                    foreach (var item in ListaPeajeEgreso)
                    {
                        if (item.Genemprcodi != 10582) //- Linea agregada egjunin
                        {
                            //- Linea agregada egjunin
                            if (item.Genemprcodi == 11153)
                            {
                                VtpPeajeEgresoMinfoDTO entityAdicional = ListaPeajeEgreso.Where(x => x.Genemprcodi == 10582).FirstOrDefault();

                                if (entityAdicional != null)
                                {
                                    item.Pegrmipotecalculada += entityAdicional.Pegrmipotecalculada;
                                    item.Pegrmipotedeclarada += entityAdicional.Pegrmipotedeclarada;
                                    item.Pegrmipreciopote += entityAdicional.Pegrmipreciopote;
                                    item.Pegrmipoteegreso += entityAdicional.Pegrmipoteegreso;
                                    item.Pegrmipeajeunitario += entityAdicional.Pegrmipeajeunitario;
                                    item.Pegrmipoteactiva += entityAdicional.Pegrmipoteactiva;
                                }
                            }
                            //- Linea agregada egjunin

                            ws.Cells[index, 2].Value = item.Genemprnombre.ToString();
                            ws.Cells[index, 3].Value = item.Pegrmipotecalculada;
                            dTotalPotenciaBilateral += Convert.ToDecimal(item.Pegrmipotecalculada);
                            ws.Cells[index, 4].Value = item.Pegrmipotedeclarada;
                            dTotalPotenciaLicitacion += Convert.ToDecimal(item.Pegrmipotedeclarada);
                            ws.Cells[index, 5].Value = item.Pegrmipreciopote;
                            dTotalPotenciaSinContrato += Convert.ToDecimal(item.Pegrmipreciopote);
                            ws.Cells[index, 6].Value = item.Pegrmipotecalculada + item.Pegrmipotedeclarada + item.Pegrmipreciopote;
                            dTotalPotencia += Convert.ToDecimal(item.Pegrmipotecalculada) + Convert.ToDecimal(item.Pegrmipotedeclarada) + Convert.ToDecimal(item.Pegrmipreciopote);
                            ws.Cells[index, 7].Value = item.Pegrmipoteegreso;
                            dTotalValorizacionBilateral += Convert.ToDecimal(item.Pegrmipoteegreso);
                            ws.Cells[index, 8].Value = item.Pegrmipeajeunitario;
                            dTotalValorizacionLicitacion += Convert.ToDecimal(item.Pegrmipeajeunitario);
                            ws.Cells[index, 9].Value = item.Pegrmipoteactiva;
                            dTotalValorizacionSinContrato += Convert.ToDecimal(item.Pegrmipoteactiva);
                            ws.Cells[index, 10].Value = item.Pegrmipoteegreso + item.Pegrmipeajeunitario + item.Pegrmipoteactiva;
                            dTotalValorizacion += Convert.ToDecimal(item.Pegrmipoteegreso) + Convert.ToDecimal(item.Pegrmipeajeunitario) + Convert.ToDecimal(item.Pegrmipoteactiva);
                            //Border por celda
                            rg = ws.Cells[index, 2, index, 10];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, 10];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            index++;

                        }

                    }
                    if (dTotalValorizacion > 0)
                    {
                        ws.Cells[index, 2].Value = "TOTAL";
                        ws.Cells[index, 3].Value = dTotalPotenciaBilateral;
                        ws.Cells[index, 4].Value = dTotalPotenciaLicitacion;
                        ws.Cells[index, 5].Value = dTotalPotenciaSinContrato;
                        ws.Cells[index, 6].Value = dTotalPotencia;
                        ws.Cells[index, 7].Value = dTotalValorizacionBilateral;
                        ws.Cells[index, 8].Value = dTotalValorizacionLicitacion;
                        ws.Cells[index, 9].Value = dTotalValorizacionSinContrato;
                        ws.Cells[index, 10].Value = dTotalValorizacion;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 10];
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index, 3, index, 10];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    //DETALLE DE PERDIDA
                    decimal dMaximaDemanda = Convert.ToDecimal(EntidadRecalculoPotencia.Recpotmaxidemamensual);
                    if (dMaximaDemanda == 0) dMaximaDemanda = 1;
                    ws.Cells[index, 2].Value = "Máxima Demanda a nivel de generación:";
                    ws.Cells[index, 2, index, 5].Merge = true;
                    ws.Cells[index, 6].Value = dMaximaDemanda;
                    index++;
                    decimal dServicioAuxiliar = Convert.ToDecimal(EntidadRecalculoPotencia.Recpotpreciodemaservauxiliares);
                    ws.Cells[index, 2].Value = "Demanda de Servicios Auxiliares de centrales de generación:";
                    ws.Cells[index, 2, index, 5].Merge = true;
                    ws.Cells[index, 6].Value = dServicioAuxiliar;
                    index++;
                    decimal dConsumidaDemanda = Convert.ToDecimal(EntidadRecalculoPotencia.Recpotconsumidademanda);
                    ws.Cells[index, 2].Value = "Potencia consumida por demanda de carácter no regulada sin contratos:";
                    ws.Cells[index, 2, index, 5].Merge = true;
                    ws.Cells[index, 6].Value = dConsumidaDemanda;
                    decimal dPerdida = (dMaximaDemanda - dTotalPotencia - dServicioAuxiliar - dConsumidaDemanda) * 100 / dMaximaDemanda;
                    index++;
                    ws.Cells[index, 2, index, 5].Merge = true;
                    index++;
                    ws.Cells[index, 2].Value = "Pérdidas (%) :";
                    ws.Cells[index, 2, index, 5].Merge = true;
                    ws.Cells[index, 6].Value = dPerdida;
                    //Border por celda
                    rg = ws.Cells[index - 4, 2, index, 6];
                    rg = ObtenerEstiloCelda(rg, 0);
                    rg = ws.Cells[index - 4, 6, index, 6];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rg = ws.Cells[index, 6, index, 6];
                    rg.Style.Numberformat.Format = "#0\\.00%";

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                #endregion

                #region RETIRO DE POTENCIA NO DECLARADO 

                ws = xlPackage.Workbook.Worksheets.Add("C6.2");

                if (ws != null)
                {
                    int indexResta = EntidadPeriodo.PeriFormNuevo == 0 ? 1 : 0;
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "RETIRO NO DECLARADO";//ASSETEC 20200421
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;

                    if (EntidadPeriodo.PeriFormNuevo == 1)
                        ws.Cells[index, 2].Value = "CODIGO";
                    ws.Cells[index, 3 - indexResta].Value = "CLIENTE";
                    ws.Cells[index, 4 - indexResta].Value = "BARRA";
                    ws.Cells[index, 5 - indexResta].Value = "PRECIO POTENCIA PPB ctm. S/KW-mes";
                    ws.Column(5 - indexResta).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 6 - indexResta].Value = EntidadPeriodo.PeriFormNuevo == 1 ? "POTENCIA COINCIDENTE KW" : "POTENCIA CONSUMIDA kW";
                    ws.Column(6 - indexResta).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7 - indexResta].Value = "VALORIZACIÓN S/";
                    ws.Column(7 - indexResta).Style.Numberformat.Format = "#,##0.000";
                    rg = ws.Cells[index, 2, index, 7 - indexResta];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    decimal dTotalRpscpoteegreso = 0;
                    decimal dTotalValorizacion = 0;
                    index++;
                    foreach (var item in ListaRetiroPotenciaSC)
                    {
                        if (EntidadPeriodo.PeriFormNuevo == 1)
                            ws.Cells[index, 2].Value = item.RpsCodiVTP?.ToString();
                        ws.Cells[index, 3 - indexResta].Value = item.Emprnomb.ToString();
                        ws.Cells[index, 4 - indexResta].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : "";
                        ws.Cells[index, 5 - indexResta].Value = (item.Rpscprecioppb != null) ? item.Rpscprecioppb : Decimal.Zero;
                        ws.Cells[index, 6 - indexResta].Value = (item.Rpscpoteegreso != null) ? item.Rpscpoteegreso : Decimal.Zero;
                        ws.Cells[index, 7 - indexResta].Value = item.Rpscprecioppb * item.Rpscpoteegreso;
                        dTotalRpscpoteegreso += Convert.ToDecimal(item.Rpscpoteegreso);
                        dTotalValorizacion += Convert.ToDecimal(item.Rpscprecioppb) * Convert.ToDecimal(item.Rpscpoteegreso);
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 7 - indexResta];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5 - indexResta, index, 7 - indexResta];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    if (dTotalValorizacion > 0)
                    {
                        if (EntidadPeriodo.PeriFormNuevo == 1)
                            indexResta = 1;
                        else
                            indexResta = 0;

                        ws.Cells[index, 3 + indexResta].Value = "TOTAL";
                        ws.Cells[index, 4 + indexResta].Value = "";
                        ws.Cells[index, 5 + indexResta].Value = dTotalRpscpoteegreso;
                        ws.Cells[index, 6 + indexResta].Value = dTotalValorizacion;
                        //Border por celda
                        rg = ws.Cells[index, 3 + indexResta, index, 6 + indexResta];
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index, 4 + indexResta, index, 6 + indexResta];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    //Lista de empresas con FactorProporcion
                    decimal dFactorProporcion = 0;
                    //Listas complementarias
                    List<VtpRetiroPotescDTO> ListaRetiroSC = (new TransfPotenciaAppServicio()).ListVtpRetiroPotenciaSCByEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                    List<IngresoRetiroSCDTO> ListaFactorProporcion = (new IngresoRetiroSCAppServicio()).BuscarIngresoRetiroSC(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recacodi);
                    //Lista de Empresas
                    index += 6;
                    decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    int iColumna = 3;
                    foreach (IngresoRetiroSCDTO dtoFactoProporcion in ListaFactorProporcion)
                    {
                        if (dtoFactoProporcion.EmprCodi != 10582) //- Linea agregada egjunin
                        {//- Linea agregada egjunin
                            if (dtoFactoProporcion.EmprNombre == null) dtoFactoProporcion.EmprNombre = "No existe empresa";
                            ws.Cells[index, iColumna].Value = dtoFactoProporcion.EmprNombre.ToString();
                            ws.Column(iColumna).Width = 15;
                            dTotalColum[iColumna] = 0; //Inicializando los valores
                            ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                            iColumna++;
                        }//- Linea agregada egjunin
                    }
                    ws.Cells[index, iColumna].Value = "TOTAL";
                    ws.Column(iColumna).Width = 15;
                    ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 3, index, iColumna];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    index++;
                    //Por cada Factor de Proporción
                    foreach (VtpRetiroPotescDTO dtoRetiroPoteSC in ListaRetiroPotenciaSC)
                    {
                        ws.Cells[index, 2].Value = dtoRetiroPoteSC.Emprnomb.ToString();
                        rg = ws.Cells[index, 2, index, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        decimal dTotalRow = 0;
                        iColumna = 3;
                        foreach (IngresoRetiroSCDTO dtoFactoProporcion in ListaFactorProporcion)
                        {
                            if (dtoFactoProporcion.EmprCodi != 10582) //- Linea agregada egjunin
                            {//- Linea agregada egjunin

                                if (dtoFactoProporcion.EmprCodi == 11153) //- Linea modificada vtajunin
                                { //- Linea modificada vtajunin
                                    IngresoRetiroSCDTO anterior = ListaFactorProporcion.Where(x => x.EmprCodi == 10582).FirstOrDefault();//- Linea modificada vtajunin
                                    if (anterior != null)//- Linea modificada vtajunin
                                    {//- Linea modificada vtajunin                                        
                                        dtoFactoProporcion.IngrscImporteVtp = dtoFactoProporcion.IngrscImporteVtp + anterior.IngrscImporteVtp; //- Linea modificada vtajunin
                                    }//- Linea modificada vtajunin
                                }//- Linea modificada vtajunin


                                dFactorProporcion = dtoFactoProporcion.IngrscImporteVtp;
                                ws.Cells[index, iColumna].Value = dFactorProporcion * dtoRetiroPoteSC.Rpscprecioppb * dtoRetiroPoteSC.Rpscpoteegreso;
                                dTotalRow += dFactorProporcion * (decimal)dtoRetiroPoteSC.Rpscprecioppb * (decimal)dtoRetiroPoteSC.Rpscpoteegreso;
                                dTotalColum[iColumna] += dFactorProporcion * (decimal)dtoRetiroPoteSC.Rpscprecioppb * (decimal)dtoRetiroPoteSC.Rpscpoteegreso;
                                iColumna++;
                            }//- Linea agregada egjunin
                        }
                        ws.Cells[index, iColumna].Value = dTotalRow; //Pinta el total por Fila
                        dTotalColum[iColumna] += dTotalRow;
                        rg = ws.Cells[index, 3, index, iColumna];
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    ws.Cells[index, 2].Value = "TOTAL";
                    rg = ws.Cells[index, 2, index, 2];
                    rg = ObtenerEstiloCelda(rg, 1);
                    iColumna = 3;
                    //for (int i = 0; i <= ListaFactorProporcion.Count(); i++)
                    for (int i = 0; i <= ListaFactorProporcion.Where(x => x.EmprCodi != 10582).Count(); i++) //- Linea modificada vtajunin
                    {
                        ws.Cells[index, iColumna].Value = dTotalColum[iColumna];
                        iColumna++;
                    }
                    rg = ws.Cells[index, 3, index, iColumna - 1];
                    rg = ObtenerEstiloCelda(rg, 0);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                #endregion

                #region EGRESO POR COMPRA DE POTENCIA

                ws = xlPackage.Workbook.Worksheets.Add("C7");

                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "EGRESO POR COMPRA DE POTENCIA";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "EGRESO POR COMPRA DE POTENCIA (S/)";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 4].Value = "SALDO POR PEAJE (S/)";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 5].Value = "EGRESO TOTAL POR COMPRA DE POTENCIA (S/)";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.000";

                    rg = ws.Cells[index, 2, index, 5];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    decimal dEgreso = 0;
                    decimal dSaldo = 0;
                    decimal dTotalEgreso = 0;
                    index++;
                    foreach (VtpPagoEgresoDTO dtoEgreso in ListaPagoEgreso)
                    {
                        if (dtoEgreso.Emprcodi != 10582)
                        {
                            if (dtoEgreso.Emprcodi == 11153)
                            {
                                VtpPagoEgresoDTO anterior = ListaPagoEgreso.Where(x => x.Emprcodi == 10582).FirstOrDefault();
                                if (anterior != null)
                                {
                                    dtoEgreso.Pagegregreso += anterior.Pagegregreso;
                                    dtoEgreso.Pagegrsaldo += anterior.Pagegrsaldo;
                                    dtoEgreso.Pagegrpagoegreso += anterior.Pagegrpagoegreso;
                                }
                            }

                            //Los atributos: Pstrnstotalrecaudacion, Pstrnstotalpago, Pstrnssaldotransmision son empleado para leer la información de la BD
                            ws.Cells[index, 2].Value = dtoEgreso.Emprnomb;
                            ws.Cells[index, 3].Value = dtoEgreso.Pagegregreso;
                            dEgreso += dtoEgreso.Pagegregreso;
                            ws.Cells[index, 4].Value = dtoEgreso.Pagegrsaldo;
                            dSaldo += dtoEgreso.Pagegrsaldo;
                            ws.Cells[index, 5].Value = dtoEgreso.Pagegrpagoegreso;
                            dTotalEgreso += dtoEgreso.Pagegrpagoegreso;
                            //Border por celda
                            rg = ws.Cells[index, 2, index, 5];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, 5];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            index++;
                        }

                    }
                    if (dEgreso > 0)
                    {
                        ws.Cells[index, 2].Value = "TOTAL";
                        rg = ws.Cells[index, 2, index, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        ws.Cells[index, 3].Value = dEgreso;
                        ws.Cells[index, 4].Value = dSaldo;
                        ws.Cells[index, 5].Value = dTotalEgreso;
                        rg = ws.Cells[index, 3, index, 5];
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(6, 0);
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

                #endregion

                #region INGRESO POR POTENCIA

                ws = xlPackage.Workbook.Worksheets.Add("C9");

                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "INGRESOS POR POTENCIA POR GENERADOR INTEGRANTE Y CENTRAL DE GENERACIÓN";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[index + 2, 3].Value = "" + ConstantesTransfPotencia.MensajeSoles + ")";
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    //Segunda Fila de cabecera
                    index++;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    rg = ws.Cells[index, 2, index + 1, 2];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Cells[index, 3].Value = "CENTRAL / UNIDAD";
                    rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    int iColumna = 4;
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                    {
                        ws.Cells[index, iColumna].Value = "Periodo " + dtoIngresoPotEFR.Ipefrintervalo;
                        ws.Cells[index + 1, iColumna].Value = dtoIngresoPotEFR.Ipefrdescripcion;
                        ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        iColumna++;
                    }
                    ws.Cells[index, iColumna].Value = "PROMEDIO";
                    rg = ws.Cells[index, iColumna, index + 1, iColumna];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 2, index + 1, iColumna];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index += 2;
                    foreach (VtpIngresoPotUnidPromdDTO dtoIngresoPotencia in ListaIngresoPotenciaEmpresa)
                    {
                        //- Linea agregada egjunin

                        if (dtoIngresoPotencia.Emprcodi == 10582)
                        {
                            Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                            dtoIngresoPotencia.Emprnomb = empresa.Emprnomb;
                        }

                        //- Linea agregada egjunin

                        ws.Cells[index, 2].Value = dtoIngresoPotencia.Emprnomb;
                        ws.Cells[index, 3].Value = dtoIngresoPotencia.Equinomb;
                        int iAux = 4;
                        foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                        {
                            List<VtpIngresoPotUnidIntervlDTO> ListaIngresoPotenciaUnidad = (new TransfPotenciaAppServicio()).GetByCriteriaVtpIngresoPotUnidIntervl(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, dtoIngresoPotencia.Emprcodi, dtoIngresoPotencia.Equicodi, dtoIngresoPotEFR.Ipefrcodi);
                            foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoIntervalo in ListaIngresoPotenciaUnidad)
                            {
                                ws.Cells[index, iAux].Value = dtoIngresoIntervalo.Inpuinimporte;
                                break;
                            }
                            iAux++;
                        }
                        ws.Cells[index, iColumna].Value = dtoIngresoPotencia.Inpuprimportepromd;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 4, index, iColumna];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    //Fijar panel
                    //ws.View.FreezePanes(6, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    int iAux2 = 4;
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                    {
                        ws.Column(iAux2).Width = 20;
                        iAux2++;
                    }
                    ws.Column(iColumna).Width = 20;
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //SEGUNDO REPORTE
                    index++;
                    List<VtpIngresoPotenciaDTO> ListaVtpIngresoPotencia = (new TransfPotenciaAppServicio()).ListVtpIngresoPotenciaEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                    ws.Cells[index, 2].Value = "EMPRESA";
                    rg = ws.Cells[index, 2, index + 1, 2];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    int iAux3 = 3;
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                    {
                        ws.Cells[index, iAux3].Value = "Periodo " + dtoIngresoPotEFR.Ipefrintervalo;
                        ws.Cells[index + 1, iAux3].Value = dtoIngresoPotEFR.Ipefrdescripcion;
                        iAux3++;
                    }
                    ws.Cells[index, iAux3].Value = "PROMEDIO";
                    rg = ws.Cells[index, iAux3, index + 1, iAux3];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 2, index + 1, iAux3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    index += 2;
                    foreach (VtpIngresoPotenciaDTO dtoIngresoPotencia in ListaVtpIngresoPotencia)
                    {
                        if (dtoIngresoPotencia.Emprcodi != 10582) //- Linea agregada egjunin
                        {//- Linea agregada egjunin

                            //- Linea agrega egjunin

                            if (dtoIngresoPotencia.Emprcodi == 11153)
                            {
                                VtpIngresoPotenciaDTO anterior = ListaVtpIngresoPotencia.Where(x => x.Emprcodi == 10582).FirstOrDefault();

                                if (anterior != null)
                                {
                                    dtoIngresoPotencia.Potipimporte += anterior.Potipimporte;
                                }
                            }

                            //- Fin linea agregada egjunin

                            ws.Cells[index, 2].Value = dtoIngresoPotencia.Emprnomb;
                            int iAux = 3;
                            foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                            {
                                List<VtpIngresoPotUnidIntervlDTO> ListaIngresoPotenciaUnidad = (new TransfPotenciaAppServicio()).ListVtpIngresoPotUnidIntervlSumIntervlEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, (int)dtoIngresoPotencia.Emprcodi, dtoIngresoPotEFR.Ipefrcodi);

                                //- Linea agregada egjunin

                                if (dtoIngresoPotencia.Emprcodi == 11153)
                                {
                                    List<VtpIngresoPotUnidIntervlDTO> ListaTemportal = (new TransfPotenciaAppServicio()).ListVtpIngresoPotUnidIntervlSumIntervlEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, 10582, dtoIngresoPotEFR.Ipefrcodi);

                                    if (ListaIngresoPotenciaUnidad.Count() == ListaTemportal.Count())
                                    {
                                        for (int i = 0; i < ListaIngresoPotenciaUnidad.Count(); i++)
                                        {
                                            ListaIngresoPotenciaUnidad[i].Inpuinimporte += ListaTemportal[i].Inpuinimporte;
                                        }
                                    }
                                }

                                //- Linea agregada egjunin

                                foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoIntervalo in ListaIngresoPotenciaUnidad)
                                {
                                    ws.Cells[index, iAux].Value = dtoIngresoIntervalo.Inpuinimporte;
                                    break;
                                }
                                iAux++;
                            }

                            ws.Cells[index, iAux3].Value = dtoIngresoPotencia.Potipimporte;
                            //Border por celda
                            rg = ws.Cells[index, 2, index, iAux3];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, iAux3];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rg.Style.Numberformat.Format = "#,##0.00";
                            index++;

                        } //- Linea agregada egjunin            
                    }

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                #endregion

                #region SALDOS VTP

                ws = xlPackage.Workbook.Worksheets.Add("C10");

                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "SALDOS VTP";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "INGRESO POR POTENCIA (S/)";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 4].Value = "EGRESO POR POTENCIA (S/)";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    //Lista de saldos anteriores
                    int iColumna = 0;
                    decimal[] dSaldoTotal = new decimal[1000];
                    List<VtpSaldoEmpresaDTO> ListaPeriodoDestino = (new TransfPotenciaAppServicio()).ListPeriodosDestino(EntidadRecalculoPotencia.Pericodi);
                    if (EntidadRecalculoPotencia.Recpotcodi == 1)
                    {
                        foreach (VtpSaldoEmpresaDTO dtoPeriodo in ListaPeriodoDestino)
                        {
                            ws.Cells[index, 5 + iColumna].Value = dtoPeriodo.Perinombre + " (S/)";
                            ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                            dSaldoTotal[iColumna] = 0;
                            iColumna++;
                        }
                        ws.Cells[index, 5 + iColumna].Value = "AJUSTE DEL MES(S/)";
                        ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                        iColumna++;
                    }
                    ws.Cells[index, 5 + iColumna].Value = "SALDO NETO MENSUAL(S/)";
                    ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                    if (EntidadRecalculoPotencia.Recpotcodi > 1)
                    {
                        iColumna++;
                        ws.Cells[index, 5 + iColumna].Value = "SALDO NETO REV. ANTERIOR (S/)";
                        ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                        iColumna++;
                        ws.Cells[index, 5 + iColumna].Value = "AJUSTE DEL MES (S/)";
                        ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                    }

                    rg = ws.Cells[index, 2, index, 5 + iColumna];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    decimal dIngreso = 0;
                    decimal dEgreso = 0;
                    decimal dAjuste = 0;
                    decimal dSaldo = 0;
                    decimal dVerAnterior = 0;
                    decimal dAjusteMes = 0;
                    index++;
                    foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresa in ListaSaldoEmpresa)
                    {
                        if (dtoSaldoEmpresa.Emprcodi != 10582) //- Linea agregada egjunin
                        {
                            //- Lineas agregadas egjunin
                            if (dtoSaldoEmpresa.Emprcodi == 11153)
                            {
                                VtpSaldoEmpresaDTO anterior = ListaSaldoEmpresa.Where(x => x.Emprcodi == 10582).FirstOrDefault();

                                if (anterior != null)
                                {
                                    dtoSaldoEmpresa.Potseingreso += anterior.Potseingreso;
                                    dtoSaldoEmpresa.Potseegreso += anterior.Potseegreso;
                                    dtoSaldoEmpresa.Potsesaldo += anterior.Potsesaldo;
                                    dtoSaldoEmpresa.Potseajuste += anterior.Potseajuste;
                                }
                            }
                            //- Fin lineas agregadas egjunin


                            ws.Cells[index, 2].Value = dtoSaldoEmpresa.Emprnomb;
                            ws.Cells[index, 3].Value = dtoSaldoEmpresa.Potseingreso;
                            dIngreso += dtoSaldoEmpresa.Potseingreso;
                            ws.Cells[index, 4].Value = dtoSaldoEmpresa.Potseegreso;
                            dEgreso += dtoSaldoEmpresa.Potseegreso;
                            //Lista dinamica de saldos de periodos anteriores
                            iColumna = 0;
                            decimal dSaldoMes = 0;
                            if (EntidadRecalculoPotencia.Recpotcodi == 1)
                            {
                                foreach (VtpSaldoEmpresaDTO dtoPeriodo in ListaPeriodoDestino)
                                {
                                    //- Linea agregada egjunin
                                    decimal saldoAdicional = 0;
                                    if (dtoSaldoEmpresa.Emprcodi == 11153)
                                    {
                                        VtpSaldoEmpresaDTO dtoSaldoAdicional = (new TransfPotenciaAppServicio()).GetSaldoEmpresaPeriodo(10582, dtoPeriodo.Pericodi, dtoSaldoEmpresa.Pericodi);

                                        if (dtoSaldoAdicional != null)
                                        {
                                            saldoAdicional = dtoSaldoAdicional.Potsesaldoreca;
                                        }
                                    }

                                    //- Fin linea agregada egjunin

                                    VtpSaldoEmpresaDTO dtoSaldoAnterior = (new TransfPotenciaAppServicio()).GetSaldoEmpresaPeriodo(dtoSaldoEmpresa.Emprcodi, dtoPeriodo.Pericodi, dtoSaldoEmpresa.Pericodi);
                                    if (dtoSaldoAnterior != null)
                                    {
                                        ws.Cells[index, 5 + iColumna].Value = dtoSaldoAnterior.Potsesaldoreca + saldoAdicional; //- Linea agregada egjunin
                                        dSaldoTotal[iColumna] += dtoSaldoAnterior.Potsesaldoreca + saldoAdicional; //- Linea agregada egjunin
                                        dSaldoMes += dtoSaldoAnterior.Potsesaldoreca + saldoAdicional;  //- Linea agregada egjunin
                                    }
                                    else
                                    {
                                        ws.Cells[index, 5 + iColumna].Value = 0 + saldoAdicional;   //- Linea agregada egjunin
                                    }

                                    iColumna++;
                                }

                                ws.Cells[index, 5 + iColumna].Value = dtoSaldoEmpresa.Potseajuste;
                                dAjuste += dtoSaldoEmpresa.Potseajuste;
                                dSaldoMes += dtoSaldoEmpresa.Potseajuste;
                                iColumna++;
                            }
                            ws.Cells[index, 5 + iColumna].Value = dtoSaldoEmpresa.Potsesaldo + dSaldoMes;
                            dSaldo += dtoSaldoEmpresa.Potsesaldo + dSaldoMes;
                            if (EntidadRecalculoPotencia.Recpotcodi > 1)
                            {
                                decimal dSaldoVerAnterior = 0;
                                int iRecpotcodiAnterior = (int)EntidadRecalculoPotencia.Recpotcodi - 1;
                                VtpSaldoEmpresaDTO dtoSaldoEmpresaAnterior = (new TransfPotenciaAppServicio()).GetByIdVtpSaldoEmpresaSaldo(EntidadRecalculoPotencia.Pericodi, iRecpotcodiAnterior, dtoSaldoEmpresa.Emprcodi);

                                //- Linea agregada egjunin
                                decimal seingreso = 0;
                                decimal seegreso = 0;
                                if (dtoSaldoEmpresa.Emprcodi == 11153)
                                {
                                    VtpSaldoEmpresaDTO dtoSaldoAdicional = (new TransfPotenciaAppServicio()).GetByIdVtpSaldoEmpresaSaldo(EntidadRecalculoPotencia.Pericodi, iRecpotcodiAnterior, 10582);

                                    if (dtoSaldoAdicional != null)
                                    {
                                        seingreso = dtoSaldoAdicional.Potseingreso;
                                        seegreso = dtoSaldoAdicional.Potseegreso;
                                    }
                                }

                                //- fin linea agregada egjunin

                                if (dtoSaldoEmpresaAnterior != null)
                                {
                                    dSaldoVerAnterior = dtoSaldoEmpresaAnterior.Potseingreso + seingreso - dtoSaldoEmpresaAnterior.Potseegreso - seegreso;
                                }

                                iColumna++;
                                ws.Cells[index, 5 + iColumna].Value = dSaldoVerAnterior;
                                dVerAnterior += dSaldoVerAnterior;
                                decimal dAjusteRevicion = (dtoSaldoEmpresa.Potseingreso - dtoSaldoEmpresa.Potseegreso) - dSaldoVerAnterior;
                                iColumna++;
                                ws.Cells[index, 5 + iColumna].Value = dAjusteRevicion;
                                dAjusteMes += dAjusteRevicion;
                            }
                            //Border por celda
                            rg = ws.Cells[index, 2, index, 5 + iColumna];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, 5 + iColumna];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            index++;
                        }
                    }
                    if (dEgreso > 0)
                    {
                        ws.Cells[index, 2].Value = "TOTAL";
                        rg = ws.Cells[index, 2, index, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        ws.Cells[index, 3].Value = dIngreso;
                        ws.Cells[index, 4].Value = dEgreso;
                        if (EntidadRecalculoPotencia.Recpotcodi == 1)
                        {
                            for (int i = 0; i < iColumna; i++)
                            {
                                ws.Cells[index, 5 + i].Value = dSaldoTotal[i];
                            }
                            ws.Cells[index, 4 + iColumna].Value = dAjuste;
                            ws.Cells[index, 5 + iColumna].Value = dSaldo;
                        }
                        else
                        {
                            ws.Cells[index, 5].Value = dSaldo;
                            ws.Cells[index, 6].Value = dVerAnterior;
                            ws.Cells[index, 7].Value = dAjusteMes;
                        }
                        rg = ws.Cells[index, 3, index, 5 + iColumna];
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(6, 0);
                    ws.Column(2).Width = 45;
                    ws.Column(3).Width = 25;
                    ws.Column(4).Width = 25;
                    if (EntidadRecalculoPotencia.Recpotcodi == 1)
                    {
                        for (int i = 0; i < iColumna; i++)
                        {
                            ws.Column(5 + i).Width = 25;
                        }
                        ws.Column(4 + iColumna).Width = 25;
                        ws.Column(5 + iColumna).Width = 25;
                    }
                    else
                    {
                        ws.Column(5).Width = 25;
                        ws.Column(6).Width = 25;
                        ws.Column(7).Width = 25;
                    }

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                #endregion

                hoja = ws;

                xlPackage.Save();
            }
        }




        /// <summary>
        /// Permite generar el Reporte de Peajes a pagarse - CU18
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeEmpresaPago">Lista de registros de VtpPeajeEmpresaPagoDTO</param>
        /// <returns></returns>
        public static void GenerarReportePeajePagarse(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaPago,
            out ExcelWorksheet hoja,
            out List<double> totales,
            out List<double> mensules,
            out List<double> saldos)
        {
            List<double> _totales_ = new List<double>();
            List<double> _mensules_ = new List<double>();
            List<double> _saldos_ = new List<double>();

            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                #region TOTALES DEL MES / VERSION
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("TOTALES");
                if (ws != null)
                {   //PAGO = SI & TRANSMISION = SI
                    int row = 2; //Fila donde inicia la data
                                 //ws.Cells[row++, 4].Value = dtoRecalculo.RecaCuadro3;
                                 //ws.Cells[row++, 4].Value = dtoRecalculo.RecaNroInforme;
                    ws.Cells[row++, 3].Value = "COMPENSACIÓN A TRANSMISORAS POR PEAJE DE CONEXIÓN Y TRANSMISIÓN";
                    ws.Cells[row++, 3].Value = EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[row++, 3].Value = "A) COMPENSACIÓN A TRANSMISORAS";
                    ws.Cells[row++, 3].Value = "PEAJE POR  CONEXIÓN Y TRANSMISIÓN QUE CORRESPONDE PAGAR (" + ConstantesTransfPotencia.MensajeSoles + ")";
                    ExcelRange rg = ws.Cells[2, 3, row++, 3];
                    rg.Style.Font.Size = 12;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws.Cells[row + 2, 2].Value = "EMPRESA";
                    ws.Cells[row + 2, 3].Value = "RUC";
                    rg = ws.Cells[row, 2, row + 3, 3];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    int iNumEmpresaCobro = 0;
                    int colum = 4;
                    for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                    {
                        if (i == 0)
                        {
                            int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                            iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                            {
                                if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                {
                                    Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                    dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                }

                                ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.Emprruc != null) ? dtoEmpresaCobro.Emprruc.ToString().Trim() : string.Empty;
                                int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                if (dtoPeajeIngreso != null)
                                {
                                    ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                    ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                }
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0; //Inicializando los valores
                                colum++;

                            }
                            ws.Cells[row + 1, colum].Value = "TOTAL";
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                            dTotalColum[colum] = 0;
                            rg = ws.Cells[row, 4, row + 3, colum];
                            rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Font.Bold = true;
                            rg.Style.Font.Size = 10;
                        }
                        break;
                    }
                    row = row + 4;
                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                    {
                        if (dtoEmpresaPago.Emprcodipeaje != 10582)
                        {
                            ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                            //- Haciendo ajuste pgjunin
                            if (dtoEmpresaPago.Emprcodipeaje == 11153)
                            {
                                List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                {
                                    for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                    {
                                        ListaPeajeEmpresaCobro[i].Pempagpeajepago += ListaAnterior[i].Pempagpeajepago;
                                        ListaPeajeEmpresaCobro[i].Pempagsaldoanterior += ListaAnterior[i].Pempagsaldoanterior;
                                        ListaPeajeEmpresaCobro[i].Pempagajuste += ListaAnterior[i].Pempagajuste;
                                    }
                                }
                            }

                            //- Haciendo ajuste egjunin

                            colum = 4;
                            decimal dTotalRow = 0;
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                            {
                                decimal dPeajePago = Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago) + Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                                ws.Cells[row, colum].Value = dPeajePago;
                                dTotalRow += dPeajePago;
                                dTotalColum[colum] += dPeajePago;
                                colum++;
                            }
                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                            dTotalColum[colum] += dTotalRow;
                            //Border por celda en la Fila
                            rg = ws.Cells[row, 2, row, colum];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Bold = false;
                            rg.Style.Font.Size = 10;
                            row++;

                        }
                    }
                    ws.Cells[row, 2].Value = "TOTAL";
                    ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                    ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                    colum = 4;
                    for (int i = 0; i <= iNumEmpresaCobro; i++)
                    {
                        ws.Cells[row, colum].Value = dTotalColum[colum];
                        _totales_.Add((double)dTotalColum[colum]);
                        colum++;
                    }
                    rg = ws.Cells[row, 2, row, colum - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //PAGO = SI & TRANSMISION = NO & CODIGO = ? -> Listar Códigos de Peaje de Ingreso...?
                    int rowaux = row += 2; //Fila donde inicia la data
                    ws.Cells[row++, 2].Value = "C) COMPENSACIONES CENTRALES DE GENERACIÓN DE ELECTRICIDAD CON ENERGÍA RENOVABLE (" + ConstantesTransfPotencia.MensajeSoles + ")";
                    rg = ws.Cells[rowaux, 2, row++, 2];
                    rg.Style.Font.Size = 12;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws.Cells[row + 1, 3].Value = "RUC";
                    ws.Cells[row + 1, 2].Value = "EMPRESA";
                    rg = ws.Cells[row, 2, row + 3, 3];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    decimal[] dTotalColum2 = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    iNumEmpresaCobro = 0;
                    colum = 4;
                    for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                    {
                        if (i == 0)
                        {
                            int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                            iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                            {
                                if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                {
                                    Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                    dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                }

                                ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.Emprruc != null) ? dtoEmpresaCobro.Emprruc.ToString().Trim() : string.Empty;
                                int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                if (dtoPeajeIngreso != null)
                                {
                                    ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                    ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                }
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum2[colum] = 0; //Inicializando los valores
                                colum++;

                            }
                            ws.Cells[row + 1, colum].Value = "TOTAL";
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                            dTotalColum2[colum] = 0;
                            rg = ws.Cells[row, 4, row + 3, colum];
                            rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Font.Bold = true;
                            rg.Style.Font.Size = 10;

                        }
                        break;
                    }
                    row = row + 4;
                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                    {
                        if (dtoEmpresaPago.Emprcodipeaje != 10582)
                        {
                            ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                            //- Haciendo ajuste pgjunin
                            if (dtoEmpresaPago.Emprcodipeaje == 11153)
                            {
                                List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                {
                                    for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                    {
                                        ListaPeajeEmpresaCobro[i].Pempagpeajepago += ListaAnterior[i].Pempagpeajepago;
                                        ListaPeajeEmpresaCobro[i].Pempagsaldoanterior += ListaAnterior[i].Pempagsaldoanterior;
                                        ListaPeajeEmpresaCobro[i].Pempagajuste += ListaAnterior[i].Pempagajuste;
                                    }
                                }
                            }

                            //- Haciendo ajuste egjunin

                            colum = 4;
                            decimal dTotalRow = 0;
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                            {
                                decimal dPeajePago = Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago) + Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                                ws.Cells[row, colum].Value = dPeajePago;
                                dTotalRow += dPeajePago;
                                dTotalColum2[colum] += dPeajePago;
                                colum++;
                            }
                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                            dTotalColum2[colum] += dTotalRow;
                            //Border por celda en la Fila
                            rg = ws.Cells[row, 2, row, colum];
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
                    ws.Cells[row, 2].Value = "TOTAL";
                    ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                    ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                    colum = 4;
                    for (int i = 0; i <= iNumEmpresaCobro; i++)
                    {
                        ws.Cells[row, colum].Value = dTotalColum2[colum];
                        ws.Column(colum).Width = 20;
                        colum++;
                    }
                    rg = ws.Cells[row, 2, row, colum - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //Grupos de repartos
                    List<VtpRepaRecaPeajeDTO> ListaRepaRecaPeaje = (new TransfPotenciaAppServicio()).GetByCriteriaVtpRepaRecaPeajes(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                    foreach (VtpRepaRecaPeajeDTO dtoRepaRecaPeaje in ListaRepaRecaPeaje)
                    {
                        rowaux = row += 2; //Fila donde inicia la data
                        ws.Cells[row++, 2].Value = dtoRepaRecaPeaje.Rrpenombre + " (" + ConstantesTransfPotencia.MensajeSoles + ")";
                        rg = ws.Cells[rowaux, 2, row++, 2];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        iNumEmpresaCobro = 0;
                        colum = 4;
                        for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {

                                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                    {
                                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                    }


                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.Emprruc != null) ? dtoEmpresaCobro.Emprruc.ToString().Trim() : string.Empty;
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum2[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum2[colum] = 0;
                                rg = ws.Cells[row, 4, row, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;

                            }
                            break;
                        }
                        row = row + 1;

                        int indiceError = 0;
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                        {
                            indiceError++;

                            //try
                            //{
                            if (dtoEmpresaPago.Emprcodipeaje != 10582)
                            {
                                ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                {
                                    List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, 10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                    {
                                        if (ListaPeajeEmpresaCobro.Count() > 0)
                                        {
                                            for (int i = 0; i <= ListaPeajeEmpresaCobro.Count(); i++)
                                            {
                                                ListaPeajeEmpresaCobro[i].Pempagpeajepago += ListaAnterior[i].Pempagpeajepago;
                                                ListaPeajeEmpresaCobro[i].Pempagsaldoanterior += ListaAnterior[i].Pempagsaldoanterior;
                                                ListaPeajeEmpresaCobro[i].Pempagajuste += ListaAnterior[i].Pempagajuste;
                                            }
                                        }
                                    }
                                }

                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    decimal dPeajePago = Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago) + Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                                    ws.Cells[row, colum].Value = dPeajePago;
                                    dTotalRow += dPeajePago;
                                    dTotalColum2[colum] += dPeajePago;
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum2[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                            //}
                            //catch (Exception ex)
                            //{
                            //    string s = indiceError.ToString();
                            //}
                        }
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresaCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum2[colum];
                            ws.Column(colum).Width = 20;
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
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

                    ws.Column(3).Width = 13;
                    ws.Column(2).Width = 30;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                hoja = ws;
                #endregion
                //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                if (EntidadRecalculoPotencia.Recpotcodi == 1)
                {
                    #region MENSUAL
                    ws = xlPackage.Workbook.Worksheets.Add("MENSUAL");
                    if (ws != null)
                    {   //PAGO = SI & TRANSMISION = SI
                        int row = 2; //Fila donde inicia la data
                        ws.Cells[row++, 3].Value = "CÁLCULOS DEL MES: A) COMPENSACIÓN A TRANSMISORAS";
                        ws.Cells[row++, 3].Value = "PEAJE POR  CONEXIÓN Y TRANSMISIÓN QUE CORRESPONDE PAGAR (" + ConstantesTransfPotencia.MensajeSoles + ")";
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                        int iNumEmpresaCobro = 0;
                        int colum = 4;
                        for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                    {
                                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                    }

                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.Emprruc != null) ? dtoEmpresaCobro.Emprruc.ToString().Trim() : string.Empty;
                                    int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                    if (dtoPeajeIngreso != null)
                                    {
                                        ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                        ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                    }
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row + 1, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                                rg = ws.Cells[row, 4, row + 3, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;

                            }
                            break;
                        }
                        row = row + 4;
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                        {
                            if (dtoEmpresaPago.Emprcodipeaje != 10582)
                            {
                                ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Haciendo ajuste pgjunin
                                if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                {
                                    List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                    {
                                        for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                        {
                                            ListaPeajeEmpresaCobro[i].Pempagpeajepago += ListaAnterior[i].Pempagpeajepago;
                                        }
                                    }
                                }
                                //- Haciendo ajuste egjunin


                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    decimal dTotalMes = dtoEmpresaCobro.Pempagpeajepago; // - dtoEmpresaCobro.Pempagsaldoanterior
                                    ws.Cells[row, colum].Value = dTotalMes;
                                    dTotalRow += Convert.ToDecimal(dTotalMes);
                                    dTotalColum[colum] += Convert.ToDecimal(dTotalMes);
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Bold = false;
                                rg.Style.Font.Size = 10;
                                row++;
                            }
                        }
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresaCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            _mensules_.Add((double)dTotalColum[colum]);
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //PAGO = SI & TRANSMISION = NO
                        int rowaux = row += 2; //Fila donde inicia la data
                        ws.Cells[row++, 2].Value = "CÁLCULOS DEL MES: C) COMPENSACIONES CENTRALES DE GENERACIÓN DE ELECTRICIDAD CON ENERGÍA RENOVABLE (" + ConstantesTransfPotencia.MensajeSoles + ")";
                        rg = ws.Cells[rowaux, 2, row++, 2];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal[] dTotalColum2 = new decimal[1000]; // Donde se almacenan los Totales por columnas
                        iNumEmpresaCobro = 0;
                        colum = 4;
                        for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                    {
                                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                    }

                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.Emprruc != null) ? dtoEmpresaCobro.Emprruc.ToString().Trim() : string.Empty;
                                    int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                    if (dtoPeajeIngreso != null)
                                    {
                                        ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                        ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                    }
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum2[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row + 1, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum2[colum] = 0;
                                rg = ws.Cells[row, 4, row + 3, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;

                            }
                            break;
                        }
                        row = row + 4;
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                        {
                            if (dtoEmpresaPago.Emprcodipeaje != 10582)
                            {
                                ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Haciendo ajuste pgjunin
                                if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                {
                                    List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                    {
                                        for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                        {
                                            ListaPeajeEmpresaCobro[i].Pempagpeajepago += ListaAnterior[i].Pempagpeajepago;
                                        }
                                    }
                                }
                                //- Haciendo ajuste egjunin

                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    decimal dotalMes = dtoEmpresaCobro.Pempagpeajepago; // -dtoEmpresaCobro.Pempagsaldoanterior;
                                    ws.Cells[row, colum].Value = dotalMes;
                                    dTotalRow += Convert.ToDecimal(dotalMes);
                                    dTotalColum2[colum] += Convert.ToDecimal(dotalMes);
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum2[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresaCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum2[colum];
                            ws.Column(colum).Width = 20;
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //Grupos de repartos
                        List<VtpRepaRecaPeajeDTO> ListaRepaRecaPeaje = (new TransfPotenciaAppServicio()).GetByCriteriaVtpRepaRecaPeajes(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                        foreach (VtpRepaRecaPeajeDTO dtoRepaRecaPeaje in ListaRepaRecaPeaje)
                        {
                            rowaux = row += 2; //Fila donde inicia la data
                            ws.Cells[row++, 2].Value = "CÁLCULOS DEL MES: " + dtoRepaRecaPeaje.Rrpenombre + " (" + ConstantesTransfPotencia.MensajeSoles + ")";
                            rg = ws.Cells[rowaux, 2, row++, 2];
                            rg.Style.Font.Size = 12;
                            rg.Style.Font.Bold = true;
                            //CABECERA DE TABLA
                            ws.Cells[row + 1, 3].Value = "RUC";
                            ws.Cells[row + 1, 2].Value = "EMPRESA";
                            rg = ws.Cells[row, 2, row + 2, 3];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            iNumEmpresaCobro = 0;
                            colum = 4;
                            for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                    List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                    iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                    {
                                        if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                        {
                                            Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                            dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                        }

                                        ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                        dTotalColum2[colum] = 0; //Inicializando los valores
                                        colum++;
                                    }
                                    ws.Cells[row, colum].Value = "TOTAL";
                                    dTotalColum2[colum] = 0;
                                    rg = ws.Cells[row, 3, row, colum];
                                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                    rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.Font.Bold = true;
                                    rg.Style.Font.Size = 10;
                                }
                                break;
                            }
                            row = row + 1;
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                            {
                                if (dtoEmpresaPago.Emprcodipeaje != 10582)
                                {
                                    ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                    ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                    ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                    ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                    ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    ws.Cells[row, 2].Style.Font.Bold = true;
                                    ws.Cells[row, 2].Style.Font.Size = 10;

                                    List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    //- Haciendo ajuste pgjunin
                                    if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                    {
                                        List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, 10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                        if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                        {
                                            for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                            {
                                                ListaPeajeEmpresaCobro[i].Pempagpeajepago += ListaAnterior[i].Pempagpeajepago;
                                            }
                                        }
                                    }
                                    //- Haciendo ajuste egjunin

                                    colum = 4;
                                    decimal dTotalRow = 0;
                                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                    {
                                        decimal dTotalMes = dtoEmpresaCobro.Pempagpeajepago; // -dtoEmpresaCobro.Pempagsaldoanterior;
                                        ws.Cells[row, colum].Value = dTotalMes;
                                        dTotalRow += Convert.ToDecimal(dTotalMes);
                                        dTotalColum2[colum] += Convert.ToDecimal(dTotalMes);
                                        colum++;
                                    }
                                    ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                    dTotalColum2[colum] += dTotalRow;
                                    //Border por celda en la Fila
                                    rg = ws.Cells[row, 2, row, colum];
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
                            ws.Cells[row, 2].Value = "TOTAL";
                            ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                            colum = 4;
                            for (int i = 0; i <= iNumEmpresaCobro; i++)
                            {
                                ws.Cells[row, colum].Value = dTotalColum2[colum];
                                ws.Column(colum).Width = 20;
                                colum++;
                            }
                            rg = ws.Cells[row, 2, row, colum - 1];
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

                        ws.Column(3).Width = 13;
                        ws.Column(2).Width = 30;

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                        picture.SetPosition(10, 10);
                        picture.SetSize(135, 45);
                    }
                    #endregion
                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    #region SALDOS
                    ws = xlPackage.Workbook.Worksheets.Add("SALDOS");
                    if (ws != null)
                    {   //PAGO = SI & TRANSMISION = SI
                        int row = 2; //Fila donde inicia la data
                        ws.Cells[row++, 3].Value = "SALDOS DEL MES: A) COMPENSACIÓN A TRANSMISORAS";
                        ws.Cells[row++, 3].Value = "PEAJE POR  CONEXIÓN Y TRANSMISIÓN QUE CORRESPONDE PAGAR (" + ConstantesTransfPotencia.MensajeSoles + ")";
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                        int iNumEmpresaCobro = 0;
                        int colum = 4;
                        for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                    {
                                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                    }


                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.Emprruc != null) ? dtoEmpresaCobro.Emprruc.ToString().Trim() : string.Empty;
                                    int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                    if (dtoPeajeIngreso != null)
                                    {
                                        ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                        ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                    }
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row + 1, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                                rg = ws.Cells[row, 4, row + 3, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;

                            }
                            break;
                        }
                        row = row + 4;
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                        {
                            if (dtoEmpresaPago.Emprcodipeaje != 10582)
                            {
                                ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Haciendo ajuste pgjunin
                                if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                {
                                    List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                    {
                                        for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                        {
                                            ListaPeajeEmpresaCobro[i].Pempagsaldoanterior += ListaAnterior[i].Pempagsaldoanterior;
                                            ListaPeajeEmpresaCobro[i].Pempagajuste += ListaAnterior[i].Pempagajuste;
                                        }
                                    }
                                }
                                //- Haciendo ajuste egjunin

                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    decimal dSaldoAnterior = Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                                    ws.Cells[row, colum].Value = dSaldoAnterior;
                                    dTotalRow += dSaldoAnterior;
                                    dTotalColum[colum] += dSaldoAnterior;
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Bold = false;
                                rg.Style.Font.Size = 10;
                                row++;
                            }
                        }
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresaCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            _saldos_.Add((double)dTotalColum[colum]);
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //PAGO = SI & TRANSMISION = NO
                        int rowaux = row += 2; //Fila donde inicia la data
                        ws.Cells[row++, 2].Value = "SALDOS DEL MES: C) COMPENSACIONES CENTRALES DE GENERACIÓN DE ELECTRICIDAD CON ENERGÍA RENOVABLE (" + ConstantesTransfPotencia.MensajeSoles + ")";
                        rg = ws.Cells[rowaux, 2, row++, 2];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal[] dTotalColum2 = new decimal[1000]; // Donde se almacenan los Totales por columnas
                        iNumEmpresaCobro = 0;
                        colum = 4;
                        for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                    {
                                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                    }

                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.Emprruc != null) ? dtoEmpresaCobro.Emprruc.ToString().Trim() : string.Empty;
                                    int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                    if (dtoPeajeIngreso != null)
                                    {
                                        ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                        ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                    }
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum2[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row + 1, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum2[colum] = 0;
                                rg = ws.Cells[row, 3, row + 3, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;

                            }
                            break;
                        }
                        row = row + 4;
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                        {
                            if (dtoEmpresaPago.Emprcodipeaje != 10582)
                            {
                                ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Haciendo ajuste pgjunin
                                if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                {
                                    List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                    {
                                        for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                        {
                                            ListaPeajeEmpresaCobro[i].Pempagsaldoanterior += ListaAnterior[i].Pempagsaldoanterior;
                                            ListaPeajeEmpresaCobro[i].Pempagajuste += ListaAnterior[i].Pempagajuste;
                                        }
                                    }
                                }
                                //- Haciendo ajuste egjunin

                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    decimal dSaldoAnterior = Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                                    ws.Cells[row, colum].Value = dSaldoAnterior;
                                    dTotalRow += dSaldoAnterior;
                                    dTotalColum2[colum] += dSaldoAnterior;
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum2[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresaCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum2[colum];
                            ws.Column(colum).Width = 20;
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //Grupos de repartos
                        List<VtpRepaRecaPeajeDTO> ListaRepaRecaPeaje = (new TransfPotenciaAppServicio()).GetByCriteriaVtpRepaRecaPeajes(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                        foreach (VtpRepaRecaPeajeDTO dtoRepaRecaPeaje in ListaRepaRecaPeaje)
                        {
                            rowaux = row += 2; //Fila donde inicia la data
                            ws.Cells[row++, 2].Value = "SALDOS DEL MES: " + dtoRepaRecaPeaje.Rrpenombre + " (" + ConstantesTransfPotencia.MensajeSoles + ")";
                            rg = ws.Cells[rowaux, 2, row++, 2];
                            rg.Style.Font.Size = 12;
                            rg.Style.Font.Bold = true;
                            //CABECERA DE TABLA
                            ws.Cells[row + 1, 3].Value = "RUC";
                            ws.Cells[row + 1, 2].Value = "EMPRESA";
                            rg = ws.Cells[row, 2, row + 3, 3];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            iNumEmpresaCobro = 0;
                            colum = 4;
                            for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                    List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                    iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                    {
                                        if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                        {
                                            Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                            dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                        }

                                        ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                        dTotalColum2[colum] = 0; //Inicializando los valores
                                        colum++;
                                    }
                                    ws.Cells[row, colum].Value = "TOTAL";
                                    dTotalColum2[colum] = 0;
                                    rg = ws.Cells[row, 4, row, colum];
                                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                    rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.Font.Bold = true;
                                    rg.Style.Font.Size = 10;
                                }
                                break;
                            }
                            row = row + 1;
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                            {
                                if (dtoEmpresaPago.Emprcodipeaje != 10582)
                                {
                                    ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                    ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                    ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                    ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                    ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                    ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                    List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    //- Haciendo ajuste pgjunin
                                    if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                    {
                                        List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, 10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                        if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                        {
                                            for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                            {
                                                ListaPeajeEmpresaCobro[i].Pempagsaldoanterior += ListaAnterior[i].Pempagsaldoanterior;
                                                ListaPeajeEmpresaCobro[i].Pempagajuste += ListaAnterior[i].Pempagajuste;
                                            }
                                        }
                                    }
                                    //- Haciendo ajuste egjunin

                                    colum = 4;
                                    decimal dTotalRow = 0;
                                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                    {
                                        decimal dSaldoAnterior = Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                                        ws.Cells[row, colum].Value = dSaldoAnterior;
                                        dTotalRow += dSaldoAnterior;
                                        dTotalColum2[colum] += dSaldoAnterior;
                                        colum++;
                                    }
                                    ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                    dTotalColum2[colum] += dTotalRow;
                                    //Border por celda en la Fila
                                    rg = ws.Cells[row, 2, row, colum];
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
                            ws.Cells[row, 2].Value = "TOTAL";
                            ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                            colum = 4;
                            for (int i = 0; i <= iNumEmpresaCobro; i++)
                            {
                                ws.Cells[row, colum].Value = dTotalColum2[colum];
                                ws.Column(colum).Width = 20;
                                colum++;
                            }
                            rg = ws.Cells[row, 2, row, colum - 1];
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

                        ws.Column(3).Width = 13;
                        ws.Column(2).Width = 30;

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                        picture.SetPosition(10, 10);
                        picture.SetSize(135, 45);
                    }
                    #endregion
                }
                else
                {
                    #region RECALCULO
                    ws = xlPackage.Workbook.Worksheets.Add("SALDO");
                    if (ws != null)
                    {   //PAGO = SI & TRANSMISION = SI
                        int row = 2; //Fila donde inicia la data
                        ws.Cells[row++, 3].Value = "SALDOS DEL RECALCULO: A) COMPENSACIÓN A TRANSMISORAS";
                        ws.Cells[row++, 3].Value = "PEAJE POR  CONEXIÓN Y TRANSMISIÓN QUE CORRESPONDE PAGAR (" + ConstantesTransfPotencia.MensajeSoles + ")";
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                        int iNumEmpresaCobro = 0;
                        int colum = 4;
                        for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                    {
                                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                    }

                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.Emprruc != null) ? dtoEmpresaCobro.Emprruc.ToString().Trim() : string.Empty;
                                    int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                    if (dtoPeajeIngreso != null)
                                    {
                                        ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                        ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                    }
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row + 1, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                                rg = ws.Cells[row, 3, row + 3, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;

                            }
                            break;
                        }
                        row = row + 4;
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                        {
                            if (dtoEmpresaPago.Emprcodipeaje != 10582)
                            {
                                ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Haciendo ajuste pgjunin
                                if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                {
                                    List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                    {
                                        for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                        {
                                            ListaPeajeEmpresaCobro[i].Pempagsaldo += ListaAnterior[i].Pempagsaldo;
                                        }
                                    }
                                }
                                //- Haciendo ajuste egjunin

                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    ws.Cells[row, colum].Value = dtoEmpresaCobro.Pempagsaldo;
                                    dTotalRow += Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldo);
                                    dTotalColum[colum] += Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldo);
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Bold = false;
                                rg.Style.Font.Size = 10;
                                row++;
                            }
                        }
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresaCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            _saldos_.Add((double)dTotalColum[colum]);
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //PAGO = SI & TRANSMISION = NO
                        int rowaux = row += 2; //Fila donde inicia la data
                        ws.Cells[row++, 2].Value = "SALDOS DEL RECALCULO: C) COMPENSACIONES CENTRALES DE GENERACIÓN DE ELECTRICIDAD CON ENERGÍA RENOVABLE (" + ConstantesTransfPotencia.MensajeSoles + ")";
                        rg = ws.Cells[rowaux, 2, row++, 2];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal[] dTotalColum2 = new decimal[1000]; // Donde se almacenan los Totales por columnas
                        iNumEmpresaCobro = 0;
                        colum = 4;
                        for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                    {
                                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                    }

                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.Emprruc != null) ? dtoEmpresaCobro.Emprruc.ToString().Trim() : string.Empty;
                                    int iPingcodi = dtoEmpresaCobro.Pingcodi;
                                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                    if (dtoPeajeIngreso != null)
                                    {
                                        ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                        ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                    }
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum2[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row + 1, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum2[colum] = 0;
                                rg = ws.Cells[row, 4, row + 3, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;

                            }
                            break;
                        }
                        row = row + 4;
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                        {
                            if (dtoEmpresaPago.Emprcodipeaje != 10582)
                            {
                                ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Haciendo ajuste pgjunin
                                if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                {
                                    List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                    {
                                        for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                        {
                                            ListaPeajeEmpresaCobro[i].Pempagsaldo += ListaAnterior[i].Pempagsaldo;
                                        }
                                    }
                                }
                                //- Haciendo ajuste egjunin

                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                {
                                    ws.Cells[row, colum].Value = dtoEmpresaCobro.Pempagsaldo;
                                    dTotalRow += Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldo);
                                    dTotalColum2[colum] += Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldo);
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum2[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresaCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum2[colum];
                            ws.Column(colum).Width = 20;
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //Grupos de repartos
                        List<VtpRepaRecaPeajeDTO> ListaRepaRecaPeaje = (new TransfPotenciaAppServicio()).GetByCriteriaVtpRepaRecaPeajes(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                        foreach (VtpRepaRecaPeajeDTO dtoRepaRecaPeaje in ListaRepaRecaPeaje)
                        {
                            rowaux = row += 2; //Fila donde inicia la data
                            ws.Cells[row++, 2].Value = "SALDOS DEL RECALCULO: " + dtoRepaRecaPeaje.Rrpenombre + " (" + ConstantesTransfPotencia.MensajeSoles + ")";
                            rg = ws.Cells[rowaux, 2, row++, 2];
                            rg.Style.Font.Size = 12;
                            rg.Style.Font.Bold = true;
                            //CABECERA DE TABLA
                            ws.Cells[row + 1, 3].Value = "RUC";
                            ws.Cells[row + 1, 2].Value = "EMPRESA";
                            rg = ws.Cells[row, 2, row + 2, 3];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            iNumEmpresaCobro = 0;
                            colum = 4;
                            for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                                    List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                    iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                    {
                                        if (dtoEmpresaCobro.Emprcodicargo == 10582)
                                        {
                                            Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                                            dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                                        }

                                        ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() : string.Empty;
                                        dTotalColum2[colum] = 0; //Inicializando los valores
                                        colum++;
                                    }
                                    ws.Cells[row, colum].Value = "TOTAL";
                                    dTotalColum2[colum] = 0;
                                    rg = ws.Cells[row, 3, row, colum];
                                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                    rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rg.Style.Font.Bold = true;
                                    rg.Style.Font.Size = 10;
                                }
                                break;
                            }
                            row = row + 1;
                            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                            {
                                if (dtoEmpresaPago.Emprcodipeaje != 10582)
                                {
                                    ws.Cells[row, 3].Value = (dtoEmpresaPago.Emprruc != null) ? dtoEmpresaPago.Emprruc.ToString().Trim() : string.Empty;
                                    ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;
                                    ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                    ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                    ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                    ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                    List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    //- Haciendo ajuste pgjunin
                                    if (dtoEmpresaPago.Emprcodipeaje == 11153)
                                    {
                                        List<VtpPeajeEmpresaPagoDTO> ListaAnterior = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroReparto(dtoRepaRecaPeaje.Rrpecodi, 10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                        if (ListaPeajeEmpresaCobro.Count() == ListaAnterior.Count())
                                        {
                                            for (int i = 0; i < ListaPeajeEmpresaCobro.Count(); i++)
                                            {
                                                ListaPeajeEmpresaCobro[i].Pempagsaldo += ListaAnterior[i].Pempagsaldo;
                                            }
                                        }
                                    }
                                    //- Haciendo ajuste egjunin

                                    colum = 4;
                                    decimal dTotalRow = 0;
                                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                                    {
                                        ws.Cells[row, colum].Value = dtoEmpresaCobro.Pempagsaldo;
                                        dTotalRow += Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldo);
                                        dTotalColum2[colum] += Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldo);
                                        colum++;
                                    }
                                    ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                    dTotalColum2[colum] += dTotalRow;
                                    //Border por celda en la Fila
                                    rg = ws.Cells[row, 2, row, colum];
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
                            ws.Cells[row, 2].Value = "TOTAL";
                            ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                            colum = 4;
                            for (int i = 0; i <= iNumEmpresaCobro; i++)
                            {
                                ws.Cells[row, colum].Value = dTotalColum2[colum];
                                ws.Column(colum).Width = 20;
                                colum++;
                            }
                            rg = ws.Cells[row, 2, row, colum - 1];
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

                        ws.Column(3).Width = 13;
                        ws.Column(2).Width = 30;

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                        picture.SetPosition(10, 10);
                        picture.SetSize(135, 45);
                    }
                    #endregion
                }
                totales = _totales_;
                mensules = _mensules_;
                saldos = _saldos_;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el Reporte de Compensación a transmisoras por ingreso tarifario - CU19
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="verificaciones">Lista de verificaciones</param>
        /// <param name="hoja">Hoja excel</param>
        /// <returns></returns>
        public static void GenerarReporteVerificacionResultados(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, bool[] verificaciones, out ExcelWorksheet hoja)
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
                    int index = 2; //Fila donde inicia la data

                    ws.Cells[index++, 3].Value = "REPORTE DE VERIFICACION DE RESULTADOS";
                    ws.Cells[index++, 3].Value = EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[2, 3, index, 3];
                    rg.Style.Font.Size = 12;
                    rg.Style.Font.Bold = true;

                    index = 6;
                    ws.Cells[index, 2].Value = "Concepto";
                    ws.Cells[index, 3].Value = "Verificación";
                    ws.Cells[index, 4].Value = "Resultado";


                    rg = ws.Cells[index, 2, index, 4];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index = 7;
                    ws.Cells[7, 2].Value = "Matriz de Pagos de Peaje";
                    rg = ws.Cells[7, 2, 9, 2];
                    rg.Merge = true;
                    ws.Cells[7, 3].Value = "Totales de saldos igual a cero";
                    ws.Cells[8, 3].Value = "Total de peajes por conexión igual a suma de peaje mensual ingresado por STR";
                    ws.Cells[9, 3].Value = "Cada celda de la hoja totales es igual a la suma del mensual más los saldos";

                    ws.Cells[10, 2].Value = "Matriz de Ingresos Tarifarios";
                    rg = ws.Cells[10, 2, 12, 2];
                    rg.Merge = true;
                    ws.Cells[10, 3].Value = "Totales de saldos igual a cero";
                    ws.Cells[11, 3].Value = "Total de ingresos tarifarios igual a suma de ingresos tarifarios ingresados por STR";
                    ws.Cells[12, 3].Value = "Cada celda de la matriz totales es igual a la suma del mensual más los saldos";

                    ws.Cells[13, 2].Value = "Matriz de Pagos de Potencia";
                    rg = ws.Cells[13, 2, 15, 2];
                    rg.Merge = true;
                    ws.Cells[13, 3].Value = "Todas las empresas con saldos positivos aparecen en las columnas de la matriz de pagos";
                    ws.Cells[14, 3].Value = "Todas las empresas con saldos negativos aparecen en las filas de la matriz de pagos";
                    ws.Cells[15, 3].Value = "En ambos reportes el número de empresas es igual";

                    ws.Cells[16, 2].Value = "Egresos";
                    rg = ws.Cells[16, 2, 16, 2];
                    rg.Merge = true;
                    ws.Cells[16, 3].Value = "El total de la columna potencia egreso es igual a la suma de los valores de clientes contrato Bilateral y Cliente contrato Licitaciones";

                    ws.Cells[17, 2].Value = "Egresos y Saldos Peajes";
                    rg = ws.Cells[17, 2, 17, 2];
                    ws.Cells[17, 3].Value = "La suma de EGRESOS TOTALES POR COMPRAS DE POTENCIA (S/.) es igual al total al egreso por potencia y tambien es igual a la columna promedio de reporte ingreso por potencia";

                    ws.Cells[18, 2].Value = "Retiros no cubiertos";
                    rg = ws.Cells[18, 2, 18, 2];
                    ws.Cells[18, 3].Value = "El total de la columna potencia consumida del reporte de retiros no declarados es igual a la suma de la columna de retiros no declarados del reporte de resumen de informacion para VTP";

                    ws.Cells[19, 2].Value = "Saldos de Potencia";
                    rg = ws.Cells[19, 2, 20, 2];
                    rg.Merge = true;
                    ws.Cells[19, 3].Value = "Suma de saldos igual a cero";
                    ws.Cells[20, 3].Value = "Todos los periodos vinculados se encuentran en el reporte";

                    //Border por celda
                    rg = ws.Cells[7, 2, 20, 4];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);
                    //rg = ws.Cells[index, 3, index, 10];
                    //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rg.AutoFitColumns();

                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 90;
                    ws.Column(4).Width = 15;

                    index = 7;
                    for (int i = 0; i < verificaciones.Length; i++)
                    {
                        rg = ws.Cells[index, 4, index, 4];
                        if (verificaciones[i])
                        {
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#339900"));
                        }
                        else
                        {
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EE3300"));
                        }

                        index++;
                    }

                }
                hoja = ws;
                xlPackage.Save();
            }

        }

        /// <summary>
        /// Permite generar el Reporte de Compensación a transmisoras por ingreso tarifario - CU19
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaIngresoTarifarioPago">Lista de registros de VtpIngresoTarifarioDTO</param>
        /// <returns></returns>
        public static void GenerarReporteIngresoTarifario(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioPago,
            out ExcelWorksheet hoja,
            out List<double> totales,
            out List<double> mensules,
            out List<double> saldos)
        {
            List<double> _totales_ = new List<double>();
            List<double> _mensules_ = new List<double>();
            List<double> _saldos_ = new List<double>();

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
                    #region TOTALES DEL MES / VERSION
                    int row = 2; //Fila donde inicia la data
                                 //ws.Cells[row++, 3].Value = dtoRecalculo.RecaCuadro3;
                                 //ws.Cells[row++, 3].Value = dtoRecalculo.RecaNroInforme;
                    ws.Cells[row++, 3].Value = "COMPENSACIÓN A TRANSMISORAS POR INGRESO TARIFARIO";
                    ws.Cells[row++, 3].Value = EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[row++, 3].Value = "INGRESO TARIFARIO QUE CORRESPONDE PAGAR (" + ConstantesTransfPotencia.MensajeSoles + ")";
                    ExcelRange rg = ws.Cells[2, 3, row++, 3];
                    rg.Style.Font.Size = 12;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws.Cells[row + 1, 3].Value = "RUC";
                    ws.Cells[row + 1, 2].Value = "EMPRESA";
                    rg = ws.Cells[row, 2, row + 3, 3];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    int iNumEmpresasCobro = 0;
                    int colum = 4;
                    for (int i = 0; i < ListaIngresoTarifarioPago.Count(); i++)
                    {
                        if (i == 0)
                        {

                            int iEmprcodiPago = ListaIngresoTarifarioPago[0].Emprcodingpot;
                            List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                            iNumEmpresasCobro = ListaIngresoTarifarioCobro.Count();
                            foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                            {
                                ws.Cells[row, colum].Value = (dtoIngresoTarifarioCobro.Emprnombping != null) ? dtoIngresoTarifarioCobro.Emprnombping.ToString().Trim() : string.Empty;
                                ws.Cells[row + 1, colum].Value = (dtoIngresoTarifarioCobro.Emprruc != null) ? dtoIngresoTarifarioCobro.Emprruc.ToString().Trim() : string.Empty;
                                int iPingcodi = dtoIngresoTarifarioCobro.Pingcodi;
                                VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                if (dtoPeajeIngreso != null)
                                {
                                    ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                    ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                }
                                rg = ws.Cells[row, colum, row + 2, colum];
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0; //Inicializando los valores
                                colum++;
                            }
                            ws.Cells[row + 1, colum].Value = "TOTAL";
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                            dTotalColum[colum] = 0;
                            rg = ws.Cells[row, 4, row + 3, colum];
                            rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Font.Bold = true;
                            rg.Style.Font.Size = 10;
                        }
                        break;
                    }
                    row = row + 4;
                    foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioPago in ListaIngresoTarifarioPago)
                    {
                        if (dtoIngresoTarifarioPago.Emprcodingpot != 10582) //- Linea agregada egjunin
                        {//- Linea agregada egjunin

                            ws.Cells[row, 3].Value = (dtoIngresoTarifarioPago.Emprruc != null) ? dtoIngresoTarifarioPago.Emprruc.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Value = (dtoIngresoTarifarioPago.Emprnombingpot != null) ? dtoIngresoTarifarioPago.Emprnombingpot.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                            List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(dtoIngresoTarifarioPago.Emprcodingpot, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                            //- Linea agregada egjunin
                            if (dtoIngresoTarifarioPago.Emprcodingpot == 11153)
                            {
                                List<VtpIngresoTarifarioDTO> subList = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (ListaIngresoTarifarioCobro.Count() == subList.Count())
                                {
                                    for (int i = 0; i < ListaIngresoTarifarioCobro.Count(); i++)
                                    {
                                        ListaIngresoTarifarioCobro[i].Ingtarimporte += subList[i].Ingtarimporte;
                                        ListaIngresoTarifarioCobro[i].Ingtarsaldoanterior += subList[i].Ingtarsaldoanterior;
                                        ListaIngresoTarifarioCobro[i].Ingtarajuste += subList[i].Ingtarajuste;
                                    }
                                }
                            }
                            //- Linea agregada egjunin

                            colum = 4;
                            decimal dTotalRow = 0;
                            foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                            {
                                decimal dIngtarimporte = Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarimporte) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarsaldoanterior) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarajuste);
                                ws.Cells[row, colum].Value = dIngtarimporte;
                                dTotalRow += dIngtarimporte;
                                dTotalColum[colum] += dIngtarimporte;
                                colum++;
                            }
                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                            dTotalColum[colum] += dTotalRow;
                            //Border por celda en la Fila
                            rg = ws.Cells[row, 2, row, colum];
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

                        }//- Linea agregada egjunin

                    }
                    ws.Cells[row, 2].Value = "TOTAL";
                    ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                    ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                    colum = 4;
                    for (int i = 0; i <= iNumEmpresasCobro; i++)
                    {
                        ws.Cells[row, colum].Value = dTotalColum[colum];
                        _totales_.Add((double)dTotalColum[colum]);
                        colum++;
                    }
                    rg = ws.Cells[row, 2, row, colum - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    //Fijar panel
                    //ws.View.FreezePanes(9, 3);//fijo hasta la Fila 7 y columna 2
                    rg = ws.Cells[7, 2, row, colum];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                    #endregion
                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    if (EntidadRecalculoPotencia.Recpotcodi == 1)
                    {
                        #region MENSUAL
                        row += 2; //Fila donde inicia la data
                        ws.Cells[row, 2].Value = "CÁLCULOS DEL MES  - COMPENSACIÓN A TRANSMISORAS POR INGRESO TARIFARIO DEL SISTEMA PRINCIPAL Y GARANTIZADO DE TRANSMISIÓN";
                        rg = ws.Cells[row, 2, row, 2];
                        row++;
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        iNumEmpresasCobro = 0;
                        colum = 4;
                        for (int i = 0; i < ListaIngresoTarifarioPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaIngresoTarifarioPago[0].Emprcodingpot;
                                List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresasCobro = ListaIngresoTarifarioCobro.Count();
                                foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                                {
                                    ws.Cells[row, colum].Value = (dtoIngresoTarifarioCobro.Emprnombping != null) ? dtoIngresoTarifarioCobro.Emprnombping.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoIngresoTarifarioCobro.Emprruc != null) ? dtoIngresoTarifarioCobro.Emprruc.ToString().Trim() : string.Empty;
                                    int iPingcodi = dtoIngresoTarifarioCobro.Pingcodi;
                                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                    if (dtoPeajeIngreso != null)
                                    {
                                        ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                        ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                    }
                                    rg = ws.Cells[row, colum, row + 3, colum];
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row + 1, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                                rg = ws.Cells[row, 4, row + 3, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;
                            }
                            break;
                        }
                        row = row + 4;
                        foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioPago in ListaIngresoTarifarioPago)
                        {
                            if (dtoIngresoTarifarioPago.Emprcodingpot != 10582) //- Linea agregada egjunin
                            { //- Linea agregada

                                ws.Cells[row, 3].Value = (dtoIngresoTarifarioPago.Emprruc != null) ? dtoIngresoTarifarioPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoIngresoTarifarioPago.Emprnombingpot != null) ? dtoIngresoTarifarioPago.Emprnombingpot.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;
                                List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(dtoIngresoTarifarioPago.Emprcodingpot, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Linea agregada egjunin
                                if (dtoIngresoTarifarioPago.Emprcodingpot == 11153)
                                {
                                    List<VtpIngresoTarifarioDTO> subList = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaIngresoTarifarioCobro.Count() == subList.Count())
                                    {
                                        for (int i = 0; i < ListaIngresoTarifarioCobro.Count(); i++)
                                        {
                                            ListaIngresoTarifarioCobro[i].Ingtarimporte += subList[i].Ingtarimporte;
                                        }
                                    }
                                }
                                //- Linea agregada egjunin

                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                                {
                                    decimal dTotalMes = dtoIngresoTarifarioCobro.Ingtarimporte;
                                    ws.Cells[row, colum].Value = dTotalMes;
                                    dTotalRow += Convert.ToDecimal(dTotalMes);
                                    dTotalColum[colum] += Convert.ToDecimal(dTotalMes);
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                            }//- Linea agregada
                        }
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresasCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            _mensules_.Add((double)dTotalColum[colum]);
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        #endregion
                        #region SALDOS
                        row += 2; //Fila donde inicia la data
                        ws.Cells[row, 2].Value = "SALDOS DE MESES ANTERIORES - COMPENSACIÓN A TRANSMISORAS POR INGRESO TARIFARIO DEL SISTEMA PRINCIPAL Y GARANTIZADO DE TRANSMISIÓN";
                        rg = ws.Cells[row, 2, row, 2];
                        row++;
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        iNumEmpresasCobro = 0;
                        colum = 4;
                        for (int i = 0; i < ListaIngresoTarifarioPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaIngresoTarifarioPago[0].Emprcodingpot;
                                List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresasCobro = ListaIngresoTarifarioCobro.Count();
                                foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                                {
                                    ws.Cells[row, colum].Value = (dtoIngresoTarifarioCobro.Emprnombping != null) ? dtoIngresoTarifarioCobro.Emprnombping.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoIngresoTarifarioCobro.Emprruc != null) ? dtoIngresoTarifarioCobro.Emprruc.ToString().Trim() : string.Empty;
                                    int iPingcodi = dtoIngresoTarifarioCobro.Pingcodi;
                                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                    if (dtoPeajeIngreso != null)
                                    {
                                        ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                        ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                    }
                                    rg = ws.Cells[row, colum, row + 3, colum];
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row + 1, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                                rg = ws.Cells[row, 4, row + 3, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;
                            }
                            break;
                        }
                        row = row + 4;
                        foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioPago in ListaIngresoTarifarioPago)
                        {
                            if (dtoIngresoTarifarioPago.Emprcodingpot != 10582) //- Linea agregada egjunin
                            {//- Linea agregada egjunin

                                ws.Cells[row, 3].Value = (dtoIngresoTarifarioPago.Emprruc != null) ? dtoIngresoTarifarioPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoIngresoTarifarioPago.Emprnombingpot != null) ? dtoIngresoTarifarioPago.Emprnombingpot.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;
                                List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(dtoIngresoTarifarioPago.Emprcodingpot, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Linea agregada egjunin
                                if (dtoIngresoTarifarioPago.Emprcodingpot == 11153)
                                {
                                    List<VtpIngresoTarifarioDTO> subList = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                    if (ListaIngresoTarifarioCobro.Count() == subList.Count())
                                    {
                                        for (int i = 0; i < ListaIngresoTarifarioCobro.Count(); i++)
                                        {
                                            ListaIngresoTarifarioCobro[i].Ingtarsaldoanterior += subList[i].Ingtarsaldoanterior;
                                            ListaIngresoTarifarioCobro[i].Ingtarajuste += subList[i].Ingtarajuste;
                                        }
                                    }
                                }
                                //- Linea agregada egjunin


                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                                {
                                    decimal dSaldo = Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarsaldoanterior) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarajuste);
                                    ws.Cells[row, colum].Value = dSaldo;
                                    dTotalRow += dSaldo;
                                    dTotalColum[colum] += dSaldo;
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                            }//-Linea agregada egjunin
                        }
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresasCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            _saldos_.Add((double)dTotalColum[colum]);
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        #endregion
                    }
                    else
                    {
                        #region RECALCULO
                        row += 2; //Fila donde inicia la data
                        ws.Cells[row, 2].Value = "SALDOS ENTRE REVISIONES - COMPENSACIÓN A TRANSMISORAS POR INGRESO TARIFARIO DEL SISTEMA PRINCIPAL Y GARANTIZADO DE TRANSMISIÓN";
                        rg = ws.Cells[row, 3, row, 3];
                        row++;
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row + 1, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 3, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        iNumEmpresasCobro = 0;
                        colum = 4;
                        for (int i = 0; i < ListaIngresoTarifarioPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodiPago = ListaIngresoTarifarioPago[0].Emprcodingpot;
                                List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresasCobro = ListaIngresoTarifarioCobro.Count();
                                foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                                {
                                    ws.Cells[row, colum].Value = (dtoIngresoTarifarioCobro.Emprnombping != null) ? dtoIngresoTarifarioCobro.Emprnombping.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoIngresoTarifarioCobro.Emprruc != null) ? dtoIngresoTarifarioCobro.Emprruc.ToString().Trim() : string.Empty;
                                    int iPingcodi = dtoIngresoTarifarioCobro.Pingcodi;
                                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                                    if (dtoPeajeIngreso != null)
                                    {
                                        ws.Cells[row + 2, colum].Value = dtoPeajeIngreso.Pingnombre;
                                        ws.Cells[row + 3, colum].Value = dtoPeajeIngreso.Pingtipo;
                                    }
                                    rg = ws.Cells[row, colum, row + 2, colum];
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row + 1, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                                rg = ws.Cells[row, 4, row + 3, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;
                            }
                            break;
                        }
                        row = row + 4;
                        foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioPago in ListaIngresoTarifarioPago)
                        {
                            if (dtoIngresoTarifarioPago.Emprcodingpot != 10582) //- Linea agregada egjunin
                            {//- Linea agregada egjunin

                                ws.Cells[row, 3].Value = (dtoIngresoTarifarioPago.Emprruc != null) ? dtoIngresoTarifarioPago.Emprruc.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Value = (dtoIngresoTarifarioPago.Emprnombingpot != null) ? dtoIngresoTarifarioPago.Emprnombingpot.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                ws.Cells[row, 2, row, 3].Style.Font.Size = 10;
                                List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(dtoIngresoTarifarioPago.Emprcodingpot, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Linea agregada egjunin
                                if (dtoIngresoTarifarioPago.Emprcodingpot == 11153)
                                {
                                    List<VtpIngresoTarifarioDTO> subList = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaIngresoTarifarioCobro.Count() == subList.Count())
                                    {
                                        for (int i = 0; i < ListaIngresoTarifarioCobro.Count(); i++)
                                        {
                                            ListaIngresoTarifarioCobro[i].Ingtarsaldo += subList[i].Ingtarsaldo;
                                        }
                                    }
                                }
                                //- Linea agregada egjunin

                                colum = 4;
                                decimal dTotalRow = 0;
                                foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                                {
                                    ws.Cells[row, colum].Value = dtoIngresoTarifarioCobro.Ingtarsaldo;
                                    dTotalRow += Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarsaldo);
                                    dTotalColum[colum] += Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarsaldo);
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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

                            }//- linea agregada egjunin
                        }
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= iNumEmpresasCobro; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            _saldos_.Add((double)dTotalColum[colum]);
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        #endregion
                    }

                }
                hoja = ws;

                totales = _totales_;
                mensules = _mensules_;
                saldos = _saldos_;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el Reporte de Compensaciones incluidas en el peaje por conexión - CU20
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeCargoEmpresa">Lista de registros de VtpPeajeCargoDTO</param>
        /// <returns></returns>
        public static void GenerarReportePeajeRecaudado(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeCargoDTO> ListaPeajeCargoEmpresa, out ExcelWorksheet hoja)
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
                {   //TOTALES DEL MES
                    int row = 2; //Fila donde inicia la data
                                 //ws.Cells[row++, 3].Value = dtoRecalculo.RecaCuadro3;
                                 //ws.Cells[row++, 3].Value = dtoRecalculo.RecaNroInforme;
                    ws.Cells[row++, 3].Value = "COMPENSACIONES INCLUIDAS EN EL PEAJE POR CONEXIÓN";
                    ws.Cells[row++, 3].Value = EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[row++, 3].Value = "PEAJES RECAUDADOS (" + ConstantesTransfPotencia.MensajeSoles + ")";
                    ExcelRange rg = ws.Cells[2, 3, row++, 3];
                    rg.Style.Font.Size = 12;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws.Cells[row, 2].Value = "EMPRESA";
                    rg = ws.Cells[row, 2, row, 2];
                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Font.Size = 10;

                    decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    int iNumEmpresaCargo = 0;
                    int colum = 3;
                    for (int i = 0; i < ListaPeajeCargoEmpresa.Count(); i++)
                    {
                        if (i == 0)
                        {
                            int iEmprcodi = ListaPeajeCargoEmpresa[0].Emprcodi;
                            //Lista de cargos donde pago = no
                            List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(iEmprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                            iNumEmpresaCargo = ListaPeajeCargo.Count();
                            foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                            {
                                ws.Cells[row, colum].Value = (dtoCargo.Pingnombre != null) ? dtoCargo.Pingnombre.ToString().Trim() : string.Empty;
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0; //Inicializando los valores
                                colum++;
                            }
                            ws.Cells[row, colum].Value = "TOTAL";
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                            dTotalColum[colum] = 0;
                            rg = ws.Cells[row, 3, row, colum];
                            rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Font.Bold = true;
                            rg.Style.Font.Size = 10;
                        }
                        break;
                    }
                    row = row + 1;
                    foreach (VtpPeajeCargoDTO dtoCargoEmpresa in ListaPeajeCargoEmpresa)
                    {
                        if (dtoCargoEmpresa.Emprcodi != 10582) //-Linea modificada etjunin
                        {
                            ws.Cells[row, 2].Value = (dtoCargoEmpresa.Emprnomb != null) ? dtoCargoEmpresa.Emprnomb.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2].Style.Font.Bold = true;
                            ws.Cells[row, 2].Style.Font.Size = 10;

                            List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(dtoCargoEmpresa.Emprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                            //- Linea modificada etjunin

                            if (dtoCargoEmpresa.Emprcodi == 11153)
                            {
                                List<VtpPeajeCargoDTO> listAdicional = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (ListaPeajeCargo.Count() == listAdicional.Count())
                                {
                                    for (int i = 0; i < ListaPeajeCargo.Count(); i++)
                                    {
                                        ListaPeajeCargo[i].Pecarpeajerecaudado += listAdicional[i].Pecarpeajerecaudado;
                                        ListaPeajeCargo[i].Pecarsaldoanterior += listAdicional[i].Pecarsaldoanterior;
                                        ListaPeajeCargo[i].Pecarajuste += listAdicional[i].Pecarajuste;
                                    }
                                }
                            }

                            //- Linea modifcada etjunin

                            colum = 3;
                            decimal dTotalRow = 0;
                            foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                            {
                                decimal dPecarpeajerecaudado = Convert.ToDecimal(dtoCargo.Pecarpeajerecaudado) + Convert.ToDecimal(dtoCargo.Pecarsaldoanterior) + Convert.ToDecimal(dtoCargo.Pecarajuste);
                                ws.Cells[row, colum].Value = dPecarpeajerecaudado;
                                dTotalRow += dPecarpeajerecaudado;
                                dTotalColum[colum] += dPecarpeajerecaudado;
                                colum++;
                            }
                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                            dTotalColum[colum] += dTotalRow;
                            //Border por celda en la Fila
                            rg = ws.Cells[row, 2, row, colum];
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
                    ws.Cells[row, 2].Value = "TOTAL";
                    ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 2].Style.Font.Bold = true;
                    ws.Cells[row, 2].Style.Font.Size = 10;

                    colum = 3;
                    for (int i = 0; i <= iNumEmpresaCargo; i++)
                    {
                        ws.Cells[row, colum].Value = dTotalColum[colum];
                        colum++;
                    }
                    rg = ws.Cells[row, 2, row, colum - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    //Fijar panel
                    //ws.View.FreezePanes(7, 3);//fijo hasta la Fila 7 y columna 3
                    rg = ws.Cells[7, 2, row, colum];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    if (EntidadRecalculoPotencia.Recpotcodi == 1)
                    {
                        #region VERSION MENSUAL
                        //CALCULO DEL MES
                        row += 2; //Fila donde inicia la data
                        ws.Cells[row, 2].Value = "CÁLCULOS DEL MES POR  PEAJE  DE  CONEXIÓN AL SISTEMA PRINCIPAL DE TRANSMISIÓN";
                        rg = ws.Cells[row, 2, row, 2];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        row++;
                        //CABECERA DE TABLA
                        ws.Cells[row, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row, 2];
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Font.Bold = true;
                        rg.Style.Font.Size = 10;

                        iNumEmpresaCargo = 0;
                        colum = 3;
                        for (int i = 0; i < ListaPeajeCargoEmpresa.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodi = ListaPeajeCargoEmpresa[0].Emprcodi;
                                //Lista de cargos donde pago = no
                                List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(iEmprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCargo = ListaPeajeCargo.Count();
                                foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                                {
                                    ws.Cells[row, colum].Value = (dtoCargo.Pingnombre != null) ? dtoCargo.Pingnombre.ToString().Trim() : string.Empty;
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                                rg = ws.Cells[row, 3, row, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;
                            }
                            break;
                        }
                        row = row + 1;
                        foreach (VtpPeajeCargoDTO dtoCargoEmpresa in ListaPeajeCargoEmpresa)
                        {
                            if (dtoCargoEmpresa.Emprcodi != 10582)//- Linea modificada egjunin
                            {
                                ws.Cells[row, 2].Value = (dtoCargoEmpresa.Emprnomb != null) ? dtoCargoEmpresa.Emprnomb.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2].Style.Font.Bold = true;
                                ws.Cells[row, 2].Style.Font.Size = 10;

                                List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(dtoCargoEmpresa.Emprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                //- Linea modificada egjunin
                                if (dtoCargoEmpresa.Emprcodi == 11153)
                                {
                                    List<VtpPeajeCargoDTO> listAdicional = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeCargo.Count() == listAdicional.Count())
                                    {
                                        for (int i = 0; i < ListaPeajeCargo.Count(); i++)
                                        {
                                            ListaPeajeCargo[i].Pecarpeajerecaudado += listAdicional[i].Pecarpeajerecaudado;
                                        }
                                    }
                                }

                                //- Linea modificada egjunin

                                colum = 3;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                                {
                                    decimal dTotalMes = dtoCargo.Pecarpeajerecaudado;
                                    ws.Cells[row, colum].Value = dTotalMes;
                                    dTotalRow += Convert.ToDecimal(dTotalMes);
                                    dTotalColum[colum] += Convert.ToDecimal(dTotalMes);
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2].Style.Font.Bold = true;
                        ws.Cells[row, 2].Style.Font.Size = 10;

                        colum = 3;
                        for (int i = 0; i <= iNumEmpresaCargo; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //SALDO DE MESES ANTERIORES
                        row += 2; //Fila donde inicia la data
                        ws.Cells[row, 2].Value = "SALDOS ANTERIORES POR  PEAJE  DE  CONEXIÓN AL SISTEMA PRINCIPAL DE TRANSMISIÓN";
                        rg = ws.Cells[row, 2, row, 2];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        row++;
                        //CABECERA DE TABLA
                        ws.Cells[row, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row, 2];
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Font.Bold = true;
                        rg.Style.Font.Size = 10;

                        iNumEmpresaCargo = 0;
                        colum = 3;
                        for (int i = 0; i < ListaPeajeCargoEmpresa.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodi = ListaPeajeCargoEmpresa[0].Emprcodi;
                                //Lista de cargos donde pago = no
                                List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(iEmprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCargo = ListaPeajeCargo.Count();
                                foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                                {
                                    ws.Cells[row, colum].Value = (dtoCargo.Pingnombre != null) ? dtoCargo.Pingnombre.ToString().Trim() : string.Empty;
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                                rg = ws.Cells[row, 3, row, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;
                            }
                            break;
                        }
                        row = row + 1;
                        foreach (VtpPeajeCargoDTO dtoCargoEmpresa in ListaPeajeCargoEmpresa)
                        {
                            if (dtoCargoEmpresa.Emprcodi != 10582)//- Linea agregada egjunin
                            {
                                ws.Cells[row, 2].Value = (dtoCargoEmpresa.Emprnomb != null) ? dtoCargoEmpresa.Emprnomb.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2].Style.Font.Bold = true;
                                ws.Cells[row, 2].Style.Font.Size = 10;

                                List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(dtoCargoEmpresa.Emprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);


                                //- Linea modificada egjunin
                                if (dtoCargoEmpresa.Emprcodi == 11153)
                                {
                                    List<VtpPeajeCargoDTO> listAdicional = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeCargo.Count() == listAdicional.Count())
                                    {
                                        for (int i = 0; i < ListaPeajeCargo.Count(); i++)
                                        {
                                            ListaPeajeCargo[i].Pecarsaldoanterior += listAdicional[i].Pecarsaldoanterior;
                                            ListaPeajeCargo[i].Pecarajuste += listAdicional[i].Pecarajuste;
                                        }
                                    }
                                }
                                //- Linea modificada egjunin


                                colum = 3;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                                {
                                    decimal dSaldo = Convert.ToDecimal(dtoCargo.Pecarsaldoanterior) + Convert.ToDecimal(dtoCargo.Pecarajuste);
                                    ws.Cells[row, colum].Value = dSaldo;
                                    dTotalRow += dSaldo;
                                    dTotalColum[colum] += dSaldo;
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2].Style.Font.Bold = true;
                        ws.Cells[row, 2].Style.Font.Size = 10;

                        colum = 3;
                        for (int i = 0; i <= iNumEmpresaCargo; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        #endregion
                    }
                    else
                    {
                        #region RECALCULO
                        row += 2; //Fila donde inicia la data
                        ws.Cells[row, 2].Value = "SALDO DE LA VERSION ANTERIOR POR  PEAJE  DE  CONEXIÓN AL SISTEMA PRINCIPAL DE TRANSMISIÓN";
                        rg = ws.Cells[row, 2, row, 2];
                        rg.Style.Font.Size = 12;
                        rg.Style.Font.Bold = true;
                        row++;
                        //CABECERA DE TABLA
                        ws.Cells[row, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row, 2];
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Font.Bold = true;
                        rg.Style.Font.Size = 10;

                        iNumEmpresaCargo = 0;
                        colum = 3;
                        for (int i = 0; i < ListaPeajeCargoEmpresa.Count(); i++)
                        {
                            if (i == 0)
                            {
                                int iEmprcodi = ListaPeajeCargoEmpresa[0].Emprcodi;
                                //Lista de cargos donde pago = no
                                List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(iEmprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                                iNumEmpresaCargo = ListaPeajeCargo.Count();
                                foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                                {
                                    ws.Cells[row, colum].Value = (dtoCargo.Pingnombre != null) ? dtoCargo.Pingnombre.ToString().Trim() : string.Empty;
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = "TOTAL";
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                                rg = ws.Cells[row, 3, row, colum];
                                rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                rg.Style.Font.Bold = true;
                                rg.Style.Font.Size = 10;
                            }
                            break;
                        }
                        row = row + 1;
                        foreach (VtpPeajeCargoDTO dtoCargoEmpresa in ListaPeajeCargoEmpresa)
                        {
                            if (dtoCargoEmpresa.Emprcodi != 10582)
                            {
                                ws.Cells[row, 2].Value = (dtoCargoEmpresa.Emprnomb != null) ? dtoCargoEmpresa.Emprnomb.ToString().Trim() : string.Empty;
                                ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[row, 2].Style.Font.Bold = true;
                                ws.Cells[row, 2].Style.Font.Size = 10;

                                List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(dtoCargoEmpresa.Emprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);


                                //- Linea modificada egjunin
                                if (dtoCargoEmpresa.Emprcodi == 11153)
                                {
                                    List<VtpPeajeCargoDTO> listAdicional = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                    if (ListaPeajeCargo.Count() == listAdicional.Count())
                                    {
                                        for (int i = 0; i < ListaPeajeCargo.Count(); i++)
                                        {
                                            ListaPeajeCargo[i].Pecarsaldo += listAdicional[i].Pecarsaldo;
                                        }
                                    }
                                }
                                //- Linea modificada egjunin


                                colum = 3;
                                decimal dTotalRow = 0;
                                foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                                {
                                    ws.Cells[row, colum].Value = dtoCargo.Pecarsaldo;
                                    dTotalRow += Convert.ToDecimal(dtoCargo.Pecarsaldo);
                                    dTotalColum[colum] += Convert.ToDecimal(dtoCargo.Pecarsaldo);
                                    colum++;
                                }
                                ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                                dTotalColum[colum] += dTotalRow;
                                //Border por celda en la Fila
                                rg = ws.Cells[row, 2, row, colum];
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
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2].Style.Font.Bold = true;
                        ws.Cells[row, 2].Style.Font.Size = 10;

                        colum = 3;
                        for (int i = 0; i <= iNumEmpresaCargo; i++)
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        #endregion
                    }
                }
                hoja = ws;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el Reporte de Resumen de información VTP - CU21
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeEgreso">Lista de registros de VtpPeajeEgresoMinfoDTO</param>
        /// <returns></returns>
        public static void GenerarReportePotenciaValor(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgreso, out ExcelWorksheet hoja)
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
                    ws.Cells[index, 3].Value = "RESUMEN DE INFORMACIÓN VTP";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 3].Value = "POTENCIA CONSUMIDA (kW)";
                    rg = ws.Cells[index, 3, index, 6];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Cells[index, 7].Value = "VALORIZACIÓN DE CONSUMOS (S/)";
                    rg = ws.Cells[index, 7, index, 10];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    rg = ws.Cells[index, 3, index, 10];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "CLIENTES CONTRATO BILATERAL";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 4].Value = "CLIENTES CONTRATO LICITACIONES";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 5].Value = "RETIRO NO DECLARADO";  //ASSETEC 20200421
                    ws.Column(5).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 6].Value = "TOTAL";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 7].Value = "CLIENTES CONTRATO BILATERAL";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 8].Value = "CLIENTES CONTRATO LICITACIONES";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 9].Value = "RETIRO NO DECLARADO";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 10].Value = "TOTAL";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.000";

                    rg = ws.Cells[index, 2, index, 10];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    decimal dTotalPotenciaBilateral = 0;
                    decimal dTotalPotenciaLicitacion = 0;
                    decimal dTotalPotenciaSinContrato = 0;
                    decimal dTotalPotencia = 0;
                    decimal dTotalValorizacionBilateral = 0;
                    decimal dTotalValorizacionLicitacion = 0;
                    decimal dTotalValorizacionSinContrato = 0;
                    decimal dTotalValorizacion = 0;
                    index++;
                    foreach (var item in ListaPeajeEgreso)
                    {
                        if (item.Genemprcodi != 10582) //- Linea agregada egjunin
                        {
                            //- Linea agregada egjunin
                            if (item.Genemprcodi == 11153)
                            {
                                VtpPeajeEgresoMinfoDTO entityAdicional = ListaPeajeEgreso.Where(x => x.Genemprcodi == 10582).FirstOrDefault();

                                if (entityAdicional != null)
                                {
                                    item.Pegrmipotecalculada += entityAdicional.Pegrmipotecalculada;
                                    item.Pegrmipotedeclarada += entityAdicional.Pegrmipotedeclarada;
                                    item.Pegrmipreciopote += entityAdicional.Pegrmipreciopote;
                                    item.Pegrmipoteegreso += entityAdicional.Pegrmipoteegreso;
                                    item.Pegrmipeajeunitario += entityAdicional.Pegrmipeajeunitario;
                                    item.Pegrmipoteactiva += entityAdicional.Pegrmipoteactiva;
                                }
                            }
                            //- Linea agregada egjunin

                            ws.Cells[index, 2].Value = item.Genemprnombre.ToString();
                            ws.Cells[index, 3].Value = item.Pegrmipotecalculada;
                            dTotalPotenciaBilateral += Convert.ToDecimal(item.Pegrmipotecalculada);
                            ws.Cells[index, 4].Value = item.Pegrmipotedeclarada;
                            dTotalPotenciaLicitacion += Convert.ToDecimal(item.Pegrmipotedeclarada);
                            ws.Cells[index, 5].Value = item.Pegrmipreciopote;
                            dTotalPotenciaSinContrato += Convert.ToDecimal(item.Pegrmipreciopote);
                            ws.Cells[index, 6].Value = item.Pegrmipotecalculada + item.Pegrmipotedeclarada + item.Pegrmipreciopote;
                            dTotalPotencia += Convert.ToDecimal(item.Pegrmipotecalculada) + Convert.ToDecimal(item.Pegrmipotedeclarada) + Convert.ToDecimal(item.Pegrmipreciopote);
                            ws.Cells[index, 7].Value = item.Pegrmipoteegreso;
                            dTotalValorizacionBilateral += Convert.ToDecimal(item.Pegrmipoteegreso);
                            ws.Cells[index, 8].Value = item.Pegrmipeajeunitario;
                            dTotalValorizacionLicitacion += Convert.ToDecimal(item.Pegrmipeajeunitario);
                            ws.Cells[index, 9].Value = item.Pegrmipoteactiva;
                            dTotalValorizacionSinContrato += Convert.ToDecimal(item.Pegrmipoteactiva);
                            ws.Cells[index, 10].Value = item.Pegrmipoteegreso + item.Pegrmipeajeunitario + item.Pegrmipoteactiva;
                            dTotalValorizacion += Convert.ToDecimal(item.Pegrmipoteegreso) + Convert.ToDecimal(item.Pegrmipeajeunitario) + Convert.ToDecimal(item.Pegrmipoteactiva);
                            //Border por celda
                            rg = ws.Cells[index, 2, index, 10];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, 10];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            index++;

                        }

                    }
                    if (dTotalValorizacion > 0)
                    {
                        ws.Cells[index, 2].Value = "TOTAL";
                        ws.Cells[index, 3].Value = dTotalPotenciaBilateral;
                        ws.Cells[index, 4].Value = dTotalPotenciaLicitacion;
                        ws.Cells[index, 5].Value = dTotalPotenciaSinContrato;
                        ws.Cells[index, 6].Value = dTotalPotencia;
                        ws.Cells[index, 7].Value = dTotalValorizacionBilateral;
                        ws.Cells[index, 8].Value = dTotalValorizacionLicitacion;
                        ws.Cells[index, 9].Value = dTotalValorizacionSinContrato;
                        ws.Cells[index, 10].Value = dTotalValorizacion;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 10];
                        rg = ObtenerEstiloCelda(rg, 1);
                        rg = ws.Cells[index, 3, index, 10];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    //DETALLE DE PERDIDA
                    decimal dMaximaDemanda = Convert.ToDecimal(EntidadRecalculoPotencia.Recpotmaxidemamensual);
                    if (dMaximaDemanda == 0) dMaximaDemanda = 1;
                    ws.Cells[index, 2].Value = "Máxima Demanda a nivel de generación:";
                    ws.Cells[index, 2, index, 5].Merge = true;
                    ws.Cells[index, 6].Value = dMaximaDemanda;
                    index++;
                    decimal dServicioAuxiliar = Convert.ToDecimal(EntidadRecalculoPotencia.Recpotpreciodemaservauxiliares);
                    ws.Cells[index, 2].Value = "Demanda de Servicios Auxiliares de centrales de generación:";
                    ws.Cells[index, 2, index, 5].Merge = true;
                    ws.Cells[index, 6].Value = dServicioAuxiliar;
                    index++;
                    decimal dConsumidaDemanda = Convert.ToDecimal(EntidadRecalculoPotencia.Recpotconsumidademanda);
                    ws.Cells[index, 2].Value = "Potencia consumida por demanda de carácter no regulada sin contratos:";
                    ws.Cells[index, 2, index, 5].Merge = true;
                    ws.Cells[index, 6].Value = dConsumidaDemanda;
                    decimal dPerdida = (dMaximaDemanda - dTotalPotencia - dServicioAuxiliar - dConsumidaDemanda) * 100 / dMaximaDemanda;
                    index++;
                    ws.Cells[index, 2, index, 5].Merge = true;
                    index++;
                    ws.Cells[index, 2].Value = "Pérdidas (%) :";
                    ws.Cells[index, 2, index, 5].Merge = true;
                    ws.Cells[index, 6].Value = dPerdida;
                    //Border por celda
                    rg = ws.Cells[index - 4, 2, index, 6];
                    rg = ObtenerEstiloCelda(rg, 0);
                    rg = ws.Cells[index - 4, 6, index, 6];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rg = ws.Cells[index, 6, index, 6];
                    rg.Style.Numberformat.Format = "#0\\.00%";

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                hoja = ws;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el Reporte de Egresos por compra de potencia - CU22
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaEmpresaEgreso">Lista de registros de VtpPeajeSaldoTransmisionDTO</param>
        /// <returns></returns>
        public static void GenerarReporteEgresos(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPagoEgresoDTO> ListaPagoEgreso, out ExcelWorksheet hoja)
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
                    ws.Cells[index, 3].Value = "EGRESO POR COMPRA DE POTENCIA";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "EGRESO POR COMPRA DE POTENCIA (S/)";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 4].Value = "SALDO POR PEAJE (S/)";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 5].Value = "EGRESO TOTAL POR COMPRA DE POTENCIA (S/)";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.000";

                    rg = ws.Cells[index, 2, index, 5];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    decimal dEgreso = 0;
                    decimal dSaldo = 0;
                    decimal dTotalEgreso = 0;
                    index++;
                    foreach (VtpPagoEgresoDTO dtoEgreso in ListaPagoEgreso)
                    {
                        if (dtoEgreso.Emprcodi != 10582)
                        {
                            if (dtoEgreso.Emprcodi == 11153)
                            {
                                VtpPagoEgresoDTO anterior = ListaPagoEgreso.Where(x => x.Emprcodi == 10582).FirstOrDefault();
                                if (anterior != null)
                                {
                                    dtoEgreso.Pagegregreso += anterior.Pagegregreso;
                                    dtoEgreso.Pagegrsaldo += anterior.Pagegrsaldo;
                                    dtoEgreso.Pagegrpagoegreso += anterior.Pagegrpagoegreso;
                                }
                            }

                            //Los atributos: Pstrnstotalrecaudacion, Pstrnstotalpago, Pstrnssaldotransmision son empleado para leer la información de la BD
                            ws.Cells[index, 2].Value = dtoEgreso.Emprnomb;
                            ws.Cells[index, 3].Value = dtoEgreso.Pagegregreso;
                            dEgreso += dtoEgreso.Pagegregreso;
                            ws.Cells[index, 4].Value = dtoEgreso.Pagegrsaldo;
                            dSaldo += dtoEgreso.Pagegrsaldo;
                            ws.Cells[index, 5].Value = dtoEgreso.Pagegrpagoegreso;
                            dTotalEgreso += dtoEgreso.Pagegrpagoegreso;
                            //Border por celda
                            rg = ws.Cells[index, 2, index, 5];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, 5];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            index++;
                        }

                    }
                    if (dEgreso > 0)
                    {
                        ws.Cells[index, 2].Value = "TOTAL";
                        rg = ws.Cells[index, 2, index, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        ws.Cells[index, 3].Value = dEgreso;
                        ws.Cells[index, 4].Value = dSaldo;
                        ws.Cells[index, 5].Value = dTotalEgreso;
                        rg = ws.Cells[index, 3, index, 5];
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(6, 0);
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
                hoja = ws;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el Reporte de Ingresos por Potencia - CU23
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaIngresoPotenciaEmpresa">Lista de registros de VtpIngresoPotUnidPromdDTO</param>
        /// <param name="ListaIngresoPotEFR">Lista de registros de VtpIngresoPotefrDTO</param>
        /// <returns></returns>
        public static void GenerarReporteIngresoPotencia(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpIngresoPotUnidPromdDTO> ListaIngresoPotenciaEmpresa,
            List<VtpIngresoPotefrDTO> ListaIngresoPotEFR, out ExcelWorksheet hoja)
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
                    ws.Cells[index, 3].Value = "INGRESOS POR POTENCIA POR GENERADOR INTEGRANTE Y CENTRAL DE GENERACIÓN";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[index + 2, 3].Value = "" + ConstantesTransfPotencia.MensajeSoles + ")";
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    //Segunda Fila de cabecera
                    index++;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    rg = ws.Cells[index, 2, index + 1, 2];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Cells[index, 3].Value = "CENTRAL / UNIDAD";
                    rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    int iColumna = 4;
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                    {
                        ws.Cells[index, iColumna].Value = "Periodo " + dtoIngresoPotEFR.Ipefrintervalo;
                        ws.Cells[index + 1, iColumna].Value = dtoIngresoPotEFR.Ipefrdescripcion;
                        ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        iColumna++;
                    }
                    ws.Cells[index, iColumna].Value = "PROMEDIO";
                    rg = ws.Cells[index, iColumna, index + 1, iColumna];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 2, index + 1, iColumna];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index += 2;
                    foreach (VtpIngresoPotUnidPromdDTO dtoIngresoPotencia in ListaIngresoPotenciaEmpresa)
                    {
                        //- Linea agregada egjunin

                        if (dtoIngresoPotencia.Emprcodi == 10582)
                        {
                            Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                            dtoIngresoPotencia.Emprnomb = empresa.Emprnomb;
                        }

                        //- Linea agregada egjunin

                        ws.Cells[index, 2].Value = dtoIngresoPotencia.Emprnomb;
                        ws.Cells[index, 3].Value = dtoIngresoPotencia.Equinomb;
                        int iAux = 4;
                        foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                        {
                            List<VtpIngresoPotUnidIntervlDTO> ListaIngresoPotenciaUnidad = (new TransfPotenciaAppServicio()).GetByCriteriaVtpIngresoPotUnidIntervl(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, dtoIngresoPotencia.Emprcodi, dtoIngresoPotencia.Equicodi, dtoIngresoPotEFR.Ipefrcodi);
                            foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoIntervalo in ListaIngresoPotenciaUnidad)
                            {
                                ws.Cells[index, iAux].Value = dtoIngresoIntervalo.Inpuinimporte;
                                break;
                            }
                            iAux++;
                        }
                        ws.Cells[index, iColumna].Value = dtoIngresoPotencia.Inpuprimportepromd;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 4, index, iColumna];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    //Fijar panel
                    //ws.View.FreezePanes(6, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    int iAux2 = 4;
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                    {
                        ws.Column(iAux2).Width = 20;
                        iAux2++;
                    }
                    ws.Column(iColumna).Width = 20;
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //SEGUNDO REPORTE
                    index++;
                    List<VtpIngresoPotenciaDTO> ListaVtpIngresoPotencia = (new TransfPotenciaAppServicio()).ListVtpIngresoPotenciaEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                    ws.Cells[index, 2].Value = "EMPRESA";
                    rg = ws.Cells[index, 2, index + 1, 2];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    int iAux3 = 3;
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                    {
                        ws.Cells[index, iAux3].Value = "Periodo " + dtoIngresoPotEFR.Ipefrintervalo;
                        ws.Cells[index + 1, iAux3].Value = dtoIngresoPotEFR.Ipefrdescripcion;
                        iAux3++;
                    }
                    ws.Cells[index, iAux3].Value = "PROMEDIO";
                    rg = ws.Cells[index, iAux3, index + 1, iAux3];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 2, index + 1, iAux3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    index += 2;
                    foreach (VtpIngresoPotenciaDTO dtoIngresoPotencia in ListaVtpIngresoPotencia)
                    {
                        if (dtoIngresoPotencia.Emprcodi != 10582) //- Linea agregada egjunin
                        {//- Linea agregada egjunin

                            //- Linea agrega egjunin

                            if (dtoIngresoPotencia.Emprcodi == 11153)
                            {
                                VtpIngresoPotenciaDTO anterior = ListaVtpIngresoPotencia.Where(x => x.Emprcodi == 10582).FirstOrDefault();

                                if (anterior != null)
                                {
                                    dtoIngresoPotencia.Potipimporte += anterior.Potipimporte;
                                }
                            }

                            //- Fin linea agregada egjunin

                            ws.Cells[index, 2].Value = dtoIngresoPotencia.Emprnomb;
                            int iAux = 3;
                            foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                            {
                                List<VtpIngresoPotUnidIntervlDTO> ListaIngresoPotenciaUnidad = (new TransfPotenciaAppServicio()).ListVtpIngresoPotUnidIntervlSumIntervlEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, (int)dtoIngresoPotencia.Emprcodi, dtoIngresoPotEFR.Ipefrcodi);

                                //- Linea agregada egjunin

                                if (dtoIngresoPotencia.Emprcodi == 11153)
                                {
                                    List<VtpIngresoPotUnidIntervlDTO> ListaTemportal = (new TransfPotenciaAppServicio()).ListVtpIngresoPotUnidIntervlSumIntervlEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, 10582, dtoIngresoPotEFR.Ipefrcodi);

                                    if (ListaIngresoPotenciaUnidad.Count() == ListaTemportal.Count())
                                    {
                                        for (int i = 0; i < ListaIngresoPotenciaUnidad.Count(); i++)
                                        {
                                            ListaIngresoPotenciaUnidad[i].Inpuinimporte += ListaTemportal[i].Inpuinimporte;
                                        }
                                    }
                                }

                                //- Linea agregada egjunin

                                foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoIntervalo in ListaIngresoPotenciaUnidad)
                                {
                                    ws.Cells[index, iAux].Value = dtoIngresoIntervalo.Inpuinimporte;
                                    break;
                                }
                                iAux++;
                            }

                            ws.Cells[index, iAux3].Value = dtoIngresoPotencia.Potipimporte;
                            //Border por celda
                            rg = ws.Cells[index, 2, index, iAux3];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, iAux3];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rg.Style.Numberformat.Format = "#,##0.00";
                            index++;

                        } //- Linea agregada egjunin            
                    }

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                hoja = ws;
                xlPackage.Save();
            }

        }

        /// <summary>
        /// Permite generar el Reporte de Ingresos por Potencia con central separado
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="EntidadRecalculoPotencia"></param>
        /// <param name="ListaIngresoPotenciaEmpresa"></param>
        /// <param name="ListaIngresoPotEFR"></param>
        /// <param name="hoja"></param>
        public static void GenerarReporteIngresoPotenciaPfr(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpIngresoPotUnidPromdDTO> ListaIngresoPotenciaEmpresa,
    List<VtpIngresoPotefrDTO> ListaIngresoPotEFR, out ExcelWorksheet hoja)
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
                    ws.Cells[index, 3].Value = "INGRESOS POR POTENCIA POR GENERADOR INTEGRANTE Y CENTRAL DE GENERACIÓN";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[index + 2, 3].Value = "" + ConstantesTransfPotencia.MensajeSoles + ")";
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    //Segunda fila de cabecera
                    index++;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    rg = ws.Cells[index, 2, index + 1, 2];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Cells[index, 3].Value = "CENTRAL";
                    rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Cells[index, 4].Value = "UNIDAD";
                    rg = ws.Cells[index, 4, index + 1, 4];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    int iColumna = 5;
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                    {
                        ws.Cells[index, iColumna].Value = "Periodo " + dtoIngresoPotEFR.Ipefrintervalo;
                        ws.Cells[index + 1, iColumna].Value = dtoIngresoPotEFR.Ipefrdescripcion;
                        ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        iColumna++;
                    }
                    ws.Cells[index, iColumna].Value = "PROMEDIO";
                    rg = ws.Cells[index, iColumna, index + 1, iColumna];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 2, index + 1, iColumna];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index += 2;
                    foreach (VtpIngresoPotUnidPromdDTO dtoIngresoPotencia in ListaIngresoPotenciaEmpresa)
                    {
                        //- Linea agregada egjunin

                        if (dtoIngresoPotencia.Emprcodi == 10582)
                        {
                            Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                            dtoIngresoPotencia.Emprnomb = empresa.Emprnomb;
                        }

                        //- Linea agregada egjunin

                        ws.Cells[index, 2].Value = dtoIngresoPotencia.Emprnomb;
                        ws.Cells[index, 3].Value = dtoIngresoPotencia.Equinomb;
                        ws.Cells[index, 4].Value = dtoIngresoPotencia.Inpuprunidadnomb;
                        int iAux = 5;
                        foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                        {
                            List<VtpIngresoPotUnidIntervlDTO> ListaIngresoPotenciaUnidad = (new TransfPotenciaAppServicio()).GetByCriteriaVtpIngresoPotUnidIntervl(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, dtoIngresoPotencia.Emprcodi, dtoIngresoPotencia.Equicodi, dtoIngresoPotEFR.Ipefrcodi).Where(x => x.Grupocodi == dtoIngresoPotencia.Grupocodi && x.Inpuinficticio == dtoIngresoPotencia.Inpuprficticio).ToList();
                            foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoIntervalo in ListaIngresoPotenciaUnidad)
                            {
                                ws.Cells[index, iAux].Value = dtoIngresoIntervalo.Inpuinimporte;
                                break;
                            }
                            iAux++;
                        }
                        ws.Cells[index, iColumna].Value = dtoIngresoPotencia.Inpuprimportepromd;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5, index, iColumna];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }
                    //Fijar panel
                    //ws.View.FreezePanes(6, 0);
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    int iAux2 = 5;
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                    {
                        ws.Column(iAux2).Width = 20;
                        iAux2++;
                    }
                    ws.Column(iColumna).Width = 20;
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //SEGUNDO REPORTE
                    index++;
                    List<VtpIngresoPotenciaDTO> ListaVtpIngresoPotencia = (new TransfPotenciaAppServicio()).ListVtpIngresoPotenciaEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                    ws.Cells[index, 2].Value = "EMPRESA";
                    rg = ws.Cells[index, 2, index + 1, 2];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    int iAux3 = 3;
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                    {
                        ws.Cells[index, iAux3].Value = "Periodo " + dtoIngresoPotEFR.Ipefrintervalo;
                        ws.Cells[index + 1, iAux3].Value = dtoIngresoPotEFR.Ipefrdescripcion;
                        iAux3++;
                    }
                    ws.Cells[index, iAux3].Value = "PROMEDIO";
                    rg = ws.Cells[index, iAux3, index + 1, iAux3];
                    rg.Merge = true;
                    rg.Style.WrapText = true;
                    ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 2, index + 1, iAux3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    index += 2;
                    foreach (VtpIngresoPotenciaDTO dtoIngresoPotencia in ListaVtpIngresoPotencia)
                    {
                        if (dtoIngresoPotencia.Emprcodi != 10582) //- Linea agregada egjunin
                        {//- Linea agregada egjunin

                            //- Linea agrega egjunin

                            if (dtoIngresoPotencia.Emprcodi == 11153)
                            {
                                VtpIngresoPotenciaDTO anterior = ListaVtpIngresoPotencia.Where(x => x.Emprcodi == 10582).FirstOrDefault();

                                if (anterior != null)
                                {
                                    dtoIngresoPotencia.Potipimporte += anterior.Potipimporte;
                                }
                            }

                            //- Fin linea agregada egjunin

                            ws.Cells[index, 2].Value = dtoIngresoPotencia.Emprnomb;
                            int iAux = 3;
                            foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                            {
                                List<VtpIngresoPotUnidIntervlDTO> ListaIngresoPotenciaUnidad = (new TransfPotenciaAppServicio()).ListVtpIngresoPotUnidIntervlSumIntervlEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, (int)dtoIngresoPotencia.Emprcodi, dtoIngresoPotEFR.Ipefrcodi);

                                //- Linea agregada egjunin

                                if (dtoIngresoPotencia.Emprcodi == 11153)
                                {
                                    List<VtpIngresoPotUnidIntervlDTO> ListaTemportal = (new TransfPotenciaAppServicio()).ListVtpIngresoPotUnidIntervlSumIntervlEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, 10582, dtoIngresoPotEFR.Ipefrcodi);

                                    if (ListaIngresoPotenciaUnidad.Count() == ListaTemportal.Count())
                                    {
                                        for (int i = 0; i < ListaIngresoPotenciaUnidad.Count(); i++)
                                        {
                                            ListaIngresoPotenciaUnidad[i].Inpuinimporte += ListaTemportal[i].Inpuinimporte;
                                        }
                                    }
                                }

                                //- Linea agregada egjunin

                                foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoIntervalo in ListaIngresoPotenciaUnidad)
                                {
                                    ws.Cells[index, iAux].Value = dtoIngresoIntervalo.Inpuinimporte;
                                    break;
                                }
                                iAux++;
                            }

                            ws.Cells[index, iAux3].Value = dtoIngresoPotencia.Potipimporte;
                            //Border por celda
                            rg = ws.Cells[index, 2, index, iAux3];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, iAux3];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rg.Style.Numberformat.Format = "#,##0.00";
                            index++;

                        } //- Linea agregada egjunin            
                    }

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                hoja = ws;
                xlPackage.Save();
            }

        }

        /// <summary>
        /// Permite generar el Reporte de Saldos VTP - CU24
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaEmpresaPago">Lista de registros de VtpEmpresaPagoDTO</param>
        /// <returns></returns>
        public static void GenerarReporteValorTransfPotencia(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpSaldoEmpresaDTO> ListaSaldoEmpresa,
            out ExcelWorksheet hoja)
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
                    ws.Cells[index, 3].Value = "SALDOS VTP";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "INGRESO POR POTENCIA (S/)";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 4].Value = "EGRESO POR POTENCIA (S/)";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    //Lista de saldos anteriores
                    int iColumna = 0;
                    decimal[] dSaldoTotal = new decimal[1000];
                    List<VtpSaldoEmpresaDTO> ListaPeriodoDestino = (new TransfPotenciaAppServicio()).ListPeriodosDestino(EntidadRecalculoPotencia.Pericodi);
                    if (EntidadRecalculoPotencia.Recpotcodi == 1)
                    {
                        foreach (VtpSaldoEmpresaDTO dtoPeriodo in ListaPeriodoDestino)
                        {
                            ws.Cells[index, 5 + iColumna].Value = dtoPeriodo.Perinombre + " (S/)";
                            ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                            dSaldoTotal[iColumna] = 0;
                            iColumna++;
                        }
                        ws.Cells[index, 5 + iColumna].Value = "AJUSTE DEL MES(S/)";
                        ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                        iColumna++;
                    }
                    ws.Cells[index, 5 + iColumna].Value = "SALDO NETO MENSUAL(S/)";
                    ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                    if (EntidadRecalculoPotencia.Recpotcodi > 1)
                    {
                        iColumna++;
                        ws.Cells[index, 5 + iColumna].Value = "SALDO NETO REV. ANTERIOR (S/)";
                        ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                        iColumna++;
                        ws.Cells[index, 5 + iColumna].Value = "AJUSTE DEL MES (S/)";
                        ws.Column(5 + iColumna).Style.Numberformat.Format = "#,##0.000";
                    }

                    rg = ws.Cells[index, 2, index, 5 + iColumna];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    decimal dIngreso = 0;
                    decimal dEgreso = 0;
                    decimal dAjuste = 0;
                    decimal dSaldo = 0;
                    decimal dVerAnterior = 0;
                    decimal dAjusteMes = 0;
                    index++;
                    foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresa in ListaSaldoEmpresa)
                    {
                        if (dtoSaldoEmpresa.Emprcodi != 10582) //- Linea agregada egjunin
                        {
                            //- Lineas agregadas egjunin
                            if (dtoSaldoEmpresa.Emprcodi == 11153)
                            {
                                VtpSaldoEmpresaDTO anterior = ListaSaldoEmpresa.Where(x => x.Emprcodi == 10582).FirstOrDefault();

                                if (anterior != null)
                                {
                                    dtoSaldoEmpresa.Potseingreso += anterior.Potseingreso;
                                    dtoSaldoEmpresa.Potseegreso += anterior.Potseegreso;
                                    dtoSaldoEmpresa.Potsesaldo += anterior.Potsesaldo;
                                    dtoSaldoEmpresa.Potseajuste += anterior.Potseajuste;
                                }
                            }
                            //- Fin lineas agregadas egjunin


                            ws.Cells[index, 2].Value = dtoSaldoEmpresa.Emprnomb;
                            ws.Cells[index, 3].Value = dtoSaldoEmpresa.Potseingreso;
                            dIngreso += dtoSaldoEmpresa.Potseingreso;
                            ws.Cells[index, 4].Value = dtoSaldoEmpresa.Potseegreso;
                            dEgreso += dtoSaldoEmpresa.Potseegreso;
                            //Lista dinamica de saldos de periodos anteriores
                            iColumna = 0;
                            decimal dSaldoMes = 0;
                            if (EntidadRecalculoPotencia.Recpotcodi == 1)
                            {
                                foreach (VtpSaldoEmpresaDTO dtoPeriodo in ListaPeriodoDestino)
                                {
                                    //- Linea agregada egjunin
                                    decimal saldoAdicional = 0;
                                    if (dtoSaldoEmpresa.Emprcodi == 11153)
                                    {
                                        VtpSaldoEmpresaDTO dtoSaldoAdicional = (new TransfPotenciaAppServicio()).GetSaldoEmpresaPeriodo(10582, dtoPeriodo.Pericodi, dtoSaldoEmpresa.Pericodi);

                                        if (dtoSaldoAdicional != null)
                                        {
                                            saldoAdicional = dtoSaldoAdicional.Potsesaldoreca;
                                        }
                                    }

                                    //- Fin linea agregada egjunin

                                    VtpSaldoEmpresaDTO dtoSaldoAnterior = (new TransfPotenciaAppServicio()).GetSaldoEmpresaPeriodo(dtoSaldoEmpresa.Emprcodi, dtoPeriodo.Pericodi, dtoSaldoEmpresa.Pericodi);
                                    if (dtoSaldoAnterior != null)
                                    {
                                        ws.Cells[index, 5 + iColumna].Value = dtoSaldoAnterior.Potsesaldoreca + saldoAdicional; //- Linea agregada egjunin
                                        dSaldoTotal[iColumna] += dtoSaldoAnterior.Potsesaldoreca + saldoAdicional; //- Linea agregada egjunin
                                        dSaldoMes += dtoSaldoAnterior.Potsesaldoreca + saldoAdicional;  //- Linea agregada egjunin
                                    }
                                    else
                                    {
                                        ws.Cells[index, 5 + iColumna].Value = 0 + saldoAdicional;   //- Linea agregada egjunin
                                    }

                                    iColumna++;
                                }

                                ws.Cells[index, 5 + iColumna].Value = dtoSaldoEmpresa.Potseajuste;
                                dAjuste += dtoSaldoEmpresa.Potseajuste;
                                dSaldoMes += dtoSaldoEmpresa.Potseajuste;
                                iColumna++;
                            }
                            ws.Cells[index, 5 + iColumna].Value = dtoSaldoEmpresa.Potsesaldo + dSaldoMes;
                            dSaldo += dtoSaldoEmpresa.Potsesaldo + dSaldoMes;
                            if (EntidadRecalculoPotencia.Recpotcodi > 1)
                            {
                                decimal dSaldoVerAnterior = 0;
                                int iRecpotcodiAnterior = (int)EntidadRecalculoPotencia.Recpotcodi - 1;
                                VtpSaldoEmpresaDTO dtoSaldoEmpresaAnterior = (new TransfPotenciaAppServicio()).GetByIdVtpSaldoEmpresaSaldo(EntidadRecalculoPotencia.Pericodi, iRecpotcodiAnterior, dtoSaldoEmpresa.Emprcodi);

                                //- Linea agregada egjunin
                                decimal seingreso = 0;
                                decimal seegreso = 0;
                                if (dtoSaldoEmpresa.Emprcodi == 11153)
                                {
                                    VtpSaldoEmpresaDTO dtoSaldoAdicional = (new TransfPotenciaAppServicio()).GetByIdVtpSaldoEmpresaSaldo(EntidadRecalculoPotencia.Pericodi, iRecpotcodiAnterior, 10582);

                                    if (dtoSaldoAdicional != null)
                                    {
                                        seingreso = dtoSaldoAdicional.Potseingreso;
                                        seegreso = dtoSaldoAdicional.Potseegreso;
                                    }
                                }

                                //- fin linea agregada egjunin

                                if (dtoSaldoEmpresaAnterior != null)
                                {
                                    dSaldoVerAnterior = dtoSaldoEmpresaAnterior.Potseingreso + seingreso - dtoSaldoEmpresaAnterior.Potseegreso - seegreso;
                                }

                                iColumna++;
                                ws.Cells[index, 5 + iColumna].Value = dSaldoVerAnterior;
                                dVerAnterior += dSaldoVerAnterior;
                                decimal dAjusteRevicion = (dtoSaldoEmpresa.Potseingreso - dtoSaldoEmpresa.Potseegreso) - dSaldoVerAnterior;
                                iColumna++;
                                ws.Cells[index, 5 + iColumna].Value = dAjusteRevicion;
                                dAjusteMes += dAjusteRevicion;
                            }
                            //Border por celda
                            rg = ws.Cells[index, 2, index, 5 + iColumna];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg = ws.Cells[index, 3, index, 5 + iColumna];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            index++;
                        }
                    }
                    if (dEgreso > 0)
                    {
                        ws.Cells[index, 2].Value = "TOTAL";
                        rg = ws.Cells[index, 2, index, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        ws.Cells[index, 3].Value = dIngreso;
                        ws.Cells[index, 4].Value = dEgreso;
                        if (EntidadRecalculoPotencia.Recpotcodi == 1)
                        {
                            for (int i = 0; i < iColumna; i++)
                            {
                                ws.Cells[index, 5 + i].Value = dSaldoTotal[i];
                            }
                            ws.Cells[index, 4 + iColumna].Value = dAjuste;
                            ws.Cells[index, 5 + iColumna].Value = dSaldo;
                        }
                        else
                        {
                            ws.Cells[index, 5].Value = dSaldo;
                            ws.Cells[index, 6].Value = dVerAnterior;
                            ws.Cells[index, 7].Value = dAjusteMes;
                        }
                        rg = ws.Cells[index, 3, index, 5 + iColumna];
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(6, 0);
                    ws.Column(2).Width = 45;
                    ws.Column(3).Width = 25;
                    ws.Column(4).Width = 25;
                    if (EntidadRecalculoPotencia.Recpotcodi == 1)
                    {
                        for (int i = 0; i < iColumna; i++)
                        {
                            ws.Column(5 + i).Width = 25;
                        }
                        ws.Column(4 + iColumna).Width = 25;
                        ws.Column(5 + iColumna).Width = 25;
                    }
                    else
                    {
                        ws.Column(5).Width = 25;
                        ws.Column(6).Width = 25;
                        ws.Column(7).Width = 25;
                    }

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                hoja = ws;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el Reporte de Matriz de pagos VTP - CU25
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaEmpresaPago">Lista de registros de VtpEmpresaPagoDTO</param>
        /// <returns></returns>
        public static void GenerarReporteMatriz(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpEmpresaPagoDTO> ListaEmpresaPago, out ExcelWorksheet hoja)
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
                {   //TITULO
                    int row = 2; //Fila donde inicia la data
                                 //ws.Cells[row++, 4].Value = dtoRecalculo.RecaCuadro3;
                                 //ws.Cells[row++, 4].Value = dtoRecalculo.RecaNroInforme;
                    ws.Cells[row++, 3].Value = "VALORIZACIÓN DE LAS TRANSFERENCIA DE POTENCIA";
                    ws.Cells[row++, 3].Value = EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[row++, 3].Value = "MATRIZ DE PAGOS VTP";
                    ws.Cells[row++, 3].Value = ConstantesTransfPotencia.MensajeSoles;
                    ExcelRange rg = ws.Cells[2, 3, row++, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    ws.Cells[row, 3].Value = "RUC";
                    ws.Cells[row, 2].Value = "EMPRESA";
                    rg = ws.Cells[row, 2, row + 1, 3];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                    int colum = 4;
                    int iNumEmpresasCobro = 0;
                    for (int i = 0; i < ListaEmpresaPago.Count(); i++)
                    {
                        if (i == 0)
                        {
                            int iEmprcodiPago = ListaEmpresaPago[0].Emprcodipago;
                            List<VtpEmpresaPagoDTO> ListaEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                            iNumEmpresasCobro = ListaEmpresaCobro.Where(x => x.Emprcodicobro != 10582).Count();
                            foreach (VtpEmpresaPagoDTO dtoEmpresaCobro in ListaEmpresaCobro)
                            {
                                if (dtoEmpresaCobro.Emprcodicobro != 10582) //- Linea modificada egjunin
                                {
                                    ws.Cells[row, colum].Value = (dtoEmpresaCobro.Emprnombcobro != null) ? dtoEmpresaCobro.Emprnombcobro.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (dtoEmpresaCobro.EmprRuc != null) ? dtoEmpresaCobro.EmprRuc.ToString().Trim() : string.Empty;
                                    ws.Cells[row + 1, colum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    ws.Cells[row + 1, colum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                    ws.Cells[row + 1, colum].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                    ws.Cells[row + 1, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[row + 1, colum].Style.Font.Bold = true;
                                    ws.Cells[row + 1, colum].Style.Font.Size = 10;
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }
                            }
                            ws.Cells[row, colum].Value = "TOTAL";
                            ws.Cells[row + 1, colum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row + 1, colum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row + 1, colum].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row + 1, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[row + 1, colum].Style.Font.Bold = true;
                            ws.Cells[row + 1, colum].Style.Font.Size = 10;
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                            dTotalColum[colum] = 0;
                        }
                        break;
                    }
                    row++;
                    row++; //int row = 8;
                    foreach (VtpEmpresaPagoDTO dtoEmpresaPago in ListaEmpresaPago)
                    {
                        if (dtoEmpresaPago.Emprcodipago != 10582)
                        {
                            ws.Cells[row, 3].Value = (dtoEmpresaPago.EmprRuc != null) ? dtoEmpresaPago.EmprRuc.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Value = (dtoEmpresaPago.Emprnombpago != null) ? dtoEmpresaPago.Emprnombpago.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                            List<VtpEmpresaPagoDTO> ListaEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(dtoEmpresaPago.Emprcodipago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                            //- Linea agregada egjunin

                            if (dtoEmpresaPago.Emprcodipago == 11153)
                            {
                                List<VtpEmpresaPagoDTO> ListaAdicional = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(10582, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                                if (ListaEmpresaCobro.Count() == ListaAdicional.Count())
                                {
                                    for (int i = 0; i < ListaEmpresaCobro.Count(); i++)
                                    {
                                        ListaEmpresaCobro[i].Potepmonto += ListaAdicional[i].Potepmonto;
                                    }
                                }
                            }

                            //- Fin linea agregada egjunin

                            colum = 4;
                            decimal dTotalRow = 0;
                            foreach (VtpEmpresaPagoDTO dtoEmpresaCobro in ListaEmpresaCobro)
                            {
                                if (dtoEmpresaCobro.Emprcodicobro != 10582)
                                {
                                    if (dtoEmpresaCobro.Emprcodicobro == 11153)
                                    {
                                        VtpEmpresaPagoDTO entidadAdicional = ListaEmpresaCobro.Where(x => x.Emprcodicobro == 10582).FirstOrDefault();

                                        if (entidadAdicional != null)
                                        {
                                            dtoEmpresaCobro.Potepmonto += entidadAdicional.Potepmonto;
                                        }
                                    }

                                    ws.Cells[row, colum].Value = dtoEmpresaCobro.Potepmonto;
                                    dTotalRow += Convert.ToDecimal(dtoEmpresaCobro.Potepmonto);
                                    dTotalColum[colum] += Convert.ToDecimal(dtoEmpresaCobro.Potepmonto);
                                    colum++;
                                }
                            }

                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por Fila
                            dTotalColum[colum] += dTotalRow;
                            //Border por celda en la Fila
                            rg = ws.Cells[row, 2, row, colum];
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
                    ws.Cells[row, 2].Value = "TOTAL";
                    ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                    ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                    ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                    ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                    colum = 4;
                    for (int i = 0; i <= iNumEmpresasCobro; i++)
                    {
                        ws.Cells[row, colum].Value = dTotalColum[colum];
                        colum++;
                    }
                    rg = ws.Cells[row, 2, row, colum - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    //Fijar panel
                    ws.View.FreezePanes(9, 4);//fijo hasta la Fila 7 y columna 2
                    rg = ws.Cells[7, 2, row, colum];
                    rg.AutoFitColumns();

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                hoja = ws;
                xlPackage.Save();
            }
        }

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


            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E9F1F8"));
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

        /// <summary>
        /// CU09 Información ingresada para VTP y peajes - Permite generar el archivo de exportación de la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="entidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="listaPeajeEgresoMinfo">Lista de registros de VtpIngresoPotefrDetalleDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoPeajeEgresoMinfoNuevo(string fileName, VtpRecalculoPotenciaDTO entidadRecalculoPotencia, List<VtpPeajeEgresoMinfoDTO> listaPeajeEgresoMinfo, out ExcelWorksheet hoja)
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
                    ws.Cells[index, 3].Value = "INFORMACIÓN INGRESADA PARA VTP Y PEAJES";
                    ws.Cells[index + 1, 3].Value = entidadRecalculoPotencia.Perinombre + "/" + entidadRecalculoPotencia.Recpotnombre;
                    ws.Cells[index + 2, 3].Value = "";
                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 4;

                    //ws.Cells[index, 7].Value = "PARA EGRESO DE POTENCIA";
                    //ws.Cells[index, 9].Value = "PARA PEAJE POR CONEXIÓN";
                    //ws.Cells[index, 12].Value = "PARA FLUJO DE CARGA OPTIMO";

                    //rg = ws.Cells[index, 7, index, 8];
                    //rg.Merge = true;
                    //rg.Style.WrapText = true;
                    //rg = ObtenerEstiloCelda(rg, 1);

                    //rg = ws.Cells[index, 9, index, 11];
                    //rg.Merge = true;
                    //rg.Style.WrapText = true;
                    //rg = ObtenerEstiloCelda(rg, 1);

                    //rg = ws.Cells[index, 12, index, 14];
                    //rg.Merge = true;
                    //rg.Style.WrapText = true;
                    //rg = ObtenerEstiloCelda(rg, 1);
                    //index++;


                    ws.Cells[index, 2].Value = "CODIGO VTP";
                    ws.Cells[index, 3].Value = "EMPRESA";
                    ws.Cells[index, 4].Value = "CLIENTE";
                    ws.Cells[index, 5].Value = "BARRA";
                    ws.Cells[index, 6].Value = "CONTRATO";
                    ws.Cells[index, 7].Value = "TIPO USUARIO";
                    ws.Cells[index, 8].Value = "PRECIO POTENCIA S/ /kW-mes";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 9].Value = "POTENCIA COINCIDENTE kW";
                    ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 10].Value = "POTENCIA DECLARADA (KW)";
                    ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 11].Value = "PEAJE UNITARIO S/ /kW-mes";
                    ws.Column(11).Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[index, 12].Value = "FACTOR PÉRDIDA";
                    ws.Column(12).Style.Numberformat.Format = "#,##0.00";

                    ws.Cells[index, 13].Value = "CALIDAD";

                    rg = ws.Cells[index, 2, index, 13];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in listaPeajeEgresoMinfo)
                    {
                        ws.Cells[index, 2].Value = (item.Coregecodvtp != null) ? item.Coregecodvtp.ToString() : string.Empty;
                        ws.Cells[index, 3].Value = (item.Genemprnombre != null) ? item.Genemprnombre.ToString() : string.Empty;
                        ws.Cells[index, 4].Value = (item.Cliemprnombre != null) ? item.Cliemprnombre.ToString() : string.Empty;
                        ws.Cells[index, 5].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                        ws.Cells[index, 6].Value = (item.Coregecodvtp != null) ? (item.Tipconnombre != null) ? item.Tipconnombre.ToString() : string.Empty : (item.Pegrmilicitacion != null) ? item.Pegrmilicitacion.ToString() : string.Empty;
                        ws.Cells[index, 7].Value = (item.Pegrmitipousuario != null) ? item.Pegrmitipousuario.ToString() : string.Empty;
                        ws.Cells[index, 8].Value = item.Pegrmipreciopote;// (item.Pegrmipreciopote != null) ? item.Pegrmipreciopote : Decimal.Zero;
                        ws.Cells[index, 9].Value = (item.Pegrdpotecoincidente ?? item.Pegrmipoteegreso); //(item.Pegrdpotecoincidente ?? 0) == 0 ? (item.Pegrmipoteegreso ?? Decimal.Zero) : item.Pegrdpotecoincidente ?? Decimal.Zero;
                        ws.Cells[index, 10].Value = item.Pegrmipotedeclarada;// (item.Pegrmipotedeclarada != null) ? item.Pegrmipotedeclarada : Decimal.Zero;
                        ws.Cells[index, 11].Value = item.Pegrmipeajeunitario;// (item.Pegrmipeajeunitario != null) ? item.Pegrmipeajeunitario : Decimal.Zero;
                        ws.Cells[index, 12].Value = (item.Pegrdfacperdida ?? item.Pegrmifacperdida);// (item.Pegrdfacperdida != null) ? item.Pegrdfacperdida : Decimal.Zero;
                        ws.Cells[index, 13].Value = (item.Pegrmicalidad != null) ? item.Pegrmicalidad.ToString() : string.Empty;
                        //Border por celda
                        rg = ws.Cells[index, 2, index, 13];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    //Fijar panel
                    //ws.View.FreezePanes(7, 0);
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 30;
                    ws.Column(4).Width = 30;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 10;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 15;
                    ws.Column(12).Width = 20;
                    ws.Column(13).Width = 15;

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                hoja = ws;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Convierte un archivo excel a una datable
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
    }
}
