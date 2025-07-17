using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010.download
{
    public partial class download : System.Web.UI.Page
    {
        string is_qs_nodeId;
        COES.MVC.Extranet.srGestor.CMSManagerClient gcCOES;

        protected void Page_Load(object sender, EventArgs e)
        {
            is_qs_nodeId = Request.QueryString["nodeId"];
            try
            {
                if (!IsPostBack)
                {
                    if (is_qs_nodeId != null && !String.IsNullOrEmpty(is_qs_nodeId))
                    {
                        nf_get_nodeId();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<h2>Error: " + ex.Message + "</h2>");
            }
        }

        protected void nf_get_nodeId()
        {
            nf_set_nodeId(is_qs_nodeId);
        }

        protected void nf_set_nodeId(string as_qs_nodeId)
        {
            gcCOES = new COES.MVC.Extranet.srGestor.CMSManagerClient();
            COES.MVC.Extranet.srGestor.File file = new COES.MVC.Extranet.srGestor.File();
            SeguridadAlfresco.Criptografia cripto = new SeguridadAlfresco.Criptografia();
            string nodeId = cripto.DecryptToString(as_qs_nodeId);
            file = gcCOES.GetFileWithContentAndName(nodeId);
            if (file.DataAsStreamByte != null)
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = file.ContentType;
                Response.Charset = String.Empty;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.OutputStream.Write(file.DataAsStreamByte, 0, file.DataAsStreamByte.Length);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.End();
            }
            else
            {
                Response.Write("<h2>Archivo no encontrado</h2>");
            }
        }
    }
}