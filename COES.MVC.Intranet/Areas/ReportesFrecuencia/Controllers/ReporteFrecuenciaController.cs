using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using COES.MVC.Intranet.Controllers;
using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Dominio.DTO.ReportesFrecuencia;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.IO.Compression;
using COES.MVC.Intranet.Helper;
using System.Globalization;
using COES.Servicios.Aplicacion.CPPA.Helper;
using System.Configuration;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class ReporteFrecuenciaController : BaseController
    {
        LecturaAppServicio servicio = new LecturaAppServicio();
        public ActionResult Index()
        {
            ReporteFrecuenciaModel model = new ReporteFrecuenciaModel();
            List<ReporteEquipoGPS> lista = new List<ReporteEquipoGPS>();
            
            //DateTime date = DateTime.Now;
            //var fechaHoraPrimerDiaMes = new DateTime(date.Year, date.Month - 1, 1, 0, 0, 0);
            //var ultDiaMesAnterior = fechaHoraPrimerDiaMes.AddMonths(1).AddDays(-1);
            //var fechaHoraUltimoDiaMes = new DateTime(ultDiaMesAnterior.Year, ultDiaMesAnterior.Month, ultDiaMesAnterior.Day, 23, 59, 59);

            //model.FechaIni = fechaHoraPrimerDiaMes.ToString();
            //model.FechaFin = fechaHoraUltimoDiaMes.ToString();

            DateTime date = DateTime.Now;
            model.FechaInicial = date;
            int intAnio = 0;
            int intMes = 0;
            if (model.FechaInicial.Month == 1)
            {
                intAnio = date.Year - 1;
                intMes = 12;
            } else 
            {
                intAnio = date.Year;
                intMes = model.FechaInicial.Month - 1;
            }
            model.FechaInicial = new DateTime(intAnio, intMes, 1, 0, 0, 0);
            var fechaFinal = model.FechaInicial.AddMonths(1).AddDays(-1);
            model.FechaFinal = new DateTime(fechaFinal.Year, fechaFinal.Month, fechaFinal.Day, 23, 59, 59);

            model.ListaGPS = lista;

            List<EquipoGPSDTO> listaEquiposSelect = new List<EquipoGPSDTO>();
            listaEquiposSelect = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").OrderBy(x => x.NombreEquipo).ToList();
            TempData["ListaEquipos"] = new SelectList(listaEquiposSelect, "GPSCODI", "NOMBREEQUIPO");

            List<SelectListItem> listaOficial = new List<SelectListItem>();
            listaOficial.Add(new SelectListItem { Text = "NO", Value = "N" });
            listaOficial.Add(new SelectListItem { Text = "SI", Value = "S" });
            TempData["ListaOficial"] = listaOficial;

            return View(model);
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesReportesFrecuencia.ModuloManualUsuario;
            string nombreArchivo = ConstantesReportesFrecuencia.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesReportesFrecuencia.FolderRaizAutomatizacionModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Lista(ReporteFrecuenciaModel model)
        {
            string mensajeError = "";
            int nroDias = 0;
            List<ReporteEquipoGPS> listaGPS = new List<ReporteEquipoGPS>();
            if ((model.FechaIni == null) || (model.FechaFin == null))
            {
                mensajeError = "Las fechas son incorrectas.";
                TempData["sMensajeExito"] = mensajeError;
            }
            else if (DateTime.Parse(model.FechaIni) > DateTime.Parse(model.FechaFin))
            {
                mensajeError = "La fecha final debe ser mayor que la fecha inicial.";
                TempData["sMensajeExito"] = mensajeError;
            }
            else
            {
                model.FechaInicial = DateTime.Parse(model.FechaIni);
                model.FechaFinal = DateTime.Parse(model.FechaFin);

                var equiposGPS = new ReporteFrecuenciaAppServicio().ObtenerGPSs(new ReporteFrecuenciaParam() { FechaInicial = model.FechaInicial, FechaFinal = model.FechaFinal, IdGPS =  model.IdGPS, IndOficial= model.IndOficial  });

                listaGPS = equiposGPS.Select(gps => new ReporteEquipoGPS()
                {
                    Id = gps.GPSCodi,
                    GPS = gps.NombreEquipo,
                    Oficial = gps.GPSOficial
                }).ToList();

                nroDias = Convert.ToInt32((model.FechaFinal - model.FechaInicial).TotalDays);
            }

            ViewBag.TotalDays = Convert.ToInt32(nroDias);
            model.ListaGPS = listaGPS;
            model.mensaje = mensajeError;
            model.mesInicio = this.mesDesc(model.FechaInicial.Month);
            return PartialView("Lista", model);
        }

        public string mesDesc(int numMes) 
        {
            string strResultado = "";
            switch (numMes)
            {
                case 1:
                    strResultado = "Enero";
                    break;
                case 2:
                    strResultado = "Febrero";
                    break;
                case 3:
                    strResultado = "Marzo";
                    break;
                case 4:
                    strResultado = "Abril";
                    break;
                case 5:
                    strResultado = "Mayo";
                    break;
                case 6:
                    strResultado = "Junio";
                    break;
                case 7:
                    strResultado = "Julio";
                    break;
                case 8:
                    strResultado = "Agosto";
                    break;
                case 9:
                    strResultado = "Setiembre";
                    break;
                case 10:
                    strResultado = "Octubre";
                    break;
                case 11:
                    strResultado = "Noviembre";
                    break;
                case 12:
                    strResultado = "Diciembre";
                    break;
                default:
                    break;
            }
            return strResultado;
        }

        public ActionResult DescargarCSV(int idGPS, string fechaIni, string fechaFin)
        {
            //string csv = "\"ID\",\"Title\",\"Release Date\",\"Genere\",\"Price\",\"Rating\" \n";
            string csv = "FECHA Y HORA (HH:MM:SS),FRECUENCIA(HZ),TENSION(KV)\n";

            ReporteFrecuenciaParam param = new ReporteFrecuenciaParam();
            param.FechaInicial = DateTime.Parse(fechaIni);
            param.FechaFinal = DateTime.Parse(fechaFin);
            param.IdGPS = idGPS;

            var query = new ReporteFrecuenciaAppServicio().ObtenerFrecuencia(param);

            foreach (var item in query)
            {
                csv = csv + string.Format("{0}\n", item.Valor);
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Fv02_" + idGPS.ToString().PadLeft(3, '0') + "_" + param.FechaInicial.ToString("yyyyMMdd") + "_1084B.csv");
        }

        public ActionResult DescargarTXT(int idGPS, string fechaIni, string fechaFin)
        {
            ReporteFrecuenciaModel request = new ReporteFrecuenciaModel();
            request.FechaIni = "01/10/2023 00:00:00";
            request.FechaFin = "01/10/2023 00:05:00";


            ReporteFrecuenciaParam param = new ReporteFrecuenciaParam();
            param.FechaInicial = DateTime.Parse(request.FechaIni);
            param.FechaFinal = DateTime.Parse(request.FechaFin);
            param.IdGPS = request.IdGPS;

            var query = new ReporteFrecuenciaAppServicio().ObtenerFrecuencia(param);


            StringBuilder sb = new StringBuilder();
            //string output = "Output";
            //sb.Append(output);
            //sb.Append("\r\n");
            foreach (var item in query)
            {
                var fechahora = item.FechaHora.Substring(0, 16) + item.ColumnH.Replace("H", "").PadLeft(2, '0');
                //sb.AppendLine($"{item.FechaHora} {item.ColumnH} {item.Valor}");
                sb.AppendLine($"{fechahora} {decimal.Parse(item.Valor).ToString("0.###")} { 0 }");
            }

            Response.Clear();
            Response.ClearHeaders();

            Response.AppendHeader("Content-Length", sb.Length.ToString());
            Response.ContentType = "text/plain";
            Response.AppendHeader("Content-Disposition", "attachment;filename=\"Report.txt\"");

            Response.Write(sb);
            Response.End();

            return Json((object)new { result = "OK" });
        }

        public ActionResult ConvertirNull(int idGPS, string fechaIni, string fechaFin)
        {
            ReporteFrecuenciaModel request = new ReporteFrecuenciaModel();
            request.FechaIni = "01/10/2023 00:00:00";
            request.FechaFin = "01/10/2023 00:05:00";


            ReporteFrecuenciaParam param = new ReporteFrecuenciaParam();
            param.FechaInicial = DateTime.Parse(request.FechaIni);
            param.FechaFinal = DateTime.Parse(request.FechaFin);
            param.IdGPS = request.IdGPS;

            //var query = new ReporteFrecuenciaAppServicio().ObtenerFrecuencia(param);

            //foreach (var item in query)
            //{
            //    var fechahora = item.FechaHora.Substring(0, 16) + item.ColumnH.Replace("H", "").PadLeft(2, '0');
            //    //sb.AppendLine($"{item.FechaHora} {item.ColumnH} {item.Valor}");
            //    //sb.AppendLine($"{fechahora} {decimal.Parse(item.Valor).ToString("0.###")} { 0 }");
            //}



            /*
            DECLARE
      P_INICIO DATE:= to_date('02/04/2021 19:35', 'dd/MM/yyyy HH24:Mi');
            P_FIN DATE:= to_date('02/04/2021 19:35', 'dd/MM/yyyy HH24:Mi');
            P_CODI NUMBER :=:GPSCODI;
            P_USU VARCHAR2(60) := 'ED';
            P_ID NUMBER:= 0;
            P_RESULTADO NUMBER:= -1;
            begin
                SELECT max(ID) + 1 into P_ID FROM sic.FRECUENCIAS_AUDIT_CAB;

            insert into sic.FRECUENCIAS_AUDIT_CAB(ID, GPSCODI, FECHAINICIAL, FECHAFINAL, USUARIO)
    values(P_ID, P_CODI, P_INICIO, P_FIN, P_USU);

            insert into sic.FRECUENCIAS_AUDIT_DET(id, fecha, valor)
    select P_ID, to_date(
    to_char(FECHAHORA, 'dd/MM/yyyy HH24:mi') || decode(length(SEC), 2, replace(SEC, 'H', ':0'), replace(SEC, 'H', ':')), 'dd/MM/yyyy HH24:mi:ss') as FECHA,
    VALUE
    from(
    SELECT FECHAHORA,
    H0, H1, H2, H3, H4, H5, H6, H7, H8, H9, H10,
    H11, H12, H13, H14, H15, H16, H17, H18, H19, H20,
    H21, H22, H23, H24, H25, H26, H27, H28, H29, H30,
    H31, H32, H33, H34, H35, H36, H37, H38, H39, H40,
    H41, H42, H43, H44, H45, H46, H47, H48, H49, H50,
    H51, H52, H53, H54, H55, H56, H57, H58, H59
    FROM sic.F_LECTURA
    WHERE GPSCODI = 7 AND
    FECHAHORA BETWEEN P_INICIO AND P_FIN
    ORDER BY FECHAHORA)
    unpivot(
    value
    for SEC in
    (H0, H1, H2, H3, H4, H5, H6, H7, H8, H9, H10,
    H11, H12, H13, H14, H15, H16, H17, H18, H19, H20,
    H21, H22, H23, H24, H25, H26, H27, H28, H29, H30,
    H31, H32, H33, H34, H35, H36, H37, H38, H39, H40,
    H41, H42, H43, H44, H45, H46, H47, H48, H49, H50,
    H51, H52, H53, H54, H55, H56, H57, H58, H59)) ;
            commit;
        P_RESULTADO:= 0;
            end;

            */



            //return Json((object)new { result = "CONVERSION EXITOSA" });
            EquipoGPSModel modelo = new EquipoGPSModel();
            return View(modelo);
        }

        public ActionResult DescargarDAT(int idGPS, string fechaIni, string fechaFin)
        {
            if (idGPS==0)
            {
                List<EquipoGPSDTO> listaGPS = new List<EquipoGPSDTO>();
                listaGPS = new ReporteFrecuenciaAppServicio().ObtenerGPSs(new ReporteFrecuenciaParam() { FechaInicial = Convert.ToDateTime(fechaIni), FechaFinal = Convert.ToDateTime(fechaFin), IdGPS = 0, IndOficial = "S" });
                string Zip = Path.GetTempFileName();
                string strMes = this.mesDesc(Convert.ToDateTime(fechaIni).Month);
                string strAnio = Convert.ToDateTime(fechaIni).Year.ToString();

                foreach (EquipoGPSDTO itemEquipoGPS in listaGPS)
                {
                    int li_temp;
                    li_temp = 0;
                    string ls_mensaje, ls_valor;
                    ls_mensaje = ls_valor = String.Empty;
                    DateTime dfecha = DateTime.ParseExact(fechaIni, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
                    DateTime ldt_fecha = new DateTime(dfecha.Year, dfecha.Month, 1);
                    DateTime ldt_fechaFin = ldt_fecha.AddMonths(1);
                    Int32 gps_Codigo = itemEquipoGPS.GPSCodi;
                    Dictionary<DateTime, decimal?> ld_dictionary_Socabaya = new Dictionary<DateTime, decimal?>();
                    List<string> lBox_resultado = new List<string>();

                    //EquipoGPSDTO equipoGPS = new EquipoGPSDTO();
                    //equipoGPS = new EquipoGPSAppServicio().GetBydId(idGPS);

                    using (var archivoZip = ZipFile.Open(Zip, ZipArchiveMode.Update))
                    {
                        while (ldt_fecha < ldt_fechaFin)
                        {
                            ld_dictionary_Socabaya = new Dictionary<DateTime, decimal?>();

                            li_temp = nf_get_numero_registro_repetidos(ldt_fecha, gps_Codigo);

                            if (li_temp > 0)
                            {
                                lBox_resultado.Add("Existen " + li_temp.ToString() + " registros repetidos para el GPS con código: " + gps_Codigo.ToString());
                            }
                            else
                            {
                                nf_get_frecuencia_x_gps(gps_Codigo, ldt_fecha, out ld_dictionary_Socabaya, out ls_mensaje);

                                StringBuilder sb1 = new StringBuilder();
                                foreach (KeyValuePair<DateTime, decimal?> item in ld_dictionary_Socabaya)
                                {
                                    if (item.Value.HasValue)
                                        sb1.AppendLine(nf_get_generar_trama(item.Key, item.Value.Value, 0, 1));
                                }

                                var entrada = archivoZip.CreateEntry(itemEquipoGPS.NombreEquipo.Trim().Replace(" ", "_") + "_" + strMes + "_" + strAnio + "/Fv02_" + gps_Codigo.ToString().PadLeft(3, '0') + "_" + ldt_fecha.ToString("yyyyMMdd") + "_1084B.dat", CompressionLevel.Optimal);
                                using (var entradaStream = entrada.Open())
                                {
                                    byte[] contenidoBytes = Encoding.UTF8.GetBytes(sb1.ToString());
                                    entradaStream.Write(contenidoBytes, 0, contenidoBytes.Length);
                                }
                            }
                            lBox_resultado.Add("GPS con código: " + gps_Codigo.ToString() + ". Día " + ldt_fecha.ToString("dd-MM-yyyy"));
                            ldt_fecha = ldt_fecha.AddDays(1);
                        }
                    }

                }

                

                byte[] contenidoArchivoZip = System.IO.File.ReadAllBytes(Zip);
                return File(contenidoArchivoZip, "application/zip", strMes + "_" + strAnio + ".zip");

            } else
            {
                string Zip = Path.GetTempFileName();

                int li_valor, li_temp;
                li_temp = li_valor = 0;
                string ls_mensaje, ls_valor;
                ls_mensaje = ls_valor = String.Empty;
                DateTime dfecha = DateTime.ParseExact(fechaIni, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
                DateTime ldt_fecha = new DateTime(dfecha.Year, dfecha.Month, 1);
                DateTime ldt_fechaFin = ldt_fecha.AddMonths(1);
                Int32 gps_Codigo = idGPS;
                Dictionary<DateTime, decimal?> ld_dictionary_Socabaya = new Dictionary<DateTime, decimal?>();
                List<string> lBox_resultado = new List<string>();


                using (var archivoZip = ZipFile.Open(Zip, ZipArchiveMode.Update))
                {
                    while (ldt_fecha < ldt_fechaFin)
                    {
                        ld_dictionary_Socabaya = new Dictionary<DateTime, decimal?>();

                        li_temp = nf_get_numero_registro_repetidos(ldt_fecha, gps_Codigo);

                        if (li_temp > 0)
                        {
                            lBox_resultado.Add("Existen " + li_temp.ToString() + " registros repetidos para el GPS con código: " + gps_Codigo.ToString());
                        }
                        else
                        {
                            nf_get_frecuencia_x_gps(gps_Codigo, ldt_fecha, out ld_dictionary_Socabaya, out ls_mensaje);

                            StringBuilder sb1 = new StringBuilder();
                            foreach (KeyValuePair<DateTime, decimal?> item in ld_dictionary_Socabaya)
                            {
                                if (item.Value.HasValue)
                                    sb1.AppendLine(nf_get_generar_trama(item.Key, item.Value.Value, 0, 1));
                            }

                            var entrada = archivoZip.CreateEntry("Fv02_" + gps_Codigo.ToString().PadLeft(3, '0') + "_" + ldt_fecha.ToString("yyyyMMdd") + "_1084B.dat", CompressionLevel.Optimal);
                            using (var entradaStream = entrada.Open())
                            {
                                byte[] contenidoBytes = Encoding.UTF8.GetBytes(sb1.ToString());
                                entradaStream.Write(contenidoBytes, 0, contenidoBytes.Length);
                            }
                        }
                        lBox_resultado.Add("GPS con código: " + gps_Codigo.ToString() + ". Día " + ldt_fecha.ToString("dd-MM-yyyy"));
                        ldt_fecha = ldt_fecha.AddDays(1);
                    }
                }

                byte[] contenidoArchivoZip = System.IO.File.ReadAllBytes(Zip);
                EquipoGPSDTO equipoGPS = new EquipoGPSDTO();
                equipoGPS = new EquipoGPSAppServicio().GetBydId(idGPS);
                string strMes = this.mesDesc(Convert.ToDateTime(fechaIni).Month);
                string strAnio = Convert.ToDateTime(fechaIni).Year.ToString();
                return File(contenidoArchivoZip, "application/zip", equipoGPS.NombreEquipo.Trim().Replace(" ", "_") + "_" + strMes + "_" + strAnio + ".zip");

            }

            
        }

        public string nf_get_generar_trama(DateTime adt_fecha, Decimal ad_frecuencia, Decimal ad_tension, Decimal ad_ivdf)
        {
            string as_trama = String.Empty;
            string ls_temp = adt_fecha.ToString("MM/dd/yyyy HH:mm:ss") + "L 00";
            double d_temp = (double)ad_frecuencia - 60;
            string s_temp1 = "";
            string s_temp2 = "";
            if (d_temp >= 0)
                s_temp1 = "+" + ((int)Math.Abs(d_temp)).ToString();
            else
                s_temp1 = "-" + ((int)Math.Abs(d_temp)).ToString();
            int l_temp1 = (int)Math.Round(Math.Abs(d_temp - (int)d_temp) * 1000);
            if (ad_ivdf >= 0)
                s_temp2 = "+" + ((int)Math.Abs(ad_ivdf)).ToString();
            else
                s_temp2 = "-" + ((int)Math.Abs(ad_ivdf)).ToString();
            int l_temp2 = (int)Math.Round(Math.Abs(ad_ivdf - (int)ad_ivdf) * 10000);
            int minuteyear = adt_fecha.DayOfYear * 86400 + adt_fecha.Hour * 3600 + adt_fecha.Minute * 60 + adt_fecha.Second;

            as_trama = minuteyear.ToString().PadLeft(8, '0') + "|" + ls_temp + s_temp1.PadLeft(4) + "." + l_temp1.ToString().PadLeft(3, '0') + "  " + s_temp2.PadLeft(5) + "." +
                l_temp2.ToString().PadLeft(4, '0') + "   0.00" + String.Format("{0:F2}", ad_tension).PadLeft(7);

            return as_trama;
        }

        public int nf_get_numero_registro_repetidos(DateTime adt_fecha, int ai_gpscodi)
        {

            int li_result;
            try
            {

                li_result = this.servicio.ContarRegistrosRepetidos(ai_gpscodi, adt_fecha, adt_fecha.AddDays(1));
                return li_result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void nf_get_frecuencia_x_gps(int ai_gps, DateTime adt_fecha, out Dictionary<DateTime, decimal?> ad_dictionary, out string as_mensaje)
        {
            DataTable dt_resultado;
            ad_dictionary = new Dictionary<DateTime, decimal?>();
            as_mensaje = String.Empty;

            dt_resultado = nf_get_info_frecuencia(ai_gps, adt_fecha);

            DateTime ldt_fechaTemp = adt_fecha;
            DataRow dr = null;
            decimal ld_dato;
            string ls_nombreColumna = "FECHAHORA";
            int li_tieneValores = 0;


            if (dt_resultado != null && dt_resultado.Rows.Count > 0)
            {
                if (adt_fecha.Date >= DateTime.Now.Date)
                {
                    as_mensaje = "La frecuencia no puede ser completada el mismo día en el que se genera (" + adt_fecha.ToString("dd/MM/yyyy") + ")";
                    return;
                }

                while (ldt_fechaTemp < adt_fecha.AddDays(1))
                {
                    if (p_get_fila_frecuencia(dt_resultado, ldt_fechaTemp, out dr, ls_nombreColumna))
                    {
                        for (Int16 i = 0; i < 60; i++) //Los 60 valores (0..59)
                        {
                            if (Decimal.TryParse(dr["h" + i.ToString()].ToString(), out ld_dato))
                            {
                                ad_dictionary.Add(ldt_fechaTemp.AddSeconds(i), Math.Round(ld_dato, 5));
                                li_tieneValores++;
                            }
                            else
                                ad_dictionary.Add(ldt_fechaTemp.AddSeconds(i), null);
                        }
                    }
                    else
                    {
                        for (Int16 i = 0; i < 60; i++) //Los 60 valores (0..59)
                        {
                            ad_dictionary.Add(ldt_fechaTemp.AddSeconds(i), null);
                        }
                    }

                    ldt_fechaTemp = ldt_fechaTemp.AddMinutes(1);
                }
            }
            else
            {
                as_mensaje = "No se recuperaron registros para el GPS " + ai_gps.ToString() + " en el día " + adt_fecha.ToString("dd/MM/yyyy");
            }
        }


        public DataTable nf_get_info_frecuencia(int ai_gpscodi, DateTime adt_fecha)
        {

            try
            {
                DataTable lds_result = this.servicio.GetByCriteriaDatatable2(ai_gpscodi, adt_fecha, adt_fecha.AddDays(1));
                return lds_result;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool p_get_fila_frecuencia(DataTable dt, DateTime fecha, out DataRow dr, string nombreColumna)
        {
            dr = null;

            // Filtrar el DataTable para encontrar la fila con la fecha especificada
            DataRow[] rows = dt.Select($"FECHAHORA = #{fecha.ToString("yyyy-MM-dd HH:mm:ss")}#");

            // Verificar si se encontró una fila
            if (rows.Length > 0)
            {
                // Se encontró una fila, asignarla al parámetro de salida
                dr = rows[0];
                return true;
            }

            return false;
        }
    }
}
