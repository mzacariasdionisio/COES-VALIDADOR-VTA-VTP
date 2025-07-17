using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Helper
{
    public class ExcelDocument
    {

        /// <summary>
        /// Permite generar el archivo excel del Nodo Energetico
        /// </summary>
        /// <param name="listIpp"></param>
        public static void GenerarArchivoNodoEnergetico(List<MeMedicion96DTO> listIpp, List<MeMedicion96DTO> listIpf,
            List<MeMedicion96DTO> listItp, List<MeMedicion96DTO> listItf, DateTime fechaIni, DateTime fechaFin,
            bool incluirLogo, List<NrProcesoDTO> listHoras, List<NrProcesoDTO> listaSitf,
            List<NrProcesoDTO> listaSipf)
        {

            ArrayList arrEdeMedida = new ArrayList();
            arrEdeMedida.Add(listIpp);
            arrEdeMedida.Add(listIpf);
            arrEdeMedida.Add(listItp);
            arrEdeMedida.Add(listItf);

            ArrayList arrEdeTitulo = new ArrayList();
            arrEdeTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloNeEdeIpp);
            arrEdeTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloNeEdeIpf);
            arrEdeTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloNeEdeItp);
            arrEdeTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloNeEdeItf);

            ArrayList arrEdeAbreviatura = new ArrayList();
            arrEdeAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaNeEdeIpp);
            arrEdeAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaNeEdeIpf);
            arrEdeAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaNeEdeItp);
            arrEdeAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaNeEdeItf);

            ArrayList arrSobrecosto = new ArrayList();
            arrSobrecosto.Add(listaSitf);
            arrSobrecosto.Add(listaSipf);

            ArrayList arrSobrecostoTitulo = new ArrayList();
            arrSobrecostoTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloNeSitf);
            arrSobrecostoTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloNeSipf);

            ArrayList arrSobrecostoAbreviatura = new ArrayList();
            arrSobrecostoAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaNeSitf);
            arrSobrecostoAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaNeSipf);


            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaInformeReservaNodo +
                              NombreArchivo.ReporteNodoEnergetico;


                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    #region EDE
                    //hojas EDE
                    int idx = 0;
                    foreach (List<MeMedicion96DTO> listaEDE in arrEdeMedida)
                    {
                        string titulo = arrEdeTitulo[idx].ToString();
                        string abreviatura = arrEdeAbreviatura[idx].ToString();
                        ExcelWorksheet ws =
                            xlPackage.Workbook.Worksheets.Add(abreviatura);

                        if (ws != null)
                        {
                            ws.Cells[5, 2].Value = "NODO ENERGÉTICO - COES";
                            ws.Cells[6, 2].Value = "Desde:  " + fechaIni.ToString(Constantes.FormatoFecha) + " Hasta: " +
                                                   fechaFin.ToString(Constantes.FormatoFecha);
                            ws.Cells[7, 2].Value = titulo;

                            int index = 9;

                            ws.Cells[index, 2].Value = "EMPRESA";
                            ws.Cells[index, 3].Value = "EQUIPO";
                            ws.Cells[index, 4].Value = "FECHA";
                            ws.Cells[index, 5].Value = "TOTAL";

                            int col = 6;
                            DateTime fechaCabecera = Convert.ToDateTime("2017-01-01");
                            for (int i = 0; i < 96; i++)
                            {
                                fechaCabecera = fechaCabecera.AddMinutes(15);
                                ws.Cells[index, col].Value = fechaCabecera.ToString(Constantes.FormatoHoraMinuto);
                                col++;
                            }

                            int columnaMax = 101;


                            ExcelRange rg = ws.Cells[index, 2, index, columnaMax];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            index = index + 1;

                            foreach (MeMedicion96DTO item in listaEDE)
                            {
                                ws.Cells[index, 2].Value = item.Emprnomb;
                                ws.Cells[index, 3].Value = item.Ptomedielenomb;
                                ws.Cells[index, 4].Value = ((DateTime)item.Medifecha).ToString(Constantes.FormatoFecha);
                                ws.Cells[index, 5].Value = item.Meditotal;

                                col = 6;

                                for (int i = 0; i < 96; i++)
                                {

                                    ws.Cells[index, col].Value =
                                        (decimal)
                                        item.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(item, null);

                                    col++;
                                }


                                rg = ws.Cells[index, 2, index, columnaMax];
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                index++;
                            }

                            rg = ws.Cells[9, 2, index - 1, columnaMax];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            ws.Column(1).Width = 7;

                            rg = ws.Cells[9, 2, index, columnaMax];
                            rg.AutoFitColumns();

                            if (incluirLogo)
                            {
                                try
                                {
                                    HttpWebRequest request =
                                        (HttpWebRequest)
                                        System.Net.HttpWebRequest.Create(
                                            ConfigurationManager.AppSettings["LogoCoes"].ToString());

                                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                                    System.Drawing.Image img =
                                        System.Drawing.Image.FromStream(response.GetResponseStream());
                                    ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                    picture.From.Column = 1;
                                    picture.From.Row = 1;
                                    picture.To.Column = 2;
                                    picture.To.Row = 2;
                                    picture.SetSize(120, 60);
                                }
                                catch { }
                            }





                        }

                        //xlPackage.Save();
                        idx++;
                    }
                    #endregion

                    #region Horas
                    //horas
                    //foreach (var itemHora in listHoras)
                    {
                        string titulo = ConstanteReservaFriaNodoEnergetico.TituloNeHoras;
                        string abreviatura = ConstanteReservaFriaNodoEnergetico.AbreviaturaNeHoras;
                        ExcelWorksheet ws =
                            xlPackage.Workbook.Worksheets.Add(abreviatura);

                        if (ws != null)
                        {
                            ws.Cells[5, 2].Value = "NODO ENERGÉTICO - COES";
                            ws.Cells[6, 2].Value = "Desde:  " + fechaIni.ToString(Constantes.FormatoFecha) + " Hasta: " +
                                                   fechaFin.ToString(Constantes.FormatoFecha);
                            ws.Cells[7, 2].Value = titulo;

                            int index = 9;

                            ws.Cells[index, 2].Value = "EMPRESA";
                            ws.Cells[index, 3].Value = "GRUPO";
                            ws.Cells[index, 4].Value = "DIA";
                            ws.Cells[index, 5].Value = "HORA INDISP. PROGRAMADA";
                            ws.Cells[index, 6].Value = "HORA INDISP. FORTUITA";



                            int columnaMax = 6;


                            ExcelRange rg = ws.Cells[index, 2, index, columnaMax];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            index = index + 1;

                            foreach (NrProcesoDTO itemHora in listHoras)
                            {
                                ws.Cells[index, 2].Value = itemHora.Emprnomb;
                                ws.Cells[index, 3].Value = itemHora.Gruponomb;
                                ws.Cells[index, 4].Value =
                                    ((DateTime)itemHora.Nrprcfechainicio).ToString(Constantes.FormatoFecha);
                                ws.Cells[index, 5].Value = itemHora.Nrprchoraunidad;
                                ws.Cells[index, 6].Value = itemHora.Nrprchoracentral;

                                index++;
                            }

                            rg = ws.Cells[9, 2, index - 1, columnaMax];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            ws.Column(1).Width = 7;

                            rg = ws.Cells[9, 2, index, columnaMax];
                            rg.AutoFitColumns();

                            if (incluirLogo)
                            {
                                try
                                {
                                    HttpWebRequest request =
                                        (HttpWebRequest)
                                        System.Net.HttpWebRequest.Create(
                                            ConfigurationManager.AppSettings["LogoCoes"].ToString());

                                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                                    System.Drawing.Image img =
                                        System.Drawing.Image.FromStream(response.GetResponseStream());
                                    ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                    picture.From.Column = 1;
                                    picture.From.Row = 1;
                                    picture.To.Column = 2;
                                    picture.To.Row = 2;
                                    picture.SetSize(120, 60);
                                }
                                catch
                                {
                                }
                            }





                        }
                    }

                    #endregion

                    #region Sobrecosto
                    //hojas EDE
                    idx = 0;
                    foreach (List<NrProcesoDTO> listaSobrec in arrSobrecosto)
                    {
                        string titulo = arrSobrecostoTitulo[idx].ToString();
                        string abreviatura = arrSobrecostoAbreviatura[idx].ToString();
                        ExcelWorksheet ws =
                            xlPackage.Workbook.Worksheets.Add(abreviatura);

                        if (ws != null)
                        {
                            ws.Cells[5, 2].Value = "NODO ENERGÉTICO - COES";
                            ws.Cells[6, 2].Value = "Desde:  " + fechaIni.ToString(Constantes.FormatoFecha) + " Hasta: " +
                                                   fechaFin.ToString(Constantes.FormatoFecha);
                            ws.Cells[7, 2].Value = titulo;

                            int index = 9;

                            ws.Cells[index, 2].Value = "EMPRESA";
                            ws.Cells[index, 3].Value = "GRUPO";
                            ws.Cells[index, 4].Value = "FECHA";
                            ws.Cells[index, 5].Value = "SOBRECOSTO";
                            ws.Cells[index, 6].Value = "OBSERVACIÓN";


                            
                            int columnaMax = 6;


                            ExcelRange rg = ws.Cells[index, 2, index, columnaMax];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            index = index + 1;

                            foreach (NrProcesoDTO itemHora in listaSobrec)
                            {
                                ws.Cells[index, 2].Value = itemHora.Emprnomb;
                                ws.Cells[index, 3].Value = itemHora.Gruponomb;
                                ws.Cells[index, 4].Value =
                                    ((DateTime)itemHora.Nrprcfechainicio).ToString(Constantes.FormatoFecha);
                                ws.Cells[index, 5].Value = itemHora.Nrprcsobrecosto;
                                ws.Cells[index, 6].Value = itemHora.Nrprcobservacion;

                                index++;
                            }
                            

                            rg = ws.Cells[9, 2, index - 1, columnaMax];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            ws.Column(1).Width = 7;

                            rg = ws.Cells[9, 2, index, columnaMax];
                            rg.AutoFitColumns();

                            if (incluirLogo)
                            {
                                try
                                {
                                    HttpWebRequest request =
                                        (HttpWebRequest)
                                        System.Net.HttpWebRequest.Create(
                                            ConfigurationManager.AppSettings["LogoCoes"].ToString());

                                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                                    System.Drawing.Image img =
                                        System.Drawing.Image.FromStream(response.GetResponseStream());
                                    ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                    picture.From.Column = 1;
                                    picture.From.Row = 1;
                                    picture.To.Column = 2;
                                    picture.To.Row = 2;
                                    picture.SetSize(120, 60);
                                }
                                catch (Exception e)
                                {
                                }

                            }





                        }

                        //xlPackage.Save();
                        idx++;
                    }
                    #endregion


                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite generar el archivo excel con las interrupciones de un evento
        /// </summary>
        /// <param name="listIpp"></param>
        public static void GenerarArchivoReservaFria(List<MeMedicion96DTO> listEde, List<NrProcesoDTO> listhda,
            List<NrProcesoDTO> listhmpe, List<NrProcesoDTO> listhmce, DateTime fechaIni, DateTime fechaFin,
            bool incluirLogo)
        {

            ArrayList arrEdeMedida = new ArrayList();
            arrEdeMedida.Add(listEde);
           

            ArrayList arrEdeTitulo = new ArrayList();
            arrEdeTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloRfEde);
            
            ArrayList arrEdeAbreviatura = new ArrayList();
            arrEdeAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaRfEde);
            

            ArrayList arrHora = new ArrayList();
            arrHora.Add(listhda);
            arrHora.Add(listhmpe);
            arrHora.Add(listhmce);

            ArrayList arrHoraTitulo = new ArrayList();
            arrHoraTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloRfHda);
            arrHoraTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloRfHmpe);
            arrHoraTitulo.Add(ConstanteReservaFriaNodoEnergetico.TituloRfHmce);

            ArrayList arrHoraAbreviatura = new ArrayList();
            arrHoraAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaRfHda);
            arrHoraAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaRfHmpe);
            arrHoraAbreviatura.Add(ConstanteReservaFriaNodoEnergetico.AbreviaturaRfHmce);


            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaInformeReservaNodo +
                              NombreArchivo.ReporteReservaFria;


                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    #region EDE

                    //hojas EDE
                    int idx = 0;
                    foreach (List<MeMedicion96DTO> listaEDE in arrEdeMedida)
                    {
                        string titulo = arrEdeTitulo[idx].ToString();
                        string abreviatura = arrEdeAbreviatura[idx].ToString();
                        ExcelWorksheet ws =
                            xlPackage.Workbook.Worksheets.Add(abreviatura);

                        if (ws != null)
                        {
                            ws.Cells[5, 2].Value = "RESERVA FRÍA - COES";
                            ws.Cells[6, 2].Value = "Desde:  " + fechaIni.ToString(Constantes.FormatoFecha) + " Hasta: " +
                                                   fechaFin.ToString(Constantes.FormatoFecha);
                            ws.Cells[7, 2].Value = titulo;

                            int index = 9;

                            ws.Cells[index, 2].Value = "EMPRESA";
                            ws.Cells[index, 3].Value = "EQUIPO";
                            ws.Cells[index, 4].Value = "FECHA";
                            ws.Cells[index, 5].Value = "TOTAL";

                            int col = 6;
                            DateTime fechaCabecera = Convert.ToDateTime("2017-01-01");
                            for (int i = 0; i < 96; i++)
                            {
                                fechaCabecera = fechaCabecera.AddMinutes(15);
                                ws.Cells[index, col].Value = fechaCabecera.ToString(Constantes.FormatoHoraMinuto);
                                col++;
                            }

                            int columnaMax = 101;


                            ExcelRange rg = ws.Cells[index, 2, index, columnaMax];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            index = index + 1;

                            foreach (MeMedicion96DTO item in listaEDE)
                            {
                                ws.Cells[index, 2].Value = item.Emprnomb;
                                ws.Cells[index, 3].Value = item.Ptomedielenomb;
                                ws.Cells[index, 4].Value = ((DateTime) item.Medifecha).ToString(Constantes.FormatoFecha);
                                ws.Cells[index, 5].Value = item.Meditotal;

                                col = 6;

                                for (int i = 0; i < 96; i++)
                                {

                                    ws.Cells[index, col].Value =
                                        (decimal)
                                        item.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(item, null);

                                    col++;
                                }


                                rg = ws.Cells[index, 2, index, columnaMax];
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                index++;
                            }

                            rg = ws.Cells[9, 2, index - 1, columnaMax];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            ws.Column(1).Width = 7;

                            rg = ws.Cells[9, 2, index, columnaMax];
                            rg.AutoFitColumns();

                            if (incluirLogo)
                            {
                                try
                                {


                                    HttpWebRequest request =
                                        (HttpWebRequest)
                                        System.Net.HttpWebRequest.Create(
                                            ConfigurationManager.AppSettings["LogoCoes"].ToString());

                                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                                    System.Drawing.Image img =
                                        System.Drawing.Image.FromStream(response.GetResponseStream());
                                    ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                    picture.From.Column = 1;
                                    picture.From.Row = 1;
                                    picture.To.Column = 2;
                                    picture.To.Row = 2;
                                    picture.SetSize(120, 60);
                                }
                                catch (Exception e)
                                {

                                }
                            }





                        }

                        //xlPackage.Save();
                        idx++;
                    }

                    #endregion

                    #region Horas

                    //hojas EDE
                    idx = 0;
                    foreach (List<NrProcesoDTO> listaSobrec in arrHora)
                    {
                        string titulo = arrHoraTitulo[idx].ToString();
                        string abreviatura = arrHoraAbreviatura[idx].ToString();
                        ExcelWorksheet ws =
                            xlPackage.Workbook.Worksheets.Add(abreviatura);

                        if (ws != null)
                        {
                            ws.Cells[5, 2].Value = "RESERVA FRÍA - COES";
                            ws.Cells[6, 2].Value = "Desde:  " + fechaIni.ToString(Constantes.FormatoFecha) + " Hasta: " +
                                                   fechaFin.ToString(Constantes.FormatoFecha);
                            ws.Cells[7, 2].Value = titulo;

                            int index = 9;

                            ws.Cells[index, 2].Value = "EMPRESA";
                            ws.Cells[index, 3].Value = "GRUPO";
                            ws.Cells[index, 4].Value = "FECHA";
                            ws.Cells[index, 5].Value = "HORAS CENTRAS";
                            ws.Cells[index, 6].Value = "OBSERVACIÓN";



                            int columnaMax = 6;


                            ExcelRange rg = ws.Cells[index, 2, index, columnaMax];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            index = index + 1;

                            foreach (NrProcesoDTO itemHora in listaSobrec)
                            {
                                ws.Cells[index, 2].Value = itemHora.Emprnomb;
                                ws.Cells[index, 3].Value = itemHora.Gruponomb;
                                ws.Cells[index, 4].Value =
                                    ((DateTime) itemHora.Nrprcfechainicio).ToString(Constantes.FormatoFecha);
                                ws.Cells[index, 5].Value = itemHora.Nrprchoracentral;
                                ws.Cells[index, 6].Value = itemHora.Nrprcobservacion;

                                index++;
                            }


                            rg = ws.Cells[9, 2, index - 1, columnaMax];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            ws.Column(1).Width = 7;

                            rg = ws.Cells[9, 2, index, columnaMax];
                            rg.AutoFitColumns();

                            if (incluirLogo)
                            {
                                try
                                {


                                    HttpWebRequest request =
                                        (HttpWebRequest)
                                        System.Net.HttpWebRequest.Create(
                                            ConfigurationManager.AppSettings["LogoCoes"].ToString());

                                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                                    System.Drawing.Image img =
                                        System.Drawing.Image.FromStream(response.GetResponseStream());
                                    ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                                    picture.From.Column = 1;
                                    picture.From.Row = 1;
                                    picture.To.Column = 2;
                                    picture.To.Row = 2;
                                    picture.SetSize(120, 60);
                                }
                                catch (Exception e)
                                {
                                    
                                }

                            }





                        }

                        //xlPackage.Save();
                        idx++;
                    }

                    #endregion


                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



    }
}