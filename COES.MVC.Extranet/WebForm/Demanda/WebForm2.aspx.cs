using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WSIC2010.Demanda
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string ls_basePath = @"d:\data\demanda\";
            //string fileName = Server.HtmlEncode(FileUpload1.FileName); /* Nombre del archivo a subir*/
            //int sizeFile = FileUpload1.PostedFile.ContentLength; /* Tamanio del archivo*/
            //string ls_extension = System.IO.Path.GetExtension(fileName); /* Extension del Archivo*/
            //string ls_path = ls_basePath + fileName;

            ////Guardar en el servidor
            //FileUpload1.SaveAs(ls_path);

            //DataSet ds = Util.ExcelReader.nf_get_excel_to_ds(ls_path, ls_extension);

            //if(ds.Tables != null)
            //    Label1.Text = ds.Tables.Count.ToString();
            //else
            //    Label1.Text = "0";
            DateTime ldt_fecha = new DateTime(2013, 10, 31);
            int li_cont = 0;

            while (li_cont <= 365)
            {
                Label1.Text += "Dia: " + ldt_fecha.ToString("dd/MM/yyyy") + " - Semana: " + EPDate.f_numerosemana(ldt_fecha).ToString();
                int li_caso = (int)ldt_fecha.DayOfWeek;
                switch (li_caso)
                {
                    case 1:
                    case 2:
                    case 6:
                    case 0:
                        Label1.Text += " Tope: Semana " + EPDate.f_numerosemana(EPDate.f_fechafinsemana(ldt_fecha).AddDays(1));
                        break;
                    case 4:
                    case 5:
                        Label1.Text += " Tope: Semana " + EPDate.f_numerosemana(EPDate.f_fechafinsemana(EPDate.f_fechafinsemana(ldt_fecha).AddDays(1)).AddDays(1));
                        break;
                    default:
                        break;
                }

                li_cont++;
                ldt_fecha = ldt_fecha.AddDays(1);

                Label1.Text += "<br />";
            }

        }
    }
}