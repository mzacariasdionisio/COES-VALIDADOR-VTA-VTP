<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_dem_historica.aspx.cs" Inherits="WSIC2010.Demanda.w_dem_historica" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.4.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.custom.min.js"></script>
    <link rel="Stylesheet" type="text/css" href="../Styles/jquery-ui-1.8.custom.css"/>
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
        .seleccion
        {
           /*font:0.9em/1.1em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;*/
           font: 1em "Helvetica Neue","Lucida Grande","Segoe UI",Arial,Helvetica,Verdana,sans-serif;
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
            width: 420px;
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
            border: 2px solid white;
            text-align:center;
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
            border: 2px solid white;
            text-align:left;
        }
        .mensajeError
        {
            color: #F00;
            font-weight: bold;
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
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $.datepicker.regional['es'] = {
                clearText: 'Borra',
                clearStatus: 'Borra fecha actual',
                closeText: 'Cerrar',
                closeStatus: 'Cerrar sin guardar',
                prevText: '<Ant',
                prevBigText: '<<',
                prevStatus: 'Mostrar mes anterior',
                prevBigStatus: 'Mostrar año anterior',
                nextText: 'Sig>',
                nextBigText: '>>',
                nextStatus: 'Mostrar mes siguiente',
                nextBigStatus: 'Mostrar año siguiente',
                currentText: 'Hoy',
                currentStatus: 'Mostrar mes actual',
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                monthStatus: 'Seleccionar otro mes',
                yearStatus: 'Seleccionar otro año',
                weekHeader: 'Sm',
                weekStatus: 'Semana del año',
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                dayStatus: 'Set DD as first week day',
                dateStatus: 'Select D, M d',
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                initStatus: 'Seleccionar fecha',
                isRTL: false
            };
            $.datepicker.setDefaults($.datepicker.regional['es']);
        });
        $(function () {
            $('#MainContent_tBoxInicio').datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: '+7d',
                changeMonth: true,
                showOthersMonths: true,
                onSelect: function (dateStr) {
                    var d = $.datepicker.parseDate('dd/mm/yy', dateStr);
                    $('#MainContent_tBoxFin').datepicker('setDate', new Date(d.getFullYear(), d.getMonth(), d.getDate() + 7));
                    $('#MainContent_tBoxFin').datepicker("option", "minDate", dateStr);
                    $('#MainContent_tBoxFin').datepicker("option", "maxDate", new Date(d.getFullYear(), d.getMonth() + 2, d.getDate()));
                }
            });
            $('#MainContent_tBoxFin').datepicker({
                dateFormat: "dd/mm/yy"
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1 style="color:#000;">LISTADO HIST&Oacute;RICO DIARIO - DEMANDA EN BARRAS</h1></center>
        <div class="seleccion">
            <table width="600px">
                <tr>
                    <td>Empresa:</td><td><asp:DropDownList ID="DDLEmpresa" runat="server" 
                        AutoPostBack="true" onselectedindexchanged="DDLEmpresa_SelectedIndexChanged"></asp:DropDownList></td>
                    <td rowspan="3" align="center">
                        <asp:Button ID="btnResult" runat="server" Text="Listar" Width="100px" 
                            onclick="btnResult_Click"/>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbl_equipo" runat="server" Text="L&iacute;nea/barra:"/></td>
                    <td><asp:DropDownList ID="DDLBarras" runat="server"/></td>
                </tr>
                <tr>
                    <td>Tipo de Lectura:</td>
                    <td>
                        <asp:DropDownList ID="DDLLectura" runat="server" >
                            <asp:ListItem Selected="True" Value="45" Text="Demanda Hist&oacute;rica Diaria" />
                            <asp:ListItem Value="46" Text="Demanda Prevista Diaria" />
                            <asp:ListItem Value="47" Text="Demanda Prevista Semanal" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Fecha Inicio:</td>
                    <td colspan="2"><input id="tBoxInicio" type="text" class="tbFecha" runat="server"/>&nbsp;(dd/mm/aaaa)</td>
                </tr>
                <tr>
                    <td>Fecha Fin:</td>
                    <td colspan="2"><input id="tBoxFin" type="text" class="tbFecha" runat="server"/>&nbsp;(dd/mm/aaaa)</td>
                </tr>
            </table>
        </div>
        <div class="texto">
            <asp:Label ID="Label1" runat="server" Text="" />
            <asp:GridView ID="GridView2" runat="server"  CssClass="gView2" ShowHeader="False">
            </asp:GridView>
            <asp:GridView ID="gViewHistorico" 
                          runat="server" 
                          BackColor="#DDDDDD"
                          ForeColor="#717171"
                          CellPadding="5"
                          CellSpacing="5"
                          GridLines="Horizontal"
                          CssClass="gView" >
                <HeaderStyle BackColor="#4a70aa" ForeColor="White" BorderColor="White" Font-Bold="false" />
                <AlternatingRowStyle BackColor="#fcfcfc" ForeColor="#717171" BorderColor="#717171" />
            </asp:GridView>
        </div>
</asp:Content>
