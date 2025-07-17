<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_eve_lista_manttos.aspx.cs" Inherits="WSIC2010.Mantto.w_eve_lista_manttos" %>
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
           width: 900px; 
           margin: 10px;
           padding: 10px;
           border: 1px solid #BBB;
        }
        .texto
        {
            clear:both;
            margin-left: 10px;
        }
        .titulo
        {
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
        .tbFecha
        {
            text-align:left;
        }
        table.ui-datepicker-calendar td
        {
            width:5px;
            text-align: center;
        }
        .celda
        {
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
    $(document).ready(function () {
        $("#MainContent_TextBoxFecha").datepicker({
            beforeShow: customDate,
            dateFormat: 'dd/mm/yy',
            buttonImage: './images/Calendar_scheduleHS.png',
            buttonText: '(dd/mm/aaaa)',
            firstDay: 6,
            showOthersMonths: true,
            buttonImageOnly: true,
            showOn: 'both',
            showAnim: 'fadeIn',
            onSelect: function () { $(this).trigger("onchange", null); }
        });


        function customDate() {
            var defaultMin = new Date();
            var defaultMax = new Date();

            var Min = defaultMin;
            var Max = defaultMax;

            // make valid date from hiddenfied value format is MM/dd/yyyy
            dateMin = $("#MainContent_hdfecha1").val();
            dateMin = new Date(dateMin);

            dateMax = $("#MainContent_hdfecha2").val();
            dateMax = new Date(dateMax);


            if (dateMin && dateMax) {
                Min = new Date(dateMin.getFullYear(), dateMin.getMonth(), dateMin.getDate());
                Max = new Date(dateMax.getFullYear(), dateMax.getMonth(), dateMax.getDate());
            }

            return {
                minDate: Min,
                maxDate: Max
            };
        }

    });
</script>
<script type="text/javascript">
    var old_backgroundColor;
    var old_foreColor;

    function onGridViewRowSelected_eve(codi) {
        window.location = "w_eve_details_mantto.aspx?qs_mancodi=" + codi;
    }
    function onMouseOver_eve(row) {
        old_backgroundColor = row.style.backgroundColor;
        old_foreColor = row.style.foreColor;
        row.style.backgroundColor = "#CCFFCC";
        row.style.foreColor = "#FFFFFF";
    }
    function onMouseOut_eve(row) {
        row.style.backgroundColor = old_backgroundColor;
        this.style.foreColor = old_foreColor;
    }
</script>
<script type="text/javascript">
    function startTime() {
        var ld_fechahora = document.getElementById('<%=hdfecha.ClientID %>').getAttribute('value');
        now = new Date();
        y2k = new Date(ld_fechahora);
        days = (y2k - now) / 1000 / 60 / 60 / 24;
        daysRound = Math.floor(days);
        hours = (y2k - now) / 1000 / 60 / 60 - (24 * daysRound);
        hoursRound = Math.floor(hours);
        minutes = (y2k - now) / 1000 / 60 - (24 * 60 * daysRound) - (60 * hoursRound);
        minutesRound = Math.floor(minutes);
        seconds = (y2k - now) / 1000 - (24 * 60 * 60 * daysRound) - (60 * 60 * hoursRound) - (60 * minutesRound);
        secondsRound = Math.round(seconds);
        sec = (secondsRound == 1) ? " segundo&nbsp;&nbsp; " : " segundos&nbsp; ";
        min = (minutesRound == 1) ? " minuto&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br/> " : " minutos&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br/> ";
        hr = (hoursRound == 1) ? " hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br/> " : " horas&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br/> ";
        dy = (daysRound == 1) ? " d&iacute;a&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br/> " : " d&iacute;as&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br/> ";
        diferencia = (y2k - now) / 1000;
        if (diferencia > 0) {
            document.getElementById('<%=LabelTiempoRestante.ClientID %>').innerHTML = "ABIERTO (restan : " + checkTime(daysRound) + dy + checkTime(hoursRound) + hr + checkTime(minutesRound) + min +
                                                                    checkTime(secondsRound) + sec + ")";
        }
        else {
            document.getElementById('<%=LabelTiempoRestante.ClientID %>').innerHTML = "CERRADO";
        }

        t = setTimeout('startTime()', 1000);
    }
    function checkTime(i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    }
    window.onload = function () { startTime(); }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="seleccion">
            <table width="900px">
                <tr>
                    <td><asp:TextBox ID="TextBoxFecha" runat="server" AutoPostBack="true" ontextchanged="TextBoxFecha_TextChanged" ></asp:TextBox></td>
                    <td align="right">
                        <asp:Label ID="LabelTiempoRestante" runat="server" Font-Bold="True"  Font-Size="14px" 
                                   ForeColor="#FF6666" style="font-family: Calibri">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2"><asp:Label ID="LabelTituloMantto" runat="server" Text=""></asp:Label></td>
                    <td><asp:HiddenField ID="hdfecha" runat="server" /></td>
                </tr>
            </table>
            <asp:HiddenField ID="hdfecha1" runat="server" />
            <asp:HiddenField ID="hdfecha2" runat="server" />
    </div>
    <asp:Label ID="LabelMensaje" runat="server" Text="" CssClass="titulo"/>
    <div class="titulo">
        <table>
            <tr>
                <td><asp:Button ID="btnNuevo" runat="server" Text="Nuevo" /></td>
                <td><asp:Button ID="btnImportarXLS" runat="server" Text="Importar Excel" 
                        onclick="btnImportarXLS_Click"/></td>
                <td><asp:Button ID="btnExportarXLS" runat="server" Text="Exportar Excel" 
                        onclick="btnExportarXLS_Click"/></td>
                <td><asp:Button ID="btnCopiarManttosAprob" runat="server" Text="Copiar de Manttos Aprobados"/></td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="Panel1" runat="server" CssClass="texto" ScrollBars="Horizontal">
        <asp:GridView ID="gView" 
                        runat="server" 
                        BackColor="#DDDDDD"
                        ForeColor="#717171"
                        CellPadding="5"
                        CellSpacing="5"
                        GridLines="Horizontal"
                        OnPageIndexChanging="gView_PageIndexChanging"
                        OnRowDataBound="gView_RowDataBound"
                        CssClass="gView"
                        AllowPaging="true"
                        AutoGenerateColumns="false"
                        PageSize="10"
                        Font-Size="10px">
            <HeaderStyle BackColor="#4a70aa" ForeColor="White" BorderColor="White" Font-Bold="false" />
            <FooterStyle CssClass="dxpFooter_Glass" />
            <PagerStyle CssClass="dxpSummary_Glass" HorizontalAlign="Center" VerticalAlign="Middle" />
            <AlternatingRowStyle BackColor="#fcfcfc" ForeColor="#717171" BorderColor="#717171" />
            <PagerSettings FirstPageImageUrl="~/webform/images/32x32/ico_pFirst.png"         
                            LastPageImageUrl="~/webform/images/32x32/ico_pLast.png" Mode="NumericFirstLast" 
                            NextPageImageUrl="~/webform/images/32x32/ico_pNext.png" 
                            PreviousPageImageUrl="~/webform/images/32x32/ico_pPrev.png" />
            <Columns>
                <asp:BoundField DataField="TIPOEVENCODI" HeaderText="TM" />
                <asp:TemplateField HeaderText="Empresa">
                    <ItemTemplate>
                        <asp:Label ID="lb_empresa" runat="server" 
                                    Text='<%# Eval("emprnomb").ToString().Trim().Length <= 12 ? Eval("emprnomb").ToString().PadRight(12).Trim() : Eval("emprnomb").ToString().PadRight(12).Substring(0,12).Trim() %>' 
                                    ToolTip='<%# String.Format("{0}",Eval("emprnomb").ToString().Trim()) %>' ></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="75px" CssClass="celda" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ubicaci&oacute;n">
                    <ItemTemplate>
                        <asp:Label ID="lb_descripcion" runat="server" 
                                    Text='<%# Eval("areanomb").ToString().Trim().Length <= 25 ? Eval("areanomb").ToString().PadRight(25).Trim() : Eval("areanomb").ToString().PadRight(25).Substring(0,25).Trim() %>'
                                    ToolTip='<%# String.Format("{0}",Eval("areanomb").ToString().Trim()) %>' ></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="75px" CssClass="celda"/>
                </asp:TemplateField>
                <asp:BoundField DataField="FAMABREV" HeaderText="Fam" />
                <asp:BoundField DataField="EQUIABREV" HeaderText="Equipo" />
                <asp:TemplateField HeaderText="Inicio">
                    <ItemTemplate>
                        <asp:Label ID="lb_fechaini" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy HH:mm}",Eval("evenini")) %>' ></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="75px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Final">
                    <ItemTemplate>
                        <asp:Label ID="lb_fechafin" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy HH:mm}",Eval("evenfin")) %>' ></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="75px" />
                </asp:TemplateField>
                <asp:BoundField DataField="EVENMWINDISP" HeaderText="MWI."/>
                <asp:BoundField DataField="EVENINDISPO" HeaderText="Ind." />
                <asp:BoundField DataField="EVENINTERRUP" HeaderText="Int." />
                <asp:TemplateField HeaderText="Descripci&oacute;n">
                    <ItemTemplate>
                        <asp:Label ID="lb_descripcion" runat="server" 
                                    Text='<%# Eval("evendescrip").ToString().Trim().Length <= 50 ? Eval("evendescrip").ToString().PadRight(50).Trim() : Eval("evendescrip").ToString().PadRight(50).Substring(0,50).Trim() %>' 
                                    ToolTip='<%# String.Format("{0}",Eval("evendescrip")) %>' ></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="115px" CssClass="celda"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="&Uacute;ltima Act.">
                    <ItemTemplate>
                        <asp:Label ID="lb_lastdate" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy HH:mm}",Eval("lastdate")) %>' ></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="75px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Creaci&oacute;n">
                    <ItemTemplate>
                        <asp:Label ID="lb_created" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy HH:mm}",Eval("created")) %>' ></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="75px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Usuario">
                    <ItemTemplate>
                        <asp:Label ID="lb_lastuser" runat="server" Text='<%# GetString(Eval("lastuser")) %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
                
        </asp:GridView>
    </asp:Panel>
</asp:Content>
