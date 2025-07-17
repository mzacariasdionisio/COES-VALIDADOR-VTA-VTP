<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_eve_cargarcco.aspx.cs" Inherits="WSIC2010.Mantto.w_eve_cargarcco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>Reemplaza la macro Excel CCOCARGAR</p>
    <asp:DropDownList ID="DropDownListEvenClase" runat="server" Width="223px">
    </asp:DropDownList>

    <div>
     <h4>Seleccionar el archivo para cargar:</h4>

       <asp:FileUpload id="FileUpload1"                 
           runat="server">
       </asp:FileUpload>
       &nbsp;Max. 4MB
       <br /><br />

       

       <asp:Button id="UploadButton" 
           Text="Verificar y Cargar "
           OnClick="UploadButton_Click"
           runat="server">
       </asp:Button>    

       <hr />

       <asp:Label id="UploadStatusLabel"
           runat="server">
       </asp:Label>        
    </div>
  <div>
    <div >
        <asp:ListBox ID="ListBox1" runat="server" Width="100%" Height="207px" ></asp:ListBox>
    </div>
    <div >  
    </div>
  </div>
</asp:Content>
