<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_login.Master" AutoEventWireup="true" CodeBehind="RestorePassword.aspx.cs" Inherits="WSIC2010.Account.RestorePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>ENVIAR CONTRASE&Ntilde;A</h1>
<div class="texto">
<table>
    <tr>
        <td>Ingrese login del usuario registrado</td>
        <td><asp:TextBox ID="TextBoxUserLogin" runat="server" /></td>
        <td align="right"><asp:Button ID="btn01" runat="server" Text="Enviar" onclick="btn01_Click" /></td>
    </tr>
    <tr>
        <td></td>
        <td>Ejemplo: usuario@dominio.dominio_padre</td>
    </tr>
</table>
 <div class="fila_general" style="text-align:left;margin-left:0px;padding-left:0px;">
    <asp:Label ID="labelmessage" runat="server" Text="" CssClass="lb_error"></asp:Label>
</div>
</div>
</asp:Content>
