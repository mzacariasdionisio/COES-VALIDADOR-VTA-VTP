<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_adm_asignaModulo.aspx.cs" Inherits="WSIC2010.Admin.w_adm_asignaModulo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .texto
    {
        margin: 20px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>SOLICITAR M&Oacute;DULO</h1>
    <div class="texto">
                <div class="fila_general">
                    <label class="etiqueta">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Modulos:</label>
                    <asp:DropDownList ID="ddlModulos" runat="server" />
                    <asp:Button ID="bt_Agregar_Rol" runat="server" Text="Agregar M&oacute;dulo" onclick="bt_AgregarRol_Click" />
                </div>
                <div class="fila_general">
                    <label class="etiqueta">Modulos a Solicitar:</label>
                    <asp:ListBox ID="lBox_modulosAsignados" runat="server" />
                    <asp:Button ID="bt_Eliminar_Rol" runat="server" Text="Eliminar M&oacute;dulo" onclick="bt_EliminarRol_Click" />
                </div>
                <div class="fila_general">
                    <asp:Button ID="bt_Aceptar" runat="server" Text="Aceptar" onclick="bt_Aceptar_Click" />
                </div>
                <div class="fila_general">
                    <asp:Label ID="label_resultado" runat="server" style="text-align:left;"></asp:Label>
                </div>


    </div>
</asp:Content>