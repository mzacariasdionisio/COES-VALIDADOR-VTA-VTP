using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using fwapp;

namespace WScoes
{
    
    public class CMantto
    {
        public int mancodi = -1; //Codigo unico de EVE_MANTTO
        public int manttocodi = -1; //COdigo unico en MAN_MANTTO
        //public int regcodi = -1; //codigo del registro de MAN_MANTTO

        public int i_emprreporta;//codigo de la empresa que reporta el mantto 

        public string s_emprnomb = "";
        public int i_equipo_emprcodi;//codigo de la empresa del equipo 
        public int i_areacodi;
        public string s_areanomb = "";
        public string s_equiabrev = "";
        public string s_famabrev = "";
        public int i_famcodi;
        public int i_tareacodi;
        public int equicodi;

        public int evenclasecodi;
        public int tipoevencodi;
        //public int compcode;
        public int subcausacodi;
        public int evenpadre;
        public string eveninterrup; //size=1 
        public string evenindispo; //size=1 (F/E) 
        public double evenmwindisp = 0;
        public string eventipoprog; //size=1
        public string evenestado; //size=1      
        public string evenobsrv; //size=255
        public string evendescrip; //size=300
        public DateTime _evenini; // fecha de inicio evento
        public DateTime _evenfin; //fecha fin evento

        public DateTime _evenpreini = new DateTime(2000, 1, 1, 0, 0, 0); // fecha previa de inicio evento (prog)
        public DateTime _evenprefin = new DateTime(2000, 1, 1, 0, 0, 0); //fecha previa fin evento (prog)
        //public bool ib_modificado = false;

        //public bool ib_simulado = false;//este registro no existe en base de datos solo existe para simular

        public string lastuser;
        public DateTime lastdate;


        public string NombreEquipo
        {
            get
            {
                return s_emprnomb.Trim() + " - " + s_areanomb.Trim() + " - " + s_famabrev.Trim() + " " + s_equiabrev.Trim();
            }
        }

        public string EMPRESA
        {
            get
            {
                return s_emprnomb.Trim();
            }
        }
        public string UBICACION
        {
            get
            {
                return s_areanomb.Trim();
            }
        }

        public string FAM
        {
            get
            {
                return s_famabrev.Trim();
            }
        }
        public string EQUIPO
        {
            get
            {
                return s_equiabrev.Trim();
            }
        }

        public string CAUSA
        {
            get
            {
                return "";
            }
        }
        public string INICIO
        {
            get
            {
                return _evenini.ToString("dd-MM-yyyy HH:mm");
            }
        }
        public string FINAL
        {
            get
            {
                return _evenfin.ToString("dd-MM-yyyy HH:mm");
            }
        }

        public string INDISP
        {
            get
            {
                return "";
            }
        }
        public string INTER
        {
            get
            {
                return "";
            }
        }
        public string DESCRIPCION
        {
            get
            {
                return this.evendescrip;
            }
        }
        public double MWINDISP
        {
            get
            {
                return this.evenmwindisp;
            }
        }
        
        public DateTime evenini
        {
            get
            {
                return this._evenini;
            }
            set
            {
                _evenini = value;
                //nf_analisismodificacionfecha();
            }
        }

        public DateTime evenfin
        {
            get
            {
                return this._evenfin;
            }
            set
            {
                _evenfin = value;
                //nf_analisismodificacionfecha();
            }
        }

        public DateTime evenpreini
        {
            get
            {
                return this._evenpreini;
            }
            set
            {
                _evenpreini = value;
                //nf_analisismodificacionfecha();
            }
        }

        public DateTime evenprefin
        {
            get
            {
                return this._evenprefin;
            }
            set
            {
                _evenprefin = value;
                //nf_analisismodificacionfecha();
            }
        }

        public static string ClaseEventoDescripcion(int ai_evenclasecodi)
        {
            return "";
        }

        public string nf_detalle()
        {
            return s_emprnomb + " " + s_areanomb + " " + s_equiabrev + " - " + evenindispo + "/S\n" + evenini.ToString("dd/MM/yyyy HH:mm") + " - " + evenfin.ToString("dd/MM/yyyy HH:mm")
                + "\n" + evendescrip;
        }

