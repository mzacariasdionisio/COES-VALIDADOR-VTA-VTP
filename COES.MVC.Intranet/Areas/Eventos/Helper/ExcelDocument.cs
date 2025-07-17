using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Eventos.Models;
using System.Net;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.MVC.Intranet.Areas.Eventos.Helper;
using Microsoft.Office.Interop.Excel;

namespace COES.MVC.Intranet.Areas.Evento.Helper
{
    public class ExcelDocument
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarArchivoRSF(List<ReservaModel> list, DateTime fecha)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo template = new FileInfo(ruta + Constantes.PlantillaExcel);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteRSF);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteRSF);
            }



            int index = 6;
            int row = 4;
            int column = 6;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                ws.Cells[2, 3].Value = fecha.ToString(Constantes.FormatoFecha);

                if (list.Count > 0)
                {
                    foreach (ReservaItemModel item in list[0].ListItems)
                    {
                        ws.Cells[row, column].Value = item.HoraInicio.ToString("HH:mm:ss") + "-" + item.HoraFin.ToString("HH:mm:ss");
                        column = column + 4;
                    }

                    column = index;
                    row++;

                    foreach (ReservaItemModel item in list[0].ListItems)
                    {
                        ws.Cells[row, column].Value = "MAN";
                        column += 2;
                        ws.Cells[row, column].Value = "AUTO";
                        column += 2;
                    }

                    row++;

                    foreach (ReservaModel item in list)
                    {
                        column = 2;

                        ws.Cells[row, column].Value = item.DesURS;
                        ws.Cells[row, column + 1].Value = item.Empresa;
                        ws.Cells[row, column + 2].Value = item.Central;
                        ws.Cells[row, column + 3].Value = item.Equipo;

                        column = column + 4;
                        foreach (ReservaItemModel child in item.ListItems)
                        {
                            ws.Cells[row, column++].Value = child.Manual.ToString();
                            ws.Cells[row, column++].Value = (child.IndManual == Constantes.SI) ? Constantes.TextoSI : string.Empty;
                            ws.Cells[row, column++].Value = child.Automatico.ToString();
                            ws.Cells[row, column++].Value = (child.IndAutomatico == Constantes.SI) ? Constantes.TextoSI : string.Empty;
                        }
                        row++;
                    }

                    for (int k = column; k <= 69; k++)
                    {
                        ws.Column(k).Hidden = true;
                    }

                    for (int k = row; k <= 68; k++)
                    {
                        ws.Row(k).Hidden = true;
                    }

                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el formato con los datos cada 30 minutos
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fecha"></param>
        public static void GenerarArchivoRSF30(List<ReservaModel> list, DateTime fecha)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo template = new FileInfo(ruta + Constantes.PlantillaExcelRSF30);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteRSF30);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteRSF30);
            }

            //iniciamos logica de generación de archivo excel   

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                ws.Cells[2, 3].Value = fecha.ToString(Constantes.FormatoFecha);

                if (list.Count > 0)
                {
                    List<HoraExcel> resultadoAutomatico = new List<HoraExcel>();
                    List<HoraExcel> resultadoManual = new List<HoraExcel>();
                    List<HoraExcel> horaExcel = new List<HoraExcel>();

                    #region Obtención de datos

                    List<int> ids = (from modelo in list select modelo.IdEquipo).Distinct().ToList();

                    foreach (int id in ids)
                    {
                        ReservaModel item = list.Where(x => x.IdEquipo == id).First();

                        List<ReservaItemModel> listItemsAutomatico = item.ListItems.Where(x => x.Automatico > 0).ToList();
                        List<ReservaItemModel> listItemsManual = item.ListItems.Where(x => x.Manual > 0).ToList();

                        foreach (ReservaItemModel child in listItemsAutomatico)
                        {
                            int horaInicio = child.HoraInicio.Hour;
                            int minutoInicio = child.HoraInicio.Minute;
                            int horaFin = child.HoraFin.Hour;
                            int minutoFin = child.HoraFin.Minute;

                            for (int i = horaInicio; i <= horaFin; i++)
                            {
                                if (i == horaInicio || i == horaFin)
                                {
                                    if (i == horaInicio)
                                    {
                                        if (i != 0 || minutoInicio != 0)
                                            resultadoAutomatico.Add(new HoraExcel { Hora = i, Minuto = minutoInicio, Valor = child.Automatico, IdEquipo = id });

                                        if (minutoInicio == 0 && horaInicio != horaFin)
                                        {
                                            resultadoAutomatico.Add(new HoraExcel { Hora = i, Minuto = 30, Valor = child.Automatico, IdEquipo = id });
                                        }
                                    }
                                    if (i == horaFin)
                                    {
                                        if (horaInicio != horaFin)
                                        {
                                            resultadoAutomatico.Add(new HoraExcel { Hora = i, Minuto = 0, Valor = child.Automatico, IdEquipo = id });
                                        }

                                        if (minutoFin != 0)
                                        {
                                            if (minutoFin > 30 && horaInicio != horaFin)
                                            {
                                                resultadoAutomatico.Add(new HoraExcel { Hora = i, Minuto = 30, Valor = child.Automatico, IdEquipo = id });
                                            }

                                            resultadoAutomatico.Add(new HoraExcel { Hora = i, Minuto = minutoFin, Valor = child.Automatico, IdEquipo = id });
                                        }
                                    }
                                }
                                else
                                {
                                    resultadoAutomatico.Add(new HoraExcel { Hora = i, Minuto = 0, Valor = child.Automatico, IdEquipo = id });
                                    resultadoAutomatico.Add(new HoraExcel { Hora = i, Minuto = 30, Valor = child.Automatico, IdEquipo = id });
                                }
                            }
                        }

                        foreach (ReservaItemModel child in listItemsManual)
                        {
                            int horaInicio = child.HoraInicio.Hour;
                            int minutoInicio = child.HoraInicio.Minute;
                            int horaFin = child.HoraFin.Hour;
                            int minutoFin = child.HoraFin.Minute;

                            for (int i = horaInicio; i <= horaFin; i++)
                            {
                                if (i == horaInicio || i == horaFin)
                                {
                                    if (i == horaInicio)
                                    {
                                        if (i != 0 || minutoInicio != 0)
                                            resultadoManual.Add(new HoraExcel { Hora = i, Minuto = minutoInicio, Valor = child.Manual, IdEquipo = id });

                                        if (minutoInicio == 0 && horaInicio != horaFin)
                                        {
                                            resultadoManual.Add(new HoraExcel { Hora = i, Minuto = 30, Valor = child.Manual, IdEquipo = id });
                                        }
                                    }
                                    if (i == horaFin)
                                    {
                                        if (horaInicio != horaFin)
                                        {
                                            resultadoManual.Add(new HoraExcel { Hora = i, Minuto = 0, Valor = child.Manual, IdEquipo = id });
                                        }
                                        if (minutoFin != 0)
                                        {
                                            if (minutoFin > 30 && horaInicio != horaFin)
                                            {
                                                resultadoManual.Add(new HoraExcel { Hora = i, Minuto = 30, Valor = child.Manual, IdEquipo = id });
                                            }

                                            resultadoManual.Add(new HoraExcel { Hora = i, Minuto = minutoFin, Valor = child.Manual, IdEquipo = id });
                                        }
                                    }
                                }
                                else
                                {
                                    resultadoManual.Add(new HoraExcel { Hora = i, Minuto = 0, Valor = child.Manual, IdEquipo = id });
                                    resultadoManual.Add(new HoraExcel { Hora = i, Minuto = 30, Valor = child.Manual, IdEquipo = id });
                                }
                            }
                        }
                    }

                    List<HoraExcel> listAutomatico = (from itemAuto in resultadoAutomatico
                                                      orderby itemAuto.Hora, itemAuto.Minuto
                                                      select new HoraExcel { Hora = itemAuto.Hora, Minuto = itemAuto.Minuto }).Distinct().ToList();

                    List<HoraExcel> listManual = (from itemManual in resultadoManual
                                                  orderby itemManual.Hora, itemManual.Minuto
                                                  select new HoraExcel { Hora = itemManual.Hora, Minuto = itemManual.Minuto }).Distinct().ToList();


                    foreach (HoraExcel item in listAutomatico)
                    {
                        if (horaExcel.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto).Count() == 0)
                        {
                            horaExcel.Add(new HoraExcel { Hora = item.Hora, Minuto = item.Minuto });
                        }
                    }

                    foreach (HoraExcel item in listManual)
                    {
                        if (horaExcel.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto).Count() == 0)
                        {
                            horaExcel.Add(new HoraExcel { Hora = item.Hora, Minuto = item.Minuto });
                        }
                    }

                    List<HoraExcel> listHora = (from hora in horaExcel orderby hora.Hora, hora.Minuto select hora).ToList();

                    #endregion

                    List<int> idsAutomatico = (from registro in resultadoAutomatico select registro.IdEquipo).Distinct().ToList();
                    List<int> idsManual = (from registro in resultadoManual select registro.IdEquipo).Distinct().ToList();

                    int indice = 0;
                    int column = 2;
                    int row = 8;

                    foreach (HoraExcel item in listHora)
                    {
                        ws.Cells[row, column].Value = item.Hora.ToString().PadLeft(2, '0') + ":" + item.Minuto.ToString().PadLeft(2, '0');
                        ws.Cells[row, column].StyleID = ws.Cells[8, 2].StyleID;
                        row++;
                    }

                    column++;
                    foreach (int id in idsAutomatico)
                    {
                        ReservaModel itemAutomatico = list.Where(x => x.IdEquipo == id).First();
                        ws.Cells[3, column].Value = "AUT";
                        ws.Cells[4, column].Value = itemAutomatico.DesURS;
                        ws.Cells[5, column].Value = itemAutomatico.Empresa;
                        ws.Cells[6, column].Value = itemAutomatico.Central;
                        ws.Cells[7, column].Value = itemAutomatico.Equipo;

                        ws.Cells[4, column].StyleID = ws.Cells[4, 2].StyleID;
                        ws.Cells[5, column].StyleID = ws.Cells[4, 2].StyleID;
                        ws.Cells[6, column].StyleID = ws.Cells[4, 2].StyleID;
                        ws.Cells[7, column].StyleID = ws.Cells[4, 2].StyleID;

                        row = 8;
                        foreach (HoraExcel item in listHora)
                        {
                            HoraExcel child = resultadoAutomatico.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto && x.IdEquipo == id).FirstOrDefault();
                            if (child != null)
                            {
                                ws.Cells[row, column].Value = child.Valor.ToString();

                            }
                            ws.Cells[row, column].StyleID = ws.Cells[8, 2].StyleID;
                            row++;
                        }

                        column++;
                    }

                    foreach (int id in idsManual)
                    {
                        ReservaModel itemManual = list.Where(x => x.IdEquipo == id).First();
                        ws.Cells[3, column].Value = "MAN";
                        ws.Cells[4, column].Value = itemManual.DesURS;
                        ws.Cells[5, column].Value = itemManual.Empresa;
                        ws.Cells[6, column].Value = itemManual.Central;
                        ws.Cells[7, column].Value = itemManual.Equipo;

                        ws.Cells[4, column].StyleID = ws.Cells[4, 2].StyleID;
                        ws.Cells[5, column].StyleID = ws.Cells[4, 2].StyleID;
                        ws.Cells[6, column].StyleID = ws.Cells[4, 2].StyleID;
                        ws.Cells[7, column].StyleID = ws.Cells[4, 2].StyleID;

                        row = 8;
                        foreach (HoraExcel item in listHora)
                        {
                            HoraExcel child = resultadoManual.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto && x.IdEquipo == id).FirstOrDefault();
                            if (child != null)
                            {
                                ws.Cells[row, column].Value = child.Valor.ToString();

                            }
                            ws.Cells[row, column].StyleID = ws.Cells[8, 2].StyleID;
                            row++;
                        }
                        column++;
                    }

                    indice++;

                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el formato de datos total
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public static void GenerarArchivoTotal(List<IeodCuadroDTO> list, string fechaInicio, string fechaFin)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo template = new FileInfo(ruta + Constantes.PlantillaExcelReporteRSF);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteRSFGeneral);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteRSFGeneral);
            }

            int row = 5;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                ws.Cells[2, 3].Value = fechaInicio;
                ws.Cells[2, 5].Value = fechaFin;

                if (list.Count > 0)
                {
                    foreach (IeodCuadroDTO item in list)
                    {
                        string[] hora = item.HORA.Split('-');
                        ws.Cells[row, column].Value = item.FECHA;
                        ws.Cells[row, column + 1].Value = hora[0];
                        ws.Cells[row, column + 2].Value = hora[1];
                        ws.Cells[row, column + 3].Value = item.RUS;
                        ws.Cells[row, column + 4].Value = item.ICVALOR1.ToString(Constantes.FormatoNumero);
                        ws.Cells[row, column + 5].Value = item.TIPO;

                        if (row > 5)
                        {
                            ws.Cells[row, column].StyleID = ws.Cells[5, column].StyleID;
                            ws.Cells[row, column + 1].StyleID = ws.Cells[5, column].StyleID;
                            ws.Cells[row, column + 2].StyleID = ws.Cells[5, column].StyleID;
                            ws.Cells[row, column + 3].StyleID = ws.Cells[5, column].StyleID;
                            ws.Cells[row, column + 4].StyleID = ws.Cells[5, column].StyleID;
                            ws.Cells[row, column + 5].StyleID = ws.Cells[5, column].StyleID;
                        }

                        row++;
                    }
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el reporte en excel
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteEvento(List<EventoDTO> list, DateTime fechaDesde, DateTime fechaHasta, int indicador)
        {
            try
            {
                string file = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteEvento;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("EVENTOS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "CONSULTA DE EVENTOS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[5, 2].Value = "FECHA DESDE: ";
                        ws.Cells[5, 3].Value = fechaDesde.ToString("dd/MM/yyyy");
                        ws.Cells[6, 2].Value = "FECHA HASTA: ";
                        ws.Cells[6, 3].Value = fechaHasta.ToString("dd/MM/yyyy");

                        rg = ws.Cells[5, 2, 6, 2];
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));

                        ws.Cells[8, 2].Value = "Empresa";
                        ws.Cells[8, 3].Value = "Ubicación";
                        ws.Cells[8, 4].Value = "Equipo";
                        ws.Cells[8, 5].Value = "Familia";
                        ws.Cells[8, 6].Value = "Inicio";
                        ws.Cells[8, 7].Value = "Final";
                        ws.Cells[8, 8].Value = "Reportó";
                        ws.Cells[8, 9].Value = "Descripción";
                        ws.Cells[8, 10].Value = "MW Indisp.";
                        ws.Cells[8, 11].Value = "Interrupc.";
                        ws.Cells[8, 12].Value = "Desconecta generación";
                        ws.Cells[8, 13].Value = "Generación desconectada";
                        ws.Cells[8, 14].Value = "Tipo";
                        ws.Cells[8, 15].Value = "A.Operac.";
                        ws.Cells[8, 16].Value = "N.Tensión";
                        ws.Cells[8, 17].Value = "Causa CIER";
                        ws.Cells[8, 18].Value = "Energía no Servida (MWh)";
                        ws.Cells[8, 19].Value = "MW Interrumpidos";


                        int lastcolumn = 19;

                        if (indicador == 0)
                        {
                            ws.Cells[8, 20].Value = "Detalle";
                            lastcolumn = 20;
                        }
                        else if (indicador == 1)
                        {
                            ws.Cells[8, 20].Value = "Pto Interrupción";
                            ws.Cells[8, 21].Value = "Cliente";
                            ws.Cells[8, 22].Value = "Suministro";
                            ws.Cells[8, 23].Value = "MW Interr.";
                            ws.Cells[8, 24].Value = "Min Interr.";
                            ws.Cells[8, 25].Value = "Observaciones";
                            ws.Cells[8, 26].Value = "Bajo Carga";
                            ws.Cells[8, 27].Value = "racMF";
                            ws.Cells[8, 28].Value = "etapaMF";
                            ws.Cells[8, 29].Value = "Tipo Falla";
                            ws.Cells[8, 30].Value = "Fase";
                            ws.Cells[8, 31].Value = "Detalle";

                            lastcolumn = 31;
                        }

                        rg = ws.Cells[8, 2, 8, lastcolumn];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 9;
                        foreach (EventoDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.EMPRNOMB;
                            ws.Cells[index, 3].Value = item.TAREAABREV + " " + item.AREANOMB;
                            ws.Cells[index, 4].Value = item.EQUIABREV;
                            ws.Cells[index, 5].Value = item.FAMABREV;
                            ws.Cells[index, 6].Value = (item.EVENINI != null) ? ((DateTime)item.EVENINI).ToString("dd/MM/yyyy HH:mm") : string.Empty;
                            ws.Cells[index, 7].Value = (item.EVENFIN != null) ? ((DateTime)item.EVENFIN).ToString("dd/MM/yyyy HH:mm") : string.Empty;
                            ws.Cells[index, 6].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                            ws.Cells[index, 7].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                            ws.Cells[index, 8].Value = item.EMPRABREV;
                            ws.Cells[index, 9].Value = item.EVENASUNTO;
                            ws.Cells[index, 10].Value = item.EVENMWINDISP;
                            ws.Cells[index, 11].Value = item.EVENINTERRUP;
                            ws.Cells[index, 12].Value = item.EVENGENDESCON;
                            ws.Cells[index, 13].Value = item.EVENMWGENDESCON;
                            ws.Cells[index, 14].Value = item.TIPOEVENABREV;
                            ws.Cells[index, 15].Value = item.TIPOEMPRNOMB;
                            ws.Cells[index, 16].Value = item.EQUITENSION;
                            ws.Cells[index, 17].Value = item.CAUSAEVENABREV;
                            ws.Cells[index, 18].Value = item.ENERGIAINTERRUMPIDA;
                            ws.Cells[index, 19].Value = item.MWINTERRUMPIDOS;

                            if (indicador == 0)
                            {
                                ws.Cells[index, 20].Value = item.EVENDESC;
                                ws.Cells[index, 20].Style.WrapText = true;
                            }
                            else if (indicador == 1)
                            {
                                ws.Cells[index, 20].Value = item.Ptointerrupnomb;
                                ws.Cells[index, 21].Value = item.Clientenomb;
                                ws.Cells[index, 22].Value = item.Ptoentrenomb;
                                ws.Cells[index, 23].Value = item.Interrmw;
                                ws.Cells[index, 24].Value = item.Interrminu;
                                ws.Cells[index, 25].Value = item.Interrdesc;
                                ws.Cells[index, 26].Value = item.Interrnivel;
                                ws.Cells[index, 27].Value = item.Interrracmf;
                                ws.Cells[index, 28].Value = item.Interrmfetapadesc;
                                ws.Cells[index, 29].Value = item.Eventipofalla;
                                ws.Cells[index, 30].Value = item.Eventipofallafase;
                                ws.Cells[index, 31].Value = item.EVENDESC;
                                ws.Cells[index, 31].Style.WrapText = true;
                            }


                            rg = ws.Cells[index, 1, index, lastcolumn];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[8, 1, index - 1, lastcolumn];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[7, 3, index, lastcolumn - 1];
                        rg.AutoFitColumns();

                        if (indicador == 0)
                        {
                            ws.Column(20).Width = 160;
                        }
                        else if (indicador == 1)
                        {
                            ws.Column(31).Width = 160;
                        }

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 2;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);

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
        /// Permite generar el archivo excel con las interrupciones de un evento
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarArchivoInterrupcion(List<EveInterrupcionDTO> list)
        {
            try
            {
                string file = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteInterrupciones;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("INTERRUPCIONES");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "INTERRUPCIONES";

                        int index = 5;      
                        
                        ws.Cells[index, 2].Value = "Pto Interrupción";
                        ws.Cells[index, 3].Value = "Cliente";
                        ws.Cells[index, 4].Value = "Pto Entrega DESC.";
                        ws.Cells[index, 5].Value = "MW";
                        ws.Cells[index, 6].Value = "Minutos";
                        ws.Cells[index, 7].Value = "Observ.";
                        ws.Cells[index, 8].Value = "Pto entrega SSEE";
                        ws.Cells[index, 9].Value = "Barra entrega";
                        ws.Cells[index, 10].Value = "Tens.";
                        ws.Cells[index, 11].Value = "Bajo Carga";
                        ws.Cells[index, 12].Value = "racMF";
                        ws.Cells[index, 13].Value = "etapaMF";
                        ws.Cells[index, 14].Value = "RM";
                        ws.Cells[index, 15].Value = "Última actualización";
                        ws.Cells[index, 16].Value = "Fecha act.";
                         
                        ExcelRange rg = ws.Cells[index, 2, index, 16];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = index + 1;

                        foreach (EveInterrupcionDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.PtoInterrupNomb;
                            ws.Cells[index, 3].Value = item.EmprNomb;
                            ws.Cells[index, 4].Value = item.PtoEntreNomb;
                            ws.Cells[index, 5].Value = item.Interrmw;
                            ws.Cells[index, 6].Value = item.Interrminu;
                            ws.Cells[index, 7].Value = item.Interrdesc;
                            ws.Cells[index, 8].Value = item.AreaNomb;
                            ws.Cells[index, 9].Value = item.EquiAbrev;
                            ws.Cells[index, 10].Value = item.EquiTension.ToString("#,###.00") + "kV";
                            ws.Cells[index, 11].Value = (item.Interrnivel == "S") ? "Si" : "No";
                            ws.Cells[index, 12].Value = (item.Interrracmf == "S") ? "Si" : "No";
                            ws.Cells[index, 13].Value = item.Interrmfetapa;
                            ws.Cells[index, 14].Value = (item.Interrmanualr == "S") ? "Si" : "No";
                            ws.Cells[index, 15].Value = item.Lastuser;
                            ws.Cells[index, 16].Value = (item.Lastdate != null) ? ((DateTime)item.Lastdate).ToString("dd/MM/yyyy") : string.Empty;

                            rg = ws.Cells[index, 2, index, 16];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[6, 2, index - 1, 16];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 16];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el archivo excel con las interrupciones de un evento
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarArchivoOperacionesVarias(List<EveIeodcuadroDTO> list, string fechaIni, string fechaFin, string clase, int subcausacodi, string nombreOperacion)
        {
            try
            {
                //string file = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaInformeEvento] + NombreArchivo.ReporteOperaciones;
                string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento + NombreArchivo.ReporteOperaciones;


                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("OPERACIONES");

                    if (ws != null)
                    {
                        //ws.Cells[0, 0].Value = "COES-SINAC";
                        ws.Cells[5, 2].Value = "Operaciones varias del COES - " + clase;
                        ws.Cells[6, 2].Value = "Desde:  " + fechaIni + " Hasta: " + fechaFin;
                        ws.Cells[8, 2].Value = nombreOperacion;

                        int index = 9;
                        int columnaMax = 7;

                        ws.Cells[index, 2].Value = "FECHA";

                        if (subcausacodi == 206)
                        {
                            ws.Cells[index, 3].Value = "AISLADO SIN CARGA";
                            ws.Cells[index, 4].Value = "INICIO";
                            ws.Cells[index, 5].Value = "FINAL";
                            ws.Cells[index, 6].Value = "EMPRESA";
                            ws.Cells[index, 7].Value = "UBICACIÓN";
                            ws.Cells[index, 8].Value = "EQUIPO";

                            columnaMax = 8;
                        }
                        else
                        {
                            ws.Cells[index, 2].Value = "FECHA";
                            ws.Cells[index, 3].Value = "INICIO";
                            ws.Cells[index, 4].Value = "FINAL";
                            ws.Cells[index, 5].Value = "EMPRESA";
                            ws.Cells[index, 6].Value = "UBICACIÓN";
                            ws.Cells[index, 7].Value = "EQUIPO";
                        }

                        switch (subcausacodi)
                        {
                            case 201: //CONGESTIÓN
                                ws.Cells[index, 8].Value = "UNIDADES GENERADORAS LIMITADAS";
                                ws.Cells[index, 9].Value = "OBSERVACIONES";
                                columnaMax = 9;
                                break;
                            case 202: //OPERACIÓN DE CALDEROS
                                ws.Cells[index, 8].Value = "DESCRIPCIÓN";
                                columnaMax = 8;
                                break;
                            case 203: //REGULACION DE TENSIÓN
                                ws.Cells[index, 8].Value = "MOTIVO";
                                columnaMax = 8;
                                break;
                            case 314: //ENERGÍA DEJADA DE INYECTAR RER
                                ws.Cells[index, 8].Value = "MOTIVO";
                                columnaMax = 8;
                                break;
                            case 205: //RESTRICCIONES OPERATIVAS
                                ws.Cells[index, 8].Value = "DESCRIPCIÓN";
                                columnaMax = 8;
                                break;
                            case 208: //POR PRUEBAS (no térmoelectrico)
                                ws.Cells[index, 8].Value = "DESCRIPCIÓN";
                                columnaMax = 8;
                                break;
                            case 204: //REGULACIÓN PRIMARIA DE FRECUENCIA
                            case 209: //REGULACIÓN SECUNDARIA DE FRECUENCIA
                                ws.Cells[index, 8].Value = "POTENCIA ASIGNADA";
                                ws.Cells[index, 9].Value = "AISLADO?";
                                columnaMax = 9;
                                break;
                            case 206: //SISTEMAS AISLADOS
                                ws.Cells[index, 9].Value = "OPERACIÓN DE CENTRALES";
                                ws.Cells[index, 10].Value = "MOTIVO";
                                ws.Cells[index, 11].Value = "SUBSISTEMA AISLADO";
                                columnaMax = 11;
                                break;
                            case 207: //VERTIMIENTO DE EMBALSES Y PRESAS
                                ws.Cells[index, 8].Value = "CAUDAL PROMEDIO (M3/S)";
                                columnaMax = 8;
                                break;
                            case 210: //VENTEO DE GAS
                                ws.Cells[index, 8].Value = "MMPCD";
                                columnaMax = 8;
                                break;
                            case 218: //DESCARGA DE LAGUNAS
                                ws.Cells[index, 8].Value = "VALOR CAUDAL (M3/S)";
                                columnaMax = 8;
                                break;

                        }



                        ExcelRange rg = ws.Cells[index, 2, index, columnaMax];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = index + 1;

                        foreach (EveIeodcuadroDTO item in list)
                        {

                            ws.Cells[index, 2].Value = ((DateTime)item.Ichorini).ToString(Constantes.FormatoFecha);

                            if (subcausacodi == 206)
                            {
                                ws.Cells[index, 3].Value = (item.Ichorinicarga != null ? ((DateTime)item.Ichorinicarga).ToString(Constantes.FormatoHora) : "");
                                ws.Cells[index, 4].Value = ((DateTime)item.Ichorini).ToString(Constantes.FormatoHora);

                                if (((DateTime)item.Ichorini).Day != ((DateTime)item.Ichorfin).Day)
                                {
                                    ws.Cells[index, 5].Value = "24:00";
                                }
                                else
                                {
                                    ws.Cells[index, 5].Value = ((DateTime)item.Ichorfin).ToString(Constantes.FormatoHora);
                                }
                                ws.Cells[index, 6].Value = item.Emprnomb;
                                ws.Cells[index, 7].Value = item.Areanomb;
                                ws.Cells[index, 8].Value = item.Equiabrev;
                            }
                            else
                            {
                                ws.Cells[index, 3].Value = ((DateTime)item.Ichorini).ToString(Constantes.FormatoHora);

                                if (((DateTime)item.Ichorini).Day != ((DateTime)item.Ichorfin).Day)
                                {
                                    ws.Cells[index, 4].Value = "24:00";
                                }
                                else
                                {
                                    ws.Cells[index, 4].Value = ((DateTime)item.Ichorfin).ToString(Constantes.FormatoHora);
                                }
                                ws.Cells[index, 5].Value = item.Emprnomb;
                                ws.Cells[index, 6].Value = item.Areanomb;
                                ws.Cells[index, 7].Value = item.Equiabrev;
                            }


                            switch (subcausacodi)
                            {
                                case 201: //CONGESTIÓN
                                    ws.Cells[index, 8].Value = item.Icdescrip1;
                                    ws.Cells[index, 9].Value = item.Icdescrip2;
                                    break;
                                case 202: //OPERACIÓN DE CALDEROS
                                    ws.Cells[index, 8].Value = item.Icdescrip1;
                                    break;
                                case 203: //REGULACION DE TENSIÓN
                                    ws.Cells[index, 8].Value = item.Icdescrip1;
                                    break;
                                case 314: //ENERGÍA DEJADA DE INYECTAR RER
                                    ws.Cells[index, 8].Value = item.Icdescrip1;
                                    break;
                                case 205: //RESTRICCIONES OPERATIVAS
                                    ws.Cells[index, 8].Value = item.Icdescrip1;
                                    break;
                                case 208: //POR PRUEBAS (no térmoelectrico)
                                    ws.Cells[index, 8].Value = item.Icdescrip1;
                                    break;
                                case 204: //REGULACIÓN PRIMARIA DE FRECUENCIA
                                case 209: //REGULACIÓN SECUNDARIA DE FRECUENCIA
                                    ws.Cells[index, 8].Value = item.Icvalor1;
                                    ws.Cells[index, 9].Value = (item.Iccheck1 == "S") ? "Si" : "No"; ;
                                    break;
                                case 206: //SISTEMAS AISLADOS
                                    ws.Cells[index, 9].Value = item.Icdescrip1;
                                    ws.Cells[index, 10].Value = item.Icdescrip2;
                                    ws.Cells[index, 11].Value = item.Icdescrip3;
                                    break;
                                case 207: //VERTIMIENTO DE EMBALSES Y PRESAS
                                    ws.Cells[index, 8].Value = item.Icvalor1;
                                    break;
                                case 210: //VENTEO DE GAS
                                    ws.Cells[index, 8].Value = item.Icvalor1;
                                    break;
                                case 218: //DESCARGA DE LAGUNAS
                                    ws.Cells[index, 8].Value = item.Icvalor1;                                    
                                    break;
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

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    if (subcausacodi == 206)
                    {
                        ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("CUADRO 1");
                        if (ws2 != null)
                        {
                            ws2.Cells[5, 2].Value = "Operaciones varias del COES - " + clase;
                            ws2.Cells[6, 2].Value = "Desde:  " + fechaIni + " Hasta: " + fechaFin;
                            ws2.Cells[8, 2].Value = "CUADRO N° 1. SISTEMAS AISLADOS EN CUMPLIMIENTO DEL LITERAL j) DEL NUMERAL 5.1.4 DE LA BMNTCSE";

                            int index = 9;
                            int columnaMax = 7;

                            ws2.Cells[index, 2].Value = "FECHA";

                           
                            ws2.Cells[index, 3].Value = "INICIO";
                            ws2.Cells[index, 4].Value = "FINAL";
                            ws2.Cells[index, 5].Value = "EMPRESA";
                            ws2.Cells[index, 6].Value = "UBICACIÓN";
                            ws2.Cells[index, 7].Value = "EQUIPO";
                            ws2.Cells[index, 8].Value = "MOTIVO";
                            ws2.Cells[index, 9].Value = "SUBSISTEMA AISLADO";
                            
                            columnaMax = 9;
                          
                            ExcelRange rg = ws2.Cells[index, 2, index, columnaMax];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            index = index + 1;

                            List<EveIeodcuadroDTO> listcuadro1 = list.Where(x => x.Ictipcuadro == 1).ToList();

                            foreach (EveIeodcuadroDTO item in listcuadro1)
                            {

                                ws2.Cells[index, 2].Value = ((DateTime)item.Ichorini).ToString(Constantes.FormatoFecha);

                                
                                ws2.Cells[index, 3].Value = ((DateTime)item.Ichorini).ToString(Constantes.FormatoHora);

                                if (((DateTime)item.Ichorini).Day != ((DateTime)item.Ichorfin).Day)
                                {
                                    ws2.Cells[index, 4].Value = "24:00";
                                }
                                else
                                {
                                    ws2.Cells[index, 4].Value = ((DateTime)item.Ichorfin).ToString(Constantes.FormatoHora);
                                }
                                ws2.Cells[index, 5].Value = item.Emprnomb;
                                ws2.Cells[index, 6].Value = item.Areanomb;
                                ws2.Cells[index, 7].Value = item.Equiabrev;  
                                
                                ws2.Cells[index, 8].Value = item.Icdescrip2;
                                ws2.Cells[index, 9].Value = item.Icdescrip3;

                                rg = ws2.Cells[index, 2, index, columnaMax];
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                index++;
                            }

                            rg = ws2.Cells[9, 2, index - 1, columnaMax];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            ws.Column(1).Width = 7;

                            rg = ws2.Cells[9, 2, index, columnaMax];
                            rg.AutoFitColumns();

                            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                            ExcelPicture picture = ws2.Drawings.AddPicture("ff", img);
                            picture.From.Column = 1;
                            picture.From.Row = 1;
                            picture.To.Column = 2;
                            picture.To.Row = 2;
                            picture.SetSize(120, 60);
                        }

                        ExcelWorksheet ws3 = xlPackage.Workbook.Worksheets.Add("CUADRO 2");
                        if (ws3 != null)
                        {
                            ws3.Cells[5, 2].Value = "Operaciones varias del COES - " + clase;
                            ws3.Cells[6, 2].Value = "Desde:  " + fechaIni + " Hasta: " + fechaFin;
                            ws3.Cells[8, 2].Value = "CUADRO N° 2. SISTEMAS AISLADOS NO INCLUIDOS EN LA APLICACIÓN DEL LITERAL j) DEL NUMERAL 5.1.4 DE LA BMNTCSE";

                            int index = 9;
                            int columnaMax = 7;

                            ws3.Cells[index, 2].Value = "FECHA";


                            ws3.Cells[index, 3].Value = "INICIO";
                            ws3.Cells[index, 4].Value = "FINAL";
                            ws3.Cells[index, 5].Value = "EMPRESA";
                            ws3.Cells[index, 6].Value = "UBICACIÓN";
                            ws3.Cells[index, 7].Value = "EQUIPO";
                            ws3.Cells[index, 8].Value = "MOTIVO";
                            ws3.Cells[index, 9].Value = "SUBSISTEMA AISLADO";

                            columnaMax = 9;

                            ExcelRange rg = ws3.Cells[index, 2, index, columnaMax];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            index = index + 1;

                            List<EveIeodcuadroDTO> listcuadro2 = list.Where(x => x.Ictipcuadro == 2).ToList();

                            foreach (EveIeodcuadroDTO item in listcuadro2)
                            {

                                ws3.Cells[index, 2].Value = ((DateTime)item.Ichorini).ToString(Constantes.FormatoFecha);


                                ws3.Cells[index, 3].Value = ((DateTime)item.Ichorini).ToString(Constantes.FormatoHora);

                                if (((DateTime)item.Ichorini).Day != ((DateTime)item.Ichorfin).Day)
                                {
                                    ws3.Cells[index, 4].Value = "24:00";
                                }
                                else
                                {
                                    ws3.Cells[index, 4].Value = ((DateTime)item.Ichorfin).ToString(Constantes.FormatoHora);
                                }
                                ws3.Cells[index, 5].Value = item.Emprnomb;
                                ws3.Cells[index, 6].Value = item.Areanomb;
                                ws3.Cells[index, 7].Value = item.Equiabrev;

                                ws3.Cells[index, 8].Value = item.Icdescrip2;
                                ws3.Cells[index, 9].Value = item.Icdescrip3;

                                rg = ws3.Cells[index, 2, index, columnaMax];
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                index++;
                            }

                            rg = ws3.Cells[9, 2, index - 1, columnaMax];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            ws.Column(1).Width = 7;

                            rg = ws3.Cells[9, 2, index, columnaMax];
                            rg.AutoFitColumns();

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

                        ExcelWorksheet ws4 = xlPackage.Workbook.Worksheets.Add("CUADRO 3");
                        if (ws4 != null)
                        {
                            ws4.Cells[5, 2].Value = "Operaciones varias del COES - " + clase;
                            ws4.Cells[6, 2].Value = "Desde:  " + fechaIni + " Hasta: " + fechaFin;
                            ws4.Cells[8, 2].Value = "CUADRO N° 3. SISTEMAS AISLADOS NO INCLUIDOS EN LA APLICACIÓN DEL LITERAL j) DEL NUMERAL 5.1.4 DE LA BMNTCSE Y CON DURACIÓN MENOR A 10 MINUTOS EN APLICACIÓN DE NUMERAL 3.3. DE LA NTCSE";

                            int index = 9;
                            int columnaMax = 7;

                            ws4.Cells[index, 2].Value = "FECHA";

                            ws4.Cells[index, 3].Value = "INICIO";
                            ws4.Cells[index, 4].Value = "FINAL";
                            ws4.Cells[index, 5].Value = "EMPRESA";
                            ws4.Cells[index, 6].Value = "UBICACIÓN";
                            ws4.Cells[index, 7].Value = "EQUIPO";
                            ws4.Cells[index, 8].Value = "MOTIVO";
                            ws4.Cells[index, 9].Value = "SUBSISTEMA AISLADO";

                            columnaMax = 9;

                            ExcelRange rg = ws4.Cells[index, 2, index, columnaMax];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            index = index + 1;

                            List<EveIeodcuadroDTO> listcuadro3 = list.Where(x => x.Ictipcuadro == 3).ToList();

                            foreach (EveIeodcuadroDTO item in listcuadro3)
                            {

                                ws4.Cells[index, 2].Value = ((DateTime)item.Ichorini).ToString(Constantes.FormatoFecha);

                                ws4.Cells[index, 3].Value = ((DateTime)item.Ichorini).ToString(Constantes.FormatoHora);

                                if (((DateTime)item.Ichorini).Day != ((DateTime)item.Ichorfin).Day)
                                {
                                    ws4.Cells[index, 4].Value = "24:00";
                                }
                                else
                                {
                                    ws4.Cells[index, 4].Value = ((DateTime)item.Ichorfin).ToString(Constantes.FormatoHora);
                                }
                                ws4.Cells[index, 5].Value = item.Emprnomb;
                                ws4.Cells[index, 6].Value = item.Areanomb;
                                ws4.Cells[index, 7].Value = item.Equiabrev;

                                //ws4.Cells[index, 9].Value = item.Icdescrip1;
                                ws4.Cells[index, 8].Value = item.Icdescrip2;
                                ws4.Cells[index, 9].Value = item.Icdescrip3;

                                rg = ws4.Cells[index, 2, index, columnaMax];
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                index++;
                            }

                            rg = ws4.Cells[9, 2, index - 1, columnaMax];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            ws4.Column(1).Width = 7;

                            rg = ws4.Cells[9, 2, index, columnaMax];
                            rg.AutoFitColumns();

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
        /// Permite generar el reporte de Envio de Correo en excel
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        public static void GenerarReporteEnvioCorreo( List<EveMailsDTO> list, DateTime fechaDesde, DateTime fechaHasta, int eveEntidad)
        {
            try
            { 

                string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento + NombreArchivo.ReporteEnvíoCorreo;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }


                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("ENVÍO DE CORREO");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "CONSULTA DE ENVÍO DE CORREOS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[5, 2].Value = "FECHA DESDE: ";
                        ws.Cells[5, 3].Value = fechaDesde.ToString("dd/MM/yyyy");
                        ws.Cells[6, 2].Value = "FECHA HASTA: ";
                        ws.Cells[6, 3].Value = fechaHasta.ToString("dd/MM/yyyy");

                        ws.Cells[5, 5].Value = "TIPO DE OPERACIÓN:";
                        ws.Cells[5, 6].Value = eveEntidad;

                        rg = ws.Cells[5, 2, 6, 2];
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));

                        rg = ws.Cells[5, 5];
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));


                        ws.Cells[8, 2].Value = "Fecha";
                        ws.Cells[8, 3].Value = "Tipo de Operación";
                        ws.Cells[8, 4].Value = "Causa";
                        ws.Cells[8, 5].Value = "Turno";
                        ws.Cells[8, 6].Value = "Programador";
                        ws.Cells[8, 7].Value = "Usuario";
                        ws.Cells[8, 8].Value = "Última modificación";
                        ws.Cells[8, 9].Value = "Emitido";

                        int lastcolumn = 9;

                        rg = ws.Cells[8, 2, 8, lastcolumn];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 9;
                        foreach (EveMailsDTO item in list)
                        {

                            ws.Cells[index, 2].Value = (item.Mailfecha != null) ? ((DateTime)item.Mailfecha).ToString("dd/MM/yyyy HH:mm") : string.Empty;

                            ws.Cells[index, 3].Value = item.Subcausadesc;
                            ws.Cells[index, 4].Value = item.Mailreprogcausa;
                            ws.Cells[index, 5].Value = item.T;
                            if(item.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiCMgHOparaIEDO || item.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiUpdateCMgHOparaIEDO ||
                                item.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReporteCMg || item.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReporteHO ||
                                item.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReportePremCMg|| item.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReportePremHO ||
                                item.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReporteFinCMg || item.Subcausacodi == ConstantesEnviarCorreo.SubcausacodiReporteFinHO)
                                ws.Cells[index, 6].Value = item.Mailespecialista;
                            else
                                ws.Cells[index, 6].Value = item.Mailprogramador;

                            ws.Cells[index, 7].Value = item.Lastuser;
                            ws.Cells[index, 8].Value = (item.Lastdate != null) ? ((DateTime)item.Lastdate).ToString("dd/MM/yyyy HH:mm") : string.Empty;
                            ws.Cells[index, 9].Value = item.Mailemitido;

                            rg = ws.Cells[index, 2, index, lastcolumn];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[8, 2, index - 1, lastcolumn];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[7, 3, index, lastcolumn];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);

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
        /// Permite generar el reporte de Indicadores de CTAF.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="anio"></param>
        public static void GenerarReporteIndicadoresCTAF(List<AfIndicadoresDTO> list, int anio)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento + NombreArchivo.ReporteIndicadoresCTAF;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }


                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("INDICADORES CTAF");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE INDICADORES CTAF";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[5, 2].Value = "AÑO: ";
                        ws.Cells[5, 3].Value = anio;

                        rg = ws.Cells[5, 2];
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));

                        //rg = ws.Cells[5, 5];
                        //rg.Style.Font.Size = 10;
                        //rg.Style.Font.Bold = true;
                        //rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
                        //rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
                        //rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        //rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
                        //rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        //rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
                        //rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        //rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                        //rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));


                        ws.Cells[8, 2].Value = "Mes";
                        ws.Cells[8, 3].Value = "Total de Eventos / MES";
                        ws.Cells[8, 4].Value = "Días utilizados Informe CTAF / MES";
                        ws.Cells[8, 5].Value = "Días utilizados Informe Técnico / MES";
                        ws.Cells[8, 6].Value = "Límite de días CTAF";
                        ws.Cells[8, 7].Value = "Límite de días IT";
                        ws.Cells[8, 8].Value = "Indicador CTAF";
                        ws.Cells[8, 9].Value = "Indicador IT";

                        int lastcolumn = 9;

                        rg = ws.Cells[8, 2, 8, lastcolumn];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 9;
                        foreach (AfIndicadoresDTO item in list)
                        {

                            ws.Cells[index, 2].Value = item.MesNombre;
                            ws.Cells[index, 3].Value = item.TotalEventosMes;
                            ws.Cells[index, 4].Value = item.Diasinfctaf;
                            ws.Cells[index, 5].Value = item.Diasinftec;
                            ws.Cells[index, 6].Value = item.Limctaf;
                            ws.Cells[index, 7].Value = item.Limit;
                            ws.Cells[index, 8].Value = item.Indctaf;
                            ws.Cells[index, 9].Value = item.Indit;

                            rg = ws.Cells[index, 2, index, lastcolumn];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[8, 2, index - 1, lastcolumn];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[7, 3, index, lastcolumn];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);

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
        /// Permite generar el reporte de Indicadores de CTAF.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="anio"></param>
        public static void GenerarReporteCriteriosCTAF(List<CrEventoDTO> list)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento + NombreArchivo.ReporteCriteriosCTAF;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }


                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CRITERIOS - CTAF");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE ANÁLISIS DE FALLA - CRITERIOS - CTAF";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        

                        ws.Cells[5, 3].Value = "DATOS DEL EVENTO";
                        ws.Cells[6, 1].Value = "Código de Evento";
                        ws.Cells[6, 2].Value = "Fecha de evento";
                        ws.Cells[6, 3].Value = "Nombre de Evento";
                        ws.Cells[6, 4].Value = "Caso Especial";
                        ws.Cells[6, 5].Value = "Impugnación";

                        ws.Cells[5, 8].Value = "DATOS DE DECISIÓN DE ASIGNACIÓN DE RESPONSABILIDAD";
                        ws.Cells[6, 6].Value = "Fecha de Decisión";
                        ws.Cells[6, 7].Value = "Descripción del Evento";
                        ws.Cells[6, 8].Value = "Resumen de la decisión de asignación de responsabilidad";
                        ws.Cells[6, 9].Value = "Empresa Responsable";
                        ws.Cells[6, 10].Value = "Comentarios a las empresas responsables";
                        ws.Cells[6, 11].Value = "Criterios";

                        ws.Cells[5, 14].Value = "DATOS DE RECONSIDERACIÓN";
                        ws.Cells[6, 12].Value = "Empresa (s) solicitante (s)";
                        ws.Cells[6, 13].Value = "Principales argumentos del recurso";
                        ws.Cells[6, 14].Value = "Decisión adoptada por la DCOES";
                        ws.Cells[6, 15].Value = "Empresa Responsable";
                        ws.Cells[6, 16].Value = "Comentarios a la nueva asignación de responsabilidad";
                        ws.Cells[6, 17].Value = "Criterios";

                        ws.Cells[5, 20].Value = "DATOS DE APELACIÓN";
                        ws.Cells[6, 18].Value = "Empresa (s) solicitante (s)";
                        ws.Cells[6, 19].Value = "Principales argumentos del recurso";
                        ws.Cells[6, 20].Value = "Decisión adoptada por el Directorio";
                        ws.Cells[6, 21].Value = "Empresa Responsable";
                        ws.Cells[6, 22].Value = "Comentarios a la nueva asignación de responsabilidad";
                        ws.Cells[6, 23].Value = "Criterios";

                        ws.Cells[5, 26].Value = "DATOS DE ARBITRAJE";
                        ws.Cells[6, 24].Value = "Empresa (s) solicitante (s)";
                        ws.Cells[6, 25].Value = "Principales argumentos del recurso";
                        ws.Cells[6, 26].Value = "Decisión adoptada por el tribunal arbitral";
                        ws.Cells[6, 27].Value = "Empresa Responsable";
                        ws.Cells[6, 28].Value = "Comentarios a la nueva asignación de responsabilidad";
                        ws.Cells[6, 29].Value = "Criterios";

                        

                        int lastcolumn = 30;

                        rg = ws.Cells[5, 1, 5, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#29b92d"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        

                        rg = ws.Cells[6, 1, 6, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#29b92d"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[5, 6, 5, 11];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff6a00"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[6, 6, 6, 11];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff6a00"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[5, 12, 5, 17];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff328a"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[6, 12, 6, 17];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff328a"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[5, 18, 5, 23];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00ff21"));
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[6, 18, 6, 23];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00ff21"));
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[5, 24, 5, 29];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00ffff"));
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[6, 24, 6, 29];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00ffff"));
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 7;
                        foreach (CrEventoDTO item in list)
                        {
                            //EVENTO
                            ws.Cells[index, 1].Value = item.CODIGO;
                            ws.Cells[index, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 2].Value = item.FECHA_EVENTO.ToString();
                            ws.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 3].Value = item.NOMBRE_EVENTO;
                            ws.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 3].Style.WrapText = true;

                            ws.Cells[index, 4].Value = item.CASOS_ESPECIAL;
                            ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 5].Value = item.IMPUGNACION;
                            ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            //ETAPA 1
                            ws.Cells[index, 6].Value = item.FECHA_DECISION;
                            ws.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 7].Value = item.DESCRIPCION_EVENTO_DECISION;
                            ws.Cells[index, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 7].Style.WrapText = true;

                            ws.Cells[index, 8].Value = item.RESUMEN_DECISION;
                            ws.Cells[index, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 8].Style.WrapText = true;

                            ws.Cells[index, 9].Value = item.RESPONSABLE_DECISION;
                            ws.Cells[index, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 9].Style.WrapText = true;

                            ws.Cells[index, 10].Value = item.COMENTARIO_EMPRESA_DECISION;
                            ws.Cells[index, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 10].Style.WrapText = true;

                            ws.Cells[index, 11].Value = item.CRITERIO_DECISION;
                            ws.Cells[index, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 11].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 11].Style.WrapText = true;

                            //ETAPA 2
                            ws.Cells[index, 12].Value = item.EMPR_SOLI_RECONSIDERACION;
                            ws.Cells[index, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 12].Style.WrapText = true;

                            ws.Cells[index, 13].Value = item.ARGUMENTO_RECONCIDERACION;
                            ws.Cells[index, 13].Style.WrapText = true;
                            ws.Cells[index, 13].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 14].Value = item.DECISION_RECONCIDERACION;
                            ws.Cells[index, 14].Style.WrapText = true;
                            ws.Cells[index, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 15].Value = item.RESPONSABLE_RECONCIDERACION;
                            ws.Cells[index, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 15].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 15].Style.WrapText = true;

                            ws.Cells[index, 16].Value = item.COMENTARIOS_RECONCIDERACION;
                            ws.Cells[index, 16].Style.WrapText = true;
                            ws.Cells[index, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 17].Value = item.CRITERIOS_RECONSIDERACION;
                            ws.Cells[index, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 17].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 17].Style.WrapText = true;

                            //ETAPA 3
                            ws.Cells[index, 18].Value = item.EMPR_SOLI_APELACION;
                            ws.Cells[index, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 18].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 18].Style.WrapText = true;

                            ws.Cells[index, 19].Value = item.ARGUMENTO_APELACION;
                            ws.Cells[index, 19].Style.WrapText = true;
                            ws.Cells[index, 19].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 20].Value = item.DECISION_APELACION;
                            ws.Cells[index, 20].Style.WrapText = true;
                            ws.Cells[index, 20].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 21].Value = item.RESPONSABLE_APELACION;
                            ws.Cells[index, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 21].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 21].Style.WrapText = true;

                            ws.Cells[index, 22].Value = item.COMENTARIOS_APELACION;
                            ws.Cells[index, 22].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 22].Style.WrapText = true;

                            ws.Cells[index, 23].Value = item.CRITERIOS_APELACION;
                            ws.Cells[index, 23].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 23].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 23].Style.WrapText = true;

                            //ETAPA 4
                            ws.Cells[index, 24].Value = item.EMPR_SOLI_ARBITRAJE;
                            ws.Cells[index, 24].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 24].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 24].Style.WrapText = true;

                            ws.Cells[index, 25].Value = item.ARGUMENTO_ARBITRAJE;
                            ws.Cells[index, 25].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 25].Style.WrapText = true;

                            ws.Cells[index, 26].Value = item.DECISION_ARBITRAJE;
                            ws.Cells[index, 26].Style.WrapText = true;
                            ws.Cells[index, 26].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 27].Value = item.RESPONSABLE_ARBITRAJE;
                            ws.Cells[index, 27].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 27].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 27].Style.WrapText = true;

                            ws.Cells[index, 28].Value = item.COMENTARIOS_ARBITRAJE;
                            ws.Cells[index, 28].Style.WrapText = true;
                            ws.Cells[index, 28].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[index, 29].Value = item.CRITERIOS_ARBITRAJE;
                            ws.Cells[index, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[index, 29].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[index, 29].Style.WrapText = true;

                            rg = ws.Cells[index, 1, index, lastcolumn];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                            
                            index++;
                        }

                        if(list.Count > 0)
                        {

                            rg = ws.Cells[5, 1, index - 1, lastcolumn];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            rg = ws.Cells[7, 1, index - 1, lastcolumn];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        }

                        rg = ws.Cells[5, 3, index, lastcolumn];
                        

                        ws.Column(1).Width = 15;
                        ws.Column(2).Width = 18;
                        ws.Column(3).Width = 90;
                        ws.Column(4).Width = 30;
                        ws.Column(5).Width = 30;
                        ws.Column(6).Width = 18;
                        ws.Column(7).Width = 100;
                        ws.Column(8).Width = 100;
                        ws.Column(9).Width = 30;
                        ws.Column(10).Width = 100;
                        ws.Column(11).Width = 30;
                        ws.Column(12).Width = 30;
                        ws.Column(13).Width = 100;
                        ws.Column(14).Width = 100;
                        ws.Column(15).Width = 30;
                        ws.Column(16).Width = 100;
                        ws.Column(17).Width = 30;
                        ws.Column(18).Width = 30;
                        ws.Column(19).Width = 100;
                        ws.Column(20).Width = 100;
                        ws.Column(21).Width = 30;
                        ws.Column(22).Width = 100;
                        ws.Column(23).Width = 30;
                        ws.Column(24).Width = 30;
                        ws.Column(25).Width = 100;
                        ws.Column(26).Width = 100;
                        ws.Column(27).Width = 30;
                        ws.Column(28).Width = 100;
                        ws.Column(29).Width = 30;
                        //rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 1;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);

                    }
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