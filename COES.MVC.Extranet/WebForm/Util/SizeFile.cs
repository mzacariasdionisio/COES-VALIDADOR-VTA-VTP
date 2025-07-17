using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.Text;
using WScoes;
using fwapp;

namespace WSIC2010.Util
{
    public class SizeFile
    {
        public static double SizeOfFile(int size)
        {
            double tamanioMB = Math.Round(Convert.ToDouble(size / 1048576.0), 2);

            return tamanioMB;
        }
    }

    public static class DiferenciaFecha
    {
        static IManttoService manttoService;

        public static bool HoraValida(DateTime pdt_fecha, int ai_limiteHoras)
        {
            bool lb_verdadero = false;
            DateTime ldt_fechaBase = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            //Diferencia en dias, horas, y minutos
            TimeSpan lts_diferencia = pdt_fecha - ldt_fechaBase;

            //Diferencia en horas
            int li_diferenciaEnHoras = lts_diferencia.Hours;
            //int li_diferenciaEnMinutos = lts_diferencia.Minutes;
            double li_diferenciaEnMinutos = lts_diferencia.TotalMinutes;
            int li_diferenciaEnSegundos = lts_diferencia.Seconds;

            if (li_diferenciaEnHoras <= ai_limiteHoras && li_diferenciaEnMinutos < ai_limiteHoras * 60 && 0 < li_diferenciaEnMinutos)
            //if (li_diferenciaEnMinutos <= Convert.ToDouble(480))
            {
                lb_verdadero = true;
            }

            return lb_verdadero;
        }

        public static bool HoraValida(DateTime pdt_fecha, int ai_limiteHoras, int pi_tipoPrograma)
        {
            bool lb_verdadero = false;
            DateTime ldt_fechaBase = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            if (pi_tipoPrograma == 1)
            {
                if (pdt_fecha.Date < ldt_fechaBase.Date)
                {
                    return lb_verdadero;
                }
                //else if (pdt_fecha.Date.Equals(ldt_fechaBase.Date))
                else if (pdt_fecha.Date >= ldt_fechaBase.Date)
                {
                    //Diferencia en dias, horas, y minutos
                    TimeSpan lts_diferencia = DateTime.Now - pdt_fecha;

                    //Diferencia en horas
                    int li_diferenciaEnHoras = lts_diferencia.Hours;
                    //int li_diferenciaEnMinutos = lts_diferencia.Minutes;
                    double li_diferenciaEnMinutos = lts_diferencia.TotalMinutes;
                    int li_diferenciaEnSegundos = lts_diferencia.Seconds;

                    //if (li_diferenciaEnHoras <= ai_limiteHoras && li_diferenciaEnMinutos < ai_limiteHoras * 60)
                    if (li_diferenciaEnHoras <= (ai_limiteHoras / 60) && li_diferenciaEnMinutos < ai_limiteHoras)
                    //if (li_diferenciaEnMinutos <= Convert.ToDouble(480))
                    {
                        lb_verdadero = true;
                    }
                }
            }
            else if (pi_tipoPrograma == 2)
            {
                DateTime ldt_fechaMartes = pdt_fecha.AddDays(-4);
                if (ldt_fechaMartes.Date < ldt_fechaBase.Date)
                {
                    return lb_verdadero;
                }
                else if (ldt_fechaMartes.Date.Equals(ldt_fechaBase.Date))
                {
                    //Diferencia en dias, horas, y minutos
                    TimeSpan lts_diferencia = DateTime.Now - pdt_fecha;

                    //Diferencia en horas
                    int li_diferenciaEnHoras = lts_diferencia.Hours;
                    //int li_diferenciaEnMinutos = lts_diferencia.Minutes;
                    double li_diferenciaEnMinutos = lts_diferencia.TotalMinutes;
                    int li_diferenciaEnSegundos = lts_diferencia.Seconds;

                    //if (li_diferenciaEnHoras <= ai_limiteHoras && li_diferenciaEnMinutos < ai_limiteHoras * 60)
                    if (li_diferenciaEnHoras <= (ai_limiteHoras / 60) && li_diferenciaEnMinutos < ai_limiteHoras)
                    //if (li_diferenciaEnMinutos <= Convert.ToDouble(480))
                    {
                        lb_verdadero = true;
                    }
                }
            }

            return lb_verdadero;
        }

