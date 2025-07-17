using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WSIC2010.Util;

namespace WSIC2010.Uploads
{
    public partial class w_eve_formato_demanda : System.Web.UI.Page
    {
        COES.MVC.Extranet.srGestor.CMSManagerClient gcCOES;
        static string BASE_PATH = "/Company Home/Contenido Web/Formatos/Extranet";
        string ls_selected = "/Demanda/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                nf_setsource_byage();
            }
        }

        private void nf_setsource_byage()
        {
            TreeView1.Nodes.Clear();
            TreeView1.DataBind();
            TreeNode ParentNode = new TreeNode("Formatos" + "<br><span class='ruta'>" + BASE_PATH + ls_selected + "</span>");
            ParentNode.Expanded = true;
            ParentNode.SelectAction = TreeNodeSelectAction.Expand;
            ParentNode.Selected = false;
            TreeView1.Nodes.Add(ParentNode);
            gcCOES = new COES.MVC.Extranet.srGestor.CMSManagerClient();
            DataTable dt_Data = gcCOES.GetDataByPath(BASE_PATH + ls_selected);

            foreach (DataRow row in dt_Data.Rows)
            {
                TreeNode newNode = new TreeNode();

                newNode.Value = row["Id"].ToString();
                newNode.Text = !String.IsNullOrEmpty(row["Title"].ToString()) ? row["Title"].ToString() : row["Name"].ToString();

                newNode.SelectAction = TreeNodeSelectAction.Expand;
                newNode.PopulateOnDemand = true;

                newNode.ToolTip = "Ruta: " + BASE_PATH.Substring(13) + ls_selected + row["Name"].ToString() + "\nNombre: " + row["Name"].ToString() + "\nTítulo: " + row["Title"].ToString();

                ParentNode.ChildNodes.Add(newNode);

            }
        }

        protected void TreeView1_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            PopulateTree(e.Node);
        }

        private void PopulateTree(TreeNode aTn_parentNode)
        {
            gcCOES = new COES.MVC.Extranet.srGestor.CMSManagerClient();
            SeguridadAlfresco.Criptografia cripto = new SeguridadAlfresco.Criptografia();
            DataTable dt_Data = gcCOES.GetDataByUUID(cripto.DecryptToString(aTn_parentNode.Value));


            if (dt_Data != null && dt_Data.Rows.Count > 0)
            {
                foreach (DataRow row in dt_Data.Rows)
                {
                    TreeNode newNode = new TreeNode();
                    string ls_glosaToolTip = String.Empty;
                    newNode.Value = row["Id"].ToString();
                    newNode.Text = row["Name"].ToString();
                    newNode.SelectAction = TreeNodeSelectAction.Expand;
                    newNode.PopulateOnDemand = true;
                    ls_glosaToolTip = "Ruta: " + row["Path"].ToString().Substring(13) + "\nNombre: " + row["Name"].ToString() + "\nTítulo: " + row["Title"].ToString();

                    if (row["Created"] != null && !String.IsNullOrEmpty(row["Created"].ToString()))
                    {
                        //newNode.Text = String.Format("{0}    {1}       <b>{2}</b>", !String.IsNullOrEmpty(row["Title"].ToString()) ? row["Title"].ToString() : row["Name"].ToString(), DateTime.Parse(row["Modified"].ToString()).ToString("dd-MM-yyyy hh:mm:ss"), row["SizeContent"].ToString());
                        newNode.Text = String.Format("{0}", !String.IsNullOrEmpty(row["Title"].ToString()) ? row["Title"].ToString() : row["Name"].ToString());
                        newNode.NavigateUrl = UtilsAlfresco.GetEnlace(row["Id"].ToString());
                        newNode.Target = "_blank";
                        //ls_glosaToolTip = "Ruta: " + row["Path"].ToString().Substring(13) + "\nTamaño: " + row["SizeContent"].ToString() + "\nFecha de Modificación: " + DateTime.Parse(row["Modified"].ToString()).ToString("dd-MM-yyyy hh:mm:ss");
                        ls_glosaToolTip = "Ruta: " + row["Path"].ToString().Substring(13) + "\nTamaño: " + row["SizeContent"].ToString();
                        newNode.ImageUrl = UtilsAlfresco.GetIcon(row["Name"].ToString());
                        newNode.PopulateOnDemand = false;
                    }

                    newNode.ToolTip = ls_glosaToolTip;


                    aTn_parentNode.ChildNodes.Add(newNode);
                }
            }


        }
    }
}