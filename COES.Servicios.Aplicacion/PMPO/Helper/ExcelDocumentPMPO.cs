using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace COES.Servicios.Aplicacion.PMPO.Helper
{
    public class ExcelDocumentPMPO
    {
        public static ExcelRange ObtenerEstiloCeldaValidacion(ExcelRange rango, int seccion)
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

        public static ExcelRange ObtenerEstiloCeldaRepCumplimiento(ExcelRange rango, int seccion)
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

        public static void GenerarHojaExcelRepCentralesGeneracion(ExcelWorksheet ws, string titulo, List<MeMedicionxintervaloDTO> listaData)
        {
            //Colores
            if (ws != null)
            {
                //Titulo del reporte                
                ws.Cells[3, 2].Value = titulo;
                ws.Cells[3, 2, 3, 12].Merge = true;
                ws.Cells[2, 2, 3, 12].Style.Font.Bold = true;
                ws.Cells[2, 2, 3, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[2, 2, 3, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int index = 6;

                if (listaData.Any())
                {
                    //Obtenemos las semanas                    
                    List<PmpoSemana> listaSemana = listaData.GroupBy(x => x.Medintfechaini).Select(p => new PmpoSemana()
                    {
                        FechaIni = p.Key,
                        SemanaDesc = p.First().Semana
                    }).OrderBy(x => x.FechaIni).ToList();

                    List<MePtomedicionDTO> listaPto = listaData.GroupBy(x => x.Ptomedicodi).Select(x => new MePtomedicionDTO()
                    {
                        Ptomedicodi = x.Key,
                        Ptomedidesc = x.First().Ptomedidesc,
                        Grupocodi = x.First().Grupocodi,
                        Gruponomb = x.First().Gruponomb
                    }).OrderBy(x => x.Ptomedidesc).ToList();

                    //Primera línea       
                    ws.Cells[index, 1].Value = "Semana";
                    ws.Column(1).Width = 10;
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index - 2, i + 1].Value = listaPto[i - 1].Ptomedicodi;
                        ws.Cells[index - 1, i + 1].Value = (listaPto[i - 1].Gruponomb ?? "").Trim();
                        ws.Cells[index, i + 1].Value = (listaPto[i - 1].Ptomedidesc ?? "").Trim();
                        ws.Column(i + 1).Width = 15;
                    }

                    ExcelRange rg1 = ws.Cells[index - 1, 2, index - 1, listaPto.Count() + 1];
                    rg1 = ObtenerEstiloCeldaRepCumplimiento(rg1, 1);
                    ExcelRange rg = ws.Cells[index, 1, index, listaPto.Count() + 1];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    UtilExcel.CeldasExcelWrapText(ws, index - 1, 2, index, listaPto.Count() + 1);

                    //cuerpo
                    foreach (var semana in listaSemana)
                    {
                        var listaMIntxSem = listaData.Where(p => p.Medintfechaini == semana.FechaIni).ToList();
                        index++;

                        ws.Cells[index, 1].Value = semana.SemanaDesc;
                        var cont = 2;
                        foreach (var item in listaPto)
                        {
                            var regMintXPto = listaMIntxSem.Find(x => x.Ptomedicodi == item.Ptomedicodi);
                            if (regMintXPto != null)
                            {
                                ws.Cells[index, cont].Value = regMintXPto.Medinth1;
                            }
                            ws.Cells[index, cont].Style.Numberformat.Format = "#,##0.00";
                            cont++;
                        }

                    }

                    //Pie de pagina
                    ws.Cells[index + 1, 1].Value = "TOTAL";
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index + 1, i + 1].Value = listaData.Where(p => p.Ptomedicodi == listaPto[i - 1].Ptomedicodi).Sum(s => s.Medinth1.GetValueOrDefault(0));
                        ws.Cells[index + 1, i + 1].Style.Numberformat.Format = "#,##0.00";
                    }

                    ExcelRange rgPie = ws.Cells[index + 1, 1, index + 1, listaPto.Count() + 1];
                    rgPie = ObtenerEstiloCeldaRepCumplimiento(rgPie, 1);

                    //
                    ws.View.FreezePanes(6 + 1, 1 + 1);
                }
            }
        }

        public static void GenerarHojaExcelRepCostosMarginales(ExcelWorksheet ws, string titulo, List<MeMedicionxintervaloDTO> listaData)
        {
            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                //Titulo del reporte                    
                ws.Cells[3, 1].Value = titulo;
                ws.Cells[3, 1, 3, 5].Merge = true;
                ws.Cells[3, 1, 3, 5].Style.Font.Bold = true;
                ws.Cells[3, 1, 3, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[3, 1, 3, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int index = 6;

                if (listaData.Any())
                {
                    //Obtenemos las semanas                    
                    List<PmpoBloqueHorario> listaBloque = listaData.GroupBy(x => new { x.Medintfechaini, x.Medintblqnumero }).Select(p => new PmpoBloqueHorario()
                    {
                        FechaIni = p.Key.Medintfechaini,
                        NroBloque = p.Key.Medintblqnumero,
                        SemanaDesc = p.First().Semana + "-" + p.Key.Medintblqnumero.ToString("D3")
                    }).OrderBy(x => x.FechaIni).ToList();

                    List<MePtomedicionDTO> listaPto = listaData.GroupBy(x => x.Ptomedicodi).Select(x => new MePtomedicionDTO()
                    {
                        Ptomedicodi = x.Key,
                        Ptomedidesc = x.First().Ptomedidesc,
                    }).OrderBy(x => x.Ptomedidesc).ToList();

                    //Primera línea       
                    ws.Cells[index, 1].Value = "Etapa";
                    ws.Column(1).Width = 12;
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index, i + 1].Value = listaPto[i - 1].Ptomedidesc.Trim();
                        ws.Column(i + 1).Width = 15;
                    }

                    ExcelRange rg = ws.Cells[index, 1, index, listaPto.Count() + 1];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    UtilExcel.CeldasExcelWrapText(ws, index, 2, index, listaPto.Count() + 1);

                    //cuerpo
                    foreach (var bloque in listaBloque)
                    {
                        var listaMIntxSem = listaData.Where(p => p.Medintfechaini == bloque.FechaIni && p.Medintblqnumero == bloque.NroBloque).ToList();
                        index++;

                        ws.Cells[index, 1].Value = bloque.SemanaDesc;
                        var cont = 2;
                        foreach (var item in listaPto)
                        {
                            var regMintXPto = listaMIntxSem.Find(x => x.Ptomedicodi == item.Ptomedicodi);
                            if (regMintXPto != null)
                            {
                                ws.Cells[index, cont].Value = regMintXPto.Medinth1;
                            }
                            ws.Cells[index, cont].Style.Numberformat.Format = "#,##0.00";
                            cont++;
                        }

                    }

                    //Pie de pagina
                    ws.Cells[index + 1, 1].Value = "TOTAL";
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index + 1, i + 1].Value = listaData.Where(p => p.Ptomedicodi == listaPto[i - 1].Ptomedicodi).Sum(s => s.Medinth1.GetValueOrDefault(0));
                        ws.Cells[index + 1, i + 1].Style.Numberformat.Format = "#,##0.00";
                    }

                    ExcelRange rgPie = ws.Cells[index + 1, 1, index + 1, listaPto.Count() + 1];
                    rgPie = ObtenerEstiloCeldaRepCumplimiento(rgPie, 1);

                    //
                    ws.View.FreezePanes(6 + 1, 1 + 1);
                }
            }
        }

        public static void GenerarHojaExcelRepFlujos(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> list)
        {
            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                //Titulo del reporte                    
                ws.Cells[3, 1].Value = "FLUJO DE LÍNEAS";
                ws.Cells[3, 1, 3, 7].Merge = true;
                ws.Cells[3, 1, 3, 7].Style.Font.Bold = true;
                ws.Cells[3, 1, 3, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[3, 1, 3, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int index = 6;
                if (list.Count() != 0)
                {
                    //Obtenemos los nombres de PtoMediDesc
                    var puntos = list.Select(p => p.Ptomedidesc).Distinct().ToList();
                    puntos.Sort();

                    ws.Cells[5, 2].Value = "Flujo en los Circuitos";
                    ws.Cells[5, 2, 5, 7].Merge = true;
                    ExcelRange rgt = ws.Cells[5, 2, 5, puntos.Count() + 1];
                    rgt = ObtenerEstiloCeldaRepCumplimiento(rgt, 1);

                    ws.Cells[index + 1, 1].Value = "Etapa";
                    ws.Column(1).Width = 12;

                    //obtenemos las cabeceras de las centrales                                        
                    for (int i = 1; i <= puntos.Count(); i++)
                    {
                        ws.Cells[index, i + 1].Value = puntos[i - 1].Trim();
                        ws.Cells[index + 1, i + 1].Value = "Prom.";
                        ws.Column(i + 1).Width = 15;
                    }
                    ExcelRange rg = ws.Cells[index, 2, index, puntos.Count() + 1];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);
                    index++;
                    rg = ws.Cells[index, 1, index, puntos.Count() + 1];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);
                    //index++;                    

                    var listaBloques = list.Select(q => q.Medintblqnumero).Distinct().ToList();
                    listaBloques.Sort();

                    var grupos = list.OrderBy(p => p.Medintfechaini).Select(p => p.Semana).Distinct().ToList();
                    foreach (var grupo in grupos)
                    {
                        var regSemana = list.Where(p => p.Semana == grupo).OrderBy(p => p.Ptomedidesc);

                        foreach (var bloque in listaBloques)
                        {
                            var cont = 2;
                            index++;

                            foreach (var item in regSemana)
                            {
                                if (item.Medintblqnumero == bloque)
                                {
                                    if (cont == 2)
                                    {
                                        ws.Cells[index, 1].Value = item.Semana + "-" + bloque.ToString("D3");
                                    }
                                    ws.Cells[index, cont].Value = item.Medinth1;
                                    ws.Cells[index, cont].Style.Numberformat.Format = "#,##0.0";
                                    cont++;
                                }


                            }
                        }

                    }


                    //Pie de pagina
                    ws.Cells[index + 1, 1].Value = "TOTAL";
                    for (int i = 1; i <= puntos.Count(); i++)
                    {
                        ws.Cells[index + 1, i + 1].Value = list.Where(p => p.Ptomedidesc == puntos[i - 1].Trim()).Sum(s => s.Medinth1);
                        ws.Cells[index + 1, i + 1].Style.Numberformat.Format = "#,##0.0";
                    }

                    ExcelRange rgPie = ws.Cells[index + 1, 1, index + 1, puntos.Count() + 1];
                    rgPie = ObtenerEstiloCeldaRepCumplimiento(rgPie, 1);

                }

            }
        }

        public static void GenerarHojaExcelRepCaudalesVertidos(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> listaData)
        {
            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                //Titulo del reporte                   
                ws.Cells[3, 2].Value = "Caudales Vertidos (m3/s)";
                ws.Cells[3, 2, 3, 12].Merge = true;
                ws.Cells[2, 2, 3, 12].Style.Font.Bold = true;
                ws.Cells[2, 2, 3, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[2, 2, 3, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int index = 6;

                if (listaData.Any())
                {
                    //Obtenemos las semanas                    
                    List<PmpoSemana> listaSemana = listaData.GroupBy(x => x.Medintfechaini).Select(p => new PmpoSemana()
                    {
                        FechaIni = p.Key,
                        SemanaDesc = p.First().Semana
                    }).OrderBy(x => x.FechaIni).ToList();

                    List<MePtomedicionDTO> listaPto = listaData.GroupBy(x => x.Ptomedicodi).Select(x => new MePtomedicionDTO()
                    {
                        Ptomedicodi = x.Key,
                        Ptomedidesc = x.First().Ptomedidesc,
                        Grupocodi = x.First().Grupocodi,
                        Gruponomb = x.First().Gruponomb
                    }).OrderBy(x => x.Ptomedidesc).ToList();

                    //Primera línea       
                    ws.Cells[index, 1].Value = "Semana";
                    ws.Column(1).Width = 10;
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index - 1, i + 1].Value = (listaPto[i - 1].Gruponomb ?? "").Trim();
                        ws.Cells[index, i + 1].Value = (listaPto[i - 1].Ptomedidesc ?? "").Trim();
                        ws.Column(i + 1).Width = 15;
                    }

                    ExcelRange rg1 = ws.Cells[index - 1, 2, index - 1, listaPto.Count() + 1];
                    rg1 = ObtenerEstiloCeldaRepCumplimiento(rg1, 1);
                    ExcelRange rg = ws.Cells[index, 1, index, listaPto.Count() + 1];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    UtilExcel.CeldasExcelWrapText(ws, index - 1, 2, index, listaPto.Count() + 1);

                    //cuerpo
                    foreach (var semana in listaSemana)
                    {
                        var listaMIntxSem = listaData.Where(p => p.Medintfechaini == semana.FechaIni).ToList();
                        index++;

                        ws.Cells[index, 1].Value = semana.SemanaDesc;
                        var cont = 2;
                        foreach (var item in listaPto)
                        {
                            var regMintXPto = listaMIntxSem.Find(x => x.Ptomedicodi == item.Ptomedicodi);
                            if (regMintXPto != null)
                            {
                                ws.Cells[index, cont].Value = regMintXPto.Medinth1;
                            }
                            ws.Cells[index, cont].Style.Numberformat.Format = "#,##0.0";
                            cont++;
                        }

                    }

                    //Pie de pagina
                    ws.Cells[index + 1, 1].Value = "TOTAL";
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index + 1, i + 1].Value = listaData.Where(p => p.Ptomedicodi == listaPto[i - 1].Ptomedicodi).Sum(s => s.Medinth1.GetValueOrDefault(0));
                        ws.Cells[index + 1, i + 1].Style.Numberformat.Format = "#,##0.0";
                    }

                    ExcelRange rgPie = ws.Cells[index + 1, 1, index + 1, listaPto.Count() + 1];
                    rgPie = ObtenerEstiloCeldaRepCumplimiento(rgPie, 1);

                    //
                    ws.View.FreezePanes(6 + 1, 1 + 1);
                }

            }
        }

        public static void GenerarHojaExcelRepConsumoCombustible(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> listaData)
        {

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                //Titulo del reporte                    
                ws.Cells[3, 2].Value = "Consumo de Combustible (gas = MPC, diesel y residual = gal, carbón y bagazo  = kg)";
                ws.Cells[3, 2, 3, 12].Merge = true;
                ws.Cells[2, 2, 3, 12].Style.Font.Bold = true;
                ws.Cells[2, 2, 3, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[2, 2, 3, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int index = 6;

                if (listaData.Any())
                {
                    //Obtenemos las semanas                    
                    List<PmpoSemana> listaSemana = listaData.GroupBy(x => x.Medintfechaini).Select(p => new PmpoSemana()
                    {
                        FechaIni = p.Key,
                        SemanaDesc = p.First().Semana
                    }).OrderBy(x => x.FechaIni).ToList();

                    List<MePtomedicionDTO> listaPto = listaData.GroupBy(x => x.Ptomedicodi).Select(x => new MePtomedicionDTO()
                    {
                        Ptomedicodi = x.Key,
                        Ptomedidesc = x.First().Ptomedidesc,
                        Grupocodi = x.First().Grupocodi,
                        Gruponomb = x.First().Gruponomb
                    }).OrderBy(x => x.Ptomedidesc).ToList();

                    //Primera línea       
                    ws.Cells[index, 1].Value = "Etapa";
                    ws.Column(1).Width = 10;
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index - 1, i + 1].Value = (listaPto[i - 1].Gruponomb ?? "").Trim();
                        ws.Cells[index, i + 1].Value = (listaPto[i - 1].Ptomedidesc ?? "").Trim();
                        ws.Column(i + 1).Width = 15;
                    }
                    UtilExcel.CeldasExcelWrapText(ws, index - 1, 2, index, listaPto.Count() + 1);

                    ExcelRange rg1 = ws.Cells[index - 1, 2, index - 1, listaPto.Count() + 1];
                    rg1 = ObtenerEstiloCeldaRepCumplimiento(rg1, 1);
                    ExcelRange rg = ws.Cells[index, 1, index, listaPto.Count() + 1];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    UtilExcel.CeldasExcelWrapText(ws, index - 1, 2, index, listaPto.Count() + 1);

                    //cuerpo
                    foreach (var semana in listaSemana)
                    {
                        var listaMIntxSem = listaData.Where(p => p.Medintfechaini == semana.FechaIni).ToList();
                        index++;

                        ws.Cells[index, 1].Value = semana.SemanaDesc;
                        var cont = 2;
                        foreach (var item in listaPto)
                        {
                            var regMintXPto = listaMIntxSem.Find(x => x.Ptomedicodi == item.Ptomedicodi);
                            if (regMintXPto != null)
                            {
                                ws.Cells[index, cont].Value = regMintXPto.Medinth1;
                            }
                            ws.Cells[index, cont].Style.Numberformat.Format = "#,##0.0";
                            cont++;
                        }

                    }

                    //Pie de pagina
                    ws.Cells[index + 1, 1].Value = "TOTAL";
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index + 1, i + 1].Value = listaData.Where(p => p.Ptomedicodi == listaPto[i - 1].Ptomedicodi).Sum(s => s.Medinth1.GetValueOrDefault(0));
                        ws.Cells[index + 1, i + 1].Style.Numberformat.Format = "#,##0.0";
                    }

                    ExcelRange rgPie = ws.Cells[index + 1, 1, index + 1, listaPto.Count() + 1];
                    rgPie = ObtenerEstiloCeldaRepCumplimiento(rgPie, 1);

                    //
                    ws.View.FreezePanes(6 + 1, 1 + 1);
                }
            }
        }

        public static void GenerarHojaExcelRepCostosOperacion(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> listaData)
        {
            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                //Titulo del reporte                   
                ws.Cells[3, 2].Value = "Costo de Operación (Miles US$)";
                ws.Cells[3, 2, 3, 12].Merge = true;
                ws.Cells[2, 2, 3, 12].Style.Font.Bold = true;
                ws.Cells[2, 2, 3, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[2, 2, 3, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int index = 6;

                if (listaData.Any())
                {
                    //Obtenemos las semanas                    
                    List<PmpoSemana> listaSemana = listaData.GroupBy(x => x.Medintfechaini).Select(p => new PmpoSemana()
                    {
                        FechaIni = p.Key,
                        SemanaDesc = p.First().Semana
                    }).OrderBy(x => x.FechaIni).ToList();

                    List<MePtomedicionDTO> listaPto = listaData.GroupBy(x => x.Ptomedicodi).Select(x => new MePtomedicionDTO()
                    {
                        Ptomedicodi = x.Key,
                        Ptomedidesc = x.First().Ptomedidesc,
                        Grupocodi = x.First().Grupocodi,
                        Gruponomb = x.First().Gruponomb
                    }).OrderBy(x => x.Ptomedidesc).ToList();

                    //Primera línea       
                    ws.Cells[index, 1].Value = "Semana";
                    ws.Column(1).Width = 10;
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index - 1, i + 1].Value = (listaPto[i - 1].Gruponomb ?? "").Trim();
                        ws.Cells[index, i + 1].Value = (listaPto[i - 1].Ptomedidesc ?? "").Trim();
                        ws.Column(i + 1).Width = 15;
                    }
                    UtilExcel.CeldasExcelWrapText(ws, index - 1, 2, index, listaPto.Count() + 1);

                    ExcelRange rg1 = ws.Cells[index - 1, 2, index - 1, listaPto.Count() + 1];
                    rg1 = ObtenerEstiloCeldaRepCumplimiento(rg1, 1);
                    ExcelRange rg = ws.Cells[index, 1, index, listaPto.Count() + 1];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    UtilExcel.CeldasExcelWrapText(ws, index - 1, 2, index, listaPto.Count() + 1);

                    //cuerpo
                    foreach (var semana in listaSemana)
                    {
                        var listaMIntxSem = listaData.Where(p => p.Medintfechaini == semana.FechaIni).ToList();
                        index++;

                        ws.Cells[index, 1].Value = semana.SemanaDesc;
                        var cont = 2;
                        foreach (var item in listaPto)
                        {
                            var regMintXPto = listaMIntxSem.Find(x => x.Ptomedicodi == item.Ptomedicodi);
                            if (regMintXPto != null)
                            {
                                ws.Cells[index, cont].Value = regMintXPto.Medinth1;
                            }
                            ws.Cells[index, cont].Style.Numberformat.Format = "#,##0.0";
                            cont++;
                        }

                    }

                    //Pie de pagina
                    ws.Cells[index + 1, 1].Value = "TOTAL";
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index + 1, i + 1].Value = listaData.Where(p => p.Ptomedicodi == listaPto[i - 1].Ptomedicodi).Sum(s => s.Medinth1.GetValueOrDefault(0));
                        ws.Cells[index + 1, i + 1].Style.Numberformat.Format = "#,##0.0";
                    }

                    ExcelRange rgPie = ws.Cells[index + 1, 1, index + 1, listaPto.Count() + 1];
                    rgPie = ObtenerEstiloCeldaRepCumplimiento(rgPie, 1);

                    //
                    ws.View.FreezePanes(6 + 1, 1 + 1);
                }
            }
        }

        public static void GenerarHojaExcelRepDeficitPotenciaEnergia(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> listaData)
        {
            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                //Titulo del reporte                    
                ws.Cells[3, 2].Value = "Déficit de suministro";
                ws.Cells[3, 2, 3, 4].Merge = true;
                ws.Cells[3, 2, 3, 4].Style.Font.Bold = true;
                ws.Cells[3, 2, 3, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[3, 2, 3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int index = 6;
                if (listaData.Any())
                {
                    //Obtenemos las semanas                    
                    List<PmpoBloqueHorario> listaBloque = listaData.GroupBy(x => new { x.Medintfechaini, x.Medintblqnumero }).Select(p => new PmpoBloqueHorario()
                    {
                        FechaIni = p.Key.Medintfechaini,
                        NroBloque = p.Key.Medintblqnumero,
                        SemanaDesc = p.First().Semana + "-" + p.Key.Medintblqnumero.ToString("D3")
                    }).OrderBy(x => x.FechaIni).ToList();

                    List<MePtomedicionDTO> listaPto = listaData.GroupBy(x => x.Ptomedicodi).Select(x => new MePtomedicionDTO()
                    {
                        Ptomedicodi = x.Key,
                        Ptomedidesc = x.First().Ptomedidesc,
                    }).OrderBy(x => x.Ptomedidesc).ToList();

                    //Primera línea       
                    ws.Cells[index, 1].Value = "Etapa";
                    ws.Column(1).Width = 12;
                    int contAux = 2;
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index, contAux + 0].Value = listaPto[i - 1].Ptomedidesc.Trim() + " - Deficit (GWh)";
                        ws.Cells[index, contAux + 1].Value = listaPto[i - 1].Ptomedidesc.Trim() + " - Duraciones";
                        ws.Cells[index, contAux + 2].Value = listaPto[i - 1].Ptomedidesc.Trim() + " - Potencia";
                        ws.Column(contAux + 0).Width = 15;
                        ws.Column(contAux + 1).Width = 15;
                        ws.Column(contAux + 2).Width = 15;

                        contAux = contAux + 3;
                    }

                    ExcelRange rg = ws.Cells[index, 1, index, listaPto.Count() * 3 + 1];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    UtilExcel.CeldasExcelWrapText(ws, index, 2, index, listaPto.Count() + 1);

                    //cuerpo
                    var cont = 2;
                    foreach (var bloque in listaBloque)
                    {
                        var listaMIntxSem = listaData.Where(p => p.Medintfechaini == bloque.FechaIni && p.Medintblqnumero == bloque.NroBloque).ToList();
                        index++;

                        ws.Cells[index, 1].Value = bloque.SemanaDesc;
                        cont = 2;
                        foreach (var item in listaPto)
                        {
                            var regMintXPto = listaMIntxSem.Find(x => x.Ptomedicodi == item.Ptomedicodi);
                            if (regMintXPto != null)
                            {
                                ws.Cells[index, cont + 0].Value = regMintXPto.Medinth1;
                                ws.Cells[index, cont + 1].Value = regMintXPto.Medintblqhoras;
                                ws.Cells[index, cont + 2].Value = regMintXPto.Medinth_1;

                            }
                            ws.Cells[index, cont, index, cont + 2].Style.Numberformat.Format = "#,##0.00";
                            cont += 3;
                        }
                    }

                    //Pie de pagina
                    ws.Cells[index + 1, 1].Value = "TOTAL";
                    cont = 2;
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        ws.Cells[index + 1, cont + 0].Value = listaData.Where(p => p.Ptomedicodi == listaPto[i - 1].Ptomedicodi).Sum(s => s.Medinth1.GetValueOrDefault(0));
                        ws.Cells[index + 1, cont + 1].Value = listaData.Where(p => p.Ptomedicodi == listaPto[i - 1].Ptomedicodi).Sum(s => s.Medintblqhoras);
                        ws.Cells[index + 1, cont + 2].Value = listaData.Where(p => p.Ptomedicodi == listaPto[i - 1].Ptomedicodi).Sum(s => s.Medinth_1.GetValueOrDefault(0));

                        ws.Cells[index + 1, cont, index + i, cont + 2].Style.Numberformat.Format = "#,##0.00";
                        cont += 3;
                    }

                    ExcelRange rgPie = ws.Cells[index + 1, 1, index + 1, listaPto.Count() * 3 + 1];
                    rgPie = ObtenerEstiloCeldaRepCumplimiento(rgPie, 1);

                    //
                    ws.View.FreezePanes(6 + 1, 1 + 1);
                }
            }
        }

        public static void GenerarHojaExcelRepVolumenesDescarga(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> listaData)
        {
            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                //Titulo del reporte                    
                ws.Cells[2, 2].Value = "Caudales (m3/s) y Volúmenes (m3)";
                ws.Cells[2, 2, 2, 12].Merge = true;
                ws.Cells[2, 2, 2, 12].Style.Font.Bold = true;
                ws.Cells[2, 2, 2, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[2, 2, 2, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int index = 7;

                if (listaData.Any())
                {
                    //Obtenemos las semanas                    
                    List<PmpoSemana> listaSemana = listaData.GroupBy(x => x.Medintfechaini).Select(p => new PmpoSemana()
                    {
                        FechaIni = p.Key,
                        SemanaDesc = p.First().Semana
                    }).OrderBy(x => x.FechaIni).ToList();

                    List<MePtomedicionDTO> listaPto = listaData.GroupBy(x => new { x.Ptomedicodi, x.Tptomedicodi }).Select(x => new MePtomedicionDTO()
                    {
                        Ptomedicodi = x.Key.Ptomedicodi,
                        Tipoptomedicodi = x.Key.Tptomedicodi,
                        Tptomedinomb = x.First().Tptomedinomb,
                        Ptomedidesc = x.First().Ptomedidesc,
                        Ptomedielenomb = x.First().Ptomedielenomb,
                        Grupocodi = x.First().Grupocodi,
                        Gruponomb = x.First().Gruponomb
                    }).OrderBy(x => x.Ptomedidesc).ToList();

                    //Primera línea       
                    ws.Cells[index, 1].Value = "Etapa";
                    ws.Column(1).Width = 10;
                    int contadorCabecera = 1;
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        var item = listaPto[i - 1];
                        ws.Cells[index - 3, contadorCabecera + 1].Value = item.Tptomedinomb;
                        ws.Cells[index - 2, contadorCabecera + 1].Value = item.Gruponomb;
                        ws.Cells[index - 1, contadorCabecera + 1].Value = item.Ptomedidesc;
                        ws.Cells[index, contadorCabecera + 1].Value = item.Ptomedielenomb;
                        ws.Column(contadorCabecera + 1).Width = 13;
                        contadorCabecera++;
                    }

                    UtilExcel.CeldasExcelWrapText(ws, index - 3, 2, index, listaPto.Count() + 1);

                    ExcelRange rg = ws.Cells[index - 3, 2, index - 1, listaPto.Count() + 1];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);
                    ExcelRange rg2 = ws.Cells[index, 1, index, listaPto.Count() + 1];
                    rg2 = ObtenerEstiloCeldaRepCumplimiento(rg2, 1);

                    UtilExcel.CeldasExcelWrapText(ws, index - 1, 2, index, listaPto.Count() + 1);

                    //cuerpo
                    foreach (var semana in listaSemana)
                    {
                        var listaMIntxSem = listaData.Where(p => p.Medintfechaini == semana.FechaIni).ToList();
                        index++;

                        ws.Cells[index, 1].Value = semana.SemanaDesc;
                        var cont = 2;
                        foreach (var item in listaPto)
                        {
                            var regMintXPto = listaMIntxSem.Find(x => x.Ptomedicodi == item.Ptomedicodi && x.Tptomedicodi == item.Tipoptomedicodi);
                            if (regMintXPto != null)
                            {
                                ws.Cells[index, cont].Value = regMintXPto.Medinth1;
                            }
                            ws.Cells[index, cont].Style.Numberformat.Format = "#,##0.00";
                            cont++;
                        }

                    }

                    //Pie de pagina
                    ws.Cells[index + 1, 1].Value = "TOTAL";
                    for (int i = 1; i <= listaPto.Count(); i++)
                    {
                        var item = listaPto[i - 1];
                        ws.Cells[index + 1, i + 1].Value = listaData.Where(p => p.Ptomedicodi == item.Ptomedicodi && p.Tptomedicodi == item.Tipoptomedicodi).Sum(s => s.Medinth1.GetValueOrDefault(0));
                        ws.Cells[index + 1, i + 1].Style.Numberformat.Format = "#,##0.00";
                    }

                    ExcelRange rgPie = ws.Cells[index + 1, 1, index + 1, listaPto.Count() + 1];
                    rgPie = ObtenerEstiloCeldaRepCumplimiento(rgPie, 1);

                    //
                    ws.View.FreezePanes(7 + 1, 1 + 1);
                }

            }
        }

        public static void GenerarFormatoExcelRepResultadosPMPO(ExcelWorksheet ws, PmpoReporteCsv regReporte)
        {
            ws.Cells[2, 1].Value = "Fecha";
            ws.Cells[3, 1].Value = "Semana";
            ws.Column(1).Width = 22;

            //Tabla de la cabcera
            for (int i = 1; i <= regReporte.ListaSemana.Count(); i++)
            {
                var regSemana = regReporte.ListaSemana[i - 1];
                ws.Cells[2, i + 1].Value = regSemana.FechaIni;
                ws.Cells[2, i + 1].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[3, i + 1].Value = regSemana.SemanaDesc;
                ws.Column(i + 1).Width = 11;
            }

            ExcelRange rg = ws.Cells[2, 1, 3, regReporte.ListaSemana.Count() + 1];
            rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

            //Cuadro  N° 5.1                    
            #region Matriz Generación Centrales Hidraúlicas (MWh)

            SubCuadroHojaGeneracion(ws, "Cuadro N° 5.1", "Generación de Centrales Hidraulicas (MWh)", "Central", 5, true, true, 0
                                    , regReporte.ListaSemana, regReporte.ListaGenHidra, out int iLastRow1);

            #endregion

            //Cuadro  N° 5.2
            #region Matriz Generación Centrales Termoeléctricas (MWh)

            SubCuadroHojaGeneracion(ws, "Cuadro N° 5.2", "Generación de Centrales TermoEléctricas (MWh)", "Central", iLastRow1 + 2, false, true, 0
                                    , regReporte.ListaSemana, regReporte.ListaGenTermo, out int iLastRow2);

            #endregion

            //Cuadro  N° 5.3
            #region Generación Centrales Renovables, Cogeneración y no Integrantes (MWh)

            SubCuadroHojaGeneracion(ws, "Cuadro N° 5.3", "Generación Centrales Renovables, Cogeneración y no Integrantes (MWh)", "Central", iLastRow2 + 2, false, true, 0
                                    , regReporte.ListaSemana, regReporte.ListaGenReno, out int iLastRow3);

            #endregion

            //Cuadro N° 5.4
            #region Costos Marginales  (US$/MWh)

            SubCuadroBloqueHoja(ws, "Cuadro N° 5.4", "Costos Marginales  (US$/MWh)", "Bloques", true, false, iLastRow3 + 2, 1
                                , regReporte.ListaSemana, regReporte.ListaBloqueCostMargi, regReporte.ListaPtoCostMargi, regReporte.ListaCostMargi, out int iLastRow4);

            #endregion

            //Cuadro  N° 5.5
            #region Costo de Operación (Miles US$)

            SubCuadroHojaGeneracion(ws, "Cuadro N° 5.5", "Costo de Operación (Miles US$)", "Costo k$", iLastRow4, false, false, 0
                                    , regReporte.ListaSemana, regReporte.ListaCostOpera, out int iLastRow5);

            #endregion

            //Cuadro N° 5.6
            #region Energía por tipo de generación (MWh)

            SubCuadroHojaGeneracion(ws, "Cuadro N° 5.6", "Energía por tipo de generación (MWh)", "Tipo de generación", iLastRow5 + 2, false, false, 0
                                    , regReporte.ListaSemana, regReporte.ListaEnergTipoGen, out int iLastRow6);

            #endregion

            //Cuadro  N° 5.7
            #region Déficit de suministro

            SubCuadroBloqueHoja(ws, "Cuadro N° 5.7", "Déficit de suministro", "Bloques", true, false, iLastRow6 + 2, 0
                                , regReporte.ListaSemana, regReporte.ListaBloqueDeficitPotencia, regReporte.ListaPtoDeficitPotencia, regReporte.ListaDeficitPotencia, out int iLastRow70);

            SubCuadroBloqueHoja(ws, "", "", "", false, false, iLastRow70 + 0, 0
                                , regReporte.ListaSemana, regReporte.ListaBloqueDeficitEnergia, regReporte.ListaPtoDeficitEnergia, regReporte.ListaDeficitEnergia, out int iLastRow7);

            #endregion

            //Cuadro  N° 5.8
            #region Volúmenes y Descargas de Embalses (Millones m3)

            SubCuadroBloqueHoja(ws, "Cuadro N° 5.8", "Volúmenes y Descargas de Embalses (Millones m3)", "", true, true, iLastRow7, 2
                                , regReporte.ListaSemana, regReporte.ListaBloqueVoluDesc, regReporte.ListaPtoVoluDesc, regReporte.ListaVoluDesc, out int iLastRow8);

            #endregion

            //Cuadro  N° 5.9
            #region Caudales Vertidos (m3/s)

            SubCuadroHojaGeneracion(ws, "Cuadro N° 5.9", "Caudales Vertidos (m3/s)", "Central", iLastRow8 + 1, false, false, 2
                                    , regReporte.ListaSemana, regReporte.ListaCaudalVertido, out int iLastRow9);

            #endregion

            //Cuadro  N° 5.10
            #region Consumo de Combustible (gas = mpc, diesel y residual = gal, carbon = kg)

            SubCuadroHojaGeneracion(ws, "Cuadro N° 5.10", "Consumo de Combustible (gas = MPC, diesel y residual = gal, carbón y bagazo  = kg)", "Central", iLastRow9 + 2, false, false, 0
                                    , regReporte.ListaSemana, regReporte.ListaConsCombu, out int iLastRow10);

            #endregion

            ws.View.FreezePanes(3 + 1, 1 + 1);

            ws.View.ZoomScale = 90;

            //excel con Font 
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = "Arial";


        }

        private static void SubCuadroHojaGeneracion(ExcelWorksheet ws, string cuadro, string titulo, string cabeceraCol1, int rowIni, bool incluirTc, bool incluirTotal, int numDecimal
                                                    , List<PmpoSemana> listaSemana, List<MeMedicionxintervaloDTO> listaData, out int iLastRow)
        {
            //Titulo del reporte

            ws.Cells[rowIni, 1].Value = cuadro;
            UtilExcel.CeldasExcelAgrupar(ws, rowIni, 1, rowIni, 12);
            UtilExcel.SetFormatoCelda(ws, rowIni, 1, rowIni, 12, "Centro", "Centro", "#000000", "#FFFFFF", "Arial", 14, true);

            rowIni += 1;
            ws.Cells[rowIni, 1].Value = titulo;
            UtilExcel.CeldasExcelAgrupar(ws, rowIni, 1, rowIni, 12);
            UtilExcel.SetFormatoCelda(ws, rowIni, 1, rowIni, 12, "Centro", "Centro", "#000000", "#FFFFFF", "Arial", 14, true);

            if (incluirTc)
            {
                ws.Cells[rowIni, 13].Value = "TC= ";
                ws.Cells[rowIni, 14].Value = (new Combustibles.CombustibleAppServicio()).GetTipoCambio(listaSemana[0].FechaIni);
                ws.Cells[rowIni, 15].Value = "S/./US$";
                UtilExcel.SetFormatoCelda(ws, rowIni, 13, rowIni, 15, "Centro", "Centro", "#000000", "#FFFFFF", "Arial", 10, false);
                UtilExcel.SetFormatoCelda(ws, rowIni, 14, rowIni, 14, "Centro", "Centro", "#000000", "#FFFFFF", "Arial", 10, true);
            }

            int index = rowIni + 2;
            ws.Cells[index, 1].Value = cabeceraCol1;

            int totalCol = listaSemana.Count();
            int posFinCol = 2 + totalCol - 1;
            //obtenemos las cabeceras de las semanas   
            for (int i = 1; i <= totalCol; i++)
            {
                ws.Cells[index, i + 1].Value = listaSemana[i - 1].SemanaDesc;
            }
            UtilExcel.SetFormatoCelda(ws, index, 1, index, posFinCol, "Centro", "Centro", "#FFFFFF", "#2980B9", "Arial", 10, true);

            //Obtenemos los nombres de Grupo   
            List<MePtomedicionDTO> listaPto = listaData.GroupBy(x => x.Ptomedicodi).Select(x => new MePtomedicionDTO()
            {
                Ptomedicodi = x.Key,
                Ptomedidesc = x.First().Ptomedidesc,
                Grupocodi = x.First().Grupocodi,
                Gruponomb = x.First().Gruponomb,
                Catecodi = x.First().Catecodi,
                Orden = x.First().Orden
            }).OrderBy(x => x.Orden).ThenBy(x => x.Ptomedidesc).ToList();

            foreach (var regPto in listaPto)
            {
                var listaMIntxPto = listaData.Where(p => p.Ptomedicodi == regPto.Ptomedicodi).ToList();
                index++;

                ws.Cells[index, 1].Value = regPto.Ptomedidesc;
                UtilExcel.SetFormatoCelda(ws, index, 1, index, 1, "Centro", "Izquierda", "#000000", "#FFFFFF", "Arial", 10, true);

                var cont = 2;
                foreach (var semana in listaSemana)
                {
                    var regMintXPto = listaMIntxPto.Find(x => x.Medintfechaini == semana.FechaIni);
                    decimal? valor = (regMintXPto != null) ? regMintXPto.Medinth1 : null;
                    if (valor == null) valor = 0;

                    ws.Cells[index, cont].Value = valor;
                    cont++;
                }

                UtilExcel.SetFormatoCelda(ws, index, 2, index, posFinCol, "Centro", "Derecha", "#000000", "#FFFFFF", "Arial", 10, false);
                UtilExcel.CeldasExcelFormatoNumero(ws, index, 2, index, posFinCol, numDecimal);
                UtilExcel.BorderCeldasLineaDelgada(ws, index, 1, index, posFinCol, "#000000", true, true);
            }

            //Pie de pagina
            if (incluirTotal)
            {
                index++;
                ws.Cells[index, 1].Value = "TOTAL";

                for (int i = 1; i <= totalCol; i++)
                {
                    ws.Cells[index, i + 1].Value = listaData.Where(p => p.Medintfechaini == listaSemana[i - 1].FechaIni).Sum(s => s.Medinth1.GetValueOrDefault(0));
                }

                UtilExcel.SetFormatoCelda(ws, index, 1, index, posFinCol, "Centro", "Derecha", "#FFFFFF", "#2980B9", "Arial", 10, true);
                UtilExcel.CeldasExcelFormatoNumero(ws, index, 1, index, posFinCol, 0);
            }

            iLastRow = index;
        }

        private static void SubCuadroBloqueHoja(ExcelWorksheet ws, string cuadro, string titulo, string cabeceraCol1, bool incluirTitulo, bool esPtoCabecera, int rowIni, int numDecimal
                                            , List<PmpoSemana> listaSemana, List<PmpoBloqueHorario> listaBloque
                                            , List<MePtomedicionDTO> listaPto, List<MeMedicionxintervaloDTO> listaData
                                            , out int iLastRow)
        {
            int index = rowIni;
            //Titulo del reporte
            if (incluirTitulo)
            {
                ws.Cells[rowIni, 1].Value = cuadro;
                UtilExcel.CeldasExcelAgrupar(ws, rowIni, 1, rowIni, 12);
                UtilExcel.SetFormatoCelda(ws, rowIni, 1, rowIni, 12, "Centro", "Centro", "#000000", "#FFFFFF", "Arial", 14, true);

                rowIni++;
                ws.Cells[rowIni, 1].Value = titulo;
                UtilExcel.CeldasExcelAgrupar(ws, rowIni, 1, rowIni, 12);
                UtilExcel.SetFormatoCelda(ws, rowIni, 1, rowIni, 12, "Centro", "Centro", "#000000", "#FFFFFF", "Arial", 14, true);

                if (!esPtoCabecera) index = rowIni + 1;
                else index = rowIni + 2;
            }

            int totalCol = listaSemana.Count();
            int posFinCol = 2 + totalCol - 1;
            //Obtenemos los nombres de Grupo   
            foreach (var regPto in listaPto)
            {
                var listaMIntxPto = listaData.Where(p => p.Ptomedicodi == regPto.Ptomedicodi).ToList();

                //obtenemos las cabeceras de las semanas
                if (!esPtoCabecera)
                {
                    ws.Cells[index, 1].Value = regPto.Ptomedidesc;
                    UtilExcel.SetFormatoCelda(ws, index, 1, index, 1, "Centro", "Izquierda", "#000000", "#FFFFFF", "Arial", 10, true);

                    index++;
                    ws.Cells[index, 1].Value = cabeceraCol1;
                }
                else
                {
                    ws.Cells[index, 1].Value = regPto.Ptomedidesc;
                }
                for (int i = 1; i <= listaSemana.Count(); i++)
                {
                    ws.Cells[index, i + 1].Value = listaSemana[i - 1].SemanaDesc;
                }
                UtilExcel.SetFormatoCelda(ws, index, 1, index, posFinCol, "Centro", "Centro", "#FFFFFF", "#2980B9", "Arial", 10, true);
                if (esPtoCabecera) UtilExcel.SetFormatoCelda(ws, index, 1, index, 1, "Centro", "Derecha", "#FFFFFF", "#2980B9", "Arial", 10, true);

                index++;
                foreach (var bloque in listaBloque)
                {
                    ws.Cells[index, 1].Value = bloque.BloqueDesc;
                    UtilExcel.SetFormatoCelda(ws, index, 1, index, 1, "Centro", "Izquierda", "#000000", "#FFFFFF", "Arial", 10, true);

                    var cont = 2;
                    foreach (var semana in listaSemana)
                    {
                        var regMintXPto = listaMIntxPto.Find(x => x.Medintfechaini == semana.FechaIni && x.Medintblqnumero == bloque.NroBloque);
                        decimal? valor = (regMintXPto != null) ? regMintXPto.Medinth1 : null;
                        if (valor == null) valor = 0;

                        ws.Cells[index, cont].Value = valor;
                        cont++;
                    }

                    UtilExcel.SetFormatoCelda(ws, index, 2, index, posFinCol, "Centro", "Derecha", "#000000", "#FFFFFF", "Arial", 10, false);
                    UtilExcel.CeldasExcelFormatoNumero(ws, index, 2, index, posFinCol, numDecimal);
                    UtilExcel.BorderCeldasLineaDelgada(ws, index, 1, index, posFinCol, "#000000", true, true);

                    index++;
                }

                if (!esPtoCabecera) index++;
            }

            iLastRow = index;
        }

        public static void GenerarReporteMantenimientos(string fileName, PmpoProcesamientoDat obj)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Mantenimientos");
                GenerarHojaManttos(ws, obj.Tsddpcodi, obj.TipoFormato, obj.ListaCodigo, obj.ListaManttos);
                xlPackage.Save();

                ws = xlPackage.Workbook.Worksheets.Add("Indisponibilidad");
                GenerarHojaResultado(ws, obj.Tsddpcodi, obj.TipoFormato, obj.ListaDisp);
                xlPackage.Save();
            }
        }

        private static void GenerarHojaManttos(ExcelWorksheet ws, int tsddpcodi, string tipoFormato, List<PmoSddpCodigoDTO> listaCodigo, List<EveManttoDTO> listaManttos)
        {
            ws.Cells[2, 2].Value = "Reporte de Mantenimientos";
            ws.Cells[2, 2, 2, 16].Merge = true;
            ws.Cells[2, 2, 2, 16].Style.Font.Bold = true;
            ws.Cells[2, 2, 2, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[2, 2, 2, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            if (tsddpcodi == ConstantesPMPO.TsddpPlantaHidraulica)
                ws.Cells[3, 2].Value = "Unidades de Generación Hidráulicas";
            else if (tsddpcodi == ConstantesPMPO.TsddpPlantaTermica)
                ws.Cells[3, 2].Value = "Unidades de Generación Térmicas";

            ws.Cells[3, 2, 3, 16].Merge = true;
            ws.Cells[3, 2, 3, 16].Style.Font.Bold = true;
            ws.Cells[3, 2, 3, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[3, 2, 3, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            bool esMensual = tipoFormato == ConstantesPMPO.FormatoHidraulicoMensual || tipoFormato == ConstantesPMPO.FormatoTermicoMensual;

            int index = 6;

            //obtenemos las cabeceras del reporte                                                           
            ws.Cells[index, 1].Value = "NRO.";
            ws.Column(1).Width = 6;
            ws.Cells[index, 2].Value = "COD SDDP";
            ws.Column(2).Width = 9;
            ws.Cells[index, 3].Value = "NOMBRE SDDP";
            ws.Column(3).Width = 12;
            ws.Cells[index, 4].Value = "EMPRESA";
            ws.Column(4).Width = 27;
            ws.Cells[index, 5].Value = "UBICACION";
            ws.Column(5).Width = 27;
            ws.Cells[index, 6].Value = "EQUIPO";
            ws.Column(6).Width = 10;

            ws.Cells[index, 7].Value = "AÑO";
            ws.Column(7).Width = 9;
            ws.Cells[index, 8].Value = esMensual ? "MES" : "SEM";
            ws.Column(8).Width = 9;
            ws.Cells[index, 9].Value = "INICIO";
            ws.Column(9).Width = 17;
            ws.Cells[index, 10].Value = "FIN";
            ws.Column(10).Width = 17;

            ws.Cells[index, 11].Value = "DESCRIPCION";
            ws.Column(11).Width = 30;
            ws.Cells[index, 12].Value = "MW INDISP.";
            ws.Column(12).Width = 11;
            ws.Cells[index, 13].Value = "DISPON.";
            ws.Column(13).Width = 8;
            ws.Cells[index, 14].Value = "INTERRUP.";
            ws.Column(14).Width = 8;
            ws.Cells[index, 15].Value = "HORIZONTE";
            ws.Column(15).Width = 12;
            ws.Cells[index, 16].Value = "TIPO";
            ws.Column(16).Width = 12;
            ws.Cells[index, 17].Value = "PROGR.";
            ws.Column(17).Width = 11;
            ws.Cells[index, 18].Value = "% INDISP";
            ws.Column(18).Width = 15;

            ExcelRange rg = ws.Cells[index, 1, index, 18];
            rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);
            var cont = 0;
            foreach (var regSddp in listaCodigo)
            {
                var listaManttoXSddp = listaManttos.Where(x => regSddp.ListaEquicodi.Contains(x.Equicodi ?? 0)).ToList();
                foreach (var item in listaManttoXSddp)
                {
                    index++;
                    cont++;
                    ws.Cells[index, 1].Value = cont;
                    ws.Cells[index, 2].Value = regSddp.Sddpnum;
                    ws.Cells[index, 3].Value = regSddp.Sddpnomb;
                    ws.Cells[index, 4].Value = item.Emprnomb;
                    ws.Cells[index, 5].Value = item.Areanomb;
                    ws.Cells[index, 6].Value = item.Equiabrev;

                    ws.Cells[index, 7].Value = item.Anio;
                    ws.Cells[index, 8].Value = esMensual ? item.Mes : item.NroSemana;
                    ws.Cells[index, 9].Value = item.Evenini;
                    ws.Cells[index, 10].Value = item.Evenfin;

                    ws.Cells[index, 11].Value = item.Evendescrip.Trim();
                    ws.Cells[index, 12].Value = item.Evenmwindisp;
                    ws.Cells[index, 13].Value = item.Evenindispo;
                    ws.Cells[index, 14].Value = item.Eveninterrup;
                    ws.Cells[index, 15].Value = item.Evenclaseabrev;
                    ws.Cells[index, 16].Value = item.Tipoevenabrev;
                    ws.Cells[index, 17].Value = item.Eventipoprog;
                    ws.Cells[index, 18].Value = item.Pmcindporcentaje;

                    ws.Cells[index, 9].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 10].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 12].Style.Numberformat.Format = "#,##0.0";
                }
            }

            //filter
            ws.Cells[6, 1, 6, 15].AutoFilter = true;

            ws.View.FreezePanes(6 + 1, 1 + 1);
            ws.View.ZoomScale = 85;
        }

        private static void GenerarHojaResultado(ExcelWorksheet ws, int tsddpcodi, string tipoFormato, List<PmpoHITotal> listaDisp)
        {
            ws.Cells[2, 2].Value = "Detalle de Indisponibilidad";
            ws.Cells[2, 2, 2, 16].Merge = true;
            ws.Cells[2, 2, 2, 16].Style.Font.Bold = true;
            ws.Cells[2, 2, 2, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[2, 2, 2, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            if (tsddpcodi == ConstantesPMPO.TsddpPlantaHidraulica)
                ws.Cells[3, 2].Value = "Unidades de Generación Hidráulicas";
            else if (tsddpcodi == ConstantesPMPO.TsddpPlantaTermica)
                ws.Cells[3, 2].Value = "Unidades de Generación Térmicas";

            ws.Cells[3, 2, 3, 16].Merge = true;
            ws.Cells[3, 2, 3, 16].Style.Font.Bold = true;
            ws.Cells[3, 2, 3, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[3, 2, 3, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            int index = 6;

            //                                       
            bool esMensual = tipoFormato == ConstantesPMPO.FormatoHidraulicoMensual || tipoFormato == ConstantesPMPO.FormatoTermicoMensual;
            ws.Cells[index - 1, 4].Value = esMensual ? "MES" : "SEMANA";
            ws.Cells[index - 1, 4, index - 1, 10].Merge = true;

            //obtenemos las cabeceras del reporte                                                           
            ws.Cells[index, 1].Value = "NRO.";
            ws.Column(1).Width = 6;
            ws.Cells[index, 2].Value = "NUM";
            ws.Column(2).Width = 15;
            ws.Cells[index, 3].Value = "SDDP";
            ws.Column(3).Width = 15;

            ws.Cells[index, 4].Value = "AÑO";
            ws.Column(4).Width = 8;
            ws.Cells[index, 5].Value = esMensual ? "MES" : "SEM";
            ws.Column(5).Width = 8;
            ws.Cells[index, 6].Value = "INI";
            ws.Column(6).Width = 10;
            ws.Cells[index, 7].Value = "FIN";
            ws.Column(7).Width = 10;
            ws.Cells[index, 8].Value = "#TOTAL";
            ws.Column(8).Width = 8;
            ws.Cells[index, 9].Value = "#INDISP";
            ws.Column(9).Width = 8;
            ws.Cells[index, 10].Value = "% DISP";
            ws.Column(10).Width = 8;

            ws.Cells[index, 11].Value = "HORA INICIO";
            ws.Column(11).Width = 18;
            ws.Cells[index, 12].Value = "HORA FIN";
            ws.Column(12).Width = 18;
            ws.Cells[index, 13].Value = "#TOTAL";
            ws.Column(13).Width = 8;
            ws.Cells[index, 14].Value = "#INDISP";
            ws.Column(14).Width = 8;

            ws.Cells[index, 15].Value = "UBICACIÓN";
            ws.Column(15).Width = 15;
            ws.Cells[index, 16].Value = "EQUIPO (%)";
            ws.Column(16).Width = 13;
            ws.Cells[index, 17].Value = "MANTTO";
            ws.Column(17).Width = 100;

            ExcelRange rg = ws.Cells[index, 1, index, 17];
            rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);
            var cont = 0;

            index++;
            cont++;
            foreach (var regtot in listaDisp)
            {
                ws.Cells[index, 8].Value = regtot.HoraTotal;
                ws.Cells[index, 9].Value = regtot.HoraIndisp;
                ws.Cells[index, 10].Value = regtot.PorcentajeDisp;

                foreach (var regDet in regtot.ListaDetXSem)
                {
                    ws.Cells[index, 11].Value = regDet.Horaini.ToString(ConstantesAppServicio.FormatoFechaFull2);
                    ws.Cells[index, 12].Value = regDet.Horafin.ToString(ConstantesAppServicio.FormatoFechaFull2);
                    ws.Cells[index, 13].Value = regDet.HorasTotal;
                    ws.Cells[index, 14].Value = regDet.HorasIndisp;

                    if (regDet.ListaCorrelaciones.Where(x => x.PmCindPorcentaje > 0).ToList().Any())
                    {
                        foreach (var regEq in regDet.ListaCorrelaciones.Where(x => x.PmCindPorcentaje > 0).ToList())
                        {
                            ws.Cells[index, 1].Value = cont;
                            ws.Cells[index, 2].Value = regtot.Sddpnum;
                            ws.Cells[index, 3].Value = regtot.Sddpnomb;
                            ws.Cells[index, 4].Value = regtot.Anio;
                            ws.Cells[index, 5].Value = esMensual ? regtot.Mes : regtot.NroSemana;
                            ws.Cells[index, 6].Value = regtot.FechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                            ws.Cells[index, 7].Value = regtot.FechaFin.ToString(ConstantesAppServicio.FormatoFecha);

                            ws.Cells[index, 15].Value = regEq.AreaNomb;
                            ws.Cells[index, 16].Value = string.Format("{0} ({1}%)", regEq.EquiAbrev, regEq.PmCindPorcentaje);
                            ws.Cells[index, 17].Value = string.Join("\n", regEq.ListaEvendescrip);

                            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, index, 1, index, 16, "Calibri", 10);
                            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, index, 17, index, 17, "Calibri", 8);
                            UtilExcel.CeldasExcelWrapText(ws, index, 17, index, 17);

                            index++;
                        }
                    }
                    else
                    {
                        ws.Cells[index, 8].Value = null;
                        ws.Cells[index, 9].Value = null;
                        ws.Cells[index, 10].Value = null;
                        ws.Cells[index, 11].Value = null;
                        ws.Cells[index, 12].Value = null;
                        ws.Cells[index, 13].Value = null;
                        ws.Cells[index, 14].Value = null;

                    }
                }
            }

            //filter
            ws.Cells[6, 1, 6, 16].AutoFilter = true;

            ws.View.FreezePanes(6 + 1, 1 + 1);
            ws.View.ZoomScale = 100;
        }

        public static void GenerarReporteCGND(string fileName, List<PmoDatCgndDTO> list)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CGND");

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {

                    ws.Cells[2, 2].Value = "Configuración de Fuentes Renovables";
                    ws.Cells[2, 2, 2, 10].Merge = true;
                    ws.Cells[2, 2, 2, 10].Style.Font.Bold = true;
                    ws.Cells[2, 2, 2, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[2, 2, 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int index = 5;

                    //obtenemos las cabeceras del reporte                                                           
                    ws.Cells[index, 1].Value = "Num.";
                    ws.Column(1).Width = 10;
                    ws.Cells[index, 2].Value = "Nombre Central";
                    ws.Column(2).Width = 30;
                    ws.Cells[index, 3].Value = "Bus.";
                    ws.Column(3).Width = 10;
                    ws.Cells[index, 4].Value = "Nombre Barra";
                    ws.Column(4).Width = 30;
                    ws.Cells[index, 5].Value = "Tipo";
                    ws.Column(5).Width = 10;
                    ws.Cells[index, 6].Value = "#Uni";
                    ws.Column(6).Width = 10;
                    ws.Cells[index, 7].Value = "PotIns";
                    ws.Column(7).Width = 16;
                    ws.Cells[index, 8].Value = "FatOpe";
                    ws.Column(8).Width = 16;
                    ws.Cells[index, 9].Value = "ProbFal";
                    ws.Column(9).Width = 16;
                    ws.Cells[index, 10].Value = "SFal";
                    ws.Column(10).Width = 16;


                    ExcelRange rg = ws.Cells[index, 1, index, 10];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);
                    var cont = 0;
                    foreach (var item in list)
                    {
                        index++;
                        cont++;
                        ws.Cells[index, 1].Value = item.CodCentral;
                        ws.Cells[index, 2].Value = item.NombCentral;
                        ws.Cells[index, 3].Value = item.CodBarra;
                        ws.Cells[index, 4].Value = item.NombBarra;
                        ws.Cells[index, 5].Value = item.PmCgndTipoPlanta;
                        ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[index, 6].Value = item.PmCgndNroUnidades;
                        //ws.Cells[index, 6].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                        ws.Cells[index, 7].Value = item.PmCgndPotInstalada;
                        ws.Cells[index, 8].Value = item.PmCgndFactorOpe;
                        ws.Cells[index, 9].Value = item.PmCgndProbFalla;
                        //ws.Cells[index, 9].Style.Numberformat.Format = "#,##0.0";
                        ws.Cells[index, 10].Value = item.PmCgndCorteOFalla;
                    }




                }

                xlPackage.Save();
            }
        }

        public static void GenerarReporteMGND(string fileName, List<PmoDatMgndDTO> list)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CGND");

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {

                    ws.Cells[2, 2].Value = "Modificación de Fuentes Renovables";
                    ws.Cells[2, 2, 2, 10].Merge = true;
                    ws.Cells[2, 2, 2, 10].Style.Font.Bold = true;
                    ws.Cells[2, 2, 2, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[2, 2, 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int index = 5;

                    //obtenemos las cabeceras del reporte                                                           
                    ws.Cells[index, 1].Value = "Fecha";
                    ws.Column(1).Width = 18;
                    ws.Cells[index, 2].Value = "Num";
                    ws.Column(2).Width = 10;
                    ws.Cells[index, 3].Value = "Nombre";
                    ws.Column(3).Width = 30;
                    ws.Cells[index, 4].Value = "Bus";
                    ws.Column(4).Width = 10;
                    ws.Cells[index, 5].Value = "Nombre Barra";
                    ws.Column(5).Width = 30;
                    ws.Cells[index, 6].Value = "Tipo";
                    ws.Column(6).Width = 10;
                    ws.Cells[index, 7].Value = "#Uni";
                    ws.Column(7).Width = 10;
                    ws.Cells[index, 8].Value = "PotIns";
                    ws.Column(8).Width = 16;
                    ws.Cells[index, 9].Value = "FatOpe";
                    ws.Column(9).Width = 16;
                    ws.Cells[index, 10].Value = "ProbFal";
                    ws.Column(10).Width = 16;


                    ExcelRange rg = ws.Cells[index, 1, index, 10];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);
                    var cont = 0;
                    foreach (var item in list)
                    {
                        index++;
                        cont++;
                        ws.Cells[index, 1].Value = item.PmMgndFecha.Value.ToString("dd/MM/yyyy");
                        ws.Cells[index, 2].Value = item.CodCentral;
                        ws.Cells[index, 3].Value = item.NombCentral;
                        ws.Cells[index, 4].Value = item.CodBarra;
                        ws.Cells[index, 5].Value = item.NombBarra;
                        ws.Cells[index, 6].Value = item.PmMgndTipoPlanta;
                        ws.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //ws.Cells[index, 6].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                        ws.Cells[index, 7].Value = item.PmMgndNroUnidades;
                        ws.Cells[index, 8].Value = item.PmMgndPotInstalada;
                        ws.Cells[index, 9].Value = item.PmMgndFactorOpe;
                        //ws.Cells[index, 9].Style.Numberformat.Format = "#,##0.0";
                        ws.Cells[index, 10].Value = item.PmMgndProbFalla;
                    }

                }

                xlPackage.Save();
            }
        }

        public static void GenerarReporteExcelTotalGndse(string fileName, List<PrGrupoDTO> gndseCabeceras, List<PmoDatGndseDTO> gndse)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("gndse05pe");

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {

                    ws.Cells[2, 2].Value = "Generación RER";
                    ws.Cells[2, 2, 2, 16].Merge = true;
                    ws.Cells[2, 2, 2, 16].Style.Font.Bold = true;
                    ws.Cells[2, 2, 2, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[2, 2, 2, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int index = 5;

                    //obtenemos las cabeceras del reporte                                                           
                    ws.Cells[index, 1].Value = "!Stg";
                    ws.Column(1).Width = 5;
                    ws.Cells[index, 2].Value = "Scn";
                    ws.Column(2).Width = 5;
                    ws.Cells[index, 3].Value = "LBlk";
                    ws.Column(3).Width = 5;

                    int cont = 3;
                    foreach (var gndseCabecera in gndseCabeceras)
                    {
                        cont++;
                        if (gndseCabecera.Grupoabrev != null)
                            ws.Cells[index, cont].Value = gndseCabecera.Grupoabrev.Trim();

                        ws.Column(cont).Width = 15;
                    }

                    ExcelRange rg = ws.Cells[index, 1, index, cont];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    int saltoRegistro = gndseCabeceras.Count();

                    for (var registro = 0; registro < gndse.Count; registro++)
                    {
                        index++;
                        ws.Cells[index, 1].Value = gndse[registro].Stg;
                        ws.Cells[index, 2].Value = gndse[registro].Scn;
                        ws.Cells[index, 3].Value = gndse[registro].Lblk;

                        int contReg = 3;
                        for (var y = 0; y < saltoRegistro; y++)
                        {
                            contReg++;
                            ws.Cells[index, contReg].Value = gndse[registro + y].strPmGnd5PU;
                            ws.Cells[index, contReg].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, contReg].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }

                        registro = registro + saltoRegistro - 1;//20190318 - NET: Corrección

                    }


                }

                xlPackage.Save();
            }
        }

        public static void GenerarRepDemandaPorBloque(string fileName, List<PmoDatDbfDTO> lPmoDatDbfDTO1)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DemandaPorBloque");

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {

                    ws.Cells[2, 1].Value = "Demanda por Bloque";
                    ws.Cells[2, 1, 2, 13].Merge = true;
                    ws.Cells[2, 1, 2, 13].Style.Font.Bold = true;
                    ws.Cells[2, 1, 2, 13].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[2, 1, 2, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;



                    ws.Cells[4, 1].Value = "Demanda";
                    ws.Cells[4, 1, 4, 3].Merge = true;
                    ws.Cells[4, 4].Value = "Vegetativa";
                    ws.Cells[4, 4, 4, 9].Merge = true;
                    ws.Cells[4, 10].Value = "Industrial";
                    ws.Cells[4, 10, 4, 15].Merge = true;


                    int index = 5;
                    //obtenemos las cabeceras del reporte                                                           
                    ws.Cells[index, 1].Value = "Nombre Barra";
                    ws.Column(1).Width = 30;
                    ws.Cells[index, 2].Value = "Num.";
                    ws.Column(2).Width = 10;
                    ws.Cells[index, 3].Value = "Fecha";
                    ws.Column(3).Width = 11;

                    ws.Cells[index, 4].Value = "BL1";
                    ws.Column(4).Width = 10;
                    ws.Cells[index, 5].Value = "BL2";
                    ws.Column(5).Width = 10;
                    ws.Cells[index, 6].Value = "BL3";
                    ws.Column(6).Width = 10;
                    ws.Cells[index, 7].Value = "BL4";
                    ws.Column(7).Width = 10;
                    ws.Cells[index, 8].Value = "BL5";
                    ws.Column(8).Width = 10;
                    ws.Cells[index, 9].Value = "TOTAL";
                    ws.Column(9).Width = 10;

                    ws.Cells[index, 10].Value = "BL1";
                    ws.Column(10).Width = 10;
                    ws.Cells[index, 11].Value = "BL2";
                    ws.Column(11).Width = 10;
                    ws.Cells[index, 12].Value = "BL3";
                    ws.Column(12).Width = 10;
                    ws.Cells[index, 13].Value = "BL4";
                    ws.Column(13).Width = 10;
                    ws.Cells[index, 14].Value = "BL5";
                    ws.Column(14).Width = 10;
                    ws.Cells[index, 15].Value = "TOTAL";
                    ws.Column(15).Width = 10;

                    ws.Cells[4, 16].Value = "TOTAL";
                    ws.Cells[4, 16, 5, 16].Merge = true;

                    //int cont = 3;                    

                    ExcelRange rg = ws.Cells[4, 1, index, 16];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    var lPmoDatDbfDTO = lPmoDatDbfDTO1.OrderBy(p => p.PmDbf5FecIni).ThenBy(p => p.BusName).ThenBy(p => p.LCod).ThenBy(p => p.PmBloqCodi);

                    string nombreBarra = "";
                    int saltoRegistro = 10;
                    int contReg = 0;
                    decimal totalVegetativaReg = 0;
                    decimal totalIndustrialReg = 0;

                    string fechaSein = "";
                    decimal totalVegeBL1 = 0;
                    decimal totalVegeBL2 = 0;
                    decimal totalVegeBL3 = 0;
                    decimal totalVegeBL4 = 0;
                    decimal totalVegeBL5 = 0;

                    decimal totalInduBL1 = 0;
                    decimal totalInduBL2 = 0;
                    decimal totalInduBL3 = 0;
                    decimal totalInduBL4 = 0;
                    decimal totalInduBL5 = 0;


                    index++;

                    foreach (var datDbfDTO in lPmoDatDbfDTO)
                    {
                        //CALCULO DE TOTALES DEL REGISTRO - INICIO

                        //Para la primera vez que entra al bucle
                        if (nombreBarra == "")
                            nombreBarra = datDbfDTO.BusName;

                        //if (contReg == saltoRegistro)
                        if (!nombreBarra.Equals(datDbfDTO.BusName))
                        {
                            //registramos los totales por registro
                            ws.Cells[index, 9].Value = totalVegetativaReg;
                            ws.Cells[index, 9].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            ws.Cells[index, 15].Value = totalIndustrialReg;
                            ws.Cells[index, 15].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            ws.Cells[index, 16].Value = totalVegetativaReg + totalIndustrialReg;
                            ws.Cells[index, 16].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            //Reiniciamos variables
                            nombreBarra = datDbfDTO.BusName;
                            index++;
                            contReg = 0;
                            totalVegetativaReg = 0;
                            totalIndustrialReg = 0;
                        }

                        //CALCULO DE TOTALES DEL REGISTRO - FIN






                        //CALCULO DE TOTALES SEIN X FECHA - INICIO

                        //Para la primera vez que entra al bucle
                        if (fechaSein == "")
                            fechaSein = datDbfDTO.Fecha;

                        //Cuando cambia la fechaSein pintamos los totales
                        if (!fechaSein.Equals(datDbfDTO.Fecha))
                        {
                            ws.Cells[index, 1].Value = "TOTAL SEIN";
                            //ws.Cells[index, 2].Value = "";
                            ws.Cells[index, 3].Value = fechaSein;
                            ws.Cells[index, 4].Value = totalVegeBL1;
                            ws.Cells[index, 4].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 5].Value = totalVegeBL2;
                            ws.Cells[index, 5].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 6].Value = totalVegeBL3;
                            ws.Cells[index, 6].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 7].Value = totalVegeBL4;
                            ws.Cells[index, 7].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 8].Value = totalVegeBL5;
                            ws.Cells[index, 8].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 9].Value = totalVegeBL1 + totalVegeBL2 + totalVegeBL3 + totalVegeBL4 + totalVegeBL5;
                            ws.Cells[index, 9].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            ws.Cells[index, 10].Value = totalInduBL1;
                            ws.Cells[index, 10].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 11].Value = totalInduBL2;
                            ws.Cells[index, 11].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 12].Value = totalInduBL3;
                            ws.Cells[index, 12].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 13].Value = totalInduBL4;
                            ws.Cells[index, 13].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 14].Value = totalInduBL5;
                            ws.Cells[index, 14].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[index, 15].Value = totalInduBL1 + totalInduBL2 + totalInduBL3 + totalInduBL4 + totalInduBL5;
                            ws.Cells[index, 15].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            ws.Cells[index, 16].Value = totalVegeBL1 + totalVegeBL2 + totalVegeBL3 + totalVegeBL4 + totalVegeBL5 + totalInduBL1 + totalInduBL2 + totalInduBL3 + totalInduBL4 + totalInduBL5;
                            ws.Cells[index, 16].Style.Numberformat.Format = "#,##0.0";
                            ws.Cells[index, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            //Reiniciamos variables totalizadores
                            fechaSein = datDbfDTO.Fecha;
                            totalVegeBL1 = 0;
                            totalVegeBL2 = 0;
                            totalVegeBL3 = 0;
                            totalVegeBL4 = 0;
                            totalVegeBL5 = 0;
                            totalInduBL1 = 0;
                            totalInduBL2 = 0;
                            totalInduBL3 = 0;
                            totalInduBL4 = 0;
                            totalInduBL5 = 0;
                            index++;
                        }
                        //CALCULO DE TOTALES SEIN X FECHA - FIN



                        //Lógica del bucle para cada registro
                        contReg++;
                        if (contReg == 1)
                        {
                            ws.Cells[index, 1].Value = datDbfDTO.BusName;
                            ws.Cells[index, 2].Value = datDbfDTO.GrupoCodiSDDP;
                            ws.Cells[index, 3].Value = datDbfDTO.Fecha;

                        }

                        if (datDbfDTO.LCod.Trim().Equals("1"))
                        {
                            switch (datDbfDTO.Llev.Trim())
                            {
                                case "1":
                                    ws.Cells[index, 4].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 4].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalVegetativaReg = totalVegetativaReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalVegeBL1 = totalVegeBL1 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                                case "2":
                                    ws.Cells[index, 5].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 5].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalVegetativaReg = totalVegetativaReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalVegeBL2 = totalVegeBL2 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                                case "3":
                                    ws.Cells[index, 6].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 6].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalVegetativaReg = totalVegetativaReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalVegeBL3 = totalVegeBL3 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                                case "4":
                                    ws.Cells[index, 7].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 7].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalVegetativaReg = totalVegetativaReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalVegeBL4 = totalVegeBL4 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                                case "5":
                                    ws.Cells[index, 8].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 8].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalVegetativaReg = totalVegetativaReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalVegeBL5 = totalVegeBL5 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                            }
                        }

                        if (datDbfDTO.LCod.Trim().Equals("501"))
                        {
                            switch (datDbfDTO.Llev.Trim())
                            {
                                case "1":
                                    ws.Cells[index, 10].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 10].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalIndustrialReg = totalIndustrialReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalInduBL1 = totalInduBL1 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                                case "2":
                                    ws.Cells[index, 11].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 11].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalIndustrialReg = totalIndustrialReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalInduBL2 = totalInduBL2 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                                case "3":
                                    ws.Cells[index, 12].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 12].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalIndustrialReg = totalIndustrialReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalInduBL3 = totalInduBL3 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                                case "4":
                                    ws.Cells[index, 13].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 13].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalIndustrialReg = totalIndustrialReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalInduBL4 = totalInduBL4 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                                case "5":
                                    ws.Cells[index, 14].Value = datDbfDTO.PmDbf5Carga;
                                    ws.Cells[index, 14].Style.Numberformat.Format = "#,##0.0";
                                    ws.Cells[index, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    totalIndustrialReg = totalIndustrialReg + (decimal)datDbfDTO.PmDbf5Carga;
                                    totalInduBL5 = totalInduBL5 + (decimal)datDbfDTO.PmDbf5Carga;
                                    break;
                            }
                        }



                    }


                    //consolidados del último registro

                    ws.Cells[index, 9].Value = totalVegetativaReg;
                    ws.Cells[index, 9].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    ws.Cells[index, 15].Value = totalIndustrialReg;
                    ws.Cells[index, 15].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    ws.Cells[index, 16].Value = totalVegetativaReg + totalIndustrialReg;
                    ws.Cells[index, 16].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    index++;

                    //totales SEIN de la última fecha

                    ws.Cells[index, 1].Value = "TOTAL SEIN";
                    //ws.Cells[index, 2].Value = "";
                    ws.Cells[index, 3].Value = fechaSein;
                    ws.Cells[index, 4].Value = totalVegeBL1;
                    ws.Cells[index, 4].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 5].Value = totalVegeBL2;
                    ws.Cells[index, 5].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 6].Value = totalVegeBL3;
                    ws.Cells[index, 6].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 7].Value = totalVegeBL4;
                    ws.Cells[index, 7].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 8].Value = totalVegeBL5;
                    ws.Cells[index, 8].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 9].Value = totalVegeBL1 + totalVegeBL2 + totalVegeBL3 + totalVegeBL4 + totalVegeBL5;
                    ws.Cells[index, 9].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    ws.Cells[index, 10].Value = totalInduBL1;
                    ws.Cells[index, 10].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 11].Value = totalInduBL2;
                    ws.Cells[index, 11].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 12].Value = totalInduBL3;
                    ws.Cells[index, 12].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 13].Value = totalInduBL4;
                    ws.Cells[index, 13].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 14].Value = totalInduBL5;
                    ws.Cells[index, 14].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[index, 15].Value = totalInduBL1 + totalInduBL2 + totalInduBL3 + totalInduBL4 + totalInduBL5;
                    ws.Cells[index, 15].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    ws.Cells[index, 16].Value = totalVegeBL1 + totalVegeBL2 + totalVegeBL3 + totalVegeBL4 + totalVegeBL5 + totalInduBL1 + totalInduBL2 + totalInduBL3 + totalInduBL4 + totalInduBL5;
                    ws.Cells[index, 16].Style.Numberformat.Format = "#,##0.0";
                    ws.Cells[index, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                }

                xlPackage.Save();

            }
        }


        public static void GenerarRepGrupoRelaso(string fileName, List<PrGrupoRelasoDTO> lPrGrupoRelasoDTO)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RelacionGrupos");

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {

                    ws.Cells[2, 1].Value = "Relación de Grupos SDDP - SIC";
                    ws.Cells[2, 1, 2, 10].Merge = true;
                    ws.Cells[2, 1, 2, 10].Style.Font.Bold = true;
                    ws.Cells[2, 1, 2, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[2, 1, 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    int index = 4;
                    //obtenemos las cabeceras del reporte                                                           
                    ws.Cells[index, 1].Value = "Cod. Relación";
                    ws.Column(1).Width = 11;
                    ws.Cells[index, 2].Value = "Tipo Relación";
                    ws.Column(2).Width = 33;
                    ws.Cells[index, 3].Value = "Cod. SDDP";
                    ws.Column(3).Width = 10;
                    ws.Cells[index, 4].Value = "Descripción SDDP";
                    ws.Column(4).Width = 30;
                    ws.Cells[index, 5].Value = "Cod. SIC";
                    ws.Column(5).Width = 10;
                    ws.Cells[index, 6].Value = "Descripción SIC";
                    ws.Column(6).Width = 30;
                    ws.Cells[index, 7].Value = "TAG";
                    ws.Column(7).Width = 8;
                    ws.Cells[index, 8].Value = "Secuencia";
                    ws.Column(8).Width = 10;
                    ws.Cells[index, 9].Value = "Fecha Mod.";
                    ws.Column(9).Width = 15;
                    ws.Cells[index, 10].Value = "Usuario Mod.";
                    ws.Column(10).Width = 15;

                    ExcelRange rg = ws.Cells[index, 1, index, 10];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    foreach (var grupoRelasoDTO in lPrGrupoRelasoDTO)
                    {
                        index++;
                        ws.Cells[index, 1].Value = grupoRelasoDTO.Grrasocodi;
                        ws.Cells[index, 2].Value = grupoRelasoDTO.Tiporel;
                        ws.Cells[index, 3].Value = grupoRelasoDTO.Codsddp;
                        ws.Cells[index, 4].Value = grupoRelasoDTO.descsddp;
                        ws.Cells[index, 5].Value = grupoRelasoDTO.Codsic;
                        ws.Cells[index, 6].Value = grupoRelasoDTO.Descsic;
                        ws.Cells[index, 7].Value = grupoRelasoDTO.Grrasotag;
                        ws.Cells[index, 8].Value = grupoRelasoDTO.Grrasosecuencia;
                        ws.Cells[index, 9].Value = grupoRelasoDTO.Grrasofecmodificacion;
                        ws.Cells[index, 9].Style.Numberformat.Format = "dd/mm/yyyy";
                        ws.Cells[index, 10].Value = grupoRelasoDTO.Grrasousumodificacion;

                    }

                }

                xlPackage.Save();

            }
        }

    }
}
