<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_adm_cambiarPassword.aspx.cs" Inherits="WSIC2010.Admin.w_adm_cambiarPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .texto
    {
        margin: 20px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>CAMBIAR CONTRASE&Ntilde;A</h1>
<div class="texto">
<table>
<tr>
    <td>Ingrese contrase&ntilde;a actual</td>
    <td><asp:TextBox ID="tbox1" runat="server" TextMode="Password" /></td>
    <td></td>
</tr>
<tr>
    <td>Ingrese nueva contrase&ntilde;a</td>
    <td><asp:TextBox ID="tbox2" runat="server" TextMode="Password" MaxLength="6"/></td>
    <td> (Ingrese contrase&ntilde;a de 6 caracteres)</td>
</tr>
<tr>
    <td>Confirmar nueva contrase&ntilde;a</td>
    <td><asp:TextBox ID="tbox3" runat="server" TextMode="Password" MaxLength="6"/></td>
    <td> (Reingrese contrase&ntilde;a)</td>
</tr>
<tr>
    <td></td>
    <td align="right"><asp:Button ID="btn01" runat="server" Text="aceptar" onclick="btn01_Click" /></td>
</tr>
<tr>
    <td colspan="2"><asp:Label ID="lbl01" runat="server" Text=""/></td>
</tr>
</table>
</div>
</asp:Content>
