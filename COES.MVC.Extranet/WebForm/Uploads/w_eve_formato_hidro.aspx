<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_eve_formato_hidro.aspx.cs" Inherits="WSIC2010.Uploads.w_eve_formato_hidro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
.tr_estad_head
{
	border-style:solid;
	border-width:thin;
	border-color:#000;
	/*border-color:#CFCFCF;*/
	/*background-color:#4B8EEE;*/
	background: #fff url("../images/gradiente.png") repeat-x top;
	color:#fff;
}
.tr_apelacion_head
{
	border-style:solid;
	border-width:thin;
	border-color:#000;
	/*border-color:#CFCFCF;*/
	/*background-color:#3399FF;*/
	background-color: #999;
	color:#fff;
}
.tr_apelacion_head th 
{
    border-style:solid;
	border-width:thin;
	border-color:#000;
}
.tr_estad_par
{
	background-color:#F5F7FC;
	/*border-style:none;*/
}
.tr_estad_par2
{
	background-color:#eee;
	/*border-style:none;*/
}
.tr_estad_impar
{
	background-color: #D5E0F0;/*#EEF2DB;*//*#CCFFFF*/
	
}
.tr_estad_impar2
{
	background-color:#FFFEFF;
	/*border-style:none;*/
}
img {
 border:0px;
} 
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label1" runat="server" 
        Text="Formato para la carga de Hidrología" 
        Font-Italic="True" Font-Names="Arial Narrow" Font-Size="Large" 
        ForeColor="#003366" style="font-weight: 700">
    </asp:Label>
    <asp:Label ID="Label2" runat="server" Text=""/>
    <asp:GridView ID="gView1" 
                  runat="server"
                  ForeColor="#000"
                  GridLines="Vertical"
                  CellPadding="5"
                  CellSpacing="5"
                  CssClass="gView"
                  AutoGenerateColumns="false">
        <HeaderStyle Font-Bold="true" CssClass="tr_apelacion_head"/>
        <AlternatingRowStyle CssClass="tr_estad_impar"/>
        <RowStyle CssClass="tr_estad_par" />
        <Columns>
                    <asp:TemplateField  HeaderText="Descargar" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderColor="White">
                        <ItemTemplate>
                            <a ID="downloadId" href='<%#WSIC2010.Util.UtilsAlfresco.GetEnlace(Eval("Id")) %>' target="_blank">
                                <img src="<%#WSIC2010.Util.UtilsAlfresco.GetIcon(Eval("Name")) %>" alt="icono" />
                            </a>
                        </ItemTemplate> 
                    </asp:TemplateField>
                    <asp:BoundField DataField="Description" HeaderText="Empresa" ItemStyle-BorderColor="White" ControlStyle-Width="30px"/>
        </Columns>
    </asp:GridView>
</asp:Content>
