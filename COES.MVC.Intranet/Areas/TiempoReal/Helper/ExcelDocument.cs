using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Interconexiones.Helper;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.TiempoReal;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace COES.MVC.Intranet.Areas.TiempoReal.Helper
{
    public class ExcelDocument
    {
        /// <summary>
        /// Permite exportar la frecuencia consultada  (SP7)
        /// </summary>
        /// <param name="lista">Lista de datos de Frecuencia</param>
        /// <param name="gpsNombre">Nombre de GPS</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        public static void GenerarArchivoFrecuenciaSP7(List<FLecturaSp7DTO> list, string gpsNombre, string fechaIni, string fechaFin)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteFrecuencia;                
                
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }


                int row = 12;
                
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {                    
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaReporteExcel);

                    if (ws != null)
                    {

                        ExcelRange rg = ws.Cells[7, 2, 9, 2];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[11, 2, 11, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;


                        ws.Cells[5, 2].Value = "Reporte de Frecuencia";

                        ws.Cells[7, 2].Value = "GPS";
                        ws.Cells[8, 2].Value = "H. Inicio";
                        ws.Cells[9, 2].Value = "H. Fin";


                        ws.Cells[7, 3].Value = gpsNombre;
                        ws.Cells[8, 3].Value = fechaIni;
                        ws.Cells[9, 3].Value = fechaFin;

                        ws.Cells[11, 2].Value = "Hora";
                        ws.Cells[11, 3].Value = "Frecuencia";


                        if (list.Count > 0)
                        {
                            foreach (FLecturaSp7DTO item in list)
                            {
                                ws.Cells[row, 2].Value = item.Fechahora.ToString(Constantes.FormatoHora);
                                ws.Cells[row, 3].Value = item.H0;
                                row++;
                            }
                            /*
                            column = index;
                            row++;

                            foreach (RsfItemModel item in list[0].ListItems)
                            {
                                ws.Cells[row, column].Value = "MAN";
                                column += 2;
                                ws.Cells[row, column].Value = "AUTO";
                                column += 2;
                            }

                            row++;

                            foreach (RsfModel item in list)
                            {
                                column = 2;

                                ws.Cells[row, column].Value = item.DesURS;
                                ws.Cells[row, column + 1].Value = item.Empresa;
                                ws.Cells[row, column + 2].Value = item.Central;
                                ws.Cells[row, column + 3].Value = item.Equipo;

                                column = column + 4;
                                foreach (RsfItemModel child in item.ListItems)
                                {
                                    ws.Cells[row, column++].Value = child.Manual.ToString();
                                    ws.Cells[row, column++].Value = (child.IndManual == Constantes.SI) ? Constantes.TextoSI : string.Empty;
                                    ws.Cells[row, column++].Value = child.Automatico.ToString();
                                    ws.Cells[row, column++].Value = (child.IndAutomatico == Constantes.SI) ? Constantes.TextoSI : string.Empty;
                                }
                                row++;
                            }
                            */
                            /*
                            for (int k = column; k <= 69; k++)
                            {
                                ws.Column(k).Hidden = true;
                            }

                            for (int k = row; k <= 68; k++)
                            {
                                ws.Row(k).Hidden = true;
                            }*/


                            //int columnaMax = 7;

                            //ws.Column(1).Width = 7;

                            //rg = ws.Cells[9, 2, index, columnaMax];

                            rg = ws.Cells[5, 2, 5, 2];
                            rg.Style.Font.Bold = true;


                            rg = ws.Cells[7, 3, 9, 3];
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
        /// Permite exportar la frecuencia consultada  (SP7)
        /// </summary>
        /// <param name="lista">Lista de datos de Frecuencia</param>
        /// <param name="gpsNombre">Nombre de GPS</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        public static void GenerarArchivoCircularSP7(List<DatosSP7DTO> list, string canalNombre, string fechaIni, string fechaFin)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteCircular;


                FileInfo newFile = new FileInfo(file);


                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }


                int row = 12;

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaReporteExcel);

                    if (ws != null)
                    {

                        ExcelRange rg = ws.Cells[7, 2, 9, 2];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        rg = ws.Cells[11, 2, 11, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;


                        ws.Cells[5, 2].Value = "Reporte de Datos SCADA";

                        ws.Cells[7, 2].Value = "Canal";
                        ws.Cells[8, 2].Value = "H. Inicio";
                        ws.Cells[9, 2].Value = "H. Fin";


                        ws.Cells[7, 3].Value = canalNombre;
                        ws.Cells[8, 3].Value = fechaIni;
                        ws.Cells[9, 3].Value = fechaFin;

                        ws.Cells[11, 2].Value = "Fecha";
                        ws.Cells[11, 3].Value = "Hora Origen";
                        ws.Cells[11, 4].Value = "Hora Llegada";
                        ws.Cells[11, 5].Value = "Calidad";
                        ws.Cells[11, 6].Value = "Valor";


                        if (list.Count > 0)
                        {
                            foreach (DatosSP7DTO item in list)
                            {
                                ws.Cells[row, 2].Value = item.Fecha.ToString(Constantes.FormatoFecha);
                                ws.Cells[row, 3].Value = item.Fecha.ToString(Constantes.FormatoHora);
                                ws.Cells[row, 4].Value = item.FechaSistema.ToString(Constantes.FormatoHora);
                                ws.Cells[row, 5].Value = item.CalidadTexto;
                                ws.Cells[row, 6].Value = item.Valor;
                                row++;
                            }

                            rg = ws.Cells[5, 2, 5, 2];
                            rg.Style.Font.Bold = true;


                            rg = ws.Cells[7, 3, 9, 3];
                            //rg.AutoFitColumns();

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
        /// Permite exportar la frecuencia consultada  (SP7)
        /// </summary>
        /// <param name="lista">Lista de datos de Frecuencia</param>
        /// <param name="gpsNombre">Nombre de GPS</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        public static void GenerarArchivoCircularSP7MultiplesCanales(List<int> listCanalCodi, List<DatosSP7DTO> list, string canalNombre, string fechaIni, string fechaFin)
        {
            ScadaSp7AppServicio servScadaSp7 = new ScadaSp7AppServicio();
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteCircular;


                FileInfo newFile = new FileInfo(file);


                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }


                int row = 12;

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    foreach (int pCanal in listCanalCodi)
                    {
                        var obCanal = servScadaSp7.GetByIdTrCanalSp7(pCanal);
                        row = 12;
                        string CanalNombre = obCanal.Canalnomb;
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(pCanal + "_" + CanalNombre.Replace(" ", string.Empty).Replace("/", "."));

                        if (ws != null)
                        {

                            ExcelRange rg = ws.Cells[7, 2, 9, 2];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            rg = ws.Cells[11, 2, 11, 6];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;


                            ws.Cells[5, 2].Value = "Reporte de Datos SCADA";

                            ws.Cells[7, 2].Value = "Canal";
                            ws.Cells[8, 2].Value = "H. Inicio";
                            ws.Cells[9, 2].Value = "H. Fin";


                            ws.Cells[7, 3].Value = obCanal.Canalnomb + "       " + obCanal.Canaliccp;
                            ws.Cells[8, 3].Value = fechaIni;
                            ws.Cells[9, 3].Value = fechaFin;

                            ws.Cells[11, 2].Value = "Fecha";
                            ws.Cells[11, 3].Value = "Hora Origen";
                            ws.Cells[11, 4].Value = "Hora Llegada";
                            ws.Cells[11, 5].Value = "Calidad";
                            ws.Cells[11, 6].Value = "Valor";


                            if (list.Count > 0)
                            {
                                foreach (DatosSP7DTO item in list)
                                {
                                    if (pCanal == item.Canalcodi)
                                    {
                                        ws.Cells[row, 2].Value = item.FechaSistema.ToString(Constantes.FormatoFecha);
                                        ws.Cells[row, 3].Value = item.Fecha.ToString(Constantes.FormatoHora);
                                        ws.Cells[row, 4].Value = item.FechaSistema.ToString(Constantes.FormatoHora);
                                        ws.Cells[row, 5].Value = item.CalidadTexto;
                                        ws.Cells[row, 6].Value = item.Valor;
                                        row++;
                                    }
                                }

                                rg = ws.Cells[5, 2, 5, 2];
                                rg.Style.Font.Bold = true;


                                rg = ws.Cells[7, 3, 9, 3];
                                //rg.AutoFitColumns();
                                
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
                        }
                        xlPackage.Save();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite exportar la frecuencia consultada  (SP7)
        /// </summary>
        /// <param name="lista">Lista de datos de Frecuencia</param>
        /// <param name="gpsNombre">Nombre de GPS</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        public static void GenerarArchivoCircularSP7MultiplesCanalesCSV(List<int> listCanalCodi, List<DatosSP7DTO> list, string canalNombre, string fechaIni, string fechaFin)
        {
            ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
            TiempoRealAppServicio servTiempoReal = new TiempoRealAppServicio();
            ScadaSp7AppServicio servScadaSp7 = new ScadaSp7AppServicio();
            DateTime fechaIniFormat = DateTime.ParseExact(fechaIni, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
            DateTime fechaFinFormat = DateTime.ParseExact(fechaFin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);

            string fileZip = ConfigurationManager.AppSettings[RutaDirectorio.ArchivoReporteCircularZip];
            string directorio = ConfigurationManager.AppSettings[RutaDirectorio.DirectorioReporteCircular];

            DirectoryInfo di = new DirectoryInfo(directorio);
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(".csv") || file.Name.Contains(".zip"))
                    file.Delete();
            }

            Parallel.ForEach(listCanalCodi, new ParallelOptions { MaxDegreeOfParallelism = 500 }, (pCanalCodi, state) =>
            {

                try
                {
                    List<TrCircularSp7DTO> listaCircularSp7 = servTiempoReal.BuscarOperacionesRangoCircularSp7(pCanalCodi.ToString(), fechaIniFormat, fechaFinFormat, -1, -1).ToList();

                    //model.ListaTrCircularSp7Grafica = ConvertirDto(listaCircularSp7);
                    string cabeceraCSV = "\"CANALCODI\",\"FHSISTEMA\",\"VALOR\",\"CALIDAD\",\"FHEMPRESA\"" + Environment.NewLine;
                    File.WriteAllText(directorio + pCanalCodi + ".csv", cabeceraCSV);

                    if (listaCircularSp7.Count > 0)
                    {
                        foreach (TrCircularSp7DTO item in listaCircularSp7)
                        {
                            if (pCanalCodi == item.Canalcodi)
                            {
                                string detalleCSV = "\"" + item.Canalcodi + "\",\"" + item.Canalfhsist.ToString(Constantes.FormatoFechaFull) + "\",\"" + item.Canalvalor + "\",\"" + item.Canalcalidad + "\",\"" + item.Canalfechahora.ToString(Constantes.FormatoFechaFull) + "\"" + Environment.NewLine;
                                File.AppendAllText(directorio + pCanalCodi + ".csv", detalleCSV);
                            }
                        }
                    }
                }

                catch (Exception e)
                {
                    exceptions.Enqueue(e);
                }
                //}
            });

            if (!exceptions.IsEmpty)
            {
                throw new AggregateException(exceptions);
            }

            if (File.Exists(fileZip))
            {
                try
                {
                    File.Delete(fileZip);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            ZipFile.CreateFromDirectory(directorio, fileZip);

        }

        /// <summary>
        /// Permite exportar los datos 15"/30" SCADA (SP7)
        /// </summary>
        /// <param name="lect15min">indica si el reporte es cada 15 minutos</param>
        /// <param name="list">Lista de datos de Lecturas 15"</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        public static void GenerarArchivoScadaSP7(bool lect15min, List<MeScadaSp7DTO> list, string fechaIni, string fechaFin)
        {
            try
            {

                int saltoLectura=1;
                if (lect15min)
                    saltoLectura=1;
                else
                    saltoLectura=2;
                
                string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteSCADA;
                FileInfo newFile = new FileInfo(file);


                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }
                
                int row = 11;
                int rowini = row;

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaReporteExcel);

                    if (ws != null)
                    {

                        ExcelRange rg = ws.Cells[7, 2, 8, 2];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;                                            


                        ws.Cells[5, 2].Value = "Reporte de Datos SCADA";
                        ws.Cells[7, 2].Value = "Inicio";
                        ws.Cells[8, 2].Value = "Fin";
                        ws.Cells[7, 3].Value = fechaIni;
                        ws.Cells[8, 3].Value = fechaFin;

                        if (list.Count > 0)
                        {
                            DateTime Bloque = new DateTime(2016,01,01);
                            //escribe bloque                            
                            int rowRec=rowini+4;
                            int col = 2;

                            for (int i = saltoLectura; i <= 96; i += saltoLectura)
                            {
                                Bloque = Bloque.AddMinutes(15 * saltoLectura);
                                ws.Cells[rowRec, col].Value = Bloque.ToString(Constantes.FormatoHoraMinuto);
                                rowRec++;
                            }

                            rowRec--;

                            rg = ws.Cells[11, 2, rowRec, col];                            
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            rowRec++;


                            foreach (MeScadaSp7DTO item in list)
                            {
                                col++;
                                rowRec = rowini-1;

                                ws.Cells[rowRec, col].Value = item.Canalcodi;
                                rowRec++;

                                ws.Cells[rowRec, col].Value = item.Zonanomb;
                                rowRec++;

                                ws.Cells[rowRec, col].Value = item.Canalnomb;
                                rowRec++;

                                ws.Cells[rowRec, col].Value = item.Canalunidad;
                                rowRec++;

                                ws.Cells[rowRec, col].Value = item.Medifecha.ToString(Constantes.FormatoFecha);
                                rowRec++;


                                for (int i = saltoLectura; i <= 96; i += saltoLectura)
                                {
                                    ws.Cells[rowRec, col].Value = item.GetType().GetProperty("H" + i).GetValue(item, null);
                                    rowRec++;
                                }                                
                            }

                            rg = ws.Cells[11, 3, 14, col];
                            //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;
                            rg = ws.Cells[5, 2, 5, 2];
                            rg.Style.Font.Bold = true;
                            rg = ws.Cells[7, 3, 9, 3];
                            //rg.AutoFitColumns();

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