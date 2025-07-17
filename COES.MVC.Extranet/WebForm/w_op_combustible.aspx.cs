using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Configuration;
using System.Text;
using System.Globalization;

namespace WSIC2010
{
    public partial class w_op_combustible : System.Web.UI.Page
    {
        #region Variables y Propiedades

        /// <summary>
        /// Instancia para acceder a los datos
        /// </summary>
        n_app dao;
        DataSet ds = new DataSet();
        string sqlGrupo = @"select distinct c.ptomedicodi,  b.gruponomb as gruponomb from  pr_grupo b                             
                        inner join me_ptomedicion c on c.grupocodi = b.grupocodi
                        where c.emprcodi = {0} and c.origlectcodi = 13                       
                        order by 2 asc ";
        string sqlGrupoRegistro = @"select distinct mp.ptomedicodi, pg.gruponomb from pr_disponibilidad sd
                                    inner join me_ptomedicion mp on sd.ptomedicodi = mp.ptomedicodi
                                    inner join pr_grupo pg on mp.grupocodi = pg.grupocodi
                                    where sd.estado = 'A' and sd.emprcodi = {0}";
        string sqlExisteRow = "select count(*) from me_medicion1 where lectcodi = {0} and medifecha = {1} and tipoinfocodi = {2} and ptomedicodi = {3}";
        string sqlRegistro = @"insert into me_medicion1(lectcodi, medifecha, tipoinfocodi, ptomedicodi, h1, lastuser, lastdate,nota)
                               values ({0}, {1}, {2}, {3}, {4}, '{5}', {6}, '{7}')";
        string sqlUpdate = @"update me_medicion1 set h1 = {4}, nota = '{7}', lastuser='{5}', lastdate={6} where lectcodi = {0} and medifecha = {1} and tipoinfocodi = {2} and ptomedicodi = {3}";
        string sqlListado = @"select 
                              me.lectcodi, me.medifecha, me.tipoinfocodi, me.ptomedicodi, se.emprnomb, pg.gruponomb, 
                              st.tipoinfodesc, me.h1, me.nota, me.lastuser, me.lastdate
                              from me_medicion1 me
                              inner join me_ptomedicion mp
                              on me.ptomedicodi = mp.ptomedicodi
                              inner join pr_grupo pg
                              on mp.grupocodi = pg.grupocodi
                              inner join si_empresa se
                              on pg.emprcodi = se.emprcodi
                              inner join si_tipoinformacion st
                              on me.tipoinfocodi = st.tipoinfocodi
                              where me.lectcodi = {0} and me.medifecha >= {1} and me.medifecha <= {2}
                              {3}
                              order by me.medifecha, se.emprnomb, pg.gruponomb desc";
        string sqlReporte = @" 
                            select {0} as medifecha, sd.emprcodi, se.emprnomb, sd.ptomedicodi, pg.gruponomb, sd.tipoinfocodi, 
                            st.tipoinfodesc,
                            (select med.h1 from me_medicion1 med where med.lectcodi = 50 and med.medifecha =  {0} and 
                             med.tipoinfocodi = sd.tipoinfocodi and med.ptomedicodi = sd.ptomedicodi ) as h1,
                            (select med.nota from me_medicion1 med where med.lectcodi = 50 and med.medifecha =  {0} and 
                             med.tipoinfocodi = sd.tipoinfocodi and med.ptomedicodi = sd.ptomedicodi ) as nota
                            from pr_disponibilidad sd 
                            inner join si_empresa se on sd.emprcodi = se.emprcodi
                            inner join me_ptomedicion mp on sd.ptomedicodi = mp.ptomedicodi
                            inner join pr_grupo pg on mp.grupocodi = pg.grupocodi
                            inner join si_tipoinformacion st on sd.tipoinfocodi = st.tipoinfocodi
                            where sd.emprcodi = {1} or '{1}' = '0' and sd.estado = 'A' and pg.grupoactivo='S'
                            order by 1,3,5,7 asc
                            ";

        string sqlEliminar = @"delete from me_medicion1 where lectcodi = {0} and medifecha = {1} and tipoinfocodi = {2} and ptomedicodi = {3}";
        string sqlEmpresas = @"select distinct se.emprcodi, se.emprnomb from si_empresa se 
                               inner join pr_grupo pg
                               on se.emprcodi = pg.emprcodi
                               inner join si_fuenteenergia f
                               on pg.fenergcodi = f.fenergcodi
                               inner join si_tipogeneracion t
                               on f.tgenercodi = t.tgenercodi
                               where pg.grupotipo = 'T' and se.emprsein = 'S' and se.emprcodi != '0' 
                               order by 2 asc";
        string sqlEmpresasFiltro = @"select distinct sd.emprcodi, se.emprnomb from pr_disponibilidad sd
                                    inner join si_empresa se
                                    on sd.emprcodi = se.emprcodi where sd.estado = 'A'
                                    order by 2 asc";
        string sqlEmpresaRegistro = @"select distinct sd.emprcodi, se.emprnomb from pr_disponibilidad sd
                                    inner join si_empresa se
                                    on sd.emprcodi = se.emprcodi where sd.estado = 'A' {0}
                                    order by 2 asc
                                    ";
        string sqlListarConfiguracion = @" 
                             select sd.emprcodi, se.emprnomb, sd.ptomedicodi, pg.gruponomb, 
                             sd.tipoinfocodi, st.tipoinfodesc,
                             (case sd.estado when 'A' then 'Activo' when 'I' then 'Inactivo' else '' end) as estado,
                             sd.usercreate, sd.userupdate, sd.fecupdate, sd.feccreate
                             from pr_disponibilidad sd
                             inner join me_ptomedicion pm on sd.ptomedicodi = pm.ptomedicodi
                             inner join pr_grupo pg on pm.grupocodi = pg.grupocodi
                             inner join si_empresa se on se.emprcodi = sd.emprcodi
                             inner join si_tipoinformacion st on st.tipoinfocodi = sd.tipoinfocodi
                             order by 2,4,6";
        string sqlObtenerFuenteEnergia = @"
                                    select distinct st.tipoinfodesc, sd.tipoinfocodi from pr_disponibilidad sd
                                    inner join si_tipoinformacion st  on sd.tipoinfocodi = st.tipoinfocodi
                                    where sd.emprcodi = {0} and sd.ptomedicodi = {1}";
        string sqlDeleteConfiguracion = @"delete from pr_disponibilidad where emprcodi = {0} and ptomedicodi = {1} and tipoinfocodi = {2}";
        string sqlGetConfiguracion = @"select * from pr_disponibilidad where emprcodi = {0} and ptomedicodi = {1} and tipoinfocodi = {2}";
        string sqlCountConfiguracion = @"select count(*) from pr_disponibilidad where emprcodi = {0} and ptomedicodi = {1} and tipoinfocodi = {2}";
        string sqlInsertaConfiguracion = @"insert into pr_disponibilidad (emprcodi,ptomedicodi, tipoinfocodi, estado, usercreate, feccreate) values({0},{1},{2},'{3}', '{4}',{5})";
        string sqlUpdateConfiguracion = @"update pr_disponibilidad set estado = '{3}', userupdate = '{4}', fecupdate = {5} where emprcodi = {0} and ptomedicodi = {1} and tipoinfocodi = {2}";
        string sqlObtenerRegistro = @"
                                    select mm.lectcodi, mm.medifecha, mm.tipoinfocodi, mm.ptomedicodi, mm.h1, mm.nota, pg.emprcodi
                                    from me_medicion1 mm inner join me_ptomedicion mp on mm.ptomedicodi = mp.ptomedicodi
                                    inner join pr_grupo pg on pg.grupocodi = mp.grupocodi
                                    where mm.lectcodi = {0} and mm.medifecha = {1} and mm.tipoinfocodi = {2} and mm.ptomedicodi = {3}";
        string sqlValidarEliminar = @"select count(*) from me_medicion1 where ptomedicodi = {0} and lectcodi = {1} and tipoinfocodi = {2}";
        string sqlListarCopiar = @"select me.lectcodi, me.medifecha, me.tipoinfocodi, me.ptomedicodi, me.h1, me.nota
                                   from me_medicion1 me where me.ptomedicodi = {0} and me.medifecha = {1} and lectcodi = {2}";
        string sqlTipoInformacion = @"select tipoinfodesc from si_tipoinformacion where tipoinfocodi = {0}";
        string tbGrupo = "pr_grupo";
        string tbEmpresa = "si_empresa";
        string tbEmpresaFiltro = "si_empresa_filtro";
        string tbListado = "me_medicion1";
        string tbCopia = "TablaCopia";
        string txtSeleccione = "--SELECCIONE--";
        string sesionApp = "in_app";
        string sqlNogas = "tipoinfocodi <> 46";
        string sqlGas = "tipoinfocodi = 46";
        private int LectCodi = 50;


