<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_dem_cargahistorica.aspx.cs" Inherits="WSIC2010.Demanda.w_dem_cargahistorica" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
    #marco
    {
        display:block;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>CARGA DE DEMANDA HISTÓRICA</h1>
    <br />
    <fieldset style="width:300px;margin:20px" id="marco">
    <table>
        <tr>
        <td colspan="2"><asp:FileUpload id="fu_carga" runat="server" Enabled="false"></asp:FileUpload></td>
    </tr>
    <tr>
        <td colspan="2" align="right"><asp:Button ID="btn_carga" runat="server" Enabled="false"
                Text="Cargar" onclick="btn_carga_Click" /></td>
    </tr>
    </table>
    
    </fieldset>
    <asp:ListBox ID="lbox_error" runat="server" style="margin:20px;overflow:scroll" ></asp:ListBox>
</asp:Content>
