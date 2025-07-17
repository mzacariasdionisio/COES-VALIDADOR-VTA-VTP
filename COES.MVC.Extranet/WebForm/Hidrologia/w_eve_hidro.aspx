<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_eve_hidro.aspx.cs" Inherits="WSIC2010.w_eve_hidro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="../../Styles/Custom.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>CARGA DEL FORMATO HIDROLOGIA</h2>

    <div>
        Seleccionar el archivo para cargar:<br />
         <asp:DropDownList ID="ddlx_archivo_item" runat="server">
             <asp:ListItem Value="2">ANEXO1 - PDIARIO</asp:ListItem>
             <asp:ListItem Value="3">ANEXO1 - PSEMANAL</asp:ListItem>
             <asp:ListItem Value="4">ANEXO2 - EJECUTADO</asp:ListItem>
             <asp:ListItem Value="5">ANEXO3 - EJECUTADO</asp:ListItem>
         </asp:DropDownList>
         <asp:FileUpload id="FileUpload1" runat="server"/>
    </div>
         <div class="clear"></div>
       
       <asp:Button id="UploadButton" Text="Verificar y Cargar " runat="server" onclick="UploadButton_Click">
       </asp:Button>    
       <hr />
        <asp:ListBox ID="ListBox1" runat="server" CssClass="texto"></asp:ListBox>
        <div class="clear"></div>
        <asp:Label id="UploadStatusLabel" runat="server"/>       
</asp:Content>
