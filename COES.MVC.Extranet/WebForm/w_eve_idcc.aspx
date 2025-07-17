<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_eve_idcc.aspx.cs" Inherits="WSIC2010.w_eve_idcc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>Carga del Formato IDCC</p>
    <asp:DropDownList ID="DropDownListEvenClase" runat="server" Width="223px">
    </asp:DropDownList>
    <br /><br />
    <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>

    <div>
     <h4>Seleccionar el archivo para cargar:</h4>

       <asp:FileUpload id="FileUpload1"                 
           runat="server">
       </asp:FileUpload>
       &nbsp;Max. 4MB
       <br /><br />

       

       <asp:Button id="UploadButton" 
           Text="Verificar y Cargar "           
           runat="server" onclick="UploadButton_Click">
       </asp:Button>    

       <hr />

       <asp:Label id="UploadStatusLabel"
           runat="server">
       </asp:Label>        
    </div>
</asp:Content>
