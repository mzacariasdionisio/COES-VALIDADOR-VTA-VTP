<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WSIC2010.Demanda.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/themes/base/jquery-ui.css" type="text/css" media="all" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/jquery-ui.min.js" type="text/javascript"></script>
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
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sab'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                dayStatus: 'Set DD as first week day',
                dateStatus: 'Select D, M d',
                dateFormat: 'dd/mm/yy',
                firstDay: 0,
                initStatus: 'Seleccionar fecha',
                isRTL: false
            };
            $.datepicker.setDefaults($.datepicker.regional['es']);
        });
        $(function () {
            $('#<%= txtDate.ClientID  %>').datepicker({
                beforeShowDay: enableSpecificDates,
                changeMonth: true,
                showOthersMonths: true,
                showWeek: true,
            });

            function enableSpecificDates(date) {
                if ($('#<%= DDL1.ClientID  %>').val() == 2) {
                    if (date.getDay() == 6) { //Solo se muestran los sabados
                        return [true];
                    } else {
                        return [false];
                    }
                }
                else {
                    return [true];
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:DropDownList ID="DDL1" runat="server" >
        <asp:ListItem Text="PDO" Value="1" />
        <asp:ListItem Text="PSO" Value="2" />
    </asp:DropDownList><br />
    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
</asp:Content>
