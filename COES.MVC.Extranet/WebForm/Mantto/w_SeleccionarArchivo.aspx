<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_SeleccionarArchivo.aspx.cs" Inherits="WSIC2010.w_SeleccionarArchivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div>                    
        
        <br/>
        <!--<div style="color: #0000FF">
        Esta opcion esta disponible <b>temporalmente</b> mientras se adecue al nuevo formato, ya que este mecanismo no permite establecer adecuadamente la integridad de la información.                        
        </div>-->
        <br /> 
        <div style="color: #008000">
        Se recomienda que el ingreso de información inicial se realice con la opcion 'Copiar de Mantto Aprobados' . Ejm en el Prog. Semanal se copia del Mensual, ... 
        </div>
        <br />
        Descargue el formato actualizado <a href="http://www.coes.org.pe/mantto/" target="_blank">aqu&iacute;</a>

       <h4>Seleccionar el archivo para cargar:</h4>

       <asp:FileUpload id="FileUpload1"                 
           runat="server">
       </asp:FileUpload>
       &nbsp;Max. 4MB
       <br /><br />

       <asp:Button id="UploadButton" 
           Text="Cargar Archivo"
           OnClick="UploadButton_Click"
           runat="server">
       </asp:Button>    

       <hr />

       <asp:Label id="UploadStatusLabel"
           runat="server">
       </asp:Label>        
    </div>
    <asp:Button ID="ButtonRegresar" runat="server" Text="Regresar" 
        onclick="ButtonRegresar_Click" Width="129px" />
    <asp:Button ID="ButtonListado" runat="server" Text="Ver Listado de Mantenimientos" 
        onclick="ButtonListado_Click" Width="129px" Enabled="false" Visible="false"/>
  <div>
    <div >
        <asp:ListBox ID="ListBox1" runat="server" Width="100%" Height="207px" ></asp:ListBox>
    </div>
    <div >  
    </div>
  </div>
</asp:Content>