        /// <summary>
        /// Tabla para almacenar la disponibilidad
        /// </summary>
        public DataTable TableDisponibilidad
        {
            get { return (ViewState["TableDisponibilidad"] != null) ? (DataTable)ViewState["TableDisponibilidad"] : new DataTable(); }
            set { ViewState["TableDisponibilidad"] = value; }
        }

        #endregion

        /// <summary>
        /// Evento de carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[sesionApp] == null)
            {
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {

                this.dao = (n_app)Session[sesionApp];

                if (!IsPostBack)
                {
                    this.CargarDatos();
                }
            }
        }

        /// <summary>
        /// Ejecuta el registro de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGrabarStock_Click(object sender, EventArgs e)
        {
            this.GrabarRegistro();
        }

        /// <summary>
        /// Cancela el proceso de registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.mpeRegistro.Hide();
        }

        /// <summary>
        /// Realiza la operación de exportado dependiendo de la seleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarExportar_Click(object sender, EventArgs e)
        {
            string empresa = (string.IsNullOrEmpty(this.ddlEmpresaFiltro.SelectedValue)) ? "0" : this.ddlEmpresaFiltro.SelectedValue;
            string sql = string.Format(this.sqlReporte, EPDate.SQLDateOracleString(DateTime.ParseExact(this.txtFechaExportar.Text,
                "dd/MM/yyyy", CultureInfo.InvariantCulture)), empresa);
            if (ds.Tables["TablaReporte"] != null) ds.Tables["TablaReporte"].Clear();
            this.dao.Fill(0, ds, "TablaReporte", sql);


            this.TableDisponibilidad = ds.Tables["TablaReporte"];
            this.Exportar(this.TableDisponibilidad);
            this.pnlExportar.Visible = false;
        }

        /// <summary>
        /// Cancela la operación de Exportado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarExportar_Click(object sender, EventArgs e)
        {
            this.pnlExportar.Visible = false;
        }

        /// <summary>
        /// Permite agregar un nuevo registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.lblMensaje.Text = "Por favor ingrese los datos";
            this.pnlMensaje.CssClass = "content-message";
            this.ddlEmpresa.SelectedIndex = -1;
            this.ddlCentral.SelectedIndex = -1;
            this.ddlTipoInformacion.SelectedIndex = -1;
            this.txtFecha.Text = (!string.IsNullOrEmpty(this.txtFechaInicio.Text)) ?
                this.txtFechaInicio.Text : DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            this.txtLimInferior.Text = (!string.IsNullOrEmpty(this.txtFechaInicio.Text)) ?
                this.txtFechaInicio.Text : DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            this.txtLimSuperior.Text = string.Empty;
            this.txtH1.Text = string.Empty;
            this.textNota.Text = string.Empty;

            this.ddlEmpresa.Enabled = true;
            this.ddlCentral.Enabled = true;
            this.txtFecha.Enabled = true;
            this.ddlTipoInformacion.Enabled = true;
            this.hfEdicion.Value = "N";

            this.txtFecha.Visible = false;
            this.tablaFecha.Visible = true;

            this.pnlConfirmarGrabar.Visible = false;
            this.pnlRegistrarNuevo.Visible = true;

            if (this.ddlEmpresa.Items.Count == 2) this.ddlEmpresa.SelectedIndex = 1;