        //private void nf_analisismodificacionfecha()
        //{
        //    if ((_evenini == _evenpreini && _evenfin == _evenprefin) || _evenpreini == new DateTime(2000, 1, 1) || _evenprefin == new DateTime(2000, 1, 1))
        //        ib_modificado = false;
        //    else
        //        ib_modificado = true;
        //}

       
        public bool LoadDR(DataRow a_dr)
        {
            //i_drmantto = a_dr;
            if (a_dr.Table.Columns.Contains("MANCODI")) mancodi = Convert.ToInt32(a_dr["MANCODI"]);
            if (a_dr.Table.Columns.Contains("MANTTOCODI")) manttocodi = Convert.ToInt32(a_dr["MANTTOCODI"]);

            if(a_dr["EMPRNOMB"]!= DBNull.Value) s_emprnomb = a_dr["EMPRNOMB"].ToString().Trim();
            if (a_dr["EMPRCODI"] != DBNull.Value) i_equipo_emprcodi = Convert.ToInt32(a_dr["EMPRCODI"]);
            if (a_dr["AREACODI"] != DBNull.Value) i_areacodi = Convert.ToInt32(a_dr["AREACODI"]);
            if (a_dr["AREANOMB"] != DBNull.Value) s_areanomb = a_dr["AREANOMB"].ToString().Trim();
            if (a_dr["EQUIABREV"] != DBNull.Value) s_equiabrev = a_dr["EQUIABREV"].ToString().Trim();
            if (a_dr["FAMABREV"] != DBNull.Value) s_famabrev = a_dr["FAMABREV"].ToString().Trim();
            if (a_dr["TAREACODI"] != DBNull.Value) i_tareacodi = Convert.ToInt32(a_dr["TAREACODI"]);
            //famcodi = Convert.ToInt32(a_dr["FAMCODI"]);
            equicodi = Convert.ToInt32(a_dr["EQUICODI"]);

            evenclasecodi = Convert.ToInt32(a_dr["EVENCLASECODI"]);
            tipoevencodi = Convert.ToInt32(a_dr["TIPOEVENCODI"]);
            //compcode = Convert.ToInt32(a_dr["COMPCODE"]);
            subcausacodi = Convert.ToInt32(a_dr["SUBCAUSACODI"]);
            evenpadre = Convert.ToInt32(a_dr["EVENPADRE"]);
            eveninterrup = a_dr["EVENINTERRUP"].ToString();
            evenindispo = a_dr["EVENINDISPO"].ToString();

            if (a_dr["EVENMWINDISP"] != System.DBNull.Value && Convert.ToDouble(a_dr["EVENMWINDISP"]) > 0)
            {
                evenmwindisp = Convert.ToDouble(a_dr["EVENMWINDISP"]);
            }
            else
            {
                if (a_dr["EQUIPOT"] != System.DBNull.Value)
                    evenmwindisp = Convert.ToDouble(a_dr["EQUIPOT"]);
            }

            eventipoprog = a_dr["EVENTIPOPROG"].ToString();
            evenestado = a_dr["EVENESTADO"].ToString();
            evenobsrv = a_dr["EVENOBSRV"].ToString().Trim();
            evendescrip = a_dr["EVENDESCRIP"].ToString().Trim();
            evenini = (DateTime)a_dr["EVENINI"];
            evenfin = (DateTime)a_dr["EVENFIN"];
            if (a_dr["EVENPREINI"] != System.DBNull.Value || a_dr["EVENPREFIN"] != System.DBNull.Value)
            {
                if (a_dr["EVENPREINI"] != System.DBNull.Value)
                    evenpreini = (DateTime)a_dr["EVENPREINI"];
                if (a_dr["EVENPREFIN"] != System.DBNull.Value)
                    evenprefin = (DateTime)a_dr["EVENPREFIN"];
            }
            lastuser = a_dr["LASTUSER"].ToString().Trim();
            lastdate = (DateTime)a_dr["LASTDATE"];
            return true;
        }

        public static int DB_Update(n_fw_data an_data, int ai_manttocodi, string sqlSET)
        {
            //return an_data.nf_ExecuteNonQuery("UPDATE MAN_MANTTO SET " + sqlSET + " WHERE MANCODI = " + ai_manttocodi.ToString());
            return an_data.nf_ExecuteNonQuery("UPDATE MAN_MANTTO SET " + sqlSET + " WHERE MANCODI = " + ai_manttocodi.ToString());
        }

