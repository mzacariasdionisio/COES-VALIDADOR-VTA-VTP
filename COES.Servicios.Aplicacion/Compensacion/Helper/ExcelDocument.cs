using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Compensacion;
using COES.Servicios.Aplicacion.Compensacion.Helper;
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
using System.Data;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.CostoOportunidad;
using System.Configuration;

namespace COES.Servicios.Aplicacion.Compensacion.Helper
{
    public class ExcelDocument
    {
        struct ExcelStruct
        {
            public int indice;
            public string nombre;
            public string tipo;
        }

        public static void GenerarFormatoExcelHorasOperacion(string fileName, int pecacodi, List<VceHoraOperacionDTO> list)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);
            
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarHojaExcelHorasOperacion(xlPackage, pecacodi, list);

                xlPackage.Save();
            }
        }

        // DSH 03-07-2017 : Evaluar el uso del metodo
        public static void GenerarFormatoExcelEnergia(string fileName, List<VceEnergiaDTO> list)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Listado de Energía");

                ws.Cells[6, 1].Value = "Listado de Energía";

                ExcelRange rg = ws.Cells[6, 1, 6, 2];
                rg.Merge = true;
                rg = ObtenerEstiloCelda(rg, 1);


                DateTime fecha = DateTime.Now;

                ws.Cells[8, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

                ExcelRange rg1 = ws.Cells[8, 1, 8, 2];
                rg1.Merge = true;

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {
                    int index = 10;
                    ws.Cells[index, 1].Value = "Fecha";
                    ws.Cells[index, 2].Value = "Total Energía";

                    int v = 2;
                    for (int n = 1; n <= 96; n++)
                    {
                        v++;
                        string timeString = TimeSpan.FromMinutes(n * 15).ToString("hh':'mm");
                        ws.Cells[index, v].Value = timeString;
                    }


                    rg = ws.Cells[index, 1, index, 98];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (VceEnergiaDTO item in list)
                    {
                        ws.Cells[index, 1].Value = item.Crmefecha;
                        ws.Cells[index, 2].Value = item.Crmemeditotal;

                        int n = 1;
                        for (int i = 3; i < 99; i++)//Mediciones del 1 - 96
                        {
                            ws.Cells[index, i].Value = item.GetType().GetProperty("Crmeh" + n).GetValue(item, null);
                            n++;
                        }

                        ws.Cells[index, 1].Style.Numberformat.Format = "dd/mm/yyyy";
                        rg = ws.Cells[index, 1, index, 98];
                        rg = ObtenerEstiloCelda(rg, 0);
                        ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;



                        index++;
                    }

                    ws.Column(1).Width = 15;
                    ws.Column(2).Width = 20;

                    for (int i = 3; i <= 99; i++)//Medicion 1 - 96
                    {
                        ws.Column(i).Width = 10;
                        ws.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesCompensacion.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesCompensacion.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        public static void GenerarFormatoExcelCostosVariables(string fileName, int pecacodi, List<VceDatcalculoDTO> list)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Listado de Costos de Operación");

                // Titulo 1
                AgregarTextoHojaExcel(ws, "LISTADO DE COSTOS DE OPERACIÓN", true,12,1,2);
                
                // Periodo-Version
                AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

                // fecha y Hora
                DateTime fecha = DateTime.Now;
                ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {
                    int index = 6;

                    ws.Cells[index, 1].Value = "Empresa";
                    ws.Cells[index, 2].Value = "Modo de Operación";
                    ws.Cells[index, 3].Value = "Fuente Energía";
                    ws.Cells[index, 4].Value = "Combustible";
                    ws.Cells[index, 5].Value = "BARRA / Día Per.";
                    ws.Cells[index, 6].Value = "Considerar Potencia Nominal";
                    ws.Cells[index, 7].Value = "LHV (KJ/m3) / (KJ/Kg)";
                    ws.Cells[index, 8].Value = "Transporte (S//gal)";
                    ws.Cells[index, 9].Value = "Trat. Mecánico (S//gal)";
                    ws.Cells[index, 10].Value = "Trat. Químico (S//gal)";
                    ws.Cells[index, 11].Value = "Días costo financiero";
                    ws.Cells[index, 12].Value = "CVNC $/kWh";
                    ws.Cells[index, 13].Value = "CVNC S//KWh";
                    ws.Cells[index, 14].Value = "Precio Combustible";
                    ws.Cells[index, 15].Value = "Energía MWh";
                    ws.Cells[index, 16].Value = "Tiempo Hrs";
                    ws.Cells[index, 16].Value = "N arr/par";
                    ws.Cells[index, 18].Value = "Potencia Efectiva MW";
                    ws.Cells[index, 19].Value = "Consumo Comb. Pot.Efe";
                    ws.Cells[index, 20].Value = "Potencia parcial 1 MW";
                    ws.Cells[index, 21].Value = "Consumo Comb. Pot.Parcial 1";
                    ws.Cells[index, 22].Value = "Potencia parcial 2 MW";
                    ws.Cells[index, 23].Value = "Consumo Comb. Pot.Parcial 2";
                    ws.Cells[index, 24].Value = "Potencia parcial 3 MW";
                    ws.Cells[index, 25].Value = "Consumo Comb. Pot.Parcial 3";
                    ws.Cells[index, 26].Value = "Potencia parcial 4 MW";
                    ws.Cells[index, 27].Value = "Consumo Comb. Pot.Parcial 4";
                    ws.Cells[index, 28].Value = "Potencia Mínima MW";

                    ws.Row(index).Height = 28;
                    ExcelRange rg = ws.Cells[index, 1, index, 28];
                    rg = ObtenerEstiloCelda(rg, 1);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.WrapText = true;

                    index++;
                    foreach (VceDatcalculoDTO item in list)
                    {
                        //Values
                        ws.Cells[index, 1].Value = item.Emprnomb;
                        ws.Cells[index, 2].Value = item.Gruponomb;
                        ws.Cells[index, 3].Value = item.Fenergnomb;
                        ws.Cells[index, 4].Value = item.Crdcgtipcom;
                        ws.Cells[index, 5].Value = item.Barradiaper;
                        ws.Cells[index, 6].Value = item.Considerarpotnominal;
                        ws.Cells[index, 7].Value = item.Crdcglhv;
                        ws.Cells[index, 8].Value = item.Crdcgtranspor;
                        ws.Cells[index, 9].Value = item.Crdcgtratmec;
                        ws.Cells[index, 10].Value = item.Crdcgtratquim;
                        ws.Cells[index, 11].Value = item.Crdcgdiasfinanc;
                        ws.Cells[index, 12].Value = item.Crdcgcvncdol;
                        ws.Cells[index, 13].Value = item.Crdcgcvncsol;
                        ws.Cells[index, 14].Value = item.Crdcgprecomb;
                        ws.Cells[index, 15].Value = item.Vcedcmenergia;
                        ws.Cells[index, 16].Value = item.Vcedcmtiempo;
                        ws.Cells[index, 17].Value = item.Crdcgnumarrpar;
                        ws.Cells[index, 18].Value = item.Crdcgpotefe;
                        ws.Cells[index, 19].Value = item.Crdcgccpotefe;
                        ws.Cells[index, 20].Value = item.Crdcgpotpar1;
                        ws.Cells[index, 21].Value = item.Crdcgconcompp1;
                        ws.Cells[index, 22].Value = item.Crdcgpotpar2;
                        ws.Cells[index, 23].Value = item.Crdcgconcompp2;
                        ws.Cells[index, 24].Value = item.Crdcgpotpar3;
                        ws.Cells[index, 25].Value = item.Crdcgconcompp3;
                        ws.Cells[index, 26].Value = item.Crdcgpotpar4;
                        ws.Cells[index, 27].Value = item.Crdcgconcompp4;
                        ws.Cells[index, 28].Value = item.Crdcgpotmin;

                        rg = ws.Cells[index, 1, index, 28];
                        rg = ObtenerEstiloCelda(rg, 0);
                        ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                        index++;
                    }

                    ws.Column(1).Width = 30;
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;

                    for (int i = 5; i <= 28; i++)
                    {
                        ws.Column(i).AutoFit(14.5);

                    }

                    AgregarLogoHojaExcel(ws);
                }

                xlPackage.Save();
            }
        }


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


        // Cambios DSH 30-03-2017 - Generar formato excel Compensaciones Regulares
        public static void GenerarFormatoExcelCompensacionesRegulares(string fileName, int pecacodi, List<VceCompBajaeficDTO> list)
        {

            string periodoVersion = string.Empty;
            
            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CompBajaEficiencia");

                // Titulo 1
                AgregarTextoHojaExcel(ws, "LISTADO DE COMPENSACIONES REGULARES", true, 12, 1, 2);

                // Periodo-Version
                AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

                
                DateTime fecha = DateTime.Now;

                ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {
                    int index = 6;

                    ws.Cells[index, 1].Value = "Empresa";
                    ws.Cells[index, 2].Value = "Calificación";
                    ws.Cells[index, 3].Value = "Modo Operación";
                    ws.Cells[index, 4].Value = "Inicio";
                    ws.Cells[index, 5].Value = "Fin";
                    ws.Cells[index, 6].Value = "Potencia";
                    ws.Cells[index, 7].Value = "Consumo";
                    ws.Cells[index, 8].Value = "CVC";
                    ws.Cells[index, 9].Value = "CVNC";
                    ws.Cells[index, 10].Value = "CV";
                    ws.Cells[index, 11].Value = "Compensación (S/.) ";

                    ExcelRange rg = ws.Cells[index, 1, index, 11];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (VceCompBajaeficDTO item in list)
                    {
                        //Values
                        ws.Cells[index, 1].Value = item.Emprnomb;
                        ws.Cells[index, 2].Value = item.Subcausadesc;
                        ws.Cells[index, 3].Value = item.Gruponomb;
                        ws.Cells[index, 4].Value = item.Crcbehorini;
                        ws.Cells[index, 5].Value = item.Crcbehorfin;
                        ws.Cells[index, 6].Value = item.Crcbepotencia;
                        ws.Cells[index, 7].Value = item.Crcbeconsumo;
                        ws.Cells[index, 8].Value = item.Crcbecvc;
                        ws.Cells[index, 9].Value = item.Crcbecvnc;
                        ws.Cells[index, 10].Value = item.Crcbecvt;
                        ws.Cells[index, 11].Value = item.Crcbecompensacion;

                        //Styles for Fecha
                        ws.Cells[index, 4].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                        ws.Cells[index, 5].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                        ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        rg = ws.Cells[index, 1, index, 11];
                        rg = ObtenerEstiloCelda(rg, 0);
                        ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                        index++;
                    }

                    ws.Column(1).Width = 30;
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 20;

                    AgregarLogoHojaExcel(ws);
                }

                xlPackage.Save();
            }
        }

        // Cambios DSH 30-03-2017 - Generar formato excel Compensaciones Especiales
        public static void GenerarFormatoExcelCompensacionesEspeciales(string fileName, int pecacodi, List<VceCompRegularDetDTO> list)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                GenerarHojaExcelCompensacionEspecial(xlPackage, pecacodi, list);
                xlPackage.Save();
            }
        }

        // Cambios DSH 22-08-2017 - Generar formato excel Verificar Horas de OperaciÃ³n
        public static void GenerarFormatoExcelVerificarHorasOperacion(string fileName, int pecacodi, List<VceHoraOperacionDTO> list)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarHojaExcelVerificarHorasOperacion(xlPackage, pecacodi, list);

                xlPackage.Save();
            }
        }

        public static void GenerarFormatoExcelDatosCalculo(string fileName, int pecacodi, string grupo, string lista)
        {
            CompensacionAppServicio servicio = new CompensacionAppServicio();

            DateTime fecha = DateTime.Now;
            FileInfo newFile = new FileInfo(fileName);

            string[] reportes = lista.Split(',');

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                foreach (var item in reportes)
                {
                    switch (item.ToString())
                    {

                        case "cr":  // compensaciones regulares
                            //List<VceCompBajaeficDTO> list = servicio.ListCompensacionesRegulares(pecacodi, "", "", "", "", "", "", "", "");
                            GenerarHojaExcelCompensacionRegular(xlPackage, pecacodi);
                            break;

                        case "ce":  // compensaciones especiales
                            List<VceCompRegularDetDTO> list2 = servicio.ListCompensacionesEspeciales(pecacodi, "", "", "", "");
                            GenerarHojaExcelCompensacionEspecial(xlPackage, pecacodi, list2);
                            break;

                        case "dcr": // detalle de compensaciones regulares
                            List<VceCompBajaeficDTO> list3 = servicio.ListCompensacionesRegulares(pecacodi, "", "", "", "", "", "", "", "");
                            GenerarHojaExcelDetalleCompensacionRegular(xlPackage, pecacodi, list3);
                            break;

                        case "dp":  // detalle x periodo 15 min
                            GenerarHojaExcelDetalleporPeriodo15min(xlPackage, pecacodi);
                            break;

                        case "cv":  // costos variables
                            List<VceCostoVariableDTO> list4 = servicio.ListaCostoVariable(pecacodi);
                            GenerarHojaExcelCostosVariables(xlPackage, pecacodi, list4);
                            break;

                        case "pg":  // paramentros generador
                            GenerarHojaExcelParametrosGenerador(xlPackage, pecacodi);
                            break;

                        case "me":  // medidores (energia)
                            List<VceEnergiaDTO> list5 = servicio.ListarEnergia(100, pecacodi);
                            GenerarHojaExcelMedidores(xlPackage, pecacodi, list5);
                            break;

                        case "cm":  // costos marginales
                            int pericodi;
                            int cosmarversion;
                            VcePeriodoCalculoDTO obj = servicio.getVersionPeriodoById(pecacodi);
                            pericodi = obj.PeriCodi;
                            cosmarversion = obj.PecaVersionVtea;

                            List<string> listHead = servicio.lstHeadCostoMarginal(pericodi);
                            List<string> listBody = servicio.lstBodyCostoMarginal(pericodi, cosmarversion);
                            GenerarHojaExcelCostosMarginales(xlPackage, pecacodi, listHead, listBody);
                            break;

                        case "ho":  // horas de operacion
                            List<VceHoraOperacionDTO> list6 = servicio.ListarHoraOperacionFiltro(pecacodi, "", "", "", "", "", "", "", "", "");
                            GenerarHojaExcelHorasOperacion(xlPackage, pecacodi, list6);
                            break;
                        case "ap":  // arranques y paradas
                            GenerarHojaExcelArranquesParadas(xlPackage, pecacodi);
                            break;
                        case "dap":  // detalle arranques y paradas
                            IDataReader dr = servicio.ListCompensacionArrPar(pecacodi, "", "", "", "");
                            IDataReader drCab = servicio.ListCabCompensacionArrPar(pecacodi);
                            GenerarHojaExcelDetalleArranquesParadas(xlPackage, pecacodi, dr, drCab);
                            break;
                        case "ctm":  // compensaciones regulares
                            //List<VceCompBajaeficDTO> list = servicio.ListCompensacionesRegulares(pecacodi, "", "", "", "", "", "", "", "");
                            GenerarHojaExcelCompensacionMME(xlPackage, pecacodi);
                            break;
                        case "csd":  // compensaciones regulares
                            //List<VceCompBajaeficDTO> list = servicio.ListCompensacionesRegulares(pecacodi, "", "", "", "", "", "", "", "");
                            GenerarHojaExcelCompensacionDiarioMME(xlPackage, pecacodi);
                            break;
                    }
                }
                xlPackage.Save();
            }
        }

        public static void GenerarHojaExcelCompensacionMME(ExcelPackage xlPackage, int pecacodi)
        {
            string periodoVersion = string.Empty;
            string titulo = "";

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            CompensacionAppServicio servicio = new CompensacionAppServicio();

            //Obtenemos las cabeceras de los eventos
            List<EveSubcausaeventoDTO> subCausaEvento = servicio.ListSubCausaEvento(pecacodi);
            subCausaEvento = subCausaEvento.OrderBy(x => x.Subcausadesc).ToList();

            //Obtenemos el detalle del reporte MME
            IDataReader list = servicio.ListCompensacionOperacionMME(pecacodi, subCausaEvento);

            // Obtener Texto de Reporte
            VceTextoReporteDTO objTextoReporte = servicio.getTextoReporteById(pecacodi, "EX_COMPREG_MME", "TITULO1");

            if (objTextoReporte != null) { titulo = objTextoReporte.Txtreptexto; }

            // Hoja 1: MME
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Compensaciones_MME");

            // Titulo 1
            AgregarTextoHojaExcel(ws, titulo, true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int nroFila = 6;
                int indiceIni = 2;
                // Cabecera Manual
                ws.Cells[nroFila, 1].Value = "Empresa";
                ws.Cells[nroFila, 2].Value = "Modo de Operación";
                foreach (var subCausa in subCausaEvento)
                {
                    indiceIni++;
                    ws.Cells[nroFila, indiceIni].Value = subCausa.Subcausadesc;
                }
                ws.Cells[nroFila, indiceIni + 1].Value = "Total por Empresa (S/.)";

                ws.Row(nroFila).Height = 30;
                ExcelRange rg = ws.Cells[nroFila, 1, nroFila, indiceIni + 1];
                rg = ObtenerEstiloCelda(rg, 1);
                rg.Style.WrapText = true;

                // Datos                
                
                using (list)
                {
                    while (list.Read())
                    {
                        //incremetar numero de fila
                        nroFila++;
                        ws.Cells[nroFila, 1].Value = list["EMPRNOMB"].ToString();
                        ws.Cells[nroFila, 2].Value = list["GRUPONOMB"].ToString();
                        indiceIni = 2;
                        foreach (var subCausa in subCausaEvento)
                        {
                            indiceIni++;
                            ws.Cells[nroFila, indiceIni].Value = list["C" + subCausa.Subcausacodi];
                        }
                        ws.Cells[nroFila, indiceIni + 1].Value = list["TOTAL"];

                        // Asigna formato numerico
                        ws.Cells[nroFila, 3, nroFila, indiceIni + 1].Style.Numberformat.Format = "#,##0.00;-#,##0.00;0";

                        rg = ws.Cells[nroFila, 1, nroFila, indiceIni + 1];
                        rg = ObtenerEstiloCelda(rg, 0);
                    }                    

                }

                // Ancho de columnas
                ws.Column(1).Width = 30;
                ws.Column(2).Width = 40;
                for (int i = 3; i <= indiceIni + 1; i++)
                {
                    ws.Column(i).Width = 20;
                }
                
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesCompensacion.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture(ConstantesCompensacion.NombreLogo, img);
                picture.SetPosition(10, 20);
                picture.SetSize(180, 60);

            }
            
        }
        public static void GenerarHojaExcelCompensacionDiarioMME(ExcelPackage xlPackage, int pecacodi)
        {
            string periodoVersion = string.Empty;
            string titulo = "";

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            CompensacionAppServicio servicio = new CompensacionAppServicio();
            CostoOportunidadAppServicio servicioCostoOportunidad = new CostoOportunidadAppServicio(); 

            //Obtenemos las cabeceras de los eventos
            List<PrGrupoDTO> listaGrupo = servicioCostoOportunidad.ListaCompensacionGrupo(pecacodi);
            listaGrupo = listaGrupo.OrderBy(x => x.Gruponomb).ToList();

            //Obtenemos el detalle del reporte MME
            IDataReader list = servicio.ListCompensacionDiarioMME(pecacodi, listaGrupo);

            // Obtener Texto de Reporte
            VceTextoReporteDTO objTextoReporte = servicio.getTextoReporteById(pecacodi, "EX_COMPREG_MME_DIA", "TITULO1");

            if (objTextoReporte != null) { titulo = objTextoReporte.Txtreptexto; }

            // Hoja 1: MME
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Compensaciones_MME_Diario");

            // Titulo 1
            AgregarTextoHojaExcel(ws, titulo, true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int nroFila = 6;
                int indiceIni = 2;
                // Cabecera Manual
                ws.Cells[nroFila, 1].Value = "DIA";
                ws.Cells[nroFila, 2].Value = "CALIFICACION";
                foreach (var grupo in listaGrupo)
                {
                    indiceIni++;
                    ws.Cells[nroFila, indiceIni].Value = grupo.Gruponomb;
                }
                //ws.Cells[nroFila, indiceIni + 1].Value = "Total por Empresa (S/.)";

                ws.Row(nroFila).Height = 30;
                ExcelRange rg = ws.Cells[nroFila, 1, nroFila, indiceIni];
                rg = ObtenerEstiloCelda(rg, 1);
                rg.Style.WrapText = true;

                // Datos                

                using (list)
                {
                    while (list.Read())
                    {
                        //incremetar numero de fila
                        nroFila++;
                        ws.Cells[nroFila, 1].Value = (list["DIA"] != null) ? Convert.ToDateTime(list["DIA"]).ToString("dd/MM/yyyy") : string.Empty;
                        ws.Cells[nroFila, 2].Value = list["CALIFICACION"].ToString();
                        indiceIni = 2;
                        foreach (var grupo in listaGrupo)
                        {
                            indiceIni++;
                            ws.Cells[nroFila, indiceIni].Value = list["C" + grupo.Grupocodi];
                        }
                        //ws.Cells[nroFila, indiceIni + 1].Value = list["TOTAL"];

                        // Asigna formato numerico
                        ws.Cells[nroFila, 3, nroFila, indiceIni].Style.Numberformat.Format = "#,##0.00;-#,##0.00;0";

                        rg = ws.Cells[nroFila, 1, nroFila, indiceIni];
                        rg = ObtenerEstiloCelda(rg, 0);
                    }

                }

                // Ancho de columnas
                ws.Column(1).Width = 30;
                ws.Column(2).Width = 40;
                for (int i = 3; i <= indiceIni; i++)
                {
                    ws.Column(i).Width = 40;
                }

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesCompensacion.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture(ConstantesCompensacion.NombreLogo, img);
                picture.SetPosition(10, 20);
                picture.SetSize(180, 60);

            }

        }
        public static void GenerarHojaExcelCompensacionRegular(ExcelPackage xlPackage, int pecacodi)
        {
            string periodoVersion = string.Empty;
            string titulo = "";

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            CompensacionAppServicio servicio = new CompensacionAppServicio();

            List<VceCompBajaeficDTO> list = servicio.ListCompensacionOperacionInflexibilidad(pecacodi);

            // Obtener Texto de Reporte
            VceTextoReporteDTO objTextoReporte = servicio.getTextoReporteById(pecacodi, "EX_COMPREG_INFOPE", "TITULO1");
            
            if (objTextoReporte != null) { titulo = objTextoReporte.Txtreptexto; }
            
            // Hoja 1: Inflexibilidad
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("InflexibilidadOperativa");

            // Titulo 1
            AgregarTextoHojaExcel(ws,titulo, true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);
            
            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int nroFila = 6;

                // Cabecera Manual
                ws.Cells[nroFila, 1].Value = "Empresa";
                ws.Cells[nroFila, 2].Value = "Modo de Operación";
                ws.Cells[nroFila, 3].Value = "Por Inflexibilidad Operativa (S/.)";
                ws.Cells[nroFila, 4].Value = "Por Inflexibilidad Operativa (Pr-25) (S/.)";
                ws.Cells[nroFila, 5].Value = "Pago a cuenta por el Exportador (*)";
                ws.Cells[nroFila, 6].Value = "Comp. total por Modo de Op. (S/.)";
                ws.Cells[nroFila, 7].Value = "Total por Empresa (S/.)";

                ws.Row(nroFila).Height = 30;
                ExcelRange rg = ws.Cells[nroFila, 1, nroFila, 7];
                rg = ObtenerEstiloCelda(rg, 1);
                rg.Style.WrapText = true;

                // Datos 
                foreach (VceCompBajaeficDTO item in list)
                {
                    //incremetar numero de fila
                    nroFila++;
                    ws.Cells[nroFila, 1].Value = item.Emprnomb;
                    ws.Cells[nroFila, 2].Value = item.Gruponomb;
                    ws.Cells[nroFila, 3].Value = item.Minimacarga;
                    ws.Cells[nroFila, 4].Value = item.Pruebapr25;
                    ws.Cells[nroFila, 5].Value = item.Pagocuenta;
                    ws.Cells[nroFila, 6].Value = item.Totalmodo;
                    ws.Cells[nroFila, 7].Value = item.Totalemp;

                    // Asigna formato numerico
                    ws.Cells[nroFila, 3, nroFila, 7].Style.Numberformat.Format = "#,##0.00;-#,##0.00;0";

                    rg = ws.Cells[nroFila, 1, nroFila, 7];
                    rg = ObtenerEstiloCelda(rg, 0);
                    
                }

                // Ancho de columnas
                ws.Column(1).Width = 30;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesCompensacion.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture(ConstantesCompensacion.NombreLogo, img);
                picture.SetPosition(10, 20);
                picture.SetSize(180, 60);

            }

            list = servicio.ListCompensacionOperacionSeguridad(pecacodi);
            
            // Obtener Texto de Reporte
            titulo = "";
            objTextoReporte = servicio.getTextoReporteById(pecacodi, "EX_COMPREG_SEGURID", "TITULO1");

            if (objTextoReporte != null) { titulo = objTextoReporte.Txtreptexto; }

            // Hoja 2: seguridad
            ws = xlPackage.Workbook.Worksheets.Add("Seguridad"); 

            // Titulo 1
            AgregarTextoHojaExcel(ws, titulo, true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            if (ws != null)
            {
                int nroFila = 6;

                // Cabecera Manual
                ws.Cells[nroFila, 1].Value = "Empresa";
                ws.Cells[nroFila, 2].Value = "Modo de Operación";
                ws.Cells[nroFila, 3].Value = "Compensación (S/.)";
                ws.Cells[nroFila, 4].Value = "Total por Empresa (S/.)";

                ws.Row(nroFila).Height = 30;
                ExcelRange rg = ws.Cells[nroFila, 1, nroFila, 4];
                rg = ObtenerEstiloCelda(rg, 1);
                rg.Style.WrapText = true;


                // Datos 
                foreach (VceCompBajaeficDTO item in list)
                {
                    //incremetar numero de fila
                    nroFila++;
                    ws.Cells[nroFila, 1].Value = item.Emprnomb;
                    ws.Cells[nroFila, 2].Value = item.Gruponomb;
                    ws.Cells[nroFila, 3].Value = item.Seguridad;
                    ws.Cells[nroFila, 4].Value = item.Totalemp;

                    // Asigna formato numerico
                    ws.Cells[nroFila, 3, nroFila, 4].Style.Numberformat.Format = "#,##0.00;-#,##0.00;0";

                    rg = ws.Cells[nroFila, 1, nroFila, 4];
                    rg = ObtenerEstiloCelda(rg, 0);

                }

                // Ancho de columnas
                ws.Column(1).Width = 30;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;

                AgregarLogoHojaExcel(ws);
            }

            // Hoja 3 : RSF
            list = servicio.ListCompensacionOperacionRSF(pecacodi);

            // Obtener Texto de Reporte
            titulo = "";
            objTextoReporte = servicio.getTextoReporteById(pecacodi, "EX_COMPREG_RSF", "TITULO1");

            if (objTextoReporte != null) { titulo = objTextoReporte.Txtreptexto; }

            // Hoja 2: seguridad
            ws = xlPackage.Workbook.Worksheets.Add("RSF");

            // Titulo 1
            AgregarTextoHojaExcel(ws, titulo, true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            if (ws != null)
            {
                int nroFila = 6;

                // Cabecera Manual
                ws.Cells[nroFila, 1].Value = "Empresa";
                ws.Cells[nroFila, 2].Value = "Modo de Operación";
                ws.Cells[nroFila, 3].Value = "Compensación por RSF (S/.)";
                ws.Cells[nroFila, 4].Value = "Compensación por Reserva Especial (S/.)";
                ws.Cells[nroFila, 5].Value = "Total por Empresa y por tipo (S/.)";

                ws.Row(nroFila).Height = 30;
                ExcelRange rg = ws.Cells[nroFila, 1, nroFila, 5];
                rg = ObtenerEstiloCelda(rg, 1);
                rg.Style.WrapText = true;


                // Datos 
                foreach (VceCompBajaeficDTO item in list)
                {
                    //incremetar numero de fila
                    nroFila++;
                    ws.Cells[nroFila, 1].Value = item.Emprnomb;
                    ws.Cells[nroFila, 2].Value = item.Gruponomb;
                    ws.Cells[nroFila, 3].Value = item.Rsf;
                    ws.Cells[nroFila, 4].Value = item.Reservaesp;
                    ws.Cells[nroFila, 5].Value = item.Totalemp;

                    // Asigna formato numerico
                    ws.Cells[nroFila, 3, nroFila, 5].Style.Numberformat.Format = "#,##0.00;-#,##0.00;0";

                    rg = ws.Cells[nroFila, 1, nroFila, 5];
                    rg = ObtenerEstiloCelda(rg, 0);
                    //ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                }

                // Ancho de columnas
                ws.Column(1).Width = 30;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;

                AgregarLogoHojaExcel(ws);
            }

            // Hoja 4 : Tension
            list = servicio.ListCompensacionRegulacionTension(pecacodi);

            // Obtener Texto de Reporte
            titulo = "";
            objTextoReporte = servicio.getTextoReporteById(pecacodi, "EX_COMPREG_TENSION", "TITULO1");

            if (objTextoReporte != null) { titulo = objTextoReporte.Txtreptexto; }

            // Hoja 2: seguridad
            ws = xlPackage.Workbook.Worksheets.Add("Tension");

            // Titulo 1
            AgregarTextoHojaExcel(ws, titulo, true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            if (ws != null)
            {
                int nroFila = 6;

                // Cabecera Manual
                ws.Cells[nroFila, 1].Value = "Empresa";
                ws.Cells[nroFila, 2].Value = "Modo de Operación";
                ws.Cells[nroFila, 3].Value = "Compensación (S/.)";
                ws.Cells[nroFila, 4].Value = "Total por Empresa (S/.)";

                ws.Row(nroFila).Height = 30;
                ExcelRange rg = ws.Cells[nroFila, 1, nroFila, 4];
                rg = ObtenerEstiloCelda(rg, 1);
                rg.Style.WrapText = true;


                // Datos 
                foreach (VceCompBajaeficDTO item in list)
                {
                    //incremetar numero de fila
                    nroFila++;
                    ws.Cells[nroFila, 1].Value = item.Emprnomb;
                    ws.Cells[nroFila, 2].Value = item.Gruponomb;
                    ws.Cells[nroFila, 3].Value = item.Tension;
                    ws.Cells[nroFila, 4].Value = item.Totalemp;

                    // Asigna formato numerico
                    ws.Cells[nroFila, 3, nroFila, 4].Style.Numberformat.Format = "#,##0.00;-#,##0.00;0";

                    rg = ws.Cells[nroFila, 1, nroFila, 4];
                    rg = ObtenerEstiloCelda(rg, 0);
                    //ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                }

                // Ancho de columnas
                ws.Column(1).Width = 30;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;

                AgregarLogoHojaExcel(ws);
            }
        }

        public static void GenerarHojaExcelCompensacionEspecial(ExcelPackage xlPackage, int pecacodi, List<VceCompRegularDetDTO> list)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CompPotenciaEnergia");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "COMPENSACIÓN ESPECIAL", true, 12, 1, 2,2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2,2);


            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int index = 6;
                ws.Cells[index, 1].Value = "Empresa";
                ws.Cells[index, 2].Value = "Modo Operación";
                ws.Cells[index, 3].Value = "Compensación (S/.) ";

                ExcelRange rg = ws.Cells[index, 1, index, 3];
                rg = ObtenerEstiloCelda(rg, 1);

                index++;
                foreach (VceCompRegularDetDTO item in list)
                {
                    //Values
                    ws.Cells[index, 1].Value = item.Emprnomb;
                    ws.Cells[index, 2].Value = item.Gruponomb;
                    ws.Cells[index, 3].Value = item.Crdetcompensacion;

                    rg = ws.Cells[index, 1, index, 3];
                    rg = ObtenerEstiloCelda(rg, 0);
                    ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    index++;
                }

                ws.Column(1).Width = 30;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 20;

                AgregarLogoHojaExcel(ws);
            }
        }

        public static void GenerarHojaExcelDetalleCompensacionRegular(ExcelPackage xlPackage, int pecacodi, List<VceCompBajaeficDTO> list)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CompBajaEficiencia");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "DETALLE COMPENSACIÓN REGULAR", true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int index = 6;

                ws.Cells[index, 1].Value = "Empresa";
                ws.Cells[index, 2].Value = "Calificación";
                ws.Cells[index, 3].Value = "Modo Operación";
                ws.Cells[index, 4].Value = "Inicio";
                ws.Cells[index, 5].Value = "Fin";
                ws.Cells[index, 6].Value = "Potencia";
                ws.Cells[index, 7].Value = "Consumo";
                ws.Cells[index, 8].Value = "CVC";
                ws.Cells[index, 9].Value = "CVNC";
                ws.Cells[index, 10].Value = "CV";
                ws.Cells[index, 11].Value = "Compensación (S/.) ";

                ExcelRange rg = ws.Cells[index, 1, index, 11];
                rg = ObtenerEstiloCelda(rg, 1);

                index++;
                foreach (VceCompBajaeficDTO item in list)
                {
                    //Values
                    ws.Cells[index, 1].Value = item.Emprnomb;
                    ws.Cells[index, 2].Value = item.Subcausadesc;
                    ws.Cells[index, 3].Value = item.Gruponomb;
                    ws.Cells[index, 4].Value = item.Crcbehorini;
                    ws.Cells[index, 5].Value = item.Crcbehorfin;
                    ws.Cells[index, 6].Value = item.Crcbepotencia;
                    ws.Cells[index, 7].Value = item.Crcbeconsumo;
                    ws.Cells[index, 8].Value = item.Crcbecvc;
                    ws.Cells[index, 9].Value = item.Crcbecvnc;
                    ws.Cells[index, 10].Value = item.Crcbecvt;
                    ws.Cells[index, 11].Value = item.Crcbecompensacion;

                    //Styles for Fecha
                    ws.Cells[index, 4].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 5].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rg = ws.Cells[index, 1, index, 11];
                    rg = ObtenerEstiloCelda(rg, 0);
                    ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    index++;
                }

                ws.Column(1).Width = 30;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 40;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Column(11).Width = 20;

                AgregarLogoHojaExcel(ws);
            }
        }

        // DSH 31-05-2017 : Cambio por Requerimiento
        public static void GenerarHojaExcelDetalleporPeriodo15min(ExcelPackage xlPackage, int pecacodi)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            List<PrGrupodatDTO> list = FactorySic.GetPrGrupodatRepository().ListaCabecera(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CvoaCMg");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "DETALLE X PERIODO 15 MINUTOS", true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            if (ws != null)
            {
                int index = 6;
                int cont = 0;

                ws.Cells[index, 1].Value = "Fecha y Hora";

                foreach (PrGrupodatDTO item in list)
                {
                    int ini = (cont * 5) + 2;
                    int fin = (cont * 5) + 6;
                    var valor = item.GrupoNomb;

                    if (valor != null)
                    {
                        //Fila cabecera inicial
                        ws.Cells[index, ini].Value = valor;
                        //Fila cabecera secundaria
                        ws.Cells[index + 1, ini].Value = "CV";
                        ws.Cells[index + 1, ini + 1].Value = "CMg";
                        ws.Cells[index + 1, ini + 2].Value = "Energía";
                        ws.Cells[index + 1, ini + 3].Value = "Comp";
                        ws.Cells[index + 1, ini + 4].Value = "Calificación";
                    }
                    cont++;
                    ExcelRange rgc = ws.Cells[index, ini, index, fin];
                    rgc.Merge = true;
                    rgc = ObtenerEstiloCelda(rgc, 1);

                }

                // Asignar formato cabecera inicial
                ExcelRange rg1 = ws.Cells[index, 1, index, 1];
                ObtenerEstiloCelda(rg1, 1);

                // Asignar formato cabecera secundaria
                ExcelRange rg = ws.Cells[index + 1, 1, index + 1, (cont * 5) + 1];
                ObtenerEstiloCelda(rg, 1);

                // Unir Filas del campo "Fecha y Hora"
                ExcelRange rgF = ws.Cells[index, 1, index + 1, 1];
                rgF.Merge = true;

                index = index + 2;
                
                IDataReader dr = FactorySic.GetPrGrupodatRepository().GetDataReaderCuerpo(pecacodi);

                int col = 0;
                using (dr)
                {
                    List<ExcelStruct> columns = new List<ExcelStruct>();

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        col++;
                        ExcelStruct reg = new ExcelStruct();
                        reg.nombre = dr.GetName(i).Trim();
                        columns.Add(reg);
                    }

                    while (dr.Read())
                    {
                        cont = 0;
                        foreach (ExcelStruct item in columns)
                        {
                            cont++;
                            var valor = dr[item.nombre];
                            if (valor != null)
                            {
                                //ws.Cells[index, cont].Value = dr[item.nombre].ToString();
                                ws.Cells[index, cont].Value = dr[item.nombre];
                                ws.Cells[index, ((cont - 1) * 5) + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[index, ((cont - 1) * 5) + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                ws.Cells[index, ((cont - 1) * 5) + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                ws.Cells[index, ((cont - 1) * 5) + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                ws.Cells[index, ((cont - 1) * 5) + 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                ws.Cells[index, ((cont - 1) * 5) + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }


                        }
                        rg = ws.Cells[index, 1, index, cont];
                        ObtenerEstiloCelda(rg, 0);
                        index++;
                    }

                    for (int i = 1; i <= columns.Count; i++)
                    {
                        ws.Column(i).Width = 25;
                    }
                }

                AgregarLogoHojaExcel(ws);
            }
        }

        public static void GenerarHojaExcelCostosVariables(ExcelPackage xlPackage, int pecacodi, List<VceCostoVariableDTO> list)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CV");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "COSTOS VARIABLES", true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);
            
            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);
            
            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int index = 6;
                ws.Cells[index, 1].Value = "Modo";
                ws.Cells[index, 2].Value = "Día";
                ws.Cells[index, 3].Value = "Potencia";
                ws.Cells[index, 4].Value = "Consumo";
                ws.Cells[index, 5].Value = "Precio a aplicar";
                ws.Cells[index, 6].Value = "Unidad";
                ws.Cells[index, 7].Value = "CVC";
                ws.Cells[index, 8].Value = "CVNC";
                ws.Cells[index, 9].Value = "cvt";
                ws.Cells[index, 10].Value = "Barra";
                ws.Cells[index, 11].Value = "Aplic. Efectiva";

                ExcelRange rg = ws.Cells[index, 1, index, 11];
                rg = ObtenerEstiloCelda(rg, 1);

                index++;
                foreach (VceCostoVariableDTO item in list)
                {
                    //Values
                    ws.Cells[index, 1].Value = item.Gruponomb;
                    ws.Cells[index, 2].Value = item.Dia;
                    ws.Cells[index, 3].Value = item.Crcvpotencia;
                    ws.Cells[index, 4].Value = item.Crcvconsumo;
                    ws.Cells[index, 5].Value = item.Crcvprecioaplic;
                    ws.Cells[index, 6].Value = item.Crdcgprecioaplicunid;
                    ws.Cells[index, 7].Value = item.Crcvcvc;
                    ws.Cells[index, 8].Value = item.Crcvcvnc;
                    ws.Cells[index, 9].Value = item.Crcvcvt;
                    ws.Cells[index, 10].Value = item.Barrbarratransferencia;
                    ws.Cells[index, 11].Value = item.Crcvaplicefectiva;

                    rg = ws.Cells[index, 1, index, 11];
                    rg = ObtenerEstiloCelda(rg, 0);
                    ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    index++;
                }

                ws.Column(1).Width = 30;
                ws.Column(2).Width = 5;
                ws.Column(3).Width = 18;
                ws.Column(4).Width = 18;
                ws.Column(5).Width = 18;
                ws.Column(6).Width = 18;
                ws.Column(7).Width = 18;
                ws.Column(8).Width = 18;
                ws.Column(9).Width = 18;
                ws.Column(10).Width = 30;
                ws.Column(11).Width = 15;

                AgregarLogoHojaExcel(ws);
            }

        }

        public static void GenerarHojaExcelParametrosGenerador(ExcelPackage xlPackage, int pecacodi)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            List<VceDatcalculoDTO> List = FactorySic.GetVceDatCalculoRepository().ListByFiltro(pecacodi, "", "", "", "");

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DatosGenerador");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "PARAMETROS GENERADOR", true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            if (ws != null)
            {

                int index = 6;
                ws.Cells[index, 1].Value = "Empresa";
                ws.Cells[index, 2].Value = "Modo de Operación";
                ws.Cells[index, 3].Value = "Fuente Energía";
                ws.Cells[index, 4].Value = "Combustible";
                ws.Cells[index, 5].Value = "BARRA / Día Per.";
                ws.Cells[index, 6].Value = "Considerar Potencia Nominal";
                ws.Cells[index, 7].Value = "LHV (KJ/m3) / (KJ/Kg)";
                ws.Cells[index, 8].Value = "Transporte (S//gal)";
                ws.Cells[index, 9].Value = "Trat. Mecánico (S//gal)";
                ws.Cells[index, 10].Value = "Trat. Químico (S//gal)";
                ws.Cells[index, 11].Value = "Días costo financiero";
                ws.Cells[index, 12].Value = "CVNC $/kWh";
                ws.Cells[index, 13].Value = "CVNC S//KWh";
                ws.Cells[index, 14].Value = "Precio Combustible";
                ws.Cells[index, 15].Value = "Precio Comb. (Unid)";
                ws.Cells[index, 16].Value = "Energía MWh";
                ws.Cells[index, 17].Value = "Tiempo Hrs";
                ws.Cells[index, 18].Value = "N arr/par";
                ws.Cells[index, 19].Value = "Potencia Efectiva MW";
                ws.Cells[index, 20].Value = "Consumo Comb. Pot.Efe";
                ws.Cells[index, 21].Value = "Potencia parcial 1 MW";
                ws.Cells[index, 22].Value = "Consumo Comb. Pot.Parcial 1";
                ws.Cells[index, 23].Value = "Potencia parcial 2 MW";
                ws.Cells[index, 24].Value = "Consumo Comb. Pot.Parcial 2";
                ws.Cells[index, 25].Value = "Potencia parcial 3 MW";
                ws.Cells[index, 26].Value = "Consumo Comb. Pot.Parcial 3";
                ws.Cells[index, 27].Value = "Potencia parcial 4 MW";
                ws.Cells[index, 28].Value = "Consumo Comb. Pot.Parcial 4";
                ws.Cells[index, 29].Value = "Potencia Mínima MW";

                ws.Row(index).Height = 28;
                ExcelRange rg = ws.Cells[index, 1, index, 29];
                rg = ObtenerEstiloCelda(rg, 1);
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.WrapText = true;

                index++;

                foreach (VceDatcalculoDTO item in List)
                {
                    ws.Cells[index, 1].Value = item.Emprnomb;
                    ws.Cells[index, 2].Value = item.Gruponomb;
                    ws.Cells[index, 3].Value = item.Fenergnomb;
                    ws.Cells[index, 4].Value = item.Crdcgtipcom;
                    ws.Cells[index, 5].Value = item.Barradiaper;
                    ws.Cells[index, 6].Value = item.Considerarpotnominal;
                    ws.Cells[index, 7].Value = item.Crdcglhv;
                    ws.Cells[index, 8].Value = item.Crdcgtranspor;
                    ws.Cells[index, 9].Value = item.Crdcgtratmec;
                    ws.Cells[index, 10].Value = item.Crdcgtratquim;
                    ws.Cells[index, 11].Value = item.Crdcgdiasfinanc;
                    ws.Cells[index, 12].Value = item.Crdcgcvncdol;
                    ws.Cells[index, 13].Value = item.Crdcgcvncsol;
                    ws.Cells[index, 14].Value = item.Crdcgprecomb;
                    ws.Cells[index, 15].Value = item.Crdcgprecombunid;
                    ws.Cells[index, 16].Value = item.Vcedcmenergia;
                    ws.Cells[index, 17].Value = item.Vcedcmtiempo;
                    ws.Cells[index, 18].Value = item.Crdcgnumarrpar;
                    ws.Cells[index, 19].Value = item.Crdcgpotefe;
                    ws.Cells[index, 20].Value = item.Crdcgccpotefe;
                    ws.Cells[index, 21].Value = item.Crdcgpotpar1;
                    ws.Cells[index, 22].Value = item.Crdcgconcompp1;
                    ws.Cells[index, 23].Value = item.Crdcgpotpar2;
                    ws.Cells[index, 24].Value = item.Crdcgconcompp2;
                    ws.Cells[index, 25].Value = item.Crdcgpotpar3;
                    ws.Cells[index, 26].Value = item.Crdcgconcompp3;
                    ws.Cells[index, 27].Value = item.Crdcgpotpar4;
                    ws.Cells[index, 28].Value = item.Crdcgconcompp4;
                    ws.Cells[index, 29].Value = item.Crdcgpotmin;

                    rg = ws.Cells[index, 1, index, 29];
                    ObtenerEstiloCelda(rg, 0);

                    index++;

                }

                ws.Column(1).Width = 30;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;

                for (int i = 5; i <= 29; i++)
                {
                    ws.Column(i).AutoFit(14.5);
                }

                AgregarLogoHojaExcel(ws);
            }
        }

        public static void GenerarHojaExcelMedidores(ExcelPackage xlPackage, int pecacodi, List<VceEnergiaDTO> list)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Medidores");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "MEDIDORES (ENERGÍA)", true, 12, 1, 3);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 3);
            
            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int index = 6;
                ws.Cells[index, 1].Value = "Fecha";
                ws.Cells[index, 2].Value = "Total Energia";

                for (int i = 1; i <= 96; i++)
                {
                    ws.Cells[index, i + 2].Value = TimeSpan.FromMinutes(i * 15).ToString("hh':'mm");
                }

                ExcelRange rg = ws.Cells[index, 1, index, 98];
                rg = ObtenerEstiloCelda(rg, 1);

                index++;
                foreach (VceEnergiaDTO item in list)
                {
                    //Values
                    ws.Cells[index, 1].Value = item.Crmefecha;
                    ws.Cells[index, 2].Value = item.Crmemeditotal;
                    for (int i = 1; i <= 96; i++)
                    {
                        ws.Cells[index, i + 2].Value = item.GetType().GetProperty("Crmeh" + i).GetValue(item, null);
                    }

                    //Styles
                    ws.Cells[index, 1].Style.Numberformat.Format = "dd/mm/yyyy";
                    rg = ws.Cells[index, 1, index, 98];
                    rg = ObtenerEstiloCelda(rg, 0);
                    ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    index++;
                }

                ws.Column(1).Width = 30;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 25;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 18;
                ws.Column(6).Width = 18;
                ws.Column(7).Width = 18;
                ws.Column(8).Width = 18;
                ws.Column(9).Width = 30;
                ws.Column(10).Width = 15;
                ws.Column(11).Width = 15;
                ws.Column(12).Width = 15;
                ws.Column(13).Width = 15;
                ws.Column(14).Width = 30;

                for (int i = 1; i <= 98; i++)
                {
                    ws.Column(i).Width = 20;
                }


                AgregarLogoHojaExcel(ws);
            }
        }

        public static void GenerarHojaExcelCostosMarginales(ExcelPackage xlPackage, int pecacodi, List<string> listHead, List<string> listBody)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CMg");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "COSTOS MARGINALES", true, 12, 1, 3);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 3);

            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int index = 6;

                int c = 0;
                foreach (string item in listHead)
                {
                    c++;
                    ws.Cells[index, c].Value = item;
                }

                ExcelRange rg = ws.Cells[index, 1, index, c];
                rg = ObtenerEstiloCelda(rg, 1);

                index++;

                int x = 0;
                foreach (string item in listBody)
                {
                    x++;
                    // ws.Cells[index, x].Value = item;

                    int y = 0;
                    foreach (string s in item.Split('|'))
                    {
                        y++;
                        ws.Cells[index, y].Value = s;

                        ws.Cells[index, y].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    rg = ws.Cells[index, 1, index, y];
                    rg = ObtenerEstiloCelda(rg, 0);
                    ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    index++;
                }

                ws.Column(1).Width = 13;

                for (int i = 2; i <= c; i++)
                {
                    ws.Column(i).Width = 20;
                }

                AgregarLogoHojaExcel(ws);
            }
        }

        public static void GenerarHojaExcelHorasOperacion(ExcelPackage xlPackage, int pecacodi, List<VceHoraOperacionDTO> list)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("HO");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "HORAS DE OPERACIÓN", true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int index = 6;
                ws.Cells[index, 1].Value = "Empresa";
                ws.Cells[index, 2].Value = "Central";
                ws.Cells[index, 3].Value = "Grupo";
                ws.Cells[index, 4].Value = "Modo Operación";
                ws.Cells[index, 5].Value = "Código";
                ws.Cells[index, 6].Value = "Fecha Inicio";
                ws.Cells[index, 7].Value = "Fecha Fin";
                ws.Cells[index, 8].Value = "O. Arranque";
                ws.Cells[index, 9].Value = "O. Parada";
                ws.Cells[index, 10].Value = "Tipo Operación";
                ws.Cells[index, 11].Value = "Sistema";
                ws.Cells[index, 12].Value = "Lim. Transmisión";
                ws.Cells[index, 13].Value = "Comp. Arranques";
                ws.Cells[index, 14].Value = "Comp. Paradas";
                ws.Cells[index, 15].Value = "Observación";

                ExcelRange rg = ws.Cells[index, 1, index, 15];
                rg = ObtenerEstiloCelda(rg, 1);

                index++;
                foreach (VceHoraOperacionDTO item in list)
                {
                    //#Verificar

                    //Values
                    ws.Cells[index, 1].Value = item.Empresa;
                    ws.Cells[index, 2].Value = item.Central;
                    ws.Cells[index, 3].Value = item.Grupo;
                    ws.Cells[index, 4].Value = item.ModoOperacion;
                    ws.Cells[index, 5].Value = item.Hopcodi;

                    if (item.Crhophorini > new DateTime(1, 1, 1, 0, 0, 0))
                    {
                        ws.Cells[index, 6].Value = item.Crhophorini;
                    }

                    if (item.Crhophorfin > new DateTime(1, 1, 1, 0, 0, 0))
                    {
                        ws.Cells[index, 7].Value = item.Crhophorfin;
                    }

                    if (item.Crhophorarranq > new DateTime(1, 1, 1, 0, 0, 0))
                    {
                        ws.Cells[index, 8].Value = item.Crhophorarranq;
                    }

                    if (item.Crhophorparada > new DateTime(1, 1, 1, 0, 0, 0))
                    {
                        ws.Cells[index, 9].Value = item.Crhophorparada;
                    }

                    ws.Cells[index, 10].Value = item.TipoOperacion;
                    ws.Cells[index, 11].Value = item.Crhopsaislado;
                    ws.Cells[index, 12].Value = item.Crhoplimtrans;
                    ws.Cells[index, 13].Value = item.Crhopcompordarrq;
                    ws.Cells[index, 14].Value = item.Crhopcompordpard;
                    ws.Cells[index, 15].Value = item.Crhopdesc;

                    //Styles
                    ws.Cells[index, 6].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 7].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 8].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 9].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[index, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[index, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg = ws.Cells[index, 1, index, 15];
                    rg = ObtenerEstiloCelda(rg, 0);
                    ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    index++;
                }

                ws.Column(1).Width = 30;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 25;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 10;
                ws.Column(6).Width = 18;
                ws.Column(7).Width = 18;
                ws.Column(8).Width = 18;
                ws.Column(9).Width = 18;
                ws.Column(10).Width = 30;
                ws.Column(11).Width = 15;
                ws.Column(12).Width = 15;
                ws.Column(13).Width = 15;
                ws.Column(14).Width = 15;
                ws.Column(15).Width = 80;

                AgregarLogoHojaExcel(ws);
            }
        }

        // DSH 22-08-2017 : Se agrego por requerimiento
        public static void GenerarHojaExcelVerificarHorasOperacion(ExcelPackage xlPackage, int pecacodi, List<VceHoraOperacionDTO> list)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("VerificarHO");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "VERIFICAR HORAS DE OPERACIÓN", true, 12, 1, 2);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2);

            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int index = 6;
                // Cabecera nivel 1
                ws.Cells[index, 1].Value = "Modo Operación";
                ws.Cells[index, 2].Value = "Rango Horario 1";
                ws.Cells[index, 6].Value = "Rango Horario 2";

                ExcelRange rg = ws.Cells[index, 1, index + 1, 1];
                rg.Merge = true;
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 1);

                rg = ws.Cells[index, 2, index, 5];
                rg.Merge = true;
                rg = ObtenerEstiloCelda(rg, 1);

                rg = ws.Cells[index, 6, index, 9];
                rg.Merge = true;
                rg = ObtenerEstiloCelda(rg, 1);

                index++;
                // cabecera Nivel 2
                ws.Cells[index, 2].Value = "Código";
                ws.Cells[index, 3].Value = "Fecha Inicio";
                ws.Cells[index, 4].Value = "Fecha Fin";
                ws.Cells[index, 5].Value = "Tipo Operación";
                ws.Cells[index, 6].Value = "Código";
                ws.Cells[index, 7].Value = "Fecha Inicio";
                ws.Cells[index, 8].Value = "Fecha Fin";
                ws.Cells[index, 9].Value = "Tipo Operación";


                rg = ws.Cells[index, 1, index, 9];
                rg = ObtenerEstiloCelda(rg, 1);

                index++;
                foreach (VceHoraOperacionDTO item in list)
                {
                    //#Verificar

                    //Values
                    ws.Cells[index, 1].Value = item.ModoOperacion;
                    ws.Cells[index, 2].Value = item.Hopcodi;
                    ws.Cells[index, 3].Value = item.Crhophorini;
                    ws.Cells[index, 4].Value = item.Crhophorfin;
                    ws.Cells[index, 5].Value = item.TipoOperacion;
                    ws.Cells[index, 6].Value = item.Hopcodi2;
                    ws.Cells[index, 7].Value = item.Crhophorini2;
                    ws.Cells[index, 8].Value = item.Crhophorfin2;
                    ws.Cells[index, 9].Value = item.TipoOperacion2;

                    //Styles
                    ws.Cells[index, 3].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 4].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 7].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 8].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                    ws.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[index, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg = ws.Cells[index, 1, index, 9];
                    rg = ObtenerEstiloCelda(rg, 0);
                    ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    index++;
                }

                ws.Column(1).Width = 30;
                ws.Column(2).Width = 10;
                ws.Column(3).Width = 18;
                ws.Column(4).Width = 18;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 10;
                ws.Column(7).Width = 18;
                ws.Column(8).Width = 18;
                ws.Column(9).Width = 25;

                AgregarLogoHojaExcel(ws);
            }
        }
        
        //
        public static void GenerarHojaExcelArranquesParadas(ExcelPackage xlPackage, int pecacodi)
        {
            string titulo="";
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            CompensacionAppServicio servicio = new CompensacionAppServicio();

      
            // Lista que almcacena codigo de grupo, nombre de hoja, titulo1, titulo 2, titulo de campos
            // DSH 03-07-2017 : los titulos se obtendran desde la tabla VCE_TEXTO_REPORTE
            List<List<String>> matrix = new List<List<String>>();

            // Hoja 1
            matrix.Add(new List<String>());
            matrix[0].Add("G1");
            matrix[0].Add("Cuadro PyE, IO, Seg.");
            matrix[0].Add("COMPENSACIÓN DE COSTOS OPERATIVOS Ccbef, Ccadic Y Cmarr:");
            matrix[0].Add("Unidades que operaron \"por Potencia y Energía\", \"por Seguridad\" y  \"por inflexibilidad operativa\"(incluidas las pruebas de disponibilidad del PR-25)");
            matrix[0].Add("Empresa");
            matrix[0].Add("CCbef para VTEA S/.");
            matrix[0].Add("CMarr para VTEA S/.");
            matrix[0].Add("CCadic para VTEA S/.");
            matrix[0].Add("Compensacion Total S/.");

            // Hoja 2
            matrix.Add(new List<String>());
            matrix[1].Add("G3");
            matrix[1].Add("Cuadro RSF");
            matrix[1].Add("COMPENSACIÓN DE COSTOS OPERATIVOS Ccbef, Ccadic Y Cmarr:");
            matrix[1].Add("Unidades que operaron \"por RSF\" y \"por Reserva Especial\"");
            matrix[1].Add("Empresa");
            matrix[1].Add("CCbef para Liq. RSF S/.");
            matrix[1].Add("CMarr para Liq. RSF S/.");
            matrix[1].Add("CCadic para Liq. RSF S/.");
            matrix[1].Add("Compensacion Total S/.");

            // Hoja 3
            matrix.Add(new List<String>());
            matrix[2].Add("G2");
            matrix[2].Add("Cuadro RT");
            matrix[2].Add("COMPENSACIÓN DE COSTOS OPERATIVOS Ccbef, Ccadic Y Cmarr:");
            matrix[2].Add("Unidades que operaron \"por Regulación de Tensión\"");
            matrix[2].Add("Empresa");
            matrix[2].Add("CCbef para VTER* S/.");
            matrix[2].Add("CMarr para VTER* S/.");
            matrix[2].Add("CCadic para VTER* S/.");
            matrix[2].Add("Compensacion Total S/.");

            for (int e = 0; e < 3; e++)
            {
                // Obtener consulta por pecacodi y grupo
                //IDataReader dr = servicio.ListByGroupCompArrpar(pecacodi, matrix[e][0]);
                List<VceArrparGrupoCabDTO> list = servicio.ListByGroupCompArrpar(pecacodi, matrix[e][0]);

                // Agregar hoja
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(matrix[e][1]);

               
                // Titulo 1
                string codreporte = "EX_ARRPAR_COSOPE_" + matrix[e][0];
                VceTextoReporteDTO objTextoReporte = servicio.getTextoReporteById(pecacodi, codreporte, "TITULO1");

                if (objTextoReporte != null) { titulo = objTextoReporte.Txtreptexto; }

                AgregarTextoHojaExcel(ws, titulo, true, 12, 1, 2,4);

                // Titulo 2
                titulo = "";
                if(e.Equals(1) && matrix[e][0] == "G3")
                {
                    codreporte = "EX_ARRPAR_COSOPE_" + matrix[e + 1][0];
                }else if (e.Equals(2) && matrix[e][0] == "G2")
                {
                    codreporte = "EX_ARRPAR_COSOPE_" + matrix[e - 1][0];
                }
                    objTextoReporte = servicio.getTextoReporteById(pecacodi, codreporte, "TITULO2");

                if (objTextoReporte != null) { titulo = objTextoReporte.Txtreptexto; }

                ws.Row(2).Height = 30;
                AgregarTextoHojaExcel(ws, titulo, true, 11, 2, 2, 4, true, true);
         
                // Periodo-Version
                AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 2, 4);

                // Fecha
                DateTime fecha = DateTime.Now;
                ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {
                    int nroFila = 6;

                    // Cabecera Manual
                    ws.Cells[nroFila, 1].Value = matrix[e][4];
                    ws.Cells[nroFila, 2].Value = matrix[e][5];
                    ws.Cells[nroFila, 3].Value = matrix[e][6];
                    ws.Cells[nroFila, 4].Value = matrix[e][7];
                    ws.Cells[nroFila, 5].Value = matrix[e][8];

                    ws.Row(nroFila).Height = 30;
                    ExcelRange rg = ws.Cells[nroFila, 1, nroFila, 5];
                    rg = ObtenerEstiloCelda(rg, 1);
                    rg.Style.WrapText = true;

                    foreach (VceArrparGrupoCabDTO item in list)
                    {
                        //incremetar numero de fila
                        nroFila++;
                        ws.Cells[nroFila, 1].Value = item.Emprnomb;
                        ws.Cells[nroFila, 2].Value = item.Apgcabccbef;
                        ws.Cells[nroFila, 3].Value = item.Apgcabccmarr;
                        ws.Cells[nroFila, 4].Value = item.Apgcabccadic;
                        ws.Cells[nroFila, 5].Value = item.Apgcabtotal;

                        // Asigna formato numerico
                        ws.Cells[nroFila, 2, nroFila, 5].Style.Numberformat.Format = "#,##0.00;-#,##0.00;0";

                        rg = ws.Cells[nroFila, 1, nroFila, 5];
                        rg = ObtenerEstiloCelda(rg, 0);

                    }

                    // Ancho de columnas
                    ws.Column(1).Width = 30;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;

                    AgregarLogoHojaExcel(ws);

                }
            }

        }


        // DSH 20-04-2017 - Generar formato excel Arranques y Paradas
        public static void GenerarFormatoExcelDetalleArranquesParadas(string fileName, int pecacodi, IDataReader drList, IDataReader drCabList)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarHojaExcelDetalleArranquesParadas(xlPackage, pecacodi, drList, drCabList);
                xlPackage.Save();
            }
        }

        // DSH 27-04-2017 : Se genero por requerimiento, para se usado en Exportar datos
        public static void GenerarHojaExcelDetalleArranquesParadas(ExcelPackage xlPackage, int pecacodi, IDataReader drList, IDataReader drCabList)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Detalle Arranques y Paradas");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "LISTADO DE ARRANQUES Y PARADAS", true, 12, 1, 3);

            // Periodo-Version
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 3);

            DateTime fecha = DateTime.Now;

            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                List<ExcelStruct> lstCabecera = new List<ExcelStruct>();
                List<string> lstCuerpo = new List<string>();


                int nroFila = 6;
                int nroCol = 0;
                int nroCol2 = 0;
                string tituloGrupo = "";
                int nroCeldasUnir = 0;
                ExcelRange rg;

                // CABECERA DE GRUPO
                using (drCabList)
                {
                    while (drCabList.Read())
                    {
                        nroCol++;
                        tituloGrupo = drCabList["TITULO"].ToString();
                        nroCeldasUnir = int.Parse(drCabList["COLSPAN"].ToString());
                        ws.Cells[nroFila, nroCol].Value = tituloGrupo;

                        if (nroCeldasUnir > 1)
                        {
                            nroCol2 = (nroCol + nroCeldasUnir) - 1;
                            rg = ws.Cells[nroFila, nroCol, nroFila, nroCol2];
                            rg.Merge = true;            // unir celdas
                            rg.Style.WrapText = true;   // ajustar celas
                            nroCol = nroCol2;
                        }

                    }
                }

                ws.Row(nroFila).Height = 30;    // Alto de fila
                rg = ws.Cells[nroFila, 1, nroFila, nroCol];
                rg = ObtenerEstiloCelda(rg, 1);

                nroFila = 7;

                using (drList)
                {
                    //CABECERA
                    for (int i = 0; i < drList.FieldCount; i++)
                    {
                        ExcelStruct data = new ExcelStruct();
                        data.indice = i;
                        data.nombre = drList.GetName(i).Trim();
                        data.tipo = drList.GetDataTypeName(i).Trim();
                        lstCabecera.Add(data);
                        nroCol = i + 1;
                        ws.Cells[nroFila, nroCol].Value = data.nombre;
                        ws.Column(nroCol).AutoFit();
                    }

                    rg = ws.Cells[nroFila, 1, nroFila, nroCol];
                    rg = ObtenerEstiloCelda(rg, 1);

                    //CUERPO
                    while (drList.Read())
                    {
                        nroCol = 0;

                        //incremetar numero de fila
                        nroFila++;

                        foreach (ExcelStruct item in lstCabecera)
                        {
                            // incrementar indice de la columna
                            nroCol++;

                            if (item.tipo.Contains("Double") || item.tipo.Contains("Float") || item.tipo.Contains("Decimal"))
                            {
                                ws.Cells[nroFila, nroCol].Style.Numberformat.Format = "#,##0.00;-#,##0.00;0";
                            }
                            else
                            {
                                // valor = drList[item.nombre].ToString();
                            }

                            ws.Cells[nroFila, nroCol].Value = drList[item.indice];
                        }

                        rg = ws.Cells[nroFila, 1, nroFila, nroCol];
                        rg = ObtenerEstiloCelda(rg, 0);
                        //ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    }
                }

                // Ancho de columnas
                ws.Column(1).Width = 10;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 25;

                AgregarLogoHojaExcel(ws);
            }
        }
        
        //-- 
        // DSH 20-06-2017 - Generar formato excel Asociacion de Puntos de medicion
        public static void GenerarFormatoExcelPtoGrupo(string fileName, int  pecacodi, List<string> ListCab, List<ComboCompensaciones> ListBody)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarHojaExcelPtoGrupo(xlPackage, pecacodi, ListCab, ListBody);
                xlPackage.Save();
            }
        }

                
        // DSH 20-06-2017 : Se genero por requerimiento
        public static void GenerarHojaExcelPtoGrupo(ExcelPackage xlPackage, int pecacodi, List<string> ListCab, List<ComboCompensaciones> ListBody)
        {
            string periodoVersion = string.Empty;

            periodoVersion = ObtenerDescripcionPeriodoVersion(pecacodi);

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Asociacion de Ptos. Medicion");

            // Titulo 1
            AgregarTextoHojaExcel(ws, "LISTADO DE ASOCIACION DE PUNTOS DE MEDICION", true, 12, 1, 3);
            
            // Periodo
            AgregarTextoHojaExcel(ws, periodoVersion, false, 11, 3, 3);

            // Fecha
            DateTime fecha = DateTime.Now;
            ws.Cells[4, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

            //Colores
            Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
            Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
            Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

            if (ws != null)
            {
                int nroFila = 6;
                int nroCol = 0;
                int nroCol2 = 0;

                // Cabecera
                foreach (string item in ListCab)
                {
                    nroCol++;
                    ws.Cells[nroFila, nroCol].Value = item;
                }

                ExcelRange rg = ws.Cells[nroFila, 1, nroFila, nroCol];
                rg = ObtenerEstiloCelda(rg, 1);

                nroFila++;

                // Detalle
                foreach (ComboCompensaciones item in ListBody)
                {
                    nroCol2 = 0;

                    foreach (string s in item.name.Split('|'))
                    {
                        nroCol2++;
                        ws.Cells[nroFila, nroCol2].Value = s;
                    }

                    rg = ws.Cells[nroFila, 1, nroFila, nroCol2];
                    rg = ObtenerEstiloCelda(rg, 0);
                    nroFila++;
                }

                for (int i = 1; i <= nroCol; i++)
                {
                    ws.Column(i).Width = 20;
                }

                AgregarLogoHojaExcel(ws);
                //HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesCompensacion.EnlaceLogoCoes);
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                //ExcelPicture picture = ws.Drawings.AddPicture(ConstantesCompensacion.NombreLogo, img);
                //picture.SetPosition(10, 20);
                //picture.SetSize(180, 60);
            }
        }
        
        /// <summary>
        /// DEscarga Formato Compensacion Manual
        /// </summary>
        /// <param name="fileName"></param>
        public static void DescargarFormatoCargaCompensacionManual(string fileName)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CompMMEDet");

                ExcelRange rg;

                //if (ws != null)
                //{
                //    int cantidad = 200;
                //    int index = 1;

                //    ws.Cells[index, 1].Value = "Empresa";
                //    ws.Cells[index, 2].Value = "Calificación";
                //    ws.Cells[index, 3].Value = "Modo Operación";
                //    ws.Cells[index, 4].Value = "Inicio";
                //    ws.Cells[index, 5].Value = "Fin";
                //    ws.Cells[index, 6].Value = "Potencia";
                //    ws.Cells[index, 7].Value = "Consumo";
                //    ws.Cells[index, 8].Value = "CVC";
                //    ws.Cells[index, 9].Value = "CVNC";
                //    ws.Cells[index, 10].Value = "CV";
                //    ws.Cells[index, 11].Value = "Compensación (S/.) ";

                //    rg = ws.Cells[index, 1, index, 11];
                //    rg = ObtenerEstiloCelda(rg, 1);

                //    index++;

                //    for (int i = 2; i <= cantidad; i++)
                //    {
                //        //Styles for Fecha
                //        ws.Cells[index, 4].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                //        ws.Cells[index, 5].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                //        ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        ws.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //        index++;
                //    }

                //    ws.Column(1).Width = 30;
                //    ws.Column(2).Width = 30;
                //    ws.Column(3).Width = 40;
                //    ws.Column(4).Width = 20;
                //    ws.Column(5).Width = 20;
                //    ws.Column(6).Width = 15;
                //    ws.Column(7).Width = 15;
                //    ws.Column(8).Width = 15;
                //    ws.Column(9).Width = 15;
                //    ws.Column(10).Width = 15;
                //    ws.Column(11).Width = 20;
                //}

                //ws = xlPackage.Workbook.Worksheets.Add("CompRegularDet");
                if (ws != null)
                {
                    int cantidad = 200;
                    int index = 1;


                    ws.Cells[index, 1].Value = "Empresa";
                    ws.Cells[index, 2].Value = "Calificación";
                    ws.Cells[index, 3].Value = "Modo Operación";
                    ws.Cells[index, 4].Value = "Periodo (dd/mm/yyyy : hh:mm)";
                    ws.Cells[index, 5].Value = "Energía";
                    ws.Cells[index, 6].Value = "Potencia";
                    ws.Cells[index, 7].Value = "Consumo Combustible";
                    ws.Cells[index, 8].Value = "Precio Aplicar";
                    ws.Cells[index, 9].Value = "CVC";
                    ws.Cells[index, 10].Value = "CVNC";
                    ws.Cells[index, 11].Value = "CVT";
                    ws.Cells[index, 12].Value = "CMG";
                    ws.Cells[index, 13].Value = "Compensación (S/.) ";

                    rg = ws.Cells[index, 1, index, 13];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;

                    for (int i = 2; i <= cantidad; i++)
                    {
                        //Styles for Fecha
                        ws.Cells[index, 4].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                        ws.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        index++;
                    }

                    ws.Column(1).Width = 25;
                    ws.Column(2).Width = 25;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 28;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 15;
                    ws.Column(12).Width = 15;
                    ws.Column(13).Width = 20;
                }

                xlPackage.Save();
            }
        }

        public static void DescargarFormatoCargaIncrementoReduccion(string fileName)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("IncrementoReduccion");

                ExcelRange rg;

                if (ws != null)
                {
                    int cantidad = 200;
                    int index = 1;

                    ws.Cells[index, 1].Value = "Modo Operación";
                    ws.Cells[index, 2].Value = "Fecha";
                    ws.Cells[index, 3].Value = "Incremento";
                    ws.Cells[index, 4].Value = "Reducción";

                    rg = ws.Cells[index, 1, index, 4];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;

                    for (int i = 2; i <= cantidad; i++)
                    {
                        //Styles for Fecha
                        ws.Cells[index, 2].Style.Numberformat.Format = "dd/mm/yyyy";
                        ws.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        index++;
                    }

                    ws.Column(1).Width = 40;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 15;

                }

                xlPackage.Save();
            }
        }

        public static string CargaManual(string file, int pecacodi)
        {
            try
            {
                CompensacionAppServicio servicio = new CompensacionAppServicio();

                int cantidad = 2000;

                //List<VceCompBajaeficDTO> listVceCompBajaefic = new List<VceCompBajaeficDTO>();
                List<VceCompMMEDetManualDetDTO> listVceCompRegularDet = new List<VceCompMMEDetManualDetDTO>();

                FileInfo fileInfo = new FileInfo(file);
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    //ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                    //for (int i = 2; i <= cantidad; i++)
                    //{
                    //    if (ws.Cells[i, 1].Value != null && ws.Cells[i, 1].Value != string.Empty)
                    //    {
                    //        VceCompBajaeficDTO item = new VceCompBajaeficDTO();

                    //        //Obtenemos los valores de Calificacion Y Modo de Operación
                    //        string calificacion = (ws.Cells[i, 2].Value != null) ? ws.Cells[i, 2].Value.ToString() : string.Empty;
                    //        string modo = (ws.Cells[i, 3].Value != null) ? ws.Cells[i, 3].Value.ToString() : string.Empty;

                    //        int subCausaCodi = servicio.GetSubCasusaCodi(calificacion);
                    //        if (subCausaCodi == 0)
                    //        {
                    //            return "No se encontro la Calificación: " + calificacion;
                    //        }

                    //        int grupoCodi = servicio.GetGrupoCodi(modo);
                    //        if (grupoCodi == 0)
                    //        {
                    //            return "No se encontro el Modo de Operación: " + modo;
                    //        }

                    //        item.Subcausacodi = subCausaCodi;
                    //        item.Grupocodi = grupoCodi;
                    //        //item.Crcbehorini = (ws.Cells[i, 4].Value != null) ? DateTime.Parse(ws.Cells[i, 4].Value.ToString()) : DateTime.Now;
                    //        //item.Crcbehorfin = (ws.Cells[i, 5].Value != null) ? DateTime.Parse(ws.Cells[i, 5].Value.ToString()) : DateTime.Now;
                    //        if (ws.Cells[i, 4].Value != null)
                    //        {
                    //            var datoFecha = ws.Cells[i, 4].Value.ToString().Split(' ');
                    //            var hora = datoFecha[1].Trim().Substring(0, 5);
                    //            //var minutos = datoFecha[1].Trim().Substring(2, 2);

                    //            var fechaFinal = datoFecha[0].Trim() + " " + hora;
                    //            item.Crcbehorini = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy HH:mm", null);
                    //        }
                    //        else
                    //        {
                    //            item.Crcbehorini = DateTime.Now;
                    //        }
                    //        if (ws.Cells[i, 5].Value != null)
                    //        {
                    //            var datoFecha = ws.Cells[i, 5].Value.ToString().Split(' ');
                    //            var hora = datoFecha[1].Trim().Substring(0, 5);
                    //            //var minutos = datoFecha[1].Trim().Substring(2, 2);

                    //            var fechaFinal = datoFecha[0].Trim() + " " + hora;
                    //            item.Crcbehorfin = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy HH:mm", null);
                    //        }
                    //        else
                    //        {
                    //            item.Crcbehorfin = DateTime.Now;
                    //        }
                    //        item.Crcbepotencia = (ws.Cells[i, 6].Value != null) ? decimal.Parse(ws.Cells[i, 6].Value.ToString()) : 0;
                    //        item.Crcbeconsumo = (ws.Cells[i, 7].Value != null) ? decimal.Parse(ws.Cells[i, 7].Value.ToString()) : 0;
                    //        item.Crcbecvc = (ws.Cells[i, 8].Value != null) ? decimal.Parse(ws.Cells[i, 8].Value.ToString()) : 0;
                    //        item.Crcbecvnc = (ws.Cells[i, 9].Value != null) ? decimal.Parse(ws.Cells[i, 9].Value.ToString()) : 0;
                    //        item.Crcbecvt = (ws.Cells[i, 10].Value != null) ? decimal.Parse(ws.Cells[i, 10].Value.ToString()) : 0;
                    //        item.Crcbecompensacion = (ws.Cells[i, 11].Value != null) ? decimal.Parse(ws.Cells[i, 11].Value.ToString()) : 0;
                    //        item.PecaCodi = pecacodi;
                    //        item.Crcbetipocalc = "M";

                    //        listVceCompBajaefic.Add(item);
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}
                    ////Eliminamos los registros VceCompBajaeficDTO
                    //servicio.DeleteCompensacionCab(pecacodi);

                    ////Guardamos los registros
                    //foreach (VceCompBajaeficDTO entity in listVceCompBajaefic)
                    //{
                    //    servicio.SaveVceCompBajaefic(entity);
                    //}

                    ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets[1];
                    for (int i = 2; i <= cantidad; i++)
                    {
                        if (ws2.Cells[i, 1].Value != null && ws2.Cells[i, 1].Value != string.Empty)
                        {
                            VceCompMMEDetManualDetDTO item = new VceCompMMEDetManualDetDTO();

                            //Obtenemos los valores de Calificacion Y Modo de Operación
                            string calificacion = (ws2.Cells[i, 2].Value != null) ? ws2.Cells[i, 2].Value.ToString() : string.Empty;
                            string modo = (ws2.Cells[i, 3].Value != null) ? ws2.Cells[i, 3].Value.ToString() : string.Empty;

                            int subCausaCodi = servicio.GetSubCasusaCodi(calificacion);
                            if (subCausaCodi == 0)
                            {
                                return "No se encontro la Calificación: " + calificacion;
                            }

                            int grupoCodi = servicio.GetGrupoCodi(modo);
                            if (grupoCodi == 0)
                            {
                                return "No se encontro el Modo de Operación: " + modo;
                            }

                            item.Subcausacodi = subCausaCodi;
                            item.Grupocodi = grupoCodi;
                            //item.Crdethora = (ws2.Cells[i, 4].Value != null) ? DateTime.Parse(ws2.Cells[i, 4].Value.ToString()) : DateTime.Now;
                            //item.Crdethora = (ws2.Cells[i, 4].Value != null) ? DateTime.ParseExact(ws2.Cells[i, 4].Value.ToString(), "dd/MM/yyyy HH:mm:ss", null) : DateTime.Now;
                            if(ws2.Cells[i, 4].Value != null)
                            {
                                
                                var fechaConv = DateTime.ParseExact(ws2.Cells[i, 4].Value.ToString(), "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);                                                               

                                item.Cmmedmhora = fechaConv;
                            }
                            else
                            {
                                item.Cmmedmhora = DateTime.Now;
                            }
                            item.Cmmedmenergia = (ws2.Cells[i, 5].Value != null) ? decimal.Parse(ws2.Cells[i, 5].Value.ToString()) : 0;
                            item.Cmmedmpotencia = (ws2.Cells[i, 6].Value != null) ? decimal.Parse(ws2.Cells[i, 6].Value.ToString()) : 0;
                            item.Cmmedmconsumocomb = (ws2.Cells[i, 7].Value != null) ? decimal.Parse(ws2.Cells[i, 7].Value.ToString()) : 0;
                            item.Cmmedmprecioaplic = (ws2.Cells[i, 8].Value != null) ? decimal.Parse(ws2.Cells[i, 8].Value.ToString()) : 0;
                            item.Cmmedmcvc = (ws2.Cells[i, 9].Value != null) ? decimal.Parse(ws2.Cells[i, 9].Value.ToString()) : 0;
                            item.Cmmedmcvnc = (ws2.Cells[i, 10].Value != null) ? decimal.Parse(ws2.Cells[i, 10].Value.ToString()) : 0;
                            item.Cmmedmcvt = (ws2.Cells[i, 11].Value != null) ? decimal.Parse(ws2.Cells[i, 11].Value.ToString()) : 0;
                            item.Cmmedmcmg = (ws2.Cells[i, 12].Value != null) ? decimal.Parse(ws2.Cells[i, 12].Value.ToString()) : 0;
                            item.Cmmedmcompensacion = (ws2.Cells[i, 13].Value != null) ? decimal.Parse(ws2.Cells[i, 13].Value.ToString()) : 0;

                            item.PecaCodi = pecacodi;
                            item.Cmmedmtipocalc = "M";

                            listVceCompRegularDet.Add(item);
                        }
                        else
                        {
                            break;
                        }
                    }

                    //eliminamos los registros
                    foreach (VceCompMMEDetManualDetDTO entity in listVceCompRegularDet)
                    {
                        //Eliminamos los registros Compensacion Manual
                        servicio.DeleteCompensacionManual(pecacodi, entity.Grupocodi, entity.Cmmedmhora);
                        
                    }

                    //Guardamos los registros
                    foreach (VceCompMMEDetManualDetDTO entity in listVceCompRegularDet)
                    {                        

                        servicio.SaveCompMMEDetManual(entity);
                    }

                    //Actualizamos los registros en la tabla temporal
                    //servicio.UpdateCompensacionDetManual(pecacodi);

                    //Insertamos los registros en la tabla temporal
                    //servicio.SaveCompensacionDetManual(pecacodi);

                    //Actualizamos los registros en la tabla Comepensacion Detalle
                    servicio.UpdateCompensacionDet(pecacodi);

                    //Insertamos los registros en la tabla Comepensacion Detalle
                    servicio.SaveCompensacionDet(pecacodi);

                }

                return "Datos Cargados Correctamente.";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public static string CargaManualIncred(string file, int pericodi, int pecacodi, string usuario)
        {
            try
            {
                CompensacionAppServicio servicio = new CompensacionAppServicio();

                int cantidad = 200;

                List<VceArrparIncredGenDTO> listVceArrparIncredGen = new List<VceArrparIncredGenDTO>();

                FileInfo fileInfo = new FileInfo(file);
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                    for (int i = 2; i <= cantidad; i++)
                    {
                        if (ws.Cells[i, 1].Value != null && ws.Cells[i, 1].Value != string.Empty)
                        {
                            VceArrparIncredGenDTO item = new VceArrparIncredGenDTO();

                            //Obtenemos Modo de Operación
                            string modo = (ws.Cells[i, 1].Value != null) ? ws.Cells[i, 1].Value.ToString() : string.Empty;


                            int grupoCodi = servicio.GetGrupoCodi(modo);
                            if (grupoCodi == 0)
                            {
                                return "No se encontro el Modo de Operación: " + modo;
                            }


                            item.PecaCodi = pecacodi;
                            item.Grupocodi = grupoCodi;

                            // DSH 16-04-2017 : Inicio de Validacion
                            //item.Apinrefecha = (ws.Cells[i, 2].Value != null) ? DateTime.Parse(ws.Cells[i, 2].Value.ToString()) : DateTime.Now;

                            if (ws.Cells[i, 2].Value != null && ws.Cells[i, 2].Value != string.Empty)
                            {
                                DateTime fechavalida;

                                string fechatexto = ws.Cells[i, 2].Value.ToString();

                                // validar conersion de fechatexto
                                if (DateTime.TryParse(fechatexto, out fechavalida))
                                {
                                    item.Apinrefecha = DateTime.Parse(fechatexto);

                                }
                                else
                                {
                                    return "No es valido el valor fecha: " + fechatexto + ", Revisar fila: " + (i);
                                }
                            }
                            else
                            {
                                item.Apinrefecha = DateTime.Now;
                            }

                            // validar año y mes de la fecha  
                            PeriodoDTO per = servicio.getPeriodoById(pericodi);

                            if (item.Apinrefecha.Year != per.AnioCodi || item.Apinrefecha.Month != per.MesCodi)
                            {
                                return "No corresponde al periodo la fecha: " + item.Apinrefecha.ToShortDateString() + ", Revisar fila: " + (i);
                            }


                            //item.Apinrenuminc = (ws.Cells[i, 3].Value != null) ? int.Parse(ws.Cells[i, 3].Value.ToString()) : 0;

                            // Validacion de Incremento
                            if (ws.Cells[i, 3].Value != null && ws.Cells[i, 3].Value != string.Empty)
                            {
                                ushort incremento = 0;

                                string numeroTexto = ws.Cells[i, 3].Value.ToString();

                                if (ushort.TryParse(numeroTexto, out incremento))
                                {
                                    item.Apinrenuminc = int.Parse(numeroTexto);

                                }
                                else
                                {
                                    return "No es valido el valor de incremento: " + numeroTexto + ", Revisar fila: " + (i);
                                }
                            }
                            else
                            {
                                item.Apinrenuminc = 0;
                            }

                            //item.Apinrenumdis = (ws.Cells[i, 4].Value != null) ? int.Parse(ws.Cells[i, 4].Value.ToString()) : 0;

                            // Validacion de Reduccion
                            if (ws.Cells[i, 4].Value != null && ws.Cells[i, 4].Value != string.Empty)
                            {
                                ushort reduccion = 0;

                                string numeroTexto = ws.Cells[i, 4].Value.ToString();

                                if (ushort.TryParse(numeroTexto, out reduccion))
                                {
                                    item.Apinrenumdis = int.Parse(numeroTexto);

                                }
                                else
                                {
                                    return "No es valido el valor de reduccion: " + numeroTexto + ", Revisar fila: " + (i);
                                }
                            }
                            else
                            {
                                item.Apinrenumdis = 0;
                            }

                            // validar incremento y reduccion
                            if (item.Apinrenuminc == 0 && item.Apinrenumdis == 0)
                            {
                                return "No es valido el incremento : " + item.Apinrenumdis + " reduccion :" + item.Apinrenumdis + " , Revisar fila: " + (i);
                            }

                            item.Apinreusucreacion = usuario;
                            item.Apinrefeccreacion = DateTime.Now;

                            listVceArrparIncredGen.Add(item);

                        }
                        else
                        {
                            break;
                        }
                    }
                    //Eliminamos los registros de Incrementos y Reduciones
                    foreach (VceArrparIncredGenDTO entity in listVceArrparIncredGen)
                    {
                        servicio.EliminarIncrementoReduccion(entity);
                    }

                    //Guardamos los registros
                    foreach (VceArrparIncredGenDTO entity in listVceArrparIncredGen)
                    {
                        servicio.CrearIncrementoReduccion(entity);
                    }

                }

                return "Datos Cargados Correctamente.";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        // DSH 30-06-2017 : Obtiene el Nombre del periodo y la version
        public static string ObtenerDescripcionPeriodoVersion(int pecacodi)
        {
            int pericodi;
            string descripcion;

            CompensacionAppServicio servicio = new CompensacionAppServicio();

            VcePeriodoCalculoDTO obj = servicio.getVersionPeriodoById(pecacodi);
            pericodi = obj.PeriCodi;

            PeriodoDTO objTrnPeriodo = servicio.getPeriodoById(pericodi);
            descripcion = "Periodo : " + objTrnPeriodo.PeriNombre.Trim() + "     Version : " + obj.PecaNombre.Trim();

            return descripcion;
        }

        // DSH 03-07-2017 : Agregar el texto a una Hoja de Excel, util para titulos
        public static void AgregarTextoHojaExcel(ExcelWorksheet ws, string texto, bool aplicarNegrita, int sizeTexto, int fila, int columna, int longitudenColumnas = 3, 
            bool ajustarTexto = true, bool alineacionVerticalCentrar = false)
        {
            ws.Cells[fila, columna].Value = texto;
            ExcelRange rg = ws.Cells[fila, columna, fila, columna + (longitudenColumnas - 1)];
            rg.Merge = true;
            rg.Style.Font.Size = sizeTexto;
            rg.Style.Font.Bold = aplicarNegrita;
            rg.Style.WrapText = ajustarTexto;
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            if (alineacionVerticalCentrar) { rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center; }
        }

        // DSH 03-07-2017 : Agregar Logo a una hoja de Excel
        public static void AgregarLogoHojaExcel(ExcelWorksheet ws)
        {
            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
            ExcelPicture picture = ws.Drawings.AddPicture(ConstantesCompensacion.NombreLogo, img);
            picture.SetPosition(10, 20);
            picture.SetSize(180, 60);

        }


    }
}
