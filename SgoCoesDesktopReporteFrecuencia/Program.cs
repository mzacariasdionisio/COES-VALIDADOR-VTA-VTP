using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Oracle.ManagedDataAccess.Client;

namespace SgoCoesDesktopReporteFrecuencia
{
    internal class Program
    {
        private static string FormatoFechaCorta = "dd/MM/yyyy";
        private static string FormatoFechaCorreoAsunto = "dd-MM-yyyy";
        private static string FormatoFechaExtendido = "yyyy-MM-dd HH:mm:ss";
        private static string FormatoFechaExtendidoPeru = "dd/MM/yyyy HH:mm:ss";
        private static string FormatoFechaExtendidoPeruArchivo = "dd_MM_yyyy";
        private static string FormatoFechaIndicadorProcedure = "yyyy-MM-dd";

        static void Main(string[] args)
        {
            DateTime fecha = DateTime.Now.Date.AddDays(-1);
            DateTime fechaDesde = fecha;
            DateTime fechaHasta = fecha.AddDays(1).AddSeconds(-1);
            string tmp = ConfigurationManager.AppSettings["DirectorioTemporal"].ToString();
            List<string> listFiles = new List<string>();

            #region PROCESANDO REPORTE
            List<DataGps> listaGps = getListadoDeGps(fechaDesde, fechaHasta);
            listaGps.ForEach(pGps => {
                string pathFull = tmp + Regex.Replace(pGps.Descripcion.Trim(), @"[^\w\s.!@$%^&*()\-\/]+", "_") + "_C_" + pGps.Codigo + "_F_" + pGps.FechaInicial.ToString(FormatoFechaExtendidoPeruArchivo) + ".xlsx";
                calcular(pGps);
                crearExcel(pathFull, pGps);
                listFiles.Add(pathFull);
            });
            #endregion

            enviarCorreo(listFiles, fechaDesde.ToString(FormatoFechaCorta));

            listFiles.ForEach(fileFull =>
            {
                 eliminarExcel(fileFull);
            });
        }

        private static void enviarCorreo(List<string> listFilesFullPath, string detalle)
        {
            using (MailMessage correo = new MailMessage())
            {
                correo.Subject = String.Format("REPORTE DE FRECUENCIA - " + detalle);
                correo.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString());
                foreach (string mailTo in ConfigurationManager.AppSettings["MailTo"].ToString().Split(';').ToList())
                {
                    correo.To.Add(new MailAddress(mailTo));
                }
                foreach (string mailBcc in ConfigurationManager.AppSettings["MailBcc"].ToString().Split(';').ToList())
                {
                    correo.Bcc.Add(new MailAddress(mailBcc));
                }
                listFilesFullPath.ForEach(fileFull =>
                {
                    Attachment data = new Attachment(fileFull, System.Net.Mime.MediaTypeNames.Application.Octet);
                    System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(fileFull);
                    correo.Attachments.Add(data);
                });
                new SmtpClient()
                {
                    Host = ConfigurationManager.AppSettings["EmailServer"].ToString()
                }.Send(correo);
            }
        }

        private static void eliminarExcel(string fileNameFull)
        {
            if (File.Exists(fileNameFull))
            {
                File.Delete(fileNameFull);
            }
        }

