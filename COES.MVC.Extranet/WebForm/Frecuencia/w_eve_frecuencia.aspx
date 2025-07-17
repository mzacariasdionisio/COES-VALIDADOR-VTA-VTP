<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_eve_frecuencia.aspx.cs" Inherits="WSIC2010.w_eve_frecuencia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <center><label class="titulo_pagina">Consulta de Frecuencia de la SE SAN JUAN</label></center>
     <div class="col_left">   
     <table style="margin-left:35px;" cellpadding="3" cellspacing="3">
     <tr>
        <td> Año:</td>
        <td><asp:DropDownList ID="DropDownListYears" runat="server"></asp:DropDownList></td>           
     </tr>
     <tr>
        <td>Mes:</td>
        <td><asp:DropDownList ID="DropDownListMonths" runat="server"></asp:DropDownList></td>
     </tr>
     <tr>
        <td>Día:</td>
        <td><asp:DropDownList ID="DropDownListDay" runat="server"></asp:DropDownList></td>
     </tr>     
      <tr>
        <td colspan="2"><asp:Button ID="ButtonGenerarMedicion" runat="server" Text="A Excel" onclick="ButtonGenerarMedicion_Click" Width="100%" /></td>
     </tr>
     </table>    
    </div>
</asp:Content>
