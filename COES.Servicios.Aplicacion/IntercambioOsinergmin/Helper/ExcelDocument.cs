using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using System;
using System.Data;

namespace COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper
{
    public class ExcelDocument
    {

        struct ExcelStruct
        {
            public int indice;
            public string nombre;
            public string tipo;
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
                //rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
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

        public static void GenerarFormatoExcelLogErrores(string fileName, List<IioLogRemisionDTO> list)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Log de errores");

                ws.Cells[6, 1].Value = "LOG DE ERRORES";

                ExcelRange rg = ws.Cells[6, 1, 6, 2];
                rg.Merge = true;
                rg = ObtenerEstiloCelda(rg, 1);

                DateTime fecha = DateTime.Now;

                ws.Cells[8, 1].Value = "Fecha y Hora: " + fecha;

                ExcelRange rg1 = ws.Cells[8, 1, 8, 2];
                rg1.Merge = true;


                if (ws != null)
                {
                    int index = 10;

                    ws.Cells[index, 1].Value = "Linea";
                    ws.Cells[index, 2].Value = "Descripción del error";

                    rg = ws.Cells[index, 1, index, 2];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (IioLogRemisionDTO item in list)
                    {
                        ws.Cells[index, 1].Value = item.RlogNroLinea;
                        ws.Cells[index, 2].Value = item.RlogDescripcionError;
                        rg = ws.Cells[index, 1, index, 2];
                        rg = ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    ws.Column(1).Width = 5;
                    ws.Column(2).Width = 100;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        public static void GenerarFormatoDataReader(string fileName, IDataReader dr)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Archivo de carga");

                int col = 0;
                if (ws != null)
                {
                    int index = 6;
                    using (dr)
                    {
                        List<ExcelStruct> columns = new List<ExcelStruct>();

                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            col++;
                            ExcelStruct reg = new ExcelStruct();
                            reg.indice = i;
                            reg.nombre = dr.GetName(i).Trim();
                            reg.tipo = dr.GetDataTypeName(i).Trim();
                            if (reg.tipo.Contains("Double") || reg.tipo.Contains("Float"))
                            {
                                reg.tipo = "Decimal";
                            }
                            else if (reg.tipo.Contains("Int"))
                            {
                                reg.tipo = "Int";
                            }
                            else if (reg.tipo.Contains("Date"))
                            {
                                reg.tipo = "Date";
                            }
                            columns.Add(reg);
                            ws.Cells[index, col].Value = dr.GetName(i);
                        }
                        ExcelRange rg = ws.Cells[index, 1, index, dr.FieldCount];
                        ObtenerEstiloCelda(rg, 1);
                        index++;

                        while (dr.Read())
                        {
                            int cont = 0;
                            foreach (ExcelStruct item in columns)
                            {
                                cont++;
                                var valor = dr[item.nombre];
                                if (valor != null)
                                {
                                    if (item.tipo.Equals("Decimal"))
                                    {
                                        ws.Cells[index, cont].Value = Double.Parse(dr[item.nombre].ToString());
                                        ws.Cells[index, cont].Style.Numberformat.Format = "#0.00";
                                    }
                                    else if (item.tipo.Contains("Int"))
                                    {
                                        ws.Cells[index, cont].Value = Int64.Parse(dr[item.nombre].ToString());
                                    }
                                    else if (item.tipo.Contains("Date"))
                                    {
                                        ws.Cells[index, cont].Value = dr.GetDateTime(item.indice);
                                    }
                                    else
                                    {
                                        ws.Cells[index, cont].Value = dr[item.nombre].ToString();
                                    }
                                }
                            }
                            rg = ws.Cells[index, 1, index, cont];
                            ObtenerEstiloCelda(rg, 0);
                            index++;
                        }
                    }
                    for (int i = 1; i <= col; i++)
                    {
                        ws.Column(i).Width = 20;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }

        }

        public static void GenerarFormato(string fileName, IEnumerable<EntidadListadoDTO> list, string entidad)
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
                    int index = 6;

                    ws.Cells[index, 1].Value = "CÓDIGO";

                    if (entidad.Equals("Empresa") || entidad.Equals("UsuarioLibre"))
                    {
                        ws.Cells[index, 2].Value = "RAZÓN SOCIAL";
                    }
                    else
                    {
                        ws.Cells[index, 2].Value = "DESCRIPCIÓN";
                    }
                    ws.Cells[index, 3].Value = "COD. OSINERGMIN";

                    int col = 3;

                    if (entidad.Equals("Empresa"))
                    {
                        ws.Cells[index, 4].Value = "TIPO DE EMPRESA";
                        col = 4;
                    }

                    ExcelRange rg = ws.Cells[index, 1, index, col];
                    ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (EntidadListadoDTO item in list)
                    {
                        ws.Cells[index, 1].Value = item.EntidadCodigo;
                        ws.Cells[index, 2].Value = item.Descripcion;
                        ws.Cells[index, 3].Value = item.CodigoOsinergmin;
                        if (entidad.Equals("Empresa"))
                        {
                            ws.Cells[index, 4].Value = item.CampoAdicional;
                        }
                        rg = ws.Cells[index, 1, index, col];
                        ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    ws.Column(1).Width = 15;
                    ws.Column(2).Width = 50;
                    ws.Column(3).Width = 18;

                    if (entidad.Equals("Empresa"))
                    {
                        ws.Column(4).Width = 20;
                    }


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permige generar el reporte a Excel de los datos cargados en la tabla 05.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="lIioTabla04DTO"></param>
        internal static void GenerarReporteTabla04ImportacionSicli(string nombreArchivo, List<IioTabla04DTO> lIioTabla04DTO)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");

                if (ws != null)
                {
                    int index = 6;

                    ws.Cells[index, 1].Value = "Id Suministrador";
                    ws.Cells[index, 2].Value = "Cód. Suministrador SICLI";
                    ws.Cells[index, 3].Value = "Suministrador SICLI";
                    ws.Cells[index, 4].Value = "RUC";
                    ws.Cells[index, 5].Value = "CIIU";
                    ws.Cells[index, 6].Value = "Usuario Libre";
                    ws.Cells[index, 7].Value = "Cód. Suministro";
                    ws.Cells[index, 8].Value = "Nombre Pto. Medición";
                    ws.Cells[index, 9].Value = "Fecha de medición";

                    int contador = 9;
                    for (int i = 1; i <= 96; i++)
                    {
                        contador ++;
                        ws.Cells[index, contador].Value = "H" + i.ToString();
                    }

                    ExcelRange rg = ws.Cells[index, 1, index, contador];
                    ObtenerEstiloCelda(rg, 1);

                    index++;

                    foreach (IioTabla04DTO iioTabla04DTO in lIioTabla04DTO)
                    {
                        ws.Cells[index, 1].Value = iioTabla04DTO.IdSuministrador;
                        ws.Cells[index, 2].Value = iioTabla04DTO.CodSuministradorSicli;
                        ws.Cells[index, 3].Value = iioTabla04DTO.SuministradorSicli;
                        ws.Cells[index, 4].Value = iioTabla04DTO.Ruc;
                        ws.Cells[index, 5].Value = iioTabla04DTO.Ciiu;
                        ws.Cells[index, 6].Value = iioTabla04DTO.UsuarioLibre;
                        ws.Cells[index, 7].Value = iioTabla04DTO.CodSuministro;
                        ws.Cells[index, 8].Value = iioTabla04DTO.NombrePtoMedicion;
                        ws.Cells[index, 9].Value = iioTabla04DTO.FechaMedicion.ToString(ConstantesAppServicio.FormatoFecha);

                        ws.Cells[index, 10].Value = iioTabla04DTO.H1;
                        ws.Cells[index, 11].Value = iioTabla04DTO.H2;
                        ws.Cells[index, 12].Value = iioTabla04DTO.H3;
                        ws.Cells[index, 13].Value = iioTabla04DTO.H4;
                        ws.Cells[index, 14].Value = iioTabla04DTO.H5;
                        ws.Cells[index, 15].Value = iioTabla04DTO.H6;
                        ws.Cells[index, 16].Value = iioTabla04DTO.H7;
                        ws.Cells[index, 17].Value = iioTabla04DTO.H8;
                        ws.Cells[index, 18].Value = iioTabla04DTO.H9;
                        ws.Cells[index, 19].Value = iioTabla04DTO.H10;
                        ws.Cells[index, 20].Value = iioTabla04DTO.H11;
                        ws.Cells[index, 21].Value = iioTabla04DTO.H12;
                        ws.Cells[index, 22].Value = iioTabla04DTO.H13;
                        ws.Cells[index, 23].Value = iioTabla04DTO.H14;
                        ws.Cells[index, 24].Value = iioTabla04DTO.H15;
                        ws.Cells[index, 25].Value = iioTabla04DTO.H16;
                        ws.Cells[index, 26].Value = iioTabla04DTO.H17;
                        ws.Cells[index, 27].Value = iioTabla04DTO.H18;
                        ws.Cells[index, 28].Value = iioTabla04DTO.H19;
                        ws.Cells[index, 29].Value = iioTabla04DTO.H20;
                        ws.Cells[index, 30].Value = iioTabla04DTO.H21;
                        ws.Cells[index, 31].Value = iioTabla04DTO.H22;
                        ws.Cells[index, 32].Value = iioTabla04DTO.H23;
                        ws.Cells[index, 33].Value = iioTabla04DTO.H24;
                        ws.Cells[index, 34].Value = iioTabla04DTO.H25;
                        ws.Cells[index, 35].Value = iioTabla04DTO.H26;
                        ws.Cells[index, 36].Value = iioTabla04DTO.H27;
                        ws.Cells[index, 37].Value = iioTabla04DTO.H28;
                        ws.Cells[index, 38].Value = iioTabla04DTO.H29;
                        ws.Cells[index, 39].Value = iioTabla04DTO.H30;
                        ws.Cells[index, 40].Value = iioTabla04DTO.H31;
                        ws.Cells[index, 41].Value = iioTabla04DTO.H32;
                        ws.Cells[index, 42].Value = iioTabla04DTO.H33;
                        ws.Cells[index, 43].Value = iioTabla04DTO.H34;
                        ws.Cells[index, 44].Value = iioTabla04DTO.H35;
                        ws.Cells[index, 45].Value = iioTabla04DTO.H36;
                        ws.Cells[index, 46].Value = iioTabla04DTO.H37;
                        ws.Cells[index, 47].Value = iioTabla04DTO.H38;
                        ws.Cells[index, 48].Value = iioTabla04DTO.H39;
                        ws.Cells[index, 49].Value = iioTabla04DTO.H40;
                        ws.Cells[index, 50].Value = iioTabla04DTO.H41;
                        ws.Cells[index, 51].Value = iioTabla04DTO.H42;
                        ws.Cells[index, 52].Value = iioTabla04DTO.H43;
                        ws.Cells[index, 53].Value = iioTabla04DTO.H44;
                        ws.Cells[index, 54].Value = iioTabla04DTO.H45;
                        ws.Cells[index, 55].Value = iioTabla04DTO.H46;
                        ws.Cells[index, 56].Value = iioTabla04DTO.H47;
                        ws.Cells[index, 57].Value = iioTabla04DTO.H48;
                        ws.Cells[index, 58].Value = iioTabla04DTO.H49;
                        ws.Cells[index, 59].Value = iioTabla04DTO.H50;
                        ws.Cells[index, 60].Value = iioTabla04DTO.H51;
                        ws.Cells[index, 61].Value = iioTabla04DTO.H52;
                        ws.Cells[index, 62].Value = iioTabla04DTO.H53;
                        ws.Cells[index, 63].Value = iioTabla04DTO.H54;
                        ws.Cells[index, 64].Value = iioTabla04DTO.H55;
                        ws.Cells[index, 65].Value = iioTabla04DTO.H56;
                        ws.Cells[index, 66].Value = iioTabla04DTO.H57;
                        ws.Cells[index, 67].Value = iioTabla04DTO.H58;
                        ws.Cells[index, 68].Value = iioTabla04DTO.H59;
                        ws.Cells[index, 69].Value = iioTabla04DTO.H60;
                        ws.Cells[index, 70].Value = iioTabla04DTO.H61;
                        ws.Cells[index, 71].Value = iioTabla04DTO.H62;
                        ws.Cells[index, 72].Value = iioTabla04DTO.H63;
                        ws.Cells[index, 73].Value = iioTabla04DTO.H64;
                        ws.Cells[index, 74].Value = iioTabla04DTO.H65;
                        ws.Cells[index, 75].Value = iioTabla04DTO.H66;
                        ws.Cells[index, 76].Value = iioTabla04DTO.H67;
                        ws.Cells[index, 77].Value = iioTabla04DTO.H68;
                        ws.Cells[index, 78].Value = iioTabla04DTO.H69;
                        ws.Cells[index, 79].Value = iioTabla04DTO.H70;
                        ws.Cells[index, 80].Value = iioTabla04DTO.H71;
                        ws.Cells[index, 81].Value = iioTabla04DTO.H72;
                        ws.Cells[index, 82].Value = iioTabla04DTO.H73;
                        ws.Cells[index, 83].Value = iioTabla04DTO.H74;
                        ws.Cells[index, 84].Value = iioTabla04DTO.H75;
                        ws.Cells[index, 85].Value = iioTabla04DTO.H76;
                        ws.Cells[index, 86].Value = iioTabla04DTO.H77;
                        ws.Cells[index, 87].Value = iioTabla04DTO.H78;
                        ws.Cells[index, 88].Value = iioTabla04DTO.H79;
                        ws.Cells[index, 89].Value = iioTabla04DTO.H80;
                        ws.Cells[index, 90].Value = iioTabla04DTO.H81;
                        ws.Cells[index, 91].Value = iioTabla04DTO.H82;
                        ws.Cells[index, 92].Value = iioTabla04DTO.H83;
                        ws.Cells[index, 93].Value = iioTabla04DTO.H84;
                        ws.Cells[index, 94].Value = iioTabla04DTO.H85;
                        ws.Cells[index, 95].Value = iioTabla04DTO.H86;
                        ws.Cells[index, 96].Value = iioTabla04DTO.H87;
                        ws.Cells[index, 97].Value = iioTabla04DTO.H88;
                        ws.Cells[index, 98].Value = iioTabla04DTO.H89;
                        ws.Cells[index, 99].Value = iioTabla04DTO.H90;
                        ws.Cells[index, 100].Value = iioTabla04DTO.H91;
                        ws.Cells[index, 101].Value = iioTabla04DTO.H92;
                        ws.Cells[index, 102].Value = iioTabla04DTO.H93;
                        ws.Cells[index, 103].Value = iioTabla04DTO.H94;
                        ws.Cells[index, 104].Value = iioTabla04DTO.H95;
                        ws.Cells[index, 105].Value = iioTabla04DTO.H96;

                        rg = ws.Cells[index, 1, index, 105];
                        ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    ws.Column(1).Width = 15;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 30;
                    ws.Column(9).Width = 15;

                    contador = 9;

                    for (int i = 1; i <= 96; i++)
                    {
                        contador++;
                        ws.Column(contador).Width = 10;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 20);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permige generar el reporte a Excel de los datos cargados en la tabla 05.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="lIioTabla05DTO"></param>
        internal static void GenerarReporteTabla05ImportacionSicli(string nombreArchivo, List<IioTabla05DTO> lIioTabla05DTO)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");

                if (ws != null)
                {
                    int index = 6;

                    ws.Cells[index, 1].Value = "Id Suministrador";
                    ws.Cells[index, 2].Value = "Cód. Suministrador SICLI";
                    ws.Cells[index, 3].Value = "Suministrador SICLI";
                    ws.Cells[index, 4].Value = "RUC";
                    ws.Cells[index, 5].Value = "Usuario Libre";
                    ws.Cells[index, 6].Value = "Cód. Suministro";
                    ws.Cells[index, 7].Value = "Barra Suministro";
                    ws.Cells[index, 8].Value = "Área Demanda";
                    ws.Cells[index, 9].Value = "Paga VAD";
                    ws.Cells[index, 10].Value = "Consumo Energía HP";
                    ws.Cells[index, 11].Value = "Consumo Energía HFP";
                    ws.Cells[index, 12].Value = "Máxima Demanda HP";
                    ws.Cells[index, 13].Value = "Máxima Demanda HFP";

                    ExcelRange rg = ws.Cells[index, 1, index, 13];
                    ObtenerEstiloCelda(rg, 1);

                    index++;

                    foreach (IioTabla05DTO item in lIioTabla05DTO)
                    {
                        ws.Cells[index, 1].Value = item.IdSuministrador;
                        ws.Cells[index, 2].Value = item.CodSuministradorSicli;
                        ws.Cells[index, 3].Value = item.SuministradorSicli;
                        ws.Cells[index, 4].Value = item.Ruc;
                        ws.Cells[index, 5].Value = item.UsuarioLibre;
                        ws.Cells[index, 6].Value = item.CodUsuarioLibre;
                        ws.Cells[index, 7].Value = item.BarraSuministro;
                        ws.Cells[index, 8].Value = item.AreaDemanda;
                        ws.Cells[index, 9].Value = item.PagaVad;
                        ws.Cells[index, 10].Value = item.ConsumoEnergiaHp;
                        ws.Cells[index, 11].Value = item.ConsumoEnergiaHfp;
                        ws.Cells[index, 12].Value = item.MaximaDemandaHp;
                        ws.Cells[index, 13].Value = item.MaximaDemandaHfp;

                        rg = ws.Cells[index, 1, index, 13];
                        ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    ws.Column(1).Width = 15;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 30;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 18;
                    ws.Column(11).Width = 18;
                    ws.Column(12).Width = 18;
                    ws.Column(13).Width = 18;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 20);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Construye el Excel de los maestros de empresas.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="empresas"></param>
        internal static void GenerarFormatoMaestroEmpresa(string nombreArchivo, empresaDTO[] empresas)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Maestro");

                if (ws != null)
                {
                    int fila = 6;
                    int columna = 0;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código COES";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Empresa";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Ubigeo";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Descripción Corta";
                    ws.Column(columna).Width = 30;

                    columna++;
                    ws.Cells[fila, columna].Value = "Descripción";
                    ws.Column(columna).Width = 40;

                    columna++;
                    ws.Cells[fila, columna].Value = "Dirección";
                    ws.Column(columna).Width = 50;

                    ExcelRange rg = ws.Cells[fila, 1, fila, columna];
                    ObtenerEstiloCelda(rg, 1);

                    fila++;                    
                    foreach (empresaDTO empresa in empresas)
                    {
                        columna = 0;

                        columna++;
                        ws.Cells[fila, columna].Value = empresa.codigoCoes;

                        columna++;
                        ws.Cells[fila, columna].Value = empresa.codigoEmpresa;

                        columna++;
                        ws.Cells[fila, columna].Value = empresa.codigoUbigeo;

                        columna++;
                        ws.Cells[fila, columna].Value = empresa.descCortaEmpresa;

                        columna++;
                        ws.Cells[fila, columna].Value = empresa.descEmpresa;

                        columna++;
                        ws.Cells[fila, columna].Value = empresa.direccion;

                        rg = ws.Cells[fila, 1, fila, columna];

                        ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 4);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Construye el Excel de los maestros de usuarios libres.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="usuarios"></param>
        internal static void GenerarFormatoMaestroUsuarioLibre(string nombreArchivo, usuarioLibreDTO[] usuarios)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Maestro");

                if (ws != null)
                {
                    int fila = 6;
                    int columna = 0;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código COES";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Usuario Libre";
                    ws.Column(columna).Width = 20;

                    columna++;
                    ws.Cells[fila, columna].Value = "Razón Social";
                    ws.Column(columna).Width = 30;

                    columna++;
                    ws.Cells[fila, columna].Value = "Terminal";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Usuario";
                    ws.Column(columna).Width = 15;

                    ExcelRange rg = ws.Cells[fila, 1, fila, columna];
                    ObtenerEstiloCelda(rg, 1);

                    fila++;
                    foreach (usuarioLibreDTO usuario in usuarios)
                    {
                        columna = 0;

                        columna++;
                        ws.Cells[fila, columna].Value = usuario.codigoCoes;

                        columna++;
                        ws.Cells[fila, columna].Value = usuario.codigoUsuarioLibre;

                        columna++;
                        ws.Cells[fila, columna].Value = usuario.razonSocial;

                        columna++;
                        ws.Cells[fila, columna].Value = usuario.terminal;

                        columna++;
                        ws.Cells[fila, columna].Value = usuario.usuario;

                        rg = ws.Cells[fila, 1, fila, columna];

                        ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 4);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Construye el Excel de los maestros de suministros.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="suministros"></param>
        internal static void GenerarFormatoMaestroSuministro(string nombreArchivo, suministroUsuarioDTO[] suministros)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Maestro");

                if (ws != null)
                {
                    int fila = 6;
                    int columna = 0;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código COES";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Suministro Usuario";
                    ws.Column(columna).Width = 22;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Usuario Libre";
                    ws.Column(columna).Width = 22;

                    columna++;
                    ws.Cells[fila, columna].Value = "Nombre Usuario Libre";
                    ws.Column(columna).Width = 40;

                    columna++;
                    ws.Cells[fila, columna].Value = "Terminal";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Usuario";
                    ws.Column(columna).Width = 15;

                    ExcelRange rg = ws.Cells[fila, 1, fila, columna];
                    ObtenerEstiloCelda(rg, 1);

                    fila++;
                    foreach (suministroUsuarioDTO suministro in suministros)
                    {
                        columna = 0;

                        columna++;
                        ws.Cells[fila, columna].Value = suministro.codigoCoes;

                        columna++;
                        ws.Cells[fila, columna].Value = suministro.codigoSuministroUsuario;

                        columna++;
                        ws.Cells[fila, columna].Value = suministro.codigoUsuarioLibre;

                        columna++;
                        ws.Cells[fila, columna].Value = suministro.nombreUsuarioLibre;

                        columna++;
                        ws.Cells[fila, columna].Value = suministro.terminal;

                        columna++;
                        ws.Cells[fila, columna].Value = suministro.usuario;

                        rg = ws.Cells[fila, 1, fila, columna];

                        ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 4);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Construye el Excel de los maestros de centrales de generación.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="suministros"></param>
        internal static void GenerarFormatoMaestroCentralGeneracion(string nombreArchivo, centralGeneracionDTO[] centralesGeneracion)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Maestro");

                if (ws != null)
                {
                    int fila = 6;
                    int columna = 0;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código COES";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Central Generación";
                    ws.Column(columna).Width = 22;

                    columna++;
                    ws.Cells[fila, columna].Value = "Nombre Central Generación";
                    ws.Column(columna).Width = 30;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Tipo Central";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Ubigeo";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Dirección";
                    ws.Column(columna).Width = 40;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Empresa";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Terminal";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Usuario";
                    ws.Column(columna).Width = 15;

                    ExcelRange rg = ws.Cells[fila, 1, fila, columna];
                    ObtenerEstiloCelda(rg, 1);

                    fila++;
                    foreach (centralGeneracionDTO centralGeneracion in centralesGeneracion)
                    {
                        columna = 0;

                        columna++;
                        ws.Cells[fila, columna].Value = centralGeneracion.codigoCoes;

                        columna++;
                        ws.Cells[fila, columna].Value = centralGeneracion.codigoCentralGeneracion;

                        columna++;
                        ws.Cells[fila, columna].Value = centralGeneracion.nombreCentralGeneracion;

                        columna++;
                        ws.Cells[fila, columna].Value = centralGeneracion.codigoTipoCentral;

                        columna++;
                        ws.Cells[fila, columna].Value = centralGeneracion.codUbigeo;

                        columna++;
                        ws.Cells[fila, columna].Value = centralGeneracion.direccion;

                        columna++;
                        ws.Cells[fila, columna].Value = centralGeneracion.codigoEmpresa;

                        columna++;
                        ws.Cells[fila, columna].Value = centralGeneracion.terminal;

                        columna++;
                        ws.Cells[fila, columna].Value = centralGeneracion.usuario;

                        rg = ws.Cells[fila, 1, fila, columna];

                        ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 4);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Construye el Excel de los maestros de grupos de generación.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="suministros"></param>
        internal static void GenerarFormatoMaestroGrupoGeneracion(string nombreArchivo, grupoGeneracionDTO[] gruposGeneracion)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Maestro");

                if (ws != null)
                {
                    int fila = 6;
                    int columna = 0;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código COES";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Central Generación";
                    ws.Column(columna).Width = 22;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Grupo Generación";
                    ws.Column(columna).Width = 22;

                    columna++;
                    ws.Cells[fila, columna].Value = "Nombre Grupo Generación";
                    ws.Column(columna).Width = 30;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Tipo Combustible";
                    ws.Column(columna).Width = 20;

                    columna++;
                    ws.Cells[fila, columna].Value = "Terminal";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Usuario";
                    ws.Column(columna).Width = 15;

                    ExcelRange rg = ws.Cells[fila, 1, fila, columna];
                    ObtenerEstiloCelda(rg, 1);

                    fila++;
                    foreach (grupoGeneracionDTO grupoGeneracion in gruposGeneracion)
                    {
                        columna = 0;

                        columna++;
                        ws.Cells[fila, columna].Value = grupoGeneracion.codigoCoes;

                        columna++;
                        ws.Cells[fila, columna].Value = grupoGeneracion.codigoCentralGeneracion;

                        columna++;
                        ws.Cells[fila, columna].Value = grupoGeneracion.codigoGrupoGeneracion;

                        columna++;
                        ws.Cells[fila, columna].Value = grupoGeneracion.nombreGrupoGeneracion;

                        columna++;
                        ws.Cells[fila, columna].Value = grupoGeneracion.codigoTipoCombustible;

                        columna++;
                        ws.Cells[fila, columna].Value = grupoGeneracion.terminal;

                        columna++;
                        ws.Cells[fila, columna].Value = grupoGeneracion.usuario;

                        rg = ws.Cells[fila, 1, fila, columna];

                        ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 4);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Construye el Excel de los maestros de modos de operación.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="suministros"></param>
        internal static void GenerarFormatoMaestroModoOperacion(string nombreArchivo, modoOperacionDTO[] modosOperacion)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Maestro");

                if (ws != null)
                {
                    int fila = 6;
                    int columna = 0;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código COES";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Modo de Operación";
                    ws.Column(columna).Width = 22;

                    columna++;
                    ws.Cells[fila, columna].Value = "Descripción Modo de Operación";
                    ws.Column(columna).Width = 30;

                    columna++;
                    ws.Cells[fila, columna].Value = "Terminal";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Usuario";
                    ws.Column(columna).Width = 15;

                    ExcelRange rg = ws.Cells[fila, 1, fila, columna];
                    ObtenerEstiloCelda(rg, 1);

                    fila++;
                    foreach (modoOperacionDTO modoOperacion in modosOperacion)
                    {
                        columna = 0;

                        columna++;
                        ws.Cells[fila, columna].Value = modoOperacion.codigoCoes;

                        columna++;
                        ws.Cells[fila, columna].Value = modoOperacion.codigoModoOperacion;

                        columna++;
                        ws.Cells[fila, columna].Value = modoOperacion.descripcionModoOperacion;

                        columna++;
                        ws.Cells[fila, columna].Value = modoOperacion.terminal;

                        columna++;
                        ws.Cells[fila, columna].Value = modoOperacion.usuario;

                        rg = ws.Cells[fila, 1, fila, columna];

                        ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 4);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Construye el Excel de los maestros de cuencas.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="suministros"></param>
        internal static void GenerarFormatoMaestroCuenca(string nombreArchivo, cuencaDTO[] cuencas)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Maestro");

                if (ws != null)
                {
                    int fila = 6;
                    int columna = 0;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código COES";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Cuenca";
                    ws.Column(columna).Width = 22;

                    columna++;
                    ws.Cells[fila, columna].Value = "Nombre Cuenca";
                    ws.Column(columna).Width = 30;

                    columna++;
                    ws.Cells[fila, columna].Value = "Terminal";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Usuario";
                    ws.Column(columna).Width = 15;

                    ExcelRange rg = ws.Cells[fila, 1, fila, columna];
                    ObtenerEstiloCelda(rg, 1);

                    fila++;
                    foreach (cuencaDTO cuenca in cuencas)
                    {
                        columna = 0;

                        columna++;
                        ws.Cells[fila, columna].Value = cuenca.codigoCoes;

                        columna++;
                        ws.Cells[fila, columna].Value = cuenca.codigoCuenca;

                        columna++;
                        ws.Cells[fila, columna].Value = cuenca.nombreCuenca;

                        columna++;
                        ws.Cells[fila, columna].Value = cuenca.terminal;

                        columna++;
                        ws.Cells[fila, columna].Value = cuenca.usuario;

                        rg = ws.Cells[fila, 1, fila, columna];

                        ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 4);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Construye el Excel de los maestros de embalses.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="suministros"></param>
        internal static void GenerarFormatoMaestroEmbalse(string nombreArchivo, embalseDTO[] embalses)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Maestro");

                if (ws != null)
                {
                    int fila = 6;
                    int columna = 0;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código COES";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Embalse";
                    ws.Column(columna).Width = 22;

                    columna++;
                    ws.Cells[fila, columna].Value = "Nombre Embalse";
                    ws.Column(columna).Width = 30;

                    columna++;
                    ws.Cells[fila, columna].Value = "Terminal";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Usuario";
                    ws.Column(columna).Width = 15;

                    ExcelRange rg = ws.Cells[fila, 1, fila, columna];
                    ObtenerEstiloCelda(rg, 1);

                    fila++;
                    foreach (embalseDTO embalse in embalses)
                    {
                        columna = 0;

                        columna++;
                        ws.Cells[fila, columna].Value = embalse.codigoCoes;

                        columna++;
                        ws.Cells[fila, columna].Value = embalse.codigoEmbalse;

                        columna++;
                        ws.Cells[fila, columna].Value = embalse.nombreEmbalse;

                        columna++;
                        ws.Cells[fila, columna].Value = embalse.terminal;

                        columna++;
                        ws.Cells[fila, columna].Value = embalse.usuario;

                        rg = ws.Cells[fila, 1, fila, columna];

                        ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 4);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        //- alpha.HDT - 29/06/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Construye el Excel de los maestros de lagos.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="suministros"></param>
        internal static void GenerarFormatoMaestroLago(string nombreArchivo, lagoDTO[] lagos)
        {
            FileInfo nuevoArchivo = new FileInfo(nombreArchivo);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(nuevoArchivo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Maestro");

                if (ws != null)
                {
                    int fila = 6;
                    int columna = 0;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código COES";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Código Lago";
                    ws.Column(columna).Width = 22;

                    columna++;
                    ws.Cells[fila, columna].Value = "Nombre Lago";
                    ws.Column(columna).Width = 30;

                    columna++;
                    ws.Cells[fila, columna].Value = "Terminal";
                    ws.Column(columna).Width = 15;

                    columna++;
                    ws.Cells[fila, columna].Value = "Usuario";
                    ws.Column(columna).Width = 15;

                    ExcelRange rg = ws.Cells[fila, 1, fila, columna];
                    ObtenerEstiloCelda(rg, 1);

                    fila++;
                    foreach (lagoDTO lago in lagos)
                    {
                        columna = 0;

                        columna++;
                        ws.Cells[fila, columna].Value = lago.codigoCoes;

                        columna++;
                        ws.Cells[fila, columna].Value = lago.codigoLago;

                        columna++;
                        ws.Cells[fila, columna].Value = lago.nombreLago;

                        columna++;
                        ws.Cells[fila, columna].Value = lago.terminal;

                        columna++;
                        ws.Cells[fila, columna].Value = lago.usuario;

                        rg = ws.Cells[fila, 1, fila, columna];

                        ObtenerEstiloCelda(rg, 0);
                        fila++;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 4);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        public static void GenerarFormatoReportSioseinDatos(string fileName, List<SioReporteDTO> datosReporte, List<string> NombreColumnas)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Archivo de carga");

                int col = 0;
                if (ws != null)
                {
                    int index = 6;

                    List<ExcelStruct> columns = new List<ExcelStruct>();
                    for (int i = 0; i < NombreColumnas.Count; i++)
                    {
                        col++;
                        ExcelStruct reg = new ExcelStruct();
                        reg.indice = i;
                        reg.nombre = NombreColumnas[i];
                        reg.tipo = datosReporte[0].GetType().GetProperty(NombreColumnas[i]).PropertyType.ToString();
                        if (reg.tipo.Contains("Double") || reg.tipo.Contains("Float"))
                        {
                            reg.tipo = "Decimal";
                        }
                        else if (reg.tipo.Contains("Int"))
                        {
                            reg.tipo = "Int";
                        }
                        else if (reg.tipo.Contains("Date"))
                        {
                            reg.tipo = "Date";
                        }
                        columns.Add(reg);
                        ws.Cells[index, col].Value = NombreColumnas[i];
                    }
                    ExcelRange rg = ws.Cells[index, 1, index, NombreColumnas.Count];
                    ObtenerEstiloCelda(rg, 1);
                    index++;

                    for (int i = 0; i < datosReporte.Count; i++)
                    {
                        int cont = 0;
                        foreach (ExcelStruct item in columns)
                        {
                            cont++;
                            var valor = datosReporte[i].GetType().GetProperty(item.nombre).GetValue(datosReporte[i], null);
                            if (valor != null)
                            {
                                if (item.tipo.Contains("Decimal"))
                                {
                                    ws.Cells[index, cont].Value = Convert.ToDouble(valor);
                                    ws.Cells[index, cont].Style.Numberformat.Format = "#0.00";
                                }
                                else if (item.tipo.Contains("Int"))
                                {
                                    ws.Cells[index, cont].Value = Int64.Parse(valor.ToString());
                                }
                                else if (item.tipo.Contains("Date"))
                                {
                                    ws.Cells[index, cont].Value = valor;
                                    ws.Cells[index, cont].Style.Numberformat.Format = "DD/MM/YYYY hh:mm:ss";
                                }
                                else
                                {
                                    ws.Cells[index, cont].Value = valor.ToString();
                                }
                            }
                        }
                        rg = ws.Cells[index, 1, index, cont];
                        ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    for (int i = 1; i <= col; i++)
                    {
                        ws.Column(i).Width = 20;
                    }
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }


            //Indentificado de data //Seleccion de data //Validacion de data //Consolidado de data //Integracion de tipo de dato //Generado de reporte

        }

        public static void GenerarFormatoDataReaderSiosein(string fileName, IDataReader dr, List<string> Columnas)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Archivo de carga");

                int col = 0;
                if (ws != null)
                {
                    int index = 6;
                    using (dr)
                    {
                        List<ExcelStruct> columns = new List<ExcelStruct>();

                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            col++;
                            ExcelStruct reg = new ExcelStruct();
                            reg.indice = i;
                            reg.nombre = dr.GetName(i).Trim();
                            reg.tipo = dr.GetDataTypeName(i).Trim();
                            if (reg.tipo.Contains("Double") || reg.tipo.Contains("Float"))
                            {
                                reg.tipo = "Decimal";
                            }
                            else if (reg.tipo.Contains("Int"))
                            {
                                reg.tipo = "Int";
                            }
                            else if (reg.tipo.Contains("Date"))
                            {
                                reg.tipo = "Date";
                            }
                            columns.Add(reg);
                            ws.Cells[index, col].Value = dr.GetName(i);
                        }
                        ExcelRange rg = ws.Cells[index, 1, index, dr.FieldCount];
                        ObtenerEstiloCelda(rg, 1);
                        index++;

                        while (dr.Read())
                        {
                            int cont = 0;
                            foreach (ExcelStruct item in columns)
                            {
                                cont++;
                                var valor = dr[item.nombre];
                                if (valor != null)
                                {
                                    if (item.tipo.Equals("Decimal"))
                                    {
                                        ws.Cells[index, cont].Value = Double.Parse(dr[item.nombre].ToString());
                                        ws.Cells[index, cont].Style.Numberformat.Format = "#0.00";
                                    }
                                    else if (item.tipo.Contains("Int"))
                                    {
                                        ws.Cells[index, cont].Value = Int64.Parse(dr[item.nombre].ToString());
                                    }
                                    else if (item.tipo.Contains("Date"))
                                    {
                                        ws.Cells[index, cont].Value = dr.GetDateTime(item.indice);
                                    }
                                    else
                                    {
                                        ws.Cells[index, cont].Value = dr[item.nombre].ToString();
                                    }
                                }
                            }
                            rg = ws.Cells[index, 1, index, cont];
                            ObtenerEstiloCelda(rg, 0);
                            index++;
                        }
                    }
                    for (int i = 1; i <= col; i++)
                    {
                        ws.Column(i).Width = 20;
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Image img = Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }
                xlPackage.Save();
            }

        }


    }
}
