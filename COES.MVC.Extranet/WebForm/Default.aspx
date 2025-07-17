<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WSIC2010._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>        
        <br />
        <span class="style11">SICOES </span>
        <br />
        <%--<span class="style2">Bienvenido al Sistema de Gestión Operativa del COES</span> --%>
    </div>
    <p>
        <asp:Label ID="labelusuario" runat="server" Text=""></asp:Label>
    </p>
    <p>
        <asp:Label ID="labelempresa" runat="server" Text=""></asp:Label>
    </p>
    <p>
        <asp:Image ID="Image21" runat="server" ImageUrl="~/webform/images/coes.gif" 
            Height="86px" Width="25%" />

    </p>
    <p style="font-style:italic;">
        En cumplimiento de la tercera disposici&oacute;n complementaria de la norma
        "NORMA T&Eacute;CNICA PARA EL INTERCAMBIO DE INFORMACI&Oacute;N EN TIEMPO REAL PARA LA OPERACI&Oacute;N DEL SEIN" - 
        RD No. 243-2012-EM/DGE se publica la documentaci&oacute;n solicitada; la cual se encuentra disponible 
        ingresando a la opci&oacute;n RIS-Tiempo Real de la extranet.
    </p>
    <p>
        Pagina Principal del COES <a href="http://www.coes.org.pe" title="COES SINAC Website">www.coes.org.pe</a>.
    </p>
    <p>
        &nbsp;<asp:Panel ID="Panel1" runat="server"  EnableViewState="False"></asp:Panel>    
    
    </p>
</asp:Content>
