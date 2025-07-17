using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using log4net;
using Newtonsoft.Json;
using Novacode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace COES.Servicios.Aplicacion.Interconexiones
{
    /// <summary>
    /// Clase de manejo de logica para las interconexiones
    /// </summary>
    public class InformeInterconexionAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InformeInterconexionAppServicio));

        /// <summary>
        /// Listar la lista de semanas
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<InformacionSemana> ObtenerSemanasPorAnio(int anio)
        {
            List<InformacionSemana> entitys = new List<InformacionSemana>();
            int nsemanas = EPDate.TotalSemanasEnAnho(anio, FirstDayOfWeek.Saturday);
            DateTime dtfecha = EPDate.f_fechainiciosemana(anio, 1);

            for (int i = 1; i <= nsemanas; i++)
            {
                DateTime dtfechaIniSem = dtfecha.AddDays(7 * (i - 1));
                DateTime dtfechaFinSem = dtfechaIniSem.AddDays(6);
                InformacionSemana reg = new InformacionSemana
                {
                    NroSemana = i,
                    NombreSemana = string.Format("Sem{0}-{1}", i, anio),
                    Inicio = dtfechaIniSem.ToString(ConstantesAppServicio.FormatoFecha),
                    Fin = dtfechaFinSem.ToString(ConstantesAppServicio.FormatoFecha),
                    FechaInicio = dtfechaIniSem,
                    FechaFin = dtfechaFinSem
                };

                entitys.Add(reg);
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener el listado de versiones
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        public List<MeInformeInterconexionDTO> ConsultarVersiones(int anio, int semana)
        {
            return FactorySic.GetMeInformeInterconexionRepository().GetByCriteria(anio, semana);
        }

        /// <summary>
        /// Genera el grafico para los reportes de Flujo de Potencia
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="tipoInterconexion"></param>
        /// <param name="parametro"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public GraficoModel GraficoFlujoPotencia(int idPtomedicion, int tipoInterconexion, int parametro, DateTime fechaIni, DateTime fechaFin)
        {
            InInterconexionDTO interconexion = (new InterconexionesAppServicio()).ObtenerInterconexion(idPtomedicion);
            GraficoModel model = new GraficoModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            int factor = 1;
            model.Grafico = grafico;
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            lista = (new InterconexionesAppServicio()).ObtenerInterconexionParametro(idPtomedicion, parametro, fechaIni, fechaFin);
            model.Grafico.SerieDataS = new DatosSerie[2][];
            model.Grafico.Series.Add(new RegistroSerie());
            model.Grafico.Series.Add(new RegistroSerie());
            switch (parametro)
            {
                case 1:
                    model.Grafico.Series[0].Name = interconexion.Internomlinea + " Exportación MW";
                    model.Grafico.Series[0].Type = "area";
                    model.Grafico.Series[0].Color = "#4F81BD";
                    model.Grafico.Series[0].YAxisTitle = "MW";
                    model.Grafico.Series[1].Name = interconexion.Internomlinea + " Importación MW";
                    model.Grafico.Series[1].Type = "area";
                    model.Grafico.Series[1].Color = "#C0504D";
                    model.Grafico.Series[1].YAxisTitle = "MW";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    factor = 4;
                    model.Grafico.TitleText = @"Flujo en la línea " + interconexion.Interenlace + " de 220kV";
                    model.Grafico.YAxixTitle.Add("MW");
                    break;
                
            }

            if (lista.Count > 0)
            {
                model.FechaInicio = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:Horas";

                // titulo el reporte               

                //model.Grafico.subtitleText = lista24[0].Equinomb + " (MW)" + " Del" + model.FechaInicio + " Al" + model.FechaFin;
                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                // Obtener Lista de intervalos categoria del grafico   
                model.Grafico.SeriesYAxis.Add(0);
                int indiceA = 0;
                int indiceB = 0;
                for (var i = 0; i < lista.Count(); i++)
                {
                    DateTime fecha = (DateTime)lista[i].Medifecha;
                    var registro = lista[i];
                    for (var j = 1; j <= 96; j++)
                    {
                        decimal? valor = 0;
                        valor = (decimal?)registro.GetType().GetProperty("H" + (j).ToString()).GetValue(registro, null);
                        if (valor == null)
                            valor = 0;
                        if (i == 0)
                            model.Grafico.XAxisCategories.Add(registro.Medifecha.Value.AddMinutes(j * 15).ToString(ConstantesAppServicio.FormatoFechaHora));
                        var serie = new DatosSerie();
                        serie.X = fecha.AddMinutes(j * 15);
                        serie.Y = valor * factor;
                        switch (registro.Tipoinfocodi)
                        {
                            case 3:
                                if (registro.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MWh)
                                {
                                    model.Grafico.SerieDataS[0][indiceB * 96 + (j - 1)] = serie;
                                }
                                else
                                {
                                    try
                                    {
                                        model.Grafico.SerieDataS[1][indiceA * 96 + (j - 1)] = serie;
                                    }
                                    catch (Exception ex)
                                    {
                                        var msg = ex.Message;
                                    }
                                }
                                break;
                            case 4:
                                if (registro.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MVARr)
                                {
                                    model.Grafico.SerieDataS[0][indiceB * 96 + (j - 1)] = serie;
                                }
                                else
                                {
                                    model.Grafico.SerieDataS[1][indiceA * 96 + (j - 1)] = serie;
                                }
                                break;
                            case 5:
                                model.Grafico.SerieDataS[0][indiceB * 96 + (j - 1)] = serie;
                                break;
                            case 9:

                                model.Grafico.SerieDataS[1][indiceA * 96 + (j - 1)] = serie;
                                break;
                        }
                    }
                    switch (registro.Tipoinfocodi)
                    {
                        case 3:
                            if (registro.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MWh) indiceB++;
                            else indiceA++;
                            break;
                        case 4:
                            if (registro.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MVARr) indiceB++;
                            else indiceA++;
                            break;
                        case 5:
                            indiceB++;
                            break;
                        case 9:
                            indiceA++;
                            break;

                    }
                }
                //modelGraficoDiario = model;
            }// end del if 
            return model;
        }

        /// <summary>
        /// Permite visualiar obtener la data del grafico de evolución de energia
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public GraficoModel GraficoEvolucionEnergia(int idPtomedicion, DateTime fechaIni, DateTime fechaFin)
        {
            GraficoModel model = new GraficoModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            int factor = 1;
            model.Grafico = grafico;
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();
            lista24 = (new InterconexionesAppServicio()).ObtenerListaIntercambiosElectricidad(idPtomedicion, fechaIni, fechaFin);
            int totalIntervalos = lista24.Count;
            model.Grafico.SeriesData = new decimal?[4][];
            if (lista24.Count > 0)
            {
                var ListaFechas = lista24.Select(x => x.Medifecha).Distinct().ToList();
                if (ListaFechas.Count > 0)
                {
                    fechaIni = ListaFechas.Min();
                    fechaFin = ListaFechas.Max();
                }
                model.FechaInicio = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia";
                // titulo el reporte               
                model.Grafico.TitleText = @"EVOLUCIÓN DE LA MÁXIMA DEMANDA Y ENERGÍA IMPORTADA Y EXPORTADA DIARIA EN LOS INTERCAMBIOS INTERNACIONALES";
                //model.Grafico.subtitleText = lista24[0].Equinomb + " (MW)" + " Del" + model.FechaInicio + " Al" + model.FechaFin;
                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";

                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesName = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                // Obtener Lista de intervalos categoria del grafico   

                foreach (var p in lista24)
                {
                    model.Grafico.XAxisCategories.Add(p.Medifecha.ToString(ConstantesAppServicio.FormatoFecha));
                }
                // Obtener lista de valores para las series del grafico

                model.Grafico.Series = new List<RegistroSerie>();
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                for (var i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            model.Grafico.Series[i].Name = "Energía Exportada (MWh)";
                            model.Grafico.Series[i].Type = "column";
                            model.Grafico.Series[i].Color = "#3498DB";
                            model.Grafico.Series[i].YAxis = 0;
                            model.Grafico.Series[i].YAxisTitle = "MWh";
                            break;
                        case 1:
                            model.Grafico.Series[i].Name = "Máxima Potencia Exportada(MW)";
                            model.Grafico.Series[i].Type = "spline";
                            model.Grafico.Series[i].Color = "#DC143C";
                            model.Grafico.Series[i].YAxis = 1;
                            model.Grafico.Series[i].YAxisTitle = "MW";
                            factor = 1;
                            break;
                        case 2:
                            model.Grafico.Series[i].Name = "Energía Importada (MWh)";
                            model.Grafico.Series[i].Type = "column";
                            model.Grafico.Series[i].Color = "#3CB371";
                            model.Grafico.Series[i].YAxis = 0;
                            model.Grafico.Series[i].YAxisTitle = "MWh";
                            break;
                        case 3:
                            model.Grafico.Series[i].Name = "Máxima Potencia Importada(MW)";
                            model.Grafico.Series[i].Type = "spline";
                            model.Grafico.Series[i].Color = "#F0E68C";
                            model.Grafico.Series[i].YAxis = 1;
                            model.Grafico.Series[i].YAxisTitle = "MW";
                            factor = 1;
                            break;

                    }
                    model.Grafico.SeriesData[i] = new decimal?[totalIntervalos];
                    for (var j = 1; j <= totalIntervalos; j++)
                    {
                        decimal? valor = 0;
                        valor = (decimal?)lista24[j - 1].GetType().GetProperty("H" + (i + 1).ToString()).GetValue(lista24[j - 1], null);
                        model.Grafico.SeriesData[i][j - 1] = valor * factor;
                    }
                }

                //modelGraficoDiario = model;

            }// end del if 
            return model;
        }

        /// <summary>
        /// Permite visualiar obtener la data del grafico de evolución de energia
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public GraficoModel GraficoEvolucionEnergiaAcumulada(int idPtomedicion, DateTime fechaIni, DateTime fechaFin)
        {
            GraficoModel model = new GraficoModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            int factor = 1;
            model.Grafico = grafico;
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            lista = (new InterconexionesAppServicio()).ObtenerListaIntercambiosElectricidad(idPtomedicion, fechaIni, fechaFin);
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idPtomedicion);

            //- Debemos obtener los datos por cada mes en los dos ultimos años
            var meses = lista.Select(x => new { x.Medifecha.Year, x.Medifecha.Month }).Distinct().ToList();
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();

            foreach (var mes in meses)
            {
                List<MeMedicion24DTO> subList = lista.Where(x => x.Medifecha.Year == mes.Year && x.Medifecha.Month == mes.Month).ToList();
                MeMedicion24DTO entity = new MeMedicion24DTO();
                entity.Ptomedicodi = (int)interconexion.Ptomedicodi;
                entity.H1 = subList.Sum(x => x.H1); 
                entity.H2 = subList.Max(x => x.H2);
                entity.Medifecha = new DateTime(mes.Year, mes.Month, 1);
                entity.H3 = subList.Sum(x => x.H3);
                entity.H4 = subList.Max(x => x.H4);
                lista24.Add(entity);
            }

            List<int> anios = lista.Select(x => x.Medifecha.Year).Distinct().ToList();



            int totalIntervalos = lista24.Count;
            model.Grafico.SeriesData = new decimal?[4][];
            if (lista24.Count > 0)
            {
                var ListaFechas = lista24.Select(x => x.Medifecha).Distinct().ToList();
                if (ListaFechas.Count > 0)
                {
                    fechaIni = ListaFechas.Min();
                    fechaFin = ListaFechas.Max();
                }
                model.FechaInicio = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia";
                // titulo el reporte               
                model.Grafico.TitleText = @"EVOLUCIÓN MENSUAL INTERCAMBIOS DE ELECTRICIDAD " + interconexion.Interdecrip;
                model.Grafico.Subtitle = string.Join(" - ", anios.Select(n => n.ToString()).ToArray());
                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";

                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesName = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                // Obtener Lista de intervalos categoria del grafico   

                foreach (var p in lista24)
                {
                    model.Grafico.XAxisCategories.Add(COES.Base.Tools.Util.ObtenerNombreMes(p.Medifecha.Month) + " " + p.Medifecha.Year);
                }
                // Obtener lista de valores para las series del grafico

                model.Grafico.Series = new List<RegistroSerie>();
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                for (var i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            model.Grafico.Series[i].Name = "Exportación (MWh)";
                            model.Grafico.Series[i].Type = "column";
                            model.Grafico.Series[i].Color = "#4F81BD";
                            model.Grafico.Series[i].YAxis = 0;
                            model.Grafico.Series[i].YAxisTitle = "MWh";
                            break;
                        case 1:
                            model.Grafico.Series[i].Name = "Máxima Exportación(MW)";
                            model.Grafico.Series[i].Type = "spline";
                            model.Grafico.Series[i].Color = "#0070C0";
                            model.Grafico.Series[i].YAxis = 1;
                            model.Grafico.Series[i].YAxisTitle = "MW";
                            factor = 1;
                            break;
                        case 2:
                            model.Grafico.Series[i].Name = "Importación (MWh)";
                            model.Grafico.Series[i].Type = "column";
                            model.Grafico.Series[i].Color = "#9BBB59";
                            model.Grafico.Series[i].YAxis = 0;
                            model.Grafico.Series[i].YAxisTitle = "MWh";
                            break;
                        case 3:
                            model.Grafico.Series[i].Name = "Máxima Importación(MW)";
                            model.Grafico.Series[i].Type = "spline";
                            model.Grafico.Series[i].Color = "#92D050";
                            model.Grafico.Series[i].YAxis = 1;
                            model.Grafico.Series[i].YAxisTitle = "MW";
                            factor = 1;
                            break;

                    }
                    model.Grafico.SeriesData[i] = new decimal?[totalIntervalos];
                    for (var j = 1; j <= totalIntervalos; j++)
                    {
                        decimal? valor = 0;
                        valor = (decimal?)lista24[j - 1].GetType().GetProperty("H" + (i + 1).ToString()).GetValue(lista24[j - 1], null);
                        model.Grafico.SeriesData[i][j - 1] = valor * factor;
                    }
                }

                //modelGraficoDiario = model;

            }// end del if 
            return model;
        }

        /// <summary>
        /// Permite generar una nueva version del informe
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="semana"></param>
        /// <param name="username"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public int GenerarVersionInforme(int anio, int semana, string path, string username)
        {
            try
            {
                int id = this.GrabarVersion(anio, semana, username);
                string fileName = string.Format(ConstantesInterconexiones.NombreArhivoInformeInterconexion, id);
                string rutaFile = path + fileName;
                List<MeInformeAntecedenteDTO> listAntecedentes = this.ObtenerAntecedentes().OrderBy(x=>x.Infantcodi).ToList();
                InformacionSemana infoSemana = this.ObtenerSemanasPorAnio(anio).Where(x => x.NroSemana == semana).First();

                List<InInterconexionDTO> listaInterconexiones = FactorySic.GetInInterconexionRepository().List();

                FileInfo newFile = new FileInfo(rutaFile);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(rutaFile);
                }

                using (DocX document = DocX.Create(rutaFile))
                {
                    GenerarContenidoInforme(document, listAntecedentes, semana, anio, infoSemana, listaInterconexiones);

                    document.SaveAs(rutaFile);
                }
                if(!FileServer.VerificarExistenciaDirectorio(ConstantesInterconexiones.FolderIntervenciones, string.Empty))
                {
                    FileServer.CreateFolder(string.Empty, ConstantesInterconexiones.FolderIntervenciones, string.Empty);
                }

                if (FileServer.VerificarExistenciaFile(ConstantesInterconexiones.FolderIntervenciones, fileName, string.Empty))
                {
                    FileServer.DeleteBlob(ConstantesInterconexiones.FolderIntervenciones + fileName, string.Empty);
                }

                FileServer.UploadFromFileDirectory(rutaFile, ConstantesInterconexiones.FolderIntervenciones, fileName, string.Empty);                

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }


        /// <summary>
        /// Permite generar el documento word
        /// </summary>
        /// <param name="document"></param>
        /// <param name="listAntecedentes"></param>
        /// <param name="nroSemana"></param>
        /// <param name="anio"></param>
        /// <param name="infoSemana"></param>
        /// <param name="listaInterconexiones"></param>
        public void GenerarContenidoInforme(DocX document, List<MeInformeAntecedenteDTO> listAntecedentes, int nroSemana, int anio,
            InformacionSemana infoSemana, List<InInterconexionDTO> listaInterconexiones)
        {
            string fontCalibri = "Calibri";
            string fontArial = "Arial";
            string fontVerdana = "Verdana";
            document.MarginLeft = 76.0f;
            document.MarginRight = 76.0f;            
            document.MarginTop = 160.0f;            

            // Add Header and Footer support to this document.
            document.AddHeaders();
            document.AddFooters();

            // Get the odd and even Headers for this document.
            Header header_odd = document.Headers.odd;

            // Get the odd and even Footer for this document.
            Footer footer_odd = document.Footers.odd;

            #region header
            // Insert a Paragraph into the odd Header.
            Novacode.Image logo = document.AddImage(AppDomain.CurrentDomain.BaseDirectory + "Content/Images/" + "Coes.png");

            Table header_first_table = header_odd.InsertTable(2, 3);
            header_first_table.Design = TableDesign.TableGrid;
            header_first_table.AutoFit = AutoFit.ColumnWidth;

            header_first_table.MergeCellsInColumn(0, 0, 1);
            header_first_table.MergeCellsInColumn(1, 0, 1);

            header_first_table.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            header_first_table.Rows[0].Cells[0].Width = 150;

            header_first_table.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
            header_first_table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            header_first_table.Rows[0].Cells[1].Width = 300;

            header_first_table.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
            header_first_table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
            header_first_table.Rows[1].Cells[2].Paragraphs[0].Alignment = Alignment.center;
            header_first_table.Rows[1].Cells[2].VerticalAlignment = VerticalAlignment.Center;
            header_first_table.Rows[0].Cells[2].Width = 200;

            //primera fila
            Paragraph upperRightParagraph = header_odd.Tables[0].Rows[0].Cells[0].Paragraphs[0];
            upperRightParagraph.AppendPicture(logo.CreatePicture(70, 135));
            upperRightParagraph.Alignment = Alignment.left;
            header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            

            Paragraph cabcentro = header_odd.Tables[0].Rows[0].Cells[1].Paragraphs[0];
            cabcentro.AppendLine();            
            string informeTitulo = "OPERACIÓN DE INTERCAMBIOS DE ELECTRICIDAD SEMANA OPERATIVA \n N ° " + nroSemana.ToString().PadLeft(2, '0') + " - " + anio;
            cabcentro.Append(informeTitulo).Font(new FontFamily(fontArial)).Bold().FontSize(9);
            cabcentro.AppendLine();
         
            Paragraph cabDerecha = header_odd.Tables[0].Rows[0].Cells[2].Paragraphs[0];            
            cabDerecha.Append("SUB DIRECCIÓN DE GESTIÓN DE LA INFORMACIÓN").Font(new FontFamily(fontArial)).Bold().FontSize(8);
            
            Paragraph cabNumeral = header_odd.Tables[0].Rows[1].Cells[2].Paragraphs[0];            
            cabNumeral.Append("INFORME N° COES/D/SGI-XXX-" + anio + "\n").Font(new FontFamily(fontArial)).Bold().FontSize(8);
            
            string fechaReporte = string.Format("{0} de {1} del {2}", string.Format("{0:D2}", DateTime.Now.Day), EPDate.f_NombreMes(DateTime.Now.Month), DateTime.Now.Year);
            cabNumeral.Append("FECHA: " + fechaReporte).Font(new FontFamily(fontArial)).Bold().FontSize(8);
            

            #endregion

            #region Footer

            Table footer_table = footer_odd.InsertTable(1, 2);
            footer_table.Design = TableDesign.TableNormal;
            footer_table.AutoFit = AutoFit.ColumnWidth;
           

            footer_table.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.left;
            footer_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            footer_table.Rows[0].Cells[0].Width = 500;

            footer_table.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.right;
            footer_table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
            footer_table.Rows[0].Cells[1].Width = 150;

            Paragraph texto_footer = footer_odd.Tables[0].Rows[0].Cells[0].Paragraphs[0];
            texto_footer.AppendLine("INFORME SEMANAL DE LA OPERACIÓN DE INTERCAMBIO DE ELECTRICIDAD");
            texto_footer.Font(new FontFamily(fontVerdana)).FontSize(6);
            DateTime inicioSemana = DateTime.ParseExact(infoSemana.Inicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime finSemana = DateTime.ParseExact(infoSemana.Fin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            string rangoFechas = string.Format("DEL {0} DE {1} DE {2}", string.Format("{0:D2}", inicioSemana.Day), EPDate.f_NombreMes(inicioSemana.Month), inicioSemana.Year).ToUpper() +
                 string.Format(" AL {0} DE {1} DE {2}", string.Format("{0:D2}", finSemana.Day), EPDate.f_NombreMes(finSemana.Month), finSemana.Year).ToUpper();
            string textoFooter = "SEMANA OPERATIVA N° " + nroSemana.ToString().PadLeft(2, '0') + " " + rangoFechas;
                 
            texto_footer.AppendLine(textoFooter);
            texto_footer.Font(new FontFamily(fontVerdana)).FontSize(6);
            Paragraph numero_pagina = footer_odd.Tables[0].Rows[0].Cells[1].Paragraphs[0];            
            numero_pagina.AppendPageNumber(PageNumberFormat.normal);

            #endregion

            #region Contenido
            Paragraph pTC = document.InsertParagraph();
            pTC.Alignment = Alignment.center;            
            pTC.AppendLine("INFORME SEMANAL").Font(new FontFamily(fontArial)).Bold().FontSize(11);
            pTC.AppendLine("OPERACIÓN DE INTERCAMBIOS DE ELECTRICIDAD").Font(new FontFamily(fontArial)).Bold().FontSize(11);
            pTC.AppendLine(rangoFechas).Font(new FontFamily(fontArial)).Bold().FontSize(11);

            pTC.AppendLine();
            pTC.AppendLine();

            var bulletedList = document.AddList("ANTECEDENTES", 0, ListItemType.Numbered);
            document.AddListItem(bulletedList, "OBJETIVO");
            document.AddListItem(bulletedList, "BASE LEGAL");
            document.AddListItem(bulletedList, "EVALUACIÓN DE LA OPERACIÓN DE INTERCAMBIO DE ELECTRICIDAD");

            foreach (InInterconexionDTO interconexion in listaInterconexiones)
            {
                document.AddListItem(bulletedList, string.Format("EVOLUCIÓN DEL INTERCAMBIO DE POTENCIA Y ENERGIA ENTRE {0}.", 
                    interconexion.Interdecrip.Replace("-", " y ")));
            }

            document.AddListItem(bulletedList, "CONCLUSIONES");
            List<Paragraph> actualListData = bulletedList.Items;

            #endregion

            #region Seccion 1 Antecedentes
            Paragraph pListas2 = document.InsertParagraph();
            pListas2.InsertParagraphBeforeSelf(actualListData.ElementAt(0).Bold());

             var bulletedListP2 = document.AddList();
            foreach (var antecedemte in listAntecedentes)
            {
                document.AddListItem(bulletedListP2, antecedemte.Intantcontenido, 1, ListItemType.Numbered);
            }
           
            List<Paragraph> actualListDataP2 = bulletedListP2.Items;

            int index = 0;
            foreach (var antecedemte in listAntecedentes)
            {
                Paragraph pListasP2_1 = document.InsertParagraph();
                actualListDataP2.ElementAt(index).Alignment = Alignment.both;  //justify A
                pListasP2_1.InsertParagraphBeforeSelf(actualListDataP2.ElementAt(index).Font(new FontFamily(fontArial)).FontSize(10));
                index++;
            }

            #endregion

            #region Seccion 2 Objetivo
            Paragraph pListas1 = document.InsertParagraph();
            pListas1.InsertParagraphBeforeSelf(actualListData.ElementAt(1).Bold());

            Paragraph pa1 = document.InsertParagraph();
            pa1.Alignment = Alignment.both;  //justify
            pa1.IndentationBefore = 0.7f;
            pa1.Append(string.Format("Evidenciar la priorización del abastecimiento de electricidad al mercado interno durante la semana en evaluación (semana operativa " + nroSemana.ToString().PadLeft(2, '0') + " - " + anio + "), considerando el Intercambio de Electricidad entre los sistemas eléctricos de Perú y Ecuador, en cumplimiento con lo establecido en el PR-43"))
                .Font(new FontFamily(fontArial)).FontSize(10);

            Paragraph paS1_1 = document.InsertParagraph();
            paS1_1.AppendLine();

            #endregion

            #region Seccion 3 Base Legal

            Paragraph pListas3 = document.InsertParagraph();
            pListas3.InsertParagraphAfterSelf(actualListData.ElementAt(2).Bold());
            Paragraph pListas31 = document.InsertParagraph();
            pListas31.AppendLine();

            List<string> marcos = new List<string>();
            marcos.Add("Decisión 757 de la Comunidad Andina, referida a la Vigencia de la Decisión 536 “Marco General para la Interconexión Subregional de Sistemas Eléctricos e Intercambio Intracomunitario de Electricidad”.");
            marcos.Add("Decreto Supremo N°011-2012-EM. Reglamento Interno para la Aplicación de la Decisión 757 de la CAN. En el numeral 3.5.");
            marcos.Add("Procedimiento Técnico del COES N°43 “Intercambios Internacionales de Electricidad en el Marco de la Decisión 757 de la CAN”.");

            var bulletedListP3 = document.AddList();
            foreach (var marco in marcos)
            {
                document.AddListItem(bulletedListP3, marco, 1, ListItemType.Numbered, 1);
            }

            List<Paragraph> actualListDataP3 = bulletedListP3.Items;

            index = 0;
            foreach (var antecedemte in marcos)
            {
                Paragraph pListasP2_1 = document.InsertParagraph();
                actualListDataP3.ElementAt(index).Alignment = Alignment.both;  //justify A
                pListasP2_1.InsertParagraphBeforeSelf(actualListDataP3.ElementAt(index).Font(new FontFamily(fontArial)).FontSize(10));
                index++;
            }

            #endregion

            #region Seccion 4 Evaluacion de la operacion

            Paragraph pListas4 = document.InsertParagraph();
            pListas4.InsertParagraphAfterSelf(actualListData.ElementAt(3).Bold());
            Paragraph pListas41 = document.InsertParagraph();
            pListas41.AppendLine();

            List<string> evaluaciones = new List<string>();
            evaluaciones.Add("PROGRAMA DE OPERACIÓN DIARIA.");
            evaluaciones.Add("RESTRICCIÓN DE SUMINISTRO ELÉCTRICO POR DÉFICIT DE GENERACIÓN ");
            evaluaciones.Add("PERTURBACIONES RELACIONADAS CON LA INTERCONEXIÓN");

            var bulletedListP4 = document.AddList();
            foreach (var marco in evaluaciones)
            {
                document.AddListItem(bulletedListP4, marco, 1, ListItemType.Numbered, 1);
            }

            List<Paragraph> actualListDataP4 = bulletedListP4.Items;

            index = 0;
            foreach (var antecedemte in evaluaciones)
            {
                Paragraph pListasP2_1 = document.InsertParagraph();
                actualListDataP4.ElementAt(index).Alignment = Alignment.both;  //justify A
                pListasP2_1.InsertParagraphBeforeSelf(actualListDataP4.ElementAt(index).Font(new FontFamily(fontArial)).FontSize(10).Bold());

                if(index == 0)
                {
                    Paragraph pSection41 = document.InsertParagraph();
                    pSection41.Append("Durante la semana operativa N° " + nroSemana.ToString().PadLeft(2, '0') + ", los programas diarios de Operación (PDO), en los cuales fue programado el Intercambio de Electricidad entre el SEIN y el Sistema Ecuatoriano, no contienen en ningún caso programación de restricciones de suministro en el SEIN a consecuencia de dicha interconexión, los referidos programas diarios se encuentran publicados en el portal del COES:\r\n");
                  
                    pSection41.Alignment = Alignment.both;
                    pSection41.AppendLine();
                    Uri url = new Uri("http://www.coes.org.pe/Portal/Operacion/ProgOperacion/ProgramaDiario");
                    Hyperlink link = document.AddHyperlink("http://www.coes.org.pe/Portal/Operacion/ProgOperacion/ProgramaDiario", url);
                  
                    Paragraph pSectionLink = document.InsertParagraph();
                    pSectionLink.AppendHyperlink(link);
                    pSectionLink.AppendLine();

                    Paragraph pSection42 = document.InsertParagraph();
                    pSection42.AppendLine();

                }

                if(index == 1)
                {
                    Paragraph pSection42 = document.InsertParagraph();
                    pSection42.Append("Durante la exportación de electricidad de Ecuador de la semana operativa N° " + nroSemana.ToString().PadLeft(2, '0') + ", no se registraron cortes de suministro a la demanda del mercado interno (SEIN) debidos a déficit de generación. Esta información puede ser corroborada en el Informe diario de evaluación de la operación del Coordinador de la Operación del Sistema (IDCOS) y en el Informe de evaluación de la operación diaria (IEOD), publicados en el portal web del COES:\r\n");
                    pSection42.Alignment = Alignment.both;
                    Uri url = new Uri("https://www.coes.org.pe/Portal/PostOperacion/Reportes/Idcos");
                    Hyperlink link = document.AddHyperlink("https://www.coes.org.pe/Portal/PostOperacion/Reportes/Idcos", url);

                    Paragraph pSectionLink = document.InsertParagraph();
                    pSectionLink.AppendHyperlink(link);
                    pSectionLink.AppendLine();

                    Uri url1 = new Uri("https://www.coes.org.pe/Portal/PostOperacion/Reportes/Ieod");
                    Hyperlink link1 = document.AddHyperlink("https://www.coes.org.pe/Portal/PostOperacion/Reportes/Ieod", url);
                    Paragraph pSectionLink1 = document.InsertParagraph();
                    pSectionLink1.AppendHyperlink(link1);
                    pSectionLink1.AppendLine();

                    Paragraph pSection44 = document.InsertParagraph();
                    pSection44.AppendLine();
                }

                if(index == 2)
                {
                    Paragraph pSection43 = document.InsertParagraph();
                    pSection43.Append("No se presentaron perturbaciones.");
                    pSection43.AppendLine();
                }

                index++;
            }

            #endregion

            #region Seccion 5

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberDecimalDigits = 3;

            index = 0;
            foreach (InInterconexionDTO interconexion in listaInterconexiones)
            {
                #region Tabla de datos

                Paragraph pInterconexion = document.InsertParagraph();
                pInterconexion.InsertParagraphBeforeSelf(actualListData.ElementAt(4 + index).Bold());

                Paragraph itemParrafo = document.InsertParagraph();
                itemParrafo.AppendLine();
                itemParrafo.Append("Durante la semana operativa en evaluación, hubo exportación de electricidad proveniente del Ecuador. Dicha exportación permitió atender la carga de la subestación Machala en los siguientes periodos:");
                itemParrafo.Alignment = Alignment.both;
                itemParrafo.AppendLine();

                List<EstructuraEvolucionEnergia> listData = (new InterconexionesAppServicio()).ObtenerDataEvolucionEnergia(interconexion.Intercodi, infoSemana.FechaInicio, infoSemana.FechaFin);

                Table tablaInterconexion = document.InsertTable(listData.Count + 1, 7);
                tablaInterconexion.AutoFit = AutoFit.Contents;
                tablaInterconexion.Design = TableDesign.TableGrid;
                tablaInterconexion.Alignment = Alignment.center;

                CentralCelda(tablaInterconexion, 0, 0);
                CentralCelda(tablaInterconexion, 0, 1);
                CentralCelda(tablaInterconexion, 0, 2);
                CentralCelda(tablaInterconexion, 0, 3);
                CentralCelda(tablaInterconexion, 0, 4);
                CentralCelda(tablaInterconexion, 0, 5);
                CentralCelda(tablaInterconexion, 0, 6);
                               
                tablaInterconexion.Rows[0].Cells[0].FillColor = Color.LightGray;
                tablaInterconexion.Rows[0].Cells[1].FillColor = Color.LightGray;
                tablaInterconexion.Rows[0].Cells[2].FillColor = Color.LightGray;
                tablaInterconexion.Rows[0].Cells[3].FillColor = Color.LightGray;
                tablaInterconexion.Rows[0].Cells[4].FillColor = Color.LightGray;
                tablaInterconexion.Rows[0].Cells[5].FillColor = Color.LightGray;
                tablaInterconexion.Rows[0].Cells[6].FillColor = Color.LightGray;

                LlenarCeldaTabla(tablaInterconexion, 0, 0, 9, fontCalibri, true, "Fecha");
                LlenarCeldaTabla(tablaInterconexion, 0, 1, 9, fontCalibri, true, "Inicio");
                LlenarCeldaTabla(tablaInterconexion, 0, 2, 9, fontCalibri, true, "Fin");
                LlenarCeldaTabla(tablaInterconexion, 0, 3, 9, fontCalibri, true, "Energía \n Exportada (MWh)");
                LlenarCeldaTabla(tablaInterconexion, 0, 4, 9, fontCalibri, true, "Máxima Potencia \n Exportada (MW)");
                LlenarCeldaTabla(tablaInterconexion, 0, 5, 9, fontCalibri, true, "Energía \n Importada (MWh)");
                LlenarCeldaTabla(tablaInterconexion, 0, 6, 9, fontCalibri, true, "Máxima Potencia \n Importada (MW) ");

                int rowData = 1;
                foreach(EstructuraEvolucionEnergia itemData in listData)
                {
                    LlenarCeldaTabla(tablaInterconexion, rowData, 0, 9, fontCalibri, false, itemData.Fecha);
                    LlenarCeldaTabla(tablaInterconexion, rowData, 1, 9, fontCalibri, false, itemData.Inicio);
                    LlenarCeldaTabla(tablaInterconexion, rowData, 2, 9, fontCalibri, false, itemData.Fin);
                    LlenarCeldaTabla(tablaInterconexion, rowData, 3, 9, fontCalibri, false, ((decimal)itemData.EnergiaExportada).ToString("N", nfi));
                    LlenarCeldaTabla(tablaInterconexion, rowData, 4, 9, fontCalibri, false, ((decimal)itemData.MaximaEnergiaExportada).ToString("N", nfi));
                    LlenarCeldaTabla(tablaInterconexion, rowData, 5, 9, fontCalibri, false, ((decimal)itemData.EnergiaImportada).ToString("N", nfi));
                    LlenarCeldaTabla(tablaInterconexion, rowData, 6, 9, fontCalibri, false, ((decimal)itemData.MaximaEnergiaImportada).ToString("N", nfi));

                    rowData++;
                }

                Paragraph paragrapthGrafico = document.InsertParagraph();
                paragrapthGrafico.AppendLine();
                paragrapthGrafico.Append("La Información de los contadores de energía remitida al COES por el titular de transmisión RED DE ENERGIA DEL PERÚ S. A., correspondiente al periodo " + rangoFechas.ToLower() + ", se encuentran publicados en el portal web del COES, en la siguiente ruta: ");
                paragrapthGrafico.AppendLine();
                paragrapthGrafico.Alignment = Alignment.both;

                Uri url = new Uri("https://www.coes.org.pe/Portal/Interconexiones/Reportes/MenuInterconexiones");
                Hyperlink link = document.AddHyperlink("https://www.coes.org.pe/Portal/Interconexiones/Reportes/MenuInterconexiones", url);
                
                Paragraph paragrapthGraficoLink = document.InsertParagraph();
                paragrapthGraficoLink.AppendLine();
                paragrapthGraficoLink.AppendHyperlink(link);
                paragrapthGraficoLink.AppendLine();

                #endregion

                #region Graficos

                GraficoModel modelFlujoPotencia = this.GraficoFlujoPotencia(interconexion.Intercodi, 1, 1, infoSemana.FechaInicio, infoSemana.FechaFin);

                var heighImagePX = 356;
                var widthImagePX = 534;
                var js = new JavaScriptSerializer();
                var jsonDataLineas = ObtenerJsonStringGraficoFlujoPotencia(modelFlujoPotencia);
                var lineasObject = js.Deserialize<dynamic>(jsonDataLineas);
                var rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                var linkGraficoLineas = ObtenerLinkImagenDesdeJsonString(lineasObject, rutaBase + "grafico1.png");
                var imageLinea = document.AddImage(linkGraficoLineas);
                var tablaGraficos = document.InsertTable(1, 1);
                tablaGraficos.AutoFit = AutoFit.Contents;
                tablaGraficos.Design = TableDesign.TableGrid;
                tablaGraficos.Alignment = Alignment.center;
                tablaGraficos.Rows[0].Cells[0].Paragraphs[0].AppendPicture(imageLinea.CreatePicture(heighImagePX, widthImagePX));
                tablaGraficos.Rows[0].Cells[0].Paragraphs[0].AppendLine("\n");
                tablaGraficos.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                Paragraph pTC1 = document.InsertParagraph();
                pTC1.Alignment = Alignment.center;
                pTC1.AppendLine("Figura Nª 1").Font(new FontFamily(fontArial)).Bold().FontSize(10);
                pTC1.AppendLine("");

                GraficoModel modelEvolucionEnergia = this.GraficoEvolucionEnergia(interconexion.Intercodi, infoSemana.FechaInicio, infoSemana.FechaFin);
                               
                js = new JavaScriptSerializer();
                jsonDataLineas = ObtenerJsonStringGraficoEvolucionEnergia(modelEvolucionEnergia);
                lineasObject = js.Deserialize<dynamic>(jsonDataLineas);                
                linkGraficoLineas = ObtenerLinkImagenDesdeJsonString(lineasObject, rutaBase + "grafico2.png");
                imageLinea = document.AddImage(linkGraficoLineas);
                var tablaGraficos1 = document.InsertTable(1, 1);
                tablaGraficos1.AutoFit = AutoFit.Contents;
                tablaGraficos1.Design = TableDesign.TableGrid;
                tablaGraficos1.Alignment = Alignment.center;
                tablaGraficos1.Rows[0].Cells[0].Paragraphs[0].AppendPicture(imageLinea.CreatePicture(heighImagePX, widthImagePX));
                tablaGraficos1.Rows[0].Cells[0].Paragraphs[0].AppendLine("\n");
                tablaGraficos1.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;

                Paragraph pTC2 = document.InsertParagraph();
                pTC2.Alignment = Alignment.center;
                pTC2.AppendLine("Figura Nª 2").Font(new FontFamily(fontArial)).Bold().FontSize(10);
                pTC2.AppendLine("");


                GraficoModel modelEvolucionEnergiaAcumulada = this.GraficoEvolucionEnergiaAcumulada(interconexion.Intercodi, infoSemana.FechaFin.AddYears(-2), infoSemana.FechaFin);

                js = new JavaScriptSerializer();
                jsonDataLineas = ObtenerJsonStringGraficoEvolucionEnergia(modelEvolucionEnergiaAcumulada);
                lineasObject = js.Deserialize<dynamic>(jsonDataLineas);
                linkGraficoLineas = ObtenerLinkImagenDesdeJsonString(lineasObject, rutaBase + "grafico3.png");
                imageLinea = document.AddImage(linkGraficoLineas);
                var tablaGraficos2 = document.InsertTable(1, 1);
                tablaGraficos2.AutoFit = AutoFit.Contents;
                tablaGraficos2.Design = TableDesign.TableGrid;
                tablaGraficos2.Alignment = Alignment.center;
                tablaGraficos2.Rows[0].Cells[0].Paragraphs[0].AppendPicture(imageLinea.CreatePicture(heighImagePX, widthImagePX));
                tablaGraficos2.Rows[0].Cells[0].Paragraphs[0].AppendLine("\n");
                tablaGraficos2.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;

                Paragraph pTC3 = document.InsertParagraph();
                pTC3.Alignment = Alignment.center;
                pTC3.AppendLine("Figura Nª 3").Font(new FontFamily(fontArial)).Bold().FontSize(10);
                pTC3.AppendLine("");
                #endregion

                index++;

            }

            #endregion

            #region Conclusiones

            Paragraph pConclusiones = document.InsertParagraph();
            pConclusiones.InsertParagraphBeforeSelf(actualListData.ElementAt(4 + listaInterconexiones.Count).Bold());

            Paragraph pa1Conclusion = document.InsertParagraph();
            pa1Conclusion.Alignment = Alignment.both;  //justify
            pa1Conclusion.IndentationBefore = 0.7f;
            pa1Conclusion.Append(string.Format("En la semana operativa N°" + nroSemana.ToString().PadLeft(2, '0') + " - " + anio + ", durante el periodo de exportación de energía eléctrica desde el sistema ecuatoriano al SEIN, no se registraron restricciones, ni cortes de suministro en el SEIN a consecuencia de dicho intercambio, con lo que se demuestra que se ha priorizado el abastecimiento de la demanda del mercado interno."))
                .Font(new FontFamily(fontArial)).FontSize(10);

            Paragraph paS1_1_Conclusion = document.InsertParagraph();
            paS1_1_Conclusion.AppendLine();
            paS1_1_Conclusion.AppendLine();
            paS1_1_Conclusion.AppendLine();
            paS1_1_Conclusion.AppendLine();
            paS1_1_Conclusion.AppendLine();
            paS1_1_Conclusion.AppendLine();


            #endregion

            #region Firma

            Paragraph paFirma = document.InsertParagraph();
            paFirma.Append("San Isidro, " + fechaReporte).FontSize(10);
            paFirma.AppendLine();
            paFirma.Alignment = Alignment.right;

            Table tablaVersion = document.InsertTable(2, 6);
            tablaVersion.AutoFit = AutoFit.Contents;
            tablaVersion.Design = TableDesign.TableGrid;
            tablaVersion.Alignment = Alignment.center;

            CentralCelda(tablaVersion, 0, 0);
            CentralCelda(tablaVersion, 0, 1);
            CentralCelda(tablaVersion, 0, 2);
            CentralCelda(tablaVersion, 0, 3);
            CentralCelda(tablaVersion, 0, 4);
            CentralCelda(tablaVersion, 0, 5);
            CentralCelda(tablaVersion, 1, 0);
            CentralCelda(tablaVersion, 1, 1);
            CentralCelda(tablaVersion, 1, 2);
            CentralCelda(tablaVersion, 1, 3);
            CentralCelda(tablaVersion, 1, 4);
            CentralCelda(tablaVersion, 1, 5);

            tablaVersion.Rows[0].Cells[0].FillColor = Color.LightGray;
            tablaVersion.Rows[0].Cells[1].FillColor = Color.LightGray;
            tablaVersion.Rows[0].Cells[2].FillColor = Color.LightGray;
            tablaVersion.Rows[0].Cells[3].FillColor = Color.LightGray;
            tablaVersion.Rows[0].Cells[4].FillColor = Color.LightGray;
            tablaVersion.Rows[0].Cells[5].FillColor = Color.LightGray;            

            LlenarCeldaTabla(tablaVersion, 0, 0, 9, fontCalibri, true, "Fecha");
            LlenarCeldaTabla(tablaVersion, 0, 1, 9, fontCalibri, true, "Rev.");
            LlenarCeldaTabla(tablaVersion, 0, 2, 9, fontCalibri, true, "Descripción");
            LlenarCeldaTabla(tablaVersion, 0, 3, 9, fontCalibri, true, "Elaboró");
            LlenarCeldaTabla(tablaVersion, 0, 4, 9, fontCalibri, true, "Revisó");
            LlenarCeldaTabla(tablaVersion, 0, 5, 9, fontCalibri, true, "Aprobó");

            LlenarCeldaTabla(tablaVersion, 1, 0, 9, fontCalibri, false, DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha));
            LlenarCeldaTabla(tablaVersion, 1, 1, 9, fontCalibri, false, "1");
            LlenarCeldaTabla(tablaVersion, 1, 2, 9, fontCalibri, false, "Elaboración del Informe");

            #endregion

        }

        private string ObtenerJsonStringGraficoFlujoPotencia(GraficoModel modelFlujoPotencia)
        {
            List<string> items = new List<string>();
            items.Add(this.ObtenerGraficoFlujoPotenciaSerie(modelFlujoPotencia.Grafico.Series[0], modelFlujoPotencia.Grafico.SerieDataS[0]));
            items.Add(this.ObtenerGraficoFlujoPotenciaSerie(modelFlujoPotencia.Grafico.Series[1], modelFlujoPotencia.Grafico.SerieDataS[1]));

            string jsonDataLineas = @"
                   {
                       chart: {
                           type: 'StockChart'
                       },
                       rangeSelector: {
                            selected: 1
                       },
                       title: {
                            text: 'titleText',
                            style: {
                                fontSize: '12'
                            }
                        },

                        yAxis: [{
                                title: {
                                    text: 'yAxisTitleText'
                                },
                                min: 0
                            }],
                        xAxis: [
                            {
                                type: 'datetime',
                                endOnTick :false,
                                index: 0,
                                isX: true,
                                ordinal: true,
                                overscroll: 0,                              
                                showLastLabel: true,
                                startOnTick: false,
                                labels: {
                                    overflow: 'justify'
                                }
                            }
                        ],
                        legend: {
                            layout: 'horizontal',
                            align: 'center',
                            verticalAlign: 'bottom',
                            borderWidth: 0,
                            enabled: true,
                            itemStyle: {
                                fontSize:'9'
                            }
                        },
                        series: [seriesGrafico]
                   }";

            jsonDataLineas = jsonDataLineas.Replace("titleText", modelFlujoPotencia.Grafico.TitleText.ToUpper());
            jsonDataLineas = jsonDataLineas.Replace("yAxisTitleText", modelFlujoPotencia.Grafico.YAxixTitle[0]);
            jsonDataLineas = jsonDataLineas.Replace("yAxisTitleText1", modelFlujoPotencia.Grafico.YAxixTitle[0]);
            jsonDataLineas = jsonDataLineas.Replace("seriesGrafico", string.Join(",", items));

            return jsonDataLineas;
        }

        /// <summary>
        /// Permite Obtener la serie para el grafico de flujo de potencia
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ObtenerGraficoEvolucionEnergiaSerie(GraficoWeb grafico)
        {
            List<string> items = new List<string>();

            int index = 0;

            if (grafico.Series != null)
            {

                foreach (var serie in grafico.Series)
                {
                    List<string> data = new List<string>();

                    foreach (var itemData in grafico.SeriesData[index])
                    {
                        data.Add((itemData != null) ? itemData.ToString() : "0");
                    }

                    string itemSerie = @"{
                    name: 'nombreSerie1',
                    color: 'colorSerie1',
                    data: [dataSerie1],
                    type: 'tipoSerie1',
                    yAxis: yAxisValor
                 }";
                    itemSerie = itemSerie.Replace("nombreSerie1", serie.Name);
                    itemSerie = itemSerie.Replace("colorSerie1", serie.Color);
                    itemSerie = itemSerie.Replace("dataSerie1", string.Join(",", data));
                    itemSerie = itemSerie.Replace("tipoSerie1", serie.Type);
                    itemSerie = itemSerie.Replace("yAxisValor", serie.YAxis.ToString());

                    items.Add(itemSerie);

                    index++;
                }
            }

            return string.Join(",", items);
        }

        /// <summary>
        /// Permite Obtener la serie para el grafico de flujo de potencia
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ObtenerGraficoFlujoPotenciaSerie(RegistroSerie registro, DatosSerie[] data)
        {
            List<string> items = new List<string>();
            for (int k = 0; k < data.Length; k++)
            {
                items.Add(string.Format("[{0},{1}]", 
                    Convert.ToInt64((DateTime.SpecifyKind(data[k].X, DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds), data[k].Y));
            }
            string serie = @"{
                name: 'nombreSerie1',
                color: 'colorSerie1',
                data: [dataSerie1],
                type: 'tipoSerie1'
            }";
            serie = serie.Replace("nombreSerie1", registro.Name);
            serie = serie.Replace("colorSerie1", registro.Color);
            serie = serie.Replace("dataSerie1", string.Join(",", items));
            serie = serie.Replace("tipoSerie1", registro.Type);
            return serie;
        }

        /// <summary>
        /// Permite obtener la data de evolución de energia
        /// </summary>
        /// <param name="modelFlujoPotencia"></param>
        /// <returns></returns>
        private string ObtenerJsonStringGraficoEvolucionEnergia(GraficoModel modelEvolucionEnergia)
        {
            List<string> categorias = new List<string>();

            if (modelEvolucionEnergia.Grafico.XAxisCategories != null)
            {
                foreach (var item in modelEvolucionEnergia.Grafico.XAxisCategories)
                {
                    categorias.Add("'" + item + "'");
                }
            }
            string dataSeries = this.ObtenerGraficoEvolucionEnergiaSerie(modelEvolucionEnergia.Grafico);

            string jsonDataLineas = @"
                   {
                        chart: {
                            type: 'spline'
                        },
                        title: {
                            text: 'titleText',
                            style: {
                                fontSize: '12'
                            }
                        },
                        subtitle: {
                            text: 'subTitleText',
                            style: {
                                fontSize: '8'
                            }
                        },
                        xAxis: {
                            categories: [dataCategories],
                            labels: {                                
                                style: {
                                    color: '#000000',
                                    fontSize: '8'
                                }
                            }
                        },
                        legend: {
                            itemStyle: {
                                fontSize:'9'
                            }
                        },
                        yAxis: [
                            {
                                title: {
                                    text: 'MWh',
                                    color: 'colorEje1'
                                },
                                labels: {
                                    style: {
                                        color: 'colorEje1'
                                    }
                                }
                            },
                            {
                                title: {
                                    text: 'MW',
                                    color: 'colorEje2'
                                },
                                labels: {
                                    style: {
                                        color: 'colorEje2'
                                    }
                                },
                                opposite: true
                        }],                        
                        plotOptions: {
                            spline: {
                                marker: {
                                    enabled: true
                                },
                                dataLabels: {
                                    enabled: true,
                                    format: '{y:.3f}'
                                }
                            }
                        },
                        series:[seriesGrafico]
                   }";

            jsonDataLineas = jsonDataLineas.Replace("titleText", modelEvolucionEnergia.Grafico.TitleText);
            jsonDataLineas = jsonDataLineas.Replace("subTitleText", modelEvolucionEnergia.Grafico.Subtitle);
            jsonDataLineas = jsonDataLineas.Replace("xAxisTitleText", modelEvolucionEnergia.Grafico.XAxisTitle);
            jsonDataLineas = jsonDataLineas.Replace("yAxixTitleText", modelEvolucionEnergia.Grafico.XAxisTitle);
            jsonDataLineas = jsonDataLineas.Replace("colorEje1", (modelEvolucionEnergia.Grafico.Series!=null)?modelEvolucionEnergia.Grafico.Series[0].Color: "#000000");
            jsonDataLineas = jsonDataLineas.Replace("colorEje2", (modelEvolucionEnergia.Grafico.Series != null) ? modelEvolucionEnergia.Grafico.Series[1].Color: "#000000");
            jsonDataLineas = jsonDataLineas.Replace("dataCategories", string.Join(",", categorias));
            jsonDataLineas = jsonDataLineas.Replace("seriesGrafico", dataSeries);

            return jsonDataLineas;
        }

        /// <summary>
        /// Get imagen desde Highcharts
        /// </summary>
        /// <param name="options"></param>
        /// <param name="rutaImagen"></param> 
        /// <returns></returns>
        private static string ObtenerLinkImagenDesdeJsonString(object options, string rutaImagen)
        {

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://export.highcharts.com/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            httpWebRequest.UserAgent = "Interconexion";
            httpWebRequest.Referer = "https://coes.org.pe";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    infile = JsonConvert.SerializeObject(options)
                });
                streamWriter.Write(json);
            }

            if (System.IO.File.Exists(rutaImagen))
                System.IO.File.Delete(rutaImagen);

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.IO.Stream responseStream = httpResponse.GetResponseStream();
            Bitmap chartPNG = new Bitmap(responseStream);
            chartPNG.Save(rutaImagen);


            return rutaImagen;
        }

        /// <summary>
        /// Central una celda horizontal y verticalmente
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="fila"></param>
        /// <param name="col"></param>
        public void CentralCelda(Table tabla, int fila, int col)
        {
            tabla.Rows[fila].Cells[col].VerticalAlignment = VerticalAlignment.Center;
            tabla.Rows[fila].Cells[col].Paragraphs[0].Alignment = Alignment.center;
        }

        /// <summary>
        /// Rellena una celda con un texto
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="fila"></param>
        /// <param name="col"></param>
        /// <param name="tamLetra"></param>
        /// <param name="tipoTexto"></param>
        /// <param name="enNegrita"></param>
        /// <param name="texto"></param>
        public void LlenarCeldaTabla(Table tabla, int fila, int col, int tamLetra, string tipoTexto, bool enNegrita, string texto)
        {
            if (enNegrita)
                tabla.Rows[fila].Cells[col].Paragraphs[0].Append(texto).FontSize(tamLetra).Font(new FontFamily(tipoTexto)).Bold();
            else
                tabla.Rows[fila].Cells[col].Paragraphs[0].Append(texto).FontSize(tamLetra).Font(new FontFamily(tipoTexto));
        }

        /// <summary>
        /// Permite grabar los datos de una versión
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="semana"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int GrabarVersion(int anio, int semana, string usuario)
        {
            try
            {
                int id = 0;

                List<MeInformeInterconexionDTO> list = this.ConsultarVersiones(anio, semana);

                MeInformeInterconexionDTO entity = new MeInformeInterconexionDTO();
                entity.Infintanio = anio;
                entity.Infintestado = ConstantesAppServicio.Activo;
                entity.Infintfeccreacion = DateTime.Now;
                entity.Infintnrosemana = semana;
                entity.Infintusucreacion = usuario;
                entity.Infintversion = list.Count + 1;

                id = FactorySic.GetMeInformeInterconexionRepository().Save(entity);

                return id;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }

        }

        #region Antecedentes

        /// <summary>
        /// Listado de antecedentes
        /// </summary>
        /// <returns></returns>
        public List<MeInformeAntecedenteDTO> ObtenerAntecedentes()
        {
            return FactorySic.GetMeInformeAntecedenteRepository().List();
        }

        /// <summary>
        /// Permite obtener un antecedente por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MeInformeAntecedenteDTO ObtenerAntecedentePorId(int id)
        {
            return FactorySic.GetMeInformeAntecedenteRepository().GetById(id);
        }

        /// <summary>
        /// Permite grabar un antecedente
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarAntecedente(MeInformeAntecedenteDTO entity)
        {
            try
            {
                if(entity.Infantcodi == 0)
                {
                    FactorySic.GetMeInformeAntecedenteRepository().Save(entity);
                }
                else
                {
                    entity.Intantusumodificacion = entity.Intantusucreacion;
                    entity.Intantfecmodificacion = entity.Intantfeccreacion;
                    FactorySic.GetMeInformeAntecedenteRepository().Update(entity);
                }

                return 1;
            }
            catch(Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite eliminar un antecedente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int EliminarAntecedente(int id)
        {
            try
            {
                FactorySic.GetMeInformeAntecedenteRepository().Delete(id);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        #endregion
    }

    /// <summary>
    /// Clase para manejo de nro de semana
    /// </summary>
    public class InformacionSemana
    {
        public int NroSemana { get; set; }
        public string NombreSemana { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
             
    }

    public class GraficoModel
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Resultado { get; set; }
        public string SheetName { get; set; }        
        public GraficoWeb Grafico { get; set; }
      
    }
}
