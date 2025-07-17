<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_eve_ccocarga.aspx.cs" Inherits="WSIC2010.Mantto.w_eve_ccocarga" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
    #marco
    {
        display:block;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>CARGA DE RESULTADO DE MEDIANO PLAZO</h1>
    <br />
    <fieldset style="width:300px;margin:20px" id="marco">
    <table>
    <tr>
        <td>A&Ntilde;O</td>
        <td><asp:DropDownList ID="DDLAnio" runat="server" Width="100px"></asp:DropDownList></td>
    </tr>
    <tr>
        <td>MES</td>
        <td><asp:DropDownList ID="DDLMes" runat="server" Width="100px"></asp:DropDownList></td>
    </tr>
    <tr>
        <td colspan="2"><asp:FileUpload id="fu_carga" runat="server"></asp:FileUpload></td>
    </tr>
    <tr>
        <td colspan="2" align="right"><asp:Button ID="btn_carga" runat="server" 
                Text="Cargar" onclick="btn_carga_Click" /></td>
    </tr>
    </table>
    </fieldset>
    <asp:ListBox ID="lbox_error" runat="server" style="margin:20px;overflow:scroll" ></asp:ListBox>
</asp:Content>
