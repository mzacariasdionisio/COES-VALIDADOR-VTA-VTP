<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_me_ratio.aspx.cs" Inherits="WSIC2010.w_me_ratio" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="../../Styles/Custom.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>ESTAD&Iacute;STICA DE CUMPLIMIENTO</h3>
    <div class="clear"></div>
    <div>
    <div class="texto">Empresa :</div>
         <asp:DropDownList ID="ddlb_empresa" runat="server">
         </asp:DropDownList>
    </div>
    <div class="clear"></div>
    <div>
    <div class="texto">Tipo de Informaci&oacute;n:</div>
            <asp:DropDownList ID="ddlb_informacion" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlb_informacion_SelectedIndexChanged">
            </asp:DropDownList>
    </div>
    <div class="clear"></div>
    <div>
    <div class="texto">Tipo de Formato :</div>
            <asp:DropDownList ID="ddlb_formato" runat="server">
            </asp:DropDownList>
    </div>
    <div class="clear"></div>
    <div>
    <div class="texto">A&ntilde;o:</div>
            <asp:DropDownList ID="ddlb_anio" runat="server">
            </asp:DropDownList>
    </div>
    <div class="clear"></div>
    <div class="button">
        <asp:Button ID="b_consultar" runat="server" onclick="b_consultar_Click" 
            Text="Consultar" />
        <asp:Button ID="Button1" runat="server" onclick="Excel_Click" Text="Excel" />
    </div>
    <div class="clear"></div>
       

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Table ID="Table1" runat="server">
        </asp:Table>

       <hr />
       <asp:Label id="UploadStatusLabel"
           runat="server">
       </asp:Label>        
        <asp:GridView ID="gv" runat="server" 
            onrowdatabound="gv_RowDataBound">
        </asp:GridView>
        <asp:ListBox ID="ListBox1" runat="server" Height="88px" Width="232px"></asp:ListBox>
    </div>
</asp:Content>
