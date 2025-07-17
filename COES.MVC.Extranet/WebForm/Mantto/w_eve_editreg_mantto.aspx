<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_eve_editreg_mantto.aspx.cs" Inherits="WSIC2010.Mantto.w_eve_editreg_mantto" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            text-align: right;
        }
        .style3
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
    <asp:Label ID="LabelError" runat="server" Text="Ingrese datos"></asp:Label>
        <table>
        <tr>
        <th class="style2">Mantenimiento :</th>
        <th class="style3"> <asp:DropDownList ID="DropDownListEvenClase" runat="server" Enabled="false"></asp:DropDownList></th>
        </tr>
        <tr>
        <th class="style2">Fecha Inicio :</th>
        <th class="style3"> <asp:TextBox ID="TextBoxFechaInicio" runat="server" Enabled="false"></asp:TextBox></th>
        <th>dd/mm/aaaa</th>
        </tr>       
        <tr>
        <th>Fecha Límite</th>
        <th class="style3"> <asp:TextBox ID="TextBoxFechaLimite" runat="server"></asp:TextBox></th>
        <th>dd/mm/aaaa</th>
        </tr>
        <tr>
        <th>Hora Límite</th>
        <th class="style3"> 
            <asp:DropDownList ID="ddl_hora" runat="server" ></asp:DropDownList>
            <asp:DropDownList ID="ddl_min" runat="server" ></asp:DropDownList>
            <asp:DropDownList ID="ddl_seg" runat="server"></asp:DropDownList>
        </th>
        <th>hh:mi:ss</th>
        </tr>
        <tr>
            <th>
                <asp:Button ID="ButtonCrearFechaLim" runat="server" Text="Crear Fecha"  
                    onclick="ButtonCrearFechaLim_Click" Width="81px" />
            </th>
            <th>
                 <asp:Button ID="ButtonCancelar" runat="server" Text="Cancelar" 
                     onclick="ButtonCancelar_Click"  />
            </th>
        </tr>
        <tr>
            <th colspan="2">
                <asp:Label ID="LabelMensaje" runat="server" Text="" ForeColor="#006600"></asp:Label>
            </th>
        </tr>
        </table>
        
       
        <br />
        
        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TextBoxFechaLimite" Format="dd/MM/yyyy"  runat="server">
        </asp:CalendarExtender>
</asp:Content>