        public static bool RangoDiasValidoPSO(DateTime sdt_fecha, int pi_limiteHoras)
        {
            int li_dia = (int)sdt_fecha.DayOfWeek;

            //if ((li_dia % 5).Equals(1) || (li_dia % 5).Equals(2) || li_dia.Equals(4))
            //if ((li_dia % 5).Equals(1) || (li_dia % 5).Equals(2))
            if (li_dia != 2)
            {
                return true;
            }
            else
            {
                DateTime ldt_fechaBase = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                TimeSpan lts_diferencia = DateTime.Now - ldt_fechaBase;

                //Diferencia en horas
                int li_diferenciaEnHoras = lts_diferencia.Hours;
                //int li_diferenciaEnMinutos = lts_diferencia.Minutes;
                double li_diferenciaEnMinutos = lts_diferencia.TotalMinutes;
                int li_diferenciaEnSegundos = lts_diferencia.Seconds;

                //if (li_diferenciaEnHoras <= pi_limiteHoras && li_diferenciaEnMinutos < pi_limiteHoras * 60)
                if (li_diferenciaEnHoras <= (pi_limiteHoras / 60) && li_diferenciaEnMinutos < pi_limiteHoras)
                //if (li_diferenciaEnMinutos <= Convert.ToDouble(480))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        internal static bool HoraValida(DateTime adt_fecha, int pi_limiteHora, string ps_tipoPrograma)
        {
            DateTime ldt_fecha = new DateTime(2000, 1, 1);
            switch (ps_tipoPrograma)
            {
                case "2":
                    ldt_fecha = adt_fecha.AddDays(-1).AddHours(9);
                    //ldt_fecha = adt_fecha.AddHours(pi_limiteHora);
                    if (ldt_fecha <= DateTime.Now)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "3":
                    //ldt_fecha = adt_fecha.AddDays(-1).AddHours(10);
                    ldt_fecha = adt_fecha.AddDays(-4).AddHours(pi_limiteHora);
                    if (ldt_fecha <= DateTime.Now)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return true;

            }
        }


        internal static bool HoraInValida(int pi_mancodi)
        {
            manttoService = new ManttoService();
            CManttoRegistro ManttoRegistro = manttoService.GetManttoRegistro(pi_mancodi);
            DateTime ldt_fechaLimite = ManttoRegistro.FechaLimiteFinal;
            DateTime ldt_fecha = new DateTime(2013, 1, 1);
            TimeSpan ts;
            double pd_limiteHora = 0;

            if (ManttoRegistro.EvenClaseCodi == 1)
            {
                pd_limiteHora = 1;
            }
            else if (ManttoRegistro.EvenClaseCodi == 2)
            {
                pd_limiteHora = 9;
            }
            else if (ManttoRegistro.EvenClaseCodi == 3)
            {
                pd_limiteHora = 0.1; //4d y 14h
            }
            switch (ManttoRegistro.EvenClaseCodi)
            {
                case 1:
                    if (ldt_fechaLimite > new DateTime(2013, 1, 1))
                    {
                        ts = ldt_fechaLimite - DateTime.Now;
                    }
                    else
                    {
                        ts = ManttoRegistro.FechaInicial.AddHours(pd_limiteHora) - DateTime.Now;
                        ts = ts.Subtract(new TimeSpan(-1, 0, 0, 0));
                    }
                    if (ts.TotalHours >= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 2:
                    if (ldt_fechaLimite > new DateTime(2013, 1, 1))
                    {
                        ts = ldt_fechaLimite - DateTime.Now;
                    }
                    else
                    {
                        ts = ManttoRegistro.FechaInicial.AddHours(pd_limiteHora) - DateTime.Now;
                        ts = ts.Subtract(new TimeSpan(1, 0, 0, 0));
                    }
                    if (ts.TotalHours >= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 3:
                    if (ldt_fechaLimite > new DateTime(2013, 1, 1))
                    {
                        ts = ldt_fechaLimite - DateTime.Now;
                    }
                    else
                    {
                        int li_dias = 4;
                        if (ManttoRegistro.FechaInicial.ToString("dd/MM/yyyy").Equals("03/08/2013"))
                        {
                            pd_limiteHora = 0.1;
                            li_dias = 8;
                        }
                        ts = ManttoRegistro.FechaInicial.AddHours(pd_limiteHora) - DateTime.Now;
                        ts = ts.Subtract(new TimeSpan(li_dias, 0, 0, 0));
                    }
                    if (ts.TotalHours >= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 4:
                    ts = ldt_fechaLimite - DateTime.Now;
                    //ts = ldt_fecha.AddMonths(8).AddDays(9).AddHours(24) - DateTime.Now; //10/09/13
                    //ts = ldt_fecha.AddMonths(6).AddDays(8).AddHours(24) - DateTime.Now; //09/07/13
                    //ts = ldt_fecha.AddMonths(6).AddDays(9).AddHours(24) - DateTime.Now;
                    if (ts.TotalHours >= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 5:
                    ts = ldt_fechaLimite - DateTime.Now;
                    //ts = ldt_fecha.AddMonths(8).AddDays(19).AddHours(24) - DateTime.Now;//06/09/13
                    if (ts.TotalHours >= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return false;

            }
        }

    }

    public static class Columnas
    {
        public static string[] GetHojasExcel(OleDbConnection conexion)
        {
            //Obtener todas las hojas 
            DataTable dtSheets = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string[] workSheetNames = new String[dtSheets.Rows.Count];
            int i = 0;
            foreach (DataRow row in dtSheets.Rows)
            {
                //insert the sheet's name in the current element of the array
                //and remove the $ sign at the end
                workSheetNames[i] = row["TABLE_NAME"].ToString().Trim(new[] { '\'' }).Trim(new[] { '$' });
                //workSheetNames[i] = row["TABLE_NAME"].ToString();
                i++;
            }

            return workSheetNames;
        }

        public static bool GetExistingCode(int ai_valueToCompare, DataTable adt_PtoxEmpresas)
        {

            bool lb_existeBarra = false;

            for (int i = 0; i < adt_PtoxEmpresas.Rows.Count; i++)
            {
                if (ai_valueToCompare == Convert.ToInt32(adt_PtoxEmpresas.Rows[i]["PtoMediCodi"].ToString()))
                {
                    lb_existeBarra = true;
                    break;
                }
            }

            return lb_existeBarra;

        }

        //public static StringBuilder GetTablaHTML(bool pb_error, bool pb_cabecera, List<Util.Pronostico_Demanda> lpd_Demandas, string ls_label)
        //{
        //    StringBuilder sb = new StringBuilder(ls_label);

        //    if (!pb_error && !pb_cabecera && lpd_Demandas.Count > 0)
        //    {
        //        sb.Append("<table CELLSPACING='3' CELLPADDING='3' BORDER='1' class='sample'>");
        //        //Cabeceras de Descripcion
        //        sb.Append("<tr><th align='center'></th>");
        //        for (int i = 0; i < lpd_Demandas.Count; i++)
        //        {
        //            if (i == 0)
        //                sb.Append("<th colspan='7'>" + lpd_Demandas[i].Descripcion + "</th>");
        //        }
        //        sb.Append("</tr>");

        //        //Cabeceras de Pronostico
        //        sb.Append("<tr><th align='center'>" + ls_array_hojas[vi].Substring(1, ls_array_hojas[vi].Length - 3) + "</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th>" +
        //                "<th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th></tr>" + "<tr><th>HORA</th><th>" +
        //                ldt_fecha.Date.AddDays(0).ToString("dd/MM/yyyy") + "</th><th>" + ldt_fecha.Date.AddDays(1).ToString("dd/MM/yyyy") + "</th><th>" +
        //                ldt_fecha.Date.AddDays(2).ToString("dd/MM/yyyy") + "</th><th>" + ldt_fecha.Date.AddDays(3).ToString("dd/MM/yyyy") + "</th><th>" +
        //                ldt_fecha.Date.AddDays(4).ToString("dd/MM/yyyy") + "</th><th>" + ldt_fecha.Date.AddDays(5).ToString("dd/MM/yyyy") + "</th><th>" +
        //                ldt_fecha.Date.AddDays(6).ToString("dd/MM/yyyy") + "</th></tr>");
        //        for (int k = 0; k < 96; k++)
        //        {
        //            StatusLabel.Text += "<tr><td>" + Util.ExcelUtil.GetTime(k + 1) + "</td>" + ls_array_filas[k] + "</tr>";
        //        }
        //        StatusLabel.Text += "</table>";

        //    }
        //    else if (ALPronosticosTotal.Count == 0)
        //    {
        //        StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: La empresa seleccionada <strong>NO PRESENTA</strong> las barras del documento</p>";

        //    }
        //    else
        //    {
        //        StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: La empresa seleccionada <strong>NO PRESENTA</strong> las barras del documento</p>";
        //    }

        //    return sb;
        //}
    }

    public static class Valida
    {
        public static IDemandaService demandaServ;

        public static bool NombreArchivo(string ps_codigoEmpr, string ps_numSem, string ps_nombreProg, string ps_nombreArch, string ps_extension, out string ps_formatoNombre, string ps_cadenaFecha)
        {
            DateTime ldt_fecha = new DateTime(2013, 1, 1);
            int pi_codigoEmpresa = Convert.ToInt32(ps_codigoEmpr);
            demandaServ = new DemandaService();
            DataTable dt_empresas = demandaServ.nf_get_empresa_detalles(pi_codigoEmpresa);
            Empresa empresa;
            bool lb_coincide = false;

            if (dt_empresas != null && dt_empresas.Rows.Count > 0)
            {
                empresa = new Empresa()
                {
                    EmpresaAbrev = dt_empresas.Rows[0]["EMPRABREV"].ToString().Trim(),
                    EmpresaCodi = Convert.ToInt32(dt_empresas.Rows[0]["EMPRCODI"].ToString()),
                    EmpresaNomb = dt_empresas.Rows[0]["EMPRNOMB"].ToString(),
                    TipoEmpresaAbrev = (dt_empresas.Rows[0]["TIPOEMPRABREV"].ToString().Trim().Equals("ULIBRE")) ? "UL" :
                    ((dt_empresas.Rows[0]["TIPOEMPRABREV"].ToString().Trim().Equals("DISTR")) ? "ED" : dt_empresas.Rows[0]["TIPOEMPRABREV"].ToString().Trim())
                };

                if (empresa.EmpresaCodi == 11772)
                {
                    empresa.TipoEmpresaAbrev = "UL";
                }
            }
            else
            {
                ps_formatoNombre = "No se obtuvo datos desde BD";
                return lb_coincide;
            }

            Dictionary<int, string> tipoPrograma = new Dictionary<int, string>();
            tipoPrograma.Add(1, "DIARIO");
            tipoPrograma.Add(2, "SEMANAL");


            string[] ls_array = ps_nombreArch.Split(new char[] { '-' });
            string ls_fecha = String.Empty;

            if (ps_nombreProg.Equals("1"))
                ls_fecha = Valida.GetFechaToCadena(ps_cadenaFecha);
            else if (ps_nombreProg.Equals("2"))
            {
                CargaDDLSemanas.LlenaSemanasTemporal(DateTime.Now.Date, out ldt_fecha);
                ls_fecha = "SEM" + ps_numSem + EPDate.f_fechafinsemana(ldt_fecha).ToString("yy");

            }

            ps_formatoNombre = "";

            if (ls_array.Count() == 4)
            {
                if (ls_array[0].Equals(empresa.TipoEmpresaAbrev) &&
                    ls_array[1].Equals(empresa.EmpresaAbrev) &&
                    ls_array[2].Equals("PR03") &&
                    ls_array[3].ToUpper().Equals(ls_fecha))
                {
                    lb_coincide = true;
                }
                else
                {
                    ps_formatoNombre = empresa.TipoEmpresaAbrev + "-" + empresa.EmpresaAbrev + "-" + ls_array[2] + "-" + ls_fecha + ps_extension;
                }

            }
            else
            {
                ps_formatoNombre = empresa.TipoEmpresaAbrev + "-" + empresa.EmpresaAbrev + "-" + ls_array[2] + "-" + ls_fecha + ps_extension;
            }

            return lb_coincide;
        }

        public static string GetFechaToCadena(string as_cadena)
        {
            StringBuilder ls_output = new StringBuilder("");
            //string ls_fecha = DateTime.Now.Date.ToString("dd/MM/yyyy");
            string ls_fecha = EPDate.ToDate(as_cadena).ToString("dd/MM/yyyy");
            string[] ls_array_fecha = ls_fecha.Split(new char[] { '/' });

            for (int i = 0; i < ls_array_fecha.Count(); i++)
            {
                ls_output.Append(ls_array_fecha[i]);
            }

            return ls_output.ToString().Trim();
        }

        //public static string GetFechaToCadena(string as_cadena1, string as_cadena2, string as_tipoPrograma)
        //{
        //    StringBuilder ls_output = new StringBuilder("");
        //    //string ls_fecha = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //    //string ls_fecha = EPDate.ToDate(as_cadena).ToString("dd/MM/yyyy");
        //    string[] ls_array_fecha = ls_fecha.Split(new char[] { '/' });

        //    for (int i = 0; i < ls_array_fecha.Count(); i++)
        //    {
        //        ls_output.Append(ls_array_fecha[i]);
        //    }

        //    return ls_output.ToString().Trim();
        //}
    }

    public static class CargaDDLSemanas
    {
        public static Dictionary<string, string> LlenaSemanas(int pi_limite)
        {
            Dictionary<string, string> Semanas = new Dictionary<string, string>();
            string ls_cont = String.Empty;

            for (int i = 1; i <= pi_limite; i++)
            {
                ls_cont = i.ToString().PadLeft(2, '0');
                Semanas.Add(ls_cont, ls_cont);
            }

            return Semanas;
        }

        public static Dictionary<string, string> LlenaSemanas(DateTime adt_fecha, out DateTime ldt_fechaPrograma)
        {
            Dictionary<string, string> Semanas = new Dictionary<string, string>();
            string ls_cont = String.Empty;
            ldt_fechaPrograma = new DateTime(2013, 1, 1);

            int li_caso = (int)adt_fecha.DayOfWeek;
            switch (li_caso)
            {
                case 1:
                case 2:
                case 6:
                case 0:
                    //case 3://NO se considera el miercoles
                    ldt_fechaPrograma = EPDate.f_fechafinsemana(adt_fecha).AddDays(1);
                    ls_cont = EPDate.f_numerosemana(EPDate.f_fechafinsemana(adt_fecha).AddDays(2)).ToString();
                    if (ls_cont == "53") ls_cont = "1";
                    break;
                case 4:
                case 5:
                    ldt_fechaPrograma = EPDate.f_fechafinsemana(EPDate.f_fechafinsemana(adt_fecha).AddDays(1)).AddDays(1);
                    ls_cont = EPDate.f_numerosemana(EPDate.f_fechafinsemana(EPDate.f_fechafinsemana(adt_fecha).AddDays(1)).AddDays(1)).ToString();
                    if (ls_cont == "53") ls_cont = "1";
                    break;
                default:
                    break;
            }

            Semanas.Add(ls_cont, ls_cont);
            //Semanas.Add((Convert.ToInt32(ls_cont) + 1).ToString(), (Convert.ToInt32(ls_cont) + 1).ToString());


            return Semanas;
        }

        public static Dictionary<string, string> LlenaSemanasTemporal(DateTime adt_fecha, out DateTime ldt_fechaPrograma)
        {
            Dictionary<string, string> Semanas = new Dictionary<string, string>();
            string ls_cont = String.Empty;
            ldt_fechaPrograma = new DateTime(2013, 1, 1);
            DateTime ldt_fechaInicial = new DateTime(2018, 12, 19, 0, 0, 0);
            DateTime ldt_fechaFinal = new DateTime(2018, 12, 25, 0, 0, 0);
            bool lb_cargasemanas = false;

            int li_cont = 0;

            int li_caso = (int)adt_fecha.DayOfWeek;
            switch (li_caso)
            {
                case 1:
                case 2:
                case 6:
                case 0:
                    //case 3://NO se considera el miercoles
                    ldt_fechaPrograma = EPDate.f_fechafinsemana(adt_fecha).AddDays(1);
                    li_cont = EPDate.f_numerosemana(EPDate.f_fechafinsemana(adt_fecha).AddDays(1));
                    if (li_cont >= 53)
                    {
                        if (ldt_fechaInicial <= adt_fecha && adt_fecha <= ldt_fechaFinal)
                            lb_cargasemanas = true;

                        li_cont = 1;
                    }

                    ls_cont = li_cont.ToString();
                    break;
                case 3:
                case 4:
                case 5:
                    ldt_fechaPrograma = EPDate.f_fechafinsemana(EPDate.f_fechafinsemana(adt_fecha).AddDays(1)).AddDays(1);
                    li_cont = EPDate.f_numerosemana(EPDate.f_fechafinsemana(EPDate.f_fechafinsemana(adt_fecha).AddDays(1)).AddDays(1));
                    if (li_cont >= 53)
                    {
                        if (ldt_fechaInicial <= adt_fecha && adt_fecha <= ldt_fechaFinal)
                            lb_cargasemanas = true;

                        //li_cont = 1;
                    }
                    ls_cont = li_cont.ToString();
                    break;
                default:
                    break;
            }

            Semanas.Add(ls_cont, ls_cont);
            if (lb_cargasemanas)
                Semanas.Add("2", "2");


            return Semanas;
        }


        public static Dictionary<string, string> LlenaSemanasSabados(DateTime adt_fecha, out DateTime ldt_fechaPrograma)
        {
            Dictionary<string, string> Semanas = new Dictionary<string, string>();
            string ls_cont = String.Empty;
            ldt_fechaPrograma = new DateTime(2013, 1, 1);

            int li_caso = (int)adt_fecha.DayOfWeek;
            switch (li_caso)
            {
                case 1:
                case 2:
                case 6:
                case 0:
                    //case 3://NO se considera el miercoles
                    ldt_fechaPrograma = EPDate.f_fechainiciosemana(adt_fecha);
                    ls_cont = EPDate.f_numerosemana(EPDate.f_fechafinsemana(adt_fecha).AddDays(-1)).ToString();
                    break;
                case 4:
                case 5:
                    ldt_fechaPrograma = EPDate.f_fechafinsemana(EPDate.f_fechafinsemana(adt_fecha).AddDays(1)).AddDays(1);
                    ls_cont = EPDate.f_numerosemana(EPDate.f_fechafinsemana(EPDate.f_fechafinsemana(adt_fecha).AddDays(1)).AddDays(1)).ToString();
                    break;
                default:
                    break;
            }

            Semanas.Add(ls_cont, ls_cont);
            //Semanas.Add((Convert.ToInt32(ls_cont) + 1).ToString(), (Convert.ToInt32(ls_cont) + 1).ToString());


            return Semanas;
        }
    }

    public static class Extension
    {
        public static bool IsExcel(string ps_extension, string ps_nameIn, out string ps_nameOut, string ps_BasePath, out string ps_conex_string)
        {
            string ls_path = String.Empty;
            ps_nameOut = ps_nameIn;
            ps_conex_string = String.Empty;

            if (ps_extension.Equals(".xls") || ps_extension.Equals(".xlsx"))
            {
                if (ps_extension == ".xls") //excel 97-2003
                {
                    //Agregar el nombre archivo a ls_path
                    ps_nameOut = ps_nameIn.Substring(0, (ps_nameIn.Length - 4)) + "-" + DateTime.Now.ToString("Hmmss") + ".xls";
                    ls_path = ps_BasePath + ps_nameOut;
                    ps_conex_string = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @ls_path + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1;\"";
                    //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ls_path + ";Extended Properties=" + (char)34 + "Excel 8.0;HDR=No;IMEX=1;" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";
                }
                else if (ps_extension == ".xlsx") //2007
                {
                    ps_nameOut = ps_nameIn.Substring(0, (ps_nameIn.Length - 5)) + "-" + DateTime.Now.ToString("Hmmss") + ".xlsx";
                    ls_path = ps_BasePath + ps_nameOut;
                    ps_conex_string = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + @ls_path + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1;\"";
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}