using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CalculoPorcentajes.Helper
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
        /// Permite generar el archivo de exportación de la tabla CaiAjusteEmpresa
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadPresupuesto">Entidad de la tabla Presupuesto</param>
        /// <param name="EntidadAjusteEmpresa">Entidad de la tabla CaiAjusteDTO</param>
        /// <param name="ListaAjusteEmpresa">Lista de elementos de la tabla CaiAjusteDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoCaiAjusteEmpresas(string fileName, CaiPresupuestoDTO EntidadPresupuesto, CaiAjusteDTO EntidadAjuste, List<CaiAjusteempresaDTO> ListaAjusteEmpresa, string caiajetipoinfo)
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
                    ws.Cells[index, 4].Value = "Lista Empresa";
                    ws.Cells[index + 1, 4].Value = EntidadPresupuesto.Caiprsnombre + " / " + EntidadAjuste.Caiajnombre + "";
                    ExcelRange rg = ws.Cells[index, 4, index + 1, 11];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "id";
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    ws.Cells[index, 3].Value = "Empresa";
                    ws.Cells[index, 3, index + 1, 3].Merge = true;
                    ws.Cells[index, 4].Value = "PuntoMedición";
                    ws.Cells[index, 4, index + 1, 4].Merge = true;
                    ws.Cells[index, 5].Value = "Tipo empresa";
                    ws.Cells[index, 5, index + 1, 5].Merge = true;

                    //Dependiendo del tipo cambia la cabezera de la tabla
                    string TextoTipo = "";
                    if (caiajetipoinfo.Equals("E"))
                    {
                        TextoTipo = "Retiro/Produccion Energía (Mw.H)";
                    }
                    else
                    {
                        TextoTipo = "Ingreso por transmisión (S/)";
                    }

                    ws.Cells[index, 6].Value = TextoTipo;
                    ws.Cells[index, 6, index, 6 + 5].Merge = true;

                    ws.Cells[index + 1, 6].Value = "Ejecuta Inicio";
                    ws.Cells[index + 1, 7].Value = "Ejecutada Fin";
                    ws.Cells[index + 1, 8].Value = "Proyectado 'A' Inicio";
                    ws.Cells[index + 1, 9].Value = "Proyectado 'A' Fin";
                    ws.Cells[index + 1, 10].Value = "Proyectado 'A+1' Inicio";
                    ws.Cells[index + 1, 11].Value = "Proyectado 'A+1' Fin";
                    rg = ws.Cells[index, 2, index, 11];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    rg = ws.Cells[index + 1, 4, index + 1, 11];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (CaiAjusteempresaDTO item in ListaAjusteEmpresa)
                    {
                        ws.Cells[index + 1, 2].Value = item.Caiajecodi.ToString();
                        ws.Cells[index + 1, 3].Value = item.Emprnomb.ToString();
                        ws.Cells[index + 1, 4].Value = item.Ptomedielenomb.ToString();
                        ws.Cells[index + 1, 5].Value = item.Tipoemprdesc.ToString();
                        if(item.Caiajereteneejeini != null)
                            ws.Cells[index + 1, 6].Value = ((DateTime)item.Caiajereteneejeini).ToString("dd/MM/yyyy");
                        if (item.Caiajereteneejefin != null)
                            ws.Cells[index + 1, 7].Value = ((DateTime)item.Caiajereteneejefin).ToString("dd/MM/yyyy");
                        if (item.Caiajeretenepryaini != null)
                            ws.Cells[index + 1, 8].Value = ((DateTime)item.Caiajeretenepryaini).ToString("dd/MM/yyyy");
                        if (item.Caiajeretenepryafin != null)
                            ws.Cells[index + 1, 9].Value = ((DateTime)item.Caiajeretenepryafin).ToString("dd/MM/yyyy");
                        if (item.Caiajereteneprybini != null)
                            ws.Cells[index + 1, 10].Value = ((DateTime)item.Caiajereteneprybini).ToString("dd/MM/yyyy");
                        if (item.Caiajereteneprybfin != null)
                            ws.Cells[index + 1, 11].Value = ((DateTime)item.Caiajereteneprybfin).ToString("dd/MM/yyyy");

                        //Border por celda
                        rg = ws.Cells[index + 1, 3, index + 1, 11];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index + 1, 6, index + 1, 11];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    ws.Column(2).Width = 5;
                    ws.Column(2).Hidden = true;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 20;
                    ws.Column(9).Width = 20;
                    ws.Column(10).Width = 20;
                    ws.Column(11).Width = 20;
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 60);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        public static void GenerarReporteEnergia(string fileName, CaiPresupuestoDTO EntidadPresupuesto, CaiAjusteDTO EntidadAjuste, List<CaiAjusteempresaDTO> ListaEmpresas)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                int iMesesYear = 12;
                //Mes Inicio
                string sMes = EntidadPresupuesto.Caiprsmesinicio.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadAjuste.Caiajanio;
                var sFechaFin = "01/" + iMesesYear + "/" + EntidadAjuste.Caiajanio;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - REPORTE DE ENERGIA (MWh)
                //Nombre de la hoja de caluculo
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Energía " + Convert.ToDateTime(sFechaInicio).ToString("yyyy"));
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "ENERGIA";
                    ws.Cells[index + 1, 3].Value = EntidadPresupuesto.Caiprsnombre + "/" + EntidadAjuste.Caiajnombre;
                    //selecciona las celdas a aplicar estilos
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    //estilos al titulo
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;//5

                    //Cabecera:
                    ws.Cells[index + 1, 2].Value = ""; //Lista de Empresas
                    //aqui une dos celdas
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    rg = ws.Cells[index, 2, index + 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws.Column(2).Width = 35;
                    //Filas:
                    int iFila = index + 2;

                    for (int i = 0; i < ListaEmpresas.Count; i++)
                    {
                        ws.Cells[iFila, 2].Value = ListaEmpresas[i].Emprnomb;
                        ws.Cells[iFila, 3].Value = 0;
                        ws.Cells[iFila, 4].Value = 0;
                        ws.Cells[iFila, 5].Value = 0;
                        ws.Cells[iFila, 6].Value = 0;
                        ws.Cells[iFila, 7].Value = 0;
                        ws.Cells[iFila, 8].Value = 0;
                        ws.Cells[iFila, 9].Value = 0;
                        ws.Cells[iFila, 10].Value = 0;
                        ws.Cells[iFila, 11].Value = 0;
                        ws.Cells[iFila, 12].Value = 0;
                        ws.Cells[iFila, 13].Value = 0;
                        ws.Cells[iFila, 14].Value = 0;
                        ws.Cells[iFila, 15].Value = 0;

                        ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(11).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(12).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(13).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(14).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(15).Style.Numberformat.Format = "#,##0.00";

                        iFila++;
                    }

                    //selecciono las celdas donde estan las empresas
                    rg = ws.Cells[index + 2, 2, iFila - 1, 2];//a iFila le resto 1 para que me borre la fila de más
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //columnas
                    int iColumna = 3;//columna inicio para las cabeceras
                    DateTime dFecha = dFecInicio;

                    //primera cabecera(ocupa todo)
                    ws.Cells[index, iColumna].Value = "Energia  (MVh)";
                    ws.Cells[index, iColumna, index, iColumna + iMesesYear - 1].Merge = true;
                    //estilo
                    rg = ws.Cells[index, iColumna, index, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    while (dFecha <= dFecFin)
                    {
                        ws.Cells[index + 1, iColumna++].Value = dFecha.ToString("MMM-yyyy");
                        dFecha = dFecha.AddMonths(1);
                    }
                    ws.Cells[index + 1, iMesesYear + 3].Value = "Total";
                    //falta pintar las celdas - segunda cabecera
                    rg = ws.Cells[index + 1, 3, index + 1, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    //rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);


                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                #endregion

                #region SEGUNDA PESTAÑA - MONTOS DE ENERGIA (S/.)

                ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("Monto de Energía (S/.)");
                if (ws2 != null)
                {
                    int index = 2;
                    //TITULO
                    ws2.Cells[index, 3].Value = "Monto de Energía (S/.)";
                    ws2.Cells[index + 1, 3].Value = EntidadPresupuesto.Caiprsnombre + "/" + EntidadAjuste.Caiajnombre;
                    //selecciona las celdas a aplicar estilos
                    ExcelRange rg = ws2.Cells[index, 3, index + 1, 3];
                    //estilos al titulo
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;//5

                    //Cabecera:
                    ws2.Cells[index + 1, 2].Value = ""; //Lista de Empresas
                    //aqui une dos celdas
                    ws2.Cells[index, 2, index + 1, 2].Merge = true;
                    rg = ws2.Cells[index, 2, index + 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws2.Column(2).Width = 35;
                    //Filas:
                    int iFila = index + 2;

                    for (int i = 0; i < ListaEmpresas.Count; i++)
                    {
                        ws2.Cells[iFila, 2].Value = ListaEmpresas[i].Emprnomb;
                        iFila++;
                    }

                    //selecciono las celdas donde estan las empresas
                    rg = ws2.Cells[index + 2, 2, iFila - 1, 2];//a iFila le resto 1 para que me borre la fila de más
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //columnas
                    int iColumna = 3;//columna inicio para las cabeceras
                    DateTime dFecha = dFecInicio;

                    //primera cabecera(ocupa todo)
                    ws2.Cells[index, iColumna].Value = "Monto de Energía (S/.)";
                    ws2.Cells[index, iColumna, index, iColumna + iMesesYear - 1].Merge = true;
                    //estilo
                    rg = ws2.Cells[index, iColumna, index, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    while (dFecha <= dFecFin)
                    {
                        //cabeceras
                        ws2.Cells[index + 1, iColumna++].Value = dFecha.ToString("MMM-yyyy");
                        //datos
                        for (int i = 1; i <= ListaEmpresas.Count; i++)
                        {
                            ws2.Cells[index + 1 + i, iColumna - 1].Value = 0;
                            ws2.Column(iColumna - 1).Style.Numberformat.Format = "#,##0.00";
                            //total
                            ws2.Cells[index + i + 1, 15].Value = 0;

                        }

                        dFecha = dFecha.AddMonths(1);

                    }
                    //columna total numerico
                    ws2.Column(15).Style.Numberformat.Format = "#,##0.00";

                    ws2.Cells[index + 1, iMesesYear + 3].Value = "Total";
                    //falta pintar las celdas - segunda cabecera
                    rg = ws2.Cells[index + 1, 3, index + 1, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    //rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                }

                #endregion

                xlPackage.Save();
            }
        }

        public static void GenerarReportePotencia(string fileName, CaiPresupuestoDTO EntidadPresupuesto, CaiAjusteDTO EntidadAjuste, List<CaiAjusteempresaDTO> ListaEmpresas)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                int iMesesYear = 12;
                //Mes Inicio
                string sMes = EntidadPresupuesto.Caiprsmesinicio.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadAjuste.Caiajanio;
                var sFechaFin = "01/" + iMesesYear + "/" + EntidadAjuste.Caiajanio;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - REPORTE DE POTENCIA (MW)
                //Nombre de la hoja de caluculo
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Potencia " + Convert.ToDateTime(sFechaInicio).ToString("yyyy"));
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "POTENCIA";
                    ws.Cells[index + 1, 3].Value = EntidadPresupuesto.Caiprsnombre + "/" + EntidadAjuste.Caiajnombre;
                    //selecciona las celdas a aplicar estilos
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    //estilos al titulo
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;//5

                    //Cabecera:
                    ws.Cells[index + 1, 2].Value = ""; //Lista de Empresas
                    //aqui une dos celdas
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    rg = ws.Cells[index, 2, index + 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws.Column(2).Width = 35;
                    //Filas:
                    int iFila = index + 2;

                    for (int i = 0; i < ListaEmpresas.Count; i++)
                    {
                        ws.Cells[iFila, 2].Value = ListaEmpresas[i].Emprnomb;
                        ws.Cells[iFila, 3].Value = 0;
                        ws.Cells[iFila, 4].Value = 0;
                        ws.Cells[iFila, 5].Value = 0;
                        ws.Cells[iFila, 6].Value = 0;
                        ws.Cells[iFila, 7].Value = 0;
                        ws.Cells[iFila, 8].Value = 0;
                        ws.Cells[iFila, 9].Value = 0;
                        ws.Cells[iFila, 10].Value = 0;
                        ws.Cells[iFila, 11].Value = 0;
                        ws.Cells[iFila, 12].Value = 0;
                        ws.Cells[iFila, 13].Value = 0;
                        ws.Cells[iFila, 14].Value = 0;
                        ws.Cells[iFila, 15].Value = 0;

                        ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(11).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(12).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(13).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(14).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(15).Style.Numberformat.Format = "#,##0.00";
                        iFila++;
                    }

                    //selecciono las celdas donde estan las empresas
                    rg = ws.Cells[index + 2, 2, iFila - 1, 2];//a iFila le resto 1 para que me borre la fila de más
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //columnas
                    int iColumna = 3;//columna inicio para las cabeceras
                    DateTime dFecha = dFecInicio;

                    //primera cabecera(ocupa todo)
                    ws.Cells[index, iColumna].Value = "Potencia  (MW)";
                    ws.Cells[index, iColumna, index, iColumna + iMesesYear - 1].Merge = true;
                    //estilo
                    rg = ws.Cells[index, iColumna, index, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    while (dFecha <= dFecFin)
                    {
                        ws.Cells[index + 1, iColumna++].Value = dFecha.ToString("MMM-yyyy");
                        dFecha = dFecha.AddMonths(1);
                    }
                    ws.Cells[index + 1, iMesesYear + 3].Value = "Total";
                    //pinta las celdas - segunda cabecera
                    rg = ws.Cells[index + 1, 3, index + 1, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    //rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                #endregion

                #region SEGUNDA PESTAÑA - MONTOS DE POTENCIA (S/.)

                ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("Monto de Potencia (S/.)");
                if (ws2 != null)
                {
                    int index = 2;
                    //TITULO
                    ws2.Cells[index, 3].Value = "Monto de Potencia (S/.)";
                    ws2.Cells[index + 1, 3].Value = EntidadPresupuesto.Caiprsnombre + "/" + EntidadAjuste.Caiajnombre;
                    //selecciona las celdas a aplicar estilos
                    ExcelRange rg = ws2.Cells[index, 3, index + 1, 3];
                    //estilos al titulo
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;//5

                    //Cabecera:
                    ws2.Cells[index + 1, 2].Value = ""; //Lista de Empresas
                    //aqui une dos celdas
                    ws2.Cells[index, 2, index + 1, 2].Merge = true;
                    rg = ws2.Cells[index, 2, index + 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws2.Column(2).Width = 35;
                    //Filas:
                    int iFila = index + 2;

                    for (int i = 0; i < ListaEmpresas.Count; i++)
                    {
                        ws2.Cells[iFila, 2].Value = ListaEmpresas[i].Emprnomb;
                        iFila++;
                    }

                    //selecciono las celdas donde estan las empresas
                    rg = ws2.Cells[index + 2, 2, iFila - 1, 2];//a iFila le resto 1 para que me borre la fila de más
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //columnas
                    int iColumna = 3;//columna inicio para las cabeceras
                    DateTime dFecha = dFecInicio;

                    //primera cabecera(ocupa todo)
                    ws2.Cells[index, iColumna].Value = "Monto de Potencia (S/.)";
                    ws2.Cells[index, iColumna, index, iColumna + iMesesYear - 1].Merge = true;
                    //estilo
                    rg = ws2.Cells[index, iColumna, index, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    while (dFecha <= dFecFin)
                    {
                        //cabeceras
                        ws2.Cells[index + 1, iColumna++].Value = dFecha.ToString("MMM-yyyy");
                        //datos
                        for (int i = 1; i <= ListaEmpresas.Count; i++)
                        {
                            ws2.Cells[index + 1 + i, iColumna - 1].Value = 0;
                            ws2.Column(iColumna - 1).Style.Numberformat.Format = "#,##0.00";
                            //total
                            ws2.Cells[index + i + 1, 15].Value = 0;
                        }

                        dFecha = dFecha.AddMonths(1);

                    }
                    //columna total numerico
                    ws2.Column(15).Style.Numberformat.Format = "#,##0.00";

                    ws2.Cells[index + 1, iMesesYear + 3].Value = "Total";
                    //falta pintar las celdas - segunda cabecera
                    rg = ws2.Cells[index + 1, 3, index + 1, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    //rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                }

                #endregion

                xlPackage.Save();
            }
        }

        public static void GenerarReporteTransmision(string fileName, CaiPresupuestoDTO EntidadPresupuesto, CaiAjusteDTO EntidadAjuste, List<CaiAjusteempresaDTO> ListaEmpresas)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                int iMesesYear = 12;
                //Mes Inicio
                string sMes = EntidadPresupuesto.Caiprsmesinicio.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadAjuste.Caiajanio;
                var sFechaFin = "01/" + iMesesYear + "/" + EntidadAjuste.Caiajanio;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - MONTOS DE TRANSMISION (MW)
                //Nombre de la hoja de caluculo
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Monto de Transmisión (S/.)" + Convert.ToDateTime(sFechaInicio).ToString("yyyy"));
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "TRANSMISION";
                    ws.Cells[index + 1, 3].Value = EntidadPresupuesto.Caiprsnombre + "/" + EntidadAjuste.Caiajnombre;
                    //selecciona las celdas a aplicar estilos
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    //estilos al titulo
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;//5

                    //Cabecera:
                    ws.Cells[index + 1, 2].Value = ""; //Lista de Empresas
                    //aqui une dos celdas
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    rg = ws.Cells[index, 2, index + 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws.Column(2).Width = 35;
                    //Filas:
                    int iFila = index + 2;

                    for (int i = 0; i < ListaEmpresas.Count; i++)
                    {
                        ws.Cells[iFila, 2].Value = ListaEmpresas[i].Emprnomb;
                        iFila++;
                    }

                    //selecciono las celdas donde estan las empresas
                    rg = ws.Cells[index + 2, 2, iFila - 1, 2];//a iFila le resto 1 para que me borre la fila de más
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //columnas
                    int iColumna = 3;//columna inicio para las cabeceras
                    DateTime dFecha = dFecInicio;

                    //primera cabecera(ocupa todo)
                    ws.Cells[index, iColumna].Value = "Monto de Transmisión (S/.)";
                    ws.Cells[index, iColumna, index, iColumna + iMesesYear - 1].Merge = true;
                    //estilo
                    rg = ws.Cells[index, iColumna, index, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    while (dFecha <= dFecFin)
                    {
                        ws.Cells[index + 1, iColumna++].Value = dFecha.ToString("MMM-yyyy");
                        for (int i = 1; i <= ListaEmpresas.Count; i++)
                        {
                            ws.Cells[index + 1 + i, iColumna - 1].Value = 0;
                            ws.Column(iColumna - 1).Style.Numberformat.Format = "#,##0.00";
                            //total
                            ws.Cells[index + i + 1, 15].Value = 0;
                        }
                        dFecha = dFecha.AddMonths(1);
                    }
                    //columna total numerico
                    ws.Column(15).Style.Numberformat.Format = "#,##0.00";

                    ws.Cells[index + 1, iMesesYear + 3].Value = "Total";
                    //pinta las celdas - segunda cabecera
                    rg = ws.Cells[index + 1, 3, index + 1, iMesesYear + 2];//más 2 por el espacio de 2 columnas
                    //rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                #endregion

                xlPackage.Save();
            }
        }

        public static void GenerarReportePorcentajes(string fileName, CaiPresupuestoDTO EntidadPresupuesto, CaiAjusteDTO EntidadAjuste, List<CaiAjusteempresaDTO> ListaEmpresas)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                #region PRIMERA PESTAÑA - REPORTE DE PORCENTAJES
                //Nombre de la hoja de caluculo
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Primer Ajuste de los Porcentaje de Aporte Anual de los Integrantes Registrados para el Presupuesto Anual del COES - " + EntidadAjuste.Caiajanio + " - Revisión 1");
                if (ws != null)
                {
                    int index = 2;
                    int columna = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "PORCENTAJES";
                    ws.Cells[index + 1, 3].Value = EntidadPresupuesto.Caiprsnombre + "/" + EntidadAjuste.Caiajnombre;
                    //selecciona las celdas a aplicar estilos
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    //estilos al titulo
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;//5

                    //CABECERA 1
                    ws.Cells[index, columna].Value = "Tipo de Integrante Registrado";
                    //aqui une dos celdas
                    ws.Cells[index, columna, index + 1, columna].Merge = true;
                    rg = ws.Cells[index, columna, index + 1, columna];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //CABECERA 2
                    ws.Cells[index, columna + 1].Value = "Integrantes Registrados";
                    //aqui une dos celdas
                    ws.Cells[index, columna + 1, index + 1, columna + 1].Merge = true;
                    rg = ws.Cells[index, columna + 1, index + 1, columna + 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //CABECERA 3
                    ws.Cells[index, columna + 2].Value = "Concesión de Generación";
                    ws.Cells[index, columna + 2, index, columna + 3].Merge = true;
                    rg = ws.Cells[index, columna + 2, index, columna + 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //subcabecera 1
                    ws.Cells[index + 1, columna + 2].Value = "Montos - Energía (miles S/.)";
                    rg = ws.Cells[index + 1, columna + 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //subcabecera 2
                    ws.Cells[index + 1, columna + 3].Value = "Montos - Potencia (miles S/.)";
                    rg = ws.Cells[index + 1, columna + 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //CABECERA 4
                    ws.Cells[index, columna + 4].Value = "Concesión de Distribución";
                    ws.Cells[index, columna + 4, index, columna + 5].Merge = true;
                    rg = ws.Cells[index, columna + 4, index, columna + 5];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //subcabecera 1
                    ws.Cells[index + 1, columna + 4].Value = "Montos - Energía (miles S/.)";
                    rg = ws.Cells[index + 1, columna + 4];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //subcabecera 2
                    ws.Cells[index + 1, columna + 5].Value = "Montos - Potencia (miles S/.)";
                    rg = ws.Cells[index + 1, columna + 5];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //CABECERA 5
                    ws.Cells[index, columna + 6].Value = "Concesión de Usuario Libre";
                    ws.Cells[index, columna + 6, index, columna + 7].Merge = true;
                    rg = ws.Cells[index, columna + 6, index, columna + 7];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //subcabecera 1
                    ws.Cells[index + 1, columna + 6].Value = "Montos - Energía (miles S/.)";
                    rg = ws.Cells[index + 1, columna + 6];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //subcabecera 2
                    ws.Cells[index + 1, columna + 7].Value = "Montos - Potencia (miles S/.)";
                    rg = ws.Cells[index + 1, columna + 7];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //CABECERA 6
                    ws.Cells[index, columna + 8].Value = "Concesión de Transmisión";
                    rg = ws.Cells[index, columna + 8];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //subcabecera
                    ws.Cells[index + 1, columna + 8].Value = "Montos - Transmisión (miles S/.)";
                    rg = ws.Cells[index + 1, columna + 8];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //CABECERA 7
                    ws.Cells[index, columna + 9].Value = "Total (miles S/.)";
                    //aqui une dos celdas
                    ws.Cells[index, columna + 9, index + 1, columna + 9].Merge = true;
                    rg = ws.Cells[index, columna + 9, index + 1, columna + 9];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //CABECERA 8
                    ws.Cells[index, columna + 10].Value = "Porcentaje de Aporte Anual";
                    //aqui une dos celdas
                    ws.Cells[index, columna + 10, index + 1, columna + 10].Merge = true;
                    rg = ws.Cells[index, columna + 10, index + 1, columna + 10];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Ancho 
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 25;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 20;
                    ws.Column(9).Width = 20;
                    ws.Column(10).Width = 20;
                    ws.Column(11).Width = 20;
                    ws.Column(12).Width = 20;

                    //DATOS
                    int iFila = index + 2;

                    //LLENANDO DATOS
                    for (int i = 0; i < ListaEmpresas.Count; i++)
                    {
                        ws.Cells[i + iFila, columna].Value = ListaEmpresas[i].Tipoemprdesc.ToString();
                        ws.Cells[i + iFila, columna + 1].Value = ListaEmpresas[i].Emprnomb.ToString();
                        ws.Cells[i + iFila, columna + 2].Value = 0;
                        ws.Cells[i + iFila, columna + 3].Value = 0;
                        ws.Cells[i + iFila, columna + 4].Value = 0;
                        ws.Cells[i + iFila, columna + 5].Value = 0;
                        ws.Cells[i + iFila, columna + 6].Value = 0;
                        ws.Cells[i + iFila, columna + 7].Value = 0;
                        ws.Cells[i + iFila, columna + 8].Value = 0;
                        ws.Cells[i + iFila, columna + 9].Value = 0;
                        ws.Cells[i + iFila, columna + 10].Value = 0;
                        ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(11).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(12).Style.Numberformat.Format = "#,##0.00";
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

                xlPackage.Save();
            }
        }
    }
}