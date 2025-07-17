<%@ Page Title="Registro" Language="C#" MasterPageFile="~/WebForm/Master/master_login.Master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="WSIC2010.Account.Register" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .texto
        {
            text-align: justify;
        }
        .textEntry
        {
            width:400px;
            /*padding-right:20px;*/
        }
        .labelEntry
        {
            font-family:Arial, sans-serif;
	        font-size: 12px;
	        /*padding-right:20px;*/
        }
        .chBoxEntry
        {
            font-family:Arial, sans-serif;
	        font-size: 12px;   
        }
        .ddlEntry
        {
            font-family:Arial, sans-serif;
            font-size: 12px;   
            overflow: hidden;
            padding: 2px;
            background-color: #ddd;
            border: 1px solid #D8D8D8;
        }
        .accountInfo
        {
            margin-top:20px;
            margin-left:20px;
            padding:10px;
            border: 1px solid #ccc;
        }
        .chBoxEntry
        {
            width: 400px;
            border: 1px solid #D8D8D8;
        }
        span.item_checkbox > label {
            display: block;
            padding-left: 15px;
            padding-top: 5px;
            width:140px;
            text-indent: -15px;
        }
        .clear{
           clear:both;
        }
    </style>
    <link href="../Styles/marco_root.css" type="text/css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <label class="titulo_pagina">
        Proceso de Registro
    </label>
    <div style="margin-left:20px;">
        Un nuevo usuario debe ingresar primero la informaci&oacute;n requerida en el formulario, para lo cual se requiere un e-mail
        perteneciente a una empresa reconocida por el COES-SINAC.
        Son empresas reconocidas por el COES-SINAC: organismos del Estado, empresas el&eacute;ctricas pertenecientes al Sistema 
        Interconectado Nacional y grandes clientes.
        <b>El COES solo atiende solicitudes inherentes a sus funciones, y procedentes de empresas y organismos del SEIN.</b>
        Si su empresa cumpliera lo antes indicado y no se encontrara en la lista, puede contactarnos en el siguiente 
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://www.coes.org.pe/wcoes/coes/otros/contactenos/comuniquese.aspx">enlace</asp:HyperLink>
    </div>
    <!--p>
        Passwords are required to be a minimum of <%= Membership.MinRequiredPasswordLength %> characters in length.
    </p-->
    <span class="failureNotification">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" CssClass="failureNotification" 
            ValidationGroup="RegisterUserValidationGroup"/>
    <div class="accountInfo">
            <div class="fila_general">
                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="labelEntry">Nombre:</asp:Label>
                <div class="clear"></div>
                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                        CssClass="failureNotification" ErrorMessage="Nombre es requerido." ToolTip="Nombre es requerido." 
                        ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </div>
            <div class="fila_general">
                <asp:Label ID="UserSurNameLabel" runat="server" AssociatedControlID="UserSurName" CssClass="labelEntry">Apellido:</asp:Label>
                <div class="clear"></div>
                <asp:TextBox ID="UserSurName" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="UserSurNameRequired" runat="server" ControlToValidate="UserSurName" 
                        CssClass="failureNotification" ErrorMessage="Apellido es requerido." ToolTip="Apellido es requerido." 
                        ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </div>
            <div class="fila_general" style="text-align:left;">
                <label class="labelEntry">Empresa:</label>
                <div class="clear"></div>
                <asp:DropDownList ID="DropDownListEmpresa" runat="server" Width="400px" CssClass="ddlEntry"></asp:DropDownList>
            </div>
            <div class="fila_general">
                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" CssClass="labelEntry">Correo corporativo: <b>(*)</b></asp:Label>
                <div class="clear"></div>
                <asp:TextBox ID="Email" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" 
                        CssClass="failureNotification" ErrorMessage="E-mail es requerido." ToolTip="E-mail es requerido." 
                        ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" 
                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                    ControlToValidate="Email" ErrorMessage="Formato de Correo Inválido" 
                    ValidationGroup="RegisterUserValidationGroup">*</asp:RegularExpressionValidator>
            </div>
            <div class="fila_general">
                <asp:Label ID="PhoneLabel" runat="server" AssociatedControlID="Phone" CssClass="labelEntry">Tel&eacute;fono:</asp:Label>
                <div class="clear"></div>
                <asp:TextBox ID="Phone" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="PhoneRequired" runat="server" ControlToValidate="Phone" 
                        CssClass="failureNotification" ErrorMessage="Tel&eacute;fono es requerido." ToolTip="Tel&eacute;fono es requerido." 
                        ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </div>
            <div class="fila_general">
                <asp:Label ID="AreaLabel" runat="server" AssociatedControlID="Area" CssClass="labelEntry">Área Laboral:</asp:Label>
                <div class="clear"></div>
                <asp:TextBox ID="Area" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="AreaRequired" runat="server" ControlToValidate="Area" 
                        CssClass="failureNotification" ErrorMessage="Área es requerido." ToolTip="Área es requerido." 
                        ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </div>
            <div class="fila_general">
                <asp:Label ID="CargoLabel" runat="server" AssociatedControlID="Cargo" CssClass="labelEntry">Cargo:</asp:Label>
                <div class="clear"></div>
                <asp:TextBox ID="Cargo" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Cargo" 
                        CssClass="failureNotification" ErrorMessage="Cargo es requerido." ToolTip="Cargo es requerido." 
                        ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </div>
            <div class="fila_general" style="margin-top:10px;">
                <label class="labelEntry">M&oacute;dulo a acceder:</label><br />
                <asp:CheckBoxList ID="chBoxModulos" runat="server" TextAlign="Left" CssClass="chBoxEntry">
                </asp:CheckBoxList>
            </div>
            <div class="fila_general" style="text-align:left;">
                <asp:Label ID="MotivoContactoLabel" runat="server" AssociatedControlID="MotivoContacto" CssClass="labelEntry">Motivo de Contacto: <b>(**)</b></asp:Label>
                <asp:TextBox ID="MotivoContacto" TextMode="MultiLine" runat="server" Height="55px" Width="400px" MaxLength="300" CssClass="labelEntry"></asp:TextBox>
                <asp:RegularExpressionValidator ID="MotivoContactoRequired" runat="server" ControlToValidate="MotivoContacto"
                    ErrorMessage="Longitud de Motivo de Contacto Inválido"  ValidationGroup="RegisterUserValidationGroup" ValidationExpression="[\s\S]{1,300}" >*
                </asp:RegularExpressionValidator>
            </div>
        <p style="">
            <b>(*)</b> El E-mail proporcionado será su identificaci&oacute;n de usuario, se 
            suguiere ingresar correo corporativo, una vez autorizada su solicitud se le remitir&aacute; vía e-mail su contraseña.
        </p>
        <p style="">
                <b>(**)</b> M&aacute;x. 300 caracteres
        </p>
        <p class="submitButton">
            <asp:Button ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="Enviar" 
                    ValidationGroup="RegisterUserValidationGroup" 
                onclick="CreateUserButton_Click"/>
        </p>
    </div>
</asp:Content>
