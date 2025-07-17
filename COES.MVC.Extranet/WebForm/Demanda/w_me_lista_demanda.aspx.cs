using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;
using System.Data;
using System.Configuration;
using System.IO;
using System.Globalization;

using System.Net;

using System.Drawing;
using WSIC2010.Util;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace WSIC2010.Demanda
{
    public partial class w_me_lista_demanda : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        IDemandaService demandaServ;
        int pi_codigoTipoEmpresa = 0;
        ExcelPackage xlPackage = null;

        string txtTodos = "--TODOS--";
        string dateFormat = "dd/MM/yyyy";
        string cero = "0";
        string TextoDH = "DiariaHistórica";
        string TextoDP = "DiariaPrevista";
        string TextoSP = "SemanalPrevista";
        string TextoExcelDH = "DEMANDA DIARIA HISTORICA (MW)";
        string TextoExcelDP = "DEMANDA DIARIA PREVISTA (MW)";
        string TextoExcelSP = "DEMANDA SEMANAL PREVISTA (MW)";
        string TituloDH = "DEMANDA DIARIA HISTORICA (MW)";
        string TituloDP = "DEMANDA DIARIA PREVISTA (MW)";
        string TituloSP = "DEMANDA SEMANAL PREVISTA (MW)";

        string sqlConsulta = @"
        select pm.ptomedicodi, pm.ptomedielenomb, m.lectcodi, m.medifecha, m.tipoinfocodi, e.equicodi, e.equinomb, e.equitension, e.areacodi, emp.emprnomb,
        (select eq_area.areanomb from eq_area where eq_area.areacodi= e.areacodi) as areanomb,
        (select x.valor from EQ_PROPEQUI x where x.fechapropequi = (select max(fechapropequi) as fechamax from EQ_PROPEQUI where propcodi = 1064 and EQ_PROPEQUI.equicodi = e.equicodi) and x.equicodi = e.equicodi) as area_operativa,
        m.h1, m.h2, m.h3, m.h4, m.h5, m.h6, m.h7, m.h8, m.h9, m.h10,
        m.h11, m.h12, m.h13, m.h14, m.h15, m.h16, m.h17, m.h18, m.h19, m.h20,
        m.h21, m.h22, m.h23, m.h24, m.h25, m.h26, m.h27, m.h28, m.h29, m.h30,
        m.h31, m.h32, m.h33, m.h34, m.h35, m.h36, m.h37, m.h38, m.h39, m.h40,
        m.h41, m.h42, m.h43, m.h44, m.h45, m.h46, m.h47, m.h48, m.meditotal, m.lastuser, m.lastdate
        from ME_MEDICION48 m, ME_PTOMEDICION pm, EQ_EQUIPO e, SI_EMPRESA emp
        where m.ptomedicodi = pm.ptomedicodi
        and emp.emprcodi = pm.emprcodi
        and pm.equicodi = e.equicodi 
        and m.tipoinfocodi = {0}
        and m.lectcodi = {1}
        and m.medifecha >= {2}
        and m.medifecha <= {3}
        and m.ptomedicodi in ({4})
        order by m.ptomedicodi, m.medifecha, m.lectcodi";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.UICulture = "es";
            this.Page.Culture = "es-MX";

            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Demanda/w_me_lista_demanda.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Demanda) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {
                        string[] array_Empresas = in_app.Ls_emprcodi.ToArray();
                        //COES.MVC.Extranet.wsDemanda.IDemanda wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();
                        COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();

                        Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                        string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

                        DataTable dtEmpresas = wsDemanda.EmpresasRepxUsuario(array_Empresas, ls_credencial);
                        DDLEmpresa.DataSource = dtEmpresas;
                        DDLEmpresa.DataValueField = dtEmpresas.Columns[0].ToString();
                        DDLEmpresa.DataTextField = dtEmpresas.Columns[1].ToString();
                        DDLEmpresa.DataBind();

                        demandaServ = new DemandaService();

                        DataTable ln_data = demandaServ.nf_get_empresa_detalles(Convert.ToInt32(DDLEmpresa.SelectedValue));
                        if (nf_get_data(ln_data))
                        {
                            pi_codigoTipoEmpresa = Convert.ToInt32(ln_data.Rows[0]["TIPOEMPRCODI"].ToString());

                            if (pi_codigoTipoEmpresa == 2)
                            {
                                lbl_equipo.Text = "Equipo:";
                            }
                            else if (pi_codigoTipoEmpresa == 4)
                            {
                                lbl_equipo.Text = "Cliente Libre:";
                            }
                        }

                        DataTable dtBarras = wsDemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);
                        bool lb_primero = false;

                        DDLBarras.Items.Clear();
                        DDLBarras.DataBind();


                        if (nf_get_data(dtBarras))
                        {
                            if (!lb_primero)
                            {
                                DDLBarras.Items.Add(new ListItem("(TODOS)", "0"));
                                lb_primero = true;
                            }

                            foreach (DataRow dr in dtBarras.Rows)
                            {
                                DDLBarras.Items.Add(new ListItem(dr[3].ToString(), dr[0].ToString()));
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('La empresa indicada no presenta barras');", true);
                        }

                        //this.rbtipoLectura.RepeatDirection = RepeatDirection.Horizontal;


                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }
            }
        }

        protected bool nf_get_data(DataTable adt_data)
        {
            if (adt_data != null && adt_data.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        protected void DDLEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            //COES.MVC.Extranet.wsDemanda.IDemanda wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();
            COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();

            Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
            string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

            demandaServ = new DemandaService();
            int pi_codigoTipoEmpresa = 0;
            DataTable ln_data = demandaServ.nf_get_empresa_detalles(Convert.ToInt32(DDLEmpresa.SelectedValue));
            if (nf_get_data(ln_data))
            {
                pi_codigoTipoEmpresa = Convert.ToInt32(ln_data.Rows[0]["TIPOEMPRCODI"].ToString());

                if (pi_codigoTipoEmpresa == 2)
                {
                    lbl_equipo.Text = "Equipo:";
                }
                else if (pi_codigoTipoEmpresa == 4)
                {
                    lbl_equipo.Text = "Cliente Libre:";
                }
            }

            DataTable dtBarras = wsDemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);
            bool lb_primero = false;

            DDLBarras.Items.Clear();
            DDLBarras.DataBind();

            if (nf_get_data(dtBarras))
            {
                if (!lb_primero)
                { 
                     DDLBarras.Items.Add(new ListItem("(TODOS)", "0"));
                     lb_primero = true;
                }

                foreach (DataRow dr in dtBarras.Rows)
                {
                    DDLBarras.Items.Add(new ListItem(dr[3].ToString(), dr[0].ToString()));
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('La empresa indicada no presenta barras');", true);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            int tipoInfoCodi = 20;
            int idEmpresa = int.Parse(this.DDLEmpresa.SelectedValue);
            int idBarra = int.Parse(this.DDLBarras.SelectedValue);
            int idLectura = int.Parse(this.rbtipoLectura.SelectedValue);
            string ps_lectura = String.Empty;
            string ps_ptoMedicion = String.Empty;
            DateTime ldt_fechaInicial = new DateTime(2000, 1, 1);
            DateTime ldt_fechaFinal = new DateTime(2000, 1, 1);
            DateTime ldt_fecha;
            String ls_filename = String.Empty;

            Label1.Text = String.Empty;

            if (idBarra == 0)
            {
                foreach (ListItem item in this.DDLBarras.Items)
                {
                    if(item.Value.Trim() != "0")
                        ps_ptoMedicion = ps_ptoMedicion + item.Value + ",";
                }
            }
            else
                ps_ptoMedicion = idBarra.ToString() + ",";


            if (idLectura == 0)
                ps_lectura = "45";
            else if (idLectura == 1)
                ps_lectura = "46";
            else
                ps_lectura = "47";

            if (DateTime.TryParse(Convert.ToString(tBoxInicio.Attributes["value"]), out ldt_fecha))
                ldt_fechaInicial = ldt_fecha;
            else
            {
                Label1.Text = "Definir correctamente fecha inicial";
                return;
            }

            if (DateTime.TryParse(Convert.ToString(tBoxFin.Attributes["value"]), out ldt_fecha))
                ldt_fechaFinal = ldt_fecha;
            else
            {
                Label1.Text = "Definir correctamente fecha final";
                return;
            }

            string fechaDesde = EPDate.SQLDateOracleString(ldt_fechaInicial);
            string fechaHasta = EPDate.SQLDateOracleString(ldt_fechaFinal.AddDays(1));//Agregamos un día

            string sql = string.Format(this.sqlConsulta, tipoInfoCodi, ps_lectura, fechaDesde, fechaHasta, ps_ptoMedicion.Substring(0, ps_ptoMedicion.Length - 1));
            DataTable dt = this.ObtenerConsulta(sql);

            if (ldt_fechaInicial >= ldt_fechaFinal)
            {
                Label1.Text = "Fecha Final debe ser mayor que Fecha Inicial";
                return;
            }

            if ((ldt_fechaFinal - ldt_fechaInicial).Days > 31)
            {
                Label1.Text = "El rango de fechas a exportar no debe exceder los 31 dias";
                return;
            }

            
            if (nf_get_data(dt))
            {
                List<DemandaBarra> list = this.ObtenerLista(dt);

                if (this.rbtipoLectura.SelectedIndex == 0)
                {
                    int li_lectcodiDH = 45;
                    List<DemandaBarra> listDH = this.Procesar(list, li_lectcodiDH);
                    ls_filename = "Reporte_de_demanda_historica_diaria_" + ldt_fechaInicial.ToString("ddMMyy") + "-" + ldt_fechaFinal.ToString("ddMMyy");
                    this.Exportar(listDH, null, null, ls_filename);
                }
                if (this.rbtipoLectura.SelectedIndex == 1)
                {
                    int li_lectcodiDP = 46;
                    List<DemandaBarra> listDP = this.Procesar(list, li_lectcodiDP);
                    ls_filename = "Reporte_de_demanda_prevista_diaria_" + ldt_fechaInicial.ToString("ddMMyy") + "-" + ldt_fechaFinal.ToString("ddMMyy");
                    this.Exportar(null, listDP, null, ls_filename);
                }
                if (this.rbtipoLectura.SelectedIndex == 2)
                {
                    int li_lectcodiSP = 47;
                    List<DemandaBarra> listSP = this.Procesar(list, li_lectcodiSP);
                    ls_filename = "Reporte_de_demanda_prevista_semanal_" + ldt_fechaInicial.ToString("ddMMyy") + "-" + ldt_fechaFinal.ToString("ddMMyy");
                    this.Exportar(null, null, listSP, ls_filename);
                }
            }
            else
            {
                Label1.Text = "No existe data para exportar en las fechas seleccionadas";
                return;
            }

        }

        private List<DemandaBarra> Procesar(List<DemandaBarra> list, int pi_lectcodi)
        {
            List<DemandaBarra> resultado = new List<DemandaBarra>();

            //Filtramos por el tipo especificado
            resultado = list.Where(x => x.Lectcodi == pi_lectcodi).OrderBy(x => x.Medifecha).ToList();
            return resultado;
        }

        
        protected DataTable ObtenerConsulta(string query)
        {
            DataTable dt = new DataTable();
            demandaServ = new DemandaService();
            dt = demandaServ.nf_get_sql(query);
            
            return dt;
        }

        protected List<DemandaBarra> ObtenerLista(DataTable dt)
        {
            List<DemandaBarra> list = new List<DemandaBarra>();
            decimal ld_valor = 0;

            foreach (DataRow row in dt.Rows)
            {
                DemandaBarra item = new DemandaBarra();
                item.Ptomedicodi = Convert.ToInt32(row["ptomedicodi"].ToString());
                item.Ptomedielenomb = row["ptomedielenomb"].ToString();
                item.Equicodi = (!row.IsNull("equicodi")) ? Convert.ToInt32(row["equicodi"]) : -1;
                item.Equinomb = (!row.IsNull("equinomb")) ? row["equinomb"].ToString() : String.Empty;
                item.Areacodi = (!row.IsNull("areacodi")) ? Convert.ToInt32(row["areacodi"]) : -1;
                item.Areanomb = (!row.IsNull("areanomb")) ? row["areanomb"].ToString() : String.Empty;
                item.Emprnomb = (!row.IsNull("emprnomb")) ? row["emprnomb"].ToString() : String.Empty;
                item.AreaOperativa = (!row.IsNull("area_operativa")) ? row["area_operativa"].ToString() : String.Empty;
                item.Medifecha = Convert.ToDateTime(row["medifecha"]);
                item.Lectcodi = Convert.ToInt32(row["lectcodi"]);
                item.Tipoinfocodi = Convert.ToInt32(row["tipoinfocodi"]);
                item.Meditotal = (!row.IsNull("meditotal")) ? Convert.ToDecimal(row["meditotal"]) : 0;
                item.Lastdate = Convert.ToDateTime(row["lastdate"]);
                item.Lastuser = row["lastuser"].ToString();

                PropertyInfo[] propiedades = item.GetType().GetProperties();
                for (int i = 1; i < 49; i++)
			    {
			        foreach (PropertyInfo propiedad in propiedades)
                    {
                        if (propiedad.Name.Equals("H" + i))
                        {
                            ld_valor = (!row.IsNull("H" + i)) ? Convert.ToDecimal(row["H" + i]) : 0;
                            propiedad.SetValue(item, ld_valor, null);
                            break;
                        }
                    }
			    }
                
                list.Add(item);
            }

            return list;
        }

        protected void Exportar(List<DemandaBarra> listDH, List<DemandaBarra> listDP, List<DemandaBarra> listSP, string as_nameFile)
        {
            #region Declaracion

            string ruta = ConfigurationManager.AppSettings["direxcel"].ToString();
            string file = ruta + as_nameFile + ".xlsx";
            FileInfo newFile = new FileInfo(file);
            DateTime ldt_fecha_inicio;
            DateTime ldt_fecha_fin;

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            #endregion

            ldt_fecha_inicio = EPDate.ToDate(Convert.ToString(tBoxInicio.Attributes["value"]));
            ldt_fecha_fin = EPDate.ToDate(Convert.ToString(tBoxFin.Attributes["value"]));

            int semanaInicio = EPDate.f_numerosemana(ldt_fecha_inicio);
            int semanaFin = EPDate.f_numerosemana(ldt_fecha_fin);

            using (xlPackage = new ExcelPackage(newFile))
            {
                if (listDH != null)
                    this.CreaHoja("HISTORICO-DIA-MW", listDH, ldt_fecha_inicio, ldt_fecha_fin, this.TituloDH);
                if (listDP != null)
                    this.CreaHoja("PREVISTO-DIA-MW", listDP, ldt_fecha_inicio, ldt_fecha_fin, this.TituloDP);
                if (listSP != null)
                    this.CreaHoja("PREVISTO-SEM-MW", listSP, ldt_fecha_inicio, ldt_fecha_fin, this.TituloSP);

                xlPackage.Save();
            }

            #region Response


            string filename = Path.GetFileName(file);
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "inline;filename=" + as_nameFile  + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.WriteFile(file);
            fs.Close();
            Response.End();

            #endregion
        }

        protected void CreaHoja(string hojaName, List<DemandaBarra> list, DateTime fechaInicio, DateTime fechaFin, string titulo)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(hojaName);

            if (ws != null)
            {
                ws.Cells[7, 2].Value = titulo;

                ExcelRange rg = ws.Cells[7, 2, 7, 2];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                ws.Cells[11, 2].Value = "FECHA (Desde)";
                ws.Cells[12, 2].Value = "FECHA (Hasta)";

                ws.Cells[11, 3].Value = fechaInicio.ToString("dd/MM/yyyy");
                ws.Cells[12, 3].Value = fechaFin.ToString("dd/MM/yyyy");

                ws.Cells[11, 4].Value = @"DD/MM/AAAA";
                ws.Cells[12, 4].Value = @"DD/MM/AAAA";

                //Obteniendo los ptos de medicion
                List<int> ptosMedicion = list.Select(x => x.Ptomedicodi).Distinct().ToList();

                ptosMedicion.Sort();

                int li_row_base = 27;
                int li_column_base = 3;
                int li_row, li_column, li_row_max;
                li_row_max = li_row_base + 1;

                li_row = li_row_base;
                li_column = li_column_base;

                //Fila de cabeceras
                ws.Cells[li_row_base, li_column_base - 2].Value = @"DIA";
                ws.Cells[li_row_base - 3, li_column_base - 1].Value = @"EMPRESA";
                ws.Cells[li_row_base - 2, li_column_base - 1].Value = @"ÁREA OPERATIVA";
                ws.Cells[li_row_base - 1, li_column_base - 1].Value = @"SUBESTACIÓN";
                ws.Cells[li_row_base, li_column_base - 1].Value = @"FECHA \ EQUIPO";
                foreach (var item in ptosMedicion)
                {
                    DemandaBarra demanda = list.Find(c => (c.Ptomedicodi == item));
                    ws.Cells[li_row - 3, li_column].Value = demanda.Emprnomb;
                    ws.Cells[li_row - 2 , li_column].Value = demanda.AreaOperativa;
                    ws.Cells[li_row - 1, li_column].Value = demanda.Areanomb;
                    ws.Cells[li_row, li_column].Value = demanda.Equinomb;
                    li_column++;
                }

                //Fila de Datos
                li_column = li_column_base;
                li_row++;
                DateTime ldt_fecha;

                foreach (var item in ptosMedicion)
                {

                    ldt_fecha = fechaInicio;
                    while (ldt_fecha <= fechaFin)
                    {
                        DemandaBarra demanda = list.Find(c => (c.Ptomedicodi == item) && (c.Medifecha == ldt_fecha));
                        if (demanda != null)
                        {
                            for (int li = 1; li < 49; li++)
                            {
                                if (li_column == 3)
                                {
                                    ws.Cells[li_row, li_column - 1].Value = ldt_fecha.AddMinutes(li * 30);
                                    ws.Cells[li_row, li_column - 1].Style.Numberformat.Format = "dd/MM/yyyy hh:mm";
                                    ws.Cells[li_row, li_column - 2].Value = ldt_fecha.AddMinutes(li * 30);
                                    ws.Cells[li_row, li_column - 2].Style.Numberformat.Format = "dddd";
                                }

                                ws.Cells[li_row, li_column].Value = Reflection.GetPropValue<decimal>(demanda, "H" + li);
                                ws.Cells[li_row, li_column].Style.Numberformat.Format = "#,##0.00";
                                li_row++;
                            }
                        }
                        //else
                        //{ 
                        //    for (int li = 1; li < 49; li++)
                        //    {
                        //        if (li_column == 3)
                        //        {
                        //            ws.Cells[li_row, li_column - 1].Value = ldt_fecha.AddMinutes(li * 30);
                        //            ws.Cells[li_row, li_column - 1].Style.Numberformat.Format = "dd/MM/yyyy hh:mm";
                        //            ws.Cells[li_row, li_column - 2].Value = ldt_fecha.AddMinutes(li * 30);
                        //            ws.Cells[li_row, li_column - 2].Style.Numberformat.Format = "dddd";
                        //        }
                        //        li_row++;
                        //    }
                        //}

                        li_row_max = li_row;
                        ldt_fecha = ldt_fecha.AddDays(1);
                    }

                    li_row = li_row_base + 1;
                    li_column++;
                }


                //Estilos para Header
                rg = ws.Cells[li_row_base - 3, li_column_base - 2, li_row_base, li_column - 1];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#1F497D"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 8;
                rg.Style.Font.Bold = true;


                //Estilos Body
                rg = ws.Cells[li_row_base - 3, li_column_base - 2, li_row_max - 1, li_column - 1];

                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 8;

                ws.Column(2).Width = 20;
                ws.DefaultRowHeight = 12;
                ws.Row(7).Height = 15;

                //ws.Column(2).Style.Numberformat.Format = "dd/MM/yyyy";
                //ws.Column(5).Style.Numberformat.Format = "dd/MM/yyyy hh:mm";

                //for (int t = 6; t <= 53; t++)
                //{
                //    ws.Column(t).Style.Numberformat.Format = "#,##0.00";
                //}

                rg = ws.Cells[1, 1, li_row_max - 1, li_column - 1];
                rg.AutoFitColumns();
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 3;
                picture.SetSize(160, 60);

            }
        }

        protected int ObtenerNroDias(int nroMes)
        {
            int[] nroDias = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            return nroDias[nroMes - 1];
        }

        protected string ObtenerColumnNumber(int nroFila, int nroColumn)
        {
            string[] columns = {"H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                               "AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ",
                               "BA","BB","BC"};

            int index = nroColumn - 8;

            return columns[index] + nroFila;
        }

        public class DemandaBarra
        {
            public int Ptomedicodi { get; set; }
            public string Ptomedielenomb { get; set; }
            public int Equicodi { get; set; }
            public string Equinomb { get; set; }
            public int Areacodi { get; set; }
            public string Areanomb { get; set; }
            public string Emprnomb { get; set; }
            public string AreaOperativa { get; set; }
            public int Tipoinfocodi { get; set; }
            public int Lectcodi { get; set; }
            public DateTime Medifecha { get; set; }
            public string Lastuser { get; set; }
            public DateTime Lastdate { get; set; }
            public decimal  Meditotal { get; set; }
            public decimal H1 { get; set; }
            public decimal H2 { get; set; }
            public decimal H3 { get; set; }
            public decimal H4 { get; set; }
            public decimal H5 { get; set; }
            public decimal H6 { get; set; }
            public decimal H7 { get; set; }
            public decimal H8 { get; set; }
            public decimal H9 { get; set; }
            public decimal H10 { get; set; }
            public decimal H11 { get; set; }
            public decimal H12 { get; set; }
            public decimal H13 { get; set; }
            public decimal H14 { get; set; }
            public decimal H15 { get; set; }
            public decimal H16 { get; set; }
            public decimal H17 { get; set; }
            public decimal H18 { get; set; }
            public decimal H19 { get; set; }
            public decimal H20 { get; set; }
            public decimal H21 { get; set; }
            public decimal H22{ get; set; }
            public decimal H23 { get; set; }
            public decimal H24 { get; set; }
            public decimal H25 { get; set; }
            public decimal H26 { get; set; }
            public decimal H27 { get; set; }
            public decimal H28 { get; set; }
            public decimal H29 { get; set; }
            public decimal H30 { get; set; }
            public decimal H31 { get; set; }
            public decimal H32 { get; set; }
            public decimal H33 { get; set; }
            public decimal H34 { get; set; }
            public decimal H35 { get; set; }
            public decimal H36 { get; set; }
            public decimal H37 { get; set; }
            public decimal H38 { get; set; }
            public decimal H39 { get; set; }
            public decimal H40 { get; set; }
            public decimal H41 { get; set; }
            public decimal H42 { get; set; }
            public decimal H43 { get; set; }
            public decimal H44 { get; set; }
            public decimal H45 { get; set; }
            public decimal H46 { get; set; }
            public decimal H47 { get; set; }
            public decimal H48 { get; set; }

            private decimal[] medida = new decimal[48];

            public decimal[] Medida
            {
                get { return medida; }
                set { medida = value; }
            }
            
        }
    }
}