<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_me_lista_demanda.aspx.cs" Inherits="WSIC2010.Demanda.w_me_lista_demanda" %>
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
            $('#MainContent_rbtipoLectura_0').attr('checked', 'checked');
            $("#MainContent_tBoxInicio").datepicker();
            $("#MainContent_tBoxFin").datepicker();
            var startdate = new Date();
            startdate.setDate(startdate.getDate() - 1);
            $("#MainContent_tBoxInicio").datepicker("setDate", startdate);
            var endDate = new Date(); // current date
            $("#MainContent_tBoxFin").datepicker("setDate", endDate);
        });
        $(function () {
            $('#MainContent_tBoxInicio').datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: '+7d',
                changeMonth: true,
                showOthersMonths: true,
                onSelect: function (dateStr) {
                    var d = $.datepicker.parseDate('dd/mm/yy', dateStr);
                    $('#MainContent_tBoxFin').datepicker('setDate', new Date(d.getFullYear(), d.getMonth(), d.getDate() + 1));
                    $('#MainContent_tBoxFin').datepicker("option", "minDate", dateStr);
                    $('#MainContent_tBoxFin').datepicker("option", "maxDate", new Date(d.getFullYear(), d.getMonth(), d.getDate() + 31));
                }
            });
            $('#MainContent_tBoxFin').datepicker({
                dateFormat: "dd/mm/yy"
            });
            $('.radioList input').click(function () {
                /* get the value of the radio button that's been clicked */
                var d = new Date();
                var selectedValue = $(this).val();
                if (selectedValue == '0') {
                    $('#MainContent_tBoxInicio').datepicker('setDate', new Date(d.getFullYear(), d.getMonth(), d.getDate() - 1));
                    $('#MainContent_tBoxFin').datepicker('setDate', new Date(d.getFullYear(), d.getMonth(), d.getDate()));
                }
                else if (selectedValue == '1') {
                    $('#MainContent_tBoxInicio').datepicker('setDate', new Date(d.getFullYear(), d.getMonth(), d.getDate() + 1));
                    $('#MainContent_tBoxFin').datepicker('setDate', new Date(d.getFullYear(), d.getMonth(), d.getDate() + 2));
                }
                else if (selectedValue == '2') {
                    $('#MainContent_tBoxInicio').datepicker('setDate', new Date(d.getFullYear(), d.getMonth(), d.getDate() + 6 - d.getDay()));
                    $('#MainContent_tBoxFin').datepicker('setDate', new Date(d.getFullYear(), d.getMonth(), d.getDate() + 13 - d.getDay()));
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1 style="color:#000;">LISTADO DE INFORMACI&Oacute;N</h1></center>
        <div class="seleccion">
            <table width="600px">
                <tr>
                    <td>Empresa:</td><td><asp:DropDownList ID="DDLEmpresa" runat="server" 
                        AutoPostBack="true" onselectedindexchanged="DDLEmpresa_SelectedIndexChanged"></asp:DropDownList></td>
                    <td rowspan="2" align="center">
                        <asp:Button ID="btnResult" runat="server" Text="Exportar" Width="100px" 
                            onclick="btnResult_Click"/>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbl_equipo" runat="server" Text="L&iacute;nea/barra:"/></td>
                    <td><asp:DropDownList ID="DDLBarras" runat="server"/></td>
                </tr>
                <tr>
                    <td>Tipo de Lectura:</td>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rbtipoLectura" runat="server" RepeatDirection="Horizontal" CssClass="radioList">
                            <asp:ListItem Text="Hist&oacute;rico Diario" Value="0"/>
                            <asp:ListItem Text="Previsto Diario" Value="1" />
                            <asp:ListItem Text="Previsto Semanal" Value="2" />
                        </asp:RadioButtonList>
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
        </div>
</asp:Content>
