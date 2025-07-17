<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_me_carga_ult.aspx.cs" Inherits="WSIC2010.Demanda.w_me_carga_ult" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
	        /*width:30%;*/
            border-top:1px solid #C8C8C8;/*e5eff8;*/
            border-right:1px solid #C8C8C8;/*e5eff8;*/
            margin:1em auto;
            margin-left: 10px;
            border-collapse:collapse;
            float: left;
        }
        table.sample th {
	        background:#4a70aa;/*f4f9fe;*/
            text-align:center;
            font:bold 0.9em/1.1em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;
            color:#ffffff;/*66a3d3;*/
        }
        table.sample td {
	        color:#678197;
            border-bottom:1px solid #C8C8C8;/*e5eff8;*/
            border-left:1px solid #C8C8C8;/*e5eff8;*/
            padding:.3em 1em;
            text-align:center;
            font:0.8em/1.0em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;
        }
        table.sample td.left {
	        color:#678197;
            border-bottom:1px solid #FFF;/*e5eff8;*/
            border-left:1px solid #FFF;/*e5eff8;*/
            padding:.3em 1em;
            text-align:left;
            font:1.0em/1.2em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;
            font-weight: bold;
            color:#ffffff;
            background:#4a70aa;
        }
        table.sample th strong {
            font:bold 0.7em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;
            margin:.5em .5em .5em 0;
            color:#66a3d3;                    
        }
        table.sample th em {
            color:#f03b58;
            font-weight: bold;
            font-size: .7em;
            font-style: normal;
        }
        .seleccion
        {
           /*font:0.9em/1.1em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;*/
           font: 1em "Helvetica Neue","Lucida Grande","Segoe UI",Arial,Helvetica,Verdana,sans-serif;
           /*background-color: #0066CC;*/
           /*background-color: #006699;*/
           background-color: #FFF;
           width: 700px; 
           margin: 10px;
           padding: 10px;
           border: 1px solid #BBB;
        }
        #FileUpload1, #DropDownList1
        {
            margin-left: 15px;
        }
        #Button1
        {
            margin-left: 15px;
        }
        #UploadStatusLabel
        {
             color: #FF0000;
             font-style: italic;
             margin-left: 40px;    
        }
        .watermarked
        {
            color: #C0C0C0;
            font-style:italic;
            font-size: 1em
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
             $('#MainContent_tBoxFecha').datepicker({
                 dateFormat: "dd/mm/yy",
                 changeMonth: true,
                 showOthersMonths: true
             });
         });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 style="color:#000;"><center>CARGA - DEMANDA EN BARRAS</center></h1>
    <div class="seleccion">
        <table>
            <tr>
                <td style="color:#000;">Empresa:</td>
                <td align="left"><asp:DropDownList ID="DDLEmpresa" runat="server" AutoPostBack="true"
                        onselectedindexchanged="DDLEmpresa_SelectedIndexChanged" /></td>
                <td rowspan="5" align="center" style="width:220px;">
                        <asp:Button id="UploadButon" runat="server" text="Cargar Información" 
                        onclick="UploadButon_Click" />
                </td>
                <td rowspan="5" align="center" style="width:50px;">
                    Manual Usuario Demanda Barras<a href="http://contenido.coes.org.pe/alfrescostruts/download.do?nodeId=226230ef-d608-487f-9258-272aa10b0d60" target="_blank"> (Descargar)</a>    
                </td>
            </tr>
            <tr>
                <td style="color:#000;">Tipo de Programa:</td>
                <td align="left">
                      <asp:DropDownList ID="ddl_tipoPrograma" runat="server" AutoPostBack="true" 
                             OnSelectedIndexChanged="ddl_tipoPrograma_SelectedIndexChanged"/>
                </td>
            </tr>
            <tr>
                <td style="color:#000;">Fecha:</td>
                <td>
                    <input id="tBoxFecha" type="text" class="tbFecha" runat="server"/>&nbsp;(dd/mm/aaaa)
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblNumSem" runat="server" Text="N° Semana" Visible="false"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="DDLNumSemana" runat="server" Enabled="false" 
                        Visible="false" AutoPostBack="True" 
                        onselectedindexchanged="DDLNumSemana_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="color:#000;">Archivo:</td>
                <td><asp:FileUpload ID="FileUpload1" runat="server" /></td>
            </tr>
        </table>
    </div>
    <div style="margin:10px;">
        <asp:ListBox ID="ListBox1" runat="server" ></asp:ListBox><br /><br />
        <asp:Button ID="btn_exporta" runat="server" Text="Exportar" Enabled="false" 
            Visible="false" onclick="btn_exporta_Click"/>
    </div>
    <div class="modal-popup" id="divInforme" runat="server" style="display:none">

                <div class="modal-header">
                    <div class="modal-titulo" runat="server" id="divMove">Registro de Datos</div>
                    <div class="modal-close" runat="server" id="divClose">X</div>
                </div>
                <div class="modal-content">
                    <asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label>

                    <asp:Panel ID="pnlMensaje" runat="server" CssClass="content-message" >
                        <asp:Label ID="lblMensaje" runat="server" Text="Por favor ingrese los datos"></asp:Label>
                    </asp:Panel>

                    <asp:HiddenField ID="hfEdicion" runat="server" />

                    <div style="clear:both; height:15px;"></div>

                    <h1 style="color:#000;"><center>ENVIO DE INFORMACIÓN - FUERA DE PLAZO</center></h1>
    <div class="seleccion">
        <table>
            <tr><td colspan="2">La información fue cargada fuera de plazo. Agradeceremos llenar el formulario indicando los motivos por los cuales está reemplazando la información cargada previamente</td></tr>
            <tr>
            <td>
            <asp:RadioButtonList ID="rb_list" runat="server" Width="229px">
                <asp:ListItem Text="OMISIÓN EN CARGA INICIAL" Value="1" Selected="True" />
                <asp:ListItem Text="MANTENIMIENTO CORRECTIVO" Value="2" />
                <asp:ListItem Text="FALLA EN EQUIPO (TIEMPO REAL)" Value="3" />
            </asp:RadioButtonList>
            </td>
            <td>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            A&ntilde;ada una breve descipci&oacute;n (150 caracteres)
                <br />
            <asp:TextBox ID="tbox_descipcion" TextMode="MultiLine" runat="server" Height="52px" 
                    Width="404px"></asp:TextBox>
            <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "tbox_descipcion" 
                                            ID="RegularExpressionValidator1" ValidationExpression = "^[\s\S]{0,150}$" runat="server" 
                                            ErrorMessage="Máximo 150 caracteres permitidos."></asp:RegularExpressionValidator>
            </td>
            </tr>
            <tr>
            <td>
            </td>
            <td></td>
            </tr>
        </table>
    </div>


                    <div style="clear:both"></div>
                </div>
    </div>
    <asp:HiddenField ID="hfPlazoDiario" runat="server" />
    <asp:HiddenField ID="hfPlazoSemanal" runat="server" />
</asp:Content>