            this.mpeRegistro.Show();
        }

        /// <summary>
        /// Carga los datos dependiendo de la empresa seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarCentrales();
            this.mpeRegistro.Show();
        }

        /// <summary>
        /// Carga los tipos de combustibles por cada central
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCentral_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarTipoCombustible();
            this.mpeRegistro.Show();
        }

        /// <summary>
        /// Carga los tipos de combustible
        /// </summary>
        protected void CargarTipoCombustible()
        {
            this.ddlTipoInformacion.Items.Clear();
            if (!string.IsNullOrEmpty(this.ddlCentral.SelectedValue))
            {
                int idEmpresa = int.Parse(this.ddlEmpresa.SelectedValue);
                int idCentral = int.Parse(this.ddlCentral.SelectedValue);

                string sql = string.Format(this.sqlObtenerFuenteEnergia, idEmpresa, idCentral);
                if (ds.Tables["TipoCombustible"] != null) ds.Tables["TipoCombustible"].Clear();
                this.dao.Fill(0, ds, "TipoCombustible", sql);

                this.ddlTipoInformacion.DataSource = ds.Tables["TipoCombustible"];
                this.ddlTipoInformacion.DataBind();
                this.ddlTipoInformacion.Items.Insert(0, new ListItem(this.txtSeleccione, string.Empty));
                if (this.ddlTipoInformacion.Items.Count == 2) this.ddlTipoInformacion.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// Evento para agregar funcionalidad javascript
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lbQuitar");
                lb.Attributes.Add("onclick", "return confirm('¿Está seguro de realizar esta operación?')");
            }
        }

        /// <summary>
        /// Evento para ejecutar comando
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                this.QuitarRegistro(int.Parse(e.CommandArgument.ToString()));
            }
            if (e.CommandName == "Modificar")
            {
                this.EditarRegistro(int.Parse(e.CommandArgument.ToString()));
            }
        }

        /// <summary>
        /// Permite controlar el paginado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvStock.PageIndex = e.NewPageIndex;
            this.CargarDataGrid();
        }

        /// <summary>
        /// Permite mostrar los registros ingresados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarDataGrid();
        }

        /// <summary>
        /// Busqueda por defecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEmpresaFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarDataGrid();
        }

        /// <summary>
        /// Permite exportar a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            this.txtFechaExportar.Text = (!string.IsNullOrEmpty(this.txtFechaInicio.Text)) ?
               this.txtFechaInicio.Text : DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");

            this.pnlExportar.Visible = true;
        }

        /// <summary>
        /// Permite cargar los datos iniciales de la pantalla
        /// </summary>
        protected void CargarDatos()
        {
            string[] idEmpresas = dao.Ls_emprcodi.ToArray();
            string whereEmpresa = string.Empty;
            string ids = string.Empty;
            if (!(idEmpresas.Length == 1 && idEmpresas[0] == "0"))
            {
                for (int i = 0; i < idEmpresas.Length; i++)
                {
                    if (i == idEmpresas.Length - 1) ids = ids + idEmpresas[i];
                    else ids = ids + idEmpresas[i] + ",";
                }
                whereEmpresa = string.Format("and se.emprcodi in({0})", ids);
            }

            string sqlEmp = string.Format(this.sqlEmpresaRegistro, whereEmpresa);

            if (ds.Tables[this.tbEmpresa] != null) ds.Tables[this.tbEmpresa].Clear();
            this.dao.Fill(0, ds, this.tbEmpresa, sqlEmp);

            this.ddlEmpresa.DataSource = ds.Tables[this.tbEmpresa];
            this.ddlEmpresa.DataBind();
            this.ddlEmpresa.Items.Insert(0, new ListItem("--SELECCIONE--", string.Empty));

            if (this.ddlEmpresa.Items.Count == 2) this.ddlEmpresa.SelectedIndex = 1;

            this.ddlEmpresaCopiar.DataSource = ds.Tables[this.tbEmpresa];
            this.ddlEmpresaCopiar.DataBind();
            this.ddlEmpresaCopiar.Items.Insert(0, new ListItem("--SELECCIONE--", string.Empty));


            if (ds.Tables[this.tbEmpresaFiltro] != null) ds.Tables[this.tbEmpresaFiltro].Clear();
            this.dao.Fill(0, ds, this.tbEmpresaFiltro, this.sqlEmpresasFiltro);
            this.ddlEmpresaFiltro.DataSource = ds.Tables[this.tbEmpresaFiltro];
            this.ddlEmpresaFiltro.DataBind();
            this.ddlEmpresaFiltro.Items.Insert(0, new ListItem("--TODOS--", ""));


            this.txtFecha.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            this.txtFechaInicio.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            this.txtFechaFin.Text = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");

            this.TableDisponibilidad = null;

            this.mpeRegistro.Hide();

            this.CargarCentrales();
            this.CargarDataGrid();

            if (ConfigurationManager.AppSettings["AdminDisponibilidad"] != null)
            {
                string[] users = ConfigurationManager.AppSettings["AdminDisponibilidad"].ToString().Split(',');

                if (users.Contains(dao.is_UserLogin))
                {
                    this.divMantenimiento.Visible = true;
                }
                else this.divMantenimiento.Visible = false;
            }
            else this.divMantenimiento.Visible = false;


            this.mvGeneral.SetActiveView(this.vRegistro);
        }

        /// <summary>
        /// Carga las centrales por cada empresa
        /// </summary>
        protected void CargarCentrales()
        {
            this.ddlCentral.Items.Clear();
            this.ddlTipoInformacion.Items.Clear();
            if (!string.IsNullOrEmpty(this.ddlEmpresa.SelectedValue))
            {
                decimal idEmpresa = decimal.Parse(this.ddlEmpresa.SelectedValue);
                if (ds.Tables[this.tbGrupo] != null) ds.Tables[this.tbGrupo].Clear();
                this.dao.Fill(0, ds, this.tbGrupo, string.Format(this.sqlGrupoRegistro, idEmpresa));

                this.ddlCentral.DataSource = ds.Tables[this.tbGrupo];
                this.ddlCentral.DataBind();
                this.ddlCentral.Items.Insert(0, new ListItem(this.txtSeleccione, string.Empty));
            }
        }

        /// <summary>
        /// Carga las grillas
        /// </summary>
        protected void CargarDataGrid()
        {
            string fechaIni = EPDate.SQLDateOracleString(DateTime.ParseExact(this.txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            string fechaFin = EPDate.SQLDateOracleString(DateTime.ParseExact(this.txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));

            string idEmpresas = string.Empty;
            if (!string.IsNullOrEmpty(this.ddlEmpresaFiltro.SelectedValue)) idEmpresas = string.Format("and se.emprcodi = {0}", this.ddlEmpresaFiltro.SelectedValue);

            string query = string.Format(this.sqlListado, this.LectCodi, fechaIni, fechaFin, idEmpresas);

            if (ds.Tables[this.tbListado] != null) ds.Tables[this.tbListado].Clear();
            this.dao.Fill(0, ds, this.tbListado, query);

            this.lblMsg.Text = string.Empty;

            this.gvStock.DataSource = ds.Tables[this.tbListado];
            this.gvStock.DataBind();
        }

        /// <summary>
        /// Obtiene la fecha en formato oracle
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        protected string ObtenerFormatoFecha(DateTime fecha)
        {
            return string.Format("TO_DATE('{0}-{1}-{2} {3}:{4}', 'YYYY-MM-DD HH24:MI')", fecha.Year, fecha.Month.ToString().PadLeft(2, '0'),
                fecha.Day.ToString().PadLeft(2, '0'), fecha.Hour.ToString().PadLeft(2, '0'), fecha.Minute.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// Permite grabar los datos ingresados
        /// </summary>
        protected void GrabarRegistro()
        {
            string msg = string.Empty;
            List<ItemCombustible> list = new List<ItemCombustible>();

            if (this.Validar(out msg))
            {
                this.lblMensaje.Text = string.Empty;

                decimal count = 0;
                decimal idPtoMedicion = decimal.Parse(this.ddlCentral.SelectedValue);
                string fecha = EPDate.SQLDateOracleString(DateTime.ParseExact(this.txtFecha.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                decimal idTipoInformacion = decimal.Parse(this.ddlTipoInformacion.SelectedValue);
                decimal idLectCodi = this.LectCodi;
                decimal h1 = 0;
                string nota = this.textNota.Text;
                if (decimal.TryParse(this.txtH1.Text, out h1)) { }

                try
                {
                    if (this.hfEdicion.Value == "N")
                    {
                        string validacionFecha = string.Empty;

                        if (this.ValidarFecha(out validacionFecha))
                        {
                            DateTime fechaIni = DateTime.ParseExact(this.txtLimInferior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            DateTime fechaFin = (!string.IsNullOrEmpty(this.txtLimSuperior.Text)) ?
                                DateTime.ParseExact(this.txtLimSuperior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) : fechaIni;
                            int nroDias = (int)fechaFin.Subtract(fechaIni).TotalDays;
                            bool validacionExistencia = false;
                            DateTime newDate = DateTime.Now; ;
                            for (int i = 0; i <= nroDias; i++)
                            {
                                newDate = fechaIni.AddDays(i);
                                string newOracleDate = EPDate.SQLDateOracleString(newDate);

                                string sqlId = string.Format(this.sqlExisteRow, idLectCodi, newOracleDate, idTipoInformacion, idPtoMedicion);
                                count = Convert.ToDecimal(dao.iL_data[0].nf_ExecuteScalar(sqlId));

                                if (count == 0)
                                {
                                    //string mensaje = string.Empty;
                                    //string sql = string.Format(this.sqlRegistro, idLectCodi, newOracleDate, idTipoInformacion, idPtoMedicion, h1);
                                    //this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out mensaje);
                                    //this.lblMensaje.Text = mensaje;
                                }
                                else
                                {
                                    ItemCombustible entity = new ItemCombustible();
                                    entity.Descripcion = newDate.ToString("dd/MM/yyyy");
                                    validacionExistencia = true;
                                    list.Add(entity);
                                }
                            }
                            if (validacionExistencia)
                            {
                                this.lblMensaje.Text = "El registro ya exite en las fechas indicadas.";
                                this.pnlMensaje.CssClass = "content-alert";
                                this.pnlConfirmarGrabar.Visible = true;
                                this.pnlRegistrarNuevo.Visible = false;
                                this.gvFechasRepetidas.DataSource = list;
                                this.gvFechasRepetidas.DataBind();

                                if (list.Count == nroDias)
                                {
                                    this.divMsgConfirmarGrabar.Visible = false;
                                    this.btnConfirmarGrabar.Visible = false;
                                }
                                else
                                {
                                    this.divMsgConfirmarGrabar.Visible = true;
                                    this.btnConfirmarGrabar.Visible = true;
                                }

                                this.mpeRegistro.Show();
                            }
                            else
                            {

                                newDate = DateTime.Now; ;
                                for (int i = 0; i <= nroDias; i++)
                                {
                                    newDate = fechaIni.AddDays(i);
                                    string newOracleDate = EPDate.SQLDateOracleString(newDate);

                                    string sqlId = string.Format(this.sqlExisteRow, idLectCodi, newOracleDate, idTipoInformacion, idPtoMedicion);
                                    count = Convert.ToDecimal(dao.iL_data[0].nf_ExecuteScalar(sqlId));

                                    if (count == 0)
                                    {
                                        string mensaje = string.Empty;
                                        string fechaUpdate = this.ObtenerFormatoFecha(DateTime.Now);
                                        string sql = string.Format(this.sqlRegistro, idLectCodi, newOracleDate, idTipoInformacion, idPtoMedicion, h1, dao.is_UserLogin, fechaUpdate, nota);
                                        this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out mensaje);
                                        this.lblMensaje.Text = mensaje;
                                    }

                                }

                                this.txtFechaFin.Text = newDate.ToString("dd/MM/yyyy");
                                this.ddlEmpresaFiltro.SelectedValue = this.ddlEmpresa.SelectedValue;
                                this.mpeRegistro.Hide();
                                this.CargarDataGrid();
                            }
                        }
                        else
                        {
                            this.lblMensaje.Text = validacionFecha;
                            this.pnlMensaje.CssClass = "content-alert";
                            this.mpeRegistro.Show();
                        }
                    }
                    else
                    {
                        string validacionFecha = string.Empty;

                        if (this.ValidarFechaEdicion(out validacionFecha, this.txtFecha.Text))
                        {
                            string mensaje = string.Empty;
                            string fechaUpdate = this.ObtenerFormatoFecha(DateTime.Now);
                            string sql = string.Format(this.sqlUpdate, idLectCodi, fecha, idTipoInformacion, idPtoMedicion, h1, dao.is_UserLogin, fechaUpdate, nota);
                            this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out mensaje);
                            this.lblMensaje.Text = mensaje;
                            this.mpeRegistro.Hide();
                            this.ddlEmpresaFiltro.SelectedValue = this.ddlEmpresa.SelectedValue;
                            this.CargarDataGrid();
                        }
                        else
                        {
                            this.lblMensaje.Text = validacionFecha;
                            this.pnlMensaje.CssClass = "content-alert";
                            this.mpeRegistro.Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.lblMensaje.Text = ex.Message;
                    this.pnlMensaje.CssClass = "content-error";
                    this.mpeRegistro.Show();
                }
            }
            else
            {
                this.lblMensaje.Text = msg;
                this.pnlMensaje.CssClass = "content-alert";
                this.mpeRegistro.Show();
            }
        }

        /// <summary>
        /// Graba el stock en los dias que no existen datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmarGrabar_Click(object sender, EventArgs e)
        {
            decimal count = 0;
            decimal idPtoMedicion = decimal.Parse(this.ddlCentral.SelectedValue);
            decimal idTipoInformacion = decimal.Parse(this.ddlTipoInformacion.SelectedValue);
            decimal idLectCodi = this.LectCodi;
            decimal h1 = 0;
            string nota = this.textNota.Text;
            if (decimal.TryParse(this.txtH1.Text, out h1)) { }


            DateTime fechaIni = DateTime.ParseExact(this.txtLimInferior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaFin = (!string.IsNullOrEmpty(this.txtLimSuperior.Text)) ?
                DateTime.ParseExact(this.txtLimSuperior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) : fechaIni;
            int nroDias = (int)fechaFin.Subtract(fechaIni).TotalDays;
            DateTime newDate = DateTime.Now; ;
            for (int i = 0; i <= nroDias; i++)
            {
                newDate = fechaIni.AddDays(i);
                string newOracleDate = EPDate.SQLDateOracleString(newDate);

                string sqlId = string.Format(this.sqlExisteRow, idLectCodi, newOracleDate, idTipoInformacion, idPtoMedicion);
                count = Convert.ToDecimal(dao.iL_data[0].nf_ExecuteScalar(sqlId));

                if (count == 0)
                {
                    string mensaje = string.Empty;
                    string fechaUpdate = this.ObtenerFormatoFecha(DateTime.Now);
                    string sql = string.Format(this.sqlRegistro, idLectCodi, newOracleDate, idTipoInformacion, idPtoMedicion, h1, dao.is_UserLogin, fechaUpdate, nota);
                    this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out mensaje);
                    this.lblMensaje.Text = mensaje;
                }
            }

            this.txtFechaFin.Text = newDate.ToString("dd/MM/yyyy");
            this.ddlEmpresaFiltro.SelectedValue = this.ddlEmpresa.SelectedValue;
            this.mpeRegistro.Hide();
            this.CargarDataGrid();
        }

        /// <summary>
        /// Cancela la operación de garabado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarConfirmar_Click(object sender, EventArgs e)
        {
            this.pnlRegistrarNuevo.Visible = true;
            this.pnlConfirmarGrabar.Visible = false;
            this.mpeRegistro.Show();
        }

        /// <summary>
        /// Permite validar el ingreso de datos
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        protected bool Validar(out string mensaje)
        {
            bool flag = true;

            StringBuilder str = new StringBuilder();
            //str.Append("<ul>");
            decimal val = 0;
            int count = 0;
            if (this.ddlEmpresa.SelectedIndex == 0) { flag = false; str.Append("<li>Seleccione empresa</li>"); }
            if (this.ddlCentral.SelectedIndex == 0) { flag = false; str.Append("<li>Seleccione el punto de medición.</li>"); }
            if (this.ddlTipoInformacion.SelectedIndex == 0) { flag = false; str.Append("<li>Seleccione el tipo de combustible.</li>"); }
            if (string.IsNullOrEmpty(this.txtFecha.Text)) { flag = false; str.Append("<li>Ingrese una fecha.</li>"); }
            if (string.IsNullOrEmpty(this.txtH1.Text)) { flag = false; str.Append("<li>Ingrese la disponibilidad.</li>"); }
            else if (!decimal.TryParse(this.txtH1.Text, out val)) { flag = false; str.Append("<li>Ingrese un valor válido</li>"); }

            if (!string.IsNullOrEmpty(this.txtFecha.Text))
            {
                DateTime fecValida = DateTime.Now;
                if (!DateTime.TryParseExact(this.txtFecha.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out fecValida))
                {
                    flag = false;
                    str.Append("<li>El formato de la fecha ingresada no es correcta.</li>");
                    count++;
                }
            }

            if (!string.IsNullOrEmpty(this.txtLimInferior.Text) && string.IsNullOrEmpty(this.txtLimSuperior.Text))
            {
                DateTime fecValida = DateTime.Now;
                if (!DateTime.TryParseExact(this.txtLimInferior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out fecValida))
                {
                    flag = false;
                    str.Append("<li>La fecha inicial no tiene un formato correcto.</li>");
                    count++;
                }
            }

            if (!string.IsNullOrEmpty(this.txtLimInferior.Text) && !string.IsNullOrEmpty(this.txtLimSuperior.Text))
            {
                DateTime fecValida = DateTime.Now;
                bool validDate = true;

                if (!DateTime.TryParseExact(this.txtLimInferior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out fecValida))
                {
                    flag = false;
                    str.Append("<li>La fecha inicial no tiene un formato correcto.</li>");
                    count++;
                    validDate = false;
                }

                if (!DateTime.TryParseExact(this.txtLimSuperior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out fecValida))
                {
                    flag = false;
                    str.Append("<li>La fecha final no tiene un formato correcto.</li>");
                    count++;
                    validDate = false;
                }

                if (validDate)
                {
                    DateTime fechaInicio = DateTime.ParseExact(this.txtLimInferior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime fechaFin = DateTime.ParseExact(this.txtLimSuperior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (fechaFin.Subtract(fechaInicio).TotalDays < 0)
                    {
                        flag = false;
                        str.Append("<li>La fecha de fín debe ser superior a la fecha de inicio.</li>");
                        count++;
                    }
                }
            }
            //str.Append("</li>");           
            if (flag) mensaje = string.Empty;
            else mensaje = str.ToString();

            return flag;
        }

        /// <summary>
        /// Permite quitar los registros previa confirmacion
        /// </summary>
        /// <param name="id"></param>
        protected void QuitarRegistro(int index)
        {
            try
            {
                GridViewRow row = this.gvStock.Rows[index];
                string fecha = EPDate.SQLDateOracleString(DateTime.ParseExact(row.Cells[2].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                string idPtoMedicion = ((Label)row.FindControl("lblPtoMedicion")).Text;
                string idTipoInformacion = ((Label)row.FindControl("lblTipoInformacion")).Text;


                string sql = string.Format(this.sqlObtenerRegistro, this.LectCodi, fecha, idTipoInformacion, idPtoMedicion);

                if (ds.Tables["ModificarRegistro"] != null) ds.Tables["ModificarRegistro"].Clear();
                this.dao.Fill(0, ds, "ModificarRegistro", sql);

                if (ds.Tables["ModificarRegistro"] != null)
                {
                    DataRow fila = ds.Tables["ModificarRegistro"].Rows[0];
                    string idEmpresa = fila["EMPRCODI"].ToString();

                    ListItem liEmpresa = this.ddlEmpresa.Items.FindByValue(idEmpresa);

                    if (liEmpresa != null)
                    {
                        string validacionFecha = string.Empty;

                        if (this.ValidarFechaEdicion(out validacionFecha, row.Cells[2].Text))
                        {
                            string msg = string.Empty;
                            string sqlDelete = string.Format(this.sqlEliminar, this.LectCodi, fecha, idTipoInformacion, idPtoMedicion);
                            this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sqlDelete, out msg);
                            this.CargarDataGrid();
                            this.lblMsg.Text = string.Empty;
                        }
                        else
                        {
                            this.lblMsg.Text = validacionFecha;
                        }
                    }
                    else
                    {
                        this.lblMsg.Text = "Usted no tiene acceso para eliminar este registro.";
                    }

                }

            }
            catch (Exception ex)
            {
                this.lblMsg.Text = ex.Message;
            }
        }

        /// <summary>
        /// Permite editar el registro
        /// </summary>
        /// <param name="index"></param>
        protected void EditarRegistro(int index)
        {
            try
            {
                this.lblMensaje.Text = "Por favor ingrese los datos";
                this.pnlMensaje.CssClass = "content-message";

                GridViewRow row = this.gvStock.Rows[index];
                string fecha = EPDate.SQLDateOracleString(DateTime.ParseExact(row.Cells[2].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                string idPtoMedicion = ((Label)row.FindControl("lblPtoMedicion")).Text;
                string idTipoInformacion = ((Label)row.FindControl("lblTipoInformacion")).Text;
                string nota = ((Label)row.FindControl("lblNota")).Text;
                int lectcodi = this.LectCodi;

                string sql = string.Format(this.sqlObtenerRegistro, lectcodi, fecha, idTipoInformacion, idPtoMedicion);

                if (ds.Tables["ModificarRegistro"] != null) ds.Tables["ModificarRegistro"].Clear();
                this.dao.Fill(0, ds, "ModificarRegistro", sql);

                if (ds.Tables["ModificarRegistro"] != null)
                {
                    DataRow fila = ds.Tables["ModificarRegistro"].Rows[0];
                    string idEmpresa = fila["EMPRCODI"].ToString();

                    ListItem liEmpresa = this.ddlEmpresa.Items.FindByValue(idEmpresa);
                    if (liEmpresa != null) this.ddlEmpresa.SelectedValue = idEmpresa;
                    this.CargarCentrales();

                    ListItem liCental = this.ddlCentral.Items.FindByValue(idPtoMedicion);
                    if (liCental != null) this.ddlCentral.SelectedValue = idPtoMedicion;
                    this.CargarTipoCombustible();

                    ListItem liTipoInfo = this.ddlTipoInformacion.Items.FindByValue(idTipoInformacion);
                    if (liTipoInfo != null) this.ddlTipoInformacion.SelectedValue = idTipoInformacion;

                    this.txtH1.Text = Convert.ToDecimal(fila["H1"]).ToString("N", CultureInfo.InvariantCulture);
                    this.txtFecha.Text = row.Cells[2].Text;
                    this.textNota.Text = nota;

                    this.ddlEmpresa.Enabled = false;
                    this.ddlCentral.Enabled = false;
                    this.txtFecha.Enabled = false;
                    this.ddlTipoInformacion.Enabled = false;
                    this.hfEdicion.Value = "S";
                    this.txtFecha.Visible = true;
                    this.tablaFecha.Visible = false;

                    this.mpeRegistro.Show();

                }

            }
            catch (Exception ex)
            {
                this.lblMsg.Text = ex.Message;
            }
        }

        /// <summary>
        /// Permite exportar el resultado a excel
        /// </summary>
        protected void Exportar(DataTable dataTable)
        {
            string ruta = ConfigurationManager.AppSettings["direxcel"].ToString();
            //FileInfo template = new FileInfo(Server.MapPath("ReporteExcel/Plantilla.xlsx"));
            //FileInfo newFile = new FileInfo(Server.MapPath("ReporteExcel/Reporte.xlsx"));

            FileInfo template = new FileInfo(ruta + "Plantilla.xlsx");
            FileInfo newFile = new FileInfo(ruta + "Reporte.xlsx");


            if (newFile.Exists)
            {
                newFile.Delete();
                //newFile = new FileInfo(Server.MapPath("ReporteExcel/Reporte.xlsx"));
                newFile = new FileInfo(ruta + "Reporte.xlsx");
            }

            DataTable tb = dataTable;
            DataView dv = new DataView(tb);
            dv.RowFilter = this.sqlNogas;
            DataTable tbStock = dv.ToTable();

            DataTable tbD = dataTable;
            DataView dvD = new DataView(tbD);
            dvD.RowFilter = this.sqlGas;
            DataTable tbDisponilidad = dvD.ToTable();

            int index = 0;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Reporte"];

                if (ws != null)
                {
                    index = 7;
                    string tipoInfo = string.Empty;
                    string unidad = string.Empty;
                    string valor = string.Empty;

                    foreach (DataRow row in tbStock.Rows)
                    {
                        this.ObtenerTextos(row["TIPOINFODESC"].ToString(), out tipoInfo, out unidad);

                        valor = "No informó.";
                        if (row["H1"] != null)
                        {
                            if (!string.IsNullOrEmpty(row["H1"].ToString()))
                            {
                                valor = row["H1"].ToString();
                            }
                        }

                        ws.Cells[index, 1].Value = Convert.ToDateTime(row["MEDIFECHA"]).ToString("dd/MM/yyyy");
                        ws.Cells[index, 2].Value = row["EMPRNOMB"].ToString().Trim();
                        ws.Cells[index, 3].Value = row["GRUPONOMB"].ToString().Trim();
                        ws.Cells[index, 4].Value = tipoInfo;
                        ws.Cells[index, 5].Value = valor;
                        ws.Cells[index, 6].Value = unidad;
                        ws.Cells[index, 7].Value = ((!string.IsNullOrEmpty(row["NOTA"].ToString())) ? row["NOTA"].ToString().Trim() : "");


                        index++;
                    }

                    index = 7;
                    foreach (DataRow row in tbDisponilidad.Rows)
                    {
                        this.ObtenerTextos(row["TIPOINFODESC"].ToString(), out tipoInfo, out unidad);

                        valor = "No informó.";
                        if (row["H1"] != null)
                        {
                            if (!string.IsNullOrEmpty(row["H1"].ToString()))
                            {
                                valor = row["H1"].ToString();
                            }
                        }

                        ws.Cells[index, 10].Value = Convert.ToDateTime(row["MEDIFECHA"]).ToString("dd/MM/yyyy");
                        ws.Cells[index, 11].Value = row["EMPRNOMB"].ToString().Trim();
                        ws.Cells[index, 12].Value = row["GRUPONOMB"].ToString().Trim();
                        ws.Cells[index, 13].Value = tipoInfo;
                        ws.Cells[index, 14].Value = valor;
                        ws.Cells[index, 15].Value = "";
                        ws.Cells[index, 16].Value = unidad;
                        ws.Cells[index, 17].Value = ((!string.IsNullOrEmpty(row["NOTA"].ToString())) ? row["NOTA"].ToString().Trim() : "");

                        index++;
                    }
                }


                int iNoGas = tbStock.Rows.Count;
                int iGas = tbDisponilidad.Rows.Count;
                int mayor = (iNoGas > iGas) ? iNoGas : iGas;
                int menor = (iNoGas < iGas) ? iNoGas : iGas;
                int grupo = (iNoGas < iGas) ? 1 : 2;

                int iDelete = 7 + mayor;

                for (int k = 150; k >= iDelete; k--)
                {
                    ws.DeleteRow(k);
                }

                for (int t = menor + 7; t <= mayor + 7; t++)
                {
                    //if (grupo == 1)
                    //{
                    //    ws.Cells[t, 1].Style = string.Empty;
                    //    ws.Cells[t, 2].Style = string.Empty;
                    //    ws.Cells[t, 3].Style = string.Empty;
                    //    ws.Cells[t, 4].Style = string.Empty;
                    //    ws.Cells[t, 5].Style = string.Empty;
                    //    ws.Cells[t, 6].Style = string.Empty;
                    //}
                    //if (grupo == 2)
                    //{
                    //    ws.Cells[t, 9].Style = string.Empty;
                    //    ws.Cells[t, 10].Style = string.Empty;
                    //    ws.Cells[t, 11].Style = string.Empty;
                    //    ws.Cells[t, 12].Style = string.Empty;
                    //    ws.Cells[t, 13].Style = string.Empty;
                    //    ws.Cells[t, 14].Style = string.Empty;
                    //    ws.Cells[t, 15].Style = string.Empty;
                    //}
                }

                xlPackage.Save();
            }

            string filePath = ruta + "Reporte.xlsx";
            //string filePath = Server.MapPath("ReporteExcel/Reporte.xlsx");
            string filename = Path.GetFileName(filePath);
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "inline;filename=Reporte.xlsx");
            Response.ContentType = "application/vnd.ms-excel";
            Response.WriteFile(filePath);
            fs.Close();
            Response.End();
        }

        /// <summary>
        /// Permite validar una fecha para el ingreso
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="mensaje"></param>
        protected bool ValidarFecha(out string mensaje)
        {
            DateTime fecha = DateTime.ParseExact(this.txtLimInferior.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String fec = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fechaActual = DateTime.ParseExact(fec, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            TimeSpan ts = fecha.Subtract(fechaActual);
            mensaje = string.Empty;

            //if (ts.Days > 0)
            if (ts.Days > 1)
            {
                return true;
            }
            //else if (ts.Days < 0)
            else if (ts.Days < 1)
            {
                mensaje = "Ha vencido el plazo de registro.";
                return false;
            }
            else
            {
                if (DateTime.Now.Hour * 60 + DateTime.Now.Minute > 545)
                {
                    mensaje = "Ha vencido el plazo de registro.";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Permite validar una fecha para el ingreso
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="mensaje"></param>
        protected bool ValidarFechaEdicion(out string mensaje, string fechaRegistro)
        {
            DateTime fecha = DateTime.ParseExact(fechaRegistro, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String fec = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fechaActual = DateTime.ParseExact(fec, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            TimeSpan ts = fecha.Subtract(fechaActual);
            mensaje = string.Empty;

            if (ts.Days > 1)
            //if (ts.Days > 0)
            {
                return true;
            }
            //else if (ts.Days < 0)
            else if (ts.Days < 1)
            {
                mensaje = "Ha vencido el plazo de registro.";
                return false;
            }
            else
            {
                if (DateTime.Now.Hour * 60 + DateTime.Now.Minute > 545)
                {
                    mensaje = "Ha vencido el plazo de registro.";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Permite obtener el tipo de información y la unidad
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="tipoInformacion"></param>
        /// <param name="unidad"></param>
        protected void ObtenerTextos(string texto, out string tipoInformacion, out string unidad)
        {
            int iIni = texto.IndexOf("(");
            int iFin = texto.IndexOf(")");
            tipoInformacion = texto.Substring(0, iIni - 1);
            unidad = texto.Substring(iIni + 1, iFin - iIni - 1);

        }


        #region Metodos y Eventos para Configuracion

        /// <summary>
        /// Permite mostrar la vista de mantenimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbMantenimiento_Click(object sender, EventArgs e)
        {
            this.MostrarConfiguracion();
        }

        /// <summary>
        /// Muestra la pantalla de configuración de centrales y fuentes de energía
        /// </summary>
        protected void MostrarConfiguracion()
        {
            if (ds.Tables["EmpresaCongiguracion"] != null) ds.Tables["EmpresaConfiguracion"].Clear();
            this.dao.Fill(0, ds, "EmpresaConfiguracion", this.sqlEmpresas);
            this.ddlEmpresaConfiguracion.DataSource = ds.Tables["EmpresaConfiguracion"];
            this.ddlEmpresaConfiguracion.DataBind();
            this.ddlEmpresaConfiguracion.Items.Insert(0, new ListItem("--SELECCIONE--", string.Empty));

            this.CargarConfiguraciones();

            this.mvGeneral.SetActiveView(this.vMantenimiento);
        }

        /// <summary>
        /// Permite agregar una configuracion de central vs fuente de generación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfiguracion_Click(object sender, EventArgs e)
        {
            this.ddlEmpresaConfiguracion.SelectedIndex = -1;
            this.ddlCentralConfiguracion.Items.Clear();
            this.ddlCombustibleConfiguracion.SelectedIndex = -1;
            this.ddlEstadoConfiguracion.SelectedIndex = 1;

            this.ddlEmpresaConfiguracion.Enabled = true;
            this.ddlCentralConfiguracion.Enabled = true;
            this.ddlCombustibleConfiguracion.Enabled = true;

            this.lblMensajeConfiguracion.Text = "Ingrese los datos.";
            this.pnlMensajeConfiguracion.CssClass = "content-message";

            this.hfIdEmpresaConfig.Value = string.Empty;
            this.hfIdCentalConfig.Value = string.Empty;
            this.hfIdTipoInfoConfig.Value = string.Empty;

            this.mpeConfiguracion.Show();
        }

        /// <summary>
        /// Carga las relaciones entre centrales y fuentes de generacion
        /// </summary>
        protected void CargarConfiguraciones()
        {
            if (ds.Tables["TablaConfiguracion"] != null) ds.Tables["TablaConfiguracion"].Clear();
            this.dao.Fill(0, ds, "TablaConfiguracion", this.sqlListarConfiguracion);

            this.gvConfiguracion.DataSource = ds.Tables["TablaConfiguracion"];
            this.gvConfiguracion.DataBind();
        }

        /// <summary>
        /// Maneja el paginado de la grilla de configuraciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguracion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvConfiguracion.PageIndex = e.NewPageIndex;
            this.CargarConfiguraciones();
        }

        /// <summary>
        /// Permite ejecutar las acciones sobre 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguracion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = this.gvConfiguracion.Rows[index];
            int idEmpresa = int.Parse(((Label)row.FindControl("lblEmpresa")).Text);
            int idGrupo = int.Parse(((Label)row.FindControl("lblGrupo")).Text);
            int idTipoInformacion = int.Parse(((Label)row.FindControl("lblTipoInformacion")).Text);
            this.lblMensajeConfiguracion.Text = string.Empty;

            try
            {
                if (e.CommandName == "Eliminar")
                {
                    string sqlId = string.Format(this.sqlValidarEliminar, idGrupo, this.LectCodi, idTipoInformacion);
                    int count = Convert.ToInt32(dao.iL_data[0].nf_ExecuteScalar(sqlId));

                    if (count == 0)
                    {
                        string msg = string.Empty;
                        string sql = string.Format(this.sqlDeleteConfiguracion, idEmpresa, idGrupo, idTipoInformacion);
                        this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out msg);
                        this.CargarConfiguraciones();
                    }
                    else
                    {
                        this.lblMsgConfiguracion.Text = "No se puede eliminar el registro, existen registros de disponibilidad de stocks asociados, por favor inactive el registro.";
                    }
                }
                if (e.CommandName == "Modificar")
                {
                    ListItem liEmpresa = this.ddlEmpresaConfiguracion.Items.FindByValue(idEmpresa.ToString());
                    if (liEmpresa != null)
                    {
                        this.ddlEmpresaConfiguracion.SelectedValue = idEmpresa.ToString();
                        this.CargarCentralesConfiguracion();
                    }

                    ListItem liGrupo = this.ddlCentralConfiguracion.Items.FindByValue(idGrupo.ToString());
                    if (liGrupo != null)
                    {
                        this.ddlCentralConfiguracion.SelectedValue = idGrupo.ToString();
                    }

                    ListItem liTipoInformacion = this.ddlCombustibleConfiguracion.Items.FindByValue(idTipoInformacion.ToString());
                    if (liTipoInformacion != null)
                    {
                        this.ddlCombustibleConfiguracion.SelectedValue = idTipoInformacion.ToString();
                    }

                    this.ddlEmpresaConfiguracion.Enabled = false;
                    this.ddlCentralConfiguracion.Enabled = false;
                    this.ddlCombustibleConfiguracion.Enabled = false;

                    string sql = string.Format(this.sqlGetConfiguracion, idEmpresa, idGrupo, idTipoInformacion);

                    if (ds.Tables["GetConfiguracion"] != null) ds.Tables["GetConfiguracion"].Clear();
                    this.dao.Fill(0, ds, "GetConfiguracion", sql);

                    if (ds.Tables["GetConfiguracion"] != null)
                    {
                        string estado = ds.Tables["GetConfiguracion"].Rows[0]["estado"].ToString();
                        ListItem liEstado = this.ddlEstadoConfiguracion.Items.FindByValue(estado);
                        this.ddlEstadoConfiguracion.SelectedValue = estado;
                    }

                    this.lblMensajeConfiguracion.Text = "Por favor modifique los datos.";
                    this.pnlMensajeConfiguracion.CssClass = "content-message";

                    this.hfIdEmpresaConfig.Value = idEmpresa.ToString();
                    this.hfIdCentalConfig.Value = idGrupo.ToString();
                    this.hfIdTipoInfoConfig.Value = idTipoInformacion.ToString();

                    this.mpeConfiguracion.Show();
                }
            }
            catch (Exception)
            {
                this.lblMsgConfiguracion.Text = "Ha ocurrido un error.";
                //this.pnlMensajeConfiguracion.CssClass = "content-error";
            }
        }

        /// <summary>
        /// Agrega funcionalidad javascript en la grilla de configuraciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguracion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lbQuitar");
                lb.Attributes.Add("onclick", "return confirm('¿Está seguro de realizar esta operación?')");
            }
        }

        /// <summary>
        /// Permite agregar una nueva configuracion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGrabarConfiguracion_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;

                if (this.ValidarConfiguracion(out msg))
                {
                    int idEmpresa = int.Parse(this.ddlEmpresaConfiguracion.SelectedValue);
                    int idGrupo = int.Parse(this.ddlCentralConfiguracion.SelectedValue);
                    int idTipoInformacion = int.Parse(this.ddlCombustibleConfiguracion.SelectedValue);
                    string estado = this.ddlEstadoConfiguracion.SelectedValue;

                    string fecha = EPDate.SQLDateOracleString(DateTime.Now);
                    string user = this.dao.is_UserLogin;

                    string sqlId = string.Format(this.sqlCountConfiguracion, idEmpresa, idGrupo, idTipoInformacion);
                    int count = Convert.ToInt32(dao.iL_data[0].nf_ExecuteScalar(sqlId));

                    if (string.IsNullOrEmpty(this.hfIdEmpresaConfig.Value) && string.IsNullOrEmpty(this.hfIdCentalConfig.Value) &&
                        string.IsNullOrEmpty(this.hfIdTipoInfoConfig.Value))
                    {
                        if (count == 0)
                        {
                            string mensaje = string.Empty;
                            string sql = string.Format(this.sqlInsertaConfiguracion, idEmpresa, idGrupo, idTipoInformacion, estado, user, fecha);
                            this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out mensaje);
                            this.lblMensajeConfiguracion.Text = mensaje;
                            this.mpeConfiguracion.Hide();
                            this.CargarConfiguraciones();
                        }
                        else
                        {
                            this.lblMensajeConfiguracion.Text = "El registro ya exite.";
                            this.pnlMensajeConfiguracion.CssClass = "content-alert";
                            this.mpeConfiguracion.Show();
                        }
                    }
                    else
                    {
                        string mensaje = string.Empty;
                        string sql = string.Format(this.sqlUpdateConfiguracion, idEmpresa, idGrupo, idTipoInformacion, estado, user, fecha);
                        this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out mensaje);
                        this.lblMensajeConfiguracion.Text = mensaje;
                        this.mpeConfiguracion.Hide();
                        this.CargarConfiguraciones();
                    }
                }
                else
                {
                    this.lblMensajeConfiguracion.Text = msg;
                    this.pnlMensajeConfiguracion.CssClass = "content-alert";
                    this.mpeConfiguracion.Show();
                }

            }
            catch (Exception)
            {
                this.lblMensajeConfiguracion.Text = "Ha ocurrido un error.";
                this.pnlMensajeConfiguracion.CssClass = "content-error";
                this.mpeConfiguracion.Show();
            }
        }

        /// <summary>
        /// Valida el ingreso de datos de las configuraciones
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected bool ValidarConfiguracion(out string msg)
        {
            msg = string.Empty;
            bool flag = true;

            if (string.IsNullOrEmpty(this.ddlEmpresaConfiguracion.SelectedValue)) flag = false;
            if (string.IsNullOrEmpty(this.ddlCentralConfiguracion.SelectedValue)) flag = false;
            if (string.IsNullOrEmpty(this.ddlCombustibleConfiguracion.SelectedValue)) flag = false;
            if (string.IsNullOrEmpty(this.ddlEstadoConfiguracion.SelectedValue)) flag = false;

            if (!flag) msg = "Por favor complete los datos.";

            return flag;
        }

        /// <summary>
        /// Cancela las operaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarConfiguracion_Click(object sender, EventArgs e)
        {
            this.mpeConfiguracion.Hide();
        }

        /// <summary>
        /// Muestra las centrales por cada empresa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEmpresaConfiguracion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarCentralesConfiguracion();
        }

        /// <summary>
        /// Carga las centrales por empresa en configuraciones
        /// </summary>
        protected void CargarCentralesConfiguracion()
        {
            this.ddlCentralConfiguracion.Items.Clear();

            if (!string.IsNullOrEmpty(this.ddlEmpresaConfiguracion.SelectedValue))
            {
                decimal idEmpresa = decimal.Parse(this.ddlEmpresaConfiguracion.SelectedValue);
                if (ds.Tables["GrupoConfiguracion"] != null) ds.Tables["GrupoConfiguracion"].Clear();
                this.dao.Fill(0, ds, "GrupoConfiguracion", string.Format(this.sqlGrupo, idEmpresa));

                this.ddlCentralConfiguracion.DataSource = ds.Tables["GrupoConfiguracion"];
                this.ddlCentralConfiguracion.DataBind();
                this.ddlCentralConfiguracion.Items.Insert(0, new ListItem(this.txtSeleccione, string.Empty));
            }

            this.mpeConfiguracion.Show();
        }

        /// <summary>
        /// Muestra la pantalla principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelConfiguracion_Click(object sender, EventArgs e)
        {
            this.CargarDatos();
            this.mvGeneral.SetActiveView(this.vRegistro);
        }

        #endregion



        #region Metodos agregados

        /// <summary>
        /// Abre el popup para el copiado de los datos de combustibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopiar_Click(object sender, EventArgs e)
        {
            this.ddlEmpresaCopiar.SelectedIndex = -1;
            this.ddlCentralCopiar.SelectedIndex = -1;
            this.ddlTipoInformacion.SelectedIndex = -1;
            this.txtFechaCopiar.Text = (!string.IsNullOrEmpty(this.txtFechaInicio.Text)) ?
                this.txtFechaInicio.Text : DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");

            this.ddlEmpresaCopiar.Enabled = true;
            this.ddlCentralCopiar.Enabled = true;
            this.txtFechaCopiar.Enabled = true;
            this.txtFechaCopiarDesde.Text = string.Empty;
            this.txtFechaCopiarHasta.Text = string.Empty;
            this.btnGrabarCopiar.Enabled = true;

            this.lblMensajeCopiar.Text = "Ingrese los datos requeridos.";
            this.pnlMensajeCopiar.CssClass = "content-message";

            if (this.ddlEmpresa.Items.Count == 2) this.ddlEmpresa.SelectedIndex = 1;

            this.pnlConfirmarCopiar.Visible = false;
            this.pnlCopiarAccion.Visible = true;
            this.ddlCentralCopiar.Items.Clear();

            this.mpeCopiar.Show();
        }

        /// <summary>
        /// Ejecuta la operación de grabado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGrabarCopiar_Click(object sender, EventArgs e)
        {
            string validacion = this.ValidarCopiado();
            if (validacion == "OK")
            {
                this.GrabarCopia();
            }
            else
            {
                this.lblMensajeCopiar.Text = validacion;
                this.pnlMensajeCopiar.CssClass = "content-alert";
                this.mpeCopiar.Show();
            }
        }

        /// <summary>
        /// Valida los datos de entrada del copiado de datos
        /// </summary>
        /// <returns></returns>
        protected string ValidarCopiado()
        {
            StringBuilder str = new StringBuilder();
            DateTime fecValida = DateTime.Now;
            string mensaje = string.Empty;
            bool flag = true;
            if (this.ddlEmpresaCopiar.SelectedIndex == 0)
            {
                str.Append("<li>Seleccione empresa.</li>");
                flag = false;
            }

            if (this.ddlCentralCopiar.SelectedIndex == 0)
            {
                str.Append("<li>Seleccione la central.</li>");
                flag = false;
            }

            if (string.IsNullOrEmpty(this.txtFechaCopiar.Text))
            {
                str.Append("<li>Seleccione una fecha de donde se copiarán los datos.</li>");
                flag = false;
            }
            else
            {
                if (!DateTime.TryParseExact(this.txtFechaCopiar.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out fecValida))
                {
                    str.Append("<li>El formato de la fecha de origen no es correcto.</li>");
                    flag = false;
                }
            }

            if (string.IsNullOrEmpty(this.txtFechaCopiarDesde.Text))
            {
                str.Append("<li>Seleccione una fecha a donde se copiarán los datos.</li>");
                flag = false;
            }
            else
            {
                if (!DateTime.TryParseExact(this.txtFechaCopiarDesde.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                   DateTimeStyles.None, out fecValida))
                {
                    str.Append("<li>La fecha de inicio no tiene el formato correcto.</li>");
                    flag = false;
                }
            }

            if (!string.IsNullOrEmpty(this.txtFechaCopiarDesde.Text) && !string.IsNullOrEmpty(this.txtFechaCopiarHasta.Text))
            {
                DateTime fechaInicio = DateTime.ParseExact(this.txtFechaCopiarDesde.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (!DateTime.TryParseExact(this.txtFechaCopiarHasta.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out fecValida))
                {
                    str.Append("<li>La fecha final no tiene el formato correcto.</li>");
                    flag = false;
                }
                else
                {
                    DateTime fechaFin = DateTime.ParseExact(this.txtFechaCopiarHasta.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (fechaFin.Subtract(fechaInicio).TotalDays < 0)
                    {
                        str.Append("<li>La fecha final debe ser mayor a la fecha inicial.</li>");
                        flag = false;
                    }
                }
            }

            if (flag) mensaje = "OK";
            else mensaje = str.ToString();

            return mensaje;
        }

        /// <summary>
        /// Permite grabar los registros de copia
        /// </summary>
        protected void GrabarCopia()
        {
            try
            {
                List<ItemCombustible> list = new List<ItemCombustible>();
                decimal idEmpresa = decimal.Parse(this.ddlEmpresaCopiar.SelectedValue);
                int idCentral = int.Parse(this.ddlCentralCopiar.SelectedValue);
                string fechaCopia = EPDate.SQLDateOracleString(DateTime.ParseExact(this.txtFechaCopiar.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                DateTime fechaInicio = DateTime.ParseExact(this.txtFechaCopiarDesde.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(this.txtFechaCopiarHasta.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                int nroDias = Convert.ToInt32(fechaFin.Subtract(fechaInicio).TotalDays);
                decimal contador = 0;
                string query = string.Empty;
                string msgFecha = string.Empty;

                if (this.ValidarFechaCopia(out msgFecha, fechaInicio))
                {
                    query = string.Format(this.sqlListarCopiar, idCentral, fechaCopia, this.LectCodi);
                    if (ds.Tables[this.tbCopia] != null) ds.Tables.Remove(this.tbCopia);
                    this.dao.Fill(0, ds, this.tbCopia, query);

                    if (ds.Tables[this.tbCopia].Rows.Count > 0)
                    {
                        for (int i = 0; i <= nroDias; i++)
                        {
                            DateTime fecha = fechaInicio.AddDays(i);
                            string fechaVerificar = EPDate.SQLDateOracleString(fecha);

                            foreach (DataRow row in ds.Tables[this.tbCopia].Rows)
                            {
                                int idTipoInformacion = (!row.IsNull("tipoinfocodi")) ? Convert.ToInt32(row["tipoinfocodi"]) : 0;
                                string sqlId = string.Format(this.sqlExisteRow, this.LectCodi, fechaVerificar, idTipoInformacion, idCentral);
                                contador = Convert.ToDecimal(dao.iL_data[0].nf_ExecuteScalar(sqlId));

                                if (contador == 0)
                                {
                                    //decimal h1 = (!row.IsNull("h1")) ? Convert.ToDecimal(row["h1"]) : 0;
                                    //string mensaje = string.Empty;
                                    //string fechaUpdate = this.ObtenerFormatoFecha(DateTime.Now);
                                    //string sql = string.Format(this.sqlRegistro, this.LectCodi, fechaVerificar, idTipoInformacion, idCentral, h1, dao.is_UserLogin, fechaUpdate);
                                    //this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out mensaje);
                                }
                                else
                                {
                                    ItemCombustible entity = new ItemCombustible();
                                    entity.Descripcion = fecha.ToString("dd/MM/yyyy");
                                    list.Add(entity);
                                }
                            }
                        }

                        if (list.Count == 0)
                        {
                            for (int i = 0; i <= nroDias; i++)
                            {
                                DateTime fecha = fechaInicio.AddDays(i);
                                string fechaVerificar = EPDate.SQLDateOracleString(fecha);

                                foreach (DataRow row in ds.Tables[this.tbCopia].Rows)
                                {
                                    int idTipoInformacion = (!row.IsNull("tipoinfocodi")) ? Convert.ToInt32(row["tipoinfocodi"]) : 0;
                                    string sqlId = string.Format(this.sqlExisteRow, this.LectCodi, fechaVerificar, idTipoInformacion, idCentral);
                                    contador = Convert.ToDecimal(dao.iL_data[0].nf_ExecuteScalar(sqlId));

                                    if (contador == 0)
                                    {
                                        decimal h1 = (!row.IsNull("h1")) ? Convert.ToDecimal(row["h1"]) : 0;
                                        string mensaje = string.Empty;
                                        string fechaUpdate = this.ObtenerFormatoFecha(DateTime.Now);
                                        string sql = string.Format(this.sqlRegistro, this.LectCodi, fechaVerificar, idTipoInformacion, idCentral, h1, dao.is_UserLogin, fechaUpdate);
                                        this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out mensaje);
                                    }

                                }
                            }
                            this.txtFechaFin.Text = (!string.IsNullOrEmpty(this.txtFechaCopiarHasta.Text)) ?
                                   this.txtFechaCopiarHasta.Text : this.txtFechaCopiarDesde.Text;
                            this.CargarDataGrid();
                            this.mpeCopiar.Hide();
                        }
                        else
                        {
                            this.lblMensajeCopiar.Text = "El registro ya existe en las fechas indicadas.";
                            this.pnlMensajeCopiar.CssClass = "content-alert";

                            this.gvRepetidosCopiar.DataSource = list;
                            this.gvRepetidosCopiar.DataBind();
                            this.pnlConfirmarCopiar.Visible = true;
                            this.pnlCopiarAccion.Visible = false;

                            if (list.Count == nroDias)
                            {
                                this.divMsgConfirmarCopiar.Visible = false;
                                this.btnConfirmarCopíar.Enabled = false;
                            }
                            else
                            {
                                this.divMsgConfirmarCopiar.Visible = true;
                                this.btnConfirmarCopíar.Enabled = true;
                            }

                            this.mpeCopiar.Show();
                        }
                    }
                    else
                    {
                        this.lblMensajeCopiar.Text = "No existen datos para la fecha seleccionada.";
                        this.pnlMensajeCopiar.CssClass = "content-alert";
                        this.mpeCopiar.Show();
                    }
                }
                else
                {
                    this.lblMensajeCopiar.Text = msgFecha;
                    this.pnlMensajeCopiar.CssClass = "content-alert";
                    this.mpeCopiar.Show();
                }

            }
            catch (Exception ex)
            {
                this.lblMensajeCopiar.Text = ex.Message;
                this.pnlMensajeCopiar.CssClass = "content-error";
                this.mpeCopiar.Show();
            }
        }

        /// <summary>
        /// Graba los datos del copiado para los dias en que no existen datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmarCopíar_Click(object sender, EventArgs e)
        {
            int idCentral = int.Parse(this.ddlCentralCopiar.SelectedValue);
            string fechaCopia = EPDate.SQLDateOracleString(DateTime.ParseExact(this.txtFechaCopiar.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            DateTime fechaInicio = DateTime.ParseExact(this.txtFechaCopiarDesde.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(this.txtFechaCopiarHasta.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int nroDias = Convert.ToInt32(fechaFin.Subtract(fechaInicio).TotalDays);
            decimal contador = 0;

            string query = string.Format(this.sqlListarCopiar, idCentral, fechaCopia, this.LectCodi);
            if (ds.Tables[this.tbCopia] != null) ds.Tables.Remove(this.tbCopia);
            this.dao.Fill(0, ds, this.tbCopia, query);

            if (ds.Tables[this.tbCopia].Rows.Count > 0)
            {
                for (int i = 0; i <= nroDias; i++)
                {
                    DateTime fecha = fechaInicio.AddDays(i);
                    string fechaVerificar = EPDate.SQLDateOracleString(fecha);

                    foreach (DataRow row in ds.Tables[this.tbCopia].Rows)
                    {
                        int idTipoInformacion = (!row.IsNull("tipoinfocodi")) ? Convert.ToInt32(row["tipoinfocodi"]) : 0;
                        string sqlId = string.Format(this.sqlExisteRow, this.LectCodi, fechaVerificar, idTipoInformacion, idCentral);
                        contador = Convert.ToDecimal(dao.iL_data[0].nf_ExecuteScalar(sqlId));

                        if (contador == 0)
                        {
                            decimal h1 = (!row.IsNull("h1")) ? Convert.ToDecimal(row["h1"]) : 0;
                            string mensaje = string.Empty;
                            string fechaUpdate = this.ObtenerFormatoFecha(DateTime.Now);
                            string sql = string.Format(this.sqlRegistro, this.LectCodi, fechaVerificar, idTipoInformacion, idCentral, h1, dao.is_UserLogin, fechaUpdate);
                            this.dao.iL_data[0].nf_ExecuteNonQueryWithMessage(sql, out mensaje);
                        }

                    }
                }
            }

            this.txtFechaFin.Text = (!string.IsNullOrEmpty(this.txtFechaCopiarHasta.Text)) ?
                                  this.txtFechaCopiarHasta.Text : this.txtFechaCopiarDesde.Text;
            this.CargarDataGrid();
            this.mpeCopiar.Hide();
        }

        /// <summary>
        /// Cancela la ventana de continuación de copiado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarConfirmarCopiar_Click(object sender, EventArgs e)
        {
            this.pnlConfirmarCopiar.Visible = false;
            this.pnlCopiarAccion.Visible = true;
            this.mpeCopiar.Show();
        }

        /// <summary>
        /// Valida las fechas para la copia de datos
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        protected bool ValidarFechaCopia(out string mensaje, DateTime fecha)
        {
            String fec = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fechaActual = DateTime.ParseExact(fec, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            TimeSpan ts = fecha.Subtract(fechaActual);
            mensaje = string.Empty;

            if (ts.Days > 1)
            //if (ts.Days > 0)
            {
                return true;
            }
            else if (ts.Days < 1)
            //else if (ts.Days < 0)
            {
                mensaje = "Ha vencido el plazo de registro.";
                return false;
            }
            else
            {
                if (DateTime.Now.Hour * 60 + DateTime.Now.Minute > 545)
                {
                    mensaje = "Ha vencido el plazo de registro.";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Cancela la operación de copiado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCopiar_Click(object sender, EventArgs e)
        {
            this.mpeCopiar.Hide();
        }

        /// <summary>
        /// Controla el índice de selección de la empresa en la operación de copiado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEmpresaCopiar_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlCentralCopiar.Items.Clear();
            if (!string.IsNullOrEmpty(this.ddlEmpresaCopiar.SelectedValue))
            {
                decimal idEmpresa = decimal.Parse(this.ddlEmpresaCopiar.SelectedValue);
                if (ds.Tables[this.tbGrupo] != null) ds.Tables[this.tbGrupo].Clear();
                this.dao.Fill(0, ds, this.tbGrupo, string.Format(this.sqlGrupoRegistro, idEmpresa));

                this.ddlCentralCopiar.DataSource = ds.Tables[this.tbGrupo];
                this.ddlCentralCopiar.DataBind();
                this.ddlCentralCopiar.Items.Insert(0, new ListItem("SELECCIONE", string.Empty));
            }

            this.mpeCopiar.Show();
        }



        #endregion

    }

    public class ItemCombustible
    {
        private decimal id;
        private string descripcion;

        public decimal Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
    }

}