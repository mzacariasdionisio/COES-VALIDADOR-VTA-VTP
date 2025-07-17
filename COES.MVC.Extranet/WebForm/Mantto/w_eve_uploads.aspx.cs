using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;

namespace WSIC2010.Mantto
{
    public partial class w_eve_uploads : System.Web.UI.Page
    {
        n_app in_app;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/mantto/w_eve_mantto.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {

            try
            {
                // Specify the path on the server to
                // save the uploaded file to.
                String savePath = @"d:\data\mantenimientos\documentos\";

                //string ls_user_and_empresa = in_app.is_UserLogin + in_app.Ls_emprcodi[0];
                ManttoService service = new ManttoService();
                string ls_empresa = service.GetEmpresaNombre(Convert.ToInt32(in_app.Ls_emprcodi[0]));
                string ls_user_and_empresa = in_app.is_UserLogin + "_" + ls_empresa + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss");

                if (RadioButtonTipos.SelectedItem.Value == "1")
                {
                    savePath += @"caudales\";
                    ls_user_and_empresa = "CAUDALES_" + ls_user_and_empresa;
                }
                else if (RadioButtonTipos.SelectedItem.Value == "2")
                {
                    savePath += @"combustibles\";
                    ls_user_and_empresa = "COMBUSTIBLES_" + ls_user_and_empresa;
                }
                else
                {
                    UploadStatusLabel.Text = "Seleccione una opción";
                }

                // Before attempting to save the file, verify
                // that the FileUpload control contains a file.
                if (FileUpload1.HasFile)
                {
                    // Get the name of the file to upload.
                    string fileName = Server.HtmlEncode(FileUpload1.FileName);

                    // Get the extension of the uploaded file.
                    string extension = System.IO.Path.GetExtension(fileName);

                    // Allow only files with .doc or .xls extensions
                    // to be uploaded.
                    if (extension == ".xls")
                    {
                        // Append the name of the file to upload to the path.
                        savePath += ls_user_and_empresa + "_"+ fileName;

                        // Call the SaveAs method to save the 
                        // uploaded file to the specified path.
                        // This example does not perform all
                        // the necessary error checking.               
                        // If a file with the same name
                        // already exists in the specified path,  
                        // the uploaded file overwrites it.
                        FileUpload1.SaveAs(savePath);

                        // Notify the user that their file was successfully uploaded.
                        UploadStatusLabel.Text = "Your file was uploaded successfully.";
                    }
                }
                else
                {
                    // Notify the user why their file was not uploaded.
                    UploadStatusLabel.Text = "Tu archivo NO ha podido ser cargado";
                }
            }
            catch(Exception ex)
            {
                UploadStatusLabel.Text = "ERROR: " + ex.Message;
            }
        }
    }
}