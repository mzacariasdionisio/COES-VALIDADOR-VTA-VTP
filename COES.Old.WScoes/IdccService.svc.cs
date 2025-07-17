using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using fwapp;
using System.Data;
using System.Configuration;

namespace WScoes
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IdccService" in code, svc and config file together.
    public class ExtService : IExtService
    {
        public int ii_Version = 21001;
        public n_fw_data[] iL_data = new n_fw_data[2];
        public n_fw_app n_app = new n_fw_app();

        public ExtService()
        {
            iL_data[0] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SICOES"].ToString());
            iL_data[1] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SCADA"].ToString());
        }

        //public string f_set_1()
        //{
        //    return "prueba";
        //}

        //public int f_set_insertIdccFile(string ps_tabla, ref string ps_file, string as_fields, string as_values, string as_lastuser)
        public int f_set_insertFile(string ps_tabla, int pi_etacodi, ref string ps_file, string as_fields, string as_values, string as_lastuser)
        {
            int ll_codi;
            //int pi_etacodi = 1;
            int li_resultado;

            try
            {

                ll_codi = iL_data[0].nf_get_next_key(ps_tabla);
                
                if (as_lastuser.Length > 40)
                    as_lastuser = as_lastuser.Substring(0, 40);
                ps_file = ll_codi + "_" + ps_file;

                string s_comando = "INSERT INTO " + ps_tabla + " (EARCODI,ETACODI," + "EARARCHNOMB," + as_fields + ",LASTUSER,LASTDATE) VALUES ("
                    + ll_codi + "," + pi_etacodi + ",'" + ps_file + "'," + as_values + ",'" + as_lastuser + "',sysdate)";
                
                li_resultado = iL_data[0].nf_ExecuteNonQuery(s_comando);

                //s_comando = "UPDATE MAN_MANTTO SET ISFILES ='S' WHERE MANCODI=" + ai_mancodi;
                //iL_data[0].nf_ExecuteNonQuery(s_comando);

                return ll_codi;
            }
            catch
            {
                return -1;
            }
        }

        public int f_set_consulta1(string ps_sql)
        {
            int li_resultado;
            //filtros 
            if ((ps_sql.ToUpper().IndexOf("WHERE") < 0) && (ps_sql.ToUpper().IndexOf("VALUES") < 0))
                return -1;
            
            try
            {                               
                li_resultado = iL_data[0].nf_ExecuteNonQuery(ps_sql);
                return li_resultado;
            }
            catch
            {
                return -1;
            }
        }

        public int f_get_consulta_escalar(string ps_sql)
        {
            int li_resultado;
            //filtros 
            if ((ps_sql.ToUpper().IndexOf("WHERE") < 0) && (ps_sql.ToUpper().IndexOf("VALUES") < 0))
                return -1;

            try
            {
                li_resultado = iL_data[0].nf_ExecuteScalar_GetInteger(ps_sql);
                return li_resultado;
            }
            catch
            {
                return -1;
            }
        }

        public int f_set_conslconfig(string ps_tabla,string ps_sql1, string ps_sql2)
        {
            int ll_codi;            
            int li_resultado;

            try
            {

                ll_codi = iL_data[0].nf_get_next_key(ps_tabla);

                string s_comando = ps_sql1 + ll_codi + ps_sql2;

                li_resultado = this.f_set_consulta1(s_comando);

                return li_resultado;
            }
            catch
            {
                return -1;
            }

        }

        public int f_set_ext_ratio_actualizar(int pi_earcodi, int pi_eaicodi, int pi_total, int pi_enviado)
        {
            //f_set_tra_logpro_actualizar
            int ll_codi;
            double ld_ratio;
            int li_resultado;

            ll_codi = iL_data[0].nf_get_next_key("EXT_RATIO");

            try
            {
                ld_ratio = (100.0 * pi_enviado) / (pi_total * 1.0);
                ld_ratio = Math.Round(ld_ratio, 2);
            }
            catch
            {
                ld_ratio = 0;
            }


            string ls_sql = "insert into EXT_RATIO (ERATCODI,EARCODI,EAICODI,ERATTOTINF,ERATENVINF,ERATRATIO,LASTDATE) values (" + 
                ll_codi + "," + pi_earcodi + "," + pi_eaicodi + "," +pi_total+ "," +pi_enviado+ "," +ld_ratio+ ",sysdate" + ")";

            try
            {
                li_resultado= iL_data[0].nf_ExecuteNonQuery(ls_sql);

                if (li_resultado <= 0)
                    return ll_codi;
                else
                    return -1;

            }
            catch
            {
                return -1;
            }


        }

        public Dictionary<int, string> H_GetListarCargas(int pi_emprcodi,int pi_etacodi, int pi_eaicodi, DateTime adt_inicio, DateTime adt_final)
        {
            Dictionary<int, string> H_ListarCarga = new Dictionary<int, string>();

            string ls_sql = "select a.earcodi CODIGO,to_char(a.lastdate,'dd/mm/yyyy hh24:mi') FECHA_CARGA, to_char(a.earfecha,'dd/mm/yyyy') FECHA_INFORME, e.estenvabrev ESTADO, a.lastuser USUARIO,'~/Consultas/w_me_log.aspx?id='||a.earcodi LOG "
            + "from ext_archivo a, ext_tipo_archivo b, ext_archivo_item c, ext_ratio d, ext_estado_envio e "
            + "where b.etacodi=a.etacodi and b.etacodi=c.etacodi and d.earcodi=a.earcodi and c.eaicodi=d.eaicodi and a.estenvcodi=e.estenvcodi "
            + "and a.usercode in (select b.usercode from ext_archivo a, fw_user b where a.usercode=b.usercode and ','||b.empresas||',' like '%,4,%') "
            + "and b.etacodi=4 "
            + "and c.eaicodi=2 "
            + "and a.lastdate>=to_date('01/09/2012','dd/mm/yyyy') "
            + "and a.lastdate<to_date('30/09/2012','dd/mm/yyyy') "
            + "order by 1 desc ";
             
            /*
            @" SELECT man.REGCODI, man.REGABREV, even.EVENCLASECODI, even.EVENCLASEABREV, even.EVENCLASEDESC, man.FECHAINI, man.FECHAFIN, man.REGNOMB  
                            FROM MAN_REGISTRO man, EVE_EVENCLASE even WHERE  man.EVENCLASECODI = even.EVENCLASECODI  ";
            ls_sql += " AND  FECHAINI >= " + EPDate.SQLDateOracleString(adt_inicio) + " AND FECHAINI < " + EPDate.SQLDateOracleString(adt_final.AddDays(1)) + " ";
            ls_sql += " ORDER BY man.FECHAINI DESC, even.EVENCLASECODI";
            */

            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "ME_LISTARCARGAS", ls_sql);

            foreach (DataRow dr in l_ds.Tables["ME_LISTARCARGAS"].Rows)
            {
                CListarCargas manreg = nf_CargarListarCargas(dr);

                H_ListarCarga.Add(manreg.Codigo, manreg.Fecha_Carga);

            }
            return H_ListarCarga;
        }

        public DataTable nf_GetListarCargas(int pi_emprcodi, int pi_etacodi, int pi_eaicodi, DateTime pdt_inicio, DateTime pdt_final)
        {            
            //string ls_sql = "select a.earcodi CODIGO,to_char(a.lastdate,'dd/mm/yyyy hh24:mi') FECHA_CARGA, to_char(a.earfecha,'dd/mm/yyyy') FECHA_INFORME, e.estenvabrev ESTADO, a.lastuser USUARIO,'~/Consultas/w_me_log.aspx?id='||a.earcodi LOG "
            string ls_sql = "select a.earcodi CODIGO,to_char(a.lastdate,'dd/mm/yyyy hh24:mi') FECHA_CARGA, to_char(a.earfecha,'dd/mm/yyyy') FECHA_INFORME, e.estenvabrev ESTADO, a.lastuser USUARIO "
            + "from ext_archivo a, ext_tipo_archivo b, ext_archivo_item c, ext_ratio d, ext_estado_envio e "
            + "where b.etacodi=a.etacodi and b.etacodi=c.etacodi and d.earcodi=a.earcodi and c.eaicodi=d.eaicodi and a.estenvcodi=e.estenvcodi "
            + "and a.usercode in (select b.usercode from ext_archivo a, fw_user b where a.usercode=b.usercode and ','||b.empresas||',' like '%," + pi_emprcodi + ",%') ";

            if(pi_etacodi!=0)
            {
                ls_sql += "and b.etacodi=" + pi_etacodi + " "
                + "and c.eaicodi=" + pi_eaicodi + " ";
            }

            ls_sql += "and a.lastdate>=to_date('" + pdt_inicio.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') "
            + "and a.lastdate<to_date('" + pdt_final.AddDays(1).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') "
            + "order by 1 desc ";

            /*
            @" SELECT man.REGCODI, man.REGABREV, even.EVENCLASECODI, even.EVENCLASEABREV, even.EVENCLASEDESC, man.FECHAINI, man.FECHAFIN, man.REGNOMB  
                            FROM MAN_REGISTRO man, EVE_EVENCLASE even WHERE  man.EVENCLASECODI = even.EVENCLASECODI  ";
            ls_sql += " AND  FECHAINI >= " + EPDate.SQLDateOracleString(adt_inicio) + " AND FECHAINI < " + EPDate.SQLDateOracleString(adt_final.AddDays(1)) + " ";
            ls_sql += " ORDER BY man.FECHAINI DESC, even.EVENCLASECODI";
            */

            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "ME_LISTARCARGAS", ls_sql);            
            return l_ds.Tables["ME_LISTARCARGAS"];
        }


        public DataTable nf_GetListarLog(int pi_earcodi)
        {
            string ls_sql = "select b.logpsecuen SEC,  to_char(b.logpfechor,'dd/mm/yyyy hh24:mi:ss') FECHA, c.menmen PROCESO,b.logpdetmen DETALLE "
            + "from ext_archivo a, ext_logpro b, ext_menlog c "
            + "where a.earcodi=b.earcodi and b.mencodi=c.mencodi "
            + "and a.earcodi=" + pi_earcodi + " "
            + "order by b.logpsecuen ";
            
            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "ME_LISTARLOG", ls_sql);
            return l_ds.Tables["ME_LISTARLOG"];
        }

        public DataTable nf_GetListarRatio(int pi_emprcodi, int pi_etacodi,int pi_eaicodi,  DateTime pdt_fechaini, DateTime pdt_fechafin)//, ref System.Web.UI.WebControls.ListBox PLBox1)
        {

            if (pdt_fechaini > pdt_fechafin)
                return null;



            

            /*
            string ls_sql = "select b.logpsecuen SEC,  to_char(b.logpfechor,'dd/mm/yyyy hh24:mi:ss') FECHA, c.menmen PROCESO,b.logpdetmen DETALLE "
            + "from ext_archivo a, ext_logpro b, ext_menlog c "
            + "where a.earcodi=b.earcodi and b.mencodi=c.mencodi "
            + "and a.earcodi=" + pi_earcodi + " "
            + "order by b.logpsecuen ";
            */

            string ls_sql = "select a.earcodi CODIGO, b.etacodi,c.eaicodi, a.earfecha, d.ERATTOTINF,d.ERATENVINF,d.ERATRATIO "
            + "from ext_archivo a, ext_tipo_archivo b, ext_archivo_item c, ext_ratio d "
            + "where b.etacodi=a.etacodi and b.etacodi=c.etacodi and d.earcodi=a.earcodi and c.eaicodi=d.eaicodi  "
            + "and a.usercode in (select b.usercode from ext_archivo a, fw_user b where a.usercode=b.usercode and ','||b.empresas||',' like '%," + pi_emprcodi + ",%') "
            + "and b.etacodi="+pi_etacodi+" "
            + "and c.eaicodi="+pi_eaicodi+" "
            + "and a.lastdate>=to_date('"+pdt_fechaini.ToString("dd/MM/yyyy")+"','dd/mm/yyyy') "
            + "and a.lastdate<to_date('" + pdt_fechafin.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') "
            + "order by a.earcodi ";

            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "ME_LISTARRATIO", ls_sql);
            //return l_ds.Tables["ME_LISTARRATIO"];

            DateTime ldt_fecha_old = Convert.ToDateTime(pdt_fechaini.Year + "-01-01");//pdt_fechaini;//
            DateTime ldt_fecha;
            double ld_ratio;
            double ld_ratio_old = 0;
            double ld_cumple = 0;
            double ld_nocumple = 0;
            DataTable ldtt_ratio = new DataTable();
            int li_earcodi;

            ldtt_ratio.Columns.Add(getNewColumn("MES", "System.String"));
            ldtt_ratio.Columns.Add(getNewColumn("%_CUMPLIMIENTO", "System.String"));
            ldtt_ratio.Columns.Add(getNewColumn("CUMPLE", "System.String"));
            ldtt_ratio.Columns.Add(getNewColumn("NO_CUMPLE", "System.String"));
            ldtt_ratio.Columns.Add(getNewColumn("TOTAL", "System.String"));
            TimeSpan ts1;

            int li_mesini;

            li_mesini = pdt_fechaini.Month;

            //crea registros del año
            for (int li_i = 0; li_i <= pdt_fechafin.Month - pdt_fechaini.Month; li_i++)
            {
                f_set_agregaratio(ref ldtt_ratio, pdt_fechaini.Year + "-" + (li_i + 1).ToString("00"), ld_cumple, ld_nocumple);
            }

            

            //procesa registros
            foreach (DataRow dread in l_ds.Tables["ME_LISTARRATIO"].Rows)
            {
                //PLBox1.Items.Add("*** INICIO ***");

                ldt_fecha = Convert.ToDateTime(dread["earfecha"]);
                ld_ratio = Convert.ToDouble(dread["ERATRATIO"]);
                li_earcodi = Convert.ToInt32(dread["CODIGO"]);

                //PLBox1.
                //PLBox1.Items.Add("CODIGO " + li_earcodi.ToString());
                //PLBox1.Items.Add(ldt_fecha_old.ToString("yyyy-MM-dd") + " " + ldt_fecha.ToString("yyyy-MM-dd"));

                //if (ldt_fecha_old.ToString("yyyy-MM") == ldt_fecha.ToString("yyyy-MM"))
                //{
                ts1 = ldt_fecha - ldt_fecha_old;

                if (ldt_fecha_old.ToString("yyyy-MM-dd") != ldt_fecha.ToString("yyyy-MM-dd"))
                {

                    //PLBox1.Items.Add("ldt_fecha_old != ldt_fecha");
                    //ts1 = ldt_fecha - ldt_fecha_old;

                    if (ts1.TotalDays > 1)
                    {

                        //PLBox1.Items.Add("ts1.TotalDays > 1 ");
                        //completar dias y ratio
                        
                        //evaluar dia anterior
                        //---2012-09-20
                        //PLBox1.Items.Add("(2) ld_ratio_old " + ld_ratio_old + " <<<");
                        f_set_ratiofecha(ref ldtt_ratio, ld_ratio_old, ldt_fecha_old, li_mesini);
                        ldt_fecha_old = ldt_fecha_old.AddDays(1);
                        //---

                        f_set_ratio(ref ldt_fecha_old, ldt_fecha, ref ldtt_ratio, li_mesini);

                        //agregando ratio del dia
                        /*
                        if (ld_ratio == 100)
                        {
                            //ld_cumple++;
                            ldtt_ratio.Rows[ldt_fecha.Month - li_mesini][2] = Convert.ToInt32(ldtt_ratio.Rows[ldt_fecha.Month - li_mesini][2].ToString()) + 1;
                        }
                        else
                        {
                            //ld_nocumple++;
                            ldtt_ratio.Rows[ldt_fecha.Month - li_mesini][3] = Convert.ToInt32(ldtt_ratio.Rows[ldt_fecha.Month - li_mesini][3].ToString()) + 1;
                        }
                        */
                    }
                    else
                    {
                        //PLBox1.Items.Add("ts1.TotalDays > 1 ELSE");
                        //PLBox1.Items.Add("ld_ratio_old " + ld_ratio_old + " <<<");
                        //agregando ratio del dia
                        f_set_ratiofecha(ref ldtt_ratio, ld_ratio_old, ldt_fecha_old, li_mesini);

                        /*
                        if (ld_ratio_old == 100)
                        {
                            //ld_cumple++;
                            ldtt_ratio.Rows[ldt_fecha.Month - li_mesini][2] = Convert.ToInt32(ldtt_ratio.Rows[ldt_fecha.Month - li_mesini][2].ToString()) + 1;
                        }
                        else
                        {
                            //ld_nocumple++;
                            ldtt_ratio.Rows[ldt_fecha.Month - li_mesini][3] = Convert.ToInt32(ldtt_ratio.Rows[ldt_fecha.Month - li_mesini][3].ToString()) + 1;
                        }
                        */
                    }
                    //

                    //}

                }
                else
                {
                    //ts1 = ldt_fecha - ldt_fecha_old;

                    if (ts1.TotalDays > 1)
                    {
                        //PLBox1.Items.Add("ts1.TotalDays > 1 (2)");
                        //completar dias y ratio 
                        //f_set_ratio(ref ldt_fecha_old, ldt_fecha, li_mesini);
                        f_set_ratio(ref ldt_fecha_old, ldt_fecha, ref ldtt_ratio, li_mesini);
                    }
                    else
                    {
                        //otro mes. procesar mes anterior
                        //f_set_agregaratio(ref ldtt_ratio, ldt_fecha_old.ToString("yyyy-MM"), ld_cumple, ld_nocumple, li_mesini);
                        //ld_cumple = 0;
                        //ld_nocumple = 0;
                    }


                }
                //*/

                ldt_fecha_old = ldt_fecha;
                ld_ratio_old = ld_ratio;

                /*
                if (Convert.ToDateTime(dread["roltfecha"].ToString()).Day == pi_dia)
                {
                    return dread[ps_campo].ToString();
                }
                */

            }


            //validar ultimo envio
            //PLBox1.Items.Add("final <<<");
            f_set_ratiofecha(ref ldtt_ratio, ld_ratio_old, ldt_fecha_old, li_mesini);

            //agregar fechas hasta terminar el mes o sysdate-1            
            ldt_fecha_old = ldt_fecha_old.AddDays(1);
            ldt_fecha = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));

            //PLBox1.Items.Add("Dia final de evaluacion : " + ldt_fecha.AddDays(-1).ToString("dd/MM/yyyy"));
                        
            f_set_ratio(ref ldt_fecha_old, ldt_fecha, ref ldtt_ratio, li_mesini);
            //f_set_ratio(ref ldt_fecha_old, ldt_fecha_old, ref ldtt_ratio, li_mesini);

            //resumen final
            f_set_ratioresumen(ref ldtt_ratio);

            return ldtt_ratio;
            
            
        }

        private void f_set_ratioresumen(ref DataTable pdtt_ratio)
        {
            double ld_cumple, ld_nocumple, ld_total, ld_porc;

            for (int li_i = 0; li_i < pdtt_ratio.Rows.Count; li_i++)
            {
                try
                {
                    ld_cumple = Convert.ToInt32(pdtt_ratio.Rows[li_i][2].ToString());
                }
                catch
                {
                    ld_cumple = 0;
                }

                try
                {
                    ld_nocumple = Convert.ToInt32(pdtt_ratio.Rows[li_i][3].ToString());
                }
                catch
                {
                    ld_nocumple = 0;
                }

                ld_total = ld_cumple+ld_nocumple;
                if (ld_total != 0)
                {
                    ld_porc = Math.Round(100 * ld_cumple / (ld_total * 1.0), 2);
                }
                else
                {
                    ld_porc=0;
                }

                try
                {
                    pdtt_ratio.Rows[li_i][1] = ld_porc;
                    pdtt_ratio.Rows[li_i][4] = ld_total;
                }
                catch
                {
                }

            }


            /*
            if (pd_ratio == 100)
            {
                //ld_cumple++;
                pdtt_ratio.Rows[pdt_fecha.Month - pi_mesini][2] = Convert.ToInt32(pdtt_ratio.Rows[pdt_fecha.Month - pi_mesini][2].ToString()) + 1;
            }
            else
            {
                //ld_nocumple++;
                pdtt_ratio.Rows[pdt_fecha.Month - pi_mesini][3] = Convert.ToInt32(pdtt_ratio.Rows[pdt_fecha.Month - pi_mesini][3].ToString()) + 1;
            }
            */

        }

        private void f_set_ratiofecha(ref DataTable pdtt_ratio, double pd_ratio, DateTime pdt_fecha, int pi_mesini)
        {
            if (pd_ratio == 100)
            {
                //ld_cumple++;
                pdtt_ratio.Rows[pdt_fecha.Month - pi_mesini][2] = Convert.ToInt32(pdtt_ratio.Rows[pdt_fecha.Month - pi_mesini][2].ToString()) + 1;
            }
            else
            {
                //ld_nocumple++;
                pdtt_ratio.Rows[pdt_fecha.Month - pi_mesini][3] = Convert.ToInt32(pdtt_ratio.Rows[pdt_fecha.Month - pi_mesini][3].ToString()) + 1;
            }

        }

        public void f_set_ratio(ref DateTime pdt_fecha_old, DateTime pdt_fecha, ref DataTable pdtt_ratio, int pi_mesini)//, ref System.Web.UI.WebControls.ListBox PLBox1)
        {

            //DateTime ldt_fecha_old = pdt_fecha_old;
            DateTime ldt_fecha = pdt_fecha;

            TimeSpan ts1 = pdt_fecha - pdt_fecha_old;

            if (ts1.TotalDays > 400)
                return;


            while (pdt_fecha_old < pdt_fecha)//.AddDays(-1))
            {
                //PLBox1.Items.Add("INCUMPLE :"+ pdt_fecha_old+">>>");
                //no cumple++
                //pdtt_ratio.Rows[pdt_fecha_old.Month-pi_mesini][3]=Convert.ToInt32(pdtt_ratio.Rows[pdt_fecha_old.Month-pi_mesini][3].ToString())+1;
                pdtt_ratio.Rows[pdt_fecha_old.Month - pi_mesini][3] = Convert.ToInt32(pdtt_ratio.Rows[pdt_fecha_old.Month - pi_mesini][3].ToString()) + 1;
                
                pdt_fecha_old = pdt_fecha_old.AddDays(1);
            }

            
        }



 

        public void f_set_agregaratio(ref DataTable pdtt_1, string ps_mes, double ld_cumple, double ld_nocumple)
        {
            double ld_ratio;
            double ld_total;

            ld_total = ld_cumple + ld_nocumple;
            if (ld_total != 0)
            {
                ld_ratio = Math.Round(100 * ld_cumple / (ld_total * 1.0), 2);                
            }
            else
            {
                ld_ratio = 0;
            }


            DataRow ARow = pdtt_1.NewRow();
            ARow[0] = ps_mes;
            ARow[1] = ld_ratio;
            ARow[2] = ld_cumple;
            ARow[3] = ld_nocumple;
            ARow[4] = ld_cumple+ld_nocumple;

            pdtt_1.Rows.Add(ARow);

        }
        private DataColumn getNewColumn(string ColName, string ColType)
        {
            DataColumn dc = new DataColumn();
            dc.ColumnName = ColName;
            dc.DataType = System.Type.GetType(ColType);
            return dc;
        }

        public CListarCargas nf_CargarListarCargas(DataRow dr)
        {
            //string ls_rangofecha = "";
            CListarCargas registro = new CListarCargas();
            registro.Codigo = Convert.ToInt32(dr["CODIGO"]);
            registro.Fecha_Carga = Convert.ToString(dr["FECHA_CARGA"]);
            registro.Fecha_Informe = Convert.ToString(dr["FECHA_INFORME"]);
            registro.Estado = Convert.ToString(dr["ESTADO"]);
            registro.Usuario = Convert.ToString(dr["USUARIO"]);
            registro.Log = Convert.ToString(dr["LOG"]);


            /*
            if (registro.FechaInicial == registro.FechaFinal)
                ls_rangofecha = registro.FechaInicial.ToString("dd/MM/yyyy");
            else
                ls_rangofecha = registro.FechaInicial.ToString("dd/MM/yyyy") + " - " + registro.FechaFinal.ToString("dd/MM/yyyy");

            registro.RegistroNombre = registro.EvenClaseDesc + " " + ls_rangofecha; //"MANTENIMIENTO " +
            */

            return registro;
        }


        [DataContract]
        public class CListarCargas
        {
            private int ii_codigo = -1;
            [DataMember]
            public int Codigo
            {
                get { return ii_codigo; }
                set { ii_codigo = value; }
            }

            private string is_fecha_carga;
            [DataMember]
            public string Fecha_Carga
            {
                get { return is_fecha_carga; }
                set { is_fecha_carga = value; }
            }

            private string is_fecha_informe = "";
            [DataMember]
            public string Fecha_Informe
            {
                get { return is_fecha_informe; }
                set { is_fecha_informe = value; }
            }

            private string is_estado = "";
            [DataMember]
            public string Estado
            {
                get { return is_estado; }
                set { is_estado = value; }
            }

            private string is_usuario = "";
            [DataMember]
            public string Usuario
            {
                get { return is_usuario; }
                set { is_usuario = value; }
            }

            private string is_log = "";
            [DataMember]
            public string Log
            {
                get { return is_log; }
                set { is_log = value; }
            }

        }



    }
}