        public static int nf_get_next_key(n_fw_data an_data, string as_table)
        {
            int ll_counter = 0;
            an_data.nf_ExecuteNonQuery("UPDATE FW_COUNTER SET MAXCOUNT = MAXCOUNT + 1 WHERE TRIM(TABLENAME) = '" + as_table.Trim() + "'");
            ll_counter = an_data.nf_ExecuteScalar_GetInteger("SELECT MAXCOUNT FROM FW_COUNTER WHERE TRIM(TABLENAME) = '" + as_table.Trim() + "'");
            return ll_counter;
        }

        public int DB_Delete(n_fw_data an_data)
        {
            string s_comando = "DELETE FROM MAN_MANTTO "
                           + " WHERE MANCODI = " + this.mancodi.ToString();

            an_data.nf_ExecuteNonQuery(s_comando);

            return 1;
        }

        public int DB_Update(n_fw_data an_data)
        {
            //string s_comando = "UPDATE EVE_MANTTO "
            string s_comando = "UPDATE EVE_MANTTO "
                          + " SET "
                          + " EQUICODI =" + this.equicodi.ToString()
                          + ", EVENCLASECODI =" + this.evenclasecodi
                          + ", TIPOEVENCODI =" + this.tipoevencodi
                          + ", SUBCAUSACODI =" + this.subcausacodi
                          + ", EVENPADRE =" + this.evenpadre
                          + ", EVENINTERRUP ='" + this.eveninterrup + "'"
                          + ", EVENINDISPO ='" + this.evenindispo + "'"
                          + ", EVENMWINDISP =" + this.evenmwindisp
                          + ", EVENTIPOPROG = '" + this.eventipoprog + "'"
                          + ", EVENESTADO = '" + this.evenestado + "'"
                          + ", EVENOBSRV = '" + this.evenobsrv + "'"
                          + ", EVENDESCRIP = '" + this.evendescrip + "'"
                          + ", EVENINI =" + EPDate.SQLDateTimeOracleString(this.evenini)
                          + ", EVENFIN =" + EPDate.SQLDateTimeOracleString(this.evenfin);
            //if (!ib_modificado)
            //{
            //    s_comando += ", EVENPREINI =" + EPDate.SQLDateTimeOracleString(this.evenpreini)
            //               + ", EVENPREFIN =" + EPDate.SQLDateTimeOracleString(this.evenprefin);
            //}
            s_comando += ", LASTUSER ='" + this.lastuser + "', LASTDATE = sysdate "
                          + " WHERE MANCODI = " + this.mancodi.ToString();

            an_data.nf_ExecuteNonQuery(s_comando);
            return 1;
        }

        public static int GetMaxManttoCodi(n_fw_data an_data)
        {
            return nf_get_next_key(an_data, "EVE_MANTTO");
        }

        public CMantto Clone()
        {
            CMantto temp = new CMantto();
            //temp.ii_grow = ii_grow;
            temp.mancodi = 0;

            temp.equicodi = equicodi;
            //temp.emprnomb = emprnomb;
            //temp.areanomb = areanomb;
            //temp.equiabrev = equiabrev;

            //temp.emprcodi = equicodi;
            //temp.areacodi = areacodi;

            temp.evenclasecodi = evenclasecodi;
            temp.tipoevencodi = tipoevencodi;
            //temp.compcode = compcode;
            temp.subcausacodi = subcausacodi;
            temp.evenpadre = evenpadre;
            temp.eveninterrup = eveninterrup;
            temp.evenindispo = evenindispo;
            temp.evenmwindisp = evenmwindisp;
            temp.eventipoprog = eventipoprog;
            temp.evenestado = evenestado;
            temp.evenobsrv = evenobsrv;
            temp.evendescrip = evendescrip;
            temp.evenini = evenini;
            temp.evenfin = evenfin;
            //temp.ib_modificado = ib_modificado; esto se calcula
            //temp.evenpreini = evenpreini;
            //temp.evenprefin = evenprefin;
            temp.lastuser = lastuser;
            temp.lastdate = lastdate;
            return temp;
        }