        private static void crearExcel(string pathFull, DataGps gps)
        {
            XLWorkbook wbook = new XLWorkbook();

            #region AGREGANDO HOJAS AL DOCUMENTO EXCEL
            IXLWorksheet wsGeneral = wbook.AddWorksheet("GENERAL");
            IXLWorksheet wsData = wbook.AddWorksheet("DATA");
            IXLWorksheet wsReporte = wbook.AddWorksheet("REPORTE");
            IXLWorksheet wsIndicadores = wbook.AddWorksheet("INDICADORES");
            #endregion

            #region HOJA GENERAL
            wsGeneral.Cell("B2").Value = "COES - SEIN";
            wsGeneral.Cell("B2").Style.Font.Bold = true;
            wsGeneral.Cell("B4").Value = "GPS";
            wsGeneral.Cell("B4").Style.Font.Bold = true;
            wsGeneral.Cell("B5").Value = gps.Descripcion.Trim();
            wsGeneral.Cell("C5").Value = gps.Codigo;
            wsGeneral.Cell("B7").Value = "FECHA INICIAL";
            wsGeneral.Cell("B7").Style.Font.Bold = true;
            wsGeneral.Cell("B8").Value = gps.FechaInicial.ToString(FormatoFechaExtendidoPeru);
            wsGeneral.Cell("B10").Value = "FECHA FINAL";
            wsGeneral.Cell("B10").Style.Font.Bold = true;
            wsGeneral.Cell("B11").Value = gps.FechaFinal.ToString(FormatoFechaExtendidoPeru);
            wsGeneral.Column("B").AdjustToContents();
            #endregion

            #region HOJA DATA
            int wsDataRow = 1;
            wsData.Cell("A" + wsDataRow).Value = "FECHA Y HORA";
            wsData.Cell("A" + wsDataRow).Style.Font.Bold = true;
            wsData.Cell("B" + wsDataRow).Value = "INTERVALO POR POSICIÓN";
            wsData.Cell("B" + wsDataRow).Style.Font.Bold = true;
            wsData.Cell("C" + wsDataRow).Value = "PERIODO";
            wsData.Cell("C" + wsDataRow).Style.Font.Bold = true;
            wsData.Cell("D" + wsDataRow).Value = "DÍA";
            wsData.Cell("D" + wsDataRow).Style.Font.Bold = true;
            wsData.Cell("E" + wsDataRow).Value = "INTERVALO DE TIEMO ";
            wsData.Cell("E" + wsDataRow).Style.Font.Bold = true;
            wsData.Cell("F" + wsDataRow).Value = "HORAS Y MINUTOS";
            wsData.Cell("F" + wsDataRow).Style.Font.Bold = true;
            wsData.Cell("G" + wsDataRow).Value = "FRECUENCIA";
            wsData.Cell("G" + wsDataRow).Style.Font.Bold = true;

            List<DataPorSegundo> dataDeGps = getDataDeGps(gps);
            dataDeGps.ForEach(d => {
                ++wsDataRow;
                wsData.Cell("A" + wsDataRow).Value = d.FechaHora;
                wsData.Cell("B" + wsDataRow).Value = d.IntervaloPorPosicion;
                wsData.Cell("C" + wsDataRow).Value = d.Periodo;
                wsData.Cell("D" + wsDataRow).Value = d.Dia;
                wsData.Cell("E" + wsDataRow).Value = d.IntervaloPorTiempo;
                wsData.Cell("F" + wsDataRow).Value = d.Hora;
                wsData.Cell("G" + wsDataRow).Value = d.Frecuencia;
            });
            #endregion

            #region HOJA REPORTE
            int wsReporteRow = 1;
            wsReporte.Cell("A" + wsReporteRow).Value = "FECHA Y HORA";
            wsReporte.Cell("A" + wsReporteRow).Style.Font.Bold = true;
            wsReporte.Cell("B" + wsReporteRow).Value = "FRECUENCIA";
            wsReporte.Cell("B" + wsReporteRow).Style.Font.Bold = true;
            wsReporte.Cell("C" + wsReporteRow).Value = "SUBITA";
            wsReporte.Cell("C" + wsReporteRow).Style.Font.Bold = true;
            wsReporte.Cell("D" + wsReporteRow).Value = "DESVIACION";
            wsReporte.Cell("D" + wsReporteRow).Style.Font.Bold = true;
            wsReporte.Cell("E" + wsReporteRow).Value = "MAXIMO";
            wsReporte.Cell("E" + wsReporteRow).Style.Font.Bold = true;
            wsReporte.Cell("F" + wsReporteRow).Value = "MINIMO";
            wsReporte.Cell("F" + wsReporteRow).Style.Font.Bold = true;
            wsReporte.Cell("G" + wsReporteRow).Value = "TENSION";
            wsReporte.Cell("G" + wsReporteRow).Style.Font.Bold = true;
            wsReporte.Cell("H" + wsReporteRow).Value = "SEG_DISP";
            wsReporte.Cell("H" + wsReporteRow).Style.Font.Bold = true;

            List<DataPorMinuto> dataPorMinuto = getDataReporte(gps);
            dataPorMinuto.ForEach(d => {
                ++wsReporteRow;
                wsReporte.Cell("A" + wsReporteRow).Value = d.FechaHora;
                wsReporte.Cell("B" + wsReporteRow).Value = d.Frecuencia;
                wsReporte.Cell("C" + wsReporteRow).Value = d.Subita;
                wsReporte.Cell("D" + wsReporteRow).Value = d.Desviacion;
                wsReporte.Cell("E" + wsReporteRow).Value = d.Maximo;
                wsReporte.Cell("F" + wsReporteRow).Value = d.Minimo;
                wsReporte.Cell("G" + wsReporteRow).Value = d.Tension;
                wsReporte.Cell("H" + wsReporteRow).Value = d.SegDisp;
            });
            #endregion

            #region HOJA INDICADORES
            int wsIndicadoresRow = 2;
            wsIndicadores.Cell("B" + wsIndicadoresRow).Value = "Máxima Frecuencia";
            wsIndicadores.Cell("B" + wsIndicadoresRow).Style.Font.Bold = true;
            List<DataIndicadores> DataIndicadores = getIndicadores(gps);
            DataIndicadores.ForEach(d => {
                if(d.Tipo == "M")
                {
                    ++wsIndicadoresRow;
                    wsIndicadores.Cell("B" + wsIndicadoresRow).Value = d.Fecha;
                    wsIndicadores.Cell("C" + wsIndicadoresRow).Value = d.Hora;
                    wsIndicadores.Cell("D" + wsIndicadoresRow).Value = d.Valor;
                }
            });
            wsIndicadoresRow = wsIndicadoresRow + 2;
            wsIndicadores.Cell("B" + wsIndicadoresRow).Value = "Mínima Frecuencia";
            wsIndicadores.Cell("B" + wsIndicadoresRow).Style.Font.Bold = true;
            DataIndicadores.ForEach(d => {
                if (d.Tipo == "m")
                {
                    ++wsIndicadoresRow;
                    wsIndicadores.Cell("B" + wsIndicadoresRow).Value = d.Fecha;
                    wsIndicadores.Cell("C" + wsIndicadoresRow).Value = d.Hora;
                    wsIndicadores.Cell("D" + wsIndicadoresRow).Value = d.Valor;
                }
            });
            wsIndicadoresRow = wsIndicadoresRow + 2;
            wsIndicadores.Cell("B" + wsIndicadoresRow).Value = "Transgresiones Súbitas";
            wsIndicadores.Cell("B" + wsIndicadoresRow).Style.Font.Bold = true;
            DataIndicadores.ForEach(d => {
                if (d.Tipo == "U")
                {
                    ++wsIndicadoresRow;
                    wsIndicadores.Cell("B" + wsIndicadoresRow).Value = d.Fecha;
                    wsIndicadores.Cell("C" + wsIndicadoresRow).Value = d.Hora;
                    wsIndicadores.Cell("D" + wsIndicadoresRow).Value = d.Valor;
                }
            });
            wsIndicadoresRow = wsIndicadoresRow + 2;
            wsIndicadores.Cell("B" + wsIndicadoresRow).Value = "Transgresiones Sostenidas";
            wsIndicadores.Cell("B" + wsIndicadoresRow).Style.Font.Bold = true;
            DataIndicadores.ForEach(d => {
                if (d.Tipo == "O")
                {
                    ++wsIndicadoresRow;
                    wsIndicadores.Cell("B" + wsIndicadoresRow).Value = d.Fecha;
                    wsIndicadores.Cell("C" + wsIndicadoresRow).Value = d.Hora;
                    wsIndicadores.Cell("D" + wsIndicadoresRow).Value = d.Valor;
                }
            });
            #endregion

            wbook.SaveAs(pathFull);
        }

