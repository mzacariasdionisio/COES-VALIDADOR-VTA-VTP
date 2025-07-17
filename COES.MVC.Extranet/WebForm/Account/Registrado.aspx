<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_login.Master" AutoEventWireup="true" CodeBehind="Registrado.aspx.cs" Inherits="WSIC2010.Account.Registrado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style11
        {
            font-size: x-large;
            font-weight: bold;
        }
        .style21
        {
            font-size: medium;
        }
        .nombre
        {
            /*font-size:large;*/
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>        
        <br />
        <span class="style11">SICOES </span>
        <br />
        <%--<span class="style2">Bienvenido al Sistema de Gestión Operativa del COES</span> --%>
    </div>
    <p>
        <asp:Label ID="labelUsuario" runat="server" Text="" CssClass="nombre"></asp:Label> usted ya es un usuario registrado!
    </p>
</asp:Content>
