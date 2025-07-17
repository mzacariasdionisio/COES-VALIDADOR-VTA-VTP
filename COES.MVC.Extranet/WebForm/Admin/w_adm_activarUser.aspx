<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_adm_activarUser.aspx.cs" Inherits="WSIC2010.Admin.w_adm_activarUser" %>
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
            width: 920px;
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
        img
        { 
            border-style: none;
            text-decoration:none;
        }
        a
        {
            text-decoration:none;
            color: #000;
        }
        td a img {
            border: 0 none;
            padding: 2px;
            text-decoration:none;
        }
        .link
        {
            text-decoration: none;
        }
        td.link input
        {
            text-decoration: none;
            border-color: transparent;
        }
        .style1
        {
            width: 342px;
        }
        .sangria
        {
            margin-left: 1px;
        }
        .style2
        {
            height: 20px;
        }
        .style3
        {
            width: 342px;
            height: 20px;
        }
        .table_user tr
        { 
            display: table;            /* this makes borders/margins work */
            border: 1px solid #EEE;
            margin: 5px;
            width: 580px;
        }
    </style>
    <link href="../Styles/marco_root.css" type="text/css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="texto">
                <div class="fila_general">
                    <asp:Label ID="lb_error" runat="server" Text=""/>
                </div>
                <div class="fila_general">
                <label class="etiqueta">Nombre:</label>
                <asp:Label ID="label_user" runat="server" style="text-align:left;"></asp:Label>
                </div>
                <div class="fila_general">
                <label class="etiqueta">Modulos Solicitados:</label>
                <asp:ListBox ID="lBox_modulosSolicitados" runat="server" />
                <asp:Button ID="bt_Agregar_Rol" runat="server" Text="Agregar M&oacute;dulo" onclick="bt_AgregarRol_Click" />
                </div>
                <div class="fila_general">
                <label class="etiqueta">Modulos Asignados:</label>
                <asp:ListBox ID="lBox_modulosAsignados" runat="server" />
                <asp:Button ID="bt_Eliminar_Rol" runat="server" Text="Eliminar M&oacute;dulo" onclick="bt_EliminarRol_Click" />
                </div>
                <div class="fila_general">
                <label class="etiqueta">Email:</label>
                <asp:Label ID="label_email" runat="server" style="text-align:left;"></asp:Label>
                </div>
                <div class="fila_general">
                    <label class="etiqueta">Fecha Creación:</label>
                    <asp:Label ID="label_fec_creacion" runat="server" style="text-align:left;"></asp:Label>
                </div>
                <div class="fila_general">
                    <label class="etiqueta">Fecha Activación:</label>
                    <asp:Label ID="label_fec_activacion" runat="server" style="text-align:left;"></asp:Label>
                </div>
                <div class="fila_general">
                    <label class="etiqueta">Fecha Baja:</label>
                    <asp:Label ID="label_fec_baja" runat="server" style="text-align:left;"></asp:Label>
                </div>
                <div class="fila_general">
                    <label class="etiqueta">Teléfono:</label>
                    <asp:Label ID="label_telefono" runat="server" style="text-align:left;"></asp:Label>
                </div>
                <div class="fila_general">
                    <label class="etiqueta">Area:</label>
                    <asp:Label ID="label_empresa" runat="server" style="text-align:left;"></asp:Label>
                </div>
                <div class="fila_general">
                    <label class="etiqueta">Motivo de Contacto:</label>
                    <asp:Label ID="label_motivo" runat="server" style="text-align:left;"></asp:Label>
                </div>
                <div class="fila_general">
                    <label class="etiqueta">Listado de Empresas:</label>
                    <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="sangria" />
                    <asp:Button ID="bt_Agregar" runat="server" Text="Agregar Empresa" onclick="bt_Agregar_Click" />
                </div>
                <div class="fila_general">
                    <label class="etiqueta">Empresas Asignadas:</label>
                    <asp:ListBox ID="lBoxEmpresas" runat="server" CssClass="sangria"></asp:ListBox>
                    <asp:Button ID="bt_Eliminar" runat="server" Text="Eliminar Empresa" onclick="bt_Eliminar_Click" />
                </div>
                <div class="fila_general">
                    <asp:Button ID="bt_Aceptar" runat="server" Text="Aceptar" onclick="bt_Aceptar_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="bt_Cancelar" runat="server" Text="Cancelar" onclick="bt_Cancelar_Click" />
                </div>
        </div>
</asp:Content>
