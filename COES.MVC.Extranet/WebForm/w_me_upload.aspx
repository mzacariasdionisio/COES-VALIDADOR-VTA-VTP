<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_me_upload.aspx.cs" Inherits="WSIC2010.w_me_upload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <br />
    <br />
   
    <label class="titulo_pagina">Formatos Excel para reportar información al COES</label>

    <br />  
    <br />
    <div style="margin-left:30px;">
    <asp:Button ID="ButtonBarras" runat="server" Text="Demanda en Barras" 
        Height="35px" onclick="ButtonBarras_Click" />
    <br />
    <br />
    Información para el Pronóstico de Demanda Diaria a ser enviada antes de 
    las 8 h, <b>todos los días</b><br />
    Información para el Pronóstico Demanda Semanal a ser enviada antes de 
    las 8 h, <b>todos los martes</b><br />
    Correo a enviar: <a class="moz-txt-link-abbreviated" 
        href="mailto:demanda22@coes.org.pe">demanda22@coes.org.pe</a><br>
    <br />
    <br />

    <asp:Button ID="ButtonCCOCarga" runat="server" Text="CCO Carga" Height="35px" 
        onclick="ButtonCCOCarga_Click" />
    
    <br />
    <br />
    <asp:Button ID="ButtonDemandaHistorica" runat="server" Text="Demanda Histórica" Height="35px" Enabled="false" Visible="false"
        onclick="ButtonDemandaHistorica_Click" />
    
    <br />
    <br />
    <asp:Button ID="ButtonIDCC" runat="server" Text="IDCC" Height="35px" 
        onclick="ButtonIDCC_Click" />

    <br />
    <br />
    <asp:Button ID="ButtonHidro" runat="server" Text="Hidrología" Height="35px" 
        onclick="ButtonHidro_Click" Visible="False" />

    <br />
    </div>

</asp:Content>