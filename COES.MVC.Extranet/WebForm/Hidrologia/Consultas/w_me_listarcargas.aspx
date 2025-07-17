<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_me_listarcargas.aspx.cs" Inherits="WSIC2010.w_me_listarcargas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="../../Styles/Custom.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>LISTAR CARGAS DE INFORMACI&Oacute;N</h3>
    <div class="clear"></div>
    <div>
        <div>
            <div class="texto">Empresa:</div>
             <asp:DropDownList ID="ddlb_empresa" runat="server">
             </asp:DropDownList>
        </div>
        <div class="clear"></div>
        <div>
            <div class="texto">Tipo de Información:</div>
                <asp:DropDownList ID="ddlb_informacion" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="ddlb_informacion_SelectedIndexChanged">
                </asp:DropDownList>
        </div>
        <div class="clear"></div>
        <div>
            <div class="texto">Tipo de Formato:</div>
            <asp:DropDownList ID="ddlb_formato" runat="server"></asp:DropDownList>
        </div>
        <div class="clear"></div>
        <div>
            <div class="texto">Fecha inicial de carga:</div>
                <asp:TextBox ID="txt_fechaini" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="txt_fechaini_CalendarExtender" runat="server" 
                    Enabled="True" Format="dd/MM/yyyy" TargetControlID="txt_fechaini">
                </asp:CalendarExtender>
        </div>
        <div class="clear"></div>
        <div>
        <div class="texto">Fecha final de Carga:</div>
            <asp:TextBox ID="txt_fechafin" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="txt_fechafin_CalendarExtender" runat="server" 
                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txt_fechafin">
            </asp:CalendarExtender>
        </div>

        &nbsp;
       <br />

       

        <asp:Button ID="b_consultar" runat="server" onclick="b_consultar_Click" 
            Text="Consultar" />
        <asp:Button ID="b_excel" runat="server" onclick="b_excel_Click" Text="Excel" />
        <br />

       

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Table ID="Table1" runat="server">
        </asp:Table>

       <hr />
       <asp:Label id="UploadStatusLabel"
           runat="server">
       </asp:Label>        
        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False" 
            onrowdatabound="gv_RowDataBound">
            <Columns>
                <asp:BoundField DataField="CODIGO" HeaderText="CODIGO" ReadOnly="True" />
                <asp:BoundField DataField="FECHA_CARGA" HeaderText="FECHA_CARGA" />
                <asp:BoundField DataField="FECHA_INFORME" HeaderText="FECHA_INFORME" />
                <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" />
                <asp:BoundField DataField="USUARIO" HeaderText="USUARIO" />
                <asp:HyperLinkField HeaderText="LOG" DataNavigateUrlFields="CODIGO" 
                    DataNavigateUrlFormatString="w_me_log.aspx?id={0}" Text="LOG" />
            </Columns>
        </asp:GridView>
        <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
    </div>
</asp:Content>