        private static void calcular(DataGps gps)
        {
            Console.WriteLine("Recuperando indicadores...");
            List<DataIndicadores> indicadores = new List<DataIndicadores>();
            string connectionStringsSicoes = ConfigurationManager.AppSettings["database-sicoes"];

            using (OracleConnection con = new OracleConnection(connectionStringsSicoes))
            {
                OracleCommand command = new OracleCommand();
                command.Connection = con;
                command.CommandText = "sp_fr_indicadores";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("pGPS", OracleDbType.Int32).Value = gps.Codigo;
                command.Parameters.Add("pFecha", OracleDbType.Date).Value = gps.FechaInicial;

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        private static List<DataGps> getListadoDeGps(DateTime fechaDesde, DateTime fechaHasta)
        {
            Console.WriteLine("Recuperando listado de GPS's...");
            List<DataGps> listaGps = new List<DataGps>();
            string query = "SELECT GPSCODI, NOMBRE FROM ME_GPS WHERE GPSESTADO = 'A'";
            string connectionStringsSicoes = ConfigurationManager.AppSettings["database-sicoes"];

            using (OracleConnection con = new OracleConnection(connectionStringsSicoes))
            {
                con.Open();
                using (OracleCommand command = new OracleCommand(query, con))
                {
                    using (OracleDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            DataGps dataGps = new DataGps();
                            dataGps.Codigo = Convert.ToInt32(dr["GPSCODI"]);
                            dataGps.Descripcion = Convert.ToString(dr["NOMBRE"]).Trim();
                            dataGps.FechaInicial = fechaDesde;
                            dataGps.FechaFinal = fechaHasta;
                            listaGps.Add(dataGps);
                            Console.WriteLine(dataGps.getInfo());
                        }
                    }
                }
                con.Close();
            }

            return listaGps;
        }

        private static List<DataPorSegundo> getDataDeGps(DataGps gps)
        {
            Console.WriteLine("Recuperando data por segundo...");
            List<DataPorSegundo> listaPorSegundo = new List<DataPorSegundo>();
            string query = string.Format("SELECT FECHAHORA, INTERVALO, C_PERIODO, C_DIA, (INTERVALO - 1) HORA, C_HORA, C_FRECUENCIA FROM\r\n(\r\nSELECT \r\nDENSE_RANK() OVER (PARTITION BY FECHAHORA ORDER BY ROWNUM ASC) INTERVALO, FECHAHORA, to_char(FECHAHORA, 'YYYY-MM') C_PERIODO, to_char(FECHAHORA, 'DD') C_DIA, to_char(FECHAHORA, 'HH24:MI') C_HORA, C_FRECUENCIA\r\nFROM F_LECTURA fl\r\nUNPIVOT\r\n(\r\n  C_FRECUENCIA\r\n  for INTERVALO in (h0, h1, h2, h3, h4, h5, h6, h7, h8, h9, h10, h11, h12, h13, h14, h15, h16, h17, h18, h19, h20,\r\n  h21, h22, h23, h24, h25, h26, h27, h28, h29, h30, h31, h32, h33, h34, h35, h36, h37, h38, h39, h40,\r\n  h41, h42, h43, h44, h45, h46, h47, h48, h49, h50, h51, h52, h53, h54, h55, h56, h57, h58, h59 )\r\n)\r\nWHERE \r\nFECHAHORA BETWEEN TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS') AND GPSCODI = {2}  \r\nORDER BY ROWNUM DESC\r\n) ORDER BY FECHAHORA ASC, INTERVALO ASC", gps.FechaInicial.ToString(FormatoFechaExtendido), gps.FechaFinal.ToString(FormatoFechaExtendido), gps.Codigo);
            string connectionStringsSicoes = ConfigurationManager.AppSettings["database-sicoes"];

            using (OracleConnection con = new OracleConnection(connectionStringsSicoes))
            {
                con.Open();
                using (OracleCommand command = new OracleCommand(query, con))
                {
                    using (OracleDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            DataPorSegundo dataPorSegundo = new DataPorSegundo();
                            dataPorSegundo.FechaHora = Convert.ToString(dr["FECHAHORA"]);
                            dataPorSegundo.IntervaloPorPosicion = Convert.ToInt32(dr["INTERVALO"]);
                            dataPorSegundo.Periodo = Convert.ToString(dr["C_PERIODO"]);
                            dataPorSegundo.Dia = Convert.ToString(dr["C_DIA"]);
                            dataPorSegundo.IntervaloPorTiempo = Convert.ToInt32(dr["HORA"]);
                            dataPorSegundo.Hora = Convert.ToString(dr["C_HORA"]);
                            dataPorSegundo.Frecuencia = Convert.ToDouble(dr["C_FRECUENCIA"]);
                            listaPorSegundo.Add(dataPorSegundo);
                            Console.WriteLine(dataPorSegundo.getInfo());
                        }
                    }
                }
                con.Close();
            }

            return listaPorSegundo;
        }

