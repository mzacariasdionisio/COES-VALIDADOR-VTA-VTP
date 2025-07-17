using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using fwapp;
using System.Configuration;
using OfficeOpenXml;
using ExcelLibrary.SpreadSheet;
using System.IO;

namespace WScoes
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DemandaService" in code, svc and config file together.
    public class DemandaService : IDemandaService
    {
        public int ii_Version = 21001;
        public n_fw_data[] iL_data = new n_fw_data[2];
        public n_fw_app n_app = new n_fw_app();

        public DemandaService()
        {
            iL_data[0] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SICOES"].ToString());
            iL_data[1] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SCADA"].ToString());
        }

        public CEquipoDemanda GetEquipo(int ai_codigoEquipo)
        {
            CEquipoDemanda equipo = null;
            bool lb_existe = false;
            int li_valor = 0;
            double ld_valor = 0;
            string ls_sql = String.Empty;
            ls_sql = "SELECT EQUICODI, EQUINOMB, EQUIABREV, FAMCODI, EQUITENSION, AREACODI, EMPRCODI FROM EQ_EQUIPO ";
            ls_sql += "WHERE EQUICODI = " + ai_codigoEquipo; //equicodi
            DataSet l_ds = new DataSet();
            iL_data[0].Fill(l_ds, "EQ_EQUIPO", ls_sql);

            foreach (DataRow dr in l_ds.Tables["EQ_EQUIPO"].Rows)
            {
                if (!lb_existe)
                {
                    equipo = new CEquipoDemanda()
                    {
                        ii_equicodi = Int32.TryParse(dr["EQUICODI"].ToString(), out li_valor) ? li_valor : 0,
                        is_equinomb = Convert.ToString(dr["EQUINOMB"].ToString()),
                        is_equiabrev = Convert.ToString(dr["EQUIABREV"].ToString()),
                        ii_famcodi = Int32.TryParse(dr["FAMCODI"].ToString(), out li_valor) ? li_valor : 0,
                        id_equitension = double.TryParse(dr["EQUITENSION"].ToString(), out ld_valor) ? ld_valor : 0,
                        ii_areacodi = Int32.TryParse(dr["AREACODI"].ToString(), out li_valor) ? li_valor : 0,
                        ii_emprcodi = Int32.TryParse(dr["EMPRCODI"].ToString(), out li_valor) ? li_valor : 0
                    };

                    lb_existe = true;
                }
            }

            return equipo;

        }

        public DataTable nf_get_empresa_detalles(int pi_emprcodi)
        {
            DataSet li_ds = new DataSet("dsempresas");
            string ls_comando = String.Empty;
            ls_comando = " SELECT SI_EMPRESA.EMPRCODI, SI_EMPRESA.EMPRNOMB, SI_EMPRESA.TIPOEMPRCODI, SI_EMPRESA.EMPRABREV, SI_TIPOEMPRESA.TIPOEMPRABREV FROM SI_EMPRESA";
            ls_comando += " INNER JOIN SI_TIPOEMPRESA ON SI_EMPRESA.TIPOEMPRCODI = SI_TIPOEMPRESA.TIPOEMPRCODI ";
            ls_comando += " WHERE SI_EMPRESA.EMPRCODI = " + pi_emprcodi.ToString() + " ";
            ls_comando += " AND SI_EMPRESA.TIPOEMPRCODI IN (1,2,3,4)";

            iL_data[0].Fill(li_ds, "SI_EMPRESA", ls_comando);

            //retornando los empresa seleccionada;
            return li_ds.Tables["SI_EMPRESA"];
        }

        public DataTable nf_get_PuntoMedicion(int ai_codigoTipoEmpresa)
        {
            DataSet li_ds = new DataSet("ptosMedicionDemanda");
            string ls_comando = String.Empty;

            ls_comando = " select m.ptomedicodi, em.emprcodi, em.emprabrev, em.emprrazsocial, em.emprruc, a.areacodi, a.areanomb, eq.equicodi, eq.equitension, m.ptomedielenomb from ME_PTOMEDICION m ";
            ls_comando += " inner join SI_EMPRESA em on m.emprcodi = em.emprcodi ";
            ls_comando += " inner join EQ_EQUIPO eq on m.equicodi = eq.equicodi ";
            ls_comando += " inner join EQ_AREA a on eq.areacodi = a.areacodi ";
            ls_comando += " where m.origlectcodi = 6 ";
            ls_comando += " and em.tipoemprcodi = " + ai_codigoTipoEmpresa.ToString();
            ls_comando += " and em.inddemanda = 'S' ";
            ls_comando += " and eq.equiestado not in ('B', 'X')";
            ls_comando += " order by em.emprrazsocial, a.areanomb, m.ptomedielenomb ";

            iL_data[0].Fill(li_ds, "ME_PTOMEDICION", ls_comando);

            //retornando los ptos de medicion;
            return nf_get_tabla(li_ds.Tables["ME_PTOMEDICION"]);
            
        }

        public DataTable nf_get_PuntoMedicion(int ai_codigoPtoMedicion, int ai_codigoEmpresa, int ai_codigoTipoEmpresa)
        {
            DataSet li_ds = new DataSet("ptosMedicionxEmpresa");
            string ls_comando = String.Empty;
            ls_comando += "select me.ptomedibarranomb,";
            //ls_comando += "eq.equinomb, me.ptomedicodi, me.ptomedibarranomb, me.ptomedielenomb, me.ptomedidesc, me.equicodi, me.grupocodi, me.emprcodi, ";
            //ls_comando += "(select max(replace(nvl(trim(valor),'0'),',','.')) ";
            //ls_comando += "from eq_propequi x ";
            //ls_comando += "where propcodi = 1481 ";
            //ls_comando += "and x.equicodi=eq.equicodi ";
            //ls_comando += "and fechapropequi=(select max(fechapropequi) from eq_propequi c where propcodi = 1481  and eq.equicodi=c.equicodi) ) as subestacion, ";
            ls_comando += "ar.areanomb as subestacion,";
            if (ai_codigoTipoEmpresa == 4)
            {
                ls_comando += "(select max(replace(nvl(trim(valor),'0'),',','.')) ";
                ls_comando += "from eq_propequi x ";
                ls_comando += "where propcodi = 1064 ";
                ls_comando += "and x.equicodi=eq.equicodi ";
                ls_comando += "and fechapropequi=(select max(fechapropequi) from eq_propequi c where propcodi = 1064  and eq.equicodi=c.equicodi) ) as \"AREA OPERATIVA\", ";
                ls_comando += "(select max(replace(nvl(trim(valor),'0'),',','.')) ";
                ls_comando += "from eq_propequi x ";
                ls_comando += "where propcodi = 1062 ";
                ls_comando += "and x.equicodi=eq.equicodi ";
                ls_comando += "and fechapropequi=(select max(fechapropequi) from eq_propequi c where propcodi = 1062  and eq.equicodi=c.equicodi) ) as suministrador ";
            }
            else if (ai_codigoTipoEmpresa == 2)
            {
                ls_comando += "nvl(eq.equitension, 0) as tension ";
            }

            ls_comando += "from ME_PTOMEDICION me, EQ_EQUIPO eq, EQ_AREA ar ";
            ls_comando += "where me.ptomedicodi = " + ai_codigoPtoMedicion.ToString();
            ls_comando += "and me.equicodi = eq.equicodi ";
            ls_comando += "and eq.areacodi = ar.areacodi ";

            iL_data[0].Fill(li_ds, "ME_PTOMEDICION", ls_comando);

            //retornando los empresa seleccionada;
            return this.nf_get_trans_tabla(this.nf_get_tabla(li_ds.Tables["ME_PTOMEDICION"]), true);
        }

        public DataTable nf_get_puntos_medicion_x_empresa(int ai_emprcodi, int ai_tipo_empresa_codigo)
        { 
           DataSet li_ds = new DataSet("ptosMedicionXEmpresa");
           try
           {
               string ls_comando = "select m.ptomedicodi,m.ptomedidesc as descrip,m.equicodi,m.grupocodi,m.ptomedicodi ||'|'|| d.canalcodi as codigos ";
               ls_comando += " from ME_PTOMEDICION m, DEM_MEDXSCADA d ";
               if (ai_tipo_empresa_codigo == 4)
                   ls_comando += ", EQ_EQUIPO e";
               ls_comando += " where m.emprcodi = " + ai_emprcodi.ToString();
               if (ai_tipo_empresa_codigo == 4)
                   ls_comando += " and m.equicodi = e.equicodi and e.famcodi = 32 ";
               ls_comando += " and origlectcodi = 6 and m.ptomedicodi = d.ptomedicodi(+) order by 3 asc";

               iL_data[0].Fill(li_ds, "ME_PTOMEDICIONXEMPRESA", ls_comando);

               //retornando los pto medicion x empresa;
               return nf_get_tabla(li_ds.Tables["ME_PTOMEDICIONXEMPRESA"]);
           }
           catch (Exception ex)
           {
               return (DataTable)null;
           }
        }

        public DataTable nf_get_medicion48(DateTime adt_fecha, int ai_lectcodi, int ai_tipoinf, int ai_ptomedicodi)
        {
            DataSet li_ds = new DataSet("ptosMedicionDiaria");
            try
            {
                string ls_comando = "select ";
                ls_comando += "TO_CHAR(t.medifecha, 'dd/MM/yyyy'),round(nvl(t.h1, 0), 2),round(nvl(t.h2, 0), 2),round(nvl(t.h3, 0), 2),round(nvl(t.h4, 0), 2),round(nvl(t.h5, 0), 2),round(nvl(t.h6, 0), 2),round(nvl(t.h7, 0), 2),round(nvl(t.h8, 0), 2),round(nvl(t.h9, 0), 2),round(nvl(t.h10, 0), 2),";
                ls_comando += "round(nvl(t.h11, 0), 2),round(nvl(t.h12, 0), 2),round(nvl(t.h13, 0), 2),round(nvl(t.h14, 0), 2),round(nvl(t.h15, 0), 2),round(nvl(t.h16, 0), 2),round(nvl(t.h17, 0), 2),round(nvl(t.h18, 0), 2),round(nvl(t.h19, 0), 2),round(nvl(t.h20, 0), 2),";
                ls_comando += "round(nvl(t.h21, 0), 2),round(nvl(t.h22, 0), 2),round(nvl(t.h23, 0), 2),round(nvl(t.h24, 0), 2),round(nvl(t.h25, 0), 2),round(nvl(t.h26, 0), 2),round(nvl(t.h27, 0), 2),round(nvl(t.h28, 0), 2),round(nvl(t.h29, 0), 2),round(nvl(t.h30, 0), 2),";
                ls_comando += "round(nvl(t.h31, 0), 2),round(nvl(t.h32, 0), 2),round(nvl(t.h33, 0), 2),round(nvl(t.h34, 0), 2),round(nvl(t.h35, 0), 2),round(nvl(t.h36, 0), 2),round(nvl(t.h37, 0), 2),round(nvl(t.h38, 0), 2),round(nvl(t.h39, 0), 2),round(nvl(t.h40, 0), 2),";
                ls_comando += "round(nvl(t.h41, 0), 2),round(nvl(t.h42, 0), 2),round(nvl(t.h43, 0), 2),round(nvl(t.h44, 0), 2),round(nvl(t.h45, 0), 2),round(nvl(t.h46, 0), 2),round(nvl(t.h47, 0), 2),round(nvl(t.h48, 0), 2) ";
                ls_comando += "from ME_MEDICION48 t,ME_PTOMEDICION u ";
                ls_comando += "where t.ptomedicodi = u.ptomedicodi ";
                //ls_comando += " and u.equicodi = eq.equicodi";
                ls_comando += " and t.medifecha = " + EPDate.SQLDateOracleString(adt_fecha);
                ls_comando += " and t.lectcodi = " + ai_lectcodi.ToString();
                ls_comando += " and t.tipoinfocodi = " + ai_tipoinf.ToString();
                ls_comando += " and u.ptomedicodi = " + ai_ptomedicodi.ToString();
                ls_comando += " order by medifecha asc";

                iL_data[0].Fill(li_ds, "ME_PTOMEDICIONDIARIA", ls_comando);

                //retornando los detalles pto medicion;
                return nf_get_tabla(li_ds.Tables["ME_PTOMEDICIONDIARIA"]);
            }
            catch (Exception ex)
            {
                return (DataTable)null;
            }
        }

        public DataTable nf_get_medicion48(DateTime adt_fechaIni, DateTime adt_fechaFin)
        {
            DataSet li_ds = new DataSet("ptosMedicionDiaria");
            try
            {
                string ls_comando = "select ";
                ls_comando += "t.ptomedicodi, to_char(t.medifecha, 'DD') as DIA, t.medifecha, t.lectcodi, t.meditotal ";
                ls_comando += "from ME_MEDICION48 t ";
                ls_comando += "where ";
                ls_comando += "t.medifecha >= " + EPDate.SQLDateOracleString(adt_fechaIni) + " and t.medifecha <= " + EPDate.SQLDateOracleString(adt_fechaFin);
                ls_comando += " and t.lectcodi in (103,110,102)";// (45, 46, 47) ";
                ls_comando += " and t.tipoinfocodi = 1 ";//20 ";
                ls_comando += " order by t.ptomedicodi, t.medifecha, t.lectcodi";

                iL_data[0].Fill(li_ds, "ME_PTOMEDICIONDIARIA", ls_comando);

                //retornando los detalles pto medicion;
                return nf_get_tabla(li_ds.Tables["ME_PTOMEDICIONDIARIA"]);
            }
            catch (Exception ex)
            {
                return (DataTable)null;
            }
        }

        protected DataTable nf_get_tabla(DataTable adt_data)
        {
            if (adt_data == null)
                return (DataTable)null;
            if (adt_data.Rows.Count > 0)
                return adt_data;
            else
                return (DataTable)null;
        }

        public DataTable nf_DemandaBarraDiario48FHora(DateTime adt_fecha_rep, int ai_lectcodi_real, int ai_lectcodi_prog, int ai_tipoinf_mw, int ai_ptomedicodi)
        {
            DataTable adt_input_real = this.nf_get_medicion48(adt_fecha_rep.AddDays(-1.0), ai_lectcodi_real, ai_tipoinf_mw, ai_ptomedicodi);
            DataTable adt_input_prog = this.nf_get_medicion48(adt_fecha_rep.AddDays(1.0), ai_lectcodi_prog, ai_tipoinf_mw, ai_ptomedicodi);

            if (adt_input_prog != null && adt_input_prog.Rows.Count > 0)
            {    
                if (adt_input_real != null && adt_input_real.Rows.Count > 0)
                {
                    DataTable dt1 = this.nf_get_trans_fhora(adt_input_real, 30, 0, ai_lectcodi_real);
                    //DataTable dt2 = this.nf_get_trans_fhora(adt_input_prog, 30, 0, ai_lectcodi_prog);

                    //dt1.Merge(dt2);
                    //DataTable output = new DataTable();
                    //output.Columns.Add(new DataColumn(ai_lectcodi_real.ToString() + adt_fecha_rep.AddDays(-1.0).ToString("dd/MM/yyyy")));
                    //output.Columns.Add(new DataColumn(ai_lectcodi_real.ToString() + adt_fecha_rep.AddDays(-1.0).ToString("dd/MM/yyyy")));
                    System.Data.DataColumn newColumn = new System.Data.DataColumn(ai_lectcodi_prog.ToString() + "-01-" + adt_fecha_rep.AddDays(1.0).ToString("dd/MM/yyyy"), typeof(System.String));
                    //newColumn.DefaultValue = "Your DropDownList value";
                    dt1.Columns.Add(newColumn);


                    int li_cont = 1;
                    //for (int i = 1; i < adt_input_prog.Columns.Count; i++)
                    //{
                        foreach (DataRow dr in dt1.Rows)
                        {
                            dr[2] = adt_input_prog.Rows[0][li_cont].ToString();
                            li_cont++;

                        }
                    //}

                    

                    return dt1;
                }
                else
                {
                    return this.nf_get_trans_fhora(adt_input_prog, 30, 0, ai_lectcodi_prog);
                }  
            }
            else if (adt_input_real != null && adt_input_real.Rows.Count > 0)
            {
                return this.nf_get_trans_fhora(adt_input_real, 30, 0, ai_lectcodi_real);
            }
            else
            {
                return null;
            }

        }

        public DataTable nf_DemandaBarraDiario48FHora(DateTime adt_fini, DateTime adt_ffin, int ai_lectcodi, int ai_tipoinf, int ai_ptomedicodi)
        {
            int num1 = 0;
            DataTable dataTable = this.nf_DemandaBarraReporteDiario48FHora(adt_fini, adt_ffin, ai_lectcodi, ai_tipoinf, ai_ptomedicodi);
            int num2 = 0;
            if (dataTable != null)
            {
                num2 = dataTable.Columns.Count;
                dataTable.Columns.Add("TOTAL");
            }
            foreach (DataRow dataRow in (InternalDataCollectionBase)dataTable.Rows)
            {
                double num3 = 0.0;
                if (num1 > 0)
                {
                    for (int columnIndex = 1; columnIndex < num2; ++columnIndex)
                    {
                        if (!dataRow.IsNull(columnIndex))
                            num3 += Convert.ToDouble(dataRow[columnIndex]);
                    }
                }
                dataRow["TOTAL"] = num1 != 0 ? (object)num3 : (object)"";
                ++num1;
            }
            return dataTable;
        }

        public DataTable nf_DemandaBarraReporteDiario48FHora(DateTime adt_fini, DateTime adt_ffin, int ai_lectcodi, int ai_tipoinf, int ai_ptomedicodi)
        {
            DataSet li_ds = new DataSet("ptosMedicionDiariaHistorica");
            try
            {
                string ls_comando = "select (CASE t.lectcodi  WHEN 45 THEN 'HISTÓRICO'  WHEN 46 THEN 'PREVISTO'  WHEN 47 THEN 'PREVISTO'  ELSE 'N/A'  END ) || ' ' || ";
                ls_comando += "(CASE t.tipoinfocodi  WHEN 20 THEN '(MW)'  WHEN 48 THEN '(MVAR)' ELSE 'N/A'  END ) || ' - ' || TO_CHAR(t.medifecha, 'dd/MM/yyyy') descripcion,";
                ls_comando += "round(nvl(t.h1, 0), 2),round(nvl(t.h2, 0), 2),round(nvl(t.h3, 0), 2),round(nvl(t.h4, 0), 2),round(nvl(t.h5, 0), 2),round(nvl(t.h6, 0), 2),round(nvl(t.h7, 0), 2),round(nvl(t.h8, 0), 2),round(nvl(t.h9, 0), 2),round(nvl(t.h10, 0), 2),";
                ls_comando += "round(nvl(t.h11, 0), 2),round(nvl(t.h12, 0), 2),round(nvl(t.h13, 0), 2),round(nvl(t.h14, 0), 2),round(nvl(t.h15, 0), 2),round(nvl(t.h16, 0), 2),round(nvl(t.h17, 0), 2),round(nvl(t.h18, 0), 2),round(nvl(t.h19, 0), 2),round(nvl(t.h20, 0), 2),";
                ls_comando += "round(nvl(t.h21, 0), 2),round(nvl(t.h22, 0), 2),round(nvl(t.h23, 0), 2),round(nvl(t.h24, 0), 2),round(nvl(t.h25, 0), 2),round(nvl(t.h26, 0), 2),round(nvl(t.h27, 0), 2),round(nvl(t.h28, 0), 2),round(nvl(t.h29, 0), 2),round(nvl(t.h30, 0), 2),";
                ls_comando += "round(nvl(t.h31, 0), 2),round(nvl(t.h32, 0), 2),round(nvl(t.h33, 0), 2),round(nvl(t.h34, 0), 2),round(nvl(t.h35, 0), 2),round(nvl(t.h36, 0), 2),round(nvl(t.h37, 0), 2),round(nvl(t.h38, 0), 2),round(nvl(t.h39, 0), 2),round(nvl(t.h40, 0), 2),";
                ls_comando += "round(nvl(t.h41, 0), 2),round(nvl(t.h42, 0), 2),round(nvl(t.h43, 0), 2),round(nvl(t.h44, 0), 2),round(nvl(t.h45, 0), 2),round(nvl(t.h46, 0), 2),round(nvl(t.h47, 0), 2),round(nvl(t.h48, 0), 2) ";
                ls_comando += "from ME_MEDICION48 t,ME_PTOMEDICION u ";
                ls_comando += "where t.medifecha between " + EPDate.SQLDateOracleString(adt_fini) + " and " + EPDate.SQLDateOracleString(adt_ffin);
                ls_comando += " and t.lectcodi = " + ai_lectcodi.ToString();
                ls_comando += " and t.tipoinfocodi = " + ai_tipoinf.ToString();
                ls_comando += " and t.ptomedicodi = " + ai_ptomedicodi.ToString();
                ls_comando += " and t.ptomedicodi = u.ptomedicodi ";
                ls_comando += " order by medifecha asc ";

                iL_data[0].Fill(li_ds, "ME_PTOMEDICIONDIARIAHISTORICA", ls_comando);

                return this.nf_get_trans_fhora(this.nf_get_tabla(li_ds.Tables["ME_PTOMEDICIONDIARIAHISTORICA"]), 30, 0);
            }
            catch (Exception ex)
            {
                return (DataTable)null;
            }
        }

        public DataTable nf_DemandaBarraSemanal48FHora(DateTime adt_fecha_rep, int ai_lectcodi, int ai_tipoinf, int ai_ptomedicodi)
        {
            DataTable dataTable = this.nf_get_medicion48(adt_fecha_rep, adt_fecha_rep.AddDays(6.0), ai_lectcodi, ai_tipoinf, ai_ptomedicodi);

            return this.nf_get_trans_fhora(dataTable, 30, 0);
        }

        public DataTable nf_get_medicion48(DateTime adt_fini, DateTime adt_ffin, int ai_lectcodi, int ai_tipoinf, int ai_ptomedicodi)
        {
            DataSet li_ds = new DataSet("ptosMedicionSemanal");
            try
            {
                string ls_comando = "select ";
                ls_comando += "TO_CHAR(t.medifecha, 'dd/MM/yyyy'),round(nvl(t.h1, 0), 2),round(nvl(t.h2, 0), 2),round(nvl(t.h3, 0), 2),round(nvl(t.h4, 0), 2),round(nvl(t.h5, 0), 2),round(nvl(t.h6, 0), 2),round(nvl(t.h7, 0), 2),round(nvl(t.h8, 0), 2),round(nvl(t.h9, 0), 2),round(nvl(t.h10, 0), 2),";
                ls_comando += "round(nvl(t.h11, 0), 2),round(nvl(t.h12, 0), 2),round(nvl(t.h13, 0), 2),round(nvl(t.h14, 0), 2),round(nvl(t.h15, 0), 2),round(nvl(t.h16, 0), 2),round(nvl(t.h17, 0), 2),round(nvl(t.h18, 0), 2),round(nvl(t.h19, 0), 2),round(nvl(t.h20, 0), 2),";
                ls_comando += "round(nvl(t.h21, 0), 2),round(nvl(t.h22, 0), 2),round(nvl(t.h23, 0), 2),round(nvl(t.h24, 0), 2),round(nvl(t.h25, 0), 2),round(nvl(t.h26, 0), 2),round(nvl(t.h27, 0), 2),round(nvl(t.h28, 0), 2),round(nvl(t.h29, 0), 2),round(nvl(t.h30, 0), 2),";
                ls_comando += "round(nvl(t.h31, 0), 2),round(nvl(t.h32, 0), 2),round(nvl(t.h33, 0), 2),round(nvl(t.h34, 0), 2),round(nvl(t.h35, 0), 2),round(nvl(t.h36, 0), 2),round(nvl(t.h37, 0), 2),round(nvl(t.h38, 0), 2),round(nvl(t.h39, 0), 2),round(nvl(t.h40, 0), 2),";
                ls_comando += "round(nvl(t.h41, 0), 2),round(nvl(t.h42, 0), 2),round(nvl(t.h43, 0), 2),round(nvl(t.h44, 0), 2),round(nvl(t.h45, 0), 2),round(nvl(t.h46, 0), 2),round(nvl(t.h47, 0), 2),round(nvl(t.h48, 0), 2) ";
                ls_comando += "from ME_MEDICION48 t,ME_PTOMEDICION u ";
                ls_comando += "where t.ptomedicodi = u.ptomedicodi ";
                ls_comando += " and t.medifecha between " + EPDate.SQLDateOracleString(adt_fini);
                ls_comando += " and " + EPDate.SQLDateOracleString(adt_ffin);
                ls_comando += " and t.lectcodi = " + ai_lectcodi.ToString();
                ls_comando += " and t.tipoinfocodi = " + ai_tipoinf.ToString();
                ls_comando += " and u.ptomedicodi = " + ai_ptomedicodi.ToString();
                ls_comando += " order by medifecha asc";

                iL_data[0].Fill(li_ds, "ME_PTOMEDICIONSEMANAL", ls_comando);

                //retornando los detalles pto medicion;
                return nf_get_tabla(li_ds.Tables["ME_PTOMEDICIONSEMANAL"]);
            }
            catch (Exception ex)
            {
                return (DataTable)null;
            }
        }

        public int nf_set_insert_archivo_envio(int ai_etacodi, string as_eararchnomb, double as_eararchtammb, string as_eararchver, string as_eararchruta, int ai_usercode, string as_earip, string as_lastuser, DateTime adt_earfecha, int ai_estenvcodi, string as_earcopiado, int ai_emprcodi)
        {
            try
            {
                int li_resultado, li_earcodi;
                li_resultado = li_earcodi = 0;
                try
                {
                    li_earcodi = iL_data[0].nf_get_next_key("EXT_ARCHIVO");
                }
                catch { }


                if (as_lastuser.Length > 50)
                    as_lastuser = as_lastuser.Substring(0, 50);

                string s_comando = "INSERT INTO EXT_ARCHIVO (earcodi,etacodi,eararchnomb,eararchtammb,eararchver,eararchruta,usercode,earip,lastuser,lastdate,earfecha,estenvcodi,earcopiado,emprcodi) VALUES (";
                s_comando += li_earcodi + "," + ai_etacodi + ",'" + as_eararchnomb + "','" + as_eararchtammb + "', '" + as_eararchver + "','" + as_eararchruta + "'," + ai_usercode.ToString() + ",'" + as_earip + "','" + as_lastuser + "',sysdate," + EPDate.SQLDateOracleString(adt_earfecha) + "," + ai_estenvcodi.ToString() + ",'" + as_earcopiado + "'," + ai_emprcodi.ToString() + ")";

                li_resultado = iL_data[0].nf_ExecuteNonQuery(s_comando);

                if (li_resultado >= 1)
                    return li_earcodi;

                return -1;
            }
            catch
            {
                return -1;
            }
        }

        public int nf_set_insert_envio(int ai_earcodi, int ai_etacodi, int ai_tipoenvio, int ai_plazo, DateTime adt_fechaenvio, DateTime adt_fechareporte, string as_lastuser)
        {
            try
            {
                int li_resultado = 0;
                try
                {
                    li_resultado = iL_data[0].nf_ExecuteScalar_GetInteger("SELECT MAX(envcodi) FROM EXT_ENVIO") + 1;
                }
                catch { }


                if (as_lastuser.Length > 50)
                    as_lastuser = as_lastuser.Substring(0, 50);

                string s_comando = "INSERT INTO EXT_ENVIO (envcodi, earcodi, etacodi, tipoenvio, estadtipoenvio, fecha_env, fecha_rep, lastuser) VALUES (";
                s_comando += li_resultado + "," + ai_earcodi + "," + ai_etacodi + "," + ai_tipoenvio + ", '" + ai_plazo + "'," + EPDate.SQLDateOracleString(adt_fechaenvio) + "," + EPDate.SQLDateOracleString(adt_fechareporte) + ", '" + as_lastuser + "')";

                li_resultado = iL_data[0].nf_ExecuteNonQuery(s_comando);


                return li_resultado;
            }
            catch
            {
                return -1;
            }
        }

        //public int nf_set_insert_informacion(int ai_earcodi, string as_observacion)
        //{
        //    try
        //    {
        //        int li_resultado = 0;
        //        try
        //        {
        //            li_resultado = iL_data[0].nf_ExecuteScalar_GetInteger("SELECT MAX(earcodi) FROM DEM_INFORMACION") + 1;
        //        }
        //        catch { }


        //        if (as_lastuser.Length > 50)
        //            as_lastuser = as_lastuser.Substring(0, 50);

        //        string s_comando = "INSERT INTO EXT_ENVIO (envcodi, earcodi, etacodi, tipoenvio, plazo, fecha_env, fecha_rep, lastuser) VALUES (";
        //        s_comando += li_resultado + "," + ai_earcodi + "," + ai_etacodi + "," + ai_tipoenvio + ", '" + as_plazo + "'," + EPDate.SQLDateOracleString(adt_fechaenvio) + "," + EPDate.SQLDateOracleString(adt_fechareporte) + ", '" + as_lastuser + "')";

        //        li_resultado = iL_data[0].nf_ExecuteNonQuery(s_comando);


        //        return li_resultado;
        //    }
        //    catch
        //    {
        //        return -1;
        //    }
        //}

        public int nf_upd_env_copiado(int ai_earcodi, bool ab_copiado, string as_lastuser)
        {
            try
            {
                int li_resultado = 0;
                string ls_copiado = !ab_copiado ? "N" : "S";
                if (as_lastuser.Length > 50)
                    as_lastuser = as_lastuser.Substring(0, 50);

                string commandText = "update EXT_ARCHIVO t set t.earcopiado = '" + ls_copiado + "', t.lastuser='" + as_lastuser + "',t.lastdate = sysdate where t.earcodi = " + ai_earcodi.ToString();
                li_resultado = iL_data[0].nf_ExecuteNonQuery(commandText);

                return li_resultado;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int nf_upd_env_estado(int ai_earcodi, int ai_estado, string as_lastuser)
        {
            try
            {
                int li_resultado = 0;
                if (as_lastuser.Length > 50)
                    as_lastuser = as_lastuser.Substring(0, 50);

                string commandText = "update EXT_ARCHIVO t set t.estenvcodi = " + ai_estado.ToString() + ", t.lastuser='" + as_lastuser + "',t.lastdate = sysdate where t.earcodi = " + ai_earcodi.ToString();
                li_resultado = iL_data[0].nf_ExecuteNonQuery(commandText);

                return li_resultado;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int nf_upd_env_estado_envio(int ai_earcodi, int ai_estadtipoenvio, DateTime adt_fechaEnviado, string as_lastUser, string as_descripcionEnvio)
        {
            try
            {
                int li_resultado = 0;
                if (as_lastUser.Length > 50)
                    as_lastUser = as_lastUser.Substring(0, 50);

                if (as_descripcionEnvio.Length > 150)
                    as_descripcionEnvio = as_descripcionEnvio.Substring(0, 150);

                string commandText = "update EXT_ENVIO t set t.estadtipoenvio = " + ai_estadtipoenvio.ToString() + ", t.lastuser='" + as_lastUser + "',t.envdescripcion = '" + as_descripcionEnvio + "',t.fecha_env = " + EPDate.SQLDateOracleString(adt_fechaEnviado) + " where t.earcodi = " + ai_earcodi.ToString();
                li_resultado = iL_data[0].nf_ExecuteNonQuery(commandText);

                return li_resultado;
            }
            catch (Exception ex)
            {
                return -1;
            } 
        }

        protected DataTable nf_get_trans_tabla(DataTable adt_input_table, bool ab_header)
        {
            try
            {
                string tableName = adt_input_table.TableName;
                if (tableName == "")
                    tableName = "TRANSPUESTA" + DateTime.Now.ToString("ddmmyyy");
                DataTable dataTable = new DataTable(tableName);
                int num1 = 0;
                if (ab_header)
                {
                    dataTable.Columns.Add(((object)adt_input_table.Columns[0].ColumnName).ToString());
                    num1 = 1;
                }
                int num2 = 1;
                foreach (DataRow dataRow in (InternalDataCollectionBase)adt_input_table.Rows)
                {
                    string str = dataRow[0].ToString();
                    dataTable.Columns.Add(string.Format("{0:00}", (object)num2) + "-" + str);
                    ++num2;
                }
                for (int index1 = 1; index1 <= adt_input_table.Columns.Count - 1; ++index1)
                {
                    DataRow row = dataTable.NewRow();
                    if (ab_header)
                        row[0] = (object)((object)adt_input_table.Columns[index1].ColumnName).ToString();
                    for (int index2 = 0; index2 <= adt_input_table.Rows.Count - 1; ++index2)
                    {
                        string str = adt_input_table.Rows[index2][index1].ToString();
                        row[index2 + num1] = (object)str;
                    }
                    dataTable.Rows.Add(row);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                return (DataTable)null;
            }
        }

        protected DataTable nf_get_trans_fhora(DataTable adt_input_table, int ai_periodo, int ai_delta)
        {
            try
            {
                string tableName = adt_input_table.TableName;
                //DateTime dateTime = DateTime.Today.AddMinutes((double)ai_periodo);
                DateTime dateTime = DateTime.Today.AddMinutes((double)0);
                if (tableName == "")
                    tableName = "TRANSPUESTA" + DateTime.Now.ToString("ddmmyyy");
                DataTable dataTable = new DataTable(tableName);
                dataTable.Columns.Add("HORA");
                int num = 1;
                foreach (DataRow dataRow in (InternalDataCollectionBase)adt_input_table.Rows)
                {
                    string str = dataRow[0].ToString();
                    dataTable.Columns.Add(string.Format("{0:00}", (object)num) + "-" + str);
                    ++num;
                }
                for (int index1 = 1; index1 <= adt_input_table.Columns.Count - 1; ++index1)
                {
                    DataRow row = dataTable.NewRow();
                    if (index1 >= 1 + ai_delta)
                    {
                        row[0] = (object)dateTime.ToString("HH:mm");
                        dateTime = dateTime.AddMinutes((double)ai_periodo);
                    }
                    else
                        row[0] = (object)((object)adt_input_table.Columns[index1].ColumnName).ToString();
                    for (int index2 = 0; index2 <= adt_input_table.Rows.Count - 1; ++index2)
                    {
                        string str = adt_input_table.Rows[index2][index1].ToString();
                        row[index2 + 1] = (object)str;
                    }
                    dataTable.Rows.Add(row);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                return (DataTable)null;
            }
        }

        protected DataTable nf_get_trans_fhora(DataTable adt_input_table, int ai_periodo, int ai_delta, int pi_codigoLectura)
        {
            try
            {
                string tableName = adt_input_table.TableName;
                //DateTime dateTime = DateTime.Today.AddMinutes((double)ai_periodo);
                DateTime dateTime = DateTime.Today.AddMinutes((double)0);
                if (tableName == "")
                    tableName = "TRANSPUESTA" + DateTime.Now.ToString("ddmmyyy");
                DataTable dataTable = new DataTable(tableName);
                dataTable.Columns.Add("HORA");
                int num = 1;
                foreach (DataRow dataRow in (InternalDataCollectionBase)adt_input_table.Rows)
                {
                    string str = dataRow[0].ToString();
                    dataTable.Columns.Add(pi_codigoLectura + "-" + string.Format("{0:00}", (object)num) + "-" + str);
                    ++num;
                }
                for (int index1 = 1; index1 <= adt_input_table.Columns.Count - 1; ++index1)
                {
                    DataRow row = dataTable.NewRow();
                    if (index1 >= 1 + ai_delta)
                    {
                        row[0] = (object)dateTime.ToString("HH:mm");
                        dateTime = dateTime.AddMinutes((double)ai_periodo);
                    }
                    else
                        row[0] = (object)((object)adt_input_table.Columns[index1].ColumnName).ToString();
                    for (int index2 = 0; index2 <= adt_input_table.Rows.Count - 1; ++index2)
                    {
                        string str = adt_input_table.Rows[index2][index1].ToString();
                        row[index2 + 1] = (object)str;
                    }
                    dataTable.Rows.Add(row);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                return (DataTable)null;
            }
        }

        public DataSet nf_get_FileDemandaExcel(string ps_path, string ps_extension)
        {
            DataSet ds = new DataSet("info");
            if (ps_path.ToUpper().EndsWith(".XLS"))
            {
                Workbook workbook = Workbook.Load(ps_path);
                foreach (Worksheet ws in workbook.Worksheets)
                {
                    DataTable dt = PopulateDataTable(ws);
                    ds.Tables.Add(dt);
                }
            }
            else if (ps_path.ToUpper().EndsWith(".XLSX"))
            {
                if (!File.Exists(ps_path))
                    throw new FileNotFoundException("Archivo No encontrado");

                var newFile = new FileInfo(ps_path);

                using (var package = new ExcelPackage(newFile))
                {
                    ExcelWorkbook workbook = package.Workbook;
                    foreach (ExcelWorksheet ws in workbook.Worksheets)
                    {
                        DataTable dt = PopulateDataTable(ws);
                        ds.Tables.Add(dt);
                    }
                }
            }

            return ds;
        }

        private static DataTable PopulateDataTable(Worksheet ws)
        {
            CellCollection Cells = ws.Cells;

            // Creates DataTable from a Worksheet
            // All values will be treated as Strings
            DataTable dt = new DataTable(ws.Name);

            // Extract columns
            for (int i = 0; i <= Cells.LastColIndex; i++)
                dt.Columns.Add(GetExcelColumnName(i + 1).ToString(), typeof(String));

            // Extract data
            for (int currentRowIndex = 0; currentRowIndex <= Cells.LastRowIndex; currentRowIndex++)
            {
                DataRow dr = dt.NewRow();
                for (int currentColumnIndex = 0; currentColumnIndex <= Cells.LastColIndex; currentColumnIndex++)
                    dr[currentColumnIndex] = Cells[currentRowIndex, currentColumnIndex].StringValue;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private DataTable PopulateDataTable(ExcelWorksheet ws)
        {
            DataTable dt = new DataTable(ws.Name);
            int i = 0;
            bool hasHeader = false;
            foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
            {
                dt.Columns.Add(GetExcelColumnName(i + 1).ToString(), typeof(String));
                i++;
            }

            var startRow = hasHeader ? 2 : 1;
            for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                var row = dt.NewRow();
                foreach (var cell in wsRow)
                {
                    if(ws.Dimension.End.Column != cell.Start.Column)
                        row[cell.Start.Column - 1] = cell.Text;
                    
                }
                dt.Rows.Add(row);
            }

            return dt;

        }

        public static string GetExcelColumnName(int pi_columnNumber)
        {
            int li_dividend = pi_columnNumber;
            string ls_columnName = String.Empty;
            int li_modulo;

            while (li_dividend > 0)
            {
                li_modulo = (li_dividend - 1) % 26;
                ls_columnName = Convert.ToChar(65 + li_modulo).ToString() + ls_columnName;
                li_dividend = (int)((li_dividend - li_modulo) / 26);
            }

            return ls_columnName;
        }



        public DataTable nf_get_sql(string ls_comando)
        {
            DataSet li_ds = new DataSet("me_medicion48");
            try
            {
                iL_data[0].Fill(li_ds, "ME_MEDICION48", ls_comando);

                //retornando los detalles pto medicion;
                return nf_get_tabla(li_ds.Tables["ME_MEDICION48"]);
            }
            catch (Exception ex)
            {
                return (DataTable)null;
            }
        }
    }
}
