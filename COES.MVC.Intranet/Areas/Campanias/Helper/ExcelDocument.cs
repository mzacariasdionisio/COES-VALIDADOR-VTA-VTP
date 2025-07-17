using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Campanias.Models;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using COES.Dominio.DTO.Campania;
using COES.Servicios.Aplicacion.Campanias;
using System.Globalization;
using DevExpress.ClipboardSource.SpreadsheetML;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace COES.MVC.Intranet.Areas.Campanias.Helper
{
    public class ExcelDocument
    {
        /// <summary>
        /// Configura los encabezados y titulos del reporte excel
        /// </summary>
        /// <param name="ws"></param>
        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, List<string> listaFechas)
        {

            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            int numCeldaFinal = listaFechas.Count + 3;
            var fill = ws.Cells[6, 2, 6, numCeldaFinal].Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Gray);
            var border = ws.Cells[6, 2, 6, numCeldaFinal].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells[6, 2, 6, numCeldaFinal])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[6, 2].Value = "";
            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20;

            int numCeldaInicio = 3;
            foreach (var fecha in listaFechas)
            {
                ws.Cells[6, numCeldaInicio].Value = fecha;
                ws.Column(numCeldaInicio).Width = 15;
                numCeldaInicio++;
            }
            ws.Cells[6, numCeldaInicio].Value = "";
            ws.Column(numCeldaInicio).Width = 15;


        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Generacion Central Hidro en formato excel
        /// </summary>
        /// <param name="model"></param>
        /// 
        public static void GenerarExcelPlanTransmision(List<PlanTransmisionDTO> listaProyecto)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas + ConstantesCampanias.FolderReporte;
            FileInfo template = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombrePlanTransmision));
            FileInfo newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombrePlanTransmisionNew));

            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombrePlanTransmisionNew));
            }

            int index = 5;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets["Reporte"];
                foreach (PlanTransmisionDTO item in listaProyecto)
                {
                    ws.Cells[index, 1].Value = item.Plancodi;
                    ws.Cells[index, 2].Value = item.Nomempresa;
                    ws.Cells[index, 3].Value = item.Fecenvio.HasValue ? item.Fecenvio.Value.ToString("dd/MM/yyyy") : "";
                    ws.Cells[index, 4].Value = item.Numreg;
                    ws.Cells[index, 5].Value = item.Planversion;
                    ws.Cells[index, 6].Value = item.Planestado;
                    ws.Cells[index, 7].Value = item.Plancumplimiento;
                    index++;
                }
                xlPackage.Save();
            }
        }
        public static void GenerarExcelFichaProyectoTipoGeneracionCentralHidro(ProyectoModel proyectoModel,string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                        ruta,
                        ConstantesCampanias.FolderTemp,
                        identificadorUnico,ConstantesCampanias.FolderFichasGeneracion,
                        ConstantesCampanias.FolderFichasGeneracionCHidro, 
                        proyectoModel.TransmisionProyectoDTO.Proynombre+"-"+proyectoModel.TransmisionProyectoDTO.Proycodi
                    );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasGeneracionSubtipo, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralHidro));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralHidro));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }

            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralHidro));
            }

            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato FichaA
                RegHojaADTO regHojaADTO = proyectoModel.RegHojaADTO;
                ws = xlPackage.Workbook.Worksheets["FichaA"];
                if (regHojaADTO != null)
                {
                    ws.Cells[9, 5].Value = regHojaADTO.Centralnombre;
                    if (proyectoModel.ubicacionDTO != null) {
                        ws.Cells[10, 5].Value = proyectoModel.ubicacionDTO.Departamento;
                        ws.Cells[11, 5].Value = proyectoModel.ubicacionDTO.Provincia;
                        ws.Cells[12, 5].Value = proyectoModel.ubicacionDTO.Distrito;
                    }
                    ws.Cells[13, 5].Value = regHojaADTO.Cuenca;
                    ws.Cells[14, 5].Value = regHojaADTO.Rio;
                    ws.Cells[15, 5].Value = regHojaADTO.Propietario;
                    ws.Cells[16, 5].Value = regHojaADTO.Sociooperador;
                    ws.Cells[17, 5].Value = regHojaADTO.Socioinversionista;
                    ws.Cells[23, 5].Value = regHojaADTO.Concesiontemporal;
                    ws.Cells[24, 5].Value = regHojaADTO.Tipoconcesionactual;
                    ws.Cells[23, 9].Value = regHojaADTO.Fechaconcesiontemporal;
                    ws.Cells[24, 9].Value = regHojaADTO.Fechaconcesionactual;
                    ws.Cells[31, 3].Value = regHojaADTO.Nombreestacion;
                    ws.Cells[33, 3].Value = regHojaADTO.Numestacion;
                    ws.Cells[31, 5].Value = regHojaADTO.Periodohistorica == "S" ? "Si" : regHojaADTO.Periodohistorica == "N" ? "No" : "";
                    ws.Cells[31, 7].Value = regHojaADTO.Periodonaturalizada == "S" ? "Si" : regHojaADTO.Periodonaturalizada == "N" ? "No" : "";
                    ws.Cells[31, 9].Value = regHojaADTO.Demandaagua == "S" ? "Si" : regHojaADTO.Demandaagua == "N" ? "No" : "";
                    ws.Cells[39, 3].Value = regHojaADTO.Estudiogeologico == "S" ? "Si" : regHojaADTO.Estudiogeologico == "N" ? "No" : "";
                    ws.Cells[39, 5].Value = regHojaADTO.Perfodiamantinas;
                    ws.Cells[39, 7].Value = regHojaADTO.Numcalicatas;
                    ws.Cells[39, 9].Value = regHojaADTO.EstudioTopografico == "S" ? "Si" : regHojaADTO.EstudioTopografico == "N" ? "No" : "";
                    ws.Cells[39, 10].Value = regHojaADTO.Levantamientotopografico;
                    ws.Cells[46, 3].Value = regHojaADTO.Alturabruta;
                    ws.Cells[46, 5].Value = regHojaADTO.Alturaneta;
                    ws.Cells[46, 7].Value = regHojaADTO.Caudaldiseno;
                    ws.Cells[46, 9].Value = regHojaADTO.Potenciainstalada;
                    ws.Cells[50, 3].Value = regHojaADTO.Conduccionlongitud;
                    ws.Cells[50, 4].Value = regHojaADTO.Tunelarea;
                    ws.Cells[50, 5].Value = regHojaADTO.Tuneltipo;
                    ws.Cells[50, 6].Value = regHojaADTO.Tuberialongitud;
                    ws.Cells[50, 7].Value = regHojaADTO.Tuberiadiametro;
                    ws.Cells[50, 8].Value = regHojaADTO.Tuberiatipo;
                    ws.Cells[50, 9].Value = regHojaADTO.Maquinatipo;
                    ws.Cells[50, 10].Value = regHojaADTO.Maquinaaltitud;
                    ws.Cells[54, 3].Value = regHojaADTO.Regestacionalvbruto;
                    ws.Cells[54, 4].Value = regHojaADTO.Regestacionalvutil;
                    ws.Cells[54, 5].Value = regHojaADTO.Regestacionalhpresa;
                    ws.Cells[54, 6].Value = regHojaADTO.Reghorariavutil;
                    ws.Cells[54, 7].Value = regHojaADTO.Reghorariahpresa;
                    ws.Cells[54, 8].Value = regHojaADTO.Reghorariaubicacion;
                    ws.Cells[54, 9].Value = regHojaADTO.Energhorapunta;
                    ws.Cells[54, 10].Value = regHojaADTO.Energfuerapunta;
                    ws.Cells[60, 3].Value = regHojaADTO.Tipoturbina;
                    ws.Cells[60, 5].Value = regHojaADTO.Velnomrotacion;
                    ws.Cells[60, 7].Value = regHojaADTO.Potturbina;
                    ws.Cells[60, 9].Value = regHojaADTO.Numturbinas;
                    ws.Cells[63, 3].Value = regHojaADTO.Potgenerador;
                    ws.Cells[63, 5].Value = regHojaADTO.Numgeneradores;
                    ws.Cells[63, 7].Value = regHojaADTO.Tensiongeneracion;
                    ws.Cells[63, 9].Value = regHojaADTO.Rendimientogenerador;
                    ws.Cells[69, 3].Value = regHojaADTO.Tensionkv;
                    ws.Cells[69, 4].Value = regHojaADTO.Longitudkm;
                    ws.Cells[69, 6].Value = regHojaADTO.Numternas;
                    ws.Cells[69, 8].Value = regHojaADTO.Nombresubestacion;
                    ws.Cells[74, 5].Value = regHojaADTO.Perfil;
                    ws.Cells[75, 5].Value = regHojaADTO.Prefactibilidad;
                    ws.Cells[76, 5].Value = regHojaADTO.Factibilidad;
                    ws.Cells[77, 5].Value = regHojaADTO.Estudiodefinitivo;
                    ws.Cells[78, 5].Value = regHojaADTO.Eia;
                    ws.Cells[74, 9].Value = regHojaADTO.Fechainicioconstruccion;
                    ws.Cells[75, 9].Value = regHojaADTO.Periodoconstruccion;
                    ws.Cells[76, 9].Value = regHojaADTO.Fechaoperacioncomercial;
                    ws.Cells[83, 3].Value = regHojaADTO.Comentarios;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato FichaB
                RegHojaBDTO regHojaBDTO = proyectoModel.RegHojaBDTO;
                ws = xlPackage.Workbook.Worksheets["FichaB"];
                if (regHojaADTO != null)
                {
                    ws.Cells[11, 3].Value = regHojaBDTO.Estudiofactibilidad;
                    ws.Cells[11, 5].Value = regHojaBDTO.Investigacionescampo;
                    ws.Cells[11, 7].Value = regHojaBDTO.Gestionesfinancieras;
                    ws.Cells[11, 9].Value = regHojaBDTO.Disenospermisos;
                    ws.Cells[15, 3].Value = regHojaBDTO.Obrasciviles;
                    ws.Cells[15, 5].Value = regHojaBDTO.Equipamiento;
                    ws.Cells[15, 7].Value = regHojaBDTO.Lineatransmision; 
                    ws.Cells[15, 9].Value = regHojaBDTO.Obrasregulacion;
                    ws.Cells[19, 3].Value = regHojaBDTO.Administracion;
                    ws.Cells[19, 5].Value = regHojaBDTO.Aduanas;
                    ws.Cells[19, 7].Value = regHojaBDTO.Supervision;
                    ws.Cells[19, 9].Value = regHojaBDTO.Gastosgestion;
                    ws.Cells[23, 3].Value = regHojaBDTO.Imprevistos;
                    ws.Cells[23, 5].Value = regHojaBDTO.Igv;
                    ws.Cells[23, 7].Value = regHojaBDTO.Usoagua;
                    ws.Cells[23, 9].Value = regHojaBDTO.Otrosgastos;
                    ws.Cells[27, 3].Value = regHojaBDTO.Inversiontotalsinigv;
                    ws.Cells[27, 7].Value = regHojaBDTO.Inversiontotalconigv;
                    ws.Cells[31, 3].Value = regHojaBDTO.Financiamientotipo;
                    ws.Cells[31, 6].Value = regHojaBDTO.Financiamientoestado;
                    ws.Cells[31, 9].Value = regHojaBDTO.Porcentajefinanciado / 100;
                    ws.Cells[35, 3].Value = regHojaBDTO.Concesiondefinitiva;
                    ws.Cells[35, 5].Value = regHojaBDTO.Ventaenergia;
                    ws.Cells[35, 7].Value = regHojaBDTO.Ejecucionobra;
                    ws.Cells[35, 9].Value = regHojaBDTO.Contratosfinancieros;
                    ws.Cells[38, 3].Value = regHojaBDTO.Observaciones;

                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato FichaC
                RegHojaCDTO regHojaCDTO = proyectoModel.RegHojaCDTO;
                List<DetRegHojaCDTO> detRegHojaCDTOs = proyectoModel.RegHojaCDTO.DetRegHojaCs;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                ws = xlPackage.Workbook.Worksheets["FichaC"];
                if (regHojaCDTO != null)
                {
                    ws.Cells[27, 4].Value = regHojaCDTO.Fecpuestaope;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexAnio = 5;
                    var indexCat = 12;
                    var indexTrim = 4;
                   
                    ws.Cells[9, 4].Value = $"Año {anioPeriodo - 1} ó antes";
                    for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[9, indexAnio, 9, indexAnio + 3].Merge = true;
                        ws.Cells[9, indexAnio, 9, indexAnio + 3].Value = anio;
                        ws.Cells[9, indexAnio, 9, indexAnio + 3].Style.Font.Size = 10;
                        ws.Cells[10, indexAnio, 10, indexAnio + 3].Merge = true;
                        ws.Cells[10, indexAnio, 10, indexAnio + 3].Value = "TRIMESTRE";
                        ws.Cells[9, indexAnio, 9, indexAnio + 3].Style.Font.Size = 8;
                        for(int t=1; t<=4; t++){
                            ws.Cells[11, indexAnio + t - 1].Value = t;
                            ws.Cells[11, indexAnio + t - 1].Style.Font.Size = 8;
                            ws.Column(indexAnio + t - 1).Width = 3;
                        }  
                        indexAnio = indexAnio + 4;
                    }

                    if (dataCatalogoDTOs != null) {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                        {
                            ws.Cells[indexCat, 3].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexCat, 3].Style.WrapText = true;
                            indexCat++;
                        }
                    }
                    if (detRegHojaCDTOs != null) {
                        foreach (var detRegHoja in detRegHojaCDTOs)
                        {
                            var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.Datacatcodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + 12;
                                int matchTrim;
                                if (detRegHoja.Anio == "0")
                                {
                                    matchTrim = indexTrim;
                                }
                                else
                                {
                                    matchTrim = indexTrim + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                }
                                ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                            }
                        }
                    }
                    rg = ws.Cells[9, 3, 11, indexAnio-1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                    rg = ws.Cells[12, 3, indexCat-1, indexAnio-1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg = ws.Cells[12, 4, indexCat-1, indexAnio-1];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Font.Size = 10;
                }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato FichaD
                List<RegHojaDDTO> listRegHojaD = proyectoModel.ListRegHojaD;
                ws = xlPackage.Workbook.Worksheets["FichaD"];
                if (listRegHojaD != null)
                {
                    var anioInicio = 1965;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var indexX = 4;
                    var indexXRg = 4;
                    var indexY = 3;
                    var indexYRg = 3;

                    var meses = new string[]
                    {
                       "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                       "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
                    };

                    foreach (var regHoja in listRegHojaD) {
                        indexX += 4;
                        indexY = 3;
                        ws.Cells[indexX, indexY, indexX, indexY + 1].Merge = true;
                        ws.Cells[indexX, indexY, indexX, indexY + 1].Value = "Cuenca:";
                        ws.Cells[indexX, indexY + 2, indexX, indexY + 4].Merge = true;
                        ws.Cells[indexX, indexY + 2, indexX, indexY + 4].Value = regHoja.Cuenca;
                        ws.Cells[indexX + 1, indexY, indexX + 1, indexY + 1].Merge = true;
                        ws.Cells[indexX + 1, indexY, indexX + 1, indexY + 1].Value = "Cuenca:";
                        ws.Cells[indexX + 1, indexY + 2, indexX + 1, indexY + 4].Merge = true;
                        ws.Cells[indexX + 1, indexY + 2, indexX + 1, indexY + 4].Value = regHoja.Caudal;
                        rg = ws.Cells[indexX, indexY, indexX + 1, indexY + 4];
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                        indexX += 3;
                        indexXRg = indexX;
                        ws.Cells[indexX, indexY].Value = "Anual";
                        ws.Cells[indexX, indexY].Style.Font.Bold = true;
                        ws.Cells[indexX, indexY].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        foreach (var str in meses)
                        {
                            ws.Cells[indexX, indexY + 1].Value = str;
                            ws.Cells[indexX, indexY + 1].Style.Font.Bold = true;
                            ws.Cells[indexX, indexY + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexY++;
                        };

                        for (int anio = anioInicio; anio <= anioPeriodo; anio++)
                        {
                            ws.Cells[indexX + 1, indexYRg].Value = anio;
                            ws.Cells[indexX + 1, indexYRg].Style.Font.Bold = true;
                            ws.Cells[indexX + 1, indexYRg].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexX++;
                        }

                        ws.Cells[indexX + 1, indexYRg].Value = "Máximo";
                        ws.Cells[indexX + 1, indexYRg].Style.Font.Bold = true;
                        ws.Cells[indexX + 1, indexYRg].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[indexX + 2, indexYRg].Value = "Mínimo";
                        ws.Cells[indexX + 2, indexYRg].Style.Font.Bold = true;
                        ws.Cells[indexX + 2, indexYRg].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[indexX + 3, indexYRg].Value = "Media";
                        ws.Cells[indexX + 3, indexYRg].Style.Font.Bold = true;
                        ws.Cells[indexX + 3, indexYRg].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[indexXRg, indexY + 1].Value = "Media Anual";
                        ws.Cells[indexXRg, indexY + 1].Style.Font.Bold = true;
                        ws.Cells[indexXRg, indexY + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        if (regHoja.ListDetRegHojaD != null)
                        {
                            var valoresPorAnio = new Dictionary<int, List<decimal>>();
                            var valoresPorMes = new Dictionary<string, List<decimal>>();

                            foreach (var detRegHoja in regHoja.ListDetRegHojaD)
                            {
                                int indexAnio = -1;
                                int indexMes = -1;

                                for (int i = anioInicio; i <= anioPeriodo; i++)
                                {
                                    if (detRegHoja.Anio == i.ToString())
                                    {
                                        indexAnio = i - anioInicio + indexXRg + 1;
                                        break;
                                    }
                                }

                                for (int j = 0; j < meses.Length; j++)
                                {
                                    if (detRegHoja.Mes == meses[j])
                                    {
                                        indexMes = j + indexYRg + 1;
                                        break;
                                    }
                                }

                                if (indexAnio != -1 && indexMes != -1)
                                {
                                    ws.Cells[indexAnio, indexMes].Value = detRegHoja.Valor;
                                    ws.Cells[indexAnio, indexMes].Style.Numberformat.Format = "0.0000";

                                    int anio = int.Parse(detRegHoja.Anio);
                                    string keyMes = detRegHoja.Anio + "-" + detRegHoja.Mes;

                                    if (!valoresPorAnio.ContainsKey(anio))
                                    {
                                        valoresPorAnio[anio] = new List<decimal>();
                                    }
                                    valoresPorAnio[anio].Add((decimal)detRegHoja.Valor);

                                    if (!valoresPorMes.ContainsKey(keyMes))
                                    {
                                        valoresPorMes[keyMes] = new List<decimal>();
                                    }
                                    valoresPorMes[keyMes].Add((decimal)detRegHoja.Valor);
                                }

                            }
                            decimal maximo, minimo, media;
                            int indexMedia = indexYRg + 1; // Para la media de cada mes

                            foreach (var mes in meses)
                            {
                                maximo = 0;
                                minimo = 0;
                                media = 0;

                                var valores = valoresPorMes
                                    .Where(v => v.Key.EndsWith(mes))
                                    .Select(v => v.Value)
                                    .Where(v => v.Any())
                                    .ToList();

                                if (valores.Any())
                                {
                                    maximo = valores.Max(v => v.Max());
                                    minimo = valores.Min(v => v.Min());
                                    media = valores.Average(v => v.Average());
                                }

                                // Colocar los resultados en las celdas correspondientes (máximo, mínimo, media)
                                ws.Cells[indexX + 1, indexMedia].Value = maximo;
                                ws.Cells[indexX + 2, indexMedia].Value = minimo;
                                ws.Cells[indexX + 3, indexMedia].Value = media;

                                // Establecer formato numérico
                                ws.Cells[indexX + 1, indexMedia].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX + 2, indexMedia].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX + 3, indexMedia].Style.Numberformat.Format = "0.0000";

                                indexMedia++; // Ajustar el índice para el siguiente mes
                            }

                            // Ahora calculamos la media anual por cada año y la agregamos a la hoja
                            decimal mediaAnual;
                            int indexAnioM = indexXRg + 1; // Para colocar la media anual en la columna adecuada

                            for (int anio = anioInicio; anio <= anioPeriodo; anio++)
                            {
                                // Verificamos si hay valores para este año
                                if (valoresPorAnio.ContainsKey(anio))
                                {
                                    var valoresAnuales = valoresPorAnio[anio];

                                    // Si existen valores para este año, calculamos la media anual
                                    if (valoresAnuales.Any())
                                    {
                                        decimal sumaAnual = valoresAnuales.Sum();
                                        int totalValoresAnuales = valoresAnuales.Count();

                                        mediaAnual = totalValoresAnuales > 0 ? sumaAnual / totalValoresAnuales : 0;

                                        // Colocamos la media anual en la celda correspondiente
                                        ws.Cells[indexAnioM, indexY + 1].Value = mediaAnual;
                                        ws.Cells[indexAnioM, indexY + 1].Style.Numberformat.Format = "0.0000";
                                    }
                                }
                                indexAnioM++; // Avanzamos a la siguiente fila para el siguiente año
                            }
                            indexX += 2;
                        }
                        rg = ws.Cells[indexXRg, indexYRg, indexX + 1, indexY + 1];
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        // Aplicar fondo gris a la última columna utilizada
                        ws.Cells[indexXRg, indexY + 1, indexX + 1, indexY + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[indexXRg, indexY + 1, indexX + 1, indexY + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    }

                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Generacion Central Termo en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoGeneracionCentralTermo(ProyectoModel proyectoModel, string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                        ruta,
                        ConstantesCampanias.FolderTemp,
                        identificadorUnico,ConstantesCampanias.FolderFichasGeneracion,
                        ConstantesCampanias.FolderFichasGeneracionCTermo,
                        proyectoModel.TransmisionProyectoDTO.Proynombre+"-"+proyectoModel.TransmisionProyectoDTO.Proycodi
                    );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasGeneracionSubtipo, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralTermo));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralTermo));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }

            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralTermo));
            }
            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato CC.TT.-A
                RegHojaCCTTADTO regHojaCCTTADTO = proyectoModel.RegHojaCCTTADTO;
                ws = xlPackage.Workbook.Worksheets["CC.TT.-A"];
                if (regHojaCCTTADTO != null)
                {
                    ws.Cells[9, 5].Value = regHojaCCTTADTO.Centralnombre;
                    if (proyectoModel.ubicacionDTO != null)
                    {
                        ws.Cells[10, 5].Value = proyectoModel.ubicacionDTO.Departamento;
                        ws.Cells[11, 5].Value = proyectoModel.ubicacionDTO.Provincia;
                        ws.Cells[12, 5].Value = proyectoModel.ubicacionDTO.Distrito;
                    }
                    ws.Cells[13, 5].Value = regHojaCCTTADTO.Propietario;
                    ws.Cells[14, 5].Value = regHojaCCTTADTO.Sociooperador;
                    ws.Cells[15, 5].Value = regHojaCCTTADTO.Socioinversionista;
                    ws.Cells[21, 5].Value = regHojaCCTTADTO.Tipoconcesionactual;
                    ws.Cells[21, 9].Value = regHojaCCTTADTO.Fechaconcesionactual;
                    ws.Cells[26, 5].Value = regHojaCCTTADTO.Potenciainstalada;
                    ws.Cells[27, 5].Value = regHojaCCTTADTO.Potenciamaxima;
                    ws.Cells[28, 5].Value = regHojaCCTTADTO.Potenciaminima;
                    ws.Cells[26, 9].Value = regHojaCCTTADTO.Combustibletipo;
                    if (!string.IsNullOrEmpty(regHojaCCTTADTO.CombustibletipoOtro)) {
                        ws.Cells[26, 9].Value = regHojaCCTTADTO.CombustibletipoOtro;
                    } 
                    ws.Cells[27, 9].Value = regHojaCCTTADTO.Podercalorificoinferior;
                    ws.Cells[27, 10].Value = regHojaCCTTADTO.Undpci;
                    ws.Cells[28, 9].Value = regHojaCCTTADTO.Podercalorificosuperior;
                    ws.Cells[28, 10].Value = regHojaCCTTADTO.Undpcs;
                    ws.Cells[33, 7].Value = regHojaCCTTADTO.Costocombustible;
                    ws.Cells[33, 9].Value = regHojaCCTTADTO.Undcomb;
                    ws.Cells[34, 7].Value = regHojaCCTTADTO.Costotratamientocombustible;
                    ws.Cells[34, 9].Value = regHojaCCTTADTO.Undtrtcomb;
                    ws.Cells[35, 7].Value = regHojaCCTTADTO.Costotransportecombustible;
                    ws.Cells[35, 9].Value = regHojaCCTTADTO.Undtrnspcomb;
                    ws.Cells[36, 7].Value = regHojaCCTTADTO.Costovariablenocombustible;
                    ws.Cells[36, 9].Value = regHojaCCTTADTO.Undvarncmb;
                    ws.Cells[37, 7].Value = regHojaCCTTADTO.Costoinversioninicial;
                    ws.Cells[37, 9].Value = regHojaCCTTADTO.Undinvinic;
                    ws.Cells[38, 7].Value = regHojaCCTTADTO.Rendimientoplantacondicion;
                    ws.Cells[38, 9].Value = regHojaCCTTADTO.Undrendcnd;
                    ws.Cells[39, 7].Value = regHojaCCTTADTO.Consespificacondicion;
                    ws.Cells[39, 9].Value = regHojaCCTTADTO.Undconscp;
                    ws.Cells[47, 3].Value = regHojaCCTTADTO.Tipomotortermico;
                    ws.Cells[47, 5].Value = regHojaCCTTADTO.Velnomrotacion;
                    ws.Cells[47, 7].Value = regHojaCCTTADTO.Potmotortermico;
                    ws.Cells[47, 9].Value = regHojaCCTTADTO.Nummotorestermicos;
                    ws.Cells[50, 3].Value = regHojaCCTTADTO.Potgenerador;
                    ws.Cells[50, 5].Value = regHojaCCTTADTO.Numgeneradores;
                    ws.Cells[50, 7].Value = regHojaCCTTADTO.Tensiongeneracion;
                    ws.Cells[50, 9].Value = regHojaCCTTADTO.Rendimientogenerador;
                    ws.Cells[56, 3].Value = regHojaCCTTADTO.Tensionkv;
                    ws.Cells[56, 4].Value = regHojaCCTTADTO.Longitudkm;
                    ws.Cells[56, 6].Value = regHojaCCTTADTO.Numternas;
                    ws.Cells[56, 8].Value = regHojaCCTTADTO.Nombresubestacion;
                    ws.Cells[61, 5].Value = regHojaCCTTADTO.Perfil;
                    ws.Cells[62, 5].Value = regHojaCCTTADTO.Prefactibilidad;
                    ws.Cells[63, 5].Value = regHojaCCTTADTO.Factibilidad;
                    ws.Cells[64, 5].Value = regHojaCCTTADTO.Estudiodefinitivo;
                    ws.Cells[65, 5].Value = regHojaCCTTADTO.Eia;
                    ws.Cells[61, 9].Value = regHojaCCTTADTO.Fechainicioconstruccion;
                    ws.Cells[62, 9].Value = regHojaCCTTADTO.Periodoconstruccion;
                    ws.Cells[63, 9].Value = regHojaCCTTADTO.Fechaoperacioncomercial;
                    ws.Cells[70, 3].Value = regHojaCCTTADTO.Comentarios;                    
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.TT.-B
                RegHojaCCTTBDTO regHojaCCTTBDTO = proyectoModel.RegHojaCCTTBDTO;
                ws = xlPackage.Workbook.Worksheets["CC.TT.-B"];
                if (regHojaCCTTBDTO != null)
                {
                    ws.Cells[11, 3].Value = regHojaCCTTBDTO.Estudiofactibilidad;
                    ws.Cells[11, 5].Value = regHojaCCTTBDTO.Investigacionescampo;
                    ws.Cells[11, 7].Value = regHojaCCTTBDTO.Gestionesfinancieras;
                    ws.Cells[11, 9].Value = regHojaCCTTBDTO.Disenospermisos;
                    ws.Cells[15, 3].Value = regHojaCCTTBDTO.Obrasciviles;
                    ws.Cells[15, 5].Value = regHojaCCTTBDTO.Equipamiento;
                    ws.Cells[15, 7].Value = regHojaCCTTBDTO.Lineatransmision;
                    ws.Cells[15, 9].Value = regHojaCCTTBDTO.Obrasregulacion;
                    ws.Cells[19, 3].Value = regHojaCCTTBDTO.Administracion;
                    ws.Cells[19, 5].Value = regHojaCCTTBDTO.Aduanas;
                    ws.Cells[19, 7].Value = regHojaCCTTBDTO.Supervision;
                    ws.Cells[19, 9].Value = regHojaCCTTBDTO.Gastosgestion;
                    ws.Cells[23, 3].Value = regHojaCCTTBDTO.Imprevistos;
                    ws.Cells[23, 5].Value = regHojaCCTTBDTO.Igv;
                    ws.Cells[23, 7].Value = regHojaCCTTBDTO.Usoagua;
                    ws.Cells[23, 9].Value = regHojaCCTTBDTO.Otrosgastos;
                    ws.Cells[27, 3].Value = regHojaCCTTBDTO.Inversiontotalsinigv;
                    ws.Cells[27, 7].Value = regHojaCCTTBDTO.Inversiontotalconigv;
                    ws.Cells[31, 3].Value = regHojaCCTTBDTO.Financiamientotipo;
                    ws.Cells[31, 6].Value = regHojaCCTTBDTO.Financiamientoestado;
                    ws.Cells[31, 9].Value = regHojaCCTTBDTO.Porcentajefinanciado / 100;
                    ws.Cells[35, 3].Value = regHojaCCTTBDTO.Concesiondefinitiva;
                    ws.Cells[35, 5].Value = regHojaCCTTBDTO.Ventaenergia;
                    ws.Cells[35, 7].Value = regHojaCCTTBDTO.Ejecucionobra;
                    ws.Cells[35, 9].Value = regHojaCCTTBDTO.Contratosfinancieros;
                    ws.Cells[38, 3].Value = regHojaCCTTBDTO.Observaciones;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.TT.-C
                int indexC = 12;
                RegHojaCCTTCDTO regHojaCDTO = proyectoModel.RegHojaCCTTCDTO;
                List<Det1RegHojaCCTTCDTO> det1RegHojaCDTO = regHojaCDTO.Det1RegHojaCCTTCDTO;
                List<Det2RegHojaCCTTCDTO> det2RegHojaCDTO = regHojaCDTO.Det2RegHojaCCTTCDTO;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                ws = xlPackage.Workbook.Worksheets["CC.TT.-C"];
                if (regHojaCDTO != null)
                {
                    ws.Cells[27, 4].Value = regHojaCDTO.Turbfecpuestaope;
                    ws.Cells[49, 4].Value = regHojaCDTO.Cicfecpuestaope;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexIni = 9;
                    var indexIniY = 3;
                    var indexAnioA = 5;
                    var indexCatA = 12;
                    var indexTrimA = 4;

                    ws.Cells[indexIni, indexIniY + 1].Value = $"Año {anioPeriodo - 1} ó antes";
                    for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Value = anio;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 10;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Value = "TRIMESTRE";
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 8;
                        for (int t = 1; t <= 4; t++)
                        {
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Value = t;
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Style.Font.Size = 8;
                            ws.Column(indexAnioA + t - 1).Width = 3;
                        }
                        indexAnioA = indexAnioA + 4;
                    }

                    if (dataCatalogoDTOs != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                        {
                            ws.Cells[indexCatA, indexIniY].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexCatA, indexIniY].Style.WrapText = true;
                            indexCatA++;
                        }
                    }
                    if (det1RegHojaCDTO != null)
                    {
                        foreach (var detRegHoja in det1RegHojaCDTO)
                        {
                            var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.Datacatcodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + indexIni + 3;
                                int matchTrim;
                                if (detRegHoja.Anio == "0")
                                {
                                    matchTrim = indexTrimA;
                                }
                                else
                                {
                                    matchTrim = indexTrimA + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                }
                                ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                            }
                        }
                    }
                    rg = ws.Cells[indexIni, indexIniY, indexIni + 2, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                    rg = ws.Cells[indexIni + 3, indexIniY, indexCatA - 1, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg = ws.Cells[indexIni + 3, indexIniY + 1, indexCatA - 1, indexAnioA - 1];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Font.Size = 10;
                    var indexIniB = 31;
                    var indexIniYB = 3;
                    var indexAnioB = 5;
                    var indexCatB = 34;
                    var indexTrimB = 4;

                    ws.Cells[indexIniB, indexIniYB + 1].Value = $"Año {anioPeriodo - 1} ó antes";
                    for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexIniB, indexAnioB, indexIniB, indexAnioB + 3].Merge = true;
                        ws.Cells[indexIniB, indexAnioB, indexIniB, indexAnioB + 3].Value = anio;
                        ws.Cells[indexIniB, indexAnioB, indexIniB, indexAnioB + 3].Style.Font.Size = 10;
                        ws.Cells[indexIniB + 1, indexAnioB, indexIniB + 1, indexAnioB + 3].Merge = true;
                        ws.Cells[indexIniB + 1, indexAnioB, indexIniB + 1, indexAnioB + 3].Value = "TRIMESTRE";
                        ws.Cells[indexIniB, indexAnioB, indexIniB, indexAnioB + 3].Style.Font.Size = 8;
                        for (int t = 1; t <= 4; t++)
                        {
                            ws.Cells[indexIniB + 2, indexAnioB + t - 1].Value = t;
                            ws.Cells[indexIniB + 2, indexAnioB + t - 1].Style.Font.Size = 8;
                            ws.Column(indexAnioB + t - 1).Width = 3;
                        }
                        indexAnioB = indexAnioB + 4;
                    }

                    if (det2RegHojaCDTO != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                        {
                            ws.Cells[indexCatB, indexIniYB].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexCatB, indexIniYB].Style.WrapText = true;
                            indexCatB++;
                        }
                    }
                    if (det2RegHojaCDTO != null)
                    {
                        foreach (var detRegHoja in det2RegHojaCDTO)
                        {
                            var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.Datacatcodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + indexIniB + 3;
                                int matchTrim;
                                if (detRegHoja.Anio == "0")
                                {
                                    matchTrim = indexTrimB;
                                }
                                else
                                {
                                    matchTrim = indexTrimB + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                }
                                ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                            }
                        }
                    }
                    rg = ws.Cells[indexIniB, indexIniYB, indexIniB + 2, indexAnioB - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                    rg = ws.Cells[indexIniB + 3, indexIniYB, indexCatB - 1, indexAnioB - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg = ws.Cells[indexIniB + 3, indexIniYB + 1, indexCatB - 1, indexAnioB - 1];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Font.Size = 10;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Generacion Central Eolica en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoGeneracionCentralEolica(ProyectoModel proyectoModel, string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                         ruta,
                         ConstantesCampanias.FolderTemp,
                         identificadorUnico,ConstantesCampanias.FolderFichasGeneracion,
                         ConstantesCampanias.FolderFichasGeneracionCEolica,
                         proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                     );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasGeneracionSubtipo, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralEolica));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralEolica));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }
            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralEolica));
            }
            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato CC.Eol.-A
                RegHojaEolADTO regHojaEolADTO = proyectoModel.RegHojaEolADTO;
                ws = xlPackage.Workbook.Worksheets["CC.Eol.-A"];
                if (regHojaEolADTO != null)
                {
                    ws.Cells[9, 5].Value = regHojaEolADTO.CentralNombre;
                    if (proyectoModel.ubicacionDTO != null)
                    {
                        ws.Cells[10, 5].Value = proyectoModel.ubicacionDTO.Departamento;
                        ws.Cells[11, 5].Value = proyectoModel.ubicacionDTO.Provincia;
                        ws.Cells[12, 5].Value = proyectoModel.ubicacionDTO.Distrito;
                    }
                    ws.Cells[13, 5].Value = regHojaEolADTO.Propietario;
                    if (!string.IsNullOrEmpty(regHojaEolADTO.OtroPropietario))
                    {
                        ws.Cells[13, 5].Value = regHojaEolADTO.OtroPropietario;
                    }
                    ws.Cells[14, 5].Value = regHojaEolADTO.SocioOperador;
                    ws.Cells[15, 5].Value = regHojaEolADTO.SocioInversionista;
                    ws.Cells[21, 5].Value = regHojaEolADTO.ConcesionTemporal;
                    ws.Cells[21, 9].Value = regHojaEolADTO.FechaConcesionTemporal;
                    ws.Cells[22, 5].Value = regHojaEolADTO.TipoConcesionActual;
                    ws.Cells[22, 9].Value = regHojaEolADTO.FechaConcesionActual;
                    ws.Cells[28, 3].Value = regHojaEolADTO.NombreEstacionMet;
                    ws.Cells[28, 5].Value = regHojaEolADTO.NumEstacionMet;
                    ws.Cells[28, 7].Value = regHojaEolADTO.SerieVelViento;
                    ws.Cells[28, 9].Value = regHojaEolADTO.PeriodoDisAnio;
                    ws.Cells[32, 3].Value = regHojaEolADTO.EstudioGeologico;
                    ws.Cells[32, 5].Value = regHojaEolADTO.PerfoDiamantinas;
                    ws.Cells[34, 5].Value = regHojaEolADTO.NumCalicatas;
                    ws.Cells[32, 7].Value = regHojaEolADTO.EstudioTopografico;
                    ws.Cells[32, 9].Value = regHojaEolADTO.LevantamientoTopografico;
                    ws.Cells[39, 3].Value = regHojaEolADTO.PotenciaInstalada;
                    ws.Cells[39, 5].Value = regHojaEolADTO.VelVientoInstalada;
                    ws.Cells[39, 6].Value = regHojaEolADTO.HorPotNominal;
                    ws.Cells[39, 9].Value = regHojaEolADTO.VelDesconexion;
                    ws.Cells[39, 8].Value = regHojaEolADTO.VelConexion;
                    ws.Cells[39, 10].Value = regHojaEolADTO.TipoContrCentral;
                    ws.Cells[41, 8].Value = regHojaEolADTO.RangoVelTurbina;
                    ws.Cells[45, 3].Value = regHojaEolADTO.TipoTurbina;
                    ws.Cells[50, 3].Value = regHojaEolADTO.EnergiaAnual;
                    ws.Cells[49, 7].Value = regHojaEolADTO.TipoParqEolico;
                    ws.Cells[49, 10].Value = regHojaEolADTO.TipoTecGenerador;
                    ws.Cells[60, 3].Value = regHojaEolADTO.NumPalTurbina;
                    ws.Cells[60, 5].Value = regHojaEolADTO.DiaRotor;
                    ws.Cells[60, 7].Value = regHojaEolADTO.LongPala;
                    ws.Cells[60, 9].Value = regHojaEolADTO.AlturaTorre;
                    ws.Cells[63, 3].Value = regHojaEolADTO.PotNomGenerador;
                    ws.Cells[63, 5].Value = regHojaEolADTO.NumUnidades;
                    ws.Cells[63, 7].Value = regHojaEolADTO.NumPolos;
                    ws.Cells[63, 9].Value = regHojaEolADTO.TensionGeneracion;
                    ws.Cells[68, 3].Value = regHojaEolADTO.Bess;
                    ws.Cells[68, 4].Value = regHojaEolADTO.EnergiaMaxBat;
                    ws.Cells[68, 6].Value = regHojaEolADTO.PotenciaMaxBat;
                    ws.Cells[68, 7].Value = regHojaEolADTO.EfiCargaBat;
                    ws.Cells[68, 8].Value = regHojaEolADTO.EfiDescargaBat;
                    ws.Cells[68, 9].Value = regHojaEolADTO.TiempoMaxRegulacion;
                    ws.Cells[68, 10].Value = regHojaEolADTO.RampaCargDescarg;
                    ws.Cells[72, 3].Value = regHojaEolADTO.TensionKv;
                    ws.Cells[72, 4].Value = regHojaEolADTO.LongitudKm;
                    ws.Cells[72, 6].Value = regHojaEolADTO.NumTernas;
                    ws.Cells[72, 8].Value = regHojaEolADTO.NombreSubestacion; //ws.Cells[].Value = regHojaEolADTO.NombreSubOtro;
                    if (!string.IsNullOrEmpty(regHojaEolADTO.NombreSubOtro))
                    {
                        ws.Cells[72, 8].Value = regHojaEolADTO.NombreSubOtro;
                    }
                    ws.Cells[77, 5].Value = regHojaEolADTO.Perfil;
                    ws.Cells[78, 5].Value = regHojaEolADTO.Prefactibilidad;
                    ws.Cells[79, 5].Value = regHojaEolADTO.Factibilidad;
                    ws.Cells[80, 5].Value = regHojaEolADTO.EstudioDefinitivo;
                    ws.Cells[81, 5].Value = regHojaEolADTO.Eia;
                    ws.Cells[77, 9].Value = regHojaEolADTO.FechaInicioConstruccion;
                    ws.Cells[78, 9].Value = regHojaEolADTO.PeriodoConstruccion;
                    ws.Cells[79, 9].Value = regHojaEolADTO.FechaOperacionComercial;
                    ws.Cells[86, 3].Value = regHojaEolADTO.Comentarios;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                //Formato Ficha Curva_P_VS_V
                List<RegHojaEolADetDTO> regHojaEolADetDTOs = proyectoModel.RegHojaEolADTO.RegHojaEolADetDTOs;
                ws = xlPackage.Workbook.Worksheets["Curva_P_VS_V"];
                if (regHojaEolADetDTOs != null)
                {
                    var sortedList = regHojaEolADetDTOs.OrderBy(item => item.CentralADetCodi).ToList();
                    int indexEolDet = 5;
                    if (sortedList.Count > 0)
                    {
                        foreach (var detReg in sortedList)
                        {
                            ws.Cells[indexEolDet, 2].Value = detReg.Speed;
                            ws.Cells[indexEolDet, 3].Value = detReg.Acciona;
                            indexEolDet++;
                        }
                        rg = ws.Cells[4, 2, indexEolDet - 1, 3];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.Eol-B
                RegHojaEolBDTO regHojaEolBDTO = proyectoModel.RegHojaEolBDTO;
                ws = xlPackage.Workbook.Worksheets["CC.Eol-B"];
                if (regHojaEolBDTO != null)
                {
                    ws.Cells[10, 2].Value = regHojaEolBDTO.Estudiofactibilidad;
                    ws.Cells[10, 4].Value = regHojaEolBDTO.Investigacionescampo;
                    ws.Cells[10, 6].Value = regHojaEolBDTO.Gestionesfinancieras;
                    ws.Cells[10, 8].Value = regHojaEolBDTO.Disenospermisos;
                    ws.Cells[14, 2].Value = regHojaEolBDTO.Obrasciviles;
                    ws.Cells[14, 4].Value = regHojaEolBDTO.Equipamiento;
                    ws.Cells[14, 7].Value = regHojaEolBDTO.Lineatransmision;
                    ws.Cells[18, 2].Value = regHojaEolBDTO.Administracion;
                    ws.Cells[18, 4].Value = regHojaEolBDTO.Aduanas;
                    ws.Cells[18, 6].Value = regHojaEolBDTO.Supervision;
                    ws.Cells[18, 8].Value = regHojaEolBDTO.Gastosgestion;
                    ws.Cells[22, 2].Value = regHojaEolBDTO.Imprevistos;
                    ws.Cells[22, 4].Value = regHojaEolBDTO.Igv;
                    ws.Cells[22, 7].Value = regHojaEolBDTO.Otrosgastos;
                    ws.Cells[26, 2].Value = regHojaEolBDTO.Inversiontotalsinigv;
                    ws.Cells[26, 6].Value = regHojaEolBDTO.Inversiontotalconigv;
                    ws.Cells[30, 2].Value = regHojaEolBDTO.Financiamientotipo;
                    ws.Cells[30, 5].Value = regHojaEolBDTO.Financiamientoestado;
                    ws.Cells[30, 8].Value = regHojaEolBDTO.Porcentajefinanciado / 100;
                    ws.Cells[34, 2].Value = regHojaEolBDTO.Concesiondefinitiva;
                    ws.Cells[34, 4].Value = regHojaEolBDTO.Ventaenergia;
                    ws.Cells[34, 6].Value = regHojaEolBDTO.Ejecucionobra;
                    ws.Cells[34, 8].Value = regHojaEolBDTO.Contratosfinancieros;
                    ws.Cells[37, 2].Value = regHojaEolBDTO.Observaciones;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.Eol-C
                RegHojaEolCDTO regHojaEolCDTO = proyectoModel.RegHojaEolCDTO;
                List<DetRegHojaEolCDTO> detRegHojaEolCDTO = regHojaEolCDTO.DetRegHojaEolCDTO;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                ws = xlPackage.Workbook.Worksheets["CC.Eol-C"];
                if (regHojaEolCDTO != null)
                {
                    ws.Cells[26, 3].Value = regHojaEolCDTO.Fecpuestaope;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexIni = 8;
                    var indexIniY = 2;
                    var indexAnioA = indexIniY + 2;
                    var indexCatA = indexIni + 3;
                    var indexTrimA = indexIniY + 1;

                    ws.Cells[indexIni, indexIniY + 1].Value = $"Año {anioPeriodo - 1} ó antes";
                    for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Value = anio;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 10;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Value = "TRIMESTRE";
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 8;
                        for (int t = 1; t <= 4; t++)
                        {
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Value = t;
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Style.Font.Size = 8;
                            ws.Column(indexAnioA + t - 1).Width = 3;
                        }
                        indexAnioA = indexAnioA + 4;
                    }

                    if (detRegHojaEolCDTO != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                        {
                            ws.Cells[indexCatA, indexIniY].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexCatA, indexIniY].Style.WrapText = true;
                            indexCatA++;
                        }
                    }
                    if (detRegHojaEolCDTO != null)
                    {
                        foreach (var detRegHoja in detRegHojaEolCDTO)
                        {
                            var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.Datacatcodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + indexIni + 3;
                                int matchTrim;
                                if (detRegHoja.Anio == "0")
                                {
                                    matchTrim = indexTrimA;
                                }
                                else
                                {
                                    matchTrim = indexTrimA + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                }
                                ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                            }
                        }
                    }
                    rg = ws.Cells[indexIni, indexIniY, indexIni + 2, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                    rg = ws.Cells[indexIni + 3, indexIniY, indexCatA - 1, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg = ws.Cells[indexIni + 3, indexIniY + 1, indexCatA - 1, indexAnioA - 1];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Font.Size = 10;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            } 
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Generacion Central Solar en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoGeneracionCentralSolar(ProyectoModel proyectoModel, string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                         ruta,
                         ConstantesCampanias.FolderTemp,
                         identificadorUnico,ConstantesCampanias.FolderFichasGeneracion,
                         ConstantesCampanias.FolderFichasGeneracionCSolar,
                         proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                     );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasGeneracionSubtipo, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralSolar));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralSolar));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }
            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralSolar));
            }
            int row = 7;
            int column = 2;

           using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato CC.Sol-A
                SolHojaADTO SolHojaADTO = proyectoModel.SolHojaADTO;
                ws = xlPackage.Workbook.Worksheets["CC.Sol-A"];
                if (SolHojaADTO != null)
                {
                    ws.Cells[9,5].Value = SolHojaADTO.Centralnombre; 
                    if (proyectoModel.ubicacionDTO != null)
                    {
                        ws.Cells[10, 5].Value = proyectoModel.ubicacionDTO.Departamento;
                        ws.Cells[11, 5].Value = proyectoModel.ubicacionDTO.Provincia;
                        ws.Cells[12, 5].Value = proyectoModel.ubicacionDTO.Distrito;
                    }
                    ws.Cells[13,5].Value = SolHojaADTO.Propietario;//fatla colocar otro 
                    if (!string.IsNullOrEmpty(SolHojaADTO.Otro))
                    {
                        ws.Cells[13, 5].Value = SolHojaADTO.Otro;
                    }
                    ws.Cells[14,5].Value = SolHojaADTO.Sociooperador;
                    ws.Cells[15,5].Value = SolHojaADTO.Socioinversionista;
                    ws.Cells[21,5].Value = SolHojaADTO.Concesiontemporal;
                    ws.Cells[22,5].Value = SolHojaADTO.Tipoconcesionact;
                    ws.Cells[21,9].Value = SolHojaADTO.Fechaconcesiontem;
                    ws.Cells[22,9].Value = SolHojaADTO.Fechaconcesionact;
                    ws.Cells[28,3].Value = SolHojaADTO.Nomestacion;
                    ws.Cells[28,6].Value = SolHojaADTO.Serieradiacion;
                    ws.Cells[35,3].Value = SolHojaADTO.Potinstnom;
                    ws.Cells[35,5].Value = SolHojaADTO.Ntotalmodfv;
                    ws.Cells[35,6].Value = SolHojaADTO.Horutilequ;
                    ws.Cells[35,8].Value = SolHojaADTO.Eneestanual;
                    ws.Cells[35,10].Value = SolHojaADTO.Facplantaact;
                    ws.Cells[42,3].Value = SolHojaADTO.Tecnologia;
                    ws.Cells[42,5].Value = SolHojaADTO.Potenciapico;
                    ws.Cells[42,6].Value = SolHojaADTO.Nivelradsol;
                    ws.Cells[42,7].Value = SolHojaADTO.Seguidorsol;
                    ws.Cells[42,8].Value = SolHojaADTO.Volpunmax;
                    ws.Cells[42,9].Value = SolHojaADTO.Intpunmax;
                    ws.Cells[45,3].Value = SolHojaADTO.Modelo;
                    ws.Cells[45,4].Value = SolHojaADTO.Entpotmax;
                    ws.Cells[45,6].Value = SolHojaADTO.Salpotmax;
                    ws.Cells[45,8].Value = SolHojaADTO.Siscontro;
                    ws.Cells[51,3].Value = SolHojaADTO.Baterias;
                    ws.Cells[51,4].Value = SolHojaADTO.Enemaxbat;
                    ws.Cells[51,6].Value = SolHojaADTO.Potmaxbat;
                    ws.Cells[51,7].Value = SolHojaADTO.Eficargamax;
                    ws.Cells[51,8].Value = SolHojaADTO.Efidesbat;
                    ws.Cells[51,9].Value = SolHojaADTO.Timmaxreg;
                    ws.Cells[51,10].Value = SolHojaADTO.Rampascardes;
                    ws.Cells[56,3].Value = SolHojaADTO.Tension;
                    ws.Cells[56,4].Value = SolHojaADTO.Longitud;
                    ws.Cells[56,6].Value = SolHojaADTO.Numternas;
                    ws.Cells[56,8].Value = SolHojaADTO.Nombsubest;
                    ws.Cells[61,5].Value = SolHojaADTO.Perfil;
                    ws.Cells[62,5].Value = SolHojaADTO.Prefact;
                    ws.Cells[63,5].Value = SolHojaADTO.Factibilidad;
                    ws.Cells[64,5].Value = SolHojaADTO.Estdefinitivo;
                    ws.Cells[65,5].Value = SolHojaADTO.Eia;
                    ws.Cells[61,9].Value = SolHojaADTO.Fecinicioconst;
                    ws.Cells[62,9].Value = SolHojaADTO.Perconstruccion;
                    ws.Cells[63,9].Value = SolHojaADTO.Fecoperacioncom;
                    ws.Cells[70,3].Value = SolHojaADTO.Comentarios;
                    
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.Sol-B
                SolHojaBDTO SolHojaBDTO = proyectoModel.SolHojaBDTO;
                ws = xlPackage.Workbook.Worksheets["CC.Sol-B"];
                if (SolHojaBDTO != null)
                {
                    ws.Cells[11,3].Value = SolHojaBDTO.Estudiofactibilidad;
                    ws.Cells[11,5].Value = SolHojaBDTO.Investigacionescampo;
                    ws.Cells[11,7].Value = SolHojaBDTO.Gestionesfinancieras;
                    ws.Cells[11,9].Value = SolHojaBDTO.Disenospermisos;
                    ws.Cells[15,3].Value = SolHojaBDTO.Obrasciviles;
                    ws.Cells[15,5].Value = SolHojaBDTO.Equipamiento;
                    ws.Cells[15,8].Value = SolHojaBDTO.Lineatransmision;
                    ws.Cells[19,3].Value = SolHojaBDTO.Administracion;
                    ws.Cells[19,5].Value = SolHojaBDTO.Aduanas;
                    ws.Cells[19,7].Value = SolHojaBDTO.Supervision;
                    ws.Cells[19,9].Value = SolHojaBDTO.Gastosgestion;
                    ws.Cells[23,3].Value = SolHojaBDTO.Imprevistos;
                    ws.Cells[23,5].Value = SolHojaBDTO.Igv;
                    ws.Cells[23,8].Value = SolHojaBDTO.Otrosgastos;
                    ws.Cells[27,3].Value = SolHojaBDTO.Inversiontotalsinigv;
                    ws.Cells[27,7].Value = SolHojaBDTO.Inversiontotalconigv;
                    ws.Cells[31,3].Value = SolHojaBDTO.Financiamientotipo;
                    ws.Cells[31,6].Value = SolHojaBDTO.Financiamientoestado;
                    ws.Cells[31,9].Value = SolHojaBDTO.Porcentajefinanciado / 100;
                    ws.Cells[35,3].Value = SolHojaBDTO.Concesiondefinitiva;
                    ws.Cells[35,5].Value = SolHojaBDTO.Ventaenergia;
                    ws.Cells[35,7].Value = SolHojaBDTO.Ejecucionobra;
                    ws.Cells[35,9].Value = SolHojaBDTO.Contratosfinancieros;
                    ws.Cells[38,3].Value = SolHojaBDTO.Observaciones;
                    
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.Sol-C
                SolHojaCDTO solHojaCDTO = proyectoModel.SolHojaCDTO;
                List<DetSolHojaCDTO> listaDetSolHojaCDTO = solHojaCDTO.ListaDetSolHojaCDTO;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                ws = xlPackage.Workbook.Worksheets["CC.Sol-C"];
                if (solHojaCDTO != null)
                {
                    ws.Cells[27, 4].Value = solHojaCDTO.Fecpuestaope;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexIni = 9;
                    var indexIniY = 3;
                    var indexAnioA = indexIniY + 2;
                    var indexCatA = indexIni + 3;
                    var indexTrimA = indexIniY + 1;

                    ws.Cells[indexIni, indexIniY + 1].Value = $"Año {anioPeriodo - 1} ó antes";
                    for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Value = anio;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 10;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Value = "TRIMESTRE";
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 8;
                        for (int t = 1; t <= 4; t++)
                        {
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Value = t;
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Style.Font.Size = 8;
                            ws.Column(indexAnioA + t - 1).Width = 3;
                        }
                        indexAnioA = indexAnioA + 4;
                    }

                    if (listaDetSolHojaCDTO != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                        {
                            ws.Cells[indexCatA, indexIniY].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexCatA, indexIniY].Style.WrapText = true;
                            indexCatA++;
                        }
                    }
                    if (listaDetSolHojaCDTO != null)
                    {
                        foreach (var detRegHoja in listaDetSolHojaCDTO)
                        {
                            var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.Datacatcodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + indexIni + 3;
                                int matchTrim;
                                if (detRegHoja.Anio == "0")
                                {
                                    matchTrim = indexTrimA;
                                }
                                else
                                {
                                    matchTrim = indexTrimA + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                }
                                ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                            }
                        }
                    }
                    rg = ws.Cells[indexIni, indexIniY, indexIni + 2, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                    rg = ws.Cells[indexIni + 3, indexIniY, indexCatA - 1, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg = ws.Cells[indexIni + 3, indexIniY + 1, indexCatA - 1, indexAnioA - 1];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Font.Size = 10;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            }
        }
         /// <summary>
            /// Genera Ficha Proyecto Tipo Generacion Central Biom en formato excel
            /// </summary>
            /// <param name="model"></param>
            public static void GenerarExcelFichaProyectoTipoGeneracionCentralBiom(ProyectoModel proyectoModel, string identificadorUnico)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                     ruta,
                     ConstantesCampanias.FolderTemp,
                     identificadorUnico,ConstantesCampanias.FolderFichasGeneracion,
                     ConstantesCampanias.FolderFichasGeneracionCBiom,
                     proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                 );
                FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasGeneracionSubtipo, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralBiom));
                FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralBiom));
                if (!Directory.Exists(pathNewFile))
                {
                    Directory.CreateDirectory(pathNewFile);
                }
                // Verificar si la plantilla existe
                if (!template.Exists)
                {
                    throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
                }
                // Si el archivo ya existe, eliminarlo
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralBiom));
                }
                int row = 7;
                int column = 2;

               using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet ws = null;
                    ExcelRange rg = null;
                    // Formato CC.BIO.-A
                    BioHojaADTO BioHojaADTO = proyectoModel.BioHojaADTO;
                    ws = xlPackage.Workbook.Worksheets["CC.BIO.-A"];
                    if (BioHojaADTO != null)
                    {
                    
                        ws.Cells[9,5].Value = BioHojaADTO.CentralNombre;
                        if (proyectoModel.ubicacionDTO != null)
                        {
                            ws.Cells[10, 5].Value = proyectoModel.ubicacionDTO.Departamento;
                            ws.Cells[11, 5].Value = proyectoModel.ubicacionDTO.Provincia;
                            ws.Cells[12, 5].Value = proyectoModel.ubicacionDTO.Distrito;
                        }
                        ws.Cells[13,5].Value = BioHojaADTO.Propietario;
                        if (!string.IsNullOrEmpty(BioHojaADTO.Otro))
                        {
                            ws.Cells[13, 5].Value = BioHojaADTO.Otro;
                        }
                        ws.Cells[14,5].Value = BioHojaADTO.SocioOperador;
                        ws.Cells[15,5].Value = BioHojaADTO.SocioInversionista;
                        ws.Cells[21,5].Value = BioHojaADTO.ConTemporal;
                        ws.Cells[21,9].Value = BioHojaADTO.FecAdjudicacionTemp;
                        ws.Cells[22,5].Value = BioHojaADTO.TipoConActual;
                        ws.Cells[22,9].Value = BioHojaADTO.FecAdjudicacionAct;
                        ws.Cells[28,5].Value = BioHojaADTO.PotInstalada;
                        ws.Cells[28,9].Value = BioHojaADTO.TipoNomComb;
                        if (!string.IsNullOrEmpty(BioHojaADTO.OtroComb))
                        {
                            ws.Cells[28, 9].Value = BioHojaADTO.OtroComb;
                        }
                        ws.Cells[29,5].Value = BioHojaADTO.PotMaxima;
                        ws.Cells[29,9].Value = BioHojaADTO.PoderCalorInf;
                        ws.Cells[29,10].Value = BioHojaADTO.CombPoderCalorInf;
                        ws.Cells[30,5].Value = BioHojaADTO.PotMinima;
                        ws.Cells[30,9].Value = BioHojaADTO.PoderCalorSup;
                        ws.Cells[30,10].Value = BioHojaADTO.CombPoderCalorSup;
                        ws.Cells[37,7].Value = BioHojaADTO.CostCombustible;
                        ws.Cells[37,9].Value = BioHojaADTO.CombCostoCombustible;
                        ws.Cells[38,7].Value = BioHojaADTO.CostTratamiento;
                        ws.Cells[38,9].Value = BioHojaADTO.CombCostTratamiento;
                        ws.Cells[39,7].Value = BioHojaADTO.CostTransporte;
                        ws.Cells[39,9].Value = BioHojaADTO.CombCostTransporte;
                        ws.Cells[40,7].Value = BioHojaADTO.CostoVariableNoComb;
                        ws.Cells[70,9].Value = BioHojaADTO.CombCostoVariableNoComb;
                        ws.Cells[41,7].Value = BioHojaADTO.CostInversion;
                        ws.Cells[41,9].Value = BioHojaADTO.CombCostoInversion;
                        ws.Cells[42,7].Value = BioHojaADTO.RendPlanta;
                        ws.Cells[42,9].Value = BioHojaADTO.CombRendPlanta;
                        ws.Cells[43,7].Value = BioHojaADTO.ConsEspec;
                        ws.Cells[43,9].Value = BioHojaADTO.CombConsEspec;
                        ws.Cells[51,3].Value = BioHojaADTO.TipoMotorTer;
                        ws.Cells[51,5].Value = BioHojaADTO.VelNomRotacion;
                        ws.Cells[51,7].Value = BioHojaADTO.PotEjeMotorTer;
                        ws.Cells[51,9].Value = BioHojaADTO.NumMotoresTer;
                        ws.Cells[54,3].Value = BioHojaADTO.PotNomGenerador;
                        ws.Cells[54,5].Value = BioHojaADTO.NumGeneradores;
                        ws.Cells[54,7].Value = BioHojaADTO.TipoGenerador;
                        ws.Cells[54,9].Value = BioHojaADTO.TenGeneracion;
                        ws.Cells[60,3].Value = BioHojaADTO.Tension;
                        ws.Cells[60,4].Value = BioHojaADTO.Longitud;
                        ws.Cells[60,6].Value = BioHojaADTO.NumTernas;
                        ws.Cells[60,8].Value = BioHojaADTO.NomSubEstacion;
                        if (!string.IsNullOrEmpty(BioHojaADTO.OtroSubEstacion))
                        {
                            ws.Cells[60, 8].Value = BioHojaADTO.OtroSubEstacion;
                        }
                        ws.Cells[65,5].Value = BioHojaADTO.Perfil;
                        ws.Cells[66,5].Value = BioHojaADTO.Prefactibilidad;
                        ws.Cells[67,5].Value = BioHojaADTO.Factibilidad;
                        ws.Cells[68,5].Value = BioHojaADTO.EstDefinitivo;
                        ws.Cells[69,5].Value = BioHojaADTO.Eia;
                        ws.Cells[65,9].Value = BioHojaADTO.FecInicioConst;
                        ws.Cells[66,9].Value = BioHojaADTO.PeriodoConst;
                        ws.Cells[67,9].Value = BioHojaADTO.FecOperacionComer;
                        ws.Cells[74,3].Value = BioHojaADTO.Comentarios;
                                            
                    }
                    else
                    {
                        xlPackage.Workbook.Worksheets.Delete(ws);
                    }
                    // Formato CC.BIO.-B
                    BioHojaBDTO BioHojaBDTO = proyectoModel.BioHojaBDTO;
                    ws = xlPackage.Workbook.Worksheets["CC.BIO.-B"];
                    if (BioHojaBDTO != null)
                    {
                        ws.Cells[11,3].Value = BioHojaBDTO.Estudiofactibilidad;
                        ws.Cells[11,5].Value = BioHojaBDTO.Investigacionescampo;
                        ws.Cells[11,7].Value = BioHojaBDTO.Gestionesfinancieras;
                        ws.Cells[11,9].Value = BioHojaBDTO.Disenospermisos;
                        ws.Cells[15,3].Value = BioHojaBDTO.Obrasciviles;
                        ws.Cells[15,5].Value = BioHojaBDTO.Equipamiento;
                        ws.Cells[15,7].Value = BioHojaBDTO.Lineatransmision;
                        ws.Cells[15,9].Value = BioHojaBDTO.Obrasregulacion;
                        ws.Cells[19,3].Value = BioHojaBDTO.Administracion;
                        ws.Cells[19,5].Value = BioHojaBDTO.Aduanas;
                        ws.Cells[19,7].Value = BioHojaBDTO.Supervision;
                        ws.Cells[19,9].Value = BioHojaBDTO.Gastosgestion;
                        ws.Cells[23,3].Value = BioHojaBDTO.Imprevistos;
                        ws.Cells[23,5].Value = BioHojaBDTO.Igv;
                        //aqui ay un campo que esta en el excel, pero falta la variable 
                        ws.Cells[23,9].Value = BioHojaBDTO.Otrosgastos;
                        ws.Cells[27,3].Value = BioHojaBDTO.Inversiontotalsinigv;
                        ws.Cells[27,7].Value = BioHojaBDTO.Inversiontotalconigv;
                        ws.Cells[31,3].Value = BioHojaBDTO.Financiamientotipo;
                        ws.Cells[31,6].Value = BioHojaBDTO.Financiamientoestado;
                        ws.Cells[31,9].Value = BioHojaBDTO.Porcentajefinanciado / 100;
                        ws.Cells[35,3].Value = BioHojaBDTO.Concesiondefinitiva;
                        ws.Cells[35,5].Value = BioHojaBDTO.Ventaenergia;
                        ws.Cells[35,7].Value = BioHojaBDTO.Ejecucionobra;
                        ws.Cells[35,9].Value = BioHojaBDTO.Contratosfinancieros;
                        ws.Cells[38,3].Value = BioHojaBDTO.Observaciones;
                        
                    }
                    else
                    {
                        xlPackage.Workbook.Worksheets.Delete(ws);
                    }
                // Formato CC.BIO.-C
                    BioHojaCDTO bioHojaCDTO = proyectoModel.BioHojaCDTO;
                    List<DetBioHojaCDTO> listaDetBioHojaCDTO = bioHojaCDTO.ListaDetBioHojaCDTO;
                    List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                    ws = xlPackage.Workbook.Worksheets["CC.BIO.-C"];
                    if (bioHojaCDTO != null)
                    {
                        ws.Cells[27, 4].Value = bioHojaCDTO.Fecpuestaope;
                        var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                        var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                        var indexIni = 9;
                        var indexIniY = 3;
                        var indexAnioA = indexIniY + 2;
                        var indexCatA = indexIni + 3;
                        var indexTrimA = indexIniY + 1;

                        ws.Cells[indexIni, indexIniY + 1].Value = $"Año {anioPeriodo - 1} ó antes";
                        for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                        {
                            ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Merge = true;
                            ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Value = anio;
                            ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 10;
                            ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Merge = true;
                            ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Value = "TRIMESTRE";
                            ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 8;
                            for (int t = 1; t <= 4; t++)
                            {
                                ws.Cells[indexIni + 2, indexAnioA + t - 1].Value = t;
                                ws.Cells[indexIni + 2, indexAnioA + t - 1].Style.Font.Size = 8;
                                ws.Column(indexAnioA + t - 1).Width = 3;
                            }
                            indexAnioA = indexAnioA + 4;
                        }

                        if (dataCatalogoDTOs != null)
                        {
                            foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                            {
                                ws.Cells[indexCatA, indexIniY].Value = dataCatalogoDTO.DesDataCat;
                                ws.Cells[indexCatA, indexIniY].Style.WrapText = true;
                                indexCatA++;
                            }
                        }
                        if (listaDetBioHojaCDTO != null)
                        {
                            foreach (var detRegHoja in listaDetBioHojaCDTO)
                            {
                                var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.Datacatcodi);
                                if (matchedCatalogo != null)
                                {
                                    var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + indexIni + 3;
                                    int matchTrim;
                                    if (detRegHoja.Anio == "0")
                                    {
                                        matchTrim = indexTrimA;
                                    }
                                    else
                                    {
                                        matchTrim = indexTrimA + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                    }
                                    ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                                }
                            }
                        }
                        rg = ws.Cells[indexIni, indexIniY, indexIni + 2, indexAnioA - 1];
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                        rg = ws.Cells[indexIni + 3, indexIniY, indexCatA - 1, indexAnioA - 1];
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg = ws.Cells[indexIni + 3, indexIniY + 1, indexCatA - 1, indexAnioA - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Font.Bold = true;
                        rg.Style.Font.Size = 10;
                    }
                    else
                    {
                        xlPackage.Workbook.Worksheets.Delete(ws);
                    }
                    xlPackage.Save();
                }
            }
            /// <summary>
            /// Genera Ficha Proyecto Tipo Generacion Subestacion en formato excel
            /// </summary>
            /// <param name="model"></param>
            public static void GenerarExcelFichaProyectoTipoGeneracionSubestacion(ProyectoModel proyectoModel, string subtipo, string identificadorUnico)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                     ruta,
                     ConstantesCampanias.FolderTemp,
                     identificadorUnico,
                     ConstantesCampanias.FolderFichasGeneracion,subtipo,
                     proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi,ConstantesCampanias.FolderFichasGeneracionSubestaciones
                 );
                FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasGeneracion, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionSubestaciones));
                FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionSubestaciones));
                if (!Directory.Exists(pathNewFile))
                {
                    Directory.CreateDirectory(pathNewFile);
                }
                // Verificar si la plantilla existe
                if (!template.Exists)
                {
                    throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
                }
                // Si el archivo ya existe, eliminarlo
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionSubestaciones));
                }
                int row = 7;
                int column = 2;

                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet ws = null;
                    ExcelRange rg = null;
                // Formato hoja 1
                SubestFicha1DTO SubestFicha1DTO = proyectoModel.subestFicha1DTO;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                List<DataCatalogoDTO> dataCatalogoDTOs2 = proyectoModel.DataCatalogoDTOs2;
                List<DataCatalogoDTO> dataCatalogoDTOs3 = proyectoModel.DataCatalogoDTOs3;
                ws = xlPackage.Workbook.Worksheets["Hoja 1"];
                if (SubestFicha1DTO != null)
                {
                    var sortedData = dataCatalogoDTOs.OrderBy(a => a.DataCatCodi).ToList();
                    var sortedData2 = dataCatalogoDTOs2.OrderBy(a => a.DataCatCodi).ToList();
                    var sortedData3 = dataCatalogoDTOs3.OrderBy(a => a.DataCatCodi).ToList();

                    ws.Cells[9, 4].Value = SubestFicha1DTO.NombreSubestacion;
                    ws.Cells[12, 4].Value = SubestFicha1DTO.TipoProyecto;
                    ws.Cells[15, 4].Value = SubestFicha1DTO.FechaPuestaServicio;
                    ws.Cells[18, 4].Value = SubestFicha1DTO.EmpresaPropietaria;
                    ws.Cells[25, 4].Value = SubestFicha1DTO.SistemaBarras;
                    if (!string.IsNullOrEmpty(SubestFicha1DTO.OtroSistemaBarras))
                    {
                        ws.Cells[25, 4].Value = SubestFicha1DTO.OtroSistemaBarras;
                    }
                    var indexX = 31;
                    var indexRgX = 31;
                    var indexY = 4;
                    var indexRgY = 4;
                    if (SubestFicha1DTO.NumTrafo > 0) {
                        indexY += 4;
                        for (int i = 1; i <= SubestFicha1DTO.NumTrafo; i++)
                        {
                            ws.Cells[indexX, indexY].Value = "Trafo "+i;
                            ws.Cells[indexX, indexY].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexY++;
                        }
                    }
                    if (sortedData != null)
                    {
                        indexX++;
                        foreach (var dataCatalogoDTO in sortedData)
                        {
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Style.WrapText = true;
                            ws.Cells[indexX, indexRgY + 3].Value = dataCatalogoDTO.DescortaDatacat;
                            indexX++;
                        }
                    }
                    if (SubestFicha1DTO.Lista1DTOs != null)
                    {
                        foreach (var detRegHoja in SubestFicha1DTO.Lista1DTOs)
                        {
                            var matchedCatalogo = sortedData.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = sortedData.IndexOf(matchedCatalogo) + indexRgX + 1;
                                ws.Cells[matchedIndex, indexRgY + Convert.ToInt32(detRegHoja.NumTrafo) + 3].Value = detRegHoja.ValorTrafo;
                            }
                        }
                    }
                    if (SubestFicha1DTO.NumTrafo > 0)
                    {
                        rg = ws.Cells[indexRgX, indexRgY, indexRgX, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    }

                    if (SubestFicha1DTO.Lista1DTOs.Count > 0) {
                        rg = ws.Cells[indexRgX + 1, indexRgY, indexX - 1, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                    indexRgX = indexX;
                    indexY = indexRgY;

                    ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                    ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = "PRUEBAS";
                    if (SubestFicha1DTO.NumTrafo > 0)
                    {
                        indexY += 4;
                        for (int i = 1; i <= SubestFicha1DTO.NumTrafo; i++)
                        {
                            ws.Cells[indexX, indexY].Value = "Trafo " + i;
                            ws.Cells[indexX, indexY].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexY++;
                        }
                    }
                    if (sortedData2 != null)
                    {
                        indexX++;
                        foreach (var dataCatalogoDTO in sortedData2)
                        {
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Style.WrapText = true;
                            ws.Cells[indexX, indexRgY + 3].Value = dataCatalogoDTO.DescortaDatacat;
                            indexX++;
                        }
                    }
                    if (SubestFicha1DTO.Lista2DTOs != null)
                    {
                        foreach (var detRegHoja in SubestFicha1DTO.Lista2DTOs)
                        {
                            var matchedCatalogo = sortedData2.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = sortedData2.IndexOf(matchedCatalogo) + indexRgX + 1;
                                ws.Cells[matchedIndex, indexRgY + Convert.ToInt32(detRegHoja.NumTrafo) + 3].Value = detRegHoja.ValorTrafo;
                            }
                        }
                    }
                    if (SubestFicha1DTO.NumTrafo > 0)
                    {
                        rg = ws.Cells[indexRgX, indexRgY, indexRgX, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    }
                    if (SubestFicha1DTO.Lista2DTOs.Count > 0)
                    {
                        rg = ws.Cells[indexRgX + 1, indexRgY, indexX - 1, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }

                    ws.Cells[indexX + 1, indexRgY - 2].Value = "4.0";
                    ws.Cells[indexX + 1, indexRgY - 2].Style.Font.Bold = true;
                    ws.Cells[indexX + 1, indexRgY, indexX + 1, indexRgY + 3].Merge = true;
                    ws.Cells[indexX + 1, indexRgY, indexX + 1, indexRgY + 3].Value = "EQUIPOS DE COMPENSACIÓN REACTIVA.";
                    ws.Cells[indexX + 1, indexRgY, indexX + 1, indexRgY + 3].Style.Font.Bold = true;

                    indexX +=  3;
                    indexRgX = indexX;
                    indexY = indexRgY;

                    ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                    ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = "CARACTERISTICAS";
                    if (SubestFicha1DTO.NumEquipos > 0)
                    {
                        indexY += 4;
                        for (int i = 1; i <= SubestFicha1DTO.NumEquipos; i++)
                        {
                            ws.Cells[indexX, indexY].Value = "Equipo " + i;
                            ws.Cells[indexX, indexY].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexY++;
                        }
                    }
                    if (sortedData3 != null)
                    {
                        indexX++;
                        foreach (var dataCatalogoDTO in sortedData3)
                        {
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Style.WrapText = true;
                            ws.Cells[indexX, indexRgY + 3].Value = dataCatalogoDTO.DescortaDatacat;
                            indexX++;
                        }
                    }
                    if (SubestFicha1DTO.Lista3DTOs != null)
                    {
                        foreach (var detRegHoja in SubestFicha1DTO.Lista3DTOs)
                        {
                            var matchedCatalogo = sortedData3.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = sortedData3.IndexOf(matchedCatalogo) + indexRgX + 1;
                                ws.Cells[matchedIndex, indexRgY + Convert.ToInt32(detRegHoja.NumEquipo) + 3].Value = detRegHoja.ValorEquipo;
                            }
                        }
                    }
                    if (SubestFicha1DTO.NumEquipos > 0)
                    {
                        rg = ws.Cells[indexRgX, indexRgY, indexRgX, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    }
                    if (SubestFicha1DTO.Lista3DTOs.Count > 0)
                    {
                        rg = ws.Cells[indexRgX + 1, indexRgY, indexX - 1, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }

                    ws.Cells[indexX, indexRgY - 1 , indexX, indexRgY + SubestFicha1DTO.NumEquipos + 3].Merge = true;
                    ws.Cells[indexX, indexRgY - 1, indexX, indexRgY + SubestFicha1DTO.NumEquipos + 3].Value = "'(1)  Las tensiones de cortocircuito (P-S, P-T Y S-T) y las pérdidas en el cobre (P-S, P-T Y S-T) se deben ";

                }
                else
                    {
                        xlPackage.Workbook.Worksheets.Delete(ws);
                    }
                    xlPackage.Save();
                }
            }
            /// <summary>
            /// Genera Ficha Proyecto Tipo Generacion Lineas en formato excel
            /// </summary>
            /// <param name="model"></param>
            public static void GenerarExcelFichaProyectoTipoGeneracionLinea(ProyectoModel proyectoModel, string subtipo, string identificadorUnico)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                     ruta,
                     ConstantesCampanias.FolderTemp,
                     identificadorUnico,ConstantesCampanias.FolderFichasGeneracion,subtipo,
                     proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi,
                     ConstantesCampanias.FolderFichasGeneracionLineas
                 );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasGeneracion, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralLineas));
                FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralLineas));
                if (!Directory.Exists(pathNewFile))
                {
                    Directory.CreateDirectory(pathNewFile);
                }
                // Verificar si la plantilla existe
                if (!template.Exists)
                {
                    throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
                }
                // Si el archivo ya existe, eliminarlo
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionCentralLineas));
                }
                int row = 7;
                int column = 2;

               using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                   ExcelWorksheet ws = null;
                   ExcelRange rg = null;
                // Formato hoja 1
                LineasFichaADTO LinFichaADTO = proyectoModel.lineasFichaADTO;
                    ws = xlPackage.Workbook.Worksheets["FichaA"];
                    if (LinFichaADTO != null)
                    {
                        ws.Cells[9,4].Value = LinFichaADTO.NombreLinea;
                        ws.Cells[12,4].Value = LinFichaADTO.FecPuestaServ;
                        ws.Cells[16,4].Value = LinFichaADTO.SubInicio;
                        if (!string.IsNullOrEmpty(LinFichaADTO.OtroSubInicio))
                        {
                            ws.Cells[16, 4].Value = LinFichaADTO.OtroSubInicio;
                        }
                        ws.Cells[16,7].Value = LinFichaADTO.SubFin;
                        if (!string.IsNullOrEmpty(LinFichaADTO.OtroSubFin))
                        {
                            ws.Cells[16, 7].Value = LinFichaADTO.OtroSubFin;
                        }
                        ws.Cells[19,4].Value = LinFichaADTO.EmpPropietaria;
                        ws.Cells[25,10].Value = LinFichaADTO.NivTension;
                        ws.Cells[26,10].Value = LinFichaADTO.CapCorriente;
                        ws.Cells[27,10].Value = LinFichaADTO.CapCorrienteA;
                        ws.Cells[28,10].Value = LinFichaADTO.TpoSobreCarga;
                        ws.Cells[29,10].Value = LinFichaADTO.NumTemas;
                        ws.Cells[30,10].Value = LinFichaADTO.LongTotal;
                        //ws.Cells[31, 10].Value = "X" //Adjuntado configuracion geometrica
                        ws.Cells[32,10].Value = LinFichaADTO.LongVanoPromedio;
                        ws.Cells[33,10].Value = LinFichaADTO.TipMatSop;
                        //ws.Cells[34, 10].Value = "X" //Adjuntado ruta geográfica
                        //ws.Cells[35, 10].Value = "X" //Adjuntado perfil longitudinal

                        var indexX = 42;
                        var indexRgX = 42;
                        var indexY = 4;
                        // Tabla 1
                        if (LinFichaADTO.LineasFichaADet1DTO != null)
                        {
                            var i = 1;
                            foreach (var regHoja in LinFichaADTO.LineasFichaADet1DTO)
                            {
                                ws.Cells[indexX, indexY].Value = regHoja.Tramo;
                                ws.Cells[indexX, indexY + 1].Value = regHoja.Tipo;
                                ws.Cells[indexX, indexY + 2].Value = regHoja.Longitud;
                                ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Merge = true;
                                ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Value = regHoja.MatConductor;
                                ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Merge = true;
                                ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Value = regHoja.SecConductor;
                                ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Merge = true;
                                ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Value = regHoja.ConductorFase;
                                ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Merge = true;
                                ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Value = regHoja.CapacidadTot;
                                ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Merge = true;
                                ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Value = regHoja.CabGuarda;
                                ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Merge = true;
                                ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Value = regHoja.ResistCabGuarda;
                                indexX++;
                                i++;
                            };
                        }
                        if (LinFichaADTO.LineasFichaADet1DTO.Count > 0) 
                        {
                            rg = ws.Cells[indexRgX, indexY, indexX - 1, indexY + 14];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Font.Color.SetColor(Color.Black);
                        }

                        indexX ++;
                        indexRgX = indexX;

                        // Tabla 2
                        if (LinFichaADTO.LineasFichaADet2DTO != null)
                        {
                            ws.Cells[indexX, indexY].Value = "Tramo";
                            ws.Cells[indexX, indexY + 1].Value = "R";
                            ws.Cells[indexX, indexY + 2].Value = "X";
                            ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Merge = true;
                            ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Value = "B";
                            ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Merge = true;
                            ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Value = "G";
                            ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Merge = true;
                            ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Value = "R0";
                            ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Merge = true;
                            ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Value = "X0";
                            ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Merge = true;
                            ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Value = "B0";
                            ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Merge = true;
                            ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Value = "G0";
                            ws.Cells[indexX, indexY, indexX, indexY + 14].Style.Font.Bold = true;
                            ws.Cells[indexX, indexY, indexX, indexY + 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexX++;
                            ws.Cells[indexX, indexY].Value = "#";
                            ws.Cells[indexX, indexY + 1].Value = "ohm/km";
                            ws.Cells[indexX, indexY + 2].Value = "ohm/km";
                            ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Merge = true;
                            ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Value = "mS/km";
                            ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Merge = true;
                            ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Value = "mS/km";
                            ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Merge = true;
                            ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Value = "ohm/km";
                            ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Merge = true;
                            ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Value = "ohm/km";
                            ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Merge = true;
                            ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Value = "mS/km";
                            ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Merge = true;
                            ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Value = "mS/km";
                            ws.Cells[indexX, indexY, indexX, indexY + 14].Style.Font.Bold = true;
                            ws.Cells[indexX, indexY, indexX, indexY + 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexX++;
                            var i = 1;
                            foreach (var regHoja in LinFichaADTO.LineasFichaADet2DTO)
                            {
                                ws.Cells[indexX, indexY].Value = regHoja.Tramo;
                                ws.Cells[indexX, indexY + 1].Value = regHoja.R;
                                ws.Cells[indexX, indexY + 2].Value = regHoja.X;
                                ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Merge = true;
                                ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Value = regHoja.B;
                                ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Merge = true;
                                ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Value = regHoja.G;
                                ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Merge = true;
                                ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Value = regHoja.R0;
                                ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Merge = true;
                                ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Value = regHoja.X0;
                                ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Merge = true;
                                ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Value = regHoja.B0;
                                ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Merge = true;
                                ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Value = regHoja.G0;
                                indexX++;
                                i++;
                            };
                        }
                        rg = ws.Cells[indexRgX, indexY, indexRgX + 1, indexY + 14];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);

                        if (LinFichaADTO.LineasFichaADet2DTO.Count > 0)
                        {
                            rg = ws.Cells[indexRgX + 2, indexY, indexX - 1, indexY + 14];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Font.Color.SetColor(Color.Black);
                        }

                        ws.Cells[indexX + 1, indexY - 2].Value = "3.0";
                        ws.Cells[indexX + 1, indexY - 2].Style.Font.Bold = true;
                        ws.Cells[indexX + 1, indexY].Value = "SISTEMA DE PROTECCION:";
                        ws.Cells[indexX + 1, indexY].Style.Font.Bold = true;


                        ws.Cells[indexX + 3, indexY - 1].Value = "3.1";
                        ws.Cells[indexX + 3, indexY].Value = "Descripción del sistema de protección principal:";

                        rg = ws.Cells[indexX + 4, indexY, indexX + 4, indexY + 17];
                        rg.Merge = true;
                        rg.Value = LinFichaADTO.DesProtecPrincipal;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);

                        ws.Cells[indexX + 6, indexY - 1].Value = "3.2";
                        ws.Cells[indexX + 6, indexY].Value = "Descripción del sistema de protección de respaldo:";

                        rg = ws.Cells[indexX + 7, indexY, indexX + 7, indexY + 17];
                        rg.Merge = true;
                        rg.Value = LinFichaADTO.DesProtecRespaldo;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);

                        ws.Cells[indexX + 9, indexY - 2].Value = "4.0";
                        ws.Cells[indexX + 9, indexY - 2].Style.Font.Bold = true;
                        ws.Cells[indexX + 9, indexY].Value = "DESCRIPCIÓN GENERAL DEL PROYECTO";
                        ws.Cells[indexX + 9, indexY].Style.Font.Bold = true;


                        rg = ws.Cells[indexX + 11, indexY, indexX + 11, indexY + 17];
                        rg.Merge = true;
                        rg.Value = LinFichaADTO.DesGenProyecto;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);

                    }
                    else
                    {
                        xlPackage.Workbook.Worksheets.Delete(ws);
                    }
                    xlPackage.Save();
                }
            }

        /// <summary>
        /// Genera Ficha Proyecto Tipo ITC Sistema Electrico en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoITCSistemaElectrico(ProyectoModel proyectoModel, string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                         ruta,
                         ConstantesCampanias.FolderTemp,
                         identificadorUnico,
                         ConstantesCampanias.FolderFichasITC,
                         proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                     );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasITC, FormatoArchivosExcelCampanias.NombreFichaExcelITCSistemaElectricoParametros));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelITCSistemaElectricoParametros));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }
            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelITCSistemaElectricoParametros));
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;

                //Formato Ficha PRM-1
                List<ItcPrm1Dto> listaItcPrm1DTO = proyectoModel.ListaItcPrm1DTO;
                ws = xlPackage.Workbook.Worksheets["PRM-1"];
                if (listaItcPrm1DTO != null)
                {
                    int indexPrm1 = 2;
                    if (listaItcPrm1DTO.Count > 0)
                    {
                        foreach (var detReg in listaItcPrm1DTO)
                        {
                            ItcPrm1Dto ObjItcPrm1 = detReg;
                            ws.Cells[indexPrm1, 1].Value = ObjItcPrm1.Electroducto;
                            ws.Cells[indexPrm1, 2].Value = ObjItcPrm1.Descripcion;
                            ws.Cells[indexPrm1, 3].Value = ObjItcPrm1.Vn;
                            ws.Cells[indexPrm1, 4].Value = ObjItcPrm1.Tipo;
                            ws.Cells[indexPrm1, 5].Value = ObjItcPrm1.Seccion;
                            ws.Cells[indexPrm1, 6].Value = ObjItcPrm1.Ctr;
                            ws.Cells[indexPrm1, 7].Value = ObjItcPrm1.R;
                            ws.Cells[indexPrm1, 8].Value = ObjItcPrm1.X;
                            ws.Cells[indexPrm1, 9].Value = ObjItcPrm1.B;
                            ws.Cells[indexPrm1, 10].Value = ObjItcPrm1.Ro;
                            ws.Cells[indexPrm1, 11].Value = ObjItcPrm1.Xo;
                            ws.Cells[indexPrm1, 12].Value = ObjItcPrm1.Bo;
                            ws.Cells[indexPrm1, 13].Value = ObjItcPrm1.Capacidad;
                            ws.Cells[indexPrm1, 14].Value = ObjItcPrm1.Tmxop;
                            indexPrm1++;
                        }

                        
                        // Aplicar formato numérico a todas las celdas de la columna con valores decimales
                        ws.Cells[2, 3, indexPrm1 - 1, 14].Style.Numberformat.Format = "0.0000";


                        rg = ws.Cells[2, 1, indexPrm1 - 1, 14];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }

                //Formato Ficha PRM-2
                List<ItcPrm2Dto> listaItcPrm2DTO = proyectoModel.ListaItcPrm2DTO;
                ws = xlPackage.Workbook.Worksheets["PRM-2"];
                if (listaItcPrm2DTO != null)
                {
                    int indexPrm2 = 2;
                    if (listaItcPrm2DTO.Count > 0) {
                        foreach (var detReg in listaItcPrm2DTO)
                        {
                            ItcPrm2Dto ObjItcPrm2 = detReg;
                            ws.Cells[indexPrm2, 1].Value = ObjItcPrm2.Transformador;
                            ws.Cells[indexPrm2, 2].Value = ObjItcPrm2.Tipo;
                            ws.Cells[indexPrm2, 3].Value = ObjItcPrm2.Fases;
                            ws.Cells[indexPrm2, 4].Value = ObjItcPrm2.Ndvn;
                            ws.Cells[indexPrm2, 5].Value = ObjItcPrm2.Vnp;
                            ws.Cells[indexPrm2, 6].Value = ObjItcPrm2.Vns;
                            ws.Cells[indexPrm2, 7].Value = ObjItcPrm2.Vnt;
                            ws.Cells[indexPrm2, 8].Value = ObjItcPrm2.Pnp;
                            ws.Cells[indexPrm2, 9].Value = ObjItcPrm2.Pns;
                            ws.Cells[indexPrm2, 10].Value = ObjItcPrm2.Pnt;
                            ws.Cells[indexPrm2, 11].Value = ObjItcPrm2.Tccps;
                            ws.Cells[indexPrm2, 12].Value = ObjItcPrm2.Tccst;
                            ws.Cells[indexPrm2, 13].Value = ObjItcPrm2.Tcctp;
                            ws.Cells[indexPrm2, 14].Value = ObjItcPrm2.Pcups;
                            ws.Cells[indexPrm2, 15].Value = ObjItcPrm2.Pcust;
                            ws.Cells[indexPrm2, 16].Value = ObjItcPrm2.Pcutp;
                            ws.Cells[indexPrm2, 17].Value = ObjItcPrm2.Pfe;
                            ws.Cells[indexPrm2, 18].Value = ObjItcPrm2.Ivacio;
                            ws.Cells[indexPrm2, 19].Value = ObjItcPrm2.Grpcnx;
                            ws.Cells[indexPrm2, 20].Value = ObjItcPrm2.Taptipo;
                            ws.Cells[indexPrm2, 21].Value = ObjItcPrm2.Taplado;
                            ws.Cells[indexPrm2, 22].Value = ObjItcPrm2.Tapdv;
                            ws.Cells[indexPrm2, 23].Value = ObjItcPrm2.Tapmin;
                            ws.Cells[indexPrm2, 24].Value = ObjItcPrm2.Tapcnt;
                            ws.Cells[indexPrm2, 25].Value = ObjItcPrm2.Tapmax;
                            indexPrm2++;
                        }
                        // Aplicar formato numérico a columnas específicas con 4 decimales
                        ws.Cells[2, 5, indexPrm2 - 1, 5].Style.Numberformat.Format = "0.0000";  // Vnp
                        ws.Cells[2, 6, indexPrm2 - 1, 6].Style.Numberformat.Format = "0.0000";  // Vns
                        ws.Cells[2, 7, indexPrm2 - 1, 7].Style.Numberformat.Format = "0.0000";  // Vnt
                        ws.Cells[2, 11, indexPrm2 - 1, 11].Style.Numberformat.Format = "0.0000";  // Tccps
                        ws.Cells[2, 12, indexPrm2 - 1, 12].Style.Numberformat.Format = "0.0000";  // Tccst
                        ws.Cells[2, 13, indexPrm2 - 1, 13].Style.Numberformat.Format = "0.0000";  // Tcctp
                        ws.Cells[2, 14, indexPrm2 - 1, 14].Style.Numberformat.Format = "0.0000";  // Pcups
                        ws.Cells[2, 15, indexPrm2 - 1, 15].Style.Numberformat.Format = "0.0000";  // Pcust
                        ws.Cells[2, 16, indexPrm2 - 1, 16].Style.Numberformat.Format = "0.0000";  // Pcutp
                        ws.Cells[2, 17, indexPrm2 - 1, 17].Style.Numberformat.Format = "0.0000";  // Pfe
                        ws.Cells[2, 18, indexPrm2 - 1, 18].Style.Numberformat.Format = "0.0000";  // Ivacio
                        ws.Cells[2, 21, indexPrm2 - 1, 21].Style.Numberformat.Format = "0.0000";  // Taplado

                        rg = ws.Cells[2, 1, indexPrm2 - 1, 25];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }

                //Formato Ficha RED-1
                List<ItcRed1Dto> listaItcRed1DTO = proyectoModel.ListaItcRed1DTO;
                ws = xlPackage.Workbook.Worksheets["RED-1"];
                if (listaItcRed1DTO != null)
                {
                    int indexRed1 = 2;
                    if (listaItcRed1DTO.Count > 0)
                    {
                        foreach (var detReg in listaItcRed1DTO)
                        {
                            ItcRed1Dto ObjItcRed1 = detReg;
                            ws.Cells[indexRed1, 1].Value = ObjItcRed1.Barra;
                            ws.Cells[indexRed1, 2].Value = ObjItcRed1.Vnpu;
                            ws.Cells[indexRed1, 3].Value = ObjItcRed1.Vopu;
                            ws.Cells[indexRed1, 4].Value = ObjItcRed1.Tipo;
                            indexRed1++;
                        }
                        rg = ws.Cells[2, 1, indexRed1 - 1, 4];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }

                //Formato Ficha RED-2
                List<ItcRed2Dto> listaItcRed2DTO = proyectoModel.ListaItcRed2DTO;
                ws = xlPackage.Workbook.Worksheets["RED-2"];
                if (listaItcRed2DTO != null)
                {
                    int indexRed2 = 2;
                    if (listaItcRed2DTO.Count > 0)
                    {
                        foreach (var detReg in listaItcRed2DTO)
                        {
                            ItcRed2Dto ObjItcRed2 = detReg;
                            ws.Cells[indexRed2, 1].Value = ObjItcRed2.Linea;
                            ws.Cells[indexRed2, 2].Value = ObjItcRed2.BarraE;
                            ws.Cells[indexRed2, 3].Value = ObjItcRed2.BarraR;
                            ws.Cells[indexRed2, 4].Value = ObjItcRed2.Nternas;
                            ws.Cells[indexRed2, 5].Value = ObjItcRed2.Tramo;
                            ws.Cells[indexRed2, 6].Value = ObjItcRed2.Electroducto;
                            ws.Cells[indexRed2, 7].Value = ObjItcRed2.Longitud;
                            indexRed2++;
                        }
                        rg = ws.Cells[2, 1, indexRed2 - 1, 7];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }   
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }

                //Formato Ficha RED-3
                List<ItcRed3Dto> listaItcRed3DTO = proyectoModel.ListaItcRed3DTO;
                ws = xlPackage.Workbook.Worksheets["RED-3"];
                if (listaItcRed3DTO != null)
                {
                    int indexRed3 = 2;
                    if (listaItcRed3DTO.Count > 0)
                    {
                        foreach (var detReg in listaItcRed3DTO)
                        {
                            ItcRed3Dto ObjItcRed3 = detReg;
                            ws.Cells[indexRed3, 1].Value = ObjItcRed3.IdCircuito;
                            ws.Cells[indexRed3, 2].Value = ObjItcRed3.BarraP;
                            ws.Cells[indexRed3, 3].Value = ObjItcRed3.BarraS;
                            ws.Cells[indexRed3, 4].Value = ObjItcRed3.BarraT;
                            ws.Cells[indexRed3, 5].Value = ObjItcRed3.CdgTrafo;
                            ws.Cells[indexRed3, 6].Value = ObjItcRed3.OprTap;
                            ws.Cells[indexRed3, 7].Value = ObjItcRed3.PosTap;
                            indexRed3++;
                        }
                        rg = ws.Cells[2, 1, indexRed3 - 1, 7];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }

                //Formato Ficha RED-4
                List<ItcRed4Dto> listaItcRed4DTO = proyectoModel.ListaItcRed4DTO;
                ws = xlPackage.Workbook.Worksheets["RED-4"];
                if (listaItcRed4DTO != null)
                {
                    int indexRed4 = 2;
                    if (listaItcRed4DTO.Count > 0)
                    {
                        foreach (var detReg in listaItcRed4DTO)
                        {
                            ItcRed4Dto ObjItcRed4 = detReg;
                            ws.Cells[indexRed4, 1].Value = ObjItcRed4.IdCmp;
                            ws.Cells[indexRed4, 2].Value = ObjItcRed4.Barra;
                            ws.Cells[indexRed4, 3].Value = ObjItcRed4.Tipo;
                            ws.Cells[indexRed4, 4].Value = ObjItcRed4.Vnkv;
                            ws.Cells[indexRed4, 5].Value = ObjItcRed4.CapmVar;
                            ws.Cells[indexRed4, 6].Value = ObjItcRed4.Npasos;
                            ws.Cells[indexRed4, 7].Value = ObjItcRed4.PasoAct;
                            indexRed4++;
                        }
                        rg = ws.Cells[2, 1, indexRed4 - 1, 7];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }

                //Formato Ficha RED-5
                List<ItcRed5Dto> listaItcRed5DTO = proyectoModel.ListaItcRed5DTO;
                ws = xlPackage.Workbook.Worksheets["RED-5"];
                if (listaItcRed5DTO != null)
                {
                    int indexRed5 = 2;
                    if (listaItcRed5DTO.Count > 0)
                    {
                        foreach (var detReg in listaItcRed5DTO)
                        {
                            ItcRed5Dto ObjItcRed5 = detReg;
                            ws.Cells[indexRed5, 1].Value = ObjItcRed5.CaiGen;
                            ws.Cells[indexRed5, 2].Value = ObjItcRed5.IdGen;
                            ws.Cells[indexRed5, 3].Value = ObjItcRed5.Barra;
                            ws.Cells[indexRed5, 4].Value = ObjItcRed5.PdMw;
                            ws.Cells[indexRed5, 5].Value = ObjItcRed5.PnMw;
                            ws.Cells[indexRed5, 6].Value = ObjItcRed5.QnMin;
                            ws.Cells[indexRed5, 7].Value = ObjItcRed5.QnMa;
                            indexRed5++;
                        }
                        rg = ws.Cells[2, 1, indexRed5 - 1, 7];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }

                xlPackage.Save();
            }
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo ITC Demanda en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoITCDemanda(ProyectoModel proyectoModel, string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                         ruta,
                         ConstantesCampanias.FolderTemp,
                         identificadorUnico,
                         ConstantesCampanias.FolderFichasITC,
                         proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                     );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasITC, FormatoArchivosExcelCampanias.NombreFichaExcelITCDemanda));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelITCDemanda));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }
            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelITCDemanda));
            }
            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;

                // Formato Ficha F-104
                int indexF104 = 8;
                int indexF104Rg = 8;
                List<Itcdf104DTO> itcdf104DTOS = proyectoModel.Itcdf104DTOs;
                ws = xlPackage.Workbook.Worksheets["F-104"];
                if (itcdf104DTOS != null)
                {
                    ws.Cells[4, 3].Value = proyectoModel.TransmisionProyectoDTO.Areademanda;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var anioIni = 1994;
                    for (int anio = anioIni; anio <= anioPeriodo; anio++)
                    {
                        ws.Cells[indexF104, 1].Value = anio;
                        ws.Cells[indexF104, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        indexF104++;
                    }
                    if (itcdf104DTOS.Count > 0)
                    {
                        foreach (var detReg in itcdf104DTOS)
                        {
                            Itcdf104DTO ObjItcdf104 = detReg;
                            int filaAnio = -1;
                            for (int r = indexF104Rg; r <= indexF104; r++)
                            {
                                var valorAnio = ws.Cells[r, 1].Value;
                                if (valorAnio != null && valorAnio.ToString() == ObjItcdf104.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1)
                            {
                                var indexX = filaAnio;
                                ws.Cells[indexX, 2].Value = ObjItcdf104.MillonesolesPbi;
                                ws.Cells[indexX, 3].Value = ObjItcdf104.TasaCrecimientoPbi;
                                ws.Cells[indexX, 4].Value = ObjItcdf104.NroClientesLibres;
                                ws.Cells[indexX, 5].Value = ObjItcdf104.NroClientesRegulados;
                                ws.Cells[indexX, 6].Value = ObjItcdf104.NroHabitantes;
                                ws.Cells[indexX, 7].Value = ObjItcdf104.TasaCrecimientoPoblacion;
                                ws.Cells[indexX, 8].Value = ObjItcdf104.MillonesClientesRegulados;
                                ws.Cells[indexX, 9].Value = ObjItcdf104.ClientesReguladoSelectr;
                                ws.Cells[indexX, 10].Value = ObjItcdf104.Usmwh;
                                ws.Cells[indexX, 11].Value = ObjItcdf104.TasaCrecimientoEnergia;
                            }

                        }
                        rg = ws.Cells[9, 1, indexF104 - 1, 11];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg = ws.Cells[8, 1, indexF104 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg = ws.Cells[indexF104 - 1, 1, indexF104 - 1, 11];
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    } 
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }



                                // Formato Ficha F-108
                int indexF108 = 8;
                List<Itcdf108DTO> itcdf108DTOS = proyectoModel.Itcdf108DTOs;
                ws = xlPackage.Workbook.Worksheets["F-108"];
                if (itcdf108DTOS != null)
                {
                    ws.Cells[4, 3].Value = proyectoModel.TransmisionProyectoDTO.Areademanda;
                    int numeracion = 0;

                    // Inicializar listas para almacenar los valores de las columnas
                    List<double> atvalList = new List<double>();
                    List<double> mtvalList = new List<double>();
                    List<double> btvalList = new List<double>();
                    List<double> totalList = new List<double>(); // Lista para la suma de Atval, Mtval y Btval
                    List<int> anioList = new List<int>();

                    if (itcdf108DTOS.Count > 0)
                    {
                        foreach (var detReg in itcdf108DTOS)
                        {
                            Itcdf108DTO ObjItcdf108 = detReg;
                            double total = (double)(ObjItcdf108.Atval ?? 0) + (double)(ObjItcdf108.Mtval ?? 0) + (double)(ObjItcdf108.Btval ?? 0);

                            ws.Cells[indexF108, 1].Value = numeracion == 0 ? "Hist." : numeracion.ToString();
                            ws.Cells[indexF108, 2].Value = ObjItcdf108.Anio;
                            ws.Cells[indexF108, 3].Value = ObjItcdf108.Atval;
                            ws.Cells[indexF108, 4].Value = ObjItcdf108.Mtval;
                            ws.Cells[indexF108, 5].Value = ObjItcdf108.Btval;
                            ws.Cells[indexF108, 6].Value = total;

                            // Agregar los valores de cada columna a sus listas correspondientes
                            atvalList.Add((double)(ObjItcdf108.Atval ?? 0));
                            mtvalList.Add((double)(ObjItcdf108.Mtval ?? 0));
                            btvalList.Add((double)(ObjItcdf108.Btval ?? 0));
                            totalList.Add(total);
                            anioList.Add((int)ObjItcdf108.Anio);
                            indexF108++;
                            numeracion++;
                        }

                        // Cálculo de la tasa promedio para cada columna
                        double tasaCrecimientoPromedioAtval = CalcularTasaCrecimientoPromedio(atvalList, anioList);
                        double tasaCrecimientoPromedioMtval = CalcularTasaCrecimientoPromedio(mtvalList, anioList);
                        double tasaCrecimientoPromedioBtval = CalcularTasaCrecimientoPromedio(btvalList, anioList);
                        double tasaCrecimientoPromedioTotal = CalcularTasaCrecimientoPromedio(totalList, anioList);

                        // Escribir las tasas de crecimiento promedio en la última fila
                        ws.Cells[indexF108, 3].Value = tasaCrecimientoPromedioAtval * 100; // Tasa de crecimiento en Atval
                        ws.Cells[indexF108, 4].Value = tasaCrecimientoPromedioMtval * 100; // Tasa de crecimiento en Mtval
                        ws.Cells[indexF108, 5].Value = tasaCrecimientoPromedioBtval * 100; // Tasa de crecimiento en Btval
                        ws.Cells[indexF108, 6].Value = tasaCrecimientoPromedioTotal * 100; // Tasa de crecimiento en Total

                        // Estilo de los resultados
                        ws.Cells[indexF108, 3].Style.Numberformat.Format = "0.0000";
                        ws.Cells[indexF108, 4].Style.Numberformat.Format = "0.0000";
                        ws.Cells[indexF108, 5].Style.Numberformat.Format = "0.0000";
                        ws.Cells[indexF108, 6].Style.Numberformat.Format = "0.0000";

                        // Agregar la fila para la tasa promedio
                        ws.Cells[indexF108, 1, indexF108, 2].Merge = true;
                        ws.Cells[indexF108, 1, indexF108, 2].Value = "TASA PROMEDIO (%)";

                        // Aplicar fondo gris claro (#FAFAFA) a la columna del Total
                        rg = ws.Cells[8, 6, indexF108, 6]; // Rango desde la cabecera hasta la última fila
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        rg = ws.Cells[9, 1, indexF108, 6];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);

                        // Estilo de la cabecera
                        rg = ws.Cells[8, 1, indexF108, 2];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Estilo para la última fila con bordes
                        rg = ws.Cells[indexF108, 1, indexF108, 6];
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }


                //Formato Ficha F-P01.1
                Itcdfp011DTO itcdfp011DTO = proyectoModel.Itcdfp011DTO;
                ws = xlPackage.Workbook.Worksheets["F-P01.1"];
                if (itcdfp011DTO != null)
                {
                    int indexBarra = 2;
                    ws.Cells[4, 2].Value = proyectoModel.TransmisionProyectoDTO.Areademanda;
                    var nroBarra = itcdfp011DTO.NroBarras;

                    // Configurar las cabeceras dinámicamente según el número de barras
                    for (int i = 1; i <= nroBarra; i++)
                    {
                        ws.Cells[7, indexBarra, 7, indexBarra + 1].Merge = true;
                        ws.Cells[7, indexBarra, 7, indexBarra + 1].Value = $"BARRA[{i}]";
                        ws.Cells[8, indexBarra, 8, indexBarra + 1].Merge = true;
                        ws.Cells[8, indexBarra, 8, indexBarra + 1].Value = "kV";
                        ws.Cells[9, indexBarra].Value = "kW";
                        ws.Cells[9, indexBarra + 1].Value = "kVAR";
                        indexBarra += 2;
                    }

                    // Crear la lista de fechas esperadas (cada 15 minutos en el año)
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year - 1;
                    var inicioAnio = new DateTime(anioPeriodo, 1, 1, 0, 15, 0);
                    var finAnio = new DateTime(anioPeriodo, 12, 31, 23, 45, 0);
                    var intervalo = TimeSpan.FromMinutes(15);

                    Dictionary<DateTime, int> mapFechaFila = new Dictionary<DateTime, int>();
                    int filaIndex = 10;

                    for (var fecha = inicioAnio; fecha <= finAnio; fecha = fecha.Add(intervalo))
                    {
                        ws.Cells[filaIndex, 1].Value = fecha.ToString("dd/MM/yyyy HH:mm");
                        mapFechaFila[fecha] = filaIndex;
                        filaIndex++;
                    }

                    // Procesar los datos en un diccionario para escritura eficiente
                    Dictionary<DateTime, Dictionary<int, (double?, double?)>> datosAgrupados = new Dictionary<DateTime, Dictionary<int, (double?, double?)>>();

                    foreach (var detReg in itcdfp011DTO.ListItcdf011Det)
                    {
                        DateTime fechaHora = DateTime.ParseExact(detReg.FechaHora, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                        if (!datosAgrupados.ContainsKey(fechaHora))
                        {
                            datosAgrupados[fechaHora] = new Dictionary<int, (double?, double?)>();
                        }
                        datosAgrupados[fechaHora][detReg.BarraNro] =(
                            (double?)(detReg.Kwval.HasValue ? (double)detReg.Kwval.Value : (double?)null),
                            (double?)(detReg.Kvarval.HasValue ? (double)detReg.Kvarval.Value : (double?)null)
                        );

                    }

                    // Escribir los datos organizados en la hoja Excel
                    foreach (var fechaRegistro in datosAgrupados)
                    {
                        if (mapFechaFila.TryGetValue(fechaRegistro.Key, out int filaExcel))
                        {
                            foreach (var barra in fechaRegistro.Value)
                            {
                                int colKw = barra.Key * 2;
                                int colKvar = colKw + 1;
                                ws.Cells[filaExcel, colKw].Value = barra.Value.Item1;  // KWVAL
                                ws.Cells[filaExcel, colKvar].Value = barra.Value.Item2; // KVARVAL
                                // Aplicar formato de 4 decimales
                                ws.Cells[filaExcel, colKw].Style.Numberformat.Format = "0.0000";
                                ws.Cells[filaExcel, colKvar].Style.Numberformat.Format = "0.0000";
                            }
                        }
                    }

                    // Aplicar estilos y bordes
                    if (nroBarra > 0)
                    {
                        rg = ws.Cells[7, 2, 9, nroBarra * 2 + 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E2EFDA"));
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        rg = ws.Cells[10, 1, filaIndex - 1, nroBarra * 2 + 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }


                //Formato Ficha F-P01.2
                List<Itcdfp012DTO> itcdfP012DTOS = proyectoModel.Itcdfp012DTOs;
                ws = xlPackage.Workbook.Worksheets["F-P01.2"];
                if (itcdfP012DTOS != null)
                {
                    int indexFP012 = 9;
                    ws.Cells[4, 2].Value = proyectoModel.TransmisionProyectoDTO.Areademanda;
                    if (itcdfP012DTOS.Count > 0)
                    {
                        foreach (var detReg in itcdfP012DTOS)
                        {
                            Itcdfp012DTO ObjItcdfP012 = detReg;
                            ws.Cells[indexFP012, 1].Value = ObjItcdfP012.CodigoSicli;
                            ws.Cells[indexFP012, 2].Value = ObjItcdfP012.NombreCliente;
                            ws.Cells[indexFP012, 3].Value = ObjItcdfP012.Subestacion;
                            ws.Cells[indexFP012, 4].Value = ObjItcdfP012.Barra;
                            ws.Cells[indexFP012, 5].Value = ObjItcdfP012.CodigoNivelTension;
                            indexFP012++;
                        }
                        rg = ws.Cells[9, 1, indexFP012 - 1, 5];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                
                // Formato Ficha F-P01.3
                List<Itcdfp013DTO> itcdfP013DTOS = proyectoModel.Itcdfp013DTOs;
                ws = xlPackage.Workbook.Worksheets["F-P01.3"];
                if (itcdfP013DTOS != null)
                {
                    int indexFP013 = 10;
                    int indexAnio = 3;
                    ws.Cells[4, 2].Value = proyectoModel.TransmisionProyectoDTO.Areademanda;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    for (int anio = anioPeriodo + 1; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[9, indexAnio].Value = anio;
                        indexAnio++;
                    }
                    ws.Cells[8, 3, 8, indexAnio - 1].Merge = true;
                    ws.Cells[8, 3, 8, indexAnio - 1].Value = "POTENCIA SOLICITADA (MW) [0]";
                    foreach (var detReg in itcdfP013DTOS)
                    {
                        Itcdfp013DTO ObjItcdfP013 = detReg;
                        ws.Cells[indexFP013, 1].Value = ObjItcdfP013.NombreCliente;
                        ws.Cells[indexFP013, 2].Value = ObjItcdfP013.TipoCarga;
                        foreach(var listItc013Det in ObjItcdfP013.ListItcdf013Det) {
                            int columnaAnio = -1;
                            for (int i = 3; i <= indexAnio - 1; i++) 
                            {
                                if (ws.Cells[9, i].Value.ToString() == listItc013Det.Anio.ToString())
                                {
                                    columnaAnio = i;
                                    break;
                                }
                            }
                            if (columnaAnio != -1)
                            {
                                ws.Cells[indexFP013, columnaAnio].Value = listItc013Det.Valor;
                            }
                        }
                        indexFP013++;
                    }
                    rg = ws.Cells[8, 3, 9, indexAnio - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E2EFDA"));
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    if (itcdfP013DTOS.Count > 0)
                    {
                        rg = ws.Cells[10, 1, indexFP013 - 1, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    } 
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato Ficha F-110
                List<Itcdf110DTO> itcdf110DTOs = proyectoModel.Itcdf110DTOs;
                ws = xlPackage.Workbook.Worksheets["F-110"];
                if (itcdf110DTOs != null)
                {
                    int indexF110 = 6;
                    int indexAnio = 7;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante + 10;
                    for (int anio = anioPeriodo - 1; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[5, indexAnio].Value = anio;
                        indexAnio++;
                    }
                    ws.Cells[4, 7, 4, indexAnio - 1].Merge = true;
                    ws.Cells[4, 7, 4, indexAnio - 1].Value = "MÁXIMA DEMANDA (MW)";
                    foreach (var detReg in itcdf110DTOs)
                    {
                        Itcdf110DTO ObjItcdf110 = detReg;
                        ws.Cells[indexF110, 1].Value = ObjItcdf110.AreaDemanda;
                        ws.Cells[indexF110, 2].Value = ObjItcdf110.Sistema;
                        ws.Cells[indexF110, 3].Value = ObjItcdf110.Subestacion;
                        ws.Cells[indexF110, 4].Value = ObjItcdf110.Tension;
                        ws.Cells[indexF110, 5].Value = ObjItcdf110.Barra;
                        ws.Cells[indexF110, 6].Value = ObjItcdf110.IdCarga;
                        foreach(var listItc110Det in ObjItcdf110.ListItcdf110Det) {
                            int columnaAnio = -1;
                            for (int i = 7; i <= indexAnio - 1; i++) 
                            {
                                if (ws.Cells[5, i].Value.ToString() == listItc110Det.Anio.ToString())
                                {
                                    columnaAnio = i;
                                    break;
                                }
                            }
                            if (columnaAnio != -1)
                            {
                                ws.Cells[indexF110, columnaAnio].Value = listItc110Det.Valor;
                            }
                        }
                        indexF110++;
                    }
                    for (int col = 7; col <= indexAnio - 1; col++) {
                        decimal totalMatValue = 0;
                        decimal totalAtValue = 0;
                        decimal totalMtValue = 0;
                        decimal totalGeneralValue = 0;
                        for (int rw = 6; rw <= indexF110 - 1; rw++) {
                            decimal cellValue = 0;
                            if (decimal.TryParse(ws.Cells[rw, col].Text, out cellValue)) {
                                if (cellValue >= 138) {
                                    totalMatValue += cellValue;
                                } else if (cellValue >= 30 && cellValue < 138) {
                                    totalAtValue += cellValue;
                                } else if (cellValue > 1 && cellValue < 30) {
                                    totalMtValue += cellValue;
                                }
                                totalGeneralValue += cellValue;
                            }
                        }
                        ws.Cells[indexF110, col].Value = totalMatValue;
                        ws.Cells[indexF110 + 1, col].Value = totalAtValue;
                        ws.Cells[indexF110 + 2, col].Value = totalMtValue;
                        ws.Cells[indexF110 + 3, col].Value = totalGeneralValue;
                        // ws.Cells[indexF110, col].Value = totalMatValue.ToString("F4");
                        // ws.Cells[indexF110 + 1, col].Value = totalAtValue.ToString("F4");
                        // ws.Cells[indexF110 + 2, col].Value = totalMtValue.ToString("F4");
                        // ws.Cells[indexF110 + 3, col].Value = totalGeneralValue.ToString("F4");
                    }
                    ws.Cells[indexF110, 6].Value = " TOTAL MAT";
                    ws.Cells[indexF110 + 1, 6].Value = " TOTAL AT";
                    ws.Cells[indexF110 + 2, 6].Value = " TOTAL MT";
                    ws.Cells[indexF110 + 3, 6].Value = " TOTAL";
                    rg = ws.Cells[4, 7, 5, indexAnio - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E2EFDA"));
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    if (itcdf110DTOs.Count > 0)
                    {
                        rg = ws.Cells[6, 1, indexF110 - 1, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = false;
                        rg = ws.Cells[indexF110, 1, indexF110 + 2, 6];
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                        rg = ws.Cells[indexF110, 7, indexF110 + 2, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = false;
                        rg = ws.Cells[indexF110 + 3, 1, indexF110 + 3, 6];
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                        rg = ws.Cells[indexF110 + 3, 7, indexF110 + 3, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = false;
                        rg = ws.Cells[indexF110, 1, indexF110, indexAnio - 1];
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato Ficha F-116
                List<Itcdf116DTO> itcdf116DTOs = proyectoModel.Itcdf116DTOs;
                ws = xlPackage.Workbook.Worksheets["F-116"];
                if (itcdf116DTOs != null)
                {
                    int indexF116 = 6;
                    int indexAnio = 8;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante + 10;
                    for (int anio = anioPeriodo - 1; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[5, indexAnio].Value = anio;
                        indexAnio++;
                    }
                    ws.Cells[4, 8, 4, indexAnio - 1].Merge = true;
                    ws.Cells[4, 8, 4, indexAnio - 1].Value = "MÁXIMA DEMANDA (MW)";
                    foreach (var detReg in itcdf116DTOs)
                    {
                        Itcdf116DTO ObjItcdf116 = detReg;
                        ws.Cells[indexF116, 1].Value = ObjItcdf116.AreaDemanda;
                        ws.Cells[indexF116, 2].Value = ObjItcdf116.Sistema;
                        ws.Cells[indexF116, 3].Value = ObjItcdf116.Subestacion;
                        ws.Cells[indexF116, 4].Value = ObjItcdf116.Tension;
                        ws.Cells[indexF116, 5].Value = ObjItcdf116.Barra;
                        ws.Cells[indexF116, 6].Value = ObjItcdf116.NombreCliente;
                        ws.Cells[indexF116, 7].Value = ObjItcdf116.IdCarga;
                        foreach(var listItc116Det in ObjItcdf116.ListItcdf116Det) {
                            int columnaAnio = -1;
                            for (int i = 8; i <= indexAnio - 1; i++) 
                            {
                                if (ws.Cells[5, i].Value.ToString() == listItc116Det.Anio.ToString())
                                {
                                    columnaAnio = i;
                                    break;
                                }
                            }
                            if (columnaAnio != -1)
                            {
                                ws.Cells[indexF116, columnaAnio].Value = listItc116Det.Valor;
                            }
                        }
                        indexF116++;
                    }
                    for (int col = 8; col <= indexAnio - 1; col++) {
                        decimal totalMatValue = 0;
                        decimal totalAtValue = 0;
                        decimal totalMtValue = 0;
                        decimal totalGeneralValue = 0;
                        for (int rw = 6; rw <= indexF116 - 1; rw++) {
                            decimal cellValue = 0;
                            if (decimal.TryParse(ws.Cells[rw, col].Text, out cellValue)) {
                                if (cellValue >= 138) {
                                    totalMatValue += cellValue;
                                } else if (cellValue >= 30 && cellValue < 138) {
                                    totalAtValue += cellValue;
                                } else if (cellValue > 1 && cellValue < 30) {
                                    totalMtValue += cellValue;
                                }
                                totalGeneralValue += cellValue;
                            }
                        }
                        ws.Cells[indexF116, col].Value = totalMatValue;
                        ws.Cells[indexF116 + 1, col].Value = totalAtValue;
                        ws.Cells[indexF116 + 2, col].Value = totalMtValue;
                        ws.Cells[indexF116 + 3, col].Value = totalGeneralValue;
                        // ws.Cells[indexF116, col].Value = totalMatValue.ToString("F4");
                        // ws.Cells[indexF116 + 1, col].Value = totalAtValue.ToString("F4");
                        // ws.Cells[indexF116 + 2, col].Value = totalMtValue.ToString("F4");
                        // ws.Cells[indexF116 + 3, col].Value = totalGeneralValue.ToString("F4");
                    }
                    ws.Cells[indexF116, 7].Value = " TOTAL MAT";
                    ws.Cells[indexF116 + 1, 7].Value = " TOTAL AT";
                    ws.Cells[indexF116 + 2, 7].Value = " TOTAL MT";
                    ws.Cells[indexF116 + 3, 7].Value = " TOTAL";
                    rg = ws.Cells[4, 8, 5, indexAnio - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E2EFDA"));
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    if (itcdf116DTOs.Count > 0)
                    {
                        rg = ws.Cells[6, 1, indexF116 - 1, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = false;
                        rg = ws.Cells[indexF116, 1, indexF116 + 2, 7];
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                        rg = ws.Cells[indexF116, 8, indexF116 + 2, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = false;
                        rg = ws.Cells[indexF116 + 3, 1, indexF116 + 3, 7];
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                        rg = ws.Cells[indexF116 + 3, 8, indexF116 + 3, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = false;
                        rg = ws.Cells[indexF116, 1, indexF116, indexAnio - 1];
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato Ficha F-121
                List<Itcdf121DTO> itcdf121DTOs = proyectoModel.Itcdf121DTOs;
                ws = xlPackage.Workbook.Worksheets["F-121"];
                if (itcdf121DTOs != null)
                {
                    int indexF121 = 6;
                    int indexAnio = 7;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante + 10;
                    for (int anio = anioPeriodo - 1; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[5, indexAnio].Value = anio;
                        indexAnio++;
                    }
                    ws.Cells[4, 7, 4, indexAnio - 1].Merge = true;
                    ws.Cells[4, 7, 4, indexAnio - 1].Value = "MÁXIMA DEMANDA (MW)";
                    foreach (var detReg in itcdf121DTOs)
                    {
                        Itcdf121DTO ObjItcdf121 = detReg;
                        ws.Cells[indexF121, 1].Value = ObjItcdf121.AreaDemanda;
                        ws.Cells[indexF121, 2].Value = ObjItcdf121.Sistema;
                        ws.Cells[indexF121, 3].Value = ObjItcdf121.Subestacion;
                        ws.Cells[indexF121, 4].Value = ObjItcdf121.Tension;
                        ws.Cells[indexF121, 5].Value = ObjItcdf121.Barra;
                        ws.Cells[indexF121, 6].Value = ObjItcdf121.IdCarga;
                        foreach(var listItc121Det in ObjItcdf121.ListItcdf121Det) {
                            int columnaAnio = -1;
                            for (int i = 7; i <= indexAnio - 1; i++) 
                            {
                                if (ws.Cells[5, i].Value.ToString() == listItc121Det.Anio.ToString())
                                {
                                    columnaAnio = i;
                                    break;
                                }
                            }
                            if (columnaAnio != -1)
                            {
                                ws.Cells[indexF121, columnaAnio].Value = listItc121Det.Valor;
                            }
                        }
                        indexF121++;
                    }
                    for (int col = 7; col <= indexAnio - 1; col++) {
                        decimal totalMatValue = 0;
                        decimal totalAtValue = 0;
                        decimal totalMtValue = 0;
                        decimal totalGeneralValue = 0;
                        for (int rw = 6; rw <= indexF121 - 1; rw++) {
                            decimal cellValue = 0;
                            if (decimal.TryParse(ws.Cells[rw, col].Text, out cellValue)) {
                                if (cellValue >= 138) {
                                    totalMatValue += cellValue;
                                } else if (cellValue >= 30 && cellValue < 138) {
                                    totalAtValue += cellValue;
                                } else if (cellValue > 1 && cellValue < 30) {
                                    totalMtValue += cellValue;
                                }
                                totalGeneralValue += cellValue;
                            }
                        }
                        ws.Cells[indexF121, col].Value = totalMatValue;
                        ws.Cells[indexF121 + 1, col].Value = totalAtValue;
                        ws.Cells[indexF121 + 2, col].Value = totalMtValue;
                        ws.Cells[indexF121 + 3, col].Value = totalGeneralValue;
                        // ws.Cells[indexF121, col].Value = totalMatValue.ToString("F4");
                        // ws.Cells[indexF121 + 1, col].Value = totalAtValue.ToString("F4");
                        // ws.Cells[indexF121 + 2, col].Value = totalMtValue.ToString("F4");
                        // ws.Cells[indexF121 + 3, col].Value = totalGeneralValue.ToString("F4");
                    }
                    ws.Cells[indexF121, 6].Value = " TOTAL MAT";
                    ws.Cells[indexF121 + 1, 6].Value = " TOTAL AT";
                    ws.Cells[indexF121 + 2, 6].Value = " TOTAL MT";
                    ws.Cells[indexF121 + 3, 6].Value = " TOTAL";
                    rg = ws.Cells[4, 7, 5, indexAnio - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E2EFDA"));
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    if (itcdf121DTOs.Count > 0)
                    {
                        rg = ws.Cells[6, 1, indexF121 - 1, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = false;
                        rg = ws.Cells[indexF121, 1, indexF121 + 2, 6];
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                        rg = ws.Cells[indexF121, 7, indexF121 + 2, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = false;
                        rg = ws.Cells[indexF121 + 3, 1, indexF121 + 3, 6];
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Font.Bold = true;
                        rg = ws.Cells[indexF121 + 3, 7, indexF121 + 3, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = false;
                        rg = ws.Cells[indexF121, 1, indexF121, indexAnio - 1];
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato Ficha F-123
                List<Itcdf123DTO> itcdf123DTOs = proyectoModel.Itcdf123DTOs;
                ws = xlPackage.Workbook.Worksheets["F-123"];
                if (itcdf123DTOs != null)
                {
                    int indexF123 = 7;
                    int indexAnio = 4;
                    ws.Cells[4, 2].Value = proyectoModel.TransmisionProyectoDTO.Areademanda;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    for (int anio = anioPeriodo + 4; anio <= anioPeriodo + 19; anio += 5)
                    {
                        ws.Cells[6, indexAnio].Value = anio;
                        indexAnio++;
                    }
                    ws.Cells[5, 4, 5, indexAnio - 1].Merge = true;
                    ws.Cells[5, 4, 5, indexAnio - 1].Value = "MW/km2";
                    foreach (var detReg in itcdf123DTOs)
                    {
                        Itcdf123DTO ObjItcdf123 = detReg;
                        ws.Cells[indexF123, 1].Value = ObjItcdf123.UtmEste;
                        ws.Cells[indexF123, 2].Value = ObjItcdf123.UtmNorte;
                        ws.Cells[indexF123, 3].Value = ObjItcdf123.UtmZona;
                        ws.Cells[indexF123, 4].Value = ObjItcdf123.Anio1;
                        ws.Cells[indexF123, 5].Value = ObjItcdf123.Anio2;
                        ws.Cells[indexF123, 6].Value = ObjItcdf123.Anio3;
                        ws.Cells[indexF123, 7].Value = ObjItcdf123.Anio4;
                        indexF123++;
                    }
                    rg = ws.Cells[5, 4, 6, indexAnio - 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E2EFDA"));
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    if (itcdf123DTOs.Count > 0)
                    {
                        rg = ws.Cells[7, 1, indexF123 - 1, indexAnio - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Transmision Linea en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoTransmisionLinea(ProyectoModel proyectoModel, string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                          ruta,
                          ConstantesCampanias.FolderTemp,
                          identificadorUnico,
                          ConstantesCampanias.FolderFichasTransmision,
                          proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                      );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasTransmision, FormatoArchivosExcelCampanias.NombreFichaExcelTransmisionLinea));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelTransmisionLinea));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }
            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelTransmisionLinea));
            }
            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato hoja 1
                T1LinFichaADTO t1LinFichaADTO = proyectoModel.T1lineasFichaADTO;
                ws = xlPackage.Workbook.Worksheets["FichaA"];
                if (t1LinFichaADTO != null)
                {
                    ws.Cells[9, 4].Value = t1LinFichaADTO.NombreLinea;
                    ws.Cells[12, 4].Value = t1LinFichaADTO.FecPuestaServ;
                    ws.Cells[16, 4].Value = t1LinFichaADTO.SubInicio;
                    if (!string.IsNullOrEmpty(t1LinFichaADTO.OtroSubInicio))
                    {
                        ws.Cells[16, 4].Value = t1LinFichaADTO.OtroSubInicio;
                    }
                    ws.Cells[16, 7].Value = t1LinFichaADTO.SubFin;
                    if (!string.IsNullOrEmpty(t1LinFichaADTO.OtroSubFin))
                    {
                        ws.Cells[16, 7].Value = t1LinFichaADTO.OtroSubFin;
                    }
                    ws.Cells[19, 4].Value = t1LinFichaADTO.EmpPropietaria;
                    ws.Cells[25, 10].Value = t1LinFichaADTO.NivTension;
                    ws.Cells[26, 10].Value = t1LinFichaADTO.CapCorriente;
                    ws.Cells[27, 10].Value = t1LinFichaADTO.CapCorrienteA;
                    ws.Cells[28,10].Value = t1LinFichaADTO.TpoSobreCarga;
                    ws.Cells[29, 10].Value = t1LinFichaADTO.NumTemas;
                    ws.Cells[30, 10].Value = t1LinFichaADTO.LongTotal;
                    //ws.Cells[31, 10].Value = "X" //Adjuntado configuracion geometrica
                    ws.Cells[32, 10].Value = t1LinFichaADTO.LongVanoPromedio;
                    ws.Cells[33, 10].Value = t1LinFichaADTO.TipMatSop;
                    //ws.Cells[34, 10].Value = "X" //Adjuntado ruta geográfica
                    //ws.Cells[35, 10].Value = "X" //Adjuntado perfil longitudinal

                    var indexX = 42;
                    var indexRgX = 42;
                    var indexY = 4;
                    // Tabla 1
                    if (t1LinFichaADTO.LineasFichaADet1DTO != null)
                    {
                        var i = 1;
                        foreach (var regHoja in t1LinFichaADTO.LineasFichaADet1DTO)
                        {
                            ws.Cells[indexX, indexY].Value = regHoja.Tramo;
                            ws.Cells[indexX, indexY + 1].Value = regHoja.Tipo;
                            ws.Cells[indexX, indexY + 2].Value = regHoja.Longitud;
                            ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Merge = true;
                            ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Value = regHoja.MatConductor;
                            ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Merge = true;
                            ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Value = regHoja.SecConductor;
                            ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Merge = true;
                            ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Value = regHoja.ConductorFase;
                            ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Merge = true;
                            ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Value = regHoja.CapacidadTot;
                            ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Merge = true;
                            ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Value = regHoja.CabGuarda;
                            ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Merge = true;
                            ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Value = regHoja.ResistCabGuarda;
                            indexX++;
                            i++;
                        };
                    }
                    if (t1LinFichaADTO.LineasFichaADet1DTO.Count > 0)
                    {
                        rg = ws.Cells[indexRgX, indexY, indexX - 1, indexY + 14];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                    indexX ++;
                    indexRgX = indexX;

                    // Tabla 2
                    if (t1LinFichaADTO.LineasFichaADet2DTO != null)
                    {
                        ws.Cells[indexX, indexY].Value = "Tramo";
                        ws.Cells[indexX, indexY + 1].Value = "R";
                        ws.Cells[indexX, indexY + 2].Value = "X";
                        ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Merge = true;
                        ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Value = "B";
                        ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Merge = true;
                        ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Value = "G";
                        ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Merge = true;
                        ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Value = "R0";
                        ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Merge = true;
                        ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Value = "X0";
                        ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Merge = true;
                        ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Value = "B0";
                        ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Merge = true;
                        ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Value = "G0";
                        ws.Cells[indexX, indexY, indexX, indexY + 14].Style.Font.Bold = true;
                        ws.Cells[indexX, indexY, indexX, indexY + 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        indexX++;
                        ws.Cells[indexX, indexY].Value = "#";
                        ws.Cells[indexX, indexY + 1].Value = "ohm/km";
                        ws.Cells[indexX, indexY + 2].Value = "ohm/km";
                        ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Merge = true;
                        ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Value = "mS/km";
                        ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Merge = true;
                        ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Value = "mS/km";
                        ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Merge = true;
                        ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Value = "ohm/km";
                        ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Merge = true;
                        ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Value = "ohm/km";
                        ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Merge = true;
                        ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Value = "mS/km";
                        ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Merge = true;
                        ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Value = "mS/km";
                        ws.Cells[indexX, indexY, indexX, indexY + 14].Style.Font.Bold = true;
                        ws.Cells[indexX, indexY, indexX, indexY + 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        indexX++;
                        var i = 1;
                        foreach (var regHoja in t1LinFichaADTO.LineasFichaADet2DTO)
                        {
                            ws.Cells[indexX, indexY].Value = regHoja.Tramo;
                            ws.Cells[indexX, indexY + 1].Value = regHoja.R;
                            ws.Cells[indexX, indexY + 2].Value = regHoja.X;
                            ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Merge = true;
                            ws.Cells[indexX, indexY + 3, indexX, indexY + 4].Value = regHoja.B;
                            ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Merge = true;
                            ws.Cells[indexX, indexY + 5, indexX, indexY + 6].Value = regHoja.G;
                            ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Merge = true;
                            ws.Cells[indexX, indexY + 7, indexX, indexY + 8].Value = regHoja.R0;
                            ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Merge = true;
                            ws.Cells[indexX, indexY + 9, indexX, indexY + 10].Value = regHoja.X0;
                            ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Merge = true;
                            ws.Cells[indexX, indexY + 11, indexX, indexY + 12].Value = regHoja.B0;
                            ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Merge = true;
                            ws.Cells[indexX, indexY + 13, indexX, indexY + 14].Value = regHoja.G0;
                            indexX++;
                            i++;
                        };
                    }
                    rg = ws.Cells[indexRgX, indexY, indexRgX + 1, indexY + 14];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    if (t1LinFichaADTO.LineasFichaADet2DTO.Count > 0)
                    {
                        rg = ws.Cells[indexRgX + 2, indexY, indexX - 1, indexY + 14];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }

                    ws.Cells[indexX + 1, indexY - 2].Value = "3.0";
                    ws.Cells[indexX + 1, indexY - 2].Style.Font.Bold = true;
                    ws.Cells[indexX + 1, indexY].Value = "SISTEMA DE PROTECCION:";
                    ws.Cells[indexX + 1, indexY].Style.Font.Bold = true;


                    ws.Cells[indexX + 3, indexY - 1].Value = "3.1";
                    ws.Cells[indexX + 3, indexY].Value = "Descripción del sistema de protección principal:";

                    rg = ws.Cells[indexX + 4, indexY, indexX + 4, indexY + 17];
                    rg.Merge = true;
                    rg.Value = t1LinFichaADTO.DesProtecPrincipal;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexX + 6, indexY - 1].Value = "3.2";
                    ws.Cells[indexX + 6, indexY].Value = "Descripción del sistema de protección de respaldo:";

                    rg = ws.Cells[indexX + 7, indexY, indexX + 7, indexY + 17];
                    rg.Merge = true;
                    rg.Value = t1LinFichaADTO.DesProtecRespaldo;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexX + 9, indexY - 2].Value = "4.0";
                    ws.Cells[indexX + 9, indexY - 2].Style.Font.Bold = true;
                    ws.Cells[indexX + 9, indexY].Value = "DESCRIPCIÓN GENERAL DEL PROYECTO";
                    ws.Cells[indexX + 9, indexY].Style.Font.Bold = true;


                    rg = ws.Cells[indexX + 11, indexY, indexX + 11, indexY + 17];
                    rg.Merge = true;
                    rg.Value = t1LinFichaADTO.DesGenProyecto;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Transmision Subestacion en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoTransmisionSubestacion(ProyectoModel proyectoModel, string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                         ruta,
                         ConstantesCampanias.FolderTemp,
                         identificadorUnico,
                         ConstantesCampanias.FolderFichasTransmision,
                         proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                     );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasTransmision, FormatoArchivosExcelCampanias.NombreFichaExcelTransmisionSubestacion));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelTransmisionSubestacion));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }
            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelTransmisionSubestacion));
            }
            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato hoja 1
                T2SubestFicha1DTO t2SubestFicha1DTO = proyectoModel.t2SubestFicha1DTO;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                List<DataCatalogoDTO> dataCatalogoDTOs2 = proyectoModel.DataCatalogoDTOs2;
                List<DataCatalogoDTO> dataCatalogoDTOs3 = proyectoModel.DataCatalogoDTOs3;
                ws = xlPackage.Workbook.Worksheets["Hoja 1"];
                if (t2SubestFicha1DTO != null)
                {
                    var sortedData = dataCatalogoDTOs.OrderBy(a => a.DataCatCodi).ToList();
                    var sortedData2 = dataCatalogoDTOs2.OrderBy(a => a.DataCatCodi).ToList();
                    var sortedData3 = dataCatalogoDTOs3.OrderBy(a => a.DataCatCodi).ToList();

                    ws.Cells[9, 4].Value = t2SubestFicha1DTO.NombreSubestacion;
                    ws.Cells[12, 4].Value = t2SubestFicha1DTO.TipoProyecto;
                    ws.Cells[15, 4].Value = t2SubestFicha1DTO.FechaPuestaServicio;
                    ws.Cells[18, 4].Value = t2SubestFicha1DTO.EmpresaPropietaria;
                    ws.Cells[25, 4].Value = t2SubestFicha1DTO.SistemaBarras;
                    if (!string.IsNullOrEmpty(t2SubestFicha1DTO.OtroSistemaBarras))
                    {
                        ws.Cells[25, 4].Value = t2SubestFicha1DTO.OtroSistemaBarras;
                    }

                    var indexX = 31;
                    var indexRgX = 31;
                    var indexY = 4;
                    var indexRgY = 4;
                    if (t2SubestFicha1DTO.NumTrafo > 0)
                    {
                        indexY += 4;
                        for (int i = 1; i <= t2SubestFicha1DTO.NumTrafo; i++)
                        {
                            ws.Cells[indexX, indexY].Value = "Trafo " + i;
                            ws.Cells[indexX, indexY].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexY++;
                        }
                    }
                    if (sortedData != null)
                    {
                        indexX++;
                        foreach (var dataCatalogoDTO in sortedData)
                        {
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Style.WrapText = true;
                            ws.Cells[indexX, indexRgY + 3].Value = dataCatalogoDTO.DescortaDatacat;
                            indexX++;
                        }
                    }
                    if (t2SubestFicha1DTO.Lista1DTOs != null)
                    {
                        foreach (var detRegHoja in t2SubestFicha1DTO.Lista1DTOs)
                        {
                            var matchedCatalogo = sortedData.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = sortedData.IndexOf(matchedCatalogo) + indexRgX + 1;
                                ws.Cells[matchedIndex, indexRgY + Convert.ToInt32(detRegHoja.NumTrafo) + 3].Value = detRegHoja.ValorTrafo;
                            }
                        }
                    }
                    if (t2SubestFicha1DTO.NumTrafo > 0)
                    {
                        rg = ws.Cells[indexRgX, indexRgY, indexRgX, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    }
                    if (t2SubestFicha1DTO.Lista1DTOs.Count > 0)
                    {
                        rg = ws.Cells[indexRgX + 1, indexRgY, indexX - 1, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    } else if (indexX > indexRgX)
                    {
                        rg = ws.Cells[indexRgX + 1, indexRgY, indexX - 1, indexRgY + 3];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }
                    indexRgX = indexX;
                    indexY = indexRgY;

                    ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                    ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = "PRUEBAS";
                    if (t2SubestFicha1DTO.NumTrafo > 0)
                    {
                        indexY += 4;
                        for (int i = 1; i <= t2SubestFicha1DTO.NumTrafo; i++)
                        {
                            ws.Cells[indexX, indexY].Value = "Trafo " + i;
                            ws.Cells[indexX, indexY].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexY++;
                        }
                    }
                    if (sortedData2 != null)
                    {
                        indexX++;
                        foreach (var dataCatalogoDTO in sortedData2)
                        {
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Style.WrapText = true;
                            ws.Cells[indexX, indexRgY + 3].Value = dataCatalogoDTO.DescortaDatacat;
                            indexX++;
                        }
                    }
                    if (t2SubestFicha1DTO.Lista2DTOs != null)
                    {
                        foreach (var detRegHoja in t2SubestFicha1DTO.Lista2DTOs)
                        {
                            var matchedCatalogo = sortedData2.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = sortedData2.IndexOf(matchedCatalogo) + indexRgX + 1;
                                ws.Cells[matchedIndex, indexRgY + Convert.ToInt32(detRegHoja.NumTrafo) + 3].Value = detRegHoja.ValorTrafo;
                            }
                        }
                    }
                    if (t2SubestFicha1DTO.NumTrafo > 0)
                    {
                        rg = ws.Cells[indexRgX, indexRgY, indexRgX, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    }
                    else {
                        rg = ws.Cells[indexRgX, indexRgY, indexRgX, indexY + 3];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    }
                    if (t2SubestFicha1DTO.Lista2DTOs.Count > 0)
                    {
                        rg = ws.Cells[indexRgX + 1, indexRgY, indexX - 1, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    } else if (indexX > indexRgX) {
                        rg = ws.Cells[indexRgX + 1, indexRgY, indexX - 1, indexRgY + 3];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }

                    ws.Cells[indexX + 1, indexRgY - 2].Value = "4.0";
                    ws.Cells[indexX + 1, indexRgY - 2].Style.Font.Bold = true;
                    ws.Cells[indexX + 1, indexRgY, indexX + 1, indexRgY + 3].Merge = true;
                    ws.Cells[indexX + 1, indexRgY, indexX + 1, indexRgY + 3].Value = "EQUIPOS DE COMPENSACIÓN REACTIVA.";
                    ws.Cells[indexX + 1, indexRgY, indexX + 1, indexRgY + 3].Style.Font.Bold = true;

                    indexX += 3;
                    indexRgX = indexX;
                    indexY = indexRgY;

                    ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                    ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = "CARACTERISTICAS";
                    if (t2SubestFicha1DTO.NumEquipos > 0)
                    {
                        indexY += 4;
                        for (int i = 1; i <= t2SubestFicha1DTO.NumEquipos; i++)
                        {
                            ws.Cells[indexX, indexY].Value = "Equipo " + i;
                            ws.Cells[indexX, indexY].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            indexY++;
                        }
                    }
                    if (sortedData3 != null)
                    {
                        indexX++;
                        foreach (var dataCatalogoDTO in sortedData3)
                        {
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Merge = true;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexX, indexRgY, indexX, indexRgY + 2].Style.WrapText = true;
                            ws.Cells[indexX, indexRgY + 3].Value = dataCatalogoDTO.DescortaDatacat;
                            indexX++;
                        }
                    }
                    if (t2SubestFicha1DTO.Lista3DTOs != null)
                    {
                        foreach (var detRegHoja in t2SubestFicha1DTO.Lista3DTOs)
                        {
                            var matchedCatalogo = sortedData3.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = sortedData3.IndexOf(matchedCatalogo) + indexRgX + 1;
                                ws.Cells[matchedIndex, indexRgY + Convert.ToInt32(detRegHoja.NumEquipo) + 3].Value = detRegHoja.ValorEquipo;
                            }
                        }
                    }
                    if (t2SubestFicha1DTO.NumEquipos > 0)
                    {
                        rg = ws.Cells[indexRgX, indexRgY, indexRgX, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    } else {
                        rg = ws.Cells[indexRgX, indexRgY, indexRgX, indexY + 3];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    }
                    if (t2SubestFicha1DTO.Lista3DTOs.Count > 0)
                    {
                        rg = ws.Cells[indexRgX + 1, indexRgY, indexX - 1, indexY - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    } else if (indexX > indexRgX) {
                        rg = ws.Cells[indexRgX + 1, indexRgY, indexX - 1, indexRgY + 3];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.Black);
                    }

                    ws.Cells[indexX, indexRgY - 1, indexX, indexRgY + t2SubestFicha1DTO.NumEquipos + 3].Merge = true;
                    ws.Cells[indexX, indexRgY - 1, indexX, indexRgY + t2SubestFicha1DTO.NumEquipos + 3].Value = "'(1)  Las tensiones de cortocircuito (P-S, P-T Y S-T) y las pérdidas en el cobre (P-S, P-T Y S-T) se deben ";

                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Transmision Cronograma en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoTransmisionCronograma(ProyectoModel proyectoModel, string identificadorUnico)
        {
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                            rutaBase,
                            ConstantesCampanias.FolderTemp,
                            identificadorUnico,
                            ConstantesCampanias.FolderFichasTransmision,
                            proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                        );
            FileInfo template = new FileInfo(Path.Combine(rutaBase, ConstantesCampanias.FolderFichasTransmision, FormatoArchivosExcelCampanias.NombreFichaExcelTransmisionCronograma));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelTransmisionCronograma));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }
            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelTransmisionCronograma));
            }
            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato Transmision cronograma
                CroFicha1DTO croFicha1DTO = proyectoModel.CroFicha1DTO;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                ws = xlPackage.Workbook.Worksheets["FichaB"];
                if (croFicha1DTO != null)
                {
                    ws.Cells[25, 4].Value = croFicha1DTO.Fecpuestaope;
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexIni = 9;
                    var indexIniY = 3;
                    var indexAnioA = indexIniY + 2;
                    var indexCatA = indexIni + 3;
                    var indexTrimA = indexIniY + 1;

                    ws.Cells[indexIni, indexIniY + 1].Value = $"Año {anioPeriodo - 1} ó antes";
                    for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Value = anio;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 10;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Value = "TRIMESTRE";
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 8;
                        for (int t = 1; t <= 4; t++)
                        {
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Value = t;
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Style.Font.Size = 8;
                            ws.Column(indexAnioA + t - 1).Width = 3;
                        }
                        indexAnioA = indexAnioA + 4;
                    }
                    if (dataCatalogoDTOs != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                        {
                            ws.Cells[indexCatA, indexIniY].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexCatA, indexIniY].Style.WrapText = true;
                            indexCatA++;
                        }
                    }
                    if (croFicha1DTO.ListaCroFicha1DetDTO != null)
                    {
                        foreach (var detRegHoja in croFicha1DTO.ListaCroFicha1DetDTO)
                        {
                            var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.Datacatcodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + indexIni + 3;
                                int matchTrim;
                                if (detRegHoja.Anio == "0")
                                {
                                    matchTrim = indexTrimA;
                                }
                                else
                                {
                                    matchTrim = indexTrimA + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                }
                                ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                            }
                        }
                    }
                    rg = ws.Cells[indexIni, indexIniY, indexIni + 2, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.WrapText = true;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                    rg = ws.Cells[indexIni + 3, indexIniY, indexCatA - 1, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg = ws.Cells[indexIni + 3, indexIniY + 1, indexCatA - 1, indexAnioA - 1];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.WrapText = true;
                    rg.Style.Font.Size = 10;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }

                xlPackage.Save();
            }
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Demanda en formato excel
        /// </summary>
        /// <param name="model"></param>
       public static void GenerarExcelFichaProyectoTipoDemanda(ProyectoModel proyectoModel, string identificadorUnico)

         {
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                                    rutaBase,
                                    ConstantesCampanias.FolderTemp,
                                    identificadorUnico,
                                    ConstantesCampanias.FolderFichasDemanda,
                                    proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                                );

            // Ruta original de la plantilla
            string pathPlantillaOriginal = Path.Combine(rutaBase, ConstantesCampanias.FolderFichasDemanda, FormatoArchivosExcelCampanias.NombreFichaExcelDemanda);

            // Ruta temporal de la plantilla (dentro del UID)
            string pathPlantillaTemp = Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelDemanda);

            // Crear directorio si no existe
            if (!Directory.Exists(pathNewFile))
                Directory.CreateDirectory(pathNewFile);

            // Validación de existencia de plantilla
            if (!File.Exists(pathPlantillaOriginal))
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + pathPlantillaOriginal);

            // Copiar plantilla a ruta temporal
            File.Copy(pathPlantillaOriginal, pathPlantillaTemp, true);

            FileInfo newFile = new FileInfo(pathPlantillaTemp);

            int row = 7;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato FormatoD1-A
                FormatoD1ADTO formatoD1ADTO = proyectoModel.FormatoD1ADTO;
                List<DataCatalogoDTO> dataCatalogoDTOs2 = proyectoModel.DataCatalogoDTOs2;
                ws = xlPackage.Workbook.Worksheets["FormatoD1-A"];
                if (formatoD1ADTO != null)
                {
                    //ws.Cells[, ].Value = formatoD1ADTO.TipoCarga;
                    ws.Cells[14, 5].Value = formatoD1ADTO.Nombre;
                    ws.Cells[17, 5].Value = formatoD1ADTO.EmpresaProp;
                    ws.Cells[21, 5].Value = formatoD1ADTO.Distrito;
                    if (proyectoModel.ubicacionDTO != null)
                    {
                        ws.Cells[21, 5].Value = proyectoModel.ubicacionDTO.Departamento;
                        ws.Cells[21, 7].Value = proyectoModel.ubicacionDTO.Provincia;
                        ws.Cells[21, 9].Value = proyectoModel.ubicacionDTO.Distrito;
                    }
                    ws.Cells[24, 5].Value = formatoD1ADTO.ActDesarrollo;
                    ws.Cells[28, 5].Value = formatoD1ADTO.SituacionAct;
                    ws.Cells[32, 9].Value = formatoD1ADTO.Exploracion;
                    ws.Cells[33, 9].Value = formatoD1ADTO.EstudioPreFactibilidad;
                    ws.Cells[34, 9].Value = formatoD1ADTO.EstudioFactibilidad;
                    ws.Cells[35, 9].Value = formatoD1ADTO.EstudioImpAmb;
                    ws.Cells[36, 9].Value = formatoD1ADTO.Financiamiento1;
                    ws.Cells[37, 9].Value = formatoD1ADTO.Ingenieria;
                    ws.Cells[38, 9].Value = formatoD1ADTO.Construccion;
                    ws.Cells[39, 9].Value = formatoD1ADTO.PuestaMarchar;
                    ws.Cells[44, 9].Value = formatoD1ADTO.TipoExtraccionMin;
                    ws.Cells[45, 9].Value = formatoD1ADTO.MetalesExtraer;
                    ws.Cells[46, 9].Value = formatoD1ADTO.TipoYacimiento;
                    ws.Cells[47, 9].Value = formatoD1ADTO.VidaUtil;
                    ws.Cells[48, 9].Value = formatoD1ADTO.Reservas;
                    ws.Cells[49, 9].Value = formatoD1ADTO.EscalaProduccion;
                    ws.Cells[50, 9].Value = formatoD1ADTO.PlantaBeneficio;
                    ws.Cells[51, 9].Value = formatoD1ADTO.RecuperacionMet;
                    ws.Cells[52, 9].Value = formatoD1ADTO.LeyesConcentrado;
                    ws.Cells[53, 9].Value = formatoD1ADTO.CapacidadTrata;
                    ws.Cells[54, 9].Value = formatoD1ADTO.ProduccionAnual;
                    ws.Cells[59, 5].Value = formatoD1ADTO.Item;
                    ws.Cells[59, 7].Value = formatoD1ADTO.ToneladaMetrica;
                    ws.Cells[59, 8].Value = formatoD1ADTO.Energia;
                    ws.Cells[59, 9].Value = formatoD1ADTO.Consumo;
                    ws.Cells[62, 8].Value = formatoD1ADTO.SubestacionCodi;
                    if (!string.IsNullOrEmpty(formatoD1ADTO.SubestacionOtros))
                    {
                        ws.Cells[62, 8].Value = formatoD1ADTO.SubestacionOtros;
                    }
                    ws.Cells[63, 8].Value = formatoD1ADTO.NivelTension;
                    ws.Cells[64, 8].Value = formatoD1ADTO.EmpresaSuminicodi;
                    if (!string.IsNullOrEmpty(formatoD1ADTO.EmpresaSuminiOtro))
                    {
                        ws.Cells[64, 8].Value = formatoD1ADTO.EmpresaSuminiOtro;
                    }

                    // Tabla 1 
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexXIni = 70;
                    var indexYIni = 5;
                    var indexRg = 70;

                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Merge = true;
                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Value = "Año";
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni, indexYIni + 4].Value = "Demanda";
                    ws.Cells[indexXIni + 1, indexYIni + 1, indexXIni + 2, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 1, indexXIni + 2, indexYIni + 1].Value = "Energia (GWh)";
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 1, indexYIni + 3].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 1, indexYIni + 3].Value = "Potencia (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 2].Value = "HP";
                    ws.Cells[indexXIni + 2, indexYIni + 3].Value = "HFP";
                    ws.Cells[indexXIni + 1, indexYIni + 4, indexXIni + 2, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 4, indexXIni + 2, indexYIni + 4].Style.WrapText = true;
                    ws.Cells[indexXIni + 1, indexYIni + 4, indexXIni + 2, indexYIni + 4].Value = "Factor de Carga (%)";
                    rg = ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni + 4];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Value = "Generación";
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Style.WrapText = true;
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Value = "Energia (GWh)";
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Value = "Potencia (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 6].Value = "HP";
                    ws.Cells[indexXIni + 2, indexYIni + 7].Value = "HFP";
                    rg = ws.Cells[indexXIni, indexYIni + 5, indexXIni + 2, indexYIni + 7];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF00"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    indexXIni += 3;
                    for (int anio = 2011; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexXIni, indexYIni].Value = anio;
                        ws.Cells[indexXIni, indexYIni].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        indexXIni++;
                    }

                    if (formatoD1ADTO.ListaFormatoDet1A != null)
                    {
                        var total = formatoD1ADTO.ListaFormatoDet1A.Count;
                        foreach (var detRegHoja in formatoD1ADTO.ListaFormatoDet1A)
                        {
                            int filaAnio = -1;
                            for (int r = indexRg; r <= indexXIni; r++)
                            {
                                var valorAnio = ws.Cells[r, indexYIni].Value;
                                if (valorAnio != null && valorAnio.ToString() == detRegHoja.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1)
                            {
                                var indexX = filaAnio;

                                ws.Cells[indexX, indexYIni + 1].Value = detRegHoja.DemandaEnergia;
                                ws.Cells[indexX, indexYIni + 2].Value = detRegHoja.DemandaHP;
                                ws.Cells[indexX, indexYIni + 3].Value = detRegHoja.DemandaHFP;
                                ws.Cells[indexX, indexYIni + 4].Value = detRegHoja.DemandaCarga;
                                ws.Cells[indexX, indexYIni + 5].Value = detRegHoja.GeneracionEnergia;
                                ws.Cells[indexX, indexYIni + 6].Value = detRegHoja.GeneracionHP;
                                ws.Cells[indexX, indexYIni + 7].Value = detRegHoja.GeneracionHFP;
                                ws.Cells[indexX, indexYIni + 1].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            }
                        }
                    }
                    indexXIni -= 3;
                    rg = ws.Cells[indexRg, indexYIni, indexXIni + 2, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                     
                    // Aplicar fondo gris a la antepenúltima columna (indexYIni + 6)
                    ws.Cells[indexRg, indexYIni + 5, indexXIni, indexYIni + 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[indexRg, indexYIni + 5, indexXIni, indexYIni + 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    // Aplicar fondo gris a la antepenúltima columna (indexYIni + 6)
                    ws.Cells[indexRg, indexYIni + 4, indexXIni, indexYIni + 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[indexRg, indexYIni + 4, indexXIni, indexYIni + 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    // Tabla 2

                    ws.Cells[indexXIni + 4, indexYIni, indexXIni + 6, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 4, indexYIni, indexXIni + 6, indexYIni + 7].Style.WrapText = true;
                    ws.Cells[indexXIni + 4, indexYIni, indexXIni + 6, indexYIni + 7].Value = "2.4.2 Proyección de Demanda a tomar del SEIN en Escenario Optimista (respecto al Esc. Medio, son las proyecciones en condiciones ideales planeadas, tanto en nivel de demanda como en fecha de entrada, estando el proyecto a cualquier etapa de desarrollo):";

                    indexXIni += 7;
                    indexRg = indexXIni;

                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Merge = true;
                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Value = "Año";
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni, indexYIni + 4].Value = "Demanda";
                    ws.Cells[indexXIni + 1, indexYIni + 1, indexXIni + 2, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 1, indexXIni + 2, indexYIni + 1].Value = "Energia (GWh)";
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 1, indexYIni + 3].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 1, indexYIni + 3].Value = "Potencia (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 2].Value = "HP";
                    ws.Cells[indexXIni + 2, indexYIni + 3].Value = "HFP";
                    ws.Cells[indexXIni + 1, indexYIni + 4, indexXIni + 2, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 4, indexXIni + 2, indexYIni + 4].Style.WrapText = true;
                    ws.Cells[indexXIni + 1, indexYIni + 4, indexXIni + 2, indexYIni + 4].Value = "Factor de Carga (%)";
                    rg = ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni + 4];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Value = "Generación";
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Style.WrapText = true;
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Value = "Energia (GWh)";
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Value = "Potencia (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 6].Value = "HP";
                    ws.Cells[indexXIni + 2, indexYIni + 7].Value = "HFP";
                    rg = ws.Cells[indexXIni, indexYIni + 5, indexXIni + 2, indexYIni + 7];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF00"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    indexXIni += 3;
                    for (int anio = 2011; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexXIni, indexYIni].Value = anio;
                        ws.Cells[indexXIni, indexYIni].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        indexXIni++;
                    }

                    if (formatoD1ADTO.ListaFormatoDet2A != null)
                    {
                        foreach (var detRegHoja in formatoD1ADTO.ListaFormatoDet2A)
                        {
                            int filaAnio = -1;
                            for (int r = indexRg; r <= indexXIni; r++)
                            {
                                var valorAnio = ws.Cells[r, indexYIni].Value;
                                if (valorAnio != null && valorAnio.ToString() == detRegHoja.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1)
                            {
                                var indexX = filaAnio;

                                ws.Cells[indexX, indexYIni + 1].Value = detRegHoja.DemandaEnergia;
                                ws.Cells[indexX, indexYIni + 2].Value = detRegHoja.DemandaHP;
                                ws.Cells[indexX, indexYIni + 3].Value = detRegHoja.DemandaHFP;
                                ws.Cells[indexX, indexYIni + 4].Value = detRegHoja.DemandaCarga;
                                ws.Cells[indexX, indexYIni + 5].Value = detRegHoja.GeneracionEnergia;
                                ws.Cells[indexX, indexYIni + 6].Value = detRegHoja.GeneracionHP;
                                ws.Cells[indexX, indexYIni + 7].Value = detRegHoja.GeneracionHFP;
                                ws.Cells[indexX, indexYIni + 1].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            }
                        }
                    }
                    indexXIni -= 3;
                    rg = ws.Cells[indexRg, indexYIni, indexXIni + 2, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    

                    // Aplicar fondo gris a la antepenúltima columna (indexYIni + 6)
                    ws.Cells[indexRg, indexYIni + 5, indexXIni,indexYIni +5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[indexRg, indexYIni + 5, indexXIni, indexYIni + 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    // Aplicar fondo gris a la antepenúltima columna (indexYIni + 6)
                    ws.Cells[indexRg, indexYIni + 4, indexXIni, indexYIni + 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[indexRg, indexYIni + 4, indexXIni, indexYIni + 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    // Tabla 3

                    ws.Cells[indexXIni + 4, indexYIni, indexXIni + 6, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 4, indexYIni, indexXIni + 6, indexYIni + 7].Style.WrapText = true;
                    ws.Cells[indexXIni + 4, indexYIni, indexXIni + 6, indexYIni + 7].Value = "2.4.3 Proyección de Demanda a tomar del SEIN en Escenario Pesimista (respecto al Esc. Medio, son las proyecciones en condiciones extremas adversas planeadas, tanto en nivel de demanda como en fecha de entrada, estando el proyecto en cualquier etapa de desarrollo):";

                    indexXIni += 7;
                    indexRg = indexXIni;

                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Merge = true;
                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Value = "Año";
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni, indexYIni + 4].Value = "Demanda";
                    ws.Cells[indexXIni + 1, indexYIni + 1, indexXIni + 2, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 1, indexXIni + 2, indexYIni + 1].Value = "Energia (GWh)";
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 1, indexYIni + 3].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 1, indexYIni + 3].Value = "Potencia (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 2].Value = "HP";
                    ws.Cells[indexXIni + 2, indexYIni + 3].Value = "HFP";
                    ws.Cells[indexXIni + 1, indexYIni + 4, indexXIni + 2, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 4, indexXIni + 2, indexYIni + 4].Style.WrapText = true;
                    ws.Cells[indexXIni + 1, indexYIni + 4, indexXIni + 2, indexYIni + 4].Value = "Factor de Carga (%)";
                    rg = ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni + 4];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Value = "Generación";
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Style.WrapText = true;
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Value = "Energia (GWh)";
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Value = "Potencia (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 6].Value = "HP";
                    ws.Cells[indexXIni + 2, indexYIni + 7].Value = "HFP";
                    rg = ws.Cells[indexXIni, indexYIni + 5, indexXIni + 2, indexYIni + 7];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF00"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    indexXIni += 3;
                    for (int anio = 2011; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexXIni, indexYIni].Value = anio;
                        ws.Cells[indexXIni, indexYIni].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        indexXIni++;
                    }

                    if (formatoD1ADTO.ListaFormatoDet3A != null)
                    {
                        foreach (var detRegHoja in formatoD1ADTO.ListaFormatoDet3A)
                        {
                            int filaAnio = -1;
                            for (int r = indexRg; r <= indexXIni; r++)
                            {
                                var valorAnio = ws.Cells[r, indexYIni].Value;
                                if (valorAnio != null && valorAnio.ToString() == detRegHoja.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1)
                            {
                                var indexX = filaAnio;

                                ws.Cells[indexX, indexYIni + 1].Value = detRegHoja.DemandaEnergia;
                                ws.Cells[indexX, indexYIni + 2].Value = detRegHoja.DemandaHP;
                                ws.Cells[indexX, indexYIni + 3].Value = detRegHoja.DemandaHFP;
                                ws.Cells[indexX, indexYIni + 4].Value = detRegHoja.DemandaCarga;
                                ws.Cells[indexX, indexYIni + 5].Value = detRegHoja.GeneracionEnergia;
                                ws.Cells[indexX, indexYIni + 6].Value = detRegHoja.GeneracionHP;
                                ws.Cells[indexX, indexYIni + 7].Value = detRegHoja.GeneracionHFP;
                                ws.Cells[indexX, indexYIni + 1].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            }
                        }
                    }
                    indexXIni -= 3;
                    rg = ws.Cells[indexRg, indexYIni, indexXIni + 2, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 4, indexYIni - 2].Value = "2.5";
                    ws.Cells[indexXIni + 4, indexYIni, indexXIni + 4, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni + 4, indexYIni, indexXIni + 4, indexYIni + 1].Value = "Factor de Potencia estimado:";
                    ws.Cells[indexXIni + 4, indexYIni + 2, indexXIni + 4, indexYIni + 3].Merge = true;
                    ws.Cells[indexXIni + 4, indexYIni + 2, indexXIni + 4, indexYIni + 3].Value = formatoD1ADTO.FactorPotencia;
                    rg = ws.Cells[indexXIni + 4, indexYIni, indexXIni + 4, indexYIni + 3];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    // Aplicar fondo gris a la antepenúltima columna (indexYIni + 6)
                    ws.Cells[indexRg, indexYIni + 5, indexXIni, indexYIni + 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[indexRg, indexYIni + 5, indexXIni, indexYIni + 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    // Aplicar fondo gris a la antepenúltima columna (indexYIni + 6)
                    ws.Cells[indexRg, indexYIni + 4, indexXIni, indexYIni + 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[indexRg, indexYIni + 4, indexXIni, indexYIni + 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    ws.Cells[indexXIni + 6, indexYIni - 2].Value = "2.6";
                    ws.Cells[indexXIni + 6, indexYIni].Value = "Equipos de compensación reactiva";
                    ws.Cells[indexXIni + 7, indexYIni + 1].Value = "MVAR";
                    ws.Cells[indexXIni + 7, indexYIni + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[indexXIni + 7, indexYIni + 4].Value = "MVAR";
                    ws.Cells[indexXIni + 7, indexYIni + 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg = ws.Cells[indexXIni + 7, indexYIni + 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg = ws.Cells[indexXIni + 7, indexYIni + 4];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 8, indexYIni].Value = "Inductivo";
                    ws.Cells[indexXIni + 8, indexYIni + 1].Value = formatoD1ADTO.Inductivo;
                    rg = ws.Cells[indexXIni + 8, indexYIni, indexXIni + 8, indexYIni + 1];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 8, indexYIni + 3].Value = "Capacitivo";
                    ws.Cells[indexXIni + 8, indexYIni + 4].Value = formatoD1ADTO.Capacitivo;
                    rg = ws.Cells[indexXIni + 8, indexYIni + 3, indexXIni + 8, indexYIni + 4];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 10, indexYIni - 2].Value = "2.7";
                    ws.Cells[indexXIni + 10, indexYIni].Value = "Diagramas unifilares adjuntos";
                    ws.Cells[indexXIni + 11, indexYIni + 2, indexXIni + 11, indexYIni + 3].Merge = true;
                    ws.Cells[indexXIni + 11, indexYIni + 2, indexXIni + 11, indexYIni + 3].Value = "Primera Etapa";
                    ws.Cells[indexXIni + 11, indexYIni + 4, indexXIni + 11, indexYIni + 5].Merge = true;
                    ws.Cells[indexXIni + 11, indexYIni + 4, indexXIni + 11, indexYIni + 5].Value = "Segunda Etapa";
                    ws.Cells[indexXIni + 11, indexYIni + 6, indexXIni + 11, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 11, indexYIni + 6, indexXIni + 11, indexYIni + 7].Value = "Final";
                    rg = ws.Cells[indexXIni + 11, indexYIni + 2, indexXIni +11, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[indexXIni + 12, indexYIni, indexXIni + 12, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni + 12, indexYIni, indexXIni + 12, indexYIni + 1].Value = "Año";
                    ws.Cells[indexXIni + 13, indexYIni, indexXIni + 13, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni + 13, indexYIni, indexXIni + 13, indexYIni + 1].Value = "Adjunto (Marcar con \"X\")";
                    ws.Cells[indexXIni + 12, indexYIni + 2, indexXIni + 12, indexYIni + 3].Merge = true;
                    ws.Cells[indexXIni + 12, indexYIni + 2, indexXIni + 12, indexYIni + 3].Value = formatoD1ADTO.PrimeraEtapa;
                    ws.Cells[indexXIni + 13, indexYIni + 2, indexXIni + 13, indexYIni + 3].Merge = true;
                    ws.Cells[indexXIni + 13, indexYIni + 2, indexXIni + 13, indexYIni + 3].Value = "";
                    ws.Cells[indexXIni + 12, indexYIni + 4, indexXIni + 12, indexYIni + 5].Merge = true;
                    ws.Cells[indexXIni + 12, indexYIni + 4, indexXIni + 12, indexYIni + 5].Value = formatoD1ADTO.SegundaEtapa;
                    ws.Cells[indexXIni + 13, indexYIni + 4, indexXIni + 13, indexYIni + 5].Merge = true;
                    ws.Cells[indexXIni + 13, indexYIni + 4, indexXIni + 13, indexYIni + 5].Value = "";
                    ws.Cells[indexXIni + 12, indexYIni + 6, indexXIni + 12, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 12, indexYIni + 6, indexXIni + 12, indexYIni + 7].Value = formatoD1ADTO.Final;
                    ws.Cells[indexXIni + 13, indexYIni + 6, indexXIni + 13, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 13, indexYIni + 6, indexXIni + 13, indexYIni + 7].Value = "";
                    rg = ws.Cells[indexXIni + 12, indexYIni, indexXIni + 13, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 15, indexYIni - 3].Value = "3.0";
                    ws.Cells[indexXIni + 15, indexYIni - 3].Style.Font.Bold = true;
                    ws.Cells[indexXIni + 15, indexYIni].Value = "ASPECTOS ECONÓMICOS: (Referencial)";
                    ws.Cells[indexXIni + 15, indexYIni - 3, indexXIni + 15, indexYIni].Style.Font.Bold = true;
                    ws.Cells[indexXIni + 17, indexYIni - 2].Value = "3.1";
                    ws.Cells[indexXIni + 17, indexYIni].Value = "Indicadores Económicos";

                    ws.Cells[indexXIni + 18, indexYIni, indexXIni + 19, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni + 18, indexYIni, indexXIni + 19, indexYIni + 1].Value = "Costo de Producción";
                    ws.Cells[indexXIni + 18, indexYIni + 2, indexXIni + 18, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni + 18, indexYIni + 2, indexXIni + 18, indexYIni + 4].Value = "Cotizaciones Promedio";
                    ws.Cells[indexXIni + 19, indexYIni + 2].Merge = true;
                    ws.Cells[indexXIni + 19, indexYIni + 2].Value = "Metales";
                    ws.Cells[indexXIni + 19, indexYIni + 3, indexXIni + 19, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni + 19, indexYIni + 3, indexXIni + 19, indexYIni + 4].Value = "Precio (US$/unidad)";
                    ws.Cells[indexXIni + 20, indexYIni, indexXIni + 20, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni + 20, indexYIni, indexXIni + 20, indexYIni + 1].Value = formatoD1ADTO.CostoProduccion;
                    ws.Cells[indexXIni + 20, indexYIni + 2].Value = formatoD1ADTO.Metales;
                    ws.Cells[indexXIni + 20, indexYIni + 3, indexXIni + 20, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni + 20, indexYIni + 3, indexXIni + 20, indexYIni + 4].Value = formatoD1ADTO.Precio;

                    rg = ws.Cells[indexXIni + 18, indexYIni, indexXIni + 20, indexYIni + 4];
                    rg.Style.WrapText = true;
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg = ws.Cells[indexXIni + 18, indexYIni, indexXIni + 19, indexYIni + 4];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

                    ws.Cells[indexXIni + 22, indexYIni - 2].Value = "3.2";
                    ws.Cells[indexXIni + 22, indexYIni].Value = "Inversiones Estimadas/Periodos";

                    indexXIni += 23;
                    indexRg = indexXIni;

                    ws.Cells[indexXIni, indexYIni, indexXIni, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni, indexYIni, indexXIni, indexYIni + 1].Value = "Periodo (Años)";
                    ws.Cells[indexXIni, indexYIni, indexXIni, indexYIni + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[indexXIni, indexYIni + 2, indexXIni, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 2, indexXIni, indexYIni + 4].Value = "Monto de Inversión (US$)";
                    ws.Cells[indexXIni, indexYIni + 2, indexXIni, indexYIni + 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    for (int anio = 2018; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexXIni + 1, indexYIni, indexXIni + 1, indexYIni + 1].Merge = true;
                        ws.Cells[indexXIni + 1, indexYIni, indexXIni + 1, indexYIni + 1].Value = anio;
                        ws.Cells[indexXIni + 1, indexYIni, indexXIni + 1, indexYIni + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        indexXIni++;
                    }

                    if (formatoD1ADTO.ListaFormatoDet4A != null)
                    {
                        foreach (var detRegHoja in formatoD1ADTO.ListaFormatoDet4A)
                        {
                            int filaAnio = -1;
                            for (int r = indexRg; r <= indexXIni; r++)
                            {
                                var valorAnio = ws.Cells[r, indexYIni].Value;
                                if (valorAnio != null && valorAnio.ToString() == detRegHoja.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1)
                            {
                                var indexX = filaAnio;
                                ws.Cells[indexX, indexYIni + 2, indexX, indexYIni + 4].Merge = true;
                                ws.Cells[indexX, indexYIni + 2, indexX, indexYIni + 4].Value = detRegHoja.MontoInversion;
                                ws.Cells[indexX, indexYIni + 2, indexX, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                            }
                        }
                    }
                    rg = ws.Cells[indexRg, indexYIni, indexXIni, indexYIni + 4];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 2, indexYIni - 2].Value = "3.3";
                    ws.Cells[indexXIni + 2, indexYIni].Value = "Financiamiento:";
                    rg = ws.Cells[indexXIni + 3, indexYIni, indexXIni + 3, indexYIni + 6];
                    rg.Merge = true;
                    rg.Value = formatoD1ADTO.Financiamiento2;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 5, indexYIni - 3].Value = "4.0";
                    ws.Cells[indexXIni + 5, indexYIni - 3].Style.Font.Bold = true;
                    ws.Cells[indexXIni + 5, indexYIni].Value = "FACTORES QUE FAVORECEN LA EJECUCIÓN DEL PROYECTO:";
                    ws.Cells[indexXIni + 5, indexYIni].Style.Font.Bold = true;
                    rg = ws.Cells[indexXIni + 6, indexYIni, indexXIni + 7, indexYIni + 7];
                    rg.Merge = true;
                    rg.Value = formatoD1ADTO.FacFavEjecuProy;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 9, indexYIni - 3].Value = "5.0";
                    ws.Cells[indexXIni + 9, indexYIni - 3].Style.Font.Bold = true;
                    ws.Cells[indexXIni + 9, indexYIni].Value = "FACTORES QUE DESFAVORECEN LA EJECUCIÓN DEL PROYECTO:";
                    ws.Cells[indexXIni + 9, indexYIni].Style.Font.Bold = true;
                    rg = ws.Cells[indexXIni + 10, indexYIni, indexXIni + 11, indexYIni + 7];
                    rg.Merge = true;
                    rg.Value = formatoD1ADTO.FactDesEjecuProy;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 13, indexYIni - 3].Value = "6.0";
                    ws.Cells[indexXIni + 13, indexYIni - 3].Style.Font.Bold = true;
                    ws.Cells[indexXIni + 13, indexYIni].Value = "RESUMEN DE LA SITUACION DEL PROYECTO";
                    ws.Cells[indexXIni + 13, indexYIni].Style.Font.Bold = true;

                    indexXIni += 14;
                    indexRg = indexXIni;

                    ws.Cells[indexXIni, indexYIni, indexXIni + 1, indexYIni + 2].Merge = true;
                    ws.Cells[indexXIni, indexYIni, indexXIni + 1, indexYIni + 2].Value = "REQUISITOS";
                    ws.Cells[indexXIni, indexYIni + 3, indexXIni, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 3, indexXIni, indexYIni + 7].Value = "ESTADO SITUACIONAL";
                    ws.Cells[indexXIni + 1, indexYIni + 3].Value = "En\r\nElaboración";
                    ws.Cells[indexXIni + 1, indexYIni + 4].Value = "Presentado";
                    ws.Cells[indexXIni + 1, indexYIni + 5].Value = "En trámite\r\n(Evaluación)";
                    ws.Cells[indexXIni + 1, indexYIni + 6].Value = "Aprobado/\r\nAutorizado";
                    ws.Cells[indexXIni + 1, indexYIni + 7].Value = "Firmado";
                    rg = ws.Cells[indexXIni, indexYIni, indexXIni + 1, indexYIni + 7];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    
                    if (dataCatalogoDTOs2 != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs2)
                        {
                            ws.Cells[indexXIni + 2, indexYIni, indexXIni + 2, indexYIni + 2].Merge = true;
                            ws.Cells[indexXIni + 2, indexYIni, indexXIni + 2, indexYIni + 2].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexXIni + 2, indexYIni, indexXIni + 2, indexYIni + 2].Style.WrapText = true;
                            indexXIni++;
                        }
                    }
                    if (formatoD1ADTO.ListaFormatoDet5A != null)
                    {
                        foreach (var detRegHoja in formatoD1ADTO.ListaFormatoDet5A)
                        {
                            var matchedCatalogo = dataCatalogoDTOs2.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs2.IndexOf(matchedCatalogo) + indexRg + 2;
                                ws.Cells[matchedIndex, indexYIni + 3].Value = detRegHoja.EnElaboracion == "true" ? "x" : "";
                                ws.Cells[matchedIndex, indexYIni + 4].Value = detRegHoja.Presentado == "true" ? "x" : "";
                                ws.Cells[matchedIndex, indexYIni + 5].Value = detRegHoja.EnTramite == "true" ? "x" : "";
                                ws.Cells[matchedIndex, indexYIni + 6].Value = detRegHoja.Aprobado == "true" ? "x" : "";
                                ws.Cells[matchedIndex, indexYIni + 7].Value = detRegHoja.Firmado == "true" ? "x" : "";
                            }
                        }
                    }

                    rg = ws.Cells[indexRg, indexYIni, indexXIni + 1, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexXIni + 3, indexYIni - 3].Value = "7.0";
                    ws.Cells[indexXIni + 3, indexYIni - 3].Style.Font.Bold = true;
                    ws.Cells[indexXIni + 3, indexYIni].Value = "COMENTARIOS:";
                    ws.Cells[indexXIni + 3, indexYIni].Style.Font.Bold = true;
                    rg = ws.Cells[indexXIni + 4, indexYIni, indexXIni + 8, indexYIni + 7];
                    rg.Merge = true;
                    rg.Value = formatoD1ADTO.Comentarios;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato FormatoD1-B
                FormatoD1BDTO formatoD1BDTO = proyectoModel.FormatoD1BDTO;
                ws = xlPackage.Workbook.Worksheets["FormatoD1-B"];
                if (formatoD1BDTO != null)
                {
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var horizonteInicio = anioPeriodo - proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAtras;
                    var indexIni = 24;
                    var indexXIni = 24;
                    var indexRg = 24;
                    var indexYIni = 3;
                    var meses = new string[]
                    {
                        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                        "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
                    }; 
                    ws.Cells[7, 5].Value = formatoD1BDTO.NombreCarga;
                    ws.Cells[8, 5].Value = formatoD1BDTO.Propietario;
                    ws.Cells[9, 5].Value = formatoD1BDTO.FechaIngreso;
                    ws.Cells[10, 5].Value = formatoD1BDTO.BarraConexion;
                    ws.Cells[11, 5].Value = formatoD1BDTO.NivelTension;
                    for (int anio = horizonteInicio; anio <= horizonteFin; anio++)
                    {
                        var rangoAnio = ws.Cells[indexIni, indexYIni, indexIni + 11, indexYIni];
                        rangoAnio.Merge = true;
                        rangoAnio.Value = anio;
                        rangoAnio.Style.Font.Bold = true;
                        rangoAnio.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        rangoAnio.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Merge = true;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Value = "TOTAL " + anio;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Style.Font.Bold = true;
                        rg = ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 7];
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

                        int index = 0;
                        foreach (var str in meses)
                        {
                            ws.Cells[indexIni + index, indexYIni + 1].Value = str;
                            ws.Cells[indexIni + index, indexYIni + 1].Style.Font.Bold = true;
                            index++;
                        };
                        indexIni = indexIni + 13;
                    }

                    if (formatoD1BDTO.ListaFormatoDet1B != null)
                    {
                        foreach (var detRegHoja in formatoD1BDTO.ListaFormatoDet1B)
                        {
                            int filaAnio = -1;
                            for (int r = indexXIni; r <= indexIni; r++)
                            {
                                var valorAnio = ws.Cells[r, indexYIni].Value;
                                if (valorAnio != null && valorAnio.ToString() == detRegHoja.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            int filaMes = -1;
                            for (int r = indexXIni; r <= indexXIni + 11; r++)
                            {
                                var valorMes = ws.Cells[r, indexYIni + 1].Value;
                                if (valorMes != null && valorMes.ToString() == detRegHoja.Mes.ToString())
                                {
                                    filaMes = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1 && filaMes != -1)
                            {
                                var indexX = filaAnio + filaMes - indexXIni;

                                ws.Cells[indexX, indexYIni + 2].Value = detRegHoja.DemandaEnergia;
                                ws.Cells[indexX, indexYIni + 3].Value = detRegHoja.DemandaHP;
                                ws.Cells[indexX, indexYIni + 4].Value = detRegHoja.DemandaHFP;
                                ws.Cells[indexX, indexYIni + 5].Value = detRegHoja.GeneracionEnergia;
                                ws.Cells[indexX, indexYIni + 6].Value = detRegHoja.GeneracionHP;
                                ws.Cells[indexX, indexYIni + 7].Value = detRegHoja.GeneracionHFP;
                                ws.Cells[indexX, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000"; 
                            }
                        }
                        for (int anio = horizonteInicio; anio <= horizonteFin; anio++)
                        {
                            double maxDemandaHP = 0.0;
                            double maxDemandaHFP = 0.0;
                            double maxGeneracionHP = 0.0;
                            double maxGeneracionHFP = 0.0;

                            double totalDemandaEnergia = 0.0;
                            double totalGeneracionEnergia = 0.0;

                            for (int mesIndex = 0; mesIndex < 12; mesIndex++)
                            {
                                var rw = indexXIni + mesIndex;
                                double valorDemandaEnergia = 0;
                                double valorGeneracionEnergia = 0;
                                double valorDemandaHP = 0;
                                double valorDemandaHFP = 0;
                                double valorGeneracionHP = 0;
                                double valorGeneracionHFP = 0;
                                if (ws.Cells[rw, indexYIni + 2].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 2].Value.ToString(), out valorDemandaEnergia))
                                    totalDemandaEnergia += valorDemandaEnergia;
                                if (ws.Cells[rw, indexYIni + 5].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 5].Value.ToString(), out valorGeneracionEnergia))
                                    totalGeneracionEnergia += valorGeneracionEnergia;
                                if (ws.Cells[rw, indexYIni + 3].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 3].Value.ToString(), out valorDemandaHP))
                                    maxDemandaHP = Math.Max(maxDemandaHP, valorDemandaHP);
                                if (ws.Cells[rw, indexYIni + 4].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 4].Value.ToString(), out valorDemandaHFP))
                                    maxDemandaHFP = Math.Max(maxDemandaHFP, valorDemandaHFP);
                                if (ws.Cells[rw, indexYIni + 6].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 6].Value.ToString(), out valorGeneracionHP))
                                    maxGeneracionHP = Math.Max(maxGeneracionHP, valorGeneracionHP);
                                if (ws.Cells[rw, indexYIni + 7].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 7].Value.ToString(), out valorGeneracionHFP))
                                    maxGeneracionHFP = Math.Max(maxGeneracionHFP, valorGeneracionHFP);                            }

                            ws.Cells[indexXIni + 12, indexYIni + 2].Value = totalDemandaEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 3].Value = maxDemandaHP;
                            ws.Cells[indexXIni + 12, indexYIni + 4].Value = maxDemandaHFP;
                            ws.Cells[indexXIni + 12, indexYIni + 5].Value = totalGeneracionEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 6].Value = maxGeneracionHP;
                            ws.Cells[indexXIni + 12, indexYIni + 7].Value = maxGeneracionHFP;

                            ws.Cells[indexXIni + 12, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            indexXIni = indexXIni + 13;
                        }
                    }
                    rg = ws.Cells[indexRg, indexYIni, indexXIni - 1, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    // Aplicar fondo gris a la antepenúltima columna (indexYIni + 5)
                    ws.Cells[indexRg, indexYIni + 5, indexXIni - 1, indexYIni + 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[indexRg, indexYIni + 5, indexXIni - 1, indexYIni + 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                //Formato FormatoD1-C
                FormatoD1CDTO formatoD1CDTO = proyectoModel.FormatoD1CDTO;
                ws = xlPackage.Workbook.Worksheets["FormatoD1-C"];
                if (formatoD1CDTO != null)
                {
                    if (formatoD1CDTO.PlanReducir == "S")
                    {
                        ws.Cells[5, 18].Value = "X";
                    }
                    else {
                        ws.Cells[5, 21].Value = "X";
                    }
                    if (formatoD1CDTO.Alternativa == "Planea Autogenerar")
                    {
                        ws.Cells[6, 8].Value = "X";
                    }
                    else if (formatoD1CDTO.Alternativa == "Planea ajustar su demanda")
                    {
                        ws.Cells[6, 15].Value = "X";
                    }
                    else {
                        ws.Cells[6, 21].Value = formatoD1CDTO.Otro;
                    }
    
                    if (proyectoModel.FormatoD1CDTO.ListaFormatoDe1CDet != null) {
                        var colHora = 30;
                        foreach (var detRegHoja in proyectoModel.FormatoD1CDTO.ListaFormatoDe1CDet)
                        {
                            if (detRegHoja.Hora != null)
                            {
                                int matchedIndex = -1;
                                for (int r = 1; r <= ws.Dimension.End.Row; r++)
                                {
                                    var valorCelda = ws.Cells[r, colHora].Value;
                                    if (valorCelda != null)
                                    {
                                        DateTime hora;
                                        if (DateTime.TryParse(valorCelda.ToString(), out hora)) 
                                        {
                                            string horaFormateada = hora.ToString("HH:mm");
                                            if (horaFormateada == detRegHoja.Hora.ToString())
                                            {
                                                matchedIndex = r;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (matchedIndex != -1)
                                {
                                    ws.Cells[matchedIndex, colHora + 1].Value = detRegHoja.Demanda;
                                    ws.Cells[matchedIndex, colHora + 2].Value = detRegHoja.Generacion;
                                }
                            }
                        }
                    }
                }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato FormatoD1-D
                List<FormatoD1DDTO> listaFormatoDs = proyectoModel.listaFormatoDs;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                ws = xlPackage.Workbook.Worksheets["FormatoD1-D"];
                if (listaFormatoDs != null)
                {
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexIni = 7;
                    var indexIniY = 2;
                    var indexAnioA = indexIniY + 2;
                    var indexCatA = indexIni + 3;
                    var indexTrimA = indexIniY + 1;

                    ws.Cells[indexIni, indexIniY + 1].Value = $"Año {anioPeriodo - 1} ó antes";
                    for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Value = anio;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 10;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Value = "TRIMESTRE";
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 8;
                        for (int t = 1; t <= 4; t++)
                        {
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Value = t;
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Style.Font.Size = 8;
                            ws.Column(indexAnioA + t - 1).Width = 3;
                        }
                        indexAnioA = indexAnioA + 4;
                    }

                    if (dataCatalogoDTOs != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                        {
                            ws.Cells[indexCatA, indexIniY].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexCatA, indexIniY].Style.WrapText = true;
                            indexCatA++;
                        }
                    }
                    if (listaFormatoDs != null)
                    {
                        foreach (var detRegHoja in listaFormatoDs)
                        {
                            var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + indexIni + 3;
                                int matchTrim;
                                if (detRegHoja.Anio == "0")
                                {
                                    matchTrim = indexTrimA;
                                }
                                else
                                {
                                    matchTrim = indexTrimA + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                }
                                ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                            }
                        }
                    }
                    rg = ws.Cells[indexIni, indexIniY, indexIni + 2, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                    rg = ws.Cells[indexIni + 3, indexIniY, indexCatA - 1, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg = ws.Cells[indexIni + 3, indexIniY + 1, indexCatA - 1, indexAnioA - 1];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.Font.Size = 10;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Generacion Distribuida en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoGeneracionDistribuida(ProyectoModel proyectoModel, string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                         ruta,
                         ConstantesCampanias.FolderTemp,
                         identificadorUnico,
                         ConstantesCampanias.FolderFichasGeneracionDistribuida,
                         proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                     );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasGeneracionDistribuida, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionDistribuida));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionDistribuida));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }
            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelGeneracionDistribuida));
            }
            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato CC.GD.-A
                CCGDADTO ccgdaDTO = proyectoModel.CcgdaDTO;
                ws = xlPackage.Workbook.Worksheets["CC.GD.-A"];
                if (ccgdaDTO != null)
                {
                    ws.Cells[10, 5].Value = ccgdaDTO.NombreUnidad;
                    if (proyectoModel.ubicacionDTO != null)
                    {
                        ws.Cells[11, 5].Value = proyectoModel.ubicacionDTO.Departamento;
                        ws.Cells[12, 5].Value = proyectoModel.ubicacionDTO.Provincia;
                        ws.Cells[13, 5].Value = proyectoModel.ubicacionDTO.Distrito;
                    }
                    ws.Cells[14, 5].Value = ccgdaDTO.NombreDistribuidor;
                    ws.Cells[15, 5].Value = ccgdaDTO.Propietario;
                    ws.Cells[16, 5].Value = ccgdaDTO.SocioOperador;
                    ws.Cells[17, 5].Value = ccgdaDTO.SocioInversionista;
                    ws.Cells[19, 5].Value = ccgdaDTO.ObjetivoProyecto;
                    ws.Cells[20, 5].Value = ccgdaDTO.OtroObjetivo;//si marco otro cual
                    ws.Cells[22, 5].Value = ccgdaDTO.IncluidoPlanTrans == "S" ? "Sí" : ccgdaDTO.IncluidoPlanTrans == "N" ? "No" : "";
                    ws.Cells[24, 5].Value = ccgdaDTO.EstadoOperacion;
                    ws.Cells[25, 5].Value = ccgdaDTO.CargaRedDistribucion == "S" ? "Sí" : ccgdaDTO.CargaRedDistribucion == "N" ? "No" : "";
                    ws.Cells[33, 5].Value = ccgdaDTO.ConexionTemporal == "S" ? "Si" : ccgdaDTO.ConexionTemporal == "N" ? "No" : "";
                    ws.Cells[34, 5].Value = ccgdaDTO.TipoTecnologia;
                    ws.Cells[33, 9].Value = ccgdaDTO.FechaAdjudicactem;
                    ws.Cells[34, 9].Value = ccgdaDTO.FechaAdjutitulo;
                    ws.Cells[38, 5].Value = ccgdaDTO.Perfil;
                    ws.Cells[39, 5].Value = ccgdaDTO.Prefactibilidad;
                    ws.Cells[40, 5].Value = ccgdaDTO.Factibilidad;
                    ws.Cells[41, 5].Value = ccgdaDTO.EstDefinitivo;
                    ws.Cells[42, 5].Value = ccgdaDTO.Eia;
                    ws.Cells[38, 9].Value = ccgdaDTO.FechaInicioConst;
                    ws.Cells[39, 9].Value = ccgdaDTO.PeriodoConst;
                    ws.Cells[40, 9].Value = ccgdaDTO.FechaOpeComercial;
                    ws.Cells[48, 5].Value = ccgdaDTO.PotInstalada;
                    ws.Cells[49, 5].Value = ccgdaDTO.RecursoUsada;
                    ws.Cells[50, 5].Value = ccgdaDTO.Tecnologia;
                    ws.Cells[51, 5].Value = ccgdaDTO.TecOtro;//si marco otro indicar
                    ws.Cells[52, 5].Value = ccgdaDTO.BarraConexion;
                    ws.Cells[53, 5].Value = ccgdaDTO.NivelTension;
                    ws.Cells[62, 5].Value = ccgdaDTO.NombreProyectoGD;
                    if (proyectoModel.ubicacionDTO2 != null)
                    {
                        ws.Cells[67, 5].Value = proyectoModel.ubicacionDTO2.Departamento;
                        ws.Cells[68, 5].Value = proyectoModel.ubicacionDTO2.Provincia;
                        ws.Cells[69, 5].Value = proyectoModel.ubicacionDTO2.Distrito;
                    }
                    ws.Cells[60, 5].Value = ccgdaDTO.IncluidoPlanTransGD == "S" ? "Sí" : ccgdaDTO.IncluidoPlanTransGD == "N" ? "No" : "";
                    ws.Cells[70, 5].Value = ccgdaDTO.NomDistribuidorGD;
                    ws.Cells[71, 5].Value = ccgdaDTO.PropietarioGD;
                    ws.Cells[72, 5].Value = ccgdaDTO.SocioOperadorGD;
                    ws.Cells[73, 5].Value = ccgdaDTO.SocioInversionistaGD;
                    ws.Cells[75, 5].Value = ccgdaDTO.EstadoOperacionGD;
                    ws.Cells[76, 5].Value = ccgdaDTO.CargaRedDistribucionGD == "S" ? "Sí" : ccgdaDTO.CargaRedDistribucionGD == "N" ? "No" : "" ;
                    ws.Cells[83, 5].Value = ccgdaDTO.BarraConexionGD;
                    ws.Cells[84, 5].Value = ccgdaDTO.NivelTensionGD;
                    ws.Cells[88, 3].Value = ccgdaDTO.Comentarios;

                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.GD.-B
                List<CCGDBDTO> ccgdbdtos = proyectoModel.Ccgdbdtos;
                ws = xlPackage.Workbook.Worksheets["CC.GD-B"];
                if (ccgdbdtos != null)
                {
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var horizonteInicio = anioPeriodo - proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAtras;
                    var indexIni = 17;
                    var indexXIni = 17;
                    var indexRg = 17;
                    var indexYIni = 3;
                    var meses = new string[]
                    {
                        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                        "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
                    };
                    for (int anio = horizonteInicio; anio <= horizonteFin; anio++)
                    {
                        var rangoAnio = ws.Cells[indexIni, indexYIni, indexIni + 11, indexYIni];
                        rangoAnio.Merge = true;
                        rangoAnio.Value = anio;
                        rangoAnio.Style.Font.Bold = true;
                        rangoAnio.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        rangoAnio.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Merge = true;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Value = "TOTAL " + anio;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Style.Font.Bold = true;
                        rg = ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 7];
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

                        int index = 0;
                        foreach (var str in meses)
                        {
                            ws.Cells[indexIni + index, indexYIni + 1].Value = str;
                            ws.Cells[indexIni + index, indexYIni + 1].Style.Font.Bold = true;
                            index++;
                        };
                        indexIni = indexIni + 13;
                    }

                    if (ccgdbdtos != null)
                    {
                        foreach (var detRegHoja in ccgdbdtos)
                        {
                            int filaAnio = -1;
                            for (int r = indexXIni; r <= indexIni; r++)
                            {
                                var valorAnio = ws.Cells[r, indexYIni].Value;
                                if (valorAnio != null && valorAnio.ToString() == detRegHoja.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            int filaMes = -1;
                            for (int r = indexXIni; r <= indexXIni + 11; r++)
                            {
                                var valorMes = ws.Cells[r, indexYIni + 1].Value;
                                if (valorMes != null && valorMes.ToString() == detRegHoja.Mes.ToString())
                                {
                                    filaMes = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1 && filaMes != -1)
                            {
                                var indexX = filaAnio + filaMes - indexXIni;

                                ws.Cells[indexX, indexYIni + 2].Value = detRegHoja.DemandaEnergia;
                                ws.Cells[indexX, indexYIni + 3].Value = detRegHoja.DemandaHP;
                                ws.Cells[indexX, indexYIni + 4].Value = detRegHoja.DemandaHFP;
                                ws.Cells[indexX, indexYIni + 5].Value = detRegHoja.GeneracionEnergia;
                                ws.Cells[indexX, indexYIni + 6].Value = detRegHoja.GeneracionHP;
                                ws.Cells[indexX, indexYIni + 7].Value = detRegHoja.GeneracionHFP;
                                ws.Cells[indexX, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            }
                        }
                        for (int anio = horizonteInicio; anio <= horizonteFin; anio++)
                        {
                            double maxDemandaHP = 0.0;
                            double maxDemandaHFP = 0.0;
                            double maxGeneracionHP = 0.0;
                            double maxGeneracionHFP = 0.0;

                            double totalDemandaEnergia = 0.0;
                            double totalGeneracionEnergia = 0.0;

                            for (int mesIndex = 0; mesIndex < 12; mesIndex++)
                            {
                                var rw = indexXIni + mesIndex;
                                double valorDemandaEnergia = 0;
                                double valorGeneracionEnergia = 0;
                                double valorDemandaHP = 0;
                                double valorDemandaHFP = 0;
                                double valorGeneracionHP = 0;
                                double valorGeneracionHFP = 0;
                                if (ws.Cells[rw, indexYIni + 2].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 2].Value.ToString(), out valorDemandaEnergia))
                                    totalDemandaEnergia += valorDemandaEnergia;
                                if (ws.Cells[rw, indexYIni + 5].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 5].Value.ToString(), out valorGeneracionEnergia))
                                    totalGeneracionEnergia += valorGeneracionEnergia;
                                if (ws.Cells[rw, indexYIni + 3].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 3].Value.ToString(), out valorDemandaHP))
                                    maxDemandaHP = Math.Max(maxDemandaHP, valorDemandaHP);
                                if (ws.Cells[rw, indexYIni + 4].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 4].Value.ToString(), out valorDemandaHFP))
                                    maxDemandaHFP = Math.Max(maxDemandaHFP, valorDemandaHFP);
                                if (ws.Cells[rw, indexYIni + 6].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 6].Value.ToString(), out valorGeneracionHP))
                                    maxGeneracionHP = Math.Max(maxGeneracionHP, valorGeneracionHP);
                                if (ws.Cells[rw, indexYIni + 7].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 7].Value.ToString(), out valorGeneracionHFP))
                                    maxGeneracionHFP = Math.Max(maxGeneracionHFP, valorGeneracionHFP);
                            }

                            ws.Cells[indexXIni + 12, indexYIni + 2].Value = totalDemandaEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 3].Value = maxDemandaHP;
                            ws.Cells[indexXIni + 12, indexYIni + 4].Value = maxDemandaHFP;
                            ws.Cells[indexXIni + 12, indexYIni + 5].Value = totalGeneracionEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 6].Value = maxGeneracionHP;
                            ws.Cells[indexXIni + 12, indexYIni + 7].Value = maxGeneracionHFP;

                            ws.Cells[indexXIni + 12, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            indexXIni = indexXIni + 13;
                        }
                    }
                    rg = ws.Cells[indexRg, indexYIni, indexXIni - 1, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.GD.-C
                List<CCGDCOptDTO> cCGDCOptDTOs = proyectoModel.CCGDCOptDTOs;
                List<CCGDCPesDTO> cCGDCPesDTOs = proyectoModel.CCGDCPesDTOs;
                ws = xlPackage.Workbook.Worksheets["CC.GD-C"];
                if (cCGDCOptDTOs != null || cCGDCPesDTOs != null)
                {
                    // Tabla 1 
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexIni = 15;
                    var indexXIni = 15;
                    var indexYIni = 3;
                    var indexRg = 15;
                    var anioIni = 2018;
                    var meses = new string[]
                    {
                        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                        "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
                    };

                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Merge = true;
                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Value = "Año";
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni + 2, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni + 2, indexYIni + 1].Value = "Mes";
                    ws.Cells[indexXIni, indexYIni + 2, indexXIni, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 2, indexXIni, indexYIni + 4].Value = "DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA\r\nASOCIADA AL PROYECTO DE GENERACIÓN DISTRIBUIDA";
                    double currentHeight = ws.Row(indexXIni).Height; 
                    ws.Row(indexXIni).Height = currentHeight * 2;
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 2, indexYIni + 2].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 2, indexYIni + 2].Value = "ENERGÍA (GWH)";
                    ws.Cells[indexXIni + 1, indexYIni + 3, indexXIni + 1, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 3, indexXIni + 1, indexYIni + 4].Value = "POTENCIA (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 3].Value = "HP (3)";
                    ws.Cells[indexXIni + 2, indexYIni + 4].Value = "HFP (4)";
                    rg = ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni + 4];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Value = "GENERACIÓN ESTIMADA DE PROYECTO DE\r\nGENERACIÓN DISTRIBUIDA";
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Value = "ENERGÍA (GWH)";
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Value = "POTENCIA (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 6].Value = "HP (3)";
                    ws.Cells[indexXIni + 2, indexYIni + 7].Value = "HFP (4)";
                    rg = ws.Cells[indexXIni, indexYIni + 5, indexXIni + 2, indexYIni + 7];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    indexIni += 3;
                    indexXIni = indexIni;
                    for (int anio = anioIni; anio <= horizonteFin; anio++)
                    {
                        var rangoAnio = ws.Cells[indexIni, indexYIni, indexIni + 11, indexYIni];
                        rangoAnio.Merge = true;
                        rangoAnio.Value = anio;
                        rangoAnio.Style.Font.Bold = true;
                        rangoAnio.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        rangoAnio.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Merge = true;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Value = "TOTAL " + anio;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Style.Font.Bold = true;
                        rg = ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 7];
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

                        int index = 0;
                        foreach (var str in meses)
                        {
                            ws.Cells[indexIni + index, indexYIni + 1].Value = str;
                            ws.Cells[indexIni + index, indexYIni + 1].Style.Font.Bold = true;
                            index++;
                        };
                        indexIni = indexIni + 13;
                    }

                    if (cCGDCOptDTOs != null)
                    {
                        foreach (var detRegHoja in cCGDCOptDTOs)
                        {
                            int filaAnio = -1;
                            for (int r = indexXIni; r <= indexIni; r++)
                            {
                                var valorAnio = ws.Cells[r, indexYIni].Value;
                                if (valorAnio != null && valorAnio.ToString() == detRegHoja.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            int filaMes = -1;
                            for (int r = indexXIni; r <= indexXIni + 11; r++)
                            {
                                var valorMes = ws.Cells[r, indexYIni + 1].Value;
                                if (valorMes != null && valorMes.ToString() == detRegHoja.Mes.ToString())
                                {
                                    filaMes = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1 && filaMes != -1)
                            {
                                var indexX = filaAnio + filaMes - indexXIni;

                                ws.Cells[indexX, indexYIni + 2].Value = detRegHoja.DemandaEnergia;
                                ws.Cells[indexX, indexYIni + 3].Value = detRegHoja.DemandaHP;
                                ws.Cells[indexX, indexYIni + 4].Value = detRegHoja.DemandaHFP;
                                ws.Cells[indexX, indexYIni + 5].Value = detRegHoja.GeneracionEnergia;
                                ws.Cells[indexX, indexYIni + 6].Value = detRegHoja.GeneracionHP;
                                ws.Cells[indexX, indexYIni + 7].Value = detRegHoja.GeneracionHFP;
                                ws.Cells[indexX, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                                rg = ws.Cells[indexX, indexYIni + 2, indexX, indexYIni + 7];
                                rg.Style.Font.Bold = false;
                            }
                        }
                        for (int anio = anioIni; anio <= horizonteFin; anio++)
                        {
                            double maxDemandaHP = 0.0;
                            double maxDemandaHFP = 0.0;
                            double maxGeneracionHP = 0.0;
                            double maxGeneracionHFP = 0.0;

                            double totalDemandaEnergia = 0.0;
                            double totalGeneracionEnergia = 0.0;

                            for (int mesIndex = 0; mesIndex < 12; mesIndex++)
                            {
                                var rw = indexXIni + mesIndex;
                                double valorDemandaEnergia = 0;
                                double valorGeneracionEnergia = 0;
                                double valorDemandaHP = 0;
                                double valorDemandaHFP = 0;
                                double valorGeneracionHP = 0;
                                double valorGeneracionHFP = 0;
                                if (ws.Cells[rw, indexYIni + 2].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 2].Value.ToString(), out valorDemandaEnergia))
                                    totalDemandaEnergia += valorDemandaEnergia;
                                if (ws.Cells[rw, indexYIni + 5].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 5].Value.ToString(), out valorGeneracionEnergia))
                                    totalGeneracionEnergia += valorGeneracionEnergia;
                                if (ws.Cells[rw, indexYIni + 3].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 3].Value.ToString(), out valorDemandaHP))
                                    maxDemandaHP = Math.Max(maxDemandaHP, valorDemandaHP);
                                if (ws.Cells[rw, indexYIni + 4].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 4].Value.ToString(), out valorDemandaHFP))
                                    maxDemandaHFP = Math.Max(maxDemandaHFP, valorDemandaHFP);
                                if (ws.Cells[rw, indexYIni + 6].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 6].Value.ToString(), out valorGeneracionHP))
                                    maxGeneracionHP = Math.Max(maxGeneracionHP, valorGeneracionHP);
                                if (ws.Cells[rw, indexYIni + 7].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 7].Value.ToString(), out valorGeneracionHFP))
                                    maxGeneracionHFP = Math.Max(maxGeneracionHFP, valorGeneracionHFP);
                            }

                            ws.Cells[indexXIni + 12, indexYIni + 2].Value = totalDemandaEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 3].Value = maxDemandaHP;
                            ws.Cells[indexXIni + 12, indexYIni + 4].Value = maxDemandaHFP;
                            ws.Cells[indexXIni + 12, indexYIni + 5].Value = totalGeneracionEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 6].Value = maxGeneracionHP;
                            ws.Cells[indexXIni + 12, indexYIni + 7].Value = maxGeneracionHFP;

                            ws.Cells[indexXIni + 12, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            indexXIni = indexXIni + 13;
                        }
                    }

                    rg = ws.Cells[indexRg, indexYIni, indexXIni - 1, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.WrapText = true;

                    ws.Cells[indexXIni + 2, indexYIni].Value = "1.2. Proyección de generación distribuida y demanda de la carga asociada en el escenario Pesimista";

                    indexXIni += 4;
                    indexIni = indexXIni;
                    indexRg = indexXIni;

                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Merge = true;
                    ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni].Value = "Año";
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni + 2, indexYIni + 1].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 1, indexXIni + 2, indexYIni + 1].Value = "Mes";
                    ws.Cells[indexXIni, indexYIni + 2, indexXIni, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 2, indexXIni, indexYIni + 4].Value = "DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA\r\nASOCIADA AL PROYECTO DE GENERACIÓN DISTRIBUIDA";
                    currentHeight = ws.Row(indexXIni).Height;
                    ws.Row(indexXIni).Height = currentHeight * 2;
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 2, indexYIni + 2].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 2, indexXIni + 2, indexYIni + 2].Value = "ENERGÍA (GWH)";
                    ws.Cells[indexXIni + 1, indexYIni + 3, indexXIni + 1, indexYIni + 4].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 3, indexXIni + 1, indexYIni + 4].Value = "POTENCIA (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 3].Value = "HP (3)";
                    ws.Cells[indexXIni + 2, indexYIni + 4].Value = "HFP (4)";
                    rg = ws.Cells[indexXIni, indexYIni, indexXIni + 2, indexYIni + 4];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni, indexYIni + 5, indexXIni, indexYIni + 7].Value = "GENERACIÓN ESTIMADA DE PROYECTO DE\r\nGENERACIÓN DISTRIBUIDA";
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 5, indexXIni + 2, indexYIni + 5].Value = "ENERGÍA (GWH)";
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Merge = true;
                    ws.Cells[indexXIni + 1, indexYIni + 6, indexXIni + 1, indexYIni + 7].Value = "POTENCIA (MW)";
                    ws.Cells[indexXIni + 2, indexYIni + 6].Value = "HP (3)";
                    ws.Cells[indexXIni + 2, indexYIni + 7].Value = "HFP (4)";
                    rg = ws.Cells[indexXIni, indexYIni + 5, indexXIni + 2, indexYIni + 7];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    indexIni += 3;
                    indexXIni = indexIni;
                    for (int anio = anioIni; anio <= horizonteFin; anio++)
                    {
                        var rangoAnio = ws.Cells[indexIni, indexYIni, indexIni + 11, indexYIni];
                        rangoAnio.Merge = true;
                        rangoAnio.Value = anio;
                        rangoAnio.Style.Font.Bold = true;
                        rangoAnio.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        rangoAnio.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Merge = true;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Value = "TOTAL " + anio;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Style.Font.Bold = true;
                        rg = ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 7];
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

                        int index = 0;
                        foreach (var str in meses)
                        {
                            ws.Cells[indexIni + index, indexYIni + 1].Value = str;
                            ws.Cells[indexIni + index, indexYIni + 1].Style.Font.Bold = true;
                            index++;
                        };
                        indexIni = indexIni + 13;
                    }

                    if (cCGDCPesDTOs != null)
                    {
                        foreach (var detRegHoja in cCGDCPesDTOs)
                        {
                            int filaAnio = -1;
                            for (int r = indexXIni; r <= indexIni; r++)
                            {
                                var valorAnio = ws.Cells[r, indexYIni].Value;
                                if (valorAnio != null && valorAnio.ToString() == detRegHoja.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            int filaMes = -1;
                            for (int r = indexXIni; r <= indexXIni + 11; r++)
                            {
                                var valorMes = ws.Cells[r, indexYIni + 1].Value;
                                if (valorMes != null && valorMes.ToString() == detRegHoja.Mes.ToString())
                                {
                                    filaMes = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1 && filaMes != -1)
                            {
                                var indexX = filaAnio + filaMes - indexXIni;

                                ws.Cells[indexX, indexYIni + 2].Value = detRegHoja.DemandaEnergia;
                                ws.Cells[indexX, indexYIni + 3].Value = detRegHoja.DemandaHP;
                                ws.Cells[indexX, indexYIni + 4].Value = detRegHoja.DemandaHFP;
                                ws.Cells[indexX, indexYIni + 5].Value = detRegHoja.GeneracionEnergia;
                                ws.Cells[indexX, indexYIni + 6].Value = detRegHoja.GeneracionHP;
                                ws.Cells[indexX, indexYIni + 7].Value = detRegHoja.GeneracionHFP;
                                ws.Cells[indexX, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                                rg = ws.Cells[indexX, indexYIni + 2, indexX, indexYIni + 7];
                                rg.Style.Font.Bold = false;
                            }
                        }
                        for (int anio = anioIni; anio <= horizonteFin; anio++)
                        {
                            double maxDemandaHP = 0.0;
                            double maxDemandaHFP = 0.0;
                            double maxGeneracionHP = 0.0;
                            double maxGeneracionHFP = 0.0;

                            double totalDemandaEnergia = 0.0;
                            double totalGeneracionEnergia = 0.0;

                            for (int mesIndex = 0; mesIndex < 12; mesIndex++)
                            {
                                var rw = indexXIni + mesIndex;
                                double valorDemandaEnergia = 0;
                                double valorGeneracionEnergia = 0;
                                double valorDemandaHP = 0;
                                double valorDemandaHFP = 0;
                                double valorGeneracionHP = 0;
                                double valorGeneracionHFP = 0;
                                if (ws.Cells[rw, indexYIni + 2].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 2].Value.ToString(), out valorDemandaEnergia))
                                    totalDemandaEnergia += valorDemandaEnergia;
                                if (ws.Cells[rw, indexYIni + 5].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 5].Value.ToString(), out valorGeneracionEnergia))
                                    totalGeneracionEnergia += valorGeneracionEnergia;
                                if (ws.Cells[rw, indexYIni + 3].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 3].Value.ToString(), out valorDemandaHP))
                                    maxDemandaHP = Math.Max(maxDemandaHP, valorDemandaHP);
                                if (ws.Cells[rw, indexYIni + 4].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 4].Value.ToString(), out valorDemandaHFP))
                                    maxDemandaHFP = Math.Max(maxDemandaHFP, valorDemandaHFP);
                                if (ws.Cells[rw, indexYIni + 6].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 6].Value.ToString(), out valorGeneracionHP))
                                    maxGeneracionHP = Math.Max(maxGeneracionHP, valorGeneracionHP);
                                if (ws.Cells[rw, indexYIni + 7].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 7].Value.ToString(), out valorGeneracionHFP))
                                    maxGeneracionHFP = Math.Max(maxGeneracionHFP, valorGeneracionHFP);
                            }

                            ws.Cells[indexXIni + 12, indexYIni + 2].Value = totalDemandaEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 3].Value = maxDemandaHP;
                            ws.Cells[indexXIni + 12, indexYIni + 4].Value = maxDemandaHFP;
                            ws.Cells[indexXIni + 12, indexYIni + 5].Value = totalGeneracionEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 6].Value = maxGeneracionHP;
                            ws.Cells[indexXIni + 12, indexYIni + 7].Value = maxGeneracionHFP;

                            ws.Cells[indexXIni + 12, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            indexXIni = indexXIni + 13;
                        }
                    }

                    rg = ws.Cells[indexRg, indexYIni, indexXIni - 1, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                    rg.Style.WrapText = true;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.GD.-D
                List<CCGDDDTO> cgdddtos = proyectoModel.Ccgdddtos;
                ws = xlPackage.Workbook.Worksheets["CC.GD-D"];
                if (cgdddtos != null)
                {
                    var colHora = 31;
                    foreach (var detRegHoja in cgdddtos)
                    {
                        if (detRegHoja.Hora != null)
                        {
                            int matchedIndex = -1;
                            for (int r = 1; r <= ws.Dimension.End.Row; r++)
                            {
                                var valorCelda = ws.Cells[r, colHora].Value;
                                if (valorCelda != null)
                                {
                                    DateTime hora;
                                    if (DateTime.TryParse(valorCelda.ToString(), out hora))
                                    {
                                        string horaFormateada = hora.ToString("HH:mm");
                                        if (horaFormateada == detRegHoja.Hora.ToString())
                                        {
                                            matchedIndex = r;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (matchedIndex != -1)
                            {
                                ws.Cells[matchedIndex, colHora + 1].Value = detRegHoja.Demanda;
                                ws.Cells[matchedIndex, colHora + 2].Value = detRegHoja.Generacion;
                            }
                        }
                    }

                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.GD.-E
                CCGDEDTO ccgdeDTO = proyectoModel.CcgdeDTO;
                ws = xlPackage.Workbook.Worksheets["CC.GD-E"];
                if (ccgdeDTO != null)
                {
                    ws.Cells[11, 3].Value = ccgdeDTO.Estudiofactibilidad;
                    ws.Cells[11, 5].Value = ccgdeDTO.Investigacionescampo;
                    ws.Cells[11, 7].Value = ccgdeDTO.Gestionesfinancieras;
                    ws.Cells[11, 9].Value = ccgdeDTO.Disenospermisos;
                    ws.Cells[15, 3].Value = ccgdeDTO.Obrasciviles;
                    ws.Cells[15, 5].Value = ccgdeDTO.Equipamiento;
                    ws.Cells[15, 8].Value = ccgdeDTO.Lineatransmision;
                    //ws.Cells[,].Value = ccgdeDTO.Obrasregulacion;
                    ws.Cells[19, 3].Value = ccgdeDTO.Administracion;
                    ws.Cells[19, 5].Value = ccgdeDTO.Aduanas;
                    ws.Cells[19, 7].Value = ccgdeDTO.Supervision;
                    ws.Cells[19, 9].Value = ccgdeDTO.Gastosgestion;
                    ws.Cells[23, 3].Value = ccgdeDTO.Imprevistos;
                    ws.Cells[23, 5].Value = ccgdeDTO.Igv;
                    ws.Cells[23, 8].Value = ccgdeDTO.Otrosgastos;
                    ws.Cells[27, 3].Value = ccgdeDTO.Inversiontotalsinigv;
                    ws.Cells[27, 7].Value = ccgdeDTO.Inversiontotalconigv;
                    ws.Cells[31, 3].Value = ccgdeDTO.Financiamientotipo;
                    ws.Cells[31, 6].Value = ccgdeDTO.Financiamientoestado;
                    ws.Cells[31, 9].Value = ccgdeDTO.Porcentajefinanciado / 100;
                    ws.Cells[35, 3].Value = ccgdeDTO.Concesiondefinitiva;
                    ws.Cells[35, 5].Value = ccgdeDTO.Ventaenergia;
                    ws.Cells[35, 7].Value = ccgdeDTO.Ejecucionobra;
                    ws.Cells[35, 9].Value = ccgdeDTO.Contratosfinancieros;
                    ws.Cells[38, 3].Value = ccgdeDTO.Observaciones;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato CC.GD-F
                List<CCGDFDTO> ccgdfDTO = proyectoModel.CcgdfDTOs;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                ws = xlPackage.Workbook.Worksheets["CC.GD-F"];
                if (ccgdfDTO != null)
                {
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexIni = 10;
                    var indexIniY = 3;
                    var indexAnioA = indexIniY + 2;
                    var indexCatA = indexIni + 3;
                    var indexTrimA = indexIniY + 1;

                    ws.Cells[indexIni, indexIniY + 1].Value = $"Año {anioPeriodo - 1} ó antes";
                    for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Value = anio;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 10;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Value = "TRIMESTRE";
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 8;
                        for (int t = 1; t <= 4; t++)
                        {
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Value = t;
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Style.Font.Size = 8;
                            ws.Column(indexAnioA + t - 1).Width = 3;
                        }
                        indexAnioA = indexAnioA + 4;
                    }
                    if (dataCatalogoDTOs != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                        {
                            ws.Cells[indexCatA, indexIniY].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexCatA, indexIniY].Style.WrapText = true;
                            indexCatA++;
                        }
                    }
                    if (ccgdfDTO != null)
                    {
                        foreach (var detRegHoja in ccgdfDTO)
                        {
                            var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + indexIni + 3;
                                int matchTrim;
                                if (detRegHoja.Anio == "0")
                                {
                                    matchTrim = indexTrimA;
                                }
                                else
                                {
                                    matchTrim = indexTrimA + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                }
                                ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                            }
                        }
                    }
                    rg = ws.Cells[indexIni, indexIniY, indexIni + 2, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.WrapText = true;
                    rg.Style.Font.Bold = true;
                    rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                    rg = ws.Cells[indexIni + 3, indexIniY, indexCatA - 1, indexAnioA - 1];
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg = ws.Cells[indexIni + 3, indexIniY + 1, indexCatA - 1, indexAnioA - 1];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Font.Bold = true;
                    rg.Style.WrapText = true;
                    rg.Style.Font.Size = 10;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Genera Ficha Proyecto Tipo Hidrogeno Verde en formato excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarExcelFichaProyectoTipoHidrogenoVerde(ProyectoModel proyectoModel,string EmpresaNom,string Proynombre,string identificadorUnico)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(
                         ruta,
                         ConstantesCampanias.FolderTemp,
                         identificadorUnico,
                         ConstantesCampanias.FolderFichasHidrogenoVerde,
                         proyectoModel.TransmisionProyectoDTO.Proynombre + "-" + proyectoModel.TransmisionProyectoDTO.Proycodi
                     );
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderFichasHidrogenoVerde, FormatoArchivosExcelCampanias.NombreFichaExcelHidrogenoVerde));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelHidrogenoVerde));
            if (!Directory.Exists(pathNewFile))
            {
                Directory.CreateDirectory(pathNewFile);
            }
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }
            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreFichaExcelHidrogenoVerde));
            }
            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ExcelRange rg = null;
                // Formato P. H2V.-A
                CuestionarioH2VADTO cuestionarioH2VADTO = proyectoModel.CuestionarioH2VADTO;
                List<DataCatalogoDTO> dataCatalogoDTOs2 = proyectoModel.DataCatalogoDTOs2;
                ws = xlPackage.Workbook.Worksheets["P. H2V.-A"];
                if (cuestionarioH2VADTO != null)
                {
                    if (cuestionarioH2VADTO.ProyNp == "NP")
                    {
                        var textBox = ws.Drawings["TextBox 1"] as ExcelShape;
                        if (textBox != null)
                        {
                            textBox.Text = "X";
                        }
                    }
                    else if (cuestionarioH2VADTO.ProyNp == "PP")
                    {
                        var textBox = ws.Drawings["TextBox 2"] as ExcelShape;
                        if (textBox != null)
                        {
                            textBox.Text = "X";
                        }
                    }
                    ws.Cells[15, 5].Value = Proynombre;
                    ws.Cells[18, 5].Value = EmpresaNom;
                    ws.Cells[21, 5].Value = cuestionarioH2VADTO.SocioOperador;
                    ws.Cells[24, 5].Value = cuestionarioH2VADTO.SocioInversionista;
                    if (proyectoModel.ubicacionDTO != null)
                    {
                        ws.Cells[28, 5].Value = proyectoModel.ubicacionDTO.Departamento;
                        ws.Cells[28, 7].Value = proyectoModel.ubicacionDTO.Provincia;
                        ws.Cells[28, 9].Value = proyectoModel.ubicacionDTO.Distrito;
                    }
                    ws.Cells[31, 5].Value = cuestionarioH2VADTO.ActDesarrollar;
                    ws.Cells[35, 5].Value = cuestionarioH2VADTO.SituacionAct;
                    ws.Cells[40, 9].Value = cuestionarioH2VADTO.TipoElectrolizador; //otro
                    if (!string.IsNullOrEmpty(cuestionarioH2VADTO.OtroElectrolizador))
                    {
                        ws.Cells[40, 9].Value = cuestionarioH2VADTO.OtroElectrolizador;
                    }
                    ws.Cells[41, 9].Value = cuestionarioH2VADTO.VidaUtil;
                    ws.Cells[42, 9].Value = cuestionarioH2VADTO.ProduccionAnual;
                    ws.Cells[43, 9].Value = cuestionarioH2VADTO.ObjetivoProyecto;
                    ws.Cells[44, 9].Value = cuestionarioH2VADTO.OtroObjetivo;//en caso haya marcado otros
                    ws.Cells[45, 9].Value = cuestionarioH2VADTO.UsoEsperadoHidro;
                    ws.Cells[46, 9].Value = cuestionarioH2VADTO.OtroUsoEsperadoHidro;//en caso haya marcado otros
                    ws.Cells[47, 9].Value = cuestionarioH2VADTO.MetodoTransH2;
                    ws.Cells[48, 9].Value = cuestionarioH2VADTO.OtroMetodoTransH2;//en caso haya marcado otros
                    ws.Cells[52, 5].Value = cuestionarioH2VADTO.PoderCalorifico;
                    ws.Cells[55, 8].Value = cuestionarioH2VADTO.SubEstacionSein;
                    ws.Cells[56, 8].Value = cuestionarioH2VADTO.NivelTension;
                    ws.Cells[57, 8].Value = cuestionarioH2VADTO.TipoSuministro;
                    ws.Cells[58, 8].Value = cuestionarioH2VADTO.OtroSuministro;//en caso haya marcado otros
                    ws.Cells[64, 7].Value = cuestionarioH2VADTO.PrimeraEtapa;
                    ws.Cells[64, 9].Value = cuestionarioH2VADTO.SegundaEtapa;
                    ws.Cells[64, 11].Value = cuestionarioH2VADTO.Final;
                    ws.Cells[73, 5].Value = cuestionarioH2VADTO.CostoProduccion;
                    ws.Cells[73, 7].Value = cuestionarioH2VADTO.PrecioVenta;
                    var indexX = 77;
                    var indexRgX = 77;
                    var indexY = 5;
                    // Tabla 1
                    if (cuestionarioH2VADTO.ListCH2VADet1DTOs != null) {
                        // Ordenar la lista por Anio
                        var listaOrdenada = cuestionarioH2VADTO.ListCH2VADet1DTOs.OrderBy(x => x.Anio).ToList();

                        foreach (var regHoja in listaOrdenada)
                        {
                            ws.Cells[indexX, indexY, indexX, indexY + 1].Merge = true;
                            ws.Cells[indexX, indexY, indexX, indexY + 1].Value = regHoja.Anio;
                            ws.Cells[indexX, indexY + 2].Value = regHoja.MontoInversion;
                            indexX++;
                        };
                        if (cuestionarioH2VADTO.ListCH2VADet1DTOs.Count > 0) {
                            rg = ws.Cells[indexRgX, indexY, indexX - 1, indexY + 2];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Font.Color.SetColor(Color.Black);
                        }
                    }

                    ws.Cells[indexX + 1, indexY - 2].Value = "3.3";
                    ws.Cells[indexX + 1, indexY].Value = "Financiamiento:";

                    rg = ws.Cells[indexX + 2, indexY, indexX + 2, indexY + 6];
                    rg.Merge = true;
                    rg.Value = cuestionarioH2VADTO.Financiamiento;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexX + 4, indexY - 3].Value = "4.0";
                    ws.Cells[indexX + 4, indexY - 3].Style.Font.Bold = true;
                    ws.Cells[indexX + 4, indexY].Value = "FACTORES QUE FAVORECEN LA EJECUCIÓN DEL PROYECTO:";
                    ws.Cells[indexX + 4, indexY].Style.Font.Bold = true;

                    rg = ws.Cells[indexX + 5, indexY, indexX + 6, indexY + 7];
                    rg.Merge = true;
                    rg.Value = cuestionarioH2VADTO.FactFavorecenProy;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexX + 8, indexY - 3].Value = "5.0";
                    ws.Cells[indexX + 8, indexY - 3].Style.Font.Bold = true;
                    ws.Cells[indexX + 8, indexY].Value = "FACTORES QUE DESFAVORECEN LA EJECUCIÓN DEL PROYECTO:";
                    ws.Cells[indexX + 8, indexY].Style.Font.Bold = true;

                    rg = ws.Cells[indexX + 9, indexY, indexX + 10, indexY + 7];
                    rg.Merge = true;
                    rg.Value = cuestionarioH2VADTO.FactDesfavorecenProy;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                    ws.Cells[indexX + 12, indexY - 3].Value = "6.0";
                    ws.Cells[indexX + 12, indexY - 3].Style.Font.Bold = true;
                    ws.Cells[indexX + 12, indexY].Value = "RESUMEN DE LA SITUACION DEL PROYECTO";
                    ws.Cells[indexX + 12, indexY].Style.Font.Bold = true;

                    indexX += 13;
                    indexRgX = indexX;

                    ws.Cells[indexX, indexY, indexX + 1, indexY + 2].Merge = true;
                    ws.Cells[indexX, indexY, indexX + 1, indexY + 2].Value = "REQUISITOS";
                    ws.Cells[indexX, indexY + 3, indexX, indexY + 7].Merge = true;
                    ws.Cells[indexX, indexY + 3, indexX, indexY + 7].Value = "ESTADO SITUACIONAL";
                    ws.Cells[indexX + 1, indexY + 3].Value = "En\r\nElaboración";
                    ws.Cells[indexX + 1, indexY + 4].Value = "Presentado";
                    ws.Cells[indexX + 1, indexY + 5].Value = "En trámite\r\n(Evaluación)";
                    ws.Cells[indexX + 1, indexY + 6].Value = "Aprobado/\r\nAutorizado";
                    ws.Cells[indexX + 1, indexY + 7].Value = "Firmado";
                    rg = ws.Cells[indexX, indexY, indexX + 1, indexY + 7];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                    rg.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    if (dataCatalogoDTOs2 != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs2)
                        {
                            ws.Cells[indexX + 2, indexY, indexX + 2, indexY + 2].Merge = true;
                            ws.Cells[indexX + 2, indexY, indexX + 2, indexY + 2].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexX + 2, indexY, indexX + 2, indexY + 2].Style.WrapText = true;
                            indexX++;
                        }
                        if (dataCatalogoDTOs2.Count > 0) {
                            rg = ws.Cells[indexRgX, indexY, indexX + 1, indexY + 7];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Font.Color.SetColor(Color.Black);
                        }
                    }
                    if ( cuestionarioH2VADTO.ListCH2VADet2DTOs != null)
                    {
                        foreach (var detRegHoja in  cuestionarioH2VADTO.ListCH2VADet2DTOs)
                        {
                            var matchedCatalogo = dataCatalogoDTOs2.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs2.IndexOf(matchedCatalogo) + indexRgX + 2;
                                ws.Cells[matchedIndex, indexY + 3].Value = detRegHoja.EnElaboracion == "true" ? "x" : "";
                                ws.Cells[matchedIndex, indexY + 4].Value = detRegHoja.Presentado == "true" ? "x" : "";
                                ws.Cells[matchedIndex, indexY + 5].Value = detRegHoja.EnTramite == "true" ? "x" : "";
                                ws.Cells[matchedIndex, indexY + 6].Value = detRegHoja.Aprobado == "true" ? "x" : "";
                                ws.Cells[matchedIndex, indexY + 7].Value = detRegHoja.Firmado == "true" ? "x" : "";
                            }
                        }
                    }

                    ws.Cells[indexX + 3, indexY - 3].Value = "7.0";
                    ws.Cells[indexX + 3, indexY - 3].Style.Font.Bold = true;
                    ws.Cells[indexX + 3, indexY].Value = "COMENTARIOS:";
                    ws.Cells[indexX + 3, indexY].Style.Font.Bold = true;

                    rg = ws.Cells[indexX + 4, indexY, indexX + 6, indexY + 7];
                    rg.Merge = true;
                    rg.Value = cuestionarioH2VADTO.Comentarios;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);

                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato P. H2V.-B
                CuestionarioH2VBDTO cuestionarioH2VBDTO = proyectoModel.CuestionarioH2VBDTO;
                ws = xlPackage.Workbook.Worksheets["P. H2V.-B"];
                if (cuestionarioH2VBDTO != null)
                {
                    ws.Cells[10, 5].Value = cuestionarioH2VBDTO.NombreUnidad;
                    if (proyectoModel.ubicacionDTO2 != null)
                    {
                        ws.Cells[11, 5].Value = proyectoModel.ubicacionDTO2.Departamento;
                        ws.Cells[12, 5].Value = proyectoModel.ubicacionDTO2.Provincia;
                        ws.Cells[13, 5].Value = proyectoModel.ubicacionDTO2.Distrito;
                    }
                    ws.Cells[14, 5].Value = cuestionarioH2VBDTO.Propietario;
                    ws.Cells[15, 5].Value = cuestionarioH2VBDTO.SocioOperador;
                    ws.Cells[16, 5].Value = cuestionarioH2VBDTO.SocioInversionista;
                    ws.Cells[17, 5].Value = cuestionarioH2VBDTO.IncluidoPlanTrans == "S" ? "Sí" : cuestionarioH2VBDTO.IncluidoPlanTrans == "N" ? "No" : ""; ;
                    ws.Cells[25, 5].Value = cuestionarioH2VBDTO.ConcesionTemporal == "S" ? "Si" : cuestionarioH2VBDTO.ConcesionTemporal == "N" ? "No" : ""; ;
                    ws.Cells[26, 5].Value = cuestionarioH2VBDTO.TipoElectrolizador;
                    ws.Cells[25, 9].Value = cuestionarioH2VBDTO.FechaConcesionTemporal;
                    ws.Cells[26, 9].Value = cuestionarioH2VBDTO.FechaTituloHabilitante;
                    ws.Cells[30, 5].Value = cuestionarioH2VBDTO.Perfil;
                    ws.Cells[31, 5].Value = cuestionarioH2VBDTO.Prefactibilidad;
                    ws.Cells[32, 5].Value = cuestionarioH2VBDTO.Factibilidad;
                    ws.Cells[33, 5].Value = cuestionarioH2VBDTO.EstudioDefinitivo;
                    ws.Cells[34, 5].Value = cuestionarioH2VBDTO.EIA;
                    ws.Cells[30, 9].Value = cuestionarioH2VBDTO.FechaInicioConstruccion;
                    ws.Cells[31, 9].Value = cuestionarioH2VBDTO.PeriodoConstruccion;
                    ws.Cells[32, 9].Value = cuestionarioH2VBDTO.FechaOperacionComercial;
                    ws.Cells[40, 5].Value = cuestionarioH2VBDTO.PotenciaInstalada;
                    ws.Cells[41, 5].Value = cuestionarioH2VBDTO.RecursoUsado;
                    ws.Cells[42, 5].Value = cuestionarioH2VBDTO.Tecnologia;
                    ws.Cells[43, 5].Value = cuestionarioH2VBDTO.OtroTecnologia;//en caso haya marcado otros
                    ws.Cells[44, 5].Value = cuestionarioH2VBDTO.BarraConexion;
                    ws.Cells[45, 5].Value = cuestionarioH2VBDTO.NivelTension;
                    ws.Cells[49, 3].Value = cuestionarioH2VBDTO.Comentarios;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato P. H2V.-C
                List<CuestionarioH2VCDTO> ch2vcDTOs = proyectoModel.Ch2vcDTOs;
                ws = xlPackage.Workbook.Worksheets["P. H2V.-C"];
                if (ch2vcDTOs != null)
                {
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var horizonteInicio = anioPeriodo - proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAtras;
                    var indexIni = 18;
                    var indexXIni = 18;
                    var indexRg = 18;
                    var indexYIni = 3;
                    var meses = new string[]
                    {
                        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                        "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
                    };
                    for (int anio = horizonteInicio; anio <= horizonteFin; anio++)
                    {
                        var rangoAnio = ws.Cells[indexIni, indexYIni, indexIni + 11, indexYIni];
                        rangoAnio.Merge = true;
                        rangoAnio.Value = anio;
                        rangoAnio.Style.Font.Bold = true;
                        rangoAnio.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        rangoAnio.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Merge = true;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Value = "TOTAL " + anio;
                        ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 1].Style.Font.Bold = true;
                        rg = ws.Cells[indexIni + 12, indexYIni, indexIni + 12, indexYIni + 7];
                        rg.Style.Font.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

                        int index = 0;
                        foreach (var str in meses)
                        {
                            ws.Cells[indexIni + index, indexYIni + 1].Value = str;
                            ws.Cells[indexIni + index, indexYIni + 1].Style.Font.Bold = true;
                            index++;
                        };
                        indexIni = indexIni + 13;
                    }

                    if (ch2vcDTOs != null)
                    {
                        foreach (var detRegHoja in ch2vcDTOs)
                        {
                            int filaAnio = -1;
                            for (int r = indexXIni; r <= indexIni; r++)
                            {
                                var valorAnio = ws.Cells[r, indexYIni].Value;
                                if (valorAnio != null && valorAnio.ToString() == detRegHoja.Anio.ToString())
                                {
                                    filaAnio = r;
                                    break;
                                }
                            }

                            int filaMes = -1;
                            for (int r = indexXIni; r <= indexXIni + 11; r++)
                            {
                                var valorMes = ws.Cells[r, indexYIni + 1].Value;
                                if (valorMes != null && valorMes.ToString() == detRegHoja.Mes.ToString())
                                {
                                    filaMes = r;
                                    break;
                                }
                            }

                            if (filaAnio != -1 && filaMes != -1)
                            {
                                var indexX = filaAnio + filaMes - indexXIni;

                                ws.Cells[indexX, indexYIni + 2].Value = detRegHoja.DemandaEnergia;
                                ws.Cells[indexX, indexYIni + 3].Value = detRegHoja.DemandaHP;
                                ws.Cells[indexX, indexYIni + 4].Value = detRegHoja.DemandaHFP;
                                ws.Cells[indexX, indexYIni + 5].Value = detRegHoja.GeneracionEnergia;
                                ws.Cells[indexX, indexYIni + 6].Value = detRegHoja.GeneracionHP;
                                ws.Cells[indexX, indexYIni + 7].Value = detRegHoja.GeneracionHFP;
                                ws.Cells[indexX, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                                ws.Cells[indexX, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            }
                        }
                        for (int anio = horizonteInicio; anio <= horizonteFin; anio++)
                        {
                            double maxDemandaHP = 0.0;
                            double maxDemandaHFP = 0.0;
                            double maxGeneracionHP = 0.0;
                            double maxGeneracionHFP = 0.0;

                            double totalDemandaEnergia = 0.0;
                            double totalGeneracionEnergia = 0.0;

                            for (int mesIndex = 0; mesIndex < 12; mesIndex++)
                            {
                                var rw = indexXIni + mesIndex;
                                double valorDemandaEnergia = 0;
                                double valorGeneracionEnergia = 0;
                                double valorDemandaHP = 0;
                                double valorDemandaHFP = 0;
                                double valorGeneracionHP = 0;
                                double valorGeneracionHFP = 0;
                                if (ws.Cells[rw, indexYIni + 2].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 2].Value.ToString(), out valorDemandaEnergia))
                                    totalDemandaEnergia += valorDemandaEnergia;
                                if (ws.Cells[rw, indexYIni + 5].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 5].Value.ToString(), out valorGeneracionEnergia))
                                    totalGeneracionEnergia += valorGeneracionEnergia;
                                if (ws.Cells[rw, indexYIni + 3].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 3].Value.ToString(), out valorDemandaHP))
                                    maxDemandaHP = Math.Max(maxDemandaHP, valorDemandaHP);
                                if (ws.Cells[rw, indexYIni + 4].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 4].Value.ToString(), out valorDemandaHFP))
                                    maxDemandaHFP = Math.Max(maxDemandaHFP, valorDemandaHFP);
                                if (ws.Cells[rw, indexYIni + 6].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 6].Value.ToString(), out valorGeneracionHP))
                                    maxGeneracionHP = Math.Max(maxGeneracionHP, valorGeneracionHP);
                                if (ws.Cells[rw, indexYIni + 7].Value != null && double.TryParse(ws.Cells[rw, indexYIni + 7].Value.ToString(), out valorGeneracionHFP))
                                    maxGeneracionHFP = Math.Max(maxGeneracionHFP, valorGeneracionHFP);
                            }

                            ws.Cells[indexXIni + 12, indexYIni + 2].Value = totalDemandaEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 3].Value = maxDemandaHP;
                            ws.Cells[indexXIni + 12, indexYIni + 4].Value = maxDemandaHFP;
                            ws.Cells[indexXIni + 12, indexYIni + 5].Value = totalGeneracionEnergia;
                            ws.Cells[indexXIni + 12, indexYIni + 6].Value = maxGeneracionHP;
                            ws.Cells[indexXIni + 12, indexYIni + 7].Value = maxGeneracionHFP;

                            ws.Cells[indexXIni + 12, indexYIni + 2].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 3].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 4].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 5].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 6].Style.Numberformat.Format = "0.0000";
                            ws.Cells[indexXIni + 12, indexYIni + 7].Style.Numberformat.Format = "0.0000";
                            indexXIni = indexXIni + 13;
                        }
                    }
                    rg = ws.Cells[indexRg, indexYIni, indexXIni - 1, indexYIni + 7];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Font.Color.SetColor(Color.Black);
                }
                else
                {
                   xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato P. H2V.-E
                List<CuestionarioH2VEDTO> ch2veDTOs = proyectoModel.Ch2veDTOs;
                ws = xlPackage.Workbook.Worksheets["P. H2V.-E"];
                if (ch2veDTOs != null)
                {
                    var colHora = 31;
                    foreach (var detRegHoja in ch2veDTOs)
                    {
                        if (detRegHoja.Hora != null)
                        {
                            int matchedIndex = -1;
                            for (int r = 1; r <= ws.Dimension.End.Row; r++)
                            {
                                var valorCelda = ws.Cells[r, colHora].Value;
                                if (valorCelda != null)
                                {
                                    DateTime hora;
                                    if (DateTime.TryParse(valorCelda.ToString(), out hora))
                                    {
                                        string horaFormateada = hora.ToString("HH:mm");
                                        if (horaFormateada == detRegHoja.Hora.ToString())
                                        {
                                            matchedIndex = r;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (matchedIndex != -1)
                            {
                                ws.Cells[matchedIndex, colHora + 1].Value = detRegHoja.ConsumoEnergetico;
                                ws.Cells[matchedIndex, colHora + 2].Value = detRegHoja.ProduccionCentral;
                            }
                        }
                    }

                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato P. H2V.-F
                CuestionarioH2VFDTO cuestionarioH2VFDTO = proyectoModel.Ch2vfDTO;
                ws = xlPackage.Workbook.Worksheets["P. H2V.-F"];
                if (cuestionarioH2VFDTO != null)
                {
                    ws.Cells[11, 3].Value = cuestionarioH2VFDTO.EstudioFactibilidad;
                    ws.Cells[11, 5].Value = cuestionarioH2VFDTO.InvestigacionesCampo;
                    ws.Cells[11, 7].Value = cuestionarioH2VFDTO.GestionesFinancieras;
                    ws.Cells[11, 9].Value = cuestionarioH2VFDTO.DisenosPermisos;
                    ws.Cells[15, 3].Value = cuestionarioH2VFDTO.ObrasCiviles;
                    ws.Cells[15, 5].Value = cuestionarioH2VFDTO.Equipamiento;
                    ws.Cells[15, 8].Value = cuestionarioH2VFDTO.LineaTransmision;
                    ws.Cells[19, 3].Value = cuestionarioH2VFDTO.Administracion;
                    ws.Cells[19, 5].Value = cuestionarioH2VFDTO.Aduanas;
                    ws.Cells[19, 7].Value = cuestionarioH2VFDTO.Supervision;
                    ws.Cells[19, 9].Value = cuestionarioH2VFDTO.GastosGestion;
                    ws.Cells[23, 3].Value = cuestionarioH2VFDTO.Imprevistos;
                    ws.Cells[23, 5].Value = cuestionarioH2VFDTO.Igv;
                    ws.Cells[23, 8].Value = cuestionarioH2VFDTO.OtrosGastos;
                    ws.Cells[27, 3].Value = cuestionarioH2VFDTO.InversionTotalSinIgv;
                    ws.Cells[27, 7].Value = cuestionarioH2VFDTO.InversionTotalConIgv;
                    ws.Cells[31, 3].Value = cuestionarioH2VFDTO.FinanciamientoTipo;
                    ws.Cells[31, 6].Value = cuestionarioH2VFDTO.FinanciamientoEstado;
                    ws.Cells[31, 9].Value = cuestionarioH2VFDTO.PorcentajeFinanciado / 100;
                    ws.Cells[35, 3].Value = cuestionarioH2VFDTO.ConcesionDefinitiva;
                    ws.Cells[35, 5].Value = cuestionarioH2VFDTO.VentaEnergia;
                    ws.Cells[35, 7].Value = cuestionarioH2VFDTO.EjecucionObra;
                    ws.Cells[25, 9].Value = cuestionarioH2VFDTO.ContratosFinancieros;
                    ws.Cells[38, 3].Value = cuestionarioH2VFDTO.Observaciones;
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                // Formato P. H2V.-G
                List<CuestionarioH2VGDTO> ch2vgDTOs = proyectoModel.Ch2vgDTOs;
                List<DataCatalogoDTO> dataCatalogoDTOs = proyectoModel.DataCatalogoDTOs;
                ws = xlPackage.Workbook.Worksheets["P. H2V.-G"];
                if (ch2vgDTOs != null)
                {
                    var anioPeriodo = proyectoModel.TransmisionProyectoDTO.Periodo.PeriFechaInicio.Year;
                    var horizonteFin = anioPeriodo + proyectoModel.TransmisionProyectoDTO.Periodo.PeriHorizonteAdelante;
                    var indexIni = 10;
                    var indexIniY = 3;
                    var indexAnioA = indexIniY + 2;
                    var indexCatA = indexIni + 3;
                    var indexTrimA = indexIniY + 1;

                    ws.Cells[indexIni, indexIniY + 1].Value = $"Año {anioPeriodo - 1} ó antes";
                    for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                    {
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Value = anio;
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 10;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Merge = true;
                        ws.Cells[indexIni + 1, indexAnioA, indexIni + 1, indexAnioA + 3].Value = "TRIMESTRE";
                        ws.Cells[indexIni, indexAnioA, indexIni, indexAnioA + 3].Style.Font.Size = 8;
                        for (int t = 1; t <= 4; t++)
                        {
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Value = t;
                            ws.Cells[indexIni + 2, indexAnioA + t - 1].Style.Font.Size = 8;
                            ws.Column(indexAnioA + t - 1).Width = 3;
                        }
                        indexAnioA = indexAnioA + 4;
                    }
                    if (dataCatalogoDTOs != null)
                    {
                        foreach (var dataCatalogoDTO in dataCatalogoDTOs)
                        {
                            ws.Cells[indexCatA, indexIniY].Value = dataCatalogoDTO.DesDataCat;
                            ws.Cells[indexCatA, indexIniY].Style.WrapText = true;
                            indexCatA++;
                        }
                    }
                    if (ch2vgDTOs != null)
                    {
                        foreach (var detRegHoja in ch2vgDTOs)
                        {
                            var matchedCatalogo = dataCatalogoDTOs.FirstOrDefault(x => x.DataCatCodi == detRegHoja.DataCatCodi);
                            if (matchedCatalogo != null)
                            {
                                var matchedIndex = dataCatalogoDTOs.IndexOf(matchedCatalogo) + indexIni + 3;
                                int matchTrim;
                                if (detRegHoja.Anio == "0")
                                {
                                    matchTrim = indexTrimA;
                                }
                                else
                                {
                                    matchTrim = indexTrimA + ((int.Parse(detRegHoja.Anio) - 1) * 4) + detRegHoja.Trimestre;
                                }
                                ws.Cells[matchedIndex, matchTrim].Value = detRegHoja.Valor == "1" ? "x" : "";
                            }
                        }
                    }
                    if (indexAnioA > indexIniY) {
                        rg = ws.Cells[indexIni, indexIniY, indexIni + 2, indexAnioA - 1];
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                        rg = ws.Cells[indexIni + 3, indexIniY, indexCatA - 1, indexAnioA - 1];
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg = ws.Cells[indexIni + 3, indexIniY + 1, indexCatA - 1, indexAnioA - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Font.Bold = true;
                        rg.Style.Font.Size = 10;
                    }
                }
                else
                {
                    xlPackage.Workbook.Worksheets.Delete(ws);
                }
                xlPackage.Save();
            }

        }
        public static double CalcularTasaCrecimientoPromedio(List<double> valores, List<int> anios)
        {
            if (valores.Count > 1 && anios.Count > 1)
            {
                double valorInicial = valores.First();
                double valorFinal = valores.Last();
                int anioInicial = anios.First();
                int anioFinal = anios.Last();

                if (valorInicial == 0)
                {
                    return 0; 
                }

                if (anioFinal <= anioInicial)
                {
                    return 0;
                }

                return Math.Pow(valorFinal / valorInicial, 1.0 / (anioFinal - anioInicial)) - 1;
            }
            return 0;
        }
    }
}