        private static List<DataPorMinuto> getDataReporte(DataGps gps)
        {
            Console.WriteLine("Recuperando data por minuto...");
            List<DataPorMinuto> listaPorMinuto = new List<DataPorMinuto>();
            string query = string.Format("SELECT FECHAHORA, NVL(H0, 0) C_FRECUENCIA, VSF C_SUBITA, DESV C_DESVIACION, MAXIMO, MINIMO, VOLTAJE C_TENSION, NUM C_SEG_DISP FROM F_LECTURA fl\r\nWHERE \r\nFECHAHORA BETWEEN TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS') AND\r\nfl.GPSCODI = {2}  \r\nORDER BY FECHAHORA ASC", gps.FechaInicial.ToString(FormatoFechaExtendido), gps.FechaFinal.ToString(FormatoFechaExtendido), gps.Codigo);
            string connectionStringsSicoes = ConfigurationManager.AppSettings["database-sicoes"];

            using (OracleConnection con = new OracleConnection(connectionStringsSicoes))
            {
                con.Open();
                using (OracleCommand command = new OracleCommand(query, con))
                {
                    using (OracleDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            DataPorMinuto dataPorMinuto = new DataPorMinuto();
                            dataPorMinuto.FechaHora = Convert.ToString(dr["FECHAHORA"]);
                            dataPorMinuto.Frecuencia = Convert.ToDouble(dr["C_FRECUENCIA"]);
                            dataPorMinuto.Subita = Convert.ToDouble(dr["C_SUBITA"]);
                            dataPorMinuto.Desviacion = Convert.ToDouble(dr["C_DESVIACION"]);
                            dataPorMinuto.Maximo = Convert.ToDouble(dr["MAXIMO"]);
                            dataPorMinuto.Minimo = Convert.ToDouble(dr["MINIMO"]);
                            dataPorMinuto.Tension = Convert.ToString(dr["C_TENSION"]);
                            dataPorMinuto.SegDisp = Convert.ToString(dr["C_SEG_DISP"]);
                            listaPorMinuto.Add(dataPorMinuto);
                            Console.WriteLine(dataPorMinuto.getInfo());
                        }
                    }
                }
                con.Close();
            }

