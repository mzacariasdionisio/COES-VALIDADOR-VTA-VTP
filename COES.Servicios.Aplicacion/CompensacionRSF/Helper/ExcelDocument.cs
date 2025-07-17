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

namespace COES.Servicios.Aplicacion.CompensacionRSF.Helper
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
        /// Permite generar el archivo de exportación de las Rservas Asignadas del SEV
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="fechaInicio">Fecha</param>
        /// <param name="fechaFin">Fecha</param>
        /// <returns></returns>
        internal static void GenerarReporteReservaAsignadaSEV2020(string fileName, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                List<EveRsfhoraDTO> list = FactoryTransferencia.GetVcrAsignacionreservaRepository().ExportarReservaAsignadaSEV2020(fechaInicio, fechaFin);

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
                        int index = 5;
                        ExcelRange rg;

                        ws.Cells[2, 3].Value = "UNIDADES DE REGULACIÓN SECUNDARIA";
                        ws.Cells[index, 2].Value = "FECHA DESDE:";
                        rg = ws.Cells[index, 2, index, 2];
                        rg = ObtenerEstiloCelda(rg, 2);

                        ws.Cells[index, 3].Value = fechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[index, 4].Value = "FECHA HASTA:";
                        rg = ws.Cells[index, 4, index, 4];
                        rg = ObtenerEstiloCelda(rg, 2);
                        ws.Cells[index, 5].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);

                        index += 2;
                        ws.Cells[index, 2].Value = "FECHA";
                        ws.Cells[index, 3].Value = "INICIO";
                        ws.Cells[index, 4].Value = "FINAL";
                        ws.Cells[index, 5].Value = "URS";
                        ws.Cells[index, 6].Value = "RESERVA ASIGNADA (MW)";
                        ws.Cells[index, 7].Value = "TIPO";

                        rg = ws.Cells[index, 2, index, 7];
                        rg = ObtenerEstiloCelda(rg, 1);

                        index++;

                        foreach (EveRsfhoraDTO item in list)
                        {
                            ws.Cells[index, 2].Value = ((DateTime)item.Rsfhorfecha).ToString("dd/MM/yyyy");
                            ws.Cells[index, 3].Value = ((DateTime)item.Rsfhorinicio).ToString("HH:mm");
                            ws.Cells[index, 4].Value = ((DateTime)item.Rsfhorfin).ToString("HH:mm");
                            ws.Cells[index, 5].Value = item.Ursnomb;
                            ws.Cells[index, 6].Value = item.Valorautomatico;
                            ws.Cells[index, 7].Value = "AUTOMATICO";
                            index++;
                        }

                        if (list.Count > 0)
                        {
                            rg = ws.Cells[8, 2, index - 1, 7];
                            rg = ObtenerEstiloCelda(rg, 0);
                        }

                        ws.Column(2).Width = 20;
                        ws.Column(3).Width = 20;
                        ws.Column(4).Width = 20;
                        ws.Column(5).Width = 20;
                        ws.Column(6).Width = 20;
                        ws.Column(7).Width = 20;

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                        picture.SetPosition(10, 60);
                        picture.SetSize(120, 40);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de las Rservas Asignadas del SEV
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="fechaInicio">Fecha</param>
        /// <param name="fechaFin">Fecha</param>
        /// <returns></returns>
        public static void GenerarReporteReservaAsignadaSEV(string fileName, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                List<VcrEveRsfhoraDTO> list = FactoryTransferencia.GetVcrAsignacionreservaRepository().ExportarReservaAsignadaSEV(fechaInicio, fechaFin);

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
                        int index = 5;
                        ExcelRange rg;

                        ws.Cells[2, 3].Value = "UNIDADES DE REGULACIÓN SECUNDARIA";
                        ws.Cells[index, 2].Value = "FECHA DESDE:";
                        rg = ws.Cells[index, 2, index, 2];
                        rg = ObtenerEstiloCelda(rg, 2);

                        ws.Cells[index, 3].Value = fechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[index, 4].Value = "FECHA HASTA:";
                        rg = ws.Cells[index, 4, index, 4];
                        rg = ObtenerEstiloCelda(rg, 2);
                        ws.Cells[index, 5].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);

                        index += 2;
                        ws.Cells[index, 2].Value = "FECHA";
                        ws.Cells[index, 3].Value = "INICIO";
                        ws.Cells[index, 4].Value = "FINAL";
                        ws.Cells[index, 5].Value = "URS";
                        ws.Cells[index, 6].Value = "RESERVA ASIGNADA sub(MW)";
                        ws.Cells[index, 7].Value = "RESERVA ASIGNADA baj(MW)";
                        ws.Cells[index, 8].Value = "TIPO";

                        rg = ws.Cells[index, 2, index, 8];
                        rg = ObtenerEstiloCelda(rg, 1);

                        index++;

                        foreach (VcrEveRsfhoraDTO item in list)
                        {
                            ws.Cells[index, 2].Value = ((DateTime)item.Rsfhorfecha).ToString("dd/MM/yyyy");
                            ws.Cells[index, 3].Value = ((DateTime)item.Rsfhorinicio).ToString("HH:mm");
                            ws.Cells[index, 4].Value = ((DateTime)item.Rsfhorfin).ToString("HH:mm");

                            //- Ajuste movisoft 05022021

                            DateTime fecIni = (DateTime)item.Rsfhorinicio;
                            DateTime fecFin = (DateTime)item.Rsfhorfin;

                            if (fecIni.Hour == 23 && fecIni.Minute == 30 && fecFin.Hour == 0 && fecFin.Minute == 0)
                            {
                                ws.Cells[index, 4].Value = "24:00";
                            }

                            if (fecIni.Hour == 23 && fecIni.Minute == 30 && fecFin.Hour == 23 && fecFin.Minute == 59)
                            {
                                ws.Cells[index, 4].Value = "24:00";
                            }

                            //- Fin ajuste movisoft 05022021

                            ws.Cells[index, 5].Value = item.Ursnomb;
                            ws.Cells[index, 6].Value = item.Rsfdetsub;
                            ws.Cells[index, 7].Value = item.Rsfdetbaj;
                            ws.Cells[index, 8].Value = "AUTOMATICO";
                            index++;
                        }

                        if (list.Count > 0)
                        {
                            rg = ws.Cells[8, 2, index - 1, 8];
                            rg = ObtenerEstiloCelda(rg, 0);
                        }

                        ws.Column(2).Width = 20;
                        ws.Column(3).Width = 20;
                        ws.Column(4).Width = 20;
                        ws.Column(5).Width = 20;
                        ws.Column(6).Width = 20;
                        ws.Column(7).Width = 20;
                        ws.Column(8).Width = 20;

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                        picture.SetPosition(10, 60);
                        picture.SetSize(120, 40);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VCR_CMPENSOPER
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="ListaURS">Lista de URS</param>
        /// <param name="entidadRecalculo">Entidad de VcrRecalculoDTO</param>
        /// <param name="ListaCompensacion">Lista de registros TRN_BARRA_URS</param>
        /// <returns></returns>
        public static void GenerarFormatoVcrComOp(string fileName, List<TrnBarraursDTO> ListaURS, VcrRecalculoDTO entidadRecalculo, List<VcrCmpensoperDTO> ListaCompensacion)
        {
            int row2 = 7;
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(entidadRecalculo.Pericodi);

                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "Compensacion de costo operativos (S/) ";
                    ws.Cells[index + 1, 3].Value = EntidadPeriodo.PeriNombre + "/" + entidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    int col = 3;
                    ws.Cells[index, 2].Value = "FECHA";
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    ws.Column(2).Width = 20;
                    foreach (var URS in ListaURS)
                    {
                        ws.Cells[index, col].Value = URS.GrupoNomb;
                        ws.Cells[index, col, index, col + 1].Merge = true;
                        ws.Cells[index + 1, col].Value = "Op. Por RSF";
                        ws.Cells[index + 1, col + 1].Value = "Baja Efic.";
                        ws.Column(col).Style.Numberformat.Format = "#,##0.000000000000";
                        ws.Column(col + 1).Style.Numberformat.Format = "#,##0.000000000000";
                        ws.Column(col).Width = 20;
                        ws.Column(col + 1).Width = 20;
                        col = col + 2;
                    }
                    rg = ws.Cells[index, 2, index + 1, col - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index = index + 2;

                    int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                    string sMes = EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                    var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                    DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                    DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                    var dates = new List<DateTime>();
                    for (var dt = dFecInicio; dt <= dFecFin; dt = dt.AddDays(1))
                    {
                        dates.Add(dt);
                    }

                    foreach (var item in dates)
                    {
                        ws.Cells[row2, 2].Value = item.ToString("dd/MM/yyyy");
                        row2++;
                    }

                    int column = 3;
                    foreach (var item in ListaCompensacion.GroupBy(x => x.Gruponomb))
                    {

                        int fila = 7;

                        foreach (var dto in item)
                        {
                            ws.Cells[fila, column].Value = (dto.Vcmpopporrsf != null) ? dto.Vcmpopporrsf : Decimal.Zero; //dto.Vcmpopporrsf();

                            ws.Cells[fila, column + 1].Value = (dto.Vcmpopbajaefic != null) ? dto.Vcmpopbajaefic : Decimal.Zero;  //dto.Vcmpopbajaefic();
                            fila++;
                        }

                        //Border por celda
                        rg = ws.Cells[7, 2, fila - 1, column + 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[7, 3, fila - 1, column + 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        column = column + 2;
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
        /// Permite generar el archivo de exportación de la tabla VCR_COSTVARIABLE
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="ListaURS">Lista de URS</param>
        /// <param name="entidadRecalculo">Entidad de VcrRecalculoDTO</param>
        /// <param name="listaCostoVariable">Lista de registros de TRN_BARRA_URS</param>
        /// <returns></returns>
        public static void GenerarFormatoVcrCostVar(string fileName, List<TrnBarraursDTO> ListaURS, VcrRecalculoDTO entidadRecalculo, List<VcrCostvariableDTO> listaCostoVariable)
        {
            int row2 = 7;
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(entidadRecalculo.Pericodi);
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "Costo Variable (S/./MWh)";
                    ws.Cells[index + 1, 3].Value = EntidadPeriodo.PeriNombre + "/" + entidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    int col = 3;
                    ws.Cells[index, 2].Value = "FECHA";
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    ws.Column(2).Width = 20;
                    foreach (var URS in ListaURS)
                    {
                        ws.Cells[index, col].Value = URS.GrupoNomb;
                        ws.Cells[index + 1, col].Value = URS.EquiNomb;
                        ws.Column(col).Style.Numberformat.Format = "#,##0.000000000000";
                        ws.Column(col).Width = 20;
                        col = col + 1;
                    }
                    rg = ws.Cells[index, 2, index + 1, col - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index = index + 2;

                    string sMes = EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                    DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                    DateTime dFecFin = dFecInicio.AddMonths(1);

                    var dates = new List<DateTime>();
                    for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddMinutes(30))
                    {
                        dates.Add(dt);
                    }

                    foreach (var item in dates)
                    {
                        ws.Cells[row2, 2].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                        row2++;
                    }

                    int column = 3;
                    foreach (var item in listaCostoVariable.GroupBy(x => x.Gruponomb))
                    {

                        int fila = 7;

                        foreach (var dto in item)
                        {
                            ws.Cells[fila, column].Value = (dto.Vcvarcostvar != null) ? dto.Vcvarcostvar : Decimal.Zero; //dto.Vcmpopporrsf();

                            fila++;
                        }

                        //Border por celda
                        rg = ws.Cells[7, 2, fila - 1, column];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[7, 3, fila - 1, column];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        column++;
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
        /// Permite generar el archivo de exportación de la tabla VCR_VERDEFICIT
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadVersionDSRNS">Entidad de VcrVersiondsrnsDTO</param>
        /// <param name="ListaDeficit">Lista de registros de VcrVerdeficitDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoVcrDeficit(string fileName, VcrVersiondsrnsDTO EntidadVersionDSRNS, List<VcrVerdeficitDTO> ListaDeficit)
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
                    ws.Cells[index, 3].Value = "Déficit de Reserva";
                    ws.Cells[index + 1, 3].Value = EntidadVersionDSRNS.Perinombre + "/" + EntidadVersionDSRNS.Vcrdsrnombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 7];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "FECHA";
                    ws.Cells[index, 3].Value = "HORA INICIO";
                    ws.Cells[index, 4].Value = "HORA FIN";
                    ws.Cells[index, 5].Value = "EMPRESA";
                    ws.Cells[index, 6].Value = "URS";
                    ws.Cells[index, 7].Value = "DÉFICIT (MW)";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.000000000000";

                    rg = ws.Cells[index, 2, index, 7];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaDeficit)
                    {
                        ws.Cells[index, 2].Value = item.Vcrvdefecha.Value.ToString("dd/MM/yyyy");
                        ws.Cells[index, 3].Value = item.Vcrvdehorinicio.Value.ToString("HH:mm");
                        ws.Cells[index, 4].Value = item.Vcrvdehorfinal.Value.ToString("HH:mm").ToString();
                        ws.Cells[index, 5].Value = item.EmprNombre.ToString();
                        ws.Cells[index, 6].Value = item.Gruponomb.ToString();
                        ws.Cells[index, 7].Value = (item.Vcrvdedeficit != null) ? item.Vcrvdedeficit : Decimal.Zero;


                        //Border por celda
                        rg = ws.Cells[index, 2, index, 7];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 7, index, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    ws.Column(2).Width = 15;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 40;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;

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
        /// Permite generar el archivo de exportación de saldos
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de VcrVersiondsrnsDTO</param>
        /// <param name="ListaCargoIncumpl">Lista de registros de VcrVerdeficitDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoSaldos(string fileName, VcrRecalculoDTO EntidadRecalculo, List<VcrCargoincumplDTO> ListaCargoIncumpl)
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
                    ws.Cells[index, 3].Value = "Saldos Del Periodo y Revisión";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 5];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "CENTRAL";
                    ws.Cells[index, 4].Value = "UNIDAD";
                    ws.Cells[index, 5].Value = "SALDOS";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.000000000000";

                    rg = ws.Cells[index, 2, index, 5];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (VcrCargoincumplDTO item in ListaCargoIncumpl)
                    {
                        var Unidad = (new CompensacionRSFAppServicio()).GetByEquicodi(item.Equicodi);//se obtiene la unidad
                        int codigoPadre = (int)Unidad.Equipadre;
                        var Central = (new CompensacionRSFAppServicio()).GetByEquicodi(codigoPadre);//se obtiene la central
                        int codigoEmpresa = (int)Central.Emprcodi;
                        var Empresa = (new EmpresaAppServicio()).GetByIdEmpresa(codigoEmpresa);

                        ws.Cells[index, 2].Value = Empresa.EmprNombre.ToString();
                        ws.Cells[index, 3].Value = Central.Equinomb.ToString();
                        ws.Cells[index, 4].Value = Unidad.Equinomb.ToString();
                        ws.Cells[index, 5].Value = (item.Vcrcisaldomes != null) ? item.Vcrcisaldomes : Decimal.Zero;

                        //Border por celda
                        rg = ws.Cells[index, 2, index, 5];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5, index, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    ws.Column(2).Width = 35;
                    ws.Column(3).Width = 35;
                    ws.Column(4).Width = 35;
                    ws.Column(5).Width = 25;

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
        /// Permite generar el archivo de exportación de la tabla VCR_VERSUPERAVIT
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadVersionDSRNS">Entidad de EntidadVersionDSRNS</param>
        /// <param name="ListaSuperavit">Lista de registros de VcrVerSuperavitDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoVcrSuperavit(string fileName, VcrVersiondsrnsDTO EntidadVersionDSRNS, List<VcrVersuperavitDTO> ListaSuperavit)
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
                    ws.Cells[index, 3].Value = "Superávit de Reserva";
                    ws.Cells[index + 1, 3].Value = EntidadVersionDSRNS.Perinombre + "/" + EntidadVersionDSRNS.Vcrdsrnombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 7];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "FECHA";
                    ws.Cells[index, 3].Value = "HORA INICIO";
                    ws.Cells[index, 4].Value = "HORA FIN";
                    ws.Cells[index, 5].Value = "EMPRESA";
                    ws.Cells[index, 6].Value = "URS";
                    ws.Cells[index, 7].Value = "SUPERÁVIT (MW)";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.000000000000";

                    rg = ws.Cells[index, 2, index, 7];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaSuperavit)
                    {
                        ws.Cells[index, 2].Value = item.Vcrvsafecha.Value.ToString("dd/MM/yyyy");
                        ws.Cells[index, 3].Value = item.Vcrvsahorinicio.Value.ToString("HH:mm");
                        ws.Cells[index, 4].Value = item.Vcrvsahorfinal.Value.ToString("HH:mm");
                        ws.Cells[index, 5].Value = item.EmprNombre.ToString();
                        ws.Cells[index, 6].Value = item.Gruponomb.ToString();
                        ws.Cells[index, 7].Value = (item.Vcrvsasuperavit != null) ? item.Vcrvsasuperavit : Decimal.Zero;


                        //Border por celda
                        rg = ws.Cells[index, 2, index, 7];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 7, index, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    ws.Column(2).Width = 15;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 40;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;

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
        /// Permite generar el archivo de exportación de la tabla VCR_VERRNS
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadVersionDSRNS">Entidad de EntidadVersionDSRNS</param>
        /// <param name="ListaRNS">Lista de registros de VcrVerdeficitDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoVcrRNS(string fileName, VcrVersiondsrnsDTO EntidadVersionDSRNS, List<VcrVerrnsDTO> ListaRNS)
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
                    ws.Cells[index, 3].Value = "Reserva No Suministrada de " + EntidadVersionDSRNS.Vcrdsrusumodificacion;
                    ws.Cells[index + 1, 3].Value = EntidadVersionDSRNS.Perinombre + "/" + EntidadVersionDSRNS.Vcrdsrnombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 7];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    index += 3;
                    ws.Cells[index, 2].Value = "FECHA";
                    ws.Cells[index, 3].Value = "HORA INICIO";
                    ws.Cells[index, 4].Value = "HORA FIN";
                    ws.Cells[index, 5].Value = "EMPRESA";
                    ws.Cells[index, 6].Value = "URS";
                    ws.Cells[index, 7].Value = "RNS (MW)";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.000000000000";

                    rg = ws.Cells[index, 2, index, 7];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaRNS)
                    {
                        ws.Cells[index, 2].Value = item.Vcvrnsfecha.Value.ToString("dd/MM/yyyy");
                        ws.Cells[index, 3].Value = item.Vcvrnshorinicio.Value.ToString("HH:mm");
                        ws.Cells[index, 4].Value = item.Vcvrnshorfinal.Value.ToString("HH:mm");
                        ws.Cells[index, 5].Value = item.EmprNombre.ToString();
                        ws.Cells[index, 6].Value = item.Gruponomb.ToString();
                        ws.Cells[index, 7].Value = item.Vcvrnsrns;


                        //Border por celda
                        rg = ws.Cells[index, 2, index, 7];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 7, index, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        index++;
                    }

                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 40;
                    ws.Column(6).Width = 30;
                    ws.Column(7).Width = 25;

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
        /// Permite generar el archivo de exportación de la tabla VCR_VERPORCTRESERV
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadPR21">Entidad de VcrVersionincplDTO</param>
        /// <param name="ListaDetalle">Lista de registros de VcrVerporctreservDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoVcrRPNS(string fileName, VcrVersionincplDTO EntidadPR21, List<VcrVerporctreservDTO> ListaDetalle)
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
                    PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadPR21.Pericodi);
                    ws.Cells[index, 3].Value = "PORCENTAJE DE LA RESERVA PRIMARIA NO SUMINISTRADA";
                    ws.Cells[index + 1, 3].Value = EntidadPeriodo.PeriNombre + "/" + EntidadPR21.Vcrincnombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 7];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    string sMes = EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                    DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                    DateTime dFecFin = dFecInicio.AddMonths(1);

                    var listaFechas = new List<DateTime>();
                    for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddDays(1))
                    {
                        listaFechas.Add(dt);
                    }

                    index += 3;
                    ws.Cells[index, 2].Value = "CENTRAL";
                    ws.Cells[index, 3].Value = "UNIDAD";
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 40;
                    int col = 4;
                    foreach (var item in listaFechas)
                    {
                        ws.Cells[index, col].Value = item.ToString("dd/MM/yyyy");
                        ws.Cells[index + 1, col].Value = "RPNS";
                        ws.Column(col).Style.Numberformat.Format = "#,##0";
                        ws.Column(col).Width = 14;
                        col = col + 1;
                    }
                    rg = ws.Cells[index, 2, index + 1, col - 1];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index = index + 1;

                    index++;

                    foreach (var item in ListaDetalle)
                    {
                        ws.Cells[index, 2].Value = item.CentralNombre;
                        ws.Cells[index, 3].Value = item.UnidadNombre;
                        int aux = 4;
                        List<VcrVerporctreservDTO> listVerporctreserv = (new CompensacionRSFAppServicio()).ListVcrVerporctreservs(EntidadPR21.Vcrinccodi, item.Equicodicen, item.Equicodiuni);
                        foreach (var dto in listVerporctreserv)
                        {
                            if (aux > (col - 1))
                                break;
                            ws.Cells[index, aux++].Value = dto.Vcrvprrpns.ToString();
                        }
                        //Border por celda
                        rg = ws.Cells[index, 2, index, aux - 1];
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5, index, aux - 1];
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
        /// Permite generar el archivo de exportación de la tabla VCR_VERINCUMPLIM
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadPR21">Entidad de VcrVersionincplDTO</param>
        /// <param name="ListaDetalle">Lista de registros de VcrVerincumplimDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoVcrPR21(string fileName, VcrVersionincplDTO EntidadPR21, List<VcrVerincumplimDTO> ListaDetalle)
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
                    PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadPR21.Pericodi);
                    ws.Cells[index, 3].Value = "Incumplimiento PR-21";
                    ws.Cells[index + 1, 3].Value = EntidadPeriodo.PeriNombre + "/" + EntidadPR21.Vcrincnombre + "";
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 7];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;
                    //CABECERA DE TABLA
                    string sMes = EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                    DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                    DateTime dFecFin = dFecInicio.AddMonths(1);

                    var listaFechas = new List<DateTime>();
                    for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddDays(1))
                    {
                        listaFechas.Add(dt);
                    }

                    index += 3;
                    int col = 5;
                    ws.Cells[index, 2].Value = "CENTRAL";
                    ws.Cells[index, 3].Value = "UNIDAD";
                    ws.Cells[index, 4].Value = "CÓDIGO RPF";
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 10;
                    foreach (var item in listaFechas)
                    {
                        ws.Cells[index, col].Value = item.ToString("dd/MM/yyyy");
                        ws.Cells[index, col, index, col + 1].Merge = true;
                        ws.Cells[index + 1, col].Value = "Incumplimiento";
                        ws.Cells[index + 1, col + 1].Value = "Observación";
                        ws.Column(col).Style.Numberformat.Format = "#,##0";
                        ws.Column(col).Width = 14;
                        ws.Column(col + 1).Width = 11;
                        col = col + 2;
                    }
                    rg = ws.Cells[index, 2, index + 1, col - 1];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index = index + 1;

                    index++;

                    foreach (var item in ListaDetalle)
                    {
                        ws.Cells[index, 2].Value = item.CentralNombre;
                        ws.Cells[index, 3].Value = item.UniNombre;
                        ws.Cells[index, 4].Value = item.Vcrvincodrpf;
                        int aux = 5;
                        List<VcrVerincumplimDTO> listVerincumplim = (new CompensacionRSFAppServicio()).ListVcrVerincumplims(EntidadPR21.Vcrinccodi, item.Equicodicen, item.Equicodiuni);
                        foreach (var dto in listVerincumplim)
                        {
                            if (aux > (col - 2))
                                break;
                            ws.Cells[index, aux++].Value = dto.Vcrvincumpli.ToString();
                            string sVcrvinobserv = "";
                            if (dto.Vcrvinobserv != null)
                                sVcrvinobserv = dto.Vcrvinobserv.ToString();
                            ws.Cells[index, aux++].Value = sVcrvinobserv;
                        }
                        //Border por celda
                        rg = ws.Cells[index, 2, index, aux - 1];
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5, index, aux - 1];
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
        /// Permite generar el archivo de exportación de la Información "Superávit" CU10.01
        /// </summary>
        public static void GenerarReporteSuperavit(string fileName, VcrRecalculoDTO EntidadRecalculo, List<TrnBarraursDTO> listaURS)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "SUPERÁVIT";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index + 1, 2].Value = ""; //Lista de intervlos de dia
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    rg = ws.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Para cada URS
                    int iColumna = 3;
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws.Cells[index, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws.Cells[index, iColumna, index, iColumna + 2].Merge = true;

                        ws.Cells[index + 1, iColumna].Value = "SUPERAVIT (+) / DEFICT (-)";
                        ws.Cells[index + 1, iColumna + 1].Value = "Mayor Precio Asignado";
                        ws.Cells[index + 1, iColumna + 2].Value = "SUPERAVIT (+) / DEFICT (-)";

                        ws.Cells[index + 2, iColumna].Value = "MW eq";
                        ws.Cells[index + 2, iColumna + 1].Value = "S/ / MW-eq";
                        ws.Cells[index + 2, iColumna + 2].Value = "S/";
                        rg = ws.Cells[index, iColumna, index + 2, iColumna + 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws.Column(iColumna).Width = 15;
                        ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 1).Width = 15;
                        ws.Column(iColumna + 1).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 2).Width = 15;
                        ws.Column(iColumna + 2).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS en un mes
                        List<VcrTermsuperavitDTO> listaTermsuperavit = (new CompensacionRSFAppServicio()).ListVcrTermsuperavitsPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrTermsuperavitDTO dtoTermsuperavit in listaTermsuperavit)
                        {
                            //if (dtoTermsuperavit.Vcrtssupmwe != 0)
                            //{
                            //    ws.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtsdefmwe + dtoTermsuperavit.Vcrtssupmwe;
                            //    ws.Cells[iFila, iColumna + 1].Value = dtoTermsuperavit.Vcrtsmpa; //20190215
                            //    ws.Cells[iFila, iColumna + 2].Value = dtoTermsuperavit.Vcrtssuperavit + dtoTermsuperavit.Vcrtsdeficit;  //Vcrtssuperavit
                            //}
                            //else
                            //{
                            ws.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtsdefmwe + dtoTermsuperavit.Vcrtssupmwe; // cambio de momento Vcrtsdefmwe
                            ws.Cells[iFila, iColumna + 1].Value = dtoTermsuperavit.Vcrtsmpa;
                            ws.Cells[iFila, iColumna + 2].Value = dtoTermsuperavit.Vcrtsdeficit + dtoTermsuperavit.Vcrtssuperavit; //+ dtoTermsuperavit.Vcrtsdeficit;
                            //}
                            //ws.Cells[iFila, iColumna + 2].Value = (dtoTermsuperavit.Vcrtssupmwe + dtoTermsuperavit.Vcrtsdefmwe) * dtoTermsuperavit.Vcrtsmpa * EntidadRecalculo.Vcrecakcalidad / 30;
                            //Border por celda
                            rg = ws.Cells[iFila, iColumna, iFila, iColumna + 2];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            if (dtoTermsuperavit.Vcrtsdefmwe < 0)
                            {
                                rg = ws.Cells[iFila, iColumna, iFila, iColumna];
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FF0000"));
                            }
                            iFila++;
                        }
                        iColumna = iColumna + 3;
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

                #region SEGUNDA PESTAÑA - Magnitud equivalente (MW eq)
                ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("MW eq");
                if (ws2 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws2.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws2.Cells[index, 2, index + 2, 2].Merge = true;
                    ExcelRange rg = ws2.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws2.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws2.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws2.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 3;
                    ws2.Cells[index, iColumna].Value = "SUPERAVIT (+) / DEFICT (-) (MW eq-dia)";
                    ws2.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws2.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws2.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws2.Cells[index + 2, iColumna].Value = dtoBarraURS.EquiNomb;
                        rg = ws2.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws2.Column(iColumna).Width = 20;
                        ws2.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS en un mes
                        List<VcrTermsuperavitDTO> listaTermsuperavit = (new CompensacionRSFAppServicio()).ListVcrTermsuperavitsPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrTermsuperavitDTO dtoTermsuperavit in listaTermsuperavit)
                        {
                            if (dtoTermsuperavit.Vcrtssupmwe != 0)
                            { ws2.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtssupmwe; }
                            else
                            { ws2.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtsdefmwe; }

                            //Border por celda
                            rg = ws2.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            if (dtoTermsuperavit.Vcrtsdefmwe < 0)
                            {
                                rg = ws2.Cells[iFila, iColumna, iFila, iColumna];
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FF0000"));
                            }
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                #region TERCERA PESTAÑA - Mayor Precio Asignado
                ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("Precio");
                if (ws3 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws3.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws3.Cells[index, 2, index + 2, 2].Merge = true;
                    ExcelRange rg = ws3.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws3.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws3.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws3.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 3;
                    ws3.Cells[index, iColumna].Value = "Mayor Precio Asignado (S/ / MW eq-dia)";
                    ws3.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws3.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws3.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws3.Cells[index + 2, iColumna].Value = dtoBarraURS.EquiNomb;
                        rg = ws3.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws3.Column(iColumna).Width = 20;
                        ws3.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS en un mes
                        List<VcrTermsuperavitDTO> listaTermsuperavit = (new CompensacionRSFAppServicio()).ListVcrTermsuperavitsPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrTermsuperavitDTO dtoTermsuperavit in listaTermsuperavit)
                        {
                            ws3.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtsmpa;
                            //Border por celda
                            rg = ws3.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                #region CUARTA PESTAÑA - SUPERAVIT (+) / DEFICT (-) (S/)
                ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("Costo");
                if (ws4 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws4.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws4.Cells[index, 2, index + 2, 2].Merge = true;
                    ExcelRange rg = ws4.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws4.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws4.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws4.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 3;
                    ws4.Cells[index, iColumna].Value = "SUPERAVIT (+) / DEFICT (-) (S/)";
                    ws4.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws4.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws4.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        //ws4.Cells[index + 2, iColumna].Value = dtoBarraURS.Emprnomb; 
                        rg = ws4.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws4.Column(iColumna).Width = 20;
                        ws4.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS en un mes
                        List<VcrTermsuperavitDTO> listaTermsuperavit = (new CompensacionRSFAppServicio()).ListVcrTermsuperavitsPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrTermsuperavitDTO dtoTermsuperavit in listaTermsuperavit)
                        {
                            if (dtoTermsuperavit.Vcrtssupmwe != 0)
                            {
                                ws4.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtssuperavit;
                            }
                            else
                            {
                                ws4.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtsdeficit;
                            }
                            //ws4.Cells[iFila, iColumna].Value = (dtoTermsuperavit.Vcrtssupmwe + dtoTermsuperavit.Vcrtsdefmwe) * dtoTermsuperavit.Vcrtsmpa * EntidadRecalculo.Vcrecakcalidad / 30;
                            //Border por celda
                            rg = ws4.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            if (dtoTermsuperavit.Vcrtsdefmwe < 0)
                            {
                                rg = ws4.Cells[iFila, iColumna, iFila, iColumna];
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FF0000"));
                            }
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Reserva No Suministrada" CU10.02
        /// </summary>
        public static void GenerarReporteReservaNoSuministrada(string fileName, VcrRecalculoDTO EntidadRecalculo, List<TrnBarraursDTO> listaURS)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "RESERVA NO SUMINISTRADA";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index + 1, 2].Value = ""; //Lista de intervlos de dia
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    rg = ws.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Para cada URS
                    int iColumna = 3;
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws.Cells[index, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws.Cells[index, iColumna, index, iColumna + 2].Merge = true;

                        ws.Cells[index + 1, iColumna].Value = "RNS";
                        ws.Cells[index + 1, iColumna + 1].Value = "Precio max MA";
                        ws.Cells[index + 1, iColumna + 2].Value = "RNS";

                        ws.Cells[index + 2, iColumna].Value = "MW eq";
                        ws.Cells[index + 2, iColumna + 1].Value = "S/ / MW-eq";
                        ws.Cells[index + 2, iColumna + 2].Value = "S/";
                        rg = ws.Cells[index, iColumna, index + 2, iColumna + 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws.Column(iColumna).Width = 15;
                        ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 1).Width = 15;
                        ws.Column(iColumna + 1).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 2).Width = 15;
                        ws.Column(iColumna + 2).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS en un mes
                        List<VcrTermsuperavitDTO> listaTermsuperavit = (new CompensacionRSFAppServicio()).ListVcrTermsuperavitsPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrTermsuperavitDTO dtoTermsuperavit in listaTermsuperavit)
                        {
                            ws.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtsrnsmwe;
                            ws.Cells[iFila, iColumna + 1].Value = EntidadRecalculo.Vcrecapaosinergmin;
                            ws.Cells[iFila, iColumna + 2].Value = dtoTermsuperavit.Vcrtsresrvnosumn;
                            //Border por celda
                            rg = ws.Cells[iFila, iColumna, iFila, iColumna + 2];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 3;
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

                #region SEGUNDA PESTAÑA - Magnitud equivalente (MW eq)
                ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("MW eq");
                if (ws2 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws2.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws2.Cells[index, 2, index + 2, 2].Merge = true;
                    ExcelRange rg = ws2.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws2.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws2.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws2.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 3;
                    ws2.Cells[index, iColumna].Value = "RNS (MW eq-dia)";
                    ws2.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws2.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws2.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws2.Cells[index + 2, iColumna].Value = dtoBarraURS.EquiNomb;
                        rg = ws2.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws2.Column(iColumna).Width = 20;
                        ws2.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS en un mes
                        List<VcrTermsuperavitDTO> listaTermsuperavit = (new CompensacionRSFAppServicio()).ListVcrTermsuperavitsPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrTermsuperavitDTO dtoTermsuperavit in listaTermsuperavit)
                        {
                            ws2.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtsrnsmwe;
                            //Border por celda
                            rg = ws2.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                #region TERCERA PESTAÑA - Precio máximo de Mercado de Ajuste
                ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("PrecioMaxMA");
                if (ws3 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws3.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws3.Cells[index, 2, index + 2, 2].Merge = true;
                    ExcelRange rg = ws3.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws3.Column(2).Width = 15;
                    //Cabecera general
                    int iColumna = 2;
                    ws3.Cells[index, iColumna].Value = "Precio máximo de Mercado de Ajuste (S/ / MW eq-mes)";
                    rg = ws3.Cells[index, iColumna, index, iColumna];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    ws3.Cells[index, iColumna + 1].Value = EntidadRecalculo.Vcrecapaosinergmin;
                    rg = ws3.Cells[index, iColumna + 1, index, iColumna + 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                #endregion

                #region CUARTA PESTAÑA - Costo total por URS y por empresa a la que pertenece (S/)
                ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("Costo");
                if (ws4 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws4.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws4.Cells[index, 2, index + 2, 2].Merge = true;
                    ExcelRange rg = ws4.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws4.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws4.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws4.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 3;
                    ws4.Cells[index, iColumna].Value = "RNS (S/)";
                    ws4.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws4.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws4.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        //ws4.Cells[index + 2, iColumna].Value = dtoBarraURS.Emprnomb; 
                        rg = ws4.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws4.Column(iColumna).Width = 20;
                        ws4.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS en un mes
                        List<VcrTermsuperavitDTO> listaTermsuperavit = (new CompensacionRSFAppServicio()).ListVcrTermsuperavitsPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrTermsuperavitDTO dtoTermsuperavit in listaTermsuperavit)
                        {
                            ws4.Cells[iFila, iColumna].Value = dtoTermsuperavit.Vcrtsresrvnosumn;
                            //Border por celda
                            rg = ws4.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Reserva Asignada" CU10.03
        /// </summary>
        public static void GenerarReporteReservaAsignada2020(string fileName, VcrRecalculoDTO EntidadRecalculo, List<TrnBarraursDTO> listaURS)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "RESERVA ASIGNADA";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index + 1, 2].Value = ""; //Lista de intervlos de dia
                    ws.Cells[index, 2, index + 1, 2].Merge = true;
                    rg = ws.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Para cada URS
                    int iColumna = 3;
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws.Cells[index, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws.Cells[index, iColumna, index, iColumna + 5].Merge = true;

                        ws.Cells[index + 1, iColumna].Value = "PBF";
                        ws.Cells[index + 1, iColumna + 1].Value = "Precios";
                        ws.Cells[index + 1, iColumna + 2].Value = "ARS / ARB";
                        ws.Cells[index + 1, iColumna + 3].Value = "MA";
                        ws.Cells[index + 1, iColumna + 4].Value = "Precio Máximo";
                        ws.Cells[index + 1, iColumna + 5].Value = "ARS / ARB";

                        ws.Cells[index + 2, iColumna].Value = "MW eq";
                        ws.Cells[index + 2, iColumna + 1].Value = "S/ / MW-mes";
                        ws.Cells[index + 2, iColumna + 2].Value = "S/";
                        ws.Cells[index + 2, iColumna + 3].Value = "MW eq";
                        ws.Cells[index + 2, iColumna + 4].Value = "S/ / MW-mes";
                        ws.Cells[index + 2, iColumna + 5].Value = "S/";
                        rg = ws.Cells[index, iColumna, index + 2, iColumna + 5];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws.Column(iColumna).Width = 15;
                        ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 1).Width = 15;
                        ws.Column(iColumna + 1).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 2).Width = 15;
                        ws.Column(iColumna + 2).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 3).Width = 15;
                        ws.Column(iColumna + 3).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 4).Width = 15;
                        ws.Column(iColumna + 4).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 5).Width = 15;
                        ws.Column(iColumna + 5).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS
                        List<VcrAsignacionreservaDTO> listaAsignacionreserva = (new CompensacionRSFAppServicio()).ListVcrAsignacionreservasPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrAsignacionreservaDTO dtoAsignacionreserva in listaAsignacionreserva)
                        {
                            ws.Cells[iFila, iColumna].Value = dtoAsignacionreserva.Vcrarrapbf;
                            ws.Cells[iFila, iColumna + 1].Value = dtoAsignacionreserva.Vcrarprbf;
                            ws.Cells[iFila, iColumna + 2].Value = (dtoAsignacionreserva.Vcrarrapbf * dtoAsignacionreserva.Vcrarprbf) / 30;
                            ws.Cells[iFila, iColumna + 3].Value = dtoAsignacionreserva.Vcrarrama;
                            ws.Cells[iFila, iColumna + 4].Value = dtoAsignacionreserva.Vcrarramaursra; //20190215
                            ws.Cells[iFila, iColumna + 5].Value = (dtoAsignacionreserva.Vcrarrama * dtoAsignacionreserva.Vcrarramaursra) / 30;
                            //Border por celda
                            rg = ws.Cells[iFila, iColumna, iFila, iColumna + 5];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 6;
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

                #region SEGUNDA PESTAÑA - Magnitud equivalente (MW eq)
                ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("MW eq");
                if (ws2 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws2.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws2.Cells[index, 2, index + 2, 2].Merge = true;
                    ExcelRange rg = ws2.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws2.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws2.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws2.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 3;
                    ws2.Cells[index, iColumna].Value = "RAS / RAB (MW eq)";
                    ws2.Cells[index, iColumna, index, iColumna + (listaURS.Count * 2) - 1].Merge = true;
                    rg = ws2.Cells[index, iColumna, index, iColumna + (listaURS.Count * 2) - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws2.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws2.Cells[index + 1, iColumna, index + 1, iColumna + 1].Merge = true;
                        ws2.Cells[index + 2, iColumna].Value = "PBF";
                        ws2.Cells[index + 2, iColumna + 1].Value = "MA";
                        rg = ws2.Cells[index + 1, iColumna, index + 2, iColumna + 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws2.Column(iColumna).Width = 20;
                        ws2.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        ws2.Column(iColumna + 1).Width = 20;
                        ws2.Column(iColumna + 1).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS
                        List<VcrAsignacionreservaDTO> listaAsignacionreserva = (new CompensacionRSFAppServicio()).ListVcrAsignacionreservasPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrAsignacionreservaDTO dtoAsignacionreserva in listaAsignacionreserva)
                        {
                            ws2.Cells[iFila, iColumna].Value = dtoAsignacionreserva.Vcrarrapbf;
                            ws2.Cells[iFila, iColumna + 1].Value = dtoAsignacionreserva.Vcrarrama;
                            //Border por celda
                            rg = ws2.Cells[iFila, iColumna, iFila, iColumna + 1];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 2;
                    }
                }
                #endregion

                #region TERCERA PESTAÑA - Precios: Potencia Base Firme (PBF) y Mercado de Ajustes (MA)
                ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("Precio");
                if (ws3 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws3.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws3.Cells[index, 2, index + 2, 2].Merge = true;
                    ExcelRange rg = ws3.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws3.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws3.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws3.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 3;
                    ws3.Cells[index, iColumna].Value = "PRECIOS (S/ / MW-mes)";
                    ws3.Cells[index, iColumna, index, iColumna + (listaURS.Count * 2) - 1].Merge = true;
                    rg = ws3.Cells[index, iColumna, index, iColumna + (listaURS.Count * 2) - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws3.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws3.Cells[index + 1, iColumna, index + 1, iColumna + 1].Merge = true;
                        ws3.Cells[index + 2, iColumna].Value = "PBF";
                        ws3.Cells[index + 2, iColumna + 1].Value = "MA";
                        rg = ws3.Cells[index + 1, iColumna, index + 2, iColumna + 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws3.Column(iColumna).Width = 20;
                        ws3.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        ws3.Column(iColumna + 1).Width = 20;
                        ws3.Column(iColumna + 1).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS
                        List<VcrAsignacionreservaDTO> listaAsignacionreserva = (new CompensacionRSFAppServicio()).ListVcrAsignacionreservasPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrAsignacionreservaDTO dtoAsignacionreserva in listaAsignacionreserva)
                        {
                            ws3.Cells[iFila, iColumna].Value = dtoAsignacionreserva.Vcrarprbf;
                            ws3.Cells[iFila, iColumna + 1].Value = dtoAsignacionreserva.Vcrarmpa;
                            //Border por celda
                            rg = ws3.Cells[iFila, iColumna, iFila, iColumna + 1];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 2;
                    }
                }
                #endregion

                #region CUARTA PESTAÑA - Costo total por URS y por empresa a la que pertenece (S/)
                ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("Costo");
                if (ws4 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws4.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws4.Cells[index, 2, index + 2, 2].Merge = true;
                    ExcelRange rg = ws4.Cells[index, 2, index + 2, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws4.Column(2).Width = 15;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws4.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws4.Cells[index + 3, 2, iFila - 1, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 3;
                    ws4.Cells[index, iColumna].Value = "ARS / ARB (S/)";
                    ws4.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws4.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws4.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        //ws4.Cells[index + 2, iColumna].Value = dtoBarraURS.Emprnomb; 
                        rg = ws4.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws4.Column(iColumna).Width = 20;
                        ws4.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS
                        List<VcrAsignacionreservaDTO> listaAsignacionreserva = (new CompensacionRSFAppServicio()).ListVcrAsignacionreservasPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                        foreach (VcrAsignacionreservaDTO dtoAsignacionreserva in listaAsignacionreserva)
                        {
                            ws4.Cells[iFila, iColumna].Value = dtoAsignacionreserva.Vcrarasignreserva;
                            //Border por celda
                            rg = ws4.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Reserva Asignada" CU10.03
        /// </summary>
        public static void GenerarReporteReservaAsignada(string fileName, VcrRecalculoDTO EntidadRecalculo, List<TrnBarraursDTO> listaURS)
        {
            try {
                FileInfo newFile = new FileInfo(fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(fileName);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                    int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                    string sMes = EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                    var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                    DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                    DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                    #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                    if (ws != null)
                    {
                        int index = 2;
                        //TITULO
                        ws.Cells[index, 3].Value = "RESERVA ASIGNADA";
                        ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                        ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        //Inicio de Fila donde se muestra todo
                        index += 3;
                        //-------------------------------------------------------------------------------------------------------------------------------------------
                        //Primeras dos columnas:
                        //Cabecera:
                        ws.Cells[index + 1, 2].Value = ""; //Lista de intervalos de dia
                        ws.Cells[index, 2, index + 2, 2].Merge = true;
                        rg = ws.Cells[index, 2, index + 2, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws.Column(2).Width = 15;
                        //Filas:
                        int iFila = index + 3;
                        DateTime dFecha = dFecInicio;
                        while (dFecha <= dFecFin)
                        {
                            ws.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                            dFecha = dFecha.AddDays(1);
                        }
                        rg = ws.Cells[index + 3, 2, iFila - 1, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        //Para cada URS
                        int iColumna = 3;
                        foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                        {
                            //Cabecera:
                            ws.Cells[index, iColumna].Value = dtoBarraURS.GrupoNomb;
                            ws.Cells[index, iColumna, index, iColumna + 9].Merge = true;

                            ws.Cells[index + 1, iColumna].Value = "PBF.sub";
                            ws.Cells[index + 1, iColumna + 1].Value = "PBF.baj";
                            ws.Cells[index + 1, iColumna + 2].Value = "Precios.sub";
                            ws.Cells[index + 1, iColumna + 3].Value = "Precios.baj";
                            ws.Cells[index + 1, iColumna + 4].Value = "ARS / ARB";
                            ws.Cells[index + 1, iColumna + 5].Value = "MA.sub";
                            ws.Cells[index + 1, iColumna + 6].Value = "MA.baj";
                            ws.Cells[index + 1, iColumna + 7].Value = "Precio Máximo.sub";
                            ws.Cells[index + 1, iColumna + 8].Value = "Precio Máximo.baj";
                            ws.Cells[index + 1, iColumna + 9].Value = "ARS / ARB";

                            ws.Cells[index + 2, iColumna].Value = "MW eq";
                            ws.Cells[index + 2, iColumna + 1].Value = "MW eq";
                            ws.Cells[index + 2, iColumna + 2].Value = "S/ / MW-mes";
                            ws.Cells[index + 2, iColumna + 3].Value = "S/ / MW-mes";
                            ws.Cells[index + 2, iColumna + 4].Value = "S/";
                            ws.Cells[index + 2, iColumna + 5].Value = "MW eq";
                            ws.Cells[index + 2, iColumna + 6].Value = "MW eq";
                            ws.Cells[index + 2, iColumna + 7].Value = "S/ / MW-mes";
                            ws.Cells[index + 2, iColumna + 8].Value = "S/ / MW-mes";
                            ws.Cells[index + 2, iColumna + 9].Value = "S/";
                            rg = ws.Cells[index, iColumna, index + 2, iColumna + 9];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                            //Ancho
                            ws.Column(iColumna).Width = 15;
                            ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(iColumna + 1).Width = 15;
                            ws.Column(iColumna + 1).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(iColumna + 2).Width = 15;
                            ws.Column(iColumna + 2).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(iColumna + 3).Width = 15;
                            ws.Column(iColumna + 3).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(iColumna + 4).Width = 15;
                            ws.Column(iColumna + 4).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(iColumna + 5).Width = 15;
                            ws.Column(iColumna + 5).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(iColumna + 6).Width = 15;
                            ws.Column(iColumna + 6).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(iColumna + 7).Width = 15;
                            ws.Column(iColumna + 7).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(iColumna + 8).Width = 15;
                            ws.Column(iColumna + 8).Style.Numberformat.Format = "#,##0.00";
                            ws.Column(iColumna + 9).Width = 15;
                            ws.Column(iColumna + 9).Style.Numberformat.Format = "#,##0.00";
                            //Filas:
                            iFila = index + 3;
                            //Traemos la lista de información por URS
                            List<VcrAsignacionreservaDTO> listaAsignacionreserva = (new CompensacionRSFAppServicio()).ListVcrAsignacionreservasPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                            foreach (VcrAsignacionreservaDTO dtoAsignacionreserva in listaAsignacionreserva)
                            {
                                /*
                                ws.Cells[iFila, iColumna].Value = dtoAsignacionreserva.Vcrarrapbf;
                                ws.Cells[iFila, iColumna + 1].Value = dtoAsignacionreserva.Vcrarprbf;
                                ws.Cells[iFila, iColumna + 2].Value = (dtoAsignacionreserva.Vcrarrapbf * dtoAsignacionreserva.Vcrarprbf) / 30;
                                ws.Cells[iFila, iColumna + 3].Value = dtoAsignacionreserva.Vcrarrama;
                                ws.Cells[iFila, iColumna + 4].Value = dtoAsignacionreserva.Vcrarramaursra; //20190215
                                ws.Cells[iFila, iColumna + 5].Value = (dtoAsignacionreserva.Vcrarrama * dtoAsignacionreserva.Vcrarramaursra) / 30;
                            
                                VCRARRAMA -> Reserva Asignada correspondiente al Mercado de Ajuste - MA. En MWeq-día.
                                VCRARMPA -> Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-mes) 
                                VCRARRAMAURSRA -> Precio de la reserva asignada correspondiente al Mercado de Ajuste entre todos los URS que no superan el PBF de las URS que estan en la provision base
                                 */
                                //ASSETEC: 202010
                                int i = 1;
                                ws.Cells[iFila, iColumna].Value = dtoAsignacionreserva.Vcrarrapbf;  //MWeqPBFsubir 
                                ws.Cells[iFila, iColumna + i++].Value = dtoAsignacionreserva.Vcrarrapbfbajar; //MWeqPBFbajar 
                                ws.Cells[iFila, iColumna + i++].Value = dtoAsignacionreserva.Vcrarprbf; //PrecioPBFsubir 
                                ws.Cells[iFila, iColumna + i++].Value = dtoAsignacionreserva.Vcrarprbfbajar; //PrecioPBFbajar 
                                //Costo = MWeqPBFsubir * PrecioPBFsubir + MWeqPBFbajar * PrecioPBFbajar 
                                ws.Cells[iFila, iColumna + i++].Value = ((dtoAsignacionreserva.Vcrarrapbf * dtoAsignacionreserva.Vcrarprbf)/30 + (dtoAsignacionreserva.Vcrarrapbfbajar * dtoAsignacionreserva.Vcrarprbfbajar)/30);

                                ws.Cells[iFila, iColumna + i++].Value = dtoAsignacionreserva.Vcrarrama; //MWeqMAsubir
                                ws.Cells[iFila, iColumna + i++].Value = dtoAsignacionreserva.Vcrarramabajar;  //MWeqMAbajar
                                ws.Cells[iFila, iColumna + i++].Value = dtoAsignacionreserva.Vcrarramaursra;  //PrecioMAsubir, creo que deberia ser -> Vcrarramaursra; y no Vcrarmpa
                                ws.Cells[iFila, iColumna + i++].Value = dtoAsignacionreserva.Vcrarramaursrabajar;  //PrecioMAbajar, -> Vcrarramaursrabajar; y no Vcrarmpabajar
                                //Costo = MWeqMAsubir * PrecioMAsubir + MWeqMAbajar * PrecioMAbajar
                                ws.Cells[iFila, iColumna + i++].Value = ((dtoAsignacionreserva.Vcrarrama * dtoAsignacionreserva.Vcrarramaursra)/30 + (dtoAsignacionreserva.Vcrarramabajar * dtoAsignacionreserva.Vcrarramaursrabajar)/30);
                                //Border por celda
                                rg = ws.Cells[iFila, iColumna, iFila, iColumna + 9];
                                rg.Style.WrapText = true;
                                rg = ObtenerEstiloCelda(rg, 0);
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                iFila++;
                            }
                            iColumna = iColumna + 10;
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

                    #region SEGUNDA PESTAÑA - Magnitud equivalente (MW eq)
                    ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("MW eq");
                    if (ws2 != null)
                    {
                        int index = 2; //Inicio de Fila donde se muestra todo
                        ws2.Cells[index + 1, 2].Value = ""; //Lista de intervalos de dia
                        ws2.Cells[index, 2, index + 2, 2].Merge = true;
                        ExcelRange rg = ws2.Cells[index, 2, index + 2, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //-------------------------------------------------------------------------------------------------------------------------------------------
                        //Ancho de la columna de fechas
                        ws2.Column(2).Width = 15;
                        //Filas:
                        int iFila = index + 3;
                        DateTime dFecha = dFecInicio;
                        while (dFecha <= dFecFin)
                        {
                            ws2.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                            dFecha = dFecha.AddDays(1);
                        }
                        rg = ws2.Cells[index + 3, 2, iFila - 1, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        //Cabecera general
                        int iColumna = 3;
                        ws2.Cells[index, iColumna].Value = "RAS / RAB (MW eq)";
                        ws2.Cells[index, iColumna, index, iColumna + (listaURS.Count * 4) - 1].Merge = true;
                        rg = ws2.Cells[index, iColumna, index, iColumna + (listaURS.Count * 4) - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Para cada URS
                        foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                        {
                            //Cabecera:
                            ws2.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                            ws2.Cells[index + 1, iColumna, index + 1, iColumna + 3].Merge = true;

                            ws2.Cells[index + 2, iColumna].Value = "PBF.sub";
                            ws2.Cells[index + 2, iColumna + 1].Value = "PBF.baj";
                            ws2.Cells[index + 2, iColumna + 2].Value = "MA.sub";
                            ws2.Cells[index + 2, iColumna + 3].Value = "MA.baj"; 
                        
                            rg = ws2.Cells[index + 1, iColumna, index + 2, iColumna + 3];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                            //Ancho
                            ws2.Column(iColumna).Width = 20;
                            ws2.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                            ws2.Column(iColumna + 1).Width = 20;
                            ws2.Column(iColumna + 1).Style.Numberformat.Format = "#,##0.00";
                            ws2.Column(iColumna + 2).Width = 20;
                            ws2.Column(iColumna + 2).Style.Numberformat.Format = "#,##0.00";
                            ws2.Column(iColumna + 3).Width = 20;
                            ws2.Column(iColumna + 3).Style.Numberformat.Format = "#,##0.00";
                            //Filas:
                            iFila = index + 3;
                            //Traemos la lista de información por URS
                            List<VcrAsignacionreservaDTO> listaAsignacionreserva = (new CompensacionRSFAppServicio()).ListVcrAsignacionreservasPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                            foreach (VcrAsignacionreservaDTO dtoAsignacionreserva in listaAsignacionreserva)
                            {
                                ws2.Cells[iFila, iColumna].Value = dtoAsignacionreserva.Vcrarrapbf;
                                ws2.Cells[iFila, iColumna + 1].Value = dtoAsignacionreserva.Vcrarrapbfbajar;
                                ws2.Cells[iFila, iColumna + 2].Value = dtoAsignacionreserva.Vcrarrama;
                                ws2.Cells[iFila, iColumna + 3].Value = dtoAsignacionreserva.Vcrarramabajar;
                                //Border por celda
                                rg = ws2.Cells[iFila, iColumna, iFila, iColumna + 3];
                                rg = ObtenerEstiloCelda(rg, 0);
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                iFila++;
                            }
                            iColumna = iColumna + 4;
                        }
                    }
                    #endregion

                    #region TERCERA PESTAÑA - Precios: Potencia Base Firme (PBF) y Mercado de Ajustes (MA)
                    ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("Precio");
                    if (ws3 != null)
                    {
                        int index = 2;
                        ws3.Cells[index + 1, 2].Value = ""; //Lista de intervalos de dia
                        ws3.Cells[index, 2, index + 2, 2].Merge = true;
                        ExcelRange rg = ws3.Cells[index, 2, index + 2, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //-------------------------------------------------------------------------------------------------------------------------------------------
                        //Ancho de la columna de fechas
                        ws3.Column(2).Width = 15;
                        //Filas:
                        int iFila = index + 3;
                        DateTime dFecha = dFecInicio;
                        while (dFecha <= dFecFin)
                        {
                            ws3.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                            dFecha = dFecha.AddDays(1);
                        }
                        rg = ws3.Cells[index + 3, 2, iFila - 1, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        //Cabecera general
                        int iColumna = 3;
                        ws3.Cells[index, iColumna].Value = "PRECIOS (S/ / MW-mes)";
                        ws3.Cells[index, iColumna, index, iColumna + (listaURS.Count * 2) - 1].Merge = true;
                        rg = ws3.Cells[index, iColumna, index, iColumna + (listaURS.Count * 2) - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Para cada URS
                        foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                        {
                            //Cabecera:
                            ws3.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                            ws3.Cells[index + 1, iColumna, index + 1, iColumna + 3].Merge = true;

                            ws3.Cells[index + 2, iColumna].Value = "PBF.sub";
                            ws3.Cells[index + 2, iColumna + 1].Value = "PBF.baj";
                            ws3.Cells[index + 2, iColumna + 2].Value = "MA.sub";
                            ws3.Cells[index + 2, iColumna + 3].Value = "MA.baj";
                        
                            rg = ws3.Cells[index + 1, iColumna, index + 2, iColumna + 3];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                            //Ancho
                            ws3.Column(iColumna).Width = 20;
                            ws3.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                            ws3.Column(iColumna + 1).Width = 20;
                            ws3.Column(iColumna + 1).Style.Numberformat.Format = "#,##0.00";
                            ws3.Column(iColumna + 2).Width = 20;
                            ws3.Column(iColumna + 2).Style.Numberformat.Format = "#,##0.00";
                            ws3.Column(iColumna + 3).Width = 20;
                            ws3.Column(iColumna + 3).Style.Numberformat.Format = "#,##0.00";
                            //Filas:
                            iFila = index + 3;
                            //Traemos la lista de información por URS
                            List<VcrAsignacionreservaDTO> listaAsignacionreserva = (new CompensacionRSFAppServicio()).ListVcrAsignacionreservasPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                            foreach (VcrAsignacionreservaDTO dtoAsignacionreserva in listaAsignacionreserva)
                            {
                                ws3.Cells[iFila, iColumna].Value = dtoAsignacionreserva.Vcrarprbf;
                                ws3.Cells[iFila, iColumna + 1].Value = dtoAsignacionreserva.Vcrarprbfbajar;
                                ws3.Cells[iFila, iColumna + 2].Value = dtoAsignacionreserva.Vcrarmpa; //dtoAsignacionreserva.Vcrarramaursra;
                                ws3.Cells[iFila, iColumna + 3].Value = dtoAsignacionreserva.Vcrarmpabajar; //dtoAsignacionreserva.Vcrarramaursrabajar;
                                //Border por celda
                                rg = ws3.Cells[iFila, iColumna, iFila, iColumna + 3];
                                rg = ObtenerEstiloCelda(rg, 0);
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                iFila++;
                            }
                            iColumna = iColumna + 4;
                        }
                    }
                    #endregion

                    #region CUARTA PESTAÑA - Costo total por URS y por empresa a la que pertenece (S/)
                    ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("Costo");
                    if (ws4 != null)
                    {
                        int index = 2;
                        ws4.Cells[index + 1, 2].Value = ""; //Lista de intervalos de dia
                        ws4.Cells[index, 2, index + 2, 2].Merge = true;
                        ExcelRange rg = ws4.Cells[index, 2, index + 2, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //-------------------------------------------------------------------------------------------------------------------------------------------
                        //Ancho de la columna de fechas
                        ws4.Column(2).Width = 15;
                        //Filas:
                        int iFila = index + 3;
                        DateTime dFecha = dFecInicio;
                        while (dFecha <= dFecFin)
                        {
                            ws4.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                            dFecha = dFecha.AddDays(1);
                        }
                        rg = ws4.Cells[index + 3, 2, iFila - 1, 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        //Cabecera general
                        int iColumna = 3;
                        ws4.Cells[index, iColumna].Value = "ARS / ARB (S/)";
                        ws4.Cells[index, iColumna, index, iColumna + listaURS.Count * 4 - 1].Merge = true;
                        rg = ws4.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Para cada URS
                        foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                        {
                            //Cabecera:
                            ws4.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                            //ws4.Cells[index + 2, iColumna].Value = dtoBarraURS.Emprnomb; 
                            rg = ws4.Cells[index + 1, iColumna, index + 2, iColumna];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                            //Ancho
                            ws4.Column(iColumna).Width = 20;
                            ws4.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                            //Filas:
                            iFila = index + 3;
                            //Traemos la lista de información por URS
                            List<VcrAsignacionreservaDTO> listaAsignacionreserva = (new CompensacionRSFAppServicio()).ListVcrAsignacionreservasPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi);
                            foreach (VcrAsignacionreservaDTO dtoAsignacionreserva in listaAsignacionreserva)
                            {
                                ws4.Cells[iFila, iColumna].Value = dtoAsignacionreserva.Vcrarasignreserva;
                                //Border por celda
                                rg = ws4.Cells[iFila, iColumna, iFila, iColumna];
                                rg = ObtenerEstiloCelda(rg, 0);
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                iFila++;
                            }
                            iColumna = iColumna + 1;
                        }
                    }
                    #endregion

                    xlPackage.Save();
                }
            }
            catch (Exception e)
            {
                string sMensaje = e.StackTrace;
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "CostoOportunidad" CU10.04
        /// </summary>
        public static void GenerarReporteCostoOportunidad(string fileName, VcrRecalculoDTO EntidadRecalculo, List<TrnBarraursDTO> listaURS)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                //var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = dFecInicio.AddMonths(1); //DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "COSTO DE OPORTUNIDAD";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index + 1, 2].Value = ""; //Lista de intervlos de 30 minutos
                    ws.Cells[index, 2, index + 1, 3].Merge = true;
                    ws.Cells[index + 2, 3].Value = "Hrs";
                    rg = ws.Cells[index, 2, index + 2, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 5;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    int iHora = 1;
                    while (dFecha < dFecFin)
                    {
                        ws.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws.Cells[iFila++, 3].Value = iHora;
                        dFecha = dFecha.AddMinutes(30);
                        ws.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws.Cells[iFila++, 3].Value = iHora++;
                        dFecha = dFecha.AddMinutes(30);
                    }
                    rg = ws.Cells[index + 3, 2, iFila - 1, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Para cada URS
                    int iColumna = 4;
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws.Cells[index, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws.Cells[index, iColumna, index, iColumna + 2].Merge = true;

                        ws.Cells[index + 1, iColumna].Value = dtoBarraURS.EquiNomb;
                        ws.Cells[index + 1, iColumna, index + 1, iColumna + 2].Merge = true;

                        ws.Cells[index + 2, iColumna].Value = "Δ PDO  (MW)";
                        ws.Cells[index + 2, iColumna + 1].Value = "∆ Costo (S/MW) "; //NO ESTA CLARO QUE COSA ES
                        ws.Cells[index + 2, iColumna + 2].Value = "CO (S/.)";
                        rg = ws.Cells[index, iColumna, index + 2, iColumna + 2];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws.Column(iColumna).Width = 15;
                        ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 1).Width = 15;
                        ws.Column(iColumna + 1).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(iColumna + 2).Width = 15;
                        ws.Column(iColumna + 2).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS y Equicodi
                        List<VcrCostoportdetalleDTO> listaCostoportdetalle = (new CompensacionRSFAppServicio()).ListVcrCostoportdetallesPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi, dtoBarraURS.EquiCodi);
                        foreach (VcrCostoportdetalleDTO dtoCostoportdetalle in listaCostoportdetalle)
                        {
                            ws.Cells[iFila, iColumna].Value = dtoCostoportdetalle.Vcrcodpdo;
                            ws.Cells[iFila, iColumna + 1].Value = "";//NOSE
                            ws.Cells[iFila, iColumna + 2].Value = dtoCostoportdetalle.Vcrcodcostoportun;
                            //Border por celda
                            rg = ws.Cells[iFila, iColumna, iFila, iColumna + 2];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 3;
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

                #region SEGUNDA PESTAÑA - Delta Despacho con y sin Reserva
                ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("PDO");
                if (ws2 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws2.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws2.Cells[index, 2, index + 1, 3].Merge = true;
                    ws2.Cells[index + 2, 3].Value = "Hrs";
                    ExcelRange rg = ws2.Cells[index, 2, index + 2, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws2.Column(2).Width = 20;
                    ws2.Column(3).Width = 5;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    int iHora = 1;
                    while (dFecha < dFecFin)
                    {
                        ws2.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws2.Cells[iFila++, 3].Value = iHora;
                        dFecha = dFecha.AddMinutes(30);
                        ws2.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws2.Cells[iFila++, 3].Value = iHora++;
                        dFecha = dFecha.AddMinutes(30);
                    }
                    rg = ws2.Cells[index + 3, 2, iFila - 1, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 4;
                    ws2.Cells[index, iColumna].Value = "Δ PDO  (MW)";
                    ws2.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws2.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws2.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws2.Cells[index + 2, iColumna].Value = dtoBarraURS.EquiNomb;
                        rg = ws2.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws2.Column(iColumna).Width = 15;
                        ws2.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS y Equicodi
                        List<VcrCostoportdetalleDTO> listaCostoportdetalle = (new CompensacionRSFAppServicio()).ListVcrCostoportdetallesPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi, dtoBarraURS.EquiCodi);
                        foreach (VcrCostoportdetalleDTO dtoCostoportdetalle in listaCostoportdetalle)
                        {
                            ws2.Cells[iFila, iColumna].Value = dtoCostoportdetalle.Vcrcodpdo;
                            //Border por celda
                            rg = ws2.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                #region TERCERA PESTAÑA - Costo Marginal
                ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("CMgCP");
                if (ws3 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws3.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws3.Cells[index, 2, index + 1, 3].Merge = true;
                    ws3.Cells[index + 2, 3].Value = "Hrs";
                    ExcelRange rg = ws3.Cells[index, 2, index + 2, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws3.Column(2).Width = 20;
                    ws3.Column(3).Width = 5;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    int iHora = 1;
                    while (dFecha < dFecFin)
                    {
                        ws3.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws3.Cells[iFila++, 3].Value = iHora;
                        dFecha = dFecha.AddMinutes(30);
                        ws3.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws3.Cells[iFila++, 3].Value = iHora++;
                        dFecha = dFecha.AddMinutes(30);
                    }
                    rg = ws3.Cells[index + 3, 2, iFila - 1, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 4;
                    ws3.Cells[index, iColumna].Value = "CMgCP (S/ / MWh)";
                    ws3.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws3.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws3.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws3.Cells[index + 2, iColumna].Value = dtoBarraURS.EquiNomb;
                        rg = ws3.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws3.Column(iColumna).Width = 15;
                        ws3.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS y Equicodi
                        List<VcrCostoportdetalleDTO> listaCostoportdetalle = (new CompensacionRSFAppServicio()).ListVcrCostoportdetallesPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi, dtoBarraURS.EquiCodi);
                        foreach (VcrCostoportdetalleDTO dtoCostoportdetalle in listaCostoportdetalle)
                        {
                            ws3.Cells[iFila, iColumna].Value = dtoCostoportdetalle.Vcrcodcmgcp;
                            //Border por celda
                            rg = ws3.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                #region CUARTA PESTAÑA - Costo Variable
                ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("CV");
                if (ws4 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws4.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws4.Cells[index, 2, index + 1, 3].Merge = true;
                    ws4.Cells[index + 2, 3].Value = "Hrs";
                    ExcelRange rg = ws4.Cells[index, 2, index + 2, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws4.Column(2).Width = 20;
                    ws4.Column(3).Width = 5;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    int iHora = 1;
                    while (dFecha < dFecFin)
                    {
                        ws4.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws4.Cells[iFila++, 3].Value = iHora;
                        dFecha = dFecha.AddMinutes(30);
                        ws4.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws4.Cells[iFila++, 3].Value = iHora++;
                        dFecha = dFecha.AddMinutes(30);
                    }
                    rg = ws4.Cells[index + 3, 2, iFila - 1, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 4;
                    ws4.Cells[index, iColumna].Value = "CV (S/ / MWh)";
                    ws4.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws4.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws4.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws4.Cells[index + 2, iColumna].Value = dtoBarraURS.EquiNomb;
                        rg = ws4.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws4.Column(iColumna).Width = 15;
                        ws4.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS y Equicodi
                        List<VcrCostoportdetalleDTO> listaCostoportdetalle = (new CompensacionRSFAppServicio()).ListVcrCostoportdetallesPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi, dtoBarraURS.EquiCodi);
                        foreach (VcrCostoportdetalleDTO dtoCostoportdetalle in listaCostoportdetalle)
                        {
                            ws4.Cells[iFila, iColumna].Value = dtoCostoportdetalle.Vcrcodcv;
                            //Border por celda
                            rg = ws4.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                #region QUINTA PESTAÑA - Costo de Oportunidad
                ExcelWorksheet ws5 = xlPackage.Workbook.Worksheets.Add("CO");
                if (ws5 != null)
                {
                    int index = 1;
                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws5.Cells[index, 2].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ws5.Cells[index, 2, index + 1, 3].Merge = true;
                    ws5.Cells[index + 2, 3].Value = "Hrs";
                    ExcelRange rg = ws5.Cells[index, 2, index + 2, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Ancho
                    ws5.Column(2).Width = 20;
                    ws5.Column(3).Width = 5;
                    //Filas:
                    int iFila = index + 3;
                    DateTime dFecha = dFecInicio;
                    int iHora = 1;
                    while (dFecha < dFecFin)
                    {
                        ws5.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws5.Cells[iFila++, 3].Value = iHora;
                        dFecha = dFecha.AddMinutes(30);
                        ws5.Cells[iFila, 2].Value = dFecha.ToString("dd/MM/yyyy HH:mm");
                        ws5.Cells[iFila++, 3].Value = iHora++;
                        dFecha = dFecha.AddMinutes(30);
                    }
                    rg = ws5.Cells[index + 3, 2, iFila - 1, 3];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Cabecera general
                    int iColumna = 4;
                    ws5.Cells[index, iColumna].Value = "CO (S/)";
                    ws5.Cells[index, iColumna, index, iColumna + listaURS.Count - 1].Merge = true;
                    rg = ws5.Cells[index, iColumna, index, iColumna + listaURS.Count - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Para cada URS
                    foreach (TrnBarraursDTO dtoBarraURS in listaURS)
                    {
                        //Cabecera:
                        ws5.Cells[index + 1, iColumna].Value = dtoBarraURS.GrupoNomb;
                        ws5.Cells[index + 2, iColumna].Value = dtoBarraURS.EquiNomb;
                        rg = ws5.Cells[index + 1, iColumna, index + 2, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);
                        //Ancho
                        ws5.Column(iColumna).Width = 15;
                        ws5.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        //Filas:
                        iFila = index + 3;
                        //Traemos la lista de información por URS y Equicodi
                        List<VcrCostoportdetalleDTO> listaCostoportdetalle = (new CompensacionRSFAppServicio()).ListVcrCostoportdetallesPorMesURS(EntidadRecalculo.Vcrecacodi, dtoBarraURS.GrupoCodi, dtoBarraURS.EquiCodi);
                        foreach (VcrCostoportdetalleDTO dtoCostoportdetalle in listaCostoportdetalle)
                        {
                            ws5.Cells[iFila, iColumna].Value = dtoCostoportdetalle.Vcrcodcostoportun;
                            //Border por celda
                            rg = ws5.Cells[iFila, iColumna, iFila, iColumna];
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            iFila++;
                        }
                        iColumna = iColumna + 1;
                    }
                }
                #endregion

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Asignación Pago Diario" CU10.06
        /// </summary>
        public static void GenerarReporteAsignacionPagoDiario(string fileName, VcrRecalculoDTO EntidadRecalculo, List<VcrAsignacionpagoDTO> listaAsignacionpago)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "ASIGNACIÓN DE PAGO DIARIO S/";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index, 2].Value = "TITULAR URS";
                    ws.Column(2).Width = 45;
                    ws.Cells[index, 3].Value = "CENTRAL";
                    ws.Column(3).Width = 30;
                    ws.Cells[index, 4].Value = "UNIDAD";
                    ws.Column(4).Width = 20;
                    //Lista de intervlos de dia e columnas
                    int iColumna = 5;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws.Column(iColumna).Width = 12;
                        ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, iColumna++].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws.Cells[index, 2, index, iColumna - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Filas:
                    index++;
                    foreach (VcrAsignacionpagoDTO dtoAsignacionpago in listaAsignacionpago)
                    {
                        iColumna = 2;
                        if (dtoAsignacionpago.Emprcodi >= 0)
                        {
                            ws.Cells[index, iColumna++].Value = dtoAsignacionpago.Emprnomb;
                            ws.Cells[index, iColumna++].Value = dtoAsignacionpago.Equinombcen;
                            ws.Cells[index, iColumna++].Value = dtoAsignacionpago.Equinombuni;
                        }
                        else
                        {
                            TrnInfoadicionalDTO dtoInfoAdicional = (new TransferenciasAppServicio()).GetByIdTrnInfoadicional(dtoAsignacionpago.Emprcodi);
                            ws.Cells[index, iColumna++].Value = dtoInfoAdicional.Infadinomb;
                            ws.Cells[index, iColumna++].Value = "";
                            ws.Cells[index, iColumna++].Value = "";
                        }
                        //Traemos la lista de información por dia---------------------------------------------------------------------------------------------------------------
                        List<VcrAsignacionpagoDTO> listaAsignacionpagoDia = (new CompensacionRSFAppServicio()).GetByCriteriaVcrAsignacionpagos(EntidadRecalculo.Vcrecacodi, dtoAsignacionpago.Emprcodi, dtoAsignacionpago.Equicodicen, dtoAsignacionpago.Equicodiuni);
                        if(iNumeroDiasMes == listaAsignacionpagoDia.Count)
                        {
                            foreach (VcrAsignacionpagoDTO dtoAsignacionpagoDia in listaAsignacionpagoDia)
                            {
                                ws.Cells[index, iColumna++].Value = dtoAsignacionpagoDia.Vcrapasignpagorsf;
                                //Border por celda

                            }
                        }
                        else
                        {
                            for(int i = 1; i <= iNumeroDiasMes; i++)
                            {
                                VcrAsignacionpagoDTO dtoAsignacionpagoDia_ = listaAsignacionpagoDia.Where(x => x.Vcrapfecha.Day == i).FirstOrDefault();
                                ws.Cells[index, iColumna++].Value = (dtoAsignacionpagoDia_ != null ? dtoAsignacionpagoDia_.Vcrapasignpagorsf : 0);
                            }
                        }
                        
                        rg = ws.Cells[index, 2, index, iColumna - 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5, index, iColumna - 1];
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
                #endregion
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Costo del Servicio RSF" CU10.07
        /// </summary>
        public static void GenerarReporteCostoServicioRSFDiario(string fileName, VcrRecalculoDTO EntidadRecalculo, List<VcrServiciorsfDTO> listaServiciorsf)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "COSTO DEL SERVICIOS RSF DIARIO S/";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Cabecera:
                    ws.Cells[index + 1, 2].Value = "Costo de Oportunidad";
                    ws.Cells[index + 2, 2].Value = "Asignación de Reserva";
                    ws.Cells[index + 3, 2].Value = "Compensación por Operación";
                    ws.Cells[index + 4, 2].Value = "Reserva no Suministrada";
                    ws.Column(2).Width = 20;
                    rg = ws.Cells[index + 1, 2, index + 4, 2];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);
                    //Lista de intervlos de dia e columnas
                    int iColumna = 3;
                    DateTime dFecha = dFecInicio;
                    while (dFecha <= dFecFin)
                    {
                        ws.Column(iColumna).Width = 12;
                        ws.Column(iColumna).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[index, iColumna++].Value = dFecha.ToString("dd/MM/yyyy");
                        dFecha = dFecha.AddDays(1);
                    }
                    rg = ws.Cells[index, 3, index, iColumna - 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Columnas:
                    iColumna = 3;
                    foreach (VcrServiciorsfDTO dtoServiciorsf in listaServiciorsf)
                    {
                        ws.Cells[index + 1, iColumna].Value = dtoServiciorsf.Vcsrsfcostportun;
                        ws.Cells[index + 2, iColumna].Value = dtoServiciorsf.Vcsrsfasignreserva;
                        ws.Cells[index + 3, iColumna].Value = dtoServiciorsf.Vcsrsfcostotcomps;
                        ws.Cells[index + 4, iColumna].Value = dtoServiciorsf.Vcsrsfresvnosumn;
                        rg = ws.Cells[index + 1, 3, index + 4, iColumna];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        iColumna++;
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

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Cuadro PR21" CU10.08 - 2020.12 hacia atras
        /// </summary>
        public static void GenerarReporteCuadroPR212020(string fileName, VcrRecalculoDTO EntidadRecalculo, List<VcrMedborneDTO> listaMedborne)
        {

            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "CUADRO PR21 S/";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index, 2].Value = "Titular URS";
                    ws.Column(2).Width = 45;
                    ws.Cells[index, 3].Value = "Central";
                    ws.Column(3).Width = 30;
                    ws.Cells[index, 4].Value = "Unidad";
                    ws.Column(4).Width = 20;
                    ws.Cells[index, 5].Value = "Evaluación RPF";
                    ws.Column(5).Width = 10;
                    ws.Cells[index, 6].Value = "Produccion de energía (MW.h)";
                    ws.Column(6).Width = 15;
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7].Value = "Incumplimientos del mes";
                    ws.Column(7).Width = 15;
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 8].Value = "Cargo del mes";
                    ws.Column(8).Width = 15;
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 9].Value = "Cargo Incumplimiento";
                    ws.Column(9).Width = 15;
                    ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 10].Value = "Cumplimiento";
                    ws.Column(10).Width = 15;
                    ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 11].Value = "Reducción Pago Máximo del mes";
                    ws.Column(11).Width = 15;
                    ws.Column(11).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 12].Value = "Reducción Pago Ejecutado";
                    ws.Column(12).Width = 15;
                    ws.Column(12).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 13].Value = "Cargo Incumplimiento Transferido";
                    ws.Column(13).Width = 15;
                    ws.Column(13).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 14].Value = "Saldo del mes";
                    ws.Column(14).Width = 15;
                    ws.Column(14).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 15].Value = "Saldo del mes Anterior";
                    ws.Column(15).Width = 15;
                    ws.Column(15).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 2, index, 15];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Filas:
                    index++;
                    foreach (VcrMedborneDTO dtoMedborne in listaMedborne)
                    {
                        if (dtoMedborne.Emprcodi >= 0)
                        {
                            ws.Cells[index, 2].Value = dtoMedborne.Emprnomb;
                            ws.Cells[index, 3].Value = dtoMedborne.Equinombcen;
                            ws.Cells[index, 4].Value = dtoMedborne.Equinombuni;
                        }
                        else
                        {
                            TrnInfoadicionalDTO dtoInfoAdicional = (new TransferenciasAppServicio()).GetByIdTrnInfoadicional(dtoMedborne.Emprcodi);
                            ws.Cells[index, 2].Value = dtoInfoAdicional.Infadinomb;
                            ws.Cells[index, 3].Value = "";
                            ws.Cells[index, 4].Value = "";
                        }
                        ws.Cells[index, 5].Value = dtoMedborne.Vcmbciconsiderar;
                        ws.Cells[index, 6].Value = dtoMedborne.Vcrmebpotenciamed;
                        //Cantidad de Incumplimientos a la RPF de la Unidad de Generación “g” durante el mes “m” SEA LA SUMA DE LA FILA
                        VcrVerincumplimDTO dtoVerincumplim = (new CompensacionRSFAppServicio()).GetByIdVcrVerincumplimPorUnidad((int)EntidadRecalculo.Vcrinccodi, dtoMedborne.Equicodiuni, dtoMedborne.Equicodicen);
                        if (dtoVerincumplim != null)
                            ws.Cells[index, 7].Value = dtoVerincumplim.Vcrvincumpli; //Incumplimientos del mes
                        //Cargo del mes
                        VcrCargoincumplDTO dtoCargoincumpl = (new CompensacionRSFAppServicio()).GetByIdVcrCargoincumpl(EntidadRecalculo.Vcrecacodi, dtoMedborne.Equicodiuni);
                        if (dtoCargoincumpl != null)
                        {
                            ws.Cells[index, 8].Value = dtoCargoincumpl.Vcrcicargoincumplmes;
                            //Cargo Incumplimiento
                            ws.Cells[index, 9].Value = dtoCargoincumpl.Vcrcicargoincumpl;
                            //Cargo Incumplimiento Transferido
                            ws.Cells[index, 13].Value = dtoCargoincumpl.Vcrcicarginctransf;
                            //Reducción Pago Ejecutado
                            ws.Cells[index, 14].Value = dtoCargoincumpl.Vcrcisaldomes;//Vcrcicargoincumpl
                            ws.Cells[index, 15].Value = dtoCargoincumpl.VcrcisaldomesAnterior;
                        }
                        //Cumplimiento
                        VcrReduccpagoejeDTO dtoReduccpagoeje = (new CompensacionRSFAppServicio()).GetByIdVcrReduccpagoeje(EntidadRecalculo.Vcrecacodi, dtoMedborne.Equicodiuni);
                        if (dtoReduccpagoeje != null)
                        {
                            ws.Cells[index, 10].Value = dtoReduccpagoeje.Vcrpecumplmes;
                            //Reducción Pago Máximo del mes
                            ws.Cells[index, 11].Value = dtoReduccpagoeje.Vcrpereduccpagomax;
                            ws.Cells[index, 12].Value = dtoReduccpagoeje.Vcrpereduccpagoeje;
                        }
                        rg = ws.Cells[index, 2, index, 15];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5, index, 15];
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
                #endregion
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Cuadro PR21" CU10.08 - 2021.01 hacia adelante
        /// </summary>
        public static void GenerarReporteCuadroPR21(string fileName, VcrRecalculoDTO EntidadRecalculo, List<VcrMedborneDTO> listaMedborne)
        {

            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "CUADRO PR21 S/";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index, 2].Value = "Empresa";
                    ws.Column(2).Width = 45;
                    ws.Cells[index, 3].Value = "Central";
                    ws.Column(3).Width = 30;
                    ws.Cells[index, 4].Value = "Unidad";
                    ws.Column(4).Width = 20;
                    ws.Cells[index, 5].Value = "Evaluación RPF";
                    ws.Column(5).Width = 10;
                    ws.Cells[index, 6].Value = "Produccion de energía (MW.h)";
                    ws.Column(6).Width = 15;
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7].Value = "Incumplimientos del mes";
                    ws.Column(7).Width = 15;
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 8].Value = "Presencia del mes";
                    ws.Column(8).Width = 15;
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 9].Value = "Cargo del mes";
                    ws.Column(9).Width = 15;
                    ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 10].Value = "Cargo Incumplimiento";
                    ws.Column(10).Width = 15;
                    ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 11].Value = "Cumplimiento";
                    ws.Column(11).Width = 15;
                    ws.Column(11).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 12].Value = "Incentivo al cumplimiento"; //Antes: Reducción Pago Máximo del mes
                    ws.Column(12).Width = 15;
                    ws.Column(12).Style.Numberformat.Format = "#,##0.00";
                    /*ws.Cells[index, 12].Value = "Reducción Pago Ejecutado";
                    ws.Column(12).Width = 15;
                    ws.Column(12).Style.Numberformat.Format = "#,##0.00";*/
                    ws.Cells[index, 13].Value = "Cargo Incumplimiento Transferido";
                    ws.Column(13).Width = 15;
                    ws.Column(13).Style.Numberformat.Format = "#,##0.00";
                    /*ws.Cells[index, 14].Value = "Saldo del mes";
                    ws.Column(14).Width = 15;
                    ws.Column(14).Style.Numberformat.Format = "#,##0.00";*/
                    /*ws.Cells[index, 15].Value = "Saldo del mes Anterior";
                    ws.Column(15).Width = 15;
                    ws.Column(15).Style.Numberformat.Format = "#,##0.00";*/
                    rg = ws.Cells[index, 2, index, 13];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Filas:
                    index++;
                    foreach (VcrMedborneDTO dtoMedborne in listaMedborne)
                    {
                        if (dtoMedborne.Emprcodi >= 0)
                        {
                            ws.Cells[index, 2].Value = dtoMedborne.Emprnomb;
                            ws.Cells[index, 3].Value = dtoMedborne.Equinombcen;
                            ws.Cells[index, 4].Value = dtoMedborne.Equinombuni;
                        }
                        else
                        {
                            TrnInfoadicionalDTO dtoInfoAdicional = (new TransferenciasAppServicio()).GetByIdTrnInfoadicional(dtoMedborne.Emprcodi);
                            ws.Cells[index, 2].Value = dtoInfoAdicional.Infadinomb;
                            ws.Cells[index, 3].Value = "";
                            ws.Cells[index, 4].Value = "";
                        }
                        ws.Cells[index, 5].Value = dtoMedborne.Vcmbciconsiderar;
                        ws.Cells[index, 6].Value = dtoMedborne.Vcrmebpotenciamed;
                        //Presencia del mes
                        ws.Cells[index, 8].Value = dtoMedborne.Vcrmebpresencia;
                        //Cantidad de Incumplimientos a la RPF de la Unidad de Generación “g” durante el mes “m” SEA LA SUMA DE LA FILA
                        VcrVerincumplimDTO dtoVerincumplim = (new CompensacionRSFAppServicio()).GetByIdVcrVerincumplimPorUnidad((int)EntidadRecalculo.Vcrinccodi, dtoMedborne.Equicodiuni, dtoMedborne.Equicodicen);
                        if (dtoVerincumplim != null)
                            ws.Cells[index, 7].Value = dtoVerincumplim.Vcrvincumpli; //Incumplimientos del mes
                        //Cargo del mes
                        VcrCargoincumplDTO dtoCargoincumpl = (new CompensacionRSFAppServicio()).GetByIdVcrCargoincumpl(EntidadRecalculo.Vcrecacodi, dtoMedborne.Equicodiuni);
                        if (dtoCargoincumpl != null)
                        {
                            //Cargo del mes
                            ws.Cells[index, 9].Value = dtoCargoincumpl.Vcrcicargoincumplmes; //Salia del calculo de dVcrciCargoIncumplmes - dTotalSaldoAnterior
                            //Cargo Incumplimiento
                            ws.Cells[index, 10].Value = dtoCargoincumpl.Vcrcicargoincumpl; //Sigue siendo el mismo
                            //Cumpliemiento
                            ws.Cells[index, 11].Value = dtoCargoincumpl.Vcrciincumplsrvrsf; //antes dtoReduccpagoeje.Vcrpecumplmes
                            //3.2.3.1.	Incentivo al cumplimiento
                            ws.Cells[index, 12].Value = dtoCargoincumpl.Vcrciincent; //antes dtoReduccpagoeje.Vcrpereduccpagomax
                            //Cargo Incumplimiento Transferido
                            ws.Cells[index, 13].Value = dtoCargoincumpl.Vcrcicarginctransf;
                            //Reducción Pago Ejecutado
                            //ws.Cells[index, 14].Value = dtoCargoincumpl.Vcrcisaldomes;//Vcrcicargoincumpl
                            //ws.Cells[index, 15].Value = dtoCargoincumpl.VcrcisaldomesAnterior;
                        }
                        rg = ws.Cells[index, 2, index, 13];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 5, index, 13];
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
                #endregion
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Reporte Resumen" CU10.09 - 2020.12 hacia atras
        /// </summary>
        public static void GenerarReporteResumen2020(string fileName, VcrRecalculoDTO EntidadRecalculo, List<VcrEmpresarsfDTO> listaEmpresarsf)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "REPORTE RESUMEN S/";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index, 2].Value = "Empresas";
                    ws.Column(2).Width = 45;
                    ws.Cells[index, 3].Value = "Costo de Oportunidad + Costo de operación + Asignación de Reserva";
                    ws.Column(3).Width = 25;
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 4].Value = "Reserva no suministrada";
                    ws.Column(4).Width = 25;
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 5].Value = "Término por superávit de reserva";
                    ws.Column(5).Width = 25;
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 6].Value = "Pagos por incumplimiento RPF";
                    ws.Column(6).Width = 25;
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7].Value = "Pagos pro RSF";
                    ws.Column(7).Width = 25;
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 8].Value = "Total";
                    ws.Column(8).Width = 25;
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 2, index, 8];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Filas:
                    index++;
                    foreach (VcrEmpresarsfDTO dtoEmpresarsf in listaEmpresarsf)
                    {
                        decimal dTotal = 0;
                        if (dtoEmpresarsf.Emprcodi < 0)
                        {
                            string nomb = FactoryTransferencia.GetTrnInfoadicionalRepository().GetById(dtoEmpresarsf.Emprcodi).Infadinomb;
                            ws.Cells[index, 2].Value = nomb;
                        }
                        else
                        {
                            ws.Cells[index, 2].Value = dtoEmpresarsf.Emprnomb;
                        }
                        ws.Cells[index, 3].Value = dtoEmpresarsf.Vcersfcostoportun + dtoEmpresarsf.Vcersfcompensacion + dtoEmpresarsf.Vcersfasignreserva;
                        dTotal += dtoEmpresarsf.Vcersfcostoportun + dtoEmpresarsf.Vcersfcompensacion + dtoEmpresarsf.Vcersfasignreserva;
                        ws.Cells[index, 4].Value = dtoEmpresarsf.Vcersfresvnosumins;
                        dTotal += dtoEmpresarsf.Vcersfresvnosumins;
                        ws.Cells[index, 5].Value = dtoEmpresarsf.Vcersftermsuperavit;
                        dTotal += dtoEmpresarsf.Vcersftermsuperavit;
                        ws.Cells[index, 6].Value = -1 * dtoEmpresarsf.Vcersfpagoincumpl;
                        dTotal -= dtoEmpresarsf.Vcersfpagoincumpl;
                        ws.Cells[index, 7].Value = dtoEmpresarsf.Vcersfpagorsf;
                        dTotal += dtoEmpresarsf.Vcersfpagorsf;
                        ws.Cells[index, 8].Value = dTotal;

                        rg = ws.Cells[index, 2, index, 8];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 3, index, 8];
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
                #endregion
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Reporte Resumen" CU10.09 - 2021.01 hacia adelante
        /// </summary>
        public static void GenerarReporteResumen(string fileName, VcrRecalculoDTO EntidadRecalculo, List<VcrEmpresarsfDTO> listaEmpresarsf)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(EntidadRecalculo.Pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);

                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RESUMEN");
                if (ws != null)
                {
                    int index = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "REPORTE RESUMEN S/";
                    ws.Cells[index + 1, 3].Value = EntidadRecalculo.Perinombre + "/" + EntidadRecalculo.Vcrecanombre;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index, 2].Value = "Empresas";
                    ws.Column(2).Width = 45;
                    ws.Cells[index, 3].Value = "Costo de Oportunidad + Costo de operación + Asignación de Reserva";
                    ws.Column(3).Width = 25;
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 4].Value = "Reserva no suministrada";
                    ws.Column(4).Width = 25;
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 5].Value = "Incumplimiento e Incentivo al cumplimiento RPF";
                    ws.Column(5).Width = 25;
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 6].Value = "Pagos por RSF";
                    ws.Column(6).Width = 25;
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[index, 7].Value = "Total";
                    ws.Column(7).Width = 25;
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    rg = ws.Cells[index, 2, index, 7];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);
                    //Filas:
                    index++;
                    foreach (VcrEmpresarsfDTO dtoEmpresarsf in listaEmpresarsf)
                    {
                        decimal dTotal = 0;
                        if (dtoEmpresarsf.Emprcodi < 0)
                        {
                            string nomb = FactoryTransferencia.GetTrnInfoadicionalRepository().GetById(dtoEmpresarsf.Emprcodi).Infadinomb;
                            ws.Cells[index, 2].Value = nomb;
                        }
                        else
                        {
                            ws.Cells[index, 2].Value = dtoEmpresarsf.Emprnomb;
                        }
                        ws.Cells[index, 3].Value = dtoEmpresarsf.Vcersfcostoportun + dtoEmpresarsf.Vcersfcompensacion + dtoEmpresarsf.Vcersfasignreserva;
                        dTotal += dtoEmpresarsf.Vcersfcostoportun + dtoEmpresarsf.Vcersfcompensacion + dtoEmpresarsf.Vcersfasignreserva;
                        ws.Cells[index, 4].Value = dtoEmpresarsf.Vcersfresvnosumins;
                        dTotal += dtoEmpresarsf.Vcersfresvnosumins;
                        ws.Cells[index, 5].Value = dtoEmpresarsf.Vcersfpagoincumpl; //antes este concepto restaba (-1 * dtoEmpresarsf.Vcersfpagoincumpl)
                        dTotal += dtoEmpresarsf.Vcersfpagoincumpl;  //antes restaba -= ahora suma +=
                        ws.Cells[index, 6].Value = dtoEmpresarsf.Vcersfpagorsf;
                        dTotal += dtoEmpresarsf.Vcersfpagorsf;
                        ws.Cells[index, 7].Value = dTotal;

                        rg = ws.Cells[index, 2, index, 7];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg = ws.Cells[index, 3, index, 7];
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
                #endregion
                xlPackage.Save();
            }
        }
    }
}