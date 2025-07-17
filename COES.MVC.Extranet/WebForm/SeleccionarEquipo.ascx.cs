using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;
//using WSIC2010.WScoes_ManttoService;

namespace WSIC2010
{
    public partial class SeleccionarEquipo : System.Web.UI.UserControl
    {
        public int EQUICODI()
        {
            int li_resultado = 0;
            if (Int32.TryParse(TextBoxEquiCodi.Text, out li_resultado))
                return li_resultado;

            return li_resultado;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                if (this.hfIndicador.Value != "S")
                    this.CargarEmpresas();
            }
        }

        protected void CargarEmpresas()
        {
            n_app in_app = (n_app)Session["in_app"];
            COES.MVC.Extranet.SeguridadServicio.SeguridadServicioClient seguridad = new COES.MVC.Extranet.SeguridadServicio.SeguridadServicioClient();

            List<COES.MVC.Extranet.SeguridadServicio.EmpresaDTO> list = seguridad.ObtenerEmpresasPorUsuario(in_app.is_UserLogin).ToList();
            this.ddlEmpresa.DataSource = list;
            this.ddlEmpresa.DataBind();
            this.hfIndicador.Value = "S";


            if (list.Count > 0)
            {
                this.ddlEmpresa.SelectedIndex = 0;
                List<EqAreaDTO> listArea = (new EquipamientoAppServicio()).ObtenerAreaPorEmpresa(list[0].EMPRCODI);
                this.ddlArea.DataSource = listArea;
                this.ddlArea.DataBind();

                if (listArea.Count > 0)
                {
                    int emprCodi = list[0].EMPRCODI;
                    int areaCodi = listArea[0].Areacodi;

                    List<EqEquipoDTO> listEquipo = (new EquipamientoAppServicio()).ObtenerEquipoPorAreaEmpresa(emprCodi, areaCodi);
                    this.ddlEquipo.DataSource = listEquipo;
                    this.ddlEquipo.DataBind();

                    if (listEquipo.Count > 0)
                    {
                        TextBoxEquiCodi.Text = listEquipo[0].Equicodi.ToString();
                        LabelEquipoElegido.Text = listEquipo[0].Equinomb;
                    }
                }
            }

        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarAreas();
        }

        protected void CargarAreas()
        {
            this.ddlArea.Items.Clear();

            if (this.ddlEmpresa.SelectedValue!=null)
            {
                if (!string.IsNullOrEmpty(this.ddlEmpresa.SelectedValue))
                {
                    List<EqAreaDTO> list = (new EquipamientoAppServicio()).ObtenerAreaPorEmpresa(int.Parse(this.ddlEmpresa.SelectedValue));

                    this.ddlArea.DataSource = list;
                    this.ddlArea.DataBind();
                    this.ddlArea.Items.Insert(0, new ListItem("Seleccione una ubicación", ""));

                    if (list.Count > 0)
                    {
                        this.ddlArea.SelectedIndex = 1;
                        this.CargarEquipos();
                    }
                }
            }
            else
            {
                this.ddlArea.Items.Insert(0, new ListItem("Seleccione una ubicación", ""));
            }
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarEquipos();
        }

        protected void CargarEquipos()
        {
            this.ddlEquipo.Items.Clear();

            if (this.ddlArea.SelectedValue != null)
            {
                if (!string.IsNullOrEmpty(this.ddlArea.SelectedValue))
                {
                    int emprCodi = int.Parse(this.ddlEmpresa.SelectedValue);
                    int areaCodi = int.Parse(this.ddlArea.SelectedValue);

                    List<EqEquipoDTO> list = (new EquipamientoAppServicio()).ObtenerEquipoPorAreaEmpresa(emprCodi, areaCodi);
                    this.ddlEquipo.DataSource = list;
                    this.ddlEquipo.DataBind();
                    this.ddlEquipo.Items.Insert(0, new ListItem("Seleccione un equipo", ""));

                    if (list.Count > 0)
                    {
                        this.ddlEquipo.SelectedIndex = 1;
                        TextBoxEquiCodi.Text = ddlEquipo.SelectedValue;
                        LabelEquipoElegido.Text = ddlEquipo.SelectedItem.Text;
                    }
                }
            }
            else
            {
                this.ddlEquipo.Items.Insert(0, new ListItem("Seleccione un equipo", ""));
            }
        }

        protected void ddlEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxEquiCodi.Text = ddlEquipo.SelectedValue;
            LabelEquipoElegido.Text = ddlEquipo.SelectedItem.Text;
        }
    }
}