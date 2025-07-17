<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_login.Master" AutoEventWireup="true" CodeBehind="Confirmacion.aspx.cs" Inherits="WSIC2010.Account.Confirmacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/marco_root.css" type="text/css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <label class="titulo_pagina">Mensaje enviado</label>
    <div class="accountInfo">
        <fieldset class="register">
            <legend>Datos del Nuevo usuario</legend>
            <div class="fila_general">
                <asp:Label ID="UserNameLabel" runat="server">Nombre:</asp:Label><br />
                <asp:Label ID="Nombre" runat="server" CssClass="textEntry2"></asp:Label>
            </div>
            <div class="fila_general">
                <asp:Label ID="UserSurNameLabel" runat="server">Apellido:</asp:Label><br />
                <asp:Label ID="Apellido" runat="server" CssClass="textEntry2"></asp:Label>
            </div>
            <div class="fila_general">
                <asp:Label ID="EmpresaLabel" runat="server">Empresa:</asp:Label><br />
                <asp:Label ID="Empresa" runat="server" CssClass="textEntry2"></asp:Label>
            </div>
            <div class="fila_general">
                <asp:Label ID="EmailLabel" runat="server">E-mail:</asp:Label><br />
                <asp:Label ID="Email" runat="server" CssClass="textEntry2"></asp:Label>
            </div>
            <div class="fila_general">
                <asp:Label ID="PhoneLabel" runat="server">Tel&eacute;fono:</asp:Label><br />
                <asp:Label ID="Phone" runat="server" CssClass="textEntry2"></asp:Label>
            </div>
        </fieldset>
        <div class="fila">
            Una vez evaluada su solicitud por el COES, recibir&aacute; un e-mail notific&aacute;ndole sobre la situaci&oacute;n de la  misma.
        </div>
    </div>

</asp:Content>
