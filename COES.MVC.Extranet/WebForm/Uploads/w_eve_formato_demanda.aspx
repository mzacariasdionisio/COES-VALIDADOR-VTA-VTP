<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_eve_formato_demanda.aspx.cs" Inherits="WSIC2010.Uploads.w_eve_formato_demanda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
        .contenido
        {
            background-color:#FFFFFF;
        }
        .contenido img
        {
            /*border:0px solid #E0EEEF;
            padding:0px;
            background-color: #CCFFFF;*/
        }

        .contenido_img
        {
            border:1px solid #E0EEEF;
            padding:4px;
            background-color: #CCFFFF;
        } 
        .contenido .ulhead
        {
            list-style-position:outside;
            list-style-image: url(../images/vineta/vineta_coes_big.jpg);
            font-size:22px;
            font-weight:bold;
            /*border-bottom:solid 1px #b7ddf2;*/
            padding-bottom:5px;
            width:85%;
            color:#4447B8;
        }
        .contenido p
        {
            margin-top:0px;
            text-align:justify;
        }
        .contenido ul li
        {
            text-align:justify;
        }
        img
        { 
            border:0px;
            border-style: none;
        }
        a
        {
            text-decoration:none;
            color: #000;
        }
        td a img {
            border: 0 none;
            padding: 1px;
        }
        table tr
        {
            padding:0px;
        }
        td > div { width: 100%; height: 100%; overflow:hidden; }
        td { height: 1px; }
        .arbol
        {
            /*height: 600px;*/
            /*overflow-y:scroll;*/
        }
        div > table { padding: 5px;}
        .ruta
        {
            color: #666666;
            font-size: 10px;
            text-decoration: none;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="contenido">
    <asp:Label ID="Label1" runat="server" 
            Text="Carga de Demanda en Barras" 
            Font-Italic="True" Font-Names="Arial Narrow" Font-Size="Large" 
            ForeColor="#003366" style="font-weight: 700">
        </asp:Label>

    <div class="arbol">
        <asp:TreeView ID="TreeView1" runat="server" 
                      ExpandDepth="0" 
                      Font-Names="Arial,Verdana,Arial,Helvetica,sans-serif" 
                      Font-Size="12px" 
                      ForeColor="Black"
                      ontreenodepopulate="TreeView1_TreeNodePopulate"
                      ParentNodeStyle-Height="0"
                      NodeIndent="5">
            <LevelStyles>
                <asp:TreeNodeStyle Font-Size="16px" ForeColor="#4447B8" Font-Bold="true" NodeSpacing="0" VerticalPadding="0" />
            </LevelStyles>
            </asp:TreeView>
    </div>
</div>
</asp:Content>
