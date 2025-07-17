using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PortalInformacionRepository: RepositoryBase, IPortalInformacionRepository
    {
        DataTable dtableHOP, dtableEJEC, dtableHIDRO, dtsbleSolar, dtsbleEolica;
        double[,] dgv_produc;

        public PortalInformacionRepository(string strConn)
            : base(strConn)
        {
        }

        public DemandadiaDTO ObtenerResumenMaximaDemanda()
        {

            return null;
        }

        public List<MeMedicion48DTO> ProduccionxTipoCombustible(DateTime fechaInicial, DateTime fechaFinal)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            double[,] dgv_producDia;

            fechaFinal = fechaInicial.AddDays(1);

            for (DateTime dti = fechaInicial; dti <= fechaFinal; dti = dti.AddDays(1))
            {
                dgv_producDia = new double[10, 48];
                dgv_producDia = CombustiblePorDia(dti);

                for (int i = 1; i < 10; i++)
                {
                    var entity = new MeMedicion48DTO();
                    entity.Medifecha = dti;

                    for (int k = 1; k <= 48; k++)
                    {
                        entity.GetType().GetProperty("H" + k).SetValue(entity, Convert.ToDecimal(dgv_producDia[i, (k - 1)]));

                        switch (i)
                        {
                            case 1:
                                entity.Fenergnomb = "HÍDRICO";
                                break;
                            case 2:
                                entity.Fenergnomb = "GAS";
                                break;
                            case 3:
                                entity.Fenergnomb = "CARBÓN";
                                break;
                            case 4:
                                entity.Fenergnomb = "RESIDUAL";
                                break;
                            case 5:
                                entity.Fenergnomb = "DIESEL";
                                break;
                            case 6:
                                entity.Fenergnomb = "SOLAR";
                                break;
                            case 7:
                                entity.Fenergnomb = "BIOGÁS";
                                break;
                            case 8:
                                entity.Fenergnomb = "BAGAZO";
                                break;
                            case 9:
                                entity.Fenergnomb = "EÓLICA";
                                break;
                        }
                        lista.Add(entity);
                    }
                }
            }
            
            List<MeMedicion48DTO> list = new List<MeMedicion48DTO>();

            list.Add(lista.FirstOrDefault(t => t.Fenergnomb == "DIESEL"));
            list.Add(lista.FirstOrDefault(t => t.Fenergnomb == "RESIDUAL"));
            list.Add(lista.FirstOrDefault(t => t.Fenergnomb == "CARBÓN"));
            list.Add(lista.FirstOrDefault(t => t.Fenergnomb == "GAS"));
            list.Add(lista.FirstOrDefault(t => t.Fenergnomb == "HÍDRICO"));
            list.Add(lista.FirstOrDefault(t => t.Fenergnomb == "BIOGÁS"));
            list.Add(lista.FirstOrDefault(t => t.Fenergnomb == "BAGAZO"));
            list.Add(lista.FirstOrDefault(t => t.Fenergnomb == "SOLAR"));
            list.Add(lista.FirstOrDefault(t => t.Fenergnomb == "EÓLICA"));           

            return list;
        }

        #region Métodos privados

        private double[,] CombustiblePorDia(DateTime adt_RepFecha)
        {
            dgv_produc = new double[10, 48]; 

            for (int li_i = 1; li_i <= 9; li_i++)
            {
                for (int li_j = 0; li_j < 48; li_j++)
                {
                    dgv_produc[li_i, li_j] = 0;
                }
            }
            
            string ls_sql = "Select a.EQUICODI,b.grupoCODI,b.grupopadre grupo,c.ptomedicodi, HOPHORINI, HOPHORFIN, grupocomb, substr(fe.fenergabrev,1,1) as comb, B.GRUPONOMB , e.emprcodi, e.emprnomb ";
            ls_sql += "FROM EVE_HORAOPERACION a, pr_grupo b, me_ptomedicion c, eq_equipo d, si_empresa e,si_fuenteenergia fe,si_tipogeneracion tg ";
            ls_sql += "WHERE a.grupocodi=b.grupocodi ";
            ls_sql += "and  c.codref=b.grupopadre ";
            ls_sql += "and  a.equicodi=d.equicodi ";
            ls_sql += "and  d.emprcodi=e.emprcodi ";
            ls_sql += "and c.origlectcodi = 2 ";
            ls_sql += "and HOPHORINI>= to_date('" + adt_RepFecha.ToString("yyyy-MM-dd") + " 00:00:00','yyyy-mm-dd hh24:mi:ss')  ";            
            ls_sql += "and HOPHORINI< to_date('" + adt_RepFecha.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00','yyyy-mm-dd hh24:mi:ss') ";
            ls_sql += "and a.equicodi in (select equicodi from eq_equipo where famcodi in (3,5)) ";
            ls_sql += "AND E.EMPRCOES = 'S' ";            
            ls_sql += " and b.fenergcodi = fe.fenergcodi ";
            ls_sql += " and fe.tgenercodi = tg.tgenercodi ";
            ls_sql += "order by b.grupopadre,HOPHORINI ";

            DbCommand command = dbProvider.GetSqlStringCommand(ls_sql);
            IDataReader dr = dbProvider.ExecuteReader(command);
            dtableHOP = new DataTable();
            dtableHOP.Load(dr);

            ls_sql = "select a.* ";
            ls_sql += "from me_medicion48 a, me_ptomedicion b ";
            ls_sql += "where a.ptomedicodi=b.ptomedicodi ";
            ls_sql += "and A.tipoinfocodi=1 ";
            ls_sql += "and lectcodi=6 ";
            ls_sql += "and medifecha = to_date('" + adt_RepFecha.ToString("yyyy-MM-dd") + "','yyyy-mm-dd') ";

            DbCommand command2 = dbProvider.GetSqlStringCommand(ls_sql);
            IDataReader dr2 = dbProvider.ExecuteReader(command2);
            dtableEJEC = new DataTable();
            dtableEJEC.Load(dr2);

            int li_h1, li_h2;
            DateTime ldt_horini, ldt_horfin;
            string ls_grupo;
            string ls_comb;
            string ls_grupo_codi;
            int li_col = 0;
            string ls_emprcodi, ls_emprnomb;
            double ld_sumacomb;
            string ls_grupo_old = "-1";
            int li_h2_old = -1;


            foreach (DataRow dread in dtableHOP.Rows)
            {

                ldt_horini = Convert.ToDateTime(dread["HOPHORINI"]);
                ldt_horfin = Convert.ToDateTime(dread["HOPHORFIN"]);
                ls_grupo = dread["ptomedicodi"].ToString();
                ls_comb = dread["comb"].ToString();
                ls_grupo_codi = dread["grupoCODI"].ToString();
                string tmp_comb = dread["grupocomb"].ToString();
                ls_emprcodi = dread["emprcodi"].ToString();
                ls_emprnomb = dread["emprnomb"].ToString();
                
                switch (ls_comb)
                {
                    case "G":
                        li_col = 2;
                        break;
                    case "C":
                        li_col = 3;
                        break;
                    case "R":
                        li_col = 4;
                        break;
                    case "D":
                        li_col = 5;
                        break;
                    case "S":
                        li_col = 6;
                        break;
                    case "B":
                        if ("BIO" == tmp_comb.Trim())
                            li_col = 7;
                        else 
                            li_col = 8;                      
                        break;
                }

                if (li_col < 0)
                    continue;

                li_h1 = 2 * ldt_horini.Hour + ldt_horini.Minute / 30;
                li_h1 += 1;
                if (ldt_horini.Day != ldt_horfin.Day)
                    li_h2 = 48;
                else
                    li_h2 = 2 * ldt_horfin.Hour + ldt_horfin.Minute / 30;

                if (li_h2 < li_h1)
                    li_h2 = li_h1;

                ld_sumacomb = 0;

                if ((ls_grupo_codi != "288") && (ls_grupo_codi != "291") && (ls_grupo_codi != "286") && (ls_grupo_codi != "289") && (ls_grupo_codi != "287") && (ls_grupo_codi != "290") &&
                    (ls_grupo_codi != "327") && (ls_grupo_codi != "328") && (ls_grupo_codi != "329") && (ls_grupo_codi != "330") && (ls_grupo_codi != "331") && (ls_grupo_codi != "332") && (ls_grupo_codi != "333") &&
                    (ls_grupo_codi != "320") && (ls_grupo_codi != "321") && (ls_grupo_codi != "322") && (ls_grupo_codi != "323") && (ls_grupo_codi != "324") && (ls_grupo_codi != "325") && (ls_grupo_codi != "326"))
                {
                    if ((ls_grupo_old == ls_grupo_codi) && (li_h2_old == li_h1))
                    {
                        li_h1++;
                    }

                    if (li_h1 > li_h2)
                        continue;

                    ld_sumacomb = f_get_combustible_ejecutado(ls_grupo, ldt_horini, li_h1, li_h2, li_col);

                }
                else
                {                    
                    if ((ls_grupo_old == "288") && (li_h2_old == li_h1))
                    {
                        li_h1++;
                    }

                    if (li_h1 > li_h2)
                        continue;
                   
                    switch (ls_grupo_codi)
                    {
                        #region Ventanilla
                        case "288":
                            ld_sumacomb = f_get_combustible_ejecutado("113", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb += f_get_combustible_ejecutado("114", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb += f_get_combustible_ejecutado("193", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "291":
                            ld_sumacomb = f_get_combustible_ejecutado("113", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb += f_get_combustible_ejecutado("114", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb += f_get_combustible_ejecutado("193", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "286":
                            ld_sumacomb = f_get_combustible_ejecutado("113", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb += f_get_combustible_ejecutado("193", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "289":
                            ld_sumacomb = f_get_combustible_ejecutado("113", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb += f_get_combustible_ejecutado("193", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "287":
                            ld_sumacomb = f_get_combustible_ejecutado("114", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb += f_get_combustible_ejecutado("193", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "290":
                            ld_sumacomb = f_get_combustible_ejecutado("114", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb += f_get_combustible_ejecutado("193", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        #endregion

                        #region //CCO Chilca

                        case "327":
                            ld_sumacomb = f_get_combustible_ejecutado("194", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("236", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "328":
                            ld_sumacomb = f_get_combustible_ejecutado("196", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("236", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "329":
                            ld_sumacomb = f_get_combustible_ejecutado("207", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("236", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "330":
                            ld_sumacomb = f_get_combustible_ejecutado("194", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("196", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("236", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "331":
                            ld_sumacomb = f_get_combustible_ejecutado("196", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("207", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("236", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "332":
                            ld_sumacomb = f_get_combustible_ejecutado("194", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("207", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("236", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "333":
                            ld_sumacomb = f_get_combustible_ejecutado("194", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("196", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("207", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("236", ldt_horini, li_h1, li_h2, li_col);
                            break;

                        #endregion

                        #region //Kallpa

                        case "320":
                            ld_sumacomb = f_get_combustible_ejecutado("197", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("230", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "321":
                            ld_sumacomb = f_get_combustible_ejecutado("203", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("230", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "322":
                            ld_sumacomb = f_get_combustible_ejecutado("204", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("230", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "323":
                            ld_sumacomb = f_get_combustible_ejecutado("197", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("203", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("230", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "324":
                            ld_sumacomb = f_get_combustible_ejecutado("203", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("204", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("230", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "325":
                            ld_sumacomb = f_get_combustible_ejecutado("197", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("204", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("230", ldt_horini, li_h1, li_h2, li_col);
                            break;
                        case "326":
                            ld_sumacomb = f_get_combustible_ejecutado("197", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("203", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("204", ldt_horini, li_h1, li_h2, li_col);
                            ld_sumacomb = f_get_combustible_ejecutado("230", ldt_horini, li_h1, li_h2, li_col);
                            break;

                        #endregion


                    }

                    ls_grupo_codi = "288";

                }

                ls_grupo_old = ls_grupo_codi;
                li_h2_old = li_h2;
            }

            ls_sql = "";
            ls_sql += "select sum(h1) as h1,sum(h2) as h2,sum(h3) as h3,sum(h4) as h4,sum(h5) as h5,sum(h6) as h6,sum(h7) as h7,sum(h8) as h8,sum(h9) as h9, ";
            ls_sql += "sum(h10) as h10,sum(h11) as h11,sum(h12) as h12,sum(h13) as h13,sum(h14) as h14,sum(h15) as h15,sum(h16) as h16,sum(h17) as h17,sum(h18) as h18,sum(h19) as h19, ";
            ls_sql += "sum(h20) as h20,sum(h21) as h21,sum(h22) as h22,sum(h23) as h23,sum(h24) as h24,sum(h25) as h25,sum(h26) as h26,sum(h27) as h27,sum(h28) as h28,sum(h29) as h29, ";
            ls_sql += "sum(h30) as h30,sum(h31) as h31,sum(h32) as h32,sum(h33) as h33,sum(h34) as h34,sum(h35) as h35,sum(h36) as h36,sum(h37) as h37,sum(h38) as h38,sum(h39) as h39, ";
            ls_sql += "sum(h40) as h40,sum(h41) as h41,sum(h42) as h42,sum(h43) as h43,sum(h44) as h44,sum(h45) as h45,sum(h46) as h46,sum(h47) as h47,sum(h48) as h48 ";
            ls_sql += "FROM me_medicion48, me_ptomedicion, pr_grupo, si_empresa ,si_fuenteenergia fe,si_tipogeneracion tg ";
            ls_sql += "WHERE ( me_medicion48.ptomedicodi = me_ptomedicion.ptomedicodi ) and  ";
            ls_sql += "( me_ptomedicion.codref = pr_grupo.grupocodi ) and ";
            ls_sql += "( si_empresa.emprcodi = pr_grupo.emprcodi ) and  ";
            ls_sql += "( ( pr_grupo.emprcodi > 0 ) AND  ";
            ls_sql += "( me_medicion48.tipoinfocodi = 1 ) AND  ";
            ls_sql += "( me_ptomedicion.origlectcodi = 2 ) )  ";
            ls_sql += "and me_medicion48.medifecha=to_date('" + adt_RepFecha.ToString("yyyy-MM-dd") + "','yyyy-mm-dd')  ";
            ls_sql += "and me_medicion48.lectcodi=6 ";
            ls_sql += "and pr_grupo.fenergcodi = fe.fenergcodi ";
            ls_sql += "and fe.tgenercodi = tg.tgenercodi ";
            ls_sql += "and tg.tgenercodi = 1 ";            
            ls_sql += "AND si_empresa.EMPRCOES = 'S' "; 

            DbCommand command3 = dbProvider.GetSqlStringCommand(ls_sql);
            IDataReader dr3 = dbProvider.ExecuteReader(command3);
            dtableHIDRO = new DataTable();
            dtableHIDRO.Load(dr3);

            foreach (DataRow dread in dtableHIDRO.Rows)
            {
                try
                {
                    for (int li_i = 1; li_i <= 48; li_i++)
                    {
                        dgv_produc[1, li_i - 1] = Convert.ToDouble(dread["H" + li_i.ToString()]);
                    }
                }
                catch { }
            }

            ls_sql = "";
            ls_sql += "select sum(h1) as h1,sum(h2) as h2,sum(h3) as h3,sum(h4) as h4,sum(h5) as h5,sum(h6) as h6,sum(h7) as h7,sum(h8) as h8,sum(h9) as h9, ";
            ls_sql += "sum(h10) as h10,sum(h11) as h11,sum(h12) as h12,sum(h13) as h13,sum(h14) as h14,sum(h15) as h15,sum(h16) as h16,sum(h17) as h17,sum(h18) as h18,sum(h19) as h19, ";
            ls_sql += "sum(h20) as h20,sum(h21) as h21,sum(h22) as h22,sum(h23) as h23,sum(h24) as h24,sum(h25) as h25,sum(h26) as h26,sum(h27) as h27,sum(h28) as h28,sum(h29) as h29, ";
            ls_sql += "sum(h30) as h30,sum(h31) as h31,sum(h32) as h32,sum(h33) as h33,sum(h34) as h34,sum(h35) as h35,sum(h36) as h36,sum(h37) as h37,sum(h38) as h38,sum(h39) as h39, ";
            ls_sql += "sum(h40) as h40,sum(h41) as h41,sum(h42) as h42,sum(h43) as h43,sum(h44) as h44,sum(h45) as h45,sum(h46) as h46,sum(h47) as h47,sum(h48) as h48 ";
            ls_sql += "FROM me_medicion48, me_ptomedicion, pr_grupo, si_empresa ,si_fuenteenergia fe,si_tipogeneracion tg ";
            ls_sql += "WHERE ( me_medicion48.ptomedicodi = me_ptomedicion.ptomedicodi ) and  ";
            ls_sql += "( me_ptomedicion.codref = pr_grupo.grupocodi ) and ";
            ls_sql += "( si_empresa.emprcodi = pr_grupo.emprcodi ) and  ";
            ls_sql += "( ( pr_grupo.emprcodi > 0 ) AND  ";
            ls_sql += "( me_medicion48.tipoinfocodi = 1 ) AND  ";
            ls_sql += "( me_ptomedicion.origlectcodi = 2 ) )  ";
            ls_sql += "and me_medicion48.medifecha=to_date('" + adt_RepFecha.ToString("yyyy-MM-dd") + "','yyyy-mm-dd')  ";
            ls_sql += "and me_medicion48.lectcodi=6 ";
            ls_sql += "and pr_grupo.fenergcodi = fe.fenergcodi ";
            ls_sql += "and fe.tgenercodi = tg.tgenercodi ";
            ls_sql += "and tg.tgenercodi = 3 "; //Solar 
            ls_sql += "and si_empresa.EMPRCOES = 'S' "; // no se considera NO INTEGRANTES

            DbCommand command4 = dbProvider.GetSqlStringCommand(ls_sql);
            IDataReader dr4 = dbProvider.ExecuteReader(command4);
            dtsbleSolar = new DataTable();
            dtsbleSolar.Load(dr4);

            foreach (DataRow dread in dtsbleSolar.Rows)
            {
                try
                {
                    for (int li_i = 1; li_i <= 48; li_i++)
                    {
                        dgv_produc[6, li_i - 1] = Convert.ToDouble(dread["H" + li_i.ToString()]);
                    }
                }
                catch { }
            }

            ls_sql = "";
            ls_sql += "select sum(h1) as h1,sum(h2) as h2,sum(h3) as h3,sum(h4) as h4,sum(h5) as h5,sum(h6) as h6,sum(h7) as h7,sum(h8) as h8,sum(h9) as h9, ";
            ls_sql += "sum(h10) as h10,sum(h11) as h11,sum(h12) as h12,sum(h13) as h13,sum(h14) as h14,sum(h15) as h15,sum(h16) as h16,sum(h17) as h17,sum(h18) as h18,sum(h19) as h19, ";
            ls_sql += "sum(h20) as h20,sum(h21) as h21,sum(h22) as h22,sum(h23) as h23,sum(h24) as h24,sum(h25) as h25,sum(h26) as h26,sum(h27) as h27,sum(h28) as h28,sum(h29) as h29, ";
            ls_sql += "sum(h30) as h30,sum(h31) as h31,sum(h32) as h32,sum(h33) as h33,sum(h34) as h34,sum(h35) as h35,sum(h36) as h36,sum(h37) as h37,sum(h38) as h38,sum(h39) as h39, ";
            ls_sql += "sum(h40) as h40,sum(h41) as h41,sum(h42) as h42,sum(h43) as h43,sum(h44) as h44,sum(h45) as h45,sum(h46) as h46,sum(h47) as h47,sum(h48) as h48 ";
            ls_sql += "FROM me_medicion48, me_ptomedicion, pr_grupo, si_empresa ,si_fuenteenergia fe,si_tipogeneracion tg ";
            ls_sql += "WHERE ( me_medicion48.ptomedicodi = me_ptomedicion.ptomedicodi ) and  ";
            ls_sql += "( me_ptomedicion.codref = pr_grupo.grupocodi ) and ";
            ls_sql += "( si_empresa.emprcodi = pr_grupo.emprcodi ) and  ";
            ls_sql += "( ( pr_grupo.emprcodi > 0 ) AND  ";
            ls_sql += "( me_medicion48.tipoinfocodi = 1 ) AND  ";
            ls_sql += "( me_ptomedicion.origlectcodi = 2 ) )  ";
            ls_sql += "and me_medicion48.medifecha=to_date('" + adt_RepFecha.ToString("yyyy-MM-dd") + "','yyyy-mm-dd')  ";
            ls_sql += "and me_medicion48.lectcodi=6 ";
            ls_sql += "and pr_grupo.fenergcodi = fe.fenergcodi ";
            ls_sql += "and fe.tgenercodi = tg.tgenercodi ";
            ls_sql += "and tg.tgenercodi = 4 "; //Eolica 
            ls_sql += "and si_empresa.EMPRCOES = 'S' "; // no se considera NO INTEGRANTES

            DbCommand command5 = dbProvider.GetSqlStringCommand(ls_sql);
            IDataReader dr5 = dbProvider.ExecuteReader(command5);
            dtsbleEolica = new DataTable();
            dtsbleEolica.Load(dr5);

            foreach (DataRow dread in dtsbleEolica.Rows)
            {
                try
                {
                    for (int li_i = 1; li_i <= 48; li_i++)
                    {
                        dgv_produc[9, li_i - 1] = Convert.ToDouble(dread["H" + li_i.ToString()]);
                    }
                }
                catch { }
            }

            return dgv_produc;
        }

        private double f_get_combustible_ejecutado(string ps_ptomedicodi, DateTime pdt_horini, int pi_h1, int pi_h2, int pi_col)
        {
            double ld_suma = 0;

            DateTime ldt_fecha = Convert.ToDateTime(pdt_horini.ToString("yyyy-MM-dd"));
            DateTime ldt_medifecha;
            string ls_ptomedicodi;

            foreach (DataRow dread in dtableEJEC.Rows)
            {
                ldt_medifecha = Convert.ToDateTime(dread["medifecha"]);
                ls_ptomedicodi = dread["ptomedicodi"].ToString();


                if ((ls_ptomedicodi == ps_ptomedicodi) && (ldt_medifecha == ldt_fecha))
                {
                    for (int li_i = pi_h1; li_i <= pi_h2; li_i++)
                    {
                        dgv_produc[pi_col, li_i - 1] = Convert.ToDouble(dgv_produc[pi_col, li_i - 1]) + Convert.ToDouble(dread["H" + li_i.ToString()]);
                        ld_suma += Convert.ToDouble(dread["H" + li_i.ToString()]);
                    }

                    return ld_suma;
                }
            }

            return ld_suma;
        }

        #endregion
    }
}