        public int EVE_InsertNew(n_fw_data an_data, string as_lastuser)
        {
            this.mancodi = nf_get_next_key(an_data, "EVE_MANTTO"); //GetMaxManttoCodi(an_data) + 1;
            //string s_comando = "INSERT INTO EVE_MANTTO "
            string s_comando = "INSERT INTO EVE_MANTTO"
                          + "(MANTTOCODI, EQUICODI, EVENCLASECODI, TIPOEVENCODI, COMPCODE, SUBCAUSACODI, EVENPADRE, EVENINTERRUP, EVENINDISPO, EVENMWINDISP,"
                          + " EVENTIPOPROG, EVENESTADO, EVENOBSRV, EVENDESCRIP, EVENINI, EVENFIN,";
            // if (evenpreini != null && evenprefin != null) s_comando += " EVENPREINI, EVENPREFIN,";

            s_comando += " LASTUSER, LASTDATE)"
                       + " VALUES ("
                       + this.mancodi.ToString() + ","
                       + this.equicodi.ToString() + ","
                       + this.evenclasecodi.ToString() + ","
                       + this.tipoevencodi.ToString() + ","
                       //+ this.compcode.ToString() + ","
                       + this.subcausacodi.ToString() + ","
                       + this.evenpadre.ToString() + ","
                       + "'" + this.eveninterrup + "',"
                       + "'" + this.evenindispo + "',"
                       + this.evenmwindisp.ToString() + ","
                       + "'" + this.eventipoprog + "',"
                       + "'" + this.evenestado + "',"
                       + "'" + this.evenobsrv + "',"
                       + "'" + this.evendescrip + "',"
                       + EPDate.SQLDateTimeOracleString(evenini) + ","
                       + EPDate.SQLDateTimeOracleString(evenfin) + ",";
            //if (evenpreini != null && evenprefin != null) s_comando += EPDate.SQLDateTimeOracleString(evenpreini) + "," + EPDate.SQLDateTimeOracleString(evenprefin) + ","
            s_comando += "'" + as_lastuser + "',"
                       + "sysdate"
                       + ")";
            an_data.nf_ExecuteNonQuery(s_comando);
            


            return 1;
        }

        public int MAN_InsertNew(n_fw_data an_data, int ai_registro, string as_lastuser)
        {
            this.mancodi = nf_get_next_key(an_data, "MAN_MANTTO") + 1;
                //GetMaxManttoCodi(an_data) + 1;
            //string s_comando = "INSERT INTO MAN_MANTTO "
            string s_comando = "INSERT INTO MAN_MANTTO "
                          + "(MANCODI, EQUICODI, TIPOEVENCODI, REGCODI, MANTTOCODI, EVENINTERRUP, EVENINDISPO, EVENMWINDISP,"
                          + " EVENTIPOPROG, EVENOBSRV, EVENDESCRIP, EVENINI, EVENFIN, ";
            // if (evenpreini != null && evenprefin != null) s_comando += " EVENPREINI, EVENPREFIN,";
            if (this.evenobsrv.Trim() == this.evendescrip.Trim())
            {
                this.evenobsrv = "";
            }

            s_comando += " LASTUSER, LASTDATE)"
                       + " VALUES ("
                       + this.mancodi.ToString() + ","
                       + this.equicodi.ToString() + ","
                       //+ this.evenclasecodi.ToString() + "," eliminado 
                       + this.tipoevencodi.ToString() + ","
                       + ai_registro.ToString() + ","
                       + this.manttocodi + ","
                       //+ this.subcausacodi.ToString() + ","
                      // + this.evenpadre.ToString() + ","
                       + "'" + this.eveninterrup + "',"
                       + "'" + this.evenindispo + "',"
                       + this.evenmwindisp.ToString() + ","
                       + "'" + this.eventipoprog + "',"
                      // + "'" + this.evenestado + "',"
                       + "'" + this.evenobsrv.Replace("'", "") + "',"
                       + "'" + this.evendescrip.Replace("'","") + "',"
                       + EPDate.SQLDateTimeOracleString(evenini) + ","
                       + EPDate.SQLDateTimeOracleString(evenfin) + ",";
            //if (evenpreini != null && evenprefin != null) s_comando += EPDate.SQLDateTimeOracleString(evenpreini) + "," + EPDate.SQLDateTimeOracleString(evenprefin) + ","
            s_comando += "'" + as_lastuser + "',"
                       + "sysdate"
                       + ")";
            an_data.nf_ExecuteNonQuery(s_comando);

            return 1;
        }
    }
}