<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_eve_mantto_new.aspx.cs" Inherits="WSIC2010.Mantto.w_eve_mantto_new" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        h1 
        {
            font: 1.5em "Helvetica Neue","Lucida Grande","Segoe UI",Arial,Helvetica,Verdana,sans-serif;
        }
        table.sample {
            border: 1px solid #C8C8C8;
            margin:1em auto;
            margin-left: 10px;
            border-collapse:collapse;
            margin: 2em;
            float: left;
        }
        table.sample th{
	        background:#C8C8C8;
            text-align:center;
            /*font:bold 0.9em/1.1em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;*/
            color:#888888;
        }
        table.sample td {
	        color:#678197;
            margin: 2em;
            padding:.3em 1em;
            font:1em/1.1em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;
        }
        table.sample th strong {
            /*font:bold 0.7em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;*/
            margin:.5em .5em .5em 0;
            color:#66a3d3;                    
        }
        table.sample th em {
            color:#f03b58;
            font-weight: normal;
            font-size: .7em;
            font-style: normal;
        }
        .label
        {
            font-weight:bold;
            width: 70px;
        }
        .seleccion
        {
           /*font:0.9em/1.1em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;*/
           font: 0.8em "Helvetica Neue","Lucida Grande","Segoe UI",Arial,Helvetica,Verdana,sans-serif;
           background-color: #FFF;
           width: 650px; 
           margin: 10px;
           padding: 10px;
           border: 1px solid #BBB;
        }
        .texto
        {
            clear:both;
            margin-left: 10px;
        }
        #FileUpload1, #DropDownList1
        {
            margin-top: 45px;
        }
        #UploadStatusLabel
        {
             color: #FF0000;
             font-style: italic;
             margin-left: 50px;    
        }
        .watermarked
        {
            color: #C0C0C0;
            font-style:italic;
            font-size: 1em
        }
        .gView
        {
            /*width: 620px;*/
        }
        .gView tr th
        {
            border: 2px solid white;
        }
        .gView th
        {
            font-weight: normal;
        }
        .gView tr td
        {
            font-size: 11px;
            border: 1px solid white;
            border-left: 1px solid transparent;
            text-align:left; /*Aca se define el centrado de las celdas*/
            color:#000;
        }
        .gView2
        {
            margin-bottom: 5px;
            margin-left: 2px;
            color: #FFF;
            background-color: #4a70aa;
        }
        .gView2 tr td
        {
            /*border: 2px solid white;*/
            text-align:left;
        }
        /*
        table.alternate{
            border-spacing: 0px;
            border-collapse: collapse;
            width: 450px;
            margin-left: 5em;
        }*/
        /*table.alternate th {
            text-align: center;
            font-weight: bold;
            padding: 2px;
            border: 2px solid #FFFFFF;
            background: #4a70aa;
            color: #FFFFFF;
        }*/
        /*table.alternate td {
            text-align: center;
            padding: 2px;
            border: 2px solid #FFFFFF;
            background: #e3f0f7;
        }*/
        /*table.alternate td {
            background: #f7f7f7;
        }*/
        .dxpSummary_Glass
        {
	        font: 12pt Arial;
	        color: #656566;
	        white-space: nowrap;
	        text-align: center;
	        vertical-align: middle;
	        padding: 1px 4px 0px 4px;
	        font-weight: bold;
        }
        .dxpFooter_Glass
        {
	        font: 8pt Arial;
	        color: #000000;
	        white-space: nowrap;
	        text-align: center;
	        vertical-align: middle;
	        padding: 1px 4px 0px 4px;
	        font-weight: bold;
        }
        .celda
        {
            text-align: left;
        }
        .hide{
            display:none;
        }
        .texto2
        {
            margin-bottom: 5px;
            margin-top: 5px;
        }
        .styled-select select {
           /*background-color: #465C71;
           color: White;
           width: 268px;
           padding: 5px;
           font-size: 10px;
           line-height: 1;
           border: 0;
           border-radius: 0;
           height: 26px;
           -webkit-appearance: none;*/
            height: 24px;
            margin-right:20px;
            margin-bottom:15px;
            border:solid 1px #aacfe4;
            margin:0px 0px 10px 10px;
            font-size:11px;
         }
    </style>
    <script type="text/javascript">
        var old_backgroundColor;
        var old_foreColor;

        function onGridViewRowSelected_eve(codi, fechalimite) {
            var lbempresa = '<%= CheckBoxEmpresas.ClientID %>';
            var empresa = document.getElementById(lbempresa).checked;
            window.location = "w_eve_mantto_list.aspx?qs_codi=" + codi + "&qs_empresa=" + empresa + "&qs_fechalimite=" + fechalimite;
        }
        function onMouseOver_eve(row) {
            old_backgroundColor = row.style.backgroundColor;
            old_foreColor = row.style.foreColor;
            //row.style.backgroundColor = "#CC3366";
            row.style.backgroundColor = "#CCFFCC";
            row.style.foreColor = "#FFFFFF";
            //alert(row.style.backgroundColor);
        }
        function onMouseOut_eve(row) {
            row.style.backgroundColor = old_backgroundColor;
            this.style.foreColor = old_foreColor;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:Label ID="lbl01" runat="server" Text="REGISTROS" />
     <br />
     &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:CheckBox ID="CheckBoxEmpresas" runat="server" Text="Filtrar equipos solo de su empresa." Checked="True" ForeColor="#0066CC" />
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="ButtonNew" runat="server" Text="Crear Nuevo" Width="120px" onclick="ButtonNew_Click" Visible="false" Enabled="false"/>
    <div class="styled-select" style="margin-top:5px;">
    <label style="padding-left:10px;">Seleccione tipo programa:</label>
    <asp:DropDownList ID="dl_TipoPrograma" runat="server" AutoPostBack="true" CssClass="texto2"
        onselectedindexchanged="dl_TipoPrograma_SelectedIndexChanged">
        <asp:ListItem Text="TODOS" Value="0" Selected="True"/>
        <asp:ListItem Text="EJECUTADO"  Value="1"/>
        <asp:ListItem Text="PROGRAMADO DIARIO"  Value="2"/>
        <asp:ListItem Text="PROGRAMADO SEMANAL"  Value="3"/>
        <asp:ListItem Text="PROGRAMADO MENSUAL"  Value="4"/>
        <asp:ListItem Text="PROGRAMADO ANUAL"  Value="5"/>
    </asp:DropDownList>
    </div>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="600px" Width="596px" CssClass="texto">
            <asp:GridView ID="gView"
                          runat="server" 
                          BackColor="#DCE6F2"
                          ForeColor="#717171"
                          CellPadding="5"
                          CellSpacing="5"
                          GridLines="Vertical"
                          CssClass="gView"
                          ShowHeader="false"
                          AutoGenerateColumns="false"
                          OnRowDataBound="gView_RowDataBound">
                     <Columns>
                        <asp:BoundField DataField="codigo" ItemStyle-CssClass="hide" />
                        <asp:BoundField DataField="tipoPrograma" ItemStyle-CssClass="hide" />
                        <asp:BoundField DataField="descripcion" ItemStyle-Width="280px" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="estado" ItemStyle-Width="280px" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="fechaInicial" ItemStyle-CssClass="hide" />
                      </Columns>
            </asp:GridView>
        </asp:Panel>
</asp:Content>