            return listaPorMinuto;
        }

        private static List<DataIndicadores> getIndicadores(DataGps gps)
        {
            Console.WriteLine("Recuperando indicadores...");
            List<DataIndicadores> indicadores = new List<DataIndicadores>();
            string query = string.Format("SELECT\r\n\t to_char(FECHAHORA, 'YYYY-MM-DD') FECHA, to_char(FECHAHORA, 'HH24:MI:SS') HORA, INDICVALOR, INDICCODI \r\nFROM\r\n\tf_indicador\r\nWHERE\r\n\tFECHAHORA BETWEEN TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS') AND GPS = {2}  \r\nORDER BY\r\n\tfechahora ASC", gps.FechaInicial.ToString(FormatoFechaExtendido), gps.FechaFinal.ToString(FormatoFechaExtendido), gps.Codigo);
            string connectionStringsSicoes = ConfigurationManager.AppSettings["database-sicoes"];

            using (OracleConnection con = new OracleConnection(connectionStringsSicoes))
            {
                con.Open();
                using (OracleCommand command = new OracleCommand(query, con))
                {
                    using (OracleDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            DataIndicadores dataIndicadores = new DataIndicadores();
                            dataIndicadores.Fecha = Convert.ToString(dr["FECHA"]);
                            dataIndicadores.Hora = Convert.ToString(dr["HORA"]);
                            dataIndicadores.Valor = Convert.ToDouble(dr["INDICVALOR"]);
                            dataIndicadores.Tipo = Convert.ToString(dr["INDICCODI"]);
                            indicadores.Add(dataIndicadores);
                            Console.WriteLine(dataIndicadores.getInfo());
                        }
                    }
                }
                con.Close();
            }

            return indicadores;
        }
    }
}
