<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SeleccionarEquipo.ascx.cs" Inherits="WSIC2010.SeleccionarEquipo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >
    <ContentTemplate>
<div>
<table>
    <tbody>
        <tr>
            <td>Empresa </td>
            <td>
                
                <asp:DropDownList ID="ddlEmpresa" runat="server"  DataValueField="EMPRCODI" DataTextField="EMPRNOMB" AutoPostBack="true"
                    onselectedindexchanged="ddlEmpresa_SelectedIndexChanged"/>               
            </td>
        </tr>
        <tr>
            <td>Ubicación</td>
            <td>
                <asp:DropDownList ID="ddlArea" runat="server"  DataValueField="Areacodi" DataTextField="Areanomb" AutoPostBack="true"
                    onselectedindexchanged="ddlArea_SelectedIndexChanged"/>
            </td>
        </tr>
        <tr>
            <td>Equipo</td>
            <td>
                <asp:DropDownList ID="ddlEquipo" runat="server" DataValueField="Equicodi" DataTextField="Equinomb"
                    onselectedindexchanged="ddlEquipo_SelectedIndexChanged" AutoPostBack="True"/>
            </td>
        </tr>
    </tbody>
</table>
</div>

<asp:TextBox ID="TextBoxEquiCodi" runat="server" Visible="false" Width="54px"></asp:TextBox>
<asp:Label ID="LabelEquipoElegido" runat="server" Text="[Equipo elegido: Ninguno]" />
<asp:HiddenField ID="hfIndicador" runat="server" />

</ContentTemplate>   

</asp:UpdatePanel>
