using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using fwapp;
using AjaxControlToolkit;
using System.Collections.Specialized;
using System.Configuration;

namespace WScoes
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ManttoService" in code, svc and config file together.
    public class ManttoService : IManttoService
    {
        public int ii_Version = 21001;
        public n_fw_data[] iL_data = new n_fw_data[2];
        public n_fw_app n_app = new n_fw_app();
        CManttoRegistro ManRegistro;

        public ManttoService()
        {
            iL_data[0] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SICOES"].ToString());
            iL_data[1] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SCADA"].ToString());
        }

        public int GetVersion()
        {
            return ii_Version;
        }

        public int Register(string as_login, int ai_PassNumber)
        {
            if (as_login == "MEMuser" && ai_PassNumber == 200911)
                return Key() + 11;

            if (as_login == "sic" && ai_PassNumber == 47896)
            {
                return Key();
            }
            if (as_login == "WebUser" && ai_PassNumber == 4785236)
            {
                return Key();
            }
            return ai_PassNumber;
        }

        private int Key()
        {
            Random i_rand = new Random(DateTime.Now.Millisecond);
            return (DateTime.Today.Day * 7 + DateTime.Today.Month * 6 + (DateTime.Today.Year - 2000) * 5) * 1000 + i_rand.Next(999);
        }

        private bool IsKey(int ai_ukey)
        {
            if (ai_ukey / 1000 == DateTime.Today.Day * 7 + DateTime.Today.Month * 6 + (DateTime.Today.Year - 2000) * 5) return true;
            return false;
        }

        public DataTable GetDTData(string as_info, int ai_key) //string as_param1, string as_param2)
        {
            string s_command = "";
            if (as_info == "TRCANAL")
                ai_key -= 11;


            ArrayList AL_param = EPString.EP_GetArrayStringSeparateWithPipes(as_info);
            DataSet l_ds = new DataSet("DTCoes");
            if (IsKey(ai_key))
            {
                switch (AL_param[0].ToString().Trim().ToUpper())
                {
                    case "SELECTSCA":
                        s_command = "SELECT " + AL_param[1].ToString();
                        iL_data[1].Fill(l_ds, AL_param[0].ToString(), s_command);
                        break;
                    case "SELECT":
                        s_command = "SELECT " + AL_param[1].ToString();
                        iL_data[0].Fill(l_ds, AL_param[0].ToString(), s_command);
                        break;
                    case "SELECTWITHPIPES": //para consultas que contienen pipes en su consulta
                        s_command = "SELECT " + as_info.Substring(as_info.IndexOf('|') + 1);
                        iL_data[0].Fill(l_ds, AL_param[0].ToString(), s_command);
                        break;
                    case "MANTTO":
                        string s_evenclasecodi = AL_param[1].ToString();
                        string s_evenini1 = AL_param[2].ToString();
                        string s_evenini2 = AL_param[3].ToString();
                        s_command =
                        String.Format("SELECT * FROM EVE_MANTTO WHERE EVENCLASECODI = {0} and EVENINI >= TO_DATE('{1}','YYYYMMDD') AND EVENINI < TO_DATE('{2}','YYYYMMDD')", s_evenclasecodi, s_evenini1, s_evenini2);

                        iL_data[0].Fill(l_ds, AL_param[0].ToString(), s_command);
                        break;
                    case "EVENTO":

                        break;
                    case "OPERA":

                        break;
                    case "TRCANAL":
                        s_command = "SELECT CANALCODI, CANALICCP, CANALNOMB, CANALVALOR, CANALFHORA, CANALCALIDAD FROM TR_CANAL WHERE CANALCODI>=50000 AND CANALUNIDAD='MW'";
                        iL_data[1].Fill(l_ds, AL_param[0].ToString(), s_command);
                        break;
                    default:
                        break;
                }
                if (l_ds.Tables[AL_param[0].ToString()] != null)
                {
                    DataTable dt = l_ds.Tables[AL_param[0].ToString()];
                    l_ds.Tables.Remove(dt);
                    return dt;
                }
            }
            return null;
        }

        public Dictionary<int, string> H_GetManttoRegistros(DateTime adt_inicio, DateTime adt_final)
        {
            Dictionary<int, string> H_MRegistros = new Dictionary<int, string>();

            string ls_comando =
                @" SELECT man.REGCODI, man.REGABREV, even.EVENCLASECODI, even.EVENCLASEABREV, even.EVENCLASEDESC, man.FECHAINI, man.FECHAFIN, man.REGNOMB  
                            FROM MAN_REGISTRO man, EVE_EVENCLASE even WHERE  man.EVENCLASECODI = even.EVENCLASECODI  ";

            ls_comando += " AND  FECHAINI >= " + EPDate.SQLDateOracleString(adt_inicio) + " AND FECHAINI < " + EPDate.SQLDateOracleString(adt_final.AddDays(1)) + " ";
            ls_comando += " ORDER BY man.FECHAINI DESC, even.EVENCLASECODI";
            //in_app.iL_data[1].Fill(i_ds, "MAN_REGISTRO", ls_comando);
            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "MAN_REGISTRO", ls_comando);

            foreach (DataRow dr in l_ds.Tables["MAN_REGISTRO"].Rows)
            {
                //string ls_rangofecha;
                //int li_regcodi = Convert.ToInt32(dr["REGCODI"]);
                //DateTime ldt_FechaIni = (DateTime)dr["FECHAINI"];
                //DateTime ldt_FechaFin = (DateTime)dr["FECHAFIN"];
                ////int ii_evenclasecodi = Convert.ToInt32(dr["EVENCLASECODI"]);
                //string ls_evenclasedesc = Convert.ToString(dr["EVENCLASEDESC"]);

                //if (ldt_FechaIni == ldt_FechaFin)
                //    ls_rangofecha = ldt_FechaIni.ToString("dd/MM/yyyy");
                //else
                //    ls_rangofecha = ldt_FechaIni.ToString("dd/MM/yyyy") + " - " + ldt_FechaFin.ToString("dd/MM/yyyy");
                CManttoRegistro manreg = nf_CargarManttoRegistro(dr);

                H_MRegistros.Add(manreg.RegCodi, manreg.RegistroNombre + ": " + GetEstadoPrograma(manreg.RegCodi));
                //ListBoxRegistros.Items.Add("MANT. " + dr["EVENCLASEDESC"].ToString() + " " + ls_rangofecha);
            }
            return H_MRegistros;
        }

        public Dictionary<int, string> H_GetManttoRegistrosXTipo(DateTime adt_inicio, DateTime adt_final, int ai_tipoPrograma)
        {
            Dictionary<int, string> H_MRegistros = new Dictionary<int, string>();

            string ls_comando =
                @" SELECT man.REGCODI, man.REGABREV, even.EVENCLASECODI, even.EVENCLASEABREV, even.EVENCLASEDESC, man.FECHAINI, man.FECHAFIN, man.REGNOMB, man.FECHALIM  
                            FROM MAN_REGISTRO man, EVE_EVENCLASE even WHERE  man.EVENCLASECODI = even.EVENCLASECODI  ";

            ls_comando += " AND  FECHAINI >= " + EPDate.SQLDateOracleString(adt_inicio) + " AND FECHAINI < " + EPDate.SQLDateOracleString(adt_final.AddDays(1)) + " ";
            if (ai_tipoPrograma != 0)
            {
                ls_comando += " AND man.EVENCLASECODI = " + ai_tipoPrograma;
            }

            ls_comando += " ORDER BY man.FECHAINI DESC, even.EVENCLASECODI";
            //in_app.iL_data[1].Fill(i_ds, "MAN_REGISTRO", ls_comando);
            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "MAN_REGISTRO", ls_comando);

            foreach (DataRow dr in l_ds.Tables["MAN_REGISTRO"].Rows)
            {
                //string ls_rangofecha;
                //int li_regcodi = Convert.ToInt32(dr["REGCODI"]);
                //DateTime ldt_FechaIni = (DateTime)dr["FECHAINI"];
                //DateTime ldt_FechaFin = (DateTime)dr["FECHAFIN"];
                ////int ii_evenclasecodi = Convert.ToInt32(dr["EVENCLASECODI"]);
                //string ls_evenclasedesc = Convert.ToString(dr["EVENCLASEDESC"]);

                //if (ldt_FechaIni == ldt_FechaFin)
                //    ls_rangofecha = ldt_FechaIni.ToString("dd/MM/yyyy");
                //else
                //    ls_rangofecha = ldt_FechaIni.ToString("dd/MM/yyyy") + " - " + ldt_FechaFin.ToString("dd/MM/yyyy");
                CManttoRegistro manreg = nf_CargarManttoRegistro(dr);

                H_MRegistros.Add(manreg.RegCodi, manreg.RegistroNombre + ": " + GetEstadoPrograma(manreg.RegCodi));
                //ListBoxRegistros.Items.Add("MANT. " + dr["EVENCLASEDESC"].ToString() + " " + ls_rangofecha);
            }

            return H_MRegistros;
        }

        public DataTable H_GetManttosRegistros(DateTime adt_inicio, DateTime adt_final)
        {
            DataTable H_MRegistros = new DataTable("ManttoRegistros");
            H_MRegistros.Columns.Add("codigo", typeof(Int32));
            H_MRegistros.Columns.Add("tipoPrograma", typeof(Int32));
            H_MRegistros.Columns.Add("descripcion", typeof(String));
            H_MRegistros.Columns.Add("estado", typeof(String));
            H_MRegistros.Columns.Add("fechaInicial", typeof(DateTime));

            string ls_comando =
                //@" SELECT man.REGCODI, man.REGABREV, even.EVENCLASECODI, even.EVENCLASEABREV, even.EVENCLASEDESC, man.FECHAINI, man.FECHAFIN, man.REGNOMB, man.FECHALIMINI, man.FECHALIMSUP  
                @" SELECT man.REGCODI, man.REGABREV, even.EVENCLASECODI, even.EVENCLASEABREV, even.EVENCLASEDESC, man.FECHAINI, man.FECHAFIN, man.REGNOMB
                            FROM MAN_REGISTRO man, EVE_EVENCLASE even WHERE  man.EVENCLASECODI = even.EVENCLASECODI  ";

            ls_comando += " AND  FECHAINI >= " + EPDate.SQLDateOracleString(adt_inicio) + " AND FECHAINI < " + EPDate.SQLDateOracleString(adt_final.AddDays(1)) + " ";
            ls_comando += " ORDER BY man.FECHAINI DESC, even.EVENCLASECODI";
            //in_app.iL_data[1].Fill(i_ds, "MAN_REGISTRO", ls_comando);
            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "MAN_REGISTRO", ls_comando);

            foreach (DataRow dr in l_ds.Tables["MAN_REGISTRO"].Rows)
            {
                CManttoRegistro manreg = nf_CargarManttoRegistro(dr);

                DataRow _row = H_MRegistros.NewRow();
                _row["codigo"] = manreg.RegCodi;
                _row["tipoPrograma"] = manreg.EvenClaseCodi;
                _row["descripcion"] = manreg.RegistroNombre;
                _row["estado"] = GetEstadoPrograma(manreg.RegCodi);
                _row["fechaInicial"] = manreg.FechaInicial;

                H_MRegistros.Rows.Add(_row);
            }

            return H_MRegistros;
        }

        public DateTime GetFechaLimitePrograma(DateTime pdt_fechaInicial, int pi_codigoPrograma)
        {
            string ls_comando = "SELECT M.FECHALIM FROM MAN_REGISTRO M WHERE M.FECHAINI = TO_DATE('" + pdt_fechaInicial.ToString("dd-MM-yyyy") + "', 'DD-MM-YYYY') AND M.EVENCLASECODI = " + pi_codigoPrograma.ToString();
            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "MAN_REGISTRO", ls_comando);

            if (l_ds.Tables["MAN_REGISTRO"] != null && l_ds.Tables["MAN_REGISTRO"].Rows.Count > 0)
            {
                return EPDate.ToDateTime(l_ds.Tables["MAN_REGISTRO"].Rows[0]["FECHALIM"].ToString());
            }

            return new DateTime(2013, 1, 1);
            
        }

        private string GetEstadoPrograma(int pi_mancodi)
        {
            ManRegistro = GetManttoRegistro(pi_mancodi);
            DateTime ldt_fechaLimite = ManRegistro.FechaLimiteFinal;
            DateTime ldt_fecha;
            TimeSpan ts;
            double pd_limiteHora = 0;
            int pi_numeroDias = 0;
            if (ManRegistro.EvenClaseCodi == 1)
            {
                 pd_limiteHora = 1;
            }
            else if (ManRegistro.EvenClaseCodi == 2)
            {
                //pd_limiteHora = 10.1;
                pd_limiteHora = 9;
                pi_numeroDias = 1;
            }
            else if (ManRegistro.EvenClaseCodi == 3)
            {
                //pd_limiteHora = 14.1;
                //pi_numeroDias = 4;
                pd_limiteHora = 0.1;
                pi_numeroDias = 4;
            }

            switch (ManRegistro.EvenClaseCodi)
            {
                case 1:
                    if (ldt_fechaLimite > new DateTime(2013, 1, 1))
                    {
                        ts = ldt_fechaLimite - DateTime.Now;
                    }
                    else
                    {
                        ts = ManRegistro.FechaInicial.AddHours(pd_limiteHora) - DateTime.Now;
                        ts = ts.Subtract(new TimeSpan(-1, 0, 0, 0));
                    }
                    
                    if (ts.TotalHours >= 0)
                    {
                        return String.Format("ABIERTO (restan : {0:dd} dias, {1:hh} horas, {2:mm} minutos", ts, ts, ts) + ").";
                    }
                    else
                    {
                        return "CERRADO ";
                    }
                case 2:
                    if (ldt_fechaLimite > new DateTime(2013, 1, 1))
                    {
                        ts = ldt_fechaLimite - DateTime.Now;
                    }
                    else
                    {
                        ts = ManRegistro.FechaInicial.AddHours(pd_limiteHora) - DateTime.Now;
                        ts = ts.Subtract(new TimeSpan(pi_numeroDias, 0, 0, 0));
                    }
                    if (ts.TotalHours >= 0)
                    {
                        return String.Format("ABIERTO (restan : {0:dd} dias, {1:hh} horas, {2:mm} minutos", ts, ts, ts) + ").";
                    }
                    else
                    {
                        return "CERRADO ";
                    }
                case 3:
                    if (ldt_fechaLimite > new DateTime(2013, 1, 1))
                    {
                        ts = ldt_fechaLimite - DateTime.Now;
                    }
                    else
                    {
                        if (ManRegistro.FechaInicial.ToString("dd/MM/yyyy").Equals("03/08/2013"))
                        {
                            pi_numeroDias = 8;
                            pd_limiteHora = 0.1;
                        }
                        ts = ManRegistro.FechaInicial.AddHours(pd_limiteHora) - DateTime.Now;
                        ts = ts.Subtract(new TimeSpan(pi_numeroDias, 0, 0, 0));
                    }
                    if (ts.TotalHours >= 0)
                    {
                        return String.Format("ABIERTO (restan : {0:dd} dias, {1:hh} horas, {2:mm} minutos", ts, ts, ts) + ").";
                    }
                    else
                    {
                        return "CERRADO ";
                    }
                case 4:
                    ldt_fecha = new DateTime(2013, 1, 1);
                    ts = ldt_fechaLimite - DateTime.Now;
                    //ts = ldt_fecha.AddMonths(8).AddDays(9).AddHours(24) - DateTime.Now; //13/08/13
                    //ts = ldt_fecha.AddMonths(6).AddDays(8).AddHours(24) - DateTime.Now; //09/07/13
                    //ts = ldt_fecha.AddMonths(6).AddDays(9).AddHours(24) - DateTime.Now;
                    if (ts.TotalHours >= 0)
                    {
                        //return String.Format("ABIERTO (restan : {0:dd} dias, {1:hh} horas, {2:mm} minutos, {3:ss} segundos", ts, ts, ts, ts) + ").";
                        return String.Format("ABIERTO (restan : {0:dd} dias, {1:hh} horas, {2:mm} minutos", ts, ts, ts) + ").";
                    }
                    else
                    {
                        return "CERRADO ";
                    }
                case 5:
                    ldt_fecha = new DateTime(2013, 1, 1);
                    ts = ldt_fechaLimite - DateTime.Now;
                    //ts = ldt_fecha.AddMonths(8).AddDays(19).AddHours(24) - DateTime.Now;//06/09/13
                    if (ts.TotalHours >= 0)
                    {
                        //return String.Format("ABIERTO (restan : {0:dd} dias, {1:hh} horas, {2:mm} minutos, {3:ss} segundos", ts, ts, ts, ts) + ").";
                        return String.Format("ABIERTO (restan : {0:dd} dias, {1:hh} horas, {2:mm} minutos", ts, ts, ts) + ").";
                    }
                    else
                    {
                        return "CERRADO ";
                    }
                default:
                    return "CERRADO";
            }

        }

        public Dictionary<int, string> H_GetManttoArchivosRegistros(DateTime adt_inicio, DateTime adt_final)
        {
            Dictionary<int, string> H_MRegistros = new Dictionary<int, string>();

            string ls_comando =
                @" SELECT man.REGCODI, man.REGABREV, even.EVENCLASECODI, even.EVENCLASEABREV, even.EVENCLASEDESC, man.FECHAINI, man.FECHAFIN, man.REGNOMB  
                            FROM MAN_REGISTRO man, EVE_EVENCLASE even WHERE  man.EVENCLASECODI = even.EVENCLASECODI  ";

            ls_comando += " AND  FECHAINI >= " + EPDate.SQLDateOracleString(adt_inicio) + " AND FECHAINI < " + EPDate.SQLDateOracleString(adt_final.AddDays(1)) + " ";
            ls_comando += " ORDER BY man.FECHAINI DESC, even.EVENCLASECODI";
            //in_app.iL_data[1].Fill(i_ds, "MAN_REGISTRO", ls_comando);
            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "MAN_REGISTRO", ls_comando);

            foreach (DataRow dr in l_ds.Tables["MAN_REGISTRO"].Rows)
            {
                CManttoRegistro manreg = nf_CargarManttoRegistro(dr);

                H_MRegistros.Add(manreg.RegCodi, manreg.RegistroNombre);
            }
            return H_MRegistros;
        }

        public CManttoRegistro nf_CargarManttoRegistro(DataRow dr)
        {
            string ls_rangofecha="";
            CManttoRegistro registro = new CManttoRegistro();
            registro.RegCodi = Convert.ToInt32(dr["REGCODI"]);
            registro.FechaInicial = (DateTime)dr["FECHAINI"];
            registro.FechaFinal = (DateTime)dr["FECHAFIN"];
            registro.EvenClaseCodi = Convert.ToInt32(dr["EVENCLASECODI"]);
            registro.EvenClaseDesc = Convert.ToString(dr["EVENCLASEDESC"]);
            registro.EvenClaseAbrev = Convert.ToString(dr["EVENCLASEABREV"]);
            if (dr["FECHALIM"] != null && !String.IsNullOrEmpty(dr["FECHALIM"].ToString()))
            {
                registro.FechaLimiteFinal = (DateTime)dr["FECHALIM"];
            }
            
            if (registro.FechaInicial == registro.FechaFinal)
                    ls_rangofecha = registro.FechaInicial.ToString("dd/MM/yyyy");
                else
                    ls_rangofecha = registro.FechaInicial.ToString("dd/MM/yyyy") + " - " + registro.FechaFinal.ToString("dd/MM/yyyy");

            if (registro.EvenClaseCodi == 3)
            {
                registro.RegistroNombre = registro.EvenClaseDesc.Replace("RAMADO", ".") + " " + EPDate.f_numerosemana(registro.FechaInicial.AddDays(1)).ToString().PadLeft(2, '0') + " - " + ls_rangofecha; //"MANTENIMIENTO " +
            }
            else
            {
                registro.RegistroNombre = registro.EvenClaseDesc.Replace("RAMADO", ".") + " " + ls_rangofecha; //"MANTENIMIENTO " +
            }
            

            return registro;
        }

        public CManttoRegistro GetManttoRegistro(int ai_regcodi)
        {
            //string ls_comando = " SELECT REG.REGCODI, EVEN.EVENCLASECODI, EVEN.EVENCLASEABREV, EVEN.EVENCLASEDESC, REG.FECHAINI, REG.FECHAFIN, REG.FECHALIMINI, REG.FECHALIMSUP";
            string ls_comando = " SELECT REG.REGCODI, EVEN.EVENCLASECODI, EVEN.EVENCLASEABREV, EVEN.EVENCLASEDESC, REG.FECHAINI, REG.FECHAFIN, REG.FECHALIM";
            ls_comando += " FROM MAN_REGISTRO REG, EVE_EVENCLASE EVEN WHERE REG.EVENCLASECODI = EVEN.EVENCLASECODI  AND REG.REGCODI=" + ai_regcodi;

            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "MAN_REGISTRO", ls_comando);
            if (l_ds.Tables["MAN_REGISTRO"] != null)
            {
                return nf_CargarManttoRegistro(l_ds.Tables["MAN_REGISTRO"].Rows[0]); 
            }
            return null;
        }


        public int InsertMantto(int ai_registro, DateTime adt_ini, DateTime adt_fin, string as_fields, string as_values, string as_lastuser)
        {
            try
            {
                int numeroactualizaciones = 0;
                long ll_mancodi = -1;

                if (as_lastuser.Length > 80)
                    as_lastuser = as_lastuser.Substring(0, 80);
                string s_comando = "select evenclasecodi from man_registro where regcodi = " + ai_registro;
                int li_evenclasecodi = iL_data[0].nf_ExecuteScalar_GetInteger(s_comando);
                if (li_evenclasecodi != 5 && adt_ini.Date.AddDays(1) < adt_fin)
                {
                    DateTime dt_fechahora1 = adt_ini;
                    DateTime dt_fechahora2 = dt_fechahora1.Date.AddDays(1);

                    int numrows = 0;
                    while (dt_fechahora2 < adt_fin)
                    {
                        //INSERTAR MANTTO dt_fechahora1 - dt_fechahora2
                        ll_mancodi = iL_data[0].nf_get_next_key("MAN_MANTTO");
                        //s_comando = "INSERT INTO MAN_MANTTO (regcodi, mancodi,EVENINI, EVENFIN," + as_fields + ", lastuser, lastdate) VALUES ("
                        s_comando = "INSERT INTO MAN_MANTTO (regcodi, mancodi,EVENINI, EVENFIN," + as_fields + ", lastuser, lastdate) VALUES ("
                            + ai_registro + "," + ll_mancodi
                            + "," + EPDate.SQLDateTimeOracleString(dt_fechahora1)
                            + "," + EPDate.SQLDateTimeOracleString(dt_fechahora2)
                            + "," + as_values + ",'" + as_lastuser + "',sysdate)";

                        numrows = iL_data[0].nf_ExecuteNonQuery(s_comando);

                        if (numrows == 1)
                        {
                            numeroactualizaciones++;
                        }
                        dt_fechahora1 = dt_fechahora2;
                        dt_fechahora2 = dt_fechahora1.Date.AddDays(1);
                    }

                    dt_fechahora2 = adt_fin;
                    //INSERTAR MANTTO dt_fechahora1 - adtfin
                    //ll_mancodi = iL_data[0].nf_get_next_key("MAN_MANTTO");
                    ll_mancodi = iL_data[0].nf_get_next_key("MAN_MANTTO");
                    //s_comando = "INSERT INTO MAN_MANTTO (regcodi, mancodi,EVENINI, EVENFIN," + as_fields + ", lastuser, lastdate) VALUES ("
                    s_comando = "INSERT INTO MAN_MANTTO (regcodi, mancodi,EVENINI, EVENFIN," + as_fields + ", lastuser, lastdate) VALUES ("
                        + ai_registro + "," + ll_mancodi
                        + "," + EPDate.SQLDateTimeOracleString(dt_fechahora1)
                        + "," + EPDate.SQLDateTimeOracleString(dt_fechahora2)
                        + "," + as_values + ",'" + as_lastuser + "',sysdate)";

                    numrows = iL_data[0].nf_ExecuteNonQuery(s_comando);
                    if (numrows == 1)
                    {
                        numeroactualizaciones++;
                    }
                    return numeroactualizaciones;
                }
                else
                {
                    //ll_mancodi = iL_data[0].nf_get_next_key("MAN_MANTTO");
                    ll_mancodi = iL_data[0].nf_get_next_key("MAN_MANTTO");
                    //s_comando = "INSERT INTO MAN_MANTTO (regcodi, mancodi,EVENINI, EVENFIN," + as_fields + ", lastuser, lastdate) VALUES ("
                    s_comando = "INSERT INTO MAN_MANTTO (regcodi, mancodi,EVENINI, EVENFIN," + as_fields + ", lastuser, lastdate) VALUES ("
                        + ai_registro + "," + ll_mancodi
                        + "," + EPDate.SQLDateTimeOracleString(adt_ini)
                        + "," + EPDate.SQLDateTimeOracleString(adt_fin)
                        + "," + as_values + ",'" + as_lastuser + "',sysdate)";
                    return iL_data[0].nf_ExecuteNonQuery(s_comando);
                }
            }
            catch
            {
            }
            return -1;
        }

        public int InsertReferenciaArchivoMantto(int ai_mancodi, string as_fields, string as_values, string as_lastuser)
        {
            try
            {
                int li_resultado=0;
                try
                {
                    li_resultado = iL_data[0].nf_ExecuteScalar_GetInteger("SELECT MAX(NUMARCHIVO) FROM MAN_ARCHIVOS WHERE MANCODI=" + ai_mancodi) + 1;
                }
                catch {}


                if (as_lastuser.Length > 40)
                    as_lastuser = as_lastuser.Substring(0, 40);
                //string s_comando = "INSERT INTO MAN_ARCHIVOS (mancodi, numarchivo, " + as_fields + ", lastuser, lastdate) VALUES (" + ai_mancodi + "," + li_resultado + "," + as_values + ",'" + as_lastuser + "',sysdate)";
                string s_comando = "INSERT INTO MAN_ARCHIVOS (mancodi, numarchivo, " + as_fields + ", lastuser, lastdate) VALUES (" + ai_mancodi + "," + li_resultado + "," + as_values + ",'" + as_lastuser + "',sysdate)";
                li_resultado = iL_data[0].nf_ExecuteNonQuery(s_comando);

                s_comando = "UPDATE MAN_MANTTO SET ISFILES ='S' WHERE MANCODI=" + ai_mancodi;
                iL_data[0].nf_ExecuteNonQuery(s_comando);

                return li_resultado;
            }
            catch
            {
                return -1;
            }
        }

        public int DeleteReferenciaArchivoMantto(int ai_mancodi, int ai_numarchivo, string as_lastuser)
        {
            try
            {
                if (as_lastuser.Length > 40)
                    as_lastuser = as_lastuser.Substring(0, 40);
                //string s_comando = "UPDATE MAN_ARCHIVOS SET DELETED = 1, lastuser ='" + as_lastuser + "', lastdate=sysdate";
                string s_comando = "UPDATE MAN_ARCHIVOS SET DELETED = 1, lastuser ='" + as_lastuser + "', lastdate=sysdate";
                s_comando += " WHERE mancodi= " + ai_mancodi + " AND numarchivo= " + ai_numarchivo;
                int resultado = iL_data[0].nf_ExecuteNonQuery(s_comando);

                int li_resultado = 0;
                try
                {
                    li_resultado = iL_data[0].nf_ExecuteScalar_GetInteger("SELECT COUNT(*) FROM MAN_ARCHIVOS WHERE DELETED=0 AND MANCODI=" + ai_mancodi);
                }
                catch { }

                if (li_resultado == 0)
                {
                    s_comando = "UPDATE MAN_MANTTO SET ISFILES ='N' WHERE MANCODI=" + ai_mancodi;
                    iL_data[0].nf_ExecuteNonQuery(s_comando);
                }

                return resultado;
            }
            catch
            {
                return -1;
            }
        }

        public DataTable GetMantto(string as_empresas, int ai_registro, DateTime adt_FechaInicial, DateTime adt_FechaFinal)
        {
            DataSet li_ds = new DataSet();
            List<string> L_empresas = EPString.EP_GetListStringSeparate(as_empresas, ',');
            string s_filtro = " AND EQ_EQUIPO.EMPRCODI in (" + as_empresas + ")";
            if (L_empresas.Count == 0 || L_empresas.Contains("0"))
                s_filtro = "";

            string s_comando = "SELECT MAN.MANCODI, MAN.EVENINI, MAN.EVENFIN, MAN.EVENMWINDISP, MAN.EVENINDISPO, MAN.EVENINTERRUP, MAN.TIPOEVENCODI, MAN.SUBCAUSACODI, MAN.EVENTIPOPROG , MAN.EMPRCODIREPORTA, MAN.EVENDESCRIP, MAN.EVENOBSRV, MAN.EVENPROCESADO,";
            s_comando += " MAN.DELETED, MAN.MANTTOCODI, MAN.ISFILES,";
            s_comando += " EVE_TIPOEVENTO.TIPOEVENABREV, SI_EMPRESA.EMPRNOMB, EQ_AREA.AREANOMB, EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EMPRCODI, EQ_EQUIPO.AREACODI, EQ_FAMILIA.TAREACODI, EQ_EQUIPO.EQUIPOT, MAN.LASTDATE, MAN.LASTUSER, MAN.CREATED";
            s_comando += " FROM MAN_MANTTO MAN, EQ_EQUIPO, EQ_FAMILIA, EQ_AREA, SI_EMPRESA, EVE_TIPOEVENTO ";
            s_comando += " WHERE MAN.EQUICODI=EQ_EQUIPO.EQUICODI AND EQ_FAMILIA.FAMCODI=EQ_EQUIPO.FAMCODI AND EQ_EQUIPO.AREACODI=EQ_AREA.AREACODI AND EQ_EQUIPO.EMPRCODI=SI_EMPRESA.EMPRCODI AND EVE_TIPOEVENTO.TIPOEVENCODI = MAN.TIPOEVENCODI ";
            s_comando += " AND MAN.regcodi = " + ai_registro.ToString() + s_filtro;
            s_comando += " AND MAN.EVENINI >= " + EPDate.SQLDateOracleString(adt_FechaInicial);
            s_comando += " AND MAN.EVENFIN <= " + EPDate.SQLDateOracleString(adt_FechaFinal);
            s_comando += " AND MAN.EVENINI < MAN.EVENFIN";
            s_comando += " ORDER BY EMPRNOMB, AREANOMB, FAMABREV, EQUIABREV,TIPOEVENCODI, EVENINI, DELETED ";

            iL_data[0].Fill(li_ds, "MAN_MANTTO", s_comando);
            return li_ds.Tables["MAN_MANTTO"];
        }

        public DataTable GetMantto(string as_empresas, int ai_registro)
        {
            DataSet li_ds = new DataSet();
            List<string> L_empresas = EPString.EP_GetListStringSeparate(as_empresas, ',');
            string s_filtro = " AND EQ_EQUIPO.EMPRCODI in (" + as_empresas + ")";
            if (L_empresas.Count == 0 || L_empresas.Contains("0"))
                s_filtro = "";

            string s_comando = "SELECT MAN.MANCODI, MAN.EVENINI, MAN.EVENFIN, MAN.EVENMWINDISP, MAN.EVENINDISPO, MAN.EVENINTERRUP, MAN.TIPOEVENCODI, MAN.SUBCAUSACODI, MAN.EVENTIPOPROG , MAN.EMPRCODIREPORTA, MAN.EVENDESCRIP, MAN.EVENOBSRV, MAN.EVENPROCESADO,";
            s_comando += " MAN.DELETED, MAN.MANTTOCODI, MAN.ISFILES,";
            s_comando += " EVE_TIPOEVENTO.TIPOEVENABREV, SI_EMPRESA.EMPRNOMB, EQ_AREA.AREANOMB, EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EMPRCODI, EQ_EQUIPO.AREACODI, EQ_FAMILIA.TAREACODI, EQ_EQUIPO.EQUIPOT, MAN.LASTDATE, MAN.LASTUSER, MAN.CREATED";
            s_comando += " FROM MAN_MANTTO MAN, EQ_EQUIPO, EQ_FAMILIA, EQ_AREA, SI_EMPRESA, EVE_TIPOEVENTO"
             + " WHERE MAN.EQUICODI=EQ_EQUIPO.EQUICODI AND EQ_FAMILIA.FAMCODI=EQ_EQUIPO.FAMCODI AND EQ_EQUIPO.AREACODI=EQ_AREA.AREACODI AND EQ_EQUIPO.EMPRCODI=SI_EMPRESA.EMPRCODI AND EVE_TIPOEVENTO.TIPOEVENCODI = MAN.TIPOEVENCODI ";
            s_comando += " AND MAN.regcodi = " + ai_registro.ToString() + s_filtro //+ " AND DELETED = 0 "
               + " ORDER BY EMPRNOMB, AREANOMB, FAMABREV, EQUIABREV,TIPOEVENCODI, EVENINI, DELETED ";

            iL_data[0].Fill(li_ds, "MAN_MANTTO", s_comando);
            return li_ds.Tables["MAN_MANTTO"];
        }

        public DataTable GetMantto(int ai_mancodi)
        {
            DataSet li_ds = new DataSet();
            string s_comando = "SELECT MAN.MANCODI, MAN.EVENINI, MAN.EVENFIN, MAN.EVENMWINDISP, MAN.EVENINDISPO, MAN.EVENINTERRUP, MAN.TIPOEVENCODI, MAN.SUBCAUSACODI, MAN.EVENTIPOPROG , MAN.EMPRCODIREPORTA, MAN.EVENDESCRIP, MAN.EVENOBSRV, MAN.EVENPROCESADO, MAN.DELETED, MAN.MANTTOCODI, MAN.ISFILES,";
            s_comando += " EVE_TIPOEVENTO.TIPOEVENABREV, SI_EMPRESA.EMPRNOMB, EQ_AREA.AREANOMB, EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EMPRCODI, EQ_EQUIPO.AREACODI, EQ_FAMILIA.TAREACODI, EQ_EQUIPO.EQUIPOT, MAN.LASTDATE, MAN.LASTUSER, MAN.CREATED";
            s_comando += " FROM MAN_MANTTO MAN, EQ_EQUIPO, EQ_FAMILIA, EQ_AREA, SI_EMPRESA, EVE_TIPOEVENTO"
             + " WHERE MAN.EQUICODI=EQ_EQUIPO.EQUICODI AND EQ_FAMILIA.FAMCODI=EQ_EQUIPO.FAMCODI AND EQ_EQUIPO.AREACODI=EQ_AREA.AREACODI AND EQ_EQUIPO.EMPRCODI=SI_EMPRESA.EMPRCODI AND EVE_TIPOEVENTO.TIPOEVENCODI = MAN.TIPOEVENCODI ";
            s_comando += " AND MAN.mancodi = " + ai_mancodi.ToString();

            iL_data[0].Fill(li_ds, "MAN_MANTTO", s_comando);
            return li_ds.Tables["MAN_MANTTO"];
        }

        public DataTable GetArchivosMantto(int ai_mancodi)
        {
            DataSet li_ds = new DataSet();
            string s_comando = "SELECT *  FROM MAN_ARCHIVOS";
            s_comando += " WHERE MANCODI = " + ai_mancodi.ToString() + " AND DELETED=0";

            iL_data[0].Fill(li_ds, "MAN_ARCHIVOS", s_comando);
            return li_ds.Tables["MAN_ARCHIVOS"];
        }

        public DataTable GetManttoPrint(string as_empresas, int ai_registro)
        {
            DataSet li_ds = new DataSet();
            List<string> L_empresas = EPString.EP_GetListStringSeparate(as_empresas, ',');
            string s_filtro = " AND EQ_EQUIPO.EMPRCODI in (" + as_empresas + ")";
            if (L_empresas.Count == 0 || L_empresas.Contains("0"))
                s_filtro = "";

            string s_comando = "SELECT SI_EMPRESA.EMPRNOMB, EQ_AREA.AREANOMB, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EQUICODI, MAN.EVENINI, MAN.EVENFIN, MAN.EVENDESCRIP, MAN.EVENMWINDISP, MAN.EVENINDISPO, MAN.EVENINTERRUP, EVE_TIPOEVENTO.TIPOEVENABREV, MAN.EVENTIPOPROG, MAN.SUBCAUSACODI ";
            s_comando += " FROM MAN_MANTTO MAN, EQ_EQUIPO, EQ_AREA, SI_EMPRESA, EVE_TIPOEVENTO"
            + " WHERE MAN.EQUICODI=EQ_EQUIPO.EQUICODI AND EQ_EQUIPO.AREACODI=EQ_AREA.AREACODI AND EQ_EQUIPO.EMPRCODI=SI_EMPRESA.EMPRCODI AND EVE_TIPOEVENTO.TIPOEVENCODI = MAN.TIPOEVENCODI ";
            s_comando += " AND MAN.regcodi = " + ai_registro.ToString() + s_filtro + " AND DELETED = 0 "
            + " ORDER BY EMPRNOMB, AREANOMB, EQUIABREV, EVENINI";

            iL_data[0].Fill(li_ds, "MAN_MANTTO", s_comando);
            return li_ds.Tables["MAN_MANTTO"];
        }

        public int CopyMantto(string as_empresas, int ai_registro, int ai_evenclasecodi, DateTime adt_FechaInicial, DateTime adt_FechaFinal, string as_lastuser)
        {
            DateTime idt_initial = adt_FechaInicial;
            DateTime idt_final = adt_FechaFinal.AddDays(1);

            int li_resultado = 0;
            try
            {
                li_resultado = iL_data[0].nf_ExecuteScalar_GetInteger("SELECT COUNT(*) FROM MAN_MANTTO MAN, EQ_EQUIPO EQUI WHERE MAN.EQUICODI=EQUI.EQUICODI AND MAN.DELETED=0 AND MAN.MANTTOCODI>=0 AND MAN.REGCODI=" + ai_registro + " AND EQUI.EMPRCODI in (" + as_empresas + ")");
            }
            catch { }

            if (li_resultado > 0) return 0;

            DataSet li_ds = new DataSet();
            List<string> L_empresas = EPString.EP_GetListStringSeparate(as_empresas, ',');
            string s_filtro = " AND EQ_EQUIPO.EMPRCODI in (" + as_empresas + ")";
            if (L_empresas.Count == 0 || L_empresas.Contains("0"))
                return 0;//s_filtro = "";

            string s_comando = "SELECT EVE_MANTTO.MANTTOCODI,EVE_MANTTO.EQUICODI,EVE_MANTTO.EVENCLASECODI,EVE_MANTTO.TIPOEVENCODI,EVE_MANTTO.COMPCODE,EVE_MANTTO.EVENINI,EVE_MANTTO.EVENPREINI,EVE_MANTTO.EVENFIN,EVE_MANTTO.SUBCAUSACODI,EVE_MANTTO.EVENPREFIN,"+
"EVE_MANTTO.EVENMWINDISP,EVE_MANTTO.EVENPADRE,EVE_MANTTO.EVENINDISPO,EVE_MANTTO.EVENINTERRUP,EVE_MANTTO.EVENTIPOPROG,EVE_MANTTO.EVENDESCRIP,EVE_MANTTO.EVENOBSRV,"+
"EVE_MANTTO.EVENESTADO, EVE_MANTTO.LASTUSER,EVE_MANTTO.LASTDATE,EVE_MANTTO.EVENRELEVANTE,EVE_MANTTO.DELETED,EVE_MANTTO.MANCODI,EVE_MANTTO.EQUIMANTRELEV,EVE_MANTTO.MANTRELEVLASTUSER,EVE_MANTTO.MANTRELEVLASTDATE,"+
"EVE_MANTTO.INTERCODI"+             
                ", SI_EMPRESA.EMPRNOMB, EQ_AREA.AREANOMB, EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EMPRCODI, EQ_EQUIPO.AREACODI, EQ_FAMILIA.TAREACODI, EQ_EQUIPO.EQUIPOT FROM EVE_MANTTO, EQ_EQUIPO, EQ_FAMILIA, EQ_AREA, SI_EMPRESA"
           + " WHERE EVE_MANTTO.EQUICODI=EQ_EQUIPO.EQUICODI AND EQ_FAMILIA.FAMCODI=EQ_EQUIPO.FAMCODI AND EQ_EQUIPO.AREACODI=EQ_AREA.AREACODI AND EQ_EQUIPO.EMPRCODI=SI_EMPRESA.EMPRCODI";

            
            s_comando += " AND EVENCLASECODI = " + ai_evenclasecodi.ToString();

            s_comando += " AND (( EVENINI >= " + EPDate.SQLDateOracleString(idt_initial) + " AND EVENINI < " + EPDate.SQLDateOracleString(idt_final) + " ) OR "
               + "( EVENFIN > " + EPDate.SQLDateOracleString(idt_initial) + " AND EVENFIN <= " + EPDate.SQLDateOracleString(idt_final) + " ) OR "
               + "( EVENINI < " + EPDate.SQLDateOracleString(idt_initial) + " AND EVENFIN > " + EPDate.SQLDateOracleString(idt_final) + " )) "
               + " AND DELETED = 0 "
               + " ORDER BY EMPRNOMB, AREANOMB, FAMABREV, EQUIABREV, EVENCLASECODI, EVENINI";

            if (li_ds.Tables["EVE_MANTTO"] != null) li_ds.Tables["EVE_MANTTO"].Clear();            
            iL_data[0].Fill(li_ds, "EVE_MANTTO", s_comando);
            int li_contador = 0;
            foreach (DataRow dr in li_ds.Tables["EVE_MANTTO"].Rows)
            {
                CMantto mantto = new CMantto();
                mantto.LoadDR(dr);
                if (L_empresas.Contains(mantto.i_equipo_emprcodi.ToString().Trim()))
                {
                    mantto.MAN_InsertNew(iL_data[0], ai_registro, as_lastuser);
                    li_contador++;
                }
            }
            return li_contador;
        }
        //public DataTable GetMantto(int ai_EvenClaseCodi, DateTime adt_EvenIni, DateTime adt_EvenFin)
        //{
        //    DataSet li_ds = new DataSet();
        //    //string s_command = "SELECT * FROM   MAN_MANTTO WHERE EVENCLASECODI = " + ai_EvenClaseCodi +
        //    //    " and EVENINI >= " + fwapp.EPDate.SQLDateOracleString(adt_EvenIni) + " AND EVENINI < " + fwapp.EPDate.SQLDateOracleString(adt_EvenFin.AddDays(1));

        //    string s_comando = "SELECT MAN.MANCODI, MAN.EVENINI, MAN.EVENFIN, MAN.EVENMWINDISP, MAN.EVENINDISPO, MAN.EMPRCODI,MAN.EVENDESCRIP , ";
        //    s_comando += " SI_EMPRESA.EMPRNOMB, EQ_AREA.AREANOMB, EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EMPRCODI, EQ_EQUIPO.AREACODI, EQ_FAMILIA.TAREACODI, EQ_EQUIPO.EQUIPOT";
        //    s_comando += " FROM MAN_MANTTO MAN, EQ_EQUIPO, EQ_FAMILIA, EQ_AREA, SI_EMPRESA"
        //     + " WHERE MAN.EQUICODI=EQ_EQUIPO.EQUICODI AND EQ_FAMILIA.FAMCODI=EQ_EQUIPO.FAMCODI AND EQ_EQUIPO.AREACODI=EQ_AREA.AREACODI AND EQ_EQUIPO.EMPRCODI=SI_EMPRESA.EMPRCODI";

        //    s_comando += " AND EVENCLASECODI = " + ai_EvenClaseCodi.ToString();

        //    s_comando += " AND (( EVENINI >= " + EPDate.SQLDateOracleString(adt_EvenIni) + " AND EVENINI < " + EPDate.SQLDateOracleString(adt_EvenFin.AddDays(1)) + " ) OR "
        //       + "( EVENFIN > " + EPDate.SQLDateOracleString(adt_EvenIni) + " AND EVENFIN <= " + EPDate.SQLDateOracleString(adt_EvenFin) + " ) OR "
        //       + "( EVENINI < " + EPDate.SQLDateOracleString(adt_EvenIni) + " AND EVENFIN > " + EPDate.SQLDateOracleString(adt_EvenFin) + " )) "
        //       + " AND DELETED = 0 "
        //       + " ORDER BY EMPRNOMB, AREANOMB, FAMABREV, EQUIABREV, EVENINI";

        //    iL_data[0].Fill(li_ds, "MAN_MANTTO", s_comando);
        //    return li_ds.Tables["MAN_MANTTO"];
        //}

        public int DeleteMantto(int ai_mancodi, string as_lastuser)
        {
            string s_comando = "UPDATE MAN_MANTTO SET DELETED=1 , LASTUSER='" + as_lastuser + "', LASTDATE=sysdate WHERE MANCODI=" + ai_mancodi;
            return iL_data[0].nf_ExecuteNonQuery(s_comando);
        }

        public int UndoDeleteMantto(int ai_mancodi, string as_lastuser)
        {
            string s_comando = "UPDATE MAN_MANTTO SET DELETED=0 , LASTUSER='" + as_lastuser + "', LASTDATE=sysdate WHERE MANCODI=" + ai_mancodi;
            return iL_data[0].nf_ExecuteNonQuery(s_comando);
        }   

        //public int UpdateMantto(int ai_mancodi, string as_SetFields, string as_lastuser)
        //{

        //    if (as_lastuser.Length > 80)
        //        as_lastuser = as_lastuser.Substring(0, 80);
        //    string s_comando = "UPDATE MAN_MANTTO SET " + as_SetFields + " , LASTUSER='" + as_lastuser + "', LASTDATE=sysdate";
        //    s_comando += " WHERE MANCODI=" + ai_mancodi;
        //    return iL_data[0].nf_ExecuteNonQuery(s_comando);
        //}

        public int UpdateMantto(int ai_mancodi, DateTime adt_ini, DateTime adt_fin, string as_SetFields, string as_lastuser)
        {
            if (as_lastuser.Length > 80)
                as_lastuser = as_lastuser.Substring(0, 80);
            string s_comando = "select reg.evenclasecodi, reg.fechaini, reg.fechafin from man_registro reg, man_mantto man where reg.regcodi = man.regcodi and man.mancodi=" + ai_mancodi;
            DataSet ds =new DataSet();
            iL_data[0].Fill(ds, "MAN_REGISTROX", s_comando);

            if (ds.Tables["MAN_REGISTROX"].Rows.Count == 1)
            {
                DataRow dr= ds.Tables["MAN_REGISTROX"].Rows[0];
                DateTime ldt_inicial = (DateTime)dr["FECHAINI"];
                DateTime ldt_final = (DateTime)dr["FECHAFIN"];
                if (adt_ini >= ldt_inicial && adt_ini < ldt_final.AddDays(1) && adt_fin > ldt_inicial && adt_fin <= ldt_final.AddDays(1))
                {
                    s_comando = "UPDATE MAN_MANTTO SET EVENINI = " + EPDate.SQLDateTimeOracleString(adt_ini) + ", EVENFIN =" + EPDate.SQLDateTimeOracleString(adt_fin)
                        + ", " + as_SetFields + " , LASTUSER='" + as_lastuser + "', LASTDATE=sysdate"
                        + " WHERE MANCODI=" + ai_mancodi;

                    return iL_data[0].nf_ExecuteNonQuery(s_comando);
                }
            }
            else
            {
                return -1;
            }            
            return -1;
        }

        public int UpdateMantto(int ai_mancodi, DateTime adt_ini, DateTime adt_fin, string as_evendescrip, string as_evenobsrv, string as_evenindispo, string as_tipoevencodi, string as_eventipoprog, string as_eveninterrup, string as_evenmwindisp, string as_lastuser)
        {
            try
            {
                int numeroactualizaciones = 0;
                long ll_mancodi = -1;

                if (as_lastuser.Length > 80)
                    as_lastuser = as_lastuser.Substring(0, 80);

                if (as_evenobsrv.Trim() == as_evendescrip.Trim())
                {
                    as_evenobsrv = "";
                }

                string s_comando = "select equicodi from man_mantto where mancodi = " + ai_mancodi;
                int li_equicodi = iL_data[0].nf_ExecuteScalar_GetInteger(s_comando);

                s_comando = "select reg.evenclasecodi, reg.fechaini, reg.fechafin, man.regcodi from man_registro reg, man_mantto man where reg.regcodi = man.regcodi and man.mancodi=" + ai_mancodi;
                DataSet ds = new DataSet();
                iL_data[0].Fill(ds, "MAN_REGISTROX", s_comando);
                if (ds.Tables["MAN_REGISTROX"].Rows.Count != 1) return -1;
                DataRow dr = ds.Tables["MAN_REGISTROX"].Rows[0];
                DateTime ldt_inicial = (DateTime)dr["FECHAINI"];
                DateTime ldt_final = (DateTime)dr["FECHAFIN"];
                int li_evenclasecodi = Convert.ToInt32(dr["EVENCLASECODI"]);
                int li_registro = Convert.ToInt32(dr["REGCODI"]);
                int numrows = 0;
                DateTime dt_fechahora1 = adt_ini;
                DateTime dt_fechahora2 = adt_fin;

                if (adt_ini >= ldt_inicial && adt_ini < ldt_final.AddDays(1) && adt_fin > ldt_inicial && adt_fin <= ldt_final.AddDays(1))
                {
                    if (li_evenclasecodi != 5 && adt_ini.Date.AddDays(1) < adt_fin && adt_ini.AddDays(31) >= adt_fin)
                    {
                        dt_fechahora1 = adt_ini;
                        dt_fechahora2 = dt_fechahora1.Date.AddDays(1);

                        while (dt_fechahora2 < adt_fin)
                        {
                            //INSERTAR MANTTO dt_fechahora1 - dt_fechahora2
                            ll_mancodi = iL_data[0].nf_get_next_key("MAN_MANTTO");
                            //s_comando = "INSERT INTO MAN_MANTTO (regcodi, mancodi, equicodi, EVENINI, EVENFIN, EVENDESCRIP, EVENOBSRV, EVENINDISPO, TIPOEVENCODI, EVENTIPOPROG, EVENINTERRUP, EVENMWINDISP, LASTUSER, LASTDATE) VALUES ("
                            s_comando = "INSERT INTO MAN_MANTTO (regcodi, mancodi, equicodi, EVENINI, EVENFIN, EVENDESCRIP, EVENOBSRV, EVENINDISPO, TIPOEVENCODI, EVENTIPOPROG, EVENINTERRUP, EVENMWINDISP, LASTUSER, LASTDATE) VALUES ("
                                + li_registro + "," + ll_mancodi + "," + li_equicodi
                                + "," + EPDate.SQLDateTimeOracleString(dt_fechahora1)
                                + "," + EPDate.SQLDateTimeOracleString(dt_fechahora2)
                                + ",'" + as_evendescrip + "'"
                                + ",'" + as_evenobsrv + "'"
                                + ",'" + as_evenindispo + "'"
                                + "," + as_tipoevencodi
                                + ",'" + as_eventipoprog + "'"
                                + ",'" + as_eveninterrup + "'"
                                + "," + as_evenmwindisp
                                + ",'" + as_lastuser + "',sysdate)";

                            numrows = iL_data[0].nf_ExecuteNonQuery(s_comando);

                            if (numrows == 1)
                            {
                                numeroactualizaciones++;
                            }
                            dt_fechahora1 = dt_fechahora2;
                            dt_fechahora2 = dt_fechahora1.Date.AddDays(1);
                        }
                        dt_fechahora2 = adt_fin;
                    }

                    //s_comando = "UPDATE MAN_MANTTO SET EVENINI = " + EPDate.SQLDateTimeOracleString(dt_fechahora1)
                    s_comando = "UPDATE MAN_MANTTO SET EVENINI = " + EPDate.SQLDateTimeOracleString(dt_fechahora1)
                        + ", EVENFIN =" + EPDate.SQLDateTimeOracleString(dt_fechahora2)
                        + ", EVENDESCRIP='" + as_evendescrip + "'"
                        + ", EVENOBSRV='" + as_evenobsrv + "'"
                        + ", EVENINDISPO='" + as_evenindispo + "'"
                        + ", TIPOEVENCODI=" + as_tipoevencodi
                        + ", EVENTIPOPROG='" + as_eventipoprog + "'"
                        + ", EVENINTERRUP='" + as_eveninterrup + "'"
                        + ", EVENMWINDISP=" + as_evenmwindisp
                        + ", LASTUSER='" + as_lastuser + "', LASTDATE=sysdate"
                        + " WHERE MANCODI=" + ai_mancodi;

                    numrows = iL_data[0].nf_ExecuteNonQuery(s_comando);
                    if (numrows == 1)
                    {
                        numeroactualizaciones++;
                    }
                    return numeroactualizaciones;
                }

            }
            catch
            {
            }
            return -1;
        }

        public bool ValidateRegMan(DateTime adt_fechini, int ai_evenclasecodi)
        {
            bool lb_existe = false;
            string s_comando = "select count(*) from man_registro where evenclasecodi = " + ai_evenclasecodi;
            s_comando += " and fechaini = " + EPDate.SQLDateOracleString(adt_fechini);
            int li_registros = iL_data[0].nf_ExecuteScalar_GetInteger(s_comando);

            if (li_registros == 0)
            {
                lb_existe = true;
            }

            return lb_existe;
        }

        public int CreateFechaLim(int ai_regcodi, DateTime adt_fechalim, string as_lastuser)
        {
            if (ai_regcodi > 0)
            {

                string ls_user = as_lastuser;
                if (ls_user.Length > 20) ls_user = ls_user.Substring(0, 20);
                string ls_comando = @" UPDATE MAN_REGISTRO SET FECHALIM = " + EPDate.SQLDateTimeOracleString(adt_fechalim) + ", LASTUSER = '" + ls_user + "', LASTDATE = SYSDATE WHERE REGCODI = " + ai_regcodi.ToString();
                try
                {
                    if (iL_data[0].nf_ExecuteNonQuery(ls_comando) > 0)
                        return 1;
                }
                catch
                {
                    return -5;
                }

            }
            Console.WriteLine("Error en obtener ultimo key!");
            return -1;
        }

        public int CreateRegMan(DateTime adt_fechini, string as_regnomb, int ai_evenclasecodi, string as_lastuser)
        {
            DateTime startDate = adt_fechini;
            DateTime endDate = new DateTime(2000, 1, 1);
            switch (ai_evenclasecodi)
            {
                case 1: //EJECUTADO
                case 2: //PDIARIO
                case 6: //AJUSTE DIARIO
                    endDate = startDate;
                    break;
                case 3: //SEMANAL
                    if (startDate.DayOfWeek != DayOfWeek.Saturday)
                    {
                        Console.WriteLine("Fecha no es inicio de semana operativa!");
                        return -2;
                    }
                    endDate = startDate.AddDays(6);
                    break;
                case 4://MENSUAL
                    if (startDate.Day != 1)
                    {
                        Console.WriteLine("Fecha no es inicio de mes!");
                        return -3;
                    }
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case 5://ANUAL
                    if (startDate.Day != 1)
                    {
                        Console.WriteLine("Fecha no es inicio de mes!");
                        return -4;
                    }
                    endDate = startDate.AddYears(1).AddDays(-1);
                    break;
            }


            int nextk = iL_data[0].nf_get_next_key("MAN_REGISTRO");
            if (nextk > 0)
            {

                string ls_user = as_lastuser;
                if (ls_user.Length > 20) ls_user = ls_user.Substring(0, 20);
                //string ls_comando = @" INSERT INTO MAN_REGISTRO (REGCODI, REGABREV, FECHAINI, FECHAFIN, REGNOMB, EVENCLASECODI,LASTUSER) VALUES(" +
                string ls_comando = @" INSERT INTO MAN_REGISTRO (REGCODI, REGABREV, FECHAINI, FECHAFIN, REGNOMB, EVENCLASECODI,LASTUSER) VALUES(" +
                    nextk + ",''," + EPDate.SQLDateOracleString(startDate) + "," + EPDate.SQLDateOracleString(endDate) + ",'" + as_regnomb + "'," + ai_evenclasecodi + ",'" + ls_user + "')";
                try
                {
                    if (iL_data[0].nf_ExecuteNonQuery(ls_comando) > 0)
                        return nextk;
                }
                catch
                {
                    return -5;
                }

            }
            Console.WriteLine("Error en obtener ultimo key!");
            return -1;
        }

        public int Create_EnvioArchivo(int ai_etacodi, int ai_emprcodi, string as_arch_nomb, double ad_arch_tam, string as_arch_vers, string as_arch_ruta, int ai_usercode, string as_user_ip, string as_last_user, DateTime ad_fecha, int ai_estad_env, string as_copiado)
        {
            int nextk = iL_data[0].nf_get_next_key("EXT_ARCHIVO");

            string ls_user = as_last_user;
            string ls_archruta = as_arch_ruta;
            if (ls_user.Length > 40) ls_user = ls_user.Substring(0, 40);
            if (ls_archruta.Length > 50) ls_archruta = ls_archruta.Substring(0, 50);
            string ls_comando = @" INSERT INTO EXT_ARCHIVO (EARCODI, ETACODI, EARARCHTAMMB, EARARCHNOMB, EARARCHVER, EARARCHRUTA, USERCODE,EARIP,LASTUSER,LASTDATE,EARFECHA,ESTENVCODI,EARCOPIADO,EMPRCODI) VALUES(" +
                nextk + "," + ai_etacodi.ToString() + "," + ad_arch_tam.ToString() + ",'" + as_arch_nomb + "','" + as_arch_vers + "','" + ls_archruta + "'," + ai_usercode.ToString() + ",'" + as_user_ip + "','" + as_last_user + "',SYSDATE," + EPDate.SQLDateTimeOracleString(ad_fecha) + "," + ai_estad_env.ToString() + ",'" + as_copiado + "', -1)";
            try
            {
                if (iL_data[0].nf_ExecuteNonQuery(ls_comando) > 0)
                    return nextk;
            }
            catch
            {
                return -5;
            }

            Console.WriteLine("Error en obtener ultimo key!");
            return -1;
        }

        //public Dictionary<int, string> H_GetEquipos(string as_empresas)
        //{
        //    Dictionary<int, string> H_Equipos = new Dictionary<int, string>();

        //    string ls_comando = @" SI_EMPRESA.EMPRNOMB, EQ_AREA.AREANOMB, EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EMPRCODI, EQ_EQUIPO.AREACODI, EQ_EQUIPO.EQUICODI";
        //    ls_comando += " FROM EQ_EQUIPO, EQ_FAMILIA, EQ_AREA, SI_EMPRESA"
        //    + " WHERE EQ_FAMILIA.FAMCODI=EQ_EQUIPO.FAMCODI AND EQ_EQUIPO.AREACODI=EQ_AREA.AREACODI AND EQ_EQUIPO.EMPRCODI=SI_EMPRESA.EMPRCODI ORDER BY EMPRNOMB, AREANOMB, FAMABREV,EQUIABREV";
        //    DataSet l_ds = new DataSet();
        //    iL_data[0].Fill(l_ds, "EQ_EQUIPO", ls_comando);

        //    foreach (DataRow dr in l_ds.Tables["EQ_EQUIPO"].Rows)
        //    {

        //        CEquipo equipo = new CEquipo();

        //        H_Equipos.Add(Convert.ToInt32(dr["EQUICODI"]), dr["EQUIABREV"].ToString());
        //    }
        //    return H_Equipos;
        //}

        public List <int> L_GetCodiEquipos(string as_empresas)
        {
            List<int> L_Equipos = new List<int>();
            string ls_comando = @"SELECT EQUICODI FROM EQ_EQUIPO WHERE EQUICODI IN (" + as_empresas + ") AND EQUIESTADO <> 'B' ORDER BY EQUICODI";
            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "EQ_EQUIPO", ls_comando);

            foreach (DataRow dr in l_ds.Tables["EQ_EQUIPO"].Rows)
                L_Equipos.Add(Convert.ToInt32(dr["EQUICODI"]));

            return L_Equipos;
        }

        public string GetEmpresaNombre(int as_emprecodi)
        {
            string s_comando = "SELECT EMPRNOMB FROM SI_EMPRESA WHERE EMPRCODI=" + as_emprecodi;
            return (string)iL_data[0].nf_ExecuteScalar(s_comando);            
        }

        public CascadingDropDownNameValue[] GetEmpresas(string knownCategoryValues, string category,string contextKey)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            //int emprCodi;
            //if (!kv.ContainsKey("Empresa") || !Int32.TryParse(kv["Empresa"], out emprCodi))
            //{
            //    return null;
            //}

            //if (!(emprCodi > 0)) return null;

            DataSet li_ds = new DataSet();
            string s_comando = "SELECT EMPRNOMB, EMPRCODI FROM SI_EMPRESA WHERE EMPRSEIN='S' AND EMPRCODI>0 ";
            if (contextKey.Trim() != "0")
            {
                s_comando += " AND EMPRCODI IN (" + contextKey +") ";
            }
            s_comando += " ORDER BY EMPRNOMB ";
            iL_data[0].Fill(li_ds, "SI_EMPRESA", s_comando);
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            foreach (DataRow dr in li_ds.Tables["SI_EMPRESA"].Rows)
            {
                if (dr["EMPRNOMB"] != DBNull.Value && dr["EMPRCODI"] != DBNull.Value )
                {
                    string empresa = (string)dr["EMPRNOMB"];
                    int emprcodi = (int)dr["EMPRCODI"];
                    values.Add(new CascadingDropDownNameValue(empresa, emprcodi.ToString()));
                }
            }
            return values.ToArray();
        }

        public CascadingDropDownNameValue[] GetAreasPorEmpresa(string knownCategoryValues, string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int emprCodi;
            if (!kv.ContainsKey("Empresa") || !Int32.TryParse(kv["Empresa"], out emprCodi))
            {
                return null;
            }

            if (!(emprCodi > 0)) return null;
            DataSet li_ds = new DataSet();
            //string s_comando = "SELECT AREANOMB, AREACODI FROM EQ_AREA WHERE EMPRCODI=" + emprCodi + " ORDER BY AREANOMB ";
            string s_comando = "SELECT DISTINCT eq_area.areacodi, eq_area.areanomb, eq_tipoarea.tareaabrev FROM eq_area, eq_equipo, eq_tipoarea";
            s_comando += " WHERE ( eq_area.areacodi = eq_equipo.areacodi ) and  ( eq_area.tareacodi = eq_tipoarea.tareacodi ) and eq_equipo.areacodi > 0 and ( (";
            s_comando += emprCodi + " = 0 ) or (eq_area.areacodi = 0) or( eq_equipo.emprcodi = " + emprCodi + " )) ORDER BY eq_area.areanomb ";

            iL_data[0].Fill(li_ds, "EQ_AREA", s_comando);

            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            foreach (DataRow dr in li_ds.Tables["EQ_AREA"].Rows)
            {
                if (dr["TAREAABREV"] != DBNull.Value && dr["AREANOMB"] != DBNull.Value && dr["AREACODI"] != DBNull.Value)
                //values.Add(new CascadingDropDownNameValue((string)dr["TAREAABREV"] +" "+ (string)dr["AREANOMB"], dr["AREACODI"].ToString()));
                    values.Add(new CascadingDropDownNameValue((string)dr["TAREAABREV"] + " " + (string)dr["AREANOMB"], emprCodi.ToString().Trim() + "," + dr["AREACODI"].ToString())); // 
            }
            return values.ToArray();
        }

        public CascadingDropDownNameValue[] GetEquiposPorArea(string knownCategoryValues, string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            //int areaCodi;
            //if (!kv.ContainsKey("Area") || !Int32.TryParse(kv["Area"], out areaCodi))
            if (!kv.ContainsKey("Area")) 
            {
                return null;
            }
            string s_valor = kv["Area"].ToString();
            ArrayList L_items = EPString.EP_GetArrayStringSeparate(s_valor,',');
            int li_emprcodi = Convert.ToInt32(L_items[0]);
            int li_areacodi = Convert.ToInt32(L_items[1]);
            //int li_emprcodi = Convert.ToInt32(L_items[0]);
            //int li_areacodi = Convert.ToInt32(kv["Area"].ToString());

            DataSet li_ds = new DataSet();
            //string s_comando = "SELECT EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EQUICODI  FROM EQ_EQUIPO, EQ_FAMILIA WHERE EQ_FAMILIA.FAMCODI=EQ_EQUIPO.FAMCODI AND EQ_EQUIPO.EQUIESTADO<>'B' AND  EQ_EQUIPO.AREACODI=" + areaCodi;
            //s_comando += " ORDER BY  EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV";
            string s_comando = "SELECT EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EQUICODI  FROM EQ_EQUIPO, EQ_FAMILIA WHERE EQ_FAMILIA.FAMCODI=EQ_EQUIPO.FAMCODI AND EQ_EQUIPO.EQUIESTADO<>'B' ";
            s_comando += " AND  EQ_EQUIPO.AREACODI=" + li_areacodi +" AND  EQ_EQUIPO.EMPRCODI=" + li_emprcodi;
            s_comando += " ORDER BY  EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV";

            iL_data[0].Fill(li_ds, "EQ_EQUIPO", s_comando);
            List<CascadingDropDownNameValue> values =
              new List<CascadingDropDownNameValue>();
            foreach (DataRow dr in li_ds.Tables["EQ_EQUIPO"].Rows)
            {
                if (dr["FAMABREV"] != DBNull.Value && dr["EQUIABREV"] != DBNull.Value && dr["EQUICODI"] != DBNull.Value)
                    values.Add(new CascadingDropDownNameValue( dr["FAMABREV"] + " - " + (string)dr["EQUIABREV"]+ "\t (" 
                        + Convert.ToString(dr["EQUICODI"])+ ")", dr["EQUICODI"].ToString()));
            }
            return values.ToArray();
        }

        public bool GetEquipoValido(string ps_empresas, int pi_equicodi, out string ps_mensaje)
        {
            bool lb_existe = false;
            ps_mensaje = "El código del equipo cargado no pertenece a las empresas asignadas al usuario";
            string ls_estado = String.Empty;
            string ls_comando = "SELECT EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV, EQ_EQUIPO.EQUICODI, EQ_EQUIPO.EQUIESTADO FROM EQ_EQUIPO, EQ_FAMILIA ";
            ls_comando += "WHERE EQ_FAMILIA.FAMCODI=EQ_EQUIPO.FAMCODI ";
            //ls_comando += "AND EQ_EQUIPO.EQUIESTADO <>'B' ";
            ls_comando += "AND  EQ_EQUIPO.EMPRCODI in ( "+ ps_empresas + " ) ";
            ls_comando += "ORDER BY  EQ_FAMILIA.FAMABREV, EQ_EQUIPO.EQUIABREV ";
            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "EQ_EQUIPO", ls_comando);

            foreach (DataRow dr in l_ds.Tables["EQ_EQUIPO"].Rows)
            {
                if (pi_equicodi == Convert.ToInt32(dr["EQUICODI"]))
                {
                    ls_estado = Convert.ToString(dr["EQUIESTADO"]).Trim();
                    if (ls_estado.Equals("B"))
                    {
                        ps_mensaje = "El código del equipo cargado se encuentra de baja en el sistema de información del COES";
                        break;
                    }

                    lb_existe = true;
                    break;
                }
            }

            return lb_existe;
        }
    }
}
