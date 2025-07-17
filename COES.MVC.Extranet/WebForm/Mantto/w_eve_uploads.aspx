<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_eve_uploads.aspx.cs" Inherits="WSIC2010.Mantto.w_eve_uploads" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<left><h2 style="color:#000;">CARGA DE CAUDALES Y STOCK DE COMBUSTIBLES</h2></left>
Desea cargar documentos correspondientes a:
    <asp:RadioButtonList ID="RadioButtonTipos" runat="server">
        <asp:ListItem Value="1" Selected="True">Caudales y Generación Pronósticada</asp:ListItem>
        <asp:ListItem Value="2">Stock de Combustibles</asp:ListItem>
    </asp:RadioButtonList>
    <asp:FileUpload id="FileUpload1" runat="server" Width="250"></asp:FileUpload>                               
    <asp:Button id="UploadButton" Text="Cargar Archivo" OnClick="UploadButton_Click" runat="server"> </asp:Button>
    <br />M&aacute;x. 2 MB.
    <br />
    <asp:Label ID="UploadStatusLabel" Text="" runat="server" />
</asp:Content>